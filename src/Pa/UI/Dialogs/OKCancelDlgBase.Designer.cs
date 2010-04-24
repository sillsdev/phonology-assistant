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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OKCancelDlgBase));
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.locExtender = new SIL.Localization.LocalizationExtender(this.components);
			this.tblLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this.pnlButtons = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.tblLayoutButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Help button text on all OK/Cancel dialog boxes.");
			this.locExtender.SetLocalizingId(this.btnHelp, "OKCancelDlgBase.btnHelp");
			this.btnHelp.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.InternalHandleHelpClick);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, "Cancel button text on all OK/Cancel dialog boxes.");
			this.locExtender.SetLocalizingId(this.btnCancel, "OKCancelDlgBase.btnCancel");
			this.btnCancel.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, "OK button text on all OK/Cancel dialog boxes.");
			this.locExtender.SetLocalizingId(this.btnOK, "OKCancelDlgBase.btnOK");
			this.btnOK.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// tblLayoutButtons
			// 
			resources.ApplyResources(this.tblLayoutButtons, "tblLayoutButtons");
			this.tblLayoutButtons.Controls.Add(this.pnlButtons, 0, 0);
			this.tblLayoutButtons.Controls.Add(this.btnOK, 1, 0);
			this.tblLayoutButtons.Controls.Add(this.btnCancel, 2, 0);
			this.tblLayoutButtons.Controls.Add(this.btnHelp, 3, 0);
			this.tblLayoutButtons.Name = "tblLayoutButtons";
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.Red;
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// OKCancelDlgBase
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.tblLayoutButtons);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "OKCancelDlgBase.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OKCancelDlgBase";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.tblLayoutButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.Button btnCancel;
		protected System.Windows.Forms.Button btnOK;
		protected System.Windows.Forms.Button btnHelp;
		private SIL.Localization.LocalizationExtender locExtender;
		protected System.Windows.Forms.TableLayoutPanel tblLayoutButtons;
		protected System.Windows.Forms.Panel pnlButtons;



	}
}