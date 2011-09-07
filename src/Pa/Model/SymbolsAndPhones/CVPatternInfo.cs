using System.Text;
using System.Xml.Serialization;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.Model
{
	#region CVPatternInfo class
	/// ----------------------------------------------------------------------------------------
	public class CVPatternInfo
	{
		private string m_phone;

		/// ------------------------------------------------------------------------------------
		public CVPatternInfo()
		{
			PatternType = IPASymbolIgnoreType.NotApplicable;
		}

		#region static methods
		/// ------------------------------------------------------------------------------------
		public static CVPatternInfo Create(string phone, IPASymbolIgnoreType patternType)
		{
			return (string.IsNullOrEmpty(phone) ? null :
				new CVPatternInfo { Phone = phone, PatternType = patternType });
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Phone
		{
			get { return m_phone; }
			set
			{
				m_phone = FFNormalizer.Normalize(value);

				// If the phone is also a base phonetic character, we're done now.
				var charInfo = App.IPASymbolCache[m_phone];
				if (charInfo != null && charInfo.IsBase)
					return;

				var bldrDiacritics = new StringBuilder();
				bool foundDiacriticPlaceholder = false;

				// Check if the "phone" contains a base character. If so, then the assumption
				// is that the CV pattern will contain exact matches of m_phone instead of a
				// C or V. If not, then keep track of diacritics before and after the diacritic
				// placeholder. If there's no diacritic placeholder and no base character found,
				// it's assumed that what's found in m_phone are diacritics that are to follow
				// the C or V. If there is a diacritic placeholder, then what precedes it are
				// diacritics that will precede a C or V (e.g. prenasalization) and what
				// follows it are diacritics that will follow the C or V.
				foreach (char c in m_phone)
				{
					charInfo = App.IPASymbolCache[c];
					if (charInfo != null && charInfo.IsBase)
						return;

					if (c != App.kDottedCircleC)
						bldrDiacritics.Append(c);
					else if (!foundDiacriticPlaceholder)
					{
						// We've hit the first diacritic placeholder, so move all that's
						// preceded it to the 
						LeftSideDiacritics = bldrDiacritics.ToString();
						foundDiacriticPlaceholder = true;
					}
				}

				RightSideDiacritics = bldrDiacritics.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets m_patternType.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Type")]
		public IPASymbolIgnoreType PatternType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string LeftSideDiacritics { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string RightSideDiacritics { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool HasLeftSideDiacritics
		{
			get { return !string.IsNullOrEmpty(LeftSideDiacritics); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool HasRightSideDiacritics
		{
			get { return !string.IsNullOrEmpty(RightSideDiacritics); }
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsLeftSideDiacritic(char diacritic)
		{
			return (HasLeftSideDiacritics && LeftSideDiacritics.Contains(diacritic.ToString()));
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRightSideDiacritic(char diacritic)
		{
			return (HasRightSideDiacritics && RightSideDiacritics.Contains(diacritic.ToString()));
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_phone;
		}
	}

	#endregion
}
