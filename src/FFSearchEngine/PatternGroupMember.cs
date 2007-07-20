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

		// This variable is only set when the member's type is SinglePhone or IPACharacterRun
		// and is only used to the ToString() method can properly display the member as it
		// was before the phone was stripped of it's diacritics.
		private string m_singlePhoneForToString = null;

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
			internal set {m_type = value;}
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
		public PatternGroupMember[] CloseMember()
		{
			m_member = m_memberBuilder.ToString();
			m_memberBuilder = null;

			if (m_member == "C" || m_member == "V")
			{
				m_type = (m_member[0] == 'C' ? MemberType.AnyConsonant : MemberType.AnyVowel);
				
				// Stip off the C or V from the member's text.
				m_member = m_member.Substring(1);
				return null;
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
					return ClosePhoneRunMember();
			}

			return null;
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
			List<PatternGroupMember> memberPhones = new List<PatternGroupMember>();

			string[] phones = DataUtils.IPACharCache.PhoneticParser(m_member, true);
			if (phones == null || phones.Length == 0)
				return null;

			// First wrap up this member using the first phone in the run.
			CloseSinglePhoneMemeber(phones[0]);
			memberPhones.Add(this);

			// Now go through any following phones in the run and create new members for them.
			for (int i = 1; i < phones.Length; i++)
			{
				PatternGroupMember member = new PatternGroupMember();
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
			SearchEngine.ParsePhone(phone, out basePhone, out diacritics);

			// Save the phone with all its diacritics stripped off.
			m_member = basePhone;
			m_type = MemberType.SinglePhone;
			
			if (string.IsNullOrEmpty(diacritics))
				return;

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
				{
					m_diacriticPattern =
						m_diacriticPattern.Substring(0, m_diacriticPattern.Length - 1);
				}
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
		/// Return a displayable version of the pattern member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			if (m_singlePhoneForToString != null)
				return m_singlePhoneForToString;

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
			//// Check if the phone is ignored, making sure the current member is not in
			//// the ignored list. If the current member is in the ignored list, then
			//// don't ignore it because it has been explicitly included in the pattern.
			//if (SearchEngine.IgnoredPhones.Contains(phone) &&
			//    !SearchEngine.IgnoredPhones.Contains(m_member))
			//    return CompareResultType.Ignored;

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
				return compareResult;

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
			// First check if phone is ignored.
			// Check if the phone is ignored, making sure the current member is not in
			// the ignored list. If the current member is in the ignored list, then
			// don't ignore it because it has been explicitly included in the pattern.
			if (SearchEngine.IgnoredPhones.Contains(phone) &&
				!SearchEngine.IgnoredPhones.Contains(m_member))
				return CompareResultType.Ignored;

			// Take the phone from the word we're searching and
			// separate it into its base character and its diacritics.
			string basePhone;
			string phonesDiacritics;
			SearchEngine.ParsePhone(phone, out basePhone, out phonesDiacritics);

			// Check if the base characters match.
			if (patternPhone != basePhone)
				return CompareResultType.NoMatch;

			bool match = SearchEngine.CompareDiacritics(m_diacriticPattern,
				phonesDiacritics == null ? null : phonesDiacritics, false);

			return (match ? CompareResultType.Match : CompareResultType.NoMatch);
		}

		#endregion
	}
}
