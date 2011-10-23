using System.Collections.Generic;
using System.Diagnostics;
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
	public class PhoneInfo : IPhoneInfo, IFeatureBearer
	{
		private PaProject _project;
		private string _moaKey;
		private string _poaKey;
		private char _baseChar = '\0';
		private FeatureMask _aMask;
		private FeatureMask _bMask;
		private FeatureMask _defaultAMask;
		private FeatureMask _defaultBMask;

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
			_project = project;
			SiblingUncertainties = new List<string>();
			CharType = IPASymbolType.Unknown;
			Phone = phone;
			IsUndefined = isUndefined;

			if (!string.IsNullOrEmpty(phone))
				InitializeBaseChar(phone);
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
				var charInfo = InventoryHelper.IPASymbolCache[c];
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
				_baseChar = lastChar.Literal[0];
				CharType = IPASymbolType.Consonant;
			}
			else
			{
				// The sequence of base char. symbols are not all consonants,
				// so use the first symbol as the base character.
				_baseChar = firstChar.Literal[0];
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
			if (_project.AmbiguousSequences == null)
				return false;

			var ambigSeq = _project.AmbiguousSequences.GetAmbiguousSeq(phone, true);

			if (ambigSeq != null)
			{
				var charInfo = InventoryHelper.IPASymbolCache[ambigSeq.BaseChar];
				if (charInfo != null)
				{
					_baseChar = ambigSeq.BaseChar[0];
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
			var clone = new PhoneInfo(_project, Phone);
			clone._project = _project;
			clone.Description = Description;
			clone.TotalCount = TotalCount;
			clone.CountAsNonPrimaryUncertainty = CountAsNonPrimaryUncertainty;
			clone.CountAsPrimaryUncertainty = CountAsPrimaryUncertainty;
			clone.CharType = CharType;
			clone._moaKey = MOAKey;
			clone._poaKey = POAKey;
			clone._baseChar = _baseChar;
			clone.SiblingUncertainties = new List<string>(SiblingUncertainties);
			clone.IsUndefined = IsUndefined;
			clone._aMask = AMask.Clone();
			clone._bMask = BMask.Clone();
			clone._defaultAMask = DefaultAMask.Clone();
			clone._defaultBMask = DefaultBMask.Clone();

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
			get { return _baseChar; }
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
		public void SetAFeatures(IEnumerable<string> featureNames)
		{
			Debug.Assert(featureNames != null);
			var list = featureNames.ToList();
			Debug.Assert(list.Count > 0);

			_aMask = App.AFeatureCache.GetMask(list);
		}

		/// ------------------------------------------------------------------------------------
		public void SetBFeatures(IEnumerable<string> featureNames)
		{
			Debug.Assert(featureNames != null);
			var list = featureNames.ToList();
			Debug.Assert(list.Count > 0);

			_bMask = App.BFeatureCache.GetMask(list);
		}

		/// ------------------------------------------------------------------------------------
		public void SetDefaultAFeatures(IEnumerable<string> featureNames)
		{
			_defaultAMask = null;

			if (featureNames == null)
				return;

			var list = featureNames.ToList();

			if (list.Count > 0)
				_defaultAMask = App.AFeatureCache.GetMask(list);
		}

		/// ------------------------------------------------------------------------------------
		public void SetDefaultBFeatures(IEnumerable<string> featureNames)
		{
			_defaultBMask = null;

			if (featureNames == null)
				return;

			var list = featureNames.ToList();

			if (list.Count > 0)
				_defaultBMask = App.BFeatureCache.GetMask(list);
		}

		/// ------------------------------------------------------------------------------------
		public void ResetAFeatures()
		{
			if (HasAFeatureOverrides)
				AMask = DefaultAMask.Clone();
		}

		/// ------------------------------------------------------------------------------------
		public void ResetBFeatures()
		{
			if (HasBFeatureOverrides)
				BMask = DefaultBMask.Clone();
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureMask DefaultAMask
		{
			get { return _defaultAMask ?? _aMask; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureMask DefaultBMask
		{
			get { return _defaultBMask ?? _bMask; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureMask AMask
		{
			get { return _aMask ?? App.AFeatureCache.GetEmptyMask(); }
			set { _aMask = (value ?? App.AFeatureCache.GetEmptyMask()); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureMask BMask
		{
			get { return _bMask ?? App.AFeatureCache.GetEmptyMask(); }
			set { _bMask = (value ?? App.BFeatureCache.GetEmptyMask()); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of articulatory features for the phone. This is only used
		/// for phones whose features are overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("articulatoryFeatures"), XmlArrayItem("feature")]
		public List<string> AFeatureNames
		{
			get { return App.AFeatureCache.GetFeatureList(_aMask).ToList(); }
			set { SetAFeatures(value); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of binary features for the phone. This is only used
		/// for phones whose features are overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("binaryFeatures"), XmlArrayItem("feature")]
		public List<string> BFeatureNames
		{
			get { return App.BFeatureCache.GetFeatureList(_bMask).ToList(); }
			set { SetBFeatures(value); }
		}

		/// ------------------------------------------------------------------------------------
		public bool HasAFeatureOverrides
		{
			get { return (AMask != DefaultAMask); }
		}

		/// ------------------------------------------------------------------------------------
		public bool HasBFeatureOverrides
		{
			get { return (BMask != DefaultBMask); }
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

				if (_moaKey == null)
				{
					// If we don't get a key back, then set the key to an empty string which
					// will tell us in future references to this property that a failed attempt
					// was already made to get the key. Therefore, the program will not keep
					// trying and failing. Thus wasting processing time.
					_moaKey = App.GetMOAKey(Phone) ?? string.Empty;
				}

				return (_moaKey == string.Empty ? null : _moaKey);
			}
			set { _moaKey = value; }
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

				if (_poaKey == null)
				{
					// When we don't get a key back, then set the key to an empty string which
					// will tell us in future references to this property that a failed attempt
					// was already made to get the key. Therefore, the program will not keep
					// trying and failing. Thus wasting processing time.
					_poaKey = App.GetPOAKey(Phone) ?? string.Empty;
				}

				return (_poaKey == string.Empty ? null : _poaKey);
			}
			set { _poaKey = value; }
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
			return Phone.Select(c => InventoryHelper.IPASymbolCache[c]);
		}
	}

	#endregion
}
