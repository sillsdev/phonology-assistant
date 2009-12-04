namespace SIL.Pa.UI.Dialogs
{
	partial class OKCancelDlgBase
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OKCancelDlgBase));
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
			this.pnlButtons.Controls.Add(this.btnHelp);
			this.pnlButtons.Controls.Add(this.btnCancel);
			this.pnlButtons.Controls.Add(this.btnOK);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.InternalHandleHelpClick);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// OKCancelDlgBase
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.pnlButtons);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OKCancelDlgBase";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnCancel;
		protected System.Windows.Forms.Button btnOK;
		protected System.Windows.Forms.Button btnHelp;



	}
}