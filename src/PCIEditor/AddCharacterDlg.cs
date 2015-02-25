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
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Dialogs;
using SilTools;

namespace SIL.Pa
{
	public partial class AddCharacterDlg : OKCancelDlgBase
	{
		private readonly string m_invalidPhoneticChars = "{}[],_/<>$+#=*%CV" +
			App.kOrc + App.kDottedCircle;

		private const string kInvalidPhoneticCharsDisplay =
			"{ } [ ] <> ( ) , $ _ % # / + = * C V\n\nU+25CC and U+FFFC";

		#region Constants
		// Define Constants
		//private const string kDecimal = "Decimal";
		//private const string kLiteral = "Literal";
		//private const string kType = "Type";
		//private const string kMOA = "MOA";
		//private const string kPOA = "POA";
		//private const string kUnknown = "Unknown";

		#endregion

		#region Member variables
		private const int kInvalidCodePoint = 31;

		// MOA & POA
		// The SortedList Key is the moa or poa and the Value is the hexIpaChar
		//private readonly SortedList<float, string> m_MOA = new SortedList<float, string>();
		//private readonly SortedList<float, string> m_POA = new SortedList<float, string>();

		private readonly bool m_addingSymbol = true;
		private readonly List<int> m_codePoints = new List<int>();
		private readonly IPASymbol m_origSymbol;
		#endregion

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// AddCharacterDlg Constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AddCharacterDlg(IPASymbol symbol)
		{
			InitializeComponent();

			lblChar.Font = App.PhoneticFont;
			cboMoa.Font = App.PhoneticFont;
			cboPoa.Font = App.PhoneticFont;

			lblChar.Top = lblCharLable.Top - ((lblChar.Height - lblCharLable.Height) / 2);

			// Load the Type combo boxes with readable strings
			foreach (string type in Enum.GetNames(typeof(IPASymbolType)))
				cboType.Items.Add(SeperateWordsWithSpace(type));
			foreach (string type in Enum.GetNames(typeof(IPASymbolSubType)))
				cboSubType.Items.Add(SeperateWordsWithSpace(type));
			foreach (string type in Enum.GetNames(typeof(IPASymbolIgnoreType)))
				cboIgnoreType.Items.Add(SeperateWordsWithSpace(type));

			cboType.SelectedIndex = 0;
			cboSubType.SelectedIndex = 0;
			cboIgnoreType.SelectedIndex = 0;

			m_addingSymbol = (symbol == null);

			if (m_addingSymbol)
			{
				Symbol = new IPASymbol();
				m_origSymbol = Symbol;
				lblUnicodeValue.Visible = false;
			}
			else
			{
				m_origSymbol = symbol;
				Symbol = symbol.Copy();

				// Identity
				txtHexValue.Visible = false;
				lblUnicodeValue.Text = Symbol.Hexadecimal;
				lblChar.Text = Symbol.Literal;
				txtCharName.Text = Symbol.Name;
				txtCharDesc.Text = Symbol.Description;

				// Types
				cboType.SelectedItem = SeperateWordsWithSpace(Symbol.Type.ToString());
				cboSubType.SelectedItem = SeperateWordsWithSpace(Symbol.SubType.ToString());
				cboIgnoreType.SelectedItem = SeperateWordsWithSpace(Symbol.IgnoreType.ToString());

				// Base Character
				chkIsBase.Checked = Symbol.IsBase;
				chkPreceedBaseChar.Checked = Symbol.CanPrecedeBase;
				chkDottedCircle.Checked = Symbol.DisplayWithDottedCircle;
			}

			_featuresTab.SetCurrentInfo(Symbol);
//			LoadMoaPoaComboBoxes(pciEditor.Grid);
			CreateDirtyStateHandlers();
			m_dirty = false;

			Settings.Default.AddCharacterDlg = App.InitializeForm(this, Settings.Default.MainWindow);
		}
		
		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Create DirtyStateChanged event handlers.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		void CreateDirtyStateHandlers()
		{
			txtHexValue.TextChanged += ((s, e) => m_dirty = true);
			txtCharName.TextChanged += ((s, e) => m_dirty = true);
			txtCharDesc.TextChanged += ((s, e) => m_dirty = true);
			cboType.SelectedIndexChanged += ((s, e) => m_dirty = true);
			cboSubType.SelectedIndexChanged += ((s, e) => m_dirty = true);
			cboIgnoreType.SelectedIndexChanged += ((s, e) => m_dirty = true);
			chkIsBase.CheckStateChanged += ((s, e) => m_dirty = true);
			chkPreceedBaseChar.CheckStateChanged += ((s, e) => m_dirty = true);
			chkDottedCircle.CheckStateChanged += ((s, e) => m_dirty = true);
			cboMoa.SelectedValueChanged += ((s, e) => m_dirty = true);
			cboPoa.SelectedValueChanged += ((s, e) => m_dirty = true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CharInfo object created from the changes made on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPASymbol Symbol { get; private set; }

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Load the MOA and POA combo boxes.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void LoadMoaPoaComboBoxes(DataGridView charGrid)
		//{
		//    foreach (DataGridViewRow gridRow in charGrid.Rows)
		//    {
		//        // Save unique codePoint's / IPAChar's for Verification later
		//        int codePoint = (int)gridRow.Cells[kDecimal].Value;
		//        if (!m_codePoints.Contains(codePoint))
		//            m_codePoints.Add(codePoint);

		//        if (gridRow.Cells[kDecimal].Value != null && gridRow.Cells[kType].Value != null)
		//        {
		//            if ((int)gridRow.Cells[kDecimal].Value <= kInvalidCodePoint ||
		//                (string)gridRow.Cells[kType].Value == kUnknown)
		//                continue;
		//        }

		//        if (gridRow.Cells[kLiteral].Value != null)
		//        {
		//            // Create sorted lists of the manners and points of articulation.
		//            float moa = float.Parse(gridRow.Cells[kMOA].Value.ToString());
		//            float poa = float.Parse(gridRow.Cells[kPOA].Value.ToString());
		//            m_MOA[moa] = gridRow.Cells[kLiteral].Value.ToString();
		//            m_POA[poa] = gridRow.Cells[kLiteral].Value.ToString();
		//        }
		//    }

			//// Load the combo boxes
			//foreach (var moa in m_MOA)
			//{
			//    if (moa.Key != m_original_moa)
			//        cboMoa.Items.Add(moa.Value);
			//    else
			//        cboMoa.SelectedIndex = cboMoa.Items.Count - 1;
			//}

			//foreach (var poa in m_POA)
			//{
			//    if (poa.Key != m_original_poa)
			//        cboPoa.Items.Add(poa.Value);
			//    else
			//        cboPoa.SelectedIndex = cboPoa.Items.Count - 1;
			//}
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a space before each capital letter to make the string easier to read.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string SeperateWordsWithSpace(IEnumerable<char> multiWord)
		{
			var newName = string.Empty;
			foreach (char letter in multiWord)
			{
				if (Char.IsUpper(letter))
					newName += ' ';
				newName += letter;
			}
			return newName.Trim();
		}
		
		#endregion

		#region Overrides
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get
			{
				return (base.IsDirty || m_addingSymbol ||
					Symbol.AMask != m_origSymbol.AMask ||
					Symbol.BMask != m_origSymbol.BMask);
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save Changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			Symbol.Hexadecimal = (m_addingSymbol ? txtHexValue.Text : lblUnicodeValue.Text);
			Symbol.Literal = lblChar.Text;
			Symbol.Name = txtCharName.Text;
			Symbol.Description = txtCharDesc.Text;

			// Types - remove the spaces in the Type strings
			Symbol.Type = (IPASymbolType)Enum.Parse(
				typeof(IPASymbolType), cboType.SelectedItem.ToString().Replace(" ", ""));
			Symbol.SubType = (IPASymbolSubType)Enum.Parse(
				typeof(IPASymbolSubType), cboSubType.SelectedItem.ToString().Replace(" ", ""));
			Symbol.IgnoreType = (IPASymbolIgnoreType)Enum.Parse(
				typeof(IPASymbolIgnoreType), cboIgnoreType.SelectedItem.ToString().Replace(" ", ""));

			// Base Character
			Symbol.IsBase = chkIsBase.Checked;
			Symbol.CanPrecedeBase = chkPreceedBaseChar.Checked;
			Symbol.DisplayWithDottedCircle = chkDottedCircle.Checked;

			//// Save the manner of articulation sort order value
			//if (cboMoa.SelectedItem == null)
			//{
			//    // use original value if not modified
			//    CharInfo.MOArticulation = (int)m_original_moa;
			//}
			//else
			//{
			//    foreach (var moa in m_MOA.Where(moa => moa.Value == cboMoa.SelectedItem.ToString()))
			//    {
			//        // Make sure the user actually changed the MOA
			//        CharInfo.MOArticulation = (int)(m_original_moa == moa.Key ? m_original_moa : moa.Key + 1f);
			//        break;
			//    }
			//}

			//// Save the place of articulation sort order value
			//if (cboPoa.SelectedItem == null)
			//{
			//    // use original value if not modified
			//    CharInfo.POArticulation = (int)m_original_poa;
			//}
			//else
			//{
			//    foreach (var poa in m_POA.Where(poa => poa.Value == cboPoa.SelectedItem.ToString()))
			//    {
			//        // Make sure the user actually changed the POA
			//        CharInfo.POArticulation = (int)(m_original_poa == poa.Key ? m_original_poa : poa.Key + 1f);
			//        break;
			//    }
			//}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return true if data is OK.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			// Check for an invalid hex number
			if (m_addingSymbol)
			{
				try
				{
					int.Parse(txtHexValue.Text, NumberStyles.AllowHexSpecifier);
				}
				catch
				{
					Utils.MsgBox(Properties.Resources.kstidInvalidUnicodeValueMsg);
					return false;
				}

				int codePoint = int.Parse(txtHexValue.Text.Trim(), NumberStyles.HexNumber);
				if (codePoint <= kInvalidCodePoint)
				{
					Utils.MsgBox(Properties.Resources.kstidUnicodeValueTooSmallMsg);
					return false;
				}

				// Make sure the codePoint is unique
				if (m_codePoints.Contains(codePoint))
				{
					Utils.MsgBox(string.Format(Properties.Resources.kstidDuplicateCharMsg));
					return false;
				}

				// Make sure the code point isn't one of the reserved characters.
				if (m_invalidPhoneticChars.Any(c => codePoint == c))
				{
					Utils.MsgBox(string.Format(
						Properties.Resources.kstidUnicodeValueIsReservedMsg,
						txtHexValue.Text, kInvalidPhoneticCharsDisplay));
					
					return false;
				}
			}

			string missingFields = string.Empty;
			if (m_addingSymbol && txtHexValue.Text == string.Empty)
				missingFields += (lblUnicode.Text + ", ");
			if (txtCharName.Text == string.Empty)
				missingFields += (lblName.Text + ", ");
			//if (m_addingChar && cboMoa.SelectedItem == null)
			//    missingFields += (kLblMoa + ", ");
			//if (m_addingChar && cboPoa.SelectedItem == null)
			//    missingFields += (kLblPoa + ", ");

			if (missingFields != string.Empty)
			{
				missingFields = missingFields.Replace("&&", "~~");
				missingFields = missingFields.Replace("&", string.Empty);
				missingFields = missingFields.Replace("~~", "&");
				missingFields = missingFields.Replace(":", string.Empty);
				missingFields = missingFields.TrimEnd(new[] { ',', ' ' });
				missingFields = string.Format(Properties.Resources.kstidMissingFieldsMsg, missingFields);
				Utils.MsgBox(missingFields);
				return false;
			}

			return true;
		}

		#endregion
		
		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			PCIEditor.ShowHelpTopic(@"Phonetic_Character_Inventory_Editor/Phonetic_Character_Properties.htm");
		}

		/// ------------------------------------------------------------------------------------
		private void txtHexValue_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar >= 'a' && e.KeyChar <= 'f')
				e.KeyChar = (char)(e.KeyChar & ~0x20);
			
			if ((e.KeyChar >= 'A' && e.KeyChar <= 'F') ||
				(e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '\b')
			{
				return;
			}

			e.KeyChar = '\0';
			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hex Value changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtHexValue_TextChanged(object sender, EventArgs e)
		{
			if (txtHexValue.Text.Trim() == string.Empty)
				return;

			int codePoint;
			if (int.TryParse(txtHexValue.Text, NumberStyles.HexNumber,
				NumberFormatInfo.InvariantInfo, out codePoint) && codePoint > 31)
			{
				lblChar.Text = ((char)codePoint).ToString();
			}
			else
				lblChar.Text = string.Empty;
		}

		#endregion
	}
}