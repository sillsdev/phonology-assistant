namespace SIL.Pa.DataSource
{
	partial class MissingFWDatabaseMsgBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingFWDatabaseMsgBox));
			this.lblMsg = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblDBName = new System.Windows.Forms.Label();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnHelp = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMsg
			// 
			resources.ApplyResources(this.lblMsg, "lblMsg");
			this.lblMsg.AutoEllipsis = true;
			this.lblMsg.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblMsg, null);
			this.locExtender.SetLocalizationComment(this.lblMsg, null);
			this.locExtender.SetLocalizingId(this.lblMsg, "MissingFWDatabaseMsgBox.lblMsg");
			this.lblMsg.Name = "lblMsg";
			// 
			// picIcon
			// 
			this.locExtender.SetLocalizableToolTip(this.picIcon, null);
			this.locExtender.SetLocalizationComment(this.picIcon, null);
			this.locExtender.SetLocalizationPriority(this.picIcon, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.picIcon, "MissingFWDatabaseMsgBox.picIcon");
			resources.ApplyResources(this.picIcon, "picIcon");
			this.picIcon.Name = "picIcon";
			this.picIcon.TabStop = false;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizingId(this.btnOK, "MissingFWDatabaseMsgBox.btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// lblDBName
			// 
			resources.ApplyResources(this.lblDBName, "lblDBName");
			this.lblDBName.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblDBName, null);
			this.locExtender.SetLocalizationComment(this.lblDBName, null);
			this.locExtender.SetLocalizationPriority(this.lblDBName, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblDBName, "MissingFWDatabaseMsgBox.lblDBName");
			this.lblDBName.Name = "lblDBName";
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizingId(this.btnHelp, "MissingFWDatabaseMsgBox.btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// MissingFWDatabaseMsgBox
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.lblDBName);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.lblMsg);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "MissingFWDatabaseMsgBox.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MissingFWDatabaseMsgBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblDBName;
		private System.Windows.Forms.ToolTip m_toolTip;
		private System.Windows.Forms.Button btnHelp;
		private Localization.UI.LocalizationExtender locExtender;
	}
}