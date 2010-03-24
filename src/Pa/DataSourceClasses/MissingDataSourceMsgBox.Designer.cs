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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingDataSourceMsgBox));
			this.lblMsg = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.btnNewLoc = new System.Windows.Forms.Button();
			this.btnSkip = new System.Windows.Forms.Button();
			this.lblFileName = new System.Windows.Forms.Label();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnHelp = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMsg
			// 
			resources.ApplyResources(this.lblMsg, "lblMsg");
			this.lblMsg.AutoEllipsis = true;
			this.lblMsg.BackColor = System.Drawing.Color.Transparent;
			this.lblMsg.Name = "lblMsg";
			// 
			// picIcon
			// 
			resources.ApplyResources(this.picIcon, "picIcon");
			this.picIcon.Name = "picIcon";
			this.picIcon.TabStop = false;
			// 
			// btnNewLoc
			// 
			resources.ApplyResources(this.btnNewLoc, "btnNewLoc");
			this.btnNewLoc.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnNewLoc.Name = "btnNewLoc";
			this.btnNewLoc.UseVisualStyleBackColor = true;
			this.btnNewLoc.Click += new System.EventHandler(this.HandleButtonClicked);
			// 
			// btnSkip
			// 
			resources.ApplyResources(this.btnSkip, "btnSkip");
			this.btnSkip.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.UseVisualStyleBackColor = true;
			this.btnSkip.Click += new System.EventHandler(this.HandleButtonClicked);
			// 
			// lblFileName
			// 
			resources.ApplyResources(this.lblFileName, "lblFileName");
			this.lblFileName.BackColor = System.Drawing.Color.Transparent;
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Paint += new System.Windows.Forms.PaintEventHandler(this.lblFileName_Paint);
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// MissingDataSourceMsgBox
			// 
			this.AcceptButton = this.btnNewLoc;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnSkip;
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.btnSkip);
			this.Controls.Add(this.btnNewLoc);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.lblMsg);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MissingDataSourceMsgBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
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
	}
}