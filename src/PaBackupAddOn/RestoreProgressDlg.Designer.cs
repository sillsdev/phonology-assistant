namespace SIL.Pa.AddOn
{
	partial class RestoreProgressDlg
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
			this.prgressBar = new System.Windows.Forms.ProgressBar();
			this.lblMsg = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// prgressBar
			// 
			this.prgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.prgressBar.Location = new System.Drawing.Point(16, 38);
			this.prgressBar.Name = "prgressBar";
			this.prgressBar.Size = new System.Drawing.Size(320, 20);
			this.prgressBar.TabIndex = 0;
			this.prgressBar.UseWaitCursor = true;
			// 
			// lblMsg
			// 
			this.lblMsg.AutoEllipsis = true;
			this.lblMsg.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMsg.Location = new System.Drawing.Point(16, 18);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(320, 15);
			this.lblMsg.TabIndex = 1;
			this.lblMsg.Text = "#";
			this.lblMsg.UseWaitCursor = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(256, 38);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 26);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Visible = false;
			// 
			// RestoreProgressDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(352, 80);
			this.ControlBox = false;
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblMsg);
			this.Controls.Add(this.prgressBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RestoreProgressDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			this.UseWaitCursor = true;
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.ProgressBar prgressBar;
		internal System.Windows.Forms.Label lblMsg;
		internal System.Windows.Forms.Button btnOK;
	}
}