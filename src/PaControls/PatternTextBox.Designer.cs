namespace SIL.Pa.Controls
{
	partial class PatternTextBox
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
			this.txtPattern = new InternalPatternTextBox();
			this.SuspendLayout();
			// 
			// txtPattern
			// 
			this.txtPattern.AcceptsReturn = true;
			this.txtPattern.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.txtPattern.AllowDrop = true;
			this.txtPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
						| System.Windows.Forms.AnchorStyles.Left))));
			this.txtPattern.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPattern.HideSelection = false;
			this.txtPattern.Location = new System.Drawing.Point(3, 5);
			this.txtPattern.Name = "txtPattern";
			this.txtPattern.Size = new System.Drawing.Size(293, 24);
			this.txtPattern.TabIndex = 0;
			this.txtPattern.Enter += new System.EventHandler(this.txtPattern_Enter);
			this.txtPattern.DragOver += new System.Windows.Forms.DragEventHandler(this.txtPattern_DragOver);
			this.txtPattern.Click += new System.EventHandler(this.txtPattern_Click);
			this.txtPattern.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtPattern_DragDrop);
			this.txtPattern.Leave += new System.EventHandler(this.txtPattern_Leave);
			this.txtPattern.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPattern_KeyUp);
			this.txtPattern.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPattern_KeyPress);
			this.txtPattern.SizeChanged += new System.EventHandler(this.txtPattern_SizeChanged);
			this.txtPattern.TextChanged += new System.EventHandler(this.txtPattern_TextChanged);
			this.txtPattern.LocationChanged += new System.EventHandler(this.txtPattern_LocationChanged);
			// 
			// PatternTextBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.txtPattern);
			this.Name = "PatternTextBox";
			this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.Size = new System.Drawing.Size(299, 34);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private InternalPatternTextBox txtPattern;
	}
}
