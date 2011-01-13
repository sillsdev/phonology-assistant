using SilTools.Controls;

namespace SIL.Pa.UI.Views
{
	partial class DataCorpusVw
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataCorpusVw));
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.paPanel1 = new SilPanel();
			this.rtfRecVw = new SIL.Pa.UI.Controls.RtfRecordView();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.paPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			resources.ApplyResources(this.splitOuter, "splitOuter");
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.BackColor = System.Drawing.SystemColors.Control;
			resources.ApplyResources(this.splitOuter.Panel1, "splitOuter.Panel1");
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.paPanel1);
			resources.ApplyResources(this.splitOuter.Panel2, "splitOuter.Panel2");
			this.splitOuter.TabStop = false;
			// 
			// paPanel1
			// 
			this.paPanel1.BackColor = System.Drawing.SystemColors.Window;
			this.paPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.paPanel1.ClipTextForChildControls = true;
			this.paPanel1.ControlReceivingFocusOnMnemonic = null;
			this.paPanel1.Controls.Add(this.rtfRecVw);
			resources.ApplyResources(this.paPanel1, "paPanel1");
			this.paPanel1.DoubleBuffered = false;
			this.paPanel1.MnemonicGeneratesClick = false;
			this.paPanel1.Name = "paPanel1";
			this.paPanel1.PaintExplorerBarBackground = false;
			// 
			// rtfRecVw
			// 
			this.rtfRecVw.BackColor = System.Drawing.SystemColors.Window;
			this.rtfRecVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtfRecVw, "rtfRecVw");
			this.rtfRecVw.Name = "rtfRecVw";
			this.rtfRecVw.ReadOnly = true;
			this.rtfRecVw.TabStop = false;
			// 
			// DataCorpusVw
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitOuter);
			this.Name = "DataCorpusVw";
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.paPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitOuter;
		private SilPanel paPanel1;
		private SIL.Pa.UI.Controls.RtfRecordView rtfRecVw;
	}
}