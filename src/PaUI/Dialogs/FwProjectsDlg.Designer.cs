namespace SIL.Pa.UI.Dialogs
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
			this.tvNetwork = new SIL.Pa.UI.Controls.NetworkTreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lblNetwork = new System.Windows.Forms.Label();
			this.lblDB = new System.Windows.Forms.Label();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.pnlButtons.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
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
			// tvNetwork
			// 
			resources.ApplyResources(this.tvNetwork, "tvNetwork");
			this.tvNetwork.HideSelection = false;
			this.tvNetwork.Name = "tvNetwork";
			this.tvNetwork.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvNetwork_AfterSelect);
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tvNetwork);
			this.splitContainer1.Panel1.Controls.Add(this.lblNetwork);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtMsg);
			this.splitContainer1.Panel2.Controls.Add(this.lstFwProjects);
			this.splitContainer1.Panel2.Controls.Add(this.lblDB);
			this.splitContainer1.TabStop = false;
			// 
			// lblNetwork
			// 
			resources.ApplyResources(this.lblNetwork, "lblNetwork");
			this.lblNetwork.Name = "lblNetwork";
			// 
			// lblDB
			// 
			resources.ApplyResources(this.lblDB, "lblDB");
			this.lblDB.Name = "lblDB";
			// 
			// txtMsg
			// 
			this.txtMsg.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.txtMsg, "txtMsg");
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.ReadOnly = true;
			// 
			// FwProjectsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.lblMsg);
			this.Controls.Add(this.splitContainer1);
			this.Name = "FwProjectsDlg";
			this.Controls.SetChildIndex(this.splitContainer1, 0);
			this.Controls.SetChildIndex(this.lblMsg, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.pnlButtons.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.ListBox lstFwProjects;
		private System.Windows.Forms.Button btnProperties;
		private SIL.Pa.UI.Controls.NetworkTreeView tvNetwork;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label lblNetwork;
		private System.Windows.Forms.Label lblDB;
		private System.Windows.Forms.TextBox txtMsg;
	}
}
