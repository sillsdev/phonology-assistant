namespace SIL.Pa
{
	partial class PCIEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PCIEditor));
			this.mnuMain = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSort = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuColSort = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPOA = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMOA = new System.Windows.Forms.ToolStripMenuItem();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnModify = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.pnlGrid = new System.Windows.Forms.Panel();
			this.pnlButtons.SuspendLayout();
			this.mnuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.Add(this.btnDelete);
			this.pnlButtons.Controls.Add(this.btnModify);
			this.pnlButtons.Controls.Add(this.btnAdd);
			this.pnlButtons.Controls.SetChildIndex(this.btnAdd, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnModify, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnDelete, 0);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuSort});
			resources.ApplyResources(this.mnuMain, "mnuMain");
			this.mnuMain.Name = "mnuMain";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.toolStripSeparator1,
            this.mnuSave,
            this.mnuSaveAs,
            this.toolStripSeparator2,
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			resources.ApplyResources(this.mnuFile, "mnuFile");
			// 
			// mnuOpen
			// 
			this.mnuOpen.Name = "mnuOpen";
			resources.ApplyResources(this.mnuOpen, "mnuOpen");
			this.mnuOpen.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// mnuSave
			// 
			this.mnuSave.Name = "mnuSave";
			resources.ApplyResources(this.mnuSave, "mnuSave");
			this.mnuSave.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Name = "mnuSaveAs";
			resources.ApplyResources(this.mnuSaveAs, "mnuSaveAs");
			this.mnuSaveAs.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			resources.ApplyResources(this.mnuExit, "mnuExit");
			this.mnuExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// mnuSort
			// 
			this.mnuSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuColSort,
            this.mnuPOA,
            this.mnuMOA});
			this.mnuSort.Name = "mnuSort";
			resources.ApplyResources(this.mnuSort, "mnuSort");
			this.mnuSort.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuSort_DropDownItemClicked);
			// 
			// mnuColSort
			// 
			this.mnuColSort.Name = "mnuColSort";
			resources.ApplyResources(this.mnuColSort, "mnuColSort");
			// 
			// mnuPOA
			// 
			this.mnuPOA.Name = "mnuPOA";
			resources.ApplyResources(this.mnuPOA, "mnuPOA");
			// 
			// mnuMOA
			// 
			this.mnuMOA.Name = "mnuMOA";
			resources.ApplyResources(this.mnuMOA, "mnuMOA");
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnModify
			// 
			resources.ApplyResources(this.btnModify, "btnModify");
			this.btnModify.Name = "btnModify";
			this.btnModify.UseVisualStyleBackColor = true;
			this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
			// 
			// btnDelete
			// 
			resources.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// pnlGrid
			// 
			resources.ApplyResources(this.pnlGrid, "pnlGrid");
			this.pnlGrid.Name = "pnlGrid";
			// 
			// PCIEditor
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = null;
			this.Controls.Add(this.pnlGrid);
			this.Controls.Add(this.mnuMain);
			this.MainMenuStrip = this.mnuMain;
			this.MaximizeBox = true;
			this.MinimizeBox = true;
			this.Name = "PCIEditor";
			this.ShowIcon = true;
			this.ShowInTaskbar = true;
			this.Controls.SetChildIndex(this.mnuMain, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.pnlButtons.ResumeLayout(false);
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuSave;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnModify;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.ToolStripMenuItem mnuSort;
		private System.Windows.Forms.ToolStripMenuItem mnuColSort;
		private System.Windows.Forms.ToolStripMenuItem mnuPOA;
		private System.Windows.Forms.ToolStripMenuItem mnuMOA;
		private System.Windows.Forms.Panel pnlGrid;
	}
}

