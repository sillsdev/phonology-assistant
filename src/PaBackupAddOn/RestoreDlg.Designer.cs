namespace SIL.Pa.AddOn
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
			this.txtPrjLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrjLocation.BackColor = System.Drawing.SystemColors.Window;
			this.txtPrjLocation.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPrjLocation.Location = new System.Drawing.Point(10, 24);
			this.txtPrjLocation.Name = "txtPrjLocation";
			this.txtPrjLocation.ReadOnly = true;
			this.txtPrjLocation.Size = new System.Drawing.Size(435, 21);
			this.txtPrjLocation.TabIndex = 0;
			// 
			// btnChgDSLocation
			// 
			this.btnChgDSLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnChgDSLocation.Location = new System.Drawing.Point(365, 118);
			this.btnChgDSLocation.Name = "btnChgDSLocation";
			this.btnChgDSLocation.Size = new System.Drawing.Size(80, 26);
			this.btnChgDSLocation.TabIndex = 2;
			this.btnChgDSLocation.Text = "Cha&nge...";
			this.btnChgDSLocation.UseVisualStyleBackColor = true;
			this.btnChgDSLocation.Click += new System.EventHandler(this.btnChgDSLocation_Click);
			// 
			// btnChgPrjLocation
			// 
			this.btnChgPrjLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnChgPrjLocation.Location = new System.Drawing.Point(365, 51);
			this.btnChgPrjLocation.Name = "btnChgPrjLocation";
			this.btnChgPrjLocation.Size = new System.Drawing.Size(80, 26);
			this.btnChgPrjLocation.TabIndex = 1;
			this.btnChgPrjLocation.Text = "Ch&ange...";
			this.btnChgPrjLocation.UseVisualStyleBackColor = true;
			this.btnChgPrjLocation.Click += new System.EventHandler(this.btnChgPrjLocation_Click);
			// 
			// btnRestore
			// 
			this.btnRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRestore.Location = new System.Drawing.Point(304, 305);
			this.btnRestore.Name = "btnRestore";
			this.btnRestore.Size = new System.Drawing.Size(80, 26);
			this.btnRestore.TabIndex = 3;
			this.btnRestore.Text = "&Restore";
			this.btnRestore.UseVisualStyleBackColor = true;
			this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(390, 305);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(80, 26);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// lblProject
			// 
			this.lblProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblProject.AutoEllipsis = true;
			this.lblProject.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProject.Location = new System.Drawing.Point(12, 13);
			this.lblProject.Name = "lblProject";
			this.lblProject.Size = new System.Drawing.Size(458, 15);
			this.lblProject.TabIndex = 0;
			this.lblProject.Text = "Phonology Assistant found the {0} project to restore.";
			// 
			// chkUseSame
			// 
			this.chkUseSame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkUseSame.AutoEllipsis = true;
			this.chkUseSame.Location = new System.Drawing.Point(10, 123);
			this.chkUseSame.Name = "chkUseSame";
			this.chkUseSame.Size = new System.Drawing.Size(349, 19);
			this.chkUseSame.TabIndex = 1;
			this.chkUseSame.Text = "Restore &where project files are restored";
			this.chkUseSame.UseVisualStyleBackColor = true;
			this.chkUseSame.CheckedChanged += new System.EventHandler(this.chkUseSame_CheckedChanged);
			// 
			// grpPrjLocation
			// 
			this.grpPrjLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpPrjLocation.Controls.Add(this.txtPrjLocation);
			this.grpPrjLocation.Controls.Add(this.btnChgPrjLocation);
			this.grpPrjLocation.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grpPrjLocation.Location = new System.Drawing.Point(15, 44);
			this.grpPrjLocation.Name = "grpPrjLocation";
			this.grpPrjLocation.Size = new System.Drawing.Size(455, 89);
			this.grpPrjLocation.TabIndex = 1;
			this.grpPrjLocation.TabStop = false;
			this.grpPrjLocation.Text = "Folder in which the &project files will be restored";
			// 
			// grpDataSources
			// 
			this.grpDataSources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpDataSources.Controls.Add(this.grid);
			this.grpDataSources.Controls.Add(this.btnChgDSLocation);
			this.grpDataSources.Controls.Add(this.chkUseSame);
			this.grpDataSources.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grpDataSources.Location = new System.Drawing.Point(15, 143);
			this.grpDataSources.Name = "grpDataSources";
			this.grpDataSources.Size = new System.Drawing.Size(455, 156);
			this.grpDataSources.TabIndex = 2;
			this.grpDataSources.TabStop = false;
			this.grpDataSources.Text = "Folders in which &data source(s) will be restored";
			// 
			// grid
			// 
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToDeleteRows = false;
			this.grid.AllowUserToResizeRows = false;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDataSource,
            this.colDataSourceFolder,
            this.colPrevRstFolder,
            this.colSaXmFile});
			this.grid.Location = new System.Drawing.Point(10, 24);
			this.grid.MultiSelect = false;
			this.grid.Name = "grid";
			this.grid.ReadOnly = true;
			this.grid.RowHeadersVisible = false;
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid.Size = new System.Drawing.Size(435, 88);
			this.grid.TabIndex = 0;
			this.grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_CellPainting);
			// 
			// colDataSource
			// 
			this.colDataSource.HeaderText = "Data Source File";
			this.colDataSource.Name = "colDataSource";
			this.colDataSource.ReadOnly = true;
			this.colDataSource.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colDataSource.Width = 150;
			// 
			// colDataSourceFolder
			// 
			this.colDataSourceFolder.HeaderText = "Restore Folder";
			this.colDataSourceFolder.Name = "colDataSourceFolder";
			this.colDataSourceFolder.ReadOnly = true;
			this.colDataSourceFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colDataSourceFolder.Width = 250;
			// 
			// colPrevRstFolder
			// 
			this.colPrevRstFolder.HeaderText = "colPrevRstFolder";
			this.colPrevRstFolder.Name = "colPrevRstFolder";
			this.colPrevRstFolder.ReadOnly = true;
			this.colPrevRstFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colPrevRstFolder.Visible = false;
			// 
			// colSaXmFile
			// 
			this.colSaXmFile.HeaderText = "colSaXmFile";
			this.colSaXmFile.Name = "colSaXmFile";
			this.colSaXmFile.ReadOnly = true;
			this.colSaXmFile.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colSaXmFile.Visible = false;
			// 
			// RestoreDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(482, 343);
			this.Controls.Add(this.grpDataSources);
			this.Controls.Add(this.grpPrjLocation);
			this.Controls.Add(this.lblProject);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnRestore);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(390, 350);
			this.Name = "RestoreDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Restore";
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