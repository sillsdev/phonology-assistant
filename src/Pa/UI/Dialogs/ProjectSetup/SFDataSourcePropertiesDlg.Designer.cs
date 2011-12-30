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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.scImport = new System.Windows.Forms.SplitContainer();
			this.pnlMappings = new SilTools.Controls.SilPanel();
			this.tblLayoutEditor = new System.Windows.Forms.TableLayoutPanel();
			this.lblEditor = new System.Windows.Forms.Label();
			this.txtEditor = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.pnlMappingsInner = new SilTools.Controls.SilPanel();
			this.tblLayoutToolBoxSortField = new System.Windows.Forms.TableLayoutPanel();
			this.lblInformation = new SilTools.Controls.AutoHeightLabel();
			this.cboToolboxSortField = new System.Windows.Forms.ComboBox();
			this.cboRecordMarkers = new System.Windows.Forms.ComboBox();
			this.lblToolboxSortField = new System.Windows.Forms.Label();
			this.lblRecordMarker = new System.Windows.Forms.Label();
			this.pnlMappingsHdg = new SilTools.Controls.SilGradientPanel();
			this.btnInformation = new SilTools.Controls.XButton();
			this.pnlSrcFile = new SilTools.Controls.SilPanel();
			this.txtFilePreview = new System.Windows.Forms.TextBox();
			this.pnlSrcFileHdg = new SilTools.Controls.SilGradientPanel();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.pnlParseType = new SilTools.Controls.SilPanel();
			this.tblLayoutParseOptions = new System.Windows.Forms.TableLayoutPanel();
			this.lblParseType = new SilTools.Controls.AutoHeightLabel();
			this.rbInterlinearize = new SilTools.Controls.AutoHeighRadioButton();
			this.rbParseOneToOne = new SilTools.Controls.AutoHeighRadioButton();
			this.lblSampleOutput = new System.Windows.Forms.Label();
			this.rbParseOnlyPhonetic = new SilTools.Controls.AutoHeighRadioButton();
			this.rbNoParse = new SilTools.Controls.AutoHeighRadioButton();
			this.pnlSampleInput = new SilTools.Controls.SilPanel();
			this.rtfSampleInput = new System.Windows.Forms.RichTextBox();
			this.cboFirstInterlinear = new System.Windows.Forms.ComboBox();
			this.lblSampleInput = new System.Windows.Forms.Label();
			this.lblFirstInterlinear = new System.Windows.Forms.Label();
			this.pnlSampeOutput = new SilTools.Controls.SilPanel();
			this.gridSampleOutput = new SilTools.SilGrid();
			this.SampleOutputPhoneticColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SampleOutputGlossColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SampleOutputPartOfSpeechColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlParseHdg = new SilTools.Controls.SilGradientPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.scImport.Panel1.SuspendLayout();
			this.scImport.Panel2.SuspendLayout();
			this.scImport.SuspendLayout();
			this.pnlMappings.SuspendLayout();
			this.tblLayoutEditor.SuspendLayout();
			this.pnlMappingsInner.SuspendLayout();
			this.tblLayoutToolBoxSortField.SuspendLayout();
			this.pnlMappingsHdg.SuspendLayout();
			this.pnlSrcFile.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.pnlParseType.SuspendLayout();
			this.tblLayoutParseOptions.SuspendLayout();
			this.pnlSampleInput.SuspendLayout();
			this.pnlSampeOutput.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridSampleOutput)).BeginInit();
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
			// scImport
			// 
			this.scImport.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scImport.Location = new System.Drawing.Point(0, 0);
			this.scImport.Name = "scImport";
			// 
			// scImport.Panel1
			// 
			this.scImport.Panel1.Controls.Add(this.pnlMappings);
			// 
			// scImport.Panel2
			// 
			this.scImport.Panel2.Controls.Add(this.pnlSrcFile);
			this.scImport.Size = new System.Drawing.Size(462, 463);
			this.scImport.SplitterDistance = 278;
			this.scImport.SplitterWidth = 6;
			this.scImport.TabIndex = 0;
			this.scImport.TabStop = false;
			this.scImport.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.HandleRightSplitterMoved);
			// 
			// pnlMappings
			// 
			this.pnlMappings.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlMappings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMappings.ClipTextForChildControls = true;
			this.pnlMappings.ControlReceivingFocusOnMnemonic = null;
			this.pnlMappings.Controls.Add(this.tblLayoutEditor);
			this.pnlMappings.Controls.Add(this.pnlMappingsInner);
			this.pnlMappings.Controls.Add(this.pnlMappingsHdg);
			this.pnlMappings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMappings.DoubleBuffered = false;
			this.pnlMappings.DrawOnlyBottomBorder = false;
			this.pnlMappings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlMappings.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlMappings, null);
			this.locExtender.SetLocalizationComment(this.pnlMappings, null);
			this.locExtender.SetLocalizationPriority(this.pnlMappings, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlMappings, "SFDataSourcePropertiesDlg.pnlMappings");
			this.pnlMappings.Location = new System.Drawing.Point(0, 0);
			this.pnlMappings.MnemonicGeneratesClick = false;
			this.pnlMappings.Name = "pnlMappings";
			this.pnlMappings.PaintExplorerBarBackground = false;
			this.pnlMappings.Size = new System.Drawing.Size(278, 463);
			this.pnlMappings.TabIndex = 0;
			// 
			// tblLayoutEditor
			// 
			this.tblLayoutEditor.ColumnCount = 3;
			this.tblLayoutEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutEditor.Controls.Add(this.lblEditor, 0, 0);
			this.tblLayoutEditor.Controls.Add(this.txtEditor, 1, 0);
			this.tblLayoutEditor.Controls.Add(this.btnBrowse, 2, 0);
			this.tblLayoutEditor.Location = new System.Drawing.Point(42, 265);
			this.tblLayoutEditor.Name = "tblLayoutEditor";
			this.tblLayoutEditor.RowCount = 1;
			this.tblLayoutEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutEditor.Size = new System.Drawing.Size(200, 73);
			this.tblLayoutEditor.TabIndex = 1;
			this.tblLayoutEditor.Visible = false;
			// 
			// lblEditor
			// 
			this.lblEditor.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblEditor.AutoSize = true;
			this.lblEditor.BackColor = System.Drawing.Color.Transparent;
			this.lblEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblEditor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblEditor, null);
			this.locExtender.SetLocalizationComment(this.lblEditor, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblEditor, "DialogBoxes.SFDataSourcePropertiesDlg.EditorLabel");
			this.lblEditor.Location = new System.Drawing.Point(3, 29);
			this.lblEditor.Name = "lblEditor";
			this.lblEditor.Size = new System.Drawing.Size(42, 15);
			this.lblEditor.TabIndex = 0;
			this.lblEditor.Text = "&Editor:";
			// 
			// txtEditor
			// 
			this.txtEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.txtEditor, "This is the application used to edit the data source file.");
			this.locExtender.SetLocalizationComment(this.txtEditor, "Text box in which to specify the editor for the data source in the standard forma" +
					"t data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.txtEditor, "DialogBoxes.SFDataSourcePropertiesDlg.EditorTextBox");
			this.txtEditor.Location = new System.Drawing.Point(51, 26);
			this.txtEditor.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
			this.txtEditor.Name = "txtEditor";
			this.txtEditor.Size = new System.Drawing.Size(40, 21);
			this.txtEditor.TabIndex = 1;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnBrowse.AutoSize = true;
			this.btnBrowse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnBrowse, null);
			this.locExtender.SetLocalizationComment(this.btnBrowse, null);
			this.locExtender.SetLocalizationPriority(this.btnBrowse, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnBrowse, "Localized in base class");
			this.btnBrowse.Location = new System.Drawing.Point(100, 23);
			this.btnBrowse.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
			this.btnBrowse.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(80, 26);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "&Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.HandleBrowseClick);
			// 
			// pnlMappingsInner
			// 
			this.pnlMappingsInner.AutoSize = true;
			this.pnlMappingsInner.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlMappingsInner.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlMappingsInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMappingsInner.ClipTextForChildControls = true;
			this.pnlMappingsInner.ControlReceivingFocusOnMnemonic = null;
			this.pnlMappingsInner.Controls.Add(this.tblLayoutToolBoxSortField);
			this.pnlMappingsInner.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlMappingsInner.DoubleBuffered = true;
			this.pnlMappingsInner.DrawOnlyBottomBorder = true;
			this.pnlMappingsInner.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlMappingsInner.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlMappingsInner, null);
			this.locExtender.SetLocalizationComment(this.pnlMappingsInner, null);
			this.locExtender.SetLocalizationPriority(this.pnlMappingsInner, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlMappingsInner, "SFDataSourcePropertiesDlg.pnlMappingsGrid");
			this.pnlMappingsInner.Location = new System.Drawing.Point(0, 24);
			this.pnlMappingsInner.MnemonicGeneratesClick = false;
			this.pnlMappingsInner.Name = "pnlMappingsInner";
			this.pnlMappingsInner.Padding = new System.Windows.Forms.Padding(0, 0, 0, 7);
			this.pnlMappingsInner.PaintExplorerBarBackground = false;
			this.pnlMappingsInner.Size = new System.Drawing.Size(276, 130);
			this.pnlMappingsInner.TabIndex = 3;
			// 
			// tblLayoutToolBoxSortField
			// 
			this.tblLayoutToolBoxSortField.AutoSize = true;
			this.tblLayoutToolBoxSortField.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblLayoutToolBoxSortField.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutToolBoxSortField.ColumnCount = 2;
			this.tblLayoutToolBoxSortField.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutToolBoxSortField.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutToolBoxSortField.Controls.Add(this.lblInformation, 0, 0);
			this.tblLayoutToolBoxSortField.Controls.Add(this.cboToolboxSortField, 1, 2);
			this.tblLayoutToolBoxSortField.Controls.Add(this.cboRecordMarkers, 1, 1);
			this.tblLayoutToolBoxSortField.Controls.Add(this.lblToolboxSortField, 0, 2);
			this.tblLayoutToolBoxSortField.Controls.Add(this.lblRecordMarker, 0, 1);
			this.tblLayoutToolBoxSortField.Dock = System.Windows.Forms.DockStyle.Top;
			this.tblLayoutToolBoxSortField.Location = new System.Drawing.Point(0, 0);
			this.tblLayoutToolBoxSortField.Name = "tblLayoutToolBoxSortField";
			this.tblLayoutToolBoxSortField.RowCount = 3;
			this.tblLayoutToolBoxSortField.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutToolBoxSortField.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutToolBoxSortField.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutToolBoxSortField.Size = new System.Drawing.Size(274, 121);
			this.tblLayoutToolBoxSortField.TabIndex = 2;
			// 
			// lblInformation
			// 
			this.lblInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblInformation.AutoEllipsis = true;
			this.lblInformation.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutToolBoxSortField.SetColumnSpan(this.lblInformation, 2);
			this.lblInformation.Image = null;
			this.locExtender.SetLocalizableToolTip(this.lblInformation, null);
			this.locExtender.SetLocalizationComment(this.lblInformation, null);
			this.locExtender.SetLocalizingId(this.lblInformation, "DialogBoxes.SFDataSourcePropertiesDlg.InformationLabel");
			this.lblInformation.Location = new System.Drawing.Point(3, 5);
			this.lblInformation.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.lblInformation.Name = "lblInformation";
			this.lblInformation.Size = new System.Drawing.Size(268, 52);
			this.lblInformation.TabIndex = 3;
			this.lblInformation.Text = resources.GetString("lblInformation.Text");
			// 
			// cboToolboxSortField
			// 
			this.cboToolboxSortField.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cboToolboxSortField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboToolboxSortField.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboToolboxSortField, "Used for jumping to the appropriate Toolbox record.");
			this.locExtender.SetLocalizationComment(this.cboToolboxSortField, null);
			this.locExtender.SetLocalizingId(this.cboToolboxSortField, "DialogBoxes.SFDataSourcePropertiesDlg.ToolboxSortFieldList");
			this.cboToolboxSortField.Location = new System.Drawing.Point(144, 98);
			this.cboToolboxSortField.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.cboToolboxSortField.Name = "cboToolboxSortField";
			this.cboToolboxSortField.Size = new System.Drawing.Size(127, 23);
			this.cboToolboxSortField.Sorted = true;
			this.cboToolboxSortField.TabIndex = 1;
			// 
			// cboRecordMarkers
			// 
			this.cboRecordMarkers.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cboRecordMarkers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRecordMarkers.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboRecordMarkers, "Select the marker that marks\\nthe beginning of each record.");
			this.locExtender.SetLocalizationComment(this.cboRecordMarkers, null);
			this.locExtender.SetLocalizingId(this.cboRecordMarkers, "DialogBoxes.SFDataSourcePropertiesDlg.RecordMarkersList");
			this.cboRecordMarkers.Location = new System.Drawing.Point(144, 69);
			this.cboRecordMarkers.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
			this.cboRecordMarkers.Name = "cboRecordMarkers";
			this.cboRecordMarkers.Size = new System.Drawing.Size(127, 23);
			this.cboRecordMarkers.TabIndex = 2;
			// 
			// lblToolboxSortField
			// 
			this.lblToolboxSortField.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblToolboxSortField.AutoSize = true;
			this.lblToolboxSortField.BackColor = System.Drawing.Color.Transparent;
			this.lblToolboxSortField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblToolboxSortField.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblToolboxSortField, null);
			this.locExtender.SetLocalizationComment(this.lblToolboxSortField, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblToolboxSortField, "DialogBoxes.SFDataSourcePropertiesDlg.ToolboxSortFieldLabel");
			this.lblToolboxSortField.Location = new System.Drawing.Point(3, 101);
			this.lblToolboxSortField.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
			this.lblToolboxSortField.Name = "lblToolboxSortField";
			this.lblToolboxSortField.Size = new System.Drawing.Size(135, 15);
			this.lblToolboxSortField.TabIndex = 0;
			this.lblToolboxSortField.Text = "First &Toolbox Sort Field:";
			// 
			// lblRecordMarker
			// 
			this.lblRecordMarker.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblRecordMarker.AutoSize = true;
			this.lblRecordMarker.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblRecordMarker, null);
			this.locExtender.SetLocalizationComment(this.lblRecordMarker, null);
			this.locExtender.SetLocalizingId(this.lblRecordMarker, "DialogBoxes.SFDataSourcePropertiesDlg.RecordMarkerLabel");
			this.lblRecordMarker.Location = new System.Drawing.Point(3, 73);
			this.lblRecordMarker.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblRecordMarker.Name = "lblRecordMarker";
			this.lblRecordMarker.Size = new System.Drawing.Size(87, 15);
			this.lblRecordMarker.TabIndex = 1;
			this.lblRecordMarker.Text = "Record Marker:";
			// 
			// pnlMappingsHdg
			// 
			this.pnlMappingsHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlMappingsHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMappingsHdg.ClipTextForChildControls = true;
			this.pnlMappingsHdg.ColorBottom = System.Drawing.Color.Empty;
			this.pnlMappingsHdg.ColorTop = System.Drawing.Color.Empty;
			this.pnlMappingsHdg.ControlReceivingFocusOnMnemonic = null;
			this.pnlMappingsHdg.Controls.Add(this.btnInformation);
			this.pnlMappingsHdg.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlMappingsHdg.DoubleBuffered = true;
			this.pnlMappingsHdg.DrawOnlyBottomBorder = true;
			this.pnlMappingsHdg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlMappingsHdg.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlMappingsHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlMappingsHdg, "Heading above the field mappings list in the standard format data source properti" +
					"es dialog box.");
			this.locExtender.SetLocalizingId(this.pnlMappingsHdg, "DialogBoxes.SFDataSourcePropertiesDlg.MappingsHeadingText");
			this.pnlMappingsHdg.Location = new System.Drawing.Point(0, 0);
			this.pnlMappingsHdg.MakeDark = false;
			this.pnlMappingsHdg.MnemonicGeneratesClick = false;
			this.pnlMappingsHdg.Name = "pnlMappingsHdg";
			this.pnlMappingsHdg.PaintExplorerBarBackground = false;
			this.pnlMappingsHdg.Size = new System.Drawing.Size(276, 24);
			this.pnlMappingsHdg.TabIndex = 0;
			this.pnlMappingsHdg.Text = "Field &Mappings";
			// 
			// btnInformation
			// 
			this.btnInformation.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnInformation.BackColor = System.Drawing.Color.Transparent;
			this.btnInformation.CanBeChecked = false;
			this.btnInformation.Checked = false;
			this.btnInformation.DrawEmpty = false;
			this.btnInformation.DrawLeftArrowButton = false;
			this.btnInformation.DrawRightArrowButton = false;
			this.btnInformation.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnInformation.Image = ((System.Drawing.Image)(resources.GetObject("btnInformation.Image")));
			this.locExtender.SetLocalizableToolTip(this.btnInformation, "Display field mapping information.");
			this.locExtender.SetLocalizationComment(this.btnInformation, null);
			this.locExtender.SetLocalizingId(this.btnInformation, "DialogBoxes.SFDataSourcePropertiesDlg.InformationButton");
			this.btnInformation.Location = new System.Drawing.Point(242, 1);
			this.btnInformation.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.btnInformation.Name = "btnInformation";
			this.btnInformation.Size = new System.Drawing.Size(29, 20);
			this.btnInformation.TabIndex = 2;
			this.btnInformation.Click += new System.EventHandler(this.HandleInformationButtonClick);
			// 
			// pnlSrcFile
			// 
			this.pnlSrcFile.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlSrcFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSrcFile.ClipTextForChildControls = true;
			this.pnlSrcFile.ControlReceivingFocusOnMnemonic = null;
			this.pnlSrcFile.Controls.Add(this.txtFilePreview);
			this.pnlSrcFile.Controls.Add(this.pnlSrcFileHdg);
			this.pnlSrcFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSrcFile.DoubleBuffered = false;
			this.pnlSrcFile.DrawOnlyBottomBorder = false;
			this.pnlSrcFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSrcFile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSrcFile, null);
			this.locExtender.SetLocalizationComment(this.pnlSrcFile, null);
			this.locExtender.SetLocalizationPriority(this.pnlSrcFile, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSrcFile, "SFDataSourcePropertiesDlg.pnlSrcFile");
			this.pnlSrcFile.Location = new System.Drawing.Point(0, 0);
			this.pnlSrcFile.MnemonicGeneratesClick = false;
			this.pnlSrcFile.Name = "pnlSrcFile";
			this.pnlSrcFile.PaintExplorerBarBackground = false;
			this.pnlSrcFile.Size = new System.Drawing.Size(178, 463);
			this.pnlSrcFile.TabIndex = 0;
			// 
			// txtFilePreview
			// 
			this.txtFilePreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtFilePreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this.txtFilePreview, null);
			this.locExtender.SetLocalizationComment(this.txtFilePreview, null);
			this.locExtender.SetLocalizationPriority(this.txtFilePreview, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtFilePreview, "SFDataSourcePropertiesDlg.txtFilePreview");
			this.txtFilePreview.Location = new System.Drawing.Point(0, 24);
			this.txtFilePreview.Multiline = true;
			this.txtFilePreview.Name = "txtFilePreview";
			this.txtFilePreview.ReadOnly = true;
			this.txtFilePreview.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtFilePreview.Size = new System.Drawing.Size(176, 437);
			this.txtFilePreview.TabIndex = 1;
			this.txtFilePreview.TabStop = false;
			this.txtFilePreview.Text = "Preview of file contents";
			this.txtFilePreview.WordWrap = false;
			// 
			// pnlSrcFileHdg
			// 
			this.pnlSrcFileHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlSrcFileHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSrcFileHdg.ClipTextForChildControls = true;
			this.pnlSrcFileHdg.ColorBottom = System.Drawing.Color.Empty;
			this.pnlSrcFileHdg.ColorTop = System.Drawing.Color.Empty;
			this.pnlSrcFileHdg.ControlReceivingFocusOnMnemonic = null;
			this.pnlSrcFileHdg.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSrcFileHdg.DoubleBuffered = true;
			this.pnlSrcFileHdg.DrawOnlyBottomBorder = true;
			this.pnlSrcFileHdg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSrcFileHdg.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSrcFileHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlSrcFileHdg, "Heading above the contents of the data source file in the standard format data so" +
					"urce properties dialog box.");
			this.locExtender.SetLocalizingId(this.pnlSrcFileHdg, "DialogBoxes.SFDataSourcePropertiesDlg.SourceFileHeadingText");
			this.pnlSrcFileHdg.Location = new System.Drawing.Point(0, 0);
			this.pnlSrcFileHdg.MakeDark = false;
			this.pnlSrcFileHdg.MnemonicGeneratesClick = false;
			this.pnlSrcFileHdg.Name = "pnlSrcFileHdg";
			this.pnlSrcFileHdg.PaintExplorerBarBackground = false;
			this.pnlSrcFileHdg.Size = new System.Drawing.Size(176, 24);
			this.pnlSrcFileHdg.TabIndex = 0;
			this.pnlSrcFileHdg.Text = "Source File: {0}";
			this.pnlSrcFileHdg.BeforeDrawText += new SilTools.Controls.SilPanel.BeforeDrawTextHandler(this.HandleSourceFilePanelBeforeDrawText);
			// 
			// splitOuter
			// 
			this.splitOuter.CausesValidation = false;
			this.splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitOuter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitOuter.IsSplitterFixed = true;
			this.splitOuter.Location = new System.Drawing.Point(10, 10);
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.pnlParseType);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.scImport);
			this.splitOuter.Size = new System.Drawing.Size(694, 463);
			this.splitOuter.SplitterDistance = 226;
			this.splitOuter.SplitterWidth = 6;
			this.splitOuter.TabIndex = 0;
			this.splitOuter.TabStop = false;
			// 
			// pnlParseType
			// 
			this.pnlParseType.AutoScroll = true;
			this.pnlParseType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlParseType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlParseType.ClipTextForChildControls = true;
			this.pnlParseType.ControlReceivingFocusOnMnemonic = null;
			this.pnlParseType.Controls.Add(this.tblLayoutParseOptions);
			this.pnlParseType.Controls.Add(this.pnlParseHdg);
			this.pnlParseType.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlParseType.DoubleBuffered = true;
			this.pnlParseType.DrawOnlyBottomBorder = false;
			this.pnlParseType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlParseType.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlParseType, null);
			this.locExtender.SetLocalizationComment(this.pnlParseType, null);
			this.locExtender.SetLocalizationPriority(this.pnlParseType, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlParseType, "SFDataSourcePropertiesDlg.pnlParseType");
			this.pnlParseType.Location = new System.Drawing.Point(0, 0);
			this.pnlParseType.MnemonicGeneratesClick = false;
			this.pnlParseType.Name = "pnlParseType";
			this.pnlParseType.PaintExplorerBarBackground = false;
			this.pnlParseType.Size = new System.Drawing.Size(226, 463);
			this.pnlParseType.TabIndex = 0;
			// 
			// tblLayoutParseOptions
			// 
			this.tblLayoutParseOptions.AutoSize = true;
			this.tblLayoutParseOptions.ColumnCount = 1;
			this.tblLayoutParseOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutParseOptions.Controls.Add(this.lblParseType, 0, 0);
			this.tblLayoutParseOptions.Controls.Add(this.rbInterlinearize, 0, 4);
			this.tblLayoutParseOptions.Controls.Add(this.rbParseOneToOne, 0, 3);
			this.tblLayoutParseOptions.Controls.Add(this.lblSampleOutput, 0, 9);
			this.tblLayoutParseOptions.Controls.Add(this.rbParseOnlyPhonetic, 0, 2);
			this.tblLayoutParseOptions.Controls.Add(this.rbNoParse, 0, 1);
			this.tblLayoutParseOptions.Controls.Add(this.pnlSampleInput, 0, 8);
			this.tblLayoutParseOptions.Controls.Add(this.cboFirstInterlinear, 0, 6);
			this.tblLayoutParseOptions.Controls.Add(this.lblSampleInput, 0, 7);
			this.tblLayoutParseOptions.Controls.Add(this.lblFirstInterlinear, 0, 5);
			this.tblLayoutParseOptions.Controls.Add(this.pnlSampeOutput, 0, 10);
			this.tblLayoutParseOptions.Dock = System.Windows.Forms.DockStyle.Top;
			this.tblLayoutParseOptions.Location = new System.Drawing.Point(0, 24);
			this.tblLayoutParseOptions.Name = "tblLayoutParseOptions";
			this.tblLayoutParseOptions.RowCount = 11;
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutParseOptions.Size = new System.Drawing.Size(224, 311);
			this.tblLayoutParseOptions.TabIndex = 1;
			// 
			// lblParseType
			// 
			this.lblParseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblParseType.AutoEllipsis = true;
			this.lblParseType.AutoSize = true;
			this.lblParseType.BackColor = System.Drawing.Color.Transparent;
			this.lblParseType.Image = null;
			this.locExtender.SetLocalizableToolTip(this.lblParseType, null);
			this.locExtender.SetLocalizationComment(this.lblParseType, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblParseType, "DialogBoxes.SFDataSourcePropertiesDlg.ParseTypeLabel");
			this.lblParseType.Location = new System.Drawing.Point(8, 8);
			this.lblParseType.Margin = new System.Windows.Forms.Padding(8, 8, 5, 12);
			this.lblParseType.Name = "lblParseType";
			this.lblParseType.Size = new System.Drawing.Size(211, 30);
			this.lblParseType.TabIndex = 0;
			this.lblParseType.Text = "Parse the data source records in the following way:";
			// 
			// rbInterlinearize
			// 
			this.rbInterlinearize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rbInterlinearize.AutoEllipsis = true;
			this.rbInterlinearize.BackColor = System.Drawing.Color.Transparent;
			this.rbInterlinearize.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbInterlinearize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbInterlinearize, null);
			this.locExtender.SetLocalizationComment(this.rbInterlinearize, "Parsing option radio button in standard format data source properties dialog box." +
					"");
			this.locExtender.SetLocalizingId(this.rbInterlinearize, "DialogBoxes.SFDataSourcePropertiesDlg.InterlinearizeRadioButton");
			this.rbInterlinearize.Location = new System.Drawing.Point(10, 65);
			this.rbInterlinearize.Margin = new System.Windows.Forms.Padding(10, 0, 5, 0);
			this.rbInterlinearize.Name = "rbInterlinearize";
			this.rbInterlinearize.Size = new System.Drawing.Size(209, 19);
			this.rbInterlinearize.TabIndex = 4;
			this.rbInterlinearize.TabStop = true;
			this.rbInterlinearize.Text = "Parse &as interlinear";
			this.rbInterlinearize.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbInterlinearize.UseVisualStyleBackColor = false;
			// 
			// rbParseOneToOne
			// 
			this.rbParseOneToOne.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rbParseOneToOne.AutoEllipsis = true;
			this.rbParseOneToOne.BackColor = System.Drawing.Color.Transparent;
			this.rbParseOneToOne.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbParseOneToOne.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbParseOneToOne, null);
			this.locExtender.SetLocalizationComment(this.rbParseOneToOne, "Parsing option radio button in standard format data source properties dialog box." +
					"");
			this.locExtender.SetLocalizingId(this.rbParseOneToOne, "DialogBoxes.SFDataSourcePropertiesDlg.ParseOneToOneRadioButton");
			this.rbParseOneToOne.Location = new System.Drawing.Point(10, 60);
			this.rbParseOneToOne.Margin = new System.Windows.Forms.Padding(10, 0, 5, 5);
			this.rbParseOneToOne.Name = "rbParseOneToOne";
			this.rbParseOneToOne.Size = new System.Drawing.Size(209, 34);
			this.rbParseOneToOne.TabIndex = 3;
			this.rbParseOneToOne.TabStop = true;
			this.rbParseOneToOne.Text = "Parse assuming a &one-to-one correspondence";
			this.rbParseOneToOne.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbParseOneToOne.UseVisualStyleBackColor = false;
			// 
			// lblSampleOutput
			// 
			this.lblSampleOutput.AutoSize = true;
			this.lblSampleOutput.BackColor = System.Drawing.Color.Transparent;
			this.lblSampleOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblSampleOutput.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSampleOutput, null);
			this.locExtender.SetLocalizationComment(this.lblSampleOutput, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblSampleOutput, "DialogBoxes.SFDataSourcePropertiesDlg.SampleOutputLabel");
			this.lblSampleOutput.Location = new System.Drawing.Point(8, 192);
			this.lblSampleOutput.Margin = new System.Windows.Forms.Padding(8, 5, 3, 0);
			this.lblSampleOutput.Name = "lblSampleOutput";
			this.lblSampleOutput.Size = new System.Drawing.Size(91, 15);
			this.lblSampleOutput.TabIndex = 9;
			this.lblSampleOutput.Text = "Sample Result:";
			// 
			// rbParseOnlyPhonetic
			// 
			this.rbParseOnlyPhonetic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rbParseOnlyPhonetic.AutoEllipsis = true;
			this.rbParseOnlyPhonetic.BackColor = System.Drawing.Color.Transparent;
			this.rbParseOnlyPhonetic.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbParseOnlyPhonetic.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbParseOnlyPhonetic, null);
			this.locExtender.SetLocalizationComment(this.rbParseOnlyPhonetic, null);
			this.locExtender.SetLocalizingId(this.rbParseOnlyPhonetic, "DialogBoxes.SFDataSourcePropertiesDlg.ParseOnlyPhoneticRadioButton");
			this.rbParseOnlyPhonetic.Location = new System.Drawing.Point(10, 55);
			this.rbParseOnlyPhonetic.Margin = new System.Windows.Forms.Padding(10, 0, 5, 5);
			this.rbParseOnlyPhonetic.Name = "rbParseOnlyPhonetic";
			this.rbParseOnlyPhonetic.Size = new System.Drawing.Size(209, 19);
			this.rbParseOnlyPhonetic.TabIndex = 2;
			this.rbParseOnlyPhonetic.TabStop = true;
			this.rbParseOnlyPhonetic.Text = "&Parse only the phonetic";
			this.rbParseOnlyPhonetic.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbParseOnlyPhonetic.UseVisualStyleBackColor = false;
			// 
			// rbNoParse
			// 
			this.rbNoParse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rbNoParse.AutoEllipsis = true;
			this.rbNoParse.BackColor = System.Drawing.Color.Transparent;
			this.rbNoParse.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.locExtender.SetLocalizableToolTip(this.rbNoParse, null);
			this.locExtender.SetLocalizationComment(this.rbNoParse, null);
			this.locExtender.SetLocalizingId(this.rbNoParse, "DialogBoxes.SFDataSourcePropertiesDlg.NoParseRadioButton");
			this.rbNoParse.Location = new System.Drawing.Point(10, 50);
			this.rbNoParse.Margin = new System.Windows.Forms.Padding(10, 0, 5, 5);
			this.rbNoParse.Name = "rbNoParse";
			this.rbNoParse.Size = new System.Drawing.Size(209, 19);
			this.rbNoParse.TabIndex = 1;
			this.rbNoParse.TabStop = true;
			this.rbNoParse.Text = "&Do not parse any fields";
			this.rbNoParse.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbNoParse.UseVisualStyleBackColor = false;
			// 
			// pnlSampleInput
			// 
			this.pnlSampleInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSampleInput.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSampleInput.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlSampleInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSampleInput.ClipTextForChildControls = true;
			this.pnlSampleInput.ControlReceivingFocusOnMnemonic = null;
			this.pnlSampleInput.Controls.Add(this.rtfSampleInput);
			this.pnlSampleInput.DoubleBuffered = false;
			this.pnlSampleInput.DrawOnlyBottomBorder = false;
			this.pnlSampleInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSampleInput.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSampleInput, null);
			this.locExtender.SetLocalizationComment(this.pnlSampleInput, null);
			this.locExtender.SetLocalizationPriority(this.pnlSampleInput, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSampleInput, "SFDataSourcePropertiesDlg.pnlSampleInput");
			this.pnlSampleInput.Location = new System.Drawing.Point(5, 134);
			this.pnlSampleInput.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.pnlSampleInput.MnemonicGeneratesClick = false;
			this.pnlSampleInput.Name = "pnlSampleInput";
			this.pnlSampleInput.Padding = new System.Windows.Forms.Padding(3);
			this.pnlSampleInput.PaintExplorerBarBackground = false;
			this.pnlSampleInput.Size = new System.Drawing.Size(214, 50);
			this.pnlSampleInput.TabIndex = 8;
			// 
			// rtfSampleInput
			// 
			this.rtfSampleInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtfSampleInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtfSampleInput.Location = new System.Drawing.Point(3, 3);
			this.rtfSampleInput.Name = "rtfSampleInput";
			this.rtfSampleInput.Size = new System.Drawing.Size(206, 42);
			this.rtfSampleInput.TabIndex = 0;
			this.rtfSampleInput.TabStop = false;
			this.rtfSampleInput.Text = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidExportAsToolTip;
			// 
			// cboFirstInterlinear
			// 
			this.cboFirstInterlinear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboFirstInterlinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFirstInterlinear.Enabled = false;
			this.cboFirstInterlinear.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboFirstInterlinear, "Select the marker of the first\\ninterlinear field in each record.");
			this.locExtender.SetLocalizationComment(this.cboFirstInterlinear, null);
			this.locExtender.SetLocalizingId(this.cboFirstInterlinear, "DialogBoxes.SFDataSourcePropertiesDlg.FirstInterlinearList");
			this.cboFirstInterlinear.Location = new System.Drawing.Point(26, 85);
			this.cboFirstInterlinear.Margin = new System.Windows.Forms.Padding(26, 3, 5, 3);
			this.cboFirstInterlinear.MaxDropDownItems = 12;
			this.cboFirstInterlinear.Name = "cboFirstInterlinear";
			this.cboFirstInterlinear.Size = new System.Drawing.Size(193, 23);
			this.cboFirstInterlinear.TabIndex = 6;
			this.cboFirstInterlinear.SelectedIndexChanged += new System.EventHandler(this.HandleFirstInterlinearComboSelectedIndexChanged);
			// 
			// lblSampleInput
			// 
			this.lblSampleInput.AutoSize = true;
			this.lblSampleInput.BackColor = System.Drawing.Color.Transparent;
			this.lblSampleInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblSampleInput.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSampleInput, null);
			this.locExtender.SetLocalizationComment(this.lblSampleInput, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblSampleInput, "DialogBoxes.SFDataSourcePropertiesDlg.SampleInputLabel");
			this.lblSampleInput.Location = new System.Drawing.Point(8, 116);
			this.lblSampleInput.Margin = new System.Windows.Forms.Padding(8, 5, 3, 0);
			this.lblSampleInput.Name = "lblSampleInput";
			this.lblSampleInput.Size = new System.Drawing.Size(83, 15);
			this.lblSampleInput.TabIndex = 7;
			this.lblSampleInput.Text = "Sample Input:";
			// 
			// lblFirstInterlinear
			// 
			this.lblFirstInterlinear.AutoSize = true;
			this.lblFirstInterlinear.BackColor = System.Drawing.Color.Transparent;
			this.lblFirstInterlinear.Enabled = false;
			this.lblFirstInterlinear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblFirstInterlinear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblFirstInterlinear, null);
			this.locExtender.SetLocalizationComment(this.lblFirstInterlinear, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblFirstInterlinear, "DialogBoxes.SFDataSourcePropertiesDlg.FirstInterlinearLabel");
			this.lblFirstInterlinear.Location = new System.Drawing.Point(26, 65);
			this.lblFirstInterlinear.Margin = new System.Windows.Forms.Padding(26, 0, 5, 2);
			this.lblFirstInterlinear.Name = "lblFirstInterlinear";
			this.lblFirstInterlinear.Size = new System.Drawing.Size(159, 15);
			this.lblFirstInterlinear.TabIndex = 5;
			this.lblFirstInterlinear.Text = "&First interlinear field marker:";
			// 
			// pnlSampeOutput
			// 
			this.pnlSampeOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSampeOutput.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlSampeOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSampeOutput.ClipTextForChildControls = true;
			this.pnlSampeOutput.ControlReceivingFocusOnMnemonic = null;
			this.pnlSampeOutput.Controls.Add(this.gridSampleOutput);
			this.pnlSampeOutput.DoubleBuffered = true;
			this.pnlSampeOutput.DrawOnlyBottomBorder = false;
			this.pnlSampeOutput.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSampeOutput.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSampeOutput, null);
			this.locExtender.SetLocalizationComment(this.pnlSampeOutput, null);
			this.locExtender.SetLocalizationPriority(this.pnlSampeOutput, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSampeOutput, "SFDataSourcePropertiesDlg.pnlSampeOutput");
			this.pnlSampeOutput.Location = new System.Drawing.Point(5, 210);
			this.pnlSampeOutput.Margin = new System.Windows.Forms.Padding(5, 3, 5, 0);
			this.pnlSampeOutput.MnemonicGeneratesClick = false;
			this.pnlSampeOutput.Name = "pnlSampeOutput";
			this.pnlSampeOutput.PaintExplorerBarBackground = false;
			this.pnlSampeOutput.Size = new System.Drawing.Size(214, 101);
			this.pnlSampeOutput.TabIndex = 10;
			this.pnlSampeOutput.Text = "silPanel1";
			// 
			// gridSampleOutput
			// 
			this.gridSampleOutput.AllowUserToAddRows = false;
			this.gridSampleOutput.AllowUserToDeleteRows = false;
			this.gridSampleOutput.AllowUserToOrderColumns = true;
			this.gridSampleOutput.AllowUserToResizeColumns = false;
			this.gridSampleOutput.AllowUserToResizeRows = false;
			this.gridSampleOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.gridSampleOutput.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.gridSampleOutput.BackgroundColor = System.Drawing.SystemColors.Window;
			this.gridSampleOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.gridSampleOutput.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridSampleOutput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridSampleOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridSampleOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SampleOutputPhoneticColumn,
            this.SampleOutputGlossColumn,
            this.SampleOutputPartOfSpeechColumn});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gridSampleOutput.DefaultCellStyle = dataGridViewCellStyle2;
			this.gridSampleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridSampleOutput.DrawTextBoxEditControlBorder = false;
			this.gridSampleOutput.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.gridSampleOutput.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.gridSampleOutput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.gridSampleOutput.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.gridSampleOutput, null);
			this.locExtender.SetLocalizationComment(this.gridSampleOutput, null);
			this.locExtender.SetLocalizationPriority(this.gridSampleOutput, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.gridSampleOutput, "SFDataSourcePropertiesDlg.gridSampleOutput");
			this.gridSampleOutput.Location = new System.Drawing.Point(0, 0);
			this.gridSampleOutput.MultiSelect = false;
			this.gridSampleOutput.Name = "gridSampleOutput";
			this.gridSampleOutput.PaintHeaderAcrossFullGridWidth = true;
			this.gridSampleOutput.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.gridSampleOutput.RowHeadersVisible = false;
			this.gridSampleOutput.RowHeadersWidth = 22;
			this.gridSampleOutput.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.gridSampleOutput.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.gridSampleOutput.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.gridSampleOutput.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.gridSampleOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.gridSampleOutput.ShowWaterMarkWhenDirty = false;
			this.gridSampleOutput.Size = new System.Drawing.Size(212, 99);
			this.gridSampleOutput.TabIndex = 0;
			this.gridSampleOutput.TabStop = false;
			this.gridSampleOutput.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.gridSampleOutput.WaterMark = "!";
			// 
			// SampleOutputPhoneticColumn
			// 
			this.SampleOutputPhoneticColumn.HeaderText = "_L10N_:DialogBoxes.SFDataSourcePropertiesDlg.SampleOutputGrid.ColumnHeadings.Phonetic!Phonetic";
			this.SampleOutputPhoneticColumn.Name = "SampleOutputPhoneticColumn";
			this.SampleOutputPhoneticColumn.ReadOnly = true;
			this.SampleOutputPhoneticColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.SampleOutputPhoneticColumn.Width = 60;
			// 
			// SampleOutputGlossColumn
			// 
			this.SampleOutputGlossColumn.HeaderText = "_L10N_:DialogBoxes.SFDataSourcePropertiesDlg.SampleOutputGrid.ColumnHeadings.Gloss!Gloss";
			this.SampleOutputGlossColumn.Name = "SampleOutputGlossColumn";
			this.SampleOutputGlossColumn.ReadOnly = true;
			this.SampleOutputGlossColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.SampleOutputGlossColumn.Width = 41;
			// 
			// SampleOutputPartOfSpeechColumn
			// 
			this.SampleOutputPartOfSpeechColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.SampleOutputPartOfSpeechColumn.HeaderText = "_L10N_:DialogBoxes.SFDataSourcePropertiesDlg.SampleOutputGrid.ColumnHeadings.PartOfSpeech!Part of Speech";
			this.SampleOutputPartOfSpeechColumn.Name = "SampleOutputPartOfSpeechColumn";
			this.SampleOutputPartOfSpeechColumn.ReadOnly = true;
			this.SampleOutputPartOfSpeechColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.SampleOutputPartOfSpeechColumn.Width = 89;
			// 
			// pnlParseHdg
			// 
			this.pnlParseHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlParseHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlParseHdg.ClipTextForChildControls = true;
			this.pnlParseHdg.ColorBottom = System.Drawing.Color.Empty;
			this.pnlParseHdg.ColorTop = System.Drawing.Color.Empty;
			this.pnlParseHdg.ControlReceivingFocusOnMnemonic = null;
			this.pnlParseHdg.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlParseHdg.DoubleBuffered = true;
			this.pnlParseHdg.DrawOnlyBottomBorder = true;
			this.pnlParseHdg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlParseHdg.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlParseHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlParseHdg, "Heading above the parsing options in the standard format data source properties d" +
					"ialog box.");
			this.locExtender.SetLocalizingId(this.pnlParseHdg, "DialogBoxes.SFDataSourcePropertiesDlg.ParseHeadingText");
			this.pnlParseHdg.Location = new System.Drawing.Point(0, 0);
			this.pnlParseHdg.MakeDark = false;
			this.pnlParseHdg.MnemonicGeneratesClick = false;
			this.pnlParseHdg.Name = "pnlParseHdg";
			this.pnlParseHdg.PaintExplorerBarBackground = false;
			this.pnlParseHdg.Size = new System.Drawing.Size(224, 24);
			this.pnlParseHdg.TabIndex = 0;
			this.pnlParseHdg.Text = "Parsing Options";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SFDataSourcePropertiesDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(714, 513);
			this.Controls.Add(this.splitOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.SFDataSourcePropertiesDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(675, 540);
			this.Name = "SFDataSourcePropertiesDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Standard Format Data Source Properties";
			this.Controls.SetChildIndex(this.splitOuter, 0);
			this.scImport.Panel1.ResumeLayout(false);
			this.scImport.Panel2.ResumeLayout(false);
			this.scImport.ResumeLayout(false);
			this.pnlMappings.ResumeLayout(false);
			this.pnlMappings.PerformLayout();
			this.tblLayoutEditor.ResumeLayout(false);
			this.tblLayoutEditor.PerformLayout();
			this.pnlMappingsInner.ResumeLayout(false);
			this.pnlMappingsInner.PerformLayout();
			this.tblLayoutToolBoxSortField.ResumeLayout(false);
			this.tblLayoutToolBoxSortField.PerformLayout();
			this.pnlMappingsHdg.ResumeLayout(false);
			this.pnlSrcFile.ResumeLayout(false);
			this.pnlSrcFile.PerformLayout();
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.pnlParseType.ResumeLayout(false);
			this.pnlParseType.PerformLayout();
			this.tblLayoutParseOptions.ResumeLayout(false);
			this.tblLayoutParseOptions.PerformLayout();
			this.pnlSampleInput.ResumeLayout(false);
			this.pnlSampeOutput.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridSampleOutput)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private Label lblFirstInterlinear;
		private ComboBox cboFirstInterlinear;
		private SilPanel pnlParseType;
		private SplitContainer splitOuter;
		private RichTextBox rtfSampleInput;
		private SilPanel pnlSampleInput;
		private SilTools.SilGrid gridSampleOutput;
		private Label lblSampleInput;
		private Label lblSampleOutput;
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
		private TableLayoutPanel tblLayoutParseOptions;
		private AutoHeighRadioButton rbNoParse;
		private AutoHeighRadioButton rbParseOnlyPhonetic;
		private AutoHeighRadioButton rbInterlinearize;
		private AutoHeighRadioButton rbParseOneToOne;
		private SilPanel pnlSampeOutput;
		private AutoHeightLabel lblParseType;
		private TableLayoutPanel tblLayoutToolBoxSortField;
		private TableLayoutPanel tblLayoutEditor;
		private DataGridViewTextBoxColumn SampleOutputPhoneticColumn;
		private DataGridViewTextBoxColumn SampleOutputGlossColumn;
		private DataGridViewTextBoxColumn SampleOutputPartOfSpeechColumn;
		private Label lblRecordMarker;
		private ComboBox cboRecordMarkers;
		private AutoHeightLabel lblInformation;
		private XButton btnInformation;
		private SilPanel pnlMappingsInner;
	}
}
