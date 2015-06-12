using SilTools.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class DefineClassBaseDlg
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
			this._panelMemberPickingContainer = new System.Windows.Forms.Panel();
			this._textBoxClassName = new System.Windows.Forms.TextBox();
			this._labelClassName = new System.Windows.Forms.Label();
			this._labelClassType = new System.Windows.Forms.Label();
			this._textBoxMembers = new System.Windows.Forms.TextBox();
			this._labelMembers = new System.Windows.Forms.Label();
			this._labelClassTypeValue = new System.Windows.Forms.Label();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _panelMemberPickingContainer
			// 
			this._panelMemberPickingContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelMemberPickingContainer.BackColor = System.Drawing.SystemColors.Window;
			this._tableLayout.SetColumnSpan(this._panelMemberPickingContainer, 3);
			this._panelMemberPickingContainer.Location = new System.Drawing.Point(0, 101);
			this._panelMemberPickingContainer.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this._panelMemberPickingContainer.Name = "_panelMemberPickingContainer";
			this._panelMemberPickingContainer.Size = new System.Drawing.Size(405, 359);
			this._panelMemberPickingContainer.TabIndex = 8;
			// 
			// _textBoxClassName
			// 
			this._textBoxClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.SetColumnSpan(this._textBoxClassName, 2);
			this.locExtender.SetLocalizableToolTip(this._textBoxClassName, null);
			this.locExtender.SetLocalizationComment(this._textBoxClassName, null);
			this.locExtender.SetLocalizationPriority(this._textBoxClassName, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._textBoxClassName, "DialogBoxes.DefineClassDlgBase.txtClassName");
			this._textBoxClassName.Location = new System.Drawing.Point(59, 37);
			this._textBoxClassName.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this._textBoxClassName.Name = "_textBoxClassName";
			this._textBoxClassName.Size = new System.Drawing.Size(346, 20);
			this._textBoxClassName.TabIndex = 3;
			this._textBoxClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
			// 
			// _labelClassName
			// 
			this._labelClassName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelClassName.AutoSize = true;
			this._labelClassName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._labelClassName, null);
			this.locExtender.SetLocalizationComment(this._labelClassName, null);
			this.locExtender.SetLocalizingId(this._labelClassName, "DialogBoxes.DefineClassDlgBase.ClassNameLabel");
			this._labelClassName.Location = new System.Drawing.Point(3, 40);
			this._labelClassName.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this._labelClassName.Name = "_labelClassName";
			this._labelClassName.Size = new System.Drawing.Size(38, 13);
			this._labelClassName.TabIndex = 2;
			this._labelClassName.Text = "N&ame:";
			// 
			// _labelClassType
			// 
			this._labelClassType.AutoSize = true;
			this._labelClassType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._labelClassType, null);
			this.locExtender.SetLocalizationComment(this._labelClassType, null);
			this.locExtender.SetLocalizingId(this._labelClassType, "DialogBoxes.DefineClassDlgBase.ClassTypeLabel");
			this._labelClassType.Location = new System.Drawing.Point(3, 10);
			this._labelClassType.Margin = new System.Windows.Forms.Padding(3, 10, 8, 2);
			this._labelClassType.Name = "_labelClassType";
			this._labelClassType.Size = new System.Drawing.Size(34, 13);
			this._labelClassType.TabIndex = 0;
			this._labelClassType.Text = "Type:";
			// 
			// _textBoxMembers
			// 
			this._textBoxMembers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.SetColumnSpan(this._textBoxMembers, 2);
			this.locExtender.SetLocalizableToolTip(this._textBoxMembers, null);
			this.locExtender.SetLocalizationComment(this._textBoxMembers, null);
			this.locExtender.SetLocalizationPriority(this._textBoxMembers, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._textBoxMembers, "DefineClassDlg.txtMembers");
			this._textBoxMembers.Location = new System.Drawing.Point(59, 69);
			this._textBoxMembers.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this._textBoxMembers.Name = "_textBoxMembers";
			this._textBoxMembers.ReadOnly = true;
			this._textBoxMembers.Size = new System.Drawing.Size(346, 20);
			this._textBoxMembers.TabIndex = 5;
			this._textBoxMembers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleMembersTextBoxKeyDown);
			// 
			// _labelMembers
			// 
			this._labelMembers.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelMembers.AutoSize = true;
			this._labelMembers.BackColor = System.Drawing.Color.Transparent;
			this._labelMembers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._labelMembers, null);
			this.locExtender.SetLocalizationComment(this._labelMembers, null);
			this.locExtender.SetLocalizingId(this._labelMembers, "DialogBoxes.DefineClassDlgBase.MembersLabel");
			this._labelMembers.Location = new System.Drawing.Point(3, 72);
			this._labelMembers.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this._labelMembers.Name = "_labelMembers";
			this._labelMembers.Size = new System.Drawing.Size(53, 13);
			this._labelMembers.TabIndex = 4;
			this._labelMembers.Text = "&Members:";
			this._labelMembers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _labelClassTypeValue
			// 
			this._labelClassTypeValue.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelClassTypeValue, 2);
			this.locExtender.SetLocalizableToolTip(this._labelClassTypeValue, null);
			this.locExtender.SetLocalizationComment(this._labelClassTypeValue, null);
			this.locExtender.SetLocalizationPriority(this._labelClassTypeValue, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelClassTypeValue, "DefineClassDlg.lblClassTypeValue");
			this._labelClassTypeValue.Location = new System.Drawing.Point(62, 10);
			this._labelClassTypeValue.Margin = new System.Windows.Forms.Padding(3, 10, 3, 2);
			this._labelClassTypeValue.Name = "_labelClassTypeValue";
			this._labelClassTypeValue.Size = new System.Drawing.Size(14, 13);
			this._labelClassTypeValue.TabIndex = 1;
			this._labelClassTypeValue.Text = "#";
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 3;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._panelMemberPickingContainer, 0, 4);
			this._tableLayout.Controls.Add(this._labelMembers, 0, 2);
			this._tableLayout.Controls.Add(this._labelClassType, 0, 0);
			this._tableLayout.Controls.Add(this._labelClassTypeValue, 1, 0);
			this._tableLayout.Controls.Add(this._labelClassName, 0, 1);
			this._tableLayout.Controls.Add(this._textBoxMembers, 1, 2);
			this._tableLayout.Controls.Add(this._textBoxClassName, 1, 1);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(10, 0);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 5;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Size = new System.Drawing.Size(405, 460);
			this._tableLayout.TabIndex = 0;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// DefineClassBaseDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(425, 500);
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DefineClassDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(435, 530);
			this.Name = "DefineClassBaseDlg";
			this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Override this form";
			this.Controls.SetChildIndex(this._tableLayout, 0);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelClassName;
		private System.Windows.Forms.Label _labelClassType;
		private System.Windows.Forms.Label _labelMembers;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		protected System.Windows.Forms.TextBox _textBoxClassName;
		protected System.Windows.Forms.TextBox _textBoxMembers;
		protected System.Windows.Forms.Label _labelClassTypeValue;
		protected System.Windows.Forms.Panel _panelMemberPickingContainer;
		protected System.Windows.Forms.TableLayoutPanel _tableLayout;
	}
}