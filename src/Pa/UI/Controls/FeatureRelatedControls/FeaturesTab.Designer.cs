namespace SIL.Pa.UI.Controls
{
	partial class FeaturesTab
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._tabCtrl = new System.Windows.Forms.TabControl();
			this.tpgAFeatures = new System.Windows.Forms.TabPage();
			this.tblLayoutAFeatures = new System.Windows.Forms.TableLayoutPanel();
			this.lblAFeatures = new System.Windows.Forms.Label();
			this.tpgBFeatures = new System.Windows.Forms.TabPage();
			this._tabCtrl.SuspendLayout();
			this.tpgAFeatures.SuspendLayout();
			this.tblLayoutAFeatures.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tabCtrl
			// 
			this._tabCtrl.Controls.Add(this.tpgAFeatures);
			this._tabCtrl.Controls.Add(this.tpgBFeatures);
			this._tabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabCtrl.Location = new System.Drawing.Point(0, 0);
			this._tabCtrl.Name = "_tabCtrl";
			this._tabCtrl.SelectedIndex = 0;
			this._tabCtrl.Size = new System.Drawing.Size(432, 311);
			this._tabCtrl.TabIndex = 1;
			// 
			// tpgAFeatures
			// 
			this.tpgAFeatures.Controls.Add(this.tblLayoutAFeatures);
			this.tpgAFeatures.Location = new System.Drawing.Point(4, 22);
			this.tpgAFeatures.Name = "tpgAFeatures";
			this.tpgAFeatures.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.tpgAFeatures.Size = new System.Drawing.Size(424, 285);
			this.tpgAFeatures.TabIndex = 0;
			this.tpgAFeatures.Text = "Articulatory Features";
			this.tpgAFeatures.UseVisualStyleBackColor = true;
			// 
			// tblLayoutAFeatures
			// 
			this.tblLayoutAFeatures.AutoSize = true;
			this.tblLayoutAFeatures.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblLayoutAFeatures.ColumnCount = 1;
			this.tblLayoutAFeatures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutAFeatures.Controls.Add(this.lblAFeatures, 0, 0);
			this.tblLayoutAFeatures.Dock = System.Windows.Forms.DockStyle.Top;
			this.tblLayoutAFeatures.Location = new System.Drawing.Point(3, 3);
			this.tblLayoutAFeatures.Name = "tblLayoutAFeatures";
			this.tblLayoutAFeatures.RowCount = 1;
			this.tblLayoutAFeatures.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutAFeatures.Size = new System.Drawing.Size(418, 26);
			this.tblLayoutAFeatures.TabIndex = 0;
			this.tblLayoutAFeatures.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutPaint);
			// 
			// lblAFeatures
			// 
			this.lblAFeatures.AutoSize = true;
			this.lblAFeatures.Location = new System.Drawing.Point(3, 3);
			this.lblAFeatures.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
			this.lblAFeatures.Name = "lblAFeatures";
			this.lblAFeatures.Size = new System.Drawing.Size(14, 13);
			this.lblAFeatures.TabIndex = 0;
			this.lblAFeatures.Text = "#";
			// 
			// tpgBFeatures
			// 
			this.tpgBFeatures.Location = new System.Drawing.Point(4, 22);
			this.tpgBFeatures.Name = "tpgBFeatures";
			this.tpgBFeatures.Padding = new System.Windows.Forms.Padding(3, 8, 3, 0);
			this.tpgBFeatures.Size = new System.Drawing.Size(424, 285);
			this.tpgBFeatures.TabIndex = 1;
			this.tpgBFeatures.Text = "Binary Features";
			this.tpgBFeatures.UseVisualStyleBackColor = true;
			// 
			// FeaturesTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._tabCtrl);
			this.Name = "FeaturesTab";
			this.Size = new System.Drawing.Size(432, 311);
			this._tabCtrl.ResumeLayout(false);
			this.tpgAFeatures.ResumeLayout(false);
			this.tpgAFeatures.PerformLayout();
			this.tblLayoutAFeatures.ResumeLayout(false);
			this.tblLayoutAFeatures.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl _tabCtrl;
		private System.Windows.Forms.TabPage tpgAFeatures;
		private System.Windows.Forms.TableLayoutPanel tblLayoutAFeatures;
		private System.Windows.Forms.Label lblAFeatures;
		private System.Windows.Forms.TabPage tpgBFeatures;
	}
}
