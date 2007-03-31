namespace SIL.Pa.Dialogs
{
	partial class AmbiguousSequencesDlg
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
			this.pnlGrid = new SIL.Pa.Controls.PaPanel();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Location = new System.Drawing.Point(10, 274);
			this.pnlButtons.Size = new System.Drawing.Size(440, 40);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(274, 7);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(188, 7);
			// 
			// btnHelp
			// 
			this.btnHelp.Location = new System.Drawing.Point(360, 7);
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.DoubleBuffered = false;
			this.pnlGrid.Location = new System.Drawing.Point(10, 10);
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			this.pnlGrid.Size = new System.Drawing.Size(440, 304);
			this.pnlGrid.TabIndex = 102;
			this.pnlGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlGrid_Paint);
			// 
			// AmbiguousSequencesDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(460, 314);
			this.Controls.Add(this.pnlGrid);
			this.Name = "AmbiguousSequencesDlg";
			this.Text = "Ambiguous Sequences";
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private SIL.Pa.Controls.PaPanel pnlGrid;


	}
}