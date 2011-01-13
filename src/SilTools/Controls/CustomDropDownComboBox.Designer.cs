namespace SilTools.Controls
{
	partial class CustomDropDownComboBox
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
			this._textBox = new System.Windows.Forms.TextBox();
			this._button = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// _textBox
			// 
			this._textBox.BackColor = System.Drawing.SystemColors.MenuHighlight;
			this._textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._textBox.Location = new System.Drawing.Point(4, 3);
			this._textBox.Name = "_textBox";
			this._textBox.Size = new System.Drawing.Size(100, 13);
			this._textBox.TabIndex = 0;
			// 
			// _button
			// 
			this._button.BackColor = System.Drawing.SystemColors.Desktop;
			this._button.Dock = System.Windows.Forms.DockStyle.Right;
			this._button.Location = new System.Drawing.Point(133, 0);
			this._button.Name = "_button";
			this._button.Size = new System.Drawing.Size(47, 21);
			this._button.TabIndex = 1;
			this._button.MouseLeave += new System.EventHandler(this.HandleButtonMouseLeave);
			this._button.Paint += new System.Windows.Forms.PaintEventHandler(this.m_button_Paint);
			this._button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleButtonMouseDown);
			this._button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleButtonMouseUp);
			this._button.MouseEnter += new System.EventHandler(this.HandleButtonMouseEnter);
			// 
			// CustomDropDownComboBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._textBox);
			this.Controls.Add(this._button);
			this.Name = "CustomDropDownComboBox";
			this.Size = new System.Drawing.Size(180, 21);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _textBox;
		private System.Windows.Forms.Panel _button;
	}
}
