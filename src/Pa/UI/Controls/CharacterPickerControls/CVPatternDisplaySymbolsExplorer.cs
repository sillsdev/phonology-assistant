using System.Windows.Forms;
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
				App.RegisterForLocalization(button, "DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.StressHeading", "Display Stress");

			if (GetHasLengthType(typeInfo))
				App.RegisterForLocalization(button, "DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.LengthHeading", "Display Length");

			if (GetHasBoundaryType(typeInfo))
				App.RegisterForLocalization(button, "DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.BoundaryHeading", "Display Boundary");

			if (GetHasToneType(typeInfo))
				App.RegisterForLocalization(button, "DialogBoxes.OptionsDlg.CVPatternsTab.CharacterPicker.ToneHeading", "Display Tone");
		}
	}
}
