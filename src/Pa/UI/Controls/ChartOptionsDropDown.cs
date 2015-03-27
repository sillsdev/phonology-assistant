// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using L10NSharp;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public partial class ChartOptionsDropDown : UserControl
	{
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a value indicating whether or not the cancel link was clicked.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public bool Canceled { get; set; }

		/// ------------------------------------------------------------------------------------
		public ChartOptionsDropDown()
		{
            Canceled = false;

			InitializeComponent();
			lnkOK.Font = FontHelper.UIFont;
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
            Canceled = true;
			App.ShowHelpTopic("hidIgnoredSuprasegmentalsPopup");
		}

        /// ------------------------------------------------------------------------------------
        public void Close()
        {
            if (Parent is ToolStripDropDown)
                ((ToolStripDropDown)Parent).Close();
            else if (Parent != null)
            {
                try
                {
                    Parent.GetType().InvokeMember("Close", BindingFlags.Instance |
                        BindingFlags.InvokeMethod | BindingFlags.Public, null, Parent, null);
                }
                catch { }
            }
        }

        /// ------------------------------------------------------------------------------------
        protected virtual void HandleCloseClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Canceled = true;
            Close();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Treat the escape key like clicking cancel.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                Canceled = true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
	}
}
