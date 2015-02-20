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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
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
	public partial class SFDataSourcePropertiesDlg : OKCancelDlgBase
	{
		#region member variables

		private List<string> m_markersInFile;
		private SfmFieldMappingGrid m_fieldsGrid;
		private readonly IEnumerable<PaField> m_potentialFields;
		private readonly PaDataSource m_datasource;
		private readonly string m_filename;

		#endregion

		#region Construction/Setup
		/// ------------------------------------------------------------------------------------
		public SFDataSourcePropertiesDlg(PaDataSource ds, IEnumerable<PaField> projectFields)
		{
			InitializeComponent();

			// If the grid is not null, we've already been here.
			if (App.DesignMode)
				return;

			m_datasource = ds;
			var possibleSfmFieldNames = Settings.Default.DefaultSfmFields.Cast<string>();
			
			// Merge the project's mapped sfm fields with the default set.
			var mappedSfmFields = m_datasource.FieldMappings.Select(m => m.Field).ToList();
			mappedSfmFields.AddRange(projectFields.Where(f => possibleSfmFieldNames.Contains(f.Name)));
			m_potentialFields = mappedSfmFields.Distinct(new FieldNameComparer());

			m_filename = m_datasource.SourceFile;
			m_tooltip.SetToolTip(pnlSrcFileHdg, m_filename);
			InitializeSomeUIStuff();
			
			ReadSfmFile();

			InitializeBottomPanel();
			InitializeToolboxSortFieldControls();
			InitializeFieldMappingsGrid();
			InitializeFirstInterlinearCombo();

			cboRecordMarkers.Items.AddRange(m_markersInFile.ToArray());
			var marker = m_markersInFile.SingleOrDefault(m => m == m_datasource.SfmRecordMarker);
			cboRecordMarkers.SelectedItem = (marker ?? m_markersInFile[0]);

			if (marker != null)
				cboRecordMarkers.SelectedItem = marker;
			else if (m_markersInFile.Count > 0)
				cboRecordMarkers.SelectedItem = m_markersInFile[0];	}

		/// ------------------------------------------------------------------------------------
		private void InitializeSomeUIStuff()
		{
			// For some reason when I set these values in the designer, I can't open
			// the form in designer again without errors.
			scImport.Panel1MinSize = 125;
			scImport.Panel2MinSize = 125;

			txtFilePreview.Font = App.PhoneticFont;
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
			lblRecordMarker.Font = FontHelper.UIFont;
			cboRecordMarkers.Font = FontHelper.UIFont;
			lblInformation.Font = FontHelper.UIFont;
			gridSampleOutput.Font = new Font(FontHelper.UIFont.FontFamily, 8f);
			gridSampleOutput.BorderStyle = BorderStyle.None;
			pnlMappingsInner.BorderStyle = BorderStyle.None;
			pnlParseHdg.BorderStyle = BorderStyle.None;
			pnlMappingsHdg.BorderStyle = BorderStyle.None;
			pnlSrcFileHdg.BorderStyle = BorderStyle.None;
			pnlSrcFileHdg.TextFormatFlags |= TextFormatFlags.PathEllipsis;
			pnlMappingsInner.DrawOnlyBottomBorder = true;
			txtFilePreview.Text = File.ReadAllText(m_filename);

			rbNoParse.Tag = Settings.Default.SFMNoParseOptionSampleOutput;
			rbParseOneToOne.Tag = Settings.Default.SFMOneToOneParseOptionSampleOutput;
			rbParseOnlyPhonetic.Tag = Settings.Default.SFMPhoneticParseOptionSampleOutput;
			rbInterlinearize.Tag = Settings.Default.SFMInterlinearParseOptionSampleOutput;

			if (!Settings.Default.ShowSFMappingsInformation)
			{
				lblInformation.Visible = false;
				btnInformation.Image = Properties.Resources.InformationShow;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeBottomPanel()
		{
			if (m_datasource.Type != DataSourceType.Toolbox)
			{
				lblToolboxSortField.Visible = false;
				cboToolboxSortField.Visible = false;
				txtEditor.Text = m_datasource.Editor;

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
			cboToolboxSortField.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.SFDataSourcePropertiesDlg.UnspecifiedToolboxSortField", "(none)"));

			cboToolboxSortField.Items.AddRange(m_markersInFile.ToArray());

			int i = 0;
			if (m_datasource.ToolboxSortField != null)
			{
				var tbsf = m_datasource.FieldMappings.SingleOrDefault(m => m.Field.Name == m_datasource.ToolboxSortField);
				if (tbsf != null)
					i = cboToolboxSortField.Items.IndexOf(tbsf.NameInDataSource);
			}

			cboToolboxSortField.SelectedIndex = (i < 0 ? 0 : i);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFirstInterlinearCombo()
		{
			cboFirstInterlinear.Items.Add(LocalizationManager.GetString(
				"DialogBoxes.SFDataSourcePropertiesDlg.UnspecifiedFirstInterlinearFieldItem", "(none)",
				"First item in the list of potential first interlinear fields."));

			cboFirstInterlinear.Items.AddRange(m_markersInFile.ToArray());

			var mapping = (m_datasource.FirstInterlinearField == null ? null :
				m_datasource.FieldMappings.SingleOrDefault(m => 
					m.Field != null && m.Field.Name == m_datasource.FirstInterlinearField));

			if (mapping == null)
				cboFirstInterlinear.SelectedIndex = 0;
			else
				cboFirstInterlinear.SelectedItem = mapping.NameInDataSource;
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

			m_fieldsGrid.IsDirty = false;
		}

		#endregion
		
		#region Mapping Grid Setup
		/// ------------------------------------------------------------------------------------
		private void InitializeFieldMappingsGrid()
		{
			var mappings = m_datasource.FieldMappings.ToList();

			// Add "empty" mappings for all the markers that haven't been mapped to a field.
			foreach (var marker in m_markersInFile.Where(mkr => !mappings.Any(m => m.NameInDataSource == mkr)))
				mappings.Add(new FieldMapping(marker, null, false));

			m_fieldsGrid = new SfmFieldMappingGrid(m_potentialFields, mappings.OrderBy(m => m.NameInDataSource),
				() => LocalizationManager.GetString("DialogBoxes.SFDataSourcePropertiesDlg.SourceFieldColumnHeadingText", "Map this Marker..."),
				() => LocalizationManager.GetString("DialogBoxes.SFDataSourcePropertiesDlg.TargetFieldColumnHeadingText", "To this Field"));

			m_fieldsGrid.Dock = DockStyle.Fill;
			pnlMappings.Controls.Add(m_fieldsGrid);
			m_fieldsGrid.BringToFront();
			pnlMappingsHdg.ControlReceivingFocusOnMnemonic = m_fieldsGrid;
			OnStringsLocalized();
			m_fieldsGrid.ShowFontColumn(false);

			if (Settings.Default.SFDataSourcePropertiesDlgMappingGrid != null)
				Settings.Default.SFDataSourcePropertiesDlgMappingGrid.InitializeGrid(m_fieldsGrid);
			else
			{
				m_fieldsGrid.AutoResizeColumnHeadersHeight();
				m_fieldsGrid.AutoResizeColumns();
				m_fieldsGrid.AutoResizeRows();
				m_fieldsGrid.Columns["tgtfield"].Width += 20;
			}
		}

		#endregion

		#region Overridden Methods when closing form
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.SFDataSourcePropertiesDlgMappingGrid = GridSettings.Create(m_fieldsGrid);
			Settings.Default.SFDataSourcePropertiesDlgSplitLoc = scImport.SplitterDistance;
			Settings.Default.ShowSFMappingsInformation = lblInformation.Visible;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verify everything.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			// Commit pending changes in the grid.
			m_fieldsGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);

			// Make sure the record marker was specified.
			if (cboRecordMarkers.SelectedItem == null)
			{
				return ShowError(cboRecordMarkers, LocalizationManager.GetString(
					"DialogBoxes.SFDataSourcePropertiesDlg.MissingRecordMarkerSpecificationMsg",
					"You must specify a record marker to identify the beginning of each record."));
			}

			// Make sure a phonetic mapping is specified.
			if (!FieldMapping.IsPhoneticMapped(m_fieldsGrid.Mappings, true))
			{
				m_fieldsGrid.Focus();
				return false;
			}

			// Make sure no field is mapped more than once.
			if (m_fieldsGrid.GetAreAnyFieldsMappedMultipleTimes())
			{
				return ShowError(m_fieldsGrid, LocalizationManager.GetString(
					"DialogBoxes.SFDataSourcePropertiesDlg.MultipleMappingsForSingleFieldMsg",
					"Each field may only be mapped once."));
			}

			// Make sure the phonetic field is not mapped more than once.
			if (m_fieldsGrid.GetIsPhoneticMappedMultipleTimes())
			{
				return ShowError(m_fieldsGrid, LocalizationManager.GetString(
					"DialogBoxes.SFDataSourcePropertiesDlg.MultiplePhoneticMappingsMsg",
					"You may only map the phonetic field once.\nA phonetic mapping is specified using the field type."));
			}

			// Make sure the field specified as the toolbox sort field is mapped to a marker.
			if (ToolBoxSortField != null && !m_fieldsGrid.GetIsSourceFieldMapped(ToolBoxSortField))
			{
				return ShowError(cboToolboxSortField, LocalizationManager.GetString(
					"DialogBoxes.SFDataSourcePropertiesDlg.InvalidToolboxSortFieldSpecifiedMsg",
					"The first Toolbox sort field marker specified was\nnot mapped. It must have a mapping."));
			}

			foreach (var mapping in m_fieldsGrid.Mappings.Where(m => PaField.GetIsReservedFieldName(m.Field.Name)))
			{
				return ShowError(m_fieldsGrid, string.Format(LocalizationManager.GetString(
					"DialogBoxes.SFDataSourcePropertiesDlg.InvalidFieldNameSpecifiedMsg",
					"The field name '{0}' is reserved and cannot be used.\nEnter a different name."),
					mapping.Field.DisplayName));
			}
				
			return VerifyInterlinearInfo();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure all the settings for interlinear fields are consistent.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyInterlinearInfo()
		{
			if (!rbInterlinearize.Checked)
				return true;
			
			// Check that the first interlinear field is specified.
			if (FirstInterlinearField == null)
			{
				return ShowError(cboFirstInterlinear,
					LocalizationManager.GetString("DialogBoxes.SFDataSourcePropertiesDlg.NoFirstInterlinearFieldMsg",
					"You must specify the first interlinear field marker."));
			}

			var interlinearFieldCount = m_fieldsGrid.Mappings.Count(m => m.IsInterlinear);

			if (interlinearFieldCount < 2)
			{
				return ShowError(m_fieldsGrid,
					LocalizationManager.GetString("DialogBoxes.SFDataSourcePropertiesDlg.NoInterlinearFieldsMsg",
					"You must specify at least two interlinear fields."));
			}

			var mapping = m_fieldsGrid.Mappings.SingleOrDefault(m => m.Field.Name == FirstInterlinearField);

			if (mapping == null)
			{
				return ShowError(m_fieldsGrid,
					string.Format(LocalizationManager.GetString("DialogBoxes.SFDataSourcePropertiesDlg.NoFirstInterlinearFieldNotMappedMsg",
						"You must specify a mapping for '{0}' because you have specified it as the first interlinear field marker."),
						cboFirstInterlinear.SelectedItem));
			}

			if (!mapping.IsInterlinear)
			{
				return ShowError(m_fieldsGrid,
					LocalizationManager.GetString("DialogBoxes.SFDataSourcePropertiesDlg.FirstInterlinearFieldNotMarkedAsInterlinearMsg",
						"The mapping for your first interlinear field marker must be set to interlinear."));
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private bool ShowError(Control ctrlToGiveFocus, string msg)
		{
			Utils.MsgBox(msg);
			ctrlToGiveFocus.Focus();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get
			{
				return (m_fieldsGrid.IsDirty || CurrentParseType != m_datasource.ParseType ||
					FirstInterlinearField != m_datasource.FirstInterlinearField ||
					ToolBoxSortField != m_datasource.ToolboxSortField ||
					(m_datasource.Type != DataSourceType.Toolbox && txtEditor.Text != m_datasource.Editor));
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
					cboToolboxSortField.SelectedItem as string);
			}
		}

		/// ------------------------------------------------------------------------------------
		private string FirstInterlinearField
		{
			get
			{
				var field = m_fieldsGrid.GetMappedFieldForSourceField(
					cboFirstInterlinear.SelectedItem as string);

				return (field == null ? null : field.Name);
			}
		}

		/// ------------------------------------------------------------------------------------
		private string GetPaFieldToolBoxSortFieldIsMappedTo()
		{
			var mapping = m_datasource.FieldMappings.SingleOrDefault(m => m.NameInDataSource == ToolBoxSortField);
			if (mapping == null || mapping.Field == null)
				return null;

			return mapping.Field.Name;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			m_datasource.ParseType = CurrentParseType;
			m_datasource.SfmRecordMarker = cboRecordMarkers.SelectedItem as string;
			m_datasource.FirstInterlinearField = FirstInterlinearField;
			m_datasource.Editor = txtEditor.Text.Trim();
			m_datasource.FieldMappings = m_fieldsGrid.Mappings.Where(m => m.Field != null).ToList();

			// Save the field name associated with the SFM assigned as the Toolbox sort field.
			m_datasource.ToolboxSortField = (m_datasource.Type != DataSourceType.Toolbox ||
				ToolBoxSortField == null ? null : GetPaFieldToolBoxSortFieldIsMappedTo());

			return true;
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure that the field chosen as the first interlinear field is also marked
		/// as an interlinear field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFirstInterlinearComboSelectedIndexChanged(object sender, EventArgs e)
		{
			m_fieldsGrid.MarkSourceFieldAsInterlinear(cboFirstInterlinear.SelectedItem as string);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the editor for the SFM data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleBrowseClick(object sender, EventArgs e)
		{
			var caption = LocalizationManager.GetString(
				"DialogBoxes.SFDataSourcePropertiesDlg.BrowseForSFMEditorDialogCaption",
				"Standard Format Data Source Editor");

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
		/// ------------------------------------------------------------------------------------
		protected void ReadSfmFile()
		{
			try 
			{
				txtFilePreview.Text = File.ReadAllText(m_filename);
				var allLines = File.ReadAllLines(m_filename);
				m_datasource.TotalLinesInFile = allLines.Length;

				// Go through all lines that start with backslashes, excluding
				// the ones used to identify a Shoebox/Toolbox file.
				m_markersInFile = (from line in allLines
                                   where line.StartsWith("\\", StringComparison.Ordinal) && !line.StartsWith("\\_Date", StringComparison.Ordinal) && !line.StartsWith(PaDataSource.kShoeboxMarker)
								   select line.Split(' ')[0]).Distinct().ToList();
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.SFDataSourcePropertiesDlg.ErrorReadingSourceFileMsg",
					"The following error occurred trying to read the source file '{0}'.\n\n{1}");

				App.NotifyUserOfProblem(e, msg, m_filename);
			}
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

			if (rbNoParse.Checked)
			{
				m_fieldsGrid.SetPhoneticAsOnlyParsedField();
				cboFirstInterlinear.SelectedIndex = 0;
			}
			else if (rbParseOnlyPhonetic.Checked)
			{
				m_fieldsGrid.SetPhoneticAsOnlyParsedField();
				cboFirstInterlinear.SelectedIndex = 0;
			}
			else if (rbParseOneToOne.Checked)
			{
				m_fieldsGrid.SetDefaultParsedFlags();
				cboFirstInterlinear.SelectedIndex = 0;
			}
			else
				m_fieldsGrid.SetPhoneticAsOnlyParsedField();

			m_fieldsGrid.ShowIsParsedColumn(rbParseOneToOne.Checked);
			m_fieldsGrid.ShowIsInterlinearColumn(rbInterlinearize.Checked);

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

		/// ------------------------------------------------------------------------------------
		private void HandleInformationButtonClick(object sender, EventArgs e)
		{
			lblInformation.Visible = !lblInformation.Visible;

			btnInformation.Image = (lblInformation.Visible ?
				Properties.Resources.InformationHide : Properties.Resources.InformationShow);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRightSplitterMoved(object sender, SplitterEventArgs e)
		{
			if (lblInformation.Visible)
			{
				// This seems to be necessary or sometimes the panel's owned table layout
				// panel doesn't adjust its height to accommodate the info. label.
				pnlMappingsInner.LayoutEngine.Layout(pnlMappings,
					new LayoutEventArgs(pnlMappingsInner, "Width"));
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnStringsLocalized()
		{
			try
			{
				var colHdrText = m_fieldsGrid.Columns["tgtfield"].HeaderText;
				var text = LocalizationManager.GetStringForObject(lblInformation, lblInformation.Text);
				lblInformation.Text = string.Format(text, colHdrText, colHdrText);
			}

			catch { }

			base.OnStringsLocalized();
		}

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Escape:
                    {
                        this.Close();
                        return true;
                    }
                case Keys.Control | Keys.Tab:
                    {
                        return true;
                    }
                case Keys.Control | Keys.Shift | Keys.Tab:
                    {
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref message, keys);
        }

		#endregion
	}
}
