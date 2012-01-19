namespace SIL.Pa.UI.Controls
{
	partial class SearchQueryValidationErrorControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._linkEmail = new System.Windows.Forms.LinkLabel();
			this._labelHeading = new System.Windows.Forms.Label();
			this._panelScrolling = new System.Windows.Forms.Panel();
			this._linkCopyToClipboard = new System.Windows.Forms.LinkLabel();
			this._labelPattern = new System.Windows.Forms.Label();
			this._tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.Controls.Add(this._linkEmail, 0, 3);
			this._tableLayout.Controls.Add(this._labelHeading, 0, 1);
			this._tableLayout.Controls.Add(this._panelScrolling, 0, 2);
			this._tableLayout.Controls.Add(this._linkCopyToClipboard, 1, 3);
			this._tableLayout.Controls.Add(this._labelPattern, 0, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(1, 1);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 4;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(264, 234);
			this._tableLayout.TabIndex = 0;
			this._tableLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleOuterTableLayoutPaint);
			// 
			// _linkEmail
			// 
			this._linkEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._linkEmail.AutoSize = true;
			this._linkEmail.BackColor = System.Drawing.Color.Transparent;
			this._linkEmail.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this._linkEmail.LinkColor = System.Drawing.SystemColors.ActiveCaptionText;
			this._linkEmail.Location = new System.Drawing.Point(92, 214);
			this._linkEmail.Margin = new System.Windows.Forms.Padding(0, 7, 5, 7);
			this._linkEmail.Name = "_linkEmail";
			this._linkEmail.Size = new System.Drawing.Size(60, 13);
			this._linkEmail.TabIndex = 4;
			this._linkEmail.TabStop = true;
			this._linkEmail.Text = "Send Email";
			this._linkEmail.VisitedLinkColor = System.Drawing.SystemColors.ActiveCaptionText;
			this._linkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleSendEmailLinkClicked);
			// 
			// _labelHeading
			// 
			this._labelHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelHeading.AutoSize = true;
			this._labelHeading.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.SetColumnSpan(this._labelHeading, 2);
			this._labelHeading.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this._labelHeading.Location = new System.Drawing.Point(12, 31);
			this._labelHeading.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
			this._labelHeading.Name = "_labelHeading";
			this._labelHeading.Size = new System.Drawing.Size(240, 13);
			this._labelHeading.TabIndex = 0;
			this._labelHeading.Text = "#";
			// 
			// _panelScrolling
			// 
			this._panelScrolling.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelScrolling.AutoScroll = true;
			this._panelScrolling.BackColor = System.Drawing.Color.White;
			this._tableLayout.SetColumnSpan(this._panelScrolling, 2);
			this._panelScrolling.Location = new System.Drawing.Point(0, 53);
			this._panelScrolling.Margin = new System.Windows.Forms.Padding(0);
			this._panelScrolling.Name = "_panelScrolling";
			this._panelScrolling.Padding = new System.Windows.Forms.Padding(18, 12, 0, 0);
			this._panelScrolling.Size = new System.Drawing.Size(264, 154);
			this._panelScrolling.TabIndex = 1;
			// 
			// _linkCopyToClipboard
			// 
			this._linkCopyToClipboard.AutoSize = true;
			this._linkCopyToClipboard.BackColor = System.Drawing.Color.Transparent;
			this._linkCopyToClipboard.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this._linkCopyToClipboard.LinkColor = System.Drawing.SystemColors.ActiveCaptionText;
			this._linkCopyToClipboard.Location = new System.Drawing.Point(162, 214);
			this._linkCopyToClipboard.Margin = new System.Windows.Forms.Padding(5, 7, 12, 7);
			this._linkCopyToClipboard.Name = "_linkCopyToClipboard";
			this._linkCopyToClipboard.Size = new System.Drawing.Size(90, 13);
			this._linkCopyToClipboard.TabIndex = 3;
			this._linkCopyToClipboard.TabStop = true;
			this._linkCopyToClipboard.Text = "Copy to Clipboard";
			this._linkCopyToClipboard.VisitedLinkColor = System.Drawing.SystemColors.ActiveCaptionText;
			this._linkCopyToClipboard.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleCopyToClipboardLinkClicked);
			// 
			// _labelPattern
			// 
			this._labelPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelPattern.AutoSize = true;
			this._labelPattern.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.SetColumnSpan(this._labelPattern, 2);
			this._labelPattern.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this._labelPattern.Location = new System.Drawing.Point(12, 9);
			this._labelPattern.Margin = new System.Windows.Forms.Padding(12, 9, 12, 0);
			this._labelPattern.Name = "_labelPattern";
			this._labelPattern.Size = new System.Drawing.Size(240, 13);
			this._labelPattern.TabIndex = 5;
			this._labelPattern.Text = "#";
			// 
			// SearchQueryValidationErrorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.Name = "SearchQueryValidationErrorControl";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(266, 236);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Label _labelHeading;
		private System.Windows.Forms.Panel _panelScrolling;
		private System.Windows.Forms.LinkLabel _linkEmail;
		private System.Windows.Forms.LinkLabel _linkCopyToClipboard;
		private System.Windows.Forms.Label _labelPattern;
	}
}
