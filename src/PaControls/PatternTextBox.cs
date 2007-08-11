using System;
using System.ComponentModel;
using System.Drawing;
//using System.Media;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic.Devices;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Data;
using SIL.Pa.FFSearchEngine;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Defines a text box control for entering find phone search patterns.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PatternTextBox : UserControl, IxCoreColleague
	{
		public event EventHandler SearchQueryChanged;

		//private bool m_allowFullSearchPattern = false;
		//private bool m_ignoreTextChange = false;
		private bool m_classDisplayBehaviorChanged = false;
		private bool m_ignoreResize = false;
		private bool m_showArrows = true;
		private string m_srchQryCategory;
		private SearchQuery m_searchQuery;
		private Form m_owningForm;
		private readonly Image m_downArrow;
		private readonly Image m_upArrow;
		private readonly SearchOptionsDropDown m_searchOptionsDropDown;
		private static readonly char kEmptyPatternChar = '\u25CA';

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternTextBox()
		{
			m_downArrow = Properties.Resources.kimidPatternTextBoxDownArrow;
			m_upArrow = Properties.Resources.kimidPatternTextBoxUpArrow;

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			InitializeComponent();

			if (PaApp.DesignMode)
				return;

			txtPattern.OwningPatternTextBoxControl = this;
			txtPattern.TextChanged += txtPatternTextChanged;
			txtPattern.KeyPress += txtPatternKeyPress;
			txtPattern.Font = FontHelper.PhoneticFont;
			txtPattern.Tag = this;
		
			m_searchOptionsDropDown = new SearchOptionsDropDown();
			base.BackColor = Color.Transparent;
			m_searchQuery = new SearchQuery();
			PaApp.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set's the pattern text box's search query, cloning it if specified by the
		/// cloneQuery flag.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetSearchQuery(SearchQuery query)
		{
			if (query == m_searchQuery || (query != null && query.IsPatternRegExpression))
				return;

			if (query == null)
				Clear();
			else
			{
				txtPattern.Text = (string.IsNullOrEmpty(query.Pattern) ?
					EmptyPattern : query.Pattern);
				
				m_searchQuery = query.Clone();
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the search options drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SearchOptionsDropDown SearchOptionsDropDown
		{
			get { return m_searchOptionsDropDown; }
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current search query. Use SetSearchQuery to set the current search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SearchQuery SearchQuery
		{
			get { return m_searchQuery; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the pattern text box's owning form.
		/// </summary>s
		/// ------------------------------------------------------------------------------------
		public Form OwningForm
		{
			get { return m_owningForm; }
			set { m_owningForm = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SearchQueryCategory
		{
			get { return m_srchQryCategory; }
			set { m_srchQryCategory = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ClassDisplayBehaviorChanged
		{
			get { return m_classDisplayBehaviorChanged; }
			set { m_classDisplayBehaviorChanged = value; }
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Indicates whether or not the user may enter the '/' and '_' to indicate a complete
		///// find phone search pattern.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public bool AllowFullSearchPattern
		//{
		//    get { return m_allowFullSearchPattern; }
		//    set
		//    {
		//        m_allowFullSearchPattern = value;

		//        if (value && txtPattern.Text == string.Empty)
		//        {
		//            txtPattern.Text = EmptyPattern;
		//            txtPattern.SelectionStart = 0;
		//            txtPattern.SelectionLength = 0;
		//        }
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the EmptyPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string EmptyPattern
		{
			get
			{
				return (/* m_allowFullSearchPattern && */ !PaApp.DesignMode &&
					(PaApp.Project == null || PaApp.Project.ShowClassNamesInSearchPatterns) ?
					DataUtils.kEmptyDiamondPattern : string.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public TextBox TextBox
		{
			get { return txtPattern; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the location of where the insertion point should be in the text box. The
		/// point returned is relative to the text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Point InsertionPointLocation
		{
			get
			{
				int selStart = txtPattern.SelectionStart;

				if (txtPattern.Text == string.Empty || txtPattern.SelectionLength > 0)
					return new Point(1, 0);

				return (selStart < txtPattern.Text.Length ?
					txtPattern.GetPositionFromCharIndex(selStart) :
					EndOfTextLocation);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the point where the IP would be if it were right after the end of the text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Point EndOfTextLocation
		{
			get
			{
				int textLen = txtPattern.Text.Length;

				if (textLen == 0)
					return new Point(0, 0);

				// Get the location of the last character.
				Point ptLastChar = txtPattern.GetPositionFromCharIndex(textLen - 1);

				using (Graphics g = txtPattern.CreateGraphics())
				{
					string lastChar = txtPattern.Text.Substring(textLen - 1);

					// Measure the width of the last character;
					int lastCharWidth = TextRenderer.MeasureText(g, lastChar,
						txtPattern.Font, Size.Empty, TextFormatFlags.NoPadding |
						TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine).Width;

					return new Point(ptLastChar.X + lastCharWidth, 0);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the pattern is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsPatternEmpty
		{
			get { return (txtPattern.Text == EmptyPattern); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the pattern is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsPatternFull
		{
			get
			{
				string text = txtPattern.Text.Trim();

				// TODO: should I care if the / and _ have stuff around them?
				return (text.IndexOf(kEmptyPatternChar) < 0 &&
					text.IndexOf('/') >= 0 && text.IndexOf('_') >= 0);
			}
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove ourselves from the colleague list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			PaApp.RemoveMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure this control's height is just enough larger than the text box's height
		/// to accomodate the arrow above and below the text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			txtPattern_SizeChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the arrows.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!m_showArrows)
				return;

			Point pt = InsertionPointLocation;
			pt = txtPattern.PointToScreen(pt);
			pt = PointToClient(pt);

			// Calculate where the arrow labels should be
			// so they're centered over and under the IP.
			pt.X -= (m_downArrow.Width / 2);
			pt.Y = 0;

			Rectangle rc = new Rectangle(pt, new Size(m_upArrow.Width, m_upArrow.Height));

			e.Graphics.DrawImageUnscaledAndClipped(m_downArrow, rc);

			rc.Y = txtPattern.Bottom;
			e.Graphics.DrawImageUnscaledAndClipped(m_upArrow, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The width of the text box should be a little less than the control's width.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (m_ignoreResize)
				return;

			txtPattern.Width = Width - (Padding.Left * 2);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clear the pattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			//m_ignoreTextChange = true;
			txtPattern.TextChanged -= txtPatternTextChanged;
			txtPattern.Text = EmptyPattern;
			m_searchQuery = new SearchQuery();
			m_srchQryCategory = null;
			txtPattern.TextChanged += txtPatternTextChanged;
			//m_ignoreTextChange = false;
		}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts the specified text in the current insertion point in the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Insert(string text)
		{
			Insert(txtPattern, text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts the specified text in the current insertion point in the specified text
		/// box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Insert(TextBox txt, string text)
		{
			if (txt == null || string.IsNullOrEmpty(text) /*|| (text == "#" && !m_allowFullSearchPattern)*/)
				return;

			int selstart = txt.SelectionStart;
			string newText = txt.Text.Trim();
			if (selstart >= newText.Length)
				selstart = newText.Length;

			// First, remove any selected text.
			if (txt.SelectionLength > 0)
				newText = newText.Remove(selstart, txt.SelectionLength);

			//// When inserting a word boundary character, move the insertion
			//// point to the nearest valid location for a word boundary.
			//// TODO: Add some checks to make sure more than 2 "#" aren't inserted.
			//if (text == "#")
			//{
			//    int i = newText.IndexOf("/");
			//    if (i < 0)
			//        selstart = newText.Length;
			//    else
			//    {
			//        i++;
			//        if (Math.Abs(selstart - newText.Length) < Math.Abs(i - selstart))
			//            selstart = newText.Length;
			//        else
			//            selstart = i;
			//    }
			//}
			
			// Now remove any diamond that's next to the insertion point.
			if (newText.Length > 0)
			{
				if (selstart < newText.Length && newText[selstart] == kEmptyPatternChar)
					newText = newText.Remove(selstart, 1);
				else if (selstart > 0 && newText[selstart - 1] == kEmptyPatternChar)
				{
					selstart--;
					newText = newText.Remove(selstart, 1);
				}
			}

			// If the new text contains a dotted circle diacritic placeholder, then make
			// sure it's not being inserted next to another and if not, make sure it is
			// surrounded by square brackets.
			if (text.IndexOf(DataUtils.kDottedCircleC) >= 0)
				ProcessTextWithDottedCircle(newText, selstart, ref text);

			newText = newText.Insert(selstart, text);
			txt.Text = newText;
			txt.SelectionStart = selstart + (text == "{}" || text == "[]"
				|| text == DataUtils.kDiacriticPlaceholder ? text.Length - 1 : text.Length);

			Application.DoEvents();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure text with a dotted circle is not being inserted next to another and if
		/// not, make sure it is surrounded by square brackets.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ProcessTextWithDottedCircle(string newText, int selstart, ref string text)
		{
			// Strip out the dotted circle and check if what's left is a single tie-bar-
			// type character. If so, then just return the text without the dotted circle.
			string nonDottedCirclePart = text.Replace(DataUtils.kDottedCircle, string.Empty);
			if (nonDottedCirclePart.Length == 1)
			{
				IPACharInfo charInfo = DataUtils.IPACharCache[nonDottedCirclePart];
				if (charInfo != null && charInfo.CanPreceedBaseChar)
				{
					text = nonDottedCirclePart;
					return;
				}
			}
			
			int dottedCircle = -1;

			// Go backward, looking for a dotted circle or open/close brackets.
			for (int i = selstart; i >= 0 && dottedCircle < 0; i--)
			{
				if (i < newText.Length)
				{
					if (newText[i] == DataUtils.kDottedCircleC)
						dottedCircle = i;
					else if (newText[i] == '[' || (newText[i] == ']' && i < selstart))
						break;
				}
			}

			// Go forward, looking for a dotted circle or open/close brackets.
			for (int i = selstart; i < newText.Length && newText.Length > 0 && dottedCircle < 0; i++)
			{
				if (newText[i] == DataUtils.kDottedCircleC)
					dottedCircle = i;
				else if (newText[i] == ']' || newText[i] == '[')
					break;
			}

			// Did we find a dotted circle. If not, then surround the
			// dotted circle and its diacritic in square brackets.
			if (dottedCircle >= 0)
			{
				// Remove the dotted circle.
				int i = text.IndexOf(DataUtils.kDottedCircleC);
				text = text.Substring(0, i) + text.Substring(i + 1);
			}
		}

		#region Message handlers for Inserting
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertConsonant(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert("[C]");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertVowel(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert("[V]");
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertZeroOrMore(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert("*");
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertOneOrMore(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert("+");
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertWordBoundary(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert("#");
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertDiacriticPlaceholder(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert(DataUtils.kDiacriticPlaceholder);
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertSyllableBoundary(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert(".");
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertANDGroup(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert("[]");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertORGroup(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			Insert("{}");
			return true;
		}

		#endregion

		#region Message handlers for non inserting actions
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownSearchOptions(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			m_searchOptionsDropDown.SearchQuery = m_searchQuery;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedSearchOptions(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			if (m_searchOptionsDropDown.OptionsChanged)
			{
				m_searchQuery = m_searchOptionsDropDown.SearchQuery;
				if (SearchQueryChanged != null)
					SearchQueryChanged(this, EventArgs.Empty);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verify the validity of the current search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnVerifyPattern(object args)
		{
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			txtPattern.Font = FontHelper.PhoneticFont;
			Invalidate();

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnFindPhonesSettingsChanged(object args)
		{
			if (txtPattern.Text == DataUtils.kEmptyDiamondPattern ||
				txtPattern.Text == string.Empty)
				txtPattern.Text = EmptyPattern;
			txtPattern.SelectionStart = 0;
			txtPattern.SelectionLength = 0;
			return true;
		}
		#endregion

		#region Update handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the Class Display Behavior has been changed by the user.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnClassDisplayBehaviorChanged(object args)
		{
			string replacedText = PaApp.Project.SearchClasses.ModifyPatternText(TextBox.Text);
			if (replacedText != string.Empty)
			{
				
				m_classDisplayBehaviorChanged = true;
				TextBox.Text = replacedText;
				m_classDisplayBehaviorChanged = false;
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateBeginSearch(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!PaApp.IsFormActive(m_owningForm) || itemProps == null ||
				itemProps.Name.StartsWith("cmnu"))
			{
				return false;
			}

			if (itemProps.Enabled != IsPatternFull)
			{
				itemProps.Visible = true;
				itemProps.Enabled = IsPatternFull;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSavePattern(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!PaApp.IsFormActive(m_owningForm) || itemProps == null)
				return false;

			if (itemProps.Enabled != IsPatternFull)
			{
				itemProps.Visible = true;
				itemProps.Enabled = IsPatternFull;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateVerifyPattern(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !PaApp.IsFormActive(m_owningForm))
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = !IsPatternEmpty;
			itemProps.Update = true;
			return true;
		}

		#endregion

		#region Text Box events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This control should size itself to be a little larger than the text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_SizeChanged(object sender, EventArgs e)
		{
			m_ignoreResize = true;
			Height = txtPattern.Height + m_upArrow.Height + m_downArrow.Height;
			m_ignoreResize = false;
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void txtPattern_LocationChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Do this on the up stroke since the insertion point will have been moved by that
		/// time, thus causing the arrows to be placed in the correct location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_KeyUp(object sender, KeyEventArgs e)
		{
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_DragOver(object sender, DragEventArgs e)
		{
			SearchQuery data =
				e.Data.GetData(typeof(SearchQuery)) as SearchQuery;

			if (data == null)
			{
				// Check if the data is plain text.
				string pattern = e.Data.GetData(typeof(string)) as string;
				if (pattern == null)
				{
					// What's being dragged is not appropriate to be dropped in a search
					// pattern, therefore, indicate that dropping not allowed.
					e.Effect = DragDropEffects.None;
					return;
				}
			}

			if (!txtPattern.Focused)
				txtPattern.Focus();

			Point pt = txtPattern.PointToClient(new Point(e.X, e.Y));
			if (pt.X >= 0)
			{
				txtPattern.SelectionStart = (pt.X >= EndOfTextLocation.X ?
					txtPattern.Text.Length : txtPattern.GetCharIndexFromPosition(pt));
			}

			// Make these visible during dragging.
			m_showArrows = true;
			Invalidate();

			e.Effect = DragDropEffects.Copy;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_DragDrop(object sender, DragEventArgs e)
		{
			SearchQuery query =
				e.Data.GetData(typeof(SearchQuery)) as SearchQuery;

			// Is what was dropped appropriate to be dropped in a search pattern?
			if (query == null)
			{
				// See if the data is plain text.
				string pattern = e.Data.GetData(typeof(string)) as string;
				if (pattern != null)
					query = new SearchQuery(pattern);
			}

			if (query != null)
			{
				if (query.PatternOnly)
					Insert(query.Pattern);
				else
				{
					// A full pattern was dropped so first clear out any pattern in the text box.
					SetSearchQuery(query);
				}
			}

			// After dropping, we know we have focus so make sure the arrows
			// aren't visible now that dropping is done.
			m_showArrows = false;
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_Click(object sender, EventArgs e)
		{
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_Enter(object sender, EventArgs e)
		{
			m_showArrows = false;
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_Leave(object sender, EventArgs e)
		{
			m_showArrows = true;
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void txtPatternTextChanged(object sender, EventArgs e)
		{
			TextBox txt = sender as TextBox;
			PatternTextBox ptrTextBox = (txt != null ? txt.Tag as PatternTextBox : null);

			if (txt == null /* || m_ignoreTextChange */)
				return;

			int selstart = txt.SelectionStart;
			int sellen = txt.SelectionLength;

			// Force all consonant class to be uppercase.
			if (txt.Text.IndexOf("[c]") >= 0)
				txt.Text = txt.Text.Replace("[c]", "[C]");

			// Force all vowel class to be uppercase.
			if (txt.Text.IndexOf("[v]") >= 0)
				txt.Text = txt.Text.Replace("[v]", "[V]");

			txt.SelectionStart = selstart;
			txt.SelectionLength = sellen;

			//if (!m_allowFullSearchPattern)
			//{
			//    // Since the Keypress event will prevent '/', '_', '<' and '>'
			//    // from being entered, this check is here only for the case when
			//    // the user pastes those characters into the text box.
			//    if (txtPattern.Text.IndexOf("/") >= 0)
			//        txtPattern.Text = txtPattern.Text.Replace("/", string.Empty);

			//    if (txtPattern.Text.IndexOf("_") >= 0)
			//        txtPattern.Text = txtPattern.Text.Replace("_", string.Empty);

			//    if (txtPattern.Text.IndexOf("<") >= 0)
			//        txtPattern.Text = txtPattern.Text.Replace("<", string.Empty);

			//    if (txtPattern.Text.IndexOf(">") >= 0)
			//        txtPattern.Text = txtPattern.Text.Replace(">", string.Empty);

			//    if (selstart <= txtPattern.Text.Length)
			//        txtPattern.SelectionStart = selstart;
			//}

			if (ptrTextBox != null)
			{
				if (ptrTextBox.m_searchQuery.Pattern != txt.Text)
				{
					ptrTextBox.m_searchQuery.Pattern = txt.Text;
					if (ptrTextBox.SearchQueryChanged != null)
						ptrTextBox.SearchQueryChanged(ptrTextBox, EventArgs.Empty);
				}

				ptrTextBox.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Ctrl-A: KeyChar = 1
		/// Ctrl-C: KeyChar = 3
		/// Ctrl-V: KeyChar = 22
		/// Ctrl-X: KeyChar = 24
		/// Ctrl-Y: KeyChar = 25
		/// Ctrl-Z: KeyChar = 26
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public static void txtPatternKeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox txt = sender as TextBox;
			PatternTextBox ptrTextBox = (txt != null ? txt.Tag as PatternTextBox : null);

			// Let ctrl sequences take their normal course.
			if (txt == null)
				return;

			if ((ModifierKeys & Keys.Control) == Keys.Control)
			{
				// Cause Ctrl-A to select all. It doesn't seem to do it by default.
				if (e.KeyChar == (char)1)
				{
					txt.SelectAll();
					e.Handled = true;
				}

				return;
			}

			char nextChar;

			// When the text box is not to contain a complete search pattern, then ignore
			// characters that are only valid for complete search patterns.
			// if (!m_allowFullSearchPattern && ("/_<>" + kEmptyPatternChar).Contains(e.KeyChar.ToString()))
			//{
			//    SystemSounds.Beep.Play();
			//    e.KeyChar = (char)0;
			//    e.Handled = true;
			//    return;
			//}

			// Process enter as though the user wants to begin a search.
			if (e.KeyChar == (char)Keys.Enter && ptrTextBox != null && ptrTextBox.IsPatternFull)
			{
				PaApp.MsgMediator.SendMessage("EnterPressedInSearchPatternTextBox", ptrTextBox.SearchQuery);
				e.Handled = true;
				return;
			}

			int selStart = txt.SelectionStart;

			// Remove any selected text.
			if (txt.SelectionLength > 0)
			{
				txt.Text = txt.Text.Remove(selStart, txt.SelectionLength);
				txt.SelectionStart = selStart;
			}

			// Cause a space to jump over adjacent close brackets, slash or underline.
			if (e.KeyChar == (char)Keys.Space && selStart < txt.Text.Length)
			{
				nextChar = txt.Text[selStart];
				if (("]}/_" + kEmptyPatternChar).Contains(nextChar.ToString()))
				{
					txt.SelectionStart++;
					e.KeyChar = (char)0;
					e.Handled = true;
					return;
				}
			}

			// If the previous character is a diamond get rid of it first.
			char prevChar = (selStart > 0 ? txt.Text[selStart - 1] : (char)0);
			if (prevChar == kEmptyPatternChar && e.KeyChar != (char)Keys.Space)
			{
				txt.Text = txt.Text.Remove(selStart - 1, 1);
				txt.SelectionStart = --selStart;
			}

			// If the next character is a diamond get rid of it first.
			nextChar = (selStart < txt.Text.Length ? txt.Text[selStart] : (char)0);
			if (nextChar == kEmptyPatternChar)
			{
				txt.Text = txt.Text.Remove(selStart, 1);
				txt.SelectionStart = selStart;
			}
			else if ((nextChar == '/' && e.KeyChar == '/') || (nextChar == '_' && e.KeyChar == '_'))
			{
				e.KeyChar = (char)0;
				e.Handled = true;
				txt.SelectionStart++;
				return;
			}

			// When 'C' is entered then automatically insert "[C]".
			// When 'V' is entered then automatically insert "[V]".
			// But only when the previous character is not '['.
			if (prevChar != '[' && (e.KeyChar == 'C' || e.KeyChar == 'V'))
			{
				txt.Text = txt.Text.Insert(selStart, ("[" + e.KeyChar + "]"));
				e.KeyChar = (char)0;
				e.Handled = true;
				txt.SelectionStart = selStart + 3;
				return;
			}

			//// Uncomment to automatically insert a closing brace or
			//// bracket when the user enters an opening counterpart.
			//if (e.KeyChar == '[' || e.KeyChar == '{')
			//{
			//    m_ignoreTextChange = true;
			//    txtPattern.Text = txtPattern.Text.Insert(selStart, (e.KeyChar == '[' ? "]" : "}"));
			//    txtPattern.SelectionStart = selStart;
			//    m_ignoreTextChange = false;
			//}
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}

	#region InternalPatternTextBox class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Subclassed for reasons stated below.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class InternalPatternTextBox : TextBox
	{
		private PatternTextBox m_ptrnTxtBox;

		public PatternTextBox OwningPatternTextBoxControl
		{
			get { return m_ptrnTxtBox; }
			set { m_ptrnTxtBox = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is the only reason we need to subclass the text box. This cannot be handled
		/// in the text box's KeyDown event because Ctrl+[ and Ctrl+] cause an incessant beep.
		/// Argh!
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override bool PreProcessMessage(ref Message m)
		{
			if (m.Msg != 0x0100)
				return base.PreProcessMessage(ref m);

			Keyboard kb = new Keyboard();

			// Handle some special cases when the Ctrl key is down. Except for Ctrl0, the
			// reason we handle {}{} and - (the dash is treated as an underscore) specially
			// is because the KeyMan IPA keyboard intercepts them for its purposes.
			if (!kb.CtrlKeyDown)
				return base.PreProcessMessage(ref m);

			string toInsert = null;
			int keyCode = m.WParam.ToInt32();

			if (keyCode == (int)Keys.OemCloseBrackets)
				toInsert = (kb.ShiftKeyDown ? "}" : "]");
			else if (keyCode == (int)Keys.OemOpenBrackets)
				toInsert = (kb.ShiftKeyDown ? "{" : "[");
			else if (keyCode == (int)Keys.OemMinus)
				toInsert = "_";
			else if (keyCode == (int)Keys.D0)
				toInsert = DataUtils.kDiacriticPlaceholder;

			if (toInsert != null)
			{
				m_ptrnTxtBox.Insert(toInsert);
				return true;
			}
		
			return base.PreProcessMessage(ref m);
		}
	}

	#endregion
}
