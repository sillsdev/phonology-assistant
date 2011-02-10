using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.DataSource;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for SFDataSourcePropertiesDlg.
	/// </summary>
	/// --------------------------------------------------------------------------------
	public partial class SFDataSourcePropertiesDlg : OKCancelDlgBase, IxCoreColleague
	{
		#region member variables

		private FieldMapperGrid m_fieldsGrid;
		private SilGrid m_grid;
		private string m_filename;
		private List<SFMarkerMapping> m_mappings = new List<SFMarkerMapping>();
		private readonly List<string> m_markersInFile = new List<string>();
		private readonly IEnumerable<PaField> m_potentialFields;
		private readonly PaDataSource m_datasource;

		#endregion

		#region Construction/Setup
		/// ------------------------------------------------------------------------------------
		public SFDataSourcePropertiesDlg(IEnumerable<PaField> projectFields, PaDataSource ds)
		{
			m_potentialFields = PaField.Merge(projectFields, PaField.GetDefaultSfmFields());
			m_datasource = ds;
			InitializeComponent();
			Initialize();
			m_grid.CellEnter += HandleGridCellEnter;
		}

		/// ------------------------------------------------------------------------------------
		private void Initialize()
		{
			// If the grid is not null, we've already been here.
			if (App.DesignMode || m_grid != null)
				return;

			// For some reason when I set these values in the designer, I can't open
			// the form in designer again without errors.
			scImport.Panel1MinSize = 125;
			scImport.Panel2MinSize = 125;

			txtFilePreview.Font = FontHelper.DefaultPhoneticFont;
			pnlSrcFileHdg.Font = FontHelper.UIFont;
			pnlParseHdg.Font = FontHelper.UIFont;
			pnlMappingsHdg.Font = FontHelper.UIFont;
			lblFirstInterlinear.Font = FontHelper.UIFont;
			cboFirstInterlinear.Font = FontHelper.UIFont;
			lblSampleInput.Font = FontHelper.UIFont;
			lblSampleOutput.Font = FontHelper.UIFont;
			lblParseType.Font = FontHelper.UIFont;
			rbInterlinearize.Font = FontHelper.UIFont;
			rbParseOnlyPhonetic.Font = FontHelper.UIFont;
			rbParseOneToOne.Font = FontHelper.UIFont;
			rbNoParse.Font = FontHelper.UIFont;
			lblEditor.Font = FontHelper.UIFont;
			txtEditor.Font = FontHelper.UIFont;
			cboToolboxSortField.Font = FontHelper.UIFont;
			lblToolboxSortField.Font = FontHelper.UIFont;
			gridSampleOutput.Font = new Font(FontHelper.UIFont.FontFamily, 8f);
			gridSampleOutput.BorderStyle = BorderStyle.None;

			pnlParseHdg.BorderStyle = BorderStyle.None;
			pnlMappingsHdg.BorderStyle = BorderStyle.None;
			pnlSrcFileHdg.BorderStyle = BorderStyle.None;
			pnlSrcFileHdg.TextFormatFlags |= TextFormatFlags.PathEllipsis;

			m_filename = m_datasource.DataSourceFile;
			m_tooltip.SetToolTip(pnlSrcFileHdg, m_filename);
			txtFilePreview.Text = File.ReadAllText(m_filename);

			cboFirstInterlinear.Items.Add(App.LocalizeString(
				"SFDataSourcePropertiesDlg.UnspecifiedFirstInterlinearFieldItem", "(none)",
				"First item in the list of potential first interlinear fields.",
				App.kLocalizationGroupDialogs));

			cboFirstInterlinear.SelectedIndex = 0;

			InitializeToolboxSortFieldControls();
			InitializeBottomPanel();
			
			LoadMappings();
			PrepareMarkerList();
			InitializeFieldMappingsGrid();
			BuildMappingGrid();
			pnlMappingsHdg.ControlReceivingFocusOnMnemonic = m_grid;

			rbNoParse.Tag = Settings.Default.SFMNoParseOptionSampleOutput;
			rbParseOneToOne.Tag = Settings.Default.SFMOneToOneParseOptionSampleOutput;
			rbParseOnlyPhonetic.Tag = Settings.Default.SFMPhoneticParseOptionSampleOutput;
			rbInterlinearize.Tag = Settings.Default.SFMInterlinearParseOptionSampleOutput;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeBottomPanel()
		{
			if (m_datasource != null && m_datasource.DataSourceType == DataSourceType.Toolbox)
			{
				// Things line up better if I first set the height to 0.
				tblLayoutToolBoxSortField.Visible = true;
				tblLayoutToolBoxSortField.Height = 0;
				tblLayoutButtons.Controls.Add(tblLayoutToolBoxSortField, 0, 0);
				tblLayoutToolBoxSortField.Dock = DockStyle.Fill;
			}
			else
			{
				// Things line up better if I first set the height to 0.
				tblLayoutEditor.Visible = true;
				tblLayoutEditor.Height = 0;
				tblLayoutButtons.Controls.Add(tblLayoutEditor, 0, 0);
				tblLayoutEditor.Dock = DockStyle.Fill;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeToolboxSortFieldControls()
		{
			cboToolboxSortField.Items.Add(App.LocalizeString(
				"SFDataSourcePropertiesDlg.UnspecifiedToolboxSortField", "(none)",
				App.kLocalizationGroupDialogs));

			cboToolboxSortField.Items.AddRange(m_potentialFields.Select(f => f.DisplayName).ToArray());
			int i = cboToolboxSortField.Items.IndexOf(m_datasource.ToolboxSortField ?? string.Empty);
			cboToolboxSortField.SelectedIndex = (i < 0 ? 0 : i);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the mappings list. This method will make sure that existing mappings that
		/// no longer have matching PA fields are not included and PA fields that don't yet
		/// have mappings are included.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadMappings()
		{
			//m_mappings = new List<SFMarkerMapping>();

			//// Clone mappings and, along the way, create the
			//// list of possible first interlinear fields.
			//foreach (var mapping in m_datasource.SFMappings)
			//{
			//    PaFieldInfo fieldInfo = m_fieldInfo[mapping.FieldName];

			//    // Data source and data source path cannot be mapped to.
			//    if (fieldInfo != null && (fieldInfo.IsDataSource || fieldInfo.IsDataSourcePath))
			//        continue;

			//    if (fieldInfo != null || mapping.FieldName == PaDataSource.kRecordMarker)
			//    {
			//        var clone = mapping.Clone();
			//        m_mappings.Add(clone);

			//        // Don't put the record marker field in the list of
			//        // possible first interlinear fields.
			//        if (clone.FieldName != PaDataSource.kRecordMarker && fieldInfo != null &&
			//            fieldInfo.CanBeInterlinear)
			//        {
			//            cboFirstInterlinear.Items.Add(clone);
			//            if (m_datasource.FirstInterlinearField == clone.FieldName)
			//                cboFirstInterlinear.SelectedItem = clone;
			//        }
			//    }
			//}

			//// Now make sure the mappings contain all the fields in the project. It may be that the
			//// mappings list doesn't for two reasons. 1) The user has added some custom fields
			//// since coming here to modify mappings or 2) A new release of PA introduced some new
			//// intrinsic PA fields.
			//foreach (var field in m_fieldInfo)
			//{
			//    // Data source and data source path cannot be mapped to.
			//    if (!field.IsDataSource && !field.IsDataSourcePath)
			//    {
			//        var newMapping = SFMarkerMapping.VerifyMappingForField(m_mappings, field);
			//        if (newMapping != null && field.CanBeInterlinear)
			//            cboFirstInterlinear.Items.Add(newMapping);
			//    }
			//}

			//// Finally, sort the fields alphabetically
			//var sortedMappings = new SortedList<string, SFMarkerMapping>();

			//foreach (var mapping in m_mappings)
			//    sortedMappings[mapping.DisplayText] = mapping;

			//m_mappings.Clear();
			//foreach (var mapping in sortedMappings.Values)
			//{
			//    if (mapping.FieldName == PaDataSource.kRecordMarker)
			//        m_mappings.Insert(0, mapping);
			//    else
			//        m_mappings.Add(mapping);
			//}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scan the file for markers and build the list of markers the user may assign to PA
		/// fields (that list will be used to fill the grid's combo box column).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PrepareMarkerList()
		{
			//if (m_filename == null)
			//    return;

			// Scan the files to find all the markers contained therein.
			m_datasource.TotalLinesInFile = GetMarkersFromFile(m_filename, m_markersInFile);

			//// Add the "<none>" item for the combo drop-down.
			//m_markersInFile.Insert(0, SFMarkerMapping.NoneText);

			// Go through the list of mappings found in the file and toss
			// out those that couldn't be found in the scanned files to import.
			//foreach (var mapping in m_mappings.Where(m => !m_markersInFile.Contains(m.Marker)))
			//    mapping.Marker = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the parse type for the currently checked parse type radio button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private DataSourceParseType CurrentParseType
		{
			get
			{
				if (rbNoParse.Checked)
					return DataSourceParseType.None;
				
				if (rbParseOneToOne.Checked)
					return DataSourceParseType.OneToOne;
				
				if (rbInterlinearize.Checked)
					return DataSourceParseType.Interlinear;

				return DataSourceParseType.PhoneticOnly;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			rbNoParse.Checked = false;
			rbParseOnlyPhonetic.Checked = false;
			rbParseOneToOne.Checked = false;
			rbInterlinearize.Checked = false;

			rbNoParse.CheckedChanged += HandleReadTypeCheckedChanged;
			rbParseOnlyPhonetic.CheckedChanged += HandleReadTypeCheckedChanged;
			rbParseOneToOne.CheckedChanged += HandleReadTypeCheckedChanged;
			rbInterlinearize.CheckedChanged += HandleReadTypeCheckedChanged;

			base.OnShown(e);

			// Make sure the stuff in the left (fixed) panel can all be seen.
			if (cboFirstInterlinear.Right > splitOuter.SplitterDistance - 11)
				splitOuter.SplitterDistance = (cboFirstInterlinear.Right + 11);

			if (Settings.Default.SFDataSourcePropertiesDlgSplitLoc > 0)
				scImport.SplitterDistance = Settings.Default.SFDataSourcePropertiesDlgSplitLoc;
			else
			{
				// By default, give the mappings grid 65% of the width of its containing
				// split container control. That means the file contents gets 35%.
				scImport.SplitterDistance = (int)(scImport.Width * .65);
			}

			// Sets the proper parsing type. This is best
			// done later rather than in the constructor.
			switch (m_datasource.ParseType)
			{
				case DataSourceParseType.None: rbNoParse.Checked = true; break;
				case DataSourceParseType.OneToOne: rbParseOneToOne.Checked = true; break;
				case DataSourceParseType.Interlinear: rbInterlinearize.Checked = true; break;
				default: rbParseOnlyPhonetic.Checked = true; break;
			}

			// I'm not sure why this has to be done so late, but for some reason rows in the
			// grid were added and removed after the handle is created but before showing
			// the dialog, thus causing the dirty flag to get set to true. The adding and
			// removing takes place in code I don't control.
			m_grid.IsDirty = false;
		}

		#endregion
		
		#region Mapping Grid Setup
		/// ------------------------------------------------------------------------------------
		private void InitializeFieldMappingsGrid()
		{
			if (m_datasource.FieldMappings == null || m_datasource.FieldMappings.Count == 0)
			{
				// Create a new list of mappings, making sure to initialize each one's
				// Field property, if possible, based on the existing project's fields
				// and the default SFM fields.
				m_datasource.FieldMappings = m_markersInFile.Select(mkr =>
				{
					var field = m_potentialFields.SingleOrDefault(f => f.GetPossibleDataSourceFieldNames().Contains(mkr));
					var isParsed = (field != null && Settings.Default.DefaultParsedSfmFields.Contains(field.Name));
					return new FieldMapping(mkr, field, isParsed);
				}).ToList();
			}

			m_fieldsGrid = new FieldMapperGrid(m_potentialFields, m_datasource.FieldMappings);
			m_fieldsGrid.Dock = DockStyle.Fill;
			pnlMappings.Controls.Add(m_fieldsGrid);
			m_fieldsGrid.BringToFront();

			m_fieldsGrid.SourceFieldColumnHeadingTextHandler = delegate
			{
				return App.LocalizeString(
					"SFDataSourcePropertiesDlg.SourceFieldColumnHeadingText",
					"Map this Marker...", App.kLocalizationGroupDialogs);
			};

			m_fieldsGrid.TargetFieldColumnHeadingTextHandler = delegate
			{
				return App.LocalizeString(
					"SFDataSourcePropertiesDlg.TargetFieldColumnHeadingText",
					"To this Field", App.kLocalizationGroupDialogs);
			};
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Sets up the query grid display.
		/// </summary>
		/// --------------------------------------------------------------------------------
		private void BuildMappingGrid()
		{
			m_grid = new SilGrid();
			return;
			m_grid.Name = Name + "Grid";
			m_grid.BorderStyle = BorderStyle.None;
			m_grid.Dock = DockStyle.Fill;
			m_grid.AutoGenerateColumns = false;
			m_grid.AllowUserToOrderColumns = false;
			m_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			m_grid.CellPainting += HandleGridCellPainting;
			m_grid.CellClick += HandleGridCellClick;
			m_grid.DataSource = m_mappings;

			// Create the marker column and pass it the list of markers found in the files to
			// use for the content of the column's combobox.
			DataGridViewColumn col = SilGrid.CreateDropDownListComboBoxColumn("marker", m_markersInFile);
			col.DataPropertyName = "Marker";
			((DataGridViewComboBoxColumn)col).ValueMember = "MarkerComboBoxDisplayText";
			((DataGridViewComboBoxColumn)col).DisplayMember = "MarkerComboBoxDisplayText";
			m_grid.Columns.Add(col);
			App.LocalizeObject(m_grid.Columns["marker"],
				"SFDataSourcePropertiesDlg.MarkerColumnHeadingText", "Map this marker...",
				App.kLocalizationGroupDialogs);

			// Create the column for the arrow.
			col = SilGrid.CreateImageColumn("arrow");
			col.HeaderText = string.Empty;
			col.ReadOnly = true;
			col.Resizable = DataGridViewTriState.False;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DataPropertyName = "MapToSymbol";
			m_grid.Columns.Add(col);

			// Create the column for the PA field name.
			col = SilGrid.CreateTextBoxColumn("pafield");
			col.ReadOnly = true;
			col.DataPropertyName = "DisplayText";
			m_grid.Columns.Add(col);
			App.LocalizeObject(m_grid.Columns["pafield"],
				"SFDataSourcePropertiesDlg.PaFieldColumnHeadingText", "To this Field",
				App.kLocalizationGroupDialogs);

			// Create the column for the interlinear check box.
			col = SilGrid.CreateCheckBoxColumn("interlinear");
			col.DataPropertyName = "IsInterlinear";
			//col.Width = 85;
			m_grid.Columns.Add(col);
			App.LocalizeObject(m_grid.Columns["interlinear"],
				"SFDataSourcePropertiesDlg.IsInterlinearColumnHeadingText", "Interlinear Field?",
				App.kLocalizationGroupDialogs);

			pnlMappings.Controls.Add(m_grid);
			m_grid.BringToFront();

			try
			{
				// Set the record Id row cells to bold.
				Font fnt = m_grid.Rows[0].Cells["marker"].Style.Font;
				m_grid.Rows[0].Cells["marker"].Style.Font = FontHelper.MakeFont(fnt, FontStyle.Bold);
				fnt = m_grid.Rows[0].Cells["pafield"].Style.Font;
				m_grid.Rows[0].Cells["pafield"].Style.Font = FontHelper.MakeFont(fnt, FontStyle.Bold);
			}
			catch { }

			if (Settings.Default.SFDataSourcePropertiesDlgGrid != null)
				Settings.Default.SFDataSourcePropertiesDlgGrid.InitializeGrid(m_grid);
			else
			{
				m_grid.AutoResizeColumnHeadersHeight();
				m_grid.AutoResizeColumns();
				m_grid.AutoResizeRows();
			}
		}

		#endregion

		#region Overridden Methods when closing form
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.SFDataSourcePropertiesDlgGrid = GridSettings.Create(m_grid);
			Settings.Default.SFDataSourcePropertiesDlgSplitLoc = scImport.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verify the mappings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			// Commit pending changes in the grid.
			m_grid.CommitEdit(DataGridViewDataErrorContexts.Commit);

			string msg = null;
			int count = 0;
			bool toolboxSortFieldFound = (ToolBoxSortField == null);

			foreach (SFMarkerMapping mapping in m_mappings)
			{
				// Check if this field's mapping is also the field
				// specified for the ToolBox sort field.
				if (!toolboxSortFieldFound && mapping.FieldName == ToolBoxSortField &&
					!string.IsNullOrEmpty(mapping.Marker))
				{
					toolboxSortFieldFound = true;
				}

				// The record marker must be mapped.
				if (mapping.FieldName == PaDataSource.kRecordMarker &&
					string.IsNullOrEmpty(mapping.Marker))
				{
					count = 1;

					msg = App.LocalizeString("SFDataSourcePropertiesDlg.MissingRecordMarkerMappingMsg",
						"You must specify a marker for the 'Record Marker' in order for {0} to identify the beginning of each record.",
						App.kLocalizationGroupDialogs);

					msg = string.Format(msg, Application.ProductName);
					break;
				}
				
				if (!string.IsNullOrEmpty(mapping.Marker))
					count++;
			}

			// There must be at least one mapping specified other than the record marker.
			if (count == 0)
			{
				msg = App.LocalizeString("SFDataSourcePropertiesDlg.NoMappingsSpecifiedMsg",
					"You must specify at least one field mapping other than one for the 'Record Marker'.",
					App.kLocalizationGroupDialogs);
			}

			if (msg == null)
				msg = VerifyInterlinearInfo();

			if (msg == null && !toolboxSortFieldFound)
			{
				msg = App.LocalizeString("SFDataSourcePropertiesDlg.InvalidToolboxSortFieldSpecifiedMsg",
					"The first Toolbox sort field specified was\nnot mapped. It must have a mapping.",
					App.kLocalizationGroupDialogs);
			}

			if (msg != null)
			{
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure all the settings for interlinear fields are consistent.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string VerifyInterlinearInfo()
		{
			HandleFirstInterlinearComboSelectedIndexChanged(null, null);
			
			SFMarkerMapping fim = cboFirstInterlinear.SelectedItem as SFMarkerMapping;
			string firstInterlinField = (fim == null ? null : fim.FieldName);
			string msg = null;
			int interlinearFieldCount = 0;

			// Check for unmapped fields that are specified as interlinear fields.
			foreach (SFMarkerMapping mapping in m_mappings)
			{
				if (mapping.IsInterlinear)
				{
					interlinearFieldCount++;
					if (string.IsNullOrEmpty(mapping.Marker))
						msg += mapping.DisplayText + "\n";
				}
			}

			if (msg != null)
			{
				msg = App.LocalizeString("SFDataSourcePropertiesDlg.NoMappingForInterlinearFieldMsg",
					"The following field(s) have been specified as interlinear fields but need to be mapped to markers.\n\n",
					App.kLocalizationGroupDialogs);
			}
			else if (firstInterlinField == null)
			{
				// Check any fields marked as interlinear but without
				// a first interlinear field having been specified too.
				if (m_mappings.Any(m => m.IsInterlinear))
				{
					msg = GetNoFirstInterlinearFieldMsg();
					cboFirstInterlinear.Focus();
				}
			}

			// Check if the first interlinear field was specified unecessarily.
			if (msg == null && interlinearFieldCount < 2 && firstInterlinField != null)
			{
				msg = App.LocalizeString("SFDataSourcePropertiesDlg.UnecessaryFirstInterlinearFieldMsg",
					"Because you have specified the first interlinear field, you must also specify at least one other interlinear field from those that are mapped.",
					App.kLocalizationGroupDialogs);
			}

			// Check if a first field was selected for the Interlinearize option
			if (msg == null && rbInterlinearize.Checked && firstInterlinField == null)
				msg = GetNoFirstInterlinearFieldMsg();

			return msg;
		}

		/// ------------------------------------------------------------------------------------
		private string GetNoFirstInterlinearFieldMsg()
		{
			return App.LocalizeString("SFDataSourcePropertiesDlg.NoFirstInterlinearFieldMsg",
						"You must specify what is each record's first interlinear field.",
						App.kLocalizationGroupDialogs);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get
			{
				var mapping = cboFirstInterlinear.SelectedItem as SFMarkerMapping;
				string firstInterlinField = (mapping == null ? null : mapping.FieldName);

				return (m_grid.IsDirty || CurrentParseType != m_datasource.ParseType ||
					firstInterlinField != m_datasource.FirstInterlinearField ||
					ToolBoxSortField != m_datasource.ToolboxSortField ||
					txtEditor.Text != m_datasource.Editor);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the Toolbox sort field specified in the combo box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string ToolBoxSortField
		{
			get
			{
				return (cboToolboxSortField.SelectedIndex <= 0 ? null :
					((PaFieldInfo)cboToolboxSortField.SelectedItem).FieldName);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			m_datasource.ParseType = CurrentParseType;
			m_datasource.ToolboxSortField = ToolBoxSortField;
			m_datasource.Editor = txtEditor.Text.Trim();
			m_datasource.FieldMappings = m_fieldsGrid.Mappings.ToList();
				
			var firstILFieldName = cboFirstInterlinear.SelectedItem as string;
			var field = m_potentialFields.SingleOrDefault(f => f.DisplayName == firstILFieldName);
			m_datasource.FirstInterlinearField = (field == null ? null : field.Name);
			
			return true;
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user clicks on the cell with the arrow, then move the cell to the SFM
		/// cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleGridCellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 1 && e.RowIndex >= 0)
				m_grid.CurrentCell = m_grid[0, e.RowIndex];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Drop-down the SFM column's combo box when the SFM column cell's become current.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleGridCellEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == 0)
				SendKeys.Send("%{DOWN}");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure any field that can't be interlinear has it's check box painted over so
		/// the check box cannot be seen. Also make sure (if the field cannot be interlinear),
		/// that its check box cell is made read-only.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			//if (e.ColumnIndex == 3 && e.RowIndex >= 0)
			//{
			//    var mapping = m_mappings[e.RowIndex];
			//    var fieldInfo = m_fieldInfo[mapping.FieldName];

			//    if (fieldInfo == null || !fieldInfo.CanBeInterlinear)
			//    {
			//        bool selected = (e.State & DataGridViewElementStates.Selected) > 0;
			//        e.PaintBackground(e.ClipBounds, selected);
			//        e.Handled = true;
			//    }
			//}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure that the field chosen as the first interlinear field is also marked
		/// as an interlinear field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFirstInterlinearComboSelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboFirstInterlinear.SelectedIndex <= 0)
				return;
			
			var mapping = cboFirstInterlinear.SelectedItem as SFMarkerMapping;
			if (mapping != null && !mapping.IsInterlinear)
			{
				mapping.IsInterlinear = true;
				if (m_grid != null)
					m_grid.Refresh();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the editor for the SFM data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleBrowseClick(object sender, EventArgs e)
		{
			var caption = App.LocalizeString("SFDataSourcePropertiesDlg.BrowseForSFMEditorDialogCaption",
						"Standard Format Data Source Editor", App.kLocalizationGroupDialogs);

			string filter = App.kstidFileTypeAllExe + "|" + App.kstidFileTypeAllFiles;
			string editor = App.OpenFileDialog("exe", filter, caption);

			if (!string.IsNullOrEmpty(editor))
				txtEditor.Text = editor.Trim();
		}

		/// ------------------------------------------------------------------------------------
		private string HandleSourceFilePanelBeforeDrawText(object sender)
		{
			return string.Format(pnlSrcFileHdg.Text, m_filename);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scan the file, searching for all unique markers (e.g., "\name") in a file.
		/// </summary>
		/// <param name="filename">Filename to parse</param>
		/// <param name="markerList">Array in which markers are stored.</param>
		/// <returns>The number of bldr read from the file.</returns>
		/// ------------------------------------------------------------------------------------
		protected int GetMarkersFromFile(string filename, List<string> markerList)
		{
			StreamReader reader = null;
			int numLines = 0;

			try 
			{
				reader = new StreamReader(filename);

				string line;
				while ((line = reader.ReadLine()) != null)
				{
					numLines++;

					// For those bldr beginning with a backslash, strip off the marker and keep it.
					if (line.StartsWith("\\") && !line.StartsWith(PaDataSource.kShoeboxMarker) &&
						!line.StartsWith("\\_Date"))
					{
						string marker = line.Split(' ')[0];
						if (!markerList.Contains(marker))
							markerList.Add(marker);
					}
				}
			}
			catch (Exception e)
			{
				var msg = App.LocalizeString("SFDataSourcePropertiesDlg.ErrorReadingSourceFileMsg",
					"The following error occurred trying to read the source file '{0}'.\n\n{1}",
					App.kLocalizationGroupDialogs);

				MessageBox.Show(string.Format(msg, e.Message));
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}
		
			return numLines;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the enabled state of the first interlinear field list and updates the
		/// input and output samples.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleReadTypeCheckedChanged(object sender, EventArgs e)
		{
			lblFirstInterlinear.Enabled = rbInterlinearize.Checked;
			cboFirstInterlinear.Enabled = rbInterlinearize.Checked;
			gridSampleOutput.Columns["SampleOutputPartOfSpeechColumn"].Visible = rbInterlinearize.Checked;

			var rb = sender as RadioButton;
			if (rb == null)
				return;

			if (rbNoParse.Checked || rbParseOnlyPhonetic.Checked || rbParseOneToOne.Checked)
			{
				// Set the First Interlinear field to NONE
				cboFirstInterlinear.SelectedIndex = 0;

				// Make all fields NOT interlinear
				foreach (var mapping in m_mappings.Where(m => m.IsInterlinear))
					mapping.IsInterlinear = false;
			}

			m_fieldsGrid.ShowIsInterlinearColumn(rbInterlinearize.Checked);
			m_fieldsGrid.ShowIsParsedColumn(rbParseOneToOne.Checked || rbInterlinearize.Checked);

			// The rest of the code in this method deals with building
			// the appropriate sample portion of the panel.
			rtfSampleInput.Rtf = (rbInterlinearize.Checked ?
				Settings.Default.SFMInterlinearParseOptionSampleInput :
				Settings.Default.SFMBasicParseOptionSampleInput);

			// Get the unparsed sample output string.
			string sampleInput = rb.Tag as string;

			if (string.IsNullOrEmpty(sampleInput))
				return;

			// Split the sample output string into the pieces that represent rows in the
			// sample grid, clear the grid and add the appropriate number of rows.
			var rows = sampleInput.Split("#".ToCharArray());
			gridSampleOutput.Rows.Clear();
			gridSampleOutput.Rows.Add(rows.Length);

			for (int r = 0; r < rows.Length; r++)
			{
				string[] cells = rows[r].Split("/".ToCharArray(), StringSplitOptions.None);
				for (int c = 0; c < cells.Length && c < gridSampleOutput.Columns.Count; c++)
					gridSampleOutput[c, r].Value = cells[c];
			}
		}

		#endregion

		#region IxCoreColleague Members
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] {this});
		}

		#endregion
	}
}
