namespace SIL.Pa.Controls
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhoneInfoPopupContent));
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
			this.pnlHeading.SuspendLayout();
			this.pnlMonogram.SuspendLayout();
			this.pnlInfo.SuspendLayout();
			this.pnlCounts.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblNormally
			// 
			resources.ApplyResources(this.lblNormally, "lblNormally");
			this.lblNormally.BackColor = System.Drawing.Color.Transparent;
			this.lblNormally.ForeColor = System.Drawing.Color.Black;
			this.lblNormally.Name = "lblNormally";
			// 
			// lblPrimary
			// 
			resources.ApplyResources(this.lblPrimary, "lblPrimary");
			this.lblPrimary.BackColor = System.Drawing.Color.Transparent;
			this.lblPrimary.ForeColor = System.Drawing.Color.Black;
			this.lblPrimary.Name = "lblPrimary";
			// 
			// lblNonPrimary
			// 
			resources.ApplyResources(this.lblNonPrimary, "lblNonPrimary");
			this.lblNonPrimary.BackColor = System.Drawing.Color.Transparent;
			this.lblNonPrimary.ForeColor = System.Drawing.Color.Black;
			this.lblNonPrimary.Name = "lblNonPrimary";
			// 
			// lblNormallyCount
			// 
			resources.ApplyResources(this.lblNormallyCount, "lblNormallyCount");
			this.lblNormallyCount.BackColor = System.Drawing.Color.Transparent;
			this.lblNormallyCount.ForeColor = System.Drawing.Color.Black;
			this.lblNormallyCount.Name = "lblNormallyCount";
			// 
			// lblPrimaryCount
			// 
			resources.ApplyResources(this.lblPrimaryCount, "lblPrimaryCount");
			this.lblPrimaryCount.BackColor = System.Drawing.Color.Transparent;
			this.lblPrimaryCount.ForeColor = System.Drawing.Color.Black;
			this.lblPrimaryCount.Name = "lblPrimaryCount";
			// 
			// lblNonPrimaryCount
			// 
			resources.ApplyResources(this.lblNonPrimaryCount, "lblNonPrimaryCount");
			this.lblNonPrimaryCount.BackColor = System.Drawing.Color.Transparent;
			this.lblNonPrimaryCount.ForeColor = System.Drawing.Color.Black;
			this.lblNonPrimaryCount.Name = "lblNonPrimaryCount";
			// 
			// lblCountHeading
			// 
			this.lblCountHeading.AutoEllipsis = true;
			this.lblCountHeading.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblCountHeading, "lblCountHeading");
			this.lblCountHeading.ForeColor = System.Drawing.Color.Black;
			this.lblCountHeading.Name = "lblCountHeading";
			// 
			// lblUncertaintyHeading
			// 
			this.lblUncertaintyHeading.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblUncertaintyHeading, "lblUncertaintyHeading");
			this.lblUncertaintyHeading.ForeColor = System.Drawing.Color.Black;
			this.lblUncertaintyHeading.Name = "lblUncertaintyHeading";
			// 
			// lblSiblingPhones
			// 
			this.lblSiblingPhones.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblSiblingPhones, "lblSiblingPhones");
			this.lblSiblingPhones.Name = "lblSiblingPhones";
			this.lblSiblingPhones.UseMnemonic = false;
			// 
			// pnlHeading
			// 
			this.pnlHeading.BackColor = System.Drawing.Color.White;
			this.pnlHeading.Controls.Add(this.lblCountHeading);
			this.pnlHeading.Controls.Add(this.pnlMonogram);
			resources.ApplyResources(this.pnlHeading, "pnlHeading");
			this.pnlHeading.MinimumSize = new System.Drawing.Size(0, 48);
			this.pnlHeading.Name = "pnlHeading";
			this.pnlHeading.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeading_Paint);
			// 
			// pnlMonogram
			// 
			this.pnlMonogram.BackColor = System.Drawing.Color.White;
			this.pnlMonogram.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMonogram.Controls.Add(this.lblMonogram);
			resources.ApplyResources(this.pnlMonogram, "pnlMonogram");
			this.pnlMonogram.Name = "pnlMonogram";
			// 
			// lblMonogram
			// 
			this.lblMonogram.BackColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.lblMonogram, "lblMonogram");
			this.lblMonogram.ForeColor = System.Drawing.Color.White;
			this.lblMonogram.MinimumSize = new System.Drawing.Size(36, 33);
			this.lblMonogram.Name = "lblMonogram";
			this.lblMonogram.Paint += new System.Windows.Forms.PaintEventHandler(this.lblPhone_Paint);
			// 
			// pnlInfo
			// 
			resources.ApplyResources(this.pnlInfo, "pnlInfo");
			this.pnlInfo.Controls.Add(this.lblSiblingPhones);
			this.pnlInfo.Controls.Add(this.lblUncertaintyHeading);
			this.pnlInfo.Controls.Add(this.pnlCounts);
			this.pnlInfo.Name = "pnlInfo";
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
			resources.ApplyResources(this.pnlCounts, "pnlCounts");
			this.pnlCounts.Name = "pnlCounts";
			this.pnlCounts.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCounts_Paint);
			// 
			// PhoneInfoPopupContent
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.pnlInfo);
			this.Controls.Add(this.pnlHeading);
			this.MinimumSize = new System.Drawing.Size(214, 2);
			this.Name = "PhoneInfoPopupContent";
			this.pnlHeading.ResumeLayout(false);
			this.pnlMonogram.ResumeLayout(false);
			this.pnlInfo.ResumeLayout(false);
			this.pnlCounts.ResumeLayout(false);
			this.pnlCounts.PerformLayout();
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
	}
}
