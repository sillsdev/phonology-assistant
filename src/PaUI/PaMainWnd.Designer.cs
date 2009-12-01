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
// File: PaMainWnd.cs
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
using System.Reflection;
using System.IO;
using SIL.Pa.Resources;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for PaMainWnd.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PaMainWnd
	{
		#region Member variables added by designer

		private System.ComponentModel.IContainer components = null;
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged 
		/// resources; <c>false</c> to release only unmanaged resources. 
		/// </param>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaMainWnd));
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.sblblMain = new System.Windows.Forms.ToolStripStatusLabel();
			this.sblblProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.vwTabGroup = new SIL.Pa.Controls.ViewTabGroup();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sblblMain,
            this.sblblProgress,
            this.sbProgress});
			resources.ApplyResources(this.statusStrip, "statusStrip");
			this.statusStrip.Name = "statusStrip";
			// 
			// sblblMain
			// 
			resources.ApplyResources(this.sblblMain, "sblblMain");
			this.sblblMain.BackColor = System.Drawing.SystemColors.Control;
			this.sblblMain.Name = "sblblMain";
			this.sblblMain.Spring = true;
			// 
			// sblblProgress
			// 
			this.sblblProgress.BackColor = System.Drawing.SystemColors.Control;
			this.sblblProgress.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.sblblProgress.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.sblblProgress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sblblProgress.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
			this.sblblProgress.Name = "sblblProgress";
			resources.ApplyResources(this.sblblProgress, "sblblProgress");
			// 
			// sbProgress
			// 
			resources.ApplyResources(this.sbProgress, "sbProgress");
			this.sbProgress.Name = "sbProgress";
			// 
			// vwTabGroup
			// 
			this.vwTabGroup.AllowDrop = true;
			this.vwTabGroup.BackColor = System.Drawing.SystemColors.Control;
			resources.ApplyResources(this.vwTabGroup, "vwTabGroup");
			this.vwTabGroup.Name = "vwTabGroup";
			// 
			// PaMainWnd
			// 
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.Controls.Add(this.vwTabGroup);
			this.Controls.Add(this.statusStrip);
			this.DoubleBuffered = true;
			this.Name = "PaMainWnd";
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private ToolStripStatusLabel sblblMain;
		private ToolStripProgressBar sbProgress;
		public StatusStrip statusStrip;
		private ToolStripStatusLabel sblblProgress;
		private SIL.Pa.Controls.ViewTabGroup vwTabGroup;
	}
}
