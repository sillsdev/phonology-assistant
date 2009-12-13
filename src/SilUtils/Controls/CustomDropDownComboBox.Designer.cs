namespace SilUtils.Controls
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
			this.m_txtBox = new System.Windows.Forms.TextBox();
			this.m_button = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// m_txtBox
			// 
			this.m_txtBox.BackColor = System.Drawing.SystemColors.MenuHighlight;
			this.m_txtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_txtBox.Location = new System.Drawing.Point(4, 3);
			this.m_txtBox.Name = "m_txtBox";
			this.m_txtBox.Size = new System.Drawing.Size(100, 13);
			this.m_txtBox.TabIndex = 0;
			// 
			// m_button
			// 
			this.m_button.BackColor = System.Drawing.SystemColors.Desktop;
			this.m_button.Dock = System.Windows.Forms.DockStyle.Right;
			this.m_button.Location = new System.Drawing.Point(133, 0);
			this.m_button.Name = "m_button";
			this.m_button.Size = new System.Drawing.Size(47, 21);
			this.m_button.TabIndex = 1;
			this.m_button.MouseLeave += new System.EventHandler(this.m_button_MouseLeave);
			this.m_button.Paint += new System.Windows.Forms.PaintEventHandler(this.m_button_Paint);
			this.m_button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_button_MouseDown);
			this.m_button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_button_MouseUp);
			this.m_button.MouseEnter += new System.EventHandler(this.m_button_MouseEnter);
			// 
			// CustomDropDownComboBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_txtBox);
			this.Controls.Add(this.m_button);
			this.Name = "CustomDropDownComboBox";
			this.Size = new System.Drawing.Size(180, 21);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox m_txtBox;
		private System.Windows.Forms.Panel m_button;
	}
}
