using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for LabelledTextBox.
	/// </summary>
	public partial class LabelledTextBox
	{
		private TextBox m_text;
		private ToolStrip toolStrip1;
		private ToolStripLabel tslLabel;
		private ToolStripDropDownButton tsddLabel;
		private ToolStripMenuItem tsmiEtic;
		private ToolStripMenuItem tsmiTone;
		private ToolStripMenuItem tsmiEmic;
		private ToolStripMenuItem tsmiOrtho;
		private ToolStripMenuItem tsmiGloss;
		private ToolStripMenuItem tsmiPOS;
		private ToolStripMenuItem tsmiRef;
		private Panel pnlLabelContainer;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelledTextBox));
			this.m_text = new System.Windows.Forms.TextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tslLabel = new System.Windows.Forms.ToolStripLabel();
			this.tsddLabel = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmiEtic = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiTone = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiEmic = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiOrtho = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiGloss = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiPOS = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiRef = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlLabelContainer = new System.Windows.Forms.Panel();
			this.toolStrip1.SuspendLayout();
			this.pnlLabelContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_text
			// 
			this.m_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.m_text, "m_text");
			this.m_text.Name = "m_text";
			this.m_text.TextChanged += new System.EventHandler(this.m_text_TextChanged);
			// 
			// toolStrip1
			// 
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.CanOverflow = false;
			this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslLabel,
            this.tsddLabel});
			this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.ShowItemToolTips = false;
			// 
			// tslLabel
			// 
			this.tslLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tslLabel.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
			this.tslLabel.Name = "tslLabel";
			resources.ApplyResources(this.tslLabel, "tslLabel");
			// 
			// tsddLabel
			// 
			this.tsddLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsddLabel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEtic,
            this.tsmiTone,
            this.tsmiEmic,
            this.tsmiOrtho,
            this.tsmiGloss,
            this.tsmiPOS,
            this.tsmiRef});
			resources.ApplyResources(this.tsddLabel, "tsddLabel");
			this.tsddLabel.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
			this.tsddLabel.Name = "tsddLabel";
			this.tsddLabel.Click += new System.EventHandler(this.tsddLabel_Click);
			// 
			// tsmiEtic
			// 
			this.tsmiEtic.Name = "tsmiEtic";
			resources.ApplyResources(this.tsmiEtic, "tsmiEtic");
			this.tsmiEtic.Click += new System.EventHandler(this.tsmiItem_Click);
			// 
			// tsmiTone
			// 
			this.tsmiTone.Name = "tsmiTone";
			resources.ApplyResources(this.tsmiTone, "tsmiTone");
			this.tsmiTone.Click += new System.EventHandler(this.tsmiItem_Click);
			// 
			// tsmiEmic
			// 
			this.tsmiEmic.Name = "tsmiEmic";
			resources.ApplyResources(this.tsmiEmic, "tsmiEmic");
			this.tsmiEmic.Click += new System.EventHandler(this.tsmiItem_Click);
			// 
			// tsmiOrtho
			// 
			this.tsmiOrtho.Name = "tsmiOrtho";
			resources.ApplyResources(this.tsmiOrtho, "tsmiOrtho");
			this.tsmiOrtho.Click += new System.EventHandler(this.tsmiItem_Click);
			// 
			// tsmiGloss
			// 
			this.tsmiGloss.Name = "tsmiGloss";
			resources.ApplyResources(this.tsmiGloss, "tsmiGloss");
			this.tsmiGloss.Click += new System.EventHandler(this.tsmiItem_Click);
			// 
			// tsmiPOS
			// 
			this.tsmiPOS.Name = "tsmiPOS";
			resources.ApplyResources(this.tsmiPOS, "tsmiPOS");
			this.tsmiPOS.Click += new System.EventHandler(this.tsmiItem_Click);
			// 
			// tsmiRef
			// 
			this.tsmiRef.Name = "tsmiRef";
			resources.ApplyResources(this.tsmiRef, "tsmiRef");
			this.tsmiRef.Click += new System.EventHandler(this.tsmiItem_Click);
			// 
			// pnlLabelContainer
			// 
			this.pnlLabelContainer.BackColor = System.Drawing.Color.Transparent;
			this.pnlLabelContainer.Controls.Add(this.toolStrip1);
			resources.ApplyResources(this.pnlLabelContainer, "pnlLabelContainer");
			this.pnlLabelContainer.Name = "pnlLabelContainer";
			// 
			// LabelledTextBox
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.m_text);
			this.Controls.Add(this.pnlLabelContainer);
			resources.ApplyResources(this, "$this");
			this.Name = "LabelledTextBox";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.pnlLabelContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}
