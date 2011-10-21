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
			this._splitFeatures = new System.Windows.Forms.SplitContainer();
			this._panelPhones = new SilTools.Controls.SilPanel();
			this._buttonReset = new System.Windows.Forms.Button();
			this._gridPhones = new SilTools.SilGrid();
			this._panelPhoneList = new SilTools.Controls.SilGradientPanel();
			this._featuresTab = new SIL.Pa.UI.Controls.FeaturesTab();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._splitFeatures.Panel1.SuspendLayout();
			this._splitFeatures.Panel2.SuspendLayout();
			this._splitFeatures.SuspendLayout();
			this._panelPhones.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._gridPhones)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _splitFeatures
			// 
			this._splitFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
			this._splitFeatures.Location = new System.Drawing.Point(10, 10);
			this._splitFeatures.Name = "_splitFeatures";
			// 
			// _splitFeatures.Panel1
			// 
			this._splitFeatures.Panel1.Controls.Add(this._panelPhones);
			this._splitFeatures.Panel1MinSize = 100;
			// 
			// _splitFeatures.Panel2
			// 
			this._splitFeatures.Panel2.Controls.Add(this._featuresTab);
			this._splitFeatures.Size = new System.Drawing.Size(597, 409);
			this._splitFeatures.SplitterDistance = 145;
			this._splitFeatures.SplitterWidth = 6;
			this._splitFeatures.TabIndex = 0;
			this._splitFeatures.TabStop = false;
			// 
			// _panelPhones
			// 
			this._panelPhones.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelPhones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelPhones.ClipTextForChildControls = true;
			this._panelPhones.ControlReceivingFocusOnMnemonic = null;
			this._panelPhones.Controls.Add(this._buttonReset);
			this._panelPhones.Controls.Add(this._gridPhones);
			this._panelPhones.Controls.Add(this._panelPhoneList);
			this._panelPhones.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelPhones.DoubleBuffered = true;
			this._panelPhones.DrawOnlyBottomBorder = false;
			this._panelPhones.DrawOnlyTopBorder = false;
			this._panelPhones.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelPhones.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelPhones, null);
			this.locExtender.SetLocalizationComment(this._panelPhones, null);
			this.locExtender.SetLocalizingId(this._panelPhones, "FeaturesDlg.pnlPhones");
			this._panelPhones.Location = new System.Drawing.Point(0, 0);
			this._panelPhones.MnemonicGeneratesClick = false;
			this._panelPhones.Name = "_panelPhones";
			this._panelPhones.PaintExplorerBarBackground = false;
			this._panelPhones.Size = new System.Drawing.Size(145, 409);
			this._panelPhones.TabIndex = 0;
			// 
			// _buttonReset
			// 
			this._buttonReset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonReset.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonReset, "Reset Features of Selected Phone");
			this.locExtender.SetLocalizationComment(this._buttonReset, null);
			this.locExtender.SetLocalizingId(this._buttonReset, "FeaturesDlg.btnReset");
			this._buttonReset.Location = new System.Drawing.Point(37, 277);
			this._buttonReset.MinimumSize = new System.Drawing.Size(80, 26);
			this._buttonReset.Name = "_buttonReset";
			this._buttonReset.Size = new System.Drawing.Size(80, 26);
			this._buttonReset.TabIndex = 1;
			this._buttonReset.Text = "Reset";
			this._buttonReset.UseVisualStyleBackColor = true;
			this._buttonReset.Click += new System.EventHandler(this.HandleResetButtonClick);
			// 
			// _gridPhones
			// 
			this._gridPhones.AllowUserToAddRows = false;
			this._gridPhones.AllowUserToDeleteRows = false;
			this._gridPhones.AllowUserToOrderColumns = true;
			this._gridPhones.AllowUserToResizeRows = false;
			this._gridPhones.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._gridPhones.BackgroundColor = System.Drawing.SystemColors.Window;
			this._gridPhones.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._gridPhones.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this._gridPhones.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._gridPhones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this._gridPhones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._gridPhones.Dock = System.Windows.Forms.DockStyle.Fill;
			this._gridPhones.DrawTextBoxEditControlBorder = false;
			this._gridPhones.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._gridPhones.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._gridPhones.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this._gridPhones.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this._gridPhones, null);
			this.locExtender.SetLocalizationComment(this._gridPhones, null);
			this.locExtender.SetLocalizingId(this._gridPhones, "FeaturesDlg.gridPhones");
			this._gridPhones.Location = new System.Drawing.Point(0, 25);
			this._gridPhones.MultiSelect = false;
			this._gridPhones.Name = "_gridPhones";
			this._gridPhones.PaintHeaderAcrossFullGridWidth = true;
			this._gridPhones.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._gridPhones.RowHeadersVisible = false;
			this._gridPhones.RowHeadersWidth = 22;
			this._gridPhones.SelectedCellBackColor = System.Drawing.Color.Empty;
			this._gridPhones.SelectedCellForeColor = System.Drawing.Color.Empty;
			this._gridPhones.SelectedRowBackColor = System.Drawing.Color.Empty;
			this._gridPhones.SelectedRowForeColor = System.Drawing.Color.Empty;
			this._gridPhones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._gridPhones.ShowWaterMarkWhenDirty = true;
			this._gridPhones.Size = new System.Drawing.Size(143, 382);
			this._gridPhones.StandardTab = true;
			this._gridPhones.TabIndex = 1;
			this._gridPhones.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this._gridPhones.WaterMark = "!";
			this._gridPhones.CurrentRowChanged += new System.EventHandler(this.HandlePhoneGridCurrentRowChanged);
			this._gridPhones.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.HandlePhoneGridCellFormatting);
			this._gridPhones.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandlePhoneGridCellMouseEnter);
			this._gridPhones.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandlePhoneGridCellMouseLeave);
			this._gridPhones.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandlePhoneGridRowEnter);
			// 
			// _panelPhoneList
			// 
			this._panelPhoneList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelPhoneList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelPhoneList.ClipTextForChildControls = true;
			this._panelPhoneList.ColorBottom = System.Drawing.Color.Empty;
			this._panelPhoneList.ColorTop = System.Drawing.Color.Empty;
			this._panelPhoneList.ControlReceivingFocusOnMnemonic = this._gridPhones;
			this._panelPhoneList.Dock = System.Windows.Forms.DockStyle.Top;
			this._panelPhoneList.DoubleBuffered = false;
			this._panelPhoneList.DrawOnlyBottomBorder = true;
			this._panelPhoneList.DrawOnlyTopBorder = false;
			this._panelPhoneList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelPhoneList.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelPhoneList, null);
			this.locExtender.SetLocalizationComment(this._panelPhoneList, null);
			this.locExtender.SetLocalizingId(this._panelPhoneList, "FeaturesDlg.pgpPhoneList");
			this._panelPhoneList.Location = new System.Drawing.Point(0, 0);
			this._panelPhoneList.MakeDark = false;
			this._panelPhoneList.MnemonicGeneratesClick = false;
			this._panelPhoneList.Name = "_panelPhoneList";
			this._panelPhoneList.PaintExplorerBarBackground = false;
			this._panelPhoneList.Size = new System.Drawing.Size(143, 25);
			this._panelPhoneList.TabIndex = 0;
			this._panelPhoneList.Text = "&Phone List";
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
			this.Controls.Add(this._splitFeatures);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "FeaturesDlg.WindowTitle");
			this.Name = "FeaturesDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Features";
			this.Controls.SetChildIndex(this._splitFeatures, 0);
			this._splitFeatures.Panel1.ResumeLayout(false);
			this._splitFeatures.Panel2.ResumeLayout(false);
			this._splitFeatures.ResumeLayout(false);
			this._panelPhones.ResumeLayout(false);
			this._panelPhones.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._gridPhones)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer _splitFeatures;
		private SilTools.Controls.SilPanel _panelPhones;
		private SilTools.SilGrid _gridPhones;
		private SilTools.Controls.SilGradientPanel _panelPhoneList;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Button _buttonReset;
		private Controls.FeaturesTab _featuresTab;
	}
}