using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public partial class ChartOptionsDropDown : UserControl
	{
		/// ------------------------------------------------------------------------------------
		public ChartOptionsDropDown()
		{
			InitializeComponent();
			lnkRefresh.Font = FontHelper.UIFont;
			lnkHelp.Font = FontHelper.UIFont;

			var fontSize = Math.Min(17, SystemInformation.MenuFont.SizeInPoints * 2);
			_charPicker.Font = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, fontSize);
			_charPicker.ItemSize = new Size(_charPicker.PreferredItemHeight, _charPicker.PreferredItemHeight);
			_charPicker.BackColor = Color.Transparent;
			_charPicker.LoadCharacters(App.IPASymbolCache.Values
				.Where(ci => !ci.IsBase && ci.SubType != IPASymbolSubType.notApplicable));

			_panelOuter.Controls.Remove(_charPicker);
			_explorerBar.SetHostedControl(_charPicker);

			// Adjust the size of the drop-down to fit 6 columns.
			_explorerBar.Width = _charPicker.GetPreferredWidth(6);
			_explorerBar.Height = _charPicker.GetPreferredHeight() + _explorerBar.Button.Height;

			_tableLayout.AutoSize = true;
			Size = new Size(_tableLayout.Width + 2, _tableLayout.Height + 2);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetIgnoredSymbols()
		{
			return _charPicker.GetTextsOfCheckedItems();
		}

		/// ------------------------------------------------------------------------------------
		public void SetIgnoredSymbols(IEnumerable<string> ignoredSymbols)
		{
			_charPicker.SetCheckedItemsByText(ignoredSymbols);
		}

		/// ------------------------------------------------------------------------------------
		private void lnkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			App.ShowHelpTopic("hidIgnoredSuprasegmentalsPopup");
		}
	}
}
