namespace SIL.Pa.UI.Dialogs
{
	partial class FwDataSourcePropertiesDlg
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
			this.lblProjectValue = new System.Windows.Forms.Label();
			this.lblProject = new System.Windows.Forms.Label();
			this.grpPhoneticDataStoreType = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.rbPronunField = new System.Windows.Forms.RadioButton();
			this.rbLexForm = new System.Windows.Forms.RadioButton();
			this.grpWritingSystems = new System.Windows.Forms.GroupBox();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.m_tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.pnlGrid = new SilTools.Controls.SilPanel();
			this.grpPhoneticDataStoreType.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpWritingSystems.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.m_tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblProjectValue
			// 
			this.lblProjectValue.AutoEllipsis = true;
			this.lblProjectValue.AutoSize = true;
			this.lblProjectValue.BackColor = System.Drawing.Color.Transparent;
			this.lblProjectValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.lblProjectValue, null);
			this.locExtender.SetLocalizationComment(this.lblProjectValue, null);
			this.locExtender.SetLocalizationPriority(this.lblProjectValue, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblProjectValue, "DialogBoxes.Fw6DataSourcePropertiesDlg.ProjectValueLabel");
			this.lblProjectValue.Location = new System.Drawing.Point(60, 3);
			this.lblProjectValue.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.lblProjectValue.Name = "lblProjectValue";
			this.lblProjectValue.Size = new System.Drawing.Size(14, 15);
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
			this.locExtender.SetLocalizingId(this.lblProject, "DialogBoxes.Fw6DataSourcePropertiesDlg.ProjectLabel");
			this.lblProject.Location = new System.Drawing.Point(7, 3);
			this.lblProject.Margin = new System.Windows.Forms.Padding(7, 3, 5, 0);
			this.lblProject.Name = "lblProject";
			this.lblProject.Size = new System.Drawing.Size(48, 15);
			this.lblProject.TabIndex = 0;
			this.lblProject.Text = "Project:";
			// 
			// grpPhoneticDataStoreType
			// 
			this.grpPhoneticDataStoreType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpPhoneticDataStoreType.AutoSize = true;
			this.m_tableLayout.SetColumnSpan(this.grpPhoneticDataStoreType, 2);
			this.grpPhoneticDataStoreType.Controls.Add(this.tableLayoutPanel2);
			this.grpPhoneticDataStoreType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.grpPhoneticDataStoreType, null);
			this.locExtender.SetLocalizationComment(this.grpPhoneticDataStoreType, "Text of frame around radio buttons on FieldWorks data source properties dialog bo" +
					"x.");
			this.locExtender.SetLocalizingId(this.grpPhoneticDataStoreType, "DialogBoxes.Fw6DataSourcePropertiesDlg.PhoneticDataStoreTypeGroupBox");
			this.grpPhoneticDataStoreType.Location = new System.Drawing.Point(0, 30);
			this.grpPhoneticDataStoreType.Margin = new System.Windows.Forms.Padding(0, 12, 0, 5);
			this.grpPhoneticDataStoreType.Name = "grpPhoneticDataStoreType";
			this.grpPhoneticDataStoreType.Padding = new System.Windows.Forms.Padding(3, 3, 3, 4);
			this.grpPhoneticDataStoreType.Size = new System.Drawing.Size(367, 84);
			this.grpPhoneticDataStoreType.TabIndex = 2;
			this.grpPhoneticDataStoreType.TabStop = false;
			this.grpPhoneticDataStoreType.Text = "Where is Phonetic Data Stored?";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.rbPronunField, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.rbLexForm, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(361, 63);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// rbPronunField
			// 
			this.rbPronunField.AutoSize = true;
			this.rbPronunField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.rbPronunField, null);
			this.locExtender.SetLocalizationComment(this.rbPronunField, "Radio button text on FieldWorks data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.rbPronunField, "DialogBoxes.Fw6DataSourcePropertiesDlg.PronunciationFieldRadioButton");
			this.rbPronunField.Location = new System.Drawing.Point(12, 34);
			this.rbPronunField.Margin = new System.Windows.Forms.Padding(12, 5, 5, 10);
			this.rbPronunField.Name = "rbPronunField";
			this.rbPronunField.Size = new System.Drawing.Size(159, 19);
			this.rbPronunField.TabIndex = 1;
			this.rbPronunField.TabStop = true;
			this.rbPronunField.Text = "In the p&ronunciation field";
			this.rbPronunField.UseVisualStyleBackColor = true;
			this.rbPronunField.CheckedChanged += new System.EventHandler(this.HandlePhoneticStorageTypeCheckedChanged);
			// 
			// rbLexForm
			// 
			this.rbLexForm.AutoSize = true;
			this.rbLexForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.rbLexForm, null);
			this.locExtender.SetLocalizationComment(this.rbLexForm, "Radiobutton text on FieldWorks data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.rbLexForm, "DialogBoxes.Fw6DataSourcePropertiesDlg.LexemeFormRadioButton");
			this.rbLexForm.Location = new System.Drawing.Point(12, 5);
			this.rbLexForm.Margin = new System.Windows.Forms.Padding(12, 5, 5, 5);
			this.rbLexForm.Name = "rbLexForm";
			this.rbLexForm.Size = new System.Drawing.Size(127, 19);
			this.rbLexForm.TabIndex = 0;
			this.rbLexForm.TabStop = true;
			this.rbLexForm.Text = "In the le&xeme form";
			this.rbLexForm.UseVisualStyleBackColor = true;
			this.rbLexForm.CheckedChanged += new System.EventHandler(this.HandlePhoneticStorageTypeCheckedChanged);
			// 
			// grpWritingSystems
			// 
			this.grpWritingSystems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_tableLayout.SetColumnSpan(this.grpWritingSystems, 2);
			this.grpWritingSystems.Controls.Add(this.pnlGrid);
			this.grpWritingSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.grpWritingSystems, null);
			this.locExtender.SetLocalizationComment(this.grpWritingSystems, "Text of frame around the list of writing systems on FieldWorks data source proper" +
					"ties dialog box.");
			this.locExtender.SetLocalizingId(this.grpWritingSystems, "DialogBoxes.Fw6DataSourcePropertiesDlg.WritingSystemsGroupBox");
			this.grpWritingSystems.Location = new System.Drawing.Point(0, 122);
			this.grpWritingSystems.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.grpWritingSystems.Name = "grpWritingSystems";
			this.grpWritingSystems.Padding = new System.Windows.Forms.Padding(10, 7, 10, 10);
			this.grpWritingSystems.Size = new System.Drawing.Size(367, 263);
			this.grpWritingSystems.TabIndex = 3;
			this.grpWritingSystems.TabStop = false;
			this.grpWritingSystems.Text = "&Writing Systems";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// m_tableLayout
			// 
			this.m_tableLayout.ColumnCount = 2;
			this.m_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_tableLayout.Controls.Add(this.grpPhoneticDataStoreType, 0, 1);
			this.m_tableLayout.Controls.Add(this.lblProject, 0, 0);
			this.m_tableLayout.Controls.Add(this.grpWritingSystems, 0, 1);
			this.m_tableLayout.Controls.Add(this.lblProjectValue, 1, 0);
			this.m_tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tableLayout.Location = new System.Drawing.Point(10, 10);
			this.m_tableLayout.Name = "m_tableLayout";
			this.m_tableLayout.RowCount = 3;
			this.m_tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_tableLayout.Size = new System.Drawing.Size(367, 385);
			this.m_tableLayout.TabIndex = 0;
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
			this.pnlGrid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlGrid.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
			this.locExtender.SetLocalizationComment(this.pnlGrid, null);
			this.locExtender.SetLocalizationPriority(this.pnlGrid, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlGrid, "FwDataSourcePropertiesDlg.pnlGrid");
			this.pnlGrid.Location = new System.Drawing.Point(10, 21);
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			this.pnlGrid.Size = new System.Drawing.Size(347, 232);
			this.pnlGrid.TabIndex = 0;
			// 
			// FwDataSourcePropertiesDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(387, 435);
			this.Controls.Add(this.m_tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Fw6DataSourcePropertiesDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(300, 375);
			this.Name = "FwDataSourcePropertiesDlg";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Auto;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FieldWorks Data Source Properties";
			this.Controls.SetChildIndex(this.m_tableLayout, 0);
			this.grpPhoneticDataStoreType.ResumeLayout(false);
			this.grpPhoneticDataStoreType.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.grpWritingSystems.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.m_tableLayout.ResumeLayout(false);
			this.m_tableLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblProjectValue;
		private System.Windows.Forms.Label lblProject;
		private System.Windows.Forms.RadioButton rbPronunField;
		private System.Windows.Forms.RadioButton rbLexForm;
		private System.Windows.Forms.GroupBox grpPhoneticDataStoreType;
		private System.Windows.Forms.GroupBox grpWritingSystems;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel m_tableLayout;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SilTools.Controls.SilPanel pnlGrid;
	}
}