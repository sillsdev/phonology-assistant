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
			this.pnlAdvOptions = new SilTools.Controls.SilPanel();
			this.flowPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnClose = new SIL.Pa.UI.Controls.PopupDialogCloseButton();
			this.btnHelp = new SIL.Pa.UI.Controls.PopupDialogHelpButton();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tblAdvSorting.SuspendLayout();
			this.pnlAdvSort2.SuspendLayout();
			this.pnlAdvSort0.SuspendLayout();
			this.pnlAdvSort1.SuspendLayout();
			this.tblLayoutOuter.SuspendLayout();
			this.pnlAdvOptions.SuspendLayout();
			this.flowPanelButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// rbUnicodeOrder
			// 
			this.rbUnicodeOrder.AutoSize = true;
			this.rbUnicodeOrder.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbUnicodeOrder, null);
			this.locExtender.SetLocalizationComment(this.rbUnicodeOrder, null);
			this.locExtender.SetLocalizingId(this.rbUnicodeOrder, "SortOptionsDropDown.rbUnicodeOrder");
			this.rbUnicodeOrder.Location = new System.Drawing.Point(12, 49);
			this.rbUnicodeOrder.Margin = new System.Windows.Forms.Padding(12, 0, 2, 0);
			this.rbUnicodeOrder.Name = "rbUnicodeOrder";
			this.rbUnicodeOrder.Size = new System.Drawing.Size(94, 17);
			this.rbUnicodeOrder.TabIndex = 2;
			this.rbUnicodeOrder.TabStop = true;
			this.rbUnicodeOrder.Text = "&Unicode Order";
			this.rbUnicodeOrder.UseVisualStyleBackColor = false;
			this.rbUnicodeOrder.Click += new System.EventHandler(this.HandleSortTypeChecked);
			// 
			// rbPlaceArticulation
			// 
			this.rbPlaceArticulation.AutoSize = true;
			this.rbPlaceArticulation.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbPlaceArticulation, null);
			this.locExtender.SetLocalizationComment(this.rbPlaceArticulation, null);
			this.locExtender.SetLocalizingId(this.rbPlaceArticulation, "SortOptionsDropDown.rbPlaceArticulation");
			this.rbPlaceArticulation.Location = new System.Drawing.Point(12, 5);
			this.rbPlaceArticulation.Margin = new System.Windows.Forms.Padding(12, 5, 2, 5);
			this.rbPlaceArticulation.Name = "rbPlaceArticulation";
			this.rbPlaceArticulation.Size = new System.Drawing.Size(119, 17);
			this.rbPlaceArticulation.TabIndex = 0;
			this.rbPlaceArticulation.TabStop = true;
			this.rbPlaceArticulation.Text = "&Place of Articulation";
			this.rbPlaceArticulation.UseVisualStyleBackColor = false;
			this.rbPlaceArticulation.Click += new System.EventHandler(this.HandleSortTypeChecked);
			// 
			// rbMannerArticulation
			// 
			this.rbMannerArticulation.AutoSize = true;
			this.rbMannerArticulation.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbMannerArticulation, null);
			this.locExtender.SetLocalizationComment(this.rbMannerArticulation, null);
			this.locExtender.SetLocalizingId(this.rbMannerArticulation, "SortOptionsDropDown.rbMannerArticulation");
			this.rbMannerArticulation.Location = new System.Drawing.Point(12, 27);
			this.rbMannerArticulation.Margin = new System.Windows.Forms.Padding(12, 0, 2, 5);
			this.rbMannerArticulation.Name = "rbMannerArticulation";
			this.rbMannerArticulation.Size = new System.Drawing.Size(128, 17);
			this.rbMannerArticulation.TabIndex = 1;
			this.rbMannerArticulation.TabStop = true;
			this.rbMannerArticulation.Text = "&Manner of Articulation";
			this.rbMannerArticulation.UseVisualStyleBackColor = false;
			this.rbMannerArticulation.Click += new System.EventHandler(this.HandleSortTypeChecked);
			// 
			// tblAdvSorting
			// 
			this.tblAdvSorting.AutoSize = true;
			this.tblAdvSorting.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblAdvSorting.ColumnCount = 5;
			this.tblAdvSorting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblAdvSorting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblAdvSorting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblAdvSorting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblAdvSorting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
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
			this.tblAdvSorting.Location = new System.Drawing.Point(0, 0);
			this.tblAdvSorting.Margin = new System.Windows.Forms.Padding(2);
			this.tblAdvSorting.Name = "tblAdvSorting";
			this.tblAdvSorting.RowCount = 4;
			this.tblAdvSorting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tblAdvSorting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tblAdvSorting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tblAdvSorting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tblAdvSorting.Size = new System.Drawing.Size(191, 96);
			this.tblAdvSorting.TabIndex = 0;
			// 
			// pnlAdvSort2
			// 
			this.pnlAdvSort2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAdvSort2.Controls.Add(this.rbItem3rd);
			this.pnlAdvSort2.Controls.Add(this.rbBefore3rd);
			this.pnlAdvSort2.Controls.Add(this.rbAfter3rd);
			this.pnlAdvSort2.Location = new System.Drawing.Point(129, 24);
			this.pnlAdvSort2.Margin = new System.Windows.Forms.Padding(0);
			this.pnlAdvSort2.MinimumSize = new System.Drawing.Size(27, 0);
			this.pnlAdvSort2.Name = "pnlAdvSort2";
			this.tblAdvSorting.SetRowSpan(this.pnlAdvSort2, 3);
			this.pnlAdvSort2.Size = new System.Drawing.Size(31, 72);
			this.pnlAdvSort2.TabIndex = 6;
			// 
			// rbItem3rd
			// 
			this.rbItem3rd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbItem3rd.BackColor = System.Drawing.Color.Transparent;
			this.rbItem3rd.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbItem3rd, null);
			this.locExtender.SetLocalizationComment(this.rbItem3rd, null);
			this.locExtender.SetLocalizationPriority(this.rbItem3rd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbItem3rd, "SortOptionsDropDown.rbItem3rd");
			this.rbItem3rd.Location = new System.Drawing.Point(1, 28);
			this.rbItem3rd.Margin = new System.Windows.Forms.Padding(2);
			this.rbItem3rd.Name = "rbItem3rd";
			this.rbItem3rd.Size = new System.Drawing.Size(29, 16);
			this.rbItem3rd.TabIndex = 1;
			this.rbItem3rd.UseVisualStyleBackColor = false;
			this.rbItem3rd.Click += new System.EventHandler(this.HandleCheckedColumn2);
			// 
			// rbBefore3rd
			// 
			this.rbBefore3rd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbBefore3rd.BackColor = System.Drawing.Color.Transparent;
			this.rbBefore3rd.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbBefore3rd, null);
			this.locExtender.SetLocalizationComment(this.rbBefore3rd, null);
			this.locExtender.SetLocalizationPriority(this.rbBefore3rd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbBefore3rd, "SortOptionsDropDown.rbBefore3rd");
			this.rbBefore3rd.Location = new System.Drawing.Point(1, 4);
			this.rbBefore3rd.Margin = new System.Windows.Forms.Padding(2);
			this.rbBefore3rd.Name = "rbBefore3rd";
			this.rbBefore3rd.Size = new System.Drawing.Size(29, 16);
			this.rbBefore3rd.TabIndex = 0;
			this.rbBefore3rd.UseVisualStyleBackColor = false;
			this.rbBefore3rd.Click += new System.EventHandler(this.HandleCheckedColumn2);
			// 
			// rbAfter3rd
			// 
			this.rbAfter3rd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbAfter3rd.BackColor = System.Drawing.Color.Transparent;
			this.rbAfter3rd.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbAfter3rd, null);
			this.locExtender.SetLocalizationComment(this.rbAfter3rd, null);
			this.locExtender.SetLocalizationPriority(this.rbAfter3rd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbAfter3rd, "SortOptionsDropDown.rbAfter3rd");
			this.rbAfter3rd.Location = new System.Drawing.Point(1, 52);
			this.rbAfter3rd.Margin = new System.Windows.Forms.Padding(2);
			this.rbAfter3rd.Name = "rbAfter3rd";
			this.rbAfter3rd.Size = new System.Drawing.Size(29, 16);
			this.rbAfter3rd.TabIndex = 2;
			this.rbAfter3rd.UseVisualStyleBackColor = false;
			this.rbAfter3rd.Click += new System.EventHandler(this.HandleCheckedColumn2);
			// 
			// pnlAdvSort0
			// 
			this.pnlAdvSort0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAdvSort0.Controls.Add(this.rbItem1st);
			this.pnlAdvSort0.Controls.Add(this.rbBefore1st);
			this.pnlAdvSort0.Controls.Add(this.rbAfter1st);
			this.pnlAdvSort0.Location = new System.Drawing.Point(67, 24);
			this.pnlAdvSort0.Margin = new System.Windows.Forms.Padding(0);
			this.pnlAdvSort0.MinimumSize = new System.Drawing.Size(27, 0);
			this.pnlAdvSort0.Name = "pnlAdvSort0";
			this.tblAdvSorting.SetRowSpan(this.pnlAdvSort0, 3);
			this.pnlAdvSort0.Size = new System.Drawing.Size(31, 72);
			this.pnlAdvSort0.TabIndex = 4;
			// 
			// rbItem1st
			// 
			this.rbItem1st.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbItem1st.BackColor = System.Drawing.Color.Transparent;
			this.rbItem1st.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbItem1st, null);
			this.locExtender.SetLocalizationComment(this.rbItem1st, null);
			this.locExtender.SetLocalizationPriority(this.rbItem1st, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbItem1st, "SortOptionsDropDown.rbItem1st");
			this.rbItem1st.Location = new System.Drawing.Point(1, 28);
			this.rbItem1st.Margin = new System.Windows.Forms.Padding(2);
			this.rbItem1st.Name = "rbItem1st";
			this.rbItem1st.Size = new System.Drawing.Size(29, 16);
			this.rbItem1st.TabIndex = 1;
			this.rbItem1st.UseVisualStyleBackColor = false;
			this.rbItem1st.Click += new System.EventHandler(this.HandleCheckedColumn0);
			// 
			// rbBefore1st
			// 
			this.rbBefore1st.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbBefore1st.BackColor = System.Drawing.Color.Transparent;
			this.rbBefore1st.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbBefore1st, null);
			this.locExtender.SetLocalizationComment(this.rbBefore1st, null);
			this.locExtender.SetLocalizationPriority(this.rbBefore1st, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbBefore1st, "SortOptionsDropDown.rbBefore1st");
			this.rbBefore1st.Location = new System.Drawing.Point(1, 4);
			this.rbBefore1st.Margin = new System.Windows.Forms.Padding(2);
			this.rbBefore1st.Name = "rbBefore1st";
			this.rbBefore1st.Size = new System.Drawing.Size(29, 16);
			this.rbBefore1st.TabIndex = 0;
			this.rbBefore1st.Tag = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidDoNothingToolTip;
			this.rbBefore1st.UseVisualStyleBackColor = false;
			this.rbBefore1st.Click += new System.EventHandler(this.HandleCheckedColumn0);
			// 
			// rbAfter1st
			// 
			this.rbAfter1st.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbAfter1st.BackColor = System.Drawing.Color.Transparent;
			this.rbAfter1st.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbAfter1st, null);
			this.locExtender.SetLocalizationComment(this.rbAfter1st, null);
			this.locExtender.SetLocalizationPriority(this.rbAfter1st, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbAfter1st, "SortOptionsDropDown.rbAfter1st");
			this.rbAfter1st.Location = new System.Drawing.Point(1, 52);
			this.rbAfter1st.Margin = new System.Windows.Forms.Padding(2);
			this.rbAfter1st.Name = "rbAfter1st";
			this.rbAfter1st.Size = new System.Drawing.Size(29, 16);
			this.rbAfter1st.TabIndex = 2;
			this.rbAfter1st.UseVisualStyleBackColor = false;
			this.rbAfter1st.Click += new System.EventHandler(this.HandleCheckedColumn0);
			// 
			// pnlAdvSort1
			// 
			this.pnlAdvSort1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAdvSort1.Controls.Add(this.rbAfter2nd);
			this.pnlAdvSort1.Controls.Add(this.rbBefore2nd);
			this.pnlAdvSort1.Controls.Add(this.rbItem2nd);
			this.pnlAdvSort1.Location = new System.Drawing.Point(98, 24);
			this.pnlAdvSort1.Margin = new System.Windows.Forms.Padding(0);
			this.pnlAdvSort1.MinimumSize = new System.Drawing.Size(27, 0);
			this.pnlAdvSort1.Name = "pnlAdvSort1";
			this.tblAdvSorting.SetRowSpan(this.pnlAdvSort1, 3);
			this.pnlAdvSort1.Size = new System.Drawing.Size(31, 72);
			this.pnlAdvSort1.TabIndex = 5;
			// 
			// rbAfter2nd
			// 
			this.rbAfter2nd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbAfter2nd.BackColor = System.Drawing.Color.Transparent;
			this.rbAfter2nd.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbAfter2nd, null);
			this.locExtender.SetLocalizationComment(this.rbAfter2nd, null);
			this.locExtender.SetLocalizationPriority(this.rbAfter2nd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbAfter2nd, "SortOptionsDropDown.rbAfter2nd");
			this.rbAfter2nd.Location = new System.Drawing.Point(1, 52);
			this.rbAfter2nd.Margin = new System.Windows.Forms.Padding(2);
			this.rbAfter2nd.Name = "rbAfter2nd";
			this.rbAfter2nd.Size = new System.Drawing.Size(29, 16);
			this.rbAfter2nd.TabIndex = 2;
			this.rbAfter2nd.UseVisualStyleBackColor = false;
			this.rbAfter2nd.Click += new System.EventHandler(this.HandleCheckedColumn1);
			// 
			// rbBefore2nd
			// 
			this.rbBefore2nd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbBefore2nd.BackColor = System.Drawing.Color.Transparent;
			this.rbBefore2nd.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbBefore2nd, null);
			this.locExtender.SetLocalizationComment(this.rbBefore2nd, null);
			this.locExtender.SetLocalizationPriority(this.rbBefore2nd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbBefore2nd, "SortOptionsDropDown.rbBefore2nd");
			this.rbBefore2nd.Location = new System.Drawing.Point(1, 4);
			this.rbBefore2nd.Margin = new System.Windows.Forms.Padding(2);
			this.rbBefore2nd.Name = "rbBefore2nd";
			this.rbBefore2nd.Size = new System.Drawing.Size(29, 16);
			this.rbBefore2nd.TabIndex = 0;
			this.rbBefore2nd.UseVisualStyleBackColor = false;
			this.rbBefore2nd.Click += new System.EventHandler(this.HandleCheckedColumn1);
			// 
			// rbItem2nd
			// 
			this.rbItem2nd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbItem2nd.BackColor = System.Drawing.Color.Transparent;
			this.rbItem2nd.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.rbItem2nd, null);
			this.locExtender.SetLocalizationComment(this.rbItem2nd, null);
			this.locExtender.SetLocalizationPriority(this.rbItem2nd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rbItem2nd, "SortOptionsDropDown.rbItem2nd");
			this.rbItem2nd.Location = new System.Drawing.Point(1, 28);
			this.rbItem2nd.Margin = new System.Windows.Forms.Padding(2);
			this.rbItem2nd.Name = "rbItem2nd";
			this.rbItem2nd.Size = new System.Drawing.Size(29, 16);
			this.rbItem2nd.TabIndex = 1;
			this.rbItem2nd.UseVisualStyleBackColor = false;
			this.rbItem2nd.Click += new System.EventHandler(this.HandleCheckedColumn1);
			// 
			// chkAfterRL
			// 
			this.chkAfterRL.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.chkAfterRL.BackColor = System.Drawing.Color.Transparent;
			this.chkAfterRL.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.chkAfterRL, null);
			this.locExtender.SetLocalizationComment(this.chkAfterRL, null);
			this.locExtender.SetLocalizationPriority(this.chkAfterRL, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.chkAfterRL, "SortOptionsDropDown.chkAfterRL");
			this.chkAfterRL.Location = new System.Drawing.Point(161, 73);
			this.chkAfterRL.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.chkAfterRL.Name = "chkAfterRL";
			this.chkAfterRL.Size = new System.Drawing.Size(29, 23);
			this.chkAfterRL.TabIndex = 9;
			this.chkAfterRL.UseVisualStyleBackColor = false;
			this.chkAfterRL.Click += new System.EventHandler(this.HandleRightToLeftCheckBoxChecked);
			// 
			// chkItemRL
			// 
			this.chkItemRL.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.chkItemRL.BackColor = System.Drawing.Color.Transparent;
			this.chkItemRL.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.chkItemRL, null);
			this.locExtender.SetLocalizationComment(this.chkItemRL, null);
			this.locExtender.SetLocalizationPriority(this.chkItemRL, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.chkItemRL, "SortOptionsDropDown.chkItemRL");
			this.chkItemRL.Location = new System.Drawing.Point(161, 49);
			this.chkItemRL.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.chkItemRL.Name = "chkItemRL";
			this.chkItemRL.Size = new System.Drawing.Size(29, 23);
			this.chkItemRL.TabIndex = 8;
			this.chkItemRL.UseVisualStyleBackColor = false;
			this.chkItemRL.Click += new System.EventHandler(this.HandleRightToLeftCheckBoxChecked);
			// 
			// chkBeforeRL
			// 
			this.chkBeforeRL.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.chkBeforeRL.BackColor = System.Drawing.Color.Transparent;
			this.chkBeforeRL.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.locExtender.SetLocalizableToolTip(this.chkBeforeRL, null);
			this.locExtender.SetLocalizationComment(this.chkBeforeRL, null);
			this.locExtender.SetLocalizationPriority(this.chkBeforeRL, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.chkBeforeRL, "SortOptionsDropDown.chkBeforeRL");
			this.chkBeforeRL.Location = new System.Drawing.Point(161, 25);
			this.chkBeforeRL.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.chkBeforeRL.Name = "chkBeforeRL";
			this.chkBeforeRL.Size = new System.Drawing.Size(29, 23);
			this.chkBeforeRL.TabIndex = 7;
			this.chkBeforeRL.UseVisualStyleBackColor = false;
			this.chkBeforeRL.Click += new System.EventHandler(this.HandleRightToLeftCheckBoxChecked);
			// 
			// lblBefore
			// 
			this.lblBefore.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblBefore.AutoEllipsis = true;
			this.lblBefore.AutoSize = true;
			this.lblBefore.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblBefore, null);
			this.locExtender.SetLocalizationComment(this.lblBefore, null);
			this.locExtender.SetLocalizingId(this.lblBefore, "SortOptionsDropDown.lblBefore");
			this.lblBefore.Location = new System.Drawing.Point(2, 28);
			this.lblBefore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblBefore.Name = "lblBefore";
			this.lblBefore.Size = new System.Drawing.Size(63, 15);
			this.lblBefore.TabIndex = 4;
			this.lblBefore.Text = "Preceding:";
			this.lblBefore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblThird
			// 
			this.lblThird.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblThird.AutoEllipsis = true;
			this.lblThird.AutoSize = true;
			this.lblThird.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblThird, null);
			this.locExtender.SetLocalizationComment(this.lblThird, null);
			this.locExtender.SetLocalizingId(this.lblThird, "SortOptionsDropDown.lblThird");
			this.lblThird.Location = new System.Drawing.Point(131, 9);
			this.lblThird.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblThird.MinimumSize = new System.Drawing.Size(27, 0);
			this.lblThird.Name = "lblThird";
			this.lblThird.Size = new System.Drawing.Size(27, 15);
			this.lblThird.TabIndex = 2;
			this.lblThird.Text = "3rd";
			this.lblThird.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblAfter
			// 
			this.lblAfter.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAfter.AutoEllipsis = true;
			this.lblAfter.AutoSize = true;
			this.lblAfter.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblAfter, null);
			this.locExtender.SetLocalizationComment(this.lblAfter, null);
			this.locExtender.SetLocalizingId(this.lblAfter, "SortOptionsDropDown.lblAfter");
			this.lblAfter.Location = new System.Drawing.Point(2, 76);
			this.lblAfter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblAfter.Name = "lblAfter";
			this.lblAfter.Size = new System.Drawing.Size(62, 15);
			this.lblAfter.TabIndex = 2;
			this.lblAfter.Text = "Following:";
			this.lblAfter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblItem
			// 
			this.lblItem.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblItem.AutoSize = true;
			this.lblItem.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblItem, null);
			this.locExtender.SetLocalizationComment(this.lblItem, null);
			this.locExtender.SetLocalizingId(this.lblItem, "SortOptionsDropDown.lblItem");
			this.lblItem.Location = new System.Drawing.Point(2, 52);
			this.lblItem.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblItem.Name = "lblItem";
			this.lblItem.Size = new System.Drawing.Size(34, 15);
			this.lblItem.TabIndex = 1;
			this.lblItem.Text = "Item:";
			this.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblFirst
			// 
			this.lblFirst.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblFirst.AutoEllipsis = true;
			this.lblFirst.AutoSize = true;
			this.lblFirst.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblFirst, null);
			this.locExtender.SetLocalizationComment(this.lblFirst, null);
			this.locExtender.SetLocalizingId(this.lblFirst, "SortOptionsDropDown.lblFirst");
			this.lblFirst.Location = new System.Drawing.Point(69, 9);
			this.lblFirst.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblFirst.MinimumSize = new System.Drawing.Size(27, 0);
			this.lblFirst.Name = "lblFirst";
			this.lblFirst.Size = new System.Drawing.Size(27, 15);
			this.lblFirst.TabIndex = 0;
			this.lblFirst.Text = "1st";
			this.lblFirst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblRL
			// 
			this.lblRL.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblRL.AutoEllipsis = true;
			this.lblRL.AutoSize = true;
			this.lblRL.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblRL, "Right-to-Left");
			this.locExtender.SetLocalizationComment(this.lblRL, null);
			this.locExtender.SetLocalizingId(this.lblRL, "SortOptionsDropDown.lblRL");
			this.lblRL.Location = new System.Drawing.Point(162, 9);
			this.lblRL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblRL.MinimumSize = new System.Drawing.Size(27, 0);
			this.lblRL.Name = "lblRL";
			this.lblRL.Size = new System.Drawing.Size(27, 15);
			this.lblRL.TabIndex = 3;
			this.lblRL.Text = "R/L";
			this.lblRL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSecond
			// 
			this.lblSecond.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblSecond.AutoEllipsis = true;
			this.lblSecond.AutoSize = true;
			this.lblSecond.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblSecond, null);
			this.locExtender.SetLocalizationComment(this.lblSecond, null);
			this.locExtender.SetLocalizingId(this.lblSecond, "SortOptionsDropDown.lblSecond");
			this.lblSecond.Location = new System.Drawing.Point(100, 9);
			this.lblSecond.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblSecond.MinimumSize = new System.Drawing.Size(27, 0);
			this.lblSecond.Name = "lblSecond";
			this.lblSecond.Size = new System.Drawing.Size(27, 15);
			this.lblSecond.TabIndex = 1;
			this.lblSecond.Text = "2nd";
			this.lblSecond.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tblLayoutOuter
			// 
			this.tblLayoutOuter.AutoSize = true;
			this.tblLayoutOuter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblLayoutOuter.ColumnCount = 1;
			this.tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutOuter.Controls.Add(this.pnlAdvOptions, 0, 3);
			this.tblLayoutOuter.Controls.Add(this.rbMannerArticulation, 0, 1);
			this.tblLayoutOuter.Controls.Add(this.rbUnicodeOrder, 0, 2);
			this.tblLayoutOuter.Controls.Add(this.rbPlaceArticulation, 0, 1);
			this.tblLayoutOuter.Controls.Add(this.flowPanelButtons, 0, 4);
			this.tblLayoutOuter.Location = new System.Drawing.Point(0, 0);
			this.tblLayoutOuter.Name = "tblLayoutOuter";
			this.tblLayoutOuter.RowCount = 5;
			this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutOuter.Size = new System.Drawing.Size(211, 203);
			this.tblLayoutOuter.TabIndex = 0;
			this.tblLayoutOuter.SizeChanged += new System.EventHandler(this.HandleOuterTableLayoutSizeChanged);
			// 
			// pnlAdvOptions
			// 
			this.pnlAdvOptions.AutoSize = true;
			this.pnlAdvOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlAdvOptions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlAdvOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlAdvOptions.ClipTextForChildControls = true;
			this.pnlAdvOptions.ControlReceivingFocusOnMnemonic = null;
			this.pnlAdvOptions.Controls.Add(this.tblAdvSorting);
			this.pnlAdvOptions.DoubleBuffered = true;
			this.pnlAdvOptions.DrawOnlyBottomBorder = false;
			this.pnlAdvOptions.DrawOnlyTopBorder = false;
			this.pnlAdvOptions.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlAdvOptions.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlAdvOptions, null);
			this.locExtender.SetLocalizationComment(this.pnlAdvOptions, null);
			this.locExtender.SetLocalizingId(this.pnlAdvOptions, "SortOptionsDropDown.pnlAdvOptions");
			this.pnlAdvOptions.Location = new System.Drawing.Point(8, 73);
			this.pnlAdvOptions.Margin = new System.Windows.Forms.Padding(8, 7, 8, 0);
			this.pnlAdvOptions.MnemonicGeneratesClick = false;
			this.pnlAdvOptions.Name = "pnlAdvOptions";
			this.pnlAdvOptions.PaintExplorerBarBackground = false;
			this.pnlAdvOptions.Size = new System.Drawing.Size(195, 100);
			this.pnlAdvOptions.TabIndex = 1;
			// 
			// flowPanelButtons
			// 
			this.flowPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flowPanelButtons.AutoSize = true;
			this.flowPanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowPanelButtons.Controls.Add(this.btnClose);
			this.flowPanelButtons.Controls.Add(this.btnHelp);
			this.flowPanelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowPanelButtons.Location = new System.Drawing.Point(0, 180);
			this.flowPanelButtons.Margin = new System.Windows.Forms.Padding(0, 7, 7, 7);
			this.flowPanelButtons.Name = "flowPanelButtons";
			this.flowPanelButtons.Size = new System.Drawing.Size(204, 16);
			this.flowPanelButtons.TabIndex = 1;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnClose.BackgroundImage = global::SIL.Pa.Properties.Resources.PopupDialogButtonClose;
			this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnClose.FlatAppearance.BorderSize = 0;
			this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.ImageHot = ((System.Drawing.Image)(resources.GetObject("btnClose.ImageHot")));
			this.btnClose.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnClose.ImageNormal")));
			this.locExtender.SetLocalizableToolTip(this.btnClose, "Close");
			this.locExtender.SetLocalizationComment(this.btnClose, null);
			this.locExtender.SetLocalizingId(this.btnClose, "SortOptionsDropDown.btnClose");
			this.btnClose.Location = new System.Drawing.Point(188, 0);
			this.btnClose.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(16, 16);
			this.btnClose.TabIndex = 4;
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.HandleCloseButtonClick);
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnHelp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHelp.BackgroundImage")));
			this.btnHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnHelp.FlatAppearance.BorderSize = 0;
			this.btnHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnHelp.ImageHot = ((System.Drawing.Image)(resources.GetObject("btnHelp.ImageHot")));
			this.btnHelp.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnHelp.ImageNormal")));
			this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnHelp, "Help");
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizingId(this.btnHelp, "SortOptionsDropDown.btnHelp");
			this.btnHelp.Location = new System.Drawing.Point(162, 0);
			this.btnHelp.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(16, 16);
			this.btnHelp.TabIndex = 5;
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.HandleHelpButtonClick);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SortOptionsDropDown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.tblLayoutOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SortOptionsDropDown.SortOptionsDropDown");
			this.Margin = new System.Windows.Forms.Padding(5);
			this.Name = "SortOptionsDropDown";
			this.Size = new System.Drawing.Size(259, 222);
			this.tblAdvSorting.ResumeLayout(false);
			this.tblAdvSorting.PerformLayout();
			this.pnlAdvSort2.ResumeLayout(false);
			this.pnlAdvSort0.ResumeLayout(false);
			this.pnlAdvSort1.ResumeLayout(false);
			this.tblLayoutOuter.ResumeLayout(false);
			this.tblLayoutOuter.PerformLayout();
			this.pnlAdvOptions.ResumeLayout(false);
			this.pnlAdvOptions.PerformLayout();
			this.flowPanelButtons.ResumeLayout(false);
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
