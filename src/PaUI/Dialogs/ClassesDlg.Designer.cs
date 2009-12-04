namespace SIL.Pa.UI.Dialogs
{
	partial class ClassesDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassesDlg));
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnModify = new System.Windows.Forms.Button();
			this.lvClasses = new SIL.Pa.UI.Controls.ClassListView();
			this.cmnuAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuAddCharClass = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuAddArtFeatureClass = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuAddBinFeatureClass = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlButtons.SuspendLayout();
			this.cmnuAdd.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnModify);
			this.pnlButtons.Controls.Add(this.btnDelete);
			this.pnlButtons.Controls.Add(this.btnAdd);
			this.pnlButtons.Controls.Add(this.btnCopy);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCopy, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnAdd, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnDelete, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnModify, 0);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// btnDelete
			// 
			resources.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnCopy
			// 
			resources.ApplyResources(this.btnCopy, "btnCopy");
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Image = global::SIL.Pa.Properties.Resources.kimidButtonDropDownArrow;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnModify
			// 
			resources.ApplyResources(this.btnModify, "btnModify");
			this.btnModify.Name = "btnModify";
			this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
			// 
			// lvClasses
			// 
			this.lvClasses.AppliesTo = SIL.Pa.UI.Controls.ClassListView.ListApplicationType.ClassesDialog;
			resources.ApplyResources(this.lvClasses, "lvClasses");
			this.lvClasses.FullRowSelect = true;
			this.lvClasses.HideSelection = false;
			this.lvClasses.MultiSelect = false;
			this.lvClasses.Name = "lvClasses";
			this.lvClasses.OwnerDraw = true;
			this.lvClasses.ShowMembersAndTypeColumns = true;
			this.lvClasses.UseCompatibleStateImageBehavior = false;
			this.lvClasses.View = System.Windows.Forms.View.Details;
			this.lvClasses.DoubleClick += new System.EventHandler(this.lvClasses_DoubleClick);
			this.lvClasses.SelectedIndexChanged += new System.EventHandler(this.lvClasses_SelectedIndexChanged);
			this.lvClasses.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvClasses_KeyDown);
			// 
			// cmnuAdd
			// 
			this.cmnuAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuAddCharClass,
            this.cmnuAddArtFeatureClass,
            this.cmnuAddBinFeatureClass});
			this.cmnuAdd.Name = "cmnuAdd";
			this.cmnuAdd.ShowImageMargin = false;
			resources.ApplyResources(this.cmnuAdd, "cmnuAdd");
			// 
			// cmnuAddCharClass
			// 
			this.cmnuAddCharClass.Name = "cmnuAddCharClass";
			resources.ApplyResources(this.cmnuAddCharClass, "cmnuAddCharClass");
			this.cmnuAddCharClass.Click += new System.EventHandler(this.cmnuAddCharClass_Click);
			// 
			// cmnuAddArtFeatureClass
			// 
			this.cmnuAddArtFeatureClass.Name = "cmnuAddArtFeatureClass";
			resources.ApplyResources(this.cmnuAddArtFeatureClass, "cmnuAddArtFeatureClass");
			this.cmnuAddArtFeatureClass.Click += new System.EventHandler(this.cmnuAddArtFeatureClass_Click);
			// 
			// cmnuAddBinFeatureClass
			// 
			this.cmnuAddBinFeatureClass.Name = "cmnuAddBinFeatureClass";
			resources.ApplyResources(this.cmnuAddBinFeatureClass, "cmnuAddBinFeatureClass");
			this.cmnuAddBinFeatureClass.Click += new System.EventHandler(this.cmnuAddBinFeatureClass_Click);
			// 
			// ClassesDlg
			// 
			this.AcceptButton = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lvClasses);
			this.Name = "ClassesDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.lvClasses, 0);
			this.pnlButtons.ResumeLayout(false);
			this.cmnuAdd.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Button btnDelete;
		protected System.Windows.Forms.Button btnCopy;
		protected System.Windows.Forms.Button btnAdd;
		protected System.Windows.Forms.Button btnModify;
		private SIL.Pa.UI.Controls.ClassListView lvClasses;
		private System.Windows.Forms.ContextMenuStrip cmnuAdd;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddCharClass;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddArtFeatureClass;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddBinFeatureClass;
	}
}