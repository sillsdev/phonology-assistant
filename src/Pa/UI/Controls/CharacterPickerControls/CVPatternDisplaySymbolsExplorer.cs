// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Windows.Forms;
using Localization;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Controls
{
	public class CVPatternDisplaySymbolsExplorer : MultiSelectSymbolExplorer
	{
		/// ------------------------------------------------------------------------------------
		public CVPatternDisplaySymbolsExplorer()
		{
			Load((int)IPASymbolSubType.All);
		}

		/// ------------------------------------------------------------------------------------
		protected override void LocalizePickerButton(int typeInfo, Button button)
		{
			if (GetHasStressType(typeInfo))
			{
				LocalizationManager.GetString("DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.StressHeading",
					"Display Stress", null, button);
			}

			if (GetHasLengthType(typeInfo))
			{
				LocalizationManager.GetString("DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.LengthHeading",
					"Display Length", null, button);
			}

			if (GetHasBoundaryType(typeInfo))
			{
				LocalizationManager.GetString("DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.BoundaryHeading",
					"Display Boundary", null, button);
			}

			if (GetHasToneType(typeInfo))
			{
				LocalizationManager.GetString("DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.ToneHeading",
					"Display Tone", null, button);
			}

            if (GetHasPitchPhonationType(typeInfo))
            {
                LocalizationManager.GetString("DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.PitchPhonationHeading",
                    "Display PitchPhonation", null, button);
            }
		}
	}
}
