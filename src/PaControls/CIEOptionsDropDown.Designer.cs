namespace SIL.Pa.Controls
{
	partial class CIEOptionsDropDown
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
			this.rbBoth = new System.Windows.Forms.RadioButton();
			this.rbAfter = new System.Windows.Forms.RadioButton();
			this.rbBefore = new System.Windows.Forms.RadioButton();
			this.lnkApply = new System.Windows.Forms.LinkLabel();
			this.lnkCancel = new System.Windows.Forms.LinkLabel();
			this.grpUncertainties.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkShowAllWords
			// 
			this.chkShowAllWords.Location = new System.Drawing.Point(205, 80);
			this.chkShowAllWords.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			// 
			// grpStress
			// 
			this.grpStress.Location = new System.Drawing.Point(14, 122);
			this.grpStress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.grpStress.TabIndex = 5;
			// 
			// grpTone
			// 
			this.grpTone.Location = new System.Drawing.Point(14, 199);
			this.grpTone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.grpTone.TabIndex = 7;
			// 
			// grpLength
			// 
			this.grpLength.Location = new System.Drawing.Point(14, 411);
			this.grpLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.grpLength.TabIndex = 9;
			// 
			// chkIgnoreDiacritics
			// 
			this.chkIgnoreDiacritics.Location = new System.Drawing.Point(24, 96);
			this.chkIgnoreDiacritics.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chkIgnoreDiacritics.TabIndex = 3;
			// 
			// chkStress
			// 
			this.chkStress.Location = new System.Drawing.Point(24, 120);
			this.chkStress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chkStress.TabIndex = 4;
			// 
			// chkTone
			// 
			this.chkTone.Location = new System.Drawing.Point(24, 197);
			this.chkTone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chkTone.TabIndex = 6;
			// 
			// chkLength
			// 
			this.chkLength.Location = new System.Drawing.Point(24, 409);
			this.chkLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chkLength.TabIndex = 8;
			// 
			// lnkHelp
			// 
			this.lnkHelp.Location = new System.Drawing.Point(218, 490);
			this.lnkHelp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lnkHelp.TabIndex = 12;
			// 
			// grpUncertainties
			// 
			this.grpUncertainties.Visible = false;
			// 
			// lblUncertainties
			// 
			this.lblUncertainties.Visible = false;
			// 
			// rbBoth
			// 
			this.rbBoth.AutoSize = true;
			this.rbBoth.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbBoth.Location = new System.Drawing.Point(24, 15);
			this.rbBoth.Name = "rbBoth";
			this.rbBoth.Size = new System.Drawing.Size(157, 17);
			this.rbBoth.TabIndex = 0;
			this.rbBoth.TabStop = true;
			this.rbBoth.Text = "B&oth Environments Identical";
			this.rbBoth.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbBoth.UseVisualStyleBackColor = true;
			// 
			// rbAfter
			// 
			this.rbAfter.AutoSize = true;
			this.rbAfter.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbAfter.Location = new System.Drawing.Point(24, 63);
			this.rbAfter.Name = "rbAfter";
			this.rbAfter.Size = new System.Drawing.Size(152, 17);
			this.rbAfter.TabIndex = 2;
			this.rbAfter.TabStop = true;
			this.rbAfter.Text = "Identical &After Environment";
			this.rbAfter.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbAfter.UseVisualStyleBackColor = true;
			// 
			// rbBefore
			// 
			this.rbBefore.AutoSize = true;
			this.rbBefore.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbBefore.Location = new System.Drawing.Point(24, 39);
			this.rbBefore.Name = "rbBefore";
			this.rbBefore.Size = new System.Drawing.Size(161, 17);
			this.rbBefore.TabIndex = 1;
			this.rbBefore.TabStop = true;
			this.rbBefore.Text = "Identical &Before Environment";
			this.rbBefore.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbBefore.UseVisualStyleBackColor = true;
			// 
			// lnkApply
			// 
			this.lnkApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkApply.AutoSize = true;
			this.lnkApply.Location = new System.Drawing.Point(140, 490);
			this.lnkApply.Name = "lnkApply";
			this.lnkApply.Size = new System.Drawing.Size(33, 13);
			this.lnkApply.TabIndex = 10;
			this.lnkApply.TabStop = true;
			this.lnkApply.Text = "Apply";
			this.lnkApply.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkApply_LinkClicked);
			// 
			// lnkCancel
			// 
			this.lnkCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkCancel.AutoSize = true;
			this.lnkCancel.Location = new System.Drawing.Point(179, 490);
			this.lnkCancel.Name = "lnkCancel";
			this.lnkCancel.Size = new System.Drawing.Size(40, 13);
			this.lnkCancel.TabIndex = 11;
			this.lnkCancel.TabStop = true;
			this.lnkCancel.Text = "Cancel";
			this.lnkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCancel_LinkClicked);
			// 
			// CIEOptionsDropDown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lnkCancel);
			this.Controls.Add(this.lnkApply);
			this.Controls.Add(this.rbBefore);
			this.Controls.Add(this.rbAfter);
			this.Controls.Add(this.rbBoth);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MinimumSize = new System.Drawing.Size(188, 2);
			this.Name = "CIEOptionsDropDown";
			this.Size = new System.Drawing.Size(248, 516);
			this.Controls.SetChildIndex(this.grpUncertainties, 0);
			this.Controls.SetChildIndex(this.lnkHelp, 0);
			this.Controls.SetChildIndex(this.rbBoth, 0);
			this.Controls.SetChildIndex(this.chkShowAllWords, 0);
			this.Controls.SetChildIndex(this.grpStress, 0);
			this.Controls.SetChildIndex(this.chkIgnoreDiacritics, 0);
			this.Controls.SetChildIndex(this.chkStress, 0);
			this.Controls.SetChildIndex(this.grpTone, 0);
			this.Controls.SetChildIndex(this.chkTone, 0);
			this.Controls.SetChildIndex(this.grpLength, 0);
			this.Controls.SetChildIndex(this.chkLength, 0);
			this.Controls.SetChildIndex(this.rbAfter, 0);
			this.Controls.SetChildIndex(this.rbBefore, 0);
			this.Controls.SetChildIndex(this.lnkApply, 0);
			this.Controls.SetChildIndex(this.lnkCancel, 0);
			this.grpUncertainties.ResumeLayout(false);
			this.grpUncertainties.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbBoth;
		private System.Windows.Forms.RadioButton rbAfter;
		private System.Windows.Forms.RadioButton rbBefore;
		public System.Windows.Forms.LinkLabel lnkApply;
		public System.Windows.Forms.LinkLabel lnkCancel;
	}
}
