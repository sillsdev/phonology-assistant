namespace SIL.Pa.UI.Controls
{
	partial class CheckedGroupBox
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
			this._checkBox = new System.Windows.Forms.CheckBox();
			this._groupBox = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// _checkBox
			// 
			this._checkBox.AutoEllipsis = true;
			this._checkBox.AutoSize = true;
			this._checkBox.BackColor = System.Drawing.Color.Transparent;
			this._checkBox.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._checkBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this._checkBox.Location = new System.Drawing.Point(10, 0);
			this._checkBox.Name = "_checkBox";
			this._checkBox.Size = new System.Drawing.Size(91, 19);
			this._checkBox.TabIndex = 3;
			this._checkBox.Text = "change me!";
			this._checkBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBox.ThreeState = true;
			this._checkBox.UseVisualStyleBackColor = false;
			// 
			// _groupBox
			// 
			this._groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._groupBox.BackColor = System.Drawing.Color.Transparent;
			this._groupBox.Location = new System.Drawing.Point(0, 2);
			this._groupBox.Name = "_groupBox";
			this._groupBox.Padding = new System.Windows.Forms.Padding(7);
			this._groupBox.Size = new System.Drawing.Size(201, 63);
			this._groupBox.TabIndex = 4;
			this._groupBox.TabStop = false;
			// 
			// CheckedGroupBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._checkBox);
			this.Controls.Add(this._groupBox);
			this.Name = "CheckedGroupBox";
			this.Size = new System.Drawing.Size(201, 65);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.CheckBox _checkBox;
		protected System.Windows.Forms.GroupBox _groupBox;
	}
}
