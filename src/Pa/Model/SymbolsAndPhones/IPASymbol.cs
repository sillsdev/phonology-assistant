using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public interface IFeatureBearer
	{
		FeatureMask AMask { get; set; }
		FeatureMask BMask { get; set; }
		void ResetAFeatures();
		void ResetBFeatures();
	}

	#region IPASymbol class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Stores information about IPA characters.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("symbolDefinition")]
	public class IPASymbol : IFeatureBearer
	{
		private static int invalidDecimalVal = -1;

		[XmlIgnore]
		public bool IsUndefined { get; set; }

		[XmlIgnore]
		public int Decimal { get; private set; }
		
		[XmlAttribute("literal")]
		public string Literal { get; set; }

		[XmlAttribute("IPA")]
		public string IPANumber { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("usage")]
		public IPASymbolUsage Usage { get; set; }

		[XmlElement("description")]
		public string Description { get; set; }

		[XmlElement("type")]
		public IPASymbolType Type { get; set; }

		[XmlElement("subtype")]
		public IPASymbolSubType SubType { get; set; }

		[XmlElement("ignoreType")]
		public IPASymbolIgnoreType IgnoreType { get; set; }

		[XmlElement("isBase")]
		public bool IsBase { get; set; }

		[XmlElement("canPrecedeBase")]
		public bool CanPrecedeBase { get; set; }

		[XmlElement("displayWithDottedCircle")]
		public bool DisplayWithDottedCircle { get; set; }

		[XmlElement("mannerOfArticulation")]
		public int MOArticulation { get; set; }

		[XmlElement("placeOfArticulation")]
		public int POArticulation { get; set; }

		[XmlElement("chartColumn")]
		public int ChartColumn { get; set; }
		
		[XmlElement("chartGroup")]
		public int ChartGroup { get; set; }

		private string _hexadecimal;
		private List<string> _aFeatures = new List<string>(0);
		private List<string> _bFeatures = new List<string>(0);
		private FeatureMask _aMask = FeatureMask.Empty;
		private FeatureMask _bMask = FeatureMask.Empty;
		
		/// ------------------------------------------------------------------------------------
		public IPASymbol Copy()
		{
			return new IPASymbol
			{
				IsUndefined = IsUndefined,
				Literal = Literal,
				Hexadecimal = Hexadecimal,
				IPANumber = IPANumber,
				Name = Name,
				Usage = Usage,
				Description = Description,
				Type = Type,
				SubType = SubType,
				IgnoreType = IgnoreType,
				IsBase = IsBase,
				CanPrecedeBase = CanPrecedeBase,
				DisplayWithDottedCircle = DisplayWithDottedCircle,
				MOArticulation = MOArticulation,
				POArticulation = POArticulation,
				AMask = AMask.Clone(),
				BMask = BMask.Clone(),
			};
		}
		
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("code")]
		public string Hexadecimal
		{
			get { return _hexadecimal; }
			set
			{
				_hexadecimal = value;

				int dec;
				Decimal = (int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out dec) ?
					dec : invalidDecimalVal--);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of articulatory features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("features"), XmlArrayItem("feature")]
		public List<string> AFeatures
		{
			get
			{
				return (_aFeatures == null && _aMask != null && !_aMask.IsEmpty ?
					App.AFeatureCache.GetFeatureList(_aMask) : _aFeatures).ToList();
			}
			set { _aFeatures = value; }
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets or sets the list of binary features.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[XmlArray("binaryFeatures"), XmlArrayItem("feature")]
		//public List<string> BFeatures
		//{
		//    get
		//    {
		//        return (m_bFeatures == null && m_bMask != null && !m_bMask.IsEmpty ?
		//            App.BFeatureCache.GetFeatureList(m_bMask) : m_bFeatures);
		//    }
		//    set { m_bFeatures = value; }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the articulatory features mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureMask AMask
		{
			get
			{
				if (_aMask == null || _aMask.IsEmpty)
				{
					_aMask = App.AFeatureCache.GetMask(_aFeatures);
					if (_aFeatures != null && _aFeatures.Count > 0)
						_aFeatures = null;
				}

				return _aMask;
			}
			set { _aMask = (value ?? App.AFeatureCache.GetEmptyMask()); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the binary features mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureMask BMask
		{
			get
			{
				if (_bMask == null || _bMask.IsEmpty)
				{
					_bMask = App.BFeatureCache.GetMask(_bFeatures);
					if (_bFeatures != null && _bFeatures.Count > 0)
						_bFeatures = null;
				}

				return _bMask;
			}
			set { _bMask = (value ?? App.BFeatureCache.GetEmptyMask()); }
		}

		/// ------------------------------------------------------------------------------------
		public void ResetAFeatures()
		{
		}

		/// ------------------------------------------------------------------------------------
		public void ResetBFeatures()
		{
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0}: U+{1:X4}, {2}, {3}", Literal, Decimal, Name, Description);
		}
	}

	#endregion

	#region IPASymbolUsage
	/// ----------------------------------------------------------------------------------------
	[XmlType("usage")]
	public class IPASymbolUsage
	{
		[XmlAttribute("replaceWith")]
		public string ReplaceWith { get; set; }

		[XmlText]
		public string Information { get; set; }
	}

	#endregion
}
