using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	partial class FontPicker
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.pnlOuter = new SilTools.Controls.SilPanel();
			this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
			this.flowPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.pnlSample = new SilTools.Controls.SilPanel();
			this.chkBold = new System.Windows.Forms.CheckBox();
			this.chkItalic = new System.Windows.Forms.CheckBox();
			this.cboFontFamily = new System.Windows.Forms.ComboBox();
			this.lblSize = new System.Windows.Forms.Label();
			this.cboSize = new System.Windows.Forms.ComboBox();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlOuter.SuspendLayout();
			this.tblLayout.SuspendLayout();
			this.flowPanelButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlOuter
			// 
			this.pnlOuter.AutoSize = true;
			this.pnlOuter.BackColor = System.Drawing.Color.Transparent;
			this.pnlOuter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlOuter.ClipTextForChildControls = true;
			this.pnlOuter.ControlReceivingFocusOnMnemonic = null;
			this.pnlOuter.Controls.Add(this.tblLayout);
			this.pnlOuter.DoubleBuffered = true;
			this.pnlOuter.DrawOnlyBottomBorder = false;
			this.pnlOuter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlOuter.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlOuter, null);
			this.locExtender.SetLocalizationComment(this.pnlOuter, null);
			this.locExtender.SetLocalizationPriority(this.pnlOuter, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlOuter, "FontPicker.pnlOuter");
			this.pnlOuter.Location = new System.Drawing.Point(0, 0);
			this.pnlOuter.MnemonicGeneratesClick = false;
			this.pnlOuter.Name = "pnlOuter";
			this.pnlOuter.Padding = new System.Windows.Forms.Padding(10, 10, 10, 7);
			this.pnlOuter.PaintExplorerBarBackground = false;
			this.pnlOuter.Size = new System.Drawing.Size(223, 149);
			this.pnlOuter.TabIndex = 0;
			// 
			// tblLayout
			// 
			this.tblLayout.AutoSize = true;
			this.tblLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblLayout.ColumnCount = 4;
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayout.Controls.Add(this.flowPanelButtons, 0, 3);
			this.tblLayout.Controls.Add(this.pnlSample, 0, 2);
			this.tblLayout.Controls.Add(this.chkBold, 0, 1);
			this.tblLayout.Controls.Add(this.chkItalic, 1, 1);
			this.tblLayout.Controls.Add(this.cboFontFamily, 0, 0);
			this.tblLayout.Controls.Add(this.lblSize, 2, 1);
			this.tblLayout.Controls.Add(this.cboSize, 3, 1);
			this.tblLayout.Location = new System.Drawing.Point(10, 10);
			this.tblLayout.Name = "tblLayout";
			this.tblLayout.RowCount = 4;
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.Size = new System.Drawing.Size(198, 127);
			this.tblLayout.TabIndex = 0;
			// 
			// flowPanelButtons
			// 
			this.flowPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flowPanelButtons.AutoSize = true;
			this.flowPanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblLayout.SetColumnSpan(this.flowPanelButtons, 4);
			this.flowPanelButtons.Controls.Add(this.btnCancel);
			this.flowPanelButtons.Controls.Add(this.btnOK);
			this.flowPanelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowPanelButtons.Location = new System.Drawing.Point(0, 104);
			this.flowPanelButtons.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
			this.flowPanelButtons.Name = "flowPanelButtons";
			this.flowPanelButtons.Size = new System.Drawing.Size(198, 23);
			this.flowPanelButtons.TabIndex = 6;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnCancel.AutoSize = true;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizingId(this.btnCancel, "FontPicker.btnCancel");
			this.btnCancel.Location = new System.Drawing.Point(141, 0);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
			this.btnCancel.MinimumSize = new System.Drawing.Size(57, 23);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(57, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.HandleCancelButtonClick);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.AutoSize = true;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizingId(this.btnOK, "FontPicker.btnOK");
			this.btnOK.Location = new System.Drawing.Point(77, 0);
			this.btnOK.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
			this.btnOK.MinimumSize = new System.Drawing.Size(57, 23);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(57, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.HandleOKButtonClick);
			// 
			// pnlSample
			// 
			this.pnlSample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSample.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSample.ClipTextForChildControls = true;
			this.tblLayout.SetColumnSpan(this.pnlSample, 4);
			this.pnlSample.ControlReceivingFocusOnMnemonic = null;
			this.pnlSample.DoubleBuffered = true;
			this.pnlSample.DrawOnlyBottomBorder = false;
			this.pnlSample.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSample.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSample, null);
			this.locExtender.SetLocalizationComment(this.pnlSample, null);
			this.locExtender.SetLocalizingId(this.pnlSample, "FontPicker.pnlSample");
			this.pnlSample.Location = new System.Drawing.Point(0, 56);
			this.pnlSample.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.pnlSample.MnemonicGeneratesClick = false;
			this.pnlSample.Name = "pnlSample";
			this.pnlSample.PaintExplorerBarBackground = false;
			this.pnlSample.Size = new System.Drawing.Size(198, 41);
			this.pnlSample.TabIndex = 5;
			this.pnlSample.Text = "Sample Text (0123)";
			// 
			// chkBold
			// 
			this.chkBold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkBold.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.chkBold, null);
			this.locExtender.SetLocalizationComment(this.chkBold, null);
			this.locExtender.SetLocalizingId(this.chkBold, "FontPicker.chkBold");
			this.chkBold.Location = new System.Drawing.Point(0, 32);
			this.chkBold.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
			this.chkBold.Name = "chkBold";
			this.chkBold.Size = new System.Drawing.Size(50, 19);
			this.chkBold.TabIndex = 1;
			this.chkBold.Text = "&Bold";
			this.chkBold.UseVisualStyleBackColor = true;
			this.chkBold.CheckedChanged += new System.EventHandler(this.HandleFontSettingChanged);
			// 
			// chkItalic
			// 
			this.chkItalic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkItalic.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.chkItalic, null);
			this.locExtender.SetLocalizationComment(this.chkItalic, null);
			this.locExtender.SetLocalizingId(this.chkItalic, "FontPicker.chkItalic");
			this.chkItalic.Location = new System.Drawing.Point(56, 32);
			this.chkItalic.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
			this.chkItalic.Name = "chkItalic";
			this.chkItalic.Size = new System.Drawing.Size(51, 19);
			this.chkItalic.TabIndex = 2;
			this.chkItalic.Text = "&Italic";
			this.chkItalic.UseVisualStyleBackColor = true;
			this.chkItalic.CheckedChanged += new System.EventHandler(this.HandleFontSettingChanged);
			// 
			// cboFontFamily
			// 
			this.cboFontFamily.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayout.SetColumnSpan(this.cboFontFamily, 4);
			this.cboFontFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFontFamily.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboFontFamily, null);
			this.locExtender.SetLocalizationComment(this.cboFontFamily, null);
			this.locExtender.SetLocalizationPriority(this.cboFontFamily, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cboFontFamily, "FontPicker.cboFontFamily");
			this.cboFontFamily.Location = new System.Drawing.Point(0, 0);
			this.cboFontFamily.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.cboFontFamily.Name = "cboFontFamily";
			this.cboFontFamily.Size = new System.Drawing.Size(198, 23);
			this.cboFontFamily.TabIndex = 0;
			this.cboFontFamily.SelectedIndexChanged += new System.EventHandler(this.HandleFontSettingChanged);
			// 
			// lblSize
			// 
			this.lblSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSize.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblSize, null);
			this.locExtender.SetLocalizationComment(this.lblSize, null);
			this.locExtender.SetLocalizingId(this.lblSize, "FontPicker.lblSize");
			this.lblSize.Location = new System.Drawing.Point(118, 33);
			this.lblSize.Margin = new System.Windows.Forms.Padding(5, 0, 0, 3);
			this.lblSize.Name = "lblSize";
			this.lblSize.Size = new System.Drawing.Size(27, 15);
			this.lblSize.TabIndex = 3;
			this.lblSize.Text = "&Size";
			// 
			// cboSize
			// 
			this.cboSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboSize.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this.cboSize, null);
			this.locExtender.SetLocalizationComment(this.cboSize, null);
			this.locExtender.SetLocalizingId(this.cboSize, "FontPicker.cboSize");
			this.cboSize.Location = new System.Drawing.Point(150, 28);
			this.cboSize.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.cboSize.Name = "cboSize";
			this.cboSize.Size = new System.Drawing.Size(48, 23);
			this.cboSize.TabIndex = 4;
			this.cboSize.SelectedIndexChanged += new System.EventHandler(this.HandleFontSettingChanged);
			this.cboSize.Validating += new System.ComponentModel.CancelEventHandler(this.HandleSizeComboValidating);
			this.cboSize.Validated += new System.EventHandler(this.HandleFontSettingChanged);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "User Interface Controls";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// FontPicker
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.Controls.Add(this.pnlOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "FontPicker.FontPicker");
			this.Name = "FontPicker";
			this.Size = new System.Drawing.Size(300, 191);
			this.pnlOuter.ResumeLayout(false);
			this.pnlOuter.PerformLayout();
			this.tblLayout.ResumeLayout(false);
			this.tblLayout.PerformLayout();
			this.flowPanelButtons.ResumeLayout(false);
			this.flowPanelButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboFontFamily;
		private System.Windows.Forms.CheckBox chkBold;
		private System.Windows.Forms.CheckBox chkItalic;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
		private SilPanel pnlSample;
		private SilPanel pnlOuter;
		private System.Windows.Forms.ComboBox cboSize;
		private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.FlowLayoutPanel flowPanelButtons;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
