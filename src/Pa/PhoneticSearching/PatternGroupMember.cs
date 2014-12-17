using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SIL.Pa.Model;
using SIL.Pa.Properties;

namespace SIL.Pa.PhoneticSearching
{
	public enum MemberType
	{
		Articulatory,
		Binary,
		Class,
		SinglePhone,
		AnyConsonant,
		AnyVowel,
		OneOrMore,
		ZeroOrMore
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single member within a find phone search pattern.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PatternGroupMember
	{
		private StringBuilder m_memberBuilder;

		// This variable is only set when the member's type is SinglePhone or IPACharacterRun
		// and is only used so the ToString() method can properly display the member as it
		// was before the phone was stripped of it's diacritics.
		private string m_singlePhoneForToString;

		/// ------------------------------------------------------------------------------------
		public PatternGroupMember()
		{
			UndefinedPhoneticChars = new List<char>(0);
			m_memberBuilder = new StringBuilder();
		}

		/// ------------------------------------------------------------------------------------
		public PatternGroupMember(string memberValue) : this()
		{
			m_memberBuilder.Append(memberValue);
			CloseMember();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public MemberType MemberType { get; internal set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pattern member (e.g. feature name or IPA characters).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Member { get; internal set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the diacritic cluster pattern that modifies feature, C or V members.
		/// Diacritic clusters should not exists for IPA character or class members.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DiacriticPattern { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the masks for the pattern member. When the member's MemberType is not a
		/// Articulatory feature, then this property is irrelevant.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureMask AMask { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the masks for the pattern member. When the member's MemberType is not a
		/// Binary feature, then this property is irrelevant.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureMask BMask { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of undefined phonetic characters found in the member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<char> UndefinedPhoneticChars { get; private set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		public void AddToMember(char c)
		{
			m_memberBuilder.Append(c == '#' ? ' ' : c);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<PatternGroupMember> AddRunAndClose(string run)
		{
			Member = run;
			return ClosePhoneRunMember();
		}

		#region Methods for closing a member
		/// ------------------------------------------------------------------------------------
		public PatternGroupMember[] CloseMember()
		{
			Member = m_memberBuilder.ToString();
			m_memberBuilder = null;

			if (Member == "C" || Member == "V")
			{
				MemberType = (Member[0] == 'C' ? MemberType.AnyConsonant : MemberType.AnyVowel);
				
				// Stip off the C or V from the member's text.
				Member = Member.Substring(1);
				return null;
			}

			if (Member == "*")
				MemberType = MemberType.ZeroOrMore;
			else if (Member == "+")
				MemberType = MemberType.OneOrMore;
            else if (Member.StartsWith("+", StringComparison.Ordinal) || Member.StartsWith("-", StringComparison.Ordinal) ||
                     Member.StartsWith(App.ProportionalToSymbol, StringComparison.Ordinal))
                CloseBinaryFeatureMember();
            else if (Member.StartsWith("<", StringComparison.Ordinal) && Member.EndsWith(">", StringComparison.Ordinal))
				CloseClassMember();
			else
			{
				if (!CloseArticulatoryFeatureMember())
					return ClosePhoneRunMember();
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes a member whose type will be binary feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CloseBinaryFeatureMember()
		{
			MemberType = MemberType.Binary;
			Member = Member.ToLower();
			BMask = App.BFeatureCache.GetMask(Member);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes a member whose type will be articulatory feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CloseArticulatoryFeatureMember()
		{
			Member = Member.Replace('\u00AB', '(');
			Member = Member.Replace('\u00BB', ')');

			if (App.AFeatureCache.FeatureExits(Member))
			{
				MemberType = MemberType.Articulatory;
				Member = Member.ToLower();
				AMask = App.AFeatureCache.GetMask(Member);
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes a member whose type will be class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CloseClassMember()
		{
			MemberType = MemberType.Class;
			// TODO: get the classes pattern and process it.
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes a member containing one or more phones.
		/// </summary>
		/// <remarks>
		/// When we arrive at this method, we know we're about to close a member containing
		/// one or more phones. When closing the current member, it will contain the first
		/// phone in the run. If there is more than one phone, then new members are generated
		/// for each of the phones following the first one. Finally, the collection of those
		/// members is returned, with the first in the collection being "this".
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		private PatternGroupMember[] ClosePhoneRunMember()
		{
			MemberType = MemberType.SinglePhone;
			var memberPhones = new List<PatternGroupMember>();

			var phones = App.Project.PhoneticParser.Parse(Member, true,
				Settings.Default.ConvertPatternsWithTranscriptionChanges);

			if (phones == null || phones.Length == 0)
				return null;

			// First wrap up this member using the first phone in the run.
			CloseSinglePhoneMemeber(phones[0]);
			memberPhones.Add(this);

			// Now go through any following phones in the run and create new members for them.
			for (int i = 1; i < phones.Length; i++)
			{
				var member = new PatternGroupMember();
				member.CloseSinglePhoneMemeber(phones[i]);
				memberPhones.Add(member);
			}

			return memberPhones.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Strips off the diacritics from the member's single phone and stores those
		/// diacritics in the member's diacritics pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CloseSinglePhoneMemeber(string phone)
		{
			m_singlePhoneForToString = phone;

			string basePhone;
			string diacritics;
			
			// Don't want a null list, even if it's empty.
			UndefinedPhoneticChars =
				SearchEngine.ParsePhone(phone, out basePhone, out diacritics) ?? new List<char>(0);

			// Save the phone with all its diacritics stripped off.
			Member = basePhone;
			MemberType = MemberType.SinglePhone;
			
			if (string.IsNullOrEmpty(diacritics))
				return;

			if (DiacriticPattern == null)
				DiacriticPattern = string.Empty;

			DiacriticPattern += diacritics;
		}

		#endregion

		#region ToString Method
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return a displayable version of the pattern member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			if (m_singlePhoneForToString != null)
				return m_singlePhoneForToString;

			string diacriticCluster = (DiacriticPattern == null ? string.Empty :
				string.Format("[{0}{1}]", App.DottedCircle, DiacriticPattern));

			if (MemberType == MemberType.Articulatory || MemberType == MemberType.Binary)
			{
				string tmpMember = "[" + Member + "]";
				return (DiacriticPattern == null ? tmpMember : "[" + tmpMember + diacriticCluster + "]");
			}

			if (MemberType == MemberType.AnyConsonant || MemberType == MemberType.AnyVowel)
			{
				string tmpMember = (MemberType == MemberType.AnyConsonant ? "[C]" : "[V]");
				return (DiacriticPattern == null ? tmpMember : "[" + tmpMember + diacriticCluster + "]");
			}

			return Member;
		}

		#endregion

		#region Pattern matching methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not a single phone matches the pattern in the member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CompareResultType ContainsMatch(string phone)
		{
			return ContainsMatch(phone, SearchEngine.PhoneCache);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not a single phone matches the pattern in the member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CompareResultType ContainsMatch(string phone,
			IDictionary<string, IPhoneInfo> phoneCache)
		{
			if (MemberType == MemberType.SinglePhone)
				return ComparePhones(Member, phone);

			if (SearchEngine.IgnoredPhones.Contains(phone))
				return CompareResultType.Ignored;

			if (phone == " ")
				return CompareResultType.NoMatch;

			// Find the phone's information in the phone cache.
			IPhoneInfo phoneInfo;
			if (!phoneCache.TryGetValue(phone, out phoneInfo))
				return CompareResultType.Error;

			var compareResult = CompareResultType.NoMatch;

			switch (MemberType)
			{
				case MemberType.AnyConsonant:
					if (phoneInfo.CharType == IPASymbolType.consonant)
						compareResult = CompareResultType.Match;
					break;
				
				case MemberType.AnyVowel:
					if (phoneInfo.CharType == IPASymbolType.vowel)
						compareResult = CompareResultType.Match;
					break;
			
				case MemberType.Articulatory:
					if (phoneInfo.AMask.ContainsOneOrMore(AMask))
						compareResult = CompareResultType.Match;
					break;
				
				case MemberType.Binary:
					if (phoneInfo.BMask.ContainsOneOrMore(BMask))
						compareResult = CompareResultType.Match;
					break;
			}

			if (compareResult == CompareResultType.NoMatch)
				return compareResult;

			if ((MemberType == MemberType.AnyConsonant || MemberType == MemberType.AnyVowel ||
				MemberType == MemberType.Articulatory || MemberType == MemberType.Binary) &&
				string.IsNullOrEmpty(DiacriticPattern))
			{
				return compareResult;
			}

			if (phone.IndexOf(App.kTopTieBarC) >= 0 || phone.IndexOf(App.kBottomTieBarC) >= 0)
				return CheckDiacriticsInTieBarPhone(phone);
	
			return (CompareDiacritics(DiacriticPattern, phone) ?
			    CompareResultType.Match : CompareResultType.NoMatch);
		}

		/// ------------------------------------------------------------------------------------
		public CompareResultType CheckDiacriticsInTieBarPhone(string phone)
		{
			// Split the phone where the tie-bar is and send each remaining piece under
			// (or over) a tie-bar to comparer that checks matches on diacritics.
			return (phone.Split(App.kTieBars, StringSplitOptions.RemoveEmptyEntries)
				.Any(p => CompareDiacritics(DiacriticPattern, p)) ?
					CompareResultType.Match : CompareResultType.NoMatch);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compares a phone from a word to see if it matches one specified in the pattern.
		/// </summary>
		/// <param name="patternPhone">phone that's part of the pattern member</param>
		/// <param name="phone">phone in the word that's being searched (i.e. user's data).
		/// </param>
		/// ------------------------------------------------------------------------------------
		private CompareResultType ComparePhones(string patternPhone, string phone)
		{
			// First check if phone is ignored.
			// Check if the phone is ignored, making sure the current member is not in
			// the ignored list. If the current member is in the ignored list, then
			// don't ignore it because it has been explicitly included in the pattern.
			if (SearchEngine.IgnoredPhones.Contains(phone) &&
				!SearchEngine.IgnoredPhones.Contains(Member))
				return CompareResultType.Ignored;

			// Take the phone from the word we're searching and
			// separate it into its base character and its diacritics.
			string basePhone;
			string phonesDiacritics;
			SearchEngine.ParsePhone(phone, out basePhone, out phonesDiacritics);

			// Check if the base characters match.
			if (patternPhone != basePhone)
				return CompareResultType.NoMatch;

			bool match = CompareDiacritics(DiacriticPattern, phonesDiacritics, false);
			return (match ? CompareResultType.Match : CompareResultType.NoMatch);
		}

		#endregion

		#region Methods for Diacritic Checking
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method kicks off comparing the specified phone's diacritics from a phone with
		/// those specified in the pattern (i.e. patternDiacritics).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CompareDiacritics(string patternDiacritics, string phone)
		{
			return CompareDiacritics(patternDiacritics, phone, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compares the specified diacritics from the value in text to determine whether or
		/// not they match the diacritics in patternDiacritics. When textIsCompletePhone is
		/// true, the diacritics are stripped out. Otherwise, this method assumes text only
		/// contains diacritics and no base characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CompareDiacritics(string patternDiacritics, string text,
			bool textIsCompletePhone)
		{
			 if (patternDiacritics == null)
				patternDiacritics = string.Empty;

			if (patternDiacritics == "*")
				return true;

			string phonesDiacritics = text;

			// If this is true it's assumed the text is a complete phone (i.e. base character with
			// all its diacritics). Otherwise it's assumed the text is just one or more diacritics.
			if (textIsCompletePhone)
			{
				// Parse the phone (i.e. text) into it's base phone and the diacritics modying it.
				string basePhone;
				SearchEngine.ParsePhone(text, out basePhone, out phonesDiacritics);
			}

			// Check for zero or more diacritics, plus the one(s) specified in the pattern.
			if (patternDiacritics.IndexOf('*') >= 0)
				return DiacriticsMatchZeroOrMore(patternDiacritics, phonesDiacritics);

			// Check for one or more diacritics, plus the one(s) specified in the pattern.
			if (patternDiacritics.IndexOf('+') >= 0)
				return DiacriticsMatchOneOrMore(patternDiacritics, phonesDiacritics);

			// Stip off the ignored diacritics from the phone.
			phonesDiacritics = RemoveIgnoredDiacritics(patternDiacritics, phonesDiacritics);

			// At this point, we know all the diacritics specified in the pattern
			// must match those modifying the phone. So first check the length of
			// the pattern of diacritics with those in the phone, then check that
			// each of the phone's diacritics are also in the pattern.
			if (patternDiacritics.Length != phonesDiacritics.Length)
				return false;

			foreach (char c in phonesDiacritics)
			{
				if (patternDiacritics.IndexOf(c) < 0)
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all ignored diacritics and suprasegmentals from the specifiec string of
		/// diacritics that were stripped from a phone and that are not in the pattern's
		/// diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string RemoveIgnoredDiacritics(string patternDiacritics,
			string phonesDiacritics)
		{
			if (phonesDiacritics == null)
				return string.Empty;

			// If non suprasegmental diacritics are ignored, then first remove them from the
			// phone's diacritics if they are not found in the pattern's diacritics.
			if (SearchEngine.IgnoreDiacritics)
			{
				for (int i = 0; i < phonesDiacritics.Length; i++)
				{
					if (patternDiacritics.IndexOf(phonesDiacritics[i]) < 0)
					{
						IPASymbol charInfo = App.IPASymbolCache[phonesDiacritics[i]];
						if (charInfo != null && charInfo.Type == IPASymbolType.diacritic)
							phonesDiacritics = phonesDiacritics.Replace(phonesDiacritics[i], App.kOrc);
					}
				}

				phonesDiacritics = phonesDiacritics.Replace(App.kOrc.ToString(CultureInfo.InvariantCulture), string.Empty);
			}

			// Now remove all ignored, non base char. suprasegmentals
			// that are not explicitly in the pattern's diacritics.
			foreach (char sseg in SearchEngine.IgnoredChars)
			{
				if (patternDiacritics.IndexOf(sseg) < 0)
					phonesDiacritics = phonesDiacritics.Replace(sseg.ToString(CultureInfo.InvariantCulture), string.Empty);

				if (phonesDiacritics.Length == 0)
					break;
			}

			return phonesDiacritics;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if a phone's diacritics match those in the pattern's diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool DiacriticsMatchZeroOrMore(string patternDiacritics,
			string phoneDiacritics)
		{
			if (patternDiacritics == null)
				patternDiacritics = string.Empty;

			// If the pattern is just *, then we have a match, regardless
			// of whether or not the phone is modified by any diacritics.
			if (patternDiacritics == "*")
				return true;

			if (phoneDiacritics == null)
				return false;

			// If the pattern contains more diacritics (after accounting for the zero
			// or more character) than those in the phone, we know we've already failed.
			if (patternDiacritics.Length - 1 > phoneDiacritics.Length)
				return false;

			foreach (char c in patternDiacritics)
			{
				if (c != '*' && phoneDiacritics.IndexOf(c) < 0)
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if a phone's diacritics match when the member contains a + following
		/// a base character, V or C.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool DiacriticsMatchOneOrMore(string patternDiacritics, string phoneDiacritics)
		{
			if (string.IsNullOrEmpty(phoneDiacritics))
				return false;

			// If the pattern is just *, then we have a match, regardless of whether or not
			// the phone is modified by any diacritics.
			if (patternDiacritics == "+")
				return (phoneDiacritics.Length > 0);

			// If the pattern contains as many or more diacritics than
			// those in the phone, we know we've already failed.
			if (patternDiacritics.Length > phoneDiacritics.Length)
				return false;

			foreach (char c in patternDiacritics)
			{
				if (c != '+' && phoneDiacritics.IndexOf(c) < 0)
					return false;
			}

			return true;
		}

		#endregion
	}
}
