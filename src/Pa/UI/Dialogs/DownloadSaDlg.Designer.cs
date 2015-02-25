namespace SIL.Pa.UI.Dialogs
{
	partial class DownloadSaDlg
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
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lnkSaDownload = new System.Windows.Forms.LinkLabel();
            this.btnOK = new System.Windows.Forms.Button();
            this.lnkSaWebsite = new System.Windows.Forms.LinkLabel();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.lblMessage = new SilTools.Controls.AutoHeightLabel();
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            this.tableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayout
            // 
            this.tableLayout.AutoSize = true;
            this.tableLayout.BackColor = System.Drawing.Color.Transparent;
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Controls.Add(this.lnkSaDownload, 1, 1);
            this.tableLayout.Controls.Add(this.btnOK, 1, 3);
            this.tableLayout.Controls.Add(this.lnkSaWebsite, 1, 2);
            this.tableLayout.Controls.Add(this.picIcon, 0, 0);
            this.tableLayout.Controls.Add(this.lblMessage, 1, 0);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayout.Location = new System.Drawing.Point(0, 0);
            this.tableLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayout.MaximumSize = new System.Drawing.Size(565, 0);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 4;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.Size = new System.Drawing.Size(565, 257);
            this.tableLayout.TabIndex = 0;
            // 
            // lnkSaDownload
            // 
            this.lnkSaDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkSaDownload.AutoSize = true;
            this.lnkSaDownload.LinkArea = new System.Windows.Forms.LinkArea(58, 13);
            this.locExtender.SetLocalizableToolTip(this.lnkSaDownload, null);
            this.locExtender.SetLocalizationComment(this.lnkSaDownload, null);
            this.locExtender.SetLocalizingId(this.lnkSaDownload, "DialogBoxes.DownloadSaDlg.SaDownloadLink");
            this.lnkSaDownload.Location = new System.Drawing.Point(94, 93);
            this.lnkSaDownload.Margin = new System.Windows.Forms.Padding(13, 18, 13, 18);
            this.lnkSaDownload.Name = "lnkSaDownload";
            this.lnkSaDownload.Size = new System.Drawing.Size(458, 20);
            this.lnkSaDownload.TabIndex = 1;
            this.lnkSaDownload.TabStop = true;
            this.lnkSaDownload.Text = "Speech Analyzer can be downloaded from the SIL website by clicking here.";
            this.lnkSaDownload.UseCompatibleTextRendering = true;
            this.lnkSaDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSaDownload_LinkClicked);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.locExtender.SetLocalizableToolTip(this.btnOK, null);
            this.locExtender.SetLocalizationComment(this.btnOK, null);
            this.locExtender.SetLocalizingId(this.btnOK, "DialogBoxes.DownloadSaDlg.OKButton");
            this.btnOK.Location = new System.Drawing.Point(452, 213);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 12, 13, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lnkSaWebsite
            // 
            this.lnkSaWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkSaWebsite.AutoSize = true;
            this.lnkSaWebsite.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.locExtender.SetLocalizableToolTip(this.lnkSaWebsite, null);
            this.locExtender.SetLocalizationComment(this.lnkSaWebsite, null);
            this.locExtender.SetLocalizingId(this.lnkSaWebsite, "DialogBoxes.DownloadSaDlg.SaWebsiteLink");
            this.lnkSaWebsite.Location = new System.Drawing.Point(94, 149);
            this.lnkSaWebsite.Margin = new System.Windows.Forms.Padding(13, 18, 13, 18);
            this.lnkSaWebsite.Name = "lnkSaWebsite";
            this.lnkSaWebsite.Size = new System.Drawing.Size(458, 34);
            this.lnkSaWebsite.TabIndex = 2;
            this.lnkSaWebsite.Text = "For more information about Speech Analyzer, visit the SIL website at {0}.";
            this.lnkSaWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSaWebsite_LinkClicked);
            // 
            // picIcon
            // 
            this.locExtender.SetLocalizableToolTip(this.picIcon, null);
            this.locExtender.SetLocalizationComment(this.picIcon, null);
            this.locExtender.SetLocalizationPriority(this.picIcon, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.picIcon, "DownloadSaDlg.picIcon");
            this.picIcon.Location = new System.Drawing.Point(13, 12);
            this.picIcon.Margin = new System.Windows.Forms.Padding(13, 12, 4, 4);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(64, 59);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picIcon.TabIndex = 4;
            this.picIcon.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.AutoSize = true;
            this.lblMessage.Image = null;
            this.locExtender.SetLocalizableToolTip(this.lblMessage, null);
            this.locExtender.SetLocalizationComment(this.lblMessage, null);
            this.locExtender.SetLocalizationPriority(this.lblMessage, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.lblMessage, "DownloadSaDlg.MessageLabel");
            this.lblMessage.Location = new System.Drawing.Point(94, 18);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(13, 18, 13, 18);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(458, 17);
            this.lblMessage.TabIndex = 5;
            this.lblMessage.Text = "#";
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
            // 
            // DownloadSaDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(565, 260);
            this.Controls.Add(this.tableLayout);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizingId(this, "DialogBoxes.DownloadSaDlg.WindowTitle");
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadSaDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Speech Analyzer Needed";
            this.tableLayout.ResumeLayout(false);
            this.tableLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayout;
		private System.Windows.Forms.Button btnOK;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.LinkLabel lnkSaDownload;
		private System.Windows.Forms.LinkLabel lnkSaWebsite;
		private System.Windows.Forms.PictureBox picIcon;
		private SilTools.Controls.AutoHeightLabel lblMessage;
	}
}