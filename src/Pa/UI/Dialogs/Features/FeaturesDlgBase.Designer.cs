namespace SIL.Pa.UI.Dialogs
{
	partial class FeaturesDlgBase
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._panelPhoneListHeading = new SilTools.Controls.SilGradientPanel();
			this._gridPhones = new SilTools.SilGrid();
			this._panelFeatures = new SilTools.Controls.SilPanel();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._panelFeaturesHeading = new SilTools.Controls.SilGradientPanel();
			this._tableLayoutDistinctiveFeatureSet = new System.Windows.Forms.TableLayoutPanel();
			this._labelDistinctiveFeaturesSet = new System.Windows.Forms.Label();
			this._labelDistinctiveFeaturesSetValue = new System.Windows.Forms.Label();
			this._labelPhoneDescription = new System.Windows.Forms.Label();
			this._buttonReset = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._splitFeatures.Panel1.SuspendLayout();
			this._splitFeatures.Panel2.SuspendLayout();
			this._splitFeatures.SuspendLayout();
			this._panelPhones.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._gridPhones)).BeginInit();
			this._panelFeatures.SuspendLayout();
			this._tableLayout.SuspendLayout();
			this._panelFeaturesHeading.SuspendLayout();
			this._tableLayoutDistinctiveFeatureSet.SuspendLayout();
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
			this._splitFeatures.Panel2.Controls.Add(this._panelFeatures);
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
			this._panelPhones.Controls.Add(this.tableLayoutPanel1);
			this._panelPhones.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelPhones.DoubleBuffered = true;
			this._panelPhones.DrawOnlyBottomBorder = false;
			this._panelPhones.DrawOnlyTopBorder = false;
			this._panelPhones.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelPhones.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelPhones, null);
			this.locExtender.SetLocalizationComment(this._panelPhones, null);
			this.locExtender.SetLocalizationPriority(this._panelPhones, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._panelPhones, "FeaturesDlg.pnlPhones");
			this._panelPhones.Location = new System.Drawing.Point(0, 0);
			this._panelPhones.MnemonicGeneratesClick = false;
			this._panelPhones.Name = "_panelPhones";
			this._panelPhones.PaintExplorerBarBackground = false;
			this._panelPhones.Size = new System.Drawing.Size(145, 409);
			this._panelPhones.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this._panelPhoneListHeading, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._gridPhones, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(143, 407);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// _panelPhoneListHeading
			// 
			this._panelPhoneListHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelPhoneListHeading.AutoHeight = true;
			this._panelPhoneListHeading.AutoHeightPadding = 8;
			this._panelPhoneListHeading.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelPhoneListHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelPhoneListHeading.ClipTextForChildControls = true;
			this._panelPhoneListHeading.ColorBottom = System.Drawing.Color.Empty;
			this._panelPhoneListHeading.ColorTop = System.Drawing.Color.Empty;
			this._panelPhoneListHeading.ControlReceivingFocusOnMnemonic = this._gridPhones;
			this._panelPhoneListHeading.DoubleBuffered = true;
			this._panelPhoneListHeading.DrawOnlyBottomBorder = true;
			this._panelPhoneListHeading.DrawOnlyTopBorder = false;
			this._panelPhoneListHeading.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelPhoneListHeading.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelPhoneListHeading, null);
			this.locExtender.SetLocalizationComment(this._panelPhoneListHeading, null);
			this.locExtender.SetLocalizingId(this._panelPhoneListHeading, "DialogBoxes.FeaturesDlgBase.PhoneListHeading");
			this._panelPhoneListHeading.Location = new System.Drawing.Point(0, 0);
			this._panelPhoneListHeading.MakeDark = false;
			this._panelPhoneListHeading.Margin = new System.Windows.Forms.Padding(0);
			this._panelPhoneListHeading.MnemonicGeneratesClick = false;
			this._panelPhoneListHeading.Name = "_panelPhoneListHeading";
			this._panelPhoneListHeading.PaintExplorerBarBackground = false;
			this._panelPhoneListHeading.Size = new System.Drawing.Size(143, 23);
			this._panelPhoneListHeading.TabIndex = 0;
			this._panelPhoneListHeading.Text = "&Phone List";
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
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._gridPhones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this._gridPhones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this._gridPhones.Dock = System.Windows.Forms.DockStyle.Fill;
			this._gridPhones.DrawTextBoxEditControlBorder = false;
			this._gridPhones.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._gridPhones.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._gridPhones.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this._gridPhones.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this._gridPhones, null);
			this.locExtender.SetLocalizationComment(this._gridPhones, null);
			this.locExtender.SetLocalizationPriority(this._gridPhones, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._gridPhones, "FeaturesDlg.gridPhones");
			this._gridPhones.Location = new System.Drawing.Point(0, 23);
			this._gridPhones.Margin = new System.Windows.Forms.Padding(0);
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
			this._gridPhones.Size = new System.Drawing.Size(143, 384);
			this._gridPhones.StandardTab = true;
			this._gridPhones.TabIndex = 1;
			this._gridPhones.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this._gridPhones.WaterMark = "!";
			this._gridPhones.CurrentRowChanged += new System.EventHandler(this.HandlePhoneGridCurrentRowChanged);
			this._gridPhones.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandlePhoneGridCellMouseEnter);
			this._gridPhones.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandlePhoneGridCellMouseLeave);
			this._gridPhones.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.HandlePhoneGridCellPainting);
			// 
			// _panelFeatures
			// 
			this._panelFeatures.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelFeatures.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelFeatures.ClipTextForChildControls = true;
			this._panelFeatures.ControlReceivingFocusOnMnemonic = null;
			this._panelFeatures.Controls.Add(this._tableLayout);
			this._panelFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelFeatures.DoubleBuffered = true;
			this._panelFeatures.DrawOnlyBottomBorder = false;
			this._panelFeatures.DrawOnlyTopBorder = false;
			this._panelFeatures.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelFeatures.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelFeatures, null);
			this.locExtender.SetLocalizationComment(this._panelFeatures, null);
			this.locExtender.SetLocalizationPriority(this._panelFeatures, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._panelFeatures, "silPanel1.silPanel1");
			this._panelFeatures.Location = new System.Drawing.Point(0, 0);
			this._panelFeatures.MnemonicGeneratesClick = false;
			this._panelFeatures.Name = "_panelFeatures";
			this._panelFeatures.PaintExplorerBarBackground = false;
			this._panelFeatures.Size = new System.Drawing.Size(446, 409);
			this._panelFeatures.TabIndex = 1;
			// 
			// _tableLayout
			// 
			this._tableLayout.BackColor = System.Drawing.Color.White;
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._panelFeaturesHeading, 0, 0);
			this._tableLayout.Controls.Add(this._labelPhoneDescription, 0, 1);
			this._tableLayout.Controls.Add(this._buttonReset, 0, 2);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.ForeColor = System.Drawing.Color.Black;
			this._tableLayout.Location = new System.Drawing.Point(0, 0);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 3;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 149F));
			this._tableLayout.Size = new System.Drawing.Size(444, 407);
			this._tableLayout.TabIndex = 0;
			this._tableLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.HandlePaintLineUnderDescription);
			// 
			// _panelFeaturesHeading
			// 
			this._panelFeaturesHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelFeaturesHeading.AutoHeight = true;
			this._panelFeaturesHeading.AutoHeightPadding = 8;
			this._panelFeaturesHeading.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelFeaturesHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelFeaturesHeading.ClipTextForChildControls = true;
			this._panelFeaturesHeading.ColorBottom = System.Drawing.Color.Empty;
			this._panelFeaturesHeading.ColorTop = System.Drawing.Color.Empty;
			this._panelFeaturesHeading.ControlReceivingFocusOnMnemonic = null;
			this._panelFeaturesHeading.Controls.Add(this._tableLayoutDistinctiveFeatureSet);
			this._panelFeaturesHeading.DoubleBuffered = true;
			this._panelFeaturesHeading.DrawOnlyBottomBorder = true;
			this._panelFeaturesHeading.DrawOnlyTopBorder = false;
			this._panelFeaturesHeading.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelFeaturesHeading.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelFeaturesHeading, null);
			this.locExtender.SetLocalizationComment(this._panelFeaturesHeading, null);
			this.locExtender.SetLocalizingId(this._panelFeaturesHeading, "DialogBoxes.FeaturesDlgBase.FeaturesListHeading");
			this._panelFeaturesHeading.Location = new System.Drawing.Point(0, 0);
			this._panelFeaturesHeading.MakeDark = false;
			this._panelFeaturesHeading.Margin = new System.Windows.Forms.Padding(0);
			this._panelFeaturesHeading.MnemonicGeneratesClick = false;
			this._panelFeaturesHeading.Name = "_panelFeaturesHeading";
			this._panelFeaturesHeading.PaintExplorerBarBackground = false;
			this._panelFeaturesHeading.Size = new System.Drawing.Size(444, 23);
			this._panelFeaturesHeading.TabIndex = 1;
			this._panelFeaturesHeading.Text = "&Features";
			// 
			// _tableLayoutDistinctiveFeatureSet
			// 
			this._tableLayoutDistinctiveFeatureSet.AutoSize = true;
			this._tableLayoutDistinctiveFeatureSet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutDistinctiveFeatureSet.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutDistinctiveFeatureSet.ColumnCount = 2;
			this._tableLayoutDistinctiveFeatureSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutDistinctiveFeatureSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutDistinctiveFeatureSet.Controls.Add(this._labelDistinctiveFeaturesSet, 0, 0);
			this._tableLayoutDistinctiveFeatureSet.Controls.Add(this._labelDistinctiveFeaturesSetValue, 1, 0);
			this._tableLayoutDistinctiveFeatureSet.Dock = System.Windows.Forms.DockStyle.Right;
			this._tableLayoutDistinctiveFeatureSet.Location = new System.Drawing.Point(347, 0);
			this._tableLayoutDistinctiveFeatureSet.Name = "_tableLayoutDistinctiveFeatureSet";
			this._tableLayoutDistinctiveFeatureSet.RowCount = 1;
			this._tableLayoutDistinctiveFeatureSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutDistinctiveFeatureSet.Size = new System.Drawing.Size(95, 21);
			this._tableLayoutDistinctiveFeatureSet.TabIndex = 1;
			this._tableLayoutDistinctiveFeatureSet.Visible = false;
			// 
			// _labelDistinctiveFeaturesSet
			// 
			this._labelDistinctiveFeaturesSet.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._labelDistinctiveFeaturesSet.AutoSize = true;
			this._labelDistinctiveFeaturesSet.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._labelDistinctiveFeaturesSet, "See project settings to modify feature set");
			this.locExtender.SetLocalizationComment(this._labelDistinctiveFeaturesSet, null);
			this.locExtender.SetLocalizingId(this._labelDistinctiveFeaturesSet, "DialogBoxes.FeaturesDlgBase.DistinctiveFeaturesSetLabel");
			this._labelDistinctiveFeaturesSet.Location = new System.Drawing.Point(5, 3);
			this._labelDistinctiveFeaturesSet.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this._labelDistinctiveFeaturesSet.Name = "_labelDistinctiveFeaturesSet";
			this._labelDistinctiveFeaturesSet.Size = new System.Drawing.Size(68, 15);
			this._labelDistinctiveFeaturesSet.TabIndex = 1;
			this._labelDistinctiveFeaturesSet.Text = "Feature Set:";
			// 
			// _labelDistinctiveFeaturesSetValue
			// 
			this._labelDistinctiveFeaturesSetValue.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._labelDistinctiveFeaturesSetValue.AutoSize = true;
			this._labelDistinctiveFeaturesSetValue.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._labelDistinctiveFeaturesSetValue, null);
			this.locExtender.SetLocalizationComment(this._labelDistinctiveFeaturesSetValue, null);
			this.locExtender.SetLocalizationPriority(this._labelDistinctiveFeaturesSetValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelDistinctiveFeaturesSetValue, "label1.label1");
			this._labelDistinctiveFeaturesSetValue.Location = new System.Drawing.Point(73, 3);
			this._labelDistinctiveFeaturesSetValue.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
			this._labelDistinctiveFeaturesSetValue.Name = "_labelDistinctiveFeaturesSetValue";
			this._labelDistinctiveFeaturesSetValue.Size = new System.Drawing.Size(14, 15);
			this._labelDistinctiveFeaturesSetValue.TabIndex = 0;
			this._labelDistinctiveFeaturesSetValue.Text = "#";
			// 
			// _labelPhoneDescription
			// 
			this._labelPhoneDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelPhoneDescription.AutoSize = true;
			this._labelPhoneDescription.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._labelPhoneDescription, null);
			this.locExtender.SetLocalizationComment(this._labelPhoneDescription, null);
			this.locExtender.SetLocalizationPriority(this._labelPhoneDescription, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelPhoneDescription, "label1.label1");
			this._labelPhoneDescription.Location = new System.Drawing.Point(3, 31);
			this._labelPhoneDescription.Margin = new System.Windows.Forms.Padding(3, 8, 3, 9);
			this._labelPhoneDescription.Name = "_labelPhoneDescription";
			this._labelPhoneDescription.Size = new System.Drawing.Size(438, 15);
			this._labelPhoneDescription.TabIndex = 0;
			this._labelPhoneDescription.Text = "#";
			// 
			// _buttonReset
			// 
			this._buttonReset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonReset.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonReset, "Reset Features of Selected Phone");
			this.locExtender.SetLocalizationComment(this._buttonReset, null);
			this.locExtender.SetLocalizingId(this._buttonReset, "DialogBoxes.FeaturesDlgBase.ResetButton");
			this._buttonReset.Location = new System.Drawing.Point(0, 218);
			this._buttonReset.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
			this._buttonReset.MinimumSize = new System.Drawing.Size(80, 26);
			this._buttonReset.Name = "_buttonReset";
			this._buttonReset.Size = new System.Drawing.Size(80, 26);
			this._buttonReset.TabIndex = 1;
			this._buttonReset.Text = "Reset";
			this._buttonReset.UseVisualStyleBackColor = true;
			this._buttonReset.Click += new System.EventHandler(this.HandleResetButtonClick);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// FeaturesDlgBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(617, 459);
			this.Controls.Add(this._splitFeatures);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "FeaturesDlg.WindowTitle");
			this.Name = "FeaturesDlgBase";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Features";
			this.Controls.SetChildIndex(this._splitFeatures, 0);
			this._splitFeatures.Panel1.ResumeLayout(false);
			this._splitFeatures.Panel2.ResumeLayout(false);
			this._splitFeatures.ResumeLayout(false);
			this._panelPhones.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._gridPhones)).EndInit();
			this._panelFeatures.ResumeLayout(false);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._panelFeaturesHeading.ResumeLayout(false);
			this._panelFeaturesHeading.PerformLayout();
			this._tableLayoutDistinctiveFeatureSet.ResumeLayout(false);
			this._tableLayoutDistinctiveFeatureSet.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilTools.Controls.SilPanel _panelPhones;
		private SilTools.Controls.SilGradientPanel _panelPhoneListHeading;
		private System.Windows.Forms.Button _buttonReset;
		private SilTools.Controls.SilPanel _panelFeatures;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Label _labelPhoneDescription;
		private SilTools.Controls.SilGradientPanel _panelFeaturesHeading;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _labelDistinctiveFeaturesSetValue;
		private Localization.UI.LocalizationExtender locExtender;
		protected SilTools.SilGrid _gridPhones;
		protected System.Windows.Forms.SplitContainer _splitFeatures;
		private System.Windows.Forms.Label _labelDistinctiveFeaturesSet;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutDistinctiveFeatureSet;
	}
}