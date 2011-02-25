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

		private string m_filename;
		private List<string> m_markersInFile;
		private FieldMappingGrid m_fieldsGrid;
		private IEnumerable<PaField> m_potentialFields;
		private readonly PaDataSource m_datasource;

		#endregion

		#region Construction/Setup
		/// ------------------------------------------------------------------------------------
		public SFDataSourcePropertiesDlg(PaDataSource ds, IEnumerable<PaField> mappedSfmFields)
		{
			InitializeComponent();
			m_datasource = ds;
			Initialize(mappedSfmFields);
		}

		/// ------------------------------------------------------------------------------------
		private void Initialize(IEnumerable<PaField> mappedSfmFields)
		{
			// If the grid is not null, we've already been here.
			if (App.DesignMode)
				return;

			InitializeSomeUIStuff();

			// Merge the project's mapped sfm fields with the default set.
			m_potentialFields = PaField.Merge(mappedSfmFields, PaField.GetDefaultSfmFields());

			m_filename = m_datasource.SourceFile;
			m_tooltip.SetToolTip(pnlSrcFileHdg, m_filename);
			ReadSfmFile();

			InitializeBottomPanel();
			InitializeToolboxSortFieldControls();
			InitializeFieldMappingsGrid();
			InitializeFirstInterlinearCombo();

			cboRecordMarkers.Items.AddRange(m_markersInFile.ToArray());
			var marker = m_markersInFile.SingleOrDefault(m => m == m_datasource.SfmRecordMarker);
			cboRecordMarkers.SelectedItem = (marker ?? m_markersInFile[0]);
		}

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
			cboFirstInterlinear.Items.Add(App.LocalizeString(
				"SFDataSourcePropertiesDlg.UnspecifiedFirstInterlinearFieldItem", "(none)",
				"First item in the list of potential first interlinear fields.",
				App.kLocalizationGroupDialogs));

			cboFirstInterlinear.Items.AddRange(m_markersInFile.ToArray());

			var marker = (m_datasource.FirstInterlinearField == null ? null :
				m_datasource.FieldMappings.SingleOrDefault(m => 
					m.Field != null && m.Field.Name == m_datasource.FirstInterlinearField));

			if (marker == null)
				cboFirstInterlinear.SelectedIndex = 0;
			else
				cboFirstInterlinear.SelectedItem = marker;
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
			m_fieldsGrid = new FieldMappingGrid(m_potentialFields, GetMappingsForGrid(),
				() => App.LocalizeString("SFDataSourcePropertiesDlg.SourceFieldColumnHeadingText", "Map this Marker...", App.kLocalizationGroupDialogs),
				() => App.LocalizeString("SFDataSourcePropertiesDlg.TargetFieldColumnHeadingText", "To this Field", App.kLocalizationGroupDialogs));

			m_fieldsGrid.Dock = DockStyle.Fill;
			pnlMappings.Controls.Add(m_fieldsGrid);
			m_fieldsGrid.BringToFront();
			pnlMappingsHdg.ControlReceivingFocusOnMnemonic = m_fieldsGrid;
			OnStringLocalized(null);
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

		/// ------------------------------------------------------------------------------------
		private IEnumerable<FieldMapping> GetMappingsForGrid()
		{
			bool createOriginalMappings =
				(m_datasource.FieldMappings == null || m_datasource.FieldMappings.Count == 0);

			var defaultParsedFlds = Settings.Default.DefaultParsedSfmFields;

			// Create a list of mappings. If creating a new one, make sure to initialize each one's Field
			// property, if possible, based on the existing project's fields and the default SFM fields.
			return m_markersInFile.Select(mkr =>
			{
				var field = m_potentialFields.SingleOrDefault(f => f.GetPossibleDataSourceFieldNames().Contains(mkr));
				var isParsed = (field != null && defaultParsedFlds.Contains(field.Name));

				if (createOriginalMappings)
					return new FieldMapping(mkr, field, isParsed);

				var mapping = m_datasource.FieldMappings.SingleOrDefault(m => m.NameInDataSource == mkr);
				return mapping ?? new FieldMapping(mkr, null, isParsed);
			}).ToList();
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
				return ShowError(cboRecordMarkers, App.LocalizeString(
					"SFDataSourcePropertiesDlg.MissingRecordMarkerSpecificationMsg",
					"You must specify a record marker to identify the beginning of each record.",
					App.kLocalizationGroupDialogs));
			}

			// Make sure a phonetic mapping specified
			if (!m_fieldsGrid.Mappings.Any(m => m.Field != null && m.Field.Type == FieldType.Phonetic))
			{
			    return ShowError(m_fieldsGrid, App.LocalizeString(
					"SFDataSourcePropertiesDlg.NoMappingsSpecifiedMsg",
			        "You must specify a mapping for the phonetic field.",
					App.kLocalizationGroupDialogs));
			}

			// Make sure no field is mapped more than once.
			if (m_fieldsGrid.GetAreAnyFieldsMappedMultipleTimes())
			{
				return ShowError(m_fieldsGrid, App.LocalizeString(
					"SFDataSourcePropertiesDlg.MultipleMappingsForSingleFieldMsg",
					"Each field may only be mapped once.", App.kLocalizationGroupDialogs));
			}

			// Make sure the field specified as the toolbox sort field is mapped to a marker.
			if (ToolBoxSortField != null && !m_fieldsGrid.GetIsSourceFieldMapped(ToolBoxSortField))
			{
				return ShowError(cboToolboxSortField, App.LocalizeString(
					"SFDataSourcePropertiesDlg.InvalidToolboxSortFieldSpecifiedMsg",
					"The first Toolbox sort field specified was\nnot mapped. It must have a mapping.",
					App.kLocalizationGroupDialogs));
			}
				


			// TODO: Verify is parsed and isinterlinear

			//msg = VerifyInterlinearInfo();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private bool ShowError(Control ctrlToGiveFocus, string msg)
		{
			Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			ctrlToGiveFocus.Focus();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure all the settings for interlinear fields are consistent.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string VerifyInterlinearInfo()
		{
			//HandleFirstInterlinearComboSelectedIndexChanged(null, null);
			
			//var fim = cboFirstInterlinear.SelectedItem as SFMarkerMapping;
			//string firstInterlinField = (fim == null ? null : fim.FieldName);
			string msg = null;
			//int interlinearFieldCount = 0;

			//// Check for unmapped fields that are specified as interlinear fields.
			//foreach (SFMarkerMapping mapping in m_mappings)
			//{
			//    if (mapping.IsInterlinear)
			//    {
			//        interlinearFieldCount++;
			//        if (string.IsNullOrEmpty(mapping.Marker))
			//            msg += mapping.DisplayText + "\n";
			//    }
			//}

			//if (msg != null)
			//{
			//    msg = App.LocalizeString("SFDataSourcePropertiesDlg.NoMappingForInterlinearFieldMsg",
			//        "The following field(s) have been specified as interlinear fields but need to be mapped to markers.\n\n",
			//        App.kLocalizationGroupDialogs);
			//}
			//else if (firstInterlinField == null)
			//{
			//    // Check any fields marked as interlinear but without
			//    // a first interlinear field having been specified too.
			//    if (m_mappings.Any(m => m.IsInterlinear))
			//    {
			//        msg = GetNoFirstInterlinearFieldMsg();
			//        cboFirstInterlinear.Focus();
			//    }
			//}

			//// Check if the first interlinear field was specified unecessarily.
			//if (msg == null && interlinearFieldCount < 2 && firstInterlinField != null)
			//{
			//    msg = App.LocalizeString("SFDataSourcePropertiesDlg.UnecessaryFirstInterlinearFieldMsg",
			//        "Because you have specified the first interlinear field, you must also specify at least one other interlinear field from those that are mapped.",
			//        App.kLocalizationGroupDialogs);
			//}

			//// Check if a first field was selected for the Interlinearize option
			//if (msg == null && rbInterlinearize.Checked && firstInterlinField == null)
			//    msg = GetNoFirstInterlinearFieldMsg();

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
				return (m_fieldsGrid.IsDirty || CurrentParseType != m_datasource.ParseType ||
					cboFirstInterlinear.SelectedItem as string != m_datasource.FirstInterlinearField ||
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
					cboToolboxSortField.SelectedItem as string);
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
			m_datasource.Editor = txtEditor.Text.Trim();
			m_datasource.FieldMappings = m_fieldsGrid.Mappings.Where(m => m.Field != null).ToList();

			// Save the field name associated with the SFM assigned as the Toolbox sort field.
			m_datasource.ToolboxSortField = (m_datasource.Type != DataSourceType.Toolbox ||
				ToolBoxSortField == null ? null : GetPaFieldToolBoxSortFieldIsMappedTo());

			var field = m_fieldsGrid.GetMappedFieldForSourceField(
				cboFirstInterlinear.SelectedItem as string);
			
			m_datasource.FirstInterlinearField = (field == null ? null : field.Name);
			
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
								   where line.StartsWith("\\") && !line.StartsWith("\\_Date") && !line.StartsWith(PaDataSource.kShoeboxMarker)
								   select line.Split(' ')[0]).Distinct().ToList();
			}
			catch (Exception e)
			{
				var msg = App.LocalizeString("SFDataSourcePropertiesDlg.ErrorReadingSourceFileMsg",
					"The following error occurred trying to read the source file '{0}'.\n\n{1}",
					App.kLocalizationGroupDialogs);

				MessageBox.Show(string.Format(msg, e.Message));
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

			if (rbNoParse.Checked || rbParseOnlyPhonetic.Checked || rbParseOneToOne.Checked)
			{
				// Set the First Interlinear field to NONE
				cboFirstInterlinear.SelectedIndex = 0;

				// TODO: Fix this
				// Make all fields NOT interlinear
				//foreach (var mapping in m_mappings.Where(m => m.IsInterlinear))
				//    mapping.IsInterlinear = false;
			}

			m_fieldsGrid.ShowIsParsedColumn(rbParseOneToOne.Checked || rbInterlinearize.Checked);
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

		#endregion

		#region IxCoreColleague Members
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] {this});
		}

		#endregion

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
		protected bool OnStringLocalized(object args)
		{
			try
			{
				var colHdrText = m_fieldsGrid.Columns["tgtfield"].HeaderText;
				var text = App.GetLocalizedString(lblInformation);
				lblInformation.Text = string.Format(text, colHdrText, colHdrText);
			}
	
			catch { }
			
			return false;
		}
	}
}
