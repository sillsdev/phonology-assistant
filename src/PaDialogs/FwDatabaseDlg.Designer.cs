namespace SIL.Pa.Dialogs
{
	partial class FwDatabaseDlg
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
			this.lblMsg = new System.Windows.Forms.Label();
			this.lstFwDatabases = new System.Windows.Forms.ListBox();
			this.btnProperties = new System.Windows.Forms.Button();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnProperties);
			this.pnlButtons.Location = new System.Drawing.Point(10, 163);
			this.pnlButtons.Size = new System.Drawing.Size(360, 40);
			this.pnlButtons.TabIndex = 2;
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnProperties, 0);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(194, 7);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(108, 7);
			// 
			// btnHelp
			// 
			this.btnHelp.Location = new System.Drawing.Point(280, 7);
			// 
			// lblMsg
			// 
			this.lblMsg.AutoSize = true;
			this.lblMsg.Font = new System.Drawing.Font("Lucida Sans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMsg.Location = new System.Drawing.Point(11, 10);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(325, 15);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "&Choose the language project to use as a data source.";
			// 
			// lstFwDatabases
			// 
			this.lstFwDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstFwDatabases.FormattingEnabled = true;
			this.lstFwDatabases.IntegralHeight = false;
			this.lstFwDatabases.Location = new System.Drawing.Point(10, 32);
			this.lstFwDatabases.Name = "lstFwDatabases";
			this.lstFwDatabases.Size = new System.Drawing.Size(361, 127);
			this.lstFwDatabases.TabIndex = 1;
			// 
			// btnProperties
			// 
			this.btnProperties.Location = new System.Drawing.Point(18, 7);
			this.btnProperties.Name = "btnProperties";
			this.btnProperties.Size = new System.Drawing.Size(86, 26);
			this.btnProperties.TabIndex = 0;
			this.btnProperties.Text = "&Properties...";
			this.btnProperties.UseVisualStyleBackColor = true;
			this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
			// 
			// FwDatabaseDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(380, 203);
			this.Controls.Add(this.lblMsg);
			this.Controls.Add(this.lstFwDatabases);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FwDatabaseDlg";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Auto;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FieldWorks Language Projects";
			this.Controls.SetChildIndex(this.lstFwDatabases, 0);
			this.Controls.SetChildIndex(this.lblMsg, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.ListBox lstFwDatabases;
		private System.Windows.Forms.Button btnProperties;
	}
}
