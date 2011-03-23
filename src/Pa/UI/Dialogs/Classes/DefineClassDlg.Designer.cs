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
			this.pnlMemberPickingContainer = new System.Windows.Forms.Panel();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.splitPhoneViewers = new System.Windows.Forms.SplitContainer();
			this.splitCV = new System.Windows.Forms.SplitContainer();
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
			this.pnlMemberPickingContainer.Controls.Add(this.charExplorer);
			this.pnlMemberPickingContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMemberPickingContainer.Location = new System.Drawing.Point(0, 126);
			this.pnlMemberPickingContainer.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this.pnlMemberPickingContainer.Name = "pnlMemberPickingContainer";
			this.pnlMemberPickingContainer.Size = new System.Drawing.Size(450, 326);
			this.pnlMemberPickingContainer.TabIndex = 8;
			// 
			// splitOuter
			// 
			this.splitOuter.BackColor = System.Drawing.SystemColors.Control;
			this.splitOuter.Location = new System.Drawing.Point(8, 112);
			this.splitOuter.Name = "splitOuter";
			this.splitOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.splitPhoneViewers);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.BackColor = System.Drawing.SystemColors.Window;
			this.splitOuter.Size = new System.Drawing.Size(199, 159);
			this.splitOuter.SplitterDistance = 89;
			this.splitOuter.SplitterWidth = 6;
			this.splitOuter.TabIndex = 1;
			// 
			// splitPhoneViewers
			// 
			this.splitPhoneViewers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitPhoneViewers.Location = new System.Drawing.Point(0, 0);
			this.splitPhoneViewers.Name = "splitPhoneViewers";
			// 
			// splitPhoneViewers.Panel1
			// 
			this.splitPhoneViewers.Panel1.Controls.Add(this.splitCV);
			// 
			// splitPhoneViewers.Panel2
			// 
			this.splitPhoneViewers.Panel2.BackColor = System.Drawing.SystemColors.Window;
			this.splitPhoneViewers.Size = new System.Drawing.Size(199, 89);
			this.splitPhoneViewers.SplitterDistance = 121;
			this.splitPhoneViewers.SplitterWidth = 6;
			this.splitPhoneViewers.TabIndex = 0;
			// 
			// splitCV
			// 
			this.splitCV.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitCV.Location = new System.Drawing.Point(0, 0);
			this.splitCV.Name = "splitCV";
			// 
			// splitCV.Panel1
			// 
			this.splitCV.Panel1.BackColor = System.Drawing.SystemColors.Window;
			// 
			// splitCV.Panel2
			// 
			this.splitCV.Panel2.BackColor = System.Drawing.SystemColors.Window;
			this.splitCV.Size = new System.Drawing.Size(121, 89);
			this.splitCV.SplitterDistance = 59;
			this.splitCV.SplitterWidth = 6;
			this.splitCV.TabIndex = 0;
			// 
			// charExplorer
			// 
			this.charExplorer.AutoScroll = true;
			this.charExplorer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.charExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.charExplorer.ClipTextForChildControls = true;
			this.charExplorer.ControlReceivingFocusOnMnemonic = null;
			this.charExplorer.DoubleBuffered = false;
			this.charExplorer.DrawOnlyBottomBorder = false;
			this.charExplorer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.charExplorer.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.charExplorer, null);
			this.locExtender.SetLocalizationComment(this.charExplorer, null);
			this.locExtender.SetLocalizationPriority(this.charExplorer, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.charExplorer, "DefineClassDlg.charExplorer");
			this.charExplorer.Location = new System.Drawing.Point(238, 172);
			this.charExplorer.MnemonicGeneratesClick = false;
			this.charExplorer.Name = "charExplorer";
			this.charExplorer.PaintExplorerBarBackground = false;
			this.charExplorer.Size = new System.Drawing.Size(137, 87);
			this.charExplorer.TabIndex = 2;
			this.charExplorer.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleIPACharPicked);
			// 
			// txtClassName
			// 
			this.txtClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayoutTop.SetColumnSpan(this.txtClassName, 2);
			this.locExtender.SetLocalizableToolTip(this.txtClassName, null);
			this.locExtender.SetLocalizationComment(this.txtClassName, null);
			this.locExtender.SetLocalizationPriority(this.txtClassName, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtClassName, "DefineClassDlg.txtClassName");
			this.txtClassName.Location = new System.Drawing.Point(59, 37);
			this.txtClassName.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this.txtClassName.Name = "txtClassName";
			this.txtClassName.Size = new System.Drawing.Size(391, 20);
			this.txtClassName.TabIndex = 3;
			this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
			// 
			// lblClassName
			// 
			this.lblClassName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblClassName.AutoSize = true;
			this.lblClassName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblClassName, null);
			this.locExtender.SetLocalizationComment(this.lblClassName, null);
			this.locExtender.SetLocalizingId(this.lblClassName, "DefineClassDlg.lblClassName");
			this.lblClassName.Location = new System.Drawing.Point(3, 40);
			this.lblClassName.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this.lblClassName.Name = "lblClassName";
			this.lblClassName.Size = new System.Drawing.Size(38, 13);
			this.lblClassName.TabIndex = 2;
			this.lblClassName.Text = "N&ame:";
			// 
			// lblClassType
			// 
			this.lblClassType.AutoSize = true;
			this.lblClassType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblClassType, null);
			this.locExtender.SetLocalizationComment(this.lblClassType, null);
			this.locExtender.SetLocalizingId(this.lblClassType, "DefineClassDlg.lblClassType");
			this.lblClassType.Location = new System.Drawing.Point(3, 10);
			this.lblClassType.Margin = new System.Windows.Forms.Padding(3, 10, 8, 2);
			this.lblClassType.Name = "lblClassType";
			this.lblClassType.Size = new System.Drawing.Size(34, 13);
			this.lblClassType.TabIndex = 0;
			this.lblClassType.Text = "Type:";
			// 
			// txtMembers
			// 
			this.txtMembers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayoutTop.SetColumnSpan(this.txtMembers, 2);
			this.locExtender.SetLocalizableToolTip(this.txtMembers, null);
			this.locExtender.SetLocalizationComment(this.txtMembers, null);
			this.locExtender.SetLocalizationPriority(this.txtMembers, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtMembers, "DefineClassDlg.txtMembers");
			this.txtMembers.Location = new System.Drawing.Point(59, 69);
			this.txtMembers.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this.txtMembers.Name = "txtMembers";
			this.txtMembers.Size = new System.Drawing.Size(391, 20);
			this.txtMembers.TabIndex = 5;
			this.txtMembers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMembers_KeyDown);
			// 
			// lblMembers
			// 
			this.lblMembers.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblMembers.AutoSize = true;
			this.lblMembers.BackColor = System.Drawing.Color.Transparent;
			this.lblMembers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblMembers, null);
			this.locExtender.SetLocalizationComment(this.lblMembers, null);
			this.locExtender.SetLocalizingId(this.lblMembers, "DefineClassDlg.lblMembers");
			this.lblMembers.Location = new System.Drawing.Point(3, 72);
			this.lblMembers.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this.lblMembers.Name = "lblMembers";
			this.lblMembers.Size = new System.Drawing.Size(53, 13);
			this.lblMembers.TabIndex = 4;
			this.lblMembers.Text = "&Members:";
			this.lblMembers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbMatchAny
			// 
			this.rbMatchAny.AutoSize = true;
			this.rbMatchAny.BackColor = System.Drawing.Color.Transparent;
			this.rbMatchAny.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbMatchAny, null);
			this.locExtender.SetLocalizationComment(this.rbMatchAny, null);
			this.locExtender.SetLocalizingId(this.rbMatchAny, "DefineClassDlg.rbMatchAny");
			this.rbMatchAny.Location = new System.Drawing.Point(62, 94);
			this.rbMatchAny.Margin = new System.Windows.Forms.Padding(3, 5, 8, 3);
			this.rbMatchAny.Name = "rbMatchAny";
			this.rbMatchAny.Size = new System.Drawing.Size(160, 17);
			this.rbMatchAny.TabIndex = 6;
			this.rbMatchAny.TabStop = true;
			this.rbMatchAny.Text = "Match A&ny Selected Feature";
			this.rbMatchAny.UseVisualStyleBackColor = false;
			this.rbMatchAny.CheckedChanged += new System.EventHandler(this.HandleScopeClick);
			// 
			// rbMatchAll
			// 
			this.rbMatchAll.AutoSize = true;
			this.rbMatchAll.BackColor = System.Drawing.Color.Transparent;
			this.rbMatchAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbMatchAll, null);
			this.locExtender.SetLocalizationComment(this.rbMatchAll, null);
			this.locExtender.SetLocalizingId(this.rbMatchAll, "DefineClassDlg.rbMatchAll");
			this.rbMatchAll.Location = new System.Drawing.Point(237, 94);
			this.rbMatchAll.Margin = new System.Windows.Forms.Padding(7, 5, 3, 3);
			this.rbMatchAll.Name = "rbMatchAll";
			this.rbMatchAll.Size = new System.Drawing.Size(158, 17);
			this.rbMatchAll.TabIndex = 7;
			this.rbMatchAll.TabStop = true;
			this.rbMatchAll.Text = "Match A&ll Selected Features";
			this.rbMatchAll.UseVisualStyleBackColor = false;
			this.rbMatchAll.CheckedChanged += new System.EventHandler(this.HandleScopeClick);
			// 
			// lblClassTypeValue
			// 
			this.lblClassTypeValue.AutoSize = true;
			this.tblLayoutTop.SetColumnSpan(this.lblClassTypeValue, 2);
			this.locExtender.SetLocalizableToolTip(this.lblClassTypeValue, null);
			this.locExtender.SetLocalizationComment(this.lblClassTypeValue, null);
			this.locExtender.SetLocalizationPriority(this.lblClassTypeValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblClassTypeValue, "DefineClassDlg.lblClassTypeValue");
			this.lblClassTypeValue.Location = new System.Drawing.Point(62, 10);
			this.lblClassTypeValue.Margin = new System.Windows.Forms.Padding(3, 10, 3, 2);
			this.lblClassTypeValue.Name = "lblClassTypeValue";
			this.lblClassTypeValue.Size = new System.Drawing.Size(14, 13);
			this.lblClassTypeValue.TabIndex = 1;
			this.lblClassTypeValue.Text = "#";
			// 
			// tblLayoutTop
			// 
			this.tblLayoutTop.AutoSize = true;
			this.tblLayoutTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblLayoutTop.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutTop.ColumnCount = 3;
			this.tblLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutTop.Controls.Add(this.pnlMemberPickingContainer, 0, 4);
			this.tblLayoutTop.Controls.Add(this.lblMembers, 0, 2);
			this.tblLayoutTop.Controls.Add(this.lblClassType, 0, 0);
			this.tblLayoutTop.Controls.Add(this.rbMatchAll, 2, 3);
			this.tblLayoutTop.Controls.Add(this.rbMatchAny, 1, 3);
			this.tblLayoutTop.Controls.Add(this.lblClassTypeValue, 1, 0);
			this.tblLayoutTop.Controls.Add(this.lblClassName, 0, 1);
			this.tblLayoutTop.Controls.Add(this.txtMembers, 1, 2);
			this.tblLayoutTop.Controls.Add(this.txtClassName, 1, 1);
			this.tblLayoutTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblLayoutTop.Location = new System.Drawing.Point(10, 0);
			this.tblLayoutTop.Name = "tblLayoutTop";
			this.tblLayoutTop.RowCount = 5;
			this.tblLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tblLayoutTop.Size = new System.Drawing.Size(450, 452);
			this.tblLayoutTop.TabIndex = 0;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// DefineClassDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(470, 492);
			this.Controls.Add(this.tblLayoutTop);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DefineClassDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(435, 530);
			this.Name = "DefineClassDlg";
			this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "{0} Class";
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
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.SplitContainer splitCV;
		private System.Windows.Forms.SplitContainer splitPhoneViewers;
		private System.Windows.Forms.Label lblClassTypeValue;
		private System.Windows.Forms.TableLayoutPanel tblLayoutTop;
		private Localization.UI.LocalizationExtender locExtender;
	}
}