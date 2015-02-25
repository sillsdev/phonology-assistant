namespace SIL.Pa.UI.Dialogs
{
	partial class Fw7DataSourcePropertiesDlg
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
            this.lblProjectValue = new System.Windows.Forms.Label();
            this.lblProject = new System.Windows.Forms.Label();
            this.grpPhoneticField = new System.Windows.Forms.GroupBox();
            this.tblLayoutPhoneticData = new System.Windows.Forms.TableLayoutPanel();
            this.lblLexicalOptions = new System.Windows.Forms.Label();
            this.lblPhoneticWritingSystem = new System.Windows.Forms.Label();
            this.cboPhoneticWritingSystem = new System.Windows.Forms.ComboBox();
            this.rbPronunField = new System.Windows.Forms.RadioButton();
            this.rbVernForm = new System.Windows.Forms.RadioButton();
            this.cboPronunciationOptions = new System.Windows.Forms.ComboBox();
            this.lblPronunciationOptions = new System.Windows.Forms.Label();
            this.cboVernacularOptions = new System.Windows.Forms.ComboBox();
            this.grpFields = new System.Windows.Forms.GroupBox();
            this.pnlGrid = new SilTools.Controls.SilPanel();
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            this.tblLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
            this.grpPhoneticField.SuspendLayout();
            this.tblLayoutPhoneticData.SuspendLayout();
            this.grpFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this.tblLayoutOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblProjectValue
            // 
            this.lblProjectValue.AutoEllipsis = true;
            this.lblProjectValue.AutoSize = true;
            this.lblProjectValue.BackColor = System.Drawing.Color.Transparent;
            this.locExtender.SetLocalizableToolTip(this.lblProjectValue, null);
            this.locExtender.SetLocalizationComment(this.lblProjectValue, null);
            this.locExtender.SetLocalizationPriority(this.lblProjectValue, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.lblProjectValue, "DialogBoxes.Fw7DataSourcePropertiesDlg.ProjectValueLabel");
            this.lblProjectValue.Location = new System.Drawing.Point(75, 4);
            this.lblProjectValue.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblProjectValue.Name = "lblProjectValue";
            this.lblProjectValue.Size = new System.Drawing.Size(16, 17);
            this.lblProjectValue.TabIndex = 1;
            this.lblProjectValue.Text = "#";
            // 
            // lblProject
            // 
            this.lblProject.AutoSize = true;
            this.lblProject.BackColor = System.Drawing.Color.Transparent;
            this.lblProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.locExtender.SetLocalizableToolTip(this.lblProject, null);
            this.locExtender.SetLocalizationComment(this.lblProject, "Label at top of FieldWorks data source properties dialog box.");
            this.locExtender.SetLocalizingId(this.lblProject, "DialogBoxes.Fw7DataSourcePropertiesDlg.ProjectLabel");
            this.lblProject.Location = new System.Drawing.Point(9, 4);
            this.lblProject.Margin = new System.Windows.Forms.Padding(9, 4, 7, 0);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(59, 18);
            this.lblProject.TabIndex = 0;
            this.lblProject.Text = "Project:";
            // 
            // grpPhoneticField
            // 
            this.grpPhoneticField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPhoneticField.AutoSize = true;
            this.grpPhoneticField.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblLayoutOuter.SetColumnSpan(this.grpPhoneticField, 2);
            this.grpPhoneticField.Controls.Add(this.tblLayoutPhoneticData);
            this.grpPhoneticField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.locExtender.SetLocalizableToolTip(this.grpPhoneticField, null);
            this.locExtender.SetLocalizationComment(this.grpPhoneticField, "");
            this.locExtender.SetLocalizingId(this.grpPhoneticField, "DialogBoxes.Fw7DataSourcePropertiesDlg.PhoneticFieldGroupBox");
            this.grpPhoneticField.Location = new System.Drawing.Point(0, 37);
            this.grpPhoneticField.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.grpPhoneticField.Name = "grpPhoneticField";
            this.grpPhoneticField.Padding = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.grpPhoneticField.Size = new System.Drawing.Size(514, 221);
            this.grpPhoneticField.TabIndex = 2;
            this.grpPhoneticField.TabStop = false;
            this.grpPhoneticField.Text = "Phonetic Data";
            // 
            // tblLayoutPhoneticData
            // 
            this.tblLayoutPhoneticData.AutoSize = true;
            this.tblLayoutPhoneticData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblLayoutPhoneticData.BackColor = System.Drawing.Color.Transparent;
            this.tblLayoutPhoneticData.ColumnCount = 2;
            this.tblLayoutPhoneticData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblLayoutPhoneticData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutPhoneticData.Controls.Add(this.lblLexicalOptions, 0, 1);
            this.tblLayoutPhoneticData.Controls.Add(this.lblPhoneticWritingSystem, 0, 4);
            this.tblLayoutPhoneticData.Controls.Add(this.cboPhoneticWritingSystem, 1, 4);
            this.tblLayoutPhoneticData.Controls.Add(this.rbPronunField, 0, 2);
            this.tblLayoutPhoneticData.Controls.Add(this.rbVernForm, 0, 0);
            this.tblLayoutPhoneticData.Controls.Add(this.cboPronunciationOptions, 1, 3);
            this.tblLayoutPhoneticData.Controls.Add(this.lblPronunciationOptions, 0, 3);
            this.tblLayoutPhoneticData.Controls.Add(this.cboVernacularOptions, 1, 1);
            this.tblLayoutPhoneticData.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayoutPhoneticData.Location = new System.Drawing.Point(4, 21);
            this.tblLayoutPhoneticData.Margin = new System.Windows.Forms.Padding(4);
            this.tblLayoutPhoneticData.Name = "tblLayoutPhoneticData";
            this.tblLayoutPhoneticData.RowCount = 5;
            this.tblLayoutPhoneticData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutPhoneticData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutPhoneticData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutPhoneticData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutPhoneticData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutPhoneticData.Size = new System.Drawing.Size(506, 189);
            this.tblLayoutPhoneticData.TabIndex = 0;
            this.tblLayoutPhoneticData.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutPhoneticDataPaint);
            // 
            // lblLexicalOptions
            // 
            this.lblLexicalOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLexicalOptions.AutoSize = true;
            this.lblLexicalOptions.BackColor = System.Drawing.Color.Transparent;
            this.locExtender.SetLocalizableToolTip(this.lblLexicalOptions, null);
            this.locExtender.SetLocalizationComment(this.lblLexicalOptions, null);
            this.locExtender.SetLocalizingId(this.lblLexicalOptions, "DialogBoxes.Fw7DataSourcePropertiesDlg.PronunciationOptionsLabel");
            this.lblLexicalOptions.Location = new System.Drawing.Point(76, 40);
            this.lblLexicalOptions.Margin = new System.Windows.Forms.Padding(37, 6, 0, 0);
            this.lblLexicalOptions.Name = "lblLexicalOptions";
            this.lblLexicalOptions.Size = new System.Drawing.Size(188, 18);
            this.lblLexicalOptions.TabIndex = 7;
            this.lblLexicalOptions.Text = "Records will be created for ";
            // 
            // lblPhoneticWritingSystem
            // 
            this.lblPhoneticWritingSystem.AutoSize = true;
            this.lblPhoneticWritingSystem.BackColor = System.Drawing.Color.Transparent;
            this.locExtender.SetLocalizableToolTip(this.lblPhoneticWritingSystem, null);
            this.locExtender.SetLocalizationComment(this.lblPhoneticWritingSystem, null);
            this.locExtender.SetLocalizingId(this.lblPhoneticWritingSystem, "DialogBoxes.Fw7DataSourcePropertiesDlg.PhoneticWritingSystemLabel");
            this.lblPhoneticWritingSystem.Location = new System.Drawing.Point(16, 166);
            this.lblPhoneticWritingSystem.Margin = new System.Windows.Forms.Padding(16, 6, 7, 0);
            this.lblPhoneticWritingSystem.Name = "lblPhoneticWritingSystem";
            this.lblPhoneticWritingSystem.Size = new System.Drawing.Size(112, 18);
            this.lblPhoneticWritingSystem.TabIndex = 3;
            this.lblPhoneticWritingSystem.Text = "&Writing System:";
            // 
            // cboPhoneticWritingSystem
            // 
            this.cboPhoneticWritingSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPhoneticWritingSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhoneticWritingSystem.FormattingEnabled = true;
            this.locExtender.SetLocalizableToolTip(this.cboPhoneticWritingSystem, null);
            this.locExtender.SetLocalizationComment(this.cboPhoneticWritingSystem, null);
            this.locExtender.SetLocalizationPriority(this.cboPhoneticWritingSystem, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.cboPhoneticWritingSystem, "Fw7DataSourcePropertiesDlg.cboPhoneticWritingSystem");
            this.cboPhoneticWritingSystem.Location = new System.Drawing.Point(264, 164);
            this.cboPhoneticWritingSystem.Margin = new System.Windows.Forms.Padding(0, 4, 7, 0);
            this.cboPhoneticWritingSystem.Name = "cboPhoneticWritingSystem";
            this.cboPhoneticWritingSystem.Size = new System.Drawing.Size(235, 26);
            this.cboPhoneticWritingSystem.TabIndex = 4;
            // 
            // rbPronunField
            // 
            this.rbPronunField.AutoSize = true;
            this.rbPronunField.BackColor = System.Drawing.Color.Transparent;
            this.rbPronunField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.locExtender.SetLocalizableToolTip(this.rbPronunField, null);
            this.locExtender.SetLocalizationComment(this.rbPronunField, "");
            this.locExtender.SetLocalizingId(this.rbPronunField, "DialogBoxes.Fw7DataSourcePropertiesDlg.PronunciationFieldRadioButton");
            this.rbPronunField.Location = new System.Drawing.Point(16, 87);
            this.rbPronunField.Margin = new System.Windows.Forms.Padding(16, 6, 0, 4);
            this.rbPronunField.Name = "rbPronunField";
            this.rbPronunField.Size = new System.Drawing.Size(248, 22);
            this.rbPronunField.TabIndex = 1;
            this.rbPronunField.TabStop = true;
            this.rbPronunField.Text = "Is stored in the p&ronunciation field";
            this.rbPronunField.UseVisualStyleBackColor = false;
            this.rbPronunField.CheckedChanged += new System.EventHandler(this.HandlePhoneticStorageTypeCheckedChanged);
            // 
            // rbVernForm
            // 
            this.rbVernForm.AutoSize = true;
            this.rbVernForm.BackColor = System.Drawing.Color.Transparent;
            this.rbVernForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.locExtender.SetLocalizableToolTip(this.rbVernForm, null);
            this.locExtender.SetLocalizationComment(this.rbVernForm, null);
            this.locExtender.SetLocalizingId(this.rbVernForm, "DialogBoxes.Fw7DataSourcePropertiesDlg.LexemeFormRadioButton");
            this.rbVernForm.Location = new System.Drawing.Point(16, 6);
            this.rbVernForm.Margin = new System.Windows.Forms.Padding(16, 6, 7, 6);
            this.rbVernForm.Name = "rbVernForm";
            this.rbVernForm.Size = new System.Drawing.Size(232, 22);
            this.rbVernForm.TabIndex = 0;
            this.rbVernForm.TabStop = true;
            this.rbVernForm.Text = "Is stored in the &vernacular form";
            this.rbVernForm.UseVisualStyleBackColor = false;
            this.rbVernForm.CheckedChanged += new System.EventHandler(this.HandlePhoneticStorageTypeCheckedChanged);
            // 
            // cboPronunciationOptions
            // 
            this.cboPronunciationOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPronunciationOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPronunciationOptions.FormattingEnabled = true;
            this.locExtender.SetLocalizableToolTip(this.cboPronunciationOptions, null);
            this.locExtender.SetLocalizationComment(this.cboPronunciationOptions, null);
            this.locExtender.SetLocalizationPriority(this.cboPronunciationOptions, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.cboPronunciationOptions, "Fw7DataSourcePropertiesDlg.cboPronunciationOptions");
            this.cboPronunciationOptions.Location = new System.Drawing.Point(264, 113);
            this.cboPronunciationOptions.Margin = new System.Windows.Forms.Padding(0, 0, 7, 21);
            this.cboPronunciationOptions.Name = "cboPronunciationOptions";
            this.cboPronunciationOptions.Size = new System.Drawing.Size(235, 26);
            this.cboPronunciationOptions.TabIndex = 2;
            // 
            // lblPronunciationOptions
            // 
            this.lblPronunciationOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPronunciationOptions.AutoSize = true;
            this.lblPronunciationOptions.BackColor = System.Drawing.Color.Transparent;
            this.locExtender.SetLocalizableToolTip(this.lblPronunciationOptions, null);
            this.locExtender.SetLocalizationComment(this.lblPronunciationOptions, null);
            this.locExtender.SetLocalizingId(this.lblPronunciationOptions, "DialogBoxes.Fw7DataSourcePropertiesDlg.PronunciationOptionsLabel");
            this.lblPronunciationOptions.Location = new System.Drawing.Point(76, 119);
            this.lblPronunciationOptions.Margin = new System.Windows.Forms.Padding(37, 6, 0, 0);
            this.lblPronunciationOptions.Name = "lblPronunciationOptions";
            this.lblPronunciationOptions.Size = new System.Drawing.Size(188, 18);
            this.lblPronunciationOptions.TabIndex = 0;
            this.lblPronunciationOptions.Text = "Records will be created for ";
            // 
            // cboVernacularOptions
            // 
            this.cboVernacularOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVernacularOptions.FormattingEnabled = true;
            this.locExtender.SetLocalizableToolTip(this.cboVernacularOptions, null);
            this.locExtender.SetLocalizationComment(this.cboVernacularOptions, null);
            this.locExtender.SetLocalizationPriority(this.cboVernacularOptions, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.cboVernacularOptions, "Fw7DataSourcePropertiesDlg.cboVernacularOptions");
            this.cboVernacularOptions.Location = new System.Drawing.Point(264, 34);
            this.cboVernacularOptions.Margin = new System.Windows.Forms.Padding(0, 0, 7, 21);
            this.cboVernacularOptions.Name = "cboVernacularOptions";
            this.cboVernacularOptions.Size = new System.Drawing.Size(235, 26);
            this.cboVernacularOptions.TabIndex = 8;
            this.cboVernacularOptions.SelectedIndexChanged += new System.EventHandler(this.HandleVernacularOptionsChanged);
           
            // 
            // grpFields
            // 
            this.grpFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblLayoutOuter.SetColumnSpan(this.grpFields, 2);
            this.grpFields.Controls.Add(this.pnlGrid);
            this.grpFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.locExtender.SetLocalizableToolTip(this.grpFields, null);
            this.locExtender.SetLocalizationComment(this.grpFields, "");
            this.locExtender.SetLocalizingId(this.grpFields, "DialogBoxes.Fw7DataSourcePropertiesDlg.FieldsGroupBox");
            this.grpFields.Location = new System.Drawing.Point(0, 270);
            this.grpFields.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.grpFields.Name = "grpFields";
            this.grpFields.Padding = new System.Windows.Forms.Padding(13, 9, 13, 12);
            this.grpFields.Size = new System.Drawing.Size(514, 173);
            this.grpFields.TabIndex = 3;
            this.grpFields.TabStop = false;
            this.grpFields.Text = "&Fields";
            // 
            // pnlGrid
            // 
            this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.ClipTextForChildControls = true;
            this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.DoubleBuffered = true;
            this.pnlGrid.DrawOnlyBottomBorder = false;
            this.pnlGrid.DrawOnlyTopBorder = false;
            this.pnlGrid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.pnlGrid.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
            this.locExtender.SetLocalizationComment(this.pnlGrid, null);
            this.locExtender.SetLocalizationPriority(this.pnlGrid, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.pnlGrid, "Fw7DataSourcePropertiesDlg.pnlGrid");
            this.pnlGrid.Location = new System.Drawing.Point(13, 26);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(4);
            this.pnlGrid.MnemonicGeneratesClick = false;
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.PaintExplorerBarBackground = false;
            this.pnlGrid.Size = new System.Drawing.Size(488, 135);
            this.pnlGrid.TabIndex = 0;
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
            // 
            // tblLayoutOuter
            // 
            this.tblLayoutOuter.ColumnCount = 2;
            this.tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutOuter.Controls.Add(this.lblProject, 0, 0);
            this.tblLayoutOuter.Controls.Add(this.lblProjectValue, 1, 0);
            this.tblLayoutOuter.Controls.Add(this.grpPhoneticField, 0, 1);
            this.tblLayoutOuter.Controls.Add(this.grpFields, 0, 2);
            this.tblLayoutOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayoutOuter.Location = new System.Drawing.Point(17, 15);
            this.tblLayoutOuter.Margin = new System.Windows.Forms.Padding(4);
            this.tblLayoutOuter.Name = "tblLayoutOuter";
            this.tblLayoutOuter.RowCount = 3;
            this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutOuter.Size = new System.Drawing.Size(514, 443);
            this.tblLayoutOuter.TabIndex = 0;
            // 
            // Fw7DataSourcePropertiesDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 507);
            this.Controls.Add(this.tblLayoutOuter);
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizingId(this, "DialogBoxes.Fw7DataSourcePropertiesDlg.WindowTitle");
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(394, 451);
            this.Name = "Fw7DataSourcePropertiesDlg";
            this.Padding = new System.Windows.Forms.Padding(17, 15, 17, 0);
            this.Text = "FieldWorks Data Source Properties";
            this.Controls.SetChildIndex(this.tblLayoutOuter, 0);
            this.grpPhoneticField.ResumeLayout(false);
            this.grpPhoneticField.PerformLayout();
            this.tblLayoutPhoneticData.ResumeLayout(false);
            this.tblLayoutPhoneticData.PerformLayout();
            this.grpFields.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this.tblLayoutOuter.ResumeLayout(false);
            this.tblLayoutOuter.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblProjectValue;
		private System.Windows.Forms.Label lblProject;
		private System.Windows.Forms.RadioButton rbPronunField;
		private System.Windows.Forms.RadioButton rbVernForm;
		private System.Windows.Forms.GroupBox grpPhoneticField;
		private System.Windows.Forms.GroupBox grpFields;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel tblLayoutOuter;
		private System.Windows.Forms.TableLayoutPanel tblLayoutPhoneticData;
		private SilTools.Controls.SilPanel pnlGrid;
		private System.Windows.Forms.ComboBox cboPronunciationOptions;
		private System.Windows.Forms.Label lblPhoneticWritingSystem;
		private System.Windows.Forms.ComboBox cboPhoneticWritingSystem;
        private System.Windows.Forms.Label lblPronunciationOptions;
        private System.Windows.Forms.Label lblLexicalOptions;
        private System.Windows.Forms.ComboBox cboVernacularOptions;
	}
}