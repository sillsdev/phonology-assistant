using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace SIL.Pa.Dialogs
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for SFMarkerMappingDlg.
	/// </summary>
	/// --------------------------------------------------------------------------------
	public partial class SFMarkerMappingDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SFMarkerMappingDlg));
			this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.cboToolboxSortField = new System.Windows.Forms.ComboBox();
			this.txtEditor = new System.Windows.Forms.TextBox();
			this.scImport = new System.Windows.Forms.SplitContainer();
			this.pnlMappingHdg = new SIL.Pa.Controls.PaGradientPanel();
			this.pnlEditor = new System.Windows.Forms.Panel();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.lblEditor = new System.Windows.Forms.Label();
			this.txtFilePreview = new System.Windows.Forms.TextBox();
			this.pnlSrcFile = new SIL.Pa.Controls.PaGradientPanel();
			this.lblFilename = new System.Windows.Forms.Label();
			this.cboFirstInterlinear = new System.Windows.Forms.ComboBox();
			this.lblFirstInterlinear = new System.Windows.Forms.Label();
			this.pnlParseType = new SIL.Pa.Controls.PaPanel();
			this.gridSampleOutput = new System.Windows.Forms.DataGridView();
			this.Phonetic = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Gloss = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.POS = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlSampleInput = new SIL.Pa.Controls.PaPanel();
			this.rtfSampleInput = new System.Windows.Forms.RichTextBox();
			this.rbParseOneToOne = new System.Windows.Forms.RadioButton();
			this.rbNoParse = new System.Windows.Forms.RadioButton();
			this.rbInterlinearize = new System.Windows.Forms.RadioButton();
			this.lblParseType = new System.Windows.Forms.Label();
			this.rbParseOnlyPhonetic = new System.Windows.Forms.RadioButton();
			this.lblSampleOutput = new System.Windows.Forms.Label();
			this.lblSampleInput = new System.Windows.Forms.Label();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.pnlParseHdg = new SIL.Pa.Controls.PaGradientPanel();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlToolboxSortField = new System.Windows.Forms.Panel();
			this.lblToolboxSortField = new System.Windows.Forms.Label();
			this.pnlButtons.SuspendLayout();
			this.scImport.Panel1.SuspendLayout();
			this.scImport.Panel2.SuspendLayout();
			this.scImport.SuspendLayout();
			this.pnlEditor.SuspendLayout();
			this.pnlSrcFile.SuspendLayout();
			this.pnlParseType.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridSampleOutput)).BeginInit();
			this.pnlSampleInput.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.pnlToolboxSortField.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.pnlToolboxSortField);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.SetChildIndex(this.pnlToolboxSortField, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// m_tooltip
			// 
			resources.ApplyResources(this.m_tooltip, "m_tooltip");
			// 
			// cboToolboxSortField
			// 
			this.cboToolboxSortField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboToolboxSortField.FormattingEnabled = true;
			resources.ApplyResources(this.cboToolboxSortField, "cboToolboxSortField");
			this.cboToolboxSortField.Name = "cboToolboxSortField";
			this.cboToolboxSortField.Sorted = true;
			this.m_tooltip.SetToolTip(this.cboToolboxSortField, resources.GetString("cboToolboxSortField.ToolTip"));
			// 
			// txtEditor
			// 
			resources.ApplyResources(this.txtEditor, "txtEditor");
			this.txtEditor.Name = "txtEditor";
			this.m_tooltip.SetToolTip(this.txtEditor, resources.GetString("txtEditor.ToolTip"));
			// 
			// scImport
			// 
			resources.ApplyResources(this.scImport, "scImport");
			this.scImport.Name = "scImport";
			// 
			// scImport.Panel1
			// 
			this.scImport.Panel1.Controls.Add(this.pnlMappingHdg);
			this.scImport.Panel1.Controls.Add(this.pnlEditor);
			// 
			// scImport.Panel2
			// 
			this.scImport.Panel2.Controls.Add(this.txtFilePreview);
			this.scImport.Panel2.Controls.Add(this.pnlSrcFile);
			this.scImport.TabStop = false;
			// 
			// pnlMappingHdg
			// 
			this.pnlMappingHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMappingHdg.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlMappingHdg, "pnlMappingHdg");
			this.pnlMappingHdg.DoubleBuffered = true;
			this.pnlMappingHdg.MakeDark = false;
			this.pnlMappingHdg.MnemonicGeneratesClick = false;
			this.pnlMappingHdg.Name = "pnlMappingHdg";
			this.pnlMappingHdg.PaintExplorerBarBackground = false;
			// 
			// pnlEditor
			// 
			resources.ApplyResources(this.pnlEditor, "pnlEditor");
			this.pnlEditor.Controls.Add(this.btnBrowse);
			this.pnlEditor.Controls.Add(this.txtEditor);
			this.pnlEditor.Controls.Add(this.lblEditor);
			this.pnlEditor.Name = "pnlEditor";
			// 
			// btnBrowse
			// 
			resources.ApplyResources(this.btnBrowse, "btnBrowse");
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// lblEditor
			// 
			resources.ApplyResources(this.lblEditor, "lblEditor");
			this.lblEditor.BackColor = System.Drawing.Color.Transparent;
			this.lblEditor.Name = "lblEditor";
			// 
			// txtFilePreview
			// 
			resources.ApplyResources(this.txtFilePreview, "txtFilePreview");
			this.txtFilePreview.Name = "txtFilePreview";
			this.txtFilePreview.ReadOnly = true;
			this.txtFilePreview.TabStop = false;
			// 
			// pnlSrcFile
			// 
			this.pnlSrcFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSrcFile.ControlReceivingFocusOnMnemonic = null;
			this.pnlSrcFile.Controls.Add(this.lblFilename);
			resources.ApplyResources(this.pnlSrcFile, "pnlSrcFile");
			this.pnlSrcFile.DoubleBuffered = true;
			this.pnlSrcFile.MakeDark = false;
			this.pnlSrcFile.MnemonicGeneratesClick = false;
			this.pnlSrcFile.Name = "pnlSrcFile";
			this.pnlSrcFile.PaintExplorerBarBackground = false;
			// 
			// lblFilename
			// 
			resources.ApplyResources(this.lblFilename, "lblFilename");
			this.lblFilename.BackColor = System.Drawing.Color.Transparent;
			this.lblFilename.Name = "lblFilename";
			this.lblFilename.MouseEnter += new System.EventHandler(this.lblFilename_MouseEnter);
			this.lblFilename.Paint += new System.Windows.Forms.PaintEventHandler(this.lblFilename_Paint);
			// 
			// cboFirstInterlinear
			// 
			this.cboFirstInterlinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.cboFirstInterlinear, "cboFirstInterlinear");
			this.cboFirstInterlinear.FormattingEnabled = true;
			this.cboFirstInterlinear.Name = "cboFirstInterlinear";
			this.cboFirstInterlinear.SelectedIndexChanged += new System.EventHandler(this.cboFirstInterlinear_SelectedIndexChanged);
			// 
			// lblFirstInterlinear
			// 
			resources.ApplyResources(this.lblFirstInterlinear, "lblFirstInterlinear");
			this.lblFirstInterlinear.BackColor = System.Drawing.Color.Transparent;
			this.lblFirstInterlinear.Name = "lblFirstInterlinear";
			// 
			// pnlParseType
			// 
			this.pnlParseType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlParseType.ControlReceivingFocusOnMnemonic = null;
			this.pnlParseType.Controls.Add(this.gridSampleOutput);
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
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gridSampleOutput.DefaultCellStyle = dataGridViewCellStyle1;
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
			// pnlSampleInput
			// 
			resources.ApplyResources(this.pnlSampleInput, "pnlSampleInput");
			this.pnlSampleInput.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSampleInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSampleInput.ControlReceivingFocusOnMnemonic = null;
			this.pnlSampleInput.Controls.Add(this.rtfSampleInput);
			this.pnlSampleInput.DoubleBuffered = false;
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
			// 
			// rbParseOneToOne
			// 
			this.rbParseOneToOne.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.rbParseOneToOne, "rbParseOneToOne");
			this.rbParseOneToOne.Name = "rbParseOneToOne";
			this.rbParseOneToOne.TabStop = true;
			this.rbParseOneToOne.UseVisualStyleBackColor = false;
			this.rbParseOneToOne.CheckedChanged += new System.EventHandler(this.HandleReadTypeCheckedChanged);
			// 
			// rbNoParse
			// 
			this.rbNoParse.AutoEllipsis = true;
			this.rbNoParse.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.rbNoParse, "rbNoParse");
			this.rbNoParse.Name = "rbNoParse";
			this.rbNoParse.TabStop = true;
			this.rbNoParse.UseVisualStyleBackColor = false;
			this.rbNoParse.CheckedChanged += new System.EventHandler(this.HandleReadTypeCheckedChanged);
			// 
			// rbInterlinearize
			// 
			this.rbInterlinearize.AutoEllipsis = true;
			this.rbInterlinearize.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.rbInterlinearize, "rbInterlinearize");
			this.rbInterlinearize.Name = "rbInterlinearize";
			this.rbInterlinearize.TabStop = true;
			this.rbInterlinearize.UseVisualStyleBackColor = false;
			this.rbInterlinearize.CheckedChanged += new System.EventHandler(this.HandleReadTypeCheckedChanged);
			// 
			// lblParseType
			// 
			this.lblParseType.AutoEllipsis = true;
			this.lblParseType.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblParseType, "lblParseType");
			this.lblParseType.Name = "lblParseType";
			// 
			// rbParseOnlyPhonetic
			// 
			this.rbParseOnlyPhonetic.AutoEllipsis = true;
			this.rbParseOnlyPhonetic.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.rbParseOnlyPhonetic, "rbParseOnlyPhonetic");
			this.rbParseOnlyPhonetic.Name = "rbParseOnlyPhonetic";
			this.rbParseOnlyPhonetic.TabStop = true;
			this.rbParseOnlyPhonetic.UseVisualStyleBackColor = false;
			this.rbParseOnlyPhonetic.CheckedChanged += new System.EventHandler(this.HandleReadTypeCheckedChanged);
			// 
			// lblSampleOutput
			// 
			resources.ApplyResources(this.lblSampleOutput, "lblSampleOutput");
			this.lblSampleOutput.BackColor = System.Drawing.Color.Transparent;
			this.lblSampleOutput.Name = "lblSampleOutput";
			// 
			// lblSampleInput
			// 
			resources.ApplyResources(this.lblSampleInput, "lblSampleInput");
			this.lblSampleInput.BackColor = System.Drawing.Color.Transparent;
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
			this.splitOuter.Panel1.Controls.Add(this.pnlParseHdg);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.scImport);
			// 
			// pnlParseHdg
			// 
			this.pnlParseHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlParseHdg.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlParseHdg, "pnlParseHdg");
			this.pnlParseHdg.DoubleBuffered = true;
			this.pnlParseHdg.MakeDark = false;
			this.pnlParseHdg.MnemonicGeneratesClick = false;
			this.pnlParseHdg.Name = "pnlParseHdg";
			this.pnlParseHdg.PaintExplorerBarBackground = false;
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
			// pnlToolboxSortField
			// 
			resources.ApplyResources(this.pnlToolboxSortField, "pnlToolboxSortField");
			this.pnlToolboxSortField.Controls.Add(this.lblToolboxSortField);
			this.pnlToolboxSortField.Controls.Add(this.cboToolboxSortField);
			this.pnlToolboxSortField.Name = "pnlToolboxSortField";
			// 
			// lblToolboxSortField
			// 
			resources.ApplyResources(this.lblToolboxSortField, "lblToolboxSortField");
			this.lblToolboxSortField.BackColor = System.Drawing.Color.Transparent;
			this.lblToolboxSortField.Name = "lblToolboxSortField";
			// 
			// SFMarkerMappingDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.splitOuter);
			this.Name = "SFMarkerMappingDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.splitOuter, 0);
			this.pnlButtons.ResumeLayout(false);
			this.scImport.Panel1.ResumeLayout(false);
			this.scImport.Panel2.ResumeLayout(false);
			this.scImport.Panel2.PerformLayout();
			this.scImport.ResumeLayout(false);
			this.pnlEditor.ResumeLayout(false);
			this.pnlEditor.PerformLayout();
			this.pnlSrcFile.ResumeLayout(false);
			this.pnlParseType.ResumeLayout(false);
			this.pnlParseType.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridSampleOutput)).EndInit();
			this.pnlSampleInput.ResumeLayout(false);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.pnlToolboxSortField.ResumeLayout(false);
			this.pnlToolboxSortField.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private Label lblFilename;
		private Label lblFirstInterlinear;
		private ComboBox cboFirstInterlinear;
		private SIL.Pa.Controls.PaPanel pnlParseType;
		private Label lblParseType;
		private SplitContainer splitOuter;
		private RadioButton rbParseOneToOne;
		private RadioButton rbNoParse;
		private RadioButton rbInterlinearize;
		private RadioButton rbParseOnlyPhonetic;
		private RichTextBox rtfSampleInput;
		private SIL.Pa.Controls.PaPanel pnlSampleInput;
		private DataGridView gridSampleOutput;
		private Label lblSampleInput;
		private Label lblSampleOutput;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn Phonetic;
		private DataGridViewTextBoxColumn Gloss;
		private DataGridViewTextBoxColumn POS;
		private Panel pnlToolboxSortField;
		private Label lblToolboxSortField;
		private ComboBox cboToolboxSortField;
		private Panel pnlEditor;
		private Label lblEditor;
		private TextBox txtEditor;
		private Button btnBrowse;
		private SIL.Pa.Controls.PaGradientPanel pnlParseHdg;
		private SIL.Pa.Controls.PaGradientPanel pnlMappingHdg;
		private SIL.Pa.Controls.PaGradientPanel pnlSrcFile;
	}
}
