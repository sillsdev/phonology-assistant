namespace SIL.Pa
{
	partial class DisablePASplashScreen
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
			this.lblDescription = new System.Windows.Forms.Label();
			this.rbDisable = new System.Windows.Forms.RadioButton();
			this.rbEnable = new System.Windows.Forms.RadioButton();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblDescription
			// 
			this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDescription.Location = new System.Drawing.Point(12, 9);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(297, 46);
			this.lblDescription.TabIndex = 0;
			this.lblDescription.Text = "This program allows you to enable or disable the Phonology Assistant splash scree" +
				"n from displaying when Phonology Assistant starts up. What would you like to do?" +
				"";
			// 
			// rbDisable
			// 
			this.rbDisable.AutoSize = true;
			this.rbDisable.Location = new System.Drawing.Point(40, 68);
			this.rbDisable.Name = "rbDisable";
			this.rbDisable.Size = new System.Drawing.Size(132, 17);
			this.rbDisable.TabIndex = 1;
			this.rbDisable.TabStop = true;
			this.rbDisable.Text = "&Disable Splash Screen";
			this.rbDisable.UseVisualStyleBackColor = true;
			// 
			// rbEnable
			// 
			this.rbEnable.AutoSize = true;
			this.rbEnable.Location = new System.Drawing.Point(40, 97);
			this.rbEnable.Name = "rbEnable";
			this.rbEnable.Size = new System.Drawing.Size(130, 17);
			this.rbEnable.TabIndex = 2;
			this.rbEnable.TabStop = true;
			this.rbEnable.Text = "&Enable Splash Screen";
			this.rbEnable.UseVisualStyleBackColor = true;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnClose.Location = new System.Drawing.Point(229, 104);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(80, 26);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// DisablePASplashScreen
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(321, 142);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.rbEnable);
			this.Controls.Add(this.rbDisable);
			this.Controls.Add(this.lblDescription);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "DisablePASplashScreen";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Phonology Assistant Splash Screen";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.RadioButton rbDisable;
		private System.Windows.Forms.RadioButton rbEnable;
		protected System.Windows.Forms.Button btnClose;
	}
}

