namespace SIL.Pa.DataSource
{
	partial class MissingDataSourceMsgBox
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
			this.lblMsg = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.btnNewLoc = new System.Windows.Forms.Button();
			this.btnSkip = new System.Windows.Forms.Button();
			this.lblFileName = new System.Windows.Forms.Label();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnHelp = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMsg
			// 
			this.lblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblMsg.AutoEllipsis = true;
			this.lblMsg.BackColor = System.Drawing.Color.Transparent;
			this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.lblMsg, null);
			this.locExtender.SetLocalizationComment(this.lblMsg, null);
			this.locExtender.SetLocalizingId(this.lblMsg, "MissingDataSourceMsgBox.lblMsg");
			this.lblMsg.Location = new System.Drawing.Point(76, 20);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(371, 39);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "The following data source file could not be found. What would you like to do?";
			// 
			// picIcon
			// 
			this.locExtender.SetLocalizableToolTip(this.picIcon, null);
			this.locExtender.SetLocalizationComment(this.picIcon, null);
			this.locExtender.SetLocalizationPriority(this.picIcon, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.picIcon, "MissingDataSourceMsgBox.picIcon");
			this.picIcon.Location = new System.Drawing.Point(24, 20);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(32, 32);
			this.picIcon.TabIndex = 1;
			this.picIcon.TabStop = false;
			// 
			// btnNewLoc
			// 
			this.btnNewLoc.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnNewLoc.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this.btnNewLoc, null);
			this.locExtender.SetLocalizationComment(this.btnNewLoc, null);
			this.locExtender.SetLocalizingId(this.btnNewLoc, "MissingDataSourceMsgBox.btnNewLoc");
			this.btnNewLoc.Location = new System.Drawing.Point(46, 116);
			this.btnNewLoc.Name = "btnNewLoc";
			this.btnNewLoc.Size = new System.Drawing.Size(139, 26);
			this.btnNewLoc.TabIndex = 2;
			this.btnNewLoc.Text = "&Choose New Location";
			this.btnNewLoc.UseVisualStyleBackColor = true;
			this.btnNewLoc.Click += new System.EventHandler(this.HandleButtonClicked);
			// 
			// btnSkip
			// 
			this.btnSkip.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSkip.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnSkip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnSkip, null);
			this.locExtender.SetLocalizationComment(this.btnSkip, null);
			this.locExtender.SetLocalizingId(this.btnSkip, "MissingDataSourceMsgBox.btnSkip");
			this.btnSkip.Location = new System.Drawing.Point(191, 116);
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.Size = new System.Drawing.Size(139, 26);
			this.btnSkip.TabIndex = 3;
			this.btnSkip.Text = "&Skip Loading";
			this.btnSkip.UseVisualStyleBackColor = true;
			this.btnSkip.Click += new System.EventHandler(this.HandleButtonClicked);
			// 
			// lblFileName
			// 
			this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblFileName.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblFileName, null);
			this.locExtender.SetLocalizationComment(this.lblFileName, null);
			this.locExtender.SetLocalizationPriority(this.lblFileName, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblFileName, "MissingDataSourceMsgBox.lblFileName");
			this.lblFileName.Location = new System.Drawing.Point(76, 68);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(371, 25);
			this.lblFileName.TabIndex = 4;
			this.lblFileName.Text = "#";
			this.lblFileName.Paint += new System.Windows.Forms.PaintEventHandler(this.lblFileName_Paint);
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizingId(this.btnHelp, "MissingDataSourceMsgBox.btnHelp");
			this.btnHelp.Location = new System.Drawing.Point(336, 116);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(87, 26);
			this.btnHelp.TabIndex = 6;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// MissingDataSourceMsgBox
			// 
			this.AcceptButton = this.btnNewLoc;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnSkip;
			this.ClientSize = new System.Drawing.Size(469, 154);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.btnSkip);
			this.Controls.Add(this.btnNewLoc);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.lblMsg);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "MissingDataSourceMsgBox.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MissingDataSourceMsgBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Data Source Missing";
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Button btnNewLoc;
		private System.Windows.Forms.Button btnSkip;
		private System.Windows.Forms.Label lblFileName;
		private System.Windows.Forms.ToolTip m_toolTip;
		private System.Windows.Forms.Button btnHelp;
		private Localization.UI.LocalizationExtender locExtender;
	}
}