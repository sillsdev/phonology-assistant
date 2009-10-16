namespace SIL.Pa.AddOn
{
	partial class QueryResultOutputDlg
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
			this.m_grid = new System.Windows.Forms.DataGridView();
			this.tabCtrl = new System.Windows.Forms.TabControl();
			this.tpgData = new System.Windows.Forms.TabPage();
			this.tpgQuery = new System.Windows.Forms.TabPage();
			this.txtSQL = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.tabCtrl.SuspendLayout();
			this.tpgData.SuspendLayout();
			this.tpgQuery.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_grid
			// 
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grid.Location = new System.Drawing.Point(10, 10);
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.ReadOnly = true;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.Size = new System.Drawing.Size(785, 466);
			this.m_grid.TabIndex = 0;
			// 
			// tabCtrl
			// 
			this.tabCtrl.Controls.Add(this.tpgData);
			this.tabCtrl.Controls.Add(this.tpgQuery);
			this.tabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabCtrl.Location = new System.Drawing.Point(10, 10);
			this.tabCtrl.Name = "tabCtrl";
			this.tabCtrl.Padding = new System.Drawing.Point(10, 5);
			this.tabCtrl.SelectedIndex = 0;
			this.tabCtrl.Size = new System.Drawing.Size(813, 516);
			this.tabCtrl.TabIndex = 1;
			// 
			// tpgData
			// 
			this.tpgData.Controls.Add(this.m_grid);
			this.tpgData.Location = new System.Drawing.Point(4, 26);
			this.tpgData.Name = "tpgData";
			this.tpgData.Padding = new System.Windows.Forms.Padding(10);
			this.tpgData.Size = new System.Drawing.Size(805, 486);
			this.tpgData.TabIndex = 0;
			this.tpgData.Text = "Data from {0}";
			this.tpgData.UseVisualStyleBackColor = true;
			// 
			// tpgQuery
			// 
			this.tpgQuery.Controls.Add(this.txtSQL);
			this.tpgQuery.Location = new System.Drawing.Point(4, 26);
			this.tpgQuery.Name = "tpgQuery";
			this.tpgQuery.Padding = new System.Windows.Forms.Padding(10);
			this.tpgQuery.Size = new System.Drawing.Size(805, 486);
			this.tpgQuery.TabIndex = 1;
			this.tpgQuery.Text = "SQL Query";
			this.tpgQuery.UseVisualStyleBackColor = true;
			// 
			// txtSQL
			// 
			this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSQL.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSQL.Location = new System.Drawing.Point(10, 10);
			this.txtSQL.Multiline = true;
			this.txtSQL.Name = "txtSQL";
			this.txtSQL.ReadOnly = true;
			this.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtSQL.Size = new System.Drawing.Size(785, 466);
			this.txtSQL.TabIndex = 0;
			// 
			// QueryResultOutputDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(833, 536);
			this.Controls.Add(this.tabCtrl);
			this.Name = "QueryResultOutputDlg";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "QueryResultOutputDlg";
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.tabCtrl.ResumeLayout(false);
			this.tpgData.ResumeLayout(false);
			this.tpgQuery.ResumeLayout(false);
			this.tpgQuery.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView m_grid;
		private System.Windows.Forms.TabControl tabCtrl;
		private System.Windows.Forms.TabPage tpgData;
		private System.Windows.Forms.TabPage tpgQuery;
		private System.Windows.Forms.TextBox txtSQL;
	}
}