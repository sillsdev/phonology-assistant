namespace SIL.Localize.Localizer
{
	partial class MainWnd
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_mainmenu = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNewProject = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenProject = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuProjectSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.m_toolstrip = new System.Windows.Forms.ToolStrip();
			this.tbbSave = new System.Windows.Forms.ToolStripButton();
			this.tbbGoogleTranslate = new System.Windows.Forms.ToolStripButton();
			this.tbbShowCommentsPane = new System.Windows.Forms.ToolStripButton();
			this.tbbShowSrcTextPane = new System.Windows.Forms.ToolStripButton();
			this.tbbShowTransPane = new System.Windows.Forms.ToolStripButton();
			this.tbbCompile = new System.Windows.Forms.ToolStripButton();
			this.m_grid = new SIL.SpeechTools.Utils.SilGrid();
			this.m_statusbar = new System.Windows.Forms.StatusStrip();
			this.sslProgressBar = new System.Windows.Forms.ToolStripStatusLabel();
			this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.tvResources = new System.Windows.Forms.TreeView();
			this.splitEntries = new System.Windows.Forms.SplitContainer();
			this.splitSrcTransCmt = new System.Windows.Forms.SplitContainer();
			this.splitSrcTrans = new System.Windows.Forms.SplitContainer();
			this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
			this.fldrBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
			this.Status = new System.Windows.Forms.DataGridViewImageColumn();
			this.ResourceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SourceText = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Translation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.txtSrcText = new SIL.Localize.Localizer.LabeledTextBox();
			this.txtTranslation = new SIL.Localize.Localizer.LabeledTextBox();
			this.txtComment = new SIL.Localize.Localizer.LabeledTextBox();
			this.cmnuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuUnreviewed = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuCompleted = new System.Windows.Forms.ToolStripMenuItem();
			this.m_mainmenu.SuspendLayout();
			this.m_toolstrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.m_statusbar.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitEntries.Panel1.SuspendLayout();
			this.splitEntries.Panel2.SuspendLayout();
			this.splitEntries.SuspendLayout();
			this.splitSrcTransCmt.Panel1.SuspendLayout();
			this.splitSrcTransCmt.Panel2.SuspendLayout();
			this.splitSrcTransCmt.SuspendLayout();
			this.splitSrcTrans.Panel1.SuspendLayout();
			this.splitSrcTrans.Panel2.SuspendLayout();
			this.splitSrcTrans.SuspendLayout();
			this.cmnuGrid.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_mainmenu
			// 
			this.m_mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
			this.m_mainmenu.Location = new System.Drawing.Point(0, 0);
			this.m_mainmenu.Name = "m_mainmenu";
			this.m_mainmenu.Size = new System.Drawing.Size(816, 24);
			this.m_mainmenu.TabIndex = 0;
			this.m_mainmenu.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewProject,
            this.mnuOpenProject,
            this.toolStripMenuItem2,
            this.mnuProjectSettings,
            this.mnuSep1,
            this.mnuSave,
            this.mnuSaveAs,
            this.toolStripMenuItem1,
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(39, 20);
			this.mnuFile.Text = "&File";
			// 
			// mnuNewProject
			// 
			this.mnuNewProject.Name = "mnuNewProject";
			this.mnuNewProject.Size = new System.Drawing.Size(203, 22);
			this.mnuNewProject.Text = "&New Project...";
			this.mnuNewProject.Click += new System.EventHandler(this.mnuNewProject_Click);
			// 
			// mnuOpenProject
			// 
			this.mnuOpenProject.Name = "mnuOpenProject";
			this.mnuOpenProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpenProject.Size = new System.Drawing.Size(203, 22);
			this.mnuOpenProject.Text = "&Open Project...";
			this.mnuOpenProject.Click += new System.EventHandler(this.mnuOpenProject_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(200, 6);
			// 
			// mnuProjectSettings
			// 
			this.mnuProjectSettings.Name = "mnuProjectSettings";
			this.mnuProjectSettings.Size = new System.Drawing.Size(203, 22);
			this.mnuProjectSettings.Text = "&Project Settings...";
			this.mnuProjectSettings.Click += new System.EventHandler(this.mnuProjectSettings_Click);
			// 
			// mnuSep1
			// 
			this.mnuSep1.Name = "mnuSep1";
			this.mnuSep1.Size = new System.Drawing.Size(200, 6);
			// 
			// mnuSave
			// 
			this.mnuSave.Image = ((System.Drawing.Image)(resources.GetObject("mnuSave.Image")));
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSave.Size = new System.Drawing.Size(203, 22);
			this.mnuSave.Text = "&Save";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.Size = new System.Drawing.Size(203, 22);
			this.mnuSaveAs.Text = "Save &As...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(200, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(203, 22);
			this.mnuExit.Text = "E&xit";
			// 
			// m_toolstrip
			// 
			this.m_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbSave,
            this.tbbGoogleTranslate,
            this.tbbShowCommentsPane,
            this.tbbShowSrcTextPane,
            this.tbbShowTransPane,
            this.tbbCompile});
			this.m_toolstrip.Location = new System.Drawing.Point(0, 24);
			this.m_toolstrip.Name = "m_toolstrip";
			this.m_toolstrip.Size = new System.Drawing.Size(816, 25);
			this.m_toolstrip.TabIndex = 1;
			this.m_toolstrip.Text = "toolStrip1";
			// 
			// tbbSave
			// 
			this.tbbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbbSave.Image = ((System.Drawing.Image)(resources.GetObject("tbbSave.Image")));
			this.tbbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbSave.Name = "tbbSave";
			this.tbbSave.Size = new System.Drawing.Size(23, 22);
			this.tbbSave.Text = "toolStripButton1";
			this.tbbSave.Click += new System.EventHandler(this.tbbSave_Click);
			// 
			// tbbGoogleTranslate
			// 
			this.tbbGoogleTranslate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbbGoogleTranslate.Image = ((System.Drawing.Image)(resources.GetObject("tbbGoogleTranslate.Image")));
			this.tbbGoogleTranslate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbGoogleTranslate.Name = "tbbGoogleTranslate";
			this.tbbGoogleTranslate.Size = new System.Drawing.Size(79, 22);
			this.tbbGoogleTranslate.Text = "Use Google";
			this.tbbGoogleTranslate.Click += new System.EventHandler(this.tbbGoogleTranslate_Click);
			// 
			// tbbShowCommentsPane
			// 
			this.tbbShowCommentsPane.Checked = true;
			this.tbbShowCommentsPane.CheckOnClick = true;
			this.tbbShowCommentsPane.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tbbShowCommentsPane.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbbShowCommentsPane.Image = ((System.Drawing.Image)(resources.GetObject("tbbShowCommentsPane.Image")));
			this.tbbShowCommentsPane.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbShowCommentsPane.Name = "tbbShowCommentsPane";
			this.tbbShowCommentsPane.Size = new System.Drawing.Size(23, 22);
			this.tbbShowCommentsPane.ToolTipText = "Show Comments Pane";
			this.tbbShowCommentsPane.Click += new System.EventHandler(this.tbbShowCommentsPane_Click);
			// 
			// tbbShowSrcTextPane
			// 
			this.tbbShowSrcTextPane.Checked = true;
			this.tbbShowSrcTextPane.CheckOnClick = true;
			this.tbbShowSrcTextPane.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tbbShowSrcTextPane.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbbShowSrcTextPane.Image = ((System.Drawing.Image)(resources.GetObject("tbbShowSrcTextPane.Image")));
			this.tbbShowSrcTextPane.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbShowSrcTextPane.Name = "tbbShowSrcTextPane";
			this.tbbShowSrcTextPane.Size = new System.Drawing.Size(23, 22);
			this.tbbShowSrcTextPane.ToolTipText = "Show Source Text Pane";
			this.tbbShowSrcTextPane.Click += new System.EventHandler(this.tbbShowSrcTextPane_Click);
			// 
			// tbbShowTransPane
			// 
			this.tbbShowTransPane.Checked = true;
			this.tbbShowTransPane.CheckOnClick = true;
			this.tbbShowTransPane.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tbbShowTransPane.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbbShowTransPane.Image = ((System.Drawing.Image)(resources.GetObject("tbbShowTransPane.Image")));
			this.tbbShowTransPane.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbShowTransPane.Name = "tbbShowTransPane";
			this.tbbShowTransPane.Size = new System.Drawing.Size(23, 22);
			this.tbbShowTransPane.ToolTipText = "Show Translation Pane";
			this.tbbShowTransPane.Click += new System.EventHandler(this.tbbShowTransPane_Click);
			// 
			// tbbCompile
			// 
			this.tbbCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbbCompile.Image = ((System.Drawing.Image)(resources.GetObject("tbbCompile.Image")));
			this.tbbCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbCompile.Name = "tbbCompile";
			this.tbbCompile.Size = new System.Drawing.Size(59, 22);
			this.tbbCompile.Text = "Compile";
			this.tbbCompile.Click += new System.EventHandler(this.tbbCompile_Click);
			// 
			// m_grid
			// 
			this.m_grid.AllowUserToAddRows = false;
			this.m_grid.AllowUserToDeleteRows = false;
			this.m_grid.AllowUserToOrderColumns = true;
			this.m_grid.AllowUserToResizeRows = false;
			this.m_grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.m_grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Status,
            this.ResourceId,
            this.SourceText,
            this.Translation,
            this.Comment});
			this.m_grid.ContextMenuStrip = this.cmnuGrid;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_grid.DefaultCellStyle = dataGridViewCellStyle5;
			this.m_grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_grid.IsDirty = false;
			this.m_grid.Location = new System.Drawing.Point(0, 0);
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersWidth = 22;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.Size = new System.Drawing.Size(522, 307);
			this.m_grid.TabIndex = 2;
			this.m_grid.VirtualMode = true;
			this.m_grid.WaterMark = "!";
			this.m_grid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_RowEnter);
			this.m_grid.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellValidated);
			this.m_grid.RowHeightInfoNeeded += new System.Windows.Forms.DataGridViewRowHeightInfoNeededEventHandler(this.m_grid_RowHeightInfoNeeded);
			this.m_grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.m_grid_CellValueNeeded);
			this.m_grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.m_grid_CellFormatting);
			this.m_grid.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.m_grid_CellValuePushed);
			this.m_grid.RowHeightInfoPushed += new System.Windows.Forms.DataGridViewRowHeightInfoPushedEventHandler(this.m_grid_RowHeightInfoPushed);
			// 
			// m_statusbar
			// 
			this.m_statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sslProgressBar,
            this.progressBar});
			this.m_statusbar.Location = new System.Drawing.Point(0, 486);
			this.m_statusbar.Name = "m_statusbar";
			this.m_statusbar.Size = new System.Drawing.Size(816, 22);
			this.m_statusbar.TabIndex = 3;
			// 
			// sslProgressBar
			// 
			this.sslProgressBar.AutoSize = false;
			this.sslProgressBar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.sslProgressBar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
			this.sslProgressBar.Name = "sslProgressBar";
			this.sslProgressBar.Size = new System.Drawing.Size(300, 17);
			this.sslProgressBar.Text = "#";
			this.sslProgressBar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.sslProgressBar.Visible = false;
			// 
			// progressBar
			// 
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(175, 16);
			this.progressBar.Visible = false;
			// 
			// splitOuter
			// 
			this.splitOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitOuter.Location = new System.Drawing.Point(12, 52);
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.tvResources);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.splitEntries);
			this.splitOuter.Size = new System.Drawing.Size(792, 431);
			this.splitOuter.SplitterDistance = 264;
			this.splitOuter.SplitterWidth = 6;
			this.splitOuter.TabIndex = 4;
			// 
			// tvResources
			// 
			this.tvResources.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvResources.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tvResources.HideSelection = false;
			this.tvResources.Location = new System.Drawing.Point(0, 0);
			this.tvResources.Name = "tvResources";
			this.tvResources.Size = new System.Drawing.Size(264, 431);
			this.tvResources.TabIndex = 0;
			this.tvResources.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvResXList_AfterSelect);
			// 
			// splitEntries
			// 
			this.splitEntries.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitEntries.Location = new System.Drawing.Point(0, 0);
			this.splitEntries.Name = "splitEntries";
			this.splitEntries.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitEntries.Panel1
			// 
			this.splitEntries.Panel1.Controls.Add(this.m_grid);
			// 
			// splitEntries.Panel2
			// 
			this.splitEntries.Panel2.Controls.Add(this.splitSrcTransCmt);
			this.splitEntries.Size = new System.Drawing.Size(522, 431);
			this.splitEntries.SplitterDistance = 307;
			this.splitEntries.SplitterWidth = 6;
			this.splitEntries.TabIndex = 3;
			// 
			// splitSrcTransCmt
			// 
			this.splitSrcTransCmt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitSrcTransCmt.Location = new System.Drawing.Point(0, 0);
			this.splitSrcTransCmt.Name = "splitSrcTransCmt";
			this.splitSrcTransCmt.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitSrcTransCmt.Panel1
			// 
			this.splitSrcTransCmt.Panel1.Controls.Add(this.splitSrcTrans);
			// 
			// splitSrcTransCmt.Panel2
			// 
			this.splitSrcTransCmt.Panel2.Controls.Add(this.txtComment);
			this.splitSrcTransCmt.Size = new System.Drawing.Size(522, 118);
			this.splitSrcTransCmt.SplitterDistance = 70;
			this.splitSrcTransCmt.TabIndex = 1;
			// 
			// splitSrcTrans
			// 
			this.splitSrcTrans.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitSrcTrans.Location = new System.Drawing.Point(0, 0);
			this.splitSrcTrans.Name = "splitSrcTrans";
			// 
			// splitSrcTrans.Panel1
			// 
			this.splitSrcTrans.Panel1.Controls.Add(this.txtSrcText);
			// 
			// splitSrcTrans.Panel2
			// 
			this.splitSrcTrans.Panel2.Controls.Add(this.txtTranslation);
			this.splitSrcTrans.Size = new System.Drawing.Size(522, 70);
			this.splitSrcTrans.SplitterDistance = 246;
			this.splitSrcTrans.TabIndex = 0;
			// 
			// openFileDlg
			// 
			this.openFileDlg.DefaultExt = "lop";
			this.openFileDlg.Filter = "Localizer Project Files (*.lop)|*.lop|All Files (*.*)|*.*";
			// 
			// fldrBrowser
			// 
			this.fldrBrowser.Description = "Specify where to save your project.";
			// 
			// saveFileDlg
			// 
			this.saveFileDlg.DefaultExt = "lop";
			this.saveFileDlg.Filter = "Localizer Project (*.lop)|*.lop|All Files (*.*)|*.*";
			this.saveFileDlg.Title = "Save Project";
			// 
			// Status
			// 
			this.Status.HeaderText = "Status";
			this.Status.Name = "Status";
			this.Status.ReadOnly = true;
			this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.Status.Width = 75;
			// 
			// ResourceId
			// 
			this.ResourceId.HeaderText = "Resource Id";
			this.ResourceId.Name = "ResourceId";
			this.ResourceId.ReadOnly = true;
			this.ResourceId.Width = 125;
			// 
			// SourceText
			// 
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.SourceText.DefaultCellStyle = dataGridViewCellStyle2;
			this.SourceText.HeaderText = "Source Text";
			this.SourceText.Name = "SourceText";
			this.SourceText.ReadOnly = true;
			this.SourceText.Width = 150;
			// 
			// Translation
			// 
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.Translation.DefaultCellStyle = dataGridViewCellStyle3;
			this.Translation.HeaderText = "Translation";
			this.Translation.Name = "Translation";
			this.Translation.Width = 150;
			// 
			// Comment
			// 
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.Comment.DefaultCellStyle = dataGridViewCellStyle4;
			this.Comment.HeaderText = "Comment";
			this.Comment.Name = "Comment";
			this.Comment.Width = 150;
			// 
			// txtSrcText
			// 
			this.txtSrcText.BackColor = System.Drawing.Color.Red;
			this.txtSrcText.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// 
			// 
			this.txtSrcText.HeadingLabel.BackColor = System.Drawing.SystemColors.Window;
			this.txtSrcText.HeadingLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSrcText.HeadingLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSrcText.HeadingLabel.Location = new System.Drawing.Point(1, 1);
			this.txtSrcText.HeadingLabel.Name = "lblHeading";
			this.txtSrcText.HeadingLabel.Size = new System.Drawing.Size(244, 18);
			this.txtSrcText.HeadingLabel.TabIndex = 0;
			this.txtSrcText.HeadingLabel.Text = "Source Text";
			this.txtSrcText.Location = new System.Drawing.Point(0, 0);
			this.txtSrcText.Name = "txtSrcText";
			this.txtSrcText.Padding = new System.Windows.Forms.Padding(1);
			this.txtSrcText.Size = new System.Drawing.Size(246, 70);
			this.txtSrcText.TabIndex = 0;
			// 
			// 
			// 
			this.txtSrcText.TextBox.BackColor = System.Drawing.SystemColors.Window;
			this.txtSrcText.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSrcText.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSrcText.TextBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSrcText.TextBox.Location = new System.Drawing.Point(1, 19);
			this.txtSrcText.TextBox.Multiline = true;
			this.txtSrcText.TextBox.Name = "txtText";
			this.txtSrcText.TextBox.ReadOnly = true;
			this.txtSrcText.TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSrcText.TextBox.Size = new System.Drawing.Size(244, 50);
			this.txtSrcText.TextBox.TabIndex = 1;
			// 
			// txtTranslation
			// 
			this.txtTranslation.BackColor = System.Drawing.Color.Red;
			this.txtTranslation.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// 
			// 
			this.txtTranslation.HeadingLabel.BackColor = System.Drawing.SystemColors.Window;
			this.txtTranslation.HeadingLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtTranslation.HeadingLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTranslation.HeadingLabel.Location = new System.Drawing.Point(1, 1);
			this.txtTranslation.HeadingLabel.Name = "lblHeading";
			this.txtTranslation.HeadingLabel.Size = new System.Drawing.Size(270, 18);
			this.txtTranslation.HeadingLabel.TabIndex = 0;
			this.txtTranslation.HeadingLabel.Text = "&Translation";
			this.txtTranslation.Location = new System.Drawing.Point(0, 0);
			this.txtTranslation.Name = "txtTranslation";
			this.txtTranslation.Padding = new System.Windows.Forms.Padding(1);
			this.txtTranslation.Size = new System.Drawing.Size(272, 70);
			this.txtTranslation.TabIndex = 1;
			// 
			// 
			// 
			this.txtTranslation.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtTranslation.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTranslation.TextBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTranslation.TextBox.Location = new System.Drawing.Point(1, 19);
			this.txtTranslation.TextBox.Multiline = true;
			this.txtTranslation.TextBox.Name = "txtText";
			this.txtTranslation.TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTranslation.TextBox.Size = new System.Drawing.Size(270, 50);
			this.txtTranslation.TextBox.TabIndex = 1;
			this.txtTranslation.TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleTextBoxKeyDown);
			this.txtTranslation.TextBox.Validated += new System.EventHandler(this.txtTranslation_Validated);
			// 
			// txtComment
			// 
			this.txtComment.BackColor = System.Drawing.Color.Red;
			this.txtComment.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// 
			// 
			this.txtComment.HeadingLabel.BackColor = System.Drawing.SystemColors.Window;
			this.txtComment.HeadingLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtComment.HeadingLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtComment.HeadingLabel.Location = new System.Drawing.Point(1, 1);
			this.txtComment.HeadingLabel.Name = "lblHeading";
			this.txtComment.HeadingLabel.Size = new System.Drawing.Size(520, 18);
			this.txtComment.HeadingLabel.TabIndex = 0;
			this.txtComment.HeadingLabel.Text = "&Comment";
			this.txtComment.Location = new System.Drawing.Point(0, 0);
			this.txtComment.Name = "txtComment";
			this.txtComment.Padding = new System.Windows.Forms.Padding(1);
			this.txtComment.Size = new System.Drawing.Size(522, 44);
			this.txtComment.TabIndex = 2;
			// 
			// 
			// 
			this.txtComment.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtComment.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtComment.TextBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtComment.TextBox.Location = new System.Drawing.Point(1, 19);
			this.txtComment.TextBox.Multiline = true;
			this.txtComment.TextBox.Name = "txtText";
			this.txtComment.TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtComment.TextBox.Size = new System.Drawing.Size(520, 24);
			this.txtComment.TextBox.TabIndex = 1;
			this.txtComment.TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleTextBoxKeyDown);
			this.txtComment.TextBox.Validated += new System.EventHandler(this.txtComment_Validated);
			// 
			// cmnuGrid
			// 
			this.cmnuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuUnreviewed,
            this.cmnuCompleted});
			this.cmnuGrid.Name = "cmnuGrid";
			this.cmnuGrid.Size = new System.Drawing.Size(194, 48);
			// 
			// cmnuUnreviewed
			// 
			this.cmnuUnreviewed.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidUnreviewed;
			this.cmnuUnreviewed.Name = "cmnuUnreviewed";
			this.cmnuUnreviewed.Size = new System.Drawing.Size(193, 22);
			this.cmnuUnreviewed.Text = "Mark as &Unreviewed";
			this.cmnuUnreviewed.Click += new System.EventHandler(this.cmnuUnreviewed_Click);
			// 
			// cmnuCompleted
			// 
			this.cmnuCompleted.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidCompleted;
			this.cmnuCompleted.Name = "cmnuCompleted";
			this.cmnuCompleted.Size = new System.Drawing.Size(193, 22);
			this.cmnuCompleted.Text = "Mark as &Completed";
			this.cmnuCompleted.Click += new System.EventHandler(this.cmnuCompleted_Click);
			// 
			// MainWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(816, 508);
			this.Controls.Add(this.m_statusbar);
			this.Controls.Add(this.m_toolstrip);
			this.Controls.Add(this.m_mainmenu);
			this.Controls.Add(this.splitOuter);
			this.MainMenuStrip = this.m_mainmenu;
			this.Name = "MainWnd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Localizer";
			this.m_mainmenu.ResumeLayout(false);
			this.m_mainmenu.PerformLayout();
			this.m_toolstrip.ResumeLayout(false);
			this.m_toolstrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.m_statusbar.ResumeLayout(false);
			this.m_statusbar.PerformLayout();
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitEntries.Panel1.ResumeLayout(false);
			this.splitEntries.Panel2.ResumeLayout(false);
			this.splitEntries.ResumeLayout(false);
			this.splitSrcTransCmt.Panel1.ResumeLayout(false);
			this.splitSrcTransCmt.Panel2.ResumeLayout(false);
			this.splitSrcTransCmt.ResumeLayout(false);
			this.splitSrcTrans.Panel1.ResumeLayout(false);
			this.splitSrcTrans.Panel2.ResumeLayout(false);
			this.splitSrcTrans.ResumeLayout(false);
			this.cmnuGrid.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip m_mainmenu;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStrip m_toolstrip;
		private SIL.SpeechTools.Utils.SilGrid m_grid;
		private System.Windows.Forms.StatusStrip m_statusbar;
		private System.Windows.Forms.ToolStripMenuItem mnuNewProject;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.TreeView tvResources;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenProject;
		private System.Windows.Forms.ToolStripSeparator mnuSep1;
		private System.Windows.Forms.OpenFileDialog openFileDlg;
		private System.Windows.Forms.ToolStripButton tbbSave;
		private System.Windows.Forms.FolderBrowserDialog fldrBrowser;
		private System.Windows.Forms.ToolStripMenuItem mnuProjectSettings;
		private System.Windows.Forms.ToolStripMenuItem mnuSave;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripButton tbbGoogleTranslate;
		private System.Windows.Forms.SplitContainer splitEntries;
		private System.Windows.Forms.ToolStripButton tbbShowCommentsPane;
		private System.Windows.Forms.SplitContainer splitSrcTransCmt;
		private System.Windows.Forms.SplitContainer splitSrcTrans;
		private LabeledTextBox txtSrcText;
		private LabeledTextBox txtTranslation;
		private LabeledTextBox txtComment;
		private System.Windows.Forms.ToolStripButton tbbShowSrcTextPane;
		private System.Windows.Forms.ToolStripButton tbbShowTransPane;
		private System.Windows.Forms.ToolStripButton tbbCompile;
		private System.Windows.Forms.ToolStripStatusLabel sslProgressBar;
		private System.Windows.Forms.ToolStripProgressBar progressBar;
		private System.Windows.Forms.SaveFileDialog saveFileDlg;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.DataGridViewImageColumn Status;
		private System.Windows.Forms.DataGridViewTextBoxColumn ResourceId;
		private System.Windows.Forms.DataGridViewTextBoxColumn SourceText;
		private System.Windows.Forms.DataGridViewTextBoxColumn Translation;
		private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
		private System.Windows.Forms.ContextMenuStrip cmnuGrid;
		private System.Windows.Forms.ToolStripMenuItem cmnuUnreviewed;
		private System.Windows.Forms.ToolStripMenuItem cmnuCompleted;
	}
}

