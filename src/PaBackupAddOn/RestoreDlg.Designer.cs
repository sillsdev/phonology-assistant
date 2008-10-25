namespace SIL.Pa.BackupRestoreAddOn
{
	partial class RestoreDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreDlg));
			this.txtPrjLocation = new System.Windows.Forms.TextBox();
			this.btnChgDSLocation = new System.Windows.Forms.Button();
			this.btnChgPrjLocation = new System.Windows.Forms.Button();
			this.btnRestore = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblProject = new System.Windows.Forms.Label();
			this.fldrBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.chkUseSame = new System.Windows.Forms.CheckBox();
			this.grpPrjLocation = new System.Windows.Forms.GroupBox();
			this.grpDataSources = new System.Windows.Forms.GroupBox();
			this.grid = new System.Windows.Forms.DataGridView();
			this.colDataSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDataSourceFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colPrevRstFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSaXmFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.grpPrjLocation.SuspendLayout();
			this.grpDataSources.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.SuspendLayout();
			// 
			// txtPrjLocation
			// 
			resources.ApplyResources(this.txtPrjLocation, "txtPrjLocation");
			this.txtPrjLocation.BackColor = System.Drawing.SystemColors.Window;
			this.txtPrjLocation.Name = "txtPrjLocation";
			this.txtPrjLocation.ReadOnly = true;
			// 
			// btnChgDSLocation
			// 
			resources.ApplyResources(this.btnChgDSLocation, "btnChgDSLocation");
			this.btnChgDSLocation.Name = "btnChgDSLocation";
			this.btnChgDSLocation.UseVisualStyleBackColor = true;
			this.btnChgDSLocation.Click += new System.EventHandler(this.btnChgDSLocation_Click);
			// 
			// btnChgPrjLocation
			// 
			resources.ApplyResources(this.btnChgPrjLocation, "btnChgPrjLocation");
			this.btnChgPrjLocation.Name = "btnChgPrjLocation";
			this.btnChgPrjLocation.UseVisualStyleBackColor = true;
			this.btnChgPrjLocation.Click += new System.EventHandler(this.btnChgPrjLocation_Click);
			// 
			// btnRestore
			// 
			resources.ApplyResources(this.btnRestore, "btnRestore");
			this.btnRestore.Name = "btnRestore";
			this.btnRestore.UseVisualStyleBackColor = true;
			this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// lblProject
			// 
			resources.ApplyResources(this.lblProject, "lblProject");
			this.lblProject.AutoEllipsis = true;
			this.lblProject.Name = "lblProject";
			// 
			// chkUseSame
			// 
			resources.ApplyResources(this.chkUseSame, "chkUseSame");
			this.chkUseSame.AutoEllipsis = true;
			this.chkUseSame.Name = "chkUseSame";
			this.chkUseSame.UseVisualStyleBackColor = true;
			this.chkUseSame.CheckedChanged += new System.EventHandler(this.chkUseSame_CheckedChanged);
			// 
			// grpPrjLocation
			// 
			resources.ApplyResources(this.grpPrjLocation, "grpPrjLocation");
			this.grpPrjLocation.Controls.Add(this.txtPrjLocation);
			this.grpPrjLocation.Controls.Add(this.btnChgPrjLocation);
			this.grpPrjLocation.Name = "grpPrjLocation";
			this.grpPrjLocation.TabStop = false;
			// 
			// grpDataSources
			// 
			resources.ApplyResources(this.grpDataSources, "grpDataSources");
			this.grpDataSources.Controls.Add(this.grid);
			this.grpDataSources.Controls.Add(this.btnChgDSLocation);
			this.grpDataSources.Controls.Add(this.chkUseSame);
			this.grpDataSources.Name = "grpDataSources";
			this.grpDataSources.TabStop = false;
			// 
			// grid
			// 
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToDeleteRows = false;
			this.grid.AllowUserToResizeRows = false;
			resources.ApplyResources(this.grid, "grid");
			this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDataSource,
            this.colDataSourceFolder,
            this.colPrevRstFolder,
            this.colSaXmFile});
			this.grid.MultiSelect = false;
			this.grid.Name = "grid";
			this.grid.ReadOnly = true;
			this.grid.RowHeadersVisible = false;
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_CellPainting);
			// 
			// colDataSource
			// 
			resources.ApplyResources(this.colDataSource, "colDataSource");
			this.colDataSource.Name = "colDataSource";
			this.colDataSource.ReadOnly = true;
			this.colDataSource.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// colDataSourceFolder
			// 
			resources.ApplyResources(this.colDataSourceFolder, "colDataSourceFolder");
			this.colDataSourceFolder.Name = "colDataSourceFolder";
			this.colDataSourceFolder.ReadOnly = true;
			this.colDataSourceFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// colPrevRstFolder
			// 
			resources.ApplyResources(this.colPrevRstFolder, "colPrevRstFolder");
			this.colPrevRstFolder.Name = "colPrevRstFolder";
			this.colPrevRstFolder.ReadOnly = true;
			this.colPrevRstFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// colSaXmFile
			// 
			resources.ApplyResources(this.colSaXmFile, "colSaXmFile");
			this.colSaXmFile.Name = "colSaXmFile";
			this.colSaXmFile.ReadOnly = true;
			this.colSaXmFile.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// RestoreDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.Controls.Add(this.grpDataSources);
			this.Controls.Add(this.grpPrjLocation);
			this.Controls.Add(this.lblProject);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnRestore);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RestoreDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.grpPrjLocation.ResumeLayout(false);
			this.grpPrjLocation.PerformLayout();
			this.grpDataSources.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox txtPrjLocation;
		private System.Windows.Forms.Button btnChgDSLocation;
		private System.Windows.Forms.Button btnChgPrjLocation;
		private System.Windows.Forms.Button btnRestore;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblProject;
		private System.Windows.Forms.FolderBrowserDialog fldrBrowser;
		private System.Windows.Forms.CheckBox chkUseSame;
		private System.Windows.Forms.GroupBox grpPrjLocation;
		private System.Windows.Forms.GroupBox grpDataSources;
		private System.Windows.Forms.DataGridView grid;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDataSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDataSourceFolder;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPrevRstFolder;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSaXmFile;
	}
}