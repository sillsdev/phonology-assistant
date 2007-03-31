namespace SIL.Pa.Dialogs
{
	partial class SaveXYChartDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveXYChartDlg));
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.tlpName = new System.Windows.Forms.TableLayoutPanel();
			this.pnlButtons.SuspendLayout();
			this.tlpName.SuspendLayout();
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
			// txtName
			// 
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			this.txtName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			this.lblName.Name = "lblName";
			// 
			// tlpName
			// 
			resources.ApplyResources(this.tlpName, "tlpName");
			this.tlpName.Controls.Add(this.lblName, 0, 0);
			this.tlpName.Controls.Add(this.txtName, 1, 0);
			this.tlpName.Name = "tlpName";
			// 
			// SaveXYChartDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlpName);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "SaveXYChartDlg";
			this.Controls.SetChildIndex(this.tlpName, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.pnlButtons.ResumeLayout(false);
			this.tlpName.ResumeLayout(false);
			this.tlpName.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TableLayoutPanel tlpName;
	}
}