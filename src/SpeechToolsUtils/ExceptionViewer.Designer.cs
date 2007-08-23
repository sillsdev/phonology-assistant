namespace SIL.SpeechTools.Utils
{
	partial class ExceptionViewer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionViewer));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpgOuter = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.txtOuterMsg = new System.Windows.Forms.TextBox();
			this.txtOuterStackTrace = new System.Windows.Forms.TextBox();
			this.tpgInner = new System.Windows.Forms.TabPage();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.txtInnerMsg = new System.Windows.Forms.TextBox();
			this.txtInnerStackTrace = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.tpgOuter.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tpgInner.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tpgOuter);
			this.tabControl1.Controls.Add(this.tpgInner);
			resources.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			// 
			// tpgOuter
			// 
			this.tpgOuter.Controls.Add(this.splitContainer1);
			resources.ApplyResources(this.tpgOuter, "tpgOuter");
			this.tpgOuter.Name = "tpgOuter";
			this.tpgOuter.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtOuterMsg);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtOuterStackTrace);
			// 
			// txtOuterMsg
			// 
			this.txtOuterMsg.AcceptsReturn = true;
			this.txtOuterMsg.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.txtOuterMsg, "txtOuterMsg");
			this.txtOuterMsg.Name = "txtOuterMsg";
			this.txtOuterMsg.ReadOnly = true;
			// 
			// txtOuterStackTrace
			// 
			this.txtOuterStackTrace.AcceptsReturn = true;
			this.txtOuterStackTrace.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.txtOuterStackTrace, "txtOuterStackTrace");
			this.txtOuterStackTrace.Name = "txtOuterStackTrace";
			this.txtOuterStackTrace.ReadOnly = true;
			// 
			// tpgInner
			// 
			this.tpgInner.Controls.Add(this.splitContainer2);
			resources.ApplyResources(this.tpgInner, "tpgInner");
			this.tpgInner.Name = "tpgInner";
			this.tpgInner.UseVisualStyleBackColor = true;
			// 
			// splitContainer2
			// 
			resources.ApplyResources(this.splitContainer2, "splitContainer2");
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.txtInnerMsg);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.txtInnerStackTrace);
			// 
			// txtInnerMsg
			// 
			this.txtInnerMsg.AcceptsReturn = true;
			this.txtInnerMsg.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.txtInnerMsg, "txtInnerMsg");
			this.txtInnerMsg.Name = "txtInnerMsg";
			this.txtInnerMsg.ReadOnly = true;
			// 
			// txtInnerStackTrace
			// 
			this.txtInnerStackTrace.AcceptsReturn = true;
			this.txtInnerStackTrace.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.txtInnerStackTrace, "txtInnerStackTrace");
			this.txtInnerStackTrace.Name = "txtInnerStackTrace";
			this.txtInnerStackTrace.ReadOnly = true;
			// 
			// ExceptionViewer
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.DoubleBuffered = true;
			this.Name = "ExceptionViewer";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.tabControl1.ResumeLayout(false);
			this.tpgOuter.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.tpgInner.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpgOuter;
		private System.Windows.Forms.TextBox txtOuterMsg;
		private System.Windows.Forms.TabPage tpgInner;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox txtOuterStackTrace;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.TextBox txtInnerMsg;
		private System.Windows.Forms.TextBox txtInnerStackTrace;
	}
}