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
			this.lblMsg = new System.Windows.Forms.Label();
			this.lstFwProjects = new System.Windows.Forms.ListBox();
			this.tvNetwork = new SIL.Pa.UI.Controls.NetworkTreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lblNetwork = new System.Windows.Forms.Label();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.lblProjects = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMsg
			// 
			this.lblMsg.AutoSize = true;
			this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.lblMsg, null);
			this.locExtender.SetLocalizationComment(this.lblMsg, null);
			this.locExtender.SetLocalizingId(this.lblMsg, "DialogBoxes.Fw6ProjectsDlg.PromptLabel");
			this.lblMsg.Location = new System.Drawing.Point(11, 10);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(305, 15);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "Choose the FieldWorks project to use as a data source.";
			// 
			// lstFwProjects
			// 
			this.lstFwProjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstFwProjects.FormattingEnabled = true;
			this.lstFwProjects.IntegralHeight = false;
			this.lstFwProjects.Location = new System.Drawing.Point(0, 23);
			this.lstFwProjects.Name = "lstFwProjects";
			this.lstFwProjects.Size = new System.Drawing.Size(211, 211);
			this.lstFwProjects.TabIndex = 1;
			// 
			// tvNetwork
			// 
			this.tvNetwork.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvNetwork.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.tvNetwork, null);
			this.locExtender.SetLocalizationComment(this.tvNetwork, null);
			this.locExtender.SetLocalizationPriority(this.tvNetwork, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tvNetwork, "FwProjectsDlg.tvNetwork");
			this.tvNetwork.Location = new System.Drawing.Point(0, 23);
			this.tvNetwork.Name = "tvNetwork";
			this.tvNetwork.Size = new System.Drawing.Size(234, 211);
			this.tvNetwork.TabIndex = 1;
			this.tvNetwork.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.HandleNetworkTreeViewAfterSelect);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(10, 32);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tvNetwork);
			this.splitContainer1.Panel1.Controls.Add(this.lblNetwork);
			this.splitContainer1.Panel1MinSize = 50;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtMsg);
			this.splitContainer1.Panel2.Controls.Add(this.lstFwProjects);
			this.splitContainer1.Panel2.Controls.Add(this.lblProjects);
			this.splitContainer1.Size = new System.Drawing.Size(451, 234);
			this.splitContainer1.SplitterDistance = 234;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 1;
			this.splitContainer1.TabStop = false;
			// 
			// lblNetwork
			// 
			this.lblNetwork.Dock = System.Windows.Forms.DockStyle.Top;
			this.locExtender.SetLocalizableToolTip(this.lblNetwork, null);
			this.locExtender.SetLocalizationComment(this.lblNetwork, "Label above the list of networks on the FieldWorks Projects dialog box.");
			this.locExtender.SetLocalizingId(this.lblNetwork, "DialogBoxes.Fw6ProjectsDlg.NetworkLabel");
			this.lblNetwork.Location = new System.Drawing.Point(0, 0);
			this.lblNetwork.Name = "lblNetwork";
			this.lblNetwork.Size = new System.Drawing.Size(234, 23);
			this.lblNetwork.TabIndex = 0;
			this.lblNetwork.Text = "&Look in:";
			this.lblNetwork.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtMsg
			// 
			this.txtMsg.BackColor = System.Drawing.SystemColors.Window;
			this.locExtender.SetLocalizableToolTip(this.txtMsg, null);
			this.locExtender.SetLocalizationComment(this.txtMsg, null);
			this.locExtender.SetLocalizationPriority(this.txtMsg, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtMsg, "FwProjectsDlg.txtMsg");
			this.txtMsg.Location = new System.Drawing.Point(43, 85);
			this.txtMsg.Multiline = true;
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.ReadOnly = true;
			this.txtMsg.Size = new System.Drawing.Size(131, 96);
			this.txtMsg.TabIndex = 2;
			this.txtMsg.Visible = false;
			// 
			// lblProjects
			// 
			this.lblProjects.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblProjects.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblProjects, null);
			this.locExtender.SetLocalizationComment(this.lblProjects, "Label above the list  of FieldWorks projects on the FieldWorks Projects dialog bo" +
					"x.");
			this.locExtender.SetLocalizingId(this.lblProjects, "DialogBoxes.Fw6ProjectsDlg.ProjectsLabel");
			this.lblProjects.Location = new System.Drawing.Point(0, 0);
			this.lblProjects.Name = "lblProjects";
			this.lblProjects.Size = new System.Drawing.Size(211, 23);
			this.lblProjects.TabIndex = 0;
			this.lblProjects.Text = "&Choose a project:";
			this.lblProjects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// FwProjectsDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(471, 310);
			this.Controls.Add(this.lblMsg);
			this.Controls.Add(this.splitContainer1);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Fw6ProjectsDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(375, 230);
			this.Name = "FwProjectsDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
			this.Text = "FieldWorks Projects";
			this.Controls.SetChildIndex(this.splitContainer1, 0);
			this.Controls.SetChildIndex(this.lblMsg, 0);
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
		private SIL.Pa.UI.Controls.NetworkTreeView tvNetwork;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label lblNetwork;
		private System.Windows.Forms.Label lblProjects;
		private System.Windows.Forms.TextBox txtMsg;
		private L10NSharp.UI.L10NSharpExtender locExtender;
	}
}
