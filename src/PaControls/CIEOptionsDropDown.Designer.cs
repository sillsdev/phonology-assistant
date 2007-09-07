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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CIEOptionsDropDown));
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
			resources.ApplyResources(this.chkShowAllWords, "chkShowAllWords");
			// 
			// grpStress
			// 
			resources.ApplyResources(this.grpStress, "grpStress");
			// 
			// grpTone
			// 
			resources.ApplyResources(this.grpTone, "grpTone");
			// 
			// grpLength
			// 
			resources.ApplyResources(this.grpLength, "grpLength");
			// 
			// chkIgnoreDiacritics
			// 
			resources.ApplyResources(this.chkIgnoreDiacritics, "chkIgnoreDiacritics");
			// 
			// chkStress
			// 
			resources.ApplyResources(this.chkStress, "chkStress");
			// 
			// chkTone
			// 
			resources.ApplyResources(this.chkTone, "chkTone");
			// 
			// chkLength
			// 
			resources.ApplyResources(this.chkLength, "chkLength");
			// 
			// lnkHelp
			// 
			resources.ApplyResources(this.lnkHelp, "lnkHelp");
			// 
			// grpUncertainties
			// 
			resources.ApplyResources(this.grpUncertainties, "grpUncertainties");
			// 
			// lblUncertainties
			// 
			resources.ApplyResources(this.lblUncertainties, "lblUncertainties");
			// 
			// rbBoth
			// 
			resources.ApplyResources(this.rbBoth, "rbBoth");
			this.rbBoth.Name = "rbBoth";
			this.rbBoth.TabStop = true;
			this.rbBoth.UseVisualStyleBackColor = true;
			// 
			// rbAfter
			// 
			resources.ApplyResources(this.rbAfter, "rbAfter");
			this.rbAfter.Name = "rbAfter";
			this.rbAfter.TabStop = true;
			this.rbAfter.UseVisualStyleBackColor = true;
			// 
			// rbBefore
			// 
			resources.ApplyResources(this.rbBefore, "rbBefore");
			this.rbBefore.Name = "rbBefore";
			this.rbBefore.TabStop = true;
			this.rbBefore.UseVisualStyleBackColor = true;
			// 
			// lnkApply
			// 
			resources.ApplyResources(this.lnkApply, "lnkApply");
			this.lnkApply.Name = "lnkApply";
			this.lnkApply.TabStop = true;
			this.lnkApply.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkApply_LinkClicked);
			// 
			// lnkCancel
			// 
			resources.ApplyResources(this.lnkCancel, "lnkCancel");
			this.lnkCancel.Name = "lnkCancel";
			this.lnkCancel.TabStop = true;
			this.lnkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCancel_LinkClicked);
			// 
			// CIEOptionsDropDown
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lnkCancel);
			this.Controls.Add(this.lnkApply);
			this.Controls.Add(this.rbBefore);
			this.Controls.Add(this.rbAfter);
			this.Controls.Add(this.rbBoth);
			this.MinimumSize = new System.Drawing.Size(188, 2);
			this.Name = "CIEOptionsDropDown";
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
