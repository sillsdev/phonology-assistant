namespace SIL.Pa
{
	partial class LaunchHTMLDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaunchHTMLDlg));
			this.lblQuestion = new System.Windows.Forms.Label();
			this.chkAlwaysOpen = new System.Windows.Forms.CheckBox();
			this.chkDontShowAgain = new System.Windows.Forms.CheckBox();
			this.btnNo = new System.Windows.Forms.Button();
			this.btnYes = new System.Windows.Forms.Button();
			this.btnHelp = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblQuestion
			// 
			resources.ApplyResources(this.lblQuestion, "lblQuestion");
			this.lblQuestion.Name = "lblQuestion";
			// 
			// chkAlwaysOpen
			// 
			resources.ApplyResources(this.chkAlwaysOpen, "chkAlwaysOpen");
			this.chkAlwaysOpen.Name = "chkAlwaysOpen";
			this.chkAlwaysOpen.UseVisualStyleBackColor = true;
			this.chkAlwaysOpen.CheckedChanged += new System.EventHandler(this.chkAlwaysOpen_CheckedChanged);
			// 
			// chkDontShowAgain
			// 
			resources.ApplyResources(this.chkDontShowAgain, "chkDontShowAgain");
			this.chkDontShowAgain.Name = "chkDontShowAgain";
			this.chkDontShowAgain.UseVisualStyleBackColor = true;
			// 
			// btnNo
			// 
			resources.ApplyResources(this.btnNo, "btnNo");
			this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnNo.Name = "btnNo";
			this.btnNo.UseVisualStyleBackColor = true;
			this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// 
			// btnYes
			// 
			resources.ApplyResources(this.btnYes, "btnYes");
			this.btnYes.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnYes.Name = "btnYes";
			this.btnYes.UseVisualStyleBackColor = true;
			this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// LaunchHTMLDlg
			// 
			this.AcceptButton = this.btnYes;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnNo;
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.btnYes);
			this.Controls.Add(this.chkDontShowAgain);
			this.Controls.Add(this.chkAlwaysOpen);
			this.Controls.Add(this.lblQuestion);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LaunchHTMLDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblQuestion;
		private System.Windows.Forms.CheckBox chkAlwaysOpen;
		private System.Windows.Forms.CheckBox chkDontShowAgain;
		protected System.Windows.Forms.Button btnNo;
		protected System.Windows.Forms.Button btnYes;
		protected System.Windows.Forms.Button btnHelp;
	}
}