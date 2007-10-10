namespace SIL.Pa.AddOn
{
	partial class RequestDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestDlg));
			this.pic = new System.Windows.Forms.PictureBox();
			this.paPanel1 = new SIL.Pa.Controls.PaPanel();
			this.txtRequest = new System.Windows.Forms.TextBox();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnNo = new System.Windows.Forms.Button();
			this.btnYes = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
			this.paPanel1.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pic
			// 
			resources.ApplyResources(this.pic, "pic");
			this.pic.Image = global::SIL.Pa.AddOn.Properties.Resources.kimidRequestDlgImage;
			this.pic.Name = "pic";
			this.pic.TabStop = false;
			// 
			// paPanel1
			// 
			resources.ApplyResources(this.paPanel1, "paPanel1");
			this.paPanel1.BackColor = System.Drawing.SystemColors.Window;
			this.paPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.paPanel1.ClipTextForChildControls = true;
			this.paPanel1.ControlReceivingFocusOnMnemonic = null;
			this.paPanel1.Controls.Add(this.txtRequest);
			this.paPanel1.Controls.Add(this.pic);
			this.paPanel1.DoubleBuffered = true;
			this.paPanel1.MnemonicGeneratesClick = false;
			this.paPanel1.Name = "paPanel1";
			this.paPanel1.PaintExplorerBarBackground = false;
			// 
			// txtRequest
			// 
			resources.ApplyResources(this.txtRequest, "txtRequest");
			this.txtRequest.BackColor = System.Drawing.SystemColors.Window;
			this.txtRequest.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtRequest.Name = "txtRequest";
			this.txtRequest.ReadOnly = true;
			this.txtRequest.TabStop = false;
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnNo);
			this.pnlButtons.Controls.Add(this.btnYes);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// btnNo
			// 
			resources.ApplyResources(this.btnNo, "btnNo");
			this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.btnNo.Name = "btnNo";
			this.btnNo.UseVisualStyleBackColor = true;
			// 
			// btnYes
			// 
			resources.ApplyResources(this.btnYes, "btnYes");
			this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.btnYes.Name = "btnYes";
			this.btnYes.UseVisualStyleBackColor = true;
			// 
			// RequestDlg
			// 
			this.AcceptButton = this.btnYes;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnNo;
			this.Controls.Add(this.pnlButtons);
			this.Controls.Add(this.paPanel1);
			this.Name = "RequestDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
			this.paPanel1.ResumeLayout(false);
			this.paPanel1.PerformLayout();
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pic;
		private SIL.Pa.Controls.PaPanel paPanel1;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.TextBox txtRequest;
		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.Button btnNo;
	}
}