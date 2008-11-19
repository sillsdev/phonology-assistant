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
			this.lblSrcTextFont = new System.Windows.Forms.Label();
			this.txtSrcTextFont = new System.Windows.Forms.TextBox();
			this.lblTransFont = new System.Windows.Forms.Label();
			this.txtTransFont = new System.Windows.Forms.TextBox();
			this.btnTransFont = new System.Windows.Forms.Button();
			this.fntDlg = new System.Windows.Forms.FontDialog();
			this.lblPrjName = new System.Windows.Forms.Label();
			this.txtPrjName = new System.Windows.Forms.TextBox();
			this.txtExe = new System.Windows.Forms.TextBox();
			this.lblExe = new System.Windows.Forms.Label();
			this.rbScanResx = new System.Windows.Forms.RadioButton();
			this.rbScanDll = new System.Windows.Forms.RadioButton();
			this.btnExe = new System.Windows.Forms.Button();
			this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// txtSrc
			// 
			this.txtSrc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSrc.Location = new System.Drawing.Point(15, 296);
			this.txtSrc.Name = "txtSrc";
			this.txtSrc.Size = new System.Drawing.Size(361, 22);
			this.txtSrc.TabIndex = 14;
			// 
			// lblSrc
			// 
			this.lblSrc.AutoSize = true;
			this.lblSrc.BackColor = System.Drawing.Color.Transparent;
			this.lblSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSrc.Location = new System.Drawing.Point(16, 279);
			this.lblSrc.Name = "lblSrc";
			this.lblSrc.Size = new System.Drawing.Size(267, 14);
			this.lblSrc.TabIndex = 13;
			this.lblSrc.Text = "&Folder to Scan for Source Language Resources:";
			// 
			// btnSrcFont
			// 
			this.btnSrcFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSrcFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSrcFont.Location = new System.Drawing.Point(381, 134);
			this.btnSrcFont.Name = "btnSrcFont";
			this.btnSrcFont.Size = new System.Drawing.Size(28, 22);
			this.btnSrcFont.TabIndex = 6;
			this.btnSrcFont.Text = "...";
			this.btnSrcFont.UseVisualStyleBackColor = true;
			this.btnSrcFont.Click += new System.EventHandler(this.btnSrcFont_Click);
			// 
			// btnSrc
			// 
			this.btnSrc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSrc.Location = new System.Drawing.Point(381, 296);
			this.btnSrc.Name = "btnSrc";
			this.btnSrc.Size = new System.Drawing.Size(28, 22);
			this.btnSrc.TabIndex = 15;
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
			this.btnOK.Location = new System.Drawing.Point(253, 357);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 26);
			this.btnOK.TabIndex = 18;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(334, 357);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 19;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblTarget
			// 
			this.lblTarget.AutoSize = true;
			this.lblTarget.BackColor = System.Drawing.Color.Transparent;
			this.lblTarget.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTarget.Location = new System.Drawing.Point(16, 63);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(105, 14);
			this.lblTarget.TabIndex = 2;
			this.lblTarget.Text = "Target &Language:";
			// 
			// cboTarget
			// 
			this.cboTarget.DropDownHeight = 250;
			this.cboTarget.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTarget.FormattingEnabled = true;
			this.cboTarget.IntegralHeight = false;
			this.cboTarget.Location = new System.Drawing.Point(15, 80);
			this.cboTarget.Name = "cboTarget";
			this.cboTarget.Size = new System.Drawing.Size(253, 22);
			this.cboTarget.Sorted = true;
			this.cboTarget.TabIndex = 3;
			// 
			// lblSrcTextFont
			// 
			this.lblSrcTextFont.AutoSize = true;
			this.lblSrcTextFont.BackColor = System.Drawing.Color.Transparent;
			this.lblSrcTextFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSrcTextFont.Location = new System.Drawing.Point(16, 117);
			this.lblSrcTextFont.Name = "lblSrcTextFont";
			this.lblSrcTextFont.Size = new System.Drawing.Size(108, 14);
			this.lblSrcTextFont.TabIndex = 4;
			this.lblSrcTextFont.Text = "&Source Text Font:";
			// 
			// txtSrcTextFont
			// 
			this.txtSrcTextFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSrcTextFont.BackColor = System.Drawing.Color.Beige;
			this.txtSrcTextFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSrcTextFont.Location = new System.Drawing.Point(15, 134);
			this.txtSrcTextFont.Name = "txtSrcTextFont";
			this.txtSrcTextFont.ReadOnly = true;
			this.txtSrcTextFont.Size = new System.Drawing.Size(361, 22);
			this.txtSrcTextFont.TabIndex = 5;
			this.txtSrcTextFont.TabStop = false;
			// 
			// lblTransFont
			// 
			this.lblTransFont.AutoSize = true;
			this.lblTransFont.BackColor = System.Drawing.Color.Transparent;
			this.lblTransFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransFont.Location = new System.Drawing.Point(16, 171);
			this.lblTransFont.Name = "lblTransFont";
			this.lblTransFont.Size = new System.Drawing.Size(99, 14);
			this.lblTransFont.TabIndex = 7;
			this.lblTransFont.Text = "&Translation Font:";
			// 
			// txtTransFont
			// 
			this.txtTransFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTransFont.BackColor = System.Drawing.Color.Beige;
			this.txtTransFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTransFont.Location = new System.Drawing.Point(15, 188);
			this.txtTransFont.Name = "txtTransFont";
			this.txtTransFont.ReadOnly = true;
			this.txtTransFont.Size = new System.Drawing.Size(361, 22);
			this.txtTransFont.TabIndex = 8;
			this.txtTransFont.TabStop = false;
			// 
			// btnTransFont
			// 
			this.btnTransFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTransFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTransFont.Location = new System.Drawing.Point(381, 188);
			this.btnTransFont.Name = "btnTransFont";
			this.btnTransFont.Size = new System.Drawing.Size(28, 22);
			this.btnTransFont.TabIndex = 9;
			this.btnTransFont.Text = "...";
			this.btnTransFont.UseVisualStyleBackColor = true;
			this.btnTransFont.Click += new System.EventHandler(this.btnTransFont_Click);
			// 
			// fntDlg
			// 
			this.fntDlg.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fntDlg.FontMustExist = true;
			// 
			// lblPrjName
			// 
			this.lblPrjName.AutoSize = true;
			this.lblPrjName.BackColor = System.Drawing.Color.Transparent;
			this.lblPrjName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPrjName.Location = new System.Drawing.Point(16, 9);
			this.lblPrjName.Name = "lblPrjName";
			this.lblPrjName.Size = new System.Drawing.Size(85, 14);
			this.lblPrjName.TabIndex = 0;
			this.lblPrjName.Text = "Project &Name:";
			// 
			// txtPrjName
			// 
			this.txtPrjName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPrjName.Location = new System.Drawing.Point(15, 26);
			this.txtPrjName.Name = "txtPrjName";
			this.txtPrjName.Size = new System.Drawing.Size(253, 22);
			this.txtPrjName.TabIndex = 1;
			// 
			// txtExe
			// 
			this.txtExe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtExe.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtExe.Location = new System.Drawing.Point(15, 242);
			this.txtExe.Name = "txtExe";
			this.txtExe.Size = new System.Drawing.Size(361, 22);
			this.txtExe.TabIndex = 11;
			// 
			// lblExe
			// 
			this.lblExe.AutoSize = true;
			this.lblExe.BackColor = System.Drawing.Color.Transparent;
			this.lblExe.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblExe.Location = new System.Drawing.Point(16, 225);
			this.lblExe.Name = "lblExe";
			this.lblExe.Size = new System.Drawing.Size(181, 14);
			this.lblExe.TabIndex = 10;
			this.lblExe.Text = "&Executable Program to Localize:";
			// 
			// rbScanResx
			// 
			this.rbScanResx.AutoSize = true;
			this.rbScanResx.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbScanResx.Location = new System.Drawing.Point(19, 324);
			this.rbScanResx.Name = "rbScanResx";
			this.rbScanResx.Size = new System.Drawing.Size(107, 18);
			this.rbScanResx.TabIndex = 16;
			this.rbScanResx.TabStop = true;
			this.rbScanResx.Text = "Scan .&resx Files";
			this.rbScanResx.UseVisualStyleBackColor = true;
			// 
			// rbScanDll
			// 
			this.rbScanDll.AutoSize = true;
			this.rbScanDll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbScanDll.Location = new System.Drawing.Point(179, 324);
			this.rbScanDll.Name = "rbScanDll";
			this.rbScanDll.Size = new System.Drawing.Size(148, 18);
			this.rbScanDll.TabIndex = 17;
			this.rbScanDll.TabStop = true;
			this.rbScanDll.Text = "Scan .exe and .&dll Files";
			this.rbScanDll.UseVisualStyleBackColor = true;
			// 
			// btnExe
			// 
			this.btnExe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExe.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExe.Location = new System.Drawing.Point(381, 242);
			this.btnExe.Name = "btnExe";
			this.btnExe.Size = new System.Drawing.Size(28, 22);
			this.btnExe.TabIndex = 12;
			this.btnExe.Text = "...";
			this.btnExe.UseVisualStyleBackColor = true;
			this.btnExe.Click += new System.EventHandler(this.btnExe_Click);
			// 
			// openFileDlg
			// 
			this.openFileDlg.DefaultExt = "exe";
			this.openFileDlg.Filter = "Program Files (*.exe)|*.exe|All Files (*.*)|*.*";
			this.openFileDlg.Title = "Executable Program to Localize";
			// 
			// ProjectDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(421, 395);
			this.Controls.Add(this.btnExe);
			this.Controls.Add(this.rbScanDll);
			this.Controls.Add(this.rbScanResx);
			this.Controls.Add(this.lblExe);
			this.Controls.Add(this.txtExe);
			this.Controls.Add(this.txtPrjName);
			this.Controls.Add(this.lblPrjName);
			this.Controls.Add(this.btnTransFont);
			this.Controls.Add(this.txtTransFont);
			this.Controls.Add(this.lblTransFont);
			this.Controls.Add(this.txtSrcTextFont);
			this.Controls.Add(this.lblSrcTextFont);
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
		private System.Windows.Forms.Label lblSrcTextFont;
		private System.Windows.Forms.TextBox txtSrcTextFont;
		private System.Windows.Forms.Label lblTransFont;
		private System.Windows.Forms.TextBox txtTransFont;
		private System.Windows.Forms.Button btnTransFont;
		private System.Windows.Forms.FontDialog fntDlg;
		private System.Windows.Forms.Label lblPrjName;
		private System.Windows.Forms.TextBox txtPrjName;
		private System.Windows.Forms.TextBox txtExe;
		private System.Windows.Forms.Label lblExe;
		private System.Windows.Forms.RadioButton rbScanResx;
		private System.Windows.Forms.RadioButton rbScanDll;
		private System.Windows.Forms.Button btnExe;
		private System.Windows.Forms.OpenFileDialog openFileDlg;
	}
}