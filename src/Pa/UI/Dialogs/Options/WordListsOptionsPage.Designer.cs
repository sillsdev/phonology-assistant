namespace SIL.Pa.UI.Dialogs
{
	partial class WordListsOptionsPage
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WordListsOptionsPage));
			this.grpColSettings = new System.Windows.Forms.GroupBox();
			this._tableLayoutColDisplayOrder = new System.Windows.Forms.TableLayoutPanel();
			this.lblShowColumns = new System.Windows.Forms.Label();
			this.btnMoveColDown = new System.Windows.Forms.Button();
			this.btnMoveColUp = new System.Windows.Forms.Button();
			this.fldSelGridWrdList = new SIL.Pa.UI.Controls.FieldSelectorGrid();
			this.grpColChanges = new System.Windows.Forms.GroupBox();
			this._tableLayoutColChanges = new System.Windows.Forms.TableLayoutPanel();
			this.chkSaveColHdrHeight = new SilTools.Controls.AutoHeightCheckBox();
			this.chkSaveReorderedColumns = new SilTools.Controls.AutoHeightCheckBox();
			this.chkSaveColWidths = new SilTools.Controls.AutoHeightCheckBox();
			this.lblExplanation = new System.Windows.Forms.Label();
			this.nudMaxEticColWidth = new System.Windows.Forms.NumericUpDown();
			this.chkAutoAdjustPhoneticCol = new SilTools.Controls.AutoHeightCheckBox();
			this.grpGridLines = new System.Windows.Forms.GroupBox();
			this._tableLayoutGridLines = new System.Windows.Forms.TableLayoutPanel();
			this.rbGridLinesNone = new System.Windows.Forms.RadioButton();
			this.rbGridLinesBoth = new System.Windows.Forms.RadioButton();
			this.rbGridLinesHorizontal = new System.Windows.Forms.RadioButton();
			this.rbGridLinesVertical = new System.Windows.Forms.RadioButton();
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this.grpColSettings.SuspendLayout();
			this._tableLayoutColDisplayOrder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridWrdList)).BeginInit();
			this.grpColChanges.SuspendLayout();
			this._tableLayoutColChanges.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxEticColWidth)).BeginInit();
			this.grpGridLines.SuspendLayout();
			this._tableLayoutGridLines.SuspendLayout();
			this._tableLayoutOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpColSettings
			// 
			this.grpColSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpColSettings.Controls.Add(this._tableLayoutColDisplayOrder);
			this.grpColSettings.Location = new System.Drawing.Point(0, 0);
			this.grpColSettings.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
			this.grpColSettings.Name = "grpColSettings";
			this.grpColSettings.Padding = new System.Windows.Forms.Padding(8, 10, 8, 8);
			this._tableLayoutOuter.SetRowSpan(this.grpColSettings, 2);
			this.grpColSettings.Size = new System.Drawing.Size(240, 263);
			this.grpColSettings.TabIndex = 5;
			this.grpColSettings.TabStop = false;
			this.grpColSettings.Text = "Column Display Options";
			// 
			// _tableLayoutColDisplayOrder
			// 
			this._tableLayoutColDisplayOrder.ColumnCount = 2;
			this._tableLayoutColDisplayOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutColDisplayOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutColDisplayOrder.Controls.Add(this.lblShowColumns, 0, 0);
			this._tableLayoutColDisplayOrder.Controls.Add(this.btnMoveColDown, 1, 2);
			this._tableLayoutColDisplayOrder.Controls.Add(this.btnMoveColUp, 1, 1);
			this._tableLayoutColDisplayOrder.Controls.Add(this.fldSelGridWrdList, 0, 1);
			this._tableLayoutColDisplayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutColDisplayOrder.Location = new System.Drawing.Point(8, 23);
			this._tableLayoutColDisplayOrder.Name = "_tableLayoutColDisplayOrder";
			this._tableLayoutColDisplayOrder.RowCount = 3;
			this._tableLayoutColDisplayOrder.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColDisplayOrder.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColDisplayOrder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutColDisplayOrder.Size = new System.Drawing.Size(224, 232);
			this._tableLayoutColDisplayOrder.TabIndex = 10;
			// 
			// lblShowColumns
			// 
			this.lblShowColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblShowColumns.AutoEllipsis = true;
			this.lblShowColumns.AutoSize = true;
			this._tableLayoutColDisplayOrder.SetColumnSpan(this.lblShowColumns, 2);
			this.lblShowColumns.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblShowColumns.Location = new System.Drawing.Point(0, 0);
			this.lblShowColumns.Margin = new System.Windows.Forms.Padding(0);
			this.lblShowColumns.Name = "lblShowColumns";
			this.lblShowColumns.Size = new System.Drawing.Size(224, 13);
			this.lblShowColumns.TabIndex = 0;
			this.lblShowColumns.Text = "&Specify the columns to display and their order.";
			this.lblShowColumns.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnMoveColDown
			// 
			this.btnMoveColDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			this.btnMoveColDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnMoveColDown.Location = new System.Drawing.Point(200, 48);
			this.btnMoveColDown.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this.btnMoveColDown.Name = "btnMoveColDown";
			this.btnMoveColDown.Size = new System.Drawing.Size(24, 24);
			this.btnMoveColDown.TabIndex = 3;
			this.btnMoveColDown.UseVisualStyleBackColor = true;
			// 
			// btnMoveColUp
			// 
			this.btnMoveColUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			this.btnMoveColUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnMoveColUp.Location = new System.Drawing.Point(200, 18);
			this.btnMoveColUp.Margin = new System.Windows.Forms.Padding(5, 5, 0, 3);
			this.btnMoveColUp.Name = "btnMoveColUp";
			this.btnMoveColUp.Size = new System.Drawing.Size(24, 24);
			this.btnMoveColUp.TabIndex = 2;
			this.btnMoveColUp.UseVisualStyleBackColor = true;
			// 
			// fldSelGridWrdList
			// 
			this.fldSelGridWrdList.AllowUserToAddRows = false;
			this.fldSelGridWrdList.AllowUserToDeleteRows = false;
			this.fldSelGridWrdList.AllowUserToResizeColumns = false;
			this.fldSelGridWrdList.AllowUserToResizeRows = false;
			this.fldSelGridWrdList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fldSelGridWrdList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.fldSelGridWrdList.BackgroundColor = System.Drawing.SystemColors.Window;
			this.fldSelGridWrdList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.fldSelGridWrdList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.fldSelGridWrdList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.fldSelGridWrdList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.fldSelGridWrdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.fldSelGridWrdList.ColumnHeadersVisible = false;
			this.fldSelGridWrdList.DrawTextBoxEditControlBorder = false;
			this.fldSelGridWrdList.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.fldSelGridWrdList.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.fldSelGridWrdList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridWrdList.IsDirty = false;
			this.fldSelGridWrdList.Location = new System.Drawing.Point(0, 18);
			this.fldSelGridWrdList.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.fldSelGridWrdList.MultiSelect = false;
			this.fldSelGridWrdList.Name = "fldSelGridWrdList";
			this.fldSelGridWrdList.PaintHeaderAcrossFullGridWidth = true;
			this.fldSelGridWrdList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridWrdList.RowHeadersVisible = false;
			this.fldSelGridWrdList.RowHeadersWidth = 22;
			this._tableLayoutColDisplayOrder.SetRowSpan(this.fldSelGridWrdList, 2);
			this.fldSelGridWrdList.SelectedCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridWrdList.SelectedCellForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridWrdList.SelectedRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridWrdList.SelectedRowForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridWrdList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridWrdList.ShowWaterMarkWhenDirty = false;
			this.fldSelGridWrdList.Size = new System.Drawing.Size(195, 214);
			this.fldSelGridWrdList.TabIndex = 1;
			this.fldSelGridWrdList.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.fldSelGridWrdList.WaterMark = "!";
			// 
			// grpColChanges
			// 
			this.grpColChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpColChanges.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this.grpColChanges, 2);
			this.grpColChanges.Controls.Add(this._tableLayoutColChanges);
			this.grpColChanges.Location = new System.Drawing.Point(248, 0);
			this.grpColChanges.Margin = new System.Windows.Forms.Padding(0);
			this.grpColChanges.Name = "grpColChanges";
			this.grpColChanges.Padding = new System.Windows.Forms.Padding(8, 10, 8, 8);
			this.grpColChanges.Size = new System.Drawing.Size(228, 153);
			this.grpColChanges.TabIndex = 6;
			this.grpColChanges.TabStop = false;
			this.grpColChanges.Text = "Column Changes";
			// 
			// _tableLayoutColChanges
			// 
			this._tableLayoutColChanges.AutoSize = true;
			this._tableLayoutColChanges.ColumnCount = 1;
			this._tableLayoutColChanges.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutColChanges.Controls.Add(this.chkSaveColHdrHeight, 0, 3);
			this._tableLayoutColChanges.Controls.Add(this.chkSaveReorderedColumns, 0, 1);
			this._tableLayoutColChanges.Controls.Add(this.chkSaveColWidths, 0, 2);
			this._tableLayoutColChanges.Controls.Add(this.lblExplanation, 0, 0);
			this._tableLayoutColChanges.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayoutColChanges.Location = new System.Drawing.Point(8, 23);
			this._tableLayoutColChanges.Name = "_tableLayoutColChanges";
			this._tableLayoutColChanges.RowCount = 4;
			this._tableLayoutColChanges.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColChanges.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColChanges.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColChanges.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColChanges.Size = new System.Drawing.Size(212, 148);
			this._tableLayoutColChanges.TabIndex = 5;
			// 
			// chkSaveColHdrHeight
			// 
			this.chkSaveColHdrHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSaveColHdrHeight.AutoSize = true;
			this.chkSaveColHdrHeight.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveColHdrHeight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.chkSaveColHdrHeight.Location = new System.Drawing.Point(8, 118);
			this.chkSaveColHdrHeight.Margin = new System.Windows.Forms.Padding(8, 7, 0, 0);
			this.chkSaveColHdrHeight.Name = "chkSaveColHdrHeight";
			this.chkSaveColHdrHeight.Size = new System.Drawing.Size(204, 30);
			this.chkSaveColHdrHeight.TabIndex = 3;
			this.chkSaveColHdrHeight.Text = "Save adjusted column heading &height as default height";
			this.chkSaveColHdrHeight.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveColHdrHeight.UseVisualStyleBackColor = true;
			// 
			// chkSaveReorderedColumns
			// 
			this.chkSaveReorderedColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSaveReorderedColumns.AutoSize = true;
			this.chkSaveReorderedColumns.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveReorderedColumns.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.chkSaveReorderedColumns.Location = new System.Drawing.Point(8, 44);
			this.chkSaveReorderedColumns.Margin = new System.Windows.Forms.Padding(8, 5, 0, 0);
			this.chkSaveReorderedColumns.Name = "chkSaveReorderedColumns";
			this.chkSaveReorderedColumns.Size = new System.Drawing.Size(204, 30);
			this.chkSaveReorderedColumns.TabIndex = 1;
			this.chkSaveReorderedColumns.Text = "Save arrangement of &reordered columns as default order";
			this.chkSaveReorderedColumns.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveReorderedColumns.UseVisualStyleBackColor = true;
			// 
			// chkSaveColWidths
			// 
			this.chkSaveColWidths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSaveColWidths.AutoSize = true;
			this.chkSaveColWidths.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveColWidths.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.chkSaveColWidths.Location = new System.Drawing.Point(8, 81);
			this.chkSaveColWidths.Margin = new System.Windows.Forms.Padding(8, 7, 0, 0);
			this.chkSaveColWidths.Name = "chkSaveColWidths";
			this.chkSaveColWidths.Size = new System.Drawing.Size(204, 30);
			this.chkSaveColWidths.TabIndex = 2;
			this.chkSaveColWidths.Text = "Save adjusted column &widths as default widths";
			this.chkSaveColWidths.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveColWidths.UseVisualStyleBackColor = true;
			// 
			// lblExplanation
			// 
			this.lblExplanation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblExplanation.AutoEllipsis = true;
			this.lblExplanation.AutoSize = true;
			this.lblExplanation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblExplanation.Location = new System.Drawing.Point(0, 0);
			this.lblExplanation.Margin = new System.Windows.Forms.Padding(0);
			this.lblExplanation.Name = "lblExplanation";
			this.lblExplanation.Size = new System.Drawing.Size(212, 39);
			this.lblExplanation.TabIndex = 0;
			this.lblExplanation.Text = "Use the following to control how the last changes made to any existing word list " +
    "will affect new word lists.";
			// 
			// nudMaxEticColWidth
			// 
			this.nudMaxEticColWidth.Location = new System.Drawing.Point(417, 268);
			this.nudMaxEticColWidth.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
			this.nudMaxEticColWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudMaxEticColWidth.Name = "nudMaxEticColWidth";
			this.nudMaxEticColWidth.Size = new System.Drawing.Size(59, 20);
			this.nudMaxEticColWidth.TabIndex = 9;
			this.nudMaxEticColWidth.Visible = false;
			// 
			// chkAutoAdjustPhoneticCol
			// 
			this.chkAutoAdjustPhoneticCol.AutoSize = true;
			this.chkAutoAdjustPhoneticCol.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._tableLayoutOuter.SetColumnSpan(this.chkAutoAdjustPhoneticCol, 2);
			this.chkAutoAdjustPhoneticCol.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.chkAutoAdjustPhoneticCol.Location = new System.Drawing.Point(8, 268);
			this.chkAutoAdjustPhoneticCol.Margin = new System.Windows.Forms.Padding(8, 5, 3, 0);
			this.chkAutoAdjustPhoneticCol.Name = "chkAutoAdjustPhoneticCol";
			this.chkAutoAdjustPhoneticCol.Size = new System.Drawing.Size(403, 43);
			this.chkAutoAdjustPhoneticCol.TabIndex = 8;
			this.chkAutoAdjustPhoneticCol.Text = resources.GetString("chkAutoAdjustPhoneticCol.Text");
			this.chkAutoAdjustPhoneticCol.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkAutoAdjustPhoneticCol.UseVisualStyleBackColor = true;
			this.chkAutoAdjustPhoneticCol.Visible = false;
			// 
			// grpGridLines
			// 
			this.grpGridLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutOuter.SetColumnSpan(this.grpGridLines, 2);
			this.grpGridLines.Controls.Add(this._tableLayoutGridLines);
			this.grpGridLines.Location = new System.Drawing.Point(248, 159);
			this.grpGridLines.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
			this.grpGridLines.Name = "grpGridLines";
			this.grpGridLines.Padding = new System.Windows.Forms.Padding(8, 10, 8, 8);
			this.grpGridLines.Size = new System.Drawing.Size(228, 104);
			this.grpGridLines.TabIndex = 7;
			this.grpGridLines.TabStop = false;
			this.grpGridLines.Text = "Grid Lines";
			// 
			// _tableLayoutGridLines
			// 
			this._tableLayoutGridLines.ColumnCount = 2;
			this._tableLayoutGridLines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutGridLines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutGridLines.Controls.Add(this.rbGridLinesNone, 1, 1);
			this._tableLayoutGridLines.Controls.Add(this.rbGridLinesBoth, 1, 0);
			this._tableLayoutGridLines.Controls.Add(this.rbGridLinesHorizontal, 0, 1);
			this._tableLayoutGridLines.Controls.Add(this.rbGridLinesVertical, 0, 0);
			this._tableLayoutGridLines.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutGridLines.Location = new System.Drawing.Point(8, 23);
			this._tableLayoutGridLines.Name = "_tableLayoutGridLines";
			this._tableLayoutGridLines.RowCount = 2;
			this._tableLayoutGridLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutGridLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutGridLines.Size = new System.Drawing.Size(212, 73);
			this._tableLayoutGridLines.TabIndex = 10;
			// 
			// rbGridLinesNone
			// 
			this.rbGridLinesNone.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rbGridLinesNone.AutoEllipsis = true;
			this.rbGridLinesNone.Image = global::SIL.Pa.Properties.Resources.kimidNoGridLines;
			this.rbGridLinesNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rbGridLinesNone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rbGridLinesNone.Location = new System.Drawing.Point(109, 39);
			this.rbGridLinesNone.Name = "rbGridLinesNone";
			this.rbGridLinesNone.Size = new System.Drawing.Size(100, 31);
			this.rbGridLinesNone.TabIndex = 3;
			this.rbGridLinesNone.TabStop = true;
			this.rbGridLinesNone.Text = "  &None";
			this.rbGridLinesNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.rbGridLinesNone.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesBoth
			// 
			this.rbGridLinesBoth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rbGridLinesBoth.AutoEllipsis = true;
			this.rbGridLinesBoth.Image = global::SIL.Pa.Properties.Resources.kimidBothGridLines;
			this.rbGridLinesBoth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rbGridLinesBoth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rbGridLinesBoth.Location = new System.Drawing.Point(109, 3);
			this.rbGridLinesBoth.Name = "rbGridLinesBoth";
			this.rbGridLinesBoth.Size = new System.Drawing.Size(100, 30);
			this.rbGridLinesBoth.TabIndex = 2;
			this.rbGridLinesBoth.TabStop = true;
			this.rbGridLinesBoth.Text = "  &Both";
			this.rbGridLinesBoth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.rbGridLinesBoth.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesHorizontal
			// 
			this.rbGridLinesHorizontal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rbGridLinesHorizontal.Image = global::SIL.Pa.Properties.Resources.kimidHorizontalGridLines;
			this.rbGridLinesHorizontal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rbGridLinesHorizontal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rbGridLinesHorizontal.Location = new System.Drawing.Point(3, 39);
			this.rbGridLinesHorizontal.Name = "rbGridLinesHorizontal";
			this.rbGridLinesHorizontal.Size = new System.Drawing.Size(100, 31);
			this.rbGridLinesHorizontal.TabIndex = 1;
			this.rbGridLinesHorizontal.TabStop = true;
			this.rbGridLinesHorizontal.Text = "  H&orizontal only";
			this.rbGridLinesHorizontal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.rbGridLinesHorizontal.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesVertical
			// 
			this.rbGridLinesVertical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rbGridLinesVertical.Image = global::SIL.Pa.Properties.Resources.kimidVerticalGridLines;
			this.rbGridLinesVertical.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rbGridLinesVertical.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rbGridLinesVertical.Location = new System.Drawing.Point(3, 3);
			this.rbGridLinesVertical.Name = "rbGridLinesVertical";
			this.rbGridLinesVertical.Size = new System.Drawing.Size(100, 30);
			this.rbGridLinesVertical.TabIndex = 0;
			this.rbGridLinesVertical.TabStop = true;
			this.rbGridLinesVertical.Text = "  &Vertical only";
			this.rbGridLinesVertical.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.rbGridLinesVertical.UseVisualStyleBackColor = true;
			// 
			// _tableLayoutOuter
			// 
			this._tableLayoutOuter.AutoSize = true;
			this._tableLayoutOuter.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutOuter.ColumnCount = 3;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.Controls.Add(this.chkAutoAdjustPhoneticCol, 0, 2);
			this._tableLayoutOuter.Controls.Add(this.nudMaxEticColWidth, 2, 2);
			this._tableLayoutOuter.Controls.Add(this.grpColChanges, 1, 0);
			this._tableLayoutOuter.Controls.Add(this.grpGridLines, 1, 1);
			this._tableLayoutOuter.Controls.Add(this.grpColSettings, 0, 0);
			this._tableLayoutOuter.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayoutOuter.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 3;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.Size = new System.Drawing.Size(476, 311);
			this._tableLayoutOuter.TabIndex = 10;
			// 
			// WordListsOptionsPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this._tableLayoutOuter);
			this.Name = "WordListsOptionsPage";
			this.Size = new System.Drawing.Size(476, 378);
			this.grpColSettings.ResumeLayout(false);
			this._tableLayoutColDisplayOrder.ResumeLayout(false);
			this._tableLayoutColDisplayOrder.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridWrdList)).EndInit();
			this.grpColChanges.ResumeLayout(false);
			this.grpColChanges.PerformLayout();
			this._tableLayoutColChanges.ResumeLayout(false);
			this._tableLayoutColChanges.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxEticColWidth)).EndInit();
			this.grpGridLines.ResumeLayout(false);
			this._tableLayoutGridLines.ResumeLayout(false);
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpColSettings;
		private Controls.FieldSelectorGrid fldSelGridWrdList;
		private System.Windows.Forms.Button btnMoveColDown;
		private System.Windows.Forms.Button btnMoveColUp;
		private System.Windows.Forms.Label lblShowColumns;
		private System.Windows.Forms.GroupBox grpColChanges;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutColChanges;
		private SilTools.Controls.AutoHeightCheckBox chkSaveColHdrHeight;
		private SilTools.Controls.AutoHeightCheckBox chkSaveReorderedColumns;
		private SilTools.Controls.AutoHeightCheckBox chkSaveColWidths;
		private System.Windows.Forms.Label lblExplanation;
		private System.Windows.Forms.NumericUpDown nudMaxEticColWidth;
		private SilTools.Controls.AutoHeightCheckBox chkAutoAdjustPhoneticCol;
		private System.Windows.Forms.GroupBox grpGridLines;
		private System.Windows.Forms.RadioButton rbGridLinesHorizontal;
		private System.Windows.Forms.RadioButton rbGridLinesVertical;
		private System.Windows.Forms.RadioButton rbGridLinesBoth;
		private System.Windows.Forms.RadioButton rbGridLinesNone;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutColDisplayOrder;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutGridLines;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
	}
}
