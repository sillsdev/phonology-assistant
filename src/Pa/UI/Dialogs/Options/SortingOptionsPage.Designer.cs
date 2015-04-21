namespace SIL.Pa.UI.Dialogs
{
	partial class SortingOptionsPage
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblSaveManual = new System.Windows.Forms.Label();
			this.grpColSortOptions = new System.Windows.Forms.GroupBox();
			this._tableLayoutColSortOptions = new System.Windows.Forms.TableLayoutPanel();
			this.btnMoveSortFieldDown = new System.Windows.Forms.Button();
			this.btnMoveSortFieldUp = new System.Windows.Forms.Button();
			this.m_sortingGrid = new SilTools.SilGrid();
			this.lblSortFldsHdr = new System.Windows.Forms.Label();
			this.chkSaveManual = new System.Windows.Forms.CheckBox();
			this.lblSortInfo = new System.Windows.Forms.Label();
			this.grpPhoneticSortOptions = new System.Windows.Forms.GroupBox();
			this.phoneticSortOptions = new SIL.Pa.UI.Controls.SortOptionsDropDown();
			this.lblListType = new System.Windows.Forms.Label();
			this.cboListType = new System.Windows.Forms.ComboBox();
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.grpColSortOptions.SuspendLayout();
			this._tableLayoutColSortOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_sortingGrid)).BeginInit();
			this.grpPhoneticSortOptions.SuspendLayout();
			this._tableLayoutOuter.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// lblSaveManual
			// 
			this.lblSaveManual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSaveManual.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this.lblSaveManual, 2);
			this.lblSaveManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblSaveManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSaveManual, null);
			this.locExtender.SetLocalizationComment(this.lblSaveManual, null);
			this.locExtender.SetLocalizingId(this.lblSaveManual, "DialogBoxes.OptionsDlg.SortingTab.SaveManualLabel");
			this.lblSaveManual.Location = new System.Drawing.Point(23, 442);
			this.lblSaveManual.Margin = new System.Windows.Forms.Padding(23, 0, 3, 0);
			this.lblSaveManual.Name = "lblSaveManual";
			this.lblSaveManual.Size = new System.Drawing.Size(473, 30);
			this.lblSaveManual.TabIndex = 13;
			this.lblSaveManual.Text = "For example, clicking column headings in the Data Corpus view will change the def" +
    "ault options set on this tab for that view.";
			// 
			// grpColSortOptions
			// 
			this.grpColSortOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.grpColSortOptions.Controls.Add(this._tableLayoutColSortOptions);
			this.locExtender.SetLocalizableToolTip(this.grpColSortOptions, null);
			this.locExtender.SetLocalizationComment(this.grpColSortOptions, null);
			this.locExtender.SetLocalizingId(this.grpColSortOptions, "DialogBoxes.OptionsDlg.SortingTab.ColSortOptionsGroupBox");
			this.grpColSortOptions.Location = new System.Drawing.Point(0, 79);
			this.grpColSortOptions.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
			this.grpColSortOptions.Name = "grpColSortOptions";
			this.grpColSortOptions.Padding = new System.Windows.Forms.Padding(8, 10, 8, 8);
			this.grpColSortOptions.Size = new System.Drawing.Size(234, 334);
			this.grpColSortOptions.TabIndex = 10;
			this.grpColSortOptions.TabStop = false;
			this.grpColSortOptions.Text = "Column Sort Options";
			// 
			// _tableLayoutColSortOptions
			// 
			this._tableLayoutColSortOptions.ColumnCount = 2;
			this._tableLayoutColSortOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutColSortOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutColSortOptions.Controls.Add(this.btnMoveSortFieldDown, 1, 2);
			this._tableLayoutColSortOptions.Controls.Add(this.btnMoveSortFieldUp, 1, 1);
			this._tableLayoutColSortOptions.Controls.Add(this.m_sortingGrid, 0, 1);
			this._tableLayoutColSortOptions.Controls.Add(this.lblSortFldsHdr, 0, 0);
			this._tableLayoutColSortOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutColSortOptions.Location = new System.Drawing.Point(8, 23);
			this._tableLayoutColSortOptions.Name = "_tableLayoutColSortOptions";
			this._tableLayoutColSortOptions.RowCount = 3;
			this._tableLayoutColSortOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColSortOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColSortOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutColSortOptions.Size = new System.Drawing.Size(218, 303);
			this._tableLayoutColSortOptions.TabIndex = 15;
			// 
			// btnMoveSortFieldDown
			// 
			this.btnMoveSortFieldDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			this.btnMoveSortFieldDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnMoveSortFieldDown, "Move selected column down");
			this.locExtender.SetLocalizationComment(this.btnMoveSortFieldDown, null);
			this.locExtender.SetLocalizingId(this.btnMoveSortFieldDown, "DialogBoxes.OptionsDlg.SortingTab.MoveSortFieldDownButton");
			this.btnMoveSortFieldDown.Location = new System.Drawing.Point(194, 62);
			this.btnMoveSortFieldDown.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this.btnMoveSortFieldDown.Name = "btnMoveSortFieldDown";
			this.btnMoveSortFieldDown.Size = new System.Drawing.Size(24, 25);
			this.btnMoveSortFieldDown.TabIndex = 3;
			this.btnMoveSortFieldDown.UseVisualStyleBackColor = true;
			this.btnMoveSortFieldDown.Click += new System.EventHandler(this.HandleButtonMoveSortFieldDownClick);
			// 
			// btnMoveSortFieldUp
			// 
			this.btnMoveSortFieldUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			this.btnMoveSortFieldUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnMoveSortFieldUp, "Move selected column up");
			this.locExtender.SetLocalizationComment(this.btnMoveSortFieldUp, null);
			this.locExtender.SetLocalizingId(this.btnMoveSortFieldUp, "DialogBoxes.OptionsDlg.SortingTab.MoveSortFieldUpButton");
			this.btnMoveSortFieldUp.Location = new System.Drawing.Point(194, 31);
			this.btnMoveSortFieldUp.Margin = new System.Windows.Forms.Padding(5, 5, 0, 3);
			this.btnMoveSortFieldUp.Name = "btnMoveSortFieldUp";
			this.btnMoveSortFieldUp.Size = new System.Drawing.Size(24, 25);
			this.btnMoveSortFieldUp.TabIndex = 2;
			this.btnMoveSortFieldUp.UseVisualStyleBackColor = true;
			this.btnMoveSortFieldUp.Click += new System.EventHandler(this.HandleButtonMoveSortFieldUpClick);
			// 
			// m_sortingGrid
			// 
			this.m_sortingGrid.AllowUserToAddRows = false;
			this.m_sortingGrid.AllowUserToDeleteRows = false;
			this.m_sortingGrid.AllowUserToOrderColumns = true;
			this.m_sortingGrid.AllowUserToResizeRows = false;
			this.m_sortingGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_sortingGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.m_sortingGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_sortingGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_sortingGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_sortingGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.m_sortingGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_sortingGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.m_sortingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_sortingGrid.DrawTextBoxEditControlBorder = false;
			this.m_sortingGrid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_sortingGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_sortingGrid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_sortingGrid, null);
			this.locExtender.SetLocalizationComment(this.m_sortingGrid, null);
			this.locExtender.SetLocalizationPriority(this.m_sortingGrid, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_sortingGrid, "SortingOptionsPage.m_sortingGrid");
			this.m_sortingGrid.Location = new System.Drawing.Point(0, 31);
			this.m_sortingGrid.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.m_sortingGrid.MultiSelect = false;
			this.m_sortingGrid.Name = "m_sortingGrid";
			this.m_sortingGrid.PaintHeaderAcrossFullGridWidth = true;
			this.m_sortingGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_sortingGrid.RowHeadersVisible = false;
			this.m_sortingGrid.RowHeadersWidth = 22;
			this._tableLayoutColSortOptions.SetRowSpan(this.m_sortingGrid, 2);
			this.m_sortingGrid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_sortingGrid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_sortingGrid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_sortingGrid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_sortingGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_sortingGrid.ShowWaterMarkWhenDirty = false;
			this.m_sortingGrid.Size = new System.Drawing.Size(189, 272);
			this.m_sortingGrid.TabIndex = 1;
			this.m_sortingGrid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_sortingGrid.WaterMark = "!";
			this.m_sortingGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleSortingGridCellContentClick);
			this.m_sortingGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleSortingGridRowEnter);
			// 
			// lblSortFldsHdr
			// 
			this.lblSortFldsHdr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSortFldsHdr.AutoSize = true;
			this._tableLayoutColSortOptions.SetColumnSpan(this.lblSortFldsHdr, 2);
			this.lblSortFldsHdr.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSortFldsHdr, null);
			this.locExtender.SetLocalizationComment(this.lblSortFldsHdr, null);
			this.locExtender.SetLocalizingId(this.lblSortFldsHdr, "DialogBoxes.OptionsDlg.SortingTab.SortFldsHdrLabel");
			this.lblSortFldsHdr.Location = new System.Drawing.Point(0, 0);
			this.lblSortFldsHdr.Margin = new System.Windows.Forms.Padding(0);
			this.lblSortFldsHdr.Name = "lblSortFldsHdr";
			this.lblSortFldsHdr.Size = new System.Drawing.Size(218, 26);
			this.lblSortFldsHdr.TabIndex = 0;
			this.lblSortFldsHdr.Text = "Specify the &Columns to sort and in what order.";
			this.lblSortFldsHdr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkSaveManual
			// 
			this.chkSaveManual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSaveManual.AutoSize = true;
			this.chkSaveManual.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._tableLayoutOuter.SetColumnSpan(this.chkSaveManual, 2);
			this.chkSaveManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkSaveManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkSaveManual, null);
			this.locExtender.SetLocalizationComment(this.chkSaveManual, null);
			this.locExtender.SetLocalizingId(this.chkSaveManual, "DialogBoxes.OptionsDlg.SortingTab.SaveManualCheckbox");
			this.chkSaveManual.Location = new System.Drawing.Point(8, 418);
			this.chkSaveManual.Margin = new System.Windows.Forms.Padding(8, 5, 3, 5);
			this.chkSaveManual.Name = "chkSaveManual";
			this.chkSaveManual.Size = new System.Drawing.Size(488, 19);
			this.chkSaveManual.TabIndex = 12;
			this.chkSaveManual.Text = "&Save last manually specified sort options as the default.";
			this.chkSaveManual.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveManual.UseVisualStyleBackColor = true;
			this.chkSaveManual.CheckedChanged += new System.EventHandler(this.HandleCheckSaveManualClick);
			// 
			// lblSortInfo
			// 
			this.lblSortInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSortInfo.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this.lblSortInfo, 2);
			this.lblSortInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSortInfo, null);
			this.locExtender.SetLocalizationComment(this.lblSortInfo, null);
			this.locExtender.SetLocalizingId(this.lblSortInfo, "DialogBoxes.OptionsDlg.SortingTab.SortInfoLabel");
			this.lblSortInfo.Location = new System.Drawing.Point(0, 0);
			this.lblSortInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this.lblSortInfo.Name = "lblSortInfo";
			this.lblSortInfo.Size = new System.Drawing.Size(499, 26);
			this.lblSortInfo.TabIndex = 7;
			this.lblSortInfo.Text = "To specify the default sort options for a specific type of list, choose one from " +
    "the list and select the desired options.";
			// 
			// grpPhoneticSortOptions
			// 
			this.grpPhoneticSortOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.grpPhoneticSortOptions.Controls.Add(this.phoneticSortOptions);
			this.locExtender.SetLocalizableToolTip(this.grpPhoneticSortOptions, null);
			this.locExtender.SetLocalizationComment(this.grpPhoneticSortOptions, null);
			this.locExtender.SetLocalizingId(this.grpPhoneticSortOptions, "DialogBoxes.OptionsDlg.SortingTab.PhoneticSortOptionsGroupBox");
			this.grpPhoneticSortOptions.Location = new System.Drawing.Point(242, 79);
			this.grpPhoneticSortOptions.Margin = new System.Windows.Forms.Padding(0);
			this.grpPhoneticSortOptions.Name = "grpPhoneticSortOptions";
			this.grpPhoneticSortOptions.Padding = new System.Windows.Forms.Padding(0);
			this.grpPhoneticSortOptions.Size = new System.Drawing.Size(236, 334);
			this.grpPhoneticSortOptions.TabIndex = 11;
			this.grpPhoneticSortOptions.TabStop = false;
			this.grpPhoneticSortOptions.Text = "Phonetic Sort Options";
			// 
			// phoneticSortOptions
			// 
			this.phoneticSortOptions.AdvancedOptionsEnabled = true;
			this.phoneticSortOptions.AutoScroll = true;
			this.phoneticSortOptions.AutoSize = true;
			this.phoneticSortOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.phoneticSortOptions.BackColor = System.Drawing.Color.Transparent;
			this.phoneticSortOptions.DrawWithGradientBackground = false;
			this.locExtender.SetLocalizableToolTip(this.phoneticSortOptions, null);
			this.locExtender.SetLocalizationComment(this.phoneticSortOptions, null);
			this.locExtender.SetLocalizationPriority(this.phoneticSortOptions, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.phoneticSortOptions, "SortingOptionsPage.SortOptionsDropDown");
			this.phoneticSortOptions.Location = new System.Drawing.Point(11, 18);
			this.phoneticSortOptions.MakePhoneticPrimarySortFieldWhenOptionsChange = true;
			this.phoneticSortOptions.Margin = new System.Windows.Forms.Padding(2);
			this.phoneticSortOptions.Name = "phoneticSortOptions";
			this.phoneticSortOptions.ShowAdvancedOptions = true;
			this.phoneticSortOptions.ShowButtons = false;
			this.phoneticSortOptions.Size = new System.Drawing.Size(214, 172);
			this.phoneticSortOptions.TabIndex = 0;
			this.phoneticSortOptions.SortOptionsChanged += new SIL.Pa.UI.Controls.SortOptionsDropDown.SortOptionsChangedHandler(this.HandlePhoneticSortOptionsChanged);
			// 
			// lblListType
			// 
			this.lblListType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblListType.AutoSize = true;
			this.lblListType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblListType, null);
			this.locExtender.SetLocalizationComment(this.lblListType, null);
			this.locExtender.SetLocalizingId(this.lblListType, "DialogBoxes.OptionsDlg.SortingTab.ListTypeLabel");
			this.lblListType.Location = new System.Drawing.Point(35, 5);
			this.lblListType.Margin = new System.Windows.Forms.Padding(35, 0, 10, 0);
			this.lblListType.Name = "lblListType";
			this.lblListType.Size = new System.Drawing.Size(82, 13);
			this.lblListType.TabIndex = 8;
			this.lblListType.Text = "&Word List Type:";
			// 
			// cboListType
			// 
			this.cboListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboListType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.cboListType.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboListType, null);
			this.locExtender.SetLocalizationComment(this.cboListType, null);
			this.locExtender.SetLocalizationPriority(this.cboListType, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cboListType, "DialogBoxes.OptionsDlg.SortingTab.cboListType");
			this.cboListType.Location = new System.Drawing.Point(127, 0);
			this.cboListType.Margin = new System.Windows.Forms.Padding(0);
			this.cboListType.Name = "cboListType";
			this.cboListType.Size = new System.Drawing.Size(216, 23);
			this.cboListType.TabIndex = 9;
			this.cboListType.SelectedIndexChanged += new System.EventHandler(this.HandleListTypeComboSelectedIndexChanged);
			// 
			// _tableLayoutOuter
			// 
			this._tableLayoutOuter.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutOuter.ColumnCount = 2;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this._tableLayoutOuter.Controls.Add(this.lblSortInfo, 0, 0);
			this._tableLayoutOuter.Controls.Add(this.lblSaveManual, 0, 4);
			this._tableLayoutOuter.Controls.Add(this.chkSaveManual, 0, 3);
			this._tableLayoutOuter.Controls.Add(this.grpColSortOptions, 0, 2);
			this._tableLayoutOuter.Controls.Add(this.grpPhoneticSortOptions, 1, 2);
			this._tableLayoutOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutOuter.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 5;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.Size = new System.Drawing.Size(499, 472);
			this._tableLayoutOuter.TabIndex = 14;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this._tableLayoutOuter.SetColumnSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblListType, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cboListType, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 41);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(499, 23);
			this.tableLayoutPanel1.TabIndex = 15;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SortingOptionsPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this._tableLayoutOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SortingOptionsPage.SortingOptionsPage");
			this.Name = "SortingOptionsPage";
			this.Size = new System.Drawing.Size(499, 472);
			this.grpColSortOptions.ResumeLayout(false);
			this._tableLayoutColSortOptions.ResumeLayout(false);
			this._tableLayoutColSortOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_sortingGrid)).EndInit();
			this.grpPhoneticSortOptions.ResumeLayout(false);
			this.grpPhoneticSortOptions.PerformLayout();
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblSaveManual;
		private System.Windows.Forms.GroupBox grpColSortOptions;
		private SilTools.SilGrid m_sortingGrid;
		private System.Windows.Forms.Button btnMoveSortFieldUp;
		private System.Windows.Forms.Label lblSortFldsHdr;
		private System.Windows.Forms.Button btnMoveSortFieldDown;
		private System.Windows.Forms.CheckBox chkSaveManual;
		private System.Windows.Forms.Label lblSortInfo;
		private System.Windows.Forms.GroupBox grpPhoneticSortOptions;
		private Controls.SortOptionsDropDown phoneticSortOptions;
		private System.Windows.Forms.Label lblListType;
		private System.Windows.Forms.ComboBox cboListType;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutColSortOptions;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		protected L10NSharp.UI.L10NSharpExtender locExtender;
	}
}
