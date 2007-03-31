namespace SIL.Pa.Dialogs
{
	partial class PaProjectDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaProjectDlg));
			this.lblProjName = new System.Windows.Forms.Label();
			this.lblTranscriber = new System.Windows.Forms.Label();
			this.lblSpeaker = new System.Windows.Forms.Label();
			this.lblComments = new System.Windows.Forms.Label();
			this.txtProjName = new System.Windows.Forms.TextBox();
			this.txtTranscriber = new System.Windows.Forms.TextBox();
			this.txtSpeaker = new System.Windows.Forms.TextBox();
			this.txtComments = new System.Windows.Forms.TextBox();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnCustomFields = new System.Windows.Forms.Button();
			this.txtLanguage = new System.Windows.Forms.TextBox();
			this.lblLanguage = new System.Windows.Forms.Label();
			this.cmnuAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuAddOtherDataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuAddFwDataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlGrid = new SIL.Pa.Controls.PaPanel();
			this.pnlGridHdg = new SIL.Pa.Controls.PaGradientPanel();
			this.pnlButtons.SuspendLayout();
			this.cmnuAdd.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnCustomFields);
			this.pnlButtons.Controls.Add(this.btnAdd);
			this.pnlButtons.Controls.Add(this.btnRemove);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnRemove, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnAdd, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCustomFields, 0);
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
			// lblProjName
			// 
			resources.ApplyResources(this.lblProjName, "lblProjName");
			this.lblProjName.Name = "lblProjName";
			// 
			// lblTranscriber
			// 
			resources.ApplyResources(this.lblTranscriber, "lblTranscriber");
			this.lblTranscriber.Name = "lblTranscriber";
			// 
			// lblSpeaker
			// 
			resources.ApplyResources(this.lblSpeaker, "lblSpeaker");
			this.lblSpeaker.Name = "lblSpeaker";
			// 
			// lblComments
			// 
			resources.ApplyResources(this.lblComments, "lblComments");
			this.lblComments.Name = "lblComments";
			// 
			// txtProjName
			// 
			resources.ApplyResources(this.txtProjName, "txtProjName");
			this.txtProjName.Name = "txtProjName";
			this.txtProjName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtTranscriber
			// 
			resources.ApplyResources(this.txtTranscriber, "txtTranscriber");
			this.txtTranscriber.Name = "txtTranscriber";
			this.txtTranscriber.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtSpeaker
			// 
			resources.ApplyResources(this.txtSpeaker, "txtSpeaker");
			this.txtSpeaker.Name = "txtSpeaker";
			this.txtSpeaker.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtComments
			// 
			resources.ApplyResources(this.txtComments, "txtComments");
			this.txtComments.Name = "txtComments";
			this.txtComments.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// btnAdd
			// 
			this.btnAdd.Image = global::SIL.Pa.Dialogs.Properties.Resources.kimidButtonDropDownArrow;
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.m_toolTip.SetToolTip(this.btnAdd, resources.GetString("btnAdd.ToolTip"));
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			resources.ApplyResources(this.btnRemove, "btnRemove");
			this.btnRemove.Name = "btnRemove";
			this.m_toolTip.SetToolTip(this.btnRemove, resources.GetString("btnRemove.ToolTip"));
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnCustomFields
			// 
			resources.ApplyResources(this.btnCustomFields, "btnCustomFields");
			this.btnCustomFields.Name = "btnCustomFields";
			this.m_toolTip.SetToolTip(this.btnCustomFields, resources.GetString("btnCustomFields.ToolTip"));
			this.btnCustomFields.UseVisualStyleBackColor = true;
			this.btnCustomFields.Click += new System.EventHandler(this.btnCustomFields_Click);
			// 
			// txtLanguage
			// 
			resources.ApplyResources(this.txtLanguage, "txtLanguage");
			this.txtLanguage.Name = "txtLanguage";
			this.txtLanguage.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblLanguage
			// 
			resources.ApplyResources(this.lblLanguage, "lblLanguage");
			this.lblLanguage.Name = "lblLanguage";
			// 
			// cmnuAdd
			// 
			this.cmnuAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuAddOtherDataSource,
            this.cmnuAddFwDataSource});
			this.cmnuAdd.Name = "cmnuAdd";
			this.cmnuAdd.ShowImageMargin = false;
			resources.ApplyResources(this.cmnuAdd, "cmnuAdd");
			// 
			// cmnuAddOtherDataSource
			// 
			this.cmnuAddOtherDataSource.Name = "cmnuAddOtherDataSource";
			resources.ApplyResources(this.cmnuAddOtherDataSource, "cmnuAddOtherDataSource");
			this.cmnuAddOtherDataSource.Click += new System.EventHandler(this.cmnuAddOtherDataSource_Click);
			// 
			// cmnuAddFwDataSource
			// 
			this.cmnuAddFwDataSource.Name = "cmnuAddFwDataSource";
			resources.ApplyResources(this.cmnuAddFwDataSource, "cmnuAddFwDataSource");
			this.cmnuAddFwDataSource.Click += new System.EventHandler(this.cmnuAddFwDataSource_Click);
			// 
			// pnlGrid
			// 
			resources.ApplyResources(this.pnlGrid, "pnlGrid");
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.DoubleBuffered = false;
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			// 
			// pnlGridHdg
			// 
			resources.ApplyResources(this.pnlGridHdg, "pnlGridHdg");
			this.pnlGridHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGridHdg.ControlReceivingFocusOnMnemonic = null;
			this.pnlGridHdg.DoubleBuffered = true;
			this.pnlGridHdg.MakeDark = false;
			this.pnlGridHdg.MnemonicGeneratesClick = false;
			this.pnlGridHdg.Name = "pnlGridHdg";
			this.pnlGridHdg.PaintExplorerBarBackground = false;
			// 
			// PaProjectDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlGridHdg);
			this.Controls.Add(this.pnlGrid);
			this.Controls.Add(this.txtLanguage);
			this.Controls.Add(this.lblLanguage);
			this.Controls.Add(this.txtComments);
			this.Controls.Add(this.txtSpeaker);
			this.Controls.Add(this.txtTranscriber);
			this.Controls.Add(this.txtProjName);
			this.Controls.Add(this.lblComments);
			this.Controls.Add(this.lblSpeaker);
			this.Controls.Add(this.lblTranscriber);
			this.Controls.Add(this.lblProjName);
			this.Name = "PaProjectDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.lblProjName, 0);
			this.Controls.SetChildIndex(this.lblTranscriber, 0);
			this.Controls.SetChildIndex(this.lblSpeaker, 0);
			this.Controls.SetChildIndex(this.lblComments, 0);
			this.Controls.SetChildIndex(this.txtProjName, 0);
			this.Controls.SetChildIndex(this.txtTranscriber, 0);
			this.Controls.SetChildIndex(this.txtSpeaker, 0);
			this.Controls.SetChildIndex(this.txtComments, 0);
			this.Controls.SetChildIndex(this.lblLanguage, 0);
			this.Controls.SetChildIndex(this.txtLanguage, 0);
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.Controls.SetChildIndex(this.pnlGridHdg, 0);
			this.pnlButtons.ResumeLayout(false);
			this.cmnuAdd.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblProjName;
		private System.Windows.Forms.Label lblTranscriber;
		private System.Windows.Forms.Label lblSpeaker;
		private System.Windows.Forms.Label lblComments;
		private System.Windows.Forms.TextBox txtProjName;
		private System.Windows.Forms.TextBox txtTranscriber;
		private System.Windows.Forms.TextBox txtSpeaker;
		private System.Windows.Forms.TextBox txtComments;
		private System.Windows.Forms.ToolTip m_toolTip;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnCustomFields;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.TextBox txtLanguage;
		private System.Windows.Forms.Label lblLanguage;
		private System.Windows.Forms.ContextMenuStrip cmnuAdd;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddOtherDataSource;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddFwDataSource;
		private SIL.Pa.Controls.PaPanel pnlGrid;
		private SIL.Pa.Controls.PaGradientPanel pnlGridHdg;
	}
}