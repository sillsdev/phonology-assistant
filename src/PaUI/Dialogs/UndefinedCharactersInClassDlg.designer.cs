namespace SIL.Pa.UI.Dialogs
{
	partial class UndefinedCharactersInClassDlg
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UndefinedCharactersInClassDlg));
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblInfo = new System.Windows.Forms.Label();
			this.txtChars = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
			this.pnlButtons.Controls.Add(this.btnHelp);
			this.pnlButtons.Controls.Add(this.btnOK);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Button on dialog box displaying a list of undefined characters that are found in " +
					"a phone class.");
			this.locExtender.SetLocalizingId(this.btnHelp, "UndefinedCharactersInClassDlg.btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, "Button on dialog box displaying a list of undefined characters that are found in " +
					"a phone class.");
			this.locExtender.SetLocalizingId(this.btnOK, "UndefinedCharactersInClassDlg.btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.locExtender.SetLocalizableToolTip(this.lblInfo, null);
			this.locExtender.SetLocalizationComment(this.lblInfo, "Information text on dialog box displaying a list of undefined characters that are" +
					" found in a phone class.");
			this.locExtender.SetLocalizingId(this.lblInfo, "UndefinedCharactersInClassDlg.lblInfo");
			this.lblInfo.Name = "lblInfo";
			// 
			// txtChars
			// 
			this.txtChars.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.txtChars, "txtChars");
			this.locExtender.SetLocalizableToolTip(this.txtChars, null);
			this.locExtender.SetLocalizationComment(this.txtChars, null);
			this.locExtender.SetLocalizationPriority(this.txtChars, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtChars, "UndefinedCharactersInClassDlg.txtChars");
			this.txtChars.Name = "txtChars";
			this.txtChars.ReadOnly = true;
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblInfo, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtChars, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// UndefinedCharactersInClassDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOK;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.pnlButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "UndefinedCharactersInClassDlg.WindowTitle");
			this.Name = "UndefinedCharactersInClassDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.pnlButtons.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblInfo;
		protected System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.TextBox txtChars;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}