namespace SIL.Pa.UI.Dialogs
{
	partial class SaveSearchQueryDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveSearchQueryDlg));
			this.cboCategories = new System.Windows.Forms.ComboBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblPattern = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblCategories = new System.Windows.Forms.Label();
			this.lblPatternLabel = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localization.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizationPriority(this.btnCancel, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnCancel, "Localized in base class");
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizationPriority(this.btnOK, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnOK, "Localized in base class");
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizationPriority(this.btnHelp, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnHelp, "Localized in base class");
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// cboCategories
			// 
			resources.ApplyResources(this.cboCategories, "cboCategories");
			this.cboCategories.DropDownHeight = 200;
			this.cboCategories.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboCategories, "Enter a category in which to save the\\npattern or choose one from the list");
			this.locExtender.SetLocalizationComment(this.cboCategories, "Drop-down list of search pattern categories on the dialog box for saving search p" +
					"atterns in search  view.");
			this.locExtender.SetLocalizationPriority(this.cboCategories, SIL.Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.cboCategories, "SaveSearchQueryDlg.cboCategories");
			this.cboCategories.Name = "cboCategories";
			this.cboCategories.Sorted = true;
			this.cboCategories.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtName
			// 
			resources.ApplyResources(this.txtName, "txtName");
			this.locExtender.SetLocalizableToolTip(this.txtName, "Enter the name to give your saved pattern.");
			this.locExtender.SetLocalizationComment(this.txtName, "Search pattern name text box on the dialog box for saving search patterns in sear" +
					"ch  view.");
			this.locExtender.SetLocalizationPriority(this.txtName, SIL.Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.txtName, "SaveSearchQueryDlg.txtName");
			this.txtName.Name = "txtName";
			this.txtName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblPattern
			// 
			this.lblPattern.AutoEllipsis = true;
			resources.ApplyResources(this.lblPattern, "lblPattern");
			this.lblPattern.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblPattern, null);
			this.locExtender.SetLocalizationComment(this.lblPattern, null);
			this.locExtender.SetLocalizationPriority(this.lblPattern, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblPattern, "SaveSearchQueryDlg.lblPattern");
			this.lblPattern.Name = "lblPattern";
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblName, null);
			this.locExtender.SetLocalizationComment(this.lblName, "Label on dialog box for saving search patterns in search view.");
			this.locExtender.SetLocalizingId(this.lblName, "SaveSearchQueryDlg.lblName");
			this.lblName.Name = "lblName";
			// 
			// lblCategories
			// 
			resources.ApplyResources(this.lblCategories, "lblCategories");
			this.lblCategories.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblCategories, null);
			this.locExtender.SetLocalizationComment(this.lblCategories, "Label on dialog box for saving search patterns in search view.");
			this.locExtender.SetLocalizingId(this.lblCategories, "SaveSearchQueryDlg.lblCategories");
			this.lblCategories.Name = "lblCategories";
			// 
			// lblPatternLabel
			// 
			resources.ApplyResources(this.lblPatternLabel, "lblPatternLabel");
			this.lblPatternLabel.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblPatternLabel, null);
			this.locExtender.SetLocalizationComment(this.lblPatternLabel, "Label on dialog box for saving search patterns in search view.");
			this.locExtender.SetLocalizingId(this.lblPatternLabel, "SaveSearchQueryDlg.lblPatternLabel");
			this.lblPatternLabel.Name = "lblPatternLabel";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// SaveSearchQueryDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblPattern);
			this.Controls.Add(this.lblPatternLabel);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.cboCategories);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblCategories);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SaveSearchQueryDlg.WindowTitle");
			this.Name = "SaveSearchQueryDlg";
			this.Controls.SetChildIndex(this.lblCategories, 0);
			this.Controls.SetChildIndex(this.txtName, 0);
			this.Controls.SetChildIndex(this.cboCategories, 0);
			this.Controls.SetChildIndex(this.lblName, 0);
			this.Controls.SetChildIndex(this.lblPatternLabel, 0);
			this.Controls.SetChildIndex(this.lblPattern, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboCategories;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblPattern;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblCategories;
		private System.Windows.Forms.Label lblPatternLabel;
		private SIL.Localization.LocalizationExtender locExtender;
	}
}