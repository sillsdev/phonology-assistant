namespace SIL.Pa.UI.Controls
{
	partial class ChartOptionsDropDown
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
            this.lnkApply = new System.Windows.Forms.LinkLabel();
            this.lnkHelp = new System.Windows.Forms.LinkLabel();
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            this._panelOuter = new SilTools.Controls.SilPanel();
            this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this._charPicker = new SIL.Pa.UI.Controls.CharPicker();
            this._explorerBar = new SIL.Pa.UI.Controls.ExplorerBarItem();
            this.lnkCancel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this._panelOuter.SuspendLayout();
            this._tableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkApply
            // 
            this.lnkApply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkApply.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this.lnkApply, null);
            this.locExtender.SetLocalizationComment(this.lnkApply, null);
            this.locExtender.SetLocalizingId(this.lnkApply, "CommonControls.ChartOptionsPopup.ApplyLink");
            this.lnkApply.Location = new System.Drawing.Point(245, 144);
            this.lnkApply.Margin = new System.Windows.Forms.Padding(3, 0, 0, 5);
            this.lnkApply.Name = "lnkApply";
            this.lnkApply.Size = new System.Drawing.Size(38, 15);
            this.lnkApply.TabIndex = 1;
            this.lnkApply.TabStop = true;
            this.lnkApply.Text = "Apply";
            // 
            // lnkHelp
            // 
            this.lnkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkHelp.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this.lnkHelp, null);
            this.locExtender.SetLocalizationComment(this.lnkHelp, null);
            this.locExtender.SetLocalizingId(this.lnkHelp, "CommonControls.ChartOptionsPopup.HelpLink");
            this.lnkHelp.Location = new System.Drawing.Point(338, 144);
            this.lnkHelp.Margin = new System.Windows.Forms.Padding(0, 0, 3, 5);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(32, 15);
            this.lnkHelp.TabIndex = 2;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.Text = "Help";
            this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
            // 
            // _panelOuter
            // 
            this._panelOuter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this._panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panelOuter.ClipTextForChildControls = true;
            this._panelOuter.ControlReceivingFocusOnMnemonic = null;
            this._panelOuter.Controls.Add(this._charPicker);
            this._panelOuter.Controls.Add(this._tableLayout);
            this._panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelOuter.DoubleBuffered = true;
            this._panelOuter.DrawOnlyBottomBorder = false;
            this._panelOuter.DrawOnlyTopBorder = false;
            this._panelOuter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this._panelOuter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this._panelOuter, null);
            this.locExtender.SetLocalizationComment(this._panelOuter, null);
            this.locExtender.SetLocalizationPriority(this._panelOuter, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._panelOuter, "silPanel1.silPanel1");
            this._panelOuter.Location = new System.Drawing.Point(0, 0);
            this._panelOuter.MnemonicGeneratesClick = false;
            this._panelOuter.Name = "_panelOuter";
            this._panelOuter.PaintExplorerBarBackground = false;
            this._panelOuter.Size = new System.Drawing.Size(444, 243);
            this._panelOuter.TabIndex = 0;
            // 
            // _tableLayout
            // 
            this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayout.BackColor = System.Drawing.Color.Transparent;
            this._tableLayout.ColumnCount = 3;
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._tableLayout.Controls.Add(this.lnkCancel, 0, 1);
            this._tableLayout.Controls.Add(this.lnkHelp, 2, 1);
            this._tableLayout.Controls.Add(this.lnkApply, 0, 1);
            this._tableLayout.Controls.Add(this._explorerBar, 0, 0);
            this._tableLayout.Location = new System.Drawing.Point(0, 0);
            this._tableLayout.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayout.Name = "_tableLayout";
            this._tableLayout.RowCount = 2;
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.Size = new System.Drawing.Size(373, 164);
            this._tableLayout.TabIndex = 6;
            // 
            // _charPicker
            // 
            this._charPicker.AutoSize = false;
            this._charPicker.AutoSizeItems = false;
            this._charPicker.BackColor = System.Drawing.Color.Orange;
            this._charPicker.CheckItemsOnClick = true;
            this._charPicker.Dock = System.Windows.Forms.DockStyle.None;
            this._charPicker.FontSize = 14F;
            this._charPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._charPicker.ItemSize = new System.Drawing.Size(30, 32);
            this._charPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.locExtender.SetLocalizableToolTip(this._charPicker, null);
            this.locExtender.SetLocalizationComment(this._charPicker, null);
            this.locExtender.SetLocalizationPriority(this._charPicker, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._charPicker, "charPicker1.charPicker1");
            this._charPicker.Location = new System.Drawing.Point(66, 194);
            this._charPicker.Name = "_charPicker";
            this._charPicker.Padding = new System.Windows.Forms.Padding(0);
            this._charPicker.Size = new System.Drawing.Size(100, 25);
            this._charPicker.TabIndex = 0;
            this._charPicker.Text = "charPicker1";
            // 
            // _explorerBar
            // 
            this._explorerBar.ButtonBackColor = System.Drawing.Color.CadetBlue;
            this._explorerBar.CanButtonGetFocus = false;
            this._explorerBar.CanCollapse = false;
            this._explorerBar.CheckedBoxState = System.Windows.Forms.CheckState.Unchecked;
            this._tableLayout.SetColumnSpan(this._explorerBar, 3);
            this._explorerBar.IsExpanded = true;
            this.locExtender.SetLocalizableToolTip(this._explorerBar, null);
            this.locExtender.SetLocalizationComment(this._explorerBar, null);
            this.locExtender.SetLocalizingId(this._explorerBar, "CommonControls.ChartOptionsPopup.IgnoredSuprasegmentalsHeadingPanel");
            this._explorerBar.Location = new System.Drawing.Point(0, 0);
            this._explorerBar.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this._explorerBar.Name = "_explorerBar";
            this._explorerBar.Size = new System.Drawing.Size(139, 104);
            this._explorerBar.TabIndex = 0;
            this._explorerBar.Text = "Ignored Suprasegmentals";
            // 
            // lnkCancel
            // 
            this.lnkCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkCancel.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this.lnkCancel, null);
            this.locExtender.SetLocalizationComment(this.lnkCancel, null);
            this.locExtender.SetLocalizingId(this.lnkCancel, "CommonControls.ChartOptionsPopup.CancelLink");
            this.lnkCancel.Location = new System.Drawing.Point(290, 144);
            this.lnkCancel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 5);
            this.lnkCancel.Name = "lnkCancel";
            this.lnkCancel.Size = new System.Drawing.Size(43, 15);
            this.lnkCancel.TabIndex = 3;
            this.lnkCancel.TabStop = true;
            this.lnkCancel.Text = "Cancel";
            this.lnkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleCloseClicked);
            // 
            // ChartOptionsDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._panelOuter);
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this, "ChartOptionsDropDown.ChartOptionsDropDown");
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ChartOptionsDropDown";
            this.Size = new System.Drawing.Size(444, 243);
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this._panelOuter.ResumeLayout(false);
            this._tableLayout.ResumeLayout(false);
            this._tableLayout.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.LinkLabel lnkApply;
		public System.Windows.Forms.LinkLabel lnkHelp;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private ExplorerBarItem _explorerBar;
		private CharPicker _charPicker;
        private SilTools.Controls.SilPanel _panelOuter;
        public System.Windows.Forms.LinkLabel lnkCancel;
	}
}
