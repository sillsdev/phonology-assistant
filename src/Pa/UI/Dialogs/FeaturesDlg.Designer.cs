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
			this.gridPhones = new SilTools.SilGrid();
			this.pgpPhoneList = new SilTools.Controls.SilGradientPanel();
			this.tabFeatures = new System.Windows.Forms.TabControl();
			this.tpgAFeatures = new System.Windows.Forms.TabPage();
			this.btnReset = new System.Windows.Forms.Button();
			this.tblLayoutAFeatures = new System.Windows.Forms.TableLayoutPanel();
			this.lblAFeatures = new System.Windows.Forms.Label();
			this.tpgBFeatures = new System.Windows.Forms.TabPage();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.splitFeatures.Panel1.SuspendLayout();
			this.splitFeatures.Panel2.SuspendLayout();
			this.splitFeatures.SuspendLayout();
			this.pnlPhones.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridPhones)).BeginInit();
			this.tabFeatures.SuspendLayout();
			this.tpgAFeatures.SuspendLayout();
			this.tblLayoutAFeatures.SuspendLayout();
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
			this.splitFeatures.Panel2.Controls.Add(this.tabFeatures);
			this.splitFeatures.Size = new System.Drawing.Size(597, 409);
			this.splitFeatures.SplitterDistance = 145;
			this.splitFeatures.TabIndex = 0;
			this.splitFeatures.TabStop = false;
			// 
			// pnlPhones
			// 
			this.pnlPhones.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlPhones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlPhones.ClipTextForChildControls = true;
			this.pnlPhones.ControlReceivingFocusOnMnemonic = null;
			this.pnlPhones.Controls.Add(this.gridPhones);
			this.pnlPhones.Controls.Add(this.pgpPhoneList);
			this.pnlPhones.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPhones.DoubleBuffered = true;
			this.pnlPhones.DrawOnlyBottomBorder = false;
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
			this.pgpPhoneList.ClipTextForChildControls = true;
			this.pgpPhoneList.ColorBottom = System.Drawing.Color.Empty;
			this.pgpPhoneList.ColorTop = System.Drawing.Color.Empty;
			this.pgpPhoneList.ControlReceivingFocusOnMnemonic = this.gridPhones;
			this.pgpPhoneList.Dock = System.Windows.Forms.DockStyle.Top;
			this.pgpPhoneList.DoubleBuffered = false;
			this.pgpPhoneList.DrawOnlyBottomBorder = true;
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
			// tabFeatures
			// 
			this.tabFeatures.Controls.Add(this.tpgAFeatures);
			this.tabFeatures.Controls.Add(this.tpgBFeatures);
			this.tabFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabFeatures.Location = new System.Drawing.Point(0, 0);
			this.tabFeatures.Name = "tabFeatures";
			this.tabFeatures.SelectedIndex = 0;
			this.tabFeatures.Size = new System.Drawing.Size(448, 409);
			this.tabFeatures.TabIndex = 0;
			this.tabFeatures.SizeChanged += new System.EventHandler(this.tabFeatures_SizeChanged);
			// 
			// tpgAFeatures
			// 
			this.tpgAFeatures.Controls.Add(this.btnReset);
			this.tpgAFeatures.Controls.Add(this.tblLayoutAFeatures);
			this.locExtender.SetLocalizableToolTip(this.tpgAFeatures, null);
			this.locExtender.SetLocalizationComment(this.tpgAFeatures, null);
			this.locExtender.SetLocalizingId(this.tpgAFeatures, "FeaturesDlg.tpgAFeatures");
			this.tpgAFeatures.Location = new System.Drawing.Point(4, 22);
			this.tpgAFeatures.Name = "tpgAFeatures";
			this.tpgAFeatures.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.tpgAFeatures.Size = new System.Drawing.Size(440, 383);
			this.tpgAFeatures.TabIndex = 0;
			this.tpgAFeatures.Text = "Articulatory Features";
			this.tpgAFeatures.UseVisualStyleBackColor = true;
			// 
			// btnReset
			// 
			this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnReset.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.btnReset, "Reset Features of Selected Phone");
			this.locExtender.SetLocalizationComment(this.btnReset, null);
			this.locExtender.SetLocalizingId(this.btnReset, "FeaturesDlg.btnReset");
			this.btnReset.Location = new System.Drawing.Point(69, 280);
			this.btnReset.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(80, 26);
			this.btnReset.TabIndex = 1;
			this.btnReset.Text = "Reset";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// tblLayoutAFeatures
			// 
			this.tblLayoutAFeatures.AutoSize = true;
			this.tblLayoutAFeatures.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblLayoutAFeatures.ColumnCount = 1;
			this.tblLayoutAFeatures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutAFeatures.Controls.Add(this.lblAFeatures, 0, 0);
			this.tblLayoutAFeatures.Dock = System.Windows.Forms.DockStyle.Top;
			this.tblLayoutAFeatures.Location = new System.Drawing.Point(3, 3);
			this.tblLayoutAFeatures.Name = "tblLayoutAFeatures";
			this.tblLayoutAFeatures.RowCount = 1;
			this.tblLayoutAFeatures.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutAFeatures.Size = new System.Drawing.Size(434, 26);
			this.tblLayoutAFeatures.TabIndex = 0;
			this.tblLayoutAFeatures.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutPaint);
			// 
			// lblAFeatures
			// 
			this.lblAFeatures.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblAFeatures, null);
			this.locExtender.SetLocalizationComment(this.lblAFeatures, null);
			this.locExtender.SetLocalizationPriority(this.lblAFeatures, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblAFeatures, "FeaturesDlg.lblAFeatures");
			this.lblAFeatures.Location = new System.Drawing.Point(3, 3);
			this.lblAFeatures.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
			this.lblAFeatures.Name = "lblAFeatures";
			this.lblAFeatures.Size = new System.Drawing.Size(14, 13);
			this.lblAFeatures.TabIndex = 0;
			this.lblAFeatures.Text = "#";
			// 
			// tpgBFeatures
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgBFeatures, null);
			this.locExtender.SetLocalizationComment(this.tpgBFeatures, null);
			this.locExtender.SetLocalizingId(this.tpgBFeatures, "FeaturesDlg.tpgBFeatures");
			this.tpgBFeatures.Location = new System.Drawing.Point(4, 22);
			this.tpgBFeatures.Name = "tpgBFeatures";
			this.tpgBFeatures.Padding = new System.Windows.Forms.Padding(3, 8, 3, 0);
			this.tpgBFeatures.Size = new System.Drawing.Size(440, 383);
			this.tpgBFeatures.TabIndex = 1;
			this.tpgBFeatures.Text = "Binary Features";
			this.tpgBFeatures.UseVisualStyleBackColor = true;
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
			((System.ComponentModel.ISupportInitialize)(this.gridPhones)).EndInit();
			this.tabFeatures.ResumeLayout(false);
			this.tpgAFeatures.ResumeLayout(false);
			this.tpgAFeatures.PerformLayout();
			this.tblLayoutAFeatures.ResumeLayout(false);
			this.tblLayoutAFeatures.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitFeatures;
		private SilTools.Controls.SilPanel pnlPhones;
		private SilTools.SilGrid gridPhones;
		private SilTools.Controls.SilGradientPanel pgpPhoneList;
		private System.Windows.Forms.TabControl tabFeatures;
		private System.Windows.Forms.TabPage tpgAFeatures;
		private System.Windows.Forms.TabPage tpgBFeatures;
		private System.Windows.Forms.Label lblAFeatures;
		private System.Windows.Forms.TableLayoutPanel tblLayoutAFeatures;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Button btnReset;
	}
}