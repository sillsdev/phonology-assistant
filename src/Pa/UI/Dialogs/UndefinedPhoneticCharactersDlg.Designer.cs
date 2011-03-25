using SilTools.Controls;

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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.chkIgnoreInSearches = new System.Windows.Forms.CheckBox();
			this.chkShowUndefinedCharDlg = new System.Windows.Forms.CheckBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblInfo = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.pnlSummary = new SilTools.Controls.SilPanel();
			this.m_gridChars = new SilTools.SilGrid();
			this.pgpChars = new SilTools.Controls.SilGradientPanel();
			this.pnlDetails = new SilTools.Controls.SilPanel();
			this.m_gridWhere = new SilTools.SilGrid();
			this.pgpWhere = new SilTools.Controls.SilGradientPanel();
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
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlButtons.Location = new System.Drawing.Point(10, 347);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(637, 74);
			this.pnlButtons.TabIndex = 2;
			// 
			// chkIgnoreInSearches
			// 
			this.chkIgnoreInSearches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkIgnoreInSearches.AutoEllipsis = true;
			this.chkIgnoreInSearches.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkIgnoreInSearches.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkIgnoreInSearches, null);
			this.locExtender.SetLocalizationComment(this.chkIgnoreInSearches, "Check box options on the dialog displaying undefined characters that were found w" +
					"hen reading data sources.");
			this.locExtender.SetLocalizingId(this.chkIgnoreInSearches, "UndefinedPhoneticCharactersDlg.chkIgnoreInSearches");
			this.chkIgnoreInSearches.Location = new System.Drawing.Point(0, 32);
			this.chkIgnoreInSearches.Name = "chkIgnoreInSearches";
			this.chkIgnoreInSearches.Size = new System.Drawing.Size(465, 38);
			this.chkIgnoreInSearches.TabIndex = 1;
			this.chkIgnoreInSearches.Text = "&Ignore these characters when searching phonetic data.";
			this.chkIgnoreInSearches.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkIgnoreInSearches.UseVisualStyleBackColor = true;
			// 
			// chkShowUndefinedCharDlg
			// 
			this.chkShowUndefinedCharDlg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowUndefinedCharDlg.AutoSize = true;
			this.chkShowUndefinedCharDlg.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkShowUndefinedCharDlg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkShowUndefinedCharDlg, null);
			this.locExtender.SetLocalizationComment(this.chkShowUndefinedCharDlg, "Check box options on the dialog displaying undefined characters that were found w" +
					"hen reading data sources.");
			this.locExtender.SetLocalizingId(this.chkShowUndefinedCharDlg, "UndefinedPhoneticCharactersDlg.chkShowUndefinedCharDlg");
			this.chkShowUndefinedCharDlg.Location = new System.Drawing.Point(0, 7);
			this.chkShowUndefinedCharDlg.Name = "chkShowUndefinedCharDlg";
			this.chkShowUndefinedCharDlg.Size = new System.Drawing.Size(261, 17);
			this.chkShowUndefinedCharDlg.TabIndex = 0;
			this.chkShowUndefinedCharDlg.Text = "&Show this dialog each time data sources are read.";
			this.chkShowUndefinedCharDlg.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkShowUndefinedCharDlg.UseVisualStyleBackColor = true;
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Button on the dialog displaying undefined characters that were found when reading" +
					" data sources.");
			this.locExtender.SetLocalizingId(this.btnHelp, "UndefinedPhoneticCharactersDlg.btnHelp");
			this.btnHelp.Location = new System.Drawing.Point(557, 41);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(80, 26);
			this.btnHelp.TabIndex = 3;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, "Button on the dialog displaying undefined characters that were found when reading" +
					" data sources.");
			this.locExtender.SetLocalizingId(this.btnOK, "UndefinedPhoneticCharactersDlg.btnOK");
			this.btnOK.Location = new System.Drawing.Point(471, 41);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 26);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblInfo
			// 
			this.lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblInfo, null);
			this.locExtender.SetLocalizationComment(this.lblInfo, "Information label on the dialog displaying undefined characters that were found w" +
					"hen reading data sources.");
			this.locExtender.SetLocalizingId(this.lblInfo, "UndefinedPhoneticCharactersDlg.lblInfo");
			this.lblInfo.Location = new System.Drawing.Point(10, 10);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(637, 83);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "The following Unicode characters were found in data sources specified in the {0} " +
				"project, but they are not found in Phonology Assistant\'s phonetic character inve" +
				"ntory. See Help for more information.";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(10, 93);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.pnlSummary);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pnlDetails);
			this.splitContainer1.Size = new System.Drawing.Size(637, 254);
			this.splitContainer1.SplitterDistance = 254;
			this.splitContainer1.SplitterWidth = 8;
			this.splitContainer1.TabIndex = 1;
			// 
			// pnlSummary
			// 
			this.pnlSummary.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSummary.ClipTextForChildControls = true;
			this.pnlSummary.ControlReceivingFocusOnMnemonic = null;
			this.pnlSummary.Controls.Add(this.m_gridChars);
			this.pnlSummary.Controls.Add(this.pgpChars);
			this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSummary.DoubleBuffered = true;
			this.pnlSummary.DrawOnlyBottomBorder = false;
			this.pnlSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSummary.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSummary, null);
			this.locExtender.SetLocalizationComment(this.pnlSummary, null);
			this.locExtender.SetLocalizationPriority(this.pnlSummary, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSummary, "UndefinedPhoneticCharactersDlg.pnlSummary");
			this.pnlSummary.Location = new System.Drawing.Point(0, 0);
			this.pnlSummary.MnemonicGeneratesClick = false;
			this.pnlSummary.Name = "pnlSummary";
			this.pnlSummary.PaintExplorerBarBackground = false;
			this.pnlSummary.Size = new System.Drawing.Size(254, 254);
			this.pnlSummary.TabIndex = 0;
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
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gridChars.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_gridChars.DefaultCellStyle = dataGridViewCellStyle2;
			this.m_gridChars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_gridChars.DrawTextBoxEditControlBorder = false;
			this.m_gridChars.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.m_gridChars.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_gridChars.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_gridChars.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_gridChars, null);
			this.locExtender.SetLocalizationComment(this.m_gridChars, null);
			this.locExtender.SetLocalizationPriority(this.m_gridChars, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_gridChars, "UndefinedPhoneticCharactersDlg.m_gridChars");
			this.m_gridChars.Location = new System.Drawing.Point(0, 25);
			this.m_gridChars.MultiSelect = false;
			this.m_gridChars.Name = "m_gridChars";
			this.m_gridChars.PaintHeaderAcrossFullGridWidth = true;
			this.m_gridChars.ReadOnly = true;
			this.m_gridChars.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_gridChars.RowHeadersVisible = false;
			this.m_gridChars.RowHeadersWidth = 22;
			this.m_gridChars.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_gridChars.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_gridChars.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_gridChars.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_gridChars.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_gridChars.ShowWaterMarkWhenDirty = false;
			this.m_gridChars.Size = new System.Drawing.Size(252, 227);
			this.m_gridChars.TabIndex = 1;
			this.m_gridChars.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_gridChars.WaterMark = "!";
			this.m_gridChars.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_gridChars_RowEnter);
			this.m_gridChars.Enter += new System.EventHandler(this.HandleGridEnter);
			this.m_gridChars.Leave += new System.EventHandler(this.HandleGridLeave);
			// 
			// pgpChars
			// 
			this.pgpChars.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pgpChars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pgpChars.ClipTextForChildControls = false;
			this.pgpChars.ColorBottom = System.Drawing.Color.Empty;
			this.pgpChars.ColorTop = System.Drawing.Color.Empty;
			this.pgpChars.ControlReceivingFocusOnMnemonic = this.m_gridChars;
			this.pgpChars.Dock = System.Windows.Forms.DockStyle.Top;
			this.pgpChars.DoubleBuffered = true;
			this.pgpChars.DrawOnlyBottomBorder = false;
			this.pgpChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pgpChars.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pgpChars, null);
			this.locExtender.SetLocalizationComment(this.pgpChars, "Heading over the list of undefined characters found. This is in the dialog displa" +
					"ying undefined characters that were found when reading data sources.");
			this.locExtender.SetLocalizingId(this.pgpChars, "UndefinedPhoneticCharactersDlg.pgpChars");
			this.pgpChars.Location = new System.Drawing.Point(0, 0);
			this.pgpChars.MakeDark = false;
			this.pgpChars.MnemonicGeneratesClick = false;
			this.pgpChars.Name = "pgpChars";
			this.pgpChars.PaintExplorerBarBackground = false;
			this.pgpChars.Size = new System.Drawing.Size(252, 25);
			this.pgpChars.TabIndex = 0;
			this.pgpChars.Text = "&Characters";
			// 
			// pnlDetails
			// 
			this.pnlDetails.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlDetails.ClipTextForChildControls = true;
			this.pnlDetails.ControlReceivingFocusOnMnemonic = null;
			this.pnlDetails.Controls.Add(this.m_gridWhere);
			this.pnlDetails.Controls.Add(this.pgpWhere);
			this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDetails.DoubleBuffered = true;
			this.pnlDetails.DrawOnlyBottomBorder = false;
			this.pnlDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlDetails.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlDetails, null);
			this.locExtender.SetLocalizationComment(this.pnlDetails, null);
			this.locExtender.SetLocalizationPriority(this.pnlDetails, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlDetails, "UndefinedPhoneticCharactersDlg.pnlDetails");
			this.pnlDetails.Location = new System.Drawing.Point(0, 0);
			this.pnlDetails.MnemonicGeneratesClick = false;
			this.pnlDetails.Name = "pnlDetails";
			this.pnlDetails.PaintExplorerBarBackground = false;
			this.pnlDetails.Size = new System.Drawing.Size(375, 254);
			this.pnlDetails.TabIndex = 3;
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
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gridWhere.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_gridWhere.DefaultCellStyle = dataGridViewCellStyle4;
			this.m_gridWhere.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_gridWhere.DrawTextBoxEditControlBorder = false;
			this.m_gridWhere.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.m_gridWhere.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_gridWhere.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_gridWhere.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_gridWhere, null);
			this.locExtender.SetLocalizationComment(this.m_gridWhere, null);
			this.locExtender.SetLocalizationPriority(this.m_gridWhere, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_gridWhere, "UndefinedPhoneticCharactersDlg.m_gridWhere");
			this.m_gridWhere.Location = new System.Drawing.Point(0, 25);
			this.m_gridWhere.MultiSelect = false;
			this.m_gridWhere.Name = "m_gridWhere";
			this.m_gridWhere.PaintHeaderAcrossFullGridWidth = true;
			this.m_gridWhere.ReadOnly = true;
			this.m_gridWhere.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_gridWhere.RowHeadersVisible = false;
			this.m_gridWhere.RowHeadersWidth = 22;
			this.m_gridWhere.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_gridWhere.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_gridWhere.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_gridWhere.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_gridWhere.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_gridWhere.ShowWaterMarkWhenDirty = false;
			this.m_gridWhere.Size = new System.Drawing.Size(373, 227);
			this.m_gridWhere.TabIndex = 1;
			this.m_gridWhere.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_gridWhere.VirtualMode = true;
			this.m_gridWhere.WaterMark = "!";
			this.m_gridWhere.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.m_gridWhere_CellValueNeeded);
			this.m_gridWhere.RowHeightInfoNeeded += new System.Windows.Forms.DataGridViewRowHeightInfoNeededEventHandler(this.m_grid_RowHeightInfoNeeded);
			this.m_gridWhere.Enter += new System.EventHandler(this.HandleGridEnter);
			this.m_gridWhere.Leave += new System.EventHandler(this.HandleGridLeave);
			// 
			// pgpWhere
			// 
			this.pgpWhere.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pgpWhere.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pgpWhere.ClipTextForChildControls = false;
			this.pgpWhere.ColorBottom = System.Drawing.Color.Empty;
			this.pgpWhere.ColorTop = System.Drawing.Color.Empty;
			this.pgpWhere.ControlReceivingFocusOnMnemonic = this.m_gridWhere;
			this.pgpWhere.Dock = System.Windows.Forms.DockStyle.Top;
			this.pgpWhere.DoubleBuffered = true;
			this.pgpWhere.DrawOnlyBottomBorder = false;
			this.pgpWhere.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pgpWhere.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pgpWhere, null);
			this.locExtender.SetLocalizationComment(this.pgpWhere, "Heading over the list of undefined characters in context. This is in the dialog d" +
					"isplaying undefined characters that were found when reading data sources.");
			this.locExtender.SetLocalizingId(this.pgpWhere, "UndefinedPhoneticCharactersDlg.pgpWhere");
			this.pgpWhere.Location = new System.Drawing.Point(0, 0);
			this.pgpWhere.MakeDark = false;
			this.pgpWhere.MnemonicGeneratesClick = false;
			this.pgpWhere.Name = "pgpWhere";
			this.pgpWhere.PaintExplorerBarBackground = false;
			this.pgpWhere.Size = new System.Drawing.Size(373, 25);
			this.pgpWhere.TabIndex = 0;
			this.pgpWhere.Text = "&Where character U+{0:X4} is found";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// UndefinedPhoneticCharactersDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOK;
			this.ClientSize = new System.Drawing.Size(657, 421);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.pnlButtons);
			this.Controls.Add(this.lblInfo);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "UndefinedPhoneticCharactersDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(435, 300);
			this.Name = "UndefinedPhoneticCharactersDlg";
			this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Undefined Phonetic Characters";
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
		private SilTools.SilGrid m_gridChars;
		private System.Windows.Forms.Label lblInfo;
		protected System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.CheckBox chkShowUndefinedCharDlg;
		private System.Windows.Forms.CheckBox chkIgnoreInSearches;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private SilTools.SilGrid m_gridWhere;
		private SilPanel pnlSummary;
		private SilGradientPanel pgpChars;
		private SilPanel pnlDetails;
		private SilGradientPanel pgpWhere;
		private Localization.UI.LocalizationExtender locExtender;
	}
}