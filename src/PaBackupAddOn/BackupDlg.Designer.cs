namespace SIL.Pa.AddOn
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
			this.btnBkup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBkup.Location = new System.Drawing.Point(208, 110);
			this.btnBkup.Name = "btnBkup";
			this.btnBkup.Size = new System.Drawing.Size(80, 26);
			this.btnBkup.TabIndex = 0;
			this.btnBkup.Text = "Backup";
			this.btnBkup.UseVisualStyleBackColor = true;
			this.btnBkup.Click += new System.EventHandler(this.btnBkup_Click);
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblInfo.AutoEllipsis = true;
			this.lblInfo.BackColor = System.Drawing.Color.Transparent;
			this.lblInfo.Location = new System.Drawing.Point(12, 18);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(366, 34);
			this.lblInfo.TabIndex = 1;
			this.lblInfo.Text = "The {0} project will be backed up to the file \'{1}\'";
			// 
			// chkIncludeDataSources
			// 
			this.chkIncludeDataSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkIncludeDataSources.BackColor = System.Drawing.Color.Transparent;
			this.chkIncludeDataSources.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkIncludeDataSources.Checked = true;
			this.chkIncludeDataSources.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIncludeDataSources.Location = new System.Drawing.Point(15, 65);
			this.chkIncludeDataSources.Name = "chkIncludeDataSources";
			this.chkIncludeDataSources.Size = new System.Drawing.Size(363, 40);
			this.chkIncludeDataSources.TabIndex = 2;
			this.chkIncludeDataSources.Text = "&Include data source files in backup file. (This does not include FieldWorks data" +
				" sources.)";
			this.chkIncludeDataSources.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkIncludeDataSources.UseVisualStyleBackColor = false;
			// 
			// prgBar
			// 
			this.prgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.prgBar.Location = new System.Drawing.Point(12, 68);
			this.prgBar.Name = "prgBar";
			this.prgBar.Size = new System.Drawing.Size(366, 23);
			this.prgBar.TabIndex = 3;
			this.prgBar.Visible = false;
			// 
			// lblProgress
			// 
			this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProgress.AutoSize = true;
			this.lblProgress.BackColor = System.Drawing.Color.Transparent;
			this.lblProgress.Location = new System.Drawing.Point(12, 50);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(78, 13);
			this.lblProgress.TabIndex = 4;
			this.lblProgress.Text = "Backing up {0}";
			this.lblProgress.Visible = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(298, 110);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 26);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// BackupDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(390, 148);
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
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Backup";
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