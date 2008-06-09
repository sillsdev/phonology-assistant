namespace SIL.Pa.AddOn
{
	partial class AddOnInfoDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddOnInfoDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.grid = new System.Windows.Forms.DataGridView();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.lblDisclaimer = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
			this.pnlButtons.Controls.Add(this.label1);
			this.pnlButtons.Controls.Add(this.btnCancel);
			this.pnlButtons.Controls.Add(this.btnOK);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// grid
			// 
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToDeleteRows = false;
			this.grid.AllowUserToResizeRows = false;
			this.grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle36.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle36.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle36.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle36;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colDescription,
            this.colFile,
            this.colVersion,
            this.colEnabled});
			dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle42.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle42.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle42.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle42.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			dataGridViewCellStyle42.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle42.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grid.DefaultCellStyle = dataGridViewCellStyle42;
			resources.ApplyResources(this.grid, "grid");
			this.grid.MultiSelect = false;
			this.grid.Name = "grid";
			this.grid.RowHeadersVisible = false;
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_CellPainting);
			this.grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grid_KeyPress);
			// 
			// colName
			// 
			dataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle37.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.colName.DefaultCellStyle = dataGridViewCellStyle37;
			resources.ApplyResources(this.colName, "colName");
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colDescription
			// 
			dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle38.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.colDescription.DefaultCellStyle = dataGridViewCellStyle38;
			resources.ApplyResources(this.colDescription, "colDescription");
			this.colDescription.Name = "colDescription";
			this.colDescription.ReadOnly = true;
			// 
			// colFile
			// 
			dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			this.colFile.DefaultCellStyle = dataGridViewCellStyle39;
			resources.ApplyResources(this.colFile, "colFile");
			this.colFile.Name = "colFile";
			this.colFile.ReadOnly = true;
			// 
			// colVersion
			// 
			dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			this.colVersion.DefaultCellStyle = dataGridViewCellStyle40;
			resources.ApplyResources(this.colVersion, "colVersion");
			this.colVersion.Name = "colVersion";
			this.colVersion.ReadOnly = true;
			// 
			// colEnabled
			// 
			this.colEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
			dataGridViewCellStyle41.NullValue = false;
			this.colEnabled.DefaultCellStyle = dataGridViewCellStyle41;
			resources.ApplyResources(this.colEnabled, "colEnabled");
			this.colEnabled.Name = "colEnabled";
			// 
			// lblDisclaimer
			// 
			this.lblDisclaimer.AutoEllipsis = true;
			resources.ApplyResources(this.lblDisclaimer, "lblDisclaimer");
			this.lblDisclaimer.Name = "lblDisclaimer";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.AutoEllipsis = true;
			this.label1.Name = "label1";
			// 
			// AddOnInfoDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.grid);
			this.Controls.Add(this.lblDisclaimer);
			this.Controls.Add(this.pnlButtons);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddOnInfoDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.pnlButtons.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnOK;
		protected System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.DataGridView grid;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
		private System.Windows.Forms.DataGridViewTextBoxColumn colFile;
		private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colEnabled;
		private System.Windows.Forms.Label lblDisclaimer;
		private System.Windows.Forms.Label label1;
	}
}