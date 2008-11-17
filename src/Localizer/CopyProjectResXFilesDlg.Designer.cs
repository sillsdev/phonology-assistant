namespace SIL.Localize.Localizer
{
	partial class CopyProjectResXFilesDlg
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
			this.txtSrc = new System.Windows.Forms.TextBox();
			this.lblSrc = new System.Windows.Forms.Label();
			this.lblDest = new System.Windows.Forms.Label();
			this.txtDest = new System.Windows.Forms.TextBox();
			this.btnDest = new System.Windows.Forms.Button();
			this.btnSrc = new System.Windows.Forms.Button();
			this.fldrBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtSrc
			// 
			this.txtSrc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSrc.Location = new System.Drawing.Point(15, 36);
			this.txtSrc.Name = "txtSrc";
			this.txtSrc.Size = new System.Drawing.Size(484, 22);
			this.txtSrc.TabIndex = 1;
			this.txtSrc.TextChanged += new System.EventHandler(this.HandlePathTextChanged);
			// 
			// lblSrc
			// 
			this.lblSrc.AutoSize = true;
			this.lblSrc.BackColor = System.Drawing.Color.Transparent;
			this.lblSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSrc.Location = new System.Drawing.Point(16, 19);
			this.lblSrc.Name = "lblSrc";
			this.lblSrc.Size = new System.Drawing.Size(115, 14);
			this.lblSrc.TabIndex = 0;
			this.lblSrc.Text = ".Net &Project Folder:";
			// 
			// lblDest
			// 
			this.lblDest.AutoSize = true;
			this.lblDest.BackColor = System.Drawing.Color.Transparent;
			this.lblDest.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDest.Location = new System.Drawing.Point(16, 82);
			this.lblDest.Name = "lblDest";
			this.lblDest.Size = new System.Drawing.Size(109, 14);
			this.lblDest.TabIndex = 3;
			this.lblDest.Text = "&Destination Folder:";
			// 
			// txtDest
			// 
			this.txtDest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDest.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDest.Location = new System.Drawing.Point(15, 99);
			this.txtDest.Name = "txtDest";
			this.txtDest.Size = new System.Drawing.Size(484, 22);
			this.txtDest.TabIndex = 4;
			this.txtDest.TextChanged += new System.EventHandler(this.HandlePathTextChanged);
			// 
			// btnDest
			// 
			this.btnDest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDest.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDest.Location = new System.Drawing.Point(505, 99);
			this.btnDest.Name = "btnDest";
			this.btnDest.Size = new System.Drawing.Size(28, 22);
			this.btnDest.TabIndex = 5;
			this.btnDest.Text = "...";
			this.btnDest.UseVisualStyleBackColor = true;
			this.btnDest.Click += new System.EventHandler(this.btnDest_Click);
			// 
			// btnSrc
			// 
			this.btnSrc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSrc.Location = new System.Drawing.Point(505, 36);
			this.btnSrc.Name = "btnSrc";
			this.btnSrc.Size = new System.Drawing.Size(28, 22);
			this.btnSrc.TabIndex = 2;
			this.btnSrc.Text = "...";
			this.btnSrc.UseVisualStyleBackColor = true;
			this.btnSrc.Click += new System.EventHandler(this.btnSrc_Click);
			// 
			// fldrBrowser
			// 
			this.fldrBrowser.ShowNewFolderButton = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Enabled = false;
			this.btnOK.Location = new System.Drawing.Point(376, 143);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 26);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(457, 143);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// CopyProjectResXFilesDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(544, 181);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnSrc);
			this.Controls.Add(this.btnDest);
			this.Controls.Add(this.lblDest);
			this.Controls.Add(this.txtDest);
			this.Controls.Add(this.lblSrc);
			this.Controls.Add(this.txtSrc);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CopyProjectResXFilesDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Copy Project ResX Files";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtSrc;
		private System.Windows.Forms.Label lblSrc;
		private System.Windows.Forms.Label lblDest;
		private System.Windows.Forms.TextBox txtDest;
		private System.Windows.Forms.Button btnDest;
		private System.Windows.Forms.Button btnSrc;
		private System.Windows.Forms.FolderBrowserDialog fldrBrowser;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}