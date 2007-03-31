namespace SIL.Pa.Dialogs
{
	partial class PhoneInventoryDlg
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
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Location = new System.Drawing.Point(10, 420);
			this.pnlButtons.Size = new System.Drawing.Size(579, 40);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(413, 7);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(327, 7);
			// 
			// btnHelp
			// 
			this.btnHelp.Location = new System.Drawing.Point(499, 7);
			// 
			// PhoneInventoryDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(599, 460);
			this.Name = "PhoneInventoryDlg";
			this.Text = "Phone Inventory";
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
	}
}