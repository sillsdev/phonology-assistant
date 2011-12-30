using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;
using SIL.Pa.Resources;

namespace SIL.Pa.UI.Dialogs
{
	public partial class OptionsDlg
	{

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.picSaveInfo = new System.Windows.Forms.PictureBox();
			this.lblSaveInfo = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tpgColors = new System.Windows.Forms.TabPage();
			this.tpgFindPhones = new System.Windows.Forms.TabPage();
			this.lblShowDiamondPattern = new System.Windows.Forms.Label();
			this.chkShowDiamondPattern = new System.Windows.Forms.CheckBox();
			this.grpClassSettings = new System.Windows.Forms.GroupBox();
			this.rdoClassMembers = new System.Windows.Forms.RadioButton();
			this.rdoClassName = new System.Windows.Forms.RadioButton();
			this.lblClassDisplayBehavior = new System.Windows.Forms.Label();
			this.tabOptions = new System.Windows.Forms.TabControl();
			((System.ComponentModel.ISupportInitialize)(this.picSaveInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.tpgFindPhones.SuspendLayout();
			this.grpClassSettings.SuspendLayout();
			this.tabOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// picSaveInfo
			// 
			this.picSaveInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.picSaveInfo.Image = global::SIL.Pa.Properties.Resources.kimidInformation;
			this.picSaveInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.picSaveInfo, null);
			this.locExtender.SetLocalizationComment(this.picSaveInfo, null);
			this.locExtender.SetLocalizationPriority(this.picSaveInfo, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.picSaveInfo, "OptionsDlg.picSaveInfo");
			this.picSaveInfo.Location = new System.Drawing.Point(12, 7);
			this.picSaveInfo.Name = "picSaveInfo";
			this.picSaveInfo.Size = new System.Drawing.Size(16, 16);
			this.picSaveInfo.TabIndex = 10;
			this.picSaveInfo.TabStop = false;
			this.picSaveInfo.Visible = false;
			// 
			// lblSaveInfo
			// 
			this.lblSaveInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblSaveInfo.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.lblSaveInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSaveInfo, null);
			this.locExtender.SetLocalizationComment(this.lblSaveInfo, "Label next to OK/Cancel/Help buttons on options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSaveInfo, "OptionsDlg.lblSaveInfo");
			this.lblSaveInfo.Location = new System.Drawing.Point(34, 5);
			this.lblSaveInfo.Name = "lblSaveInfo";
			this.lblSaveInfo.Size = new System.Drawing.Size(245, 33);
			this.lblSaveInfo.TabIndex = 0;
			this.lblSaveInfo.Text = "The options for this tab will be saved only for the current project.";
			this.lblSaveInfo.Visible = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// tpgColors
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgColors, null);
			this.locExtender.SetLocalizationComment(this.tpgColors, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizationPriority(this.tpgColors, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tpgColors, "OptionsDlg.tpgColors");
			this.tpgColors.Location = new System.Drawing.Point(4, 22);
			this.tpgColors.Name = "tpgColors";
			this.tpgColors.Size = new System.Drawing.Size(588, 364);
			this.tpgColors.TabIndex = 3;
			this.tpgColors.Text = "Colors";
			this.tpgColors.UseVisualStyleBackColor = true;
			// 
			// tpgFindPhones
			// 
			this.tpgFindPhones.Controls.Add(this.lblShowDiamondPattern);
			this.tpgFindPhones.Controls.Add(this.chkShowDiamondPattern);
			this.tpgFindPhones.Controls.Add(this.grpClassSettings);
			this.locExtender.SetLocalizableToolTip(this.tpgFindPhones, null);
			this.locExtender.SetLocalizationComment(this.tpgFindPhones, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgFindPhones, "OptionsDlg.SearchPatternsTab.Text");
			this.tpgFindPhones.Location = new System.Drawing.Point(4, 22);
			this.tpgFindPhones.Name = "tpgFindPhones";
			this.tpgFindPhones.Size = new System.Drawing.Size(589, 439);
			this.tpgFindPhones.TabIndex = 2;
			this.tpgFindPhones.Text = "Search Patterns";
			this.tpgFindPhones.UseVisualStyleBackColor = true;
			// 
			// lblShowDiamondPattern
			// 
			this.lblShowDiamondPattern.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblShowDiamondPattern, null);
			this.locExtender.SetLocalizationComment(this.lblShowDiamondPattern, "Label text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblShowDiamondPattern, "OptionsDlg.SearchPatternsTab.lblShowDiamondPattern");
			this.lblShowDiamondPattern.Location = new System.Drawing.Point(45, 189);
			this.lblShowDiamondPattern.Name = "lblShowDiamondPattern";
			this.lblShowDiamondPattern.Size = new System.Drawing.Size(261, 177);
			this.lblShowDiamondPattern.TabIndex = 4;
			this.lblShowDiamondPattern.Text = "Displays a diamond pattern (i.e. {0}) when the Current Search Pattern is empty in" +
    " the Search view.";
			// 
			// chkShowDiamondPattern
			// 
			this.chkShowDiamondPattern.AutoSize = true;
			this.chkShowDiamondPattern.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkShowDiamondPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkShowDiamondPattern.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkShowDiamondPattern, null);
			this.locExtender.SetLocalizationComment(this.chkShowDiamondPattern, "Check box text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkShowDiamondPattern, "OptionsDlg.SearchPatternsTab.chkShowDiamondPattern");
			this.chkShowDiamondPattern.Location = new System.Drawing.Point(29, 170);
			this.chkShowDiamondPattern.Name = "chkShowDiamondPattern";
			this.chkShowDiamondPattern.Size = new System.Drawing.Size(159, 19);
			this.chkShowDiamondPattern.TabIndex = 3;
			this.chkShowDiamondPattern.Text = "&Display diamond pattern";
			this.chkShowDiamondPattern.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkShowDiamondPattern.UseVisualStyleBackColor = true;
			// 
			// grpClassSettings
			// 
			this.grpClassSettings.AutoSize = true;
			this.grpClassSettings.Controls.Add(this.rdoClassMembers);
			this.grpClassSettings.Controls.Add(this.rdoClassName);
			this.grpClassSettings.Controls.Add(this.lblClassDisplayBehavior);
			this.locExtender.SetLocalizableToolTip(this.grpClassSettings, null);
			this.locExtender.SetLocalizationComment(this.grpClassSettings, "Frame text on search pattern tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpClassSettings, "OptionsDlg.SearchPatternsTab.grpClassSettings");
			this.grpClassSettings.Location = new System.Drawing.Point(17, 13);
			this.grpClassSettings.Name = "grpClassSettings";
			this.grpClassSettings.Size = new System.Drawing.Size(231, 134);
			this.grpClassSettings.TabIndex = 2;
			this.grpClassSettings.TabStop = false;
			this.grpClassSettings.Text = "Class Display Behavior";
			// 
			// rdoClassMembers
			// 
			this.rdoClassMembers.AutoSize = true;
			this.rdoClassMembers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rdoClassMembers, null);
			this.locExtender.SetLocalizationComment(this.rdoClassMembers, "Radio button text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rdoClassMembers, "OptionsDlg.SearchPatternsTab.rdoClassMembers");
			this.rdoClassMembers.Location = new System.Drawing.Point(12, 98);
			this.rdoClassMembers.Name = "rdoClassMembers";
			this.rdoClassMembers.Size = new System.Drawing.Size(95, 17);
			this.rdoClassMembers.TabIndex = 1;
			this.rdoClassMembers.TabStop = true;
			this.rdoClassMembers.Text = "Class &members";
			this.rdoClassMembers.UseVisualStyleBackColor = true;
			// 
			// rdoClassName
			// 
			this.rdoClassName.AutoSize = true;
			this.rdoClassName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rdoClassName, null);
			this.locExtender.SetLocalizationComment(this.rdoClassName, "Radio button text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rdoClassName, "OptionsDlg.SearchPatternsTab.rdoClassName");
			this.rdoClassName.Location = new System.Drawing.Point(12, 75);
			this.rdoClassName.Name = "rdoClassName";
			this.rdoClassName.Size = new System.Drawing.Size(79, 17);
			this.rdoClassName.TabIndex = 0;
			this.rdoClassName.TabStop = true;
			this.rdoClassName.Text = "Class &name";
			this.rdoClassName.UseVisualStyleBackColor = true;
			// 
			// lblClassDisplayBehavior
			// 
			this.lblClassDisplayBehavior.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblClassDisplayBehavior, null);
			this.locExtender.SetLocalizationComment(this.lblClassDisplayBehavior, "Label text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblClassDisplayBehavior, "OptionsDlg.SearchPatternsTab.lblClassDisplayBehavior");
			this.lblClassDisplayBehavior.Location = new System.Drawing.Point(9, 20);
			this.lblClassDisplayBehavior.Name = "lblClassDisplayBehavior";
			this.lblClassDisplayBehavior.Size = new System.Drawing.Size(216, 44);
			this.lblClassDisplayBehavior.TabIndex = 3;
			this.lblClassDisplayBehavior.Text = "When displaying classes in search patterns and nested class definitions, show:";
			this.lblClassDisplayBehavior.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabOptions
			// 
			this.tabOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabOptions.Controls.Add(this.tpgFindPhones);
			this.tabOptions.Controls.Add(this.tpgColors);
			this.tabOptions.HotTrack = true;
			this.tabOptions.Location = new System.Drawing.Point(12, 12);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.SelectedIndex = 0;
			this.tabOptions.Size = new System.Drawing.Size(597, 465);
			this.tabOptions.TabIndex = 0;
			// 
			// OptionsDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(620, 520);
			this.Controls.Add(this.tabOptions);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Title of options dialog box.");
			this.locExtender.SetLocalizingId(this, "OptionsDlg.WindowTitle");
			this.Name = "OptionsDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Options";
			this.Controls.SetChildIndex(this.tabOptions, 0);
			((System.ComponentModel.ISupportInitialize)(this.picSaveInfo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.tpgFindPhones.ResumeLayout(false);
			this.tpgFindPhones.PerformLayout();
			this.grpClassSettings.ResumeLayout(false);
			this.grpClassSettings.PerformLayout();
			this.tabOptions.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private Label lblSaveInfo;
		private PictureBox picSaveInfo;
		private IContainer components;
		private Localization.UI.LocalizationExtender locExtender;
		private TabPage tpgColors;
		private TabPage tpgFindPhones;
		private Label lblShowDiamondPattern;
		private CheckBox chkShowDiamondPattern;
		private GroupBox grpClassSettings;
		private RadioButton rdoClassMembers;
		private RadioButton rdoClassName;
		private Label lblClassDisplayBehavior;
		private TabControl tabOptions;
	}
}
