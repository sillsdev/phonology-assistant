namespace SIL.Pa.BackupRestoreAddOn
{
	partial class BackupDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupDlg));
			this.btnBkup = new System.Windows.Forms.Button();
			this.lblInfo = new System.Windows.Forms.Label();
			this.chkIncludeDataSources = new System.Windows.Forms.CheckBox();
			this.prgBar = new System.Windows.Forms.ProgressBar();
			this.lblProgress = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnBkup
			// 
			resources.ApplyResources(this.btnBkup, "btnBkup");
			this.btnBkup.Name = "btnBkup";
			this.btnBkup.UseVisualStyleBackColor = true;
			this.btnBkup.Click += new System.EventHandler(this.btnBkup_Click);
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.AutoEllipsis = true;
			this.lblInfo.BackColor = System.Drawing.Color.Transparent;
			this.lblInfo.Name = "lblInfo";
			// 
			// chkIncludeDataSources
			// 
			resources.ApplyResources(this.chkIncludeDataSources, "chkIncludeDataSources");
			this.chkIncludeDataSources.BackColor = System.Drawing.Color.Transparent;
			this.chkIncludeDataSources.Checked = true;
			this.chkIncludeDataSources.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIncludeDataSources.Name = "chkIncludeDataSources";
			this.chkIncludeDataSources.UseVisualStyleBackColor = false;
			// 
			// prgBar
			// 
			resources.ApplyResources(this.prgBar, "prgBar");
			this.prgBar.Name = "prgBar";
			// 
			// lblProgress
			// 
			resources.ApplyResources(this.lblProgress, "lblProgress");
			this.lblProgress.BackColor = System.Drawing.Color.Transparent;
			this.lblProgress.Name = "lblProgress";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// BackupDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnBkup);
			this.Controls.Add(this.lblProgress);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.prgBar);
			this.Controls.Add(this.chkIncludeDataSources);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BackupDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBkup;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.CheckBox chkIncludeDataSources;
		private System.Windows.Forms.ProgressBar prgBar;
		private System.Windows.Forms.Label lblProgress;
		private System.Windows.Forms.Button btnCancel;
	}
}