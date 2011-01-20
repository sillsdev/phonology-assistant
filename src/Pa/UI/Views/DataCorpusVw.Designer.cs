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
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.paPanel1 = new SilTools.Controls.SilPanel();
			this.rtfRecVw = new SIL.Pa.UI.Controls.RtfRecordView();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.paPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			this.splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitOuter.Location = new System.Drawing.Point(0, 0);
			this.splitOuter.Name = "splitOuter";
			this.splitOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitOuter.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.paPanel1);
			this.splitOuter.Panel2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
			this.splitOuter.Panel2MinSize = 0;
			this.splitOuter.Size = new System.Drawing.Size(676, 468);
			this.splitOuter.SplitterDistance = 371;
			this.splitOuter.SplitterWidth = 8;
			this.splitOuter.TabIndex = 0;
			this.splitOuter.TabStop = false;
			// 
			// paPanel1
			// 
			this.paPanel1.BackColor = System.Drawing.SystemColors.Window;
			this.paPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.paPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.paPanel1.ClipTextForChildControls = true;
			this.paPanel1.ControlReceivingFocusOnMnemonic = null;
			this.paPanel1.Controls.Add(this.rtfRecVw);
			this.paPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.paPanel1.DoubleBuffered = false;
			this.paPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.paPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.paPanel1.Location = new System.Drawing.Point(10, 0);
			this.paPanel1.MnemonicGeneratesClick = false;
			this.paPanel1.Name = "paPanel1";
			this.paPanel1.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
			this.paPanel1.PaintExplorerBarBackground = false;
			this.paPanel1.Size = new System.Drawing.Size(656, 79);
			this.paPanel1.TabIndex = 0;
			// 
			// rtfRecVw
			// 
			this.rtfRecVw.BackColor = System.Drawing.SystemColors.Window;
			this.rtfRecVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtfRecVw.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtfRecVw.Location = new System.Drawing.Point(4, 4);
			this.rtfRecVw.Name = "rtfRecVw";
			this.rtfRecVw.ReadOnly = true;
			this.rtfRecVw.Size = new System.Drawing.Size(650, 73);
			this.rtfRecVw.TabIndex = 0;
			this.rtfRecVw.TabStop = false;
			this.rtfRecVw.Text = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidExportAsToolTip;
			this.rtfRecVw.WordWrap = false;
			// 
			// DataCorpusVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitOuter);
			this.Name = "DataCorpusVw";
			this.Size = new System.Drawing.Size(676, 468);
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