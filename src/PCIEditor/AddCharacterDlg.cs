using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.Pa.Dialogs;
using SIL.Pa.Properties;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	public partial class AddCharacterDlg : OKCancelDlgBase
	{
		private readonly string m_invalidPhoneticChars = "{}[],_/<>$+#=*%CV" +
			DataUtils.kOrc.ToString() + DataUtils.kDottedCircle;

		private const string kInvalidPhoneticCharsDisplay =
			"{ } [ ] <> ( ) , $ _ % # / + = * C V\n\nU+25CC and U+FFFC";

		#region Constants
		// Define Constants
		private const string kCodePoint = "CodePoint";
		private const string kIpaChar = "IpaChar";
		private const string kHexIPAChar = "HexIPAChar";
		private const string kName = "Name";
		private const string kDescription = "Description";
		private const string kCharType = "CharType";
		private const string kCharSubType = "CharSubType";
		private const string kIgnoreType = "IgnoreType";
		private const string kIsBaseChar = "IsBaseChar";
		private const string kCanPreceedBaseChar = "CanPreceedBaseChar";
		private const string kDisplayWDottedCircle = "DisplayWDottedCircle";
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
		private const string kPulmonic = "Pulmonic";
		private const string kNonPulmonic = "Non Pulmonic";
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
		private readonly int invalidCodePoint = 31;

		// MOA & POA
		// The SortedList Key is the moa or poa and the Value is the hexIpaChar
		private readonly SortedList<float, string> m_MOA = new SortedList<float, string>();
		private readonly SortedList<float, string> m_POA = new SortedList<float, string>();
		private readonly float m_original_moa;
		private readonly float m_original_poa;

		// Chart Columns and Groups
		// The SortedList Key is the col/grp number and the Value is the col/grp name
		private readonly string m_origChartGroup = string.Empty;
		private readonly string m_origChartColumn = string.Empty;
		private readonly int m_origChartColumnOtherSymbol;
		private readonly SortedList<float, string> m_CChartCols = new SortedList<float, string>();
		private readonly SortedList<float, string> m_CChartGrps = new SortedList<float, string>();
		private readonly SortedList<float, string> m_VChartGrps = new SortedList<float, string>();
		private readonly SortedList<float, string> m_VChartCols = new SortedList<float, string>();

		private readonly bool m_addingChar = true;
		private readonly List<int> m_codePoints = new List<int>();
		private readonly PCIEditor m_pciEditor;
		private readonly string m_saveAFeatureDropDownName;
		private readonly string m_saveBFeatureDropDownName;
		private IPACharInfo m_charInfo;
		private ulong[] m_masks = new ulong[] { 0, 0 };
		private ulong m_binaryMask = 0;
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
			m_pciEditor = pciEditor;

			// Prepare things to use the same feature drop-downs that are used on the
			// main form, for the grid's feature columns.
			m_saveAFeatureDropDownName = m_pciEditor.m_sddpAFeatures.SavedSettingsName;
			m_saveBFeatureDropDownName = m_pciEditor.m_sddpBFeatures.SavedSettingsName;
			m_pciEditor.m_sddpAFeatures.SavedSettingsName = Name + "AFeatureDropDown";
			m_pciEditor.m_sddpBFeatures.SavedSettingsName = Name + "BFeatureDropDown";
			m_pciEditor.m_aFeatureDropdown.Closing += m_aFeatureDropdown_Closing;
			m_pciEditor.m_bFeatureDropdown.Closing += m_bFeatureDropdown_Closing;

			// Unhook the delegates from the the main form.
			m_pciEditor.m_aFeatureDropdown.Closing -= m_pciEditor.m_featureDropdown_Closing;
			m_pciEditor.m_bFeatureDropdown.Closing -= m_pciEditor.m_featureDropdown_Closing;

			lblChar.Font = FontHelper.PhoneticFont;
			cboMoa.Font = FontHelper.PhoneticFont;
			cboPoa.Font = FontHelper.PhoneticFont;

			lblChar.Top = lblCharLable.Top - ((lblChar.Height - lblCharLable.Height) / 2);

			// Load the Type combo boxes with readable strings
			foreach (string type in Enum.GetNames(typeof(IPACharacterType)))
				cboType.Items.Add(SeperateWordsWithSpace(type));
			foreach (string type in Enum.GetNames(typeof(IPACharacterSubType)))
				cboSubType.Items.Add(SeperateWordsWithSpace(type));
			foreach (string type in Enum.GetNames(typeof(IPACharIgnoreTypes)))
				cboIgnoreType.Items.Add(SeperateWordsWithSpace(type));

			cboType.SelectedIndex = 0;
			cboSubType.SelectedIndex = 0;
			cboIgnoreType.SelectedIndex = 0;

			LoadChartColumnsGroups();

			m_addingChar = addingChar;
			DataGridViewRow row;
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

				LoadMoaPoaComboBoxes(pciEditor.m_grid);
				CreateDirtyStateHandlers();
				return;
			}

			row = pciEditor.m_grid.CurrentRow;
			if (row.Tag is IPACharInfo)
			{
				m_binaryMask = ((IPACharInfo)row.Tag).BinaryMask;
				m_masks = new ulong[] {((IPACharInfo)row.Tag).Mask0, ((IPACharInfo)row.Tag).Mask1};
				txtBinary.Text = DataUtils.BFeatureCache.GetFeaturesText(m_binaryMask);
				txtArticulatory.Text = DataUtils.AFeatureCache.GetFeaturesText(m_masks);
			}

			// Identity
			txtHexValue.Visible = false;
			lblUnicodeValue.Text = row.Cells[kHexIPAChar].Value as string;
			lblChar.Text = row.Cells[kIpaChar].Value as string;
			txtCharName.Text = row.Cells[kName].Value as string;
			txtCharDesc.Text = row.Cells[kDescription].Value as string;

			// Types
			cboType.SelectedItem = SeperateWordsWithSpace(row.Cells[kCharType].Value as string);
			cboSubType.SelectedItem = SeperateWordsWithSpace(row.Cells[kCharSubType].Value as string);
			cboIgnoreType.SelectedItem = SeperateWordsWithSpace(row.Cells[kIgnoreType].Value as string);

			// Base Character
			chkIsBase.Checked = (bool)row.Cells[kIsBaseChar].Value;
			chkPreceedBaseChar.Checked = (bool)row.Cells[kCanPreceedBaseChar].Value;
			chkDottedCircle.Checked = (bool)row.Cells[kDisplayWDottedCircle].Value;

			// Articulation - load the Moa/Poa combo boxes
			m_original_moa = float.Parse(row.Cells[kMOA].Value.ToString());
			m_original_poa = float.Parse(row.Cells[kPOA].Value.ToString());
			LoadMoaPoaComboBoxes(pciEditor.m_grid);

			// Chart Position
			if (cboType.SelectedItem.ToString() == kConsonant)
			{
				if (!m_CChartGrps.TryGetValue((int)row.Cells[kChartGroup].Value, out m_origChartGroup))
					m_origChartGroup = m_CChartGrps[0];

				if (m_origChartGroup == kOtherSymbols)
					m_origChartColumnOtherSymbol = (int)row.Cells[kChartColumn].Value;
				else if (!m_CChartCols.TryGetValue(
					(int)row.Cells[kChartColumn].Value, out m_origChartColumn))
				{
					m_origChartColumn = m_CChartCols[0];
				}
			}
			else if (cboType.SelectedItem.ToString() == kVowel)
			{
				if (!m_VChartGrps.TryGetValue((int)row.Cells[kChartGroup].Value, out m_origChartGroup))
					m_origChartGroup = m_VChartGrps[0];

				if (!m_VChartCols.TryGetValue((int)row.Cells[kChartColumn].Value, out m_origChartColumn))
					m_origChartColumn = m_VChartCols[0];
			}

			if (m_origChartGroup != string.Empty)
				cboChartGroup.SelectedItem = m_origChartGroup;
			if (m_origChartGroup == kOtherSymbols)
				cboChartColumn.SelectedIndex = m_origChartColumnOtherSymbol;
			else
			{
				if (m_origChartColumn != string.Empty)
					cboChartColumn.SelectedItem = m_origChartColumn;
			}

			CreateDirtyStateHandlers();
			m_dirty = false;
		}
		
		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the Chart Column and Group sorted lists.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		void LoadChartColumnsGroups()
		{
			// Vowel Groups
			m_VChartGrps[0] = kClose;
			m_VChartGrps[1] = kNearClose;
			m_VChartGrps[2] = kCloseMid;
			m_VChartGrps[3] = kMid;
			m_VChartGrps[4] = kOpenMid;
			m_VChartGrps[5] = kNearOpen;
			m_VChartGrps[6] = kOpen;
			m_VChartGrps[7] = kOther;

			// Vowel Columns
			m_VChartCols[0] = kFrontUnrounded;
			m_VChartCols[1] = kFrontRounded;
			m_VChartCols[2] = kNearFrontUnrounded;
			m_VChartCols[3] = kNearFrontRounded;
			m_VChartCols[4] = kCentralUnrounded;
			m_VChartCols[5] = kCentral;
			m_VChartCols[6] = kCentralRounded;
			m_VChartCols[7] = kNearBackRounded;
			m_VChartCols[8] = kBackUnrounded;
			m_VChartCols[9] = kBackRounded;

			// CONSONANT PULMONIC Columns
			m_CChartCols[0] = kVoicelessBilabial;
			m_CChartCols[1] = kVoicedBilabial;
			m_CChartCols[2] = kVoicelessLabiodental;
			m_CChartCols[3] = kVoicedLabiodental;
			m_CChartCols[4] = kVoicelessDental;
			m_CChartCols[5] = kVoicedDental;
			m_CChartCols[6] = kVoicelessAlveolar;
			m_CChartCols[7] = kVoicedAlveolar;
			m_CChartCols[8] = kVoicelessPostalveolar;
			m_CChartCols[9] = kVoicedPostalveolar;
			m_CChartCols[10] = kVoicelessRetroflex;
			m_CChartCols[11] = kVoicedRetroflex;
			m_CChartCols[12] = kVoicedAlvPalatal;
			m_CChartCols[13] = kVoicelessAlvPalatal;
			m_CChartCols[14] = kVoicelessPalatal;
			m_CChartCols[15] = kVoicedPalatal;
			m_CChartCols[16] = kVoicelessVelar;
			m_CChartCols[17] = kVoicedVelar;                      
			m_CChartCols[18] = kVoicelessUvular;
			m_CChartCols[19] = kVoicedUvular;
			m_CChartCols[20] = kVoicelessPharyngeal;
			m_CChartCols[21] = kVoicedPharyngeal;
			m_CChartCols[22] = kVoicelessGlottal;
			m_CChartCols[23] = kVoicedGlottal;
			m_CChartCols[24] = kVoicelessEpiglottal;
			m_CChartCols[25] = kVoicedEpiglottal;

			// CONSONANT Groups
			m_CChartGrps[0] = kPlosive;
			m_CChartGrps[1] = kNasal;
			m_CChartGrps[2] = kTrill;
			m_CChartGrps[3] = kTapOrFlap;
			m_CChartGrps[4] = kFricative;
			m_CChartGrps[5] = kLateralFricative;
			m_CChartGrps[6] = kApproximant;
			m_CChartGrps[7] = kLateralApproximant;
			m_CChartGrps[8] = kImplosive;
			m_CChartGrps[9] = kClick;
			m_CChartGrps[10] = kOther;
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Create DirtyStateChanged event handlers.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		void CreateDirtyStateHandlers()
		{
			txtHexValue.TextChanged += ipaCharAdd_DirtyStateChanged;
			txtCharName.TextChanged += ipaCharAdd_DirtyStateChanged;
			txtCharDesc.TextChanged += ipaCharAdd_DirtyStateChanged;
			cboType.SelectedIndexChanged += ipaCharAdd_DirtyStateChanged;
			cboSubType.SelectedIndexChanged += ipaCharAdd_DirtyStateChanged;
			cboIgnoreType.SelectedIndexChanged += ipaCharAdd_DirtyStateChanged;
			chkIsBase.CheckStateChanged += ipaCharAdd_DirtyStateChanged;
			chkPreceedBaseChar.CheckStateChanged += ipaCharAdd_DirtyStateChanged;
			chkDottedCircle.CheckStateChanged += ipaCharAdd_DirtyStateChanged;
			cboMoa.SelectedValueChanged += ipaCharAdd_DirtyStateChanged;
			cboPoa.SelectedValueChanged += ipaCharAdd_DirtyStateChanged;
			cboChartColumn.SelectedValueChanged += ipaCharAdd_DirtyStateChanged;
			cboChartGroup.SelectedValueChanged += ipaCharAdd_DirtyStateChanged;
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// The data has been changed, so set the dirty flag.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		void ipaCharAdd_DirtyStateChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the MOA and POA combo boxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadMoaPoaComboBoxes(SilGrid charGrid)
		{
			foreach (DataGridViewRow gridRow in charGrid.Rows)
			{
				// Save unique codePoint's / IPAChar's for Verification later
				int codePoint = (int)gridRow.Cells[kCodePoint].Value;
				if (!m_codePoints.Contains(codePoint))
					m_codePoints.Add(codePoint);

				if (gridRow.Cells[kCodePoint].Value != null && gridRow.Cells[kCharType].Value != null)
				{
					if ((int)gridRow.Cells[kCodePoint].Value <= invalidCodePoint ||
						(string)gridRow.Cells[kCharType].Value == kUnknown)
						continue;
				}

				if (gridRow.Cells[kIpaChar].Value != null)
				{
					// Create sorted lists of the manners and points of articulation.
					float moa = float.Parse(gridRow.Cells[kMOA].Value.ToString());
					float poa = float.Parse(gridRow.Cells[kPOA].Value.ToString());
					m_MOA[moa] = gridRow.Cells[kIpaChar].Value.ToString();
					m_POA[poa] = gridRow.Cells[kIpaChar].Value.ToString();
				}
			}

			// Load the combo boxes
			foreach (KeyValuePair<float, string> moa in m_MOA)
			{
				if (moa.Key != m_original_moa)
					cboMoa.Items.Add(moa.Value);
				else
					cboMoa.SelectedIndex = cboMoa.Items.Count - 1;
			}

			foreach (KeyValuePair<float, string> poa in m_POA)
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
		private static string SeperateWordsWithSpace(string multiWord)
		{
			string newName = string.Empty;
			foreach (char letter in multiWord)
			{
				if (Char.IsUpper(letter))
					newName += ' ';
				newName += letter;
			}
			return newName.Trim();
		}
		
		#endregion

		#region Private Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chart group combo box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadChartGroupItems()
		{
			cboChartGroup.Items.Clear();
			cboChartColumn.Items.Clear();

			if (cboType.SelectedItem.ToString() == kConsonant)
			{
				foreach (KeyValuePair<float, string> col in m_CChartGrps)
					cboChartGroup.Items.Add(col.Value);
			}
			else if (cboType.SelectedItem.ToString() == kVowel)
			{
				foreach (KeyValuePair<float, string> col in m_VChartGrps)
					cboChartGroup.Items.Add(col.Value);
			}
			else
			{
				cboChartGroup.Enabled = false;
				return;
			}

			// Select the correct cbo item
			if (cboChartGroup.Items.Contains(m_origChartGroup))
				cboChartGroup.SelectedItem = m_origChartGroup;
			else if (cboChartGroup.Items.Count > 0)
				cboChartGroup.SelectedIndex = 0;

			cboChartGroup.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chart column combo box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadChartColumnItems()
		{
			cboChartColumn.Items.Clear();

			if (cboType.SelectedItem.ToString() == kConsonant)
			{
				foreach (KeyValuePair<float, string> col in m_CChartCols)
					cboChartColumn.Items.Add(col.Value);
			}
			else if (cboType.SelectedItem.ToString() == kVowel)
			{
				foreach (KeyValuePair<float, string> col in m_VChartCols)
					cboChartColumn.Items.Add(col.Value);
			}
			else
			{
				cboChartColumn.Enabled = false;
				return;
			}

			if (cboChartColumn.Items.Contains(m_origChartColumn))
				cboChartColumn.SelectedItem = m_origChartColumn;
			else if (cboChartColumn.Items.Count > 0)
				cboChartColumn.SelectedIndex = 0;

			cboChartColumn.Enabled = true;
		}

		#endregion

		#region Overrides
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			m_pciEditor.m_sddpAFeatures.SavedSettingsName = m_saveAFeatureDropDownName;
			m_pciEditor.m_sddpBFeatures.SavedSettingsName = m_saveBFeatureDropDownName;
			m_pciEditor.m_aFeatureDropdown.Closing -= m_aFeatureDropdown_Closing;
			m_pciEditor.m_bFeatureDropdown.Closing -= m_bFeatureDropdown_Closing;

			// Hook up the delegates back on the main form.
			m_pciEditor.m_aFeatureDropdown.Closing += m_pciEditor.m_featureDropdown_Closing;
			m_pciEditor.m_bFeatureDropdown.Closing += m_pciEditor.m_featureDropdown_Closing;

			PCIEditor.SettingsHandler.SaveFormProperties(this);
			base.OnFormClosing(e);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return the column key.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int GetColumnKey(SortedList<float, string> sortedList)
		{
			// Check if the selectedItem is a number
			if (cboChartColumn.SelectedItem.ToString().Length < 3)
				return (int)cboChartColumn.SelectedItem;

			foreach (KeyValuePair<float, string> column in sortedList)
			{
				if (column.Value == cboChartColumn.SelectedItem.ToString())
				{
					return (int)column.Key;
				}
			}
			return 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return the group key.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int GetGroupKey(SortedList<float, string> sortedList)
		{
			// Check if the selectedItem is a number
			if (cboChartGroup.SelectedItem.ToString().Length < 3)
				return (int)cboChartGroup.SelectedItem;

			foreach (KeyValuePair<float, string> column in sortedList)
			{
				if (column.Value == cboChartGroup.SelectedItem.ToString())
				{
					return (int)column.Key;
				}
			}
			return 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CharInfo object created from the changes made on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharInfo CharInfo
		{
			get { return m_charInfo; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save Changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			m_charInfo = new IPACharInfo();

			m_charInfo.Codepoint = int.Parse((m_addingChar ?
				txtHexValue.Text : lblUnicodeValue.Text), NumberStyles.HexNumber);

			m_charInfo.HexIPAChar = (m_addingChar ? txtHexValue.Text : lblUnicodeValue.Text);
			m_charInfo.IPAChar = lblChar.Text;
			m_charInfo.Name = txtCharName.Text;
			m_charInfo.Description = txtCharDesc.Text;
			m_charInfo.BinaryMask = m_binaryMask;
			
			// Features
			m_charInfo.Mask0 = m_masks[0];
			m_charInfo.Mask1 = m_masks[1];

			// Types - remove the spaces in the Type strings
			m_charInfo.CharType = (IPACharacterType)Enum.Parse(
				typeof(IPACharacterType), cboType.SelectedItem.ToString().Replace(" ", ""));
			m_charInfo.CharSubType = (IPACharacterSubType)Enum.Parse(
				typeof(IPACharacterSubType), cboSubType.SelectedItem.ToString().Replace(" ", ""));
			m_charInfo.IgnoreType = (IPACharIgnoreTypes)Enum.Parse(
				typeof(IPACharIgnoreTypes), cboIgnoreType.SelectedItem.ToString().Replace(" ", ""));

			// Base Character
			m_charInfo.IsBaseChar = chkIsBase.Checked;
			m_charInfo.CanPreceedBaseChar = chkPreceedBaseChar.Checked;
			m_charInfo.DisplayWDottedCircle = chkDottedCircle.Checked;

			// Save the manner of articulation sort order value
			if (cboMoa.SelectedItem == null)
			{
				// use original value if not modified
				m_charInfo.MOArticulation = (int)m_original_moa;
			}
			else
			{
				foreach (KeyValuePair<float, string> moa in m_MOA)
				{
					if (moa.Value == cboMoa.SelectedItem.ToString())
					{
						// Make sure the user actually changed the MOA
						m_charInfo.MOArticulation =
							(int)(m_original_moa == moa.Key ? m_original_moa : moa.Key + 1f);
						break;
					}
				}
			}

			// Save the place of articulation sort order value
			if (cboPoa.SelectedItem == null)
			{
				// use original value if not modified
				m_charInfo.POArticulation = (int)m_original_poa;
			}
			else
			{
				foreach (KeyValuePair<float, string> poa in m_POA)
				{
					if (poa.Value == cboPoa.SelectedItem.ToString())
					{
						// Make sure the user actually changed the POA
						m_charInfo.POArticulation =
							(int)(m_original_poa == poa.Key ? m_original_poa : poa.Key + 1f);
						break;
					}
				}
			}

			// Save Chart Position
			if (cboType.SelectedItem.ToString() == kConsonant)
			{
				m_charInfo.ChartGroup = GetGroupKey(m_CChartGrps);
				m_charInfo.ChartColumn = GetColumnKey(m_CChartCols);
			}
			else if (cboType.SelectedItem.ToString() == kVowel)
			{
				m_charInfo.ChartGroup = GetGroupKey(m_VChartGrps);
				m_charInfo.ChartColumn = GetColumnKey(m_VChartCols);
			}
			else
			{
				m_charInfo.ChartColumn = 0;
				m_charInfo.ChartGroup = 0;
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
					STUtils.STMsgBox(Resources.kstidInvalidUnicodeValueMsg,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				int codePoint = int.Parse(txtHexValue.Text.Trim(), NumberStyles.HexNumber);
				if (codePoint <= invalidCodePoint)
				{
					STUtils.STMsgBox(Resources.kstidUnicodeValueTooSmallMsg,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				// Make sure the codePoint is unique
				if (m_codePoints.Contains(codePoint))
				{
					STUtils.STMsgBox(string.Format(Resources.kstidDuplicateCharMsg,
						txtHexValue.Text), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				// Make sure the code point isn't one of the reserved characters.
				foreach (char c in m_invalidPhoneticChars)
				{
					if (codePoint == c)
					{
						STUtils.STMsgBox(string.Format(
							Properties.Resources.kstidUnicodeValueIsReservedMsg,
							txtHexValue.Text, kInvalidPhoneticCharsDisplay),
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					
						return false;
					}
				}
			}

			bool mustHaveColAndGrp = (cboType.SelectedItem != null && (
				cboType.SelectedItem.ToString() == kConsonant ||
				cboType.SelectedItem.ToString() == kVowel));

			string missingFields = string.Empty;
			if (m_addingChar && txtHexValue.Text == string.Empty)
				missingFields += (lblUnicode.Text + ", ");
			if (txtCharName.Text == string.Empty)
				missingFields += (lblName.Text + ", ");
			if (m_addingChar && cboMoa.SelectedItem == null)
				missingFields += (kLblMoa + ", ");
			if (m_addingChar && cboPoa.SelectedItem == null)
				missingFields += (kLblPoa + ", ");
			if (cboChartColumn.SelectedItem == null && mustHaveColAndGrp)
				missingFields += (kLblChartColumn + ", ");
			if (cboChartGroup.SelectedItem == null && mustHaveColAndGrp)
				missingFields += (kLblChartGroup + ", ");

			if (missingFields != string.Empty)
			{
				missingFields = missingFields.Replace("&&", "~~");
				missingFields = missingFields.Replace("&", string.Empty);
				missingFields = missingFields.Replace("~~", "&");
				missingFields = missingFields.Replace(":", string.Empty);
				missingFields = missingFields.TrimEnd(new char[] { ',', ' ' });
				missingFields = string.Format(Resources.kstidMissingFieldsMsg, missingFields);
				STUtils.STMsgBox(missingFields, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			return true;
		}

		#endregion
		
		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked the OK button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked the CANCEL button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_dirty = false;
			m_charInfo = null;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			PCIEditor.ShowHelpTopic(@"Phonetic_Character_Inventory_Editor/Phonetic_Character_Properties.htm");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the Selected Index Changed event for the Type combo box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cboType_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadChartGroupItems();
			LoadChartColumnItems();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnArticulatory_Click(object sender, EventArgs e)
		{
			// Use the drop-down from the main form's grid.
			m_pciEditor.m_lvAFeatures.CurrentMasks = m_masks;
			Point pt = new Point(0, hlblArticulatory.Height);
			pt = hlblArticulatory.PointToScreen(pt);
			m_pciEditor.m_aFeatureDropdown.Show(pt);
			m_pciEditor.m_lvAFeatures.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnBinary_Click(object sender, EventArgs e)
		{
			// Use the drop-down from the main form's grid.
			m_pciEditor.m_lvBFeatures.CurrentMasks = new ulong[] {m_binaryMask, 0};
			Point pt = new Point(0, hlblBinary.Height);
			pt = hlblBinary.PointToScreen(pt);
			m_pciEditor.m_bFeatureDropdown.Show(pt);
			m_pciEditor.m_lvBFeatures.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the articulatory feature list based on what the user chose
		/// in the articulatory feature drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_aFeatureDropdown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (m_pciEditor.m_lvAFeatures.CurrentMasks[0] == m_masks[0] &&
				m_pciEditor.m_lvAFeatures.CurrentMasks[1] == m_masks[1])
			{
				return;
			}

			m_masks = new ulong[] {m_pciEditor.m_lvAFeatures.CurrentMasks[0],
				m_pciEditor.m_lvAFeatures.CurrentMasks[1]};

			txtArticulatory.Text = DataUtils.AFeatureCache.GetFeaturesText(m_masks);
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the binary feature list based on what the user chose in the binary feature
		/// drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_bFeatureDropdown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (m_pciEditor.m_lvBFeatures.CurrentMasks[0] != m_binaryMask)
			{
				m_binaryMask = m_pciEditor.m_lvBFeatures.CurrentMasks[0];
				txtBinary.Text = DataUtils.BFeatureCache.GetFeaturesText(m_binaryMask);
				m_dirty = true;
			}
		}
	}
}