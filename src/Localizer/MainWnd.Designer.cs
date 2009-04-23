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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
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
			this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowSrcTextPane = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowTransPane = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowCommentPane = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowOmittedItems = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoogleTranslate = new System.Windows.Forms.ToolStripMenuItem();
			this.m_toolstrip = new System.Windows.Forms.ToolStrip();
			this.tbbSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
			this.tbbGoogleTranslate = new System.Windows.Forms.ToolStripDropDownButton();
			this.mnuGoggleTransSelected = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoogleTransAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tbbCompile = new System.Windows.Forms.ToolStripButton();
			this.tbbRescan = new System.Windows.Forms.ToolStripButton();
			this.m_grid = new SIL.SpeechTools.Utils.SilGrid();
			this.colStatus = new System.Windows.Forms.DataGridViewImageColumn();
			this.colAssembly = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colResource = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colResourceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSourceText = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTranslation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cmnuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuUnreviewed = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuCompleted = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.cmnuOmitResourceItem = new System.Windows.Forms.ToolStripMenuItem();
			this.m_statusbar = new System.Windows.Forms.StatusStrip();
			this.sslMain = new System.Windows.Forms.ToolStripStatusLabel();
			this.sslProgressBar = new System.Windows.Forms.ToolStripStatusLabel();
			this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.tvResources = new System.Windows.Forms.TreeView();
			this.cmnuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuOmitAssembly = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuOmitResource = new System.Windows.Forms.ToolStripMenuItem();
			this.splitEntries = new System.Windows.Forms.SplitContainer();
			this.splitSrcTransCmt = new System.Windows.Forms.SplitContainer();
			this.splitSrcTrans = new System.Windows.Forms.SplitContainer();
			this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
			this.fldrBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
			this.tsContainer = new System.Windows.Forms.ToolStripContainer();
			this.txtSrcText = new SIL.Localize.Localizer.LabeledTextBox();
			this.txtTranslation = new SIL.Localize.Localizer.LabeledTextBox();
			this.txtComment = new SIL.Localize.Localizer.LabeledTextBox();
			this.m_mainmenu.SuspendLayout();
			this.m_toolstrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.cmnuGrid.SuspendLayout();
			this.m_statusbar.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.cmnuTree.SuspendLayout();
			this.splitEntries.Panel1.SuspendLayout();
			this.splitEntries.Panel2.SuspendLayout();
			this.splitEntries.SuspendLayout();
			this.splitSrcTransCmt.Panel1.SuspendLayout();
			this.splitSrcTransCmt.Panel2.SuspendLayout();
			this.splitSrcTransCmt.SuspendLayout();
			this.splitSrcTrans.Panel1.SuspendLayout();
			this.splitSrcTrans.Panel2.SuspendLayout();
			this.splitSrcTrans.SuspendLayout();
			this.tsContainer.ContentPanel.SuspendLayout();
			this.tsContainer.TopToolStripPanel.SuspendLayout();
			this.tsContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_mainmenu
			// 
			this.m_mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuView,
            this.mnuTools});
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
			this.mnuNewProject.Size = new System.Drawing.Size(212, 22);
			this.mnuNewProject.Text = "&New Project...";
			this.mnuNewProject.Click += new System.EventHandler(this.mnuNewProject_Click);
			// 
			// mnuOpenProject
			// 
			this.mnuOpenProject.Name = "mnuOpenProject";
			this.mnuOpenProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpenProject.Size = new System.Drawing.Size(212, 22);
			this.mnuOpenProject.Text = "&Open Project...";
			this.mnuOpenProject.Click += new System.EventHandler(this.mnuOpenProject_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuProjectSettings
			// 
			this.mnuProjectSettings.Name = "mnuProjectSettings";
			this.mnuProjectSettings.Size = new System.Drawing.Size(212, 22);
			this.mnuProjectSettings.Text = "&Project Settings...";
			this.mnuProjectSettings.Click += new System.EventHandler(this.mnuProjectSettings_Click);
			// 
			// mnuSep1
			// 
			this.mnuSep1.Name = "mnuSep1";
			this.mnuSep1.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuSave
			// 
			this.mnuSave.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidSave;
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSave.Size = new System.Drawing.Size(212, 22);
			this.mnuSave.Text = "&Save";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.Size = new System.Drawing.Size(212, 22);
			this.mnuSaveAs.Text = "Save &As...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(212, 22);
			this.mnuExit.Text = "E&xit";
			// 
			// mnuView
			// 
			this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowSrcTextPane,
            this.mnuShowTransPane,
            this.mnuShowCommentPane,
            this.toolStripMenuItem4,
            this.mnuShowOmittedItems});
			this.mnuView.Name = "mnuView";
			this.mnuView.Size = new System.Drawing.Size(49, 20);
			this.mnuView.Text = "&View";
			this.mnuView.DropDownOpening += new System.EventHandler(this.mnuView_DropDownOpening);
			// 
			// mnuShowSrcTextPane
			// 
			this.mnuShowSrcTextPane.Name = "mnuShowSrcTextPane";
			this.mnuShowSrcTextPane.Size = new System.Drawing.Size(184, 22);
			this.mnuShowSrcTextPane.Text = "&Source Text Pane";
			this.mnuShowSrcTextPane.Click += new System.EventHandler(this.mnuShowSrcTextPane_Click);
			// 
			// mnuShowTransPane
			// 
			this.mnuShowTransPane.Name = "mnuShowTransPane";
			this.mnuShowTransPane.Size = new System.Drawing.Size(184, 22);
			this.mnuShowTransPane.Text = "&Translation Pane";
			this.mnuShowTransPane.Click += new System.EventHandler(this.mnuShowTransPane_Click);
			// 
			// mnuShowCommentPane
			// 
			this.mnuShowCommentPane.Name = "mnuShowCommentPane";
			this.mnuShowCommentPane.Size = new System.Drawing.Size(184, 22);
			this.mnuShowCommentPane.Text = "&Comment Pane";
			this.mnuShowCommentPane.Click += new System.EventHandler(this.mnuShowCommentPane_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(181, 6);
			// 
			// mnuShowOmittedItems
			// 
			this.mnuShowOmittedItems.Name = "mnuShowOmittedItems";
			this.mnuShowOmittedItems.Size = new System.Drawing.Size(184, 22);
			this.mnuShowOmittedItems.Text = "&Omitted Items";
			this.mnuShowOmittedItems.Click += new System.EventHandler(this.mnuShowOmittedItems_Click);
			// 
			// mnuTools
			// 
			this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGoogleTranslate});
			this.mnuTools.Name = "mnuTools";
			this.mnuTools.Size = new System.Drawing.Size(51, 20);
			this.mnuTools.Text = "&Tools";
			// 
			// mnuGoogleTranslate
			// 
			this.mnuGoogleTranslate.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidGoogle;
			this.mnuGoogleTranslate.Name = "mnuGoogleTranslate";
			this.mnuGoogleTranslate.Size = new System.Drawing.Size(219, 22);
			this.mnuGoogleTranslate.Text = "Translate Using Google";
			// 
			// m_toolstrip
			// 
			this.m_toolstrip.Dock = System.Windows.Forms.DockStyle.None;
			this.m_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbSave,
            this.toolStripButton1,
            this.tbbGoogleTranslate,
            this.toolStripSeparator1,
            this.tbbCompile,
            this.tbbRescan});
			this.m_toolstrip.Location = new System.Drawing.Point(0, 0);
			this.m_toolstrip.Name = "m_toolstrip";
			this.m_toolstrip.Size = new System.Drawing.Size(816, 25);
			this.m_toolstrip.Stretch = true;
			this.m_toolstrip.TabIndex = 1;
			this.m_toolstrip.Text = "toolStrip1";
			// 
			// tbbSave
			// 
			this.tbbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbbSave.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidSave;
			this.tbbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbSave.Name = "tbbSave";
			this.tbbSave.Size = new System.Drawing.Size(23, 22);
			this.tbbSave.Text = "toolStripButton1";
			this.tbbSave.Click += new System.EventHandler(this.tbbSave_Click);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(6, 25);
			// 
			// tbbGoogleTranslate
			// 
			this.tbbGoogleTranslate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbbGoogleTranslate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGoggleTransSelected,
            this.mnuGoogleTransAll});
			this.tbbGoogleTranslate.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidGoogle;
			this.tbbGoogleTranslate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbGoogleTranslate.Name = "tbbGoogleTranslate";
			this.tbbGoogleTranslate.Size = new System.Drawing.Size(29, 22);
			this.tbbGoogleTranslate.Text = "Use Google";
			this.tbbGoogleTranslate.ToolTipText = "Translate Using Google";
			this.tbbGoogleTranslate.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbbGoogleTranslate_DropDownItemClicked);
			// 
			// mnuGoggleTransSelected
			// 
			this.mnuGoggleTransSelected.Name = "mnuGoggleTransSelected";
			this.mnuGoggleTransSelected.Size = new System.Drawing.Size(177, 22);
			this.mnuGoggleTransSelected.Text = "&Selected Row(s)";
			// 
			// mnuGoogleTransAll
			// 
			this.mnuGoogleTransAll.Name = "mnuGoogleTransAll";
			this.mnuGoogleTransAll.Size = new System.Drawing.Size(177, 22);
			this.mnuGoogleTransAll.Text = "&All Rows";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tbbCompile
			// 
			this.tbbCompile.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidCompile;
			this.tbbCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbCompile.Name = "tbbCompile";
			this.tbbCompile.Size = new System.Drawing.Size(75, 22);
			this.tbbCompile.Text = "Compile";
			this.tbbCompile.ToolTipText = "Compile Translations";
			this.tbbCompile.Click += new System.EventHandler(this.tbbCompile_Click);
			// 
			// tbbRescan
			// 
			this.tbbRescan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbbRescan.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidRescan;
			this.tbbRescan.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbRescan.Name = "tbbRescan";
			this.tbbRescan.Size = new System.Drawing.Size(23, 22);
			this.tbbRescan.Text = "Rescan";
			this.tbbRescan.ToolTipText = "Rescan Source Resources";
			this.tbbRescan.Click += new System.EventHandler(this.tbbRescan_Click);
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
            this.colStatus,
            this.colAssembly,
            this.colResource,
            this.colResourceId,
            this.colSourceText,
            this.colTranslation,
            this.colComment});
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
			this.m_grid.Size = new System.Drawing.Size(539, 306);
			this.m_grid.TabIndex = 2;
			this.m_grid.VirtualMode = true;
			this.m_grid.WaterMark = "!";
			this.m_grid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_RowEnter);
			this.m_grid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseDown);
			this.m_grid.RowHeightInfoNeeded += new System.Windows.Forms.DataGridViewRowHeightInfoNeededEventHandler(this.m_grid_RowHeightInfoNeeded);
			this.m_grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.m_grid_CellValueNeeded);
			this.m_grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.m_grid_CellFormatting);
			this.m_grid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.m_grid_CellValidating);
			this.m_grid.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.m_grid_CellValuePushed);
			this.m_grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellEnter);
			this.m_grid.RowHeightInfoPushed += new System.Windows.Forms.DataGridViewRowHeightInfoPushedEventHandler(this.m_grid_RowHeightInfoPushed);
			// 
			// colStatus
			// 
			this.colStatus.HeaderText = "Status";
			this.colStatus.Name = "colStatus";
			this.colStatus.ReadOnly = true;
			this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colStatus.Width = 75;
			// 
			// colAssembly
			// 
			this.colAssembly.HeaderText = "Assembly";
			this.colAssembly.Name = "colAssembly";
			this.colAssembly.ReadOnly = true;
			// 
			// colResource
			// 
			this.colResource.HeaderText = "Resource";
			this.colResource.Name = "colResource";
			this.colResource.ReadOnly = true;
			// 
			// colResourceId
			// 
			this.colResourceId.HeaderText = "Resource Id";
			this.colResourceId.Name = "colResourceId";
			this.colResourceId.ReadOnly = true;
			this.colResourceId.Width = 125;
			// 
			// colSourceText
			// 
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.colSourceText.DefaultCellStyle = dataGridViewCellStyle2;
			this.colSourceText.HeaderText = "Source Text";
			this.colSourceText.Name = "colSourceText";
			this.colSourceText.ReadOnly = true;
			this.colSourceText.Width = 150;
			// 
			// colTranslation
			// 
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.colTranslation.DefaultCellStyle = dataGridViewCellStyle3;
			this.colTranslation.HeaderText = "Translation";
			this.colTranslation.Name = "colTranslation";
			this.colTranslation.Width = 150;
			// 
			// colComment
			// 
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.colComment.DefaultCellStyle = dataGridViewCellStyle4;
			this.colComment.HeaderText = "Comment";
			this.colComment.Name = "colComment";
			this.colComment.Width = 150;
			// 
			// cmnuGrid
			// 
			this.cmnuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuUnreviewed,
            this.cmnuCompleted,
            this.toolStripMenuItem3,
            this.cmnuOmitResourceItem});
			this.cmnuGrid.Name = "cmnuGrid";
			this.cmnuGrid.Size = new System.Drawing.Size(203, 76);
			// 
			// cmnuUnreviewed
			// 
			this.cmnuUnreviewed.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidUnreviewed;
			this.cmnuUnreviewed.Name = "cmnuUnreviewed";
			this.cmnuUnreviewed.Size = new System.Drawing.Size(202, 22);
			this.cmnuUnreviewed.Text = "Mark as &Unreviewed";
			this.cmnuUnreviewed.Click += new System.EventHandler(this.cmnuUnreviewed_Click);
			// 
			// cmnuCompleted
			// 
			this.cmnuCompleted.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidCompleted;
			this.cmnuCompleted.Name = "cmnuCompleted";
			this.cmnuCompleted.Size = new System.Drawing.Size(202, 22);
			this.cmnuCompleted.Text = "Mark as &Completed";
			this.cmnuCompleted.Click += new System.EventHandler(this.cmnuCompleted_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(199, 6);
			// 
			// cmnuOmitResourceItem
			// 
			this.cmnuOmitResourceItem.Image = global::SIL.Localize.Localizer.Properties.Resources.kimidOmitted;
			this.cmnuOmitResourceItem.Name = "cmnuOmitResourceItem";
			this.cmnuOmitResourceItem.Size = new System.Drawing.Size(202, 22);
			this.cmnuOmitResourceItem.Text = "&Omit";
			this.cmnuOmitResourceItem.Click += new System.EventHandler(this.cmnuOmitResourceItem_Click);
			// 
			// m_statusbar
			// 
			this.m_statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sslMain,
            this.sslProgressBar,
            this.progressBar});
			this.m_statusbar.Location = new System.Drawing.Point(0, 486);
			this.m_statusbar.Name = "m_statusbar";
			this.m_statusbar.Size = new System.Drawing.Size(816, 22);
			this.m_statusbar.TabIndex = 3;
			// 
			// sslMain
			// 
			this.sslMain.Name = "sslMain";
			this.sslMain.Size = new System.Drawing.Size(801, 17);
			this.sslMain.Spring = true;
			this.sslMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// sslProgressBar
			// 
			this.sslProgressBar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.sslProgressBar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
			this.sslProgressBar.Name = "sslProgressBar";
			this.sslProgressBar.Size = new System.Drawing.Size(16, 17);
			this.sslProgressBar.Text = "#";
			this.sslProgressBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.sslProgressBar.Visible = false;
			// 
			// progressBar
			// 
			this.progressBar.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(175, 16);
			this.progressBar.Visible = false;
			// 
			// splitOuter
			// 
			this.splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitOuter.Location = new System.Drawing.Point(0, 0);
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.tvResources);
			this.splitOuter.Panel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.splitEntries);
			this.splitOuter.Panel2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.splitOuter.Size = new System.Drawing.Size(816, 437);
			this.splitOuter.SplitterDistance = 271;
			this.splitOuter.SplitterWidth = 6;
			this.splitOuter.TabIndex = 4;
			this.splitOuter.TabStop = false;
			// 
			// tvResources
			// 
			this.tvResources.ContextMenuStrip = this.cmnuTree;
			this.tvResources.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvResources.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tvResources.HideSelection = false;
			this.tvResources.Location = new System.Drawing.Point(0, 5);
			this.tvResources.Name = "tvResources";
			this.tvResources.Size = new System.Drawing.Size(271, 432);
			this.tvResources.TabIndex = 0;
			this.tvResources.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvResources_AfterSelect);
			this.tvResources.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvResources_MouseDown);
			// 
			// cmnuTree
			// 
			this.cmnuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuOmitAssembly,
            this.cmnuOmitResource});
			this.cmnuTree.Name = "cmnuTree";
			this.cmnuTree.Size = new System.Drawing.Size(173, 48);
			this.cmnuTree.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmnuTree_ItemClicked);
			this.cmnuTree.Opening += new System.ComponentModel.CancelEventHandler(this.cmnuTree_Opening);
			// 
			// cmnuOmitAssembly
			// 
			this.cmnuOmitAssembly.Name = "cmnuOmitAssembly";
			this.cmnuOmitAssembly.Size = new System.Drawing.Size(172, 22);
			this.cmnuOmitAssembly.Text = "&Omit Assembly";
			// 
			// cmnuOmitResource
			// 
			this.cmnuOmitResource.Name = "cmnuOmitResource";
			this.cmnuOmitResource.Size = new System.Drawing.Size(172, 22);
			this.cmnuOmitResource.Text = "&Omit Resource";
			// 
			// splitEntries
			// 
			this.splitEntries.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitEntries.Location = new System.Drawing.Point(0, 5);
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
			this.splitEntries.Size = new System.Drawing.Size(539, 432);
			this.splitEntries.SplitterDistance = 306;
			this.splitEntries.SplitterWidth = 6;
			this.splitEntries.TabIndex = 3;
			this.splitEntries.TabStop = false;
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
			this.splitSrcTransCmt.Size = new System.Drawing.Size(539, 120);
			this.splitSrcTransCmt.SplitterDistance = 69;
			this.splitSrcTransCmt.TabIndex = 1;
			this.splitSrcTransCmt.TabStop = false;
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
			this.splitSrcTrans.Size = new System.Drawing.Size(539, 69);
			this.splitSrcTrans.SplitterDistance = 252;
			this.splitSrcTrans.TabIndex = 0;
			this.splitSrcTrans.TabStop = false;
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
			// tsContainer
			// 
			// 
			// tsContainer.ContentPanel
			// 
			this.tsContainer.ContentPanel.Controls.Add(this.splitOuter);
			this.tsContainer.ContentPanel.Size = new System.Drawing.Size(816, 437);
			this.tsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tsContainer.Location = new System.Drawing.Point(0, 24);
			this.tsContainer.Name = "tsContainer";
			this.tsContainer.Size = new System.Drawing.Size(816, 462);
			this.tsContainer.TabIndex = 5;
			// 
			// tsContainer.TopToolStripPanel
			// 
			this.tsContainer.TopToolStripPanel.Controls.Add(this.m_toolstrip);
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
			this.txtSrcText.HeadingLabel.Size = new System.Drawing.Size(250, 18);
			this.txtSrcText.HeadingLabel.TabIndex = 0;
			this.txtSrcText.HeadingLabel.Text = "Source Text";
			this.txtSrcText.Location = new System.Drawing.Point(0, 0);
			this.txtSrcText.Name = "txtSrcText";
			this.txtSrcText.Padding = new System.Windows.Forms.Padding(1);
			this.txtSrcText.Size = new System.Drawing.Size(252, 69);
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
			this.txtSrcText.TextBox.Size = new System.Drawing.Size(250, 49);
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
			this.txtTranslation.HeadingLabel.Size = new System.Drawing.Size(281, 18);
			this.txtTranslation.HeadingLabel.TabIndex = 0;
			this.txtTranslation.HeadingLabel.Text = "&Translation";
			this.txtTranslation.Location = new System.Drawing.Point(0, 0);
			this.txtTranslation.Name = "txtTranslation";
			this.txtTranslation.Padding = new System.Windows.Forms.Padding(1);
			this.txtTranslation.Size = new System.Drawing.Size(283, 69);
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
			this.txtTranslation.TextBox.Size = new System.Drawing.Size(281, 49);
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
			this.txtComment.HeadingLabel.Size = new System.Drawing.Size(537, 18);
			this.txtComment.HeadingLabel.TabIndex = 0;
			this.txtComment.HeadingLabel.Text = "&Comment";
			this.txtComment.Location = new System.Drawing.Point(0, 0);
			this.txtComment.Name = "txtComment";
			this.txtComment.Padding = new System.Windows.Forms.Padding(1);
			this.txtComment.Size = new System.Drawing.Size(539, 47);
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
			this.txtComment.TextBox.Size = new System.Drawing.Size(537, 27);
			this.txtComment.TextBox.TabIndex = 1;
			this.txtComment.TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleTextBoxKeyDown);
			this.txtComment.TextBox.Validated += new System.EventHandler(this.txtComment_Validated);
			// 
			// MainWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(816, 508);
			this.Controls.Add(this.tsContainer);
			this.Controls.Add(this.m_statusbar);
			this.Controls.Add(this.m_mainmenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.m_mainmenu;
			this.Name = "MainWnd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Localizer";
			this.m_mainmenu.ResumeLayout(false);
			this.m_mainmenu.PerformLayout();
			this.m_toolstrip.ResumeLayout(false);
			this.m_toolstrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.cmnuGrid.ResumeLayout(false);
			this.m_statusbar.ResumeLayout(false);
			this.m_statusbar.PerformLayout();
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.cmnuTree.ResumeLayout(false);
			this.splitEntries.Panel1.ResumeLayout(false);
			this.splitEntries.Panel2.ResumeLayout(false);
			this.splitEntries.ResumeLayout(false);
			this.splitSrcTransCmt.Panel1.ResumeLayout(false);
			this.splitSrcTransCmt.Panel2.ResumeLayout(false);
			this.splitSrcTransCmt.ResumeLayout(false);
			this.splitSrcTrans.Panel1.ResumeLayout(false);
			this.splitSrcTrans.Panel2.ResumeLayout(false);
			this.splitSrcTrans.ResumeLayout(false);
			this.tsContainer.ContentPanel.ResumeLayout(false);
			this.tsContainer.TopToolStripPanel.ResumeLayout(false);
			this.tsContainer.TopToolStripPanel.PerformLayout();
			this.tsContainer.ResumeLayout(false);
			this.tsContainer.PerformLayout();
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
		private System.Windows.Forms.SplitContainer splitEntries;
		private System.Windows.Forms.SplitContainer splitSrcTransCmt;
		private System.Windows.Forms.SplitContainer splitSrcTrans;
		private LabeledTextBox txtSrcText;
		private LabeledTextBox txtTranslation;
		private LabeledTextBox txtComment;
		private System.Windows.Forms.ToolStripButton tbbCompile;
		private System.Windows.Forms.ToolStripStatusLabel sslProgressBar;
		private System.Windows.Forms.ToolStripProgressBar progressBar;
		private System.Windows.Forms.SaveFileDialog saveFileDlg;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.ContextMenuStrip cmnuGrid;
		private System.Windows.Forms.ToolStripMenuItem cmnuUnreviewed;
		private System.Windows.Forms.ToolStripMenuItem cmnuCompleted;
		private System.Windows.Forms.ToolStripStatusLabel sslMain;
		private System.Windows.Forms.ToolStripButton tbbRescan;
		private System.Windows.Forms.ToolStripMenuItem mnuView;
		private System.Windows.Forms.ToolStripMenuItem mnuShowSrcTextPane;
		private System.Windows.Forms.ToolStripMenuItem mnuShowTransPane;
		private System.Windows.Forms.ToolStripMenuItem mnuShowCommentPane;
		private System.Windows.Forms.ToolStripMenuItem mnuTools;
		private System.Windows.Forms.ToolStripMenuItem mnuGoogleTranslate;
		private System.Windows.Forms.DataGridViewImageColumn colStatus;
		private System.Windows.Forms.DataGridViewTextBoxColumn colAssembly;
		private System.Windows.Forms.DataGridViewTextBoxColumn colResource;
		private System.Windows.Forms.DataGridViewTextBoxColumn colResourceId;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSourceText;
		private System.Windows.Forms.DataGridViewTextBoxColumn colTranslation;
		private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
		private System.Windows.Forms.ToolStripContainer tsContainer;
		private System.Windows.Forms.ToolStripDropDownButton tbbGoogleTranslate;
		private System.Windows.Forms.ToolStripMenuItem mnuGoggleTransSelected;
		private System.Windows.Forms.ToolStripMenuItem mnuGoogleTransAll;
		private System.Windows.Forms.ToolStripSeparator toolStripButton1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem cmnuOmitResourceItem;
		private System.Windows.Forms.ContextMenuStrip cmnuTree;
		private System.Windows.Forms.ToolStripMenuItem cmnuOmitAssembly;
		private System.Windows.Forms.ToolStripMenuItem cmnuOmitResource;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem mnuShowOmittedItems;
	}
}

