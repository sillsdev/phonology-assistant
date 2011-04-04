using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Palaso.IO;
using Palaso.Reporting;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Dialogs;
using SilTools;

namespace SIL.Pa
{
	public partial class PCIEditor : OKCancelDlgBase
	{
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
		//private const string kMOA = "MOA";
		//private const string kPOA = "POA";
		//private const string kChartColumn = "ChartColumn";
		//private const string kChartGroup = "ChartGroup";
		//private const string kAFeatures = "afeatures";
		//private const string kBFeatures = "bfeatures";
		//private const string kUnknown = "Unknown";
		// These values of these fields should be left at zero
		private const string kAFeatures = "AMask";
		private const string kBFeatures = "BMask";

		private const int kDefaultFeatureColWidth = 225;

		private List<IPASymbol> m_symbols;
		private List<IPASymbol> m_nonEditableSymbols;
		private readonly List<string> unknownCharTypes;
		private readonly List<string> knownCharTypes;
		private string m_xmlFilePath = string.Empty;
		//private readonly SortedDictionary<int, DataGridViewRow> m_gridDictionary =
		//    new SortedDictionary<int, DataGridViewRow>();

		//// The SortedList Key is the moa or poa and the Value is the hexIpaChar
		//private readonly SortedList<float, int> m_MOA = new SortedList<float, int>();
		//private readonly SortedList<float, int> m_POA = new SortedList<float, int>();

		private SortOrder m_sortDirection = SortOrder.Ascending;
		private string m_sortField = kHexadecimal;
		private static SmallFadingWnd s_loadingWnd;

		/// ------------------------------------------------------------------------------------
		public SilGrid Grid { get; private set; }

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

			ErrorReport.AddProperty("EmailAddress", "PaFeedback@sil.org");
			ErrorReport.AddStandardProperties();
			ExceptionHandler.Init();
			
			s_loadingWnd = new SmallFadingWnd(Properties.Resources.kstidLoadingProgramMsg);

			PortableSettingsProvider.SettingsFileFolder = App.ProjectFolder;
			PortableSettingsProvider.SettingsFileName = "PCIEditor.settings";

			string inventoryFilePath =
				FileLocator.GetDirectoryDistributedWithApplication(App.ConfigFolderName);

			// This is the poor man's way of determining whether or not the user has
			// write access to the folder in which the phonetic inventory
			// is stored. I'm sure there is a great class in .Net to use for such a
			// thing, but I couldn't find it in the little bit of digging I did. Sigh!
			string tmpFile = Path.Combine(inventoryFilePath, "!~tmpaccesstest~!");
			try
			{
				File.WriteAllText(tmpFile, string.Empty);
			}
			catch
			{
				string msg = string.Format(Properties.Resources.kstidWriteAccessErrorMsg, inventoryFilePath);
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

			var editor = new PCIEditor();
			editor.OpenFile(inventoryPath);
			Application.Run(editor);
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

			var ver = new Version(Application.ProductVersion);
			string version = string.Format(Properties.Resources.kstidVersionFormat, ver.ToString(3));
			var tslbl = new ToolStripLabel(version);
			tslbl.Alignment = ToolStripItemAlignment.Right;
			var pdg = tslbl.Margin;
			tslbl.Margin = new Padding(pdg.Left, pdg.Top, 5, pdg.Bottom);
			tslbl.Font = SystemInformation.MenuFont;
			mnuMain.Items.Add(tslbl);

			if (Settings.Default.PhoneticFont != null)
				App.PhoneticFont = Settings.Default.PhoneticFont;
			else
				Settings.Default.PhoneticFont = App.PhoneticFont;

			// Load 'knownCharTypes'
			knownCharTypes = new List<string>();
			knownCharTypes.Add("Consonant");
			knownCharTypes.Add("Suprasegmentals");

			// Load 'unknownCharTypes'
			unknownCharTypes = new List<string>();
			unknownCharTypes.Add("Vowel");
			unknownCharTypes.Add("Diacritics");
			unknownCharTypes.Add("Breaking");

			Settings.Default.MainWindow = App.InitializeForm(this, Settings.Default.MainWindow);
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
			
			SortList(m_symbols[0]);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleCancelClick(object sender, EventArgs e)
		{
			base.HandleCancelClick(sender, e);
			Close();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleOKButtonClick(object sender, EventArgs e)
		{
			base.HandleOKButtonClick(sender, e);
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the form's state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Settings.Default.GridSettings = GridSettings.Create(Grid);
			Settings.Default.Save();
			base.OnFormClosing(e);
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
			for (int i = 0; i < m_symbols.Count; i++)
			{
				if (m_symbols[i].IsBase && m_symbols[i].CanPrecedeBase)
					return ShowErrorMessage(Properties.Resources.kstidIpaGridErrBothTrue, i);

				//string charTypeVal = row.Cells[kType].Value.ToString();
				//string charSubTypeVal = row.Cells[kSubType].Value.ToString();

				//// Don't allow CharType's of 'Unknown'
				//if (row.Visible && row.Cells[kType].Value.ToString() == kUnknown)
				//    return ShowErrorMessage(Properties.Resources.kstidIpaGridErrUnknownCharType, row);

				//if (unknownCharTypes.Contains(charTypeVal))
				//{
				//    if (charSubTypeVal != kUnknown)
				//    {
				//        return ShowErrorMessage(string.Format(
				//            Properties.Resources.kstidIpaGridErrNeedUnknown, charTypeVal), row);
				//    }
				//}

				//if (knownCharTypes.Contains(charTypeVal))
				//{
				//    if (charSubTypeVal == kUnknown)
				//        return ShowErrorMessage(
				//            string.Format(Properties.Resources.kstidIpaGridErrRemoveUnknown, charTypeVal), row);
				//}
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
		private bool ShowErrorMessage(string errMsg, int row)
		{
			Grid.Rows[row].Selected = true;
			Grid.FirstDisplayedScrollingRowIndex = row; // Scroll error row to the top
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
			
			m_symbols.AddRange(m_nonEditableSymbols);
			m_symbols = m_symbols.OrderBy(x => x.Decimal).ToList();

			var xml = XmlSerializationHelper.SerializeToString(m_symbols, true);
			var newSymbols = XElement.Parse(xml);
			newSymbols.Name = "symbols";

			var element = XElement.Load(m_xmlFilePath);
			element.Element("symbols").ReplaceWith(newSymbols);
			element.Save(m_xmlFilePath);
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Renumber the articulation (moa & poa) values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RenumberArticulators()
		{
			//int newValue = 0;
			//m_gridDictionary.Clear();
			//m_MOA.Clear();
			//m_POA.Clear();

			//foreach (var row in Grid.GetRows())
			//{
			//    // Don't verify the 'new' record at the very bottom
			//    if (row.Cells[kHexadecimal].Value == null || row.Cells[kDecimal].Value == null)
			//        continue;

			//    int codepoint = (int)row.Cells[kDecimal].Value;
			//    m_gridDictionary[codepoint] = row;

			//    if (codepoint <= kInvalidCodePoint || row.Cells[kType].Value.ToString() == kUnknown)
			//    {
			//        // Make the MOA & POA negative
			//        newValue -= 5;
			//        row.Cells[kMOA].Value = newValue;
			//        row.Cells[kPOA].Value = newValue;
			//    }
			//    else
			//    {
			//        LoadArticulators(float.Parse(row.Cells[kMOA].Value.ToString()), codepoint, m_MOA);
			//        LoadArticulators(float.Parse(row.Cells[kPOA].Value.ToString()), codepoint, m_POA);
			//    }
			//}

			//UpdateArticulators();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the articulation (moa & poa) values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void LoadArticulators(float val, int codepoint,
			IDictionary<float, int> articulators)
		{
			//while (true)
			//{
			//    if (articulators.ContainsKey(val))
			//        val += 0.1F; // Fix any duplicate articulators
			//    else
			//    {
			//        // Key is articulationVal (moa or poa) and Value is "Name + HexIpa"
			//        articulators[val] = codepoint;
			//        return;
			//    }
			//}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update moa & poa in the grid dictionary with new values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateArticulators()
		{
			//int newValue = 0;
			//foreach (int codepoint in m_MOA.Values)
			//{
			//    newValue += 5;
			//    m_gridDictionary[codepoint].Cells[kMOA].Value = newValue;
			//}

			//newValue = 0;
			//foreach (int codepoint in m_POA.Values)
			//{
			//    newValue += 5;
			//    m_gridDictionary[codepoint].Cells[kPOA].Value = newValue;
			//}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OpenFile(string filePath)
		{
			m_xmlFilePath = filePath;

			// Make sure the file exists
			if (!File.Exists(m_xmlFilePath))
			{
				string path = Utils.PrepFilePathForMsgBox(m_xmlFilePath);
				string msg = string.Format(Properties.Resources.kstidIpaGridErrNoFile, path);
				Utils.MsgBox(msg);
				return;
			}

			InventoryHelper.Load();
			m_nonEditableSymbols = InventoryHelper.IPASymbolCache.Values.Where(x => x.Decimal < 0).ToList();
			m_symbols = InventoryHelper.IPASymbolCache.Values.Where(x => x.Decimal > 0).ToList();

			BuildGrid();
			RenumberArticulators();

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
			Grid.VirtualMode = true;
			Grid.CellValueNeeded += HandleCellValueNeeded;
			Grid.CellValuePushed += HandleCellValuePushed;

			AddColumns();

			Grid.RowCount = m_symbols.Count;
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
			Grid.ColumnHeaderMouseClick += HandleGridColumnHeaderMouseClick;

			Grid.FirstDisplayedCell = Grid.CurrentCell = Grid[0, 0];
			
			if (Settings.Default.GridSettings != null)
				Settings.Default.GridSettings.InitializeGrid(Grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			// Add the HexIpa field column.
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(kHexadecimal);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridHexIpa);
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
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
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.ReadOnly = true;
			col.Frozen = true;
			Grid.Columns.Add(col);

			// Add the Name field column.
			col = SilGrid.CreateTextBoxColumn(kName);
			col.HeaderText = Properties.Resources.kstidIpaGridName;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			Grid.Columns.Add(col);

			// Add the Description field column.
			col = SilGrid.CreateTextBoxColumn(kDescription);
			col.HeaderText = Properties.Resources.kstidIpaGridDesc;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			Grid.Columns.Add(col);

			// Add the CharType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kType, Enum.GetNames(typeof(IPASymbolType)).Cast<object>());
			col.HeaderText = Properties.Resources.kstidIpaGridCharType;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			Grid.Columns.Add(col);

			// Add the CharSubType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kSubType, Enum.GetNames(typeof(IPASymbolSubType)).Cast<object>());
			col.HeaderText = Properties.Resources.kstidIpaGridCharSubType;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			Grid.Columns.Add(col);

			// Add the CharIgnoreType drop down list combo box column.
			col = SilGrid.CreateDropDownListComboBoxColumn(
				kIgnoreType, Enum.GetNames(typeof(IPASymbolIgnoreType)).Cast<object>());
			col.HeaderText = Properties.Resources.kstidIpaGridCharIgnoreType;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			Grid.Columns.Add(col);

			// Add the IsBaseChar check box column.
			col = SilGrid.CreateCheckBoxColumn(kIsBase);
			col.HeaderText = Properties.Resources.kstidIpaGridIsBase;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.ValueType = typeof(bool);
			Grid.Columns.Add(col);

			// Add the Can preceed base character check box column.
			col = SilGrid.CreateCheckBoxColumn(kCanPrecedeBase);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridCanPreceedBase);
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.ValueType = typeof(bool);
			Grid.Columns.Add(col);

			// Add the DisplayWDottedCircle check box column.
			col = SilGrid.CreateCheckBoxColumn(kDisplayWithDottedCircle);
			col.HeaderText = Utils.ConvertLiteralNewLines(Properties.Resources.kstidIpaGridWDotCircle);
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.ValueType = typeof(bool);
			Grid.Columns.Add(col);

			//// Add the MOA column.
			//col = SilGrid.CreateTextBoxColumn(kMOA);
			//col.HeaderText = Properties.Resources.kstidIpaGridMOA;
			//col.SortMode = DataGridViewColumnSortMode.Automatic;
			//col.ValueType = typeof(int);
			//if (!m_amTesting)
			//    col.Visible = false;
			//Grid.Columns.Add(col);

			//// Add the POA column.
			//col = SilGrid.CreateTextBoxColumn(kPOA);
			//col.HeaderText = Properties.Resources.kstidIpaGridPOA;
			//col.SortMode = DataGridViewColumnSortMode.Automatic;
			//col.ValueType = typeof(int);
			//if (!m_amTesting)
			//    col.Visible = false;
			//Grid.Columns.Add(col);

			// Add the articulatory Mask field.
			col = SilGrid.CreateTextBoxColumn(kAFeatures);
			col.HeaderText = Properties.Resources.kstidAfeaturesHdg;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.ReadOnly = true;
			Grid.Columns.Add(col);

			// Add the binary Mask field.
			col = SilGrid.CreateTextBoxColumn(kBFeatures);
			col.HeaderText = Properties.Resources.kstidBFeaturesHdg;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.ReadOnly = true;
			Grid.Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		void HandleCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex < 0)
				return;

			var charInfo = m_symbols[e.RowIndex];

			switch (Grid.Columns[e.ColumnIndex].Name)
			{
				case kHexadecimal: e.Value = charInfo.Decimal.ToString("X4"); break;
				case kLiteral: e.Value = (charInfo.DisplayWithDottedCircle ? App.kDottedCircle : string.Empty) + charInfo.Literal; break;
				case kName: e.Value = charInfo.Name; break;
				case kDescription: e.Value = charInfo.Description; break;
				case kType: e.Value = charInfo.Type.ToString(); break;
				case kSubType: e.Value = charInfo.SubType.ToString(); break;
				case kIgnoreType: e.Value = charInfo.IgnoreType.ToString(); break;
				case kIsBase: e.Value = charInfo.IsBase; break;
				case kCanPrecedeBase: e.Value = charInfo.CanPrecedeBase; break;
				case kDisplayWithDottedCircle: e.Value = charInfo.DisplayWithDottedCircle; break;
				case kAFeatures: e.Value = InventoryHelper.AFeatureCache.GetFeaturesText(charInfo.AMask); break;
				case kBFeatures: e.Value = InventoryHelper.BFeatureCache.GetFeaturesText(charInfo.BMask); break;
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex < 0)
				return;

			var charInfo = m_symbols[e.RowIndex];

			switch (Grid.Columns[e.ColumnIndex].Name)
			{
				case kName:  charInfo.Name = e.Value as string; break;
				case kDescription: charInfo.Description = e.Value as string; break;
				case kType: charInfo.Type = (IPASymbolType)Enum.Parse(typeof(IPASymbolType), e.Value as string); break;
				case kSubType: charInfo.SubType = (IPASymbolSubType)Enum.Parse(typeof(IPASymbolSubType), e.Value as string); break;
				case kIgnoreType: charInfo.IgnoreType = (IPASymbolIgnoreType)Enum.Parse(typeof(IPASymbolIgnoreType), e.Value as string); break;
				case kIsBase: charInfo.IsBase = (bool)e.Value; break;
				case kCanPrecedeBase: charInfo.CanPrecedeBase = (bool)e.Value; break;
				case kDisplayWithDottedCircle: charInfo.DisplayWithDottedCircle = (bool)e.Value; break;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the new sort column and direction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleGridColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex < 0)
				return;

			if (m_sortField != Grid.GetColumnName(e.ColumnIndex))
			{
				m_sortField = Grid.GetColumnName(e.ColumnIndex);
				m_sortDirection = SortOrder.Ascending;
			}
			else
			{
				m_sortDirection = (m_sortDirection == SortOrder.Ascending ?
					SortOrder.Descending : SortOrder.Ascending);
			}

			SortList();
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

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			ShowHelpTopic(@"Phonetic_Character_Inventory_Editor/Phonetic_Character_Inventory_Editor_overview.htm");
		}

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
			using (var dlg = new AddCharacterDlg(null))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Symbol != null)
				{
					m_symbols.Add(dlg.Symbol);
					Grid.RowCount++;
					m_dirty = true;
					SortList(dlg.Symbol);
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
			int i = Grid.CurrentCellAddress.Y;

			if (i < 0 || i >= m_symbols.Count)
				return;

			using (var dlg = new AddCharacterDlg(m_symbols[i]))
			{
				if (dlg.ShowDialog(this) != DialogResult.OK || dlg.Symbol == null)
					return;

				m_symbols[i] = dlg.Symbol;
				m_dirty = true;
				SortList(m_symbols[i]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked the DELETE button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDelete_Click(object sender, EventArgs e)
		{
			int i = Grid.CurrentCellAddress.Y;

			if (i < 0 || i >= m_symbols.Count)
				return;

			if (Utils.MsgBox(Properties.Resources.kstidIpaGridDeletion,
				MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				m_symbols.RemoveAt(i);
				m_dirty = true;
				Grid.RowCount--;
				SortList(i == m_symbols.Count ? m_symbols[i - 1] : m_symbols[i]);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void SortList()
		{
			int i = Grid.CurrentCellAddress.Y;
			SortList(i < 0 || i >= m_symbols.Count ? m_symbols[0] : m_symbols[i]);
		}

		/// ------------------------------------------------------------------------------------
		private void SortList(IPASymbol symbolToReturnTo)
		{
			foreach (DataGridViewColumn col in Grid.Columns)
			{
				col.HeaderCell.SortGlyphDirection =
					(col.Name != m_sortField ? SortOrder.None : m_sortDirection);
			}

			switch (m_sortField)
			{
				case kHexadecimal:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.Hexadecimal) :
						m_symbols.OrderByDescending(x => x.Hexadecimal)).ToList();
					break;

				case kLiteral:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.Literal) :
						m_symbols.OrderByDescending(x => x.Literal)).ToList();
					break;

				case kName:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.Name) :
						m_symbols.OrderByDescending(x => x.Name)).ToList();
					break;

				case kDescription:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.Description) :
						m_symbols.OrderByDescending(x => x.Description)).ToList();
					break;

				case kType:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.Type.ToString()) :
						m_symbols.OrderByDescending(x => x.Type.ToString())).ToList();
					break;

				case kSubType:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.SubType.ToString()) :
						m_symbols.OrderByDescending(x => x.SubType.ToString())).ToList();
					break;

				case kIgnoreType:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.IgnoreType.ToString()) :
						m_symbols.OrderByDescending(x => x.IgnoreType.ToString())).ToList();
					break;

				case kIsBase:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.IsBase) :
						m_symbols.OrderByDescending(x => x.IsBase)).ToList();
					break;

				case kCanPrecedeBase:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.CanPrecedeBase) :
						m_symbols.OrderByDescending(x => x.CanPrecedeBase)).ToList();
					break;

				case kDisplayWithDottedCircle:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => x.DisplayWithDottedCircle) :
						m_symbols.OrderByDescending(x => x.DisplayWithDottedCircle)).ToList();
					break;

				case kAFeatures:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => InventoryHelper.AFeatureCache.GetFeaturesText(x.AMask)) :
						m_symbols.OrderByDescending(x => InventoryHelper.AFeatureCache.GetFeaturesText(x.AMask))).ToList();
					break;

				case kBFeatures:
					m_symbols = (m_sortDirection == SortOrder.Ascending ?
						m_symbols.OrderBy(x => InventoryHelper.BFeatureCache.GetFeaturesText(x.BMask)) :
						m_symbols.OrderByDescending(x => InventoryHelper.BFeatureCache.GetFeaturesText(x.BMask))).ToList();
					break;
			}

			int c = Grid.CurrentCellAddress.X;
			int r = m_symbols.IndexOf(symbolToReturnTo);
			Grid.Refresh();
			Grid.CurrentCell = Grid[c, r];
			Grid.FirstDisplayedScrollingRowIndex = r;
		}
	}
}