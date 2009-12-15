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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FwProjectsDlg));
			this.lblMsg = new System.Windows.Forms.Label();
			this.lstFwProjects = new System.Windows.Forms.ListBox();
			this.btnProperties = new System.Windows.Forms.Button();
			this.tvNetwork = new SIL.Pa.UI.Controls.NetworkTreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lblNetwork = new System.Windows.Forms.Label();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.lblProjects = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
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
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizationPriority(this.btnCancel, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnCancel, "Localized in base class");
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizationPriority(this.btnOK, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnOK, "Localized in base class");
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizationPriority(this.btnHelp, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnHelp, "Localized in base class");
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// lblMsg
			// 
			resources.ApplyResources(this.lblMsg, "lblMsg");
			this.locExtender.SetLocalizableToolTip(this.lblMsg, null);
			this.locExtender.SetLocalizationComment(this.lblMsg, null);
			this.locExtender.SetLocalizingId(this.lblMsg, "FwProjectsDlg.lblMsg");
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
			this.locExtender.SetLocalizableToolTip(this.btnProperties, null);
			this.locExtender.SetLocalizationComment(this.btnProperties, "Text on button on FieldWorks Projects dialog box.");
			this.locExtender.SetLocalizingId(this.btnProperties, "FwProjectsDlg.btnProperties");
			resources.ApplyResources(this.btnProperties, "btnProperties");
			this.btnProperties.Name = "btnProperties";
			this.btnProperties.UseVisualStyleBackColor = true;
			this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
			// 
			// tvNetwork
			// 
			resources.ApplyResources(this.tvNetwork, "tvNetwork");
			this.tvNetwork.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.tvNetwork, null);
			this.locExtender.SetLocalizationComment(this.tvNetwork, null);
			this.locExtender.SetLocalizationPriority(this.tvNetwork, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tvNetwork, "FwProjectsDlg.tvNetwork");
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
			this.splitContainer1.Panel2.Controls.Add(this.lblProjects);
			this.splitContainer1.TabStop = false;
			// 
			// lblNetwork
			// 
			resources.ApplyResources(this.lblNetwork, "lblNetwork");
			this.locExtender.SetLocalizableToolTip(this.lblNetwork, null);
			this.locExtender.SetLocalizationComment(this.lblNetwork, "Label above the list of networks on the FieldWorks Projects dialog box.");
			this.locExtender.SetLocalizingId(this.lblNetwork, "FwProjectsDlg.lblNetwork");
			this.lblNetwork.Name = "lblNetwork";
			// 
			// txtMsg
			// 
			this.txtMsg.BackColor = System.Drawing.SystemColors.Window;
			this.locExtender.SetLocalizableToolTip(this.txtMsg, null);
			this.locExtender.SetLocalizationComment(this.txtMsg, null);
			this.locExtender.SetLocalizationPriority(this.txtMsg, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtMsg, "FwProjectsDlg.txtMsg");
			resources.ApplyResources(this.txtMsg, "txtMsg");
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.ReadOnly = true;
			// 
			// lblProjects
			// 
			resources.ApplyResources(this.lblProjects, "lblProjects");
			this.locExtender.SetLocalizableToolTip(this.lblProjects, null);
			this.locExtender.SetLocalizationComment(this.lblProjects, "Label above the list  of FieldWorks projects on the FieldWorks Projects dialog bo" +
					"x.");
			this.locExtender.SetLocalizingId(this.lblProjects, "FwProjectsDlg.lblProjects");
			this.lblProjects.Name = "lblProjects";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// FwProjectsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.lblMsg);
			this.Controls.Add(this.splitContainer1);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "FwProjectsDlg.WindowTitle");
			this.Name = "FwProjectsDlg";
			this.Controls.SetChildIndex(this.splitContainer1, 0);
			this.Controls.SetChildIndex(this.lblMsg, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.pnlButtons.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
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
		private System.Windows.Forms.Label lblProjects;
		private System.Windows.Forms.TextBox txtMsg;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}
