namespace SIL.Pa.UI.Dialogs
{
	partial class SaveDistributionChartDlg
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
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.tlpName = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tlpName.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.txtName, null);
			this.locExtender.SetLocalizationComment(this.txtName, null);
			this.locExtender.SetLocalizationPriority(this.txtName, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtName, "SaveDistributionChartDlg.txtName");
			this.txtName.Location = new System.Drawing.Point(48, 0);
			this.txtName.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(349, 20);
			this.txtName.TabIndex = 1;
			this.txtName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblName
			// 
			this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lblName.AutoSize = true;
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblName, null);
			this.locExtender.SetLocalizationComment(this.lblName, "Label on dialog box for saving distribution charts in distribution chart view.");
			this.locExtender.SetLocalizingId(this.lblName, "SaveDistributionChartDlg.lblName");
			this.lblName.Location = new System.Drawing.Point(0, 0);
			this.lblName.Margin = new System.Windows.Forms.Padding(0, 0, 4, 5);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(44, 20);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Na&me:";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tlpName
			// 
			this.tlpName.AutoSize = true;
			this.tlpName.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpName.ColumnCount = 2;
			this.tlpName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpName.Controls.Add(this.lblName, 0, 0);
			this.tlpName.Controls.Add(this.txtName, 1, 0);
			this.tlpName.Location = new System.Drawing.Point(10, 13);
			this.tlpName.Name = "tlpName";
			this.tlpName.RowCount = 1;
			this.tlpName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpName.Size = new System.Drawing.Size(397, 25);
			this.tlpName.TabIndex = 7;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidExportAsToolTip;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SaveDistributionChartDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(418, 87);
			this.Controls.Add(this.tlpName);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SaveDistributionChartDlg.WindowTitle");
			this.Name = "SaveDistributionChartDlg";
			this.Padding = new System.Windows.Forms.Padding(10, 13, 10, 0);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Save XY Chart";
			this.Controls.SetChildIndex(this.tlpName, 0);
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
		private Localization.UI.LocalizationExtender locExtender;
	}
}