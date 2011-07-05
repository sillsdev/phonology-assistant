namespace SIL.Pa.UI.Dialogs
{
	partial class FeaturesDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitFeatures = new System.Windows.Forms.SplitContainer();
			this.pnlPhones = new SilTools.Controls.SilPanel();
			this.btnReset = new System.Windows.Forms.Button();
			this.gridPhones = new SilTools.SilGrid();
			this.pgpPhoneList = new SilTools.Controls.SilGradientPanel();
			this._featuresTab = new SIL.Pa.UI.Controls.FeaturesTab();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.splitFeatures.Panel1.SuspendLayout();
			this.splitFeatures.Panel2.SuspendLayout();
			this.splitFeatures.SuspendLayout();
			this.pnlPhones.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridPhones)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// splitFeatures
			// 
			this.splitFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitFeatures.Location = new System.Drawing.Point(10, 10);
			this.splitFeatures.Name = "splitFeatures";
			// 
			// splitFeatures.Panel1
			// 
			this.splitFeatures.Panel1.Controls.Add(this.pnlPhones);
			this.splitFeatures.Panel1MinSize = 100;
			// 
			// splitFeatures.Panel2
			// 
			this.splitFeatures.Panel2.Controls.Add(this._featuresTab);
			this.splitFeatures.Size = new System.Drawing.Size(597, 409);
			this.splitFeatures.SplitterDistance = 145;
			this.splitFeatures.SplitterWidth = 6;
			this.splitFeatures.TabIndex = 0;
			this.splitFeatures.TabStop = false;
			// 
			// pnlPhones
			// 
			this.pnlPhones.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlPhones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlPhones.ClipTextForChildControls = true;
			this.pnlPhones.ControlReceivingFocusOnMnemonic = null;
			this.pnlPhones.Controls.Add(this.btnReset);
			this.pnlPhones.Controls.Add(this.gridPhones);
			this.pnlPhones.Controls.Add(this.pgpPhoneList);
			this.pnlPhones.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPhones.DoubleBuffered = true;
			this.pnlPhones.DrawOnlyBottomBorder = false;
			this.pnlPhones.DrawOnlyTopBorder = false;
			this.pnlPhones.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlPhones.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlPhones, null);
			this.locExtender.SetLocalizationComment(this.pnlPhones, null);
			this.locExtender.SetLocalizingId(this.pnlPhones, "FeaturesDlg.pnlPhones");
			this.pnlPhones.Location = new System.Drawing.Point(0, 0);
			this.pnlPhones.MnemonicGeneratesClick = false;
			this.pnlPhones.Name = "pnlPhones";
			this.pnlPhones.PaintExplorerBarBackground = false;
			this.pnlPhones.Size = new System.Drawing.Size(145, 409);
			this.pnlPhones.TabIndex = 0;
			// 
			// btnReset
			// 
			this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnReset.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.btnReset, "Reset Features of Selected Phone");
			this.locExtender.SetLocalizationComment(this.btnReset, null);
			this.locExtender.SetLocalizingId(this.btnReset, "FeaturesDlg.btnReset");
			this.btnReset.Location = new System.Drawing.Point(37, 277);
			this.btnReset.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(80, 26);
			this.btnReset.TabIndex = 1;
			this.btnReset.Text = "Reset";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// gridPhones
			// 
			this.gridPhones.AllowUserToAddRows = false;
			this.gridPhones.AllowUserToDeleteRows = false;
			this.gridPhones.AllowUserToOrderColumns = true;
			this.gridPhones.AllowUserToResizeRows = false;
			this.gridPhones.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.gridPhones.BackgroundColor = System.Drawing.SystemColors.Window;
			this.gridPhones.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.gridPhones.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.gridPhones.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridPhones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridPhones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridPhones.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridPhones.DrawTextBoxEditControlBorder = false;
			this.gridPhones.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.gridPhones.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.gridPhones.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.gridPhones.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.gridPhones, null);
			this.locExtender.SetLocalizationComment(this.gridPhones, null);
			this.locExtender.SetLocalizingId(this.gridPhones, "FeaturesDlg.gridPhones");
			this.gridPhones.Location = new System.Drawing.Point(0, 25);
			this.gridPhones.MultiSelect = false;
			this.gridPhones.Name = "gridPhones";
			this.gridPhones.PaintHeaderAcrossFullGridWidth = true;
			this.gridPhones.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.gridPhones.RowHeadersVisible = false;
			this.gridPhones.RowHeadersWidth = 22;
			this.gridPhones.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.gridPhones.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.gridPhones.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.gridPhones.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.gridPhones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridPhones.ShowWaterMarkWhenDirty = true;
			this.gridPhones.Size = new System.Drawing.Size(143, 382);
			this.gridPhones.StandardTab = true;
			this.gridPhones.TabIndex = 1;
			this.gridPhones.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.gridPhones.WaterMark = "!";
			this.gridPhones.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandlePhoneGridCellMouseEnter);
			this.gridPhones.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandlePhoneGridCellMouseLeave);
			this.gridPhones.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandlePhoneGridRowEnter);
			// 
			// pgpPhoneList
			// 
			this.pgpPhoneList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pgpPhoneList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pgpPhoneList.ClipTextForChildControls = true;
			this.pgpPhoneList.ColorBottom = System.Drawing.Color.Empty;
			this.pgpPhoneList.ColorTop = System.Drawing.Color.Empty;
			this.pgpPhoneList.ControlReceivingFocusOnMnemonic = this.gridPhones;
			this.pgpPhoneList.Dock = System.Windows.Forms.DockStyle.Top;
			this.pgpPhoneList.DoubleBuffered = false;
			this.pgpPhoneList.DrawOnlyBottomBorder = true;
			this.pgpPhoneList.DrawOnlyTopBorder = false;
			this.pgpPhoneList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pgpPhoneList.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pgpPhoneList, null);
			this.locExtender.SetLocalizationComment(this.pgpPhoneList, null);
			this.locExtender.SetLocalizingId(this.pgpPhoneList, "FeaturesDlg.pgpPhoneList");
			this.pgpPhoneList.Location = new System.Drawing.Point(0, 0);
			this.pgpPhoneList.MakeDark = false;
			this.pgpPhoneList.MnemonicGeneratesClick = false;
			this.pgpPhoneList.Name = "pgpPhoneList";
			this.pgpPhoneList.PaintExplorerBarBackground = false;
			this.pgpPhoneList.Size = new System.Drawing.Size(143, 25);
			this.pgpPhoneList.TabIndex = 0;
			this.pgpPhoneList.Text = "&Phone List";
			// 
			// _featuresTab
			// 
			this._featuresTab.BackColor = System.Drawing.Color.Transparent;
			this._featuresTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._featuresTab, null);
			this.locExtender.SetLocalizationComment(this._featuresTab, null);
			this.locExtender.SetLocalizingId(this._featuresTab, "featuresTab1.FeaturesTab");
			this._featuresTab.Location = new System.Drawing.Point(0, 0);
			this._featuresTab.Name = "_featuresTab";
			this._featuresTab.Size = new System.Drawing.Size(446, 409);
			this._featuresTab.TabIndex = 0;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// FeaturesDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(617, 459);
			this.Controls.Add(this.splitFeatures);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "FeaturesDlg.WindowTitle");
			this.Name = "FeaturesDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Features";
			this.Controls.SetChildIndex(this.splitFeatures, 0);
			this.splitFeatures.Panel1.ResumeLayout(false);
			this.splitFeatures.Panel2.ResumeLayout(false);
			this.splitFeatures.ResumeLayout(false);
			this.pnlPhones.ResumeLayout(false);
			this.pnlPhones.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridPhones)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitFeatures;
		private SilTools.Controls.SilPanel pnlPhones;
		private SilTools.SilGrid gridPhones;
		private SilTools.Controls.SilGradientPanel pgpPhoneList;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Button btnReset;
		private Controls.FeaturesTab _featuresTab;
	}
}