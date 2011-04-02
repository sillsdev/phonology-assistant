using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Palaso.IO;
using SIL.Pa.Model;
using SIL.Pa.UI.Controls;
using SIL.Pa.UI.Dialogs;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa
{
	public partial class PCIEditor : OKCancelDlgBase
	{
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
		private const string kCanPrecedeBase = "CanPrecedeBaseChar";
		private const string kDisplayWithDottedCircle = "DisplayWithDottedCircle";
		private const string kMOA = "MOA";
		private const string kPOA = "POA";
		private const string kChartColumn = "ChartColumn";
		private const string kChartGroup = "ChartGroup";
		private const string kAFeatures = "afeatures";
		private const string kBFeatures = "bfeatures";
		private const string kUnknown = "Unknown";
		// These values of these fields should be left at zero
		private const string kDisplayOrder = "DisplayOrder";
		private const string kAMask = "AMask";
		private const string kBMask = "BMask";

		private const int kDefaultFeatureColWidth = 225;

		private static SettingsHandler s_settingsHndlr;
		private List<IPASymbol> m_charInventory;
		private readonly bool m_amTesting;
		private readonly List<string> unknownCharTypes;
		private readonly List<string> knownCharTypes;
		private string m_xmlFilePath = string.Empty;
		private readonly List<int> m_codePoints = new List<int>();
		private const int kInvalidCodePoint = 31;
		private readonly SortedDictionary<int, DataGridViewRow> m_gridDictionary =
			new SortedDictionary<int, DataGridViewRow>();

		// The SortedList Key is the moa or poa and the Value is the hexIpaChar
		private readonly SortedList<float, int> m_MOA = new SortedList<float, int>();
		private readonly SortedList<float, int> m_POA = new SortedList<float, int>();

		private DataGridViewColumn m_lastSortedCol;
		private ListSortDirection m_lastSortDirection = ListSortDirection.Ascending;

		internal SizableDropDownPanel m_sddpAFeatures;
		internal CustomDropDown m_aFeatureDropdown;
		internal FeatureListView m_lvAFeatures;
		internal SizableDropDownPanel m_sddpBFeatures;
		internal CustomDropDown m_bFeatureDropdown;
		internal FeatureListView m_lvBFeatures;
		internal int m_startupChar;

		private static SmallFadingWnd s_loadingWnd;

		/// ------------------------------------------------------------------------------------
		public SilGrid Grid { get; set; }

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

			string exePath = FileLocator.GetDirectoryDistributedWithApplication(App.ConfigFolderName);

			// This is the poor man's way of determining whether or not the user has
			// write access to the folder in which the phonetic inventory
			// is stored. I'm sure there is a great class in .Net to use for such a
			// thing, but I couldn't find it in the little bit of digging I did. Sigh!
			string tmpFile = Path.Combine(exePath, "!~tmpaccesstest~!");
			try
			{
				File.WriteAllText(tmpFile, string.Empty);
			}
			catch
			{
				string msg = string.Format(Properties.Resources.kstidWriteAccessErrorMsg, exePath);
				Utils.MsgBox(msg);
				return;
			}

			File.Delete(tmpFile);

			string inventoryPath = null;

			try
			{
				// Make sure the phonetic inventory file exists.
				inventoryPath = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
					InventoryHelper.kDefaultInventoryFileName);
			}
			catch
			{
				string filePath = Utils.PrepFilePathForMsgBox(inventoryPath);
				string msg = string.Format(Properties.Resources.kstidInventoryFileMissing, filePath);
				Utils.MsgBox(msg);
				return;
			}

			int startupChar = 0;
			if (rgArgs != null && rgArgs.Length > 0)
			{
				foreach (string arg in rgArgs)
				{
					if (arg.ToLower().StartsWith("/edit:"))
					{
						string unicodeVal = arg.Substring(6).ToLower();
						unicodeVal = unicodeVal.Replace("u+", string.Empty);
						unicodeVal = unicodeVal.Replace("0x", string.Empty);
						int.TryParse(unicodeVal,
							System.Globalization.NumberStyles.HexNumber, null, out startupChar);
						
						break;
					}
				}
			}

			s_settingsHndlr = new PaSettingsHandler(Path.Combine(exePath, "pcieditor.xml"));
			var editor = new PCIEditor(startupChar);

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
		public PCIEditor(int startupChar) : this()
		{
			if (startupChar > 32)
				m_startupChar = startupChar;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// PCIEditor Constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PCIEditor()
		{
			InitializeComponent();

			btnAdd.Margin = new Padding(0, btnOK.Margin.Top, btnOK.Margin.Left, btnOK.Margin.Bottom);
			btnModify.Margin = btnAdd.Margin;
			btnDelete.Margin = btnAdd.Margin;

			tblLayoutButtons.ColumnCount += 3;
			tblLayoutButtons.ColumnStyles.Insert(0, new ColumnStyle());
			tblLayoutButtons.ColumnStyles.Insert(0, new ColumnStyle());
			tblLayoutButtons.ColumnStyles.Insert(0, new ColumnStyle());

			tblLayoutButtons.Controls.Add(btnAdd, 0, 0);
			tblLayoutButtons.Controls.Add(btnModify, 1, 0);
			tblLayoutButtons.Controls.Add(btnDelete, 2, 0);
			ReAddButtons(4);

			InventoryHelper.Load();

			Version ver = new Version(Application.ProductVersion);
			string version = string.Format(Properties.Resources.kstidVersionFormat, ver.ToString(3));
			ToolStripLabel tslbl = new ToolStripLabel(version);
			tslbl.Alignment = ToolStripItemAlignment.Right;
			Padding pdg = tslbl.Margin;
			tslbl.Margin = new Padding(pdg.Left, pdg.Top, 5, pdg.Bottom);
			tslbl.Font = SystemInformation.MenuFont;
			mnuMain.Items.Add(tslbl);

			string eticFntName = SettingsHandler.GetStringSettingsValue("phoneticfont", "name", "Doulos SIL");
			float eticFntSz = SettingsHandler.GetFloatSettingsValue("phoneticfont", "size", 13f);
			SettingsHandler.SaveSettingsValue("phoneticfont", "name", eticFntName);
			SettingsHandler.SaveSettingsValue("phoneticfont", "size", eticFntSz);
			App.PhoneticFont = new Font(eticFntName, eticFntSz, GraphicsUnit.Point);

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

			m_amTesting = false;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFeatureDropDowns()
		{
			// Build the articulatory features drop-down.
			m_sddpAFeatures = new SizableDropDownPanel();
			m_sddpAFeatures.MinimumSize = new Size(200, 100);
			m_sddpAFeatures.BorderStyle = BorderStyle.FixedSingle;
			m_sddpAFeatures.Padding = new Padding(7, 0, 7, m_sddpAFeatures.Padding.Bottom);

			var lbl = new Label();
			lbl.AutoSize = false;
			lbl.Text = Properties.Resources.kstidAfeaturesHdg;
			lbl.Font = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			lbl.Dock = DockStyle.Top;
			lbl.Height = lbl.PreferredHeight + 10;
			lbl.TextAlign = ContentAlignment.MiddleLeft;
			m_sddpAFeatures.Controls.Add(lbl);

			m_aFeatureDropdown = new CustomDropDown();
			m_aFeatureDropdown.AutoCloseWhenMouseLeaves = false;
			m_aFeatureDropdown.AddControl(m_sddpAFeatures);
			m_aFeatureDropdown.Closing += m_featureDropdown_Closing;

			m_lvAFeatures = new FeatureListView(App.FeatureType.Articulatory, m_aFeatureDropdown);
			m_lvAFeatures.Dock = DockStyle.Fill;
			m_lvAFeatures.Load();
			m_sddpAFeatures.Controls.Add(m_lvAFeatures);
			m_lvAFeatures.BringToFront();

			// Build the binary features drop-down.
			m_sddpBFeatures = new SizableDropDownPanel();
			m_sddpBFeatures.MinimumSize = new Size(200, 100);
			m_sddpBFeatures.BorderStyle = BorderStyle.FixedSingle;
			m_sddpBFeatures.Padding = new Padding(7, 0, 7, m_sddpBFeatures.Padding.Bottom);

			lbl = new Label();
			lbl.AutoSize = false;
			lbl.Text = Properties.Resources.kstidBFeaturesHdg;
			lbl.Font = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			lbl.Dock = DockStyle.Top;
			lbl.Height = lbl.PreferredHeight + 10;
			lbl.TextAlign = ContentAlignment.MiddleLeft;
			m_sddpBFeatures.Controls.Add(lbl);

			m_bFeatureDropdown = new CustomDropDown();
			m_bFeatureDropdown.AutoCloseWhenMouseLeaves = false;
			m_bFeatureDropdown.AddControl(m_sddpBFeatures);
			m_bFeatureDropdown.Closing += m_featureDropdown_Closing;

			m_lvBFeatures = new FeatureListView(App.FeatureType.Binary, m_bFeatureDropdown);
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
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (s_loadingWnd != null)
			{
				s_loadingWnd.CloseFade();
				s_loadingWnd.Dispose();
				s_loadingWnd = null;
			}

			if (m_startupChar < 32)
				return;

			foreach (var row in Grid.GetRows().Where(r => (int) r.Cells[kDecimal].Value == m_startupChar))
			{
				Grid.CurrentCell = row.Cells[kHexadecimal];
				Grid.FirstDisplayedCell = Grid.CurrentCell;
				btnModify_Click(null, null);
				m_startupChar = 0;
				return;
			}

			AddChar(m_startupChar);
			m_startupChar = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update a phones feature mask(s) after one of the feature drop-down lists closes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void m_featureDropdown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			// If this form doesn't have focus, it probably means the drop-down was used
			// and closed on the AddCharacterDlg.
			if (!Focused)
				Grid.Focus();

			FeatureListView lv;
			if (sender == m_aFeatureDropdown)
				lv = m_lvAFeatures;
			else if (sender == m_bFeatureDropdown)
				lv = m_lvBFeatures;
			else
				return;

			if (Grid.CurrentRow == null)
				return;

			var charInfo = Grid.CurrentRow.Tag as IPASymbol;
			if (charInfo == null)
				return;

			bool changed;

			if (sender == m_aFeatureDropdown)
			{
				changed = (charInfo.AMask != lv.CurrentMask);
				charInfo.AMask = lv.CurrentMask;
				Grid.CurrentRow.Cells[kAMask].Value = lv.CurrentMask;
				Grid.CurrentRow.Cells[kAFeatures].Value =
					App.AFeatureCache.GetFeaturesText(lv.CurrentMask);
			}
			else
			{
				changed = (charInfo.BMask != lv.CurrentMask);
				charInfo.BMask = lv.CurrentMask;
				Grid.CurrentRow.Cells[kBMask].Value = lv.CurrentMask;
				Grid.CurrentRow.Cells[kBFeatures].Value =
					App.BFeatureCache.GetFeaturesText(charInfo.BMask);
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
			s_settingsHndlr.SaveGridProperties(Grid);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveInventory()
		{
			var tmpCache = new List<IPASymbol>();

			foreach (var row in m_gridDictionary.Values)
			{
				IPASymbol info = new IPASymbol();
				info.Decimal = (int)row.Cells[kDecimal].Value;

				info.Literal = (info.Decimal < 0 ? row.Cells[kLiteral].Value as string :
					((char)info.Decimal).ToString());
				
				info.Hexadecimal = row.Cells[kHexadecimal].Value as string;
				info.Name = (string)row.Cells[kName].Value;
				info.Description = (string)row.Cells[kDescription].Value;
				info.Type = (IPASymbolType)Enum.Parse(
					typeof(IPASymbolType), (string)row.Cells[kType].Value);
				info.SubType = (IPASymbolSubType)Enum.Parse(
					typeof(IPASymbolSubType), (string)row.Cells[kSubType].Value);
				info.IgnoreType = (IPASymbolIgnoreType)Enum.Parse(
					typeof(IPASymbolIgnoreType), (string)row.Cells[kIgnoreType].Value);
				info.IsBase = (bool)row.Cells[kIsBase].Value;
				info.CanPrecedeBase = (bool)row.Cells[kCanPrecedeBase].Value;
				info.DisplayWithDottedCircle = (bool)row.Cells[kDisplayWithDottedCircle].Value;
				info.MOArticulation = (int)row.Cells[kMOA].Value;
				info.POArticulation = (int)row.Cells[kPOA].Value;
				info.AMask = (FeatureMask)row.Cells[kAMask].Value;
				info.BMask = (FeatureMask)row.Cells[kBMask].Value;
				info.ChartColumn = (int)row.Cells[kChartColumn].Value;
				info.ChartGroup = (int)row.Cells[kChartGroup].Value;

				tmpCache.Add(info);
			}

			App.IPASymbolCache.LoadFromList(tmpCache);
			//InventoryHelper.Save();
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

			foreach (var row in Grid.GetRows())
			{
				// Don't verify the 'new' record at the very bottom
				if (row.Cells[kHexadecimal].Value == null || row.Cells[kDecimal].Value == null)
					continue;

				int codePoint = (int)row.Cells[kDecimal].Value;
				m_codePoints.Add(codePoint);

				string charTypeVal = row.Cells[kType].Value.ToString();
				string charSubTypeVal = row.Cells[kSubType].Value.ToString();

				// Don't allow CharType's of 'Unknown'
				if (row.Visible && row.Cells[kType].Value.ToString() == kUnknown)
					return ShowErrorMessage(Properties.Resources.kstidIpaGridErrUnknownCharType, row);

				if (unknownCharTypes.Contains(charTypeVal))
				{
					if (charSubTypeVal != kUnknown)
					{
						return ShowErrorMessage(string.Format(
							Properties.Resources.kstidIpaGridErrNeedUnknown, charTypeVal), row);
					}
				}

				if (knownCharTypes.Contains(charTypeVal))
				{
					if (charSubTypeVal == kUnknown)
						return ShowErrorMessage(
							string.Format(Properties.Resources.kstidIpaGridErrRemoveUnknown, charTypeVal), row);
				}

				bool isBaseChar = (bool)row.Cells[kIsBase].Value;
				bool canProceedBase = (bool)row.Cells[kCanPrecedeBase].Value;

				if (isBaseChar && canProceedBase)
					return ShowErrorMessage(Properties.Resources.kstidIpaGridErrBothTrue, row);
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
			Grid.FirstDisplayedScrollingRowIndex = row.Index; // Scroll error row to the top
			warningMessage(Properties.Resources.kstidIpaGridErrTitleDataConflict, errMsg);
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

			foreach (var row in Grid.GetRows())
			{
				// Don't verify the 'new' record at the very bottom
				if (row.Cells[kHexadecimal].Value == null || row.Cells[kDecimal].Value == null)
					continue;

				int codepoint = (int)row.Cells[kDecimal].Value;
				m_gridDictionary[codepoint] = row;

				if (codepoint <= kInvalidCodePoint || row.Cells[kType].Value.ToString() == kUnknown)
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
			IDictionary<float, int> articulators)
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
				var dlg = new OpenFileDialog();
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Title = Properties.Resources.kstidIpaGridOpenFileTitle;
				dlg.Filter = Properties.Resources.kstidIpaGridOpenFileFilter;
				// Set the initial directory to the startup path
				dlg.InitialDirectory = App.ConfigFolderName;
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
			// Add the HexIpa field column.
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(kHexadecimal);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridHexIpa);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ReadOnly = true;
			col.Frozen = true;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.BackColor = ColorHelper.CalculateColor(Color.Black, SystemColors.Window, 15);
			Grid.Columns.Add(col);

			// Add the IpaChar field column.
			col = SilGrid.CreateTextBoxColumn(kLiteral);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridIpaChar);
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = App.PhoneticFont;
			col.CellTemplate.Style.Font = App.PhoneticFont;
			col.DefaultCellStyle.BackColor = ColorHelper.CalculateColor(Color.Black, SystemColors.Window, 15);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ReadOnly = true;
			col.Frozen = true;
			Grid.Columns.Add(col);

			// Add the CodePoint field column (NOT a visible column).
			col = SilGrid.CreateTextBoxColumn(kDecimal);
			col.Visible = false;
			Grid.Columns.Add(col);

			// Add the Name field column.
			col = SilGrid.CreateTextBoxColumn(kName);
			col.HeaderText = Properties.Resources.kstidIpaGridName;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			Grid.Columns.Add(col);

			// Add the Description field column.
			col = SilGrid.CreateTextBoxColumn(kDescription);
			col.HeaderText = Properties.Resources.kstidIpaGridDesc;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			Grid.Columns.Add(col);

			// Add the CharType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kType, Enum.GetNames(typeof(IPASymbolType)).Cast<object>());
			col.HeaderText = Properties.Resources.kstidIpaGridCharType;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			Grid.Columns.Add(col);

			// Add the CharSubType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kSubType, Enum.GetNames(typeof(IPASymbolSubType)).Cast<object>());
			col.HeaderText = Properties.Resources.kstidIpaGridCharSubType;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			Grid.Columns.Add(col);

			// Add the CharIgnoreType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kIgnoreType, Enum.GetNames(typeof(IPASymbolIgnoreType)).Cast<object>());
			col.HeaderText = Properties.Resources.kstidIpaGridCharIgnoreType;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			Grid.Columns.Add(col);

			// Add the IsBaseChar check box column.
			col = SilGrid.CreateCheckBoxColumn(kIsBase);
			col.HeaderText = Properties.Resources.kstidIpaGridIsBase;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(bool);
			Grid.Columns.Add(col);

			// Add the Can preceed base character check box column.
			col = SilGrid.CreateCheckBoxColumn(kCanPrecedeBase);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridCanPreceedBase);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(bool);
			Grid.Columns.Add(col);

			// Add the DisplayWDottedCircle check box column.
			col = SilGrid.CreateCheckBoxColumn(kDisplayWithDottedCircle);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridWDotCircle);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(bool);
			Grid.Columns.Add(col);

			// Add the Display Order column.
			col = SilGrid.CreateTextBoxColumn(kDisplayOrder);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridDisplayOrder);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.Visible = false;	// Currently, not used.
			Grid.Columns.Add(col);

			// Add the MOA column.
			col = SilGrid.CreateTextBoxColumn(kMOA);
			col.HeaderText = Properties.Resources.kstidIpaGridMOA;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(int);
			if (!m_amTesting)
				col.Visible = false;
			Grid.Columns.Add(col);

			// Add the POA column.
			col = SilGrid.CreateTextBoxColumn(kPOA);
			col.HeaderText = Properties.Resources.kstidIpaGridPOA;
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(int);
			if (!m_amTesting)
				col.Visible = false;
			Grid.Columns.Add(col);

			// Add the articulatory Mask field (NOT a visible column).
			col = SilGrid.CreateTextBoxColumn(kAMask);
			col.Visible = false;
			Grid.Columns.Add(col);

			// Add the binary Mask field (NOT a visible column).
			col = SilGrid.CreateTextBoxColumn(kBMask);
			col.Visible = false;
			Grid.Columns.Add(col);

			// Add the ChartColumn column.
			col = SilGrid.CreateTextBoxColumn(kChartColumn);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridChartColumn);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(int);
			if (!m_amTesting)
				col.Visible = false;
			Grid.Columns.Add(col);

			// Add the ChartGroup column.
			col = SilGrid.CreateTextBoxColumn(kChartGroup);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridChartGroup);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			col.ValueType = typeof(int);
			if (!m_amTesting)
				col.Visible = false;
			Grid.Columns.Add(col);

			col = SilGrid.CreateSilButtonColumn(kAFeatures);
			col.ReadOnly = true;
			col.HeaderText = Properties.Resources.kstidAfeaturesHdg;
			col.Width = kDefaultFeatureColWidth;
			((SilButtonColumn)col).ButtonStyle = SilButtonColumn.ButtonType.PlainCombo;
			((SilButtonColumn)col).ButtonClicked += HandleFeatureDropDownClick;
			Grid.Columns.Add(col);

			col = SilGrid.CreateSilButtonColumn(kBFeatures);
			col.ReadOnly = true;
			col.HeaderText = Properties.Resources.kstidBFeaturesHdg;
			col.Width = kDefaultFeatureColWidth;
			((SilButtonColumn)col).ButtonStyle = SilButtonColumn.ButtonType.PlainCombo;
			((SilButtonColumn)col).ButtonClicked += HandleFeatureDropDownClick;
			Grid.Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		void HandleFeatureDropDownClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (Grid.CurrentRow == null)
				return;

			var charInfo = Grid.CurrentRow.Tag as IPASymbol;
			if (charInfo == null)
				return;

			// Figure out what drop-down and list view to use based on the column index.
			FeatureListView lv;
			CustomDropDown cdd;
			if (Grid.Columns[e.ColumnIndex].Name == kAFeatures)
			{
				cdd = m_aFeatureDropdown;
				lv = m_lvAFeatures;
				lv.CurrentMask = charInfo.AMask.Clone();
			}
			else if (Grid.Columns[e.ColumnIndex].Name == kBFeatures)
			{
				cdd = m_bFeatureDropdown;
				lv = m_lvBFeatures;
				lv.CurrentMask = charInfo.BMask.Clone();
			}
			else
				return;

			var rc = Grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			
			// Use this line to align the right edge of the drop-down to
			// the right edge of the cell.
			// Point pt = new Point(rc.Right - cdd.Width, rc.Bottom);

			// Use this line to align the left edge of the drop-down to
			// the left edge of the cell.
			var pt = new Point(rc.X, rc.Bottom);
			
			pt = Grid.PointToScreen(pt);
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
			foreach (var charInfo in m_charInventory)
				LoadRowWithCharInfo(null, charInfo);

			// Hide all rows with a CharType of "Unknown" OR a CodePoint that is below 32
			foreach (var row in Grid.GetRows())
			{
				if (row.Cells[kDecimal].Value == null || row.Cells[kType].Value == null)
					continue;

				if ((int)row.Cells[kDecimal].Value <= kInvalidCodePoint ||
					(string)row.Cells[kType].Value == kUnknown)
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
		private int LoadRowWithCharInfo(DataGridViewRow row, IPASymbol charInfo)
		{
			int rowIndex;
			
			if (row != null)
				rowIndex = row.Index;
			else
			{
				Grid.Rows.Add();
				rowIndex = Grid.RowCount - 1;
				row = Grid.Rows[rowIndex];
			}

			row.Cells[kHexadecimal].Value = charInfo.Decimal.ToString("X4");
			row.Cells[kDecimal].Value = charInfo.Decimal;
			row.Cells[kLiteral].Value = (charInfo.DisplayWithDottedCircle ?
				App.kDottedCircle : string.Empty) + charInfo.Literal;

			// Identity
			row.Cells[kName].Value = charInfo.Name;
			row.Cells[kDescription].Value = charInfo.Description;

			// Type
			row.Cells[kType].Value = charInfo.Type.ToString();
			row.Cells[kSubType].Value = charInfo.SubType.ToString();
			row.Cells[kIgnoreType].Value = charInfo.IgnoreType.ToString();

			// Base Character
			row.Cells[kIsBase].Value = charInfo.IsBase;
			row.Cells[kCanPrecedeBase].Value = charInfo.CanPrecedeBase;
			row.Cells[kDisplayWithDottedCircle].Value = charInfo.DisplayWithDottedCircle;

			// Articulation
			row.Cells[kMOA].Value = charInfo.MOArticulation;
			row.Cells[kPOA].Value = charInfo.POArticulation;

			// Chart Position
			row.Cells[kChartColumn].Value = charInfo.ChartColumn;
			row.Cells[kChartGroup].Value = charInfo.ChartGroup;

			// Features
			row.Cells[kAFeatures].Value = App.AFeatureCache.GetFeaturesText(charInfo.AMask);
			row.Cells[kBFeatures].Value = App.BFeatureCache.GetFeaturesText(charInfo.BMask);
			row.Cells[kAMask].Value = charInfo.AMask;
			row.Cells[kBMask].Value = charInfo.BMask;

			row.Tag = charInfo;

			return rowIndex;
		}

		public class test
		{
			/// ------------------------------------------------------------------------------------
			/// <summary>
			/// Use only for serialization/deserialization
			/// </summary>
			/// ------------------------------------------------------------------------------------
			[XmlArray("symbols"), XmlArrayItem("symbol")]
			public List<IPASymbol> IPASymbols { get; set; }
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
				string path = Utils.PrepFilePathForMsgBox(m_xmlFilePath);
				string msg = string.Format(Properties.Resources.kstidIpaGridErrNoFile, path);
				Utils.MsgBox(msg);
				return;
			}

			InventoryHelper.Load();
			m_charInventory = InventoryHelper.IPASymbolCache.Values.ToList();

			//var xml = XmlSerializationHelper.SerializeToString(m_charInventory, true);
			//var t = XElement.Parse(xml);
			//t.Name = "symbols";

			//InventoryHelper.Load(m_xmlFilePath);
			//m_charInventory = App.IPASymbolCache.Values.ToList();

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
			if (Grid != null)
				Grid.Dispose();

			Grid = new SilGrid();
			Grid.Name = Name + "Grid";
			Grid.Dock = DockStyle.Fill;
			Grid.RowHeadersVisible = true;
			Grid.AllowUserToAddRows = false;
			Grid.AllowUserToOrderColumns = true;

			AddColumns();
			LoadRows();

			pnlGrid.Controls.Add(Grid);
			Grid.BringToFront();

			// Resize rows & columns.
			Grid.AutoResizeColumns();
			Grid.AutoResizeRows();
			Grid.ColumnHeadersHeight *= 2; // Make room for 2 line headers
			Grid.Columns[kAFeatures].Width = kDefaultFeatureColWidth;
			Grid.Columns[kBFeatures].Width = kDefaultFeatureColWidth;
			Grid.CurrentCellDirtyStateChanged += m_grid_CurrentCellDirtyStateChanged;
			Grid.MouseDoubleClick += m_grid_MouseDoubleClick;
			Grid.ColumnHeaderMouseClick += m_grid_ColumnHeaderMouseClick;

			m_lastSortedCol = Grid.Columns[kHexadecimal];
			m_lastSortDirection = ListSortDirection.Ascending;
			Grid.Sort(m_lastSortedCol, m_lastSortDirection);
			mnuColSort.Checked = true;

			Grid.FirstDisplayedCell = Grid.CurrentCell = Grid[0, 0];
			s_settingsHndlr.LoadGridProperties(Grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the new sort column and direction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			m_lastSortedCol = Grid.SortedColumn;
			m_lastSortDirection = (Grid.SortOrder == SortOrder.Descending ?
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
			if (Grid == null)
			{
				warningMessage(Properties.Resources.kstidIpaGridErrTitleNoFileOpen,
					Properties.Resources.kstidIpaGridErrNoFileOpen);
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
			if (Grid == null)
			{
				warningMessage(Properties.Resources.kstidIpaGridErrTitleNoFileOpen,
					Properties.Resources.kstidIpaGridErrNoFileOpen);
				return;
			}

			// Only save after verifying the data
			if (Verify())
			{
				string filter = Properties.Resources.kstidIpaGridFiletypeXML;
				int filterIndex = 0;
				m_xmlFilePath = App.SaveFileDialog("xml", filter, ref filterIndex,
					Properties.Resources.kstidIpaGridSaveAsCaption, string.Empty);

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
		//    Close();
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			ShowHelpTopic(@"Phonetic_Character_Inventory_Editor/Phonetic_Character_Inventory_Editor_overview.htm");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ShowHelpTopic(string topicPath)
		{
			string helpFilePath = Application.StartupPath;
			helpFilePath = Path.Combine(helpFilePath, "Phonology_Assistant_Help.chm");

			if (File.Exists(helpFilePath))
				Help.ShowHelp(new Label(), helpFilePath, topicPath);
			else
			{
				string filePath = Utils.PrepFilePathForMsgBox(helpFilePath);
				string msg = string.Format(Properties.Resources.kstidHelpFileMissingMsg, filePath);
				Utils.MsgBox(msg);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open AddCharacterDlg on the row double clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && Grid.CurrentRow != null)
			{
				var hti = Grid.HitTest(e.X, e.Y);
				if (hti != null && hti.RowIndex == Grid.CurrentRow.Index)
					btnModify_Click(null, null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked the ADD button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnAdd_Click(object sender, EventArgs e)
		{
			AddChar(0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddChar(int codepoint)
		{
			using (var dlg = (codepoint == 0 ?
				new AddCharacterDlg(this, true) : new AddCharacterDlg(this, codepoint)))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK && dlg.CharInfo != null)
				{
					int newRowIndex = LoadRowWithCharInfo(null, dlg.CharInfo);
					Grid.CurrentCell = (Grid.Rows[newRowIndex]).Cells[kHexadecimal];
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
			using (var dlg = new AddCharacterDlg(this, false))
			{
				if (dlg.ShowDialog(this) != DialogResult.OK || dlg.CharInfo == null)
					return;

				LoadRowWithCharInfo(Grid.CurrentRow, dlg.CharInfo);
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
			if (Utils.MsgBox(Properties.Resources.kstidIpaGridDeletion,
				MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				Grid.Rows.Remove(Grid.CurrentRow);
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
			var item = e.ClickedItem as ToolStripMenuItem;
			if (item == null)
				return;

			mnuColSort.Checked = false;
			mnuPOA.Checked = false;
			mnuMOA.Checked = false;

			if (item == mnuColSort)
				Grid.Sort(m_lastSortedCol, m_lastSortDirection);
			else if (item == mnuMOA)
				Grid.Sort(Grid.Columns[kMOA], ListSortDirection.Ascending);
			else if (item == mnuPOA)
				Grid.Sort(Grid.Columns[kPOA], ListSortDirection.Ascending);

			item.Checked = true;
		}

		#endregion
	}
}