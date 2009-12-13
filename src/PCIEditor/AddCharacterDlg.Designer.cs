namespace SIL.Pa
{
	partial class AddCharacterDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCharacterDlg));
			this.txtHexValue = new System.Windows.Forms.TextBox();
			this.txtCharName = new System.Windows.Forms.TextBox();
			this.txtCharDesc = new System.Windows.Forms.TextBox();
			this.cboType = new System.Windows.Forms.ComboBox();
			this.cboSubType = new System.Windows.Forms.ComboBox();
			this.cboIgnoreType = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblIgnoreType = new System.Windows.Forms.Label();
			this.lblSubType = new System.Windows.Forms.Label();
			this.lblType = new System.Windows.Forms.Label();
			this.cboMoa = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblUnicodeValue = new System.Windows.Forms.Label();
			this.lblChar = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblUnicode = new System.Windows.Forms.Label();
			this.lblCharLable = new System.Windows.Forms.Label();
			this.lblUPlus = new System.Windows.Forms.Label();
			this.cboPoa = new System.Windows.Forms.ComboBox();
			this.grpSortOrder = new System.Windows.Forms.GroupBox();
			this.lblPOA = new System.Windows.Forms.Label();
			this.lblMOA = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.chkDottedCircle = new System.Windows.Forms.CheckBox();
			this.chkPreceedBaseChar = new System.Windows.Forms.CheckBox();
			this.chkIsBase = new System.Windows.Forms.CheckBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.cboChartGroup = new System.Windows.Forms.ComboBox();
			this.cboChartColumn = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.grpFeatures = new System.Windows.Forms.GroupBox();
			this.pnlBinary = new SilUtils.Controls.SilPanel();
			this.txtBinary = new System.Windows.Forms.TextBox();
			this.hlblBinary = new SIL.Pa.UI.Controls.HeaderLabel();
			this.btnBinary = new SIL.Pa.UI.Controls.XButton();
			this.paPanel1 = new SilUtils.Controls.SilPanel();
			this.txtArticulatory = new System.Windows.Forms.TextBox();
			this.hlblArticulatory = new SIL.Pa.UI.Controls.HeaderLabel();
			this.btnArticulatory = new SIL.Pa.UI.Controls.XButton();
			this.pnlButtons.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.grpSortOrder.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.grpFeatures.SuspendLayout();
			this.pnlBinary.SuspendLayout();
			this.hlblBinary.SuspendLayout();
			this.paPanel1.SuspendLayout();
			this.hlblArticulatory.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// txtHexValue
			// 
			resources.ApplyResources(this.txtHexValue, "txtHexValue");
			this.txtHexValue.Name = "txtHexValue";
			this.txtHexValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHexValue_KeyPress);
			this.txtHexValue.TextChanged += new System.EventHandler(this.txtHexValue_TextChanged);
			// 
			// txtCharName
			// 
			resources.ApplyResources(this.txtCharName, "txtCharName");
			this.txtCharName.Name = "txtCharName";
			// 
			// txtCharDesc
			// 
			resources.ApplyResources(this.txtCharDesc, "txtCharDesc");
			this.txtCharDesc.Name = "txtCharDesc";
			// 
			// cboType
			// 
			this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboType.FormattingEnabled = true;
			resources.ApplyResources(this.cboType, "cboType");
			this.cboType.Name = "cboType";
			this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
			// 
			// cboSubType
			// 
			this.cboSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSubType.FormattingEnabled = true;
			resources.ApplyResources(this.cboSubType, "cboSubType");
			this.cboSubType.Name = "cboSubType";
			// 
			// cboIgnoreType
			// 
			this.cboIgnoreType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboIgnoreType.FormattingEnabled = true;
			resources.ApplyResources(this.cboIgnoreType, "cboIgnoreType");
			this.cboIgnoreType.Name = "cboIgnoreType";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblIgnoreType);
			this.groupBox1.Controls.Add(this.lblSubType);
			this.groupBox1.Controls.Add(this.lblType);
			this.groupBox1.Controls.Add(this.cboType);
			this.groupBox1.Controls.Add(this.cboIgnoreType);
			this.groupBox1.Controls.Add(this.cboSubType);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// lblIgnoreType
			// 
			resources.ApplyResources(this.lblIgnoreType, "lblIgnoreType");
			this.lblIgnoreType.Name = "lblIgnoreType";
			// 
			// lblSubType
			// 
			resources.ApplyResources(this.lblSubType, "lblSubType");
			this.lblSubType.Name = "lblSubType";
			// 
			// lblType
			// 
			resources.ApplyResources(this.lblType, "lblType");
			this.lblType.Name = "lblType";
			// 
			// cboMoa
			// 
			resources.ApplyResources(this.cboMoa, "cboMoa");
			this.cboMoa.DropDownHeight = 300;
			this.cboMoa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMoa.FormattingEnabled = true;
			this.cboMoa.Name = "cboMoa";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lblUnicodeValue);
			this.groupBox2.Controls.Add(this.lblChar);
			this.groupBox2.Controls.Add(this.lblDescription);
			this.groupBox2.Controls.Add(this.lblName);
			this.groupBox2.Controls.Add(this.lblUnicode);
			this.groupBox2.Controls.Add(this.lblCharLable);
			this.groupBox2.Controls.Add(this.txtHexValue);
			this.groupBox2.Controls.Add(this.txtCharName);
			this.groupBox2.Controls.Add(this.txtCharDesc);
			this.groupBox2.Controls.Add(this.lblUPlus);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// lblUnicodeValue
			// 
			resources.ApplyResources(this.lblUnicodeValue, "lblUnicodeValue");
			this.lblUnicodeValue.Name = "lblUnicodeValue";
			// 
			// lblChar
			// 
			resources.ApplyResources(this.lblChar, "lblChar");
			this.lblChar.Name = "lblChar";
			// 
			// lblDescription
			// 
			resources.ApplyResources(this.lblDescription, "lblDescription");
			this.lblDescription.Name = "lblDescription";
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			// 
			// lblUnicode
			// 
			resources.ApplyResources(this.lblUnicode, "lblUnicode");
			this.lblUnicode.Name = "lblUnicode";
			// 
			// lblCharLable
			// 
			resources.ApplyResources(this.lblCharLable, "lblCharLable");
			this.lblCharLable.Name = "lblCharLable";
			// 
			// lblUPlus
			// 
			resources.ApplyResources(this.lblUPlus, "lblUPlus");
			this.lblUPlus.Name = "lblUPlus";
			// 
			// cboPoa
			// 
			resources.ApplyResources(this.cboPoa, "cboPoa");
			this.cboPoa.DropDownHeight = 300;
			this.cboPoa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPoa.FormattingEnabled = true;
			this.cboPoa.Name = "cboPoa";
			// 
			// grpSortOrder
			// 
			this.grpSortOrder.Controls.Add(this.lblPOA);
			this.grpSortOrder.Controls.Add(this.lblMOA);
			this.grpSortOrder.Controls.Add(this.label4);
			this.grpSortOrder.Controls.Add(this.cboMoa);
			this.grpSortOrder.Controls.Add(this.cboPoa);
			resources.ApplyResources(this.grpSortOrder, "grpSortOrder");
			this.grpSortOrder.Name = "grpSortOrder";
			this.grpSortOrder.TabStop = false;
			// 
			// lblPOA
			// 
			resources.ApplyResources(this.lblPOA, "lblPOA");
			this.lblPOA.Name = "lblPOA";
			// 
			// lblMOA
			// 
			resources.ApplyResources(this.lblMOA, "lblMOA");
			this.lblMOA.Name = "lblMOA";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.chkDottedCircle);
			this.groupBox4.Controls.Add(this.chkPreceedBaseChar);
			this.groupBox4.Controls.Add(this.chkIsBase);
			resources.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			// 
			// chkDottedCircle
			// 
			resources.ApplyResources(this.chkDottedCircle, "chkDottedCircle");
			this.chkDottedCircle.Name = "chkDottedCircle";
			this.chkDottedCircle.UseVisualStyleBackColor = true;
			// 
			// chkPreceedBaseChar
			// 
			resources.ApplyResources(this.chkPreceedBaseChar, "chkPreceedBaseChar");
			this.chkPreceedBaseChar.Name = "chkPreceedBaseChar";
			this.chkPreceedBaseChar.UseVisualStyleBackColor = true;
			// 
			// chkIsBase
			// 
			resources.ApplyResources(this.chkIsBase, "chkIsBase");
			this.chkIsBase.Name = "chkIsBase";
			this.chkIsBase.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.label11);
			this.groupBox5.Controls.Add(this.label10);
			this.groupBox5.Controls.Add(this.cboChartGroup);
			this.groupBox5.Controls.Add(this.cboChartColumn);
			resources.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			// 
			// label11
			// 
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			// 
			// cboChartGroup
			// 
			this.cboChartGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChartGroup.FormattingEnabled = true;
			resources.ApplyResources(this.cboChartGroup, "cboChartGroup");
			this.cboChartGroup.Name = "cboChartGroup";
			// 
			// cboChartColumn
			// 
			this.cboChartColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChartColumn.FormattingEnabled = true;
			resources.ApplyResources(this.cboChartColumn, "cboChartColumn");
			this.cboChartColumn.Name = "cboChartColumn";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// grpFeatures
			// 
			resources.ApplyResources(this.grpFeatures, "grpFeatures");
			this.grpFeatures.Controls.Add(this.pnlBinary);
			this.grpFeatures.Controls.Add(this.paPanel1);
			this.grpFeatures.Name = "grpFeatures";
			this.grpFeatures.TabStop = false;
			// 
			// pnlBinary
			// 
			resources.ApplyResources(this.pnlBinary, "pnlBinary");
			this.pnlBinary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBinary.ControlReceivingFocusOnMnemonic = null;
			this.pnlBinary.Controls.Add(this.txtBinary);
			this.pnlBinary.Controls.Add(this.hlblBinary);
			this.pnlBinary.DoubleBuffered = false;
			this.pnlBinary.MnemonicGeneratesClick = false;
			this.pnlBinary.Name = "pnlBinary";
			this.pnlBinary.PaintExplorerBarBackground = false;
			// 
			// txtBinary
			// 
			this.txtBinary.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.txtBinary, "txtBinary");
			this.txtBinary.Name = "txtBinary";
			// 
			// hlblBinary
			// 
			this.hlblBinary.ControlReceivingFocusOnMnemonic = null;
			this.hlblBinary.Controls.Add(this.btnBinary);
			resources.ApplyResources(this.hlblBinary, "hlblBinary");
			this.hlblBinary.MnemonicGeneratesClick = true;
			this.hlblBinary.Name = "hlblBinary";
			this.hlblBinary.ShowWindowBackgroudOnTopAndRightEdge = true;
			this.hlblBinary.Click += new System.EventHandler(this.btnBinary_Click);
			// 
			// btnBinary
			// 
			resources.ApplyResources(this.btnBinary, "btnBinary");
			this.btnBinary.BackColor = System.Drawing.Color.Transparent;
			this.btnBinary.CanBeChecked = false;
			this.btnBinary.Checked = false;
			this.btnBinary.DrawEmpty = false;
			this.btnBinary.DrawLeftArrowButton = false;
			this.btnBinary.DrawRightArrowButton = false;
			this.btnBinary.Image = null;
			this.btnBinary.Name = "btnBinary";
			this.btnBinary.Click += new System.EventHandler(this.btnBinary_Click);
			// 
			// paPanel1
			// 
			this.paPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.paPanel1.ControlReceivingFocusOnMnemonic = null;
			this.paPanel1.Controls.Add(this.txtArticulatory);
			this.paPanel1.Controls.Add(this.hlblArticulatory);
			this.paPanel1.DoubleBuffered = false;
			resources.ApplyResources(this.paPanel1, "paPanel1");
			this.paPanel1.MnemonicGeneratesClick = false;
			this.paPanel1.Name = "paPanel1";
			this.paPanel1.PaintExplorerBarBackground = false;
			// 
			// txtArticulatory
			// 
			this.txtArticulatory.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.txtArticulatory, "txtArticulatory");
			this.txtArticulatory.Name = "txtArticulatory";
			// 
			// hlblArticulatory
			// 
			this.hlblArticulatory.ControlReceivingFocusOnMnemonic = null;
			this.hlblArticulatory.Controls.Add(this.btnArticulatory);
			resources.ApplyResources(this.hlblArticulatory, "hlblArticulatory");
			this.hlblArticulatory.MnemonicGeneratesClick = true;
			this.hlblArticulatory.Name = "hlblArticulatory";
			this.hlblArticulatory.ShowWindowBackgroudOnTopAndRightEdge = true;
			this.hlblArticulatory.Click += new System.EventHandler(this.btnArticulatory_Click);
			// 
			// btnArticulatory
			// 
			resources.ApplyResources(this.btnArticulatory, "btnArticulatory");
			this.btnArticulatory.BackColor = System.Drawing.Color.Transparent;
			this.btnArticulatory.CanBeChecked = false;
			this.btnArticulatory.Checked = false;
			this.btnArticulatory.DrawEmpty = false;
			this.btnArticulatory.DrawLeftArrowButton = false;
			this.btnArticulatory.DrawRightArrowButton = false;
			this.btnArticulatory.Image = null;
			this.btnArticulatory.Name = "btnArticulatory";
			this.btnArticulatory.Click += new System.EventHandler(this.btnArticulatory_Click);
			// 
			// AddCharacterDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpFeatures);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.grpSortOrder);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AddCharacterDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.groupBox1, 0);
			this.Controls.SetChildIndex(this.groupBox2, 0);
			this.Controls.SetChildIndex(this.grpSortOrder, 0);
			this.Controls.SetChildIndex(this.groupBox4, 0);
			this.Controls.SetChildIndex(this.groupBox5, 0);
			this.Controls.SetChildIndex(this.grpFeatures, 0);
			this.pnlButtons.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.grpSortOrder.ResumeLayout(false);
			this.grpSortOrder.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.grpFeatures.ResumeLayout(false);
			this.pnlBinary.ResumeLayout(false);
			this.pnlBinary.PerformLayout();
			this.hlblBinary.ResumeLayout(false);
			this.paPanel1.ResumeLayout(false);
			this.paPanel1.PerformLayout();
			this.hlblArticulatory.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox txtHexValue;
		private System.Windows.Forms.TextBox txtCharName;
		private System.Windows.Forms.TextBox txtCharDesc;
		private System.Windows.Forms.ComboBox cboType;
		private System.Windows.Forms.ComboBox cboSubType;
		private System.Windows.Forms.ComboBox cboIgnoreType;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblIgnoreType;
		private System.Windows.Forms.Label lblSubType;
		private System.Windows.Forms.Label lblType;
		private System.Windows.Forms.ComboBox cboMoa;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblUnicode;
		private System.Windows.Forms.Label lblCharLable;
		private System.Windows.Forms.ComboBox cboPoa;
		private System.Windows.Forms.Label lblChar;
		private System.Windows.Forms.Label lblUnicodeValue;
		private System.Windows.Forms.GroupBox grpSortOrder;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox chkDottedCircle;
		private System.Windows.Forms.CheckBox chkPreceedBaseChar;
		private System.Windows.Forms.CheckBox chkIsBase;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cboChartGroup;
		private System.Windows.Forms.ComboBox cboChartColumn;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox grpFeatures;
		private SilUtils.Controls.SilPanel paPanel1;
		private System.Windows.Forms.TextBox txtArticulatory;
		private SIL.Pa.UI.Controls.HeaderLabel hlblArticulatory;
		private SIL.Pa.UI.Controls.XButton btnArticulatory;
		private SilUtils.Controls.SilPanel pnlBinary;
		private System.Windows.Forms.TextBox txtBinary;
		private SIL.Pa.UI.Controls.HeaderLabel hlblBinary;
		private SIL.Pa.UI.Controls.XButton btnBinary;
		private System.Windows.Forms.Label lblUPlus;
		private System.Windows.Forms.Label lblMOA;
		private System.Windows.Forms.Label lblPOA;

	}
}

