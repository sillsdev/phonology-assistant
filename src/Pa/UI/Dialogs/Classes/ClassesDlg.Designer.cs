using System.Windows.Forms;

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
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnModify = new System.Windows.Forms.Button();
			this.lvClasses = new SIL.Pa.UI.Controls.ClassListView();
			this.cmnuAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuAddCharClass = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuAddArtFeatureClass = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuAddBinFeatureClass = new System.Windows.Forms.ToolStripMenuItem();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.cmnuAdd.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.AutoSize = true;
			this.btnDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnDelete, null);
			this.locExtender.SetLocalizationComment(this.btnDelete, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizingId(this.btnDelete, "ClassesDlg.btnDelete");
			this.btnDelete.Location = new System.Drawing.Point(53, 236);
			this.btnDelete.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(80, 26);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnCopy
			// 
			this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCopy.AutoSize = true;
			this.btnCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCopy, null);
			this.locExtender.SetLocalizationComment(this.btnCopy, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizingId(this.btnCopy, "ClassesDlg.btnCopy");
			this.btnCopy.Location = new System.Drawing.Point(150, 246);
			this.btnCopy.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(80, 26);
			this.btnCopy.TabIndex = 2;
			this.btnCopy.Text = "&Copy";
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.AutoSize = true;
			this.btnAdd.Image = global::SIL.Pa.Properties.Resources.kimidButtonDropDownArrow;
			this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnAdd, null);
			this.locExtender.SetLocalizationComment(this.btnAdd, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizationPriority(this.btnAdd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnAdd, "ClassesDlg.btnAdd");
			this.btnAdd.Location = new System.Drawing.Point(135, 181);
			this.btnAdd.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(80, 26);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "&Add";
			this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnModify
			// 
			this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnModify.AutoSize = true;
			this.btnModify.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnModify, null);
			this.locExtender.SetLocalizationComment(this.btnModify, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizingId(this.btnModify, "ClassesDlg.btnModify");
			this.btnModify.Location = new System.Drawing.Point(250, 181);
			this.btnModify.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnModify.Name = "btnModify";
			this.btnModify.Size = new System.Drawing.Size(80, 26);
			this.btnModify.TabIndex = 1;
			this.btnModify.Text = "&Modify...";
			this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
			// 
			// lvClasses
			// 
			this.lvClasses.AppliesTo = SIL.Pa.UI.Controls.ClassListView.ListApplicationType.ClassesDialog;
			this.lvClasses.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvClasses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.lvClasses.FullRowSelect = true;
			this.lvClasses.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.lvClasses, null);
			this.locExtender.SetLocalizationComment(this.lvClasses, null);
			this.locExtender.SetLocalizingId(this.lvClasses, "ClassesDlg.lvClasses");
			this.lvClasses.Location = new System.Drawing.Point(10, 10);
			this.lvClasses.MultiSelect = false;
			this.lvClasses.Name = "lvClasses";
			this.lvClasses.OwnerDraw = true;
			this.lvClasses.ShowMembersAndTypeColumns = true;
			this.lvClasses.Size = new System.Drawing.Size(617, 402);
			this.lvClasses.TabIndex = 0;
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
			this.locExtender.SetLocalizationPriority(this.cmnuAdd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cmnuAdd, "ClassesDlg.cmnuAdd");
			this.cmnuAdd.Name = "cmnuAdd";
			this.cmnuAdd.ShowImageMargin = false;
			this.cmnuAdd.Size = new System.Drawing.Size(198, 92);
			// 
			// cmnuAddCharClass
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddCharClass, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddCharClass, "Text on drop-down displayed when clicking Add button on classes dialog box.");
			this.locExtender.SetLocalizingId(this.cmnuAddCharClass, "ClassesDlg.cmnuAddCharClass");
			this.cmnuAddCharClass.Name = "cmnuAddCharClass";
			this.cmnuAddCharClass.Size = new System.Drawing.Size(197, 22);
			this.cmnuAddCharClass.Text = "&Phones Class...";
			this.cmnuAddCharClass.Click += new System.EventHandler(this.cmnuAddCharClass_Click);
			// 
			// cmnuAddArtFeatureClass
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddArtFeatureClass, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddArtFeatureClass, "Text on drop-down displayed when clicking Add button on classes dialog box.");
			this.locExtender.SetLocalizingId(this.cmnuAddArtFeatureClass, "ClassesDlg.cmnuAddArtFeatureClass");
			this.cmnuAddArtFeatureClass.Name = "cmnuAddArtFeatureClass";
			this.cmnuAddArtFeatureClass.Size = new System.Drawing.Size(197, 22);
			this.cmnuAddArtFeatureClass.Text = "&Articulatory Features Class...";
			this.cmnuAddArtFeatureClass.Click += new System.EventHandler(this.cmnuAddArtFeatureClass_Click);
			// 
			// cmnuAddBinFeatureClass
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddBinFeatureClass, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddBinFeatureClass, "Text on drop-down displayed when clicking Add button on classes dialog box.");
			this.locExtender.SetLocalizingId(this.cmnuAddBinFeatureClass, "ClassesDlg.cmnuAddBinFeatureClass");
			this.cmnuAddBinFeatureClass.Name = "cmnuAddBinFeatureClass";
			this.cmnuAddBinFeatureClass.Size = new System.Drawing.Size(197, 22);
			this.cmnuAddBinFeatureClass.Text = "&Binary Features Class...";
			this.cmnuAddBinFeatureClass.Click += new System.EventHandler(this.cmnuAddBinFeatureClass_Click);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// ClassesDlg
			// 
			this.AcceptButton = null;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(637, 452);
			this.Controls.Add(this.btnModify);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnCopy);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.lvClasses);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ClassesDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(645, 375);
			this.Name = "ClassesDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Classes";
			this.Controls.SetChildIndex(this.lvClasses, 0);
			this.Controls.SetChildIndex(this.btnDelete, 0);
			this.Controls.SetChildIndex(this.btnCopy, 0);
			this.Controls.SetChildIndex(this.btnAdd, 0);
			this.Controls.SetChildIndex(this.btnModify, 0);
			this.cmnuAdd.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private Localization.UI.LocalizationExtender locExtender;
	}
}