namespace SIL.Localize.Localizer
{
	partial class LabeledTextBox
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblHeading = new System.Windows.Forms.Label();
			this.txtText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblHeading
			// 
			this.lblHeading.BackColor = System.Drawing.SystemColors.Window;
			this.lblHeading.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeading.Location = new System.Drawing.Point(1, 1);
			this.lblHeading.Name = "lblHeading";
			this.lblHeading.Size = new System.Drawing.Size(289, 18);
			this.lblHeading.TabIndex = 0;
			this.lblHeading.Text = "#";
			this.lblHeading.Paint += new System.Windows.Forms.PaintEventHandler(this.lblHeading_Paint);
			// 
			// txtText
			// 
			this.txtText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtText.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtText.Location = new System.Drawing.Point(1, 19);
			this.txtText.Multiline = true;
			this.txtText.Name = "txtText";
			this.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtText.Size = new System.Drawing.Size(289, 130);
			this.txtText.TabIndex = 1;
			// 
			// LabeledTextBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Red;
			this.Controls.Add(this.txtText);
			this.Controls.Add(this.lblHeading);
			this.DoubleBuffered = true;
			this.Name = "LabeledTextBox";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(291, 150);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblHeading;
		private System.Windows.Forms.TextBox txtText;
	}
}
