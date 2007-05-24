namespace SIL.Pa.Dialogs
{
	partial class FwProjectsDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FwProjectsDlg));
			this.lblMsg = new System.Windows.Forms.Label();
			this.lstFwProjects = new System.Windows.Forms.ListBox();
			this.btnProperties = new System.Windows.Forms.Button();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnProperties);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnProperties, 0);
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
			// lblMsg
			// 
			resources.ApplyResources(this.lblMsg, "lblMsg");
			this.lblMsg.Name = "lblMsg";
			// 
			// lstFwProjects
			// 
			resources.ApplyResources(this.lstFwProjects, "lstFwProjects");
			this.lstFwProjects.FormattingEnabled = true;
			this.lstFwProjects.Name = "lstFwProjects";
			// 
			// btnProperties
			// 
			resources.ApplyResources(this.btnProperties, "btnProperties");
			this.btnProperties.Name = "btnProperties";
			this.btnProperties.UseVisualStyleBackColor = true;
			this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
			// 
			// FwProjectsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.lblMsg);
			this.Controls.Add(this.lstFwProjects);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FwProjectsDlg";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Auto;
			this.Controls.SetChildIndex(this.lstFwProjects, 0);
			this.Controls.SetChildIndex(this.lblMsg, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.ListBox lstFwProjects;
		private System.Windows.Forms.Button btnProperties;
	}
}
