namespace SIL.Pa.UI.Dialogs
{
	partial class CVPatternsOptionsPage
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
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._cvPatternSymbolExplorer = new SIL.Pa.UI.Controls.CVPatternDisplaySymbolsExplorer();
			this.grpDisplayChars = new System.Windows.Forms.GroupBox();
			this._tableLayoutCustomChars = new System.Windows.Forms.TableLayoutPanel();
			this.lblInstruction = new System.Windows.Forms.Label();
			this.lblExampleCV = new System.Windows.Forms.Label();
			this.lblExampleDesc2 = new System.Windows.Forms.Label();
			this.lblExampleCVCV = new System.Windows.Forms.Label();
			this.txtCustomChars = new System.Windows.Forms.TextBox();
			this.lblExampleDesc1 = new System.Windows.Forms.Label();
			this.txtExampleInput = new System.Windows.Forms.TextBox();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._tableLayoutOuter.SuspendLayout();
			this.grpDisplayChars.SuspendLayout();
			this._tableLayoutCustomChars.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutOuter
			// 
			this._tableLayoutOuter.ColumnCount = 2;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.7284F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.2716F));
			this._tableLayoutOuter.Controls.Add(this._cvPatternSymbolExplorer, 0, 0);
			this._tableLayoutOuter.Controls.Add(this.grpDisplayChars, 1, 0);
			this._tableLayoutOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutOuter.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 1;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.Size = new System.Drawing.Size(538, 329);
			this._tableLayoutOuter.TabIndex = 17;
			// 
			// _cvPatternSymbolExplorer
			// 
			this._cvPatternSymbolExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._cvPatternSymbolExplorer.AutoScroll = true;
			this._cvPatternSymbolExplorer.BackColor = System.Drawing.Color.White;
			this._cvPatternSymbolExplorer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._cvPatternSymbolExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._cvPatternSymbolExplorer.ClipTextForChildControls = true;
			this._cvPatternSymbolExplorer.ControlReceivingFocusOnMnemonic = null;
			this._cvPatternSymbolExplorer.DoubleBuffered = true;
			this._cvPatternSymbolExplorer.DrawOnlyBottomBorder = false;
			this._cvPatternSymbolExplorer.DrawOnlyTopBorder = false;
			this._cvPatternSymbolExplorer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._cvPatternSymbolExplorer.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._cvPatternSymbolExplorer, null);
			this.locExtender.SetLocalizationComment(this._cvPatternSymbolExplorer, null);
			this.locExtender.SetLocalizationPriority(this._cvPatternSymbolExplorer, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._cvPatternSymbolExplorer, "CVPatternsOptionsPage._cvPatternSymbolExplorer");
			this._cvPatternSymbolExplorer.Location = new System.Drawing.Point(0, 0);
			this._cvPatternSymbolExplorer.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
			this._cvPatternSymbolExplorer.MnemonicGeneratesClick = false;
			this._cvPatternSymbolExplorer.Name = "_cvPatternSymbolExplorer";
			this._cvPatternSymbolExplorer.PaintExplorerBarBackground = false;
			this._cvPatternSymbolExplorer.Size = new System.Drawing.Size(261, 329);
			this._cvPatternSymbolExplorer.TabIndex = 15;
			// 
			// grpDisplayChars
			// 
			this.grpDisplayChars.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpDisplayChars.Controls.Add(this._tableLayoutCustomChars);
			this.locExtender.SetLocalizableToolTip(this.grpDisplayChars, null);
			this.locExtender.SetLocalizationComment(this.grpDisplayChars, null);
			this.locExtender.SetLocalizingId(this.grpDisplayChars, "DialogBoxes.OptionsDlg.CVPatternsTab.DisplayCharsGroupBox");
			this.grpDisplayChars.Location = new System.Drawing.Point(273, 0);
			this.grpDisplayChars.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.grpDisplayChars.Name = "grpDisplayChars";
			this.grpDisplayChars.Padding = new System.Windows.Forms.Padding(15, 8, 15, 15);
			this.grpDisplayChars.Size = new System.Drawing.Size(265, 329);
			this.grpDisplayChars.TabIndex = 14;
			this.grpDisplayChars.TabStop = false;
			this.grpDisplayChars.Text = "Display these characters";
			// 
			// _tableLayoutCustomChars
			// 
			this._tableLayoutCustomChars.ColumnCount = 2;
			this._tableLayoutCustomChars.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.44444F));
			this._tableLayoutCustomChars.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.55556F));
			this._tableLayoutCustomChars.Controls.Add(this.lblInstruction, 0, 0);
			this._tableLayoutCustomChars.Controls.Add(this.lblExampleCV, 1, 5);
			this._tableLayoutCustomChars.Controls.Add(this.lblExampleDesc2, 0, 4);
			this._tableLayoutCustomChars.Controls.Add(this.lblExampleCVCV, 0, 5);
			this._tableLayoutCustomChars.Controls.Add(this.txtCustomChars, 0, 1);
			this._tableLayoutCustomChars.Controls.Add(this.lblExampleDesc1, 0, 2);
			this._tableLayoutCustomChars.Controls.Add(this.txtExampleInput, 0, 3);
			this._tableLayoutCustomChars.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutCustomChars.Location = new System.Drawing.Point(15, 21);
			this._tableLayoutCustomChars.Name = "_tableLayoutCustomChars";
			this._tableLayoutCustomChars.RowCount = 6;
			this._tableLayoutCustomChars.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutCustomChars.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutCustomChars.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutCustomChars.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutCustomChars.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutCustomChars.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutCustomChars.Size = new System.Drawing.Size(235, 293);
			this._tableLayoutCustomChars.TabIndex = 10;
			// 
			// lblInstruction
			// 
			this.lblInstruction.AutoSize = true;
			this._tableLayoutCustomChars.SetColumnSpan(this.lblInstruction, 2);
			this.lblInstruction.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblInstruction, null);
			this.locExtender.SetLocalizationComment(this.lblInstruction, null);
			this.locExtender.SetLocalizingId(this.lblInstruction, "DialogBoxes.OptionsDlg.CVPatternsTab.InstructionLabel");
			this.lblInstruction.Location = new System.Drawing.Point(0, 0);
			this.lblInstruction.Margin = new System.Windows.Forms.Padding(0);
			this.lblInstruction.Name = "lblInstruction";
			this.lblInstruction.Size = new System.Drawing.Size(226, 52);
			this.lblInstruction.TabIndex = 2;
			this.lblInstruction.Text = "To include characters not shown in the lists to the left, enter them below. Inclu" +
    "de a space between each character. Include a diacritic placeholder (Ctrl+0) for " +
    "non base characters.";
			// 
			// lblExampleCV
			// 
			this.lblExampleCV.AutoSize = true;
			this.lblExampleCV.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.lblExampleCV.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExampleCV, null);
			this.locExtender.SetLocalizationComment(this.lblExampleCV, null);
			this.locExtender.SetLocalizingId(this.lblExampleCV, "DialogBoxes.OptionsDlg.CVPatternsTab.ExampleCVLabel");
			this.lblExampleCV.Location = new System.Drawing.Point(107, 195);
			this.lblExampleCV.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
			this.lblExampleCV.Name = "lblExampleCV";
			this.lblExampleCV.Size = new System.Drawing.Size(46, 24);
			this.lblExampleCV.TabIndex = 6;
			this.lblExampleCV.Text = "CVʔ";
			this.lblExampleCV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblExampleCV.UseMnemonic = false;
			// 
			// lblExampleDesc2
			// 
			this.lblExampleDesc2.AutoSize = true;
			this._tableLayoutCustomChars.SetColumnSpan(this.lblExampleDesc2, 2);
			this.lblExampleDesc2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExampleDesc2, null);
			this.locExtender.SetLocalizationComment(this.lblExampleDesc2, null);
			this.locExtender.SetLocalizingId(this.lblExampleDesc2, "DialogBoxes.OptionsDlg.CVPatternsTab.ExampleDesc2Label");
			this.lblExampleDesc2.Location = new System.Drawing.Point(0, 176);
			this.lblExampleDesc2.Margin = new System.Windows.Forms.Padding(0, 15, 3, 0);
			this.lblExampleDesc2.Name = "lblExampleDesc2";
			this.lblExampleDesc2.Size = new System.Drawing.Size(62, 13);
			this.lblExampleDesc2.TabIndex = 9;
			this.lblExampleDesc2.Text = "would yield:";
			// 
			// lblExampleCVCV
			// 
			this.lblExampleCVCV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblExampleCVCV.AutoSize = true;
			this.lblExampleCVCV.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.lblExampleCVCV.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExampleCVCV, null);
			this.locExtender.SetLocalizationComment(this.lblExampleCVCV, null);
			this.locExtender.SetLocalizingId(this.lblExampleCVCV, "DialogBoxes.OptionsDlg.CVPatternsTab.ExampleCVCVLabel");
			this.lblExampleCVCV.Location = new System.Drawing.Point(39, 195);
			this.lblExampleCVCV.Margin = new System.Windows.Forms.Padding(0, 6, 3, 0);
			this.lblExampleCVCV.Name = "lblExampleCVCV";
			this.lblExampleCVCV.Size = new System.Drawing.Size(62, 24);
			this.lblExampleCVCV.TabIndex = 7;
			this.lblExampleCVCV.Text = "CṼCV";
			this.lblExampleCVCV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblExampleCVCV.UseMnemonic = false;
			// 
			// txtCustomChars
			// 
			this.txtCustomChars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutCustomChars.SetColumnSpan(this.txtCustomChars, 2);
			this.txtCustomChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.locExtender.SetLocalizableToolTip(this.txtCustomChars, null);
			this.locExtender.SetLocalizationComment(this.txtCustomChars, null);
			this.locExtender.SetLocalizationPriority(this.txtCustomChars, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtCustomChars, "CVPatternsOptionsPage.txtCustomChars");
			this.txtCustomChars.Location = new System.Drawing.Point(0, 57);
			this.txtCustomChars.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.txtCustomChars.Name = "txtCustomChars";
			this.txtCustomChars.Size = new System.Drawing.Size(235, 29);
			this.txtCustomChars.TabIndex = 0;
			// 
			// lblExampleDesc1
			// 
			this.lblExampleDesc1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblExampleDesc1.AutoSize = true;
			this._tableLayoutCustomChars.SetColumnSpan(this.lblExampleDesc1, 2);
			this.lblExampleDesc1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExampleDesc1, null);
			this.locExtender.SetLocalizationComment(this.lblExampleDesc1, null);
			this.locExtender.SetLocalizingId(this.lblExampleDesc1, "DialogBoxes.OptionsDlg.CVPatternsTab.ExampleDesc1Label");
			this.lblExampleDesc1.Location = new System.Drawing.Point(0, 101);
			this.lblExampleDesc1.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this.lblExampleDesc1.Name = "lblExampleDesc1";
			this.lblExampleDesc1.Size = new System.Drawing.Size(235, 26);
			this.lblExampleDesc1.TabIndex = 3;
			this.lblExampleDesc1.Text = "Examples: Entering nasalization and a glottal stop like this above,";
			this.lblExampleDesc1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// txtExampleInput
			// 
			this.txtExampleInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutCustomChars.SetColumnSpan(this.txtExampleInput, 2);
			this.txtExampleInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.locExtender.SetLocalizableToolTip(this.txtExampleInput, null);
			this.locExtender.SetLocalizationComment(this.txtExampleInput, null);
			this.locExtender.SetLocalizationPriority(this.txtExampleInput, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtExampleInput, "CVPatternsOptionsPage.txtExampleInput");
			this.txtExampleInput.Location = new System.Drawing.Point(0, 132);
			this.txtExampleInput.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.txtExampleInput.Name = "txtExampleInput";
			this.txtExampleInput.ReadOnly = true;
			this.txtExampleInput.Size = new System.Drawing.Size(235, 29);
			this.txtExampleInput.TabIndex = 8;
			this.txtExampleInput.Text = "◌̃ ʔ";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// CVPatternsOptionsPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this._tableLayoutOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "CVPatternsOptionsPage.CVPatternsOptionsPage");
			this.Name = "CVPatternsOptionsPage";
			this.Size = new System.Drawing.Size(538, 329);
			this._tableLayoutOuter.ResumeLayout(false);
			this.grpDisplayChars.ResumeLayout(false);
			this._tableLayoutCustomChars.ResumeLayout(false);
			this._tableLayoutCustomChars.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		private Controls.CVPatternDisplaySymbolsExplorer _cvPatternSymbolExplorer;
		private System.Windows.Forms.GroupBox grpDisplayChars;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutCustomChars;
		private System.Windows.Forms.Label lblInstruction;
		private System.Windows.Forms.Label lblExampleCV;
		private System.Windows.Forms.Label lblExampleDesc2;
		private System.Windows.Forms.Label lblExampleCVCV;
		private System.Windows.Forms.TextBox txtCustomChars;
		private System.Windows.Forms.Label lblExampleDesc1;
		private System.Windows.Forms.TextBox txtExampleInput;
		protected L10NSharp.UI.L10NSharpExtender locExtender;
	}
}
