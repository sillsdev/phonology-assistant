namespace SIL.Pa.Dialogs
{
	partial class FiltersDialog
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
			this.tvCatAndFldr = new System.Windows.Forms.TreeView();
			this.clbDialects = new System.Windows.Forms.CheckedListBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lvNames = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.lblFilters = new System.Windows.Forms.Label();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.lblCatAndFldrs = new System.Windows.Forms.Label();
			this.lblDialects = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvCatAndFldr
			// 
			this.tvCatAndFldr.AllowDrop = true;
			this.tvCatAndFldr.CheckBoxes = true;
			this.tvCatAndFldr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvCatAndFldr.Location = new System.Drawing.Point(0, 30);
			this.tvCatAndFldr.Name = "tvCatAndFldr";
			this.tvCatAndFldr.Size = new System.Drawing.Size(205, 318);
			this.tvCatAndFldr.TabIndex = 1;
			this.tvCatAndFldr.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvCatAndFldr_AfterCheck);
			// 
			// clbDialects
			// 
			this.clbDialects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clbDialects.FormattingEnabled = true;
			this.clbDialects.IntegralHeight = false;
			this.clbDialects.Location = new System.Drawing.Point(0, 30);
			this.clbDialects.Name = "clbDialects";
			this.clbDialects.Size = new System.Drawing.Size(172, 318);
			this.clbDialects.TabIndex = 1;
			this.clbDialects.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbDialects_ItemCheck);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lvNames);
			this.splitContainer1.Panel1.Controls.Add(this.lblFilters);
			this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(10, 30, 0, 0);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(576, 348);
			this.splitContainer1.SplitterDistance = 173;
			this.splitContainer1.SplitterWidth = 8;
			this.splitContainer1.TabIndex = 0;
			// 
			// lvNames
			// 
			this.lvNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lvNames.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvNames.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvNames.HideSelection = false;
			this.lvNames.LabelEdit = true;
			this.lvNames.LabelWrap = false;
			this.lvNames.Location = new System.Drawing.Point(10, 30);
			this.lvNames.MultiSelect = false;
			this.lvNames.Name = "lvNames";
			this.lvNames.ShowItemToolTips = true;
			this.lvNames.Size = new System.Drawing.Size(163, 318);
			this.lvNames.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvNames.TabIndex = 1;
			this.lvNames.UseCompatibleStateImageBehavior = false;
			this.lvNames.View = System.Windows.Forms.View.Details;
			this.lvNames.Resize += new System.EventHandler(this.lvNames_Resize);
			this.lvNames.SelectedIndexChanged += new System.EventHandler(this.lvNames_SelectedIndexChanged);
			this.lvNames.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvNames_AfterLabelEdit);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Name = "columnHeader1";
			// 
			// lblFilters
			// 
			this.lblFilters.AutoEllipsis = true;
			this.lblFilters.AutoSize = true;
			this.lblFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFilters.Location = new System.Drawing.Point(11, 13);
			this.lblFilters.Name = "lblFilters";
			this.lblFilters.Size = new System.Drawing.Size(34, 13);
			this.lblFilters.TabIndex = 0;
			this.lblFilters.Text = "Filters";
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.tvCatAndFldr);
			this.splitContainer2.Panel1.Controls.Add(this.lblCatAndFldrs);
			this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.clbDialects);
			this.splitContainer2.Panel2.Controls.Add(this.lblDialects);
			this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(0, 30, 10, 0);
			this.splitContainer2.Size = new System.Drawing.Size(395, 348);
			this.splitContainer2.SplitterDistance = 205;
			this.splitContainer2.SplitterWidth = 8;
			this.splitContainer2.TabIndex = 0;
			// 
			// lblCatAndFldrs
			// 
			this.lblCatAndFldrs.AutoEllipsis = true;
			this.lblCatAndFldrs.AutoSize = true;
			this.lblCatAndFldrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCatAndFldrs.Location = new System.Drawing.Point(1, 13);
			this.lblCatAndFldrs.Name = "lblCatAndFldrs";
			this.lblCatAndFldrs.Size = new System.Drawing.Size(115, 13);
			this.lblCatAndFldrs.TabIndex = 0;
			this.lblCatAndFldrs.Text = "Categories and Folders";
			// 
			// lblDialects
			// 
			this.lblDialects.AutoEllipsis = true;
			this.lblDialects.AutoSize = true;
			this.lblDialects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDialects.Location = new System.Drawing.Point(1, 13);
			this.lblDialects.Name = "lblDialects";
			this.lblDialects.Size = new System.Drawing.Size(45, 13);
			this.lblDialects.TabIndex = 0;
			this.lblDialects.Text = "Dialects";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(400, 355);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(80, 26);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(486, 355);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 26);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Location = new System.Drawing.Point(10, 355);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(80, 26);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "&Add";
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Location = new System.Drawing.Point(96, 355);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(80, 26);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "&Delete";
			// 
			// FiltersDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(576, 388);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.splitContainer1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(375, 270);
			this.Name = "FiltersDialog";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 40);
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Filters";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView tvCatAndFldr;
		private System.Windows.Forms.CheckedListBox clbDialects;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Label lblCatAndFldrs;
		private System.Windows.Forms.Label lblFilters;
		private System.Windows.Forms.Label lblDialects;
		private System.Windows.Forms.ListView lvNames;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}