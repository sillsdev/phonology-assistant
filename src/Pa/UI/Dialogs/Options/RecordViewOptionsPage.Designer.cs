namespace SIL.Pa.UI.Dialogs
{
	partial class RecordViewOptionsPage
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.grpFieldSettings = new System.Windows.Forms.GroupBox();
			this._tableLayoutColDisplayOrder = new System.Windows.Forms.TableLayoutPanel();
			this.fldSelGridRecView = new SIL.Pa.UI.Controls.FieldSelectorGrid();
			this.lblShowFields = new System.Windows.Forms.Label();
			this._buttonMoveDown = new System.Windows.Forms.Button();
			this._buttonMoveUp = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.grpFieldSettings.SuspendLayout();
			this._tableLayoutColDisplayOrder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridRecView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// grpFieldSettings
			// 
			this.grpFieldSettings.Controls.Add(this._tableLayoutColDisplayOrder);
			this.grpFieldSettings.Dock = System.Windows.Forms.DockStyle.Left;
			this.locExtender.SetLocalizableToolTip(this.grpFieldSettings, null);
			this.locExtender.SetLocalizationComment(this.grpFieldSettings, null);
			this.locExtender.SetLocalizingId(this.grpFieldSettings, "DialogBoxes.OptionsDlg.RecordViewTab.FieldSettingsGroupBox");
			this.grpFieldSettings.Location = new System.Drawing.Point(0, 0);
			this.grpFieldSettings.Name = "grpFieldSettings";
			this.grpFieldSettings.Padding = new System.Windows.Forms.Padding(8, 10, 8, 8);
			this.grpFieldSettings.Size = new System.Drawing.Size(240, 263);
			this.grpFieldSettings.TabIndex = 1;
			this.grpFieldSettings.TabStop = false;
			this.grpFieldSettings.Text = "Field Display Options";
			// 
			// _tableLayoutColDisplayOrder
			// 
			this._tableLayoutColDisplayOrder.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutColDisplayOrder.ColumnCount = 2;
			this._tableLayoutColDisplayOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutColDisplayOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutColDisplayOrder.Controls.Add(this.fldSelGridRecView, 0, 1);
			this._tableLayoutColDisplayOrder.Controls.Add(this.lblShowFields, 0, 0);
			this._tableLayoutColDisplayOrder.Controls.Add(this._buttonMoveDown, 1, 2);
			this._tableLayoutColDisplayOrder.Controls.Add(this._buttonMoveUp, 1, 1);
			this._tableLayoutColDisplayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutColDisplayOrder.Location = new System.Drawing.Point(8, 23);
			this._tableLayoutColDisplayOrder.Name = "_tableLayoutColDisplayOrder";
			this._tableLayoutColDisplayOrder.RowCount = 3;
			this._tableLayoutColDisplayOrder.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColDisplayOrder.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutColDisplayOrder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutColDisplayOrder.Size = new System.Drawing.Size(224, 232);
			this._tableLayoutColDisplayOrder.TabIndex = 2;
			// 
			// fldSelGridRecView
			// 
			this.fldSelGridRecView.AllowUserToAddRows = false;
			this.fldSelGridRecView.AllowUserToDeleteRows = false;
			this.fldSelGridRecView.AllowUserToOrderColumns = true;
			this.fldSelGridRecView.AllowUserToResizeColumns = false;
			this.fldSelGridRecView.AllowUserToResizeRows = false;
			this.fldSelGridRecView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fldSelGridRecView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.fldSelGridRecView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.fldSelGridRecView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.fldSelGridRecView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.fldSelGridRecView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.fldSelGridRecView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.fldSelGridRecView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.fldSelGridRecView.ColumnHeadersVisible = false;
			this.fldSelGridRecView.DrawTextBoxEditControlBorder = false;
			this.fldSelGridRecView.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.fldSelGridRecView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridRecView.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.fldSelGridRecView, null);
			this.locExtender.SetLocalizationComment(this.fldSelGridRecView, null);
			this.locExtender.SetLocalizationPriority(this.fldSelGridRecView, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.fldSelGridRecView, "RecordViewOptionsPage.fldSelGridRecView");
			this.fldSelGridRecView.Location = new System.Drawing.Point(0, 35);
			this.fldSelGridRecView.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.fldSelGridRecView.MultiSelect = false;
			this.fldSelGridRecView.Name = "fldSelGridRecView";
			this.fldSelGridRecView.PaintHeaderAcrossFullGridWidth = true;
			this.fldSelGridRecView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridRecView.RowHeadersVisible = false;
			this.fldSelGridRecView.RowHeadersWidth = 22;
			this._tableLayoutColDisplayOrder.SetRowSpan(this.fldSelGridRecView, 2);
			this.fldSelGridRecView.SelectedCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridRecView.SelectedCellForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridRecView.SelectedRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridRecView.SelectedRowForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridRecView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridRecView.ShowWaterMarkWhenDirty = false;
			this.fldSelGridRecView.Size = new System.Drawing.Size(195, 197);
			this.fldSelGridRecView.TabIndex = 1;
			this.fldSelGridRecView.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.fldSelGridRecView.WaterMark = "!";
			// 
			// lblShowFields
			// 
			this.lblShowFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblShowFields.AutoEllipsis = true;
			this.lblShowFields.AutoSize = true;
			this._tableLayoutColDisplayOrder.SetColumnSpan(this.lblShowFields, 2);
			this.lblShowFields.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblShowFields, null);
			this.locExtender.SetLocalizationComment(this.lblShowFields, null);
			this.locExtender.SetLocalizingId(this.lblShowFields, "DialogBoxes.OptionsDlg.RecordViewTab.ShowFieldsLabel");
			this.lblShowFields.Location = new System.Drawing.Point(0, 0);
			this.lblShowFields.Margin = new System.Windows.Forms.Padding(0);
			this.lblShowFields.Name = "lblShowFields";
			this.lblShowFields.Size = new System.Drawing.Size(224, 30);
			this.lblShowFields.TabIndex = 0;
			this.lblShowFields.Text = "&Specify the fields to display and their order.";
			this.lblShowFields.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _buttonMoveDown
			// 
			this._buttonMoveDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			this._buttonMoveDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._buttonMoveDown, "Move selected field down");
			this.locExtender.SetLocalizationComment(this._buttonMoveDown, null);
			this.locExtender.SetLocalizingId(this._buttonMoveDown, "DialogBoxes.OptionsDlg.RecordViewTab.MoveDownButton");
			this._buttonMoveDown.Location = new System.Drawing.Point(200, 65);
			this._buttonMoveDown.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._buttonMoveDown.Name = "_buttonMoveDown";
			this._buttonMoveDown.Size = new System.Drawing.Size(24, 24);
			this._buttonMoveDown.TabIndex = 3;
			this._buttonMoveDown.UseVisualStyleBackColor = true;
			// 
			// _buttonMoveUp
			// 
			this._buttonMoveUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			this._buttonMoveUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._buttonMoveUp, "Move selected field up");
			this.locExtender.SetLocalizationComment(this._buttonMoveUp, null);
			this.locExtender.SetLocalizingId(this._buttonMoveUp, "DialogBoxes.OptionsDlg.RecordViewTab.MoveUpButton");
			this._buttonMoveUp.Location = new System.Drawing.Point(200, 35);
			this._buttonMoveUp.Margin = new System.Windows.Forms.Padding(5, 5, 0, 3);
			this._buttonMoveUp.Name = "_buttonMoveUp";
			this._buttonMoveUp.Size = new System.Drawing.Size(24, 24);
			this._buttonMoveUp.TabIndex = 2;
			this._buttonMoveUp.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// RecordViewOptionsPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.grpFieldSettings);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "RecordViewOptionsPage.RecordViewOptionsPage");
			this.Name = "RecordViewOptionsPage";
			this.Size = new System.Drawing.Size(355, 263);
			this.grpFieldSettings.ResumeLayout(false);
			this._tableLayoutColDisplayOrder.ResumeLayout(false);
			this._tableLayoutColDisplayOrder.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridRecView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpFieldSettings;
		private Controls.FieldSelectorGrid fldSelGridRecView;
		private System.Windows.Forms.Label lblShowFields;
		private System.Windows.Forms.Button _buttonMoveDown;
		private System.Windows.Forms.Button _buttonMoveUp;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutColDisplayOrder;
		protected Localization.UI.LocalizationExtender locExtender;
	}
}
