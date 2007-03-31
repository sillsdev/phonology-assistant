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
using SIL.FieldWorks.Common.Controls;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for SetMasks.
	/// </summary>
	public partial class SetMasks
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
			this.components = new System.ComponentModel.Container();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.mnuSort = new System.Windows.Forms.MenuItem();
			this.mnuSortChars = new System.Windows.Forms.MenuItem();
			this.mnuSortANSI = new System.Windows.Forms.MenuItem();
			this.mnuSortMoA = new System.Windows.Forms.MenuItem();
			this.mnuSortPoA = new System.Windows.Forms.MenuItem();
			this.mnuSortFeatures = new System.Windows.Forms.MenuItem();
			this.mnuSortCharType = new System.Windows.Forms.MenuItem();
			this.mnuSortAlpha = new System.Windows.Forms.MenuItem();
			this.mnuEdit = new System.Windows.Forms.MenuItem();
			this.mnuEditFeatureName = new System.Windows.Forms.MenuItem();
			this.lstIPA = new System.Windows.Forms.ListBox();
			this.lblIPA = new System.Windows.Forms.Label();
			this.lblDesc = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnRestore = new System.Windows.Forms.Button();
			this.mslbFeatures = new SIL.Pa.MultiStateListBox();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSort,
            this.mnuEdit});
			// 
			// mnuSort
			// 
			this.mnuSort.Index = 0;
			this.mnuSort.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSortChars,
            this.mnuSortFeatures});
			this.mnuSort.Text = "&Sort";
			// 
			// mnuSortChars
			// 
			this.mnuSortChars.Index = 0;
			this.mnuSortChars.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSortANSI,
            this.mnuSortMoA,
            this.mnuSortPoA});
			this.mnuSortChars.Text = "&IPA Characters";
			// 
			// mnuSortANSI
			// 
			this.mnuSortANSI.Index = 0;
			this.mnuSortANSI.Text = "&ANSI Order (A-Z)";
			// 
			// mnuSortMoA
			// 
			this.mnuSortMoA.Index = 1;
			this.mnuSortMoA.Text = "&Manner of Articulation";
			// 
			// mnuSortPoA
			// 
			this.mnuSortPoA.Index = 2;
			this.mnuSortPoA.Text = "&Place of Articulation";
			// 
			// mnuSortFeatures
			// 
			this.mnuSortFeatures.Index = 1;
			this.mnuSortFeatures.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSortCharType,
            this.mnuSortAlpha});
			this.mnuSortFeatures.Text = "&Features";
			// 
			// mnuSortCharType
			// 
			this.mnuSortCharType.Index = 0;
			this.mnuSortCharType.Text = "Character &Type";
			// 
			// mnuSortAlpha
			// 
			this.mnuSortAlpha.Index = 1;
			this.mnuSortAlpha.Text = "&Alphabetic";
			// 
			// mnuEdit
			// 
			this.mnuEdit.Index = 1;
			this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuEditFeatureName});
			this.mnuEdit.Text = "&Edit";
			// 
			// mnuEditFeatureName
			// 
			this.mnuEditFeatureName.Index = 0;
			this.mnuEditFeatureName.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.mnuEditFeatureName.Text = "Feature Name";
			// 
			// lstIPA
			// 
			this.lstIPA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lstIPA.IntegralHeight = false;
			this.lstIPA.Location = new System.Drawing.Point(4, 40);
			this.lstIPA.Name = "lstIPA";
			this.lstIPA.Size = new System.Drawing.Size(74, 384);
			this.lstIPA.TabIndex = 0;
			this.lstIPA.SelectedIndexChanged += new System.EventHandler(this.lstIPA_SelectedIndexChanged);
			// 
			// lblIPA
			// 
			this.lblIPA.AutoEllipsis = true;
			this.lblIPA.Location = new System.Drawing.Point(6, 3);
			this.lblIPA.MaximumSize = new System.Drawing.Size(64, 0);
			this.lblIPA.Name = "lblIPA";
			this.lblIPA.Size = new System.Drawing.Size(64, 32);
			this.lblIPA.TabIndex = 2;
			this.lblIPA.Text = "IPA Character:";
			this.lblIPA.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lblDesc
			// 
			this.lblDesc.AutoSize = true;
			this.lblDesc.Location = new System.Drawing.Point(94, 22);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.Size = new System.Drawing.Size(14, 13);
			this.lblDesc.TabIndex = 3;
			this.lblDesc.Text = "#";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.Location = new System.Drawing.Point(4, 440);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(88, 440);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnEdit.Location = new System.Drawing.Point(172, 440);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(75, 23);
			this.btnEdit.TabIndex = 4;
			this.btnEdit.Text = "E&dit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnRestore
			// 
			this.btnRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnRestore.Location = new System.Drawing.Point(256, 440);
			this.btnRestore.Name = "btnRestore";
			this.btnRestore.Size = new System.Drawing.Size(96, 23);
			this.btnRestore.TabIndex = 4;
			this.btnRestore.Text = "&Restore Defaults";
			this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
			// 
			// mslbFeatures
			// 
			this.mslbFeatures.AllowLabelEdit = false;
			this.mslbFeatures.AllowStateEdit = false;
			this.mslbFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mslbFeatures.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.mslbFeatures.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.mslbFeatures.IntegralHeight = false;
			this.mslbFeatures.ItemHeight = 16;
			this.mslbFeatures.Location = new System.Drawing.Point(88, 40);
			this.mslbFeatures.MultiStateType = SIL.Pa.MultiStateListBox.MultiStateTypes.CheckBox;
			this.mslbFeatures.Name = "mslbFeatures";
			this.mslbFeatures.Size = new System.Drawing.Size(264, 384);
			this.mslbFeatures.TabIndex = 5;
			this.mslbFeatures.AfterItemEdit += new SIL.Pa.EditableListBox.AfterItemEditHandler(this.mslbFeatures_AfterItemEdit);
			// 
			// SetMasks
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(360, 474);
			this.Controls.Add(this.mslbFeatures);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblDesc);
			this.Controls.Add(this.lblIPA);
			this.Controls.Add(this.lstIPA);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnRestore);
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(368, 250);
			this.Name = "SetMasks";
			this.Text = "Features";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuSort;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuSortChars;
		private System.Windows.Forms.MenuItem mnuSortANSI;
		private System.Windows.Forms.MenuItem mnuSortMoA;
		private System.Windows.Forms.MenuItem mnuSortPoA;
		private System.Windows.Forms.MenuItem mnuSortFeatures;
		private System.Windows.Forms.MenuItem mnuSortAlpha;
		private System.Windows.Forms.Label lblIPA;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.ListBox lstIPA;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnRestore;
		private System.ComponentModel.IContainer components;
		private SIL.Pa.MultiStateListBox mslbFeatures;
		private System.Windows.Forms.MenuItem mnuEditFeatureName;
		private System.Windows.Forms.MenuItem mnuSortCharType;
	}
}
