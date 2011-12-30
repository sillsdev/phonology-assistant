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
			this.cboCategories = new System.Windows.Forms.ComboBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblPattern = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblCategories = new System.Windows.Forms.Label();
			this.lblPatternLabel = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// cboCategories
			// 
			this.cboCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboCategories.DropDownHeight = 200;
			this.cboCategories.FormattingEnabled = true;
			this.cboCategories.IntegralHeight = false;
			this.locExtender.SetLocalizableToolTip(this.cboCategories, "Enter a category in which to save the\\npattern or choose one from the list");
			this.locExtender.SetLocalizationComment(this.cboCategories, "Drop-down list of search pattern categories on the dialog box for saving search p" +
					"atterns in search  view.");
			this.locExtender.SetLocalizationPriority(this.cboCategories, Localization.LocalizationPriority.High);
			this.locExtender.SetLocalizingId(this.cboCategories, "DialogBoxes.SaveSearchQueryDlg.cboCategories");
			this.cboCategories.Location = new System.Drawing.Point(137, 100);
			this.cboCategories.Margin = new System.Windows.Forms.Padding(0);
			this.cboCategories.Name = "cboCategories";
			this.cboCategories.Size = new System.Drawing.Size(271, 21);
			this.cboCategories.Sorted = true;
			this.cboCategories.TabIndex = 5;
			this.cboCategories.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.txtName, "Enter the name to give your saved pattern.");
			this.locExtender.SetLocalizationComment(this.txtName, "Search pattern name text box on the dialog box for saving search patterns in sear" +
					"ch  view.");
			this.locExtender.SetLocalizationPriority(this.txtName, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.txtName, "DialogBoxes.SaveSearchQueryDlg.txtName");
			this.txtName.Location = new System.Drawing.Point(137, 54);
			this.txtName.Margin = new System.Windows.Forms.Padding(0);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(271, 20);
			this.txtName.TabIndex = 3;
			this.txtName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblPattern
			// 
			this.lblPattern.AutoEllipsis = true;
			this.lblPattern.AutoSize = true;
			this.lblPattern.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblPattern, null);
			this.locExtender.SetLocalizationComment(this.lblPattern, null);
			this.locExtender.SetLocalizationPriority(this.lblPattern, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblPattern, "SaveSearchQueryDlg.lblPattern");
			this.lblPattern.Location = new System.Drawing.Point(134, 10);
			this.lblPattern.Name = "lblPattern";
			this.lblPattern.Size = new System.Drawing.Size(14, 13);
			this.lblPattern.TabIndex = 1;
			this.lblPattern.Text = "#";
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblName, null);
			this.locExtender.SetLocalizationComment(this.lblName, "Label on dialog box for saving search patterns in search view.");
			this.locExtender.SetLocalizingId(this.lblName, "DialogBoxes.SaveSearchQueryDlg.NameLabel");
			this.lblName.Location = new System.Drawing.Point(13, 49);
			this.lblName.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(44, 15);
			this.lblName.TabIndex = 2;
			this.lblName.Text = "&Name:";
			// 
			// lblCategories
			// 
			this.lblCategories.AutoSize = true;
			this.lblCategories.BackColor = System.Drawing.Color.Transparent;
			this.lblCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblCategories.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblCategories, null);
			this.locExtender.SetLocalizationComment(this.lblCategories, "Label on dialog box for saving search patterns in search view.");
			this.locExtender.SetLocalizingId(this.lblCategories, "DialogBoxes.SaveSearchQueryDlg.CategoriesLabel");
			this.lblCategories.Location = new System.Drawing.Point(13, 127);
			this.lblCategories.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.lblCategories.Name = "lblCategories";
			this.lblCategories.Size = new System.Drawing.Size(215, 15);
			this.lblCategories.TabIndex = 4;
			this.lblCategories.Text = "&Category in which\\nto save the pattern:";
			// 
			// lblPatternLabel
			// 
			this.lblPatternLabel.AutoSize = true;
			this.lblPatternLabel.BackColor = System.Drawing.Color.Transparent;
			this.lblPatternLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblPatternLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblPatternLabel, null);
			this.locExtender.SetLocalizationComment(this.lblPatternLabel, "Label on dialog box for saving search patterns in search view.");
			this.locExtender.SetLocalizingId(this.lblPatternLabel, "DialogBoxes.SaveSearchQueryDlg.PatternLabel");
			this.lblPatternLabel.Location = new System.Drawing.Point(13, 13);
			this.lblPatternLabel.Name = "lblPatternLabel";
			this.lblPatternLabel.Size = new System.Drawing.Size(49, 15);
			this.lblPatternLabel.TabIndex = 0;
			this.lblPatternLabel.Text = "Pattern:";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SaveSearchQueryDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(418, 205);
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
			this.locExtender.SetLocalizingId(this, "DialogBoxes.SaveSearchQueryDlg.WindowTitle");
			this.Name = "SaveSearchQueryDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Save Search Pattern";
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
		private Localization.UI.LocalizationExtender locExtender;
	}
}