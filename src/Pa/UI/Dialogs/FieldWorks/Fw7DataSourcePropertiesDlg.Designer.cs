namespace SIL.Pa.UI.Dialogs
{
	partial class Fw7DataSourcePropertiesDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblProjectValue = new System.Windows.Forms.Label();
			this.lblProject = new System.Windows.Forms.Label();
			this.grpPhoneticDataStoreType = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.rbPronunField = new System.Windows.Forms.RadioButton();
			this.rbLexForm = new System.Windows.Forms.RadioButton();
			this.grpWritingSystems = new System.Windows.Forms.GroupBox();
			this.m_grid = new SilTools.SilGrid();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.grpPhoneticDataStoreType.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpWritingSystems.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
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
			this.locExtender.SetLocalizationPriority(this.lblProjectValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblProjectValue, "FwDataSourcePropertiesDlg.lblProjectValue");
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
			this.locExtender.SetLocalizingId(this.lblProject, "Fw7DataSourcePropertiesDlg.lblProject");
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
			this.tableLayoutPanel1.SetColumnSpan(this.grpPhoneticDataStoreType, 2);
			this.grpPhoneticDataStoreType.Controls.Add(this.tableLayoutPanel2);
			this.grpPhoneticDataStoreType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.grpPhoneticDataStoreType, null);
			this.locExtender.SetLocalizationComment(this.grpPhoneticDataStoreType, "Text of frame around radio buttons on FieldWorks data source properties dialog bo" +
					"x.");
			this.locExtender.SetLocalizingId(this.grpPhoneticDataStoreType, "Fw7DataSourcePropertiesDlg.grpPhoneticDataStoreType");
			this.grpPhoneticDataStoreType.Location = new System.Drawing.Point(0, 280);
			this.grpPhoneticDataStoreType.Margin = new System.Windows.Forms.Padding(0, 3, 0, 5);
			this.grpPhoneticDataStoreType.Name = "grpPhoneticDataStoreType";
			this.grpPhoneticDataStoreType.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grpPhoneticDataStoreType.Size = new System.Drawing.Size(327, 80);
			this.grpPhoneticDataStoreType.TabIndex = 3;
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
			this.tableLayoutPanel2.Size = new System.Drawing.Size(321, 63);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// rbPronunField
			// 
			this.rbPronunField.AutoSize = true;
			this.rbPronunField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.rbPronunField, null);
			this.locExtender.SetLocalizationComment(this.rbPronunField, "Radio button text on FieldWorks data source properties dialog box.");
			this.locExtender.SetLocalizingId(this.rbPronunField, "Fw7DataSourcePropertiesDlg.rbPronunField");
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
			this.locExtender.SetLocalizingId(this.rbLexForm, "Fw7DataSourcePropertiesDlg.rbLexForm");
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
			this.tableLayoutPanel1.SetColumnSpan(this.grpWritingSystems, 2);
			this.grpWritingSystems.Controls.Add(this.m_grid);
			this.grpWritingSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.grpWritingSystems, null);
			this.locExtender.SetLocalizationComment(this.grpWritingSystems, "Text of frame around the list of writing systems on FieldWorks data source proper" +
					"ties dialog box.");
			this.locExtender.SetLocalizingId(this.grpWritingSystems, "Fw7DataSourcePropertiesDlg.grpWritingSystems");
			this.grpWritingSystems.Location = new System.Drawing.Point(0, 30);
			this.grpWritingSystems.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this.grpWritingSystems.Name = "grpWritingSystems";
			this.grpWritingSystems.Padding = new System.Windows.Forms.Padding(10, 7, 10, 10);
			this.grpWritingSystems.Size = new System.Drawing.Size(327, 247);
			this.grpWritingSystems.TabIndex = 2;
			this.grpWritingSystems.TabStop = false;
			this.grpWritingSystems.Text = "&Writing Systems";
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
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grid.DrawTextBoxEditControlBorder = false;
			this.m_grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.m_grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_grid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_grid, null);
			this.locExtender.SetLocalizationComment(this.m_grid, null);
			this.locExtender.SetLocalizationPriority(this.m_grid, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_grid, "FwDataSourcePropertiesDlg.m_grid");
			this.m_grid.Location = new System.Drawing.Point(10, 21);
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.PaintHeaderAcrossFullGridWidth = true;
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersVisible = false;
			this.m_grid.RowHeadersWidth = 22;
			this.m_grid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.Size = new System.Drawing.Size(307, 216);
			this.m_grid.TabIndex = 0;
			this.m_grid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_grid.WaterMark = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidDoNothingToolTip;
			this.m_grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellEnter);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dailog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.grpPhoneticDataStoreType, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblProject, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.grpWritingSystems, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblProjectValue, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(327, 365);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// Fw7DataSourcePropertiesDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(347, 415);
			this.Controls.Add(this.tableLayoutPanel1);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "FwDataSourcePropertiesDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(300, 375);
			this.Name = "Fw7DataSourcePropertiesDlg";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Auto;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FieldWorks Data Source Properties";
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.grpPhoneticDataStoreType.ResumeLayout(false);
			this.grpPhoneticDataStoreType.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.grpWritingSystems.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblProjectValue;
		private System.Windows.Forms.Label lblProject;
		private System.Windows.Forms.RadioButton rbPronunField;
		private System.Windows.Forms.RadioButton rbLexForm;
		private System.Windows.Forms.GroupBox grpPhoneticDataStoreType;
		private System.Windows.Forms.GroupBox grpWritingSystems;
		private SilTools.SilGrid m_grid;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
	}
}