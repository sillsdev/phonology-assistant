namespace SIL.Pa.FiltersAddOn
{
	partial class FiltersDropDownCtrl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lstFilters = new System.Windows.Forms.ListBox();
			this.lblFilters = new System.Windows.Forms.Label();
			this.lnkDefine = new System.Windows.Forms.LinkLabel();
			this.lnkApply = new System.Windows.Forms.LinkLabel();
			this.lnkCancel = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// lstFilters
			// 
			this.lstFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstFilters.FormattingEnabled = true;
			this.lstFilters.IntegralHeight = false;
			this.lstFilters.Location = new System.Drawing.Point(0, 24);
			this.lstFilters.Name = "lstFilters";
			this.lstFilters.Size = new System.Drawing.Size(150, 113);
			this.lstFilters.TabIndex = 1;
			this.lstFilters.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstFilters_MouseDoubleClick);
			this.lstFilters.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstFilters_KeyPress);
			// 
			// lblFilters
			// 
			this.lblFilters.BackColor = System.Drawing.Color.Transparent;
			this.lblFilters.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblFilters.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFilters.Location = new System.Drawing.Point(0, 0);
			this.lblFilters.Name = "lblFilters";
			this.lblFilters.Size = new System.Drawing.Size(150, 24);
			this.lblFilters.TabIndex = 0;
			this.lblFilters.Text = "Available Filters";
			this.lblFilters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lnkDefine
			// 
			this.lnkDefine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkDefine.AutoSize = true;
			this.lnkDefine.BackColor = System.Drawing.Color.Transparent;
			this.lnkDefine.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkDefine.Location = new System.Drawing.Point(0, 145);
			this.lnkDefine.Name = "lnkDefine";
			this.lnkDefine.Size = new System.Drawing.Size(54, 14);
			this.lnkDefine.TabIndex = 2;
			this.lnkDefine.TabStop = true;
			this.lnkDefine.Text = "Define...";
			this.lnkDefine.Click += new System.EventHandler(this.lnkDefine_Click);
			// 
			// lnkApply
			// 
			this.lnkApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lnkApply.AutoSize = true;
			this.lnkApply.BackColor = System.Drawing.Color.Transparent;
			this.lnkApply.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkApply.Location = new System.Drawing.Point(62, 145);
			this.lnkApply.Name = "lnkApply";
			this.lnkApply.Size = new System.Drawing.Size(37, 14);
			this.lnkApply.TabIndex = 3;
			this.lnkApply.TabStop = true;
			this.lnkApply.Text = "Apply";
			this.lnkApply.Click += new System.EventHandler(this.lnkApply_Click);
			// 
			// lnkCancel
			// 
			this.lnkCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkCancel.AutoSize = true;
			this.lnkCancel.BackColor = System.Drawing.Color.Transparent;
			this.lnkCancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkCancel.Location = new System.Drawing.Point(107, 145);
			this.lnkCancel.Name = "lnkCancel";
			this.lnkCancel.Size = new System.Drawing.Size(42, 14);
			this.lnkCancel.TabIndex = 4;
			this.lnkCancel.TabStop = true;
			this.lnkCancel.Text = "Cancel";
			this.lnkCancel.Click += new System.EventHandler(this.lnkCancel_Click);
			// 
			// FiltersDropDownCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.lnkCancel);
			this.Controls.Add(this.lnkApply);
			this.Controls.Add(this.lnkDefine);
			this.Controls.Add(this.lstFilters);
			this.Controls.Add(this.lblFilters);
			this.MinimumSize = new System.Drawing.Size(135, 85);
			this.Name = "FiltersDropDownCtrl";
			this.Size = new System.Drawing.Size(150, 165);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstFilters;
		private System.Windows.Forms.Label lblFilters;
		private System.Windows.Forms.LinkLabel lnkDefine;
		private System.Windows.Forms.LinkLabel lnkApply;
		private System.Windows.Forms.LinkLabel lnkCancel;
	}
}
