using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SIL.Pa.Data;

namespace SIL.Pa.FFSearchEngine
{
	public enum MemberType
	{
		Articulatory,
		Binary,
		Class,
		SinglePhone,
		IPACharacterRun,
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
		private MemberType m_type;
		private string m_member = null;
		private string m_diacriticPattern = null;

		private StringBuilder m_memberBuilder;
		private ulong[] m_masks = new ulong[] {0, 0};

		// When the member type gets set to IPACharacterRun, this variable holds
		// the character run broken into it's individual phones.
		private string[] m_ipaRun;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternGroupMember()
		{
			m_memberBuilder = new StringBuilder();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pattern member's type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MemberType MemberType
		{
			get {return m_type;}
			internal set
			{
				if (value == MemberType.IPACharacterRun && m_member != null)
					m_ipaRun = IPACharCache.PhoneticParser(m_member, true);

				m_type = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pattern member (e.g. feature name or IPA characters).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Member
		{
			get {return m_member;}
			internal set {m_member = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the diacritic cluster pattern that modifies feature, C or V members.
		/// Diacritic clusters should not exists for IPA character or class members.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DiacriticPattern
		{
			get {return m_diacriticPattern;}
			set {m_diacriticPattern = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the array of IPA characters that make up member when the member type is
		/// an SinglePhone member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] MemberArray
		{
			get { return m_ipaRun; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the masks for the pattern member. When the member's MemberType is not Binary
		/// or Articulator, then this property is irrelevant. Otherwise, if the MemberType is
		/// Binary only the first element in Masks is relevant.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ulong[] Masks
		{
			get {return m_masks;}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddToMember(char c)
		{
			m_memberBuilder.Append(c);
		}

		#region Methods for closing a member
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MemberType CloseMember()
		{
			m_member = m_memberBuilder.ToString();
			m_memberBuilder = null;

			if (m_member == "C" || m_member == "V")
			{
				m_type = (m_member[0] == 'C' ? MemberType.AnyConsonant : MemberType.AnyVowel);
				
				// Stip off the C or V from the member's text.
				m_member = m_member.Substring(1);
				return m_type;
			}

			if (m_member == "*")
				m_type = MemberType.ZeroOrMore;
			else if (m_member == "+")
				m_type = MemberType.OneOrMore;
			else if (m_member.StartsWith("+") || m_member.StartsWith("-"))
				CloseBinaryFeatureMember();
			else if (m_member.StartsWith("<") && m_member.EndsWith(">"))
				CloseClassMember();
			else
			{
				string feature = m_member.ToLower();

				if (IsArticulatoryFeature(feature))
					CloseArticulatoryFeatureMember(feature);
				else
					CloseIPACharacterMember();
			}

			return m_type;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsArticulatoryFeature(string feature)
		{
			foreach (string key in DataUtils.AFeatureCache.Keys)
			{
				string spacelessKey = key.Replace(" ", string.Empty).ToLower();
				if (feature.ToLower() == spacelessKey)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes a member whose type will be binary feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CloseBinaryFeatureMember()
		{
			m_type = MemberType.Binary;
			m_member = m_member.ToLower();

			// Strip off the + or -
			string featureText = m_member.Substring(1);

			// Find the feature in the cache.
			BFeature feature = DataUtils.BFeatureCache.FeatureFromCompactedKey(featureText);
			if (feature != null)
				m_masks[0] = (m_member.StartsWith("+") ? feature.PlusMask :	feature.MinusMask);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes a member whose type will be articulatory feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CloseArticulatoryFeatureMember(string featureText)
		{
			m_type = MemberType.Articulatory;
			m_member = m_member.ToLower();

			// Find the feature in the cache.
			AFeature feature = DataUtils.AFeatureCache.FeatureFromCompactedKey(featureText);
			if (feature != null)
				m_masks[feature.MaskNumber] = feature.Mask;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes a member whose type will be class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CloseClassMember()
		{
			m_type = MemberType.Class;
			// TODO: get the classes pattern and process it.
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes a member whose type will be SinglePhone or IPACharacterRun
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CloseIPACharacterMember()
		{
			m_ipaRun = IPACharCache.PhoneticParser(m_member, true);

			// Determine whether or not we have a single phone or a run.
			m_type = (m_ipaRun == null || m_ipaRun.Length <= 1 ?
				MemberType.SinglePhone : MemberType.IPACharacterRun);

			// When the member is a single phone then strip off the diacritics from the phone
			// and store them in the member's diacritics pattern.
			if (m_type == MemberType.SinglePhone && m_ipaRun.Length == 1)
				PostProcessClosedSinglePhoneMemeber();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Strips off the diacritics from the member's single phone and stores those
		/// diacritics in the member's diacritics pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PostProcessClosedSinglePhoneMemeber()
		{
			string basePhone;
			string diacritics;
			SearchEngine.ParsePhone(m_ipaRun[0], out basePhone, out diacritics);

			if (string.IsNullOrEmpty(diacritics))
				return;

			// Save the phone with all its diacritics stripped off.
			m_ipaRun[0] = basePhone;
			m_member = m_ipaRun[0];

			if (m_diacriticPattern == null)
				m_diacriticPattern = string.Empty;

			char lastChar = '\0';

			// Save the last character in the current diacritic pattern if it's the
			// zero or one or more symbol. Then strip it out of the diacritic pattern.
			if (m_diacriticPattern.Length > 0)
			{
				lastChar = m_diacriticPattern[m_diacriticPattern.Length - 1];
				if (lastChar != '*' && lastChar != '+')
					lastChar = '\0';
				else
					m_diacriticPattern = m_diacriticPattern.Substring(0, m_diacriticPattern.Length - 1);
			}

			// Add the diacritics removed from the phone to the ones found in the diacritic
			// placeholder cluster. Also insert a dotted circle so normalization has a base
			// character, around which to normalize the diacritics. Then remove the dotted circle.
			diacritics = DataUtils.kDottedCircle + diacritics + m_diacriticPattern;
			diacritics = diacritics.Normalize(NormalizationForm.FormD);
			m_diacriticPattern = diacritics.Remove(0, 1);

			if (lastChar != '\0')
				m_diacriticPattern += lastChar.ToString();
		}

		#endregion

		#region ToString Method
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return a linquistically appropriate version of the pattern member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			string diacriticCluster = (m_diacriticPattern == null ? string.Empty :
				string.Format("[{0}{1}]", DataUtils.kDottedCircle, m_diacriticPattern));

			if (m_type == MemberType.Articulatory || m_type == MemberType.Binary)
			{
				string tmpMember = "[" + m_member + "]";
				return (m_diacriticPattern == null ? tmpMember : "[" + tmpMember + diacriticCluster + "]");
			}

			if (m_type == MemberType.AnyConsonant || m_type == MemberType.AnyVowel)
			{
				string tmpMember = (m_type == MemberType.AnyConsonant ? "[C]" : "[V]");
				return (m_diacriticPattern == null ? tmpMember : "[" + tmpMember + diacriticCluster + "]");
			}

			return m_member;
		}

		#endregion

		#region Pattern matching methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks a run of phones (i.e. phones in a word) to see if they match those in the
		/// member. Checking begins in the word at the specified start index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CompareResultType ContainsMatch(EnvironmentType envType, string[] phones,
			ref int startIndex)
		{
			int runLen = m_ipaRun.Length - 1;
			if (runLen < 0)
				return CompareResultType.NoMatch;

			// First, verify that the index into the word is where it won't under
			// or overflow if compared to the IPA character run in the pattern member.
			if (envType != EnvironmentType.Before)
			{
				if ((startIndex + runLen) >= phones.Length)
					return CompareResultType.NoMatch;
			}
			else
			{
				if ((startIndex - runLen) < 0)
					return CompareResultType.NoMatch;
			}

			// When the environment is before, we begin searching from the end of the run
			// of phones in the member as well as step backward in the word starting at
			// startIndex. Otherwise, start from the beginning of the run as well as step
			// forward through the word starting at startIndex.
			int ridx = (envType != EnvironmentType.Before ? 0 : runLen);
			int pidx = startIndex;
			int newStartIndex = pidx;
			int incAmount = (envType == EnvironmentType.Before ? -1 : 1);
			CompareResultType compareResult = CompareResultType.NoMatch;

			while (pidx >= 0 && pidx < phones.Length && ridx >= 0 && ridx <= runLen)
			{
				compareResult = ComparePhones(m_ipaRun[ridx], phones[pidx]);
				
				if (compareResult == CompareResultType.NoMatch)
					return CompareResultType.NoMatch;

				if (compareResult != CompareResultType.Ignored)
				{
					// Move to the next phone in the pattern's run and save the
					// last index where we got a match in the word we're searching.
					ridx += incAmount;
					newStartIndex = pidx;
				}

				// Move to the next phone in the word we're checking.
				pidx += incAmount;
			}

			startIndex = newStartIndex;
			return (compareResult == CompareResultType.Ignored ?
				CompareResultType.NoMatch : CompareResultType.Match);
		}

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
			Dictionary<string, IPhoneInfo> phoneCache)
		{
			if (phone == SearchEngine.kIgnoredPhone)
				return CompareResultType.Ignored;

			if (m_type == MemberType.SinglePhone)
				return ComparePhones(m_member, phone);

			if (phone.IndexOf(DataUtils.kTopTieBarC) >= 0 ||
				phone.IndexOf(DataUtils.kBottomTieBarC) >= 0)
			{
				return CheckPhoneContainingTieBar(phone);
			}

			if (phone == " ")
				return CompareResultType.NoMatch;

			// Find the phone's information in the phone cache.
			IPhoneInfo phoneInfo;
			if (!phoneCache.TryGetValue(phone, out phoneInfo))
				return CompareResultType.Error;

			CompareResultType compareResult = CompareResultType.NoMatch;

			switch (m_type)
			{
				case MemberType.AnyConsonant:
					if (phoneInfo.CharType == IPACharacterType.Consonant)
						compareResult = CompareResultType.Match;
					break;
				
				case MemberType.AnyVowel:
					if (phoneInfo.CharType == IPACharacterType.Vowel)
						compareResult = CompareResultType.Match;
					break;
			
				case MemberType.Binary:
					if ((phoneInfo.BinaryMask & m_masks[0]) != 0)
						compareResult = CompareResultType.Match;
					break;
				
				case MemberType.Articulatory:
					if (((phoneInfo.Masks[0] & m_masks[0]) != 0) ||
						((phoneInfo.Masks[1] & m_masks[1]) != 0))
					{
						compareResult = CompareResultType.Match;
					}
					break;
			}

			if (compareResult == CompareResultType.NoMatch)
			{
				return compareResult;

				////////return (SearchEngine.IgnoredList.Contains(phone) ?
				////////    CompareResultType.Ignored : CompareResultType.NoMatch);
			}

			if ((m_type == MemberType.AnyConsonant || m_type == MemberType.AnyVowel ||
				m_type == MemberType.Articulatory || m_type == MemberType.Binary) &&
				string.IsNullOrEmpty(m_diacriticPattern))
			{
				return compareResult;
			}

			return (SearchEngine.CompareDiacritics(m_diacriticPattern, phone) ?
				CompareResultType.Match : CompareResultType.NoMatch);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CompareResultType CheckPhoneContainingTieBar(string phone)
		{
			PhoneCache tmpPhoneCache = new PhoneCache();

			// Split the phone where the tie-bar is and send each remaining piece to
			// ContainMatch as though each piece were a separate phone.
			foreach (string phonePart in phone.Split(DataUtils.kTieBars,
				StringSplitOptions.RemoveEmptyEntries))
			{
				tmpPhoneCache.AddPhone(phonePart);
				
				CompareResultType compareResult = ContainsMatch(phonePart, tmpPhoneCache);
				if (compareResult == CompareResultType.Error)
					return CompareResultType.Error;

				if (compareResult == CompareResultType.Match)
					return CompareResultType.Match;
			}

			return CompareResultType.NoMatch;
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
			if (phone == SearchEngine.kIgnoredPhone)
				return CompareResultType.Ignored;

			// Take the phone from the word we're searching and
			// separate it into its base character and its diacritics.
			string basePhone;
			string phonesDiacritics;
			SearchEngine.ParsePhone(phone, out basePhone, out phonesDiacritics);

			// Check if the base characters match. If not,
			// check if the base character is ignored.
			if (patternPhone != basePhone)
			{
				return (SearchEngine.IgnoredList.Contains(basePhone) ?
					CompareResultType.Ignored : CompareResultType.NoMatch);
			}

			bool match = SearchEngine.CompareDiacritics(m_diacriticPattern,
				phonesDiacritics == null ? null : phonesDiacritics, false);

			return (match ? CompareResultType.Match : CompareResultType.NoMatch);
		}

		#endregion
	}
}
