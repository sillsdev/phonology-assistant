namespace SIL.Pa.UI.Dialogs
{
	partial class CustomFieldsDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomFieldsDlg));
			this.lblInfo = new System.Windows.Forms.Label();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.Name = "lblInfo";
			// 
			// CustomFieldsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.lblInfo);
			this.Name = "CustomFieldsDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.lblInfo, 0);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblInfo;
	}
}
