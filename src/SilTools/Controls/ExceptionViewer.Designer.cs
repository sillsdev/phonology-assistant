namespace SilTools
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
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.txtOuterMsg = new System.Windows.Forms.TextBox();
			this.txtOuterStackTrace = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.tpgOuter.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tpgOuter);
			resources.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			// 
			// tpgOuter
			// 
			this.tpgOuter.Controls.Add(this.splitOuter);
			resources.ApplyResources(this.tpgOuter, "tpgOuter");
			this.tpgOuter.Name = "tpgOuter";
			this.tpgOuter.UseVisualStyleBackColor = true;
			// 
			// splitOuter
			// 
			resources.ApplyResources(this.splitOuter, "splitOuter");
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.txtOuterMsg);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.txtOuterStackTrace);
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
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel1.PerformLayout();
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.Panel2.PerformLayout();
			this.splitOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpgOuter;
		private System.Windows.Forms.TextBox txtOuterMsg;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.TextBox txtOuterStackTrace;
	}
}