namespace SIL.Pa.AddOn
{
	partial class RestoreMarkersMessageDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreMarkersMessageDlg));
			this.lblmsg = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.chkDontShowAgain = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// lblmsg
			// 
			resources.ApplyResources(this.lblmsg, "lblmsg");
			this.lblmsg.AutoEllipsis = true;
			this.lblmsg.Name = "lblmsg";
			// 
			// picIcon
			// 
			resources.ApplyResources(this.picIcon, "picIcon");
			this.picIcon.Name = "picIcon";
			this.picIcon.TabStop = false;
			// 
			// chkDontShowAgain
			// 
			resources.ApplyResources(this.chkDontShowAgain, "chkDontShowAgain");
			this.chkDontShowAgain.AutoEllipsis = true;
			this.chkDontShowAgain.Name = "chkDontShowAgain";
			this.chkDontShowAgain.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// RestoreMarkersMessageDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.chkDontShowAgain);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.lblmsg);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RestoreMarkersMessageDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblmsg;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.CheckBox chkDontShowAgain;
		protected System.Windows.Forms.Button btnOK;
	}
}