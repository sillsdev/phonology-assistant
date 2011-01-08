using SilUtils.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class UndefinedPhoneticCharactersDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UndefinedPhoneticCharactersDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.chkIgnoreInSearches = new System.Windows.Forms.CheckBox();
			this.chkShowUndefinedCharDlg = new System.Windows.Forms.CheckBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblInfo = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.pnlSummary = new SilUtils.Controls.SilPanel();
			this.m_gridChars = new SilUtils.SilGrid();
			this.pgpChars = new SilUtils.Controls.SilGradientPanel();
			this.pnlDetails = new SilUtils.Controls.SilPanel();
			this.m_gridWhere = new SilUtils.SilGrid();
			this.pgpWhere = new SilUtils.Controls.SilGradientPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.pnlSummary.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_gridChars)).BeginInit();
			this.pnlDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_gridWhere)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
			this.pnlButtons.Controls.Add(this.chkIgnoreInSearches);
			this.pnlButtons.Controls.Add(this.chkShowUndefinedCharDlg);
			this.pnlButtons.Controls.Add(this.btnHelp);
			this.pnlButtons.Controls.Add(this.btnOK);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// chkIgnoreInSearches
			// 
			resources.ApplyResources(this.chkIgnoreInSearches, "chkIgnoreInSearches");
			this.chkIgnoreInSearches.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.chkIgnoreInSearches, null);
			this.locExtender.SetLocalizationComment(this.chkIgnoreInSearches, "Check box options on the dialog displaying undefined characters that were found w" +
					"hen reading data sources.");
			this.locExtender.SetLocalizingId(this.chkIgnoreInSearches, "UndefinedPhoneticCharactersDlg.chkIgnoreInSearches");
			this.chkIgnoreInSearches.Name = "chkIgnoreInSearches";
			this.chkIgnoreInSearches.UseVisualStyleBackColor = true;
			// 
			// chkShowUndefinedCharDlg
			// 
			resources.ApplyResources(this.chkShowUndefinedCharDlg, "chkShowUndefinedCharDlg");
			this.locExtender.SetLocalizableToolTip(this.chkShowUndefinedCharDlg, null);
			this.locExtender.SetLocalizationComment(this.chkShowUndefinedCharDlg, "Check box options on the dialog displaying undefined characters that were found w" +
					"hen reading data sources.");
			this.locExtender.SetLocalizingId(this.chkShowUndefinedCharDlg, "UndefinedPhoneticCharactersDlg.chkShowUndefinedCharDlg");
			this.chkShowUndefinedCharDlg.Name = "chkShowUndefinedCharDlg";
			this.chkShowUndefinedCharDlg.UseVisualStyleBackColor = true;
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Button on the dialog displaying undefined characters that were found when reading" +
					" data sources.");
			this.locExtender.SetLocalizingId(this.btnHelp, "UndefinedPhoneticCharactersDlg.btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, "Button on the dialog displaying undefined characters that were found when reading" +
					" data sources.");
			this.locExtender.SetLocalizingId(this.btnOK, "UndefinedPhoneticCharactersDlg.btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.locExtender.SetLocalizableToolTip(this.lblInfo, null);
			this.locExtender.SetLocalizationComment(this.lblInfo, "Information label on the dialog displaying undefined characters that were found w" +
					"hen reading data sources.");
			this.locExtender.SetLocalizingId(this.lblInfo, "UndefinedPhoneticCharactersDlg.lblInfo");
			this.lblInfo.Name = "lblInfo";
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.pnlSummary);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pnlDetails);
			// 
			// pnlSummary
			// 
			this.pnlSummary.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSummary.ClipTextForChildControls = true;
			this.pnlSummary.ControlReceivingFocusOnMnemonic = null;
			this.pnlSummary.Controls.Add(this.m_gridChars);
			this.pnlSummary.Controls.Add(this.pgpChars);
			resources.ApplyResources(this.pnlSummary, "pnlSummary");
			this.pnlSummary.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlSummary, null);
			this.locExtender.SetLocalizationComment(this.pnlSummary, null);
			this.locExtender.SetLocalizationPriority(this.pnlSummary, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSummary, "UndefinedPhoneticCharactersDlg.pnlSummary");
			this.pnlSummary.MnemonicGeneratesClick = false;
			this.pnlSummary.Name = "pnlSummary";
			this.pnlSummary.PaintExplorerBarBackground = false;
			// 
			// m_gridChars
			// 
			this.m_gridChars.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.m_gridChars.AllowUserToAddRows = false;
			this.m_gridChars.AllowUserToDeleteRows = false;
			this.m_gridChars.AllowUserToOrderColumns = true;
			this.m_gridChars.AllowUserToResizeRows = false;
			this.m_gridChars.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
			this.m_gridChars.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_gridChars.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_gridChars.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gridChars.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_gridChars.DefaultCellStyle = dataGridViewCellStyle6;
			resources.ApplyResources(this.m_gridChars, "m_gridChars");
			this.m_gridChars.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_gridChars.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_gridChars, null);
			this.locExtender.SetLocalizationComment(this.m_gridChars, null);
			this.locExtender.SetLocalizationPriority(this.m_gridChars, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_gridChars, "UndefinedPhoneticCharactersDlg.m_gridChars");
			this.m_gridChars.MultiSelect = false;
			this.m_gridChars.Name = "m_gridChars";
			this.m_gridChars.ReadOnly = true;
			this.m_gridChars.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_gridChars.RowHeadersVisible = false;
			this.m_gridChars.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_gridChars.ShowWaterMarkWhenDirty = false;
			this.m_gridChars.WaterMark = "!";
			this.m_gridChars.Enter += new System.EventHandler(this.HandleGridEnter);
			this.m_gridChars.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_gridChars_RowEnter);
			this.m_gridChars.Leave += new System.EventHandler(this.HandleGridLeave);
			// 
			// pgpChars
			// 
			this.pgpChars.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pgpChars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pgpChars.ClipTextForChildControls = false;
			this.pgpChars.ControlReceivingFocusOnMnemonic = this.m_gridChars;
			resources.ApplyResources(this.pgpChars, "pgpChars");
			this.pgpChars.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pgpChars, null);
			this.locExtender.SetLocalizationComment(this.pgpChars, "Heading over the list of undefined characters found. This is in the dialog displa" +
					"ying undefined characters that were found when reading data sources.");
			this.locExtender.SetLocalizingId(this.pgpChars, "UndefinedPhoneticCharactersDlg.pgpChars");
			this.pgpChars.MakeDark = false;
			this.pgpChars.MnemonicGeneratesClick = false;
			this.pgpChars.Name = "pgpChars";
			this.pgpChars.PaintExplorerBarBackground = false;
			// 
			// pnlDetails
			// 
			this.pnlDetails.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlDetails.ClipTextForChildControls = true;
			this.pnlDetails.ControlReceivingFocusOnMnemonic = null;
			this.pnlDetails.Controls.Add(this.m_gridWhere);
			this.pnlDetails.Controls.Add(this.pgpWhere);
			resources.ApplyResources(this.pnlDetails, "pnlDetails");
			this.pnlDetails.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlDetails, null);
			this.locExtender.SetLocalizationComment(this.pnlDetails, null);
			this.locExtender.SetLocalizationPriority(this.pnlDetails, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlDetails, "UndefinedPhoneticCharactersDlg.pnlDetails");
			this.pnlDetails.MnemonicGeneratesClick = false;
			this.pnlDetails.Name = "pnlDetails";
			this.pnlDetails.PaintExplorerBarBackground = false;
			// 
			// m_gridWhere
			// 
			this.m_gridWhere.AllowUserToAddRows = false;
			this.m_gridWhere.AllowUserToDeleteRows = false;
			this.m_gridWhere.AllowUserToOrderColumns = true;
			this.m_gridWhere.AllowUserToResizeRows = false;
			this.m_gridWhere.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_gridWhere.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_gridWhere.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_gridWhere.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle7.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gridWhere.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_gridWhere.DefaultCellStyle = dataGridViewCellStyle8;
			resources.ApplyResources(this.m_gridWhere, "m_gridWhere");
			this.m_gridWhere.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_gridWhere.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_gridWhere, null);
			this.locExtender.SetLocalizationComment(this.m_gridWhere, null);
			this.locExtender.SetLocalizationPriority(this.m_gridWhere, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_gridWhere, "UndefinedPhoneticCharactersDlg.m_gridWhere");
			this.m_gridWhere.MultiSelect = false;
			this.m_gridWhere.Name = "m_gridWhere";
			this.m_gridWhere.ReadOnly = true;
			this.m_gridWhere.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_gridWhere.RowHeadersVisible = false;
			this.m_gridWhere.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_gridWhere.ShowWaterMarkWhenDirty = false;
			this.m_gridWhere.VirtualMode = true;
			this.m_gridWhere.WaterMark = "!";
			this.m_gridWhere.Enter += new System.EventHandler(this.HandleGridEnter);
			this.m_gridWhere.RowHeightInfoNeeded += new System.Windows.Forms.DataGridViewRowHeightInfoNeededEventHandler(this.m_grid_RowHeightInfoNeeded);
			this.m_gridWhere.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.m_gridWhere_CellValueNeeded);
			this.m_gridWhere.Leave += new System.EventHandler(this.HandleGridLeave);
			// 
			// pgpWhere
			// 
			this.pgpWhere.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pgpWhere.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pgpWhere.ClipTextForChildControls = false;
			this.pgpWhere.ControlReceivingFocusOnMnemonic = this.m_gridWhere;
			resources.ApplyResources(this.pgpWhere, "pgpWhere");
			this.pgpWhere.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pgpWhere, null);
			this.locExtender.SetLocalizationComment(this.pgpWhere, "Heading over the list of undefined characters in context. This is in the dialog d" +
					"isplaying undefined characters that were found when reading data sources.");
			this.locExtender.SetLocalizingId(this.pgpWhere, "UndefinedPhoneticCharactersDlg.pgpWhere");
			this.pgpWhere.MakeDark = false;
			this.pgpWhere.MnemonicGeneratesClick = false;
			this.pgpWhere.Name = "pgpWhere";
			this.pgpWhere.PaintExplorerBarBackground = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// UndefinedPhoneticCharactersDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOK;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.pnlButtons);
			this.Controls.Add(this.lblInfo);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "UndefinedPhoneticCharactersDlg.WindowTitle");
			this.Name = "UndefinedPhoneticCharactersDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.pnlButtons.ResumeLayout(false);
			this.pnlButtons.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.pnlSummary.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_gridChars)).EndInit();
			this.pnlDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_gridWhere)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnOK;
		private SilUtils.SilGrid m_gridChars;
		private System.Windows.Forms.Label lblInfo;
		protected System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.CheckBox chkShowUndefinedCharDlg;
		private System.Windows.Forms.CheckBox chkIgnoreInSearches;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private SilUtils.SilGrid m_gridWhere;
		private SilPanel pnlSummary;
		private SilGradientPanel pgpChars;
		private SilPanel pnlDetails;
		private SilGradientPanel pgpWhere;
		private Localization.UI.LocalizationExtender locExtender;
	}
}