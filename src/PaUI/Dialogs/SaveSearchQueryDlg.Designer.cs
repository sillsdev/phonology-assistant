namespace SIL.Pa.UI.Dialogs
{
	partial class SaveSearchQueryDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveSearchQueryDlg));
			this.cboCategories = new System.Windows.Forms.ComboBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblPattern = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblCategories = new System.Windows.Forms.Label();
			this.lblPatternLabel = new System.Windows.Forms.Label();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.pnlButtons.SuspendLayout();
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
			// cboCategories
			// 
			resources.ApplyResources(this.cboCategories, "cboCategories");
			this.cboCategories.DropDownHeight = 200;
			this.cboCategories.FormattingEnabled = true;
			this.cboCategories.Name = "cboCategories";
			this.cboCategories.Sorted = true;
			this.m_toolTip.SetToolTip(this.cboCategories, resources.GetString("cboCategories.ToolTip"));
			this.cboCategories.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtName
			// 
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			this.m_toolTip.SetToolTip(this.txtName, resources.GetString("txtName.ToolTip"));
			this.txtName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblPattern
			// 
			this.lblPattern.AutoEllipsis = true;
			resources.ApplyResources(this.lblPattern, "lblPattern");
			this.lblPattern.BackColor = System.Drawing.Color.Transparent;
			this.lblPattern.Name = "lblPattern";
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			this.lblName.Name = "lblName";
			// 
			// lblCategories
			// 
			resources.ApplyResources(this.lblCategories, "lblCategories");
			this.lblCategories.BackColor = System.Drawing.Color.Transparent;
			this.lblCategories.Name = "lblCategories";
			// 
			// lblPatternLabel
			// 
			resources.ApplyResources(this.lblPatternLabel, "lblPatternLabel");
			this.lblPatternLabel.BackColor = System.Drawing.Color.Transparent;
			this.lblPatternLabel.Name = "lblPatternLabel";
			// 
			// SaveSearchQueryDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblPattern);
			this.Controls.Add(this.lblPatternLabel);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.cboCategories);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblCategories);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "SaveSearchQueryDlg";
			this.Controls.SetChildIndex(this.lblCategories, 0);
			this.Controls.SetChildIndex(this.txtName, 0);
			this.Controls.SetChildIndex(this.cboCategories, 0);
			this.Controls.SetChildIndex(this.lblName, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.lblPatternLabel, 0);
			this.Controls.SetChildIndex(this.lblPattern, 0);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboCategories;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblPattern;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblCategories;
		private System.Windows.Forms.Label lblPatternLabel;
		private System.Windows.Forms.ToolTip m_toolTip;
	}
}