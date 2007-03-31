using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Text;
using SIL.FieldWorks.Common.Controls;
using SIL.Pa.Database;
using XCore;

namespace SIL.Pa
{
	/// <summary>
	/// The Keyboard Transcription window class.
	/// In VB6, this behaves kind of like a dialog box, in that all
	/// other forms are disabled while it's onscreen. However, it
	/// differs in that it isn't displayed using ShowDialog (Show vbModal)
	/// and as such is able to access the menus and toolbar.
	/// </summary>
	/// Update: created an alternate constructor to be called when an
	/// existing document is being opened.
	/// BUG: due to the way FwGrid works, hidden columns are still accessible
	/// by the keyboard, and sometimes, their column headers, which are still
	/// visible, may partially or completely obscure another column header at
	/// the same location.
	/// BUG: (not reproducible yet as exact cause is unknown) the dropdown arrow
	/// sometimes becomes disabled (in a clicked/dropped down state?) and we can
	/// only restore it by closing this window and calling it again (new).
	public class TextEd : System.Windows.Forms.Form, IxCoreColleague
	{
		private int oldFFHeight;
		private int oldPanelHeight;
		private TextBox txtTrans;
		private System.Windows.Forms.Label label1;
		private InterlinearBox m_interlinear;
		private ArrayList m_docWords;
		private int m_DocID;
		private bool m_ViewOnly;
		private FwGrid m_WordList;
		/// <summary>
		/// Determines how spacebar affects movement in the grid.
		/// true for rows, false for cols.
		/// </summary>
		private bool m_SpcMovesRow;
		private int m_FolderID;
		private PhoneInfoStruct[] m_PhoneInfo;
		private PaDataTable tblWrdLst;
		private PaDataTable tblAWI;
		private PaDataTable tblChrIdx;
		private PaDataTable tblIPASet;
		private PaDataTable tblEtic;
		private PaDataTable tblGloss;
		private PaDataTable tblOrtho;
		private PaDataTable tblEmic;
		private PaDataTable tblTone;
		private PaDataTable tblPOS;
		private FFWordsInfoStruct FFWrdInfo;
		private string m_BaseEtic;
		private string m_CVPattern;
		private string m_MOAWrdKey;
		private string m_POAWrdKey;
		private string m_UDWrdKey;
		#region Variables added by Designer
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rbEtic;
		private System.Windows.Forms.RadioButton rbEmic;
		private System.Windows.Forms.RadioButton rbOrtho;
		private System.Windows.Forms.RadioButton rbWordList;
		private System.Windows.Forms.RadioButton rbInterlinear;
		private System.Windows.Forms.TextBox txtWaveFile;
		private System.Windows.Forms.RadioButton rbAutoSeg;
		private System.Windows.Forms.RadioButton rbEqualSeg;
		private System.Windows.Forms.Button btnAutoAlign;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private SIL.Pa.LeftLabelledBox llbNoteRef;
		private SIL.Pa.LeftLabelledBox llbTranscriber;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private SIL.Pa.LabelledTextBox ltbFreeform;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem mnuShowHide;
		private System.Windows.Forms.MenuItem mnuLabels;
		private System.Windows.Forms.MenuItem mnuGridLine;
		private System.Windows.Forms.MenuItem mnuSep0;
		private System.Windows.Forms.MenuItem mnuSpc;
		private System.Windows.Forms.MenuItem mnuSep1;
		private System.Windows.Forms.MenuItem mnuInsRow;
		private System.Windows.Forms.MenuItem mnuDelRow;
		private System.Windows.Forms.MenuItem mnuAssocAudio;
		private System.Windows.Forms.MenuItem mnuSpcCol;
		private System.Windows.Forms.MenuItem mnuSpcRow;
		private System.Windows.Forms.MenuItem mnuLabelsNone;
		private System.Windows.Forms.MenuItem mnuLabelsSFM;
		private System.Windows.Forms.MenuItem mnuLabelsFull;
		private System.Windows.Forms.MenuItem mnuGridLineHorz;
		private System.Windows.Forms.MenuItem mnuGridLineVert;
		private System.Windows.Forms.MenuItem mnuGridLineBoth;
		private System.Windows.Forms.ContextMenu contextMenu;
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor calls as DocID == 0
		/// follows VB6 version, where a new doc has id 0
		/// </summary>
		public TextEd() : this(0)
		{
		}

		/// <summary>
		/// Use this version of the constructor when opening an existing
		/// document.
		/// </summary>
		/// <param name="doc">ID number of the document to be edited.</param>
		public TextEd(int doc) : this(doc, false)
		{
		}

		/// <summary>
		/// This constructor does the actual set-up.
		/// </summary>
		/// <param name="doc">ID number of the document to be viewed or edited.</param>
		/// <param name="ViewOnly">
		/// true means we just want the interlinear view.\n
		/// false means we want to be able to edit it as well.
		/// </param>
		public TextEd(int doc, bool ViewOnly)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_DocID = doc;
			m_docWords = new ArrayList();
			oldFFHeight = ltbFreeform.Height;
			oldPanelHeight = panel1.Height;
			SetupUpperHalf();

			/// add if condition so it won't do it when creating a new document.
			if (m_DocID != 0)
				GetWords();

			PaApp.MsgMediator.AddColleague(this);

			m_ViewOnly = ViewOnly;
			if (!ViewOnly)
			{
				/// create and set up the word list view.
				SetupWordList();
				if (m_DocID == 0)
					rbEtic.Checked = true;
				else
					rbWordList.Checked = true;
				m_SpcMovesRow = true;
			}
			else
			{
				/// we won't create the word list view since it isn't
				/// needed here.
				rbInterlinear.Checked = true;
				int newHeight = panel1.Bottom;
				panel1.Location = groupBox1.Location;
				panel1.Height = newHeight;
				groupBox1.Visible = false;
				groupBox2.Visible = false;
			}
		}
		#endregion

		#region Overridden methods
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

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			if (!IsHandleCreated)
				return;

			ltbFreeform.Height = (panel1.Height * oldFFHeight) / oldPanelHeight;
			if (ltbFreeform.Height < splitter1.MinSize) ltbFreeform.Height = splitter1.MinSize;
			if (ltbFreeform.Height > (panel1.Height - splitter1.MinExtra))
				ltbFreeform.Height = panel1.Height - splitter1.MinExtra - splitter1.Height;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.Enter) && (ActiveControl != ltbFreeform))
				return;
			base.OnKeyDown (e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			foreach(DocWords dw in m_docWords)
			{
				// make each set of labels get the new font settings.
				dw.UpdateFonts();
			}

			if (e.ClipRectangle.IntersectsWith(panel1.ClientRectangle))
			{
				if (rbEtic.Checked) txtTrans.Font = PaApp.PhoneticFont;
				else if (rbEmic.Checked) txtTrans.Font = PaApp.PhonemicFont;
				else if (rbOrtho.Checked) txtTrans.Font = PaApp.OrthograpicFont;
				else if (rbInterlinear.Checked &&
					e.ClipRectangle.IntersectsWith(m_interlinear.ClientRectangle))
				{
					// inefficient way of updating the control so that
					// its labels reflect the new font settings.
					//	m_interlinear = null;
					//	GC.Collect();
					//	m_interlinear = new InterlinearBox();
					//	panel1.Controls.Add(m_interlinear);
					//	m_interlinear.Words = m_wordData;
					//	m_interlinear.FillCells();
					//	m_interlinear.Dock = DockStyle.Fill;
					//	m_interlinear.Show();
					//	m_interlinear.BringToFront();
					m_interlinear.Invalidate();
				}
			}
			base.OnPaint (e);
		}

		/// <summary>
		/// We chose to override this so that we can prevent spacebar
		/// from functioning in the grid when it shouldn't, but instead
		/// perform movement to the next row/col, as in VB.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData == Keys.Space) && m_WordList.InEditMode &&
				(m_WordList.CurrentColumn != m_WordList.Columns["gloss"]) &&
				(m_WordList.CurrentColumn != m_WordList.Columns["ref"]))
			{
				SendKeys.Send("{ESC} ");
				return true;
			}
			return base.ProcessCmdKey (ref msg, keyData);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			// basically we want the data to be saved as long as we don't click
			// the Cancel button btnCancel.
			if (this.DialogResult != DialogResult.Cancel)
			{
				DialogResult ShouldSave = DialogResult.Cancel;
				switch (ShouldSave)
				{
					case DialogResult.Yes:
						SaveDoc();
						break;
					case DialogResult.No:
						break;
					default:
						e.Cancel = true;
						break;
				}
				e.Cancel = false;
			}
			base.OnClosing (e);
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbEtic = new System.Windows.Forms.RadioButton();
			this.rbEmic = new System.Windows.Forms.RadioButton();
			this.rbOrtho = new System.Windows.Forms.RadioButton();
			this.rbWordList = new System.Windows.Forms.RadioButton();
			this.rbInterlinear = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnAutoAlign = new System.Windows.Forms.Button();
			this.rbAutoSeg = new System.Windows.Forms.RadioButton();
			this.txtWaveFile = new System.Windows.Forms.TextBox();
			this.rbEqualSeg = new System.Windows.Forms.RadioButton();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.ltbFreeform = new SIL.Pa.LabelledTextBox();
			this.llbNoteRef = new SIL.Pa.LeftLabelledBox();
			this.llbTranscriber = new SIL.Pa.LeftLabelledBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.mnuShowHide = new System.Windows.Forms.MenuItem();
			this.mnuLabels = new System.Windows.Forms.MenuItem();
			this.mnuLabelsNone = new System.Windows.Forms.MenuItem();
			this.mnuLabelsSFM = new System.Windows.Forms.MenuItem();
			this.mnuLabelsFull = new System.Windows.Forms.MenuItem();
			this.mnuGridLine = new System.Windows.Forms.MenuItem();
			this.mnuGridLineHorz = new System.Windows.Forms.MenuItem();
			this.mnuGridLineVert = new System.Windows.Forms.MenuItem();
			this.mnuGridLineBoth = new System.Windows.Forms.MenuItem();
			this.mnuSep0 = new System.Windows.Forms.MenuItem();
			this.mnuSpc = new System.Windows.Forms.MenuItem();
			this.mnuSpcCol = new System.Windows.Forms.MenuItem();
			this.mnuSpcRow = new System.Windows.Forms.MenuItem();
			this.mnuSep1 = new System.Windows.Forms.MenuItem();
			this.mnuInsRow = new System.Windows.Forms.MenuItem();
			this.mnuDelRow = new System.Windows.Forms.MenuItem();
			this.mnuAssocAudio = new System.Windows.Forms.MenuItem();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbEtic);
			this.groupBox1.Controls.Add(this.rbEmic);
			this.groupBox1.Controls.Add(this.rbOrtho);
			this.groupBox1.Controls.Add(this.rbWordList);
			this.groupBox1.Controls.Add(this.rbInterlinear);
			this.groupBox1.Location = new System.Drawing.Point(8, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 104);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Transcription Type";
			// 
			// rbEtic
			// 
			this.rbEtic.Location = new System.Drawing.Point(16, 24);
			this.rbEtic.Name = "rbEtic";
			this.rbEtic.Size = new System.Drawing.Size(72, 24);
			this.rbEtic.TabIndex = 0;
			this.rbEtic.Tag = "0";
			this.rbEtic.Text = "Phone&tic";
			this.rbEtic.CheckedChanged += new System.EventHandler(this.TransTypeChanged);
			// 
			// rbEmic
			// 
			this.rbEmic.Location = new System.Drawing.Point(16, 48);
			this.rbEmic.Name = "rbEmic";
			this.rbEmic.Size = new System.Drawing.Size(80, 24);
			this.rbEmic.TabIndex = 0;
			this.rbEmic.Tag = "1";
			this.rbEmic.Text = "Phone&mic";
			this.rbEmic.CheckedChanged += new System.EventHandler(this.TransTypeChanged);
			// 
			// rbOrtho
			// 
			this.rbOrtho.Location = new System.Drawing.Point(16, 72);
			this.rbOrtho.Name = "rbOrtho";
			this.rbOrtho.Size = new System.Drawing.Size(88, 24);
			this.rbOrtho.TabIndex = 0;
			this.rbOrtho.Tag = "2";
			this.rbOrtho.Text = "Ort&hographic";
			this.rbOrtho.CheckedChanged += new System.EventHandler(this.TransTypeChanged);
			// 
			// rbWordList
			// 
			this.rbWordList.Location = new System.Drawing.Point(112, 24);
			this.rbWordList.Name = "rbWordList";
			this.rbWordList.Size = new System.Drawing.Size(80, 24);
			this.rbWordList.TabIndex = 0;
			this.rbWordList.Tag = "3";
			this.rbWordList.Text = "Word &List";
			this.rbWordList.CheckedChanged += new System.EventHandler(this.TransTypeChanged);
			// 
			// rbInterlinear
			// 
			this.rbInterlinear.Location = new System.Drawing.Point(112, 56);
			this.rbInterlinear.Name = "rbInterlinear";
			this.rbInterlinear.Size = new System.Drawing.Size(80, 24);
			this.rbInterlinear.TabIndex = 0;
			this.rbInterlinear.Tag = "4";
			this.rbInterlinear.Text = "&Interlinear View";
			this.rbInterlinear.CheckedChanged += new System.EventHandler(this.TransTypeChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnAutoAlign);
			this.groupBox2.Controls.Add(this.rbAutoSeg);
			this.groupBox2.Controls.Add(this.txtWaveFile);
			this.groupBox2.Controls.Add(this.rbEqualSeg);
			this.groupBox2.Controls.Add(this.btnBrowse);
			this.groupBox2.Location = new System.Drawing.Point(216, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(352, 104);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Asso&ciated Audio File";
			// 
			// btnAutoAlign
			// 
			this.btnAutoAlign.Enabled = false;
			this.btnAutoAlign.Location = new System.Drawing.Point(152, 64);
			this.btnAutoAlign.Name = "btnAutoAlign";
			this.btnAutoAlign.Size = new System.Drawing.Size(72, 24);
			this.btnAutoAlign.TabIndex = 2;
			this.btnAutoAlign.Text = "&Auto Align";
			this.btnAutoAlign.Click += new EventHandler(btnAutoAlign_Click);
			// 
			// rbAutoSeg
			// 
			this.rbAutoSeg.Enabled = false;
			this.rbAutoSeg.Location = new System.Drawing.Point(8, 48);
			this.rbAutoSeg.Name = "rbAutoSeg";
			this.rbAutoSeg.Size = new System.Drawing.Size(160, 24);
			this.rbAutoSeg.TabIndex = 1;
			this.rbAutoSeg.Text = "A&utomatic Segmentation";
			// 
			// txtWaveFile
			// 
			this.txtWaveFile.Location = new System.Drawing.Point(8, 24);
			this.txtWaveFile.Name = "txtWaveFile";
			this.txtWaveFile.Size = new System.Drawing.Size(200, 20);
			this.txtWaveFile.TabIndex = 0;
			this.txtWaveFile.Text = "(none)";
			this.txtWaveFile.TextChanged += new EventHandler(txtWaveFile_TextChanged);
			// 
			// rbEqualSeg
			// 
			this.rbEqualSeg.Enabled = false;
			this.rbEqualSeg.Location = new System.Drawing.Point(8, 72);
			this.rbEqualSeg.Name = "rbEqualSeg";
			this.rbEqualSeg.Size = new System.Drawing.Size(136, 24);
			this.rbEqualSeg.TabIndex = 1;
			this.rbEqualSeg.Text = "Equa&l Segmentation";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(232, 64);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(72, 24);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Brows&e...";
			this.btnBrowse.Click += new EventHandler(btnBrowse_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.ltbFreeform);
			this.panel1.Location = new System.Drawing.Point(8, 112);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(560, 144);
			this.panel1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Silver;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(560, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Tran&scription";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 80);
			this.splitter1.MinExtra = 40;
			this.splitter1.MinSize = 40;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(560, 8);
			this.splitter1.TabIndex = 0;
			this.splitter1.TabStop = false;
			// 
			// ltbFreeform
			// 
			this.ltbFreeform.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ltbFreeform.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ltbFreeform.LabelText = "Freeform T&ranslation";
			this.ltbFreeform.Location = new System.Drawing.Point(0, 88);
			this.ltbFreeform.Name = "ltbFreeform";
			this.ltbFreeform.Size = new System.Drawing.Size(560, 56);
			this.ltbFreeform.TabIndex = 1;
			// 
			// llbNoteRef
			// 
			this.llbNoteRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.llbNoteRef.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.llbNoteRef.LabelText = "&Notebook Ref.:";
			this.llbNoteRef.Location = new System.Drawing.Point(8, 264);
			this.llbNoteRef.Name = "llbNoteRef";
			this.llbNoteRef.Size = new System.Drawing.Size(208, 24);
			this.llbNoteRef.TabIndex = 3;
			// 
			// llbTranscriber
			// 
			this.llbTranscriber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.llbTranscriber.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.llbTranscriber.LabelText = "Transcri&ber";
			this.llbTranscriber.Location = new System.Drawing.Point(232, 264);
			this.llbTranscriber.Name = "llbTranscriber";
			this.llbTranscriber.Size = new System.Drawing.Size(192, 24);
			this.llbTranscriber.TabIndex = 3;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(440, 264);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(48, 24);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(496, 264);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(48, 24);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuShowHide,
																						this.mnuLabels,
																						this.mnuGridLine,
																						this.mnuSep0,
																						this.mnuSpc,
																						this.mnuSep1,
																						this.mnuInsRow,
																						this.mnuDelRow,
																						this.mnuAssocAudio});
			// 
			// mnuShowHide
			// 
			this.mnuShowHide.Index = 0;
			this.mnuShowHide.Text = "Show...";
			// 
			// mnuLabels
			// 
			this.mnuLabels.Index = 1;
			this.mnuLabels.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuLabelsNone,
																					  this.mnuLabelsSFM,
																					  this.mnuLabelsFull});
			this.mnuLabels.Text = "&Margin Labels";
			// 
			// mnuLabelsNone
			// 
			this.mnuLabelsNone.Index = 0;
			this.mnuLabelsNone.Text = "&None";
			// 
			// mnuLabelsSFM
			// 
			this.mnuLabelsSFM.Index = 1;
			this.mnuLabelsSFM.Text = "&Standard Format Markers";
			// 
			// mnuLabelsFull
			// 
			this.mnuLabelsFull.Index = 2;
			this.mnuLabelsFull.Text = "&Full Field Name";
			// 
			// mnuGridLine
			// 
			this.mnuGridLine.Index = 2;
			this.mnuGridLine.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuGridLineHorz,
																						this.mnuGridLineVert,
																						this.mnuGridLineBoth});
			this.mnuGridLine.Text = "Show Grid &Lines";
			// 
			// mnuGridLineHorz
			// 
			this.mnuGridLineHorz.Index = 0;
			this.mnuGridLineHorz.Text = "&Horizontal";
			// 
			// mnuGridLineVert
			// 
			this.mnuGridLineVert.Index = 1;
			this.mnuGridLineVert.Text = "&Vertical";
			// 
			// mnuGridLineBoth
			// 
			this.mnuGridLineBoth.Index = 2;
			this.mnuGridLineBoth.Text = "&Both";
			// 
			// mnuSep0
			// 
			this.mnuSep0.Index = 3;
			this.mnuSep0.Text = "-";
			// 
			// mnuSpc
			// 
			this.mnuSpc.Index = 4;
			this.mnuSpc.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.mnuSpcCol,
																				   this.mnuSpcRow});
			this.mnuSpc.Text = "Spacebar &Behavior in Grid";
			// 
			// mnuSpcCol
			// 
			this.mnuSpcCol.Index = 0;
			this.mnuSpcCol.Text = "Moves Cursor to Next &Column";
			// 
			// mnuSpcRow
			// 
			this.mnuSpcRow.Index = 1;
			this.mnuSpcRow.Text = "Moves Cursor to Next &Row";
			// 
			// mnuSep1
			// 
			this.mnuSep1.Index = 5;
			this.mnuSep1.Text = "-";
			// 
			// mnuInsRow
			// 
			this.mnuInsRow.Index = 6;
			this.mnuInsRow.Text = "&Insert Row";
			// 
			// mnuDelRow
			// 
			this.mnuDelRow.Index = 7;
			this.mnuDelRow.Text = "&Delete Row";
			// 
			// mnuAssocAudio
			// 
			this.mnuAssocAudio.Index = 8;
			this.mnuAssocAudio.Text = "Associate A&udio File";
			// 
			// TextEd
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(576, 293);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.llbNoteRef);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.llbTranscriber);
			this.Controls.Add(this.btnCancel);
			this.MinimumSize = new System.Drawing.Size(560, 288);
			this.Name = "TextEd";
			this.ShowInTaskbar = false;
			this.Text = "#";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Menu command handlers
		protected bool OnShowHide(object args)
		{
			//ShowColFlag fields;
			//if (rbInterlinear.Checked)
			//{
			//    fields =
			//        (m_interlinear.ShowTone ? ShowColFlag.ShowTone : 0) |
			//        (m_interlinear.ShowEmic ? ShowColFlag.ShowEmic : 0) |
			//        (m_interlinear.ShowOrtho ? ShowColFlag.ShowOrtho : 0) |
			//        (m_interlinear.ShowGloss ? ShowColFlag.ShowGloss : 0) |
			//        (m_interlinear.ShowPOS ? ShowColFlag.ShowPOS : 0) |
			//        (m_interlinear.ShowRef ? ShowColFlag.ShowRef : 0);
			//    ShowColsDlg col = new ShowColsDlg();
			//    col.Result = fields;
			//    col.SettableCols = ShowColFlag.ShowTone | ShowColFlag.ShowEmic |
			//        ShowColFlag.ShowOrtho | ShowColFlag.ShowGloss |
			//        ShowColFlag.ShowPOS | ShowColFlag.ShowRef;
			//    col.ShowDialog(this);
			//    if (col.DialogResult == DialogResult.OK)
			//    {
			//        m_interlinear.ShowTone = ((col.Result & ShowColFlag.ShowTone) > 0);
			//        m_interlinear.ShowEmic = ((col.Result & ShowColFlag.ShowEmic) > 0);
			//        m_interlinear.ShowOrtho = ((col.Result & ShowColFlag.ShowOrtho) > 0);
			//        m_interlinear.ShowGloss = ((col.Result & ShowColFlag.ShowGloss) > 0);
			//        m_interlinear.ShowPOS = ((col.Result & ShowColFlag.ShowPOS) > 0);
			//        m_interlinear.ShowRef = ((col.Result & ShowColFlag.ShowRef) > 0);
			//        m_interlinear.Invalidate();
			//    }
			//}
			//else if (rbWordList.Checked)
			//{
			//    fields =
			//        (m_WordList.Columns["tone"].Visible ? ShowColFlag.ShowTone : 0) |
			//        (m_WordList.Columns["emic"].Visible ? ShowColFlag.ShowEmic : 0) |
			//        (m_WordList.Columns["ortho"].Visible ? ShowColFlag.ShowOrtho : 0) |
			//        (m_WordList.Columns["gloss"].Visible ? ShowColFlag.ShowGloss : 0) |
			//        (m_WordList.Columns["pos"].Visible ? ShowColFlag.ShowPOS : 0) |
			//        (m_WordList.Columns["ref"].Visible ? ShowColFlag.ShowRef : 0);
			//    ShowColsDlg col = new ShowColsDlg();
			//    col.Result = fields;
			//    col.SettableCols = ShowColFlag.ShowTone | ShowColFlag.ShowEmic |
			//        ShowColFlag.ShowOrtho | ShowColFlag.ShowGloss |
			//        ShowColFlag.ShowPOS | ShowColFlag.ShowRef;
			//    col.ShowDialog(this);
			//    if (col.DialogResult == DialogResult.OK)
			//    {
			//        m_WordList.Columns["tone"].Visible = ((col.Result & ShowColFlag.ShowTone) > 0);
			//        m_WordList.Columns["emic"].Visible = ((col.Result & ShowColFlag.ShowEmic) > 0);
			//        m_WordList.Columns["ortho"].Visible = ((col.Result & ShowColFlag.ShowOrtho) > 0);
			//        m_WordList.Columns["gloss"].Visible = ((col.Result & ShowColFlag.ShowGloss) > 0);
			//        m_WordList.Columns["pos"].Visible = ((col.Result & ShowColFlag.ShowPOS) > 0);
			//        m_WordList.Columns["ref"].Visible = ((col.Result & ShowColFlag.ShowRef) > 0);
			//        panel1.Controls.Remove(m_WordList);
			//        panel1.Controls.Add(m_WordList);
			//        m_WordList.BringToFront();
			//    }
			//}
			return true;
		}

		protected bool OnLabelsNone(object args)
		{
			m_interlinear.ShowHeaders = 0;
			return true;
		}

		protected bool OnLabelsSFM(object args)
		{
			m_interlinear.ShowHeaders = 2;
			return true;
		}

		protected bool OnLabelsFull(object args)
		{
			m_interlinear.ShowHeaders = 1;
			return true;
		}

		protected bool OnGridLineHorz(object args)
		{
			return true;
		}

		protected bool OnGridLineVert(object args)
		{
			return true;
		}

		protected bool OnGridLineBoth(object args)
		{
			return true;
		}

		protected bool OnSpcCol(object args)
		{
			m_SpcMovesRow = false;
			return true;
		}

		protected bool OnSpcRow(object args)
		{
			m_SpcMovesRow = true;
			return true;
		}

		protected bool OnInsRow(object args)
		{
			return true;
		}

		protected bool OnDelRow(object args)
		{
			return true;
		}

		protected bool OnAssocAudio(object args)
		{
			MessageBox.Show("This didn't do anything in the original, " +
				"so it doesn't do anything here, either.");
			return true;
		}
		#endregion

		#region Update message handlers
		protected bool OnUpdateShowHide(object args)
		{
			mnuShowHide.Visible = rbInterlinear.Checked || rbWordList.Checked;
			if (rbInterlinear.Checked)
				mnuShowHide.Text = "Show &Fields...";
			else if (rbWordList.Checked)
				mnuShowHide.Text = "Show &Columns...";
			return true;
		}

		protected bool OnUpdateLabelsNone(object args)
		{
			mnuLabelsNone.Enabled = mnuLabels.Visible;
			return true;
		}

		protected bool OnUpdateLabelsSFM(object args)
		{
			mnuLabelsSFM.Enabled = mnuLabels.Visible;
			return true;
		}

		protected bool OnUpdateLabelsFull(object args)
		{
			mnuLabelsFull.Enabled = mnuLabels.Visible;
			return true;
		}

		protected bool OnUpdateGridLineHorz(object args)
		{
			mnuGridLineHorz.Enabled = mnuGridLine.Visible;
			return true;
		}

		protected bool OnUpdateGridLineVert(object args)
		{
			mnuGridLineVert.Enabled = mnuGridLine.Visible;
			return true;
		}

		protected bool OnUpdateGridLineBoth(object args)
		{
			mnuGridLineBoth.Enabled = mnuGridLine.Visible;
			return true;
		}

		protected bool OnUpdateSpcCol(object args)
		{
			mnuSpcCol.Enabled = mnuSpc.Visible;
			mnuSpcCol.Checked = !m_SpcMovesRow;
			return true;
		}

		protected bool OnUpdateSpcRow(object args)
		{
			mnuSpcRow.Enabled = mnuSpc.Visible;
			mnuSpcRow.Checked = m_SpcMovesRow;
			return true;
		}

		protected bool OnUpdateInsRow(object args)
		{
			mnuInsRow.Enabled = mnuInsRow.Visible = rbWordList.Checked;
			return true;
		}

		protected bool OnUpdateDelRow(object args)
		{
			mnuDelRow.Enabled = mnuDelRow.Visible = rbWordList.Checked;
			return true;
		}

		protected bool OnUpdateAssocAudio(object args)
		{
			return true;
		}
		#endregion

		/// <summary>
		/// Sets up the controls which will appear in the
		/// upper half of the panel.
		/// </summary>
		private void SetupUpperHalf()
		{
			/// Setup for txtTrans
			txtTrans = new TextBox();
			txtTrans.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			txtTrans.Location = new System.Drawing.Point(0, 0);
			txtTrans.Name = "txtTrans";
			txtTrans.Size = new System.Drawing.Size(560, 100);
			txtTrans.TabIndex = 0;
			txtTrans.Multiline = true;
			txtTrans.Visible = false;
			panel1.Controls.Add(txtTrans);
		}

		/// <summary>
		/// Gets the words of the current document.
		/// </summary>
		private void GetWords()
		{
			string sql = "SELECT DISTINCTROW DocID, AnnOffset, AllWordIndexID, Phonetic, " +
				"Phonemic, Ortho, Gloss, POS, Tone, WavOffset, WavLength, WordRef, " +
				"AllWordsIndex.PhonemicListID, AllWordsIndex.OrthoListID, " +
				"AllWordsIndex.GlossListID, AllWordsIndex.ToneListID, " +
				"AllWordsIndex.POSListID FROM WordList INNER JOIN " +
				"(PhonemicList RIGHT JOIN (ToneList RIGHT JOIN " +
				"(OrthoList RIGHT JOIN (GlossList RIGHT JOIN " +
				"(POSList RIGHT JOIN AllWordsIndex ON " +
				"POSList.POSListID = AllWordsIndex.POSListID) ON " +
				"GlossList.GlossListID = AllWordsIndex.GlossListID) ON " +
				"OrthoList.OrthoListID = AllWordsIndex.OrthoListID) ON " +
				"ToneList.ToneListID = AllWordsIndex.ToneListID) ON " +
				"PhonemicList.PhonemicListID = AllWordsIndex.PhonemicListID) ON " +
				"WordList.WordListID = AllWordsIndex.WordListID WHERE (DocID=" +
				m_DocID + ") ORDER BY AnnOffset;";

			PaDataTable table = new PaDataTable(sql);

			if (table.Rows.Count == 0)
				MessageBox.Show(this, "No words found for document.", Application.ProductName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
			{
				foreach (DataRow row in table.Rows)
				{
					DocWords dw = new DocWords();
					if (row["Phonetic"].ToString() != "")
						dw.Phonetic = row["Phonetic"].ToString();
					if (row["Tone"].ToString() != "")
						dw.Tone = row["Tone"].ToString();
					if (row["Phonemic"].ToString() != "")
						dw.Phonemic = row["Phonemic"].ToString();
					if (row["Ortho"].ToString() != "")
						dw.Ortho = row["Ortho"].ToString();
					dw.Gloss = row["Gloss"].ToString();
					dw.POS = row["POS"].ToString();
					dw.Ref = row["WordRef"].ToString();
					m_docWords.Add(dw);
				}
			}
		}

		private void SetupWordList()
		{
			int fontHeight = 0;
			
			m_WordList = new FwGrid();

			m_WordList.Dock = DockStyle.Fill;
			m_WordList.FullRowSelect = true;
			m_WordList.ColumnHeaderHeight = SystemInformation.MenuFont.Height + 8;
			m_WordList.ShowColumnHeadings = true;
			m_WordList.BorderStyle = BorderStyle.Fixed3D;
			m_WordList.GridLineColor = Color.FromArgb(m_WordList.BackColor.R - 15,
				m_WordList.BackColor.G - 15, m_WordList.BackColor.B - 15);

			int index = m_WordList.Columns.Add(new FwGridColumn("Phonetic", 150));
			m_WordList.Columns[index].Font = SystemInformation.MenuFont;
			m_WordList.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_WordList.Columns[index].Name = "etic";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;

			index = m_WordList.Columns.Add(new FwGridColumn("Tone", 150));
			m_WordList.Columns[index].Font = SystemInformation.MenuFont;
			m_WordList.Columns[index].ColumnFont = PaApp.ToneFont;
			m_WordList.Columns[index].Name = "tone";
			if (fontHeight < PaApp.ToneFont.Height)
				fontHeight = PaApp.ToneFont.Height;
			
			index = m_WordList.Columns.Add(new FwGridColumn("Phonemic", 150));
			m_WordList.Columns[index].Font = SystemInformation.MenuFont;
			m_WordList.Columns[index].ColumnFont = PaApp.PhonemicFont;
			m_WordList.Columns[index].Name = "emic";
			if (fontHeight < PaApp.PhonemicFont.Height)
				fontHeight = PaApp.PhonemicFont.Height;

			index = m_WordList.Columns.Add(new FwGridColumn("Orthographic", 150));
			m_WordList.Columns[index].Font = SystemInformation.MenuFont;
			m_WordList.Columns[index].ColumnFont = PaApp.OrthograpicFont;
			m_WordList.Columns[index].Name = "ortho";
			if (fontHeight < PaApp.OrthograpicFont.Height)
				fontHeight = PaApp.OrthograpicFont.Height;
			
			index = m_WordList.Columns.Add(new FwGridColumn("Gloss", 150));
			m_WordList.Columns[index].Font = SystemInformation.MenuFont;
			m_WordList.Columns[index].ColumnFont = PaApp.GlossFont;
			m_WordList.Columns[index].Name = "gloss";
			if (fontHeight < PaApp.GlossFont.Height)
				fontHeight = PaApp.GlossFont.Height;
			
			index = m_WordList.Columns.Add(new FwGridColumn("POS (Part of Speech)", 150));
			// Always allow editing the part of speech column.
			m_WordList.Columns[index].AllowEdit = true;
			m_WordList.Columns[index].Font = SystemInformation.MenuFont;
			m_WordList.Columns[index].ColumnFont = PaApp.PartOfSpeechFont;
			m_WordList.Columns[index].Name = "pos";
			if (fontHeight < PaApp.PartOfSpeechFont.Height)
				fontHeight = PaApp.PartOfSpeechFont.Height;
			
			index = m_WordList.Columns.Add(new FwGridColumn("Reference", 150));
			m_WordList.Columns[index].Font = SystemInformation.MenuFont;
			m_WordList.Columns[index].ColumnFont = PaApp.ReferenceFont;
			m_WordList.Columns[index].Name = "ref";
			if (fontHeight < PaApp.ReferenceFont.Height)
				fontHeight = PaApp.ReferenceFont.Height;
			
			// Set the row height according to the column with the tallest font.
			m_WordList.RowHeight = fontHeight + 3;
			m_WordList.Visible = false;
			m_WordList.ContextMenu = contextMenu;
			m_WordList.KeyDown += new KeyEventHandler(GridKeyDown);
			m_WordList.AfterRowChange += new SIL.FieldWorks.Common.Controls.FwGrid.RowColChangeHandler(GridAfterRowColChange);
			m_WordList.AfterColumnChange += new SIL.FieldWorks.Common.Controls.FwGrid.RowColChangeHandler(GridAfterRowColChange);
			FillGridPOSCombo();
			panel1.Controls.Add(m_WordList);
			m_WordList.BringToFront();
		}


		#region Properties
		public int DocID
		{
			set { m_DocID = value; }
		}
		public int FolderID
		{
			set { m_FolderID = value; }
		}
		#endregion

		#region IxCoreColleague Members

		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] {this});
		}

		#endregion

		/// <summary>
		/// Format m_wordData to have placeholders, or spaces (for interlinear, is it
		/// really needed?) or just null strings for empty fields. Assume that we
		/// always convert to and from placeholders.
		/// </summary>
		/// <param name="how">
		/// Can take the following values:
		/// 0 for null, >0 for spaces, &lt;0 for placeholders.
		/// </param>
		private void FormatWords(int how)
		{
			foreach (object obj in m_docWords)
			{
				DocWords dw = obj as DocWords;
				if (how > 0)
				{
					if (dw.Phonetic == "Å") dw.Phonetic = " ";
					if (dw.Tone == "Å") dw.Tone = " ";
					if (dw.Phonemic == "Å") dw.Phonemic = " ";
					if (dw.Ortho == ".") dw.Ortho = " ";
					if (dw.Gloss == "") dw.Gloss = " ";
					if (dw.POS == "") dw.POS = " ";
					if (dw.Ref == "") dw.Ref = " ";
				}
				else if (how < 0)
				{
					if (dw.Phonetic.Trim() == "") dw.Phonetic = "Å";
					if (dw.Tone.Trim() == "") dw.Tone = "Å";
					if (dw.Phonemic.Trim() == "") dw.Phonemic = "Å";
					if (dw.Ortho.Trim() == "") dw.Ortho = ".";
					if (dw.Gloss.Trim() == "") dw.Gloss = "";
					if (dw.POS.Trim() == "") dw.POS = "";
					if (dw.Ref.Trim() == "") dw.Ref = "";
				}
				else
				{
					if (dw.Phonetic == "Å") dw.Phonetic = "";
					if (dw.Tone == "Å") dw.Tone = "";
					if (dw.Phonemic == "Å") dw.Phonemic = "";
					if (dw.Ortho == ".") dw.Ortho = "";
				}
			}
		}

		private void FillGridPOSCombo()
		{
			PaDataTable table = new PaDataTable("SELECT DISTINCTROW POS FROM " +
				"POSList ORDER BY POS;");
			if (m_WordList.Columns["pos"].DropDownWindow != null)
				m_WordList.Columns["pos"].DropDownWindow.Dispose();
			m_WordList.Columns["pos"].DropDownWindow = new FwGridColumnList();
			((FwGridColumnList)m_WordList.Columns["pos"].DropDownWindow).Items.Add("");
			foreach (DataRow row in table.Rows)
			{
				((FwGridColumnList)m_WordList.Columns["pos"].DropDownWindow).Items.Add(row["POS"]);
			}
			m_WordList.Columns["pos"].DropDownWindow.AfterDropDownClosed +=
				new SIL.FieldWorks.Common.Controls.DropDownContainer.AfterDropDownClosedHandler(AfterDropDownClosed);
		}

		private DialogResult ShouldDocBeSaved()
		{
			// find out which view we're currently looking at and toggle it so that the
			// CheckedChanged event handler will trigger and save the data to m_wordData
			RadioButton rbChecked;
			if (rbEtic.Checked)
				rbChecked = rbEtic;
			else if (rbEmic.Checked)
				rbChecked = rbEmic;
			else if (rbOrtho.Checked)
				rbChecked = rbOrtho;
			else if (rbWordList.Checked)
				rbChecked = rbWordList;
			else // obviously the only case remaining
				rbChecked = rbInterlinear;
			rbChecked.Checked = false;
			rbChecked.Checked = true;
			// PackWordsArray is not needed subsequently as empty entries would already
			// have been removed during the forced save of the data to m_wordData.
			if (m_DocID == 0) // new KB doc
			{
				if (m_docWords.Count > 0)
					return MessageBox.Show(this, "Would you like to save changes?",
						Application.ProductName, MessageBoxButtons.YesNoCancel,
						MessageBoxIcon.Question);
				else
				{
					MessageBox.Show(this," New Document is empty so it will not be saved. ",
						Application.ProductName);
					return DialogResult.No;
				}
			}
			else if (m_DocID > 0)
			{
				if (m_docWords.Count == 0) // no kbd data
				{
					MessageBox.Show("* EXISTING KBD DOC: has no data. Re-open and enter data or delete file. *",
						Application.ProductName);
					return DialogResult.No;
				}
				else if (m_docWords.Count > 0)
				{
					if (NonWordsChanged() || WordsChanged())
					{
						return MessageBox.Show(this, "Would you like to save changes?", Application.ProductName,
							MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					}
					else
					{
						// original code tests if !NonWordsChanged() && !WordsChanged(), but
						// this is the ONLY other case since the earlier if was changed (in vb6)
						// from AND to OR.
						return DialogResult.No;
					}
				}
			}
			// probably will never get here, but if all else fails. besides, the compiler
			// complains if "not all code paths return a value" (error CS0161)
			return DialogResult.No;
		}

		private bool NonWordsChanged()
		{
			string sTemp;
			PaDataTable table = new PaDataTable(
				"SELECT DISTINCTROW Document.FolderID AS DocsFolderID, Document.DocID, DocTitle, " +
				"IPAFile, Comments, SAFileName, Wave, KBWave, LastUpdate, Dialect, " +
				"FullPhonemic, FullOrtho, Transcriber, Reference, OriginalDate, " +
				"FreeForm, SamplesPerSecond, BitsPerSample, RecordTimeStamp, " +
				"RecordFileFormat, RecordingLength, Channels, Document.SpeakerID, " +
				"SpeakerName, Gender FROM (Category INNER JOIN Folder ON Category.CatID = " +
				"Folder.CatID) INNER JOIN ((Speaker RIGHT JOIN Document ON " +
				"Speaker.SpeakerID = Document.SpeakerID) LEFT JOIN DocHeader ON " +
				"Document.DocID = DocHeader.DocID) ON Folder.FolderID = Document.FolderID " +
				"ORDER BY CatTitle, FolderTitle, DocTitle;");
			table.PrimaryKey = new DataColumn[] {table.Columns["DocID"]};
			if (!table.Rows.Contains(m_DocID))
				return true;
			DataRow row = table.Rows.Find(m_DocID);
			if (ltbFreeform.Text != (string) row["FreeForm"])
				return true;
			else if (txtTrans.Text != (string) row["Transcriber"])
				return true;
			else if (llbNoteRef.Text != (string) row["Reference"])
				return true;
			else
			{
				sTemp = txtWaveFile.Text.Trim();
				if (sTemp == "(none)")
					sTemp = "";
				if ((sTemp != (string) row["SAFileName"]) ||
					((sTemp.Length == 0) && (((string)row["SAFileName"]).Length > 0)))
					return true;
			}
			return false;
		}

		private bool WordsChanged()
		{
			int i;
			string sEtic;
			PaDataTable table = new PaDataTable(
				"SELECT DISTINCTROW DocID, AnnOffset, AllWordIndexID, Phonetic, " +
				"Phonemic, Ortho, Gloss, POS, Tone, WavOffset, WavLength, WordRef, " +
				"AllWordsIndex.PhonemicListID, AllWordsIndex.OrthoListID, " +
				"AllWordsIndex.GlossListID, AllWordsIndex.ToneListID, " +
				"AllWordsIndex.POSListID FROM WordList INNER JOIN " +
				"(PhonemicList RIGHT JOIN (ToneList RIGHT JOIN " +
				"(OrthoList RIGHT JOIN (GlossList RIGHT JOIN " +
				"(POSList RIGHT JOIN AllWordsIndex ON " +
				"POSList.POSListID = AllWordsIndex.POSListID) ON " +
				"GlossList.GlossListID = AllWordsIndex.GlossListID) ON " +
				"OrthoList.OrthoListID = AllWordsIndex.OrthoListID) ON " +
				"ToneList.ToneListID = AllWordsIndex.ToneListID) ON " +
				"PhonemicList.PhonemicListID = AllWordsIndex.PhonemicListID) ON " +
				"WordList.WordListID = AllWordsIndex.WordListID " +
				((m_DocID > 0)? "WHERE (DocID = " + m_DocID + ") " : "") +
				"ORDER BY AnnOffset;");
			// if the number of words is different, they have definitely been changed.
			if (table.Rows.Count != m_docWords.Count)
				return true;
			// if not, we have to see if any word has been changed.
			i = 0;
			foreach (DataRow row in table.Rows)
			{
				DocWords word = m_docWords[i] as DocWords;
				sEtic = (word.Phonetic.Length == 0 ? "Å" : word.Phonetic);
				if (sEtic != (string) row["Phonetic"])
					return true;
				else if (word.Tone != (string) row["TONE"])
					return true;
				else if (word.Phonemic != (string) row["Phonemic"])
					return true;
				else if (word.Ortho != (string) row["ORTHO"])
					return true;
				else if (word.Gloss != (string) row["GLOSS"])
					return true;
				else if (word.POS != (string) row["POS"])
					return true;
				else if (word.Ref != (string) row["WordRef"])
					return true;
				i++;
			}
			// otherwise, nothing has changed.
			return false;
		}

		/// <summary>
		/// Performs the work of saving the currently edited document to the database.
		/// Presently only supports new keyboard documents, but modifications may be
		/// added to allow updating of existing documents. Locations where such changes
		/// may be inserted may be marked as the author becomes aware of them. (JonL)
		/// </summary>
		private void SaveDoc()
		{
			PaDataTable table = new PaDataTable(
				"SELECT DISTINCTROW Document.FolderID AS DocsFolderID, Document.DocID, DocTitle, " +
				"IPAFile, Comments, SAFileName, Wave, KBWave, LastUpdate, Dialect, " +
				"FullPhonemic, FullOrtho, Transcriber, Reference, OriginalDate, " +
				"FreeForm, SamplesPerSecond, BitsPerSample, RecordTimeStamp, " +
				"RecordFileFormat, RecordingLength, Channels, Document.SpeakerID, " +
				"SpeakerName, Gender FROM (Category INNER JOIN Folder ON Category.CatID = " +
				"Folder.CatID) INNER JOIN ((Speaker RIGHT JOIN Document ON " +
				"Speaker.SpeakerID = Document.SpeakerID) LEFT JOIN DocHeader ON " +
				"Document.DocID = DocHeader.DocID) ON Folder.FolderID = Document.FolderID " +
				"ORDER BY CatTitle, FolderTitle, DocTitle;");
			table.PrimaryKey = new DataColumn[] {table.Columns["DocID"]};
			/// if an existing document exists, the document's data should be trashed
			/// as in VB6 where TrashExistingDocData was called
			/// --TODO: Add Trashing code or a function call here.
			DataRow row;
			if (m_DocID == 0)
			{
				row = table.NewRow();
				row["OriginalDate"] = DateTime.Now;
				row["FolderID"] = m_FolderID;
				row["Wave"] = (txtWaveFile.Text.Length > 0);
				row["DocTitle"] = "";
				row["Dialect"] = "";
				row["SAFileName"] = txtWaveFile.Text;
			}
			else
			{
				row = table.Rows.Find(m_DocID);
				if (row == null)
					return;
			}
			string sEtic = "";
			foreach (object obj in m_docWords)
			{
				DocWords dw = obj as DocWords;
				sEtic += dw.Phonetic + " ";
			}
			sEtic = sEtic.Trim();
			row["IPAFile"] = sEtic;
			/// since we're still using the symbol font, we'll have to convert the string
			/// to its ANSI encoding, then create the hex string accordingly.
			string sHex = "";
			byte[] EticBytes = Encoding.Convert(Encoding.Unicode,
				Encoding.Default, Encoding.Unicode.GetBytes(sEtic));
			foreach (byte byt in EticBytes)
			{
				sHex += byt.ToString("X2") + " ";
			}
			sHex = sHex.Trim();
			row["HexIPAFile"] = sHex;
			row["LastUpdate"] = DateTime.Now;
			if (m_DocID == 0)
				table.Rows.Add(row);
			table.AcceptChanges();
			table.Commit();
			m_DocID = (int) row["DocID"];
			if (m_DocID != 0)
			{
				if (ParseKBIPAStr(sEtic))
				{
					CheckForGlossForNewKBDoc();
					SaveWordsAndIPAChars();
				}
			}
		}

		private bool ParseKBIPAStr(string sIPA)
		{
			byte [] Bytes;
			bool bEndOfSeg;
			int i, j;
			string sSeg;
			byte FrcdSegBrkEnd;
			PhoneInfoStruct tmpInfo;

			/// since we're still working with the symbol font based
			/// database, which is ANSI-encoded, we shall have to convert back to
			/// ANSI from Unicode which .NET automatically converts all strings
			/// to when reading any string data.
			if (((int)sIPA[0]) == 2)
				sSeg = "";
			else
				sSeg = sIPA[0].ToString();
			Bytes = Encoding.Unicode.GetBytes(sIPA);
			Bytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, Bytes);
			foreach (byte B in Bytes) { Debug.WriteLine(B.ToString("X2")); }
			PaDataTable table = new PaDataTable("SELECT * FROM IPACharSet");
			table.PrimaryKey = new DataColumn[] {table.Columns["SILIPACharNum"]};

			int numBytes = sIPA.Length;
			ArrayList tmpPhoneInfo = new ArrayList();
			FrcdSegBrkEnd = 1;
			i = j = 0;
			while (i <= Bytes.Length)
			{
				if ((Bytes[i] == 131) || (Bytes[i] == 233)) // top/bottom tie bar
				{
					sSeg += (char)Bytes[i] + (char)Bytes[i+i];
					i += 2;
					if (i >= Bytes.Length)
						break;
				}

				/// If byte is a space, or we're at the beginning/end of a forced segment,
				/// then we're at the end of a segment that needs to be saved in an array
				/// element. Otherwise, search for the byte in IPA char table. If the byte
				/// isn't in the table (should never happen) then assume it's non-base.
				/// Else IsBaseChar flag tells us whether to start a new segment.
				if ((Bytes[i] == 32) || (Bytes[i] == FrcdSegBrkEnd) || (Bytes[i] == 2))
					bEndOfSeg = true;
				else if (Bytes[i] == 197) // Empty Etic
					bEndOfSeg = true;
				else
				{
					DataRow row = table.Rows.Find(Bytes[i]);
					if (row == null)
						bEndOfSeg = false;
					else
						bEndOfSeg = (bool) row["IsBaseChar"];
				}

				/// If there is a segment to save and the end of segment flag is set,
				/// then the contents of sSeg need to be saved to an element of structures
				/// that stores segment info. Once sSeg is saved in an array element,
				/// allocate a spot for a new segment. If the current byte is a space,
				/// we know that the next segment begins a word, so set the bStartofWord
				/// flag accordingly.
				if ((sSeg.Length > 0) && bEndOfSeg)
				{
					tmpInfo = new PhoneInfoStruct();
					tmpInfo.Char = sSeg;
					tmpInfo.ByteOff = (short) (i + 1 - sSeg.Length);
					tmpInfo.StartOfWord = (Bytes[i + 1] == 32);
					tmpPhoneInfo.Add(tmpInfo);
					j++;
					sSeg = "";
					bEndOfSeg = false;
				}

				/// if current byte is a forced break phone then accumulate bytes
				/// that belong to the forced segment. if the forced segment was terminated
				/// with another forced segment phone, then convert it to the forced
				/// break end phone so we know that the next iteration of the loop
				/// knows we just ended a force segment and are not at the beginning of
				/// one. if current byte is not a forced break phone then, if it's
				/// greater than a space, append it to the bytes that have accumulated for
				/// the current segment.
				if (Bytes[i] == 2)
				{
					i++;
					do
					{
						sSeg += (char)Bytes[i];
						i++;
					} while ((Bytes[i] != 32) && (Bytes[i] != 0) && (Bytes[i] != 2));
					if (Bytes[i] == 2)
						Bytes[i] = FrcdSegBrkEnd;
				}
				else
				{
					if ((Bytes[i] > 32) || (Bytes[i] == 197))
						sSeg += (char)Bytes[i];
					i++;
				}
			}

			tmpInfo = new PhoneInfoStruct();
			tmpInfo.Char = sSeg;
			tmpInfo.ByteOff = (short) (i - sSeg.Length - 1);
			tmpPhoneInfo.Add(tmpInfo);
			m_PhoneInfo = (PhoneInfoStruct[]) tmpPhoneInfo.ToArray(typeof(PhoneInfoStruct));
			return true;
		}

		private void CheckForGlossForNewKBDoc()
		{
			int i, j;
			if ((!rbWordList.Checked) || (m_WordList.Rows.Count == 0))
				return;
			j = 0;
			for (i = 0; i < m_PhoneInfo.Length; i++)
			{
				if ((i == 0) || (m_PhoneInfo[i].StartOfWord))
				{
					m_PhoneInfo[i].Gloss = m_WordList.Rows[j].Cells[1].Text;
					j++;
				}
			}
		}

		private bool SaveWordsAndIPAChars()
		{
			int [] AllConMask = new int [3], AllVowMask = new int [3],
				BaseConMask = new int [3], BaseVowMask = new int [3];
			int i, j;
			/// Byte offset in wave file of a word's first phone.
			int WrdWavOff = m_PhoneInfo[0].Offset;
			/// Sum of phone lengths (in bytes) of phones in a word.
			int WrdWavLen = m_PhoneInfo[0].WrdWavLen;
			/// WordListID of new WordList record. (returned from SavWord)
			int WrdLstID;
			/// AllWordIndexID of new word's AllWordsIndex record. (returned from SavWord)
			int NewAWIID;
			/// index in m_PhoneInfo of elements whose StartOfWord flag is true.
			short SONWIndex = 0;
			/// number of characters that make up a word.
			short NumChars = 0;
			short TotalChars = (short) m_PhoneInfo.Length;
			/// Byte offset in IPA string of a word's first phone.
			short WrdOff = 0;
			/// index in m_PhoneInfo of a word's first phone.
			short IPISIdx = 0;
			short Ret;
			/// Start Of New Word flag
			bool bSONW;
			string sWord = "", sGloss = "", sPOS = "", sRef = "", sEmic, sTone,
				sOrtho, sFullEmic = "", sFullOrtho = "", sFreeform;
			PaDataTable table = new PaDataTable("SELECT * FROM PhoneticClass;");
			table.PrimaryKey = new DataColumn[] {table.Columns["AllConClass"]};
			DataRow row = table.Rows.Find(true);
			if (row != null)
			{
				for (i = 0; i <= 3; i++)
				{
					AllConMask[i] = (int) row["Mask" + i];
				}
			}
			table.PrimaryKey = new DataColumn[] {table.Columns["AllVowClass"]};
			row = table.Rows.Find(true);
			if (row != null)
			{
				for (i = 0; i <= 3; i++)
				{
					AllVowMask[i] = (int) row["Mask" + i];
				}
			}
			table.PrimaryKey = new DataColumn[] {table.Columns["BaseConClass"]};
			row = table.Rows.Find(true);
			if (row != null)
			{
				for (i = 0; i <= 3; i++)
				{
					BaseConMask[i] = (int) row["Mask" + i];
				}
			}
			table.PrimaryKey = new DataColumn[] {table.Columns["BaseVowClass"]};
			row = table.Rows.Find(true);
			if (row != null)
			{
				for (i = 0; i <= 3; i++)
				{
					BaseVowMask[i] = (int) row["Mask" + i];
				}
			}

			tblWrdLst = new PaDataTable("SELECT * FROM WordList;");
			tblAWI = new PaDataTable("SELECT * FROM AllWordsIndex;");
			tblChrIdx = new PaDataTable("SELECT * FROM CharIndex;");
			tblIPASet = new PaDataTable("SELECT * FROM IPACharSet;");
			tblEtic = new PaDataTable("SELECT * FROM PhoneticList;");
			tblGloss = new PaDataTable("SELECT * FROM GlossList;");
			tblOrtho = new PaDataTable("SELECT * FROM OrthoList;");
			tblEmic = new PaDataTable("SELECT * FROM PhonemicList;");
			tblTone = new PaDataTable("SELECT * FROM ToneList;");
			tblPOS = new PaDataTable("SELECT * FROM POSList;");
			tblChrIdx.PrimaryKey = new DataColumn[] {tblChrIdx.Columns["WordListID"]};
			tblIPASet.PrimaryKey = new DataColumn[] {tblIPASet.Columns["SILIPACharNum"]};
			tblEtic.PrimaryKey = new DataColumn[] {tblEtic.Columns["HexCharStr"]};
			tblGloss.PrimaryKey = new DataColumn[] {tblGloss.Columns["HexGloss"]};
			tblOrtho.PrimaryKey = new DataColumn[] {tblOrtho.Columns["HexOrtho"]};
			tblEmic.PrimaryKey = new DataColumn[] {tblEmic.Columns["HexPhonemic"]};
			tblTone.PrimaryKey = new DataColumn[] {tblTone.Columns["HexTone"]};
			tblPOS.PrimaryKey = new DataColumn[] {tblPOS.Columns["POS"]};

			/// loop thru all phones read from the transcription & save words & phones.
			for (i = 0; i <= m_PhoneInfo.Length; i++)
			{
				/// Determine whether or not a word accumulated in sWord should be saved.
				/// If at end of transcr., need to save.
				/// If at beg. of new word, but not of transcr., need to save.
				/// If at beg. of word (i.e. StartOfWord flag = true) store .Gloss
				/// into sGloss var. and save it with the word.
				if (i == m_PhoneInfo.Length)
					bSONW = true;
				else if (i == 0)
				{
					sGloss = m_PhoneInfo[i].Gloss;
					sPOS = m_PhoneInfo[i].POS;
					sRef = m_PhoneInfo[i].Ref;
					bSONW = false;
				}
				else
					bSONW = m_PhoneInfo[i].StartOfWord;
				/// If bSONW (start-of-new-word) is set, then we must save the word before going on.
				if (bSONW)
				{
					/// ***** Maybe disregard this part since we don't have read wave files & assoc yet.
					/// Get emic, ortho & tone data of word (called GetETO())
					/// ***** for now, we'll just set them to be empty strings.
					/// ***** End Maybe
					sEmic = sTone = sOrtho = "";
					/// check if any words too long to store (max 80)
					/// called WordsNotTooLong(), optionally noting affected wavefile if error.
					/// return value of WordsNotTooLong() stored in Ret
					Ret = WordsNotTooLong(sWord, sEmic, sTone, sOrtho, sGloss);
					/// save word. if word is new, Ret==1, else Ret==2 unless error (Ret==0)
					/// calls SavWord(), return value saved to Ret.
					if (Ret != 0)
					{
						Ret = SavWord(m_DocID, sWord, sEmic, sTone, sOrtho, sGloss, sPOS,
							sRef, WrdWavOff, WrdWavLen, WrdOff, out WrdLstID, out NewAWIID);
						/// Now go back & fill in AllWordIndexID of all the segments included in
						/// the word just saved. for the sake of SavSegments().
						for (j = SONWIndex; j < i; j++)
							m_PhoneInfo[j].AllWordIndexID = NewAWIID;
						if (sEmic.Length > 0)
							sFullEmic += (" " + sEmic);
						if (sOrtho.Length > 0)
							sFullOrtho += (" " + sOrtho);
						/// if something wrong in SavWord, Ret==0, so exit.
						/// else if Ret==1, (new word) save each ipa char of that word.
						/// else Ret==2 (already exists), so no need to save IPA chars (already there)
						/// if problem occurs while adding IPA chars, also exit.
						/// calls SavIPAChars(), WriteSortKeysForWord()
						if (Ret == 0)
							return false;

						if (!SavIPAChars(WrdLstID, IPISIdx, NumChars, (Ret == 1)))
							return false;

						if (Ret == 1)
							WriteSortKeysForWord(WrdLstID);
					}
					/// if no more chars, break out of for loop
					if (i == (TotalChars + 1))
						break;
					/// we're starting a new word, so save index of 1st elem. of new word,
					/// clear sWord, clear counter for no. of phones in the word, clear counter
					/// for wave-length of word (omit?) increment word offset within IPA transcr.,
					/// save offset in wave file of this new word, and save the new word's gloss.
					IPISIdx = (short) i;
					sWord = "";
					NumChars = 0;
					WrdOff++;

					WrdWavOff = m_PhoneInfo[i].Offset;
					WrdWavLen = m_PhoneInfo[i].WrdWavLen;
					sGloss = m_PhoneInfo[i].Gloss;
					sPOS = m_PhoneInfo[i].POS;
					sRef = m_PhoneInfo[i].Ref;
				}
				/// Increment phone counter, add length (in wave data) of current phone & append
				/// current phone to word being accumulated in sWord.
				NumChars++;
				sWord += m_PhoneInfo[i].Char;
			}

			SavNonEticTranscriptions(m_DocID, sFullEmic, sFullOrtho);
			return true;
		}

		/// <summary>
		/// Checks if any of the fields for the current word are too long.
		/// Corresponds to the function of the same name in the VB version.
		/// </summary>
		/// <param name="sWord">The phonetic string.</param>
		/// <param name="sEmic">The phonemic string.</param>
		/// <param name="sTone">The tone string.</param>
		/// <param name="sOrtho">The orthographic string.</param>
		/// <param name="sGloss">The gloss string.</param>
		/// <returns>Nonzero if true, zero if false</returns>
		short WordsNotTooLong(string sWord, string sEmic, string sTone, string sOrtho, string sGloss)
		{
			string sOffender = "";

			if (sWord.Length > 80)
				sOffender = "phonetic";
			else if (sEmic.Length > 80)
				sOffender = "phonemic";
			else if (sTone.Length > 80)
				sOffender = "tone";
			else if (sOrtho.Length > 80)
				sOffender = "orthographic";
			else if (sGloss.Length > 80)
				sOffender = "gloss";
			if (sOffender != "")
			{
				MessageBox.Show(this, "A " + sOffender + " word was found that is too " +
					"long to store in the\ndatabase. Use Speech Analyzer (or " + Application.ProductName +
					" for\nkeyboard documents) to shorten any " + sOffender + " words whose\n" +
					"length is greater than 80 characters (including diacritics).\nYour " +
					"utterance will still be stored but will not be included\nin the word" +
					"lists or chart information.");
				return 0;
			}
			return -1;
		}

		/// <summary>
		/// Saves a word (found in the IPA string read from a .WAV file or keyboard)
		/// in the Word List table. It will also add a reference to it in the all words
		/// index table. If there is already a record for the word (i.e. sWord) in the
		/// table, no record is added, but that record's total count field is incremented
		/// to indicate that the database now contains one more occurence of that word.
		/// </summary>
		/// <param name="DocID"></param>
		/// <param name="sWord"></param>
		/// <param name="sEmic"></param>
		/// <param name="sTone"></param>
		/// <param name="sOrtho"></param>
		/// <param name="sGloss"></param>
		/// <param name="sPOS"></param>
		/// <param name="sRef"></param>
		/// <param name="WrdWavOff"></param>
		/// <param name="WrdWavLen"></param>
		/// <param name="WrdOff"></param>
		/// <param name="WrdLstID"></param>
		/// <param name="AWIID"></param>
		/// <returns>0 on error, 1 if new word, 2 if pre-existing word.</returns>
		public short SavWord(int DocID, string sWord, string sEmic, string sTone,
			string sOrtho, string sGloss, string sPOS, string sRef, int WrdWavOff,
			int WrdWavLen, short WrdOff, out int WrdLstID, out int AWIID)
		{
			string sHexWrd;
			WrdLstID = 0;
			AWIID = 0;

			FFWrdInfo = new FFWordsInfoStruct();

			FFWrdInfo.WordListID = 0;
			/// Convert the string of characters to a string of hex digits where each
			/// two-digit hex number represents one phone of the string.
			sHexWrd = string.Format("X2 ", Encoding.Default.GetChars(
				Encoding.Convert(Encoding.Unicode, Encoding.Default,
				Encoding.Unicode.GetBytes(sWord))));
			tblWrdLst.PrimaryKey = new DataColumn[] {tblWrdLst.Columns["HexPhonetic"]};
			DataRow WLRow = tblWrdLst.Rows.Find(sHexWrd);
			if (WLRow == null)
			{
				WLRow = tblWrdLst.NewRow();
				WLRow["Phonetic"] = sWord;
				WLRow["HexPhonetic"] = sHexWrd;
				WLRow["TotalCount"] = 1;
				tblWrdLst.Rows.Add(WLRow);
				FFWrdInfo.Word = sWord;
			}
			else
			{
				WLRow["TotalCount"] = ((int)WLRow["TotalCount"]) + 1;
			}
			tblWrdLst.Commit();

			DataRow AWIRow = tblAWI.NewRow();
			AWIRow["WordListID"] = WLRow["WordListID"];
			AWIRow["DocID"] = DocID;
			AWIRow["GlossListID"] = GetNonEticWordID(tblGloss, sGloss);
			AWIRow["PhonemicListID"] = GetNonEticWordID(tblEmic, sEmic);
			AWIRow["ToneListID"] = GetNonEticWordID(tblTone, sTone);
			AWIRow["OrthoListID"] = GetNonEticWordID(tblOrtho, sOrtho);
			AWIRow["POSListID"] = GetNonEticWordID(tblPOS, sPOS, false);
			if (sRef.Length > 0)
				AWIRow["WordRef"] = sRef;
			AWIRow["AnnOffset"] = WrdOff;
			AWIRow["AnnLength"] = sWord.Length;
			AWIRow["WavOffset"] = WrdWavOff;
			AWIRow["WavLength"] = WrdWavLen;
			tblAWI.Rows.Add(AWIRow);
			tblAWI.Commit();
			AWIID = (int) AWIRow["AllWordIndexID"];
			WrdLstID = (int) WLRow["WordListID"];
			return (short) (((int) WLRow["TotalCount"] == 1) ? 1 : 2);
		}

		/// <summary>
		/// Saves all the IPA characters associated with one word. The tables updated
		/// are CharIndex and phoneticlist. This function is only called when the current
		/// word (i.i. specified by WrdLstID) is new to the database.
		/// </summary>
		/// <param name="WrdLstID"></param>
		/// <param name="IPAISIdx"></param>
		/// <param name="NumChars"></param>
		/// <param name="FromNewWord"></param>
		/// <returns></returns>
		private bool SavIPAChars(int WrdLstID, short IPAISIdx, short NumChars, bool FromNewWord)
		{
			int i, j = 0, WordCount;
			short ByteOff = 0, CharLen, ChrType;
			string str1,
				WrdLoc = (NumChars == 1 ? "A" : "I"), // 1st phone either alone or initial.
				sChar, HexChar;
			m_MOAWrdKey = "";
			m_POAWrdKey = "";
			m_UDWrdKey = "";
			ArrayList rgsChars = new ArrayList(), rgsHexChars = new ArrayList(),
				rgsBaseHexChars = new ArrayList(), rgsMOAKeys = new ArrayList(),
				rgsPOAKeys = new ArrayList(), rgsUDKeys = new ArrayList(),
				rgiCharOffsets = new ArrayList(), rglFeatures = new ArrayList(),
				rglBinaryFeatures = new ArrayList();
			m_BaseEtic = "";
			m_CVPattern = "";
			for (i = IPAISIdx ; i < IPAISIdx + NumChars - 1 ; i++)
			{
				sChar = m_PhoneInfo[i].Char;
				CharLen = (short) sChar.Length;
				HexChar = StrToHex(sChar);
				DataRow EticRow = tblEtic.Rows.Find(HexChar);
				/// if phone is not found in the phonetic list table, add
				/// a new row to contain it.
				if (EticRow == null)
				{
					EticRow = tblEtic.NewRow();
					EticRow["ANSICharStr"] = sChar;
					EticRow["HexCharStr"] = HexChar;
					EticRow["UDSortOrder"] = 0;
					EticRow["TotalCount"] = 1;
					tblEtic.Rows.Add(EticRow);
					tblEtic.Commit();
					UpdatePhonesSortKeysAndFeatures(sChar, EticRow);
				}
				else
				{
					/// phone is already in the table, so just increment its total count.
					EticRow["TotalCount"] = ((int) EticRow["TotalCount"]) + 1;
					tblEtic.Commit();
				}
				ChrType = (short) EticRow["BaseCharType"];
				if (FromNewWord)
				{
					m_MOAWrdKey += EticRow["MOArticulation"];
					m_POAWrdKey += EticRow["POArticulation"];
					if (!EticRow.IsNull("UDSortOrder"))
					{
						m_UDWrdKey +=  ((int)EticRow["UDSortOrder"]).ToString("X3");
					}
					/// if the base phone is a consonant, add a C to the CV
					/// pattern. otherwise, if it's a vowel, add a V.
					if ((ChrType == 1) || (ChrType == 3) || (ChrType == 4))
						m_CVPattern += "C";
					else if (ChrType == 2)
						m_CVPattern += "V";
					/// Here we add a record to the CharIndex table that identifies the
					/// current phone as being part of the word specified by WrdLstID.
					/// Stored in this record is the current phones pointer into the phonetic
					/// list table, the position this phone occupies in the word (i.e. 1,
					/// 2, 3, etc) and this phone's location within the word (i.e initial,
					/// medial or final)
					DataRow ChrIdxRow = tblChrIdx.NewRow();
					ChrIdxRow["WordListID"] = WrdLstID;
					ChrIdxRow["Position"] = i - IPAISIdx + 1;
					ChrIdxRow["ByteOffset"] = ByteOff;
					ChrIdxRow["WordLocation"] = WrdLoc;
					ChrIdxRow["CharListID"] = EticRow["CharListID"];
					tblChrIdx.Rows.Add(ChrIdxRow);
					tblChrIdx.Commit();

					rgsChars.Capacity = j + 1;
					rgsHexChars.Capacity = j + 1;
					rgsBaseHexChars.Capacity = j + 1;
					rgsMOAKeys.Capacity = j + 1;
					rgsPOAKeys.Capacity = j + 1;
					rgsUDKeys.Capacity = j + 1;
					rgiCharOffsets.Capacity = j + 1;
					rglFeatures.Capacity = (j + 1) * 4;
					rglBinaryFeatures.Capacity = (j + 1) * 2;

					/// If base phone is a vowel or consonant then add just
					/// base phone to the base phonetic string.
					if ((ChrType > 0) && (ChrType <= 4))
					{
						rgsBaseHexChars[j] = HexChar;
						m_BaseEtic += sChar;
					}
					else
						rgsBaseHexChars[j] = "";

					rgsChars[j] = (string) sChar;
					rgsHexChars[j] = (string) HexChar;
					rgsMOAKeys[j] = (string) EticRow["MOArticulation"];
					rgsPOAKeys[j] = (string) EticRow["POArticulation"];
					rgsUDKeys[j] = ((int) EticRow["UDSortOrder"]).ToString("X4");
					rgiCharOffsets[j] = (short) ByteOff;
					rglFeatures[j * 4] = (int) EticRow["Mask0"];
					rglFeatures[j * 4 + 1] = (int) EticRow["Mask1"];
					rglFeatures[j * 4 + 2] = (int) EticRow["Mask2"];
					rglFeatures[j * 4 + 3] = (int) EticRow["Mask3"];
					rglBinaryFeatures[j * 2] = (int) EticRow["BinaryMask0"];
					rglBinaryFeatures[j * 2 + 1] = (int) EticRow["BinaryMask1"];
					j++;

					ByteOff += (short) sChar.Length;

					/// Determine the next IPA phone's location within its word.
					/// At this point it can only be medial or final.
					WrdLoc = ((i == (IPAISIdx + NumChars - 2)) ? "F" : "M");
				}
			}

			/// If the phones just written were from a new word, add a record for
			/// that word to the non jet word list file.
			/// ***NB: not sure about non jet file, so not coding the logic for this
			/// part yet. but do take note that in the VB code, writing to the non
			/// jet file was done here.
			return true;
		}

		/// <summary>
		/// Gets the ID of the word passed as sWord.
		/// This version calls GetNonEticWordID(tbl, sWord, true).
		/// </summary>
		/// <param name="tbl">List table to search for ID</param>
		/// <param name="sWord">Word whose ID is to be found.</param>
		/// <returns>The ID of the record containing sWord.</returns>
		private int GetNonEticWordID(PaDataTable tbl, string sWord)
		{
			return GetNonEticWordID(tbl, sWord, true);
		}

		/// <summary>
		/// Gets the ID of the word passed as sWord.
		/// Corresponds to the method of the same name found in the VB code.
		/// </summary>
		/// <param name="tbl">List table to search for ID</param>
		/// <param name="sWord">Word whose ID is to be found.</param>
		/// <param name="bHasHex">Does this word have a hex representation?</param>
		/// <returns>The ID of the record containing sWord.</returns>
		private int GetNonEticWordID(PaDataTable tbl, string sWord, bool bHasHex)
		{
			string sField, sHexField;

			if (sWord.Length == 0)
				return 0;
			sHexField = tbl.PrimaryKey[0].ColumnName;
			sField = (bHasHex ? sHexField.Substring(3) : sHexField);
			DataRow row = tbl.Rows.Find((bHasHex ? StrToHex(sWord) : sWord));
			if (row == null)
			{
				row = tbl.NewRow();
				row[sField] = sWord;
				if (bHasHex)
					row[sHexField] = StrToHex(sWord);
				tbl.Rows.Add(row);
			}

			return (int) row[sField + "ListID"];
		}

		/// <summary>
		/// Converts a string to its hex representation as used throughout the database.
		/// </summary>
		/// <param name="str">The string to be converted.</param>
		/// <returns>The hex representation of str, i.e. "ABC" would become "41 42 43".</returns>
		private string StrToHex(string str)
		{
			byte[] Bytes = Encoding.Unicode.GetBytes(str);
			Bytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, Bytes);
			string hex = "";
			foreach (byte byt in Bytes)
			{
				hex += byt.ToString("X2 ");
			}
			hex = hex.Trim();
			return hex;
		}

		/// <summary>
		/// This routine will update the current record in the phonetic list table
		/// with the appropriate IDs for each byte in the phone. The IDs are taken
		/// from the IPACharSet table. Phone types are:
		/// 1,2,3,4 = Base types of characters,
		/// 5 = Suprasegmentals,
		/// 6 = Tone & Accents, and
		/// 7 = Diacritics.
		/// </summary>
		/// <param name="sPhone"></param>
		private void UpdatePhonesSortKeysAndFeatures(string sPhone, DataRow EticRow)
		{
			int i, j, IPABaseID = 0;
			short PhoneLen, ChrType;
			int [] Masks = new int[4], BinaryMasks = new int[2];
			string sMOAKey, sPOAKey;
			string [] sMaskDBFlds = new string[4];
			DataRow IPARow;
			PhoneLen = (short) sPhone.Length;
			ChrType = 0;
			sMOAKey = "";
			sPOAKey = "";
			BinaryMasks[0] = 0;
			BinaryMasks[1] = 0;
			int [] AllConMask = new int[4], AllVowMask = new int[4],
				BaseConMask = new int[4], BaseVowMask = new int[4];

			/// Basically, this is the code from GetCVClassMasks, so we can use the
			/// Mask arrays further down in this function.
			PaDataTable tblEticClass = new PaDataTable("SELECT * FROM PhoneticClass");

			tblEticClass.PrimaryKey = new DataColumn[] {tblEticClass.Columns["AllConClass"]};
			DataRow TmpRow = tblEticClass.Rows.Find(true);
			if (TmpRow != null)
				for (i = 0; i <= 3; i++)
					AllConMask[i] = (int) TmpRow["Mask" + i];

			tblEticClass.PrimaryKey = new DataColumn[] {tblEticClass.Columns["AllVowClass"]};
			TmpRow = tblEticClass.Rows.Find(true);
			if (TmpRow != null)
				for (i = 0; i <= 3; i++)
					AllVowMask[i] = (int) TmpRow["Mask" + i];

			tblEticClass.PrimaryKey = new DataColumn[] {tblEticClass.Columns["BaseConClass"]};
			TmpRow = tblEticClass.Rows.Find(true);
			if (TmpRow != null)
				for (i = 0; i <= 3; i++)
					BaseConMask[i] = (int) TmpRow["Mask" + i];

			tblEticClass.PrimaryKey = new DataColumn[] {tblEticClass.Columns["BaseVowClass"]};
			TmpRow = tblEticClass.Rows.Find(true);
			if (TmpRow != null)
				for (i = 0; i <= 3; i++)
					BaseVowMask[i] = (int) TmpRow["Mask" + i];

			for (i = 0; i <= 3; i++)
			{
				Masks[i] = 0;
				sMaskDBFlds[i] = "Mask" + i;
			}

			/// Go through each byte in the phone. If this is not a multibyte
			/// phone then this loop only runs once.
			for (i = 1; i <= PhoneLen; i++)
			{
				/// Find the current byte's record in the IPA char. set table.
				IPARow = tblIPASet.Rows.Find((short) sPhone[i]);
				if (IPARow == null)
				{
					/// If the byte wasn't found in IPA char. table, tell the user.
					if (sPhone[i] != 'Å')
					{
						MessageBox.Show(this, "Character code " + ((short) sPhone[i]) +
							" not found in IPA Character Set Table.", Application.ProductName,
							MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					sMOAKey += "FFF";
					sPOAKey += "FFF";
				}
				else
				{
					/// If we're looking at the first byte in the multibyte phone,
					/// save its phone type and ID.
					if (i == 1)
					{
						IPABaseID = (int) IPARow["IPACharSetID"];
						ChrType = (short) IPARow["CharType"];
					}
					/// Accumulate the manner and point of articulation sort key.
					if ((short) IPARow["MOArticulation"] > 0)
						sMOAKey += ((short) IPARow["MOArticulation"]).ToString("X3");
					else
						sMOAKey += "FFF";

					if ((short) IPARow["POArticulation"] > 0)
						sPOAKey += ((short) IPARow["POArticulation"]).ToString("X3");
					else
						sPOAKey += "FFF";

					/// Accumulate the IPA features for this phone.
					for (j = 0; j <= 3; j++)
					{
						if (!IPARow.IsNull(sMaskDBFlds[j]))
							Masks[j] |= (int) IPARow[sMaskDBFlds[j]];
					}

					/// Accumulate the binary features for this phone.
					if (!IPARow.IsNull("BinaryMask0"))
						BinaryMasks[0] |= (int) IPARow["BinaryMask0"];
					if (!IPARow.IsNull("BinaryMask1"))
						BinaryMasks[1] |= (int) IPARow["BinaryMask1"];
				}
			}

			/// Save the phone of the base phone, the manner and point of
			/// articulation sort keys and the binary mask for this phone.
			EticRow["IPABaseID"] = IPABaseID;
			EticRow["MOArticulation"] = sMOAKey;
			EticRow["POArticulation"] = sPOAKey;
			EticRow["BinaryMask0"] = BinaryMasks[0];
			EticRow["BinaryMask1"] = BinaryMasks[1];
			EticRow["BaseCharType"] = ChrType;
			for (i = 0; i <= 3; i++)
			{
				if ((ChrType == 1) || (ChrType == 3) || (ChrType == 4))
				{
					Masks[i] |= AllConMask[i];
					if (PhoneLen > 1)
						Masks[i] |= BaseConMask[i];
				}
				else if (ChrType == 2)
				{
					Masks[i] |= AllVowMask[i];
					if (PhoneLen > 1)
						Masks[i] |= BaseVowMask[i];
				}

				EticRow[sMaskDBFlds[i]] = Masks[i];
			}

			tblEtic.Commit();
		}

		private void WriteSortKeysForWord(int ID)
		{
			tblWrdLst.PrimaryKey = new DataColumn[] {tblWrdLst.Columns["WordListID"]};
			DataRow row = tblWrdLst.Rows.Find(ID);
			if (row == null)
				return;
			row["BaseHexPhonetic"] = StrToHex(m_BaseEtic);
			row["CVPattern"] = StrToHex(m_CVPattern);
			if (m_MOAWrdKey.Length > 0)
				row["MOASortKey"] = m_MOAWrdKey;
			if (m_POAWrdKey.Length > 0)
				row["POASortKey"] = m_POAWrdKey;
			if (m_UDWrdKey.Length > 0)
				row["UDSortKey"] = m_UDWrdKey;
			tblWrdLst.Commit();
			tblWrdLst.PrimaryKey = new DataColumn[] {tblWrdLst.Columns["HexPhonetic"]};
		}

		private void SavNonEticTranscriptions(int DocID, string FullEmic, string FullOrtho)
		{
			PaDataTable table = new PaDataTable("SELECT * FROM Document");
			table.PrimaryKey = new DataColumn[] {table.Columns["DocID"]};
			DataRow row = table.Rows.Find(DocID);
			if (row == null)
				return;
			row["FullPhonemic"] = FullEmic.Trim();
			row["FullOrtho"] = FullOrtho.Trim();
			row["FreeForm"] = ltbFreeform.Text;
			row["Transcriber"] = llbTranscriber.Text;
			row["LastUpdate"] = DateTime.Now;
			table.Commit();
		}

		#region Event handlers
		private void splitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			oldFFHeight = ltbFreeform.Height;
		}

		private void btnAutoAlign_Click(object sender, System.EventArgs e)
		{
			///Not completed for now.
			MessageBox.Show("This is not completed yet","Phonology Assistant",
				MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			txtWaveFile.Text = PaApp.OpenFileDialog("*.wav", "Audio Files (*.wav)|*.wav", "Add Audio File(s)");
		}

		private void TransTypeChanged(object sender, System.EventArgs e)
		{
			/// taking advantage of the fact that a radiobutton is unchecked
			/// before another is checked, we shall save the data in the
			/// case of unchecking, and hide, then during the checking of
			/// the new radiobutton, setup and show the new view.
			RadioButton rb = sender as RadioButton;
			int tagValue = int.Parse(rb.Tag.ToString());
			/// for txt -> ? type, break the string into words and
			/// save them into the respective section in the array
			/// Tag corresponds to the old index of the radio button
			/// array in VB6
			if (tagValue < 3)
			{
				/// since we're being unchecked, save the data
				/// as appropriate, then hide the control.
				if (!(rb.Checked))
				{
					int i;
					txtTrans.Text = txtTrans.Text.Trim();
					string [] words = txtTrans.Text.Split(" ".ToCharArray());
					/// as long as we have words to be saved, and there are still existing
					/// words to which our transcriptions can be saved, save to the
					/// appropriate field.
					for (i = 0; (i < m_docWords.Count); i++)
					{
						/// if there are still words to be saved, save them
						/// else set the respective field to null
						if (i < words.Length)
						{
							switch (tagValue)
							{
								case 0: // Phonetic
									((DocWords)m_docWords[i]).Phonetic = words[i];
									break;
								case 1: // Phonemic
									((DocWords)m_docWords[i]).Phonemic = words[i];
									break;
								case 2: // Orthographic
									((DocWords)m_docWords[i]).Ortho = words[i];
									break;
							}
						}
						else
						{
							switch (tagValue)
							{
								case 0: // Phonetic
									((DocWords)m_docWords[i]).Phonetic = "Å";
									break;
								case 1: // Phonemic
									((DocWords)m_docWords[i]).Phonemic = "Å";
									break;
								case 2: // Orthographic
									((DocWords)m_docWords[i]).Ortho = ".";
									break;
							}
						}
					}
					/// if we enter this at all, we have more words to save than
					/// the current number of words stored.
					for ( ; i < words.Length ; i++)
					{
						DocWords dw = new DocWords();
						switch (tagValue)
						{
							case 0: // Phonetic
								dw.Phonetic = words[i];
								break;
							case 1: // Phonemic
								dw.Phonemic = words[i];
								break;
							case 2: // Orthographic
								dw.Ortho = words[i];
								break;
						}
						m_docWords.Add(dw);
					}

					while (((DocWords)m_docWords[m_docWords.Count - 1]).IsEmpty())
					{
						m_docWords.RemoveAt(m_docWords.Count - 1);
					}
					txtTrans.Hide();
				}
				else // it's checked, so set it up!
				{
					txtTrans.Text = "";
					switch (tagValue)
					{
						case 0: // Phonetic
							foreach (object obj in m_docWords)
							{
								txtTrans.Text += ((DocWords)obj).Phonetic + " ";
							}
							txtTrans.Text.Trim();
							txtTrans.Font = PaApp.PhoneticFont;
							break;
						case 1: // Phonemic
							foreach (object obj in m_docWords)
							{
								txtTrans.Text += ((DocWords)obj).Phonemic + " ";
							}
							txtTrans.Text.Trim();
							txtTrans.Font = PaApp.PhonemicFont;
							break;
						case 2: // Orthographic
							foreach (object obj in m_docWords)
							{
								txtTrans.Text += ((DocWords)obj).Ortho + " ";
							}
							txtTrans.Text.Trim();
							txtTrans.Font = PaApp.OrthograpicFont;
							break;
					}
					txtTrans.Dock = DockStyle.Fill;
					txtTrans.Show();
					txtTrans.BringToFront();
				}
			}
			else if (tagValue == 3)
			{
				if (rb.Checked)
				{
					// get rid of any rows that currently exist, then make
					// new rows using the words data that we have.
					m_WordList.Rows.Clear();
					FormatWords(0);
					foreach (object obj in m_docWords)
					{
						DocWords dw = obj as DocWords;
						m_WordList.Rows.Add(new FwGridRow(
							new string[]{dw.Phonetic, dw.Tone, dw.Phonemic,
											dw.Ortho, dw.Gloss, dw.POS, dw.Ref}));
					}
					m_WordList.Visible = true;
					// redraw so that the updated grid is shown
					panel1.Invalidate();
				}
				else
				{
					foreach (FwGridRow row in m_WordList.Rows)
					{
						int i = m_WordList.Rows.IndexOf(row);
						DocWords dw;
						if (i < m_docWords.Count)
							dw = m_docWords[i] as DocWords;
						else
							dw = new DocWords();
						dw.Phonetic = row.Cells[0].Text;
						dw.Tone = row.Cells[1].Text;
						dw.Phonemic = row.Cells[2].Text;
						dw.Ortho = row.Cells[3].Text;
						dw.Gloss = row.Cells[4].Text;
						dw.POS = row.Cells[5].Text;
						dw.Ref = row.Cells[6].Text;
						if (i >= m_docWords.Count)
							m_docWords.Add(dw);
					}
					FormatWords(-1)	;
					m_WordList.Visible = false;
				}
			}
			else if (tagValue == 4)
			{
				if (!(rb.Checked))
				{
					FormatWords(-1);
					ArrayList toDelete = new ArrayList();
					foreach (object obj in m_docWords)
					{
						DocWords dw = obj as DocWords;
						if (dw.IsEmpty())
							toDelete.Add(dw);
					}
					foreach (object obj in toDelete)
					{
						m_docWords.Remove(obj);
					}
					// this was used when i didn't know how to remove
					// event handlers
					//	foreach (object obj in m_wordData)
					//	{
					//		((DocWords)obj).RecreateLabels();
					//	}
					GC.Collect();
					m_interlinear.Visible = false;
				}
				else
				{
					if (m_interlinear == null)
					{
						m_interlinear = new InterlinearBox();
						m_interlinear.AllowEdit = !m_ViewOnly;
						m_interlinear.ContextMenu = contextMenu;
						panel1.Controls.Add(m_interlinear);
					}
					FormatWords(1);
					///load the info
					m_interlinear.Words = m_docWords;
					m_interlinear.FillCells();
					m_interlinear.Dock = DockStyle.Fill;
					m_interlinear.Show();
					m_interlinear.BringToFront();
				}
			}
			mnuLabels.Visible = mnuLabels.Enabled = rbInterlinear.Checked;
			mnuGridLine.Visible = mnuGridLine.Enabled = rbWordList.Checked;
			mnuSep1.Visible = mnuSpc.Visible = mnuSpc.Enabled = rbWordList.Checked;
		}

		private void GridKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ')
			{
				if ((m_WordList.CurrentColumnIndex != m_WordList.Columns.IndexOf(m_WordList.Columns["gloss"])) &&
					(m_WordList.CurrentColumnIndex != m_WordList.Columns.IndexOf(m_WordList.Columns["ref"])))
				{
					e.Handled = true;
					if (m_SpcMovesRow)
					{
						if (m_WordList.CurrentRowIndex < m_WordList.Rows.Count - 1)
							m_WordList.CurrentRowIndex++;
					}
					else
					{
						m_WordList.CurrentColumnIndex++;
					}
					SendKeys.Send("{F2}");
				}
			}
		}

		private void GridKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Space)
			{
				if ((m_WordList.CurrentColumn != m_WordList.Columns["gloss"]) &&
					(m_WordList.CurrentColumn != m_WordList.Columns["ref"]))
				{
					/// if we choose to change rows when pressing space,
					/// then it increments the rowindex, otherwise, it
					/// increments the column index;
					if ((m_WordList.CurrentRowIndex < m_WordList.Rows.Count - 1) && m_SpcMovesRow)
						m_WordList.CurrentRowIndex++;
					else if (!m_SpcMovesRow)
						m_WordList.CurrentColumnIndex++;
					m_WordList.MakeCellVisible(m_WordList.CurrentCell);
				}
			}
		}

		/// <summary>
		/// We use this to put the currently selected cell into edit mode.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void GridAfterRowColChange(object sender, FwGridRowColChangeArgs args)
		{
			// this works when the cell is clicked, or when the previous cell
			// wasn't in edit mode when spacebar was pressed. however, when we're
			// in edit mode and change cells by spacebar, the cell that we end up
			// in isn't in edit mode by the time the user gets control, even though
			// debugging has shown that it *was* put into edit mode within this
			// handler. this problem applies whether we change rows or cols using
			// spacebar.
			SendKeys.SendWait("{F2}");
		}

		private void AfterDropDownClosed(DropDownContainer dropDownContainer, object eventData)
		{
			try
			{
				((FwGridDropDownEventArgs)eventData).Cell.Text =
					((FwGridColumnList)dropDownContainer).SelectedItem;
			}
			catch {}
		}

		private void txtWaveFile_TextChanged(object sender, EventArgs e)
		{
			btnAutoAlign.Enabled = File.Exists(txtWaveFile.Text);
			rbAutoSeg.Enabled = rbEqualSeg.Enabled = btnAutoAlign.Enabled;
			if (!rbAutoSeg.Checked && !rbEqualSeg.Checked)
				rbEqualSeg.Checked = true;
		}
		#endregion
	}

	internal class PhoneInfoStruct
	{
		private bool m_StartOfWord;
		private string m_Char;
		private string m_Gloss;
		private string m_POS;
		private string m_Ref;
		private int m_Offset;
		private int m_Length;
		private int m_WrdWavLength;
		private short m_ByteOff;
		private int m_AllWordIndexID;
		private short [] m_CharTypes;
		private int [] m_IPACharSetIDs;

		/// <summary>
		/// Flag marking this phone as the beginning of a word.
		/// </summary>
		public bool StartOfWord
		{ get { return m_StartOfWord; } set { m_StartOfWord = value; } }
		/// <summary>
		/// Stores Phonetic and phonemic IPA phone.
		/// </summary>
		public string Char
		{ get { return m_Char; } set { m_Char = value; } }
		/// <summary>
		/// Gloss of Phonetic word (only occurs in rec. that is the first char. in word).
		/// </summary>
		public string Gloss
		{ get { return m_Gloss; } set { m_Gloss = value; } }
		public string POS
		{ get { return m_POS; } set { m_POS = value; } }
		public string Ref
		{ get { return m_Ref; } set { m_Ref = value; } }
		/// <summary>
		/// Byte offset into wave data of IPA phone.
		/// </summary>
		public int Offset
		{ get { return m_Offset; } set { m_Offset = value; } }
		/// <summary>
		/// Byte length of IPA phone in wave data.
		/// </summary>
		public int Length
		{ get { return m_Length; } set { m_Length = value; } }
		/// <summary>
		/// Byte length of word in .WAV file. This is only relevant when StartOfWord is true.
		/// </summary>
		public int WrdWavLen
		{ get { return m_WrdWavLength; } set { m_WrdWavLength = value; } }
		/// <summary>
		/// Byte offset of this phone within IPA string.
		/// </summary>
		public short ByteOff
		{ get { return m_ByteOff; } set { m_ByteOff = value; } }
		/// <summary>
		/// AllWordIndexID of record in AllWordsIndex table where phone is found.
		/// </summary>
		public int AllWordIndexID
		{ get { return m_AllWordIndexID; } set { m_AllWordIndexID = value; } }
		/// <summary>
		/// Array of phone types for each byte of phone.
		/// </summary>
		public short [] CharTypes
		{ get { return m_CharTypes; } set { m_CharTypes = value; } }
		/// <summary>
		/// Array of IPACharSetIDs - one for each byte in phone.
		/// These IDs are the record IDs from the IPACharSet table.
		/// </summary>
		public int [] IPACharSetIDs
		{ get { return m_IPACharSetIDs; } set { m_IPACharSetIDs = value; } }
	}

	/// <summary>
	/// This class corresponds to the structure of the same name found in the VB
	/// code. It is currently located in TextEd.cs, original was GVARS.BAS in VB.
	/// </summary>
	internal class FFWordsInfoStruct
	{
		private int m_WordListID;
		private string m_Word;
		private string [] m_Chars, m_HexChars, m_BaseHexChars, m_MOAKeys, m_POAKeys,
			m_UDKeys;
		private short [] m_CharOffsets;
		private int [] m_Features, m_BinaryFeatures;

		public int WordListID { get { return m_WordListID; } set { m_WordListID = value; } }
		public string Word { get { return m_Word; } set { m_Word = value; } }
		public string [] Chars { get { return m_Chars; } set { m_Chars = value; } }
		public string [] HexChars { get { return m_HexChars; } set { m_HexChars = value; } }
		public string [] BaseHexChars { get { return m_BaseHexChars; } set { m_BaseHexChars = value; } }
		public string [] MOAKeys { get { return m_MOAKeys; } set { m_MOAKeys = value; } }
		public string [] POAKeys { get { return m_POAKeys; } set { m_POAKeys = value; } }
		public string [] UDKeys { get { return m_UDKeys; } set { m_UDKeys = value; } }
	}
}
