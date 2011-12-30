namespace SIL.Pa.UI.Dialogs
{
	partial class UndefinedCharactersInClassDlg
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
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblInfo = new System.Windows.Forms.Label();
			this.txtChars = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
			this.pnlButtons.Controls.Add(this.btnHelp);
			this.pnlButtons.Controls.Add(this.btnOK);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlButtons.Location = new System.Drawing.Point(10, 132);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(318, 42);
			this.pnlButtons.TabIndex = 2;
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Button on dialog box displaying a list of undefined characters that are found in " +
					"a phone class.");
			this.locExtender.SetLocalizingId(this.btnHelp, "DialogBoxes.UndefinedCharactersInClassDlg.HelpButton");
			this.btnHelp.Location = new System.Drawing.Point(162, 9);
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
			this.locExtender.SetLocalizationComment(this.btnOK, "Button on dialog box displaying a list of undefined characters that are found in " +
					"a phone class.");
			this.locExtender.SetLocalizingId(this.btnOK, "DialogBoxes.UndefinedCharactersInClassDlg.OKButton");
			this.btnOK.Location = new System.Drawing.Point(76, 9);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 26);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblInfo
			// 
			this.lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this.lblInfo, null);
			this.locExtender.SetLocalizationComment(this.lblInfo, "Information text on dialog box displaying a list of undefined characters that are" +
					" found in a phone class.");
			this.locExtender.SetLocalizingId(this.lblInfo, "DialogBoxes.UndefinedCharactersInClassDlg.InfoLabel");
			this.lblInfo.Location = new System.Drawing.Point(0, 0);
			this.lblInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(318, 54);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "The following characters were found in the members of your class, but they are no" +
				"t found in Phonology Assistant\'s phonetic character inventory. See Help for more" +
				" information.";
			this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtChars
			// 
			this.txtChars.BackColor = System.Drawing.SystemColors.Window;
			this.txtChars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this.txtChars, null);
			this.locExtender.SetLocalizationComment(this.txtChars, null);
			this.locExtender.SetLocalizationPriority(this.txtChars, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtChars, "UndefinedCharactersInClassDlg.txtChars");
			this.txtChars.Location = new System.Drawing.Point(0, 61);
			this.txtChars.Margin = new System.Windows.Forms.Padding(0);
			this.txtChars.Multiline = true;
			this.txtChars.Name = "txtChars";
			this.txtChars.ReadOnly = true;
			this.txtChars.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtChars.Size = new System.Drawing.Size(318, 61);
			this.txtChars.TabIndex = 3;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.lblInfo, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtChars, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(318, 122);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// UndefinedCharactersInClassDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOK;
			this.ClientSize = new System.Drawing.Size(338, 174);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.pnlButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.UndefinedCharactersInClassDlg.WindowTitle");
			this.Name = "UndefinedCharactersInClassDlg";
			this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Undefined Characters in Class";
			this.pnlButtons.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblInfo;
		protected System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.TextBox txtChars;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Localization.UI.LocalizationExtender locExtender;
	}
}