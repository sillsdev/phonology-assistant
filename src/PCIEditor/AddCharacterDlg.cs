using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.UI.Controls;
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
		private const string kDecimal = "Decimal";
		private const string kLiteral = "Literal";
		private const string kHexadecimal = "Hexadecimal";
		private const string kName = "Name";
		private const string kDescription = "Description";
		private const string kType = "Type";
		private const string kSubType = "SubType";
		private const string kIgnoreType = "IgnoreType";
		private const string kIsBase = "IsBase";
		private const string kCanPrecedeBaseChar = "CanPrecedeBaseChar";
		private const string kDisplayWithDottedCircle = "DisplayWithDottedCircle";
		private const string kMOA = "MOA";
		private const string kPOA = "POA";
		private const string kLblMoa = "MOA Sort Order";
		private const string kLblPoa = "POA Sort Order";
		private const string kChartColumn = "ChartColumn";
		private const string kChartGroup = "ChartGroup";
		private const string kLblChartColumn = "Column";
		private const string kLblChartGroup = "Group";
		private const string kUnknown = "Unknown";
		private const string kConsonant = "Consonant";
		private const string kVowel = "Vowel";
		private const string kOtherSymbols = "Other Symbols";

		// VOWEL Groups
		private const string kClose = "Close";
		private const string kNearClose = "Near-close";
		private const string kCloseMid = "Close-mid";
		private const string kMid = "Mid";
		private const string kOpenMid = "Open-mid";
		private const string kNearOpen = "Near-open";
		private const string kOpen = "Open";
		private const string kOther = "Other";

		// VOWEL Columns
		private const string kFrontUnrounded = "Front Unrounded";
		private const string kFrontRounded = "Front Rounded";
		private const string kNearFrontUnrounded = "Near-front Unrounded";
		private const string kNearFrontRounded = "Near-front Rounded";
		private const string kCentralUnrounded = "Central Unrounded";
		private const string kCentral = "Central";
		private const string kCentralRounded = "Central Rounded";
		private const string kNearBackRounded = "Near-back Rounded";
		private const string kBackUnrounded = "Back Unrounded";
		private const string kBackRounded = "Back Rounded";

		// CONSONANT Columns
		private const string kVoicelessBilabial = "Voiceless Bilabial";
		private const string kVoicedBilabial = "Voiced Bilabial";
		private const string kVoicelessLabiodental = "Voiceless Labiodental";
		private const string kVoicedLabiodental = "Voiced Labiodental";
		private const string kVoicelessDental = "Voiceless Dental";
		private const string kVoicedDental = "Voiced Dental";
		private const string kVoicelessAlveolar = "Voiceless Alveolar";
		private const string kVoicedAlveolar = "Voiced Alveolar";
		private const string kVoicelessPostalveolar = "Voiceless Postalveolar";
		private const string kVoicedPostalveolar = "Voiced Postalveolar";
		private const string kVoicelessRetroflex = "Voiceless Retroflex";
		private const string kVoicedRetroflex = "Voiced Retroflex";
		private const string kVoicelessAlvPalatal = "Voiceless Alv-palatal";
		private const string kVoicedAlvPalatal = "Voiced Alv-palatal";
		private const string kVoicelessPalatal = "Voiceless Palatal";
		private const string kVoicedPalatal = "Voiced Palatal";
		private const string kVoicelessVelar = "Voiceless Velar";
		private const string kVoicedVelar = "Voiced Velar";
		private const string kVoicelessUvular = "Voiceless Uvular";
		private const string kVoicedUvular = "Voiced Uvular";
		private const string kVoicelessPharyngeal = "Voiceless Pharyngeal";
		private const string kVoicedPharyngeal = "Voiced Pharyngeal";
		private const string kVoicelessGlottal = "Voiceless Glottal";
		private const string kVoicedGlottal = "Voiced Glottal";
		private const string kVoicelessEpiglottal = "Voiceless Epiglottal";
		private const string kVoicedEpiglottal = "Voiced Epiglottal";

		// CONSONANT Groups
		private const string kPlosive = "Plosive";
		private const string kNasal = "Nasal";
		private const string kTrill = "Trill";
		private const string kTapOrFlap = "Tap or Flap";
		private const string kFricative = "Fricative";
		private const string kLateralFricative = "Lateral Fricative";
		private const string kApproximant = "Approximant";
		private const string kLateralApproximant = "Lateral Approximant";
		private const string kImplosive = "Implosives";
		private const string kClick = "Clicks";

		#endregion

		#region Member variables
		private const int kInvalidCodePoint = 31;

		// MOA & POA
		// The SortedList Key is the moa or poa and the Value is the hexIpaChar
		private readonly SortedList<float, string> m_MOA = new SortedList<float, string>();
		private readonly SortedList<float, string> m_POA = new SortedList<float, string>();
		private readonly float m_original_moa;
		private readonly float m_original_poa;

		private readonly bool m_addingChar = true;
		private readonly List<int> m_codePoints = new List<int>();
		#endregion

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// AddCharacterDlg Constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AddCharacterDlg(PCIEditor pciEditor, int newChar) : this(pciEditor, true)
		{
			txtHexValue.Text = newChar.ToString("X4");
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// AddCharacterDlg Constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AddCharacterDlg(PCIEditor pciEditor,  bool addingChar)
		{
			InitializeComponent();

			PCIEditor.SettingsHandler.LoadFormProperties(this);

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

			m_addingChar = addingChar;
			
			if (m_addingChar)
			{
				lblUnicodeValue.Visible = false;
				txtHexValue.Text = string.Empty;
				lblChar.Text = string.Empty;
				txtCharName.Text = string.Empty;
				txtCharDesc.Text = string.Empty;

				// Load the Moa/Poa combo boxes
				m_original_moa = -1f;
				m_original_poa = -1f;

				LoadMoaPoaComboBoxes(pciEditor.Grid);
				CreateDirtyStateHandlers();
				return;
			}

			DataGridViewRow row = pciEditor.Grid.CurrentRow;
			if (row.Tag is IPASymbol)
			{
				CharInfo = (IPASymbol)row.Tag;
				_featuresTab.SetCurrentInfo((IPASymbol)row.Tag);
			}

			// Identity
			txtHexValue.Visible = false;
			lblUnicodeValue.Text = row.Cells[kHexadecimal].Value as string;
			lblChar.Text = row.Cells[kLiteral].Value as string;
			txtCharName.Text = row.Cells[kName].Value as string;
			txtCharDesc.Text = row.Cells[kDescription].Value as string;

			// Types
			cboType.SelectedItem = SeperateWordsWithSpace(row.Cells[kType].Value as string);
			cboSubType.SelectedItem = SeperateWordsWithSpace(row.Cells[kSubType].Value as string);
			cboIgnoreType.SelectedItem = SeperateWordsWithSpace(row.Cells[kIgnoreType].Value as string);

			// Base Character
			chkIsBase.Checked = (bool)row.Cells[kIsBase].Value;
			chkPreceedBaseChar.Checked = (bool)row.Cells[kCanPrecedeBaseChar].Value;
			chkDottedCircle.Checked = (bool)row.Cells[kDisplayWithDottedCircle].Value;

			// Articulation - load the Moa/Poa combo boxes
			m_original_moa = float.Parse(row.Cells[kMOA].Value.ToString());
			m_original_poa = float.Parse(row.Cells[kPOA].Value.ToString());
			LoadMoaPoaComboBoxes(pciEditor.Grid);

			CreateDirtyStateHandlers();
			m_dirty = false;
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
		public IPASymbol CharInfo { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the MOA and POA combo boxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadMoaPoaComboBoxes(DataGridView charGrid)
		{
			foreach (DataGridViewRow gridRow in charGrid.Rows)
			{
				// Save unique codePoint's / IPAChar's for Verification later
				int codePoint = (int)gridRow.Cells[kDecimal].Value;
				if (!m_codePoints.Contains(codePoint))
					m_codePoints.Add(codePoint);

				if (gridRow.Cells[kDecimal].Value != null && gridRow.Cells[kType].Value != null)
				{
					if ((int)gridRow.Cells[kDecimal].Value <= kInvalidCodePoint ||
						(string)gridRow.Cells[kType].Value == kUnknown)
						continue;
				}

				if (gridRow.Cells[kLiteral].Value != null)
				{
					// Create sorted lists of the manners and points of articulation.
					float moa = float.Parse(gridRow.Cells[kMOA].Value.ToString());
					float poa = float.Parse(gridRow.Cells[kPOA].Value.ToString());
					m_MOA[moa] = gridRow.Cells[kLiteral].Value.ToString();
					m_POA[poa] = gridRow.Cells[kLiteral].Value.ToString();
				}
			}

			// Load the combo boxes
			foreach (var moa in m_MOA)
			{
				if (moa.Key != m_original_moa)
					cboMoa.Items.Add(moa.Value);
				else
					cboMoa.SelectedIndex = cboMoa.Items.Count - 1;
			}

			foreach (var poa in m_POA)
			{
				if (poa.Key != m_original_poa)
					cboPoa.Items.Add(poa.Value);
				else
					cboPoa.SelectedIndex = cboPoa.Items.Count - 1;
			}
		}

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
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PCIEditor.SettingsHandler.SaveFormProperties(this);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save Changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			CharInfo = new IPASymbol();

			CharInfo.Decimal = int.Parse((m_addingChar ?
				txtHexValue.Text : lblUnicodeValue.Text), NumberStyles.HexNumber);

			CharInfo.Hexadecimal = (m_addingChar ? txtHexValue.Text : lblUnicodeValue.Text);
			CharInfo.Literal = lblChar.Text;
			CharInfo.Name = txtCharName.Text;
			CharInfo.Description = txtCharDesc.Text;
			//CharInfo.AMask =  m_aMask;
			//CharInfo.BMask = m_bMask;

			// Types - remove the spaces in the Type strings
			CharInfo.Type = (IPASymbolType)Enum.Parse(
				typeof(IPASymbolType), cboType.SelectedItem.ToString().Replace(" ", ""));
			CharInfo.SubType = (IPASymbolSubType)Enum.Parse(
				typeof(IPASymbolSubType), cboSubType.SelectedItem.ToString().Replace(" ", ""));
			CharInfo.IgnoreType = (IPASymbolIgnoreType)Enum.Parse(
				typeof(IPASymbolIgnoreType), cboIgnoreType.SelectedItem.ToString().Replace(" ", ""));

			// Base Character
			CharInfo.IsBase = chkIsBase.Checked;
			CharInfo.CanPrecedeBase = chkPreceedBaseChar.Checked;
			CharInfo.DisplayWithDottedCircle = chkDottedCircle.Checked;

			// Save the manner of articulation sort order value
			if (cboMoa.SelectedItem == null)
			{
				// use original value if not modified
				CharInfo.MOArticulation = (int)m_original_moa;
			}
			else
			{
				foreach (var moa in m_MOA)
				{
					if (moa.Value == cboMoa.SelectedItem.ToString())
					{
						// Make sure the user actually changed the MOA
						CharInfo.MOArticulation =
							(int)(m_original_moa == moa.Key ? m_original_moa : moa.Key + 1f);
						break;
					}
				}
			}

			// Save the place of articulation sort order value
			if (cboPoa.SelectedItem == null)
			{
				// use original value if not modified
				CharInfo.POArticulation = (int)m_original_poa;
			}
			else
			{
				foreach (var poa in m_POA)
				{
					if (poa.Value == cboPoa.SelectedItem.ToString())
					{
						// Make sure the user actually changed the POA
						CharInfo.POArticulation =
							(int)(m_original_poa == poa.Key ? m_original_poa : poa.Key + 1f);
						break;
					}
				}
			}

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
			if (m_addingChar)
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
			if (m_addingChar && txtHexValue.Text == string.Empty)
				missingFields += (lblUnicode.Text + ", ");
			if (txtCharName.Text == string.Empty)
				missingFields += (lblName.Text + ", ");
			if (m_addingChar && cboMoa.SelectedItem == null)
				missingFields += (kLblMoa + ", ");
			if (m_addingChar && cboPoa.SelectedItem == null)
				missingFields += (kLblPoa + ", ");

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
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Clicked the OK button.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void btnOK_Click(object sender, EventArgs e)
		//{
		//    Close();
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Clicked the CANCEL button.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void btnCancel_Click(object sender, EventArgs e)
		//{
		//    m_dirty = false;
		//    m_charInfo = null;
		//    Close();
		//}

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			PCIEditor.ShowHelpTopic(@"Phonetic_Character_Inventory_Editor/Phonetic_Character_Properties.htm");
		}

		/// ------------------------------------------------------------------------------------
		private static void txtHexValue_KeyPress(object sender, KeyPressEventArgs e)
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