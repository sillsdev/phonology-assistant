using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.Pa.Dialogs;
using SIL.Pa.Properties;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	public partial class PCIEditor : OKCancelDlgBase
	{
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
		private const string kChartColumn = "ChartColumn";
		private const string kChartGroup = "ChartGroup";
		private const string kAFeatures = "afeatures";
		private const string kBFeatures = "bfeatures";
		private const string kUnknown = "Unknown";
		// These values of these fields should be left at zero
		private const string kDisplayOrder = "DisplayOrder";
		private const string kMask0 = "Mask0";
		private const string kMask1 = "Mask1";
		private const string kBinaryMask = "BinaryMask";

		private const int kDefaultFeatureColWidth = 225;

		private readonly bool m_amTesting = false;
		private readonly List<string> unknownCharTypes;
		private readonly List<string> knownCharTypes;
		private string m_xmlFilePath = string.Empty;
		internal SilGrid m_grid;
		private readonly List<int> m_codePoints = new List<int>();
		private List<IPACharInfo> m_charInventory;
		private readonly int invalidCodePoint = 31;
		private readonly SortedDictionary<int, DataGridViewRow> m_gridDictionary =
			new SortedDictionary<int, DataGridViewRow>();

		// The SortedList Key is the moa or poa and the Value is the hexIpaChar
		private readonly SortedList<float, int> m_MOA = new SortedList<float, int>();
		private readonly SortedList<float, int> m_POA = new SortedList<float, int>();

		private DataGridViewColumn m_lastSortedCol;
		private ListSortDirection m_lastSortDirection = ListSortDirection.Ascending;

		private const string kInventoryFile = "PhoneticCharacterInventory.xml";
		private static SettingsHandler s_settingsHndlr;

		internal SizableDropDownPanel m_sddpAFeatures;
		internal CustomDropDown m_aFeatureDropdown;
		internal FeatureListView m_lvAFeatures;
		internal SizableDropDownPanel m_sddpBFeatures;
		internal CustomDropDown m_bFeatureDropdown;
		internal FeatureListView m_lvBFeatures;

		private static SmallFadingWnd s_loadingWnd = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[STAThread]
		public static void Main(string[] rgArgs)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			s_loadingWnd = new SmallFadingWnd(Properties.Resources.kstidLoadingProgramMsg);

			string exePath = Path.GetDirectoryName(Application.ExecutablePath);

			// This is the poor man's way of determining whether or not the user has
			// write access to the folder in which the phonetic character inventory
			// is stored. I'm sure there is a great class in .Net to use for such a
			// thing, but I couldn't find it in the little bit of digging I did. Sigh!
			string tmpFile = Path.Combine(exePath, "!~tmpaccesstest~!");
			try
			{
				File.WriteAllText(tmpFile, string.Empty);
			}
			catch
			{
				string msg = string.Format(Resources.kstidWriteAccessErrorMsg,
					Path.GetFileName(Application.ExecutablePath));
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			File.Delete(tmpFile);
			
			// Make sure the phonetic character inventory file exists.
			string inventoryPath = Path.Combine(exePath, kInventoryFile);
			if (!File.Exists(inventoryPath))
			{
				string filePath = STUtils.PrepFilePathForSTMsgBox(inventoryPath);
				string msg = string.Format(Resources.kstidInventoryFileMissing, filePath);
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			s_settingsHndlr = new PaSettingsHandler(Path.Combine(exePath, "pcieditor.xml"));
			PCIEditor editor = new PCIEditor();

			//if (rgArgs != null && rgArgs.Length > 0)
			//    editor.OpenFile(rgArgs[0]);

			editor.OpenFile(inventoryPath);
			Application.Run(editor);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the settings handler for the application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SettingsHandler SettingsHandler
		{
			get { return s_settingsHndlr; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// PCIEditor Constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PCIEditor()
		{
			InitializeComponent();

			// Load 'knownCharTypes'
			knownCharTypes = new List<string>();
			knownCharTypes.Add("Consonant");
			knownCharTypes.Add("Suprasegmentals");

			// Load 'unknownCharTypes'
			unknownCharTypes = new List<string>();
			unknownCharTypes.Add("Vowel");
			unknownCharTypes.Add("Diacritics");
			unknownCharTypes.Add("Breaking");

			s_settingsHndlr.LoadFormProperties(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeFeatureDropDowns()
		{
			// Build the articulatory features drop-down.
			m_sddpAFeatures = new SizableDropDownPanel(s_settingsHndlr, Name + "AFeatureDropDown",
				new Size((int)(kDefaultFeatureColWidth * 2.5), 175));
			m_sddpAFeatures.MinimumSize = new Size(200, 100);
			m_sddpAFeatures.BorderStyle = BorderStyle.FixedSingle;
			m_sddpAFeatures.Padding = new Padding(7, 0, 7, m_sddpAFeatures.Padding.Bottom);

			Label lbl = new Label();
			lbl.AutoSize = false;
			lbl.Text = Resources.kstidAfeaturesHdg;
			lbl.Font = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			lbl.Dock = DockStyle.Top;
			lbl.Height = lbl.PreferredHeight + 10;
			lbl.TextAlign = ContentAlignment.MiddleLeft;
			m_sddpAFeatures.Controls.Add(lbl);

			m_aFeatureDropdown = new CustomDropDown();
			m_aFeatureDropdown.AutoCloseWhenMouseLeaves = false;
			m_aFeatureDropdown.AddControl(m_sddpAFeatures);
			m_aFeatureDropdown.Closing += m_featureDropdown_Closing;

			m_lvAFeatures = new FeatureListView(PaApp.FeatureType.Articulatory, m_aFeatureDropdown);
			m_lvAFeatures.Dock = DockStyle.Fill;
			m_lvAFeatures.Load();
			m_sddpAFeatures.Controls.Add(m_lvAFeatures);
			m_lvAFeatures.BringToFront();

			// Build the binary features drop-down.
			m_sddpBFeatures = new SizableDropDownPanel(s_settingsHndlr, Name + "BFeatureDropDown",
				new Size((int)(kDefaultFeatureColWidth * 2.5), 175));
			m_sddpBFeatures.MinimumSize = new Size(200, 100);
			m_sddpBFeatures.BorderStyle = BorderStyle.FixedSingle;
			m_sddpBFeatures.Padding = new Padding(7, 0, 7, m_sddpBFeatures.Padding.Bottom);

			lbl = new Label();
			lbl.AutoSize = false;
			lbl.Text = Resources.kstidBFeaturesHdg;
			lbl.Font = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			lbl.Dock = DockStyle.Top;
			lbl.Height = lbl.PreferredHeight + 10;
			lbl.TextAlign = ContentAlignment.MiddleLeft;
			m_sddpBFeatures.Controls.Add(lbl);

			m_bFeatureDropdown = new CustomDropDown();
			m_bFeatureDropdown.AutoCloseWhenMouseLeaves = false;
			m_bFeatureDropdown.AddControl(m_sddpBFeatures);
			m_bFeatureDropdown.Closing += m_featureDropdown_Closing;

			m_lvBFeatures = new FeatureListView(PaApp.FeatureType.Binary, m_bFeatureDropdown);
			m_lvBFeatures.Dock = DockStyle.Fill;
			m_lvBFeatures.Load();
			m_sddpBFeatures.Controls.Add(m_lvBFeatures);
			m_lvBFeatures.BringToFront();

			if (!PaintingHelper.CanPaintVisualStyle())
			{
				m_lvAFeatures.BorderStyle = BorderStyle.FixedSingle;
				m_lvBFeatures.BorderStyle = BorderStyle.FixedSingle;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (s_loadingWnd != null)
			{
				s_loadingWnd.CloseFade();
				s_loadingWnd.Dispose();
				s_loadingWnd = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update a phones feature mask(s) after one of the feature drop-down lists closes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_featureDropdown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			// If this form doesn't have focus, it probably means the drop-down was used
			// and closed on the AddCharacterDlg.
			if (!Focused)
				m_grid.Focus();

			FeatureListView lv;
			if (sender == m_aFeatureDropdown)
				lv = m_lvAFeatures;
			else if (sender == m_bFeatureDropdown)
				lv = m_lvBFeatures;
			else
				return;

			if (m_grid.CurrentRow == null)
				return;

			IPACharInfo charInfo = m_grid.CurrentRow.Tag as IPACharInfo;
			if (charInfo == null)
				return;

			bool changed;

			if (sender == m_bFeatureDropdown)
			{
				changed = (charInfo.BinaryMask != lv.CurrentMasks[0]);
				charInfo.BinaryMask = lv.CurrentMasks[0];
				m_grid.CurrentRow.Cells[kBinaryMask].Value = lv.CurrentMasks[0];
				m_grid.CurrentRow.Cells[kBFeatures].Value =
					DataUtils.BFeatureCache.GetFeaturesText(charInfo.BinaryMask);
			}
			else
			{
				changed = (charInfo.Mask0 != lv.CurrentMasks[0] || charInfo.Mask1 != lv.CurrentMasks[1]);
				charInfo.Mask0 = lv.CurrentMasks[0];
				charInfo.Mask1 = lv.CurrentMasks[1];
				m_grid.CurrentRow.Cells[kMask0].Value = lv.CurrentMasks[0];
				m_grid.CurrentRow.Cells[kMask1].Value = lv.CurrentMasks[1];
				m_grid.CurrentRow.Cells[kAFeatures].Value =
					DataUtils.AFeatureCache.GetFeaturesText(lv.CurrentMasks);
			}

			if (!m_dirty)
				m_dirty = changed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the form's state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			s_settingsHndlr.SaveFormProperties(this);
			s_settingsHndlr.SaveGridProperties(m_grid);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveInventory()
		{
			List<IPACharInfo> tmpCache = new List<IPACharInfo>();

			foreach (DataGridViewRow row in m_gridDictionary.Values)
			{
				IPACharInfo info = new IPACharInfo();
				info.Codepoint = (int)row.Cells[kCodePoint].Value;

				info.IPAChar = (info.Codepoint < 0 ? row.Cells[kIpaChar].Value as string :
					((char)info.Codepoint).ToString());
				
				info.HexIPAChar = row.Cells[kHexIPAChar].Value as string;
				info.Name = (string)row.Cells[kName].Value;
				info.Description = (string)row.Cells[kDescription].Value;
				info.CharType = (IPACharacterType)Enum.Parse(
					typeof(IPACharacterType), (string)row.Cells[kCharType].Value);
				info.CharSubType = (IPACharacterSubType)Enum.Parse(
					typeof(IPACharacterSubType), (string)row.Cells[kCharSubType].Value);
				info.IgnoreType = (IPACharIgnoreTypes)Enum.Parse(
					typeof(IPACharIgnoreTypes), (string)row.Cells[kIgnoreType].Value);
				info.IsBaseChar = (bool)row.Cells[kIsBaseChar].Value;
				info.CanPreceedBaseChar = (bool)row.Cells[kCanPreceedBaseChar].Value;
				info.DisplayWDottedCircle = (bool)row.Cells[kDisplayWDottedCircle].Value;
				info.DisplayOrder = (int)row.Cells[kDisplayOrder].Value;
				info.MOArticulation = (int)row.Cells[kMOA].Value;
				info.POArticulation = (int)row.Cells[kPOA].Value;
				info.Mask0 = (ulong)row.Cells[kMask0].Value;
				info.Mask1 = (ulong)row.Cells[kMask1].Value;
				info.BinaryMask = (ulong)row.Cells[kBinaryMask].Value;
				info.ChartColumn = (int)row.Cells[kChartColumn].Value;
				info.ChartGroup = (int)row.Cells[kChartGroup].Value;

				tmpCache.Add(info);
			}

			STUtils.SerializeData(m_xmlFilePath, tmpCache);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// The data has been changed, so set the dirty flag.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		void m_grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return true if data is OK.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			m_codePoints.Clear();

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				// Don't verify the 'new' record at the very bottom
				if (row.Cells[kHexIPAChar].Value == null || row.Cells[kCodePoint].Value == null)
					continue;

				int codePoint = (int)row.Cells[kCodePoint].Value;
				m_codePoints.Add(codePoint);

				string charTypeVal = row.Cells[kCharType].Value.ToString();
				string charSubTypeVal = row.Cells[kCharSubType].Value.ToString();

				// Don't allow CharType's of 'Unknown'
				if (row.Visible && row.Cells[kCharType].Value.ToString() == kUnknown)
					return ShowErrorMessage(Resources.kstidIpaGridErrUnknownCharType, row);

				if (unknownCharTypes.Contains(charTypeVal))
				{
					if (charSubTypeVal != kUnknown)
					{
						return ShowErrorMessage(string.Format(
							Resources.kstidIpaGridErrNeedUnknown, charTypeVal), row);
					}
				}

				if (knownCharTypes.Contains(charTypeVal))
				{
					if (charSubTypeVal == kUnknown)
						return ShowErrorMessage(
							string.Format(Resources.kstidIpaGridErrRemoveUnknown, charTypeVal), row);
				}

				bool isBaseChar = (bool)row.Cells[kIsBaseChar].Value;
				bool canProceedBase = (bool)row.Cells[kCanPreceedBaseChar].Value;

				if (isBaseChar && canProceedBase)
					return ShowErrorMessage(Resources.kstidIpaGridErrBothTrue, row);
			}
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the warning message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void warningMessage(string errTitle, string errMsg)
		{
			MessageBox.Show(errMsg, errTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the error message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ShowErrorMessage(string errMsg, DataGridViewRow row)
		{
			row.Selected = true;
			m_grid.FirstDisplayedScrollingRowIndex = row.Index; // Scroll error row to the top
			warningMessage(Resources.kstidIpaGridErrTitleDataConflict, errMsg);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called after data has been determined to be dirty, verified and OK is clicked or
		/// the user has confirmed saving the changes.
		/// </summary>
		/// <returns>False if closing the form should be canceled. Otherwise, true.</returns>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			RenumberArticulators();
			SaveInventory();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Renumber the articulation (moa & poa) values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RenumberArticulators()
		{
			int newValue = 0;
			m_gridDictionary.Clear();
			m_MOA.Clear();
			m_POA.Clear();

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				// Don't verify the 'new' record at the very bottom
				if (row.Cells[kHexIPAChar].Value == null || row.Cells[kCodePoint].Value == null)
					continue;

				int codepoint = (int)row.Cells[kCodePoint].Value;
				m_gridDictionary[codepoint] = row;

				if (codepoint <= invalidCodePoint || row.Cells[kCharType].Value.ToString() == kUnknown)
				{
					// Make the MOA & POA negative
					newValue -= 5;
					row.Cells[kMOA].Value = newValue;
					row.Cells[kPOA].Value = newValue;
				}
				else
				{
					LoadArticulators(float.Parse(row.Cells[kMOA].Value.ToString()), codepoint, m_MOA);
					LoadArticulators(float.Parse(row.Cells[kPOA].Value.ToString()), codepoint, m_POA);
				}
			}

			UpdateArticulators();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the articulation (moa & poa) values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void LoadArticulators(float val, int codepoint,
			SortedList<float, int> articulators)
		{
			while (true)
			{
				if (articulators.ContainsKey(val))
					val += 0.1F; // Fix any duplicate articulators
				else
				{
					// Key is articulationVal (moa or poa) and Value is "Name + HexIpa"
					articulators[val] = codepoint;
					return;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update moa & poa in the grid dictionary with new values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateArticulators()
		{
			int newValue = 0;
			foreach (int codepoint in m_MOA.Values)
			{
				newValue += 5;
				m_gridDictionary[codepoint].Cells[kMOA].Value = newValue;
			}

			newValue = 0;
			foreach (int codepoint in m_POA.Values)
			{
				newValue += 5;
				m_gridDictionary[codepoint].Cells[kPOA].Value = newValue;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open the DefaultIPACharCache file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string OpenDefaultIPACharCache()
		{
			string fileName = string.Empty;

			while (fileName == string.Empty)
			{
				// clear the status bar
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Title = Resources.kstidIpaGridOpenFileTitle;
				dlg.Filter = Resources.kstidIpaGridOpenFileFilter;
				// Set the initial directory to the startup path
				dlg.InitialDirectory = Application.StartupPath;
				dlg.Multiselect = false;
				dlg.ShowReadOnly = false;
				dlg.FilterIndex = 1;
				dlg.ValidateNames = true;

				if (dlg.ShowDialog(this) == DialogResult.Cancel)
					fileName = "NONE";
				if (dlg.FileName.Length > 0)
					fileName = dlg.FileName;
			}

			return fileName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to charCache grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			DataGridViewColumn col;

			// Add the HexIpa field column.
			col = SilGrid.CreateTextBoxColumn(kHexIPAChar);
			col.HeaderText = STUtils.ConvertLiteralNewLines(Resources.kstidIpaGridHexIpa);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ReadOnly = true;
			col.Frozen = true;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.BackColor = ColorHelper.CalculateColor(Color.Black, SystemColors.Window, 15);
			m_grid.Columns.Add(col);

			// Add the IpaChar field column.
			col = SilGrid.CreateTextBoxColumn(kIpaChar);
			col.HeaderText = STUtils.ConvertLiteralNewLines(Resources.kstidIpaGridIpaChar);
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.DefaultCellStyle.BackColor = ColorHelper.CalculateColor(Color.Black, SystemColors.Window, 15);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ReadOnly = true;
			col.Frozen = true;
			m_grid.Columns.Add(col);

			// Add the CodePoint field column (NOT a visible column).
			col = SilGrid.CreateTextBoxColumn(kCodePoint);
			col.Visible = false;
			m_grid.Columns.Add(col);

			// Add the Name field column.
			col = SilGrid.CreateTextBoxColumn(kName);
			col.HeaderText = Resources.kstidIpaGridName;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			m_grid.Columns.Add(col);

			// Add the Description field column.
			col = SilGrid.CreateTextBoxColumn(kDescription);
			col.HeaderText = Resources.kstidIpaGridDesc;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			m_grid.Columns.Add(col);

			// Add the CharType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kCharType, Enum.GetNames(typeof(IPACharacterType)));
			col.HeaderText = Resources.kstidIpaGridCharType;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			m_grid.Columns.Add(col);

			// Add the CharSubType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kCharSubType, Enum.GetNames(typeof(IPACharacterSubType)));
			col.HeaderText = Resources.kstidIpaGridCharSubType;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			m_grid.Columns.Add(col);

			// Add the CharIgnoreType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kIgnoreType, Enum.GetNames(typeof(IPACharIgnoreTypes)));
			col.HeaderText = Resources.kstidIpaGridCharIgnoreType;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			m_grid.Columns.Add(col);

			// Add the IsBaseChar check box column.
			col = SilGrid.CreateCheckBoxColumn(kIsBaseChar);
			col.HeaderText = Resources.kstidIpaGridIsBase;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(bool);
			m_grid.Columns.Add(col);

			// Add the Can preceed base character check box column.
			col = SilGrid.CreateCheckBoxColumn(kCanPreceedBaseChar);
			col.HeaderText = STUtils.ConvertLiteralNewLines(Resources.kstidIpaGridCanPreceedBase);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(bool);
			m_grid.Columns.Add(col);

			// Add the DisplayWDottedCircle check box column.
			col = SilGrid.CreateCheckBoxColumn(kDisplayWDottedCircle);
			col.HeaderText = STUtils.ConvertLiteralNewLines(Resources.kstidIpaGridWDotCircle);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(bool);
			m_grid.Columns.Add(col);

			// Add the Display Order column.
			col = SilGrid.CreateTextBoxColumn(kDisplayOrder);
			col.HeaderText = STUtils.ConvertLiteralNewLines(Resources.kstidIpaGridDisplayOrder);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.Visible = false;	// Currently, not used.
			m_grid.Columns.Add(col);

			// Add the MOA column.
			col = SilGrid.CreateTextBoxColumn(kMOA);
			col.HeaderText = Resources.kstidIpaGridMOA;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(int);
			if (!m_amTesting)
				col.Visible = false;
			m_grid.Columns.Add(col);

			// Add the POA column.
			col = SilGrid.CreateTextBoxColumn(kPOA);
			col.HeaderText = Resources.kstidIpaGridPOA;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(int);
			if (!m_amTesting)
				col.Visible = false;
			m_grid.Columns.Add(col);

			// Add the Mask0 field (NOT a visible column).
			col = SilGrid.CreateTextBoxColumn(kMask0);
			col.Visible = false;
			m_grid.Columns.Add(col);

			// Add the Mask1 field (NOT a visible column).
			col = SilGrid.CreateTextBoxColumn(kMask1);
			col.Visible = false;
			m_grid.Columns.Add(col);

			// Add the BinaryMask field (NOT a visible column).
			col = SilGrid.CreateTextBoxColumn(kBinaryMask);
			col.Visible = false;
			m_grid.Columns.Add(col);

			// Add the ChartColumn column.
			col = SilGrid.CreateTextBoxColumn(kChartColumn);
			col.HeaderText = STUtils.ConvertLiteralNewLines(Resources.kstidIpaGridChartColumn);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(int);
			if (!m_amTesting)
				col.Visible = false;
			m_grid.Columns.Add(col);

			// Add the ChartGroup column.
			col = SilGrid.CreateTextBoxColumn(kChartGroup);
			col.HeaderText = STUtils.ConvertLiteralNewLines(Resources.kstidIpaGridChartGroup);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(int);
			if (!m_amTesting)
				col.Visible = false;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateSilButtonColumn(kAFeatures);
			col.ReadOnly = true;
			col.HeaderText = Resources.kstidAfeaturesHdg;
			col.Width = kDefaultFeatureColWidth;
			((SilButtonColumn)col).UseComboButtonStyle = true;
			((SilButtonColumn)col).ButtonClicked += HandleFeatureDropDownClick;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateSilButtonColumn(kBFeatures);
			col.ReadOnly = true;
			col.HeaderText = Resources.kstidBFeaturesHdg;
			col.Width = kDefaultFeatureColWidth;
			((SilButtonColumn)col).UseComboButtonStyle = true;
			((SilButtonColumn)col).ButtonClicked += HandleFeatureDropDownClick;
			m_grid.Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFeatureDropDownClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (m_grid.CurrentRow == null)
				return;

			IPACharInfo charInfo = m_grid.CurrentRow.Tag as IPACharInfo;
			if (charInfo == null)
				return;

			// Figure out what drop-down and list view to use based on the column index.
			FeatureListView lv;
			CustomDropDown cdd;
			if (m_grid.Columns[e.ColumnIndex].Name == kAFeatures)
			{
				cdd = m_aFeatureDropdown;
				lv = m_lvAFeatures;
				lv.CurrentMasks = new ulong[] { charInfo.Mask0, charInfo.Mask1 };
			}
			else if (m_grid.Columns[e.ColumnIndex].Name == kBFeatures)
			{
				cdd = m_bFeatureDropdown;
				lv = m_lvBFeatures;
				lv.CurrentMasks = new ulong[] { charInfo.BinaryMask, 0 };
			}
			else
				return;

			Rectangle rc = m_grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			
			// Use this line to align the right edge of the drop-down to
			// the right edge of the cell.
			// Point pt = new Point(rc.Right - cdd.Width, rc.Bottom);

			// Use this line to align the left edge of the drop-down to
			// the left edge of the cell.
			Point pt = new Point(rc.X, rc.Bottom);
			
			pt = m_grid.PointToScreen(pt);
			cdd.Show(pt);
			lv.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load grid of all the field fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadRows()
		{
			foreach (IPACharInfo charInfo in m_charInventory)
				LoadRowWithCharInfo(null, charInfo);

			// Hide all rows with a CharType of "Unknown" OR a CodePoint that is below 32
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (row.Cells[kCodePoint].Value == null || row.Cells[kCharType].Value == null)
					continue;

				if ((int)row.Cells[kCodePoint].Value <= invalidCodePoint ||
					(string)row.Cells[kCharType].Value == kUnknown)
				{
					row.Visible = false;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a grid row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int LoadRowWithCharInfo(DataGridViewRow row, IPACharInfo charInfo)
		{
			int rowIndex;
			
			if (row != null)
				rowIndex = row.Index;
			else
			{
				m_grid.Rows.Add();
				rowIndex = m_grid.RowCount - 1;
				row = m_grid.Rows[rowIndex];
			}

			row.Cells[kHexIPAChar].Value = charInfo.Codepoint.ToString("X4");
			row.Cells[kCodePoint].Value = charInfo.Codepoint;
			row.Cells[kIpaChar].Value = (charInfo.DisplayWDottedCircle ?
				DataUtils.kDottedCircle : string.Empty) + charInfo.IPAChar;

			// Identity
			row.Cells[kName].Value = charInfo.Name;
			row.Cells[kDescription].Value = charInfo.Description;

			// Type
			row.Cells[kCharType].Value = charInfo.CharType.ToString();
			row.Cells[kCharSubType].Value = charInfo.CharSubType.ToString();
			row.Cells[kIgnoreType].Value = charInfo.IgnoreType.ToString();

			// Base Character
			row.Cells[kIsBaseChar].Value = charInfo.IsBaseChar;
			row.Cells[kCanPreceedBaseChar].Value = charInfo.CanPreceedBaseChar;
			row.Cells[kDisplayWDottedCircle].Value = charInfo.DisplayWDottedCircle;
			row.Cells[kDisplayOrder].Value = charInfo.DisplayOrder;

			// Articulation
			row.Cells[kMOA].Value = charInfo.MOArticulation;
			row.Cells[kPOA].Value = charInfo.POArticulation;

			// Chart Position
			row.Cells[kChartColumn].Value = charInfo.ChartColumn;
			row.Cells[kChartGroup].Value = charInfo.ChartGroup;

			// Features
			ulong[] features = new ulong[] { charInfo.Mask0, charInfo.Mask1 };
			row.Cells[kAFeatures].Value = DataUtils.AFeatureCache.GetFeaturesText(features);
			row.Cells[kBFeatures].Value = DataUtils.BFeatureCache.GetFeaturesText(charInfo.BinaryMask);
			row.Cells[kMask0].Value = charInfo.Mask0;
			row.Cells[kMask1].Value = charInfo.Mask1;
			row.Cells[kBinaryMask].Value = charInfo.BinaryMask;

			row.Tag = charInfo;

			return rowIndex;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OpenFile(string filePath)
		{
			if (m_amTesting)
			{
				// TODO: FOR TESTING ONLY!
				//filePath = "C:\\Speech Tools\\src\\pa\\IPACharCacheEditor\\Test2.xml";
				filePath = "C:\\Speech Tools\\Output\\Debug\\aTest DefaultIPACharCache.xml";
				//filePath = "C:\\Speech Tools\\Output\\Debug\\DefaultIPACharCache.xml";
				//filePath = "C:\\Speech Tools\\distfiles\\pa\\DefaultIPACharCache.xml";
				//filePath = "C:\\Speech Tools\\Output\\Debug\\aTest_Output.xml";
			}

			m_xmlFilePath = (string.IsNullOrEmpty(filePath) ? OpenDefaultIPACharCache() : filePath);

			// The user clicked 'Cancel' so exit
			if (m_xmlFilePath == "NONE")
				return;

			// Make sure the file exists
			if (!File.Exists(m_xmlFilePath))
			{
				string path = STUtils.PrepFilePathForSTMsgBox(m_xmlFilePath);
				string msg = string.Format(Resources.kstidIpaGridErrNoFile, path);
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			m_charInventory = STUtils.DeserializeData(m_xmlFilePath,
				 typeof(List<IPACharInfo>)) as List<IPACharInfo>;

			// Make sure the format is correct
			if (m_charInventory == null)
			{
				STUtils.STMsgBox(Resources.kstidIpaGridErrBadXmlFormat,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (m_amTesting)
			{
				// TODO: FOR TESTING ONLY!
				//m_xmlFilePath = "C:\\Speech Tools\\src\\pa\\IPACharCacheEditor\\Test2_Output2.xml";
				//m_xmlFilePath = "C:\\Speech Tools\\Output\\Debug\\DefaultIPACharCache.xml";
				//m_xmlFilePath = "C:\\Speech Tools\\distfiles\\pa\\DefaultIPACharCache.xml";
				m_xmlFilePath = "C:\\Speech Tools\\Output\\Debug\\aTest_Output.xml";
			}

			BuildGrid();
			RenumberArticulators();

			if (m_sddpAFeatures != null)
				m_sddpAFeatures.Dispose();

			if (m_sddpBFeatures != null)
				m_sddpBFeatures.Dispose();

			InitializeFeatureDropDowns();

			// Allow users to addingChar records to the opened file
			btnAdd.Enabled = true;
			btnModify.Enabled = true;
			btnDelete.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create the grid and resize the rows & columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			if (m_grid != null)
				m_grid.Dispose();

			m_grid = new SilGrid();
			m_grid.Name = Name + "Grid";
			m_grid.Dock = DockStyle.Fill;
			m_grid.RowHeadersVisible = true;
			m_grid.AllowUserToAddRows = false;
			m_grid.AllowUserToOrderColumns = true;

			AddColumns();
			LoadRows();

			pnlGrid.Controls.Add(m_grid);
			m_grid.BringToFront();

			// Resize rows & columns.
			m_grid.AutoResizeColumns();
			m_grid.AutoResizeRows();
			m_grid.ColumnHeadersHeight *= 2; // Make room for 2 line headers
			m_grid.Columns[kAFeatures].Width = kDefaultFeatureColWidth;
			m_grid.Columns[kBFeatures].Width = kDefaultFeatureColWidth;
			m_grid.CurrentCellDirtyStateChanged += m_grid_CurrentCellDirtyStateChanged;
			m_grid.MouseDoubleClick += m_grid_MouseDoubleClick;
			m_grid.ColumnHeaderMouseClick += m_grid_ColumnHeaderMouseClick;

			m_lastSortedCol = m_grid.Columns[kHexIPAChar];
			m_lastSortDirection = ListSortDirection.Ascending;
			m_grid.Sort(m_lastSortedCol, m_lastSortDirection);
			mnuColSort.Checked = true;

			m_grid.FirstDisplayedCell = m_grid.CurrentCell = m_grid[0, 0];
			s_settingsHndlr.LoadGridProperties(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the new sort column and direction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			m_lastSortedCol = m_grid.SortedColumn;
			m_lastSortDirection = (m_grid.SortOrder == SortOrder.Descending ?
				ListSortDirection.Descending : ListSortDirection.Ascending);

			if (!mnuColSort.Checked)
			{
				mnuColSort.Checked = true;
				mnuMOA.Checked = false;
				mnuPOA.Checked = false;
			}
		}
		
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Ensure user entered a correct Hex number.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private bool validHexNumber(string hexIpaChar, DataGridViewRow row)
		//{
		//    // Don't chk the hex nbr's of rows not visible
		//    if (!row.Visible)
		//        return true;

		//    try
		//    {
		//        int.Parse(hexIpaChar, NumberStyles.AllowHexSpecifier);
		//    }
		//    catch
		//    {
		//        row.Selected = true;
		//        // Scroll error row to the top
		//        m_grid.FirstDisplayedScrollingRowIndex = row.Index - 1;

		//        warningMessage(Properties.Resources.kstidInvalidUnicodeValueMsg,
		//            Properties.Resources.kstidIpaGridErrInvalidHex);
		//        return false;
		//    }
		//    return true;
		//}

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked "Open" in the File menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFile(string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked "Save" in the File menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m_grid == null)
			{
				warningMessage(
					Resources.kstidIpaGridErrTitleNoFileOpen, Resources.kstidIpaGridErrNoFileOpen);
				return;
			}

			if (m_dirty && Verify())
				SaveChanges();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked "Save As" in the File menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m_grid == null)
			{
				warningMessage(
					Resources.kstidIpaGridErrTitleNoFileOpen, Resources.kstidIpaGridErrNoFileOpen);
				return;
			}

			// Only save after verifying the data
			if (Verify())
			{
				string filter = Resources.kstidIpaGridFiletypeXML;
				int filterIndex = 0;
				m_xmlFilePath = PaApp.SaveFileDialog("xml", filter, ref filterIndex,
					Resources.kstidIpaGridSaveAsCaption, string.Empty);

				if (m_xmlFilePath != string.Empty)
					SaveChanges();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked "Exit" in the File menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_cancelButtonPressed = true;
			Close();
		}

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
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			ShowHelpTopic(@"Phonetic_Character_Inventory_Editor/Phonetic_Characters_Inventory_Editor_overview.htm");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ShowHelpTopic(string topicPath)
		{
			string helpFilePath = Path.GetDirectoryName(Application.ExecutablePath);
			helpFilePath = Path.Combine(helpFilePath, "Helps");
			helpFilePath = Path.Combine(helpFilePath, "Phonology_Assistant_Help.chm");

			if (File.Exists(helpFilePath))
				Help.ShowHelp(new Label(), helpFilePath, topicPath);
			else
			{
				string filePath = STUtils.PrepFilePathForSTMsgBox(helpFilePath);
				string msg = string.Format(Resources.kstidHelpFileMissingMsg, filePath);
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open AddCharacterDlg on the row double clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && m_grid.CurrentRow != null)
				btnModify_Click(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked the ADD button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (AddCharacterDlg dlg = new AddCharacterDlg(this, true))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK && dlg.CharInfo != null)
				{
					int newRowIndex = LoadRowWithCharInfo(null, dlg.CharInfo);
					m_grid.CurrentCell = (m_grid.Rows[newRowIndex]).Cells[kHexIPAChar];
					m_dirty = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked the MODIFY button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnModify_Click(object sender, EventArgs e)
		{
			using (AddCharacterDlg dlg = new AddCharacterDlg(this, false))
			{
				if (dlg.ShowDialog(this) != DialogResult.OK || dlg.CharInfo == null)
					return;

				LoadRowWithCharInfo(m_grid.CurrentRow, dlg.CharInfo);
				m_dirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked the DELETE button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (STUtils.STMsgBox(Resources.kstidIpaGridDeletion,
				MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				m_grid.Rows.Remove(m_grid.CurrentRow);
				m_dirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked the drop down Sort menu option.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuSort_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			ToolStripMenuItem item = e.ClickedItem as ToolStripMenuItem;
			if (item == null)
				return;

			mnuColSort.Checked = false;
			mnuPOA.Checked = false;
			mnuMOA.Checked = false;

			if (item == mnuColSort)
				m_grid.Sort(m_lastSortedCol, m_lastSortDirection);
			else if (item == mnuMOA)
				m_grid.Sort(m_grid.Columns[kMOA], ListSortDirection.Ascending);
			else if (item == mnuPOA)
				m_grid.Sort(m_grid.Columns[kPOA], ListSortDirection.Ascending);

			item.Checked = true;
		}

		#endregion

		#region Accessors
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets CharCacheGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilGrid CharCacheGrid
		{
			get { return m_grid; }
			set { m_grid = value; }
		}
		#endregion
	}
}