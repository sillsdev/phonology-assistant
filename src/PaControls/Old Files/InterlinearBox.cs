using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using XCore;

namespace SIL.Pa
{
	/// clsInterlinear is intended to be included in this implementation
	/// on top of that, we intend to replicate the rest of the features
	/// of that particular panel in here.
	/// Note: BorderStyle of the margin panel has been removed to match
	/// the vb6 version. we need to draw in a vertical line beside the
	/// margin panel, as in vb6
	/// Note: The margin labels shall be positioned based on the height
	/// of the word labels.
	/// Note: pnlHeaders and pnlWords must always be the same height.
	/// pnlWords shall be exactly as wide as the client area of panel1.
	/// Labels on pnlWords shall start from a point just to the right of
	/// the right border of pnlHeaders, and pnlWords MUST be BEHIND
	/// pnlHeaders
	/// Note: this class is still highly inefficient and will be quite
	/// unpleasant as the user will have to wait quite long each time
	/// the labels are re-laid.
	/// Bug: labels have a tendency to disappear when one of them is
	/// clicked on.
	/// Note: adjust pnlwords' width to fit within the clientrectangle
	/// of panel1 to prevent panel1's horizontal scrollbar from appearing
	/// and adjust pnlwords' height to prevent the vertical scrollbar from
	/// appearing. GetWindowLong() is used for checking.
	/// Note: another problem. the horizontal scrollbar isn't visible
	/// for pnlWords unless one scrolls all the way down. how to fix?
	/// Question: is it possible to keep pnlWords at one size, then pretend
	/// to scroll it when panel1 scrolls? (likewise for pnlHeaders?)
	/// idea: what if we use an HScrollBar, then set its scrolling to
	/// scroll pnlWords accordingly.
	/// Note: Given up. Will handle as in frmTED and clsInterlinear,
	/// using separate scrollbars and scrolling by moving panels.
	/// <summary>
	/// Summary description for InterlinearBox.
	/// </summary>
	public class InterlinearBox : System.Windows.Forms.UserControl, IxCoreColleague
	{
		[DllImport("user32")]
		private extern static int SendMessage(
			IntPtr handle, int msg,
			int wParam, int lParam);
		/// <summary>
		/// Simulates mouse events.
		/// </summary>
		/// <param name="dwFlags">What sort of event info?</param>
		/// <param name="dx">Position or movement along x-axis.</param>
		/// <param name="dy">Position or movement along y-axis.</param>
		/// <param name="cButtons">Mousewheel movement, or 0 if none.</param>
		/// <param name="dwExtraInfo">Extra info.</param>
		[DllImport("user32")]
		private extern static void mouse_event(
			int dwFlags, int dx, int dy,
			int cButtons, int dwExtraInfo);
		[DllImport("user32")]
		public static extern int SetCursorPos(int x, int y);
		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_MIDDLEDOWN = 0x020;
		private const int MOUSEEVENTF_MIDDLEUP = 0x40;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;
		private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
		private const int EM_POSFROMCHAR = 0x00D6;
		private const int EM_CHARFROMPOS = 0x00D7;
		private const int EticIndex = 0;
		private const int ToneIndex = 1;
		private const int EmicIndex = 2;
		private const int OrthoIndex = 3;
		private const int GlossIndex = 4;
		private const int POSIndex = 5;
		private const int RefIndex = 6;
		private Mediator m_msgMediator;
		//private SettingsHandler m_settingsHndlr;
		private string[] m_HeaderNames;
		private string[] m_HeaderSFMs;
		private Label m_currLabel;
		/// <summary>
		/// This is our array of row headers.
		/// </summary>
		private Label[] lblHeaders;
		/// <summary>
		/// The words are contained in the Text of the labels contained in
		/// this class, and can be accessed directly as .Phonetic, .Tone, etc.
		/// </summary>
		private ArrayList m_docWords;
		/// <summary>
		/// Appears when a label is clicked on.
		/// </summary>
		private TextBox txtEdit;
		/// <summary>
		/// 0 for no headers, 1 for names, 2 for sfm
		/// </summary>
		private short m_ShowHeaders;
		private int m_currCol, m_currRow;
		/// <summary>
		/// Panel used to cover the space between the 2 scrollbars
		/// when they are both visible.
		/// </summary>
		private Panel pnlCorner;
		private bool[] m_RowIsVisible;
		private int[] m_StartOfBlock;
		private bool m_AllowEdit;
		#region Variables added by Designer
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel pnlHeaders;
		private System.Windows.Forms.Panel pnlWords;
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		public InterlinearBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			m_msgMediator = new Mediator();
			m_msgMediator.AddColleague(this);
			ReadSFMs();
			SetupHeaders();
			m_ShowHeaders = 1;
			txtEdit = new TextBox();
            txtEdit.Visible = false;
			txtEdit.BorderStyle = BorderStyle.None;
			txtEdit.Multiline = false;
			txtEdit.BackColor = pnlWords.BackColor;
			txtEdit.KeyPress += new KeyPressEventHandler(txtEdit_KeyPress);
			txtEdit.KeyDown += new KeyEventHandler(txtEdit_KeyDown);
			pnlCorner = new Panel();
			pnlCorner.BackColor = Color.FromKnownColor(KnownColor.ScrollBar);
			pnlCorner.Visible = false;
			panel1.Controls.Add(pnlCorner);
			pnlWords.Controls.Add(txtEdit);
			m_RowIsVisible =
				new bool[] { true, true, true, true, true, true, true };
		}

		/// <summary>
		/// Fills the cells of the interlinear box with data
		/// from the record whose ID is specified in DocID.
		/// </summary>
		public void FillCells()
		{
			/// Layout the labels already found in m_DocWords
			LayoutLabels();
		}

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
			if (m_docWords == null) return;
			LayoutLabels();
			LayoutHeaders();
			SetupScrollBars();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			if (Visible)
				LayoutHeaders();
			base.OnVisibleChanged (e);
		}

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated (e);
			LayoutLabels();
			LayoutHeaders();
			SetupScrollBars();
			SetupScrollBars();
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.pnlHeaders = new System.Windows.Forms.Panel();
			this.pnlWords = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ScrollBar;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.hScrollBar1);
			this.panel1.Controls.Add(this.vScrollBar1);
			this.panel1.Controls.Add(this.pnlHeaders);
			this.panel1.Controls.Add(this.pnlWords);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(536, 168);
			this.panel1.TabIndex = 0;
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Location = new System.Drawing.Point(0, 148);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(516, 16);
			this.hScrollBar1.TabIndex = 0;
			this.hScrollBar1.Visible = false;
			this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Location = new System.Drawing.Point(516, 0);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(16, 148);
			this.vScrollBar1.TabIndex = 2;
			this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
			// 
			// pnlHeaders
			// 
			this.pnlHeaders.Location = new System.Drawing.Point(0, 0);
			this.pnlHeaders.Name = "pnlHeaders";
			this.pnlHeaders.Size = new System.Drawing.Size(128, 144);
			this.pnlHeaders.TabIndex = 0;
			// 
			// pnlWords
			// 
			this.pnlWords.Location = new System.Drawing.Point(0, 0);
			this.pnlWords.Name = "pnlWords";
			this.pnlWords.Size = new System.Drawing.Size(528, 144);
			this.pnlWords.TabIndex = 1;
			// 
			// InterlinearBox
			// 
			this.Controls.Add(this.panel1);
			this.Name = "InterlinearBox";
			this.Size = new System.Drawing.Size(536, 168);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		protected bool OnInitializeItem(object args)
		{
			ArrayList list = args as ArrayList;

			if (list == null || list.Count == 0)
				return false;

			if (((string)list[0]).StartsWith("field"))
			{
				int i = -1;
				if ((string)list[3] == "Phonetic") i = EticIndex;
				else if ((string)list[3] == "Tone") i = ToneIndex;
				else if ((string)list[3] == "Phonemic") i = EmicIndex;
				else if ((string)list[3] == "Ortho") i = OrthoIndex;
				else if ((string)list[3] == "Gloss") i = GlossIndex;
				else if ((string)list[3] == "POS") i = POSIndex;
				else if ((string)list[3] == "WordRef") i = RefIndex;
				if (i != -1)
				{
					m_HeaderNames[i] = (string)list[4];
					m_HeaderSFMs[i] = (string)list[5];
				}
				return true;
			}

			return false;
		}

		private void LayoutLabels()
		{
			/// TODO. start to lay columns until we can't get it without
			/// being hidden (if it's the only one in a block, lay it anyway)
			/// then create a new block and continue laying.
			int rightmost = 0;
			ArrayList newStartOfBlock = new ArrayList();
			try
			{
				newStartOfBlock.Add(0);
				foreach (object obj in m_docWords)
				{
					DocWords dw = obj as DocWords;
					if (dw.ClickAssigned)
					{
						foreach (Label lbl in dw.Cells)
						{
							lbl.Click -= new EventHandler(lbl_Click);
							pnlWords.Controls.Remove(lbl);
						}
						dw.ClickAssigned = false;
					}
				}
				foreach (object obj in m_docWords)
				{
					DocWords dw = obj as DocWords;
					pnlWords.Controls.AddRange(dw.Cells);
				}
				pnlWords.SuspendLayout();
				int left = 0, blkno = 0, currTop = 0;
				for (int i = 0; i < m_docWords.Count; i++)
				{
					foreach (Label lbl in ((DocWords)m_docWords[i]).Cells)
					{
						lbl.AutoSize = true;
					}
					// we will only consider the width of the visible cells
					// when we determine the width of the column.
					int colwidth = ((DocWords)m_docWords[i]).Cells[EticIndex].Width;
					if (m_RowIsVisible[ToneIndex])
					{
						colwidth = ((colwidth > ((DocWords)m_docWords[i]).Cells[ToneIndex].Width) ? colwidth
							: ((DocWords)m_docWords[i]).Cells[ToneIndex].Width);
					}
					if (m_RowIsVisible[EmicIndex])
					{
						colwidth = ((colwidth > ((DocWords)m_docWords[i]).Cells[EmicIndex].Width) ?	colwidth
							: ((DocWords)m_docWords[i]).Cells[EmicIndex].Width);
					}
					if (m_RowIsVisible[OrthoIndex])
					{
						colwidth = ((colwidth >((DocWords)m_docWords[i]).Cells[OrthoIndex].Width) ?	colwidth
							: ((DocWords)m_docWords[i]).Cells[OrthoIndex].Width);
					}
					if (m_RowIsVisible[GlossIndex])
					{
						colwidth = ((colwidth > ((DocWords)m_docWords[i]).Cells[GlossIndex].Width) ?	colwidth
							: ((DocWords)m_docWords[i]).Cells[GlossIndex].Width);
					}
					if (m_RowIsVisible[POSIndex])
					{
						colwidth = ((colwidth > ((DocWords)m_docWords[i]).Cells[POSIndex].Width) ?	colwidth
							: ((DocWords)m_docWords[i]).Cells[POSIndex].Width);
					}
					if (m_RowIsVisible[RefIndex])
					{
						colwidth = ((colwidth > ((DocWords)m_docWords[i]).Cells[RefIndex].Width) ?	colwidth
							: ((DocWords)m_docWords[i]).Cells[RefIndex].Width);
					}
					((DocWords)m_docWords[i]).ColWidth = colwidth;
					if (!((DocWords)m_docWords[i]).ClickAssigned && m_AllowEdit)
					{
						((DocWords)m_docWords[i]).Cells[EticIndex].Click += new EventHandler(lbl_Click);
						((DocWords)m_docWords[i]).Cells[ToneIndex].Click += new EventHandler(lbl_Click);
						((DocWords)m_docWords[i]).Cells[EmicIndex].Click += new EventHandler(lbl_Click);
						((DocWords)m_docWords[i]).Cells[OrthoIndex].Click += new EventHandler(lbl_Click);
						((DocWords)m_docWords[i]).Cells[GlossIndex].Click += new EventHandler(lbl_Click);
						((DocWords)m_docWords[i]).Cells[POSIndex].Click += new EventHandler(lbl_Click);
						((DocWords)m_docWords[i]).Cells[RefIndex].Click += new EventHandler(lbl_Click);
						((DocWords)m_docWords[i]).ClickAssigned = true;
					}

					/// if the current word doesn't fit within the visible portion
					/// of the Words panel, and it isn't the first word of the current
					/// block, put it on a new row (set left to 0) and increase
					/// the block number.
					if (((left + ((DocWords)m_docWords[i]).ColWidth) >=
						(panel1.ClientRectangle.Width - pnlHeaders.Width)) &&
						(left > 0))
					{
						left = 0;
						blkno++;
						newStartOfBlock.Add(i);
						// this was used when all rows were always visible.
						// currTop = ((DocWords)m_wordData[i-1]).Cells[6].Bottom;
						// now we go from the bottom row up to find the last
						// visible row, and that's where we'll start the next
						// block.
						int lastRow = RefIndex;
						for ( ; lastRow > 0; lastRow--)
							if (m_RowIsVisible[lastRow])
								break;
						currTop = ((DocWords)m_docWords[i-1]).Cells[lastRow].Bottom;
					}

					int tmpTop = currTop;
					((DocWords)m_docWords[i]).Cells[EticIndex].Location =
						new Point(left, tmpTop);
					tmpTop = ((DocWords)m_docWords[i]).Cells[EticIndex].Bottom;
					if (m_RowIsVisible[ToneIndex])
					{
						((DocWords)m_docWords[i]).Cells[ToneIndex].Location =
							new Point(left, tmpTop);
						tmpTop = ((DocWords)m_docWords[i]).Cells[ToneIndex].Bottom;
					}
					if (m_RowIsVisible[EmicIndex])
					{
						((DocWords)m_docWords[i]).Cells[EmicIndex].Location =
							new Point(left, tmpTop);
						tmpTop = ((DocWords)m_docWords[i]).Cells[EmicIndex].Bottom;
					}
					if (m_RowIsVisible[OrthoIndex])
					{
						((DocWords)m_docWords[i]).Cells[OrthoIndex].Location =
							new Point(left, tmpTop);
						tmpTop = ((DocWords)m_docWords[i]).Cells[OrthoIndex].Bottom;
					}
					if (m_RowIsVisible[GlossIndex])
					{
						((DocWords)m_docWords[i]).Cells[GlossIndex].Location =
							new Point(left, tmpTop);
						tmpTop = ((DocWords)m_docWords[i]).Cells[GlossIndex].Bottom;
					}
					if (m_RowIsVisible[POSIndex])
					{
						((DocWords)m_docWords[i]).Cells[POSIndex].Location =
							new Point(left, tmpTop);
						tmpTop = ((DocWords)m_docWords[i]).Cells[POSIndex].Bottom;
					}
					if (m_RowIsVisible[RefIndex])
					{
						((DocWords)m_docWords[i]).Cells[RefIndex].Location =
							new Point(left, tmpTop);
					}
					for (int j = 0; j <= RefIndex; j++)
					{
						((DocWords)m_docWords[i]).Cells[j].Visible = m_RowIsVisible[j];
					}
					foreach (Label lbl in ((DocWords)m_docWords[i]).Cells)
					{
						int ht = lbl.Height;
						lbl.AutoSize = false;
						lbl.Height = ht;
						lbl.Width = ((DocWords)m_docWords[i]).ColWidth;
					}
					((DocWords)m_docWords[i]).BlockNum = blkno;
					if (rightmost < ((DocWords)m_docWords[i]).Cells[0].Right)
					{
						rightmost = ((DocWords)m_docWords[i]).Cells[0].Right;
					}
					left += colwidth + 4;
				}
				m_StartOfBlock = (int []) newStartOfBlock.ToArray(typeof(int));
			}
			catch {}
			// was applicable when everything was together on one line
			// pnlWords.Height = ((DocWords)m_wordData[0]).Cells[RefIndex].Bottom;
			// pnlWords.Width = ((DocWords)m_wordData[m_wordData.Count - 1]).Cells[0].Left +
			//	((DocWords)m_wordData[m_wordData.Count - 1]).ColWidth;
			pnlWords.Height = ((DocWords)m_docWords[m_docWords.Count - 1]).Cells[RefIndex].Bottom;
			pnlWords.Width = rightmost;
			pnlWords.ResumeLayout(true);
		}

		private void LayoutHeaders()
		{
			pnlHeaders.Controls.Clear();
			int maxwidth = 0;
			for(int i = 0; i < 7; i++)
			{
				lblHeaders[i].AutoSize = true;
				switch (m_ShowHeaders)
				{
					case 1:
						lblHeaders[i].Text = m_HeaderNames[i];
						break;
					case 2:
						lblHeaders[i].Text = m_HeaderSFMs[i];
						break;
					default:
						lblHeaders[i].Text = "";
						break;
				}
				if (lblHeaders[i].Width > maxwidth)
					maxwidth = lblHeaders[i].Width;
				lblHeaders[i].AutoSize = false;
				lblHeaders[i].Height = ((DocWords)m_docWords[0]).Cells[i].Height;
				lblHeaders[i].Location = new Point(0, ((DocWords)m_docWords[0]).Cells[i].Top);
			}
			for(int i = 0; i < lblHeaders.Length; i++)
			{
				lblHeaders[i].Width = maxwidth;
			}
			int currTop = 0;
			ArrayList newHeaders = new ArrayList();
			for (int j = 0; j < 7 * m_StartOfBlock.Length; j++)
			{
				Label lbl = new Label();
				lbl.Size = lblHeaders[j % 7].Size;
				lbl.Text = lblHeaders[j % 7].Text;
				lbl.Visible = m_RowIsVisible[j % 7];
				if (lbl.Visible)
				{
					lbl.Location = new Point(0, currTop);
					currTop = lbl.Bottom;
				}
				newHeaders.Add(lbl);
			}
			lblHeaders = (Label []) newHeaders.ToArray(typeof(Label));
			pnlHeaders.Width = maxwidth;
			int lastRow;
			for (lastRow = lblHeaders.Length - 1; lastRow >= 0; lastRow--)
				if (lblHeaders[lastRow].Visible)
					break;
			pnlHeaders.Height = lblHeaders[lastRow].Bottom;
			pnlHeaders.Controls.AddRange(lblHeaders);
			pnlWords.Left = pnlHeaders.Visible ? pnlHeaders.Right : 0;
		}

		private void ReadSFMs()
		{
			m_HeaderSFMs = new string[7];
			m_HeaderNames = new string[7];
			string xmlString = null;

			try 
			{
				StreamReader sr = new StreamReader(PaApp.SettingsFile);
				if (sr != null)
					xmlString = sr.ReadToEnd();
			}

			catch {}

//			m_settingsHndlr = new SettingsHandler(xmlString, m_msgMediator);
//			m_settingsHndlr.LoadSettings(this, "sfmmap");
		}

		private void SetupHeaders()
		{
			lblHeaders = new Label[7];
			for (int i=0; i<7; i++)
			{
				Label lbl = new Label();
				lbl.Font = SystemInformation.MenuFont;
				lbl.AutoSize = true;
				lbl.TextAlign = ContentAlignment.MiddleLeft;
				lblHeaders[i] = lbl;
			}
		}

		private void SetupScrollBars()
		{
			if ((pnlWords.Right > panel1.ClientRectangle.Width) ||
				(vScrollBar1.Visible &&
				(pnlWords.Right > vScrollBar1.Left)))
			{
				hScrollBar1.Location = new Point(0,
					panel1.ClientRectangle.Height - hScrollBar1.Height);
				hScrollBar1.Width = panel1.ClientRectangle.Width;
				if (vScrollBar1.Visible)
					hScrollBar1.Width -= vScrollBar1.Width;
				hScrollBar1.Minimum = 0;
				hScrollBar1.Maximum = (pnlWords.Width - panel1.ClientRectangle.Width) * 10 / 9;
				hScrollBar1.LargeChange = (hScrollBar1.Maximum > 0) ? hScrollBar1.Maximum / 10 : 0;
				hScrollBar1.SmallChange = hScrollBar1.LargeChange / 10;
				hScrollBar1.Visible = true;
			}
			else hScrollBar1.Visible = false;

			if ((pnlWords.Height > panel1.ClientRectangle.Height) ||
				(hScrollBar1.Visible &&
				(pnlWords.Height > panel1.ClientRectangle.Height - hScrollBar1.Height)))
			{
				vScrollBar1.Location = new Point(
					panel1.ClientRectangle.Width - vScrollBar1.Width,0);
				vScrollBar1.Height = panel1.ClientRectangle.Height;
				if (hScrollBar1.Visible)
					vScrollBar1.Height -= hScrollBar1.Height;
				vScrollBar1.Minimum = 0;
				vScrollBar1.Maximum = (pnlWords.Height - panel1.ClientRectangle.Height) * 5 / 4;
				vScrollBar1.LargeChange = (vScrollBar1.Maximum > 0) ? vScrollBar1.Maximum / 10 : 0;
				vScrollBar1.SmallChange = vScrollBar1.LargeChange / 10;
				vScrollBar1.Visible = true;
			}
			else vScrollBar1.Visible = false;

			if (hScrollBar1.Visible && vScrollBar1.Visible)
			{
				pnlCorner.Size = new Size(vScrollBar1.Width, hScrollBar1.Height);
				pnlCorner.Location = new Point(vScrollBar1.Left, hScrollBar1.Top);
				pnlCorner.Visible = true;
				pnlCorner.BringToFront();
			}
			else
				pnlCorner.Visible = false;
		}

		private void ProcessNewText(string newText, int newSelStart)
		{
			txtEdit.Text = newText;
			txtEdit.SelectionStart = newSelStart;
			m_currLabel.Text = newText.Trim();
			if (m_currLabel.Text == "") m_currLabel.Text = " ";
			if (ProcessCellEmpty()) return;
			SetMaxColWidth(m_currCol);
		}

		private bool ProcessCellEmpty()
		{
			if (ColEmpty(m_currCol, true))
			{
				/// DeleteCol(m_currCol);
				/// DisplayTextBoxes () which will do layout.
				txtEdit.Visible = false;
				/// ChangeColumns()
				txtEdit.SelectionStart = txtEdit.Text.Length;
				return true;
			}
			return false;
		}

		private void SetMaxColWidth(int Col)
		{
			int MaxWidth = 0;

			for (int i = 0; i < 7; i++)
			{
				if (MaxWidth < ((DocWords)m_docWords[Col]).Cells[i].Width)
					MaxWidth = ((DocWords)m_docWords[Col]).Cells[i].Width;
			}

			((DocWords)m_docWords[Col]).ColWidth = MaxWidth;
		}

		private void DeleteCol(int Col)
		{
			// ArrayList newArray = new ArrayList();

			foreach (Label lbl in ((DocWords)m_docWords[Col]).Cells)
			{
				lbl.Click -= new EventHandler(lbl_Click);
				pnlWords.Controls.Remove(lbl);
			}
			// newArray.AddRange(m_wordData);
			// newArray.RemoveAt(Col);
			// m_wordData = newArray.ToArray(typeof(DocWords)) as DocWords[];
			m_docWords.RemoveAt(Col);
			GC.Collect();
			LayoutLabels();
		}

		private bool ColEmpty(int Col, bool CheckAllLines)
		{
			bool Empty = ((((DocWords)m_docWords[Col]).Phonetic.Trim() == "") &&
				(((DocWords)m_docWords[Col]).Phonemic.Trim() == "") &&
				(((DocWords)m_docWords[Col]).Ortho.Trim() == ""));

			if (CheckAllLines)
			{
				Empty &= ((((DocWords)m_docWords[Col]).Tone.Trim() == "") &&
					(((DocWords)m_docWords[Col]).Gloss.Trim() == "") &&
					(((DocWords)m_docWords[Col]).POS.Trim() == "") &&
					(((DocWords)m_docWords[Col]).Ref.Trim() == ""));
			}

			return Empty;
		}

		private void ZoomToNewCursor(Label lbl)
		{
			// original idea, more trouble than it's worth to get it right:
			//	int lblTop = panel1.PointToClient(pnlWords.PointToScreen(lbl.Location)).Y;
			//	int lblBottom = panel1.PointToClient(pnlWords.PointToScreen(lbl.Location + lbl.Size)).Y;
			//	int lblLeft = panel1.PointToClient(pnlWords.PointToScreen(lbl.Location)).X;
			//	int lblRight = panel1.PointToClient(pnlWords.PointToScreen(lbl.Location + lbl.Size)).X;
			//	while (!panel1.ClientRectangle.Contains(panel1.RectangleToClient(
			//		lbl.RectangleToScreen(lbl.ClientRectangle))))
			//	{
			//		// we want to be able to see the cell, but not get caught in
			//		// an infinite while{} if we can't fit the whole thing into the
			//		// current clientrectangle.
			//		if ((lbl.Width < panel1.ClientRectangle.Width) &&
			//			(lbl.Height < panel1.ClientRectangle.Height))
			//		{
			//			if (!panel1.ClientRectangle.Contains(lblTop, lblLeft) &&
			//				!panel1.ClientRectangle.Contains(lblTop, lblRight))
			//			{
			//				vScrollBar1.Value -= vScrollBar1.SmallChange;
			//			}
			//			else if (!panel1.ClientRectangle.Contains(lblBottom, lblLeft) &&
			//				!panel1.ClientRectangle.Contains(lblBottom, lblRight))
			//			{
			//				vScrollBar1.Value += vScrollBar1.SmallChange;
			//			}
			//
			//			if (!panel1.ClientRectangle.Contains(lblTop, lblLeft) &&
			//				!panel1.ClientRectangle.Contains(lblBottom, lblLeft))
			//			{
			//				hScrollBar1.Value -= hScrollBar1.SmallChange;
			//			}
			//			else if (!panel1.ClientRectangle.Contains(lblTop, lblRight) &&
			//				!panel1.ClientRectangle.Contains(lblBottom, lblRight))
			//			{
			//				hScrollBar1.Value += hScrollBar1.SmallChange;
			//			}
			//		}
			//		else
			//		{
			//			int diffX = lblLeft - panel1.ClientRectangle.Left +
			//				(pnlHeaders.Visible ? pnlHeaders.Width : 0);
			//			int diffY = lblTop - panel1.ClientRectangle.Top;
			//			hScrollBar1.Value += diffX;
			//			vScrollBar1.Value += diffY;
			//			break;
			//		}
			//	}
			// as such, we shall use the original vb6 idea, but since
			// we're scrolling to a label before clicking on it, we will
			// deviate somewhat. in addition, we shall break out if the
			// scrollbar isn't even visible to begin with so as to avoid
			// an infinite while{} loop.
			int i;
			if (lbl.Top <= vScrollBar1.Value)
			{
				i = vScrollBar1.Value;
				while (i >= lbl.Top)
				{
					if (!vScrollBar1.Visible)
						break;
					i -= vScrollBar1.SmallChange;
				}
				vScrollBar1.Value = ((i < 0) ? 0 : i);
			}
			else if ((lbl.Top - vScrollBar1.Value) + lbl.Height >= panel1.Height)
			{
				i = vScrollBar1.Value;
				while ((lbl.Top - i) + lbl.Height >= panel1.Height)
				{
					if (!vScrollBar1.Visible)
						break;
					i += vScrollBar1.SmallChange;
				}
				vScrollBar1.Value = ((i > vScrollBar1.Maximum) ? vScrollBar1.Maximum : i);
			}

			// this is where we depart from the vb6 idea since we do
			// the scrolling *before* we place the cursor (vb6 scrolls *after*)
			if (lbl.Left <= hScrollBar1.Value)
			{
				i = hScrollBar1.Value;
				while (i >= lbl.Left)
				{
					if (!hScrollBar1.Visible)
						break;
					i -= hScrollBar1.SmallChange;
				}
				if (pnlHeaders.Visible)
					i -= pnlHeaders.Width;
				hScrollBar1.Value = ((i < hScrollBar1.Minimum) ? hScrollBar1.Minimum : i);
			}
			else if ((lbl.Left - hScrollBar1.Value) + lbl.Width >= panel1.Width)
			{
				i = hScrollBar1.Value;
				while ((lbl.Left - i) + lbl.Width >= panel1.Width)
				{
					if (!hScrollBar1.Visible)
						break;
					i += hScrollBar1.SmallChange;
				}
				hScrollBar1.Value = ((i > hScrollBar1.Maximum) ? hScrollBar1.Maximum : i);
			}
		}

		private Label GetLeftCell()
		{
			// before word wrap was implemented:
			//	if (m_currCol > 0)
			//		return ((DocWords)m_wordData[m_currCol - 1]).Cells[m_currRow];
			//	else if ((m_currCol == 0) && (m_currRow > 0))
			//		return ((DocWords)m_wordData[m_wordData.Count - 1]).Cells[m_currRow - 1];
			//	else
			//		return null;
			// now, with word wrap on...

			/// Note: I may have the order of testing mixed up, but it
			/// should still work correctly anyway.
			/// Pseudo-code:
			/// if we're at the first block
			///   if we're at the first column
			///     if there aren't any visible rows above this one,
			///       return nothing
			///     else
			///       return the cell at the end of the previous visible
			///         row in this block
			///   else
			///     return the cell to the left of this one
			/// else
			///   if we're at the first column of this block
			///     if the previous visible row belongs to another block
			///       return the cell at the end of that row in that block
			///     else
			///       return the cell at the end of that row in this block
			///   else
			///     return the cell to the left of this one
			int blkno = ((DocWords)m_docWords[m_currCol]).BlockNum;
			int newRow = NextVisibleRow(false);
			if (blkno == 0)
			{
				if (m_currCol == 0)
				{
					if (newRow >= m_currRow)
						return null;
					else
					{
						if (m_StartOfBlock.Length > 1)
							return ((DocWords)m_docWords[m_StartOfBlock[1] - 1]).Cells[newRow];
						else
							return ((DocWords)m_docWords[m_docWords.Count - 1]).Cells[newRow];
					}
				}
				else
					return ((DocWords)m_docWords[m_currCol - 1]).Cells[m_currRow];
			}
			else
			{
				if (m_currCol == m_StartOfBlock[blkno])
				{
					if (newRow >= m_currRow)
						return ((DocWords)m_docWords[m_currCol - 1]).Cells[newRow];
					else
					{
						if (blkno < m_StartOfBlock.Length - 1)
							return ((DocWords)m_docWords[m_StartOfBlock[blkno + 1] - 1]).Cells[newRow];
						else
							return ((DocWords)m_docWords[m_docWords.Count - 1]).Cells[newRow];
					}
				}
				else
					return ((DocWords)m_docWords[m_currCol - 1]).Cells[m_currRow];
			}
		}

		private Label GetRightCell()
		{
			// before word wrap was implemented:
			//	if (m_currCol < m_wordData.Count - 1)
			//		return ((DocWords)m_wordData[m_currCol + 1]).Cells[m_currRow];
			//	else if ((m_currCol == m_wordData.Count - 1) && (m_currRow < ((DocWords)m_wordData[0]).Cells.Length))
			//		return ((DocWords)m_wordData[0]).Cells[m_currRow + 1];
			//	else
			//		return null;
			// now, with word wrap:

			/// Pseudo-code:
			/// if we're at the end of the lot
			///   if the next visible row would be in another block
			///     return nothing
			///   else
			///     return the cell at the beginning of the next visible row
			/// else
			///   if we're not in the last block and we're at the end of a row
			///     if the next visible row belongs to another block,
			///       return the first cell in that row of that block
			///     else
			///       return the first cell in that row of this block
			///   else
			///     return the cell to the right of this one (i.e. next column)
			int blkno = ((DocWords)m_docWords[m_currCol]).BlockNum;
			int newRow = NextVisibleRow(true);

			if (m_currCol == m_docWords.Count - 1)
			{
				if (newRow <= m_currRow)
					return null;
				else
					return ((DocWords)m_docWords[m_StartOfBlock[blkno]]).Cells[newRow];
			}
			else
			{
				if ((blkno < m_StartOfBlock.Length - 1) &&
					(m_currCol == m_StartOfBlock[blkno + 1] - 1))
				{
					if (newRow <= m_currRow)
						return ((DocWords)m_docWords[m_StartOfBlock[blkno + 1]]).Cells[newRow];
					else
						return ((DocWords)m_docWords[m_StartOfBlock[blkno]]).Cells[newRow];
				}
				else
					return ((DocWords)m_docWords[m_currCol + 1]).Cells[m_currRow];
			}
		}

		private Label GetUpCell()
		{
			// broken by implementation of word wrapping
			//	if (m_currRow > 0)
			//		return ((DocWords)m_wordData[m_currCol]).Cells[m_currRow - 1];
			//	else
			//		return null;
			int blkno = ((DocWords)m_docWords[m_currCol]).BlockNum;
			int newRow = NextVisibleRow(false);

			if (newRow >= m_currRow)
			{
				if (blkno == 0)
					return null;
				else
				{
					int i = m_StartOfBlock[blkno - 1];
					int currX = pnlWords.PointToClient(txtEdit.PointToScreen(
						new Point(SendMessage(txtEdit.Handle, EM_POSFROMCHAR,
						txtEdit.SelectionStart, 0)))).X;
					while (i < m_currCol - 1)
					{
						Label lbl = ((DocWords)m_docWords[i]).Cells[newRow];
						if ((currX >= lbl.Left) && (currX <= lbl.Right))
							break;
						i++;
					}
					return ((DocWords)m_docWords[i]).Cells[newRow];
				}
			}
			else
				return ((DocWords)m_docWords[m_currCol]).Cells[newRow];
		}

		private Label GetDownCell()
		{
			// broken by implementation of word wrapping
			//	if (m_currRow < ((DocWords)m_wordData[0]).Cells.Length - 1)
			//		return ((DocWords)m_wordData[m_currCol]).Cells[m_currRow + 1];
			//	else
			//		return null;
			int blkno = ((DocWords)m_docWords[m_currCol]).BlockNum;
			int newRow = NextVisibleRow(true);

			if (newRow <= m_currRow)
			{
				if (blkno == m_StartOfBlock.Length - 1)
					return null;
				else
				{
					int i = m_StartOfBlock[blkno + 1];
					int currX = pnlWords.PointToClient(txtEdit.PointToScreen(
						new Point(SendMessage(txtEdit.Handle, EM_POSFROMCHAR,
						txtEdit.SelectionStart, 0)))).X;
					if (currX < 0)
						currX = 0;
					while ((i < m_docWords.Count) &&
						(((DocWords)m_docWords[i]).BlockNum == blkno + 1))
					{
						Label lbl = ((DocWords)m_docWords[i]).Cells[newRow];
						if ((currX >= lbl.Left) && (currX <= lbl.Right))
							break;
						i++;
					}
					if ((((DocWords)m_docWords[i]).BlockNum > blkno + 1) ||
						(i == m_docWords.Count))
						i--;
					return ((DocWords)m_docWords[i]).Cells[newRow];
				}
			}
			else
				return ((DocWords)m_docWords[m_currCol]).Cells[newRow];
		}

		private Label GetStartCell()
		{
			int blkno = ((DocWords)m_docWords[m_currCol]).BlockNum;
			return ((DocWords)m_docWords[m_StartOfBlock[blkno]]).Cells[m_currRow];
		}

		private Label GetEndCell()
		{
			int blkno = ((DocWords)m_docWords[m_currCol]).BlockNum;
			if (blkno < m_StartOfBlock.Length - 1)
				return ((DocWords)m_docWords[m_StartOfBlock[blkno + 1] - 1]).Cells[m_currRow];
			else
				return ((DocWords)m_docWords[m_docWords.Count - 1]).Cells[m_currRow];
		}

		/// <summary>
		/// Gets the next visible row. Will be needed once we start
		/// hiding rows.
		/// </summary>
		/// <param name="Down">Direction - down = true, up = false.</param>
		/// <returns>Index of the next visible row.</returns>
		private int NextVisibleRow(bool Down)
		{
			int newRow = m_currRow + (Down ? 1 : -1);
			newRow = (newRow < 0) ? 6 : (newRow % 7);
			while (!m_RowIsVisible[newRow])
			{
				newRow += (Down ? 1 : -1);
				newRow = (newRow < 0) ? 6 : (newRow % 7);
			}
			return newRow;
		}

		#region Properties
		/// <summary>
		/// Stores the words of the current document. Remember to set
		/// before displaying the InterlinearBox.
		/// </summary>
		public ArrayList Words
		{
			set { m_docWords = value; }
			get { return m_docWords; }
		}

		public bool ShowTone
		{
			set { m_RowIsVisible[ToneIndex] = value; }
			get { return m_RowIsVisible[ToneIndex]; }
		}

		public bool ShowEmic
		{
			set { m_RowIsVisible[EmicIndex] = value; }
			get { return m_RowIsVisible[EmicIndex]; }
		}

		public bool ShowOrtho
		{
			set { m_RowIsVisible[OrthoIndex] = value; }
			get { return m_RowIsVisible[OrthoIndex]; }
		}

		public bool ShowGloss
		{
			set { m_RowIsVisible[GlossIndex] = value; }
			get { return m_RowIsVisible[GlossIndex]; }
		}

		public bool ShowPOS
		{
			set { m_RowIsVisible[POSIndex] = value; }
			get { return m_RowIsVisible[POSIndex]; }
		}

		public bool ShowRef
		{
			set { m_RowIsVisible[RefIndex] = value; }
			get { return m_RowIsVisible[RefIndex]; }
		}

		public bool AllowEdit
		{
			set { m_AllowEdit = value; }
			get { return m_AllowEdit; }
		}

		/// <summary>
		/// Sets the flag to determine what kind of row header, if any,
		/// to show. 0 for none, 1 for names, 2 for sfm.
		/// </summary>
		public short ShowHeaders
		{
			set
			{
				if (value == m_ShowHeaders)
					return;
				else if (value > 2)
					value = 2;
				else if (value < 0)
					value = 0;

				m_ShowHeaders = value;
				pnlHeaders.Visible = (m_ShowHeaders != 0);
				LayoutHeaders();
			}
			get { return m_ShowHeaders; }
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

		#region Event handlers
		/// <summary>
		/// Handles the keypresses while editing is in progress.
		/// if we reach the limit of a cell, and Control isn't pressed,
		/// go to the next cell in that direction.
		/// up/down changes the row we're on.
		/// if we reach the end of a row, go to the start of the next
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtEdit_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch ((int)e.KeyChar)
			{
				case 8:
					/// if nothing is selected, backspace is equivalent
					/// to [LEFT] followed by [DELETE], otherwise, it's
					/// equivalent to [DELETE].
					/// since word wrapping was implemented, [LEFT] then [DELETE]
					/// no longer works, so we shall move left ourselves
					/// if we need to leave the current cell, but otherwise
					/// everything else can remain as it is.
					e.Handled = true;
					if (txtEdit.SelectionLength == 0)
					{
					//	if ((m_currCol > 0) || (txtEdit.SelectionStart > 0))
						if (txtEdit.SelectionStart > 0)
						{
							//	if (txtEdit.SelectionStart == 0)
							//	{
							// Debug.WriteLine("Move left");
							// this part is broken by the word wrapping
							// txtEdit_KeyDown(txtEdit, new KeyEventArgs(Keys.Left));
							//	}
							//	else
							//	{
							// Debug.WriteLine("Force cursor left");
							// using keydown doesn't seem to work here
							txtEdit.SelectionStart -= 1;
							//	}
							//	now moved outside of this block level
							//	txtEdit_KeyDown(txtEdit, new KeyEventArgs(Keys.Delete));
						}
						else if (m_currCol > 0)
						{
							// implement the old [LEFT] movement ourselves.
							Label newCurrLbl = ((DocWords)m_docWords[m_currCol - 1]).Cells[m_currRow];
							ZoomToNewCursor(newCurrLbl);
							Point newPt = pnlWords.PointToScreen(
								new Point(newCurrLbl.Right - 2, newCurrLbl.Top - 2));
							Point oldPos = MousePosition;
							SetCursorPos(newPt.X, newPt.Y);
							lbl_Click(newCurrLbl, null);
							txtEdit.SelectionStart = txtEdit.Text.Length;
							SetCursorPos(oldPos.X, oldPos.Y);
						}
						else // we don't want to do anything if we're in word 0
							return;
					}
					//	else if (txtEdit.SelectionLength > 0)
					//	{
						//	txtEdit_KeyDown(txtEdit, new KeyEventArgs(Keys.Delete));
						//	return;
					//	}
					// since we always end at [DELETE], we only need to do
					// movement if we have nothing selected, then call
					// the Event handler for [DELETE] before we return
					txtEdit_KeyDown(txtEdit, new KeyEventArgs(Keys.Delete));
					return;
				case 13:
					if (m_ShowHeaders < 2)
					{
						m_ShowHeaders++;
					}
					else
						m_ShowHeaders = 0;
					pnlHeaders.Visible = (m_ShowHeaders != 0);
					LayoutHeaders();
					return;
				case 32:
					/// if ortho and below, treat as normal typing
					/// else split cell or create new column. if we're
					/// at the beginning of a cell, just push the stuff
					/// into the next cell, and don't leave a copy behind.
					if (m_currRow < OrthoIndex)
					{
						string LeftString = txtEdit.Text.Substring(0, txtEdit.SelectionStart);
						string RightString = txtEdit.Text.Substring(txtEdit.SelectionStart);
						txtEdit.Text = LeftString;
						if ((m_currCol == m_docWords.Count - 1) ||
							(((DocWords)m_docWords[m_currCol + 1]).Cells[m_currRow].Text.Trim() != ""))
						{
							DocWords dw = new DocWords();
							foreach (Label lbl in dw.Cells)
							{
								lbl.Text = "";
							}
							dw.Cells[m_currRow].Text = RightString;
							m_docWords.Insert(m_currCol + 1, dw);
						}
						else
						{
							((DocWords)m_docWords[m_currCol + 1]).Cells[m_currRow].Text = RightString;
						}
						LayoutLabels();
						Label nextLabel = ((DocWords)m_docWords[m_currCol + 1]).Cells[m_currRow];
						Point OldMousePos = MousePosition;
						Point NewMousePos = pnlWords.PointToScreen(nextLabel.Location);
						SetCursorPos(NewMousePos.X, NewMousePos.Y);
						lbl_Click(nextLabel, null);
						SetCursorPos(OldMousePos.X, OldMousePos.Y);
						e.Handled = true;
						return;
					}
					return;
				default:
					m_currLabel.Text = txtEdit.Text;
					if (m_currLabel.Text == "") m_currLabel.Text = " ";
					LayoutLabels();
					SetupScrollBars();
					txtEdit.Width = m_currLabel.Width;
					return;
			}
		}

		/// <summary>
		/// Handles up, down, left, right, home, end and delete keys.
		/// Up/Down: movement within a column.
		/// Left/Right: movement along a row, or from beginning/end of one
		/// row to end/beginning of previous/next.
		/// Home/End: movement to beginning/end of current row.
		/// Delete: join next cell's contents to the current one.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtEdit_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Modifiers != 0) return;
			else
			{
				Label newCurrLbl;
				Point newPt;
				/// saves the old mouse position so we can move it back when
				/// we are done clicking on the new label.
				Point oldPos;
				switch (e.KeyCode)
				{
					case Keys.Up:
						e.Handled = true;
						newCurrLbl = GetUpCell();
						if (newCurrLbl == null)
							return;
						ZoomToNewCursor(newCurrLbl);
						newPt = txtEdit.PointToScreen(new Point(
							SendMessage(txtEdit.Handle,
							EM_POSFROMCHAR, txtEdit.SelectionStart, 0)));
						newPt.Y += 3;
						oldPos = MousePosition;
						SetCursorPos(newPt.X, newPt.Y);
						lbl_Click(newCurrLbl, null);
						SetCursorPos(oldPos.X, oldPos.Y);
						while ((txtEdit.PointToScreen(new Point(
							SendMessage(txtEdit.Handle, EM_POSFROMCHAR,
							txtEdit.SelectionStart, 0))).X < newPt.X) &&
							(txtEdit.SelectionStart < txtEdit.Text.Length))
							txtEdit.SelectionStart++;
						return;
					case Keys.Down:
						e.Handled = true;
						newCurrLbl = GetDownCell();
						if (newCurrLbl == null)
							return;
						ZoomToNewCursor(newCurrLbl);
						newPt = txtEdit.PointToScreen(new Point(
							SendMessage(txtEdit.Handle,
							EM_POSFROMCHAR, txtEdit.SelectionStart, 0)));
						newPt.Y += 3;
						oldPos = MousePosition;
						SetCursorPos(newPt.X, newPt.Y);
						lbl_Click(newCurrLbl, null);
						SetCursorPos(oldPos.X, oldPos.Y);
						while ((txtEdit.PointToScreen(new Point(
							SendMessage(txtEdit.Handle, EM_POSFROMCHAR,
							txtEdit.SelectionStart, 0))).X < newPt.X) &&
							(txtEdit.SelectionStart < txtEdit.Text.Length))
							txtEdit.SelectionStart++;
						return;
					case Keys.Left:
						if (txtEdit.SelectionStart == 0)
						{
							if ((m_currRow > 0) || (m_currCol > 0))
							{
								e.Handled = true;
								newCurrLbl = GetLeftCell();
								ZoomToNewCursor(newCurrLbl);
								newPt = pnlWords.PointToScreen(
									new Point(newCurrLbl.Right - 2, newCurrLbl.Top - 2));
								oldPos = MousePosition;
								SetCursorPos(newPt.X, newPt.Y);
								lbl_Click(newCurrLbl, null);
								txtEdit.SelectionStart = txtEdit.Text.Length;
								SetCursorPos(oldPos.X, oldPos.Y);
							}
						}
						return;
					case Keys.Right:
						if (txtEdit.SelectionStart == txtEdit.Text.Length)
						{
							if ((m_currRow < ((DocWords)m_docWords[0]).Cells.Length - 1) ||
								(m_currCol < m_docWords.Count - 1))
							{
								e.Handled = true;
								newCurrLbl = GetRightCell();
								ZoomToNewCursor(newCurrLbl);
								newPt = pnlWords.PointToScreen(newCurrLbl.Location);
								oldPos = MousePosition;
								SetCursorPos(newPt.X, newPt.Y);
								lbl_Click(newCurrLbl, null);
								SetCursorPos(oldPos.X, oldPos.Y);
							}
						}
						return;
					case Keys.Home:
						e.Handled = true;
						/// Go to the beginning
						//	newCurrLbl = ((DocWords)m_wordData[0]).Cells[m_currRow];
						newCurrLbl = GetStartCell();
						ZoomToNewCursor(newCurrLbl);
						newPt = pnlWords.PointToScreen(newCurrLbl.Location);
						oldPos = MousePosition;
						SetCursorPos(newPt.X, newPt.Y);
						lbl_Click(newCurrLbl, null);
						SetCursorPos(oldPos.X, oldPos.Y);
						return;
					case Keys.End:
						e.Handled = true;
						/// Go to the end
						//	newCurrLbl = ((DocWords)m_wordData[m_wordData.Count - 1]).Cells[m_currRow];
						newCurrLbl = GetEndCell();
						ZoomToNewCursor(newCurrLbl);
						newPt = pnlWords.PointToScreen(
							new Point(newCurrLbl.Right - 2, newCurrLbl.Top - 2));
						oldPos = MousePosition;
						SetCursorPos(newPt.X, newPt.Y);
						lbl_Click(newCurrLbl, null);
						SetCursorPos(oldPos.X, oldPos.Y);
						txtEdit.SelectionStart = txtEdit.Text.Length;
						return;
					case Keys.Delete:
						e.Handled = true;
						string newText = "";
						int newSelStart;
						if (txtEdit.SelectionStart == txtEdit.Text.Length)
						{
							if (m_currCol == m_docWords.Count - 1) return;
							newSelStart = txtEdit.SelectionStart;
							Label lbl = ((DocWords)m_docWords[m_currCol + 1]).Cells[m_currRow];
							newText = txtEdit.Text + lbl.Text.Trim();
							lbl.Text = " ";
							if (ColEmpty(m_currCol + 1, false))
								DeleteCol(m_currCol + 1);
						}
						else if (txtEdit.SelectionLength == txtEdit.Text.Length)
						{
							newText = "";
							newSelStart = 0;
						}
						else if (txtEdit.SelectionLength > 0)
						{
							if (txtEdit.SelectionStart > 0)
								newText = txtEdit.Text.Substring(0,
									txtEdit.SelectionStart);
							newText += txtEdit.Text.Substring(txtEdit.SelectionStart +
								txtEdit.SelectionLength);
							newSelStart = txtEdit.SelectionStart;
						}
						else
						{
							if (txtEdit.SelectionStart > 0)
								newText = txtEdit.Text.Substring(0,
									txtEdit.SelectionStart);
							newText += txtEdit.Text.Substring(txtEdit.SelectionStart + 1);
							newSelStart = txtEdit.SelectionStart;
						}
						ProcessNewText(newText, newSelStart);
						LayoutLabels();
						txtEdit.Left = m_currLabel.Left;
						txtEdit.Width = m_currLabel.Width;
						return;
					default:
						return;
				}
			}
		}

		private void lbl_Click(object sender, EventArgs e)
		{
			if (m_currLabel != null)
			{
				m_currLabel.Text = txtEdit.Text;
				if (m_currLabel.Text == "") m_currLabel.Text = " ";
				txtEdit.Text = "";
				LayoutLabels();
			}
			Label lbl = sender as Label;

			for (int i = 0; i < m_docWords.Count; i++)
			{
				for (int j = 0; j < 7 ; j++)
					if (lbl == ((DocWords)m_docWords[i]).Cells[j])
					{
						m_currCol = i;
						m_currRow = j;
					}
			}

			txtEdit.Location = lbl.Location;
			txtEdit.Width = ((DocWords)m_docWords[m_currCol]).ColWidth;
			txtEdit.Visible = true;
			txtEdit.Font = lbl.Font;
			txtEdit.Text = lbl.Text.Trim();
			txtEdit.BringToFront();
			txtEdit.Select();
			Point pt = txtEdit.PointToClient(MousePosition);
			/// This useful little function allows us to get the caret
			/// in the right position (i.e. where you click) without
			/// the legwork associated with the vb6 version. of course,
			/// this also means that it won't be portable unless an
			/// equivalent mechanism exists in linux, or elsewhere,
			/// since SendMessage comes from user32.dll.
			/// UPDATE: this apparently doesn't work when lbl_Click is
			/// called by txtEdit_KeyDown (why?)
			int sel = SendMessage(txtEdit.Handle,
				EM_CHARFROMPOS, 0,
				(pt.X & 0xFFFF) + ((pt.Y & 0xFFFF)<< 16));
			if (sel > -1) txtEdit.SelectionStart = sel;
			m_currLabel = lbl;
		}

		private void hScrollBar1_ValueChanged(object sender, System.EventArgs e)
		{
			pnlWords.Left = (pnlHeaders.Visible ? pnlHeaders.Width : 0) - hScrollBar1.Value;
		}

		private void vScrollBar1_ValueChanged(object sender, System.EventArgs e)
		{
			pnlWords.Top = pnlHeaders.Top = -vScrollBar1.Value;
		}
		#endregion
	}
}
