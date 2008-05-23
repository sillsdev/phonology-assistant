namespace SIL.Pa.Updates
{
	partial class UpdateDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateDlg));
			this.lblAdminMsg = new System.Windows.Forms.Label();
			this.lblUpdateMsg = new System.Windows.Forms.Label();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblAdminMsg
			// 
			resources.ApplyResources(this.lblAdminMsg, "lblAdminMsg");
			this.lblAdminMsg.Name = "lblAdminMsg";
			// 
			// lblUpdateMsg
			// 
			resources.ApplyResources(this.lblUpdateMsg, "lblUpdateMsg");
			this.lblUpdateMsg.Name = "lblUpdateMsg";
			// 
			// btnUpdate
			// 
			resources.ApplyResources(this.btnUpdate, "btnUpdate");
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// UpdateDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.lblUpdateMsg);
			this.Controls.Add(this.lblAdminMsg);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UpdateDlg";
			this.ShowIcon = false;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblAdminMsg;
		private System.Windows.Forms.Label lblUpdateMsg;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnClose;
	}
}

