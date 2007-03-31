// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.   
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: SetMasks.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace SIL.Pa.Dialogs
{
	/// <summary>
	/// Summary description for SetMasks.
	/// </summary>
	public partial class DefineFeaturesDlgBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefineFeaturesDlgBase));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lblIPA = new System.Windows.Forms.Label();
			this.lstIPAChar = new System.Windows.Forms.ListBox();
			this.lblFeatures = new System.Windows.Forms.Label();
			this.btnRestoreDefaults = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.sslCharDescription = new System.Windows.Forms.ToolStripStatusLabel();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.pnlButtons.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnRemove);
			this.pnlButtons.Controls.Add(this.btnAdd);
			this.pnlButtons.Controls.Add(this.btnRestoreDefaults);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnRestoreDefaults, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnAdd, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnRemove, 0);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lblIPA);
			this.splitContainer1.Panel1.Controls.Add(this.lstIPAChar);
			resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lblFeatures);
			resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.splitContainer1.TabStop = false;
			// 
			// lblIPA
			// 
			this.lblIPA.AutoEllipsis = true;
			resources.ApplyResources(this.lblIPA, "lblIPA");
			this.lblIPA.Name = "lblIPA";
			// 
			// lstIPAChar
			// 
			resources.ApplyResources(this.lstIPAChar, "lstIPAChar");
			this.lstIPAChar.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstIPAChar.MultiColumn = true;
			this.lstIPAChar.Name = "lstIPAChar";
			this.lstIPAChar.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstIPAChar_DrawItem);
			this.lstIPAChar.SelectedIndexChanged += new System.EventHandler(this.lstIPAChar_SelectedIndexChanged);
			// 
			// lblFeatures
			// 
			resources.ApplyResources(this.lblFeatures, "lblFeatures");
			this.lblFeatures.Name = "lblFeatures";
			// 
			// btnRestoreDefaults
			// 
			resources.ApplyResources(this.btnRestoreDefaults, "btnRestoreDefaults");
			this.btnRestoreDefaults.Name = "btnRestoreDefaults";
			this.btnRestoreDefaults.UseVisualStyleBackColor = true;
			this.btnRestoreDefaults.Click += new System.EventHandler(this.btnRestoreDefaults_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sslCharDescription});
			resources.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.Name = "statusStrip1";
			// 
			// sslCharDescription
			// 
			this.sslCharDescription.Name = "sslCharDescription";
			resources.ApplyResources(this.sslCharDescription, "sslCharDescription");
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			// 
			// btnRemove
			// 
			resources.ApplyResources(this.btnRemove, "btnRemove");
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.UseVisualStyleBackColor = true;
			// 
			// DefineFeaturesDlgBase
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.DoubleBuffered = true;
			this.Name = "DefineFeaturesDlgBase";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Controls.SetChildIndex(this.statusStrip1, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.splitContainer1, 0);
			this.pnlButtons.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.ComponentModel.IContainer components = null;
		private Label lblIPA;
		private Label lblFeatures;
		private StatusStrip statusStrip1;
		protected ToolStripStatusLabel sslCharDescription;
		protected Button btnRestoreDefaults;
		protected SplitContainer splitContainer1;
		private ListBox lstIPAChar;
		protected Button btnRemove;
		protected Button btnAdd;
	}
}
