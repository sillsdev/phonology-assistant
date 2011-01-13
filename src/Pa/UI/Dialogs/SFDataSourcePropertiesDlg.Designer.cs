using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using SilTools.Controls;

namespace SIL.Pa.UI.Dialogs
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for SFDataSourcePropertiesDlg.
	/// </summary>
	/// --------------------------------------------------------------------------------
	public partial class SFDataSourcePropertiesDlg
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolTip m_tooltip;
		private SplitContainer scImport;
		private TextBox txtFilePreview;
		
		protected override void Dispose( bool disposing )
		{
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SFDataSourcePropertiesDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.cboToolboxSortField = new System.Windows.Forms.ComboBox();
			this.txtEditor = new System.Windows.Forms.TextBox();
			this.scImport = new System.Windows.Forms.SplitContainer();
			this.pnlMappings = new SilTools.Controls.SilPanel();
			this.lblEditor = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.lblToolboxSortField = new System.Windows.Forms.Label();
			this.pnlMappingsHdg = new SilTools.Controls.SilGradientPanel();
			this.pnlSrcFile = new SilTools.Controls.SilPanel();
			this.txtFilePreview = new System.Windows.Forms.TextBox();
			this.pnlSrcFileHdg = new SilTools.Controls.SilGradientPanel();
			this.lblFilename = new System.Windows.Forms.Label();
			this.cboFirstInterlinear = new System.Windows.Forms.ComboBox();
			this.lblFirstInterlinear = new System.Windows.Forms.Label();
			this.pnlParseType = new SilTools.Controls.SilPanel();
			this.gridSampleOutput = new System.Windows.Forms.DataGridView();
			this.Phonetic = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Gloss = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.POS = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlParseHdg = new SilTools.Controls.SilGradientPanel();
			this.pnlSampleInput = new SilTools.Controls.SilPanel();
			this.rtfSampleInput = new System.Windows.Forms.RichTextBox();
			this.rbParseOneToOne = new System.Windows.Forms.RadioButton();
			this.rbNoParse = new System.Windows.Forms.RadioButton();
			this.rbInterlinearize = new System.Windows.Forms.RadioButton();
			this.lblParseType = new System.Windows.Forms.Label();
			this.rbParseOnlyPhonetic = new System.Windows.Forms.RadioButton();
			this.lblSampleOutput = new System.Windows.Forms.Label();
			this.lblSampleInput = new System.Windows.Forms.Label();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.scImport.Panel1.SuspendLayout();
			this.scImport.Panel2.SuspendLayout();
			this.scImport.SuspendLayout();
			this.pnlMappings.SuspendLayout();
			this.pnlSrcFile.SuspendLayout();
			this.pnlSrcFileHdg.SuspendLayout();
			this.pnlParseType.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridSampleOutput)).BeginInit();
			this.pnlSampleInput.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// m_tooltip
			// 
			this.m_tooltip.AutoPopDelay = 9000;
			this.m_tooltip.InitialDelay = 500;
			this.m_tooltip.ReshowDelay = 100;
			this.m_tooltip.ShowAlways = true;
			// 
			// cboToolboxSortField
			// 
			resources.ApplyResources(this.cboToolboxSortField, "cboToolboxSortField");
			this.cboToolboxSortField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboToolboxSortField.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboToolboxSortField, "This is used for jumping to the appropriate Toolbox record.");
			this.locExtender.SetLocalizationComment(this.cboToolboxSortField, null);
			this.locExtender.SetLocalizationPriority(this.cboToolboxSortField, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.cboToolboxSortField, "SFDataSourcePropertiesDlg.cboToolboxSortField");
			this.cboToolboxSortField.Name = "cboToolboxSortField";
			this.cboToolboxSortField.Sorted = true;
			// 
			// txtEditor
			// 
			resources.ApplyResources(this.txtEditor, "txtEditor");
			this.locExtender.SetLocalizableToolTip(this.txtEditor, "This is the application used to edit the data source file.");
			this.locExtender.SetLocalizationComment(this.txtEditor, "Text box in which to specify the editor for the data source in the standard forma" +
					"t data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.txtEditor, "SFDataSourcePropertiesDlg.txtEditor");
			this.txtEditor.Name = "txtEditor";
			// 
			// scImport
			// 
			resources.ApplyResources(this.scImport, "scImport");
			this.scImport.Name = "scImport";
			// 
			// scImport.Panel1
			// 
			this.scImport.Panel1.Controls.Add(this.pnlMappings);
			// 
			// scImport.Panel2
			// 
			this.scImport.Panel2.Controls.Add(this.pnlSrcFile);
			this.scImport.TabStop = false;
			// 
			// pnlMappings
			// 
			this.pnlMappings.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlMappings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMappings.ClipTextForChildControls = true;
			this.pnlMappings.ControlReceivingFocusOnMnemonic = null;
			this.pnlMappings.Controls.Add(this.lblEditor);
			this.pnlMappings.Controls.Add(this.txtEditor);
			this.pnlMappings.Controls.Add(this.btnBrowse);
			this.pnlMappings.Controls.Add(this.cboToolboxSortField);
			this.pnlMappings.Controls.Add(this.lblToolboxSortField);
			this.pnlMappings.Controls.Add(this.pnlMappingsHdg);
			resources.ApplyResources(this.pnlMappings, "pnlMappings");
			this.pnlMappings.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlMappings, null);
			this.locExtender.SetLocalizationComment(this.pnlMappings, null);
			this.locExtender.SetLocalizationPriority(this.pnlMappings, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlMappings, "SFDataSourcePropertiesDlg.pnlMappings");
			this.pnlMappings.MnemonicGeneratesClick = false;
			this.pnlMappings.Name = "pnlMappings";
			this.pnlMappings.PaintExplorerBarBackground = false;
			// 
			// lblEditor
			// 
			resources.ApplyResources(this.lblEditor, "lblEditor");
			this.lblEditor.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblEditor, null);
			this.locExtender.SetLocalizationComment(this.lblEditor, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblEditor, "SFDataSourcePropertiesDlg.lblEditor");
			this.lblEditor.Name = "lblEditor";
			// 
			// btnBrowse
			// 
			resources.ApplyResources(this.btnBrowse, "btnBrowse");
			this.locExtender.SetLocalizableToolTip(this.btnBrowse, null);
			this.locExtender.SetLocalizationComment(this.btnBrowse, "Button for browsing to an SFM editor in the standard format data source propertie" +
					"s dialog box.");
			this.locExtender.SetLocalizationPriority(this.btnBrowse, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnBrowse, "Localized in base class");
			this.btnBrowse.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// lblToolboxSortField
			// 
			resources.ApplyResources(this.lblToolboxSortField, "lblToolboxSortField");
			this.lblToolboxSortField.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblToolboxSortField, null);
			this.locExtender.SetLocalizationComment(this.lblToolboxSortField, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblToolboxSortField, "SFDataSourcePropertiesDlg.lblToolboxSortField");
			this.lblToolboxSortField.Name = "lblToolboxSortField";
			// 
			// pnlMappingsHdg
			// 
			this.pnlMappingsHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlMappingsHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMappingsHdg.ClipTextForChildControls = true;
			this.pnlMappingsHdg.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlMappingsHdg, "pnlMappingsHdg");
			this.pnlMappingsHdg.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlMappingsHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlMappingsHdg, "Heading above the field mappings list in the standard format data source properti" +
					"es dialog box.");
			this.locExtender.SetLocalizingId(this.pnlMappingsHdg, "SFDataSourcePropertiesDlg.pnlMappingsHdg");
			this.pnlMappingsHdg.MakeDark = false;
			this.pnlMappingsHdg.MnemonicGeneratesClick = false;
			this.pnlMappingsHdg.Name = "pnlMappingsHdg";
			this.pnlMappingsHdg.PaintExplorerBarBackground = false;
			// 
			// pnlSrcFile
			// 
			this.pnlSrcFile.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSrcFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSrcFile.ClipTextForChildControls = true;
			this.pnlSrcFile.ControlReceivingFocusOnMnemonic = null;
			this.pnlSrcFile.Controls.Add(this.txtFilePreview);
			this.pnlSrcFile.Controls.Add(this.pnlSrcFileHdg);
			resources.ApplyResources(this.pnlSrcFile, "pnlSrcFile");
			this.pnlSrcFile.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlSrcFile, null);
			this.locExtender.SetLocalizationComment(this.pnlSrcFile, null);
			this.locExtender.SetLocalizationPriority(this.pnlSrcFile, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSrcFile, "SFDataSourcePropertiesDlg.pnlSrcFile");
			this.pnlSrcFile.MnemonicGeneratesClick = false;
			this.pnlSrcFile.Name = "pnlSrcFile";
			this.pnlSrcFile.PaintExplorerBarBackground = false;
			// 
			// txtFilePreview
			// 
			this.txtFilePreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.txtFilePreview, "txtFilePreview");
			this.locExtender.SetLocalizableToolTip(this.txtFilePreview, null);
			this.locExtender.SetLocalizationComment(this.txtFilePreview, null);
			this.locExtender.SetLocalizationPriority(this.txtFilePreview, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtFilePreview, "SFDataSourcePropertiesDlg.txtFilePreview");
			this.txtFilePreview.Name = "txtFilePreview";
			this.txtFilePreview.ReadOnly = true;
			this.txtFilePreview.TabStop = false;
			// 
			// pnlSrcFileHdg
			// 
			this.pnlSrcFileHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSrcFileHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSrcFileHdg.ClipTextForChildControls = true;
			this.pnlSrcFileHdg.ControlReceivingFocusOnMnemonic = null;
			this.pnlSrcFileHdg.Controls.Add(this.lblFilename);
			resources.ApplyResources(this.pnlSrcFileHdg, "pnlSrcFileHdg");
			this.pnlSrcFileHdg.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlSrcFileHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlSrcFileHdg, "Heading above the contents of the data source file in the standard format data so" +
					"urce properties dialog box.");
			this.locExtender.SetLocalizingId(this.pnlSrcFileHdg, "SFDataSourcePropertiesDlg.pnlSrcFileHdg");
			this.pnlSrcFileHdg.MakeDark = false;
			this.pnlSrcFileHdg.MnemonicGeneratesClick = false;
			this.pnlSrcFileHdg.Name = "pnlSrcFileHdg";
			this.pnlSrcFileHdg.PaintExplorerBarBackground = false;
			// 
			// lblFilename
			// 
			resources.ApplyResources(this.lblFilename, "lblFilename");
			this.lblFilename.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblFilename, null);
			this.locExtender.SetLocalizationComment(this.lblFilename, global::SIL.Pa.ResourceStuff.PaTMStrings.kstidExportAsToolTip);
			this.locExtender.SetLocalizationPriority(this.lblFilename, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblFilename, "SFDataSourcePropertiesDlg.lblFilename");
			this.lblFilename.Name = "lblFilename";
			this.lblFilename.Paint += new System.Windows.Forms.PaintEventHandler(this.lblFilename_Paint);
			this.lblFilename.MouseEnter += new System.EventHandler(this.lblFilename_MouseEnter);
			// 
			// cboFirstInterlinear
			// 
			this.cboFirstInterlinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.cboFirstInterlinear, "cboFirstInterlinear");
			this.cboFirstInterlinear.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboFirstInterlinear, null);
			this.locExtender.SetLocalizationComment(this.cboFirstInterlinear, null);
			this.locExtender.SetLocalizationPriority(this.cboFirstInterlinear, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cboFirstInterlinear, "SFDataSourcePropertiesDlg.cboFirstInterlinear");
			this.cboFirstInterlinear.Name = "cboFirstInterlinear";
			this.cboFirstInterlinear.SelectedIndexChanged += new System.EventHandler(this.cboFirstInterlinear_SelectedIndexChanged);
			// 
			// lblFirstInterlinear
			// 
			resources.ApplyResources(this.lblFirstInterlinear, "lblFirstInterlinear");
			this.lblFirstInterlinear.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblFirstInterlinear, null);
			this.locExtender.SetLocalizationComment(this.lblFirstInterlinear, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblFirstInterlinear, "SFDataSourcePropertiesDlg.lblFirstInterlinear");
			this.lblFirstInterlinear.Name = "lblFirstInterlinear";
			// 
			// pnlParseType
			// 
			this.pnlParseType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlParseType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlParseType.ClipTextForChildControls = true;
			this.pnlParseType.ControlReceivingFocusOnMnemonic = null;
			this.pnlParseType.Controls.Add(this.gridSampleOutput);
			this.pnlParseType.Controls.Add(this.pnlParseHdg);
			this.pnlParseType.Controls.Add(this.pnlSampleInput);
			this.pnlParseType.Controls.Add(this.cboFirstInterlinear);
			this.pnlParseType.Controls.Add(this.rbParseOneToOne);
			this.pnlParseType.Controls.Add(this.rbNoParse);
			this.pnlParseType.Controls.Add(this.rbInterlinearize);
			this.pnlParseType.Controls.Add(this.lblParseType);
			this.pnlParseType.Controls.Add(this.rbParseOnlyPhonetic);
			this.pnlParseType.Controls.Add(this.lblFirstInterlinear);
			this.pnlParseType.Controls.Add(this.lblSampleOutput);
			this.pnlParseType.Controls.Add(this.lblSampleInput);
			resources.ApplyResources(this.pnlParseType, "pnlParseType");
			this.pnlParseType.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlParseType, null);
			this.locExtender.SetLocalizationComment(this.pnlParseType, null);
			this.locExtender.SetLocalizationPriority(this.pnlParseType, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlParseType, "SFDataSourcePropertiesDlg.pnlParseType");
			this.pnlParseType.MnemonicGeneratesClick = false;
			this.pnlParseType.Name = "pnlParseType";
			this.pnlParseType.PaintExplorerBarBackground = false;
			// 
			// gridSampleOutput
			// 
			this.gridSampleOutput.AllowUserToAddRows = false;
			this.gridSampleOutput.AllowUserToDeleteRows = false;
			this.gridSampleOutput.AllowUserToResizeColumns = false;
			this.gridSampleOutput.AllowUserToResizeRows = false;
			resources.ApplyResources(this.gridSampleOutput, "gridSampleOutput");
			this.gridSampleOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.gridSampleOutput.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.gridSampleOutput.BackgroundColor = System.Drawing.SystemColors.Window;
			this.gridSampleOutput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridSampleOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridSampleOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Phonetic,
            this.Gloss,
            this.POS});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gridSampleOutput.DefaultCellStyle = dataGridViewCellStyle1;
			this.locExtender.SetLocalizableToolTip(this.gridSampleOutput, null);
			this.locExtender.SetLocalizationComment(this.gridSampleOutput, "Grid showing sample output in the standard form data source properties dialog box" +
					".");
			this.locExtender.SetLocalizingId(this.gridSampleOutput, "SFDataSourcePropertiesDlg.gridSampleOutput");
			this.gridSampleOutput.MultiSelect = false;
			this.gridSampleOutput.Name = "gridSampleOutput";
			this.gridSampleOutput.RowHeadersVisible = false;
			this.gridSampleOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.gridSampleOutput.TabStop = false;
			// 
			// Phonetic
			// 
			resources.ApplyResources(this.Phonetic, "Phonetic");
			this.Phonetic.Name = "Phonetic";
			this.Phonetic.ReadOnly = true;
			this.Phonetic.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Gloss
			// 
			resources.ApplyResources(this.Gloss, "Gloss");
			this.Gloss.Name = "Gloss";
			this.Gloss.ReadOnly = true;
			this.Gloss.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// POS
			// 
			this.POS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.POS, "POS");
			this.POS.Name = "POS";
			this.POS.ReadOnly = true;
			this.POS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// pnlParseHdg
			// 
			this.pnlParseHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlParseHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlParseHdg.ClipTextForChildControls = true;
			this.pnlParseHdg.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlParseHdg, "pnlParseHdg");
			this.pnlParseHdg.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlParseHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlParseHdg, "Heading above the parsing options in the standard format data source properties d" +
					"ialog box.");
			this.locExtender.SetLocalizingId(this.pnlParseHdg, "SFDataSourcePropertiesDlg.pnlParseHdg");
			this.pnlParseHdg.MakeDark = false;
			this.pnlParseHdg.MnemonicGeneratesClick = false;
			this.pnlParseHdg.Name = "pnlParseHdg";
			this.pnlParseHdg.PaintExplorerBarBackground = false;
			// 
			// pnlSampleInput
			// 
			resources.ApplyResources(this.pnlSampleInput, "pnlSampleInput");
			this.pnlSampleInput.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSampleInput.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSampleInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSampleInput.ClipTextForChildControls = true;
			this.pnlSampleInput.ControlReceivingFocusOnMnemonic = null;
			this.pnlSampleInput.Controls.Add(this.rtfSampleInput);
			this.pnlSampleInput.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlSampleInput, null);
			this.locExtender.SetLocalizationComment(this.pnlSampleInput, null);
			this.locExtender.SetLocalizationPriority(this.pnlSampleInput, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSampleInput, "SFDataSourcePropertiesDlg.pnlSampleInput");
			this.pnlSampleInput.MnemonicGeneratesClick = false;
			this.pnlSampleInput.Name = "pnlSampleInput";
			this.pnlSampleInput.PaintExplorerBarBackground = false;
			// 
			// rtfSampleInput
			// 
			this.rtfSampleInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtfSampleInput, "rtfSampleInput");
			this.rtfSampleInput.Name = "rtfSampleInput";
			this.rtfSampleInput.TabStop = false;
			this.rtfSampleInput.Text = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidExportAsToolTip;
			// 
			// rbParseOneToOne
			// 
			this.rbParseOneToOne.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.rbParseOneToOne, "rbParseOneToOne");
			this.locExtender.SetLocalizableToolTip(this.rbParseOneToOne, null);
			this.locExtender.SetLocalizationComment(this.rbParseOneToOne, "Parsing option radio button in standard format data source properties dialog box." +
					"");
			this.locExtender.SetLocalizingId(this.rbParseOneToOne, "SFDataSourcePropertiesDlg.rbParseOneToOne");
			this.rbParseOneToOne.Name = "rbParseOneToOne";
			this.rbParseOneToOne.TabStop = true;
			this.rbParseOneToOne.UseVisualStyleBackColor = false;
			// 
			// rbNoParse
			// 
			this.rbNoParse.AutoEllipsis = true;
			this.rbNoParse.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.rbNoParse, "rbNoParse");
			this.locExtender.SetLocalizableToolTip(this.rbNoParse, null);
			this.locExtender.SetLocalizationComment(this.rbNoParse, "Parsing option radio button in standard format data source properties dialog box." +
					"");
			this.locExtender.SetLocalizingId(this.rbNoParse, "SFDataSourcePropertiesDlg.rbNoParse");
			this.rbNoParse.Name = "rbNoParse";
			this.rbNoParse.TabStop = true;
			this.rbNoParse.UseVisualStyleBackColor = false;
			// 
			// rbInterlinearize
			// 
			this.rbInterlinearize.AutoEllipsis = true;
			this.rbInterlinearize.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.rbInterlinearize, "rbInterlinearize");
			this.locExtender.SetLocalizableToolTip(this.rbInterlinearize, null);
			this.locExtender.SetLocalizationComment(this.rbInterlinearize, "Parsing option radio button in standard format data source properties dialog box." +
					"");
			this.locExtender.SetLocalizingId(this.rbInterlinearize, "SFDataSourcePropertiesDlg.rbInterlinearize");
			this.rbInterlinearize.Name = "rbInterlinearize";
			this.rbInterlinearize.TabStop = true;
			this.rbInterlinearize.UseVisualStyleBackColor = false;
			// 
			// lblParseType
			// 
			this.lblParseType.AutoEllipsis = true;
			this.lblParseType.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblParseType, "lblParseType");
			this.locExtender.SetLocalizableToolTip(this.lblParseType, null);
			this.locExtender.SetLocalizationComment(this.lblParseType, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblParseType, "SFDataSourcePropertiesDlg.lblParseType");
			this.lblParseType.Name = "lblParseType";
			// 
			// rbParseOnlyPhonetic
			// 
			this.rbParseOnlyPhonetic.AutoEllipsis = true;
			this.rbParseOnlyPhonetic.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.rbParseOnlyPhonetic, "rbParseOnlyPhonetic");
			this.locExtender.SetLocalizableToolTip(this.rbParseOnlyPhonetic, null);
			this.locExtender.SetLocalizationComment(this.rbParseOnlyPhonetic, "Parsing option radio button in standard format data source properties dialog box." +
					"");
			this.locExtender.SetLocalizingId(this.rbParseOnlyPhonetic, "SFDataSourcePropertiesDlg.rbParseOnlyPhonetic");
			this.rbParseOnlyPhonetic.Name = "rbParseOnlyPhonetic";
			this.rbParseOnlyPhonetic.TabStop = true;
			this.rbParseOnlyPhonetic.UseVisualStyleBackColor = false;
			// 
			// lblSampleOutput
			// 
			resources.ApplyResources(this.lblSampleOutput, "lblSampleOutput");
			this.lblSampleOutput.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblSampleOutput, null);
			this.locExtender.SetLocalizationComment(this.lblSampleOutput, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblSampleOutput, "SFDataSourcePropertiesDlg.lblSampleOutput");
			this.lblSampleOutput.Name = "lblSampleOutput";
			// 
			// lblSampleInput
			// 
			resources.ApplyResources(this.lblSampleInput, "lblSampleInput");
			this.lblSampleInput.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblSampleInput, null);
			this.locExtender.SetLocalizationComment(this.lblSampleInput, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblSampleInput, "SFDataSourcePropertiesDlg.lblSampleInput");
			this.lblSampleInput.Name = "lblSampleInput";
			// 
			// splitOuter
			// 
			this.splitOuter.CausesValidation = false;
			resources.ApplyResources(this.splitOuter, "splitOuter");
			this.splitOuter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.pnlParseType);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.scImport);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SFDataSourcePropertiesDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.splitOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SFDataSourcePropertiesDlg.WindowTitle");
			this.Name = "SFDataSourcePropertiesDlg";
			this.Controls.SetChildIndex(this.splitOuter, 0);
			this.scImport.Panel1.ResumeLayout(false);
			this.scImport.Panel2.ResumeLayout(false);
			this.scImport.ResumeLayout(false);
			this.pnlMappings.ResumeLayout(false);
			this.pnlMappings.PerformLayout();
			this.pnlSrcFile.ResumeLayout(false);
			this.pnlSrcFile.PerformLayout();
			this.pnlSrcFileHdg.ResumeLayout(false);
			this.pnlParseType.ResumeLayout(false);
			this.pnlParseType.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridSampleOutput)).EndInit();
			this.pnlSampleInput.ResumeLayout(false);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private Label lblFilename;
		private Label lblFirstInterlinear;
		private ComboBox cboFirstInterlinear;
		private SilPanel pnlParseType;
		private Label lblParseType;
		private SplitContainer splitOuter;
		private RadioButton rbParseOneToOne;
		private RadioButton rbNoParse;
		private RadioButton rbInterlinearize;
		private RadioButton rbParseOnlyPhonetic;
		private RichTextBox rtfSampleInput;
		private SilPanel pnlSampleInput;
		private DataGridView gridSampleOutput;
		private Label lblSampleInput;
		private Label lblSampleOutput;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn Phonetic;
		private DataGridViewTextBoxColumn Gloss;
		private DataGridViewTextBoxColumn POS;
		private Label lblToolboxSortField;
		private ComboBox cboToolboxSortField;
		private Label lblEditor;
		private TextBox txtEditor;
		private Button btnBrowse;
		private SilGradientPanel pnlParseHdg;
		private SilGradientPanel pnlMappingsHdg;
		private SilGradientPanel pnlSrcFileHdg;
		private SilPanel pnlMappings;
		private SilPanel pnlSrcFile;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
