namespace SIL.Pa.AddOn
{
	partial class FeedbackReportDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedbackReportDlg));
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSend = new System.Windows.Forms.Button();
			this.pnlSurveyOuter = new SIL.Pa.Controls.PaPanel();
			this.lbl5 = new System.Windows.Forms.Label();
			this.lbl4 = new System.Windows.Forms.Label();
			this.lbl3 = new System.Windows.Forms.Label();
			this.lbl2 = new System.Windows.Forms.Label();
			this.lbl1 = new System.Windows.Forms.Label();
			this.lblSurveyInstructions = new System.Windows.Forms.Label();
			this.pnlSurveyInner = new System.Windows.Forms.Panel();
			this.lblHeading = new System.Windows.Forms.Label();
			this.pnlComments = new SIL.Pa.Controls.PaPanel();
			this.txtComments = new System.Windows.Forms.TextBox();
			this.lblComments = new System.Windows.Forms.Label();
			this.pnlButtons.SuspendLayout();
			this.pnlSurveyOuter.SuspendLayout();
			this.pnlComments.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnCopy);
			this.pnlButtons.Controls.Add(this.btnClose);
			this.pnlButtons.Controls.Add(this.btnSend);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// btnCopy
			// 
			resources.ApplyResources(this.btnCopy, "btnCopy");
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// btnSend
			// 
			resources.ApplyResources(this.btnSend, "btnSend");
			this.btnSend.Name = "btnSend";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// pnlSurveyOuter
			// 
			resources.ApplyResources(this.pnlSurveyOuter, "pnlSurveyOuter");
			this.pnlSurveyOuter.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSurveyOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSurveyOuter.ClipTextForChildControls = false;
			this.pnlSurveyOuter.ControlReceivingFocusOnMnemonic = null;
			this.pnlSurveyOuter.Controls.Add(this.lbl5);
			this.pnlSurveyOuter.Controls.Add(this.lbl4);
			this.pnlSurveyOuter.Controls.Add(this.lbl3);
			this.pnlSurveyOuter.Controls.Add(this.lbl2);
			this.pnlSurveyOuter.Controls.Add(this.lbl1);
			this.pnlSurveyOuter.Controls.Add(this.lblSurveyInstructions);
			this.pnlSurveyOuter.Controls.Add(this.pnlSurveyInner);
			this.pnlSurveyOuter.DoubleBuffered = true;
			this.pnlSurveyOuter.MnemonicGeneratesClick = false;
			this.pnlSurveyOuter.Name = "pnlSurveyOuter";
			this.pnlSurveyOuter.PaintExplorerBarBackground = false;
			this.pnlSurveyOuter.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSurveyOuter_Paint);
			// 
			// lbl5
			// 
			resources.ApplyResources(this.lbl5, "lbl5");
			this.lbl5.Name = "lbl5";
			// 
			// lbl4
			// 
			resources.ApplyResources(this.lbl4, "lbl4");
			this.lbl4.Name = "lbl4";
			// 
			// lbl3
			// 
			resources.ApplyResources(this.lbl3, "lbl3");
			this.lbl3.Name = "lbl3";
			// 
			// lbl2
			// 
			resources.ApplyResources(this.lbl2, "lbl2");
			this.lbl2.Name = "lbl2";
			// 
			// lbl1
			// 
			resources.ApplyResources(this.lbl1, "lbl1");
			this.lbl1.Name = "lbl1";
			// 
			// lblSurveyInstructions
			// 
			resources.ApplyResources(this.lblSurveyInstructions, "lblSurveyInstructions");
			this.lblSurveyInstructions.Name = "lblSurveyInstructions";
			// 
			// pnlSurveyInner
			// 
			resources.ApplyResources(this.pnlSurveyInner, "pnlSurveyInner");
			this.pnlSurveyInner.Name = "pnlSurveyInner";
			this.pnlSurveyInner.Resize += new System.EventHandler(this.pnlSurveyInner_Resize);
			// 
			// lblHeading
			// 
			this.lblHeading.AutoEllipsis = true;
			resources.ApplyResources(this.lblHeading, "lblHeading");
			this.lblHeading.Name = "lblHeading";
			// 
			// pnlComments
			// 
			this.pnlComments.BackColor = System.Drawing.SystemColors.Window;
			this.pnlComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlComments.ClipTextForChildControls = false;
			this.pnlComments.ControlReceivingFocusOnMnemonic = null;
			this.pnlComments.Controls.Add(this.txtComments);
			this.pnlComments.Controls.Add(this.lblComments);
			resources.ApplyResources(this.pnlComments, "pnlComments");
			this.pnlComments.DoubleBuffered = true;
			this.pnlComments.MnemonicGeneratesClick = false;
			this.pnlComments.Name = "pnlComments";
			this.pnlComments.PaintExplorerBarBackground = false;
			this.pnlComments.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlComments_Paint);
			// 
			// txtComments
			// 
			this.txtComments.AcceptsReturn = true;
			this.txtComments.AcceptsTab = true;
			resources.ApplyResources(this.txtComments, "txtComments");
			this.txtComments.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtComments.HideSelection = false;
			this.txtComments.Name = "txtComments";
			// 
			// lblComments
			// 
			resources.ApplyResources(this.lblComments, "lblComments");
			this.lblComments.Name = "lblComments";
			// 
			// FeedbackReportDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.Controls.Add(this.pnlComments);
			this.Controls.Add(this.pnlSurveyOuter);
			this.Controls.Add(this.lblHeading);
			this.Controls.Add(this.pnlButtons);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FeedbackReportDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.pnlButtons.ResumeLayout(false);
			this.pnlSurveyOuter.ResumeLayout(false);
			this.pnlSurveyOuter.PerformLayout();
			this.pnlComments.ResumeLayout(false);
			this.pnlComments.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSend;
		private SIL.Pa.Controls.PaPanel pnlSurveyOuter;
		private System.Windows.Forms.Panel pnlSurveyInner;
		private System.Windows.Forms.Label lblSurveyInstructions;
		private System.Windows.Forms.Label lbl1;
		private System.Windows.Forms.Label lbl5;
		private System.Windows.Forms.Label lbl4;
		private System.Windows.Forms.Label lbl3;
		private System.Windows.Forms.Label lbl2;
		private System.Windows.Forms.Label lblHeading;
		private SIL.Pa.Controls.PaPanel pnlComments;
		private System.Windows.Forms.Label lblComments;
		private System.Windows.Forms.TextBox txtComments;
		private System.Windows.Forms.Button btnCopy;
	}
}