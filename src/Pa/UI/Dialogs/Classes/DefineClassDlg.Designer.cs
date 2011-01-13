using SilTools.Controls;

namespace SIL.Pa.UI.Dialogs
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
			this.lvClasses = new SIL.Pa.UI.Controls.ClassListView();
			this.charExplorer = new SIL.Pa.UI.Controls.IPACharacterExplorer();
			this.txtClassName = new System.Windows.Forms.TextBox();
			this.lblClassName = new System.Windows.Forms.Label();
			this.lblClassType = new System.Windows.Forms.Label();
			this.txtMembers = new System.Windows.Forms.TextBox();
			this.lblMembers = new System.Windows.Forms.Label();
			this.rbMatchAny = new System.Windows.Forms.RadioButton();
			this.rbMatchAll = new System.Windows.Forms.RadioButton();
			this.lblClassTypeValue = new System.Windows.Forms.Label();
			this.tblLayoutTop = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlMemberPickingContainer.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitPhoneViewers.Panel1.SuspendLayout();
			this.splitPhoneViewers.SuspendLayout();
			this.splitCV.SuspendLayout();
			this.tblLayoutTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlMemberPickingContainer
			// 
			this.pnlMemberPickingContainer.BackColor = System.Drawing.SystemColors.Window;
			this.tblLayoutTop.SetColumnSpan(this.pnlMemberPickingContainer, 3);
			this.pnlMemberPickingContainer.Controls.Add(this.splitOuter);
			this.pnlMemberPickingContainer.Controls.Add(this.lvClasses);
			this.pnlMemberPickingContainer.Controls.Add(this.charExplorer);
			resources.ApplyResources(this.pnlMemberPickingContainer, "pnlMemberPickingContainer");
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
			resources.ApplyResources(this.lvClasses, "lvClasses");
			this.lvClasses.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
			this.lvClasses.AppliesTo = SIL.Pa.UI.Controls.ClassListView.ListApplicationType.DefineClassDialog;
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
			this.locExtender.SetLocalizableToolTip(this.lvClasses, null);
			this.locExtender.SetLocalizationComment(this.lvClasses, null);
			this.locExtender.SetLocalizationPriority(this.lvClasses, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lvClasses, "DefineClassDlg.lvClasses");
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
			this.charExplorer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.charExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.charExplorer.ClipTextForChildControls = true;
			this.charExplorer.ControlReceivingFocusOnMnemonic = null;
			this.charExplorer.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.charExplorer, null);
			this.locExtender.SetLocalizationComment(this.charExplorer, null);
			this.locExtender.SetLocalizationPriority(this.charExplorer, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.charExplorer, "DefineClassDlg.charExplorer");
			this.charExplorer.MnemonicGeneratesClick = false;
			this.charExplorer.Name = "charExplorer";
			this.charExplorer.PaintExplorerBarBackground = false;
			this.charExplorer.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleIPACharPicked);
			// 
			// txtClassName
			// 
			resources.ApplyResources(this.txtClassName, "txtClassName");
			this.tblLayoutTop.SetColumnSpan(this.txtClassName, 2);
			this.locExtender.SetLocalizableToolTip(this.txtClassName, null);
			this.locExtender.SetLocalizationComment(this.txtClassName, null);
			this.locExtender.SetLocalizationPriority(this.txtClassName, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtClassName, "DefineClassDlg.txtClassName");
			this.txtClassName.Name = "txtClassName";
			this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
			// 
			// lblClassName
			// 
			resources.ApplyResources(this.lblClassName, "lblClassName");
			this.locExtender.SetLocalizableToolTip(this.lblClassName, null);
			this.locExtender.SetLocalizationComment(this.lblClassName, null);
			this.locExtender.SetLocalizingId(this.lblClassName, "DefineClassDlg.lblClassName");
			this.lblClassName.Name = "lblClassName";
			// 
			// lblClassType
			// 
			resources.ApplyResources(this.lblClassType, "lblClassType");
			this.locExtender.SetLocalizableToolTip(this.lblClassType, null);
			this.locExtender.SetLocalizationComment(this.lblClassType, null);
			this.locExtender.SetLocalizingId(this.lblClassType, "DefineClassDlg.lblClassType");
			this.lblClassType.Name = "lblClassType";
			// 
			// txtMembers
			// 
			resources.ApplyResources(this.txtMembers, "txtMembers");
			this.tblLayoutTop.SetColumnSpan(this.txtMembers, 2);
			this.locExtender.SetLocalizableToolTip(this.txtMembers, null);
			this.locExtender.SetLocalizationComment(this.txtMembers, null);
			this.locExtender.SetLocalizationPriority(this.txtMembers, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtMembers, "DefineClassDlg.txtMembers");
			this.txtMembers.Name = "txtMembers";
			this.txtMembers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMembers_KeyDown);
			// 
			// lblMembers
			// 
			resources.ApplyResources(this.lblMembers, "lblMembers");
			this.lblMembers.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblMembers, null);
			this.locExtender.SetLocalizationComment(this.lblMembers, null);
			this.locExtender.SetLocalizingId(this.lblMembers, "DefineClassDlg.lblMembers");
			this.lblMembers.Name = "lblMembers";
			// 
			// rbMatchAny
			// 
			resources.ApplyResources(this.rbMatchAny, "rbMatchAny");
			this.rbMatchAny.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbMatchAny, null);
			this.locExtender.SetLocalizationComment(this.rbMatchAny, null);
			this.locExtender.SetLocalizingId(this.rbMatchAny, "DefineClassDlg.rbMatchAny");
			this.rbMatchAny.Name = "rbMatchAny";
			this.rbMatchAny.TabStop = true;
			this.rbMatchAny.UseVisualStyleBackColor = false;
			this.rbMatchAny.CheckedChanged += new System.EventHandler(this.HandleScopeClick);
			// 
			// rbMatchAll
			// 
			resources.ApplyResources(this.rbMatchAll, "rbMatchAll");
			this.rbMatchAll.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbMatchAll, null);
			this.locExtender.SetLocalizationComment(this.rbMatchAll, null);
			this.locExtender.SetLocalizingId(this.rbMatchAll, "DefineClassDlg.rbMatchAll");
			this.rbMatchAll.Name = "rbMatchAll";
			this.rbMatchAll.TabStop = true;
			this.rbMatchAll.UseVisualStyleBackColor = false;
			this.rbMatchAll.CheckedChanged += new System.EventHandler(this.HandleScopeClick);
			// 
			// lblClassTypeValue
			// 
			resources.ApplyResources(this.lblClassTypeValue, "lblClassTypeValue");
			this.tblLayoutTop.SetColumnSpan(this.lblClassTypeValue, 2);
			this.locExtender.SetLocalizableToolTip(this.lblClassTypeValue, null);
			this.locExtender.SetLocalizationComment(this.lblClassTypeValue, null);
			this.locExtender.SetLocalizationPriority(this.lblClassTypeValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblClassTypeValue, "DefineClassDlg.lblClassTypeValue");
			this.lblClassTypeValue.Name = "lblClassTypeValue";
			// 
			// tblLayoutTop
			// 
			resources.ApplyResources(this.tblLayoutTop, "tblLayoutTop");
			this.tblLayoutTop.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutTop.Controls.Add(this.pnlMemberPickingContainer, 0, 4);
			this.tblLayoutTop.Controls.Add(this.lblMembers, 0, 2);
			this.tblLayoutTop.Controls.Add(this.lblClassType, 0, 0);
			this.tblLayoutTop.Controls.Add(this.rbMatchAll, 2, 3);
			this.tblLayoutTop.Controls.Add(this.rbMatchAny, 1, 3);
			this.tblLayoutTop.Controls.Add(this.lblClassTypeValue, 1, 0);
			this.tblLayoutTop.Controls.Add(this.lblClassName, 0, 1);
			this.tblLayoutTop.Controls.Add(this.txtMembers, 1, 2);
			this.tblLayoutTop.Controls.Add(this.txtClassName, 1, 1);
			this.tblLayoutTop.Name = "tblLayoutTop";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// DefineClassDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tblLayoutTop);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DefineClassDlg.WindowTitle");
			this.Name = "DefineClassDlg";
			this.Controls.SetChildIndex(this.tblLayoutTop, 0);
			this.pnlMemberPickingContainer.ResumeLayout(false);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitPhoneViewers.Panel1.ResumeLayout(false);
			this.splitPhoneViewers.ResumeLayout(false);
			this.splitCV.ResumeLayout(false);
			this.tblLayoutTop.ResumeLayout(false);
			this.tblLayoutTop.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlMemberPickingContainer;
		private System.Windows.Forms.TextBox txtClassName;
		private System.Windows.Forms.Label lblClassName;
		private System.Windows.Forms.Label lblClassType;
		private SIL.Pa.UI.Controls.IPACharacterExplorer charExplorer;
		private System.Windows.Forms.TextBox txtMembers;
		private System.Windows.Forms.RadioButton rbMatchAll;
		private System.Windows.Forms.RadioButton rbMatchAny;
		private System.Windows.Forms.Label lblMembers;
		private SIL.Pa.UI.Controls.ClassListView lvClasses;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.SplitContainer splitCV;
		private System.Windows.Forms.SplitContainer splitPhoneViewers;
		private System.Windows.Forms.Label lblClassTypeValue;
		private System.Windows.Forms.TableLayoutPanel tblLayoutTop;
		private Localization.UI.LocalizationExtender locExtender;
	}
}