namespace SIL.Pa.UI.Controls
{
	partial class SortOptionsDropDown
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SortOptionsDropDown));
			this.rbUnicodeOrder = new System.Windows.Forms.RadioButton();
			this.rbPlaceArticulation = new System.Windows.Forms.RadioButton();
			this.rbMannerArticulation = new System.Windows.Forms.RadioButton();
			this.tblAdvSorting = new System.Windows.Forms.TableLayoutPanel();
			this.pnlAdvSort2 = new System.Windows.Forms.Panel();
			this.rbItem3rd = new System.Windows.Forms.RadioButton();
			this.rbBefore3rd = new System.Windows.Forms.RadioButton();
			this.rbAfter3rd = new System.Windows.Forms.RadioButton();
			this.pnlAdvSort0 = new System.Windows.Forms.Panel();
			this.rbItem1st = new System.Windows.Forms.RadioButton();
			this.rbBefore1st = new System.Windows.Forms.RadioButton();
			this.rbAfter1st = new System.Windows.Forms.RadioButton();
			this.pnlAdvSort1 = new System.Windows.Forms.Panel();
			this.rbAfter2nd = new System.Windows.Forms.RadioButton();
			this.rbBefore2nd = new System.Windows.Forms.RadioButton();
			this.rbItem2nd = new System.Windows.Forms.RadioButton();
			this.chkAfterRL = new System.Windows.Forms.CheckBox();
			this.chkItemRL = new System.Windows.Forms.CheckBox();
			this.chkBeforeRL = new System.Windows.Forms.CheckBox();
			this.lblBefore = new System.Windows.Forms.Label();
			this.lblThird = new System.Windows.Forms.Label();
			this.lblAfter = new System.Windows.Forms.Label();
			this.lblItem = new System.Windows.Forms.Label();
			this.lblFirst = new System.Windows.Forms.Label();
			this.lblRL = new System.Windows.Forms.Label();
			this.lblSecond = new System.Windows.Forms.Label();
			this.tblLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this.flowPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnHelp = new SIL.Pa.UI.Controls.PopupDialogHelpButton();
			this.btnClose = new SIL.Pa.UI.Controls.PopupDialogCloseButton();
			this.pnlAdvOptions = new SilTools.Controls.SilPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tblAdvSorting.SuspendLayout();
			this.pnlAdvSort2.SuspendLayout();
			this.pnlAdvSort0.SuspendLayout();
			this.pnlAdvSort1.SuspendLayout();
			this.tblLayoutOuter.SuspendLayout();
			this.flowPanelButtons.SuspendLayout();
			this.pnlAdvOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// rbUnicodeOrder
			// 
			resources.ApplyResources(this.rbUnicodeOrder, "rbUnicodeOrder");
			this.rbUnicodeOrder.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbUnicodeOrder, null);
			this.locExtender.SetLocalizationComment(this.rbUnicodeOrder, null);
			this.locExtender.SetLocalizingId(this.rbUnicodeOrder, "SortOptionsDropDown.rbUnicodeOrder");
			this.rbUnicodeOrder.Name = "rbUnicodeOrder";
			this.rbUnicodeOrder.TabStop = true;
			this.rbUnicodeOrder.UseVisualStyleBackColor = false;
			this.rbUnicodeOrder.Click += new System.EventHandler(this.HandleSortTypeChecked);
			// 
			// rbPlaceArticulation
			// 
			resources.ApplyResources(this.rbPlaceArticulation, "rbPlaceArticulation");
			this.rbPlaceArticulation.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbPlaceArticulation, null);
			this.locExtender.SetLocalizationComment(this.rbPlaceArticulation, null);
			this.locExtender.SetLocalizingId(this.rbPlaceArticulation, "SortOptionsDropDown.rbPlaceArticulation");
			this.rbPlaceArticulation.Name = "rbPlaceArticulation";
			this.rbPlaceArticulation.TabStop = true;
			this.rbPlaceArticulation.UseVisualStyleBackColor = false;
			this.rbPlaceArticulation.Click += new System.EventHandler(this.HandleSortTypeChecked);
			// 
			// rbMannerArticulation
			// 
			resources.ApplyResources(this.rbMannerArticulation, "rbMannerArticulation");
			this.rbMannerArticulation.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbMannerArticulation, null);
			this.locExtender.SetLocalizationComment(this.rbMannerArticulation, null);
			this.locExtender.SetLocalizingId(this.rbMannerArticulation, "SortOptionsDropDown.rbMannerArticulation");
			this.rbMannerArticulation.Name = "rbMannerArticulation";
			this.rbMannerArticulation.TabStop = true;
			this.rbMannerArticulation.UseVisualStyleBackColor = false;
			this.rbMannerArticulation.Click += new System.EventHandler(this.HandleSortTypeChecked);
			// 
			// tblAdvSorting
			// 
			resources.ApplyResources(this.tblAdvSorting, "tblAdvSorting");
			this.tblAdvSorting.Controls.Add(this.pnlAdvSort2, 3, 1);
			this.tblAdvSorting.Controls.Add(this.pnlAdvSort0, 1, 1);
			this.tblAdvSorting.Controls.Add(this.pnlAdvSort1, 2, 1);
			this.tblAdvSorting.Controls.Add(this.chkAfterRL, 4, 3);
			this.tblAdvSorting.Controls.Add(this.chkItemRL, 4, 2);
			this.tblAdvSorting.Controls.Add(this.chkBeforeRL, 4, 1);
			this.tblAdvSorting.Controls.Add(this.lblBefore, 0, 1);
			this.tblAdvSorting.Controls.Add(this.lblThird, 3, 0);
			this.tblAdvSorting.Controls.Add(this.lblAfter, 0, 3);
			this.tblAdvSorting.Controls.Add(this.lblItem, 0, 2);
			this.tblAdvSorting.Controls.Add(this.lblFirst, 1, 0);
			this.tblAdvSorting.Controls.Add(this.lblRL, 4, 0);
			this.tblAdvSorting.Controls.Add(this.lblSecond, 2, 0);
			this.tblAdvSorting.Name = "tblAdvSorting";
			// 
			// pnlAdvSort2
			// 
			resources.ApplyResources(this.pnlAdvSort2, "pnlAdvSort2");
			this.pnlAdvSort2.Controls.Add(this.rbItem3rd);
			this.pnlAdvSort2.Controls.Add(this.rbBefore3rd);
			this.pnlAdvSort2.Controls.Add(this.rbAfter3rd);
			this.pnlAdvSort2.MinimumSize = new System.Drawing.Size(27, 0);
			this.pnlAdvSort2.Name = "pnlAdvSort2";
			this.tblAdvSorting.SetRowSpan(this.pnlAdvSort2, 3);
			// 
			// rbItem3rd
			// 
			resources.ApplyResources(this.rbItem3rd, "rbItem3rd");
			this.rbItem3rd.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbItem3rd, null);
			this.locExtender.SetLocalizationComment(this.rbItem3rd, null);
			this.locExtender.SetLocalizationPriority(this.rbItem3rd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbItem3rd, "SortOptionsDropDown.rbItem3rd");
			this.rbItem3rd.Name = "rbItem3rd";
			this.rbItem3rd.UseVisualStyleBackColor = false;
			this.rbItem3rd.Click += new System.EventHandler(this.HandleCheckedColumn2);
			// 
			// rbBefore3rd
			// 
			resources.ApplyResources(this.rbBefore3rd, "rbBefore3rd");
			this.rbBefore3rd.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbBefore3rd, null);
			this.locExtender.SetLocalizationComment(this.rbBefore3rd, null);
			this.locExtender.SetLocalizationPriority(this.rbBefore3rd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbBefore3rd, "SortOptionsDropDown.rbBefore3rd");
			this.rbBefore3rd.Name = "rbBefore3rd";
			this.rbBefore3rd.UseVisualStyleBackColor = false;
			this.rbBefore3rd.Click += new System.EventHandler(this.HandleCheckedColumn2);
			// 
			// rbAfter3rd
			// 
			resources.ApplyResources(this.rbAfter3rd, "rbAfter3rd");
			this.rbAfter3rd.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbAfter3rd, null);
			this.locExtender.SetLocalizationComment(this.rbAfter3rd, null);
			this.locExtender.SetLocalizationPriority(this.rbAfter3rd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbAfter3rd, "SortOptionsDropDown.rbAfter3rd");
			this.rbAfter3rd.Name = "rbAfter3rd";
			this.rbAfter3rd.UseVisualStyleBackColor = false;
			this.rbAfter3rd.Click += new System.EventHandler(this.HandleCheckedColumn2);
			// 
			// pnlAdvSort0
			// 
			resources.ApplyResources(this.pnlAdvSort0, "pnlAdvSort0");
			this.pnlAdvSort0.Controls.Add(this.rbItem1st);
			this.pnlAdvSort0.Controls.Add(this.rbBefore1st);
			this.pnlAdvSort0.Controls.Add(this.rbAfter1st);
			this.pnlAdvSort0.MinimumSize = new System.Drawing.Size(27, 0);
			this.pnlAdvSort0.Name = "pnlAdvSort0";
			this.tblAdvSorting.SetRowSpan(this.pnlAdvSort0, 3);
			// 
			// rbItem1st
			// 
			resources.ApplyResources(this.rbItem1st, "rbItem1st");
			this.rbItem1st.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbItem1st, null);
			this.locExtender.SetLocalizationComment(this.rbItem1st, null);
			this.locExtender.SetLocalizationPriority(this.rbItem1st, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbItem1st, "SortOptionsDropDown.rbItem1st");
			this.rbItem1st.Name = "rbItem1st";
			this.rbItem1st.UseVisualStyleBackColor = false;
			this.rbItem1st.Click += new System.EventHandler(this.HandleCheckedColumn0);
			// 
			// rbBefore1st
			// 
			resources.ApplyResources(this.rbBefore1st, "rbBefore1st");
			this.rbBefore1st.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbBefore1st, null);
			this.locExtender.SetLocalizationComment(this.rbBefore1st, null);
			this.locExtender.SetLocalizationPriority(this.rbBefore1st, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbBefore1st, "SortOptionsDropDown.rbBefore1st");
			this.rbBefore1st.Name = "rbBefore1st";
			this.rbBefore1st.Tag = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidDoNothingToolTip;
			this.rbBefore1st.UseVisualStyleBackColor = false;
			this.rbBefore1st.Click += new System.EventHandler(this.HandleCheckedColumn0);
			// 
			// rbAfter1st
			// 
			resources.ApplyResources(this.rbAfter1st, "rbAfter1st");
			this.rbAfter1st.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbAfter1st, null);
			this.locExtender.SetLocalizationComment(this.rbAfter1st, null);
			this.locExtender.SetLocalizationPriority(this.rbAfter1st, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbAfter1st, "SortOptionsDropDown.rbAfter1st");
			this.rbAfter1st.Name = "rbAfter1st";
			this.rbAfter1st.UseVisualStyleBackColor = false;
			this.rbAfter1st.Click += new System.EventHandler(this.HandleCheckedColumn0);
			// 
			// pnlAdvSort1
			// 
			resources.ApplyResources(this.pnlAdvSort1, "pnlAdvSort1");
			this.pnlAdvSort1.Controls.Add(this.rbAfter2nd);
			this.pnlAdvSort1.Controls.Add(this.rbBefore2nd);
			this.pnlAdvSort1.Controls.Add(this.rbItem2nd);
			this.pnlAdvSort1.MinimumSize = new System.Drawing.Size(27, 0);
			this.pnlAdvSort1.Name = "pnlAdvSort1";
			this.tblAdvSorting.SetRowSpan(this.pnlAdvSort1, 3);
			// 
			// rbAfter2nd
			// 
			resources.ApplyResources(this.rbAfter2nd, "rbAfter2nd");
			this.rbAfter2nd.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbAfter2nd, null);
			this.locExtender.SetLocalizationComment(this.rbAfter2nd, null);
			this.locExtender.SetLocalizationPriority(this.rbAfter2nd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbAfter2nd, "SortOptionsDropDown.rbAfter2nd");
			this.rbAfter2nd.Name = "rbAfter2nd";
			this.rbAfter2nd.UseVisualStyleBackColor = false;
			this.rbAfter2nd.Click += new System.EventHandler(this.HandleCheckedColumn1);
			// 
			// rbBefore2nd
			// 
			resources.ApplyResources(this.rbBefore2nd, "rbBefore2nd");
			this.rbBefore2nd.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbBefore2nd, null);
			this.locExtender.SetLocalizationComment(this.rbBefore2nd, null);
			this.locExtender.SetLocalizationPriority(this.rbBefore2nd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbBefore2nd, "SortOptionsDropDown.rbBefore2nd");
			this.rbBefore2nd.Name = "rbBefore2nd";
			this.rbBefore2nd.UseVisualStyleBackColor = false;
			this.rbBefore2nd.Click += new System.EventHandler(this.HandleCheckedColumn1);
			// 
			// rbItem2nd
			// 
			resources.ApplyResources(this.rbItem2nd, "rbItem2nd");
			this.rbItem2nd.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbItem2nd, null);
			this.locExtender.SetLocalizationComment(this.rbItem2nd, null);
			this.locExtender.SetLocalizationPriority(this.rbItem2nd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbItem2nd, "SortOptionsDropDown.rbItem2nd");
			this.rbItem2nd.Name = "rbItem2nd";
			this.rbItem2nd.UseVisualStyleBackColor = false;
			this.rbItem2nd.Click += new System.EventHandler(this.HandleCheckedColumn1);
			// 
			// chkAfterRL
			// 
			resources.ApplyResources(this.chkAfterRL, "chkAfterRL");
			this.chkAfterRL.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.chkAfterRL, null);
			this.locExtender.SetLocalizationComment(this.chkAfterRL, null);
			this.locExtender.SetLocalizationPriority(this.chkAfterRL, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.chkAfterRL, "SortOptionsDropDown.chkAfterRL");
			this.chkAfterRL.Name = "chkAfterRL";
			this.chkAfterRL.UseVisualStyleBackColor = false;
			this.chkAfterRL.Click += new System.EventHandler(this.HandleRightToLeftCheckBoxChecked);
			// 
			// chkItemRL
			// 
			resources.ApplyResources(this.chkItemRL, "chkItemRL");
			this.chkItemRL.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.chkItemRL, null);
			this.locExtender.SetLocalizationComment(this.chkItemRL, null);
			this.locExtender.SetLocalizationPriority(this.chkItemRL, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.chkItemRL, "SortOptionsDropDown.chkItemRL");
			this.chkItemRL.Name = "chkItemRL";
			this.chkItemRL.UseVisualStyleBackColor = false;
			this.chkItemRL.Click += new System.EventHandler(this.HandleRightToLeftCheckBoxChecked);
			// 
			// chkBeforeRL
			// 
			resources.ApplyResources(this.chkBeforeRL, "chkBeforeRL");
			this.chkBeforeRL.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.chkBeforeRL, null);
			this.locExtender.SetLocalizationComment(this.chkBeforeRL, null);
			this.locExtender.SetLocalizationPriority(this.chkBeforeRL, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.chkBeforeRL, "SortOptionsDropDown.chkBeforeRL");
			this.chkBeforeRL.Name = "chkBeforeRL";
			this.chkBeforeRL.UseVisualStyleBackColor = false;
			this.chkBeforeRL.Click += new System.EventHandler(this.HandleRightToLeftCheckBoxChecked);
			// 
			// lblBefore
			// 
			resources.ApplyResources(this.lblBefore, "lblBefore");
			this.lblBefore.AutoEllipsis = true;
			this.lblBefore.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblBefore, null);
			this.locExtender.SetLocalizationComment(this.lblBefore, null);
			this.locExtender.SetLocalizingId(this.lblBefore, "SortOptionsDropDown.lblBefore");
			this.lblBefore.Name = "lblBefore";
			// 
			// lblThird
			// 
			resources.ApplyResources(this.lblThird, "lblThird");
			this.lblThird.AutoEllipsis = true;
			this.lblThird.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblThird, null);
			this.locExtender.SetLocalizationComment(this.lblThird, null);
			this.locExtender.SetLocalizingId(this.lblThird, "SortOptionsDropDown.lblThird");
			this.lblThird.MinimumSize = new System.Drawing.Size(27, 0);
			this.lblThird.Name = "lblThird";
			// 
			// lblAfter
			// 
			resources.ApplyResources(this.lblAfter, "lblAfter");
			this.lblAfter.AutoEllipsis = true;
			this.lblAfter.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblAfter, null);
			this.locExtender.SetLocalizationComment(this.lblAfter, null);
			this.locExtender.SetLocalizingId(this.lblAfter, "SortOptionsDropDown.lblAfter");
			this.lblAfter.Name = "lblAfter";
			// 
			// lblItem
			// 
			resources.ApplyResources(this.lblItem, "lblItem");
			this.lblItem.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblItem, null);
			this.locExtender.SetLocalizationComment(this.lblItem, null);
			this.locExtender.SetLocalizingId(this.lblItem, "SortOptionsDropDown.lblItem");
			this.lblItem.Name = "lblItem";
			// 
			// lblFirst
			// 
			resources.ApplyResources(this.lblFirst, "lblFirst");
			this.lblFirst.AutoEllipsis = true;
			this.lblFirst.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblFirst, null);
			this.locExtender.SetLocalizationComment(this.lblFirst, null);
			this.locExtender.SetLocalizingId(this.lblFirst, "SortOptionsDropDown.lblFirst");
			this.lblFirst.MinimumSize = new System.Drawing.Size(27, 0);
			this.lblFirst.Name = "lblFirst";
			// 
			// lblRL
			// 
			resources.ApplyResources(this.lblRL, "lblRL");
			this.lblRL.AutoEllipsis = true;
			this.lblRL.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblRL, "Right-to-Left");
			this.locExtender.SetLocalizationComment(this.lblRL, null);
			this.locExtender.SetLocalizingId(this.lblRL, "SortOptionsDropDown.lblRL");
			this.lblRL.MinimumSize = new System.Drawing.Size(27, 0);
			this.lblRL.Name = "lblRL";
			// 
			// lblSecond
			// 
			resources.ApplyResources(this.lblSecond, "lblSecond");
			this.lblSecond.AutoEllipsis = true;
			this.lblSecond.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblSecond, null);
			this.locExtender.SetLocalizationComment(this.lblSecond, null);
			this.locExtender.SetLocalizingId(this.lblSecond, "SortOptionsDropDown.lblSecond");
			this.lblSecond.MinimumSize = new System.Drawing.Size(27, 0);
			this.lblSecond.Name = "lblSecond";
			// 
			// tblLayoutOuter
			// 
			resources.ApplyResources(this.tblLayoutOuter, "tblLayoutOuter");
			this.tblLayoutOuter.Controls.Add(this.flowPanelButtons, 0, 4);
			this.tblLayoutOuter.Controls.Add(this.pnlAdvOptions, 0, 3);
			this.tblLayoutOuter.Controls.Add(this.rbPlaceArticulation, 0, 0);
			this.tblLayoutOuter.Controls.Add(this.rbMannerArticulation, 0, 1);
			this.tblLayoutOuter.Controls.Add(this.rbUnicodeOrder, 0, 2);
			this.tblLayoutOuter.Name = "tblLayoutOuter";
			this.tblLayoutOuter.SizeChanged += new System.EventHandler(this.HandleOuterTableLayoutSizeChanged);
			// 
			// flowPanelButtons
			// 
			resources.ApplyResources(this.flowPanelButtons, "flowPanelButtons");
			this.flowPanelButtons.Controls.Add(this.btnHelp);
			this.flowPanelButtons.Controls.Add(this.btnClose);
			this.flowPanelButtons.Name = "flowPanelButtons";
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.FlatAppearance.BorderSize = 0;
			this.btnHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.btnHelp.ImageHot = ((System.Drawing.Image)(resources.GetObject("btnHelp.ImageHot")));
			this.btnHelp.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnHelp.ImageNormal")));
			this.locExtender.SetLocalizableToolTip(this.btnHelp, "Help");
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizingId(this.btnHelp, "SortOptionsDropDown.btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.HandleHelpButtonClick);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackgroundImage = global::SIL.Pa.Properties.Resources.PopupDialogButtonClose;
			this.btnClose.FlatAppearance.BorderSize = 0;
			this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.btnClose.ImageHot = ((System.Drawing.Image)(resources.GetObject("btnClose.ImageHot")));
			this.btnClose.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnClose.ImageNormal")));
			this.locExtender.SetLocalizableToolTip(this.btnClose, "Close");
			this.locExtender.SetLocalizationComment(this.btnClose, null);
			this.locExtender.SetLocalizingId(this.btnClose, "SortOptionsDropDown.btnClose");
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.HandleCloseButtonClick);
			// 
			// pnlAdvOptions
			// 
			resources.ApplyResources(this.pnlAdvOptions, "pnlAdvOptions");
			this.pnlAdvOptions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlAdvOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlAdvOptions.ClipTextForChildControls = true;
			this.pnlAdvOptions.ControlReceivingFocusOnMnemonic = null;
			this.pnlAdvOptions.Controls.Add(this.tblAdvSorting);
			this.pnlAdvOptions.DoubleBuffered = true;
			this.pnlAdvOptions.DrawOnlyBottomBorder = false;
			this.pnlAdvOptions.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlAdvOptions, null);
			this.locExtender.SetLocalizationComment(this.pnlAdvOptions, null);
			this.locExtender.SetLocalizingId(this.pnlAdvOptions, "SortOptionsDropDown.pnlAdvOptions");
			this.pnlAdvOptions.MnemonicGeneratesClick = false;
			this.pnlAdvOptions.Name = "pnlAdvOptions";
			this.pnlAdvOptions.PaintExplorerBarBackground = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "User Interface Controls";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SortOptionsDropDown
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.tblLayoutOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SortOptionsDropDown.SortOptionsDropDown");
			this.Name = "SortOptionsDropDown";
			this.tblAdvSorting.ResumeLayout(false);
			this.tblAdvSorting.PerformLayout();
			this.pnlAdvSort2.ResumeLayout(false);
			this.pnlAdvSort0.ResumeLayout(false);
			this.pnlAdvSort1.ResumeLayout(false);
			this.tblLayoutOuter.ResumeLayout(false);
			this.tblLayoutOuter.PerformLayout();
			this.flowPanelButtons.ResumeLayout(false);
			this.pnlAdvOptions.ResumeLayout(false);
			this.pnlAdvOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbUnicodeOrder;
		private System.Windows.Forms.RadioButton rbPlaceArticulation;
		private System.Windows.Forms.RadioButton rbMannerArticulation;
		private System.Windows.Forms.CheckBox chkAfterRL;
		private System.Windows.Forms.CheckBox chkItemRL;
		private System.Windows.Forms.CheckBox chkBeforeRL;
		private System.Windows.Forms.RadioButton rbAfter3rd;
		private System.Windows.Forms.RadioButton rbAfter2nd;
		private System.Windows.Forms.RadioButton rbAfter1st;
		private System.Windows.Forms.RadioButton rbItem3rd;
		private System.Windows.Forms.RadioButton rbItem2nd;
		private System.Windows.Forms.RadioButton rbItem1st;
		private System.Windows.Forms.Label lblRL;
		private System.Windows.Forms.Label lblThird;
		private System.Windows.Forms.Label lblSecond;
		private System.Windows.Forms.Label lblFirst;
		private System.Windows.Forms.Label lblAfter;
		private System.Windows.Forms.Label lblItem;
		private System.Windows.Forms.Label lblBefore;
		private System.Windows.Forms.RadioButton rbBefore3rd;
		private System.Windows.Forms.RadioButton rbBefore2nd;
		private System.Windows.Forms.RadioButton rbBefore1st;
		private System.Windows.Forms.TableLayoutPanel tblAdvSorting;
		private System.Windows.Forms.Panel pnlAdvSort0;
		private System.Windows.Forms.Panel pnlAdvSort2;
		private System.Windows.Forms.Panel pnlAdvSort1;
		private System.Windows.Forms.TableLayoutPanel tblLayoutOuter;
		private PopupDialogHelpButton btnHelp;
		private PopupDialogCloseButton btnClose;
		private SilTools.Controls.SilPanel pnlAdvOptions;
		private System.Windows.Forms.FlowLayoutPanel flowPanelButtons;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
