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
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.webBrowser2 = new System.Windows.Forms.WebBrowser();
			this.lblMessage = new SilTools.Controls.AutoHeightLabel();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.btnOK = new System.Windows.Forms.Button();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayout
			// 
			this.tableLayout.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tableLayout.AutoSize = true;
			this.tableLayout.BackColor = System.Drawing.Color.Transparent;
			this.tableLayout.ColumnCount = 2;
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayout.Controls.Add(this.picIcon, 0, 0);
			this.tableLayout.Controls.Add(this.lblMessage, 1, 0);
			this.tableLayout.Controls.Add(this.webBrowser1, 1, 1);
			this.tableLayout.Controls.Add(this.webBrowser2, 1, 2);
			this.tableLayout.Location = new System.Drawing.Point(0, 0);
			this.tableLayout.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayout.MaximumSize = new System.Drawing.Size(565, 0);
			this.tableLayout.Name = "tableLayout";
			this.tableLayout.RowCount = 4;
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayout.Size = new System.Drawing.Size(565, 207);
			this.tableLayout.TabIndex = 0;
			// 
			// picIcon
			// 
			this.locExtender.SetLocalizableToolTip(this.picIcon, null);
			this.locExtender.SetLocalizationComment(this.picIcon, null);
			this.locExtender.SetLocalizationPriority(this.picIcon, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.picIcon, "DownloadSaDlg.picIcon");
			this.picIcon.Location = new System.Drawing.Point(13, 12);
			this.picIcon.Margin = new System.Windows.Forms.Padding(13, 12, 4, 4);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(64, 59);
			this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picIcon.TabIndex = 4;
			this.picIcon.TabStop = false;
			// 
			// webBrowser2
			// 
			this.locExtender.SetLocalizableToolTip(this.webBrowser2, null);
			this.locExtender.SetLocalizationComment(this.webBrowser2, null);
			this.locExtender.SetLocalizingId(this.webBrowser2, "webBrowser1");
			this.webBrowser2.Location = new System.Drawing.Point(84, 144);
			this.webBrowser2.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser2.Name = "webBrowser2";
			this.webBrowser2.ScrollBarsEnabled = false;
			this.webBrowser2.Size = new System.Drawing.Size(478, 60);
			this.webBrowser2.TabIndex = 7;
			this.webBrowser2.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser2_Navigating);
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)             | System.Windows.Forms.AnchorStyles.Left)             | System.Windows.Forms.AnchorStyles.Right)));            this.lblMessage.AutoEllipsis = true;            this.lblMessage.AutoSize = false;            this.lblMessage.Image = null;            this.locExtender.SetLocalizableToolTip(this.lblMessage, null);            this.locExtender.SetLocalizationComment(this.lblMessage, null);            this.locExtender.SetLocalizationPriority(this.lblMessage, L10NSharp.LocalizationPriority.NotLocalizable);            this.locExtender.SetLocalizingId(this.lblMessage, "DownloadSaDlg.MessageLabel");            this.lblMessage.Location = new System.Drawing.Point(94, 18);            this.lblMessage.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);            this.lblMessage.Name = "lblMessage";            this.lblMessage.Size = new System.Drawing.Size(458, 17);            this.lblMessage.TabIndex = 5;            this.lblMessage.Text = "#";
			// 
			// webBrowser1
			// 
			this.locExtender.SetLocalizableToolTip(this.webBrowser1, null);
			this.locExtender.SetLocalizationComment(this.webBrowser1, null);
			this.locExtender.SetLocalizingId(this.webBrowser1, "webBrowser1");
			this.webBrowser1.Location = new System.Drawing.Point(84, 78);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.ScrollBarsEnabled = false;
			this.webBrowser1.Size = new System.Drawing.Size(455, 60);
			this.webBrowser1.TabIndex = 6;
			this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating_1);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizingId(this.btnOK, "DialogBoxes.DownloadSaDlg.OKButton");
			this.btnOK.Location = new System.Drawing.Point(452, 246);
			this.btnOK.Margin = new System.Windows.Forms.Padding(4, 12, 13, 12);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(100, 32);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			this.locExtender.PrefixForNewItems = null;
			// 
			// DownloadSaDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(565, 288);
			this.Controls.Add(this.tableLayout);
			this.Controls.Add(this.btnOK);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.DownloadSaDlg.WindowTitle");
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DownloadSaDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Speech Analyzer Needed";
			this.tableLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayout;
		private System.Windows.Forms.Button btnOK;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.PictureBox picIcon;
		private SilTools.Controls.AutoHeightLabel lblMessage;
		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.WebBrowser webBrowser2;
	}
}