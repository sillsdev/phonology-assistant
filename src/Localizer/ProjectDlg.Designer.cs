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
			this.components = new System.ComponentModel.Container();
			this.lblSrc = new System.Windows.Forms.Label();
			this.btnSrcFont = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
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
			this.btnExe = new System.Windows.Forms.Button();
			this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
			this.lstSrcPaths = new System.Windows.Forms.ListBox();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnScan = new System.Windows.Forms.Button();
			this.cmnuScan = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuScanForDlls = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuScanForResx = new System.Windows.Forms.ToolStripMenuItem();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lblScanning = new System.Windows.Forms.Label();
			this.lblResCatalog = new System.Windows.Forms.Label();
			this.btnResCatalog = new System.Windows.Forms.Button();
			this.txtResCatalog = new System.Windows.Forms.TextBox();
			this.cmnuScan.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblSrc
			// 
			this.lblSrc.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblSrc.AutoSize = true;
			this.lblSrc.BackColor = System.Drawing.Color.Transparent;
			this.lblSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSrc.Location = new System.Drawing.Point(16, 225);
			this.lblSrc.Name = "lblSrc";
			this.lblSrc.Size = new System.Drawing.Size(277, 14);
			this.lblSrc.TabIndex = 16;
			this.lblSrc.Text = "&Assemblies (i.e. DLLs) to Scan for Source Strings:";
			// 
			// btnSrcFont
			// 
			this.btnSrcFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSrcFont.Location = new System.Drawing.Point(287, 80);
			this.btnSrcFont.Name = "btnSrcFont";
			this.btnSrcFont.Size = new System.Drawing.Size(28, 22);
			this.btnSrcFont.TabIndex = 6;
			this.btnSrcFont.Text = "...";
			this.btnSrcFont.UseVisualStyleBackColor = true;
			this.btnSrcFont.Click += new System.EventHandler(this.btnSrcFont_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnAdd.Location = new System.Drawing.Point(562, 242);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 26);
			this.btnAdd.TabIndex = 18;
			this.btnAdd.Text = "&Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// fldrBrowser
			// 
			this.fldrBrowser.Description = "Scan for Resources";
			this.fldrBrowser.ShowNewFolderButton = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(481, 435);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 26);
			this.btnOK.TabIndex = 23;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(562, 435);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 24;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblTarget
			// 
			this.lblTarget.AutoSize = true;
			this.lblTarget.BackColor = System.Drawing.Color.Transparent;
			this.lblTarget.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTarget.Location = new System.Drawing.Point(334, 9);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(105, 14);
			this.lblTarget.TabIndex = 2;
			this.lblTarget.Text = "Target &Language:";
			// 
			// cboTarget
			// 
			this.cboTarget.DropDownHeight = 250;
			this.cboTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTarget.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTarget.FormattingEnabled = true;
			this.cboTarget.IntegralHeight = false;
			this.cboTarget.Location = new System.Drawing.Point(333, 26);
			this.cboTarget.Name = "cboTarget";
			this.cboTarget.Size = new System.Drawing.Size(304, 22);
			this.cboTarget.Sorted = true;
			this.cboTarget.TabIndex = 3;
			// 
			// lblSrcTextFont
			// 
			this.lblSrcTextFont.AutoSize = true;
			this.lblSrcTextFont.BackColor = System.Drawing.Color.Transparent;
			this.lblSrcTextFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSrcTextFont.Location = new System.Drawing.Point(16, 63);
			this.lblSrcTextFont.Name = "lblSrcTextFont";
			this.lblSrcTextFont.Size = new System.Drawing.Size(108, 14);
			this.lblSrcTextFont.TabIndex = 4;
			this.lblSrcTextFont.Text = "&Source Text Font:";
			// 
			// txtSrcTextFont
			// 
			this.txtSrcTextFont.BackColor = System.Drawing.Color.Beige;
			this.txtSrcTextFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSrcTextFont.Location = new System.Drawing.Point(15, 80);
			this.txtSrcTextFont.Name = "txtSrcTextFont";
			this.txtSrcTextFont.ReadOnly = true;
			this.txtSrcTextFont.Size = new System.Drawing.Size(267, 22);
			this.txtSrcTextFont.TabIndex = 5;
			this.txtSrcTextFont.TabStop = false;
			// 
			// lblTransFont
			// 
			this.lblTransFont.AutoSize = true;
			this.lblTransFont.BackColor = System.Drawing.Color.Transparent;
			this.lblTransFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTransFont.Location = new System.Drawing.Point(334, 63);
			this.lblTransFont.Name = "lblTransFont";
			this.lblTransFont.Size = new System.Drawing.Size(134, 14);
			this.lblTransFont.TabIndex = 7;
			this.lblTransFont.Text = "&Target Language Font:";
			// 
			// txtTransFont
			// 
			this.txtTransFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTransFont.BackColor = System.Drawing.Color.Beige;
			this.txtTransFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTransFont.Location = new System.Drawing.Point(333, 80);
			this.txtTransFont.Name = "txtTransFont";
			this.txtTransFont.ReadOnly = true;
			this.txtTransFont.Size = new System.Drawing.Size(271, 22);
			this.txtTransFont.TabIndex = 8;
			this.txtTransFont.TabStop = false;
			// 
			// btnTransFont
			// 
			this.btnTransFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTransFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTransFont.Location = new System.Drawing.Point(609, 80);
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
			this.txtPrjName.Size = new System.Drawing.Size(300, 22);
			this.txtPrjName.TabIndex = 1;
			// 
			// txtExe
			// 
			this.txtExe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtExe.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtExe.Location = new System.Drawing.Point(15, 134);
			this.txtExe.Name = "txtExe";
			this.txtExe.Size = new System.Drawing.Size(589, 22);
			this.txtExe.TabIndex = 11;
			// 
			// lblExe
			// 
			this.lblExe.AutoSize = true;
			this.lblExe.BackColor = System.Drawing.Color.Transparent;
			this.lblExe.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblExe.Location = new System.Drawing.Point(16, 117);
			this.lblExe.Name = "lblExe";
			this.lblExe.Size = new System.Drawing.Size(307, 14);
			this.lblExe.TabIndex = 10;
			this.lblExe.Text = "&Executable Program of the Application Being Localized:";
			// 
			// btnExe
			// 
			this.btnExe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExe.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExe.Location = new System.Drawing.Point(609, 134);
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
			// 
			// lstSrcPaths
			// 
			this.lstSrcPaths.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lstSrcPaths.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstSrcPaths.HorizontalScrollbar = true;
			this.lstSrcPaths.IntegralHeight = false;
			this.lstSrcPaths.ItemHeight = 14;
			this.lstSrcPaths.Location = new System.Drawing.Point(15, 242);
			this.lstSrcPaths.Name = "lstSrcPaths";
			this.lstSrcPaths.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstSrcPaths.Size = new System.Drawing.Size(535, 180);
			this.lstSrcPaths.TabIndex = 17;
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnRemove.Location = new System.Drawing.Point(562, 274);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 26);
			this.btnRemove.TabIndex = 19;
			this.btnRemove.Text = "&Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnScan
			// 
			this.btnScan.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnScan.Location = new System.Drawing.Point(562, 318);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(75, 26);
			this.btnScan.TabIndex = 20;
			this.btnScan.Text = "S&can";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
			// 
			// cmnuScan
			// 
			this.cmnuScan.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuScanForDlls,
            this.cmnuScanForResx});
			this.cmnuScan.Name = "cmnuScan";
			this.cmnuScan.Size = new System.Drawing.Size(249, 48);
			// 
			// cmnuScanForDlls
			// 
			this.cmnuScanForDlls.Name = "cmnuScanForDlls";
			this.cmnuScanForDlls.Size = new System.Drawing.Size(248, 22);
			this.cmnuScanForDlls.Text = "Scan a Folder for .&DLL Files...";
			// 
			// cmnuScanForResx
			// 
			this.cmnuScanForResx.Name = "cmnuScanForResx";
			this.cmnuScanForResx.Size = new System.Drawing.Size(248, 22);
			this.cmnuScanForResx.Text = "Scan a Folder for .&Resx Files...";
			// 
			// progressBar
			// 
			this.progressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.progressBar.Location = new System.Drawing.Point(90, 438);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(355, 20);
			this.progressBar.TabIndex = 22;
			this.progressBar.Visible = false;
			// 
			// lblScanning
			// 
			this.lblScanning.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblScanning.AutoSize = true;
			this.lblScanning.BackColor = System.Drawing.Color.Transparent;
			this.lblScanning.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblScanning.Location = new System.Drawing.Point(16, 441);
			this.lblScanning.Name = "lblScanning";
			this.lblScanning.Size = new System.Drawing.Size(68, 14);
			this.lblScanning.TabIndex = 21;
			this.lblScanning.Text = "Scanning...";
			this.lblScanning.Visible = false;
			// 
			// lblResCatalog
			// 
			this.lblResCatalog.AutoSize = true;
			this.lblResCatalog.BackColor = System.Drawing.Color.Transparent;
			this.lblResCatalog.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblResCatalog.Location = new System.Drawing.Point(16, 171);
			this.lblResCatalog.Name = "lblResCatalog";
			this.lblResCatalog.Size = new System.Drawing.Size(246, 14);
			this.lblResCatalog.TabIndex = 13;
			this.lblResCatalog.Text = "&Resource Catalog (for resource comments):";
			// 
			// btnResCatalog
			// 
			this.btnResCatalog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnResCatalog.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResCatalog.Location = new System.Drawing.Point(609, 188);
			this.btnResCatalog.Name = "btnResCatalog";
			this.btnResCatalog.Size = new System.Drawing.Size(28, 22);
			this.btnResCatalog.TabIndex = 15;
			this.btnResCatalog.Text = "...";
			this.btnResCatalog.UseVisualStyleBackColor = true;
			this.btnResCatalog.Click += new System.EventHandler(this.btnResCatalog_Click);
			// 
			// txtResCatalog
			// 
			this.txtResCatalog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtResCatalog.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtResCatalog.Location = new System.Drawing.Point(15, 188);
			this.txtResCatalog.Name = "txtResCatalog";
			this.txtResCatalog.Size = new System.Drawing.Size(589, 22);
			this.txtResCatalog.TabIndex = 14;
			// 
			// ProjectDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(649, 473);
			this.Controls.Add(this.lblResCatalog);
			this.Controls.Add(this.btnResCatalog);
			this.Controls.Add(this.txtResCatalog);
			this.Controls.Add(this.txtPrjName);
			this.Controls.Add(this.lstSrcPaths);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.lblScanning);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.lblExe);
			this.Controls.Add(this.btnExe);
			this.Controls.Add(this.lblPrjName);
			this.Controls.Add(this.txtExe);
			this.Controls.Add(this.txtTransFont);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblTransFont);
			this.Controls.Add(this.btnTransFont);
			this.Controls.Add(this.txtSrcTextFont);
			this.Controls.Add(this.lblSrcTextFont);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cboTarget);
			this.Controls.Add(this.lblTarget);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnSrcFont);
			this.Controls.Add(this.lblSrc);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProjectDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Project Settings";
			this.cmnuScan.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSrc;
		private System.Windows.Forms.Button btnSrcFont;
		private System.Windows.Forms.Button btnAdd;
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
		private System.Windows.Forms.Button btnExe;
		private System.Windows.Forms.OpenFileDialog openFileDlg;
		private System.Windows.Forms.ListBox lstSrcPaths;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.ContextMenuStrip cmnuScan;
		private System.Windows.Forms.ToolStripMenuItem cmnuScanForDlls;
		private System.Windows.Forms.ToolStripMenuItem cmnuScanForResx;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label lblScanning;
		private System.Windows.Forms.Label lblResCatalog;
		private System.Windows.Forms.Button btnResCatalog;
		private System.Windows.Forms.TextBox txtResCatalog;
	}
}