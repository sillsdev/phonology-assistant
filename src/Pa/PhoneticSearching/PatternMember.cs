using System.Collections.Generic;
using System.Linq;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public enum PatternPart
	{
		SearchItem,
		Preceding,
		Following
	}

	public enum PhoneMatchResult
	{
		Ignored,
		Match,
		NoMatch
	}

	public class PatternMember
	{
		public bool IsInitialWordBoundary { get; set; }
		public bool IsFinalWordBoundary { get; set; }
		public bool IsZeroOrMore { get; set; }
		public bool IsOneOrMore { get; set; }

		private List<object> _groups;
		private LinkedList<PhoneGroup> _groupsLinkedList;
		private readonly bool _ignoreDiacritics;
		private readonly List<string> _ignoredBaseSuprasegmentals;
		private readonly List<string> _ignoredNonBaseSuprasegmentals;
		private readonly PatternPart _patternPart;

		private string _runOfSymbols = string.Empty;

		/// ------------------------------------------------------------------------------------
		public PatternMember(PatternPart environment, bool ignoreDiacritics,
			IEnumerable<string> ignoredSuprasegmentals)
		{
			_groups = new List<object>();
			_patternPart = environment;
			_ignoreDiacritics = ignoreDiacritics;

			var ignoredSsegs = ignoredSuprasegmentals.ToList();

			_ignoredBaseSuprasegmentals =
				(from chr in ignoredSsegs
				 let symbolInfo = App.IPASymbolCache[chr]
				 where symbolInfo != null && symbolInfo.IsBase && symbolInfo.Type == IPASymbolType.suprasegmental
				 select chr).ToList();

			_ignoredNonBaseSuprasegmentals =
				(from chr in ignoredSsegs
				let symbolInfo = App.IPASymbolCache[chr]
				where symbolInfo != null && !symbolInfo.IsBase && symbolInfo.Type == IPASymbolType.suprasegmental
				select chr).ToList();
		}

		#region Member building methods
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			_groups = new List<object>();
			_groupsLinkedList = null;
		}

		/// ------------------------------------------------------------------------------------
		public void AddSymbol(char symbol)
		{
			if (!"#+*".Contains(symbol))
				_runOfSymbols += symbol;
		}

		/// ------------------------------------------------------------------------------------
		public void AddGroup(IEnumerable<string> group)
		{
			if (_runOfSymbols != string.Empty)
			{
				_groups.Add(_runOfSymbols);
				_runOfSymbols = string.Empty;
			}

			_groups.Add(group.ToList());
		}

		/// ------------------------------------------------------------------------------------
		public void FinalizeParse(string pattern, PhoneticParser phoneticParser)
		{
			// At this point we're assuming the pattern has already been checked to make
			// sure the *, +, and # are not misplaced or coexistent with each other.
			IsZeroOrMore = (pattern == "*");
			IsOneOrMore = pattern.Contains('+');
			IsInitialWordBoundary = pattern.StartsWith("#");
			IsFinalWordBoundary = pattern.EndsWith("#");
			
			if (_runOfSymbols != string.Empty)
				_groups.Add(_runOfSymbols);

			_groupsLinkedList = new LinkedList<PhoneGroup>();

			for (int i = 0; i < _groups.Count; i++)
			{
				if (!(_groups[i] is string))
					_groupsLinkedList.AddLast(new PhoneGroup(_groups[i] as IEnumerable<string>));
				else
				{
					var run = (string)_groups[i];
					_groups.RemoveAt(i--);

					foreach (var phone in phoneticParser.Parse(run, true, false))
						_groupsLinkedList.AddLast(new PhoneGroup(new List<string> { phone }));
				}
			}

			_groups = null;
		}

		#endregion

		#region Methods for finding search item match
		/// ------------------------------------------------------------------------------------
		public bool GetSearchItemMatch(IEnumerable<string> phoneList, int startIndex,
			out int matchIndex, out int numberPhonesInMatch)
		{
			matchIndex = -1;
			numberPhonesInMatch = 0;

			var phones = phoneList.ToArray();
			var currgroup = _groupsLinkedList.First;

			for (int i = startIndex; i < phones.Length; i++)
			{
				var matchResult = GetDoesPhoneMatch(currgroup.Value, phones[i]);
				if (matchResult == PhoneMatchResult.Ignored && matchIndex > -1)
					numberPhonesInMatch++;
				else if (matchResult == PhoneMatchResult.Match)
				{
					if (matchIndex == -1)
						matchIndex = i;

					numberPhonesInMatch++;
					currgroup = currgroup.Next;

					if (currgroup == null)
					{
						if (matchIndex >= 0)
							return true;
						
						currgroup = _groupsLinkedList.First;
						//matchIndex = -1;
						numberPhonesInMatch = 0;
					}
				}
			}

			return false;
		}

		#endregion
		
		/// ------------------------------------------------------------------------------------
		public PhoneMatchResult GetDoesPhoneMatch(PhoneGroup group, string phone)
		{
			if (group.Matches(phone, _ignoreDiacritics, _ignoredNonBaseSuprasegmentals))
				return PhoneMatchResult.Match;

			return _ignoredBaseSuprasegmentals.Contains(phone) ?
				PhoneMatchResult.Ignored : PhoneMatchResult.NoMatch;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This overload should only be called when searching the preceding or following
		/// environment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetEnvironmentMatch(IEnumerable<string> phoneList)
		{
			if (IsZeroOrMore)
				return true;

			var phones = phoneList.ToArray();

			return (_patternPart == PatternPart.Preceding ?
				GetMatchInPrecedingEnvironment(phones) :
				GetMatchInFollowingEnvironment(phones));
		}

		#region Methods for finding match in preceding environment
		/// ------------------------------------------------------------------------------------
		private bool GetMatchInPrecedingEnvironment(string[] phones)
		{
			if (IsInitialWordBoundary && _groupsLinkedList.Count == 0 && phones.Length == 0)
				return true;

			int matchIndex = -1;
			int i = phones.Length - 1;
			var currgroup = _groupsLinkedList.Last;

			for (; i >= 0 && currgroup != null; i--)
			{
				var matchResult = GetDoesPhoneMatch(currgroup.Value, phones[i]);

				if (matchResult == PhoneMatchResult.NoMatch)
					return false;

				if (matchResult == PhoneMatchResult.Ignored)
					continue;

				currgroup = currgroup.Previous;
				if (matchIndex == -1)
					matchIndex = i;
			}

			if (matchIndex == -1 && _groupsLinkedList.Count > 0)
				return false;

			if (!GetDoesMatchPassInitialWordBoundaryConditions(i, phones))
				return false;

			return (!IsOneOrMore || GetIsThereAtLeastOnePhoneBeforeMatch(i, phones));
		}

		/// ------------------------------------------------------------------------------------
		private bool GetDoesMatchPassInitialWordBoundaryConditions(int indexOfPhoneAfterMatch, string[] phones)
		{
			// When there's a initial word boundary symbol, then make sure the match extends
			// to the beginning of the list of phones and if not, check if all the preceding
			// symbols are ignored.
			return (!IsInitialWordBoundary || indexOfPhoneAfterMatch < 0 ||
				phones.Take(indexOfPhoneAfterMatch + 1).All(p => _ignoredBaseSuprasegmentals.Contains(p)));
		}

		/// ------------------------------------------------------------------------------------
		private bool GetIsThereAtLeastOnePhoneBeforeMatch(int indexOfPhoneAfterMatch, string[] phones)
		{
			if (indexOfPhoneAfterMatch < 0)
				return false;

			int i = indexOfPhoneAfterMatch;
			while (i >= 0 && _ignoredBaseSuprasegmentals.Contains(phones[i]))
				i--;

			return (i >= 0);
		}

		#endregion

		#region Methods for finding match in following environment
		/// ------------------------------------------------------------------------------------
		private bool GetMatchInFollowingEnvironment(string[] phones)
		{
			if (IsFinalWordBoundary && _groupsLinkedList.Count == 0 && phones.Length == 0)
				return true;

			int matchIndex = -1;
			int i = 0;
			var currgroup = _groupsLinkedList.First;

			for (; i < phones.Length && currgroup != null; i++)
			{
				var matchResult = GetDoesPhoneMatch(currgroup.Value, phones[i]);

				if (matchResult == PhoneMatchResult.NoMatch)
					return false;

				if (matchResult == PhoneMatchResult.Ignored)
					continue;
				
				currgroup = currgroup.Next;
				if (matchIndex == -1)
					matchIndex = i;
			}

			if (matchIndex == -1 && _groupsLinkedList.Count > 0)
				return false;

			if (!GetDoesMatchPassFinalWordBoundaryConditions(i, phones))
				return false;

			return (!IsOneOrMore || GetIsThereAtLeastOnePhoneAfterMatch(i, phones));
		}

		/// ------------------------------------------------------------------------------------
		private bool GetDoesMatchPassFinalWordBoundaryConditions(int indexOfPhoneAfterMatch, string[] phones)
		{
			// When there's a final word boundary symbol, then make sure the match extends
			// to the end of the list of phones and if not, check if all the rest of the
			// symbols are ignored.
			return (!IsFinalWordBoundary || indexOfPhoneAfterMatch == phones.Length ||
				phones.Skip(indexOfPhoneAfterMatch).All(p => _ignoredBaseSuprasegmentals.Contains(p)));
		}

		/// ------------------------------------------------------------------------------------
		private bool GetIsThereAtLeastOnePhoneAfterMatch(int indexOfPhoneAfterMatch, string[] phones)
		{
			if (indexOfPhoneAfterMatch == phones.Length)
				return false;
		
			int i = indexOfPhoneAfterMatch;
			while (i < phones.Length && _ignoredBaseSuprasegmentals.Contains(phones[i]))
				i++;

			return (i < phones.Length);
		}

		#endregion
	}
}
