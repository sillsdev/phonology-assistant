// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Localization;
using Localization.UI;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	#region AboutDlg implementation
	/// ----------------------------------------------------------------------------------------
	public class AboutDlg : Form
	{
		#region Data members

		private IContainer components;

		private readonly bool _isBetaVersion;
		private Font _bigFont;
		
		private Label _labelBuild;
		private Label _labelAppVersion;
		private Label _labelAvailableMemoryValue;
		private Label _labelAvailableDiskSpaceValue;
		private Label _labelCopyright;
		private Label _labelAppName;

		private LinkLabel _linkFeedback;
		private LinkLabel _linkWebsite;
		private TableLayoutPanel _tableLayout;
		private TableLayoutPanel _tableLayoutButtons;
		private Label _labelAvailableMemory;
		private Label _labelAvailableDiskSpace;
		private Panel panel1;
		private LocalizationExtender locExtender;
		private PictureBox fieldWorksIcon;
		#endregion

		#region Construction and Disposal
		/// ----------------------------------------------------------------------------------------
		public AboutDlg()
		{
			InitializeComponent();

			fieldWorksIcon.Image = Utils.GetSilLogo();

			_bigFont = FontHelper.MakeFont(FontHelper.UIFont, 22, FontStyle.Bold);
			_labelAppName.Font = _bigFont;
			_labelAppVersion.Font = FontHelper.UIFont;
			_labelAvailableDiskSpace.Font = FontHelper.UIFont;
			_labelAvailableMemory.Font = FontHelper.UIFont;
			_labelBuild.Font = FontHelper.UIFont;
			_labelCopyright.Font = FontHelper.UIFont;
			_linkFeedback.Font = FontHelper.UIFont;
			_linkWebsite.Font = FontHelper.UIFont;

			UpdateLabels();

			LocalizeItemDlg.StringsLocalized += delegate { UpdateLabels(); };
		}

		/// ------------------------------------------------------------------------------------
		public AboutDlg(bool isBetaVersion) : this()
		{
			_isBetaVersion = isBetaVersion;
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateLabels()
		{
			try
			{
				var address = "PaFeedback@sil.org";
				var website = "http://phonologyassistant.sil.org";

				_linkWebsite.Text = string.Format(_linkWebsite.Text, website);
				_linkFeedback.Text = string.Format(_linkFeedback.Text, address);

				_linkFeedback.Links.Clear();
				_linkWebsite.Links.Clear();

				int i = _linkWebsite.Text.IndexOf(website, StringComparison.Ordinal);
				_linkWebsite.LinkArea = new LinkArea(i, website.Length);

				i = _linkFeedback.Text.IndexOf(address, StringComparison.Ordinal);
				_linkFeedback.LinkArea = new LinkArea(i, address.Length);

				SetBuild();
				SetVersionNumber();
				SetCopyrightInformation();
				SetMemoryAndDiskInformation();
			}
			catch (Exception err)
			{
				var msg = LocalizationManager.GetString("DialogBoxes.AboutDlg.LoadingDialogInformation",
					"There was an error trying to load information into the 'About' dialog box.");

				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(err, msg);
			}
		}

		/// ----------------------------------------------------------------------------------------
		protected override void Dispose( bool disposing )
		{
			// Must not be run more than once.
			if (IsDisposed)
				return;

			if( disposing )
			{
				if (_bigFont != null)
				{
					_bigFont.Dispose();
					_bigFont = null;
				}

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = ClientRectangle;
			rc.Height--;
			rc.Width--;
			e.Graphics.DrawRectangle(Pens.CornflowerBlue, rc);
		}
		
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button buttonOk;
            this._labelAvailableMemory = new System.Windows.Forms.Label();
            this._labelAvailableDiskSpace = new System.Windows.Forms.Label();
            this.fieldWorksIcon = new System.Windows.Forms.PictureBox();
            this._linkWebsite = new System.Windows.Forms.LinkLabel();
            this._linkFeedback = new System.Windows.Forms.LinkLabel();
            this._labelBuild = new System.Windows.Forms.Label();
            this._labelAppVersion = new System.Windows.Forms.Label();
            this._labelAvailableMemoryValue = new System.Windows.Forms.Label();
            this._labelAvailableDiskSpaceValue = new System.Windows.Forms.Label();
            this._labelCopyright = new System.Windows.Forms.Label();
            this._labelAppName = new System.Windows.Forms.Label();
            this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            buttonOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fieldWorksIcon)).BeginInit();
            this._tableLayout.SuspendLayout();
            this._tableLayoutButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            buttonOk.AutoSize = true;
            buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(buttonOk, "Click to close window");
            this.locExtender.SetLocalizationComment(buttonOk, null);
            this.locExtender.SetLocalizingId(buttonOk, "DialogBoxes.AboutDlg.OKButtonText");
            buttonOk.Location = new System.Drawing.Point(445, 3);
            buttonOk.Margin = new System.Windows.Forms.Padding(3, 3, 13, 13);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new System.Drawing.Size(90, 32);
            buttonOk.TabIndex = 21;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = true;
            // 
            // _labelAvailableMemory
            // 
            this._labelAvailableMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._labelAvailableMemory.AutoSize = true;
            this._labelAvailableMemory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._labelAvailableMemory, null);
            this.locExtender.SetLocalizationComment(this._labelAvailableMemory, null);
            this.locExtender.SetLocalizingId(this._labelAvailableMemory, "DialogBoxes.AboutDlg.AvailableMemoryLabel");
            this._labelAvailableMemory.Location = new System.Drawing.Point(181, 180);
            this._labelAvailableMemory.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this._labelAvailableMemory.Name = "_labelAvailableMemory";
            this._labelAvailableMemory.Size = new System.Drawing.Size(143, 17);
            this._labelAvailableMemory.TabIndex = 17;
            this._labelAvailableMemory.Text = "Available memory:";
            // 
            // _labelAvailableDiskSpace
            // 
            this._labelAvailableDiskSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._labelAvailableDiskSpace.AutoSize = true;
            this._labelAvailableDiskSpace.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._labelAvailableDiskSpace, null);
            this.locExtender.SetLocalizationComment(this._labelAvailableDiskSpace, null);
            this.locExtender.SetLocalizingId(this._labelAvailableDiskSpace, "DialogBoxes.AboutDlg.AvailableDiskSpaceLabel");
            this._labelAvailableDiskSpace.Location = new System.Drawing.Point(181, 203);
            this._labelAvailableDiskSpace.Margin = new System.Windows.Forms.Padding(15, 4, 3, 15);
            this._labelAvailableDiskSpace.Name = "_labelAvailableDiskSpace";
            this._labelAvailableDiskSpace.Size = new System.Drawing.Size(140, 17);
            this._labelAvailableDiskSpace.TabIndex = 19;
            this._labelAvailableDiskSpace.Text = "Available disk space:";
            // 
            // fieldWorksIcon
            // 
            this.fieldWorksIcon.ErrorImage = null;
            this.fieldWorksIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.fieldWorksIcon.InitialImage = null;
            this.locExtender.SetLocalizableToolTip(this.fieldWorksIcon, null);
            this.locExtender.SetLocalizationComment(this.fieldWorksIcon, null);
            this.locExtender.SetLocalizationPriority(this.fieldWorksIcon, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.fieldWorksIcon, "AboutDlg.fieldWorksIcon");
            this.fieldWorksIcon.Location = new System.Drawing.Point(13, 13);
            this.fieldWorksIcon.Margin = new System.Windows.Forms.Padding(13, 13, 3, 3);
            this.fieldWorksIcon.Name = "fieldWorksIcon";
            this._tableLayout.SetRowSpan(this.fieldWorksIcon, 3);
            this.fieldWorksIcon.Size = new System.Drawing.Size(150, 163);
            this.fieldWorksIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fieldWorksIcon.TabIndex = 15;
            this.fieldWorksIcon.TabStop = false;
            // 
            // _linkWebsite
            // 
            this._linkWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._linkWebsite.AutoEllipsis = true;
            this._linkWebsite.AutoSize = true;
            this._linkWebsite.BackColor = System.Drawing.Color.Transparent;
            this._tableLayout.SetColumnSpan(this._linkWebsite, 3);
            this._linkWebsite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._linkWebsite.LinkArea = new System.Windows.Forms.LinkArea(28, 55);
            this.locExtender.SetLocalizableToolTip(this._linkWebsite, null);
            this.locExtender.SetLocalizationComment(this._linkWebsite, null);
            this.locExtender.SetLocalizingId(this._linkWebsite, "DialogBoxes.AboutDlg.WebsiteLabel");
            this._linkWebsite.Location = new System.Drawing.Point(13, 265);
            this._linkWebsite.Margin = new System.Windows.Forms.Padding(13, 12, 0, 0);
            this._linkWebsite.Name = "_linkWebsite";
            this._linkWebsite.Size = new System.Drawing.Size(514, 20);
            this._linkWebsite.TabIndex = 24;
            this._linkWebsite.TabStop = true;
            this._linkWebsite.Text = "For more information, visit {0}";
            this._linkWebsite.UseCompatibleTextRendering = true;
            this._linkWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebsiteLinkClicked);
            // 
            // _linkFeedback
            // 
            this._linkFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._linkFeedback.AutoEllipsis = true;
            this._linkFeedback.AutoSize = true;
            this._linkFeedback.BackColor = System.Drawing.Color.Transparent;
            this._tableLayout.SetColumnSpan(this._linkFeedback, 3);
            this._linkFeedback.LinkArea = new System.Windows.Forms.LinkArea(37, 55);
            this.locExtender.SetLocalizableToolTip(this._linkFeedback, null);
            this.locExtender.SetLocalizationComment(this._linkFeedback, null);
            this.locExtender.SetLocalizingId(this._linkFeedback, "DialogBoxes.AboutDlg.FeedbackLabel");
            this._linkFeedback.Location = new System.Drawing.Point(13, 285);
            this._linkFeedback.Margin = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this._linkFeedback.Name = "_linkFeedback";
            this._linkFeedback.Size = new System.Drawing.Size(514, 20);
            this._linkFeedback.TabIndex = 23;
            this._linkFeedback.TabStop = true;
            this._linkFeedback.Text = "Please report errors and comments to {0}";
            this._linkFeedback.UseCompatibleTextRendering = true;
            this._linkFeedback.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleFeedbackLinkClicked);
            // 
            // _labelBuild
            // 
            this._labelBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._labelBuild.AutoEllipsis = true;
            this._labelBuild.AutoSize = true;
            this._labelBuild.BackColor = System.Drawing.Color.Transparent;
            this._tableLayout.SetColumnSpan(this._labelBuild, 3);
            this._labelBuild.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._labelBuild, null);
            this.locExtender.SetLocalizationComment(this._labelBuild, null);
            this.locExtender.SetLocalizingId(this._labelBuild, "DialogBoxes.AboutDlg.BuildDateLabel");
            this._labelBuild.Location = new System.Drawing.Point(13, 236);
            this._labelBuild.Margin = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this._labelBuild.Name = "_labelBuild";
            this._labelBuild.Size = new System.Drawing.Size(514, 17);
            this._labelBuild.TabIndex = 22;
            this._labelBuild.Text = "Build: {0}";
            // 
            // _labelAppVersion
            // 
            this._labelAppVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._labelAppVersion.AutoSize = true;
            this._labelAppVersion.BackColor = System.Drawing.Color.Transparent;
            this._tableLayout.SetColumnSpan(this._labelAppVersion, 2);
            this._labelAppVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._labelAppVersion, null);
            this.locExtender.SetLocalizationComment(this._labelAppVersion, null);
            this.locExtender.SetLocalizingId(this._labelAppVersion, "DialogBoxes.AboutDlg.AppVersionLabel");
            this._labelAppVersion.Location = new System.Drawing.Point(181, 103);
            this._labelAppVersion.Margin = new System.Windows.Forms.Padding(15, 5, 0, 0);
            this._labelAppVersion.Name = "_labelAppVersion";
            this._labelAppVersion.Size = new System.Drawing.Size(346, 17);
            this._labelAppVersion.TabIndex = 14;
            this._labelAppVersion.Text = "Version: {0} {1} {2}";
            // 
            // _labelAvailableMemoryValue
            // 
            this._labelAvailableMemoryValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._labelAvailableMemoryValue.AutoEllipsis = true;
            this._labelAvailableMemoryValue.AutoSize = true;
            this._labelAvailableMemoryValue.BackColor = System.Drawing.Color.Transparent;
            this._labelAvailableMemoryValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._labelAvailableMemoryValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._labelAvailableMemoryValue, null);
            this.locExtender.SetLocalizationComment(this._labelAvailableMemoryValue, null);
            this.locExtender.SetLocalizingId(this._labelAvailableMemoryValue, "DialogBoxes.AboutDlg.AvailableMemoryValueLabel");
            this._labelAvailableMemoryValue.Location = new System.Drawing.Point(324, 179);
            this._labelAvailableMemoryValue.Margin = new System.Windows.Forms.Padding(0, 0, 13, 0);
            this._labelAvailableMemoryValue.Name = "_labelAvailableMemoryValue";
            this._labelAvailableMemoryValue.Size = new System.Drawing.Size(190, 19);
            this._labelAvailableMemoryValue.TabIndex = 18;
            this._labelAvailableMemoryValue.Text = "{0} MB out of {1} MB";
            this._labelAvailableMemoryValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _labelAvailableDiskSpaceValue
            // 
            this._labelAvailableDiskSpaceValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._labelAvailableDiskSpaceValue.AutoEllipsis = true;
            this._labelAvailableDiskSpaceValue.AutoSize = true;
            this._labelAvailableDiskSpaceValue.BackColor = System.Drawing.Color.Transparent;
            this._labelAvailableDiskSpaceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._labelAvailableDiskSpaceValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._labelAvailableDiskSpaceValue, null);
            this.locExtender.SetLocalizationComment(this._labelAvailableDiskSpaceValue, null);
            this.locExtender.SetLocalizingId(this._labelAvailableDiskSpaceValue, "DialogBoxes.AboutDlg.AvailableDiskSpaceValueLabel");
            this._labelAvailableDiskSpaceValue.Location = new System.Drawing.Point(324, 202);
            this._labelAvailableDiskSpaceValue.Margin = new System.Windows.Forms.Padding(0, 4, 13, 15);
            this._labelAvailableDiskSpaceValue.Name = "_labelAvailableDiskSpaceValue";
            this._labelAvailableDiskSpaceValue.Size = new System.Drawing.Size(190, 19);
            this._labelAvailableDiskSpaceValue.TabIndex = 20;
            this._labelAvailableDiskSpaceValue.Text = "{0} KB  ({1} GB)  Free on {2}";
            this._labelAvailableDiskSpaceValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _labelCopyright
            // 
            this._labelCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._labelCopyright.AutoSize = true;
            this._labelCopyright.BackColor = System.Drawing.Color.Transparent;
            this._tableLayout.SetColumnSpan(this._labelCopyright, 2);
            this._labelCopyright.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._labelCopyright, null);
            this.locExtender.SetLocalizationComment(this._labelCopyright, null);
            this.locExtender.SetLocalizingId(this._labelCopyright, "DialogBoxes.AboutDlg.CopyrightLabel");
            this._labelCopyright.Location = new System.Drawing.Point(181, 150);
            this._labelCopyright.Margin = new System.Windows.Forms.Padding(15, 30, 0, 12);
            this._labelCopyright.Name = "_labelCopyright";
            this._labelCopyright.Size = new System.Drawing.Size(346, 17);
            this._labelCopyright.TabIndex = 16;
            this._labelCopyright.Text = "{0}.";
            // 
            // _labelAppName
            // 
            this._labelAppName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._labelAppName.AutoSize = true;
            this._labelAppName.BackColor = System.Drawing.Color.Transparent;
            this._tableLayout.SetColumnSpan(this._labelAppName, 2);
            this._labelAppName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._labelAppName, null);
            this.locExtender.SetLocalizationComment(this._labelAppName, null);
            this.locExtender.SetLocalizingId(this._labelAppName, "DialogBoxes.AboutDlg.ApplicationNameLabel");
            this._labelAppName.Location = new System.Drawing.Point(178, 81);
            this._labelAppName.Margin = new System.Windows.Forms.Padding(12, 40, 0, 0);
            this._labelAppName.Name = "_labelAppName";
            this._labelAppName.Size = new System.Drawing.Size(349, 17);
            this._labelAppName.TabIndex = 13;
            this._labelAppName.Text = "Phonology Assistant";
            // 
            // _tableLayout
            // 
            this._tableLayout.AutoSize = true;
            this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayout.ColumnCount = 3;
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout.Controls.Add(this.fieldWorksIcon, 0, 0);
            this._tableLayout.Controls.Add(this._labelAppName, 1, 0);
            this._tableLayout.Controls.Add(this._labelAppVersion, 1, 1);
            this._tableLayout.Controls.Add(this._labelBuild, 0, 5);
            this._tableLayout.Controls.Add(this._labelAvailableDiskSpaceValue, 2, 4);
            this._tableLayout.Controls.Add(this._labelAvailableMemoryValue, 2, 3);
            this._tableLayout.Controls.Add(this._labelAvailableDiskSpace, 1, 4);
            this._tableLayout.Controls.Add(this._labelCopyright, 1, 2);
            this._tableLayout.Controls.Add(this._labelAvailableMemory, 1, 3);
            this._tableLayout.Controls.Add(this._linkWebsite, 0, 6);
            this._tableLayout.Controls.Add(this._linkFeedback, 0, 7);
            this._tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this._tableLayout.Location = new System.Drawing.Point(0, 0);
            this._tableLayout.Name = "_tableLayout";
            this._tableLayout.RowCount = 8;
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.Size = new System.Drawing.Size(527, 305);
            this._tableLayout.TabIndex = 1;
            // 
            // _tableLayoutButtons
            // 
            this._tableLayoutButtons.AutoSize = true;
            this._tableLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayoutButtons.ColumnCount = 1;
            this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutButtons.Controls.Add(buttonOk, 0, 0);
            this._tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._tableLayoutButtons.Location = new System.Drawing.Point(1, 278);
            this._tableLayoutButtons.Name = "_tableLayoutButtons";
            this._tableLayoutButtons.RowCount = 1;
            this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutButtons.Size = new System.Drawing.Size(548, 48);
            this._tableLayoutButtons.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this._tableLayout);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(548, 277);
            this.panel1.TabIndex = 3;
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
            // 
            // AboutDlg
            // 
            this.AcceptButton = buttonOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(550, 327);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._tableLayoutButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this, "AboutDlg.WindowTitle");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDlg";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.fieldWorksIcon)).EndInit();
            this._tableLayout.ResumeLayout(false);
            this._tableLayout.PerformLayout();
            this._tableLayoutButtons.ResumeLayout(false);
            this._tableLayoutButtons.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Initialization Methods
		/// ------------------------------------------------------------------------------------
		private void SetVersionNumber()
		{
			var version = new Version(Application.ProductVersion).ToString(3);
#if DEBUG
			_labelAppVersion.Text = string.Format(_labelAppVersion.Text, version,
				"(Debug version)", (_isBetaVersion ? "Beta" : string.Empty));
#else
			_labelAppVersion.Text = string.Format(_labelAppVersion.Text, version,
				string.Empty, (_isBetaVersion ? "Beta" : string.Empty));
#endif
		}

		/// ------------------------------------------------------------------------------------
		private void SetBuild()
		{
			var bldDate = File.GetLastWriteTime(Application.ExecutablePath);
			_labelBuild.Text = string.Format(_labelBuild.Text, bldDate.ToString("dd-MMM-yyyy"));
		}

		/// ------------------------------------------------------------------------------------
		private void SetCopyrightInformation()
		{
			// Get copyright information from assembly info. By doing this we don't have
			// to update the about dialog each year.
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			var attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

			// Try to get the copyright from the executing assembly.
			// If that fails, use a generic one.
			var copyright = (attributes.Length > 0 ?
				((AssemblyCopyrightAttribute)attributes[0]).Copyright :
				"(C) 2002-" + DateTime.Now.Year);

			_labelCopyright.Text = string.Format(_labelCopyright.Text, copyright.Replace("(C)", "©"));
		}

		/// ------------------------------------------------------------------------------------
		private void SetMemoryAndDiskInformation()
		{
			var strRoot = Application.ExecutablePath.Substring(0, 2);

            if (!strRoot.EndsWith(Path.VolumeSeparatorChar.ToString(), StringComparison.Ordinal))
				strRoot += Path.VolumeSeparatorChar;

			strRoot = Application.ExecutablePath.Substring(0, 2) + Path.DirectorySeparatorChar;

			// Set the memory information in MB.
			var ms = new Utils.MemoryStatus();
			Utils.GlobalMemoryStatus(ref ms);
			var available = ms.dwAvailPhys / Math.Pow(1024, 2);
			var total = ms.dwTotalPhys / Math.Pow(1024, 2);
			_labelAvailableMemoryValue.Text = string.Format(_labelAvailableMemoryValue.Text,
				available.ToString("###,###,###,###,###.##"),
				total.ToString("###,###,###,###,###"));

			// Set the disk space information in KB and GB.
			var freeDiskSpace = Utils.GetFreeDiskSpace(strRoot);
			var kbFree = freeDiskSpace / 1024;
			var gbFree = freeDiskSpace / Math.Pow(1024, 3);
			_labelAvailableDiskSpaceValue.Text = string.Format(_labelAvailableDiskSpaceValue.Text,
				kbFree.ToString("###,###,###,###,###,###,###"),
				gbFree.ToString("##,###.#"), strRoot);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void HandleFeedbackLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("mailto:PaFeedback@sil.org?subject=Bug%20Report");
			}
			catch (Exception err)
			{
				var msg = LocalizationManager.GetString("DialogBoxes.AboutDlg.EmailFailureMsg",
					"There was an error trying to create an e-mail. It's possible an e-mail client has not been installed or configured.");
				
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(err, msg);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWebsiteLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("http://phonologyassistant.sil.org");
			}
			catch (Exception err)
			{
				var msg = LocalizationManager.GetString("DialogBoxes.AboutDlg.GoingToWebsiteFailureMsg",
					"There was an error trying to go to the website {0}.");
				
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(err, msg, "http://phonologyassistant.sil.org");
			}
		}

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Escape:
                    {
                        this.Close();
                        return true;
                    }
                case Keys.Control | Keys.Tab:
                    {
                        return true;
                    }
                case Keys.Control | Keys.Shift | Keys.Tab:
                    {
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref message, keys);
        }
	}

	#endregion
}
