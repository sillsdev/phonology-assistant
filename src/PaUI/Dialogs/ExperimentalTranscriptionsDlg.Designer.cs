using SilUtils.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class ExperimentalTranscriptionsDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExperimentalTranscriptionsDlg));
			this.pnlGrid = new SilUtils.Controls.SilPanel();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
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
			this.locExtender.SetLocalizingId(this.btnCancel, "Localized in Base Class");
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizationPriority(this.btnOK, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnOK, "Localized in Base Class");
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizationPriority(this.btnHelp, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnHelp, "Localized in Base Class");
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlGrid, "pnlGrid");
			this.pnlGrid.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
			this.locExtender.SetLocalizationComment(this.pnlGrid, null);
			this.locExtender.SetLocalizationPriority(this.pnlGrid, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlGrid, "ExperimentalTranscriptionsDlg.pnlGrid");
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// ExperimentalTranscriptionsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlGrid);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "ExperimentalTranscriptionsDlg.WindowTitle");
			this.Name = "ExperimentalTranscriptionsDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.pnlButtons.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilPanel pnlGrid;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;


	}
}