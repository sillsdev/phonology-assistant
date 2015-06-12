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
            this.sblblPercent = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.sblblFilter = new System.Windows.Forms.ToolStripStatusLabel();
            this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
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
            this.sblblPercent,
            this.sbProgress,
            this.sblblFilter});
            this.statusStrip.Location = new System.Drawing.Point(0, 509);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(794, 29);
            this.statusStrip.TabIndex = 1;
            // 
            // sblblMain
            // 
            this.sblblMain.AutoSize = false;
            this.sblblMain.BackColor = System.Drawing.SystemColors.Control;
            this.locExtender.SetLocalizableToolTip(this.sblblMain, null);
            this.locExtender.SetLocalizationComment(this.sblblMain, null);
            this.locExtender.SetLocalizationPriority(this.sblblMain, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.sblblMain, "PaMainWnd.sblblMain");
            this.sblblMain.Name = "sblblMain";
            this.sblblMain.Size = new System.Drawing.Size(376, 24);
            this.sblblMain.Spring = true;
            this.sblblMain.Text = "#";
            this.sblblMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sblblProgress
            // 
            this.sblblProgress.BackColor = System.Drawing.Color.Transparent;
            this.sblblProgress.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.sblblProgress.Image = global::SIL.Pa.Properties.Resources.LoadingWheel;
            this.sblblProgress.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.locExtender.SetLocalizableToolTip(this.sblblProgress, null);
            this.locExtender.SetLocalizationComment(this.sblblProgress, null);
            this.locExtender.SetLocalizationPriority(this.sblblProgress, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.sblblProgress, "PaMainWnd.sblblProgress");
            this.sblblProgress.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
            this.sblblProgress.Name = "sblblProgress";
            this.sblblProgress.Size = new System.Drawing.Size(34, 24);
            this.sblblProgress.Text = "#";
            this.sblblProgress.VisibleChanged += new System.EventHandler(this.HandleProgressLabelVisibleChanged);
            // 
            // sblblPercent
            // 
            this.sblblPercent.BackColor = System.Drawing.Color.Transparent;
            this.sblblPercent.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.sblblPercent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.locExtender.SetLocalizableToolTip(this.sblblPercent, null);
            this.locExtender.SetLocalizationComment(this.sblblPercent, null);
            this.locExtender.SetLocalizationPriority(this.sblblPercent, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.sblblPercent, "PaMainWnd.sblblPercent");
            this.sblblPercent.Name = "sblblPercent";
            this.sblblPercent.Size = new System.Drawing.Size(18, 24);
            this.sblblPercent.Text = "#";
            // 
            // sbProgress
            // 
            this.sbProgress.AutoSize = false;
            this.sbProgress.Name = "sbProgress";
            this.sbProgress.Size = new System.Drawing.Size(240, 23);
            // 
            // sblblFilter
            // 
            this.sblblFilter.AutoSize = false;
            this.sblblFilter.Image = global::SIL.Pa.Properties.Resources.kimidFilter;
            this.locExtender.SetLocalizableToolTip(this.sblblFilter, null);
            this.locExtender.SetLocalizationComment(this.sblblFilter, null);
            this.locExtender.SetLocalizationPriority(this.sblblFilter, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.sblblFilter, "PaMainWnd.sblblFilter");
            this.sblblFilter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.sblblFilter.Name = "sblblFilter";
            this.sblblFilter.Size = new System.Drawing.Size(96, 24);
            this.sblblFilter.Text = "FilterIndicator";
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
            // 
            // vwTabGroup
            // 
            this.vwTabGroup.AllowDrop = true;
            this.vwTabGroup.BackColor = System.Drawing.SystemColors.Control;
            this.vwTabGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.locExtender.SetLocalizableToolTip(this.vwTabGroup, null);
            this.locExtender.SetLocalizationComment(this.vwTabGroup, null);
            this.locExtender.SetLocalizationPriority(this.vwTabGroup, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.vwTabGroup, "PaMainWnd.vwTabGroup");
            this.vwTabGroup.Location = new System.Drawing.Point(0, 0);
            this.vwTabGroup.Name = "vwTabGroup";
            this.vwTabGroup.Size = new System.Drawing.Size(794, 509);
            this.vwTabGroup.TabIndex = 5;
            this.vwTabGroup.Visible = false;
            // 
            // PaMainWnd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(794, 538);
            this.Controls.Add(this.vwTabGroup);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizingId(this, "MainWindow.WindowTitle.WithOutProject");
            this.Name = "PaMainWnd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Phonology Assistant";
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
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private ToolStripStatusLabel sblblFilter;
		private ToolStripStatusLabel sblblPercent;
	}
}
