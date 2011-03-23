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
			resources.ApplyResources(this.pnlMappings, "pnlMappings");
			this.pnlMappings.DoubleBuffered = false;
			this.pnlMappings.DrawOnlyBottomBorder = false;
			this.pnlMappings.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlMappings, null);
			this.locExtender.SetLocalizationComment(this.pnlMappings, null);
			this.locExtender.SetLocalizationPriority(this.pnlMappings, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlMappings, "SFDataSourcePropertiesDlg.pnlMappings");
			this.pnlMappings.MnemonicGeneratesClick = false;
			this.pnlMappings.Name = "pnlMappings";
			this.pnlMappings.PaintExplorerBarBackground = false;
			// 
			// tblLayoutEditor
			// 
			resources.ApplyResources(this.tblLayoutEditor, "tblLayoutEditor");
			this.tblLayoutEditor.Controls.Add(this.lblEditor, 0, 0);
			this.tblLayoutEditor.Controls.Add(this.txtEditor, 1, 0);
			this.tblLayoutEditor.Controls.Add(this.btnBrowse, 2, 0);
			this.tblLayoutEditor.Name = "tblLayoutEditor";
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
			// txtEditor
			// 
			resources.ApplyResources(this.txtEditor, "txtEditor");
			this.locExtender.SetLocalizableToolTip(this.txtEditor, "This is the application used to edit the data source file.");
			this.locExtender.SetLocalizationComment(this.txtEditor, "Text box in which to specify the editor for the data source in the standard forma" +
					"t data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.txtEditor, "SFDataSourcePropertiesDlg.txtEditor");
			this.txtEditor.Name = "txtEditor";
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
			this.btnBrowse.Click += new System.EventHandler(this.HandleBrowseClick);
			// 
			// pnlMappingsInner
			// 
			resources.ApplyResources(this.pnlMappingsInner, "pnlMappingsInner");
			this.pnlMappingsInner.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlMappingsInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMappingsInner.ClipTextForChildControls = true;
			this.pnlMappingsInner.ControlReceivingFocusOnMnemonic = null;
			this.pnlMappingsInner.Controls.Add(this.tblLayoutToolBoxSortField);
			this.pnlMappingsInner.DoubleBuffered = true;
			this.pnlMappingsInner.DrawOnlyBottomBorder = true;
			this.pnlMappingsInner.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlMappingsInner, null);
			this.locExtender.SetLocalizationComment(this.pnlMappingsInner, null);
			this.locExtender.SetLocalizationPriority(this.pnlMappingsInner, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlMappingsInner, "SFDataSourcePropertiesDlg.pnlMappingsGrid");
			this.pnlMappingsInner.MnemonicGeneratesClick = false;
			this.pnlMappingsInner.Name = "pnlMappingsInner";
			this.pnlMappingsInner.PaintExplorerBarBackground = false;
			// 
			// tblLayoutToolBoxSortField
			// 
			resources.ApplyResources(this.tblLayoutToolBoxSortField, "tblLayoutToolBoxSortField");
			this.tblLayoutToolBoxSortField.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutToolBoxSortField.Controls.Add(this.lblInformation, 0, 0);
			this.tblLayoutToolBoxSortField.Controls.Add(this.cboToolboxSortField, 1, 2);
			this.tblLayoutToolBoxSortField.Controls.Add(this.cboRecordMarkers, 1, 1);
			this.tblLayoutToolBoxSortField.Controls.Add(this.lblToolboxSortField, 0, 2);
			this.tblLayoutToolBoxSortField.Controls.Add(this.lblRecordMarker, 0, 1);
			this.tblLayoutToolBoxSortField.Name = "tblLayoutToolBoxSortField";
			// 
			// lblInformation
			// 
			resources.ApplyResources(this.lblInformation, "lblInformation");
			this.lblInformation.AutoEllipsis = true;
			this.lblInformation.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutToolBoxSortField.SetColumnSpan(this.lblInformation, 2);
			this.locExtender.SetLocalizableToolTip(this.lblInformation, null);
			this.locExtender.SetLocalizationComment(this.lblInformation, null);
			this.locExtender.SetLocalizingId(this.lblInformation, "SFDataSourcePropertiesDlg.lblInformation");
			this.lblInformation.Name = "lblInformation";
			// 
			// cboToolboxSortField
			// 
			resources.ApplyResources(this.cboToolboxSortField, "cboToolboxSortField");
			this.cboToolboxSortField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboToolboxSortField.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboToolboxSortField, "Used for jumping to the appropriate Toolbox record.");
			this.locExtender.SetLocalizationComment(this.cboToolboxSortField, null);
			this.locExtender.SetLocalizationPriority(this.cboToolboxSortField, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.cboToolboxSortField, "SFDataSourcePropertiesDlg.cboToolboxSortField");
			this.cboToolboxSortField.Name = "cboToolboxSortField";
			this.cboToolboxSortField.Sorted = true;
			// 
			// cboRecordMarkers
			// 
			resources.ApplyResources(this.cboRecordMarkers, "cboRecordMarkers");
			this.cboRecordMarkers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRecordMarkers.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboRecordMarkers, "Select the marker that marks\\nthe beginning of each record.");
			this.locExtender.SetLocalizationComment(this.cboRecordMarkers, null);
			this.locExtender.SetLocalizingId(this.cboRecordMarkers, "SFDataSourcePropertiesDlg.cboRecordMarkers");
			this.cboRecordMarkers.Name = "cboRecordMarkers";
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
			// lblRecordMarker
			// 
			resources.ApplyResources(this.lblRecordMarker, "lblRecordMarker");
			this.lblRecordMarker.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblRecordMarker, null);
			this.locExtender.SetLocalizationComment(this.lblRecordMarker, null);
			this.locExtender.SetLocalizingId(this.lblRecordMarker, "SFDataSourcePropertiesDlg.lblRecordMarker");
			this.lblRecordMarker.Name = "lblRecordMarker";
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
			resources.ApplyResources(this.pnlMappingsHdg, "pnlMappingsHdg");
			this.pnlMappingsHdg.DoubleBuffered = true;
			this.pnlMappingsHdg.DrawOnlyBottomBorder = true;
			this.pnlMappingsHdg.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlMappingsHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlMappingsHdg, "Heading above the field mappings list in the standard format data source properti" +
					"es dialog box.");
			this.locExtender.SetLocalizingId(this.pnlMappingsHdg, "SFDataSourcePropertiesDlg.pnlMappingsHdg");
			this.pnlMappingsHdg.MakeDark = false;
			this.pnlMappingsHdg.MnemonicGeneratesClick = false;
			this.pnlMappingsHdg.Name = "pnlMappingsHdg";
			this.pnlMappingsHdg.PaintExplorerBarBackground = false;
			// 
			// btnInformation
			// 
			resources.ApplyResources(this.btnInformation, "btnInformation");
			this.btnInformation.BackColor = System.Drawing.Color.Transparent;
			this.btnInformation.CanBeChecked = false;
			this.btnInformation.Checked = false;
			this.btnInformation.DrawEmpty = false;
			this.btnInformation.DrawLeftArrowButton = false;
			this.btnInformation.DrawRightArrowButton = false;
			this.locExtender.SetLocalizableToolTip(this.btnInformation, "Display field mapping information.");
			this.locExtender.SetLocalizationComment(this.btnInformation, null);
			this.locExtender.SetLocalizingId(this.btnInformation, "SFDataSourcePropertiesDlg.btnInformation");
			this.btnInformation.Name = "btnInformation";
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
			resources.ApplyResources(this.pnlSrcFile, "pnlSrcFile");
			this.pnlSrcFile.DoubleBuffered = false;
			this.pnlSrcFile.DrawOnlyBottomBorder = false;
			this.pnlSrcFile.ForeColor = System.Drawing.SystemColors.ControlText;
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
			this.pnlSrcFileHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlSrcFileHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSrcFileHdg.ClipTextForChildControls = true;
			this.pnlSrcFileHdg.ColorBottom = System.Drawing.Color.Empty;
			this.pnlSrcFileHdg.ColorTop = System.Drawing.Color.Empty;
			this.pnlSrcFileHdg.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlSrcFileHdg, "pnlSrcFileHdg");
			this.pnlSrcFileHdg.DoubleBuffered = true;
			this.pnlSrcFileHdg.DrawOnlyBottomBorder = true;
			this.pnlSrcFileHdg.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSrcFileHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlSrcFileHdg, "Heading above the contents of the data source file in the standard format data so" +
					"urce properties dialog box.");
			this.locExtender.SetLocalizingId(this.pnlSrcFileHdg, "SFDataSourcePropertiesDlg.pnlSrcFileHdg");
			this.pnlSrcFileHdg.MakeDark = false;
			this.pnlSrcFileHdg.MnemonicGeneratesClick = false;
			this.pnlSrcFileHdg.Name = "pnlSrcFileHdg";
			this.pnlSrcFileHdg.PaintExplorerBarBackground = false;
			this.pnlSrcFileHdg.BeforeDrawText += new SilTools.Controls.SilPanel.BeforeDrawTextHandler(this.HandleSourceFilePanelBeforeDrawText);
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
			this.splitOuter.TabStop = false;
			// 
			// pnlParseType
			// 
			resources.ApplyResources(this.pnlParseType, "pnlParseType");
			this.pnlParseType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlParseType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlParseType.ClipTextForChildControls = true;
			this.pnlParseType.ControlReceivingFocusOnMnemonic = null;
			this.pnlParseType.Controls.Add(this.tblLayoutParseOptions);
			this.pnlParseType.Controls.Add(this.pnlParseHdg);
			this.pnlParseType.DoubleBuffered = true;
			this.pnlParseType.DrawOnlyBottomBorder = false;
			this.pnlParseType.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlParseType, null);
			this.locExtender.SetLocalizationComment(this.pnlParseType, null);
			this.locExtender.SetLocalizationPriority(this.pnlParseType, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlParseType, "SFDataSourcePropertiesDlg.pnlParseType");
			this.pnlParseType.MnemonicGeneratesClick = false;
			this.pnlParseType.Name = "pnlParseType";
			this.pnlParseType.PaintExplorerBarBackground = false;
			// 
			// tblLayoutParseOptions
			// 
			resources.ApplyResources(this.tblLayoutParseOptions, "tblLayoutParseOptions");
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
			this.tblLayoutParseOptions.Name = "tblLayoutParseOptions";
			// 
			// lblParseType
			// 
			resources.ApplyResources(this.lblParseType, "lblParseType");
			this.lblParseType.AutoEllipsis = true;
			this.lblParseType.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblParseType, null);
			this.locExtender.SetLocalizationComment(this.lblParseType, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblParseType, "SFDataSourcePropertiesDlg.lblParseType");
			this.lblParseType.Name = "lblParseType";
			// 
			// rbInterlinearize
			// 
			resources.ApplyResources(this.rbInterlinearize, "rbInterlinearize");
			this.rbInterlinearize.AutoEllipsis = true;
			this.rbInterlinearize.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbInterlinearize, null);
			this.locExtender.SetLocalizationComment(this.rbInterlinearize, "Parsing option radio button in standard format data source properties dialog box." +
					"");
			this.locExtender.SetLocalizingId(this.rbInterlinearize, "SFDataSourcePropertiesDlg.rbInterlinearize");
			this.rbInterlinearize.Name = "rbInterlinearize";
			this.rbInterlinearize.TabStop = true;
			this.rbInterlinearize.UseVisualStyleBackColor = false;
			// 
			// rbParseOneToOne
			// 
			resources.ApplyResources(this.rbParseOneToOne, "rbParseOneToOne");
			this.rbParseOneToOne.AutoEllipsis = true;
			this.rbParseOneToOne.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbParseOneToOne, null);
			this.locExtender.SetLocalizationComment(this.rbParseOneToOne, "Parsing option radio button in standard format data source properties dialog box." +
					"");
			this.locExtender.SetLocalizingId(this.rbParseOneToOne, "SFDataSourcePropertiesDlg.rbParseOneToOne");
			this.rbParseOneToOne.Name = "rbParseOneToOne";
			this.rbParseOneToOne.TabStop = true;
			this.rbParseOneToOne.UseVisualStyleBackColor = false;
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
			// rbParseOnlyPhonetic
			// 
			resources.ApplyResources(this.rbParseOnlyPhonetic, "rbParseOnlyPhonetic");
			this.rbParseOnlyPhonetic.AutoEllipsis = true;
			this.rbParseOnlyPhonetic.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbParseOnlyPhonetic, null);
			this.locExtender.SetLocalizationComment(this.rbParseOnlyPhonetic, null);
			this.locExtender.SetLocalizingId(this.rbParseOnlyPhonetic, "SFDataSourcePropertiesDlg.rbParseOnlyPhonetic");
			this.rbParseOnlyPhonetic.Name = "rbParseOnlyPhonetic";
			this.rbParseOnlyPhonetic.TabStop = true;
			this.rbParseOnlyPhonetic.UseVisualStyleBackColor = false;
			// 
			// rbNoParse
			// 
			resources.ApplyResources(this.rbNoParse, "rbNoParse");
			this.rbNoParse.AutoEllipsis = true;
			this.rbNoParse.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbNoParse, null);
			this.locExtender.SetLocalizationComment(this.rbNoParse, null);
			this.locExtender.SetLocalizingId(this.rbNoParse, "SFDataSourcePropertiesDlg.rbNoParse");
			this.rbNoParse.Name = "rbNoParse";
			this.rbNoParse.TabStop = true;
			this.rbNoParse.UseVisualStyleBackColor = false;
			// 
			// pnlSampleInput
			// 
			resources.ApplyResources(this.pnlSampleInput, "pnlSampleInput");
			this.pnlSampleInput.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSampleInput.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlSampleInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSampleInput.ClipTextForChildControls = true;
			this.pnlSampleInput.ControlReceivingFocusOnMnemonic = null;
			this.pnlSampleInput.Controls.Add(this.rtfSampleInput);
			this.pnlSampleInput.DoubleBuffered = false;
			this.pnlSampleInput.DrawOnlyBottomBorder = false;
			this.pnlSampleInput.ForeColor = System.Drawing.SystemColors.ControlText;
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
			// cboFirstInterlinear
			// 
			resources.ApplyResources(this.cboFirstInterlinear, "cboFirstInterlinear");
			this.cboFirstInterlinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFirstInterlinear.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboFirstInterlinear, "Select the marker of the first\\ninterlinear field in each record.");
			this.locExtender.SetLocalizationComment(this.cboFirstInterlinear, null);
			this.locExtender.SetLocalizingId(this.cboFirstInterlinear, "SFDataSourcePropertiesDlg.cboFirstInterlinear");
			this.cboFirstInterlinear.Name = "cboFirstInterlinear";
			this.cboFirstInterlinear.SelectedIndexChanged += new System.EventHandler(this.HandleFirstInterlinearComboSelectedIndexChanged);
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
			// lblFirstInterlinear
			// 
			resources.ApplyResources(this.lblFirstInterlinear, "lblFirstInterlinear");
			this.lblFirstInterlinear.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblFirstInterlinear, null);
			this.locExtender.SetLocalizationComment(this.lblFirstInterlinear, "Label in standard format data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.lblFirstInterlinear, "SFDataSourcePropertiesDlg.lblFirstInterlinear");
			this.lblFirstInterlinear.Name = "lblFirstInterlinear";
			// 
			// pnlSampeOutput
			// 
			resources.ApplyResources(this.pnlSampeOutput, "pnlSampeOutput");
			this.pnlSampeOutput.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlSampeOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSampeOutput.ClipTextForChildControls = true;
			this.pnlSampeOutput.ControlReceivingFocusOnMnemonic = null;
			this.pnlSampeOutput.Controls.Add(this.gridSampleOutput);
			this.pnlSampeOutput.DoubleBuffered = true;
			this.pnlSampeOutput.DrawOnlyBottomBorder = false;
			this.pnlSampeOutput.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSampeOutput, null);
			this.locExtender.SetLocalizationComment(this.pnlSampeOutput, null);
			this.locExtender.SetLocalizationPriority(this.pnlSampeOutput, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSampeOutput, "SFDataSourcePropertiesDlg.pnlSampeOutput");
			this.pnlSampeOutput.MnemonicGeneratesClick = false;
			this.pnlSampeOutput.Name = "pnlSampeOutput";
			this.pnlSampeOutput.PaintExplorerBarBackground = false;
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
			resources.ApplyResources(this.gridSampleOutput, "gridSampleOutput");
			this.gridSampleOutput.DrawTextBoxEditControlBorder = false;
			this.gridSampleOutput.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.gridSampleOutput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.gridSampleOutput.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.gridSampleOutput, null);
			this.locExtender.SetLocalizationComment(this.gridSampleOutput, "Grid showing sample output in the standard format data source properties dialog b" +
					"ox.");
			this.locExtender.SetLocalizingId(this.gridSampleOutput, "SFDataSourcePropertiesDlg.gridSampleOutput");
			this.gridSampleOutput.MultiSelect = false;
			this.gridSampleOutput.Name = "gridSampleOutput";
			this.gridSampleOutput.PaintHeaderAcrossFullGridWidth = true;
			this.gridSampleOutput.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.gridSampleOutput.RowHeadersVisible = false;
			this.gridSampleOutput.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.gridSampleOutput.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.gridSampleOutput.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.gridSampleOutput.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.gridSampleOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.gridSampleOutput.ShowWaterMarkWhenDirty = false;
			this.gridSampleOutput.TabStop = false;
			this.gridSampleOutput.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.gridSampleOutput.WaterMark = "!";
			// 
			// SampleOutputPhoneticColumn
			// 
			resources.ApplyResources(this.SampleOutputPhoneticColumn, "SampleOutputPhoneticColumn");
			this.SampleOutputPhoneticColumn.Name = "SampleOutputPhoneticColumn";
			this.SampleOutputPhoneticColumn.ReadOnly = true;
			this.SampleOutputPhoneticColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// SampleOutputGlossColumn
			// 
			resources.ApplyResources(this.SampleOutputGlossColumn, "SampleOutputGlossColumn");
			this.SampleOutputGlossColumn.Name = "SampleOutputGlossColumn";
			this.SampleOutputGlossColumn.ReadOnly = true;
			this.SampleOutputGlossColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// SampleOutputPartOfSpeechColumn
			// 
			this.SampleOutputPartOfSpeechColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.SampleOutputPartOfSpeechColumn, "SampleOutputPartOfSpeechColumn");
			this.SampleOutputPartOfSpeechColumn.Name = "SampleOutputPartOfSpeechColumn";
			this.SampleOutputPartOfSpeechColumn.ReadOnly = true;
			this.SampleOutputPartOfSpeechColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// pnlParseHdg
			// 
			this.pnlParseHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlParseHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlParseHdg.ClipTextForChildControls = true;
			this.pnlParseHdg.ColorBottom = System.Drawing.Color.Empty;
			this.pnlParseHdg.ColorTop = System.Drawing.Color.Empty;
			this.pnlParseHdg.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlParseHdg, "pnlParseHdg");
			this.pnlParseHdg.DoubleBuffered = true;
			this.pnlParseHdg.DrawOnlyBottomBorder = true;
			this.pnlParseHdg.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlParseHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlParseHdg, "Heading above the parsing options in the standard format data source properties d" +
					"ialog box.");
			this.locExtender.SetLocalizingId(this.pnlParseHdg, "SFDataSourcePropertiesDlg.pnlParseHdg");
			this.pnlParseHdg.MakeDark = false;
			this.pnlParseHdg.MnemonicGeneratesClick = false;
			this.pnlParseHdg.Name = "pnlParseHdg";
			this.pnlParseHdg.PaintExplorerBarBackground = false;
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
