namespace SIL.Pa
{
	partial class DefineClassDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefineClassDlg));
			this.pnlMemberPickingContainer = new System.Windows.Forms.Panel();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.splitPhoneViewers = new System.Windows.Forms.SplitContainer();
			this.splitCV = new System.Windows.Forms.SplitContainer();
			this.lvClasses = new SIL.Pa.Controls.ClassListView();
			this.charExplorer = new SIL.Pa.Controls.IPACharacterExplorer();
			this.txtClassName = new System.Windows.Forms.TextBox();
			this.lblClassName = new System.Windows.Forms.Label();
			this.lblClassType = new System.Windows.Forms.Label();
			this.pnlMembers = new SIL.Pa.Controls.PaGradientPanel();
			this.txtMembers = new System.Windows.Forms.TextBox();
			this.lblMembers = new System.Windows.Forms.Label();
			this.rdoOr = new System.Windows.Forms.RadioButton();
			this.rdoAnd = new System.Windows.Forms.RadioButton();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.lblClassTypeValue = new System.Windows.Forms.Label();
			this.pnlButtons.SuspendLayout();
			this.pnlMemberPickingContainer.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitPhoneViewers.Panel1.SuspendLayout();
			this.splitPhoneViewers.SuspendLayout();
			this.splitCV.SuspendLayout();
			this.pnlMembers.SuspendLayout();
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
			// pnlMemberPickingContainer
			// 
			resources.ApplyResources(this.pnlMemberPickingContainer, "pnlMemberPickingContainer");
			this.pnlMemberPickingContainer.BackColor = System.Drawing.SystemColors.Window;
			this.pnlMemberPickingContainer.Controls.Add(this.splitOuter);
			this.pnlMemberPickingContainer.Controls.Add(this.lvClasses);
			this.pnlMemberPickingContainer.Controls.Add(this.charExplorer);
			this.pnlMemberPickingContainer.Name = "pnlMemberPickingContainer";
			// 
			// splitOuter
			// 
			this.splitOuter.BackColor = System.Drawing.SystemColors.Control;
			resources.ApplyResources(this.splitOuter, "splitOuter");
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.splitPhoneViewers);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.BackColor = System.Drawing.SystemColors.Window;
			// 
			// splitPhoneViewers
			// 
			resources.ApplyResources(this.splitPhoneViewers, "splitPhoneViewers");
			this.splitPhoneViewers.Name = "splitPhoneViewers";
			// 
			// splitPhoneViewers.Panel1
			// 
			this.splitPhoneViewers.Panel1.Controls.Add(this.splitCV);
			// 
			// splitPhoneViewers.Panel2
			// 
			this.splitPhoneViewers.Panel2.BackColor = System.Drawing.SystemColors.Window;
			// 
			// splitCV
			// 
			resources.ApplyResources(this.splitCV, "splitCV");
			this.splitCV.Name = "splitCV";
			// 
			// splitCV.Panel1
			// 
			this.splitCV.Panel1.BackColor = System.Drawing.SystemColors.Window;
			// 
			// splitCV.Panel2
			// 
			this.splitCV.Panel2.BackColor = System.Drawing.SystemColors.Window;
			// 
			// lvClasses
			// 
			this.lvClasses.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.lvClasses.AppliesTo = SIL.Pa.Controls.ClassListView.ListApplicationType.DefineClassesDialog;
			resources.ApplyResources(this.lvClasses, "lvClasses");
			this.lvClasses.FullRowSelect = true;
			this.lvClasses.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups1"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups2"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups3"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups4"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups5"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups6"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups7"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups8"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups9"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups10"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups11"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups12"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups13"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups14"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups15"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups16"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups17"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups18"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups19"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups20"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups21"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups22"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups23"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups24"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups25"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups26"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups27"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups28"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups29"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups30"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups31"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups32"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups33"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups34"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups35"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups36"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups37"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups38"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups39"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups40"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups41"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups42"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups43"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups44"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups45"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups46"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups47"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups48"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups49"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups50"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups51"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups52"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups53"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups54"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups55"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvClasses.Groups56")))});
			this.lvClasses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvClasses.HideSelection = false;
			this.lvClasses.MultiSelect = false;
			this.lvClasses.Name = "lvClasses";
			this.lvClasses.OwnerDraw = true;
			this.lvClasses.ShowMembersAndTypeColumns = true;
			this.lvClasses.UseCompatibleStateImageBehavior = false;
			this.lvClasses.View = System.Windows.Forms.View.Details;
			this.lvClasses.DoubleClick += new System.EventHandler(this.lvClasses_DoubleClick);
			this.lvClasses.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvClasses_KeyPress);
			// 
			// charExplorer
			// 
			resources.ApplyResources(this.charExplorer, "charExplorer");
			this.charExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.charExplorer.ControlReceivingFocusOnMnemonic = null;
			this.charExplorer.DoubleBuffered = false;
			this.charExplorer.MnemonicGeneratesClick = false;
			this.charExplorer.Name = "charExplorer";
			this.charExplorer.PaintExplorerBarBackground = false;
			this.charExplorer.CharPicked += new SIL.Pa.Controls.CharPicker.CharPickedHandler(this.HandleIPACharPicked);
			// 
			// txtClassName
			// 
			resources.ApplyResources(this.txtClassName, "txtClassName");
			this.txtClassName.Name = "txtClassName";
			this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
			// 
			// lblClassName
			// 
			resources.ApplyResources(this.lblClassName, "lblClassName");
			this.lblClassName.Name = "lblClassName";
			// 
			// lblClassType
			// 
			resources.ApplyResources(this.lblClassType, "lblClassType");
			this.lblClassType.Name = "lblClassType";
			// 
			// pnlMembers
			// 
			this.pnlMembers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMembers.ControlReceivingFocusOnMnemonic = null;
			this.pnlMembers.Controls.Add(this.txtMembers);
			this.pnlMembers.Controls.Add(this.lblMembers);
			this.pnlMembers.Controls.Add(this.rdoOr);
			this.pnlMembers.Controls.Add(this.rdoAnd);
			resources.ApplyResources(this.pnlMembers, "pnlMembers");
			this.pnlMembers.DoubleBuffered = true;
			this.pnlMembers.MakeDark = false;
			this.pnlMembers.MnemonicGeneratesClick = false;
			this.pnlMembers.Name = "pnlMembers";
			this.pnlMembers.PaintExplorerBarBackground = false;
			// 
			// txtMembers
			// 
			resources.ApplyResources(this.txtMembers, "txtMembers");
			this.txtMembers.Name = "txtMembers";
			this.txtMembers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMembers_KeyDown);
			// 
			// lblMembers
			// 
			resources.ApplyResources(this.lblMembers, "lblMembers");
			this.lblMembers.BackColor = System.Drawing.Color.Transparent;
			this.lblMembers.Name = "lblMembers";
			// 
			// rdoOr
			// 
			resources.ApplyResources(this.rdoOr, "rdoOr");
			this.rdoOr.BackColor = System.Drawing.Color.Transparent;
			this.rdoOr.Name = "rdoOr";
			this.rdoOr.TabStop = true;
			this.m_toolTip.SetToolTip(this.rdoOr, resources.GetString("rdoOr.ToolTip"));
			this.rdoOr.UseVisualStyleBackColor = false;
			this.rdoOr.CheckedChanged += new System.EventHandler(this.HandleScopeClick);
			// 
			// rdoAnd
			// 
			resources.ApplyResources(this.rdoAnd, "rdoAnd");
			this.rdoAnd.BackColor = System.Drawing.Color.Transparent;
			this.rdoAnd.Name = "rdoAnd";
			this.rdoAnd.TabStop = true;
			this.m_toolTip.SetToolTip(this.rdoAnd, resources.GetString("rdoAnd.ToolTip"));
			this.rdoAnd.UseVisualStyleBackColor = false;
			this.rdoAnd.CheckedChanged += new System.EventHandler(this.HandleScopeClick);
			// 
			// lblClassTypeValue
			// 
			resources.ApplyResources(this.lblClassTypeValue, "lblClassTypeValue");
			this.lblClassTypeValue.Name = "lblClassTypeValue";
			// 
			// DefineClassDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblClassTypeValue);
			this.Controls.Add(this.pnlMembers);
			this.Controls.Add(this.pnlMemberPickingContainer);
			this.Controls.Add(this.lblClassType);
			this.Controls.Add(this.lblClassName);
			this.Controls.Add(this.txtClassName);
			this.Name = "DefineClassDlg";
			this.Controls.SetChildIndex(this.txtClassName, 0);
			this.Controls.SetChildIndex(this.lblClassName, 0);
			this.Controls.SetChildIndex(this.lblClassType, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.pnlMemberPickingContainer, 0);
			this.Controls.SetChildIndex(this.pnlMembers, 0);
			this.Controls.SetChildIndex(this.lblClassTypeValue, 0);
			this.pnlButtons.ResumeLayout(false);
			this.pnlMemberPickingContainer.ResumeLayout(false);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitPhoneViewers.Panel1.ResumeLayout(false);
			this.splitPhoneViewers.ResumeLayout(false);
			this.splitCV.ResumeLayout(false);
			this.pnlMembers.ResumeLayout(false);
			this.pnlMembers.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlMemberPickingContainer;
		private System.Windows.Forms.TextBox txtClassName;
		private System.Windows.Forms.Label lblClassName;
		private System.Windows.Forms.Label lblClassType;
		private SIL.Pa.Controls.IPACharacterExplorer charExplorer;
		private SIL.Pa.Controls.PaGradientPanel pnlMembers;
		private System.Windows.Forms.TextBox txtMembers;
		private System.Windows.Forms.RadioButton rdoAnd;
		private System.Windows.Forms.RadioButton rdoOr;
		private System.Windows.Forms.Label lblMembers;
		private SIL.Pa.Controls.ClassListView lvClasses;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.SplitContainer splitCV;
		private System.Windows.Forms.SplitContainer splitPhoneViewers;
		private System.Windows.Forms.ToolTip m_toolTip;
		private System.Windows.Forms.Label lblClassTypeValue;
	}
}