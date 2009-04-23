namespace SIL.Localize.CreateResourceCatalog
{
	partial class Dialog
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
			this.lblSrc = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.fldrBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblPrjName = new System.Windows.Forms.Label();
			this.txtAppName = new System.Windows.Forms.TextBox();
			this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
			this.lstSrcPaths = new System.Windows.Forms.ListBox();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnScan = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lblScanning = new System.Windows.Forms.Label();
			this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// lblSrc
			// 
			this.lblSrc.AutoSize = true;
			this.lblSrc.BackColor = System.Drawing.Color.Transparent;
			this.lblSrc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSrc.Location = new System.Drawing.Point(16, 59);
			this.lblSrc.Name = "lblSrc";
			this.lblSrc.Size = new System.Drawing.Size(213, 14);
			this.lblSrc.TabIndex = 13;
			this.lblSrc.Text = "&Projects to Scan for .ResX Resources:";
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(550, 76);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 26);
			this.btnAdd.TabIndex = 15;
			this.btnAdd.Text = "&Add...";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// fldrBrowser
			// 
			this.fldrBrowser.Description = "Scan for C# Projects";
			this.fldrBrowser.ShowNewFolderButton = false;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(469, 269);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 26);
			this.btnOK.TabIndex = 18;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(550, 269);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 19;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblPrjName
			// 
			this.lblPrjName.AutoSize = true;
			this.lblPrjName.BackColor = System.Drawing.Color.Transparent;
			this.lblPrjName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPrjName.Location = new System.Drawing.Point(16, 9);
			this.lblPrjName.Name = "lblPrjName";
			this.lblPrjName.Size = new System.Drawing.Size(105, 14);
			this.lblPrjName.TabIndex = 0;
			this.lblPrjName.Text = "Application &Name:";
			// 
			// txtAppName
			// 
			this.txtAppName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtAppName.Location = new System.Drawing.Point(15, 26);
			this.txtAppName.Name = "txtAppName";
			this.txtAppName.Size = new System.Drawing.Size(529, 22);
			this.txtAppName.TabIndex = 1;
			// 
			// openFileDlg
			// 
			this.openFileDlg.DefaultExt = "csproj";
			this.openFileDlg.Filter = "C# Projects (*.csproj)|*.csproj";
			this.openFileDlg.Multiselect = true;
			this.openFileDlg.Title = "Specify C# Projects";
			// 
			// lstSrcPaths
			// 
			this.lstSrcPaths.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstSrcPaths.HorizontalScrollbar = true;
			this.lstSrcPaths.IntegralHeight = false;
			this.lstSrcPaths.ItemHeight = 14;
			this.lstSrcPaths.Location = new System.Drawing.Point(15, 76);
			this.lstSrcPaths.Name = "lstSrcPaths";
			this.lstSrcPaths.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstSrcPaths.Size = new System.Drawing.Size(529, 180);
			this.lstSrcPaths.TabIndex = 14;
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(550, 108);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 26);
			this.btnRemove.TabIndex = 16;
			this.btnRemove.Text = "&Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnScan
			// 
			this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnScan.Location = new System.Drawing.Point(550, 152);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(75, 26);
			this.btnScan.TabIndex = 17;
			this.btnScan.Text = "S&can...";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(90, 272);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(355, 20);
			this.progressBar.TabIndex = 20;
			this.progressBar.Visible = false;
			// 
			// lblScanning
			// 
			this.lblScanning.AutoSize = true;
			this.lblScanning.BackColor = System.Drawing.Color.Transparent;
			this.lblScanning.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblScanning.Location = new System.Drawing.Point(16, 275);
			this.lblScanning.Name = "lblScanning";
			this.lblScanning.Size = new System.Drawing.Size(68, 14);
			this.lblScanning.TabIndex = 21;
			this.lblScanning.Text = "Scanning...";
			this.lblScanning.Visible = false;
			// 
			// saveFileDlg
			// 
			this.saveFileDlg.Filter = "ResX Comment Catalog (*.rcc)|*.rcc|All Files (*.*)|*.*";
			this.saveFileDlg.RestoreDirectory = true;
			// 
			// Dialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(637, 305);
			this.ControlBox = false;
			this.Controls.Add(this.txtAppName);
			this.Controls.Add(this.lstSrcPaths);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.lblScanning);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.lblPrjName);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lblSrc);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Dialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Create Resource Catalog";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSrc;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.FolderBrowserDialog fldrBrowser;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblPrjName;
		private System.Windows.Forms.TextBox txtAppName;
		private System.Windows.Forms.OpenFileDialog openFileDlg;
		private System.Windows.Forms.ListBox lstSrcPaths;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label lblScanning;
		private System.Windows.Forms.SaveFileDialog saveFileDlg;
	}
}