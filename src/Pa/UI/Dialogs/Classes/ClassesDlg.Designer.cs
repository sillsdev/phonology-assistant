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
			this.ClassListView = new SIL.Pa.UI.Controls.ClassListView();
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
			this.locExtender.SetLocalizingId(this.btnDelete, "DialogBoxes.ClassesDlg.DeleteButton");
			this.btnDelete.Location = new System.Drawing.Point(53, 236);
			this.btnDelete.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(80, 26);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.HandleDeleteButtonClick);
			// 
			// btnCopy
			// 
			this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCopy.AutoSize = true;
			this.btnCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCopy, null);
			this.locExtender.SetLocalizationComment(this.btnCopy, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizingId(this.btnCopy, "DialogBoxes.ClassesDlg.CopyButton");
			this.btnCopy.Location = new System.Drawing.Point(150, 246);
			this.btnCopy.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(80, 26);
			this.btnCopy.TabIndex = 2;
			this.btnCopy.Text = "&Copy";
			this.btnCopy.Click += new System.EventHandler(this.HandleCopyButtonClick);
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
			this.locExtender.SetLocalizingId(this.btnAdd, "DialogBoxes.ClassesDlg.AddButton");
			this.btnAdd.Location = new System.Drawing.Point(135, 181);
			this.btnAdd.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(80, 26);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "&Add";
			this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnAdd.Click += new System.EventHandler(this.HandleAddButtonClick);
			// 
			// btnModify
			// 
			this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnModify.AutoSize = true;
			this.btnModify.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnModify, null);
			this.locExtender.SetLocalizationComment(this.btnModify, "Text on add button drop-down on classes dialog box.");
			this.locExtender.SetLocalizingId(this.btnModify, "DialogBoxes.ClassesDlg.ModifyButton");
			this.btnModify.Location = new System.Drawing.Point(250, 181);
			this.btnModify.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnModify.Name = "btnModify";
			this.btnModify.Size = new System.Drawing.Size(80, 26);
			this.btnModify.TabIndex = 1;
			this.btnModify.Text = "&Modify...";
			this.btnModify.Click += new System.EventHandler(this.HandleModifyButtonClick);
			// 
			// lvClasses
			// 
			this.ClassListView.AppliesTo = SIL.Pa.UI.Controls.ClassListView.ListApplicationType.ClassesDialog;
			this.ClassListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClassListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.ClassListView.FullRowSelect = true;
			this.ClassListView.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.ClassListView, null);
			this.locExtender.SetLocalizationComment(this.ClassListView, null);
			this.locExtender.SetLocalizingId(this.ClassListView, "ClassesDlg.lvClasses");
			this.ClassListView.Location = new System.Drawing.Point(10, 10);
			this.ClassListView.MultiSelect = false;
			this.ClassListView.Name = "ClassListView";
			this.ClassListView.OwnerDraw = true;
			this.ClassListView.ShowMembersAndTypeColumns = true;
			this.ClassListView.Size = new System.Drawing.Size(617, 402);
			this.ClassListView.TabIndex = 0;
			this.ClassListView.UseCompatibleStateImageBehavior = false;
			this.ClassListView.View = System.Windows.Forms.View.Details;
			this.ClassListView.SelectedIndexChanged += new System.EventHandler(this.HandleClassesListViewSelectedIndexChanged);
			this.ClassListView.DoubleClick += new System.EventHandler(this.HandleClassesListViewDoubleClick);
			this.ClassListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleClassesListViewKeyDown);
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
			this.locExtender.SetLocalizingId(this.cmnuAddCharClass, "DialogBoxes.ClassesDlg.AddPhonesClass");
			this.cmnuAddCharClass.Name = "cmnuAddCharClass";
			this.cmnuAddCharClass.Size = new System.Drawing.Size(197, 22);
			this.cmnuAddCharClass.Text = "&Phones Class...";
			// 
			// cmnuAddArtFeatureClass
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddArtFeatureClass, null);
			this.locExtender.SetLocalizingId(this.cmnuAddArtFeatureClass, "DialogBoxes.ClassesDlg.AddDescriptiveFeaturesClassMenu");
			this.cmnuAddArtFeatureClass.Name = "cmnuAddArtFeatureClass";
			this.cmnuAddArtFeatureClass.Size = new System.Drawing.Size(197, 22);
			this.cmnuAddArtFeatureClass.Text = "D&escriptive Features Class...";
			// 
			// cmnuAddBinFeatureClass
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddBinFeatureClass, null);
			this.locExtender.SetLocalizingId(this.cmnuAddBinFeatureClass, "DialogBoxes.ClassesDlg.AddDistinctiveFeaturesClassMenu");
			this.cmnuAddBinFeatureClass.Name = "cmnuAddBinFeatureClass";
			this.cmnuAddBinFeatureClass.Size = new System.Drawing.Size(197, 22);
			this.cmnuAddBinFeatureClass.Text = "D&istinctive Features Class...";
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
			this.Controls.Add(this.ClassListView);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.ClassesDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(645, 375);
			this.Name = "ClassesDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Classes";
			this.Controls.SetChildIndex(this.ClassListView, 0);
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
		private System.Windows.Forms.ContextMenuStrip cmnuAdd;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddCharClass;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddArtFeatureClass;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddBinFeatureClass;
		private Localization.UI.LocalizationExtender locExtender;
	}
}