namespace SIL.Pa.UI.Controls
{
	partial class SearchResultTabPopup
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
			this.components = new System.ComponentModel.Container();
			this.lblRecordCount = new System.Windows.Forms.Label();
			this.lblPattern = new System.Windows.Forms.Label();
			this.lblPatternValue = new System.Windows.Forms.Label();
			this.tlpInfo = new System.Windows.Forms.TableLayoutPanel();
			this.lblNameValue = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblRecordsValue = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tlpInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// lblRecordCount
			// 
			this.lblRecordCount.AutoSize = true;
			this.lblRecordCount.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblRecordCount, null);
			this.locExtender.SetLocalizationComment(this.lblRecordCount, null);
			this.locExtender.SetLocalizingId(this.lblRecordCount, "Views.WordLists.SearchResults.TabPopup.RecordCountLabel");
			this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
			this.lblRecordCount.Margin = new System.Windows.Forms.Padding(0);
			this.lblRecordCount.Name = "lblRecordCount";
			this.lblRecordCount.Size = new System.Drawing.Size(50, 13);
			this.lblRecordCount.TabIndex = 0;
			this.lblRecordCount.Text = "Records:";
			// 
			// lblPattern
			// 
			this.lblPattern.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPattern.AutoSize = true;
			this.lblPattern.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblPattern, null);
			this.locExtender.SetLocalizationComment(this.lblPattern, null);
			this.locExtender.SetLocalizingId(this.lblPattern, "Views.WordLists.SearchResults.TabPopup.PatternLabel");
			this.lblPattern.Location = new System.Drawing.Point(0, 26);
			this.lblPattern.Margin = new System.Windows.Forms.Padding(0);
			this.lblPattern.Name = "lblPattern";
			this.lblPattern.Size = new System.Drawing.Size(44, 13);
			this.lblPattern.TabIndex = 1;
			this.lblPattern.Text = "Pattern:";
			// 
			// lblPatternValue
			// 
			this.lblPatternValue.AutoSize = true;
			this.lblPatternValue.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblPatternValue, null);
			this.locExtender.SetLocalizationComment(this.lblPatternValue, null);
			this.locExtender.SetLocalizationPriority(this.lblPatternValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblPatternValue, "SearchResultTabPopup.lblPatternValue");
			this.lblPatternValue.Location = new System.Drawing.Point(53, 26);
			this.lblPatternValue.Name = "lblPatternValue";
			this.lblPatternValue.Size = new System.Drawing.Size(14, 13);
			this.lblPatternValue.TabIndex = 2;
			this.lblPatternValue.Text = "#";
			// 
			// tlpInfo
			// 
			this.tlpInfo.AutoSize = true;
			this.tlpInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpInfo.BackColor = System.Drawing.Color.Transparent;
			this.tlpInfo.ColumnCount = 2;
			this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpInfo.Controls.Add(this.lblPatternValue, 1, 2);
			this.tlpInfo.Controls.Add(this.lblNameValue, 1, 1);
			this.tlpInfo.Controls.Add(this.lblName, 0, 1);
			this.tlpInfo.Controls.Add(this.lblPattern, 0, 2);
			this.tlpInfo.Controls.Add(this.lblRecordCount, 0, 0);
			this.tlpInfo.Controls.Add(this.lblRecordsValue, 1, 0);
			this.tlpInfo.Location = new System.Drawing.Point(5, 5);
			this.tlpInfo.Margin = new System.Windows.Forms.Padding(0);
			this.tlpInfo.Name = "tlpInfo";
			this.tlpInfo.RowCount = 3;
			this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpInfo.Size = new System.Drawing.Size(70, 39);
			this.tlpInfo.TabIndex = 3;
			// 
			// lblNameValue
			// 
			this.lblNameValue.AutoSize = true;
			this.lblNameValue.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblNameValue, null);
			this.locExtender.SetLocalizationComment(this.lblNameValue, null);
			this.locExtender.SetLocalizationPriority(this.lblNameValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblNameValue, "SearchResultTabPopup.lblNameValue");
			this.lblNameValue.Location = new System.Drawing.Point(53, 13);
			this.lblNameValue.Name = "lblNameValue";
			this.lblNameValue.Size = new System.Drawing.Size(14, 13);
			this.lblNameValue.TabIndex = 3;
			this.lblNameValue.Text = "#";
			// 
			// lblName
			// 
			this.lblName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblName.AutoSize = true;
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblName, null);
			this.locExtender.SetLocalizationComment(this.lblName, null);
			this.locExtender.SetLocalizingId(this.lblName, "Views.WordLists.SearchResults.TabPopup.NameLabel");
			this.lblName.Location = new System.Drawing.Point(0, 13);
			this.lblName.Margin = new System.Windows.Forms.Padding(0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(38, 13);
			this.lblName.TabIndex = 2;
			this.lblName.Text = "Name:";
			// 
			// lblRecordsValue
			// 
			this.lblRecordsValue.AutoSize = true;
			this.lblRecordsValue.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblRecordsValue, null);
			this.locExtender.SetLocalizationComment(this.lblRecordsValue, null);
			this.locExtender.SetLocalizationPriority(this.lblRecordsValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblRecordsValue, "SearchResultTabPopup.lblRecordsValue");
			this.lblRecordsValue.Location = new System.Drawing.Point(53, 0);
			this.lblRecordsValue.Name = "lblRecordsValue";
			this.lblRecordsValue.Size = new System.Drawing.Size(14, 13);
			this.lblRecordsValue.TabIndex = 1;
			this.lblRecordsValue.Text = "#";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SearchResultTabPopup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.SystemColors.Info;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.tlpInfo);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SearchResultTabPopup.SearchResultTabPopup");
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SearchResultTabPopup";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(80, 49);
			this.tlpInfo.ResumeLayout(false);
			this.tlpInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblRecordCount;
		private System.Windows.Forms.Label lblPattern;
		private System.Windows.Forms.Label lblPatternValue;
		private System.Windows.Forms.TableLayoutPanel tlpInfo;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblRecordsValue;
		private System.Windows.Forms.Label lblNameValue;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
