namespace SIL.Pa.UI.Dialogs
{
	partial class OpenProjectDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenProjectDlg));
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._grid = new SilTools.SilGrid();
			this._labelProjectFilesFound = new System.Windows.Forms.Label();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._buttonOpen = new System.Windows.Forms.Button();
			this._checkBoxOpenInNewWindow = new System.Windows.Forms.CheckBox();
			this._linkSelectAdditionalFolderToScan = new System.Windows.Forms.LinkLabel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._colProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._colFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._colProjectType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.ColumnCount = 3;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.Controls.Add(this._grid, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._labelProjectFilesFound, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._buttonCancel, 2, 3);
			this._tableLayoutPanel.Controls.Add(this._buttonOpen, 1, 3);
			this._tableLayoutPanel.Controls.Add(this._checkBoxOpenInNewWindow, 0, 3);
			this._tableLayoutPanel.Controls.Add(this._linkSelectAdditionalFolderToScan, 0, 2);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(12, 12);
			this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 4;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(398, 275);
			this._tableLayoutPanel.TabIndex = 0;
			// 
			// _grid
			// 
			this._grid.AllowUserToAddRows = false;
			this._grid.AllowUserToDeleteRows = false;
			this._grid.AllowUserToOrderColumns = true;
			this._grid.AllowUserToResizeRows = false;
			this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this._grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
			this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colProject,
            this._colFile,
            this._colProjectType});
			this._tableLayoutPanel.SetColumnSpan(this._grid, 3);
			this._grid.DrawTextBoxEditControlBorder = false;
			this._grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this._grid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this._grid, null);
			this.locExtender.SetLocalizationComment(this._grid, null);
			this.locExtender.SetLocalizingId(this._grid, "OpenProjectDlg._grid");
			this._grid.Location = new System.Drawing.Point(0, 19);
			this._grid.Margin = new System.Windows.Forms.Padding(0, 6, 0, 5);
			this._grid.MultiSelect = false;
			this._grid.Name = "_grid";
			this._grid.PaintHeaderAcrossFullGridWidth = true;
			this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._grid.RowHeadersVisible = false;
			this._grid.RowHeadersWidth = 22;
			this._grid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this._grid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this._grid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this._grid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._grid.ShowWaterMarkWhenDirty = false;
			this._grid.Size = new System.Drawing.Size(398, 147);
			this._grid.TabIndex = 1;
			this._grid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this._grid.VirtualMode = true;
			this._grid.WaterMark = "!";
			this._grid.CurrentRowChanged += new System.EventHandler(this.HandleGridCurrentRowChanged);
			this._grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleGridCellDoubleClicked);
			this._grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.HandleGridCellFormatting);
			this._grid.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.HandleGridCellToolTipTextNeeded);
			this._grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.HandleGridCellValueNeeded);
			this._grid.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleGridPainting);
			this._grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleGridKeyDown);
			this._grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleGridKeyPress);
			// 
			// _labelProjectFilesFound
			// 
			this._labelProjectFilesFound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelProjectFilesFound.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._labelProjectFilesFound, 3);
			this.locExtender.SetLocalizableToolTip(this._labelProjectFilesFound, null);
			this.locExtender.SetLocalizationComment(this._labelProjectFilesFound, null);
			this.locExtender.SetLocalizingId(this._labelProjectFilesFound, "OpenProjectDlg._labelProjectFilesFound");
			this._labelProjectFilesFound.Location = new System.Drawing.Point(2, 0);
			this._labelProjectFilesFound.Margin = new System.Windows.Forms.Padding(2, 0, 3, 0);
			this._labelProjectFilesFound.Name = "_labelProjectFilesFound";
			this._labelProjectFilesFound.Size = new System.Drawing.Size(393, 13);
			this._labelProjectFilesFound.TabIndex = 0;
			this._labelProjectFilesFound.Text = "Projects Found: {0}";
			// 
			// _buttonCancel
			// 
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "OpenProjectDlg._buttonCancel");
			this._buttonCancel.Location = new System.Drawing.Point(323, 249);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 5;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _buttonOpen
			// 
			this._buttonOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonOpen, null);
			this.locExtender.SetLocalizationComment(this._buttonOpen, null);
			this.locExtender.SetLocalizingId(this._buttonOpen, "OpenProjectDlg._buttonOpen");
			this._buttonOpen.Location = new System.Drawing.Point(242, 249);
			this._buttonOpen.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._buttonOpen.Name = "_buttonOpen";
			this._buttonOpen.Size = new System.Drawing.Size(75, 26);
			this._buttonOpen.TabIndex = 4;
			this._buttonOpen.Text = "Open";
			this._buttonOpen.UseVisualStyleBackColor = true;
			// 
			// _checkBoxOpenInNewWindow
			// 
			this._checkBoxOpenInNewWindow.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._checkBoxOpenInNewWindow.AutoSize = true;
			this._checkBoxOpenInNewWindow.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._checkBoxOpenInNewWindow, null);
			this.locExtender.SetLocalizationComment(this._checkBoxOpenInNewWindow, null);
			this.locExtender.SetLocalizingId(this._checkBoxOpenInNewWindow, "OpenProjectDlg._checkBoxOpenInNewWindow");
			this._checkBoxOpenInNewWindow.Location = new System.Drawing.Point(3, 253);
			this._checkBoxOpenInNewWindow.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this._checkBoxOpenInNewWindow.Name = "_checkBoxOpenInNewWindow";
			this._checkBoxOpenInNewWindow.Size = new System.Drawing.Size(130, 17);
			this._checkBoxOpenInNewWindow.TabIndex = 3;
			this._checkBoxOpenInNewWindow.Text = "&Open in New Window";
			this._checkBoxOpenInNewWindow.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBoxOpenInNewWindow.UseVisualStyleBackColor = false;
			// 
			// _linkSelectAdditionalFolderToScan
			// 
			this._linkSelectAdditionalFolderToScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._linkSelectAdditionalFolderToScan.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._linkSelectAdditionalFolderToScan, 3);
			this._linkSelectAdditionalFolderToScan.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
			this.locExtender.SetLocalizableToolTip(this._linkSelectAdditionalFolderToScan, null);
			this.locExtender.SetLocalizationComment(this._linkSelectAdditionalFolderToScan, null);
			this.locExtender.SetLocalizingId(this._linkSelectAdditionalFolderToScan, "OpenProjectDlg._linkSelectAdditionalFolderToScan.FullText");
			this._linkSelectAdditionalFolderToScan.Location = new System.Drawing.Point(0, 176);
			this._linkSelectAdditionalFolderToScan.Margin = new System.Windows.Forms.Padding(0, 5, 0, 8);
			this._linkSelectAdditionalFolderToScan.Name = "_linkSelectAdditionalFolderToScan";
			this._linkSelectAdditionalFolderToScan.Size = new System.Drawing.Size(398, 65);
			this._linkSelectAdditionalFolderToScan.TabIndex = 2;
			this._linkSelectAdditionalFolderToScan.Text = resources.GetString("_linkSelectAdditionalFolderToScan.Text");
			this._linkSelectAdditionalFolderToScan.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleLinkClick);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.HeaderText = "Project";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 150;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.FillWeight = 80F;
			this.dataGridViewTextBoxColumn2.HeaderText = "Backup File";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.dataGridViewTextBoxColumn3.HeaderText = "Type";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 56;
			// 
			// _colProject
			// 
			this._colProject.HeaderText = "Project";
			this._colProject.Name = "_colProject";
			this._colProject.ReadOnly = true;
			this._colProject.Width = 150;
			// 
			// _colFile
			// 
			this._colFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this._colFile.FillWeight = 80F;
			this._colFile.HeaderText = "Project File";
			this._colFile.Name = "_colFile";
			this._colFile.ReadOnly = true;
			// 
			// _colProjectType
			// 
			this._colProjectType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this._colProjectType.HeaderText = "Type";
			this._colProjectType.Name = "_colProjectType";
			this._colProjectType.ReadOnly = true;
			this._colProjectType.Width = 56;
			// 
			// OpenProjectDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(422, 299);
			this.Controls.Add(this._tableLayoutPanel);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "OpenProjectDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(365, 320);
			this.Name = "OpenProjectDlg";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Open Project";
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		protected Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Label _labelProjectFilesFound;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.LinkLabel _linkSelectAdditionalFolderToScan;
		private System.Windows.Forms.CheckBox _checkBoxOpenInNewWindow;
		private SilTools.SilGrid _grid;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colProject;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colFile;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colProjectType;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Button _buttonOpen;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
	}
}