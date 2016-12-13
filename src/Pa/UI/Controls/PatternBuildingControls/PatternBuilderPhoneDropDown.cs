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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class PatternBuilderPhoneDropDown : UserControl
	{
		private Action<string> m_itemSelectedHandler;
		private Func<int> m_getWidthHandler;
		
		/// ------------------------------------------------------------------------------------
		public static CustomDropDown Create(Func<int> getWidthHandler, Action<string> itemSelectedHandler)
		{
			var phoneDropDown = new PatternBuilderPhoneDropDown();
			phoneDropDown.m_getWidthHandler = getWidthHandler;
			phoneDropDown.m_itemSelectedHandler = itemSelectedHandler;

			var hostingDropDown = new CustomDropDown();
			hostingDropDown.AddControl(phoneDropDown);
			hostingDropDown.Opening += phoneDropDown.LoadControls;
			hostingDropDown.Opened += delegate { phoneDropDown.Focus(); };
	
			return hostingDropDown;
		}

		/// ------------------------------------------------------------------------------------
		public PatternBuilderPhoneDropDown()
		{
			InitializeComponent();

			m_conPicker.FontSize = 15;
			m_vowPicker.FontSize = 15;
			m_otherPicker.FontSize = 17;
			m_otherPicker.Dock = DockStyle.Top;
		}

		/// ------------------------------------------------------------------------------------
		public void LoadControls(object sender, CancelEventArgs e)
		{
			m_conPicker.Clear();
			m_vowPicker.Clear();
			m_otherPicker.Clear();

			if (m_conPicker.Font.Name != App.PhoneticFont.Name)
			{
				m_conPicker.RefreshFont();
				m_vowPicker.RefreshFont();
				m_otherPicker.RefreshFont();
			}

			LoadPhonePickers();
			LoadOtherPicker();
			AdjustSize();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadPhonePickers()
		{
			m_conPicker.LoadCharacters(App.Project.PhoneCache.Values
				.Where(p => p.CharType == IPASymbolType.consonant).OrderBy(p => p.POAKey));

			m_vowPicker.LoadCharacters(App.Project.PhoneCache.Values
				.Where(p => p.CharType == IPASymbolType.vowel).OrderBy(p => p.POAKey));
		}

		/// ------------------------------------------------------------------------------------
		private void LoadOtherPicker()
		{
			//// Go through all the phones in the cache and strip off their diacritics. These
			//// will be used when determining which ones to display in the other picker.
			//var symbols = new List<IPASymbol>();
			//foreach (var pi in App.Project.PhoneCache.Values)
			//    symbols.AddRange(pi.GetSymbols().Where(ci => ci != null && !ci.IsBase));

			//m_otherPicker.LoadCharacters(ci =>
			//{
			//    switch (ci.Type)
			//    {
			//        case IPASymbolType.diacritic:
			//            return true;
			//        case IPASymbolType.suprasegmental:
			//            return (ci.SubType == IPASymbolSubType.StressAndLength || ci.SubType == IPASymbolSubType.ToneAndAccents);
			//        case IPASymbolType.consonant:
			//            if (ci.SubType == IPASymbolSubType.OtherSymbols)
			//            {
			//                // The only consonants to allow are the tie bars.
			//                return ((symbols.Contains(ci) || ci.Literal[0] == App.kTopTieBarC ||
			//                    ci.Literal[0] == App.kBottomTieBarC));
			//            }

			//            break;
			//    }

			//    return false;
			//});
		}

		/// ------------------------------------------------------------------------------------
		private void AdjustSize()
		{
			int desiredWidth = m_getWidthHandler() - 35;

			int cols = m_conPicker.GetMaxNumberOfColumnsToFitWidth((int)(desiredWidth * 0.6f));
			m_conPicker.Width = m_conPicker.GetPreferredWidth(cols);

			cols = m_conPicker.GetMaxNumberOfColumnsToFitWidth((int)(desiredWidth * 0.4f));
			m_vowPicker.Width = m_vowPicker.GetPreferredWidth(cols);

			m_conPicker.Height = Math.Max(m_conPicker.PreferredHeight, m_vowPicker.PreferredHeight);
			m_vowPicker.Height = m_conPicker.Height;
			m_otherPicker.Height = m_otherPicker.PreferredHeight;

			Size = m_tableLayout.Size;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutPaint(object sender, PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = m_tableLayout.ClientRectangle;
			rc.Width--;
			rc.Height--;

			using (var pen = new Pen(Properties.Settings.Default.GradientPanelTopColor))
				e.Graphics.DrawRectangle(pen, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			m_itemSelectedHandler(e.ClickedItem.Text.Replace(App.DottedCircle, string.Empty));
		}
	}
}
