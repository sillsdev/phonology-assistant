namespace SIL.Pa.UI.Dialogs
{
	partial class BRProgressDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BRProgressDlg));
			this.prgressBar = new System.Windows.Forms.ProgressBar();
			this.lblMsg = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// prgressBar
			// 
			resources.ApplyResources(this.prgressBar, "prgressBar");
			this.prgressBar.Name = "prgressBar";
			this.prgressBar.UseWaitCursor = true;
			// 
			// lblMsg
			// 
			this.lblMsg.AutoEllipsis = true;
			resources.ApplyResources(this.lblMsg, "lblMsg");
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.UseWaitCursor = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.UseWaitCursor = true;
			// 
			// RestoreProgressDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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