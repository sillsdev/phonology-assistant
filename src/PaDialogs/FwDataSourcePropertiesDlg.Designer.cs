namespace SIL.Pa.Dialogs
{
	partial class FwDataSourcePropertiesDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FwDataSourcePropertiesDlg));
			this.lblDatabase = new System.Windows.Forms.Label();
			this.lblDatabaseValue = new System.Windows.Forms.Label();
			this.lblLangProjValue = new System.Windows.Forms.Label();
			this.lblLangProj = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grpPhoneticDataStoreType = new System.Windows.Forms.GroupBox();
			this.rbPronunField = new System.Windows.Forms.RadioButton();
			this.rbLexForm = new System.Windows.Forms.RadioButton();
			this.grpWritingSystems = new System.Windows.Forms.GroupBox();
			this.cboNatGloss = new System.Windows.Forms.ComboBox();
			this.cboEngGloss = new System.Windows.Forms.ComboBox();
			this.cboOrtho = new System.Windows.Forms.ComboBox();
			this.cboPhonemic = new System.Windows.Forms.ComboBox();
			this.cboPhonetic = new System.Windows.Forms.ComboBox();
			this.lblPhonemic = new System.Windows.Forms.Label();
			this.lblOrtho = new System.Windows.Forms.Label();
			this.lblEngGloss = new System.Windows.Forms.Label();
			this.lblNatGloss = new System.Windows.Forms.Label();
			this.lblPhonetic = new System.Windows.Forms.Label();
			this.pnlButtons.SuspendLayout();
			this.panel1.SuspendLayout();
			this.grpPhoneticDataStoreType.SuspendLayout();
			this.grpWritingSystems.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
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
			// lblDatabase
			// 
			resources.ApplyResources(this.lblDatabase, "lblDatabase");
			this.lblDatabase.BackColor = System.Drawing.Color.Transparent;
			this.lblDatabase.Name = "lblDatabase";
			// 
			// lblDatabaseValue
			// 
			this.lblDatabaseValue.AutoEllipsis = true;
			resources.ApplyResources(this.lblDatabaseValue, "lblDatabaseValue");
			this.lblDatabaseValue.BackColor = System.Drawing.Color.Transparent;
			this.lblDatabaseValue.Name = "lblDatabaseValue";
			// 
			// lblLangProjValue
			// 
			this.lblLangProjValue.AutoEllipsis = true;
			resources.ApplyResources(this.lblLangProjValue, "lblLangProjValue");
			this.lblLangProjValue.BackColor = System.Drawing.Color.Transparent;
			this.lblLangProjValue.Name = "lblLangProjValue";
			// 
			// lblLangProj
			// 
			resources.ApplyResources(this.lblLangProj, "lblLangProj");
			this.lblLangProj.BackColor = System.Drawing.Color.Transparent;
			this.lblLangProj.Name = "lblLangProj";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.grpPhoneticDataStoreType);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// grpPhoneticDataStoreType
			// 
			this.grpPhoneticDataStoreType.Controls.Add(this.rbPronunField);
			this.grpPhoneticDataStoreType.Controls.Add(this.rbLexForm);
			resources.ApplyResources(this.grpPhoneticDataStoreType, "grpPhoneticDataStoreType");
			this.grpPhoneticDataStoreType.Name = "grpPhoneticDataStoreType";
			this.grpPhoneticDataStoreType.TabStop = false;
			// 
			// rbPronunField
			// 
			resources.ApplyResources(this.rbPronunField, "rbPronunField");
			this.rbPronunField.Name = "rbPronunField";
			this.rbPronunField.TabStop = true;
			this.rbPronunField.UseVisualStyleBackColor = true;
			this.rbPronunField.CheckedChanged += new System.EventHandler(this.HandlePhoneticStorageTypeCheckedChanged);
			// 
			// rbLexForm
			// 
			resources.ApplyResources(this.rbLexForm, "rbLexForm");
			this.rbLexForm.Name = "rbLexForm";
			this.rbLexForm.TabStop = true;
			this.rbLexForm.UseVisualStyleBackColor = true;
			this.rbLexForm.CheckedChanged += new System.EventHandler(this.HandlePhoneticStorageTypeCheckedChanged);
			// 
			// grpWritingSystems
			// 
			resources.ApplyResources(this.grpWritingSystems, "grpWritingSystems");
			this.grpWritingSystems.Controls.Add(this.cboNatGloss);
			this.grpWritingSystems.Controls.Add(this.cboEngGloss);
			this.grpWritingSystems.Controls.Add(this.cboOrtho);
			this.grpWritingSystems.Controls.Add(this.cboPhonemic);
			this.grpWritingSystems.Controls.Add(this.cboPhonetic);
			this.grpWritingSystems.Controls.Add(this.lblPhonemic);
			this.grpWritingSystems.Controls.Add(this.lblOrtho);
			this.grpWritingSystems.Controls.Add(this.lblEngGloss);
			this.grpWritingSystems.Controls.Add(this.lblNatGloss);
			this.grpWritingSystems.Controls.Add(this.lblPhonetic);
			this.grpWritingSystems.Name = "grpWritingSystems";
			this.grpWritingSystems.TabStop = false;
			// 
			// cboNatGloss
			// 
			resources.ApplyResources(this.cboNatGloss, "cboNatGloss");
			this.cboNatGloss.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboNatGloss.FormattingEnabled = true;
			this.cboNatGloss.Name = "cboNatGloss";
			this.cboNatGloss.SelectionChangeCommitted += new System.EventHandler(this.HandleWsSelectionChangeCommitted);
			// 
			// cboEngGloss
			// 
			resources.ApplyResources(this.cboEngGloss, "cboEngGloss");
			this.cboEngGloss.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEngGloss.FormattingEnabled = true;
			this.cboEngGloss.Name = "cboEngGloss";
			this.cboEngGloss.SelectionChangeCommitted += new System.EventHandler(this.HandleWsSelectionChangeCommitted);
			// 
			// cboOrtho
			// 
			resources.ApplyResources(this.cboOrtho, "cboOrtho");
			this.cboOrtho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOrtho.FormattingEnabled = true;
			this.cboOrtho.Name = "cboOrtho";
			this.cboOrtho.SelectionChangeCommitted += new System.EventHandler(this.HandleWsSelectionChangeCommitted);
			// 
			// cboPhonemic
			// 
			resources.ApplyResources(this.cboPhonemic, "cboPhonemic");
			this.cboPhonemic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPhonemic.FormattingEnabled = true;
			this.cboPhonemic.Name = "cboPhonemic";
			this.cboPhonemic.SelectionChangeCommitted += new System.EventHandler(this.HandleWsSelectionChangeCommitted);
			// 
			// cboPhonetic
			// 
			resources.ApplyResources(this.cboPhonetic, "cboPhonetic");
			this.cboPhonetic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPhonetic.FormattingEnabled = true;
			this.cboPhonetic.Name = "cboPhonetic";
			this.cboPhonetic.SelectionChangeCommitted += new System.EventHandler(this.HandleWsSelectionChangeCommitted);
			// 
			// lblPhonemic
			// 
			resources.ApplyResources(this.lblPhonemic, "lblPhonemic");
			this.lblPhonemic.BackColor = System.Drawing.Color.Transparent;
			this.lblPhonemic.Name = "lblPhonemic";
			// 
			// lblOrtho
			// 
			resources.ApplyResources(this.lblOrtho, "lblOrtho");
			this.lblOrtho.BackColor = System.Drawing.Color.Transparent;
			this.lblOrtho.Name = "lblOrtho";
			// 
			// lblEngGloss
			// 
			resources.ApplyResources(this.lblEngGloss, "lblEngGloss");
			this.lblEngGloss.BackColor = System.Drawing.Color.Transparent;
			this.lblEngGloss.Name = "lblEngGloss";
			// 
			// lblNatGloss
			// 
			resources.ApplyResources(this.lblNatGloss, "lblNatGloss");
			this.lblNatGloss.BackColor = System.Drawing.Color.Transparent;
			this.lblNatGloss.Name = "lblNatGloss";
			// 
			// lblPhonetic
			// 
			resources.ApplyResources(this.lblPhonetic, "lblPhonetic");
			this.lblPhonetic.BackColor = System.Drawing.Color.Transparent;
			this.lblPhonetic.Name = "lblPhonetic";
			// 
			// FwDataSourcePropertiesDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpWritingSystems);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblLangProj);
			this.Controls.Add(this.lblLangProjValue);
			this.Controls.Add(this.lblDatabase);
			this.Controls.Add(this.lblDatabaseValue);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FwDataSourcePropertiesDlg";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Auto;
			this.Controls.SetChildIndex(this.lblDatabaseValue, 0);
			this.Controls.SetChildIndex(this.lblDatabase, 0);
			this.Controls.SetChildIndex(this.lblLangProjValue, 0);
			this.Controls.SetChildIndex(this.lblLangProj, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.panel1, 0);
			this.Controls.SetChildIndex(this.grpWritingSystems, 0);
			this.pnlButtons.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.grpPhoneticDataStoreType.ResumeLayout(false);
			this.grpPhoneticDataStoreType.PerformLayout();
			this.grpWritingSystems.ResumeLayout(false);
			this.grpWritingSystems.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblDatabase;
		private System.Windows.Forms.Label lblDatabaseValue;
		private System.Windows.Forms.Label lblLangProjValue;
		private System.Windows.Forms.Label lblLangProj;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton rbPronunField;
		private System.Windows.Forms.RadioButton rbLexForm;
		private System.Windows.Forms.GroupBox grpPhoneticDataStoreType;
		private System.Windows.Forms.GroupBox grpWritingSystems;
		private System.Windows.Forms.ComboBox cboNatGloss;
		private System.Windows.Forms.ComboBox cboEngGloss;
		private System.Windows.Forms.ComboBox cboOrtho;
		private System.Windows.Forms.ComboBox cboPhonemic;
		private System.Windows.Forms.ComboBox cboPhonetic;
		private System.Windows.Forms.Label lblPhonemic;
		private System.Windows.Forms.Label lblOrtho;
		private System.Windows.Forms.Label lblEngGloss;
		private System.Windows.Forms.Label lblNatGloss;
		private System.Windows.Forms.Label lblPhonetic;
	}
}