using System;
using System.Collections.Generic;
using System.Linq;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public class PhoneGroup
	{
//		private readonly List<object> _groupItems;

		private readonly List<string> _phoneItems;
		
		private readonly bool _consonantClassPresent;
		private readonly bool _vowelClassPresent;

		private FeatureMask _descriptiveFeatureMask = App.AFeatureCache.GetEmptyMask();
		private FeatureMask _distinctiveFeatureMask = App.BFeatureCache.GetEmptyMask();
		
		private string _diacriticCluster;
		private bool _diacritiClusterHasOneOrMore;
		private bool _diacritiClusterHasZeroOrMore;

		/// ---------------------------------------------------------------------------------
		public PhoneGroup(IEnumerable<string> groupItems)
		{
	//		_groupItems = groupItems.ToList();

			var items = groupItems.ToList();

			if (items.Any(i => i == "[C]"))
			{
				_consonantClassPresent = true;
				items.Remove("[C]");
			}

			if (items.Any(i => i == "[V]"))
			{
				_vowelClassPresent = true;
				items.Remove("[V]");
			}

			CheckForDiacriticPlaceholderPattern(items);
			GetFeatureMasksFromItems(items);
			_phoneItems = items;
		}

		/// ---------------------------------------------------------------------------------
		private void CheckForDiacriticPlaceholderPattern(ICollection<string> items)
		{
			_diacriticCluster = items.FirstOrDefault(p => p.Contains(App.kDottedCircleC));
			if (_diacriticCluster == null)
				return;

			items.Remove(_diacriticCluster);
			_diacriticCluster = _diacriticCluster.Replace(App.kDottedCircle, string.Empty);
			_diacritiClusterHasOneOrMore = (_diacriticCluster.IndexOf('+') >= 0);
			_diacritiClusterHasZeroOrMore = (_diacriticCluster.IndexOf('*') >= 0);
			_diacriticCluster = _diacriticCluster.Replace("+", string.Empty).Replace("*", string.Empty);
		}

		/// ---------------------------------------------------------------------------------
		private void GetFeatureMasksFromItems(List<string> items)
		{
			var distinctiveFeatureNames = (from fname in items.Where(i => i.StartsWith("F:"))
										   where fname.StartsWith("+") || fname.StartsWith("-")
										   select fname).ToList();

			if (distinctiveFeatureNames.Count > 0)
				_distinctiveFeatureMask = App.BFeatureCache.GetMask(distinctiveFeatureNames);

			var descriptiveFeatureNames = (from fname in items.Where(i => i.StartsWith("F:"))
										   where !fname.StartsWith("+") && !fname.StartsWith("-")
										   select fname).ToList();
			
			if (descriptiveFeatureNames.Count > 0)
				_descriptiveFeatureMask = App.BFeatureCache.GetMask(descriptiveFeatureNames);

			items = items.Where(i => !i.StartsWith("F:")).ToList();
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
			//if (_groupItems.Contains(phone))
			if (_phoneItems.Contains(phone))
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
			foreach (var phoneItem in _phoneItems)
			{
				var testPhone = nonBaseSymbolRemover(phone)
					.Where(chr => !phoneItem.Contains(chr))
					.Aggregate(phone, (curr, chr) => curr.Replace(chr.ToString(), string.Empty));

				if (testPhone == phoneItem)
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
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> PhonesThatMatchIgnoredNonBaseSymbols(
			Func<IPASymbol, bool> symbolQualifiesProvider)
		{
			int initialNumberOfPhones = _phoneItems.Count;

			for (int i = 0; i < initialNumberOfPhones; i++)
			{
				var nonBaseSymbolsInPhone = (from s in _phoneItems[i]
											 let symbolInfo = App.IPASymbolCache[s]
											 where symbolInfo != null && !symbolInfo.IsBase && symbolQualifiesProvider(symbolInfo)
											 select s).ToArray();

				if (nonBaseSymbolsInPhone.Length == 0)
					continue;

				var finalNewPhone = _phoneItems[i];

				foreach (var symbol in nonBaseSymbolsInPhone)
				{
					finalNewPhone = finalNewPhone.Replace(symbol.ToString(), string.Empty);
					var newPhone = _phoneItems[i].Replace(symbol.ToString(), string.Empty);
					if (!_phoneItems.Contains(newPhone))
						yield return newPhone;
				}

				if (!_phoneItems.Contains(finalNewPhone))
					yield return finalNewPhone;
			}
		}
	}
}
