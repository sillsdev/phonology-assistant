namespace SIL.Pa.UI.Dialogs
{
	partial class RegularExpressionSearchDebugDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this._buttonClose = new System.Windows.Forms.Button();
			this._labelSearchPattern = new System.Windows.Forms.Label();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._textBoxRegExpression = new System.Windows.Forms.TextBox();
			this._labelSearchPatternValue = new System.Windows.Forms.Label();
			this._labelMatches = new System.Windows.Forms.Label();
			this._grid = new SilTools.SilGrid();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._colPhonetic = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._colMatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._colMatchIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._colMatchLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
			this.SuspendLayout();
			// 
			// _buttonClose
			// 
			this._buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._buttonClose.Location = new System.Drawing.Point(578, 330);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(80, 26);
			this._buttonClose.TabIndex = 0;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// _labelSearchPattern
			// 
			this._labelSearchPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSearchPattern.AutoSize = true;
			this._labelSearchPattern.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelSearchPattern.Location = new System.Drawing.Point(0, 0);
			this._labelSearchPattern.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._labelSearchPattern.Name = "_labelSearchPattern";
			this._labelSearchPattern.Size = new System.Drawing.Size(419, 15);
			this._labelSearchPattern.TabIndex = 1;
			this._labelSearchPattern.Text = "Search Pattern:";
			// 
			// _tableLayout
			// 
			this._tableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this._tableLayout.Controls.Add(this._textBoxRegExpression, 0, 2);
			this._tableLayout.Controls.Add(this._labelSearchPattern, 0, 0);
			this._tableLayout.Controls.Add(this._labelSearchPatternValue, 0, 1);
			this._tableLayout.Controls.Add(this._labelMatches, 1, 0);
			this._tableLayout.Controls.Add(this._grid, 1, 1);
			this._tableLayout.Location = new System.Drawing.Point(12, 12);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 3;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Size = new System.Drawing.Size(646, 310);
			this._tableLayout.TabIndex = 2;
			// 
			// _textBoxRegExpression
			// 
			this._textBoxRegExpression.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._textBoxRegExpression.BackColor = System.Drawing.SystemColors.Window;
			this._textBoxRegExpression.HideSelection = false;
			this._textBoxRegExpression.Location = new System.Drawing.Point(0, 43);
			this._textBoxRegExpression.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._textBoxRegExpression.Multiline = true;
			this._textBoxRegExpression.Name = "_textBoxRegExpression";
			this._textBoxRegExpression.ReadOnly = true;
			this._textBoxRegExpression.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._textBoxRegExpression.Size = new System.Drawing.Size(419, 267);
			this._textBoxRegExpression.TabIndex = 4;
			this._textBoxRegExpression.WordWrap = false;
			// 
			// _labelSearchPatternValue
			// 
			this._labelSearchPatternValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSearchPatternValue.AutoSize = true;
			this._labelSearchPatternValue.Location = new System.Drawing.Point(3, 20);
			this._labelSearchPatternValue.Name = "_labelSearchPatternValue";
			this._labelSearchPatternValue.Size = new System.Drawing.Size(413, 13);
			this._labelSearchPatternValue.TabIndex = 3;
			this._labelSearchPatternValue.Text = "#";
			// 
			// _labelMatches
			// 
			this._labelMatches.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelMatches.AutoSize = true;
			this._labelMatches.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelMatches.Location = new System.Drawing.Point(419, 0);
			this._labelMatches.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._labelMatches.Name = "_labelMatches";
			this._labelMatches.Size = new System.Drawing.Size(227, 15);
			this._labelMatches.TabIndex = 5;
			this._labelMatches.Text = "Match Information";
			// 
			// _grid
			// 
			this._grid.AllowUserToAddRows = false;
			this._grid.AllowUserToDeleteRows = false;
			this._grid.AllowUserToOrderColumns = true;
			this._grid.AllowUserToResizeRows = false;
			this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this._grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colPhonetic,
            this._colMatch,
            this._colMatchIndex,
            this._colMatchLength});
			this._grid.DrawTextBoxEditControlBorder = false;
			this._grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this._grid.IsDirty = false;
			this._grid.Location = new System.Drawing.Point(424, 20);
			this._grid.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this._grid.MultiSelect = false;
			this._grid.Name = "_grid";
			this._grid.PaintHeaderAcrossFullGridWidth = true;
			this._grid.ReadOnly = true;
			this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._grid.RowHeadersVisible = false;
			this._grid.RowHeadersWidth = 22;
			this._tableLayout.SetRowSpan(this._grid, 2);
			this._grid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this._grid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this._grid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this._grid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._grid.ShowWaterMarkWhenDirty = false;
			this._grid.Size = new System.Drawing.Size(222, 290);
			this._grid.TabIndex = 6;
			this._grid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this._grid.WaterMark = "!";
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.dataGridViewTextBoxColumn1.HeaderText = "Phonetic";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 77;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.dataGridViewTextBoxColumn2.HeaderText = "Match";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Width = 5;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.dataGridViewTextBoxColumn3.HeaderText = "Match Index";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 5;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.dataGridViewTextBoxColumn4.HeaderText = "Match Length";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.Width = 5;
			// 
			// _colPhonetic
			// 
			this._colPhonetic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this._colPhonetic.HeaderText = "Phonetic";
			this._colPhonetic.Name = "_colPhonetic";
			this._colPhonetic.ReadOnly = true;
			this._colPhonetic.Width = 77;
			// 
			// _colMatch
			// 
			this._colMatch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this._colMatch.HeaderText = "Match";
			this._colMatch.Name = "_colMatch";
			this._colMatch.ReadOnly = true;
			this._colMatch.Width = 64;
			// 
			// _colMatchIndex
			// 
			this._colMatchIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this._colMatchIndex.HeaderText = "Match Index";
			this._colMatchIndex.Name = "_colMatchIndex";
			this._colMatchIndex.ReadOnly = true;
			this._colMatchIndex.Width = 95;
			// 
			// _colMatchLength
			// 
			this._colMatchLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this._colMatchLength.HeaderText = "Match Length";
			this._colMatchLength.Name = "_colMatchLength";
			this._colMatchLength.ReadOnly = true;
			this._colMatchLength.Width = 104;
			// 
			// RegularExpressionSearchDebugDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(670, 366);
			this.Controls.Add(this._tableLayout);
			this.Controls.Add(this._buttonClose);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RegularExpressionSearchDebugDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Phonology Assistant Regular Expression Search Debugging";
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.Label _labelSearchPattern;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.TextBox _textBoxRegExpression;
		private System.Windows.Forms.Label _labelSearchPatternValue;
		private System.Windows.Forms.Label _labelMatches;
		private SilTools.SilGrid _grid;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colPhonetic;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colMatch;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colMatchIndex;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colMatchLength;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
	}
}