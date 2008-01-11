namespace SIL.Pa.AddOn
{
	partial class CopiedToClipboardDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopiedToClipboardDlg));
			this.lblMsg = new System.Windows.Forms.Label();
			this.txtMailAddress = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblMailAddress = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtSuggestedSubject = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblMsg
			// 
			resources.ApplyResources(this.lblMsg, "lblMsg");
			this.lblMsg.Name = "lblMsg";
			// 
			// txtMailAddress
			// 
			resources.ApplyResources(this.txtMailAddress, "txtMailAddress");
			this.txtMailAddress.BackColor = System.Drawing.SystemColors.Window;
			this.txtMailAddress.Name = "txtMailAddress";
			this.txtMailAddress.ReadOnly = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// lblMailAddress
			// 
			resources.ApplyResources(this.lblMailAddress, "lblMailAddress");
			this.lblMailAddress.Name = "lblMailAddress";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// txtSuggestedSubject
			// 
			resources.ApplyResources(this.txtSuggestedSubject, "txtSuggestedSubject");
			this.txtSuggestedSubject.BackColor = System.Drawing.SystemColors.Window;
			this.txtSuggestedSubject.Name = "txtSuggestedSubject";
			this.txtSuggestedSubject.ReadOnly = true;
			// 
			// CopiedToClipboardDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtSuggestedSubject);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblMailAddress);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtMailAddress);
			this.Controls.Add(this.lblMsg);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CopiedToClipboardDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.TextBox txtMailAddress;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblMailAddress;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtSuggestedSubject;
	}
}