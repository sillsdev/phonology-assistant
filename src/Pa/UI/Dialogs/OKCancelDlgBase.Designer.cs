namespace SIL.Pa.UI.Dialogs
{
	partial class OKCancelDlgBase
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
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tblLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.tblLayoutButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp.AutoSize = true;
			this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Help button text on all OK/Cancel dialog boxes.");
			this.locExtender.SetLocalizingId(this.btnHelp, "OKCancelDlgBase.btnHelp");
			this.btnHelp.Location = new System.Drawing.Point(300, 7);
			this.btnHelp.Margin = new System.Windows.Forms.Padding(5, 7, 0, 7);
			this.btnHelp.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(80, 26);
			this.btnHelp.TabIndex = 3;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.InternalHandleHelpClick);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.AutoSize = true;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, "Cancel button text on all OK/Cancel dialog boxes.");
			this.locExtender.SetLocalizingId(this.btnCancel, "OKCancelDlgBase.btnCancel");
			this.btnCancel.Location = new System.Drawing.Point(215, 7);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 7, 0, 7);
			this.btnCancel.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 26);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.HandleCancelClick);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.AutoSize = true;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, "OK button text on all OK/Cancel dialog boxes.");
			this.locExtender.SetLocalizingId(this.btnOK, "OKCancelDlgBase.btnOK");
			this.btnOK.Location = new System.Drawing.Point(130, 7);
			this.btnOK.Margin = new System.Windows.Forms.Padding(5, 7, 0, 7);
			this.btnOK.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 26);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.HandleOKButtonClick);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// tblLayoutButtons
			// 
			this.tblLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblLayoutButtons.ColumnCount = 4;
			this.tblLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutButtons.Controls.Add(this.btnOK, 1, 0);
			this.tblLayoutButtons.Controls.Add(this.btnCancel, 2, 0);
			this.tblLayoutButtons.Controls.Add(this.btnHelp, 3, 0);
			this.tblLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tblLayoutButtons.Location = new System.Drawing.Point(10, 231);
			this.tblLayoutButtons.Name = "tblLayoutButtons";
			this.tblLayoutButtons.RowCount = 1;
			this.tblLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutButtons.Size = new System.Drawing.Size(380, 40);
			this.tblLayoutButtons.TabIndex = 102;
			// 
			// OKCancelDlgBase
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(400, 271);
			this.Controls.Add(this.tblLayoutButtons);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "OKCancelDlgBase.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OKCancelDlgBase";
			this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "OKCancelDlgBase";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.tblLayoutButtons.ResumeLayout(false);
			this.tblLayoutButtons.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Button btnCancel;
		protected System.Windows.Forms.Button btnOK;
		protected System.Windows.Forms.Button btnHelp;
		private Localization.UI.LocalizationExtender locExtender;
		protected System.Windows.Forms.TableLayoutPanel tblLayoutButtons;



	}
}