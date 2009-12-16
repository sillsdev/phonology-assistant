namespace SIL.Pa.UI.Dialogs
{
	partial class SaveXYChartDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveXYChartDlg));
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.tlpName = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
			this.tlpName.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			// 
			// btnCancel
			// 
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizationPriority(this.btnCancel, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnCancel, "Localized in base class");
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizationPriority(this.btnOK, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnOK, "Localized in base class");
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizationPriority(this.btnHelp, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnHelp, "Localized in base class");
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// txtName
			// 
			resources.ApplyResources(this.txtName, "txtName");
			this.locExtender.SetLocalizableToolTip(this.txtName, null);
			this.locExtender.SetLocalizationComment(this.txtName, null);
			this.locExtender.SetLocalizationPriority(this.txtName, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtName, "SaveXYChartDlg.txtName");
			this.txtName.Name = "txtName";
			this.txtName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblName, null);
			this.locExtender.SetLocalizationComment(this.lblName, "Label on dialog box for saving XY charts in XY chart view.");
			this.locExtender.SetLocalizingId(this.lblName, "SaveXYChartDlg.lblName");
			this.lblName.Name = "lblName";
			// 
			// tlpName
			// 
			resources.ApplyResources(this.tlpName, "tlpName");
			this.tlpName.Controls.Add(this.lblName, 0, 0);
			this.tlpName.Controls.Add(this.txtName, 1, 0);
			this.locExtender.SetLocalizableToolTip(this.tlpName, null);
			this.locExtender.SetLocalizationComment(this.tlpName, null);
			this.locExtender.SetLocalizingId(this.tlpName, "SaveXYChartDlg.tlpName");
			this.tlpName.Name = "tlpName";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// SaveXYChartDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlpName);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SaveXYChartDlg.WindowTitle");
			this.Name = "SaveXYChartDlg";
			this.Controls.SetChildIndex(this.tlpName, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.pnlButtons.ResumeLayout(false);
			this.tlpName.ResumeLayout(false);
			this.tlpName.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TableLayoutPanel tlpName;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}