// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: PhoneInfo.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SIL.Pa.Data
{
	#region PhoneInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A class defining an object to store the information for a single phonetic character.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneInfo : IPhoneInfo
	{
		private int m_totalCount;
		private int m_countAsNonPrimaryUncertainty;
		private int m_countAsPrimaryUncertainty;
		private IPACharacterType m_charType = IPACharacterType.Unknown;
		private List<string> m_siblingUncertainties = new List<string>();
		private string m_moaKey;
		private string m_poaKey;
		private char m_baseChar = '\0';
		private string m_phone;
		private bool m_isUndefined;
		private bool m_featuresOverridden;
		
		private List<string> m_aFeatures;
		private List<string> m_bFeatures;
		private FeatureMask m_aMask;
		private FeatureMask m_bMask;

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

			bool phoneIsAmbiguous = CheckIfAmbiguous(phone);
			m_featuresOverridden = ShouldFeaturesBeOverridden(phone);

			if (phoneIsAmbiguous && m_featuresOverridden)
				return;

			m_aMask = DataUtils.AFeatureCache.GetEmptyMask();
			m_bMask = DataUtils.BFeatureCache.GetEmptyMask();

			// Go through each codepoint of the phone, building the feature masks along the way.
			foreach (char c in phone)
			{
				IPACharInfo charInfo = DataUtils.IPACharCache[c];
				if (charInfo == null)
					continue;

				// This will make the final base char in the phone the one that determines
				// what type of phone this is. If the phone is an ambiguous sequence, then
				// it has already had it's character type and base character specified.
				if (!phoneIsAmbiguous && charInfo.IsBaseChar)
				{
					m_charType = charInfo.CharType;
					m_baseChar = c;
				}

				if (!m_featuresOverridden)
				{
					m_aMask |= charInfo.AMask;
					m_bMask |= charInfo.BMask;
				}
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

			m_aMask = phoneInfo.AMask.Clone();
			m_bMask = phoneInfo.BMask.Clone();
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
			clone.m_moaKey = m_moaKey;
			clone.m_poaKey = m_poaKey;
			clone.m_baseChar = m_baseChar;
			clone.m_siblingUncertainties = new List<string>(m_siblingUncertainties);
			clone.m_isUndefined = m_isUndefined;
			clone.m_featuresOverridden = m_featuresOverridden;
			clone.m_aMask = m_aMask.Clone();
			clone.m_bMask = m_bMask.Clone();

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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool FeaturesAreOverridden
		{
			get { return m_featuresOverridden; }
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
			set { }
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
		/// Gets or sets the articulatory features mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureMask AMask
		{
			get
			{
				if (m_aMask == null || m_aMask.IsEmpty)
				{
					m_aMask = DataUtils.AFeatureCache.GetMask(m_aFeatures);
					if (m_aFeatures != null && m_aFeatures.Count > 0)
						m_aFeatures = null;
				}

				return m_aMask;
			}
			set { m_aMask = (value ?? DataUtils.AFeatureCache.GetEmptyMask()); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the binary features mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureMask BMask
		{
			get
			{
				if (m_bMask == null || m_bMask.IsEmpty)
				{
					m_bMask = DataUtils.BFeatureCache.GetMask(m_bFeatures);
					if (m_bFeatures != null && m_bFeatures.Count > 0)
						m_bFeatures = null;
				}

				return m_bMask;
			}
			set { m_bMask = (value ?? DataUtils.BFeatureCache.GetEmptyMask()); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of articulatory features for the phone. This is only used
		/// for phones whose features are overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("ArticulatoryFeatures")]
		public List<string> AFeatures
		{
			get
			{
				return (m_aFeatures == null && m_aMask != null && !m_aMask.IsEmpty ?
					DataUtils.AFeatureCache.GetFeatureList(m_aMask) : m_aFeatures);
			}
			set { m_aFeatures = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of binary features for the phone. This is only used
		/// for phones whose features are overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("BinaryFeatures")]
		public List<string> BFeatures
		{
			get
			{
				return (m_bFeatures == null && m_bMask != null && !m_bMask.IsEmpty ?
					DataUtils.BFeatureCache.GetFeatureList(m_bMask) : m_bFeatures);
			}
			set { m_bFeatures = value; }
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
					// If we don't get a key back, then set the key to an empty string which
					// will tell us in future references to this property that a failed attempt
					// was already made to get the key. Therefore, the program will not keep
					// trying and failing. Thus wasting processing time.
					m_moaKey = DataUtils.GetMOAKey(m_phone) ?? string.Empty;
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
					// When we don't get a key back, then set the key to an empty string which
					// will tell us in future references to this property that a failed attempt
					// was already made to get the key. Therefore, the program will not keep
					// trying and failing. Thus wasting processing time.
					m_poaKey = DataUtils.GetPOAKey(m_phone) ?? string.Empty;
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
			internal set { m_isUndefined = value; }
		}
	}

	#endregion
}
