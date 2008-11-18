namespace SIL.Localize.Localizer
{
	partial class ProjectDlg
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
			this.btnSrcFont = new System.Windows.Forms.Button();
			this.btnSrc = new System.Windows.Forms.Button();
			this.fldrBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblTarget = new System.Windows.Forms.Label();
			this.cboTarget = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtSrcTextFont = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTransFont = new System.Windows.Forms.TextBox();
			this.btnTransFont = new System.Windows.Forms.Button();
			this.fntDlg = new System.Windows.Forms.FontDialog();
			this.SuspendLayout();
			// 
			// txtSrc
			// 
			this.txtSrc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSrc.Location = new System.Drawing.Point(15, 73);
			this.txtSrc.Name = "txtSrc";
			this.txtSrc.Size = new System.Drawing.Size(484, 22);
			this.txtSrc.TabIndex = 3;
			this.txtSrc.TextChanged += new System.EventHandler(this.HandlePathTextChanged);
			// 
			// lblSrc
			// 
			this.lblSrc.AutoSize = true;
			this.lblSrc.BackColor = System.Drawing.Color.Transparent;
			this.lblSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSrc.Location = new System.Drawing.Point(16, 56);
			this.lblSrc.Name = "lblSrc";
			this.lblSrc.Size = new System.Drawing.Size(275, 14);
			this.lblSrc.TabIndex = 2;
			this.lblSrc.Text = "&Folder to scan for source language resource files:";
			// 
			// btnSrcFont
			// 
			this.btnSrcFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSrcFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSrcFont.Location = new System.Drawing.Point(505, 119);
			this.btnSrcFont.Name = "btnSrcFont";
			this.btnSrcFont.Size = new System.Drawing.Size(28, 22);
			this.btnSrcFont.TabIndex = 7;
			this.btnSrcFont.Text = "...";
			this.btnSrcFont.UseVisualStyleBackColor = true;
			this.btnSrcFont.Click += new System.EventHandler(this.btnSrcFont_Click);
			// 
			// btnSrc
			// 
			this.btnSrc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSrc.Location = new System.Drawing.Point(505, 73);
			this.btnSrc.Name = "btnSrc";
			this.btnSrc.Size = new System.Drawing.Size(28, 22);
			this.btnSrc.TabIndex = 4;
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
			this.btnOK.Location = new System.Drawing.Point(376, 214);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 26);
			this.btnOK.TabIndex = 11;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(457, 214);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblTarget
			// 
			this.lblTarget.AutoSize = true;
			this.lblTarget.BackColor = System.Drawing.Color.Transparent;
			this.lblTarget.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTarget.Location = new System.Drawing.Point(16, 10);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(105, 14);
			this.lblTarget.TabIndex = 0;
			this.lblTarget.Text = "Target Language:";
			// 
			// cboTarget
			// 
			this.cboTarget.DropDownHeight = 250;
			this.cboTarget.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTarget.FormattingEnabled = true;
			this.cboTarget.IntegralHeight = false;
			this.cboTarget.Location = new System.Drawing.Point(15, 27);
			this.cboTarget.Name = "cboTarget";
			this.cboTarget.Size = new System.Drawing.Size(253, 22);
			this.cboTarget.Sorted = true;
			this.cboTarget.TabIndex = 1;
			this.cboTarget.SelectedIndexChanged += new System.EventHandler(this.cboTarget_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 102);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 14);
			this.label1.TabIndex = 5;
			this.label1.Text = "Source Text Font:";
			// 
			// txtSrcTextFont
			// 
			this.txtSrcTextFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSrcTextFont.BackColor = System.Drawing.Color.Beige;
			this.txtSrcTextFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSrcTextFont.Location = new System.Drawing.Point(15, 119);
			this.txtSrcTextFont.Name = "txtSrcTextFont";
			this.txtSrcTextFont.ReadOnly = true;
			this.txtSrcTextFont.Size = new System.Drawing.Size(484, 22);
			this.txtSrcTextFont.TabIndex = 6;
			this.txtSrcTextFont.TabStop = false;
			this.txtSrcTextFont.TextChanged += new System.EventHandler(this.HandlePathTextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(16, 148);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 14);
			this.label2.TabIndex = 8;
			this.label2.Text = "Translation:";
			// 
			// txtTransFont
			// 
			this.txtTransFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTransFont.BackColor = System.Drawing.Color.Beige;
			this.txtTransFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTransFont.Location = new System.Drawing.Point(15, 165);
			this.txtTransFont.Name = "txtTransFont";
			this.txtTransFont.ReadOnly = true;
			this.txtTransFont.Size = new System.Drawing.Size(484, 22);
			this.txtTransFont.TabIndex = 9;
			this.txtTransFont.TabStop = false;
			this.txtTransFont.TextChanged += new System.EventHandler(this.HandlePathTextChanged);
			// 
			// btnTransFont
			// 
			this.btnTransFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTransFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTransFont.Location = new System.Drawing.Point(505, 165);
			this.btnTransFont.Name = "btnTransFont";
			this.btnTransFont.Size = new System.Drawing.Size(28, 22);
			this.btnTransFont.TabIndex = 10;
			this.btnTransFont.Text = "...";
			this.btnTransFont.UseVisualStyleBackColor = true;
			this.btnTransFont.Click += new System.EventHandler(this.btnTransFont_Click);
			// 
			// fntDlg
			// 
			this.fntDlg.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fntDlg.FontMustExist = true;
			// 
			// ProjectDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(544, 252);
			this.Controls.Add(this.btnTransFont);
			this.Controls.Add(this.txtTransFont);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtSrcTextFont);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboTarget);
			this.Controls.Add(this.lblTarget);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnSrc);
			this.Controls.Add(this.btnSrcFont);
			this.Controls.Add(this.lblSrc);
			this.Controls.Add(this.txtSrc);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProjectDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Localization Project";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtSrc;
		private System.Windows.Forms.Label lblSrc;
		private System.Windows.Forms.Button btnSrcFont;
		private System.Windows.Forms.Button btnSrc;
		private System.Windows.Forms.FolderBrowserDialog fldrBrowser;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblTarget;
		private System.Windows.Forms.ComboBox cboTarget;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSrcTextFont;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtTransFont;
		private System.Windows.Forms.Button btnTransFont;
		private System.Windows.Forms.FontDialog fntDlg;
	}
}