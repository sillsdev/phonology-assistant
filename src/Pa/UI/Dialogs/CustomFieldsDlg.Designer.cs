namespace SIL.Pa.UI.Dialogs
{
	partial class CustomFieldsDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomFieldsDlg));
			this.lblInfo = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.tblLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.locExtender.SetLocalizableToolTip(this.lblInfo, null);
			this.locExtender.SetLocalizationComment(this.lblInfo, "Text describing custom fields on the custom fields dialog box.");
			this.locExtender.SetLocalizingId(this.lblInfo, "CustomFieldsDlg.lblInfo");
			this.lblInfo.Name = "lblInfo";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// tblLayout
			// 
			resources.ApplyResources(this.tblLayout, "tblLayout");
			this.tblLayout.Controls.Add(this.lblInfo, 0, 0);
			this.tblLayout.Name = "tblLayout";
			// 
			// CustomFieldsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.tblLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "CustomFieldsDlg.WindowTitle");
			this.Name = "CustomFieldsDlg";
			this.Controls.SetChildIndex(this.tblLayout, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.tblLayout.ResumeLayout(false);
			this.tblLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblInfo;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
	}
}
