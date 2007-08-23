namespace SIL.SpeechTools.Utils
{
	partial class IdenticalMD5Dlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IdenticalMD5Dlg));
			this.rbUpdatePath = new System.Windows.Forms.RadioButton();
			this.btnOK = new System.Windows.Forms.Button();
			this.rbCopy = new System.Windows.Forms.RadioButton();
			this.rbConvert = new System.Windows.Forms.RadioButton();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// rbUpdatePath
			// 
			resources.ApplyResources(this.rbUpdatePath, "rbUpdatePath");
			this.rbUpdatePath.Checked = true;
			this.rbUpdatePath.Name = "rbUpdatePath";
			this.rbUpdatePath.TabStop = true;
			this.rbUpdatePath.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// rbCopy
			// 
			resources.ApplyResources(this.rbCopy, "rbCopy");
			this.rbCopy.Name = "rbCopy";
			this.rbCopy.UseVisualStyleBackColor = true;
			// 
			// rbConvert
			// 
			resources.ApplyResources(this.rbConvert, "rbConvert");
			this.rbConvert.Name = "rbConvert";
			this.rbConvert.UseVisualStyleBackColor = true;
			// 
			// txtMessage
			// 
			this.txtMessage.BackColor = System.Drawing.SystemColors.Control;
			this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.txtMessage, "txtMessage");
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbUpdatePath);
			this.groupBox1.Controls.Add(this.rbCopy);
			this.groupBox1.Controls.Add(this.rbConvert);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txtMessage);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// IdenticalMD5Dlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnOK);
			this.Name = "IdenticalMD5Dlg";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.RadioButton rbUpdatePath;
		private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rbCopy;
		private System.Windows.Forms.RadioButton rbConvert;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}