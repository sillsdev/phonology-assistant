namespace SIL.Pa.Dialogs
{
	partial class FwDataSourcePropertiesDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FwDataSourcePropertiesDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblProjectValue = new System.Windows.Forms.Label();
			this.lblProject = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grpPhoneticDataStoreType = new System.Windows.Forms.GroupBox();
			this.rbPronunField = new System.Windows.Forms.RadioButton();
			this.rbLexForm = new System.Windows.Forms.RadioButton();
			this.grpWritingSystems = new System.Windows.Forms.GroupBox();
			this.m_grid = new SilUtils.SilGrid();
			this.pnlButtons.SuspendLayout();
			this.panel1.SuspendLayout();
			this.grpPhoneticDataStoreType.SuspendLayout();
			this.grpWritingSystems.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// lblProjectValue
			// 
			this.lblProjectValue.AutoEllipsis = true;
			resources.ApplyResources(this.lblProjectValue, "lblProjectValue");
			this.lblProjectValue.BackColor = System.Drawing.Color.Transparent;
			this.lblProjectValue.Name = "lblProjectValue";
			// 
			// lblProject
			// 
			resources.ApplyResources(this.lblProject, "lblProject");
			this.lblProject.BackColor = System.Drawing.Color.Transparent;
			this.lblProject.Name = "lblProject";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.grpPhoneticDataStoreType);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// grpPhoneticDataStoreType
			// 
			this.grpPhoneticDataStoreType.Controls.Add(this.rbPronunField);
			this.grpPhoneticDataStoreType.Controls.Add(this.rbLexForm);
			resources.ApplyResources(this.grpPhoneticDataStoreType, "grpPhoneticDataStoreType");
			this.grpPhoneticDataStoreType.Name = "grpPhoneticDataStoreType";
			this.grpPhoneticDataStoreType.TabStop = false;
			// 
			// rbPronunField
			// 
			resources.ApplyResources(this.rbPronunField, "rbPronunField");
			this.rbPronunField.Name = "rbPronunField";
			this.rbPronunField.TabStop = true;
			this.rbPronunField.UseVisualStyleBackColor = true;
			this.rbPronunField.CheckedChanged += new System.EventHandler(this.HandlePhoneticStorageTypeCheckedChanged);
			// 
			// rbLexForm
			// 
			resources.ApplyResources(this.rbLexForm, "rbLexForm");
			this.rbLexForm.Name = "rbLexForm";
			this.rbLexForm.TabStop = true;
			this.rbLexForm.UseVisualStyleBackColor = true;
			this.rbLexForm.CheckedChanged += new System.EventHandler(this.HandlePhoneticStorageTypeCheckedChanged);
			// 
			// grpWritingSystems
			// 
			resources.ApplyResources(this.grpWritingSystems, "grpWritingSystems");
			this.grpWritingSystems.Controls.Add(this.m_grid);
			this.grpWritingSystems.Name = "grpWritingSystems";
			this.grpWritingSystems.TabStop = false;
			// 
			// m_grid
			// 
			this.m_grid.AllowUserToAddRows = false;
			this.m_grid.AllowUserToDeleteRows = false;
			this.m_grid.AllowUserToOrderColumns = true;
			this.m_grid.AllowUserToResizeRows = false;
			this.m_grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			resources.ApplyResources(this.m_grid, "m_grid");
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_grid.IsDirty = false;
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersVisible = false;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.WaterMark = "";
			this.m_grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellEnter);
			// 
			// FwDataSourcePropertiesDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpWritingSystems);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblProject);
			this.Controls.Add(this.lblProjectValue);
			this.Name = "FwDataSourcePropertiesDlg";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Auto;
			this.Controls.SetChildIndex(this.lblProjectValue, 0);
			this.Controls.SetChildIndex(this.lblProject, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.panel1, 0);
			this.Controls.SetChildIndex(this.grpWritingSystems, 0);
			this.pnlButtons.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.grpPhoneticDataStoreType.ResumeLayout(false);
			this.grpPhoneticDataStoreType.PerformLayout();
			this.grpWritingSystems.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblProjectValue;
		private System.Windows.Forms.Label lblProject;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton rbPronunField;
		private System.Windows.Forms.RadioButton rbLexForm;
		private System.Windows.Forms.GroupBox grpPhoneticDataStoreType;
		private System.Windows.Forms.GroupBox grpWritingSystems;
		private SilUtils.SilGrid m_grid;
	}
}