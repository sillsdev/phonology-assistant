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
// File: CVPatternInfo.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Text;
using System.Xml.Serialization;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.Model
{
	#region CVPatternInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CVPatternInfo
	{
		private string m_phone;
		private string m_leftSideDiacritics;
		private string m_rightSideDiacritics;
		private IPASymbolIgnoreType m_patternType = IPASymbolIgnoreType.NotApplicable;

		#region static methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static CVPatternInfo Create(string phone, IPASymbolIgnoreType patternType)
		{
			if (string.IsNullOrEmpty(phone))
				return null;

			CVPatternInfo cv = new CVPatternInfo();
			cv.Phone = phone;
			cv.PatternType = patternType;
			return cv;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if PhoneCache.CVPatternInfoList contains a CVPatternInfo object whose
		/// phone is the same as that specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Contains(string phone)
		{
			if (PhoneCache.CVPatternInfoList != null)
			{
				foreach (CVPatternInfo cvpi in PhoneCache.CVPatternInfoList)
				{
					if (cvpi.Phone == phone)
						return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks the specified phone for any modifying diacritics found in any of the items
		/// in PhoneCache.CVPatternInfoList.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetMatchingModifiers(string phone, StringBuilder bldr)
		{
			if (PhoneCache.CVPatternInfoList == null)
				return null;

			// Find out which codepoint in the phone represents the base character. For
			// tie-barred phones, just use the first of the two base characters.
			int baseIndex = -1;
			for (int i = 0; i < phone.Length; i++)
			{
				IPASymbol charInfo = PaApp.IPASymbolCache[phone[i]];
				if (charInfo != null && charInfo.IsBase)
				{
					baseIndex = i;
					break;
				}
			}

			// This should never happen, but if the phone has no base character,
			// what more can we do. 
			if (baseIndex < 0)
				return null;

			// Get the pieces of the phone that are before and after the base character.
			string preBase = (baseIndex == 0 ? string.Empty : phone.Substring(0, baseIndex));
			string postBase = (baseIndex == phone.Length - 1 ? string.Empty :
				phone.Substring(baseIndex + 1));

			StringBuilder diacriticsAfterBase = new StringBuilder();
			foreach (CVPatternInfo cvpi in PhoneCache.CVPatternInfoList)
			{
				if (cvpi.HasLeftSideDiacritics && cvpi.LeftSideDiacritics == preBase)
					bldr.Append(cvpi.LeftSideDiacritics);

				if (cvpi.HasRightSideDiacritics)
				{
					foreach (char c in cvpi.RightSideDiacritics)
					{
						if (postBase.IndexOf(c) >= 0)
							diacriticsAfterBase.Append(c);
					}
				}
			}

			return (diacriticsAfterBase.Length == 0 ? null : diacriticsAfterBase.ToString());
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets m_cvPatternPhone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Phone
		{
			get { return m_phone; }
			set
			{
				m_phone = FFNormalizer.Normalize(value);

				// If the phone is also a base phonetic character, we're done now.
				IPASymbol charInfo = PaApp.IPASymbolCache[m_phone];
				if (charInfo != null && charInfo.IsBase)
					return;

				StringBuilder bldrDiacritics = new StringBuilder();
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
					charInfo = PaApp.IPASymbolCache[c];
					if (charInfo != null && charInfo.IsBase)
						return;

					if (c != PaApp.kDottedCircleC)
						bldrDiacritics.Append(c);
					else if (!foundDiacriticPlaceholder)
					{
						// We've hit the first diacritic placeholder, so move all that's
						// preceded it to the 
						m_leftSideDiacritics = bldrDiacritics.ToString();
						foundDiacriticPlaceholder = true;
					}
				}

				m_rightSideDiacritics = bldrDiacritics.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets m_patternType.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Type")]
		public IPASymbolIgnoreType PatternType
		{
			get { return m_patternType; }
			set { m_patternType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string LeftSideDiacritics
		{
			get { return m_leftSideDiacritics; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string RightSideDiacritics
		{
			get { return m_rightSideDiacritics; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool HasLeftSideDiacritics
		{
			get { return !string.IsNullOrEmpty(m_leftSideDiacritics); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool HasRightSideDiacritics
		{
			get { return !string.IsNullOrEmpty(m_rightSideDiacritics); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_phone;
		}
	}

	#endregion
}
