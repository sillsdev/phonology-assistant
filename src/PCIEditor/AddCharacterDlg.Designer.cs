
using SilTools.Controls;

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
			this.label5 = new System.Windows.Forms.Label();
			this._featuresTab = new SIL.Pa.UI.Controls.FeaturesTab();
			this.chkIsBase = new System.Windows.Forms.CheckBox();
			this.chkPreceedBaseChar = new System.Windows.Forms.CheckBox();
			this.chkDottedCircle = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.grpSortOrder.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtHexValue
			// 
			resources.ApplyResources(this.txtHexValue, "txtHexValue");
			this.txtHexValue.Name = "txtHexValue";
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
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.lblIgnoreType);
			this.groupBox1.Controls.Add(this.lblSubType);
			this.groupBox1.Controls.Add(this.lblType);
			this.groupBox1.Controls.Add(this.cboType);
			this.groupBox1.Controls.Add(this.cboIgnoreType);
			this.groupBox1.Controls.Add(this.cboSubType);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// lblIgnoreType
			// 
			resources.ApplyResources(this.lblIgnoreType, "lblIgnoreType");
			this.lblIgnoreType.BackColor = System.Drawing.Color.Transparent;
			this.lblIgnoreType.Name = "lblIgnoreType";
			// 
			// lblSubType
			// 
			resources.ApplyResources(this.lblSubType, "lblSubType");
			this.lblSubType.BackColor = System.Drawing.Color.Transparent;
			this.lblSubType.Name = "lblSubType";
			// 
			// lblType
			// 
			resources.ApplyResources(this.lblType, "lblType");
			this.lblType.BackColor = System.Drawing.Color.Transparent;
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
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Name = "label4";
			// 
			// groupBox2
			// 
			resources.ApplyResources(this.groupBox2, "groupBox2");
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
			this.lblDescription.BackColor = System.Drawing.Color.Transparent;
			this.lblDescription.Name = "lblDescription";
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			this.lblName.Name = "lblName";
			// 
			// lblUnicode
			// 
			resources.ApplyResources(this.lblUnicode, "lblUnicode");
			this.lblUnicode.BackColor = System.Drawing.Color.Transparent;
			this.lblUnicode.Name = "lblUnicode";
			// 
			// lblCharLable
			// 
			resources.ApplyResources(this.lblCharLable, "lblCharLable");
			this.lblCharLable.BackColor = System.Drawing.Color.Transparent;
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
			resources.ApplyResources(this.grpSortOrder, "grpSortOrder");
			this.grpSortOrder.Controls.Add(this.lblPOA);
			this.grpSortOrder.Controls.Add(this.lblMOA);
			this.grpSortOrder.Controls.Add(this.label4);
			this.grpSortOrder.Controls.Add(this.cboMoa);
			this.grpSortOrder.Controls.Add(this.cboPoa);
			this.grpSortOrder.Name = "grpSortOrder";
			this.grpSortOrder.TabStop = false;
			// 
			// lblPOA
			// 
			resources.ApplyResources(this.lblPOA, "lblPOA");
			this.lblPOA.BackColor = System.Drawing.Color.Transparent;
			this.lblPOA.Name = "lblPOA";
			// 
			// lblMOA
			// 
			resources.ApplyResources(this.lblMOA, "lblMOA");
			this.lblMOA.BackColor = System.Drawing.Color.Transparent;
			this.lblMOA.Name = "lblMOA";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// _featuresTab
			// 
			resources.ApplyResources(this._featuresTab, "_featuresTab");
			this._featuresTab.BackColor = System.Drawing.Color.Transparent;
			this._featuresTab.Name = "_featuresTab";
			// 
			// chkIsBase
			// 
			resources.ApplyResources(this.chkIsBase, "chkIsBase");
			this.chkIsBase.BackColor = System.Drawing.Color.Transparent;
			this.chkIsBase.Name = "chkIsBase";
			this.chkIsBase.UseVisualStyleBackColor = false;
			// 
			// chkPreceedBaseChar
			// 
			resources.ApplyResources(this.chkPreceedBaseChar, "chkPreceedBaseChar");
			this.chkPreceedBaseChar.BackColor = System.Drawing.Color.Transparent;
			this.chkPreceedBaseChar.Name = "chkPreceedBaseChar";
			this.chkPreceedBaseChar.UseVisualStyleBackColor = false;
			// 
			// chkDottedCircle
			// 
			resources.ApplyResources(this.chkDottedCircle, "chkDottedCircle");
			this.chkDottedCircle.BackColor = System.Drawing.Color.Transparent;
			this.chkDottedCircle.Name = "chkDottedCircle";
			this.chkDottedCircle.UseVisualStyleBackColor = false;
			// 
			// AddCharacterDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkDottedCircle);
			this.Controls.Add(this.chkPreceedBaseChar);
			this.Controls.Add(this.chkIsBase);
			this.Controls.Add(this.grpSortOrder);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this._featuresTab);
			this.Controls.Add(this.groupBox1);
			this.Name = "AddCharacterDlg";
			this.Controls.SetChildIndex(this.groupBox1, 0);
			this.Controls.SetChildIndex(this._featuresTab, 0);
			this.Controls.SetChildIndex(this.groupBox2, 0);
			this.Controls.SetChildIndex(this.grpSortOrder, 0);
			this.Controls.SetChildIndex(this.chkIsBase, 0);
			this.Controls.SetChildIndex(this.chkPreceedBaseChar, 0);
			this.Controls.SetChildIndex(this.chkDottedCircle, 0);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.grpSortOrder.ResumeLayout(false);
			this.grpSortOrder.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblUPlus;
		private System.Windows.Forms.Label lblMOA;
		private System.Windows.Forms.Label lblPOA;
		private UI.Controls.FeaturesTab _featuresTab;
		private System.Windows.Forms.CheckBox chkIsBase;
		private System.Windows.Forms.CheckBox chkPreceedBaseChar;
		private System.Windows.Forms.CheckBox chkDottedCircle;

	}
}

