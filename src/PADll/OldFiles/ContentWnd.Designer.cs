using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for ContentWnd.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ContentWnd
	{
		private System.Windows.Forms.TabControl tabDocViews;
		private System.Windows.Forms.TabPage tpgTrans;
		private System.Windows.Forms.TabPage tpgColView;
		private System.Windows.Forms.TabPage tpgDocInfo;
		private System.Windows.Forms.TreeView tvContents;
		private System.ComponentModel.IContainer components;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentWnd));
			this.scTrans = new System.Windows.Forms.SplitContainer();
			this.tvContents = new System.Windows.Forms.TreeView();
			this.lblContents = new SIL.Pa.PaLabel();
			this.tabDocViews = new System.Windows.Forms.TabControl();
			this.tpgTrans = new System.Windows.Forms.TabPage();
			this.ltbTransWnd3 = new SIL.Pa.LabelledTextBox();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.ltbTransWnd2 = new SIL.Pa.LabelledTextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.ltbTransWnd1 = new SIL.Pa.LabelledTextBox();
			this.tpgColView = new System.Windows.Forms.TabPage();
			this.tpgDocInfo = new System.Windows.Forms.TabPage();
			this.pnlDocInfo = new System.Windows.Forms.Panel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.grpSpeaker = new System.Windows.Forms.GroupBox();
			this.rbMale = new System.Windows.Forms.RadioButton();
			this.rbFemale = new System.Windows.Forms.RadioButton();
			this.cboSpeaker = new System.Windows.Forms.ComboBox();
			this.pnlDialect = new System.Windows.Forms.Panel();
			this.cboDialect = new System.Windows.Forms.ComboBox();
			this.lblDialect = new SIL.Pa.PaLabel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblTranscriber = new SIL.Pa.PaLabel();
			this.txtNBRef = new System.Windows.Forms.TextBox();
			this.txtTranscriber = new System.Windows.Forms.TextBox();
			this.lblNBRef = new SIL.Pa.PaLabel();
			this.grpDates = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblLastUpdate = new SIL.Pa.PaLabel();
			this.lblLastUpdateVal = new SIL.Pa.PaLabel();
			this.lblOrigDate = new SIL.Pa.PaLabel();
			this.lblOrigDateVal = new SIL.Pa.PaLabel();
			this.grpAudioFile = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.lblLengthVal = new SIL.Pa.PaLabel();
			this.lblLength = new SIL.Pa.PaLabel();
			this.lblRecDateVal = new SIL.Pa.PaLabel();
			this.lblBitsVal = new SIL.Pa.PaLabel();
			this.lblBits = new SIL.Pa.PaLabel();
			this.lblRecDate = new SIL.Pa.PaLabel();
			this.lblOrigFmtVal = new SIL.Pa.PaLabel();
			this.lblOrigFmt = new SIL.Pa.PaLabel();
			this.lblRateVal = new SIL.Pa.PaLabel();
			this.lblRate = new SIL.Pa.PaLabel();
			this.lblFileNameVal = new SIL.Pa.PaLabel();
			this.lblFileName = new SIL.Pa.PaLabel();
			this.scDocInfo = new System.Windows.Forms.SplitContainer();
			this.ltbFreeform = new SIL.Pa.LabelledTextBox();
			this.ltbComments = new SIL.Pa.LabelledTextBox();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.scTrans.Panel1.SuspendLayout();
			this.scTrans.Panel2.SuspendLayout();
			this.scTrans.SuspendLayout();
			this.tabDocViews.SuspendLayout();
			this.tpgTrans.SuspendLayout();
			this.tpgDocInfo.SuspendLayout();
			this.pnlDocInfo.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.grpSpeaker.SuspendLayout();
			this.pnlDialect.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpDates.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.grpAudioFile.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.scDocInfo.Panel1.SuspendLayout();
			this.scDocInfo.Panel2.SuspendLayout();
			this.scDocInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// scTrans
			// 
			resources.ApplyResources(this.scTrans, "scTrans");
			this.scTrans.Name = "scTrans";
			// 
			// scTrans.Panel1
			// 
			this.scTrans.Panel1.Controls.Add(this.tvContents);
			this.scTrans.Panel1.Controls.Add(this.lblContents);
			resources.ApplyResources(this.scTrans.Panel1, "scTrans.Panel1");
			// 
			// scTrans.Panel2
			// 
			this.scTrans.Panel2.Controls.Add(this.tabDocViews);
			resources.ApplyResources(this.scTrans.Panel2, "scTrans.Panel2");
			// 
			// tvContents
			// 
			this.tvContents.AllowDrop = true;
			resources.ApplyResources(this.tvContents, "tvContents");
			this.tvContents.HideSelection = false;
			this.tvContents.LabelEdit = true;
			this.tvContents.Name = "tvContents";
			this.tvContents.ShowNodeToolTips = true;
			this.tvContents.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvContents_DragDrop);
			this.tvContents.DragOver += new System.Windows.Forms.DragEventHandler(this.tvContents_DragOver);
			this.tvContents.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvContents_AfterLabelEdit);
			this.tvContents.DragLeave += new System.EventHandler(this.tvContents_DragLeave);
			this.tvContents.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvContents_AfterSelect);
			this.tvContents.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvContents_BeforeSelect);
			this.tvContents.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvContents_KeyDown);
			this.tvContents.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvContents_ItemDrag);
			// 
			// lblContents
			// 
			this.lblContents.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblContents, "lblContents");
			this.lblContents.Name = "lblContents";
			// 
			// tabDocViews
			// 
			this.tabDocViews.Controls.Add(this.tpgTrans);
			this.tabDocViews.Controls.Add(this.tpgColView);
			this.tabDocViews.Controls.Add(this.tpgDocInfo);
			resources.ApplyResources(this.tabDocViews, "tabDocViews");
			this.tabDocViews.HotTrack = true;
			this.tabDocViews.Name = "tabDocViews";
			this.tabDocViews.SelectedIndex = 0;
			this.tabDocViews.ClientSizeChanged += new System.EventHandler(this.tabDocViews_ClientSizeChanged);
			// 
			// tpgTrans
			// 
			this.tpgTrans.Controls.Add(this.ltbTransWnd3);
			this.tpgTrans.Controls.Add(this.splitter2);
			this.tpgTrans.Controls.Add(this.ltbTransWnd2);
			this.tpgTrans.Controls.Add(this.splitter1);
			this.tpgTrans.Controls.Add(this.ltbTransWnd1);
			resources.ApplyResources(this.tpgTrans, "tpgTrans");
			this.tpgTrans.Name = "tpgTrans";
			this.tpgTrans.UseVisualStyleBackColor = true;
			// 
			// ltbTransWnd3
			// 
			this.ltbTransWnd3.AllowDropDown = true;
			this.ltbTransWnd3.BackColor = System.Drawing.SystemColors.Window;
			this.ltbTransWnd3.DBField = null;
			this.ltbTransWnd3.DocId = 0;
			resources.ApplyResources(this.ltbTransWnd3, "ltbTransWnd3");
			this.ltbTransWnd3.LabelText = "#";
			this.ltbTransWnd3.Name = "ltbTransWnd3";
			// 
			// splitter2
			// 
			resources.ApplyResources(this.splitter2, "splitter2");
			this.splitter2.Name = "splitter2";
			this.splitter2.TabStop = false;
			this.splitter2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitter_Paint);
			// 
			// ltbTransWnd2
			// 
			this.ltbTransWnd2.AllowDropDown = true;
			this.ltbTransWnd2.BackColor = System.Drawing.SystemColors.Window;
			this.ltbTransWnd2.DBField = null;
			this.ltbTransWnd2.DocId = 0;
			resources.ApplyResources(this.ltbTransWnd2, "ltbTransWnd2");
			this.ltbTransWnd2.LabelText = "#";
			this.ltbTransWnd2.Name = "ltbTransWnd2";
			// 
			// splitter1
			// 
			resources.ApplyResources(this.splitter1, "splitter1");
			this.splitter1.Name = "splitter1";
			this.splitter1.TabStop = false;
			this.splitter1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitter_Paint);
			// 
			// ltbTransWnd1
			// 
			this.ltbTransWnd1.AllowDropDown = true;
			this.ltbTransWnd1.BackColor = System.Drawing.SystemColors.Window;
			this.ltbTransWnd1.DBField = null;
			this.ltbTransWnd1.DocId = 0;
			resources.ApplyResources(this.ltbTransWnd1, "ltbTransWnd1");
			this.ltbTransWnd1.LabelText = "#";
			this.ltbTransWnd1.Name = "ltbTransWnd1";
			// 
			// tpgColView
			// 
			resources.ApplyResources(this.tpgColView, "tpgColView");
			this.tpgColView.Name = "tpgColView";
			this.tpgColView.UseVisualStyleBackColor = true;
			// 
			// tpgDocInfo
			// 
			this.tpgDocInfo.Controls.Add(this.pnlDocInfo);
			resources.ApplyResources(this.tpgDocInfo, "tpgDocInfo");
			this.tpgDocInfo.Name = "tpgDocInfo";
			this.tpgDocInfo.UseVisualStyleBackColor = true;
			// 
			// pnlDocInfo
			// 
			this.pnlDocInfo.Controls.Add(this.tableLayoutPanel4);
			this.pnlDocInfo.Controls.Add(this.tableLayoutPanel1);
			this.pnlDocInfo.Controls.Add(this.grpDates);
			this.pnlDocInfo.Controls.Add(this.grpAudioFile);
			this.pnlDocInfo.Controls.Add(this.scDocInfo);
			resources.ApplyResources(this.pnlDocInfo, "pnlDocInfo");
			this.pnlDocInfo.Name = "pnlDocInfo";
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.grpSpeaker, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.pnlDialect, 1, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			// 
			// grpSpeaker
			// 
			resources.ApplyResources(this.grpSpeaker, "grpSpeaker");
			this.grpSpeaker.Controls.Add(this.rbMale);
			this.grpSpeaker.Controls.Add(this.rbFemale);
			this.grpSpeaker.Controls.Add(this.cboSpeaker);
			this.grpSpeaker.Name = "grpSpeaker";
			this.grpSpeaker.TabStop = false;
			// 
			// rbMale
			// 
			resources.ApplyResources(this.rbMale, "rbMale");
			this.rbMale.Name = "rbMale";
			this.rbMale.TabStop = true;
			// 
			// rbFemale
			// 
			resources.ApplyResources(this.rbFemale, "rbFemale");
			this.rbFemale.Name = "rbFemale";
			this.rbFemale.TabStop = true;
			// 
			// cboSpeaker
			// 
			resources.ApplyResources(this.cboSpeaker, "cboSpeaker");
			this.cboSpeaker.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cboSpeaker.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cboSpeaker.Name = "cboSpeaker";
			// 
			// pnlDialect
			// 
			resources.ApplyResources(this.pnlDialect, "pnlDialect");
			this.pnlDialect.Controls.Add(this.cboDialect);
			this.pnlDialect.Controls.Add(this.lblDialect);
			this.pnlDialect.Name = "pnlDialect";
			// 
			// cboDialect
			// 
			resources.ApplyResources(this.cboDialect, "cboDialect");
			this.cboDialect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cboDialect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cboDialect.Name = "cboDialect";
			// 
			// lblDialect
			// 
			this.lblDialect.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblDialect, "lblDialect");
			this.lblDialect.Name = "lblDialect";
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblTranscriber, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtNBRef, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtTranscriber, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblNBRef, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// lblTranscriber
			// 
			this.lblTranscriber.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblTranscriber, "lblTranscriber");
			this.lblTranscriber.Name = "lblTranscriber";
			// 
			// txtNBRef
			// 
			resources.ApplyResources(this.txtNBRef, "txtNBRef");
			this.txtNBRef.Name = "txtNBRef";
			// 
			// txtTranscriber
			// 
			resources.ApplyResources(this.txtTranscriber, "txtTranscriber");
			this.txtTranscriber.Name = "txtTranscriber";
			// 
			// lblNBRef
			// 
			this.lblNBRef.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblNBRef, "lblNBRef");
			this.lblNBRef.Name = "lblNBRef";
			// 
			// grpDates
			// 
			resources.ApplyResources(this.grpDates, "grpDates");
			this.grpDates.Controls.Add(this.tableLayoutPanel2);
			this.grpDates.Name = "grpDates";
			this.grpDates.TabStop = false;
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.lblLastUpdate, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblLastUpdateVal, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblOrigDate, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblOrigDateVal, 0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// lblLastUpdate
			// 
			this.lblLastUpdate.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblLastUpdate, "lblLastUpdate");
			this.lblLastUpdate.Name = "lblLastUpdate";
			// 
			// lblLastUpdateVal
			// 
			this.lblLastUpdateVal.AlwaysShowToolTip = true;
			resources.ApplyResources(this.lblLastUpdateVal, "lblLastUpdateVal");
			this.lblLastUpdateVal.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblLastUpdateVal.Name = "lblLastUpdateVal";
			// 
			// lblOrigDate
			// 
			this.lblOrigDate.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblOrigDate, "lblOrigDate");
			this.lblOrigDate.Name = "lblOrigDate";
			// 
			// lblOrigDateVal
			// 
			this.lblOrigDateVal.AlwaysShowToolTip = true;
			resources.ApplyResources(this.lblOrigDateVal, "lblOrigDateVal");
			this.lblOrigDateVal.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblOrigDateVal.Name = "lblOrigDateVal";
			// 
			// grpAudioFile
			// 
			resources.ApplyResources(this.grpAudioFile, "grpAudioFile");
			this.grpAudioFile.Controls.Add(this.tableLayoutPanel5);
			this.grpAudioFile.Name = "grpAudioFile";
			this.grpAudioFile.TabStop = false;
			// 
			// tableLayoutPanel5
			// 
			resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.lblLengthVal, 3, 2);
			this.tableLayoutPanel5.Controls.Add(this.lblLength, 2, 2);
			this.tableLayoutPanel5.Controls.Add(this.lblRecDateVal, 1, 2);
			this.tableLayoutPanel5.Controls.Add(this.lblBitsVal, 3, 1);
			this.tableLayoutPanel5.Controls.Add(this.lblBits, 2, 1);
			this.tableLayoutPanel5.Controls.Add(this.lblRecDate, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.lblOrigFmtVal, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.lblOrigFmt, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.lblRateVal, 3, 0);
			this.tableLayoutPanel5.Controls.Add(this.lblRate, 2, 0);
			this.tableLayoutPanel5.Controls.Add(this.lblFileNameVal, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.lblFileName, 0, 0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			// 
			// lblLengthVal
			// 
			this.lblLengthVal.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblLengthVal, "lblLengthVal");
			this.lblLengthVal.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblLengthVal.Name = "lblLengthVal";
			// 
			// lblLength
			// 
			this.lblLength.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblLength, "lblLength");
			this.lblLength.Name = "lblLength";
			// 
			// lblRecDateVal
			// 
			this.lblRecDateVal.AlwaysShowToolTip = true;
			this.lblRecDateVal.AutoEllipsis = true;
			resources.ApplyResources(this.lblRecDateVal, "lblRecDateVal");
			this.lblRecDateVal.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblRecDateVal.Name = "lblRecDateVal";
			// 
			// lblBitsVal
			// 
			this.lblBitsVal.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblBitsVal, "lblBitsVal");
			this.lblBitsVal.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblBitsVal.Name = "lblBitsVal";
			// 
			// lblBits
			// 
			this.lblBits.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblBits, "lblBits");
			this.lblBits.Name = "lblBits";
			// 
			// lblRecDate
			// 
			this.lblRecDate.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblRecDate, "lblRecDate");
			this.lblRecDate.Name = "lblRecDate";
			// 
			// lblOrigFmtVal
			// 
			this.lblOrigFmtVal.AlwaysShowToolTip = false;
			this.lblOrigFmtVal.AutoEllipsis = true;
			resources.ApplyResources(this.lblOrigFmtVal, "lblOrigFmtVal");
			this.lblOrigFmtVal.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblOrigFmtVal.Name = "lblOrigFmtVal";
			// 
			// lblOrigFmt
			// 
			this.lblOrigFmt.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblOrigFmt, "lblOrigFmt");
			this.lblOrigFmt.Name = "lblOrigFmt";
			// 
			// lblRateVal
			// 
			this.lblRateVal.AlwaysShowToolTip = false;
			this.lblRateVal.AutoEllipsis = true;
			resources.ApplyResources(this.lblRateVal, "lblRateVal");
			this.lblRateVal.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblRateVal.Name = "lblRateVal";
			// 
			// lblRate
			// 
			this.lblRate.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblRate, "lblRate");
			this.lblRate.Name = "lblRate";
			// 
			// lblFileNameVal
			// 
			this.lblFileNameVal.AlwaysShowToolTip = true;
			this.lblFileNameVal.AutoEllipsis = true;
			resources.ApplyResources(this.lblFileNameVal, "lblFileNameVal");
			this.lblFileNameVal.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblFileNameVal.Name = "lblFileNameVal";
			// 
			// lblFileName
			// 
			this.lblFileName.AlwaysShowToolTip = false;
			resources.ApplyResources(this.lblFileName, "lblFileName");
			this.lblFileName.Name = "lblFileName";
			// 
			// scDocInfo
			// 
			resources.ApplyResources(this.scDocInfo, "scDocInfo");
			this.scDocInfo.Name = "scDocInfo";
			// 
			// scDocInfo.Panel1
			// 
			this.scDocInfo.Panel1.Controls.Add(this.ltbFreeform);
			// 
			// scDocInfo.Panel2
			// 
			this.scDocInfo.Panel2.Controls.Add(this.ltbComments);
			// 
			// ltbFreeform
			// 
			this.ltbFreeform.AllowDropDown = false;
			this.ltbFreeform.BackColor = System.Drawing.SystemColors.Window;
			this.ltbFreeform.DBField = null;
			this.ltbFreeform.DocId = 0;
			resources.ApplyResources(this.ltbFreeform, "ltbFreeform");
			this.ltbFreeform.LabelText = "#";
			this.ltbFreeform.Name = "ltbFreeform";
			// 
			// ltbComments
			// 
			this.ltbComments.AllowDropDown = false;
			this.ltbComments.BackColor = System.Drawing.SystemColors.Window;
			this.ltbComments.DBField = null;
			this.ltbComments.DocId = 0;
			resources.ApplyResources(this.ltbComments, "ltbComments");
			this.ltbComments.LabelText = "#";
			this.ltbComments.Name = "ltbComments";
			// 
			// ContentWnd
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.scTrans);
			this.DoubleBuffered = true;
			this.Name = "ContentWnd";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.scTrans.Panel1.ResumeLayout(false);
			this.scTrans.Panel1.PerformLayout();
			this.scTrans.Panel2.ResumeLayout(false);
			this.scTrans.ResumeLayout(false);
			this.tabDocViews.ResumeLayout(false);
			this.tpgTrans.ResumeLayout(false);
			this.tpgDocInfo.ResumeLayout(false);
			this.pnlDocInfo.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.grpSpeaker.ResumeLayout(false);
			this.pnlDialect.ResumeLayout(false);
			this.pnlDialect.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.grpDates.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.grpAudioFile.ResumeLayout(false);
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.scDocInfo.Panel1.ResumeLayout(false);
			this.scDocInfo.Panel2.ResumeLayout(false);
			this.scDocInfo.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private SplitContainer scTrans;
		private Splitter splitter2;
		private LabelledTextBox ltbTransWnd1;
		private Splitter splitter1;
		private ToolTip m_toolTip;
		private LabelledTextBox ltbTransWnd2;
		private LabelledTextBox ltbTransWnd3;
		private PaLabel lblContents;
		private Panel pnlDocInfo;
		private TableLayoutPanel tableLayoutPanel4;
		private GroupBox grpSpeaker;
		private RadioButton rbMale;
		private RadioButton rbFemale;
		private ComboBox cboSpeaker;
		private Panel pnlDialect;
		private ComboBox cboDialect;
		private PaLabel lblDialect;
		private TableLayoutPanel tableLayoutPanel1;
		private PaLabel lblTranscriber;
		private TextBox txtNBRef;
		private TextBox txtTranscriber;
		private PaLabel lblNBRef;
		private GroupBox grpDates;
		private TableLayoutPanel tableLayoutPanel2;
		private PaLabel lblLastUpdate;
		private PaLabel lblLastUpdateVal;
		private PaLabel lblOrigDate;
		private PaLabel lblOrigDateVal;
		private GroupBox grpAudioFile;
		private TableLayoutPanel tableLayoutPanel5;
		private PaLabel lblLengthVal;
		private PaLabel lblLength;
		private PaLabel lblRecDateVal;
		private PaLabel lblBitsVal;
		private PaLabel lblBits;
		private PaLabel lblRecDate;
		private PaLabel lblOrigFmtVal;
		private PaLabel lblOrigFmt;
		private PaLabel lblRateVal;
		private PaLabel lblRate;
		private PaLabel lblFileNameVal;
		private PaLabel lblFileName;
		private SplitContainer scDocInfo;
		private LabelledTextBox ltbFreeform;
		private LabelledTextBox ltbComments;
	}
}
