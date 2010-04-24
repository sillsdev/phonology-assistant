using SilUtils.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class TranscriptionChangesDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranscriptionChangesDlg));
			this.pnlGrid = new SilUtils.Controls.SilPanel();
			this.locExtender = new SIL.Localization.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
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
			this.locExtender.SetLocalizationPriority(this.pnlGrid, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlGrid, "TranscriptionChangesDlg.pnlGrid");
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// TranscriptionChangesDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlGrid);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "TranscriptionChangesDlg.WindowTitle");
			this.Name = "TranscriptionChangesDlg";
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SilPanel pnlGrid;
		private SIL.Localization.LocalizationExtender locExtender;
	}
}