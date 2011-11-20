using System;
using System.Collections.Generic;
using System.Linq;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public class PhoneGroup
	{
		private readonly List<string> _phones;
		private readonly string _diacriticCluster;
		private readonly bool _diacritiClusterHasOneOrMore;
		private readonly bool _diacritiClusterHasZeroOrMore;

		/// ---------------------------------------------------------------------------------
		public PhoneGroup(IEnumerable<string> phoneList)
		{
			_phones = phoneList.ToList();

			_diacriticCluster = _phones.FirstOrDefault(p => p.Contains(App.kDottedCircleC));
			if (_diacriticCluster == null)
				return;

			_phones.Remove(_diacriticCluster);
			_diacriticCluster = _diacriticCluster.Replace(App.kDottedCircle, string.Empty);
			_diacritiClusterHasOneOrMore = (_diacriticCluster.IndexOf('+') >= 0);
			_diacritiClusterHasZeroOrMore = (_diacriticCluster.IndexOf('*') >= 0);
			_diacriticCluster = _diacriticCluster.Replace("+", string.Empty).Replace("*", string.Empty);
		}

		/// ---------------------------------------------------------------------------------
		public bool HasDiacriticPlaceholder
		{
			get { return _diacriticCluster != null; }
		}

		/// ---------------------------------------------------------------------------------
		public bool Matches(string phone, bool ignoreDiacritics,
			List<string> ignoredNonBaseSuprasegmentals)
		{
			if (_phones.Contains(phone))
				return (!HasDiacriticPlaceholder || GetDoesPhoneMatchDiacriticPlaceholderCluster(phone));

			if (ignoreDiacritics && GetDoesPhoneMatchWhenNonBaseSymbolsAreIgnored(phone, GetDiacriticsInPhone))
				return true;

			return GetDoesPhoneMatchWhenNonBaseSymbolsAreIgnored(phone, p =>
				GetIgnoredNonBaseSuprasegmentalsInPhone(p, ignoredNonBaseSuprasegmentals));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified phone meets the criteria in the specified
		/// diacritic placeholder cluster pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetDoesPhoneMatchDiacriticPlaceholderCluster(string phone)
		{
			if (_diacriticCluster == null)
				return false;

			// If the phone does not contain all the diacritics in the cluster, we're done.
			if (!_diacriticCluster.All(diacritic => phone.Contains(diacritic)))
				return false;

			// At this point, we know the phone contains all the diacritics. If the zero
			// or more symbol was found then the phone passes the test and we're done.
			if (_diacritiClusterHasZeroOrMore)
				return true;

			var phonesDiacriticCount = phone.Count(s =>
				App.IPASymbolCache[s] != null && !App.IPASymbolCache[s].IsBase);

			return (_diacritiClusterHasOneOrMore && phonesDiacriticCount > _diacriticCluster.Length) ||
				(!_diacritiClusterHasOneOrMore && phonesDiacriticCount == _diacriticCluster.Length);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesPhoneMatchWhenNonBaseSymbolsAreIgnored(string phone,
			Func<string, IEnumerable<char>> nonBaseSymbolRemover)
		{
			foreach (var phoneInGroup in _phones)
			{
				var testPhone = nonBaseSymbolRemover(phone)
					.Where(chr => !phoneInGroup.Contains(chr))
					.Aggregate(phone, (curr, chr) => curr.Replace(chr.ToString(), string.Empty));

				if (testPhone == phoneInGroup)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<char> GetDiacriticsInPhone(string phone)
		{
			return from chr in phone
				   let symbolInfo = App.IPASymbolCache[chr]
				   where symbolInfo != null && symbolInfo.Type == IPASymbolType.diacritic
				   select chr;
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<char> GetIgnoredNonBaseSuprasegmentalsInPhone(string phone,
			ICollection<string> ignoredNonBaseSuprasegmentals)
		{
			return from chr in phone
				   let symbolInfo = App.IPASymbolCache[chr]
				   where symbolInfo != null && ignoredNonBaseSuprasegmentals.Contains(symbolInfo.Literal)
				   select chr;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will go through each non-base symbol in each phone and check whether
		/// or not the symbols are ignored based on the search query's ignored diacritics
		/// flag value and ignored symbols collection. For each symbol that's ignored, a
		/// version of the phone without that ignored symbol is added to the phone group.
		/// When a phone in phoneGroup is found that has a '>' prefix it means that previously
		/// in the pattern parsing process, the phone matched the the criteria imposed by a
		/// diacritic placeholder cluster pattern. Those cases trump the query's ignore
		/// diacritics value and ignored symbols collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> PhonesThatMatchIgnoredNonBaseSymbols(
			Func<IPASymbol, bool> symbolQualifiesProvider)
		{
			int initialNumberOfPhones = _phones.Count;

			for (int i = 0; i < initialNumberOfPhones; i++)
			{
				var nonBaseSymbolsInPhone = (from s in _phones[i]
											 let symbolInfo = App.IPASymbolCache[s]
											 where symbolInfo != null && !symbolInfo.IsBase && symbolQualifiesProvider(symbolInfo)
											 select s).ToArray();

				if (nonBaseSymbolsInPhone.Length == 0)
					continue;

				var finalNewPhone = _phones[i];

				foreach (var symbol in nonBaseSymbolsInPhone)
				{
					finalNewPhone = finalNewPhone.Replace(symbol.ToString(), string.Empty);
					var newPhone = _phones[i].Replace(symbol.ToString(), string.Empty);
					if (!_phones.Contains(newPhone))
						yield return newPhone;
				}

				if (!_phones.Contains(finalNewPhone))
					yield return finalNewPhone;
			}
		}
	}
}
