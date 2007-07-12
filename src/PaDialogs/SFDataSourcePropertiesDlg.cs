using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text;
using SIL.Pa.Controls;
using SIL.Pa.Resources;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa.Dialogs
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for SFDataSourcePropertiesDlg.
	/// </summary>
	/// --------------------------------------------------------------------------------
	public partial class SFDataSourcePropertiesDlg : OKCancelDlgBase, IxCoreColleague
	{
		#region member variables

		private SilGrid m_grid;
		private string m_filename;
		private List<string> m_markersInFile = new List<string>();
		private List<SFMarkerMapping> m_mappings;
		private PaDataSource m_datasource;
		private PaFieldInfoList m_fieldInfo;

		#endregion

		#region Construction/Setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SFDataSourcePropertiesDlg(PaFieldInfoList fieldInfo, PaDataSource datasource) : base()
		{
			m_datasource = datasource;
			m_fieldInfo = fieldInfo;

			// Required for Windows Form Designer support
			InitializeComponent();

			Initialize();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize()
		{
			// If the grid is not null, we've already been here.
			if (PaApp.DesignMode || m_grid != null)
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

			pnlParseHdg.BorderStyle = BorderStyle.None;
			pnlMappingsHdg.BorderStyle = BorderStyle.None;
			pnlSrcFileHdg.BorderStyle = BorderStyle.None;

			lblFilename.Font = FontHelper.UIFont;
			lblFilename.Text = string.Empty;

			m_filename = m_datasource.DataSourceFile;
			txtFilePreview.Text = File.ReadAllText(m_filename);

			cboFirstInterlinear.Items.Add(Properties.Resources.kstidSFMNoFirstInterlinearFieldItem);
			cboFirstInterlinear.SelectedIndex = 0;

			pnlEditor.Parent.Controls.Remove(pnlEditor);
			pnlButtons.Controls.Add(pnlEditor);
			pnlEditor.Width = pnlToolboxSortField.Width;

			if (m_datasource != null && m_datasource.DataSourceType == DataSourceType.Toolbox)
				pnlToolboxSortField.Visible = true;
			else
			{
				pnlEditor.Location = new Point(0, 0);
				pnlEditor.Visible = true;
			}

			InitializeEditorSpecificationControls();
			InitializeToolboxSortFieldControls();
			LoadMappings();
			PrepareMarkerList();
			BuildMappingGrid();
			pnlMappingsHdg.ControlReceivingFocusOnMnemonic = m_grid;

			PaApp.SettingsHandler.LoadFormProperties(this);

			if (cboFirstInterlinear.Right > (pnlParseType.Width - 10))
			{
				splitOuter.SplitterDistance +=
					(cboFirstInterlinear.Right + 10 - pnlParseType.Width);
			}

			try
			{
				int splitterDistance = PaApp.SettingsHandler.GetIntSettingsValue(Name, "splitter", -1);
				if (splitterDistance > -1)
					scImport.SplitterDistance = splitterDistance;
			}
			catch { }

			rbNoParse.Tag = Properties.Resources.kstidNoParseSampleOutput;
			rbParseOneToOne.Tag = Properties.Resources.kstidOneToOneSampleOutput;
			rbParseOnlyPhonetic.Tag = Properties.Resources.kstidParsePhoneticSampleOutput;
			rbInterlinearize.Tag = Properties.Resources.kstidInterlinearSampleOutput;

			string tooltip = STUtils.ConvertLiteralNewLines(
				Properties.Resources.kstidOneToOneParsingToolTip);

			m_tooltip.SetToolTip(rbParseOneToOne, tooltip);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeEditorSpecificationControls()
		{
			lblEditor.Font = FontHelper.UIFont;
			txtEditor.Font = FontHelper.UIFont;
			lblEditor.Top = (pnlEditor.Height - lblEditor.Height) / 2;
			txtEditor.Top = (pnlEditor.Height - txtEditor.Height) / 2;
			txtEditor.Text = m_datasource.Editor;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeToolboxSortFieldControls()
		{
			cboToolboxSortField.Items.AddRange(m_fieldInfo.ToArray());
			cboToolboxSortField.Items.Insert(0, Properties.Resources.kstidNoToolboxSortField);
			cboToolboxSortField.Font = FontHelper.UIFont;
			lblToolboxSortField.Font = FontHelper.UIFont;
			lblToolboxSortField.Top = (pnlToolboxSortField.Height - lblToolboxSortField.Height) / 2;
			cboToolboxSortField.Top = (pnlToolboxSortField.Height - cboToolboxSortField.Height) / 2;

			string sortField = m_datasource.ToolboxSortField;

			if (string.IsNullOrEmpty(sortField))
				cboToolboxSortField.SelectedIndex = 0;
			else
			{
				// Go through the fields in the combo. and find the one that matches the
				// toolbox sort field specified in the project.
				for (int i = 1; i < cboToolboxSortField.Items.Count; i++)
				{
					PaFieldInfo fieldInfo = cboToolboxSortField.Items[i] as PaFieldInfo;
					if (fieldInfo != null && fieldInfo.FieldName == sortField)
					{
						cboToolboxSortField.SelectedIndex = i;
						break;
					}
				}
			}
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
			m_mappings = new List<SFMarkerMapping>();

			// Clone mappings and, along the way, create the
			// list of possible first interlinear fields.
			foreach (SFMarkerMapping mapping in m_datasource.SFMappings)
			{
				PaFieldInfo fieldInfo = m_fieldInfo[mapping.FieldName];

				// Data source and data source path cannot be mapped to.
				if (fieldInfo != null && (fieldInfo.IsDataSource || fieldInfo.IsDataSourcePath))
					continue;

				// This should only happen if the user removed a custom field since
				// the time he was in here to modify the data source's mappings.
				if (fieldInfo != null || mapping.FieldName == PaDataSource.kRecordMarker)
				{
					SFMarkerMapping clone = mapping.Clone();
					m_mappings.Add(clone);

					// Don't put the record marker field in the list of
					// possible first interlinear fields.
					if (clone.FieldName != PaDataSource.kRecordMarker && fieldInfo.CanBeInterlinear)
					{
						cboFirstInterlinear.Items.Add(clone);
						if (m_datasource.FirstInterlinearField == clone.FieldName)
							cboFirstInterlinear.SelectedItem = clone;
					}
				}
			}

			// Now make sure the mappings contain all the fields in the project. It may be that the
			// mappings list doesn't for for two reasons. 1) The user has added some custom fields
			// since coming here to modify mappings or 2) A new release of PA introduced some new
			// intrinsic PA fields.
			for (int i = 0; i < m_fieldInfo.Count; i++)
			{
				// Data source and data source path cannot be mapped to.
				if (!m_fieldInfo[i].IsDataSource && !m_fieldInfo[i].IsDataSourcePath)
				{
					SFMarkerMapping newMapping =
						SFMarkerMapping.VerifyMappingForField(m_mappings, m_fieldInfo[i]);

					if (newMapping != null && m_fieldInfo[i].CanBeInterlinear)
						cboFirstInterlinear.Items.Add(newMapping);
				}
			}
	
			// Finally, sort the fields alphabetically
			SortedList<string, SFMarkerMapping> sortedMappings =
				new SortedList<string, SFMarkerMapping>();

			foreach (SFMarkerMapping mapping in m_mappings)
				sortedMappings[mapping.DisplayText] = mapping;

			m_mappings.Clear();
			foreach (SFMarkerMapping mapping in sortedMappings.Values)
			{
				if (mapping.FieldName == PaDataSource.kRecordMarker)
					m_mappings.Insert(0, mapping);
				else
					m_mappings.Add(mapping);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scan the file for markers and build the list of markers the user may assign to PA
		/// fields (that list will be used to fill the grid's combo box column).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PrepareMarkerList()
		{
			if (m_filename == null)
				return;

			// Scan the files to find all the markers contained therein.
			m_datasource.TotalLinesInFile = GetMarkersFromFile(m_filename, m_markersInFile);

			// Add the "<none>" item for the combo drop-down.
			m_markersInFile.Insert(0, SFMarkerMapping.NoneText);

			// Go through the list of mappings found in the file and toss
			// out those that couldn't found in the scanned files to import.
			foreach (SFMarkerMapping mapping in m_mappings)
			{
				// If there's a marker in the mappings list that doesn't occur in the
				// scanned files to import, then unmap that DB field.
				if (!m_markersInFile.Contains(mapping.Marker))
					mapping.Marker = null;
			}
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
				else if (rbParseOneToOne.Checked)
					return DataSourceParseType.OneToOne;
				else if (rbInterlinearize.Checked)
					return DataSourceParseType.Interlinear;

				return DataSourceParseType.PhoneticOnly;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the proper parsing type. This is best done after the grid, handle is created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// This is called here because, for some reason, on some machines, resuming
			// layout in the InitializeComponents method forces the handle to be created.
			Initialize();

			switch (m_datasource.ParseType)
			{
				case DataSourceParseType.None: rbNoParse.Checked = true; break;
				case DataSourceParseType.OneToOne: rbParseOneToOne.Checked = true; break;
				case DataSourceParseType.Interlinear: rbInterlinearize.Checked = true; break;
				default: rbParseOnlyPhonetic.Checked = true; break;
			}

			m_grid.CellEnter += new DataGridViewCellEventHandler(m_grid_CellEnter);
		}

		#endregion
		
		#region Mapping Grid Setup
		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Sets up the query grid display.
		/// </summary>
		/// --------------------------------------------------------------------------------
		private void BuildMappingGrid()
		{
			m_grid = new SilGrid();
			m_grid.Name = Name + "Grid";
			m_grid.BorderStyle = BorderStyle.None;
			m_grid.Dock = DockStyle.Fill;
			m_grid.AutoGenerateColumns = false;
			m_grid.AllowUserToOrderColumns = false;
			m_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			m_grid.CellPainting += new DataGridViewCellPaintingEventHandler(m_grid_CellPainting);
			m_grid.CellClick += new DataGridViewCellEventHandler(m_grid_CellClick);
			m_grid.DataSource = m_mappings;

			// Create the marker column and pass it the list of markers found in the files to
			// use for the content of the column's combobox.
			DataGridViewColumn col = SilGrid.CreateDropDownListComboBoxColumn("marker", m_markersInFile);
			col.HeaderText = Properties.Resources.kstidSFMMappingsGridMarker;
			col.DataPropertyName = "Marker";
			((DataGridViewComboBoxColumn)col).ValueMember = "MarkerComboBoxDisplayText";
			((DataGridViewComboBoxColumn)col).DisplayMember = "MarkerComboBoxDisplayText";
			m_grid.Columns.Add(col);

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
			col.HeaderText = Properties.Resources.kstidSFMMappingGridPaField;
			col.ReadOnly = true;
			col.DataPropertyName = "DisplayText";
			m_grid.Columns.Add(col);

			// Create the column for the interlinear check box.
			col = SilGrid.CreateCheckBoxColumn("interlinear");
			col.HeaderText = Properties.Resources.kstidSFMMappingGridInterlinear;
			col.DataPropertyName = "IsInterlinear";
			col.Width = 85;
			m_grid.Columns.Add(col);

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
			catch {}

			// If there weren't values previously saved for this grid, then set some of the
			// grid's properties to their initial values.
			string gridLinesValue;
			if (!PaApp.SettingsHandler.LoadGridProperties(m_grid, out gridLinesValue))
			{
				m_grid.AutoResizeColumns();
				m_grid.AutoResizeRows();
				m_grid.Columns["marker"].Width = 75;
				m_grid.Columns["interlinear"].Width = 75;
				m_grid.AutoResizeColumnHeadersHeight();
			}
		}

		#endregion

		#region Overridden Methods when closing form
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			base.SaveSettings();
			PaApp.SettingsHandler.SaveFormProperties(this);
			PaApp.SettingsHandler.SaveGridProperties(m_grid, null);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitter", scImport.SplitterDistance);
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
					msg = string.Format(Properties.Resources.kstidSFMMissingRecMarkerMapping,
						Application.ProductName);
					break;
				}
				else if (!string.IsNullOrEmpty(mapping.Marker))
					count++;
			}

			// There must be at least one mapping specified other than the record marker.
			if (count == 0)
				msg = Properties.Resources.kstidSFMNoMappingsSpecified;

			if (msg == null)
				msg = VerifyInterlinearInfo();

			if (msg == null && !toolboxSortFieldFound)
				msg = Properties.Resources.kstidInvalidToolboxSortFieldSpecified;

			if (msg != null)
			{
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
			cboFirstInterlinear_SelectedIndexChanged(null, null);
			
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
				msg = string.Format(Properties.Resources.kstidSFMNomappingForInterlinearField,
					"\n", "\n") + msg;
			}
			else
			{
				// Check any fields marked as interlinear but without
				// a first interlinear field having been specified too.
				foreach (SFMarkerMapping mapping in m_mappings)
				{
					if (mapping.IsInterlinear && firstInterlinField == null)
					{
						msg = Properties.Resources.kstidSFMNoFirstInterlinearField;
						cboFirstInterlinear.Focus();
						break;
					}
				}
			}

			// Check if the first interlinear field was specified unecessarily.
			if (msg == null && interlinearFieldCount < 2 && firstInterlinField != null)
				msg = Properties.Resources.kstidSFMUnecessaryFirstInterlinearField;

			// Check if a first field was selected for the Interlinearize option
			if (msg == null && rbInterlinearize.Checked && firstInterlinField == null)
				msg = Properties.Resources.kstidSFMNoFirstInterlinearField;

			return msg;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get
			{
				SFMarkerMapping mapping = cboFirstInterlinear.SelectedItem as SFMarkerMapping;
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
				return (cboToolboxSortField.SelectedIndex == 0 ? null :
						((PaFieldInfo)cboToolboxSortField.SelectedItem).FieldName);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			m_datasource.SFMappings = m_mappings;
			SFMarkerMapping mapping = cboFirstInterlinear.SelectedItem as SFMarkerMapping;
			m_datasource.FirstInterlinearField = (mapping == null ? null : mapping.FieldName);
			m_datasource.ParseType = CurrentParseType;
			m_datasource.ToolboxSortField = ToolBoxSortField;
			m_datasource.Editor = txtEditor.Text.Trim();
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
		void m_grid_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 1 && e.RowIndex >= 0)
				m_grid.CurrentCell = m_grid[0, e.RowIndex];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Drop-down the SFM column's combo box when the SFM column cell's become current.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CellEnter(object sender, DataGridViewCellEventArgs e)
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
		void m_grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex == 3 && e.RowIndex >= 0)
			{
				SFMarkerMapping mapping = m_mappings[e.RowIndex];
				PaFieldInfo fieldInfo = m_fieldInfo[mapping.FieldName];

				if (fieldInfo == null || !fieldInfo.CanBeInterlinear)
				{
					bool selected = (e.State & DataGridViewElementStates.Selected) > 0;
					e.PaintBackground(e.ClipBounds, selected);
					e.Handled = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure that the field chosen as the first interlinear field is also marked
		/// as an interlinear field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cboFirstInterlinear_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboFirstInterlinear.SelectedIndex > 0)
			{
				SFMarkerMapping mapping = cboFirstInterlinear.SelectedItem as SFMarkerMapping;
				if (!mapping.IsInterlinear)
				{
					mapping.IsInterlinear = true;
					if (m_grid != null)
						m_grid.Refresh();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the editor for the SFM data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			string filter = ResourceHelper.GetString("kstidFileTypeAllExe") +"|" +
				ResourceHelper.GetString("kstidFileTypeAllFiles");

			string editor = PaApp.OpenFileDialog("exe", filter,
				Properties.Resources.kstidSFMEditorCaptionOFD);

			if (!string.IsNullOrEmpty(editor))
				txtEditor.Text = editor.Trim();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the file name with EllipsisPath trimming.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lblFilename_Paint(object sender, PaintEventArgs e)
		{
			using (StringFormat sf = STUtils.GetStringFormat(false))
			{
				e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
				sf.Trimming = StringTrimming.EllipsisPath;
				e.Graphics.DrawString(m_filename, lblFilename.Font, SystemBrushes.ControlText,
					lblFilename.ClientRectangle, sf);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lblFilename_MouseEnter(object sender, EventArgs e)
		{
			Size szPreferred = TextRenderer.MeasureText(m_filename, lblFilename.Font);
			m_tooltip.SetToolTip(lblFilename,
				(lblFilename.Width < szPreferred.Width + 8 ? m_filename : null));
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

				string marker;
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					numLines++;

					// For those bldr beginning with a backslash, strip off the marker and keep it.
					if (line.StartsWith("\\") && !line.StartsWith(PaDataSource.kShoeboxMarker) &&
						!line.StartsWith("\\_Date"))
					{
						marker = line.Split(' ')[0];
						
						if (!markerList.Contains(marker))
							markerList.Add(marker);
					}
				}
			}
			catch (Exception e)
			{
				// TODO: Localize message
				MessageBox.Show("Error reading file: " + e.Message);
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
			gridSampleOutput.Columns["POS"].Visible = rbInterlinearize.Checked;

			RadioButton rb = sender as RadioButton;
			if (rb == null)
				return;

			if (rbNoParse.Checked || rbParseOnlyPhonetic.Checked || rbParseOneToOne.Checked)
			{
				// Set the First Interlinear field to NONE
				cboFirstInterlinear.SelectedIndex = 0;

				// Make all fields NOT interlinear
				foreach (SFMarkerMapping mapping in m_mappings)
				{
					if (mapping.IsInterlinear)
						mapping.IsInterlinear = false;
				}
			}

			if (m_grid != null)
			{
				m_grid.Columns[3].ReadOnly = !rbInterlinearize.Checked;
				m_grid.Refresh();
			}

			// The rest of the code in this method deals with building
			// the appropriate sample portion of the panel.
			rtfSampleInput.Rtf = (rbInterlinearize.Checked ?
				Properties.Resources.kstidInterlinearSampleInput :
				Properties.Resources.kstidSampleInput);

			// Get the unparsed sample output string.
			string sampleInput = rb.Tag as string;

			if (string.IsNullOrEmpty(sampleInput))
				return;

			// Split the sample output string into the pieces that represent rows in the
			// sample grid, clear the grid and add the appropriate number of rows.
			string[] rows = sampleInput.Split("#".ToCharArray());
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
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="configurationParameters"></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
			// TODO:  Add SFDataSourcePropertiesDlg.Init implementation
		}

		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] {this});
		}

		#endregion
	}
}
