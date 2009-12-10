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
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
			this.cmnuAdd.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
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
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizationPriority(this.btnCancel, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnCancel, "ClassesDlg.btnCancel");
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizationPriority(this.btnOK, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnOK, "ClassesDlg.btnOK");
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizationPriority(this.btnHelp, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnHelp, "ClassesDlg.btnHelp");
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// btnDelete
			// 
			resources.ApplyResources(this.btnDelete, "btnDelete");
			this.locExtender.SetLocalizableToolTip(this.btnDelete, null);
			this.locExtender.SetLocalizationComment(this.btnDelete, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizingId(this.btnDelete, "ClassesDlg.btnDelete");
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnCopy
			// 
			resources.ApplyResources(this.btnCopy, "btnCopy");
			this.locExtender.SetLocalizableToolTip(this.btnCopy, null);
			this.locExtender.SetLocalizationComment(this.btnCopy, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizingId(this.btnCopy, "ClassesDlg.btnCopy");
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Image = global::SIL.Pa.Properties.Resources.kimidButtonDropDownArrow;
			this.locExtender.SetLocalizableToolTip(this.btnAdd, null);
			this.locExtender.SetLocalizationComment(this.btnAdd, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizationPriority(this.btnAdd, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnAdd, "ClassesDlg.btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnModify
			// 
			resources.ApplyResources(this.btnModify, "btnModify");
			this.locExtender.SetLocalizableToolTip(this.btnModify, null);
			this.locExtender.SetLocalizationComment(this.btnModify, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizingId(this.btnModify, "ClassesDlg.btnModify");
			this.btnModify.Name = "btnModify";
			this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
			// 
			// lvClasses
			// 
			this.lvClasses.AppliesTo = SIL.Pa.UI.Controls.ClassListView.ListApplicationType.ClassesDialog;
			resources.ApplyResources(this.lvClasses, "lvClasses");
			this.lvClasses.FullRowSelect = true;
			this.lvClasses.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.lvClasses, null);
			this.locExtender.SetLocalizationComment(this.lvClasses, null);
			this.locExtender.SetLocalizingId(this.lvClasses, "ClassesDlg.lvClasses");
			this.lvClasses.MultiSelect = false;
			this.lvClasses.Name = "lvClasses";
			this.lvClasses.OwnerDraw = true;
			this.lvClasses.ShowMembersAndTypeColumns = true;
			this.lvClasses.UseCompatibleStateImageBehavior = false;
			this.lvClasses.View = System.Windows.Forms.View.Details;
			this.lvClasses.SelectedIndexChanged += new System.EventHandler(this.lvClasses_SelectedIndexChanged);
			this.lvClasses.DoubleClick += new System.EventHandler(this.lvClasses_DoubleClick);
			this.lvClasses.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvClasses_KeyDown);
			// 
			// cmnuAdd
			// 
			this.cmnuAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuAddCharClass,
            this.cmnuAddArtFeatureClass,
            this.cmnuAddBinFeatureClass});
			this.locExtender.SetLocalizableToolTip(this.cmnuAdd, null);
			this.locExtender.SetLocalizationComment(this.cmnuAdd, null);
			this.locExtender.SetLocalizingId(this.cmnuAdd, "cmnuAdd.cmnuAdd");
			this.cmnuAdd.Name = "cmnuAdd";
			this.cmnuAdd.ShowImageMargin = false;
			resources.ApplyResources(this.cmnuAdd, "cmnuAdd");
			// 
			// cmnuAddCharClass
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddCharClass, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddCharClass, "Text on the drop-down displayed when clicking Add button on the classes dialog bo" +
					"x.");
			this.locExtender.SetLocalizingId(this.cmnuAddCharClass, "ClassesDlg.cmnuAddCharClass");
			this.cmnuAddCharClass.Name = "cmnuAddCharClass";
			resources.ApplyResources(this.cmnuAddCharClass, "cmnuAddCharClass");
			this.cmnuAddCharClass.Click += new System.EventHandler(this.cmnuAddCharClass_Click);
			// 
			// cmnuAddArtFeatureClass
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddArtFeatureClass, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddArtFeatureClass, "Text on the drop-down displayed when clicking Add button on the classes dialog bo" +
					"x.");
			this.locExtender.SetLocalizingId(this.cmnuAddArtFeatureClass, "ClassesDlg.cmnuAddArtFeatureClass");
			this.cmnuAddArtFeatureClass.Name = "cmnuAddArtFeatureClass";
			resources.ApplyResources(this.cmnuAddArtFeatureClass, "cmnuAddArtFeatureClass");
			this.cmnuAddArtFeatureClass.Click += new System.EventHandler(this.cmnuAddArtFeatureClass_Click);
			// 
			// cmnuAddBinFeatureClass
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddBinFeatureClass, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddBinFeatureClass, "Text on the drop-down displayed when clicking Add button on the classes dialog bo" +
					"x.");
			this.locExtender.SetLocalizingId(this.cmnuAddBinFeatureClass, "ClassesDlg.cmnuAddBinFeatureClass");
			this.cmnuAddBinFeatureClass.Name = "cmnuAddBinFeatureClass";
			resources.ApplyResources(this.cmnuAddBinFeatureClass, "cmnuAddBinFeatureClass");
			this.cmnuAddBinFeatureClass.Click += new System.EventHandler(this.cmnuAddBinFeatureClass_Click);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// ClassesDlg
			// 
			this.AcceptButton = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lvClasses);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "ClassesDlg.WindowTitle");
			this.Name = "ClassesDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.lvClasses, 0);
			this.pnlButtons.ResumeLayout(false);
			this.cmnuAdd.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
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
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}