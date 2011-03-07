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

namespace SIL.Pa.UI
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaMainWnd));
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.sblblMain = new System.Windows.Forms.ToolStripStatusLabel();
			this.sblblProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.sblblFilter = new System.Windows.Forms.ToolStripStatusLabel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.vwTabGroup = new SIL.Pa.UI.Controls.ViewTabGroup();
			this.statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sblblMain,
            this.sblblProgress,
            this.sbProgress,
            this.sblblFilter});
			resources.ApplyResources(this.statusStrip, "statusStrip");
			this.statusStrip.Name = "statusStrip";
			// 
			// sblblMain
			// 
			resources.ApplyResources(this.sblblMain, "sblblMain");
			this.sblblMain.BackColor = System.Drawing.SystemColors.Control;
			this.locExtender.SetLocalizableToolTip(this.sblblMain, null);
			this.locExtender.SetLocalizationComment(this.sblblMain, null);
			this.locExtender.SetLocalizationPriority(this.sblblMain, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.sblblMain, "PaMainWnd.sblblMain");
			this.sblblMain.Name = "sblblMain";
			this.sblblMain.Spring = true;
			// 
			// sblblProgress
			// 
			this.sblblProgress.BackColor = System.Drawing.SystemColors.Control;
			this.sblblProgress.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.sblblProgress.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.sblblProgress.Image = global::SIL.Pa.Properties.Resources.LoadingWheel;
			resources.ApplyResources(this.sblblProgress, "sblblProgress");
			this.locExtender.SetLocalizableToolTip(this.sblblProgress, null);
			this.locExtender.SetLocalizationComment(this.sblblProgress, null);
			this.locExtender.SetLocalizationPriority(this.sblblProgress, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.sblblProgress, "PaMainWnd.sblblProgress");
			this.sblblProgress.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
			this.sblblProgress.Name = "sblblProgress";
			// 
			// sbProgress
			// 
			resources.ApplyResources(this.sbProgress, "sbProgress");
			this.sbProgress.Name = "sbProgress";
			// 
			// sblblFilter
			// 
			resources.ApplyResources(this.sblblFilter, "sblblFilter");
			this.locExtender.SetLocalizableToolTip(this.sblblFilter, null);
			this.locExtender.SetLocalizationComment(this.sblblFilter, null);
			this.locExtender.SetLocalizationPriority(this.sblblFilter, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.sblblFilter, "PaMainWnd.sblblFilter");
			this.sblblFilter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
			this.sblblFilter.Name = "sblblFilter";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Views";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// vwTabGroup
			// 
			this.vwTabGroup.AllowDrop = true;
			this.vwTabGroup.BackColor = System.Drawing.SystemColors.Control;
			resources.ApplyResources(this.vwTabGroup, "vwTabGroup");
			this.locExtender.SetLocalizableToolTip(this.vwTabGroup, null);
			this.locExtender.SetLocalizationComment(this.vwTabGroup, null);
			this.locExtender.SetLocalizationPriority(this.vwTabGroup, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.vwTabGroup, "PaMainWnd.vwTabGroup");
			this.vwTabGroup.Name = "vwTabGroup";
			// 
			// PaMainWnd
			// 
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.Controls.Add(this.vwTabGroup);
			this.Controls.Add(this.statusStrip);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "Main Window.WindowTitle");
			this.Name = "PaMainWnd";
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private ToolStripStatusLabel sblblMain;
		private ToolStripProgressBar sbProgress;
		public StatusStrip statusStrip;
		private ToolStripStatusLabel sblblProgress;
		private SIL.Pa.UI.Controls.ViewTabGroup vwTabGroup;
		private Localization.UI.LocalizationExtender locExtender;
		private ToolStripStatusLabel sblblFilter;
	}
}
