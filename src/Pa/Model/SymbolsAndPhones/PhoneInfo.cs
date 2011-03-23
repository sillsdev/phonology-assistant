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
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SIL.Pa.Model
{
	#region PhoneInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A class defining an object to store the information for a single phonetic character.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneInfo : IPhoneInfo
	{
		private string m_moaKey;
		private string m_poaKey;
		private char m_baseChar = '\0';

		private List<string> m_aFeatures;
		private List<string> m_bFeatures;
		private FeatureMask m_aMask;
		private FeatureMask m_bMask;

		private PaProject m_project;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new phone information object for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfo()
		{
			SiblingUncertainties = new List<string>();
			CharType = IPASymbolType.Unknown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new phone information object for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfo(PaProject project, string phone) : this(project, phone, false)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new phone information object for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfo(PaProject project, string phone, bool isUndefined)
		{
			m_project = project;
			SiblingUncertainties = new List<string>();
			CharType = IPASymbolType.Unknown;
			Phone = phone;
			IsUndefined = isUndefined;

			if (!string.IsNullOrEmpty(phone))
			{
				InitializeFeatureMasks(phone);
				InitializeBaseChar(phone);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFeatureMasks(IEnumerable<char> phone)
		{
			m_aMask = DefaultAMask = App.AFeatureCache.GetEmptyMask();
			m_bMask = DefaultBMask = App.BFeatureCache.GetEmptyMask();

			// Go through each codepoint of the phone, building the feature masks along the way.
			foreach (var ci in phone.Select(ci => App.IPASymbolCache[ci]).Where(ci => ci != null))
			{
				m_aMask |= ci.AMask;
				m_bMask |= ci.BMask;
				DefaultAMask |= ci.AMask;
				DefaultBMask |= ci.BMask;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeBaseChar(string phone)
		{
			if (CheckIfAmbiguous(phone))
				return;

			var bldr = new StringBuilder();
			IPASymbol firstChar = null;
			IPASymbol lastChar = null;

			foreach (char c in phone)
			{
				var charInfo = App.IPASymbolCache[c];
				if (charInfo != null && charInfo.IsBase)
				{
					if (charInfo.Type == IPASymbolType.Consonant)
						bldr.Append('c');
					else if (charInfo.Type == IPASymbolType.Vowel)
						bldr.Append('v');

					if (firstChar == null)
						firstChar = charInfo;

					lastChar = charInfo;
				}
			}

			if (bldr.Length == 0)
			{
				if (firstChar != null && CharType == IPASymbolType.Unknown)
					CharType = firstChar.Type;

				return;
			}

			if (bldr.Replace("c", string.Empty).Length == 0)
			{
				// When the sequence of base char. symbols are all consonants,
				// then use the last symbol as the base character.
				m_baseChar = lastChar.Literal[0];
				CharType = IPASymbolType.Consonant;
			}
			else
			{
				// The sequence of base char. symbols are not all consonants,
				// so use the first symbol as the base character.
				m_baseChar = firstChar.Literal[0];
				CharType = IPASymbolType.Vowel;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if the specified phone is in the list of ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CheckIfAmbiguous(string phone)
		{
			if (m_project.AmbiguousSequences == null)
				return false;

			var ambigSeq = m_project.AmbiguousSequences.GetAmbiguousSeq(phone, true);

			if (ambigSeq != null)
			{
				var charInfo = App.IPASymbolCache[ambigSeq.BaseChar];
				if (charInfo != null)
				{
					m_baseChar = ambigSeq.BaseChar[0];
					CharType = charInfo.Type;
					return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a clone of the phone information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPhoneInfo Clone()
		{
			var clone = new PhoneInfo(m_project, Phone);
			clone.m_project = m_project;
			clone.Description = Description;
			clone.TotalCount = TotalCount;
			clone.CountAsNonPrimaryUncertainty = CountAsNonPrimaryUncertainty;
			clone.CountAsPrimaryUncertainty = CountAsPrimaryUncertainty;
			clone.CharType = CharType;
			clone.m_moaKey = MOAKey;
			clone.m_poaKey = POAKey;
			clone.m_baseChar = m_baseChar;
			clone.SiblingUncertainties = new List<string>(SiblingUncertainties);
			clone.IsUndefined = IsUndefined;
			clone.AFeaturesAreOverridden = AFeaturesAreOverridden;
			clone.BFeaturesAreOverridden = BFeaturesAreOverridden;
			clone.m_aMask = AMask.Clone();
			clone.m_bMask = BMask.Clone();
			clone.DefaultAMask = DefaultAMask;
			clone.DefaultBMask = DefaultBMask;

			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the phone associated with the object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Phone + (string.IsNullOrEmpty(Description) ? string.Empty : ": " + Description);
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
		public string Phone { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("articulatoryFeaturesChanged")]
		public bool AFeaturesAreOverridden { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("binaryFeaturesChanged")]
		public bool BFeaturesAreOverridden { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Description { get; set; }

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
		public List<string> SiblingUncertainties { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when not found in an uncertain group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int TotalCount { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when found as the non primary phone
		/// in a group of uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int CountAsNonPrimaryUncertainty { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of times the phone occurs in the data when found as the primary phone
		/// (i.e. the first in group) in a group of uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int CountAsPrimaryUncertainty { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPASymbolType CharType { get; set; }

		/// ------------------------------------------------------------------------------------
		public void SetAFeatures(List<string> list)
		{
			m_aMask = null;
			AFeatures = list;
		}

		/// ------------------------------------------------------------------------------------
		public void SetBFeatures(List<string> list)
		{
			m_bMask = null;
			BFeatures = list;
		}

		/// ------------------------------------------------------------------------------------
		public void OverrideAFeature(FeatureMask mask)
		{
			if (AMask != mask && !mask.IsEmpty && mask.IsAnyBitSet)
			{
				AMask = mask.Clone();
				AFeaturesAreOverridden = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void OverrideBFeature(FeatureMask mask)
		{
			if (BMask != mask && !mask.IsEmpty && mask.IsAnyBitSet)
			{
				BMask = mask.Clone();
				BFeaturesAreOverridden = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void ResetAFeatures()
		{
			AMask = DefaultAMask;
			AFeaturesAreOverridden = false;
		}

		/// ------------------------------------------------------------------------------------
		public void ResetBFeatures()
		{
			BMask = DefaultBMask;
			BFeaturesAreOverridden = false;
		}

		[XmlIgnore]
		public FeatureMask DefaultAMask { get; private set; }

		[XmlIgnore]
		public FeatureMask DefaultBMask { get; private set; }

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
					m_aMask = App.AFeatureCache.GetMask(m_aFeatures);
					if (m_aFeatures != null && m_aFeatures.Count > 0)
						m_aFeatures = null;
				}

				return m_aMask;
			}
			set { m_aMask = (value ?? App.AFeatureCache.GetEmptyMask()); }
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
					m_bMask = App.BFeatureCache.GetMask(m_bFeatures);
					if (m_bFeatures != null && m_bFeatures.Count > 0)
						m_bFeatures = null;
				}

				return m_bMask;
			}
			set { m_bMask = (value ?? App.BFeatureCache.GetEmptyMask()); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of articulatory features for the phone. This is only used
		/// for phones whose features are overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("articulatoryFeatures"), XmlArrayItem("feature")]
		public List<string> AFeatures
		{
			get
			{
				return (m_aFeatures == null && m_aMask != null && !m_aMask.IsEmpty ?
					App.AFeatureCache.GetFeatureList(m_aMask) : m_aFeatures);
			}
			set { m_aFeatures = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of binary features for the phone. This is only used
		/// for phones whose features are overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("binaryFeatures"), XmlArrayItem("feature")]
		public List<string> BFeatures
		{
			get
			{
				return (m_bFeatures == null && m_bMask != null && !m_bMask.IsEmpty ?
					App.BFeatureCache.GetFeatureList(m_bMask) : m_bFeatures);
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
				if (IsUndefined)
					return "000";

				if (m_moaKey == null)
				{
					// If we don't get a key back, then set the key to an empty string which
					// will tell us in future references to this property that a failed attempt
					// was already made to get the key. Therefore, the program will not keep
					// trying and failing. Thus wasting processing time.
					m_moaKey = App.GetMOAKey(Phone) ?? string.Empty;
				}

				return (m_moaKey == string.Empty ? null : m_moaKey);
			}
			set { m_moaKey = value; }
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
				if (IsUndefined)
					return "000";

				if (m_poaKey == null)
				{
					// When we don't get a key back, then set the key to an empty string which
					// will tell us in future references to this property that a failed attempt
					// was already made to get the key. Therefore, the program will not keep
					// trying and failing. Thus wasting processing time.
					m_poaKey = App.GetPOAKey(Phone) ?? string.Empty;
				}

				return (m_poaKey == string.Empty ? null : m_poaKey);
			}
			set { m_poaKey = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the phone is a character that isn't found
		/// in the phonetic character inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsUndefined { get; internal set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the symbols (or codepoints) of which the phone consists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<IPASymbol> GetSymbols()
		{
			return Phone.Select(c => App.IPASymbolCache[c]);
		}
	}

	#endregion
}
