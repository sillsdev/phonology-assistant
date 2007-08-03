using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace SIL.Pa.Data
{
	#region PhoneCache class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneCache : Dictionary<string, IPhoneInfo>
	{
		public const string kDefaultChartSupraSegsToIgnore = "\u0300\u0301\u0302\u0304" +
			"\u0306\u030b\u030c\u030f\u1dc4\u1dc5\u1dc8\u203f";

		private static List<CVPatternInfo> s_cvPatternInfoList;
		private static AmbiguousSequences s_ambiguousSeqList = null;
		private static PhoneFeatureOverrides s_featureOverrides = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the IPhoneInfo object for the specified key.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new IPhoneInfo this[string key]
		{
			get
			{
				IPhoneInfo phoneInfo;
				return (TryGetValue(key, out phoneInfo) ? phoneInfo : null);
			}
			set {base[key] = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds to the phone cache, information about the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddPhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				PhoneInfo phoneInfo = new PhoneInfo(phone);
				this[phone] = phoneInfo;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddUndefinedPhone(string phone)
		{
			IPhoneInfo phoneInfo;
			if (!TryGetValue(phone, out phoneInfo))
				this[phone] = new PhoneInfo(phone, true);
			else
			{
				if (phoneInfo is PhoneInfo)
					(phoneInfo as PhoneInfo).IsUndefined = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Process the CV pattern base character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string ProcessCVBaseChar(string phone)
		{
			bool containsBase = false;
			StringBuilder stbCvBase = new StringBuilder();

			IPhoneInfo phoneInfo = this[phone];

			if (s_cvPatternInfoList != null)
			{
				// Is it a CVPatternInfo display base character?
				foreach (CVPatternInfo info in s_cvPatternInfoList)
				{
					if (info.IsBase && phone == info.Phone)
					{
						containsBase = true;
						stbCvBase.Append(phone);
						break;
					}
				}
			}

			if (!containsBase)
			{
				if (phoneInfo.CharType == IPACharacterType.Consonant)
					stbCvBase.Append('C');
				else if (phoneInfo.CharType == IPACharacterType.Vowel)
					stbCvBase.Append('V');
				else if (phoneInfo.CharType == IPACharacterType.Breaking)
					stbCvBase.Append(' ');
			}
			return stbCvBase.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CV pattern for the specified phonetic string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCVPattern(string phonetic)
		{
			return GetCVPattern(DataUtils.IPACharCache.PhoneticParser(phonetic, true));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CV pattern for the specified array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCVPattern(string[] phones)
		{
			if (phones == null || phones.Length == 0)
				return null;

			StringBuilder stbPattern = new StringBuilder();
			foreach (string phone in phones)
			{
				// Add the phone to the cache if there isn't one.
				if (!ContainsKey(phone))
					AddPhone(phone);

				IPACharInfo charInfo = DataUtils.IPACharCache[phone];
				bool containsModifier = false;

				if (phone.Length > 1)
				{
					StringBuilder stbPattern2 = new StringBuilder();
					for (int i = phone.Length - 1; i >= 0; i--)
					{
						string phoneStr = phone[i].ToString();

						if (s_cvPatternInfoList != null)
						{
							// Is it a modifier?
							foreach (CVPatternInfo info in s_cvPatternInfoList)
							{
								if (!info.IsBase && phoneStr == info.Phone)
								{
									containsModifier = true;
									stbPattern2.Insert(0, phoneStr);
									break;
								}
							}
						}

						if (!containsModifier && charInfo != null && charInfo.IsBaseChar)
						{
							stbPattern2.Insert(0, ProcessCVBaseChar(phone));
							break;
						}

						containsModifier = false;
					}
					stbPattern.Append(stbPattern2);
				}
				else
				{
					if (charInfo != null && charInfo.IsBaseChar)
						stbPattern.Append(ProcessCVBaseChar(phone));
				}
			}

			return stbPattern.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Consonants
		{
			get {return GetTypeOfPhones(IPACharacterType.Consonant);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CommaDelimitedConsonants
		{
			get {return GetCommaDelimitedPhones(IPACharacterType.Consonant);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Vowels
		{
			get { return GetTypeOfPhones(IPACharacterType.Vowel); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CommaDelimitedVowels
		{
			get { return GetCommaDelimitedPhones(IPACharacterType.Vowel); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of phones in the cache of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string[] GetTypeOfPhones(IPACharacterType type)
		{
			List<string> phones = new List<string>();

			foreach (KeyValuePair<string, IPhoneInfo> kvp in this)
			{
				if (kvp.Value.CharType == type)
					phones.Add(kvp.Key);
			}

			return phones.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of phones in the form of a comma delimited list that are found in
		/// the cache and are of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetCommaDelimitedPhones(IPACharacterType type)
		{
			string[] phones = GetTypeOfPhones(type);

			StringBuilder bldr = new StringBuilder();
			foreach (string phone in phones)
			{
				bldr.Append(phone);
				bldr.Append(",");
			}

			// Get rid of the last comma.
			if (bldr.Length > 0)
				bldr.Length--;

			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of phones having the specified feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCommaDelimitedPhonesInFeature(string featureName)
		{
			Debug.Assert(featureName != null);

			featureName = featureName.Trim().ToLower();
			featureName = featureName.Replace("[", string.Empty);
			featureName = featureName.Replace("]", string.Empty);

			if (featureName.Length == 0)
				return string.Empty;

			StringBuilder bldr = new StringBuilder();

			bool isPlus = (featureName[0] == '+');
			bool isMinus = (featureName[0] == '-');

			if (isPlus || isMinus)
			{
				BFeature bfeature = DataUtils.BFeatureCache[featureName];
				if (bfeature != null)
				{
					foreach (KeyValuePair<string, IPhoneInfo> kvp in this)
					{
						if ((isPlus && (bfeature.PlusMask & kvp.Value.BinaryMask) > 0) ||
							(isMinus && (bfeature.MinusMask & kvp.Value.BinaryMask) > 0))
						{
							bldr.Append(kvp.Key);
							bldr.Append(',');
						}
					}
				}
			}
			else
			{
				AFeature afeature = DataUtils.AFeatureCache[featureName];
				if (afeature != null)
				{
					foreach (KeyValuePair<string, IPhoneInfo> kvp in this)
					{
						if ((kvp.Value.Masks[afeature.MaskNumber] & afeature.Mask) > 0)
						{
							bldr.Append(kvp.Key);
							bldr.Append(',');
						}
					}
				}
			}

			// Get rid of the last comma.
			if (bldr.Length > 0)
				bldr.Length--;

			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the CVPatternInfoList for the phone cache. This list should be set to the
		/// list owned by a PA project when the project is opened.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<CVPatternInfo> CVPatternInfoList
		{
			get { return s_cvPatternInfoList; }
			set { s_cvPatternInfoList = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of phones whose features should be overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PhoneFeatureOverrides FeatureOverrides
		{
			get { return s_featureOverrides; }
			set { s_featureOverrides = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of ambiguous sequences used while adding phones to the cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AmbiguousSequences AmbiguousSequences
		{
			get { return s_ambiguousSeqList; }
			set { s_ambiguousSeqList = value; }
		}
	}

	#endregion

	#region PhoneInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A class defining an object to store the information for a single phonetic character.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneInfo : IPhoneInfo
	{
		private int m_totalCount = 0;
		private int m_countAsNonPrimaryUncertainty = 0;
		private int m_countAsPrimaryUncertainty = 0;
		private IPACharacterType m_charType = IPACharacterType.Unknown;
		private ulong m_binaryMask;
		private ulong[] m_masks = new ulong[] { 0, 0 }; 
		private List<string> m_siblingUncertainties = new List<string>();
		private string m_moaKey;
		private string m_poaKey;
		private char m_baseChar = '\0';
		private string m_phone;
		private bool m_isUndefined = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new phone information object for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfo()
		{
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new phone information object for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfo(string phone) : this(phone, false) 
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new phone information object for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal PhoneInfo(string phone, bool isUndefined)
		{
			m_phone = phone;
			m_isUndefined = isUndefined;

			if (string.IsNullOrEmpty(phone))
				return;

			ulong bmask = 0;
			ulong mask0 = 0;
			ulong mask1 = 0;

			bool phoneIsAmbiguous = CheckIfAmbiguous(phone);
			bool phoneFeaturesOverridden = ShouldFeaturesBeOverridden(phone);

			if (!phoneIsAmbiguous || !phoneFeaturesOverridden)
			{
				// Go through each codepoint of the phone, building the feature masks along the way.
				for (int i = phone.Length - 1; i >= 0; i--)
				{
					IPACharInfo charInfo = DataUtils.IPACharCache[phone[i]];
					if (charInfo != null)
					{
						// This will make the final base char in the phone the one that determines
						// what type of phone this is. If the phone is an ambiguous sequence, then
						// it has already had it's character type and base character specified.
						if (!phoneIsAmbiguous && charInfo.IsBaseChar)
						{
							m_charType = charInfo.CharType;
							m_baseChar = phone[i];
						}

						if (!phoneFeaturesOverridden)
						{
							bmask |= charInfo.BinaryMask;
							mask0 |= charInfo.Mask0;
							mask1 |= charInfo.Mask1;
						}
					}
				}
			}

			if (!phoneFeaturesOverridden)
			{
				m_masks = new ulong[] { mask0, mask1 };
				m_binaryMask = bmask;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if the specified phone is in the list of ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CheckIfAmbiguous(string phone)
		{
			if (DataUtils.IPACharCache.AmbiguousSequences == null)
				return false;

			AmbiguousSeq ambigSeq =
				DataUtils.IPACharCache.AmbiguousSequences.GetAmbiguousSeq(phone, true);
			
			if (ambigSeq != null)
			{
				IPACharInfo charInfo = DataUtils.IPACharCache[ambigSeq.BaseChar];
				if (charInfo != null)
				{
					m_baseChar = ambigSeq.BaseChar[0];
					m_charType = charInfo.CharType;
					return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the features for the specified phone should be overridden
		/// by those specified in the PhoneFeatureOverrides list. If so, the masks are set
		/// from that list rather than determined by the features found in the IPACharCache
		/// for each of the phone's codepoints.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ShouldFeaturesBeOverridden(string phone)
		{
			if (PhoneCache.FeatureOverrides == null)
				return false;

			IPhoneInfo phoneInfo = PhoneCache.FeatureOverrides[phone];
			if (phoneInfo == null)
				return false;

			m_masks = new ulong[] {phoneInfo.Masks[0], phoneInfo.Masks[1]};
			m_binaryMask = phoneInfo.BinaryMask;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a clone of the phone information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPhoneInfo Clone()
		{
			PhoneInfo clone = new PhoneInfo(m_phone);
			clone.m_totalCount = m_totalCount;
			clone.m_countAsNonPrimaryUncertainty = m_countAsNonPrimaryUncertainty;
			clone.m_countAsPrimaryUncertainty = m_countAsPrimaryUncertainty;
			clone.m_charType = m_charType;
			clone.m_binaryMask = m_binaryMask;
			clone.m_masks = new ulong[] {m_masks[0], m_masks[1]};
			clone.m_moaKey = m_moaKey;
			clone.m_poaKey = m_poaKey;
			clone.m_baseChar = m_baseChar;
			clone.m_siblingUncertainties = new List<string>(m_siblingUncertainties);
			clone.m_isUndefined = m_isUndefined;

			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the phone associated with the object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_phone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This property is only exposed for PhoneInfo instances that are included in
		/// collections other than a PhoneCache (e.g. list of phones whose features are
		/// overridden.) and, even then, it is used mainly for XML
		/// serialization/deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Phone
		{
			get { return m_phone; }
			set { m_phone = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This property is only exposed for PhoneInfo instances that are included in
		/// collections other than a PhoneCache (e.g. list of ambiguous sequences) and, even
		/// then, it is used mainly for XML serialization/deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public char BaseCharacter
		{
		    get { return m_baseChar; }
		    set { ; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of phones found in the same uncertain group(s) with the phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> SiblingUncertainties
		{
			get { return m_siblingUncertainties; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when not found in an uncertain group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int TotalCount
		{
			get { return m_totalCount; }
			set { m_totalCount = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when found as the non primary phone
		/// in a group of uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int CountAsNonPrimaryUncertainty
		{
			get { return m_countAsNonPrimaryUncertainty; }
			set { m_countAsNonPrimaryUncertainty = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when found as the primary phone
		/// (i.e. the first in group) in a group of uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int CountAsPrimaryUncertainty
		{
			get { return m_countAsPrimaryUncertainty; }
			set { m_countAsPrimaryUncertainty = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPACharacterType CharType
		{
			get { return m_charType; }
			set { m_charType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("BMask")]
		public ulong BinaryMask
		{
			get { return m_binaryMask; }
			set { m_binaryMask = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the articulatory feature masks for the phonetic character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("AMasks")]
		public ulong[] Masks
		{
			get {return m_masks;}
			set {m_masks = new ulong[] {value[0], value[1] };}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the phones manner of articulation 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string MOAKey
		{
			get
			{
				if (m_isUndefined)
					return "000";

				if (m_moaKey == null)
				{
					m_moaKey = DataUtils.GetMOAKey(m_phone);

					// When we don't get a key back, then set the key to an empty string which
					// will tell us in future references to this property that a failed attempt
					// was already made to get the key. Therefore, the program will not keep
					// trying and failing. Thus wasting processing time.
					if (m_moaKey == null)
						m_moaKey = string.Empty;
				}

				return (m_moaKey == string.Empty ? null : m_moaKey);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the phones point of articulation 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string POAKey
		{
			get
			{
				if (m_isUndefined)
					return "000";

				if (m_poaKey == null)
				{
					m_poaKey = DataUtils.GetPOAKey(m_phone);

					// When we don't get a key back, then set the key to an empty string which
					// will tell us in future references to this property that a failed attempt
					// was already made to get the key. Therefore, the program will not keep
					// trying and failing. Thus wasting processing time.
					if (m_poaKey == null)
						m_poaKey = string.Empty;
				}

				return (m_poaKey == string.Empty ? null : m_poaKey);
			}
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the phone is a character that isn't found
		/// in the phonetic character inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsUndefined
		{
			get { return m_isUndefined; }
			internal set { m_isUndefined = true; }
		}
	}

	#endregion

	#region CVPatternInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A class 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CVPatternInfo
	{
		private string m_phone;
		private IPACharIgnoreTypes m_patternType = 0;
		private bool m_isBase = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets m_cvPatternPhone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Phone
		{
			get { return m_phone; }
			set { m_phone = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets m_patternType.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Type")]
		public IPACharIgnoreTypes PatternType
		{
			get { return m_patternType; }
			set { m_patternType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets m_isBase.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool IsBase
		{
			get { return m_isBase; }
			set { m_isBase = value; }
		}
	}
	#endregion
}
