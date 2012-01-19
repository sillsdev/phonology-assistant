namespace SIL.Pa.UI.Controls
{
	partial class PhoneInfoPopupContent
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
			this.lblNormally = new System.Windows.Forms.Label();
			this.lblPrimary = new System.Windows.Forms.Label();
			this.lblNonPrimary = new System.Windows.Forms.Label();
			this.lblNormallyCount = new System.Windows.Forms.Label();
			this.lblPrimaryCount = new System.Windows.Forms.Label();
			this.lblNonPrimaryCount = new System.Windows.Forms.Label();
			this.lblCountHeading = new System.Windows.Forms.Label();
			this.lblUncertaintyHeading = new System.Windows.Forms.Label();
			this.lblSiblingPhones = new System.Windows.Forms.Label();
			this.pnlHeading = new System.Windows.Forms.Panel();
			this.pnlMonogram = new System.Windows.Forms.Panel();
			this.lblMonogram = new System.Windows.Forms.Label();
			this.pnlInfo = new System.Windows.Forms.Panel();
			this.pnlCounts = new System.Windows.Forms.Panel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlHeading.SuspendLayout();
			this.pnlMonogram.SuspendLayout();
			this.pnlInfo.SuspendLayout();
			this.pnlCounts.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// lblNormally
			// 
			this.lblNormally.AutoSize = true;
			this.lblNormally.BackColor = System.Drawing.Color.Transparent;
			this.lblNormally.Font = new System.Drawing.Font("Arial", 9F);
			this.lblNormally.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblNormally, null);
			this.locExtender.SetLocalizationComment(this.lblNormally, null);
			this.locExtender.SetLocalizingId(this.lblNormally, "CommonControls.PhoneInfoPopupContent.NormallyLabel");
			this.lblNormally.Location = new System.Drawing.Point(0, 4);
			this.lblNormally.Name = "lblNormally";
			this.lblNormally.Size = new System.Drawing.Size(59, 15);
			this.lblNormally.TabIndex = 1;
			this.lblNormally.Text = "Normally:";
			// 
			// lblPrimary
			// 
			this.lblPrimary.AutoSize = true;
			this.lblPrimary.BackColor = System.Drawing.Color.Transparent;
			this.lblPrimary.Font = new System.Drawing.Font("Arial", 9F);
			this.lblPrimary.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblPrimary, null);
			this.locExtender.SetLocalizationComment(this.lblPrimary, null);
			this.locExtender.SetLocalizingId(this.lblPrimary, "CommonControls.PhoneInfoPopupContent.PrimaryLabel");
			this.lblPrimary.Location = new System.Drawing.Point(0, 22);
			this.lblPrimary.Name = "lblPrimary";
			this.lblPrimary.Size = new System.Drawing.Size(130, 15);
			this.lblPrimary.TabIndex = 2;
			this.lblPrimary.Text = "As primary uncertainty:";
			// 
			// lblNonPrimary
			// 
			this.lblNonPrimary.AutoSize = true;
			this.lblNonPrimary.BackColor = System.Drawing.Color.Transparent;
			this.lblNonPrimary.Font = new System.Drawing.Font("Arial", 9F);
			this.lblNonPrimary.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblNonPrimary, null);
			this.locExtender.SetLocalizationComment(this.lblNonPrimary, null);
			this.locExtender.SetLocalizingId(this.lblNonPrimary, "CommonControls.PhoneInfoPopupContent.NonPrimaryLabel");
			this.lblNonPrimary.Location = new System.Drawing.Point(0, 40);
			this.lblNonPrimary.Name = "lblNonPrimary";
			this.lblNonPrimary.Size = new System.Drawing.Size(154, 15);
			this.lblNonPrimary.TabIndex = 3;
			this.lblNonPrimary.Text = "As non primary uncertainty:";
			// 
			// lblNormallyCount
			// 
			this.lblNormallyCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblNormallyCount.BackColor = System.Drawing.Color.Transparent;
			this.lblNormallyCount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.lblNormallyCount.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblNormallyCount, null);
			this.locExtender.SetLocalizationComment(this.lblNormallyCount, null);
			this.locExtender.SetLocalizationPriority(this.lblNormallyCount, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblNormallyCount, "CommonControls.PhoneInfoPopupContent.NormallyCountLabel");
			this.lblNormallyCount.Location = new System.Drawing.Point(142, 4);
			this.lblNormallyCount.Name = "lblNormallyCount";
			this.lblNormallyCount.Size = new System.Drawing.Size(57, 15);
			this.lblNormallyCount.TabIndex = 4;
			this.lblNormallyCount.Text = "0";
			this.lblNormallyCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblPrimaryCount
			// 
			this.lblPrimaryCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPrimaryCount.BackColor = System.Drawing.Color.Transparent;
			this.lblPrimaryCount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.lblPrimaryCount.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblPrimaryCount, null);
			this.locExtender.SetLocalizationComment(this.lblPrimaryCount, null);
			this.locExtender.SetLocalizationPriority(this.lblPrimaryCount, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblPrimaryCount, "CommonControls.PhoneInfoPopupContent.lblPrimaryCount");
			this.lblPrimaryCount.Location = new System.Drawing.Point(157, 22);
			this.lblPrimaryCount.Name = "lblPrimaryCount";
			this.lblPrimaryCount.Size = new System.Drawing.Size(42, 15);
			this.lblPrimaryCount.TabIndex = 5;
			this.lblPrimaryCount.Text = "0";
			this.lblPrimaryCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblNonPrimaryCount
			// 
			this.lblNonPrimaryCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblNonPrimaryCount.BackColor = System.Drawing.Color.Transparent;
			this.lblNonPrimaryCount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.lblNonPrimaryCount.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblNonPrimaryCount, null);
			this.locExtender.SetLocalizationComment(this.lblNonPrimaryCount, null);
			this.locExtender.SetLocalizationPriority(this.lblNonPrimaryCount, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblNonPrimaryCount, "CommonControls.PhoneInfoPopupContent.NonPrimaryCountLabel");
			this.lblNonPrimaryCount.Location = new System.Drawing.Point(157, 40);
			this.lblNonPrimaryCount.Name = "lblNonPrimaryCount";
			this.lblNonPrimaryCount.Size = new System.Drawing.Size(42, 15);
			this.lblNonPrimaryCount.TabIndex = 6;
			this.lblNonPrimaryCount.Text = "0";
			this.lblNonPrimaryCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCountHeading
			// 
			this.lblCountHeading.AutoEllipsis = true;
			this.lblCountHeading.BackColor = System.Drawing.Color.Transparent;
			this.lblCountHeading.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblCountHeading.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.lblCountHeading.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblCountHeading, null);
			this.locExtender.SetLocalizationComment(this.lblCountHeading, null);
			this.locExtender.SetLocalizingId(this.lblCountHeading, "CommonControls.PhoneInfoPopupContent.CountHeadingLabel");
			this.lblCountHeading.Location = new System.Drawing.Point(59, 6);
			this.lblCountHeading.Name = "lblCountHeading";
			this.lblCountHeading.Size = new System.Drawing.Size(143, 37);
			this.lblCountHeading.TabIndex = 7;
			this.lblCountHeading.Text = "Number of times phone occurs:";
			this.lblCountHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblUncertaintyHeading
			// 
			this.lblUncertaintyHeading.BackColor = System.Drawing.Color.Transparent;
			this.lblUncertaintyHeading.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblUncertaintyHeading.Font = new System.Drawing.Font("Arial", 9F);
			this.lblUncertaintyHeading.ForeColor = System.Drawing.Color.Black;
			this.lblUncertaintyHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblUncertaintyHeading, null);
			this.locExtender.SetLocalizationComment(this.lblUncertaintyHeading, null);
			this.locExtender.SetLocalizingId(this.lblUncertaintyHeading, "CommonControls.PhoneInfoPopupContent.UncertaintyHeadingLabel");
			this.lblUncertaintyHeading.Location = new System.Drawing.Point(6, 65);
			this.lblUncertaintyHeading.Name = "lblUncertaintyHeading";
			this.lblUncertaintyHeading.Size = new System.Drawing.Size(200, 56);
			this.lblUncertaintyHeading.TabIndex = 9;
			this.lblUncertaintyHeading.Text = "This phone is an uncertain phone occurring in one or more groups with these other" +
				" uncertain phones:";
			this.lblUncertaintyHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSiblingPhones
			// 
			this.lblSiblingPhones.BackColor = System.Drawing.Color.Transparent;
			this.lblSiblingPhones.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSiblingPhones.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.lblSiblingPhones.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSiblingPhones, null);
			this.locExtender.SetLocalizationComment(this.lblSiblingPhones, null);
			this.locExtender.SetLocalizationPriority(this.lblSiblingPhones, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblSiblingPhones, "PhoneInfoPopupContent.lblSiblingPhones");
			this.lblSiblingPhones.Location = new System.Drawing.Point(6, 121);
			this.lblSiblingPhones.Name = "lblSiblingPhones";
			this.lblSiblingPhones.Size = new System.Drawing.Size(200, 29);
			this.lblSiblingPhones.TabIndex = 10;
			this.lblSiblingPhones.Text = "#";
			this.lblSiblingPhones.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblSiblingPhones.UseMnemonic = false;
			// 
			// pnlHeading
			// 
			this.pnlHeading.BackColor = System.Drawing.Color.White;
			this.pnlHeading.Controls.Add(this.lblCountHeading);
			this.pnlHeading.Controls.Add(this.pnlMonogram);
			this.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeading.Location = new System.Drawing.Point(0, 0);
			this.pnlHeading.MinimumSize = new System.Drawing.Size(0, 48);
			this.pnlHeading.Name = "pnlHeading";
			this.pnlHeading.Padding = new System.Windows.Forms.Padding(13, 6, 10, 6);
			this.pnlHeading.Size = new System.Drawing.Size(212, 49);
			this.pnlHeading.TabIndex = 11;
			this.pnlHeading.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeading_Paint);
			// 
			// pnlMonogram
			// 
			this.pnlMonogram.BackColor = System.Drawing.Color.White;
			this.pnlMonogram.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMonogram.Controls.Add(this.lblMonogram);
			this.pnlMonogram.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlMonogram.Location = new System.Drawing.Point(13, 6);
			this.pnlMonogram.Name = "pnlMonogram";
			this.pnlMonogram.Padding = new System.Windows.Forms.Padding(1);
			this.pnlMonogram.Size = new System.Drawing.Size(40, 37);
			this.pnlMonogram.TabIndex = 12;
			// 
			// lblMonogram
			// 
			this.lblMonogram.BackColor = System.Drawing.Color.Black;
			this.lblMonogram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblMonogram.ForeColor = System.Drawing.Color.White;
			this.lblMonogram.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblMonogram, null);
			this.locExtender.SetLocalizationComment(this.lblMonogram, null);
			this.locExtender.SetLocalizationPriority(this.lblMonogram, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblMonogram, "PhoneInfoPopupContent.lblMonogram");
			this.lblMonogram.Location = new System.Drawing.Point(1, 1);
			this.lblMonogram.MinimumSize = new System.Drawing.Size(36, 33);
			this.lblMonogram.Name = "lblMonogram";
			this.lblMonogram.Size = new System.Drawing.Size(36, 33);
			this.lblMonogram.TabIndex = 0;
			this.lblMonogram.Text = "X";
			this.lblMonogram.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlInfo
			// 
			this.pnlInfo.AutoSize = true;
			this.pnlInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlInfo.Controls.Add(this.lblSiblingPhones);
			this.pnlInfo.Controls.Add(this.lblUncertaintyHeading);
			this.pnlInfo.Controls.Add(this.pnlCounts);
			this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlInfo.Location = new System.Drawing.Point(0, 49);
			this.pnlInfo.Name = "pnlInfo";
			this.pnlInfo.Padding = new System.Windows.Forms.Padding(6, 0, 6, 8);
			this.pnlInfo.Size = new System.Drawing.Size(212, 158);
			this.pnlInfo.TabIndex = 12;
			this.pnlInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInfo_Paint);
			// 
			// pnlCounts
			// 
			this.pnlCounts.Controls.Add(this.lblPrimary);
			this.pnlCounts.Controls.Add(this.lblPrimaryCount);
			this.pnlCounts.Controls.Add(this.lblNormally);
			this.pnlCounts.Controls.Add(this.lblNormallyCount);
			this.pnlCounts.Controls.Add(this.lblNonPrimaryCount);
			this.pnlCounts.Controls.Add(this.lblNonPrimary);
			this.pnlCounts.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlCounts.Location = new System.Drawing.Point(6, 0);
			this.pnlCounts.Name = "pnlCounts";
			this.pnlCounts.Size = new System.Drawing.Size(200, 65);
			this.pnlCounts.TabIndex = 11;
			this.pnlCounts.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCounts_Paint);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// PhoneInfoPopupContent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.Transparent;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.pnlInfo);
			this.Controls.Add(this.pnlHeading);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "PhoneInfoPopupContent.PhoneInfoPopupContent");
			this.MinimumSize = new System.Drawing.Size(214, 2);
			this.Name = "PhoneInfoPopupContent";
			this.Size = new System.Drawing.Size(212, 207);
			this.pnlHeading.ResumeLayout(false);
			this.pnlMonogram.ResumeLayout(false);
			this.pnlInfo.ResumeLayout(false);
			this.pnlCounts.ResumeLayout(false);
			this.pnlCounts.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblNormally;
		private System.Windows.Forms.Label lblPrimary;
		private System.Windows.Forms.Label lblNonPrimary;
		public System.Windows.Forms.Label lblNormallyCount;
		public System.Windows.Forms.Label lblPrimaryCount;
		public System.Windows.Forms.Label lblNonPrimaryCount;
		private System.Windows.Forms.Label lblUncertaintyHeading;
		private System.Windows.Forms.Label lblSiblingPhones;
		public System.Windows.Forms.Label lblMonogram;
		private System.Windows.Forms.Panel pnlMonogram;
		private System.Windows.Forms.Panel pnlInfo;
		internal System.Windows.Forms.Panel pnlHeading;
		internal System.Windows.Forms.Label lblCountHeading;
		private System.Windows.Forms.Panel pnlCounts;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
