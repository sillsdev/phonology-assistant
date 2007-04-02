using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.FFSearchEngine;
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

		private const char kEmptyPatternChar = '\u25CA';
		private bool m_allowFullSearchPattern = false;
		private bool m_ignoreTextChange = false;
		private SearchQuery m_searchQuery;
		private SearchOptionsDropDown m_searchOptionsDropDown;
		private string m_srchQryCategory;
		private Form m_owningForm;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternTextBox()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			InitializeComponent();

			if (PaApp.DesignMode)
				return;

			txtPattern.OwningPatternTextBoxControl = this;
			m_searchOptionsDropDown = new SearchOptionsDropDown();
			BackColor = Color.Transparent;
			txtPattern.Font = FontHelper.PhoneticFont;
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
			if (query == m_searchQuery)
				return;

			if (query == null)
				Clear();
			else
			{
				m_searchQuery = query.Clone();
				txtPattern.Text = (string.IsNullOrEmpty(query.Pattern) ?
					EmptyPattern : query.Pattern);
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
		/// Indicates whether or not the user may enter the '/' and '_' to indicate a complete
		/// find phone search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllowFullSearchPattern
		{
			get { return m_allowFullSearchPattern; }
			set
			{
				m_allowFullSearchPattern = value;

				if (value && txtPattern.Text == string.Empty)
				{
					txtPattern.Text = EmptyPattern;
					txtPattern.SelectionStart = 0;
					txtPattern.SelectionLength = 0;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the EmptyPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string EmptyPattern
		{
			get
			{
				return (m_allowFullSearchPattern && PaApp.ShowEmptyDiamondSearchPattern ?
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
			Height = txtPattern.Height + 10;
			LocateArrows();
		}

		#endregion

		#region Text Box events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_TextChanged(object sender, EventArgs e)
		{
			if (m_ignoreTextChange)
				return;

			int selstart = txtPattern.SelectionStart;
			int sellen = txtPattern.SelectionLength;

			// Force all consonant class to be uppercase.
			if (txtPattern.Text.IndexOf("[c]") >= 0)
				txtPattern.Text = txtPattern.Text.Replace("[c]", "[C]");

			// Force all vowel class to be uppercase.
			if (txtPattern.Text.IndexOf("[v]") >= 0)
				txtPattern.Text = txtPattern.Text.Replace("[v]", "[V]");

			txtPattern.SelectionStart = selstart;
			txtPattern.SelectionLength = sellen;

			if (!m_allowFullSearchPattern)
			{
				// Since the Keypress event will prevent '/', '_', '<' and '>'
				// from being entered, this check is here only for the case when
				// the user pastes those characters into the text box.
				if (txtPattern.Text.IndexOf("/") >= 0)
					txtPattern.Text = txtPattern.Text.Replace("/", string.Empty);

				if (txtPattern.Text.IndexOf("_") >= 0)
					txtPattern.Text = txtPattern.Text.Replace("_", string.Empty);

				if (txtPattern.Text.IndexOf("<") >= 0)
					txtPattern.Text = txtPattern.Text.Replace("<", string.Empty);

				if (txtPattern.Text.IndexOf(">") >= 0)
					txtPattern.Text = txtPattern.Text.Replace(">", string.Empty);

				if (selstart <= txtPattern.Text.Length)
					txtPattern.SelectionStart = selstart;
			}

			if (m_searchQuery.Pattern != txtPattern.Text)
			{
				m_searchQuery.Pattern = txtPattern.Text;
				if (SearchQueryChanged != null)
					SearchQueryChanged(this, EventArgs.Empty);
			}
			
			LocateArrows();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Let Ctrl-C takes its normal course.
			if (e.KeyChar == (char)3 && (Control.ModifierKeys & Keys.Control) == Keys.Control)
				return;
			
			char nextChar;

			// When the text box is not to contain a complete search pattern, then ignore
			// characters that are only valid for complete search patterns.
			if (!m_allowFullSearchPattern && ("/_<>" + kEmptyPatternChar).Contains(e.KeyChar.ToString()))
			{
				System.Media.SystemSounds.Beep.Play();
				e.KeyChar = (char)0;
				e.Handled = true;
				return;
			}

			// Process enter as though the user wants to begin a search.
			if (e.KeyChar == (char)Keys.Enter && IsPatternFull)
			{
				PaApp.MsgMediator.SendMessage("EnterPressedInSearchPatternTextBox", SearchQuery);
				e.Handled = true;
				return;
			}

			int selStart = txtPattern.SelectionStart;

			// Remove any selected text.
			if (txtPattern.SelectionLength > 0)
			{
				txtPattern.Text = txtPattern.Text.Remove(selStart, txtPattern.SelectionLength);
				txtPattern.SelectionStart = selStart;
			}

			// Cause a space to jump over adjacent close brackets, slash or underline.
			if (e.KeyChar == (char)Keys.Space && selStart < txtPattern.Text.Length)
			{
				nextChar = txtPattern.Text[selStart];
				if (("]}/_" + kEmptyPatternChar).Contains(nextChar.ToString()))
				{
					txtPattern.SelectionStart++;
					e.KeyChar = (char)0;
					e.Handled = true;
					return;
				}
			}

			// If the previous character is a diamond get rid of it first.
			char prevChar = (selStart > 0 ? txtPattern.Text[selStart - 1] : (char)0);
			if (prevChar == kEmptyPatternChar && e.KeyChar != (char)Keys.Space)
			{
				txtPattern.Text = txtPattern.Text.Remove(selStart - 1, 1);
				txtPattern.SelectionStart = --selStart;
			}

			// If the next character is a diamond get rid of it first.
			nextChar = (selStart < txtPattern.Text.Length ? txtPattern.Text[selStart] : (char)0);
			if (nextChar == kEmptyPatternChar)
			{
				txtPattern.Text = txtPattern.Text.Remove(selStart, 1);
				txtPattern.SelectionStart = selStart;
			}
			else if ((nextChar == '/' && e.KeyChar == '/') || (nextChar == '_' && e.KeyChar == '_'))
			{
				e.KeyChar = (char)0;
				e.Handled = true;
				txtPattern.SelectionStart++;
				return;
			}

			// Automatically insert a closing brace or bracket
			// when the user enters an opening counterpart.
			if (e.KeyChar == '[' || e.KeyChar == '{')
			{
				m_ignoreTextChange = true;
				txtPattern.Text = txtPattern.Text.Insert(selStart, (e.KeyChar == '[' ? "]" : "}"));
				txtPattern.SelectionStart = selStart;
				m_ignoreTextChange = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Do this on the up stroke since the insertion point will have been moved by that
		/// time, thus causing the arrows to be placed in the correct location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_KeyUp(object sender, KeyEventArgs e)
		{
			LocateArrows();
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
				// What's being dragged is not appropriate to be dropped in a search
				// pattern, therefore, indicate that dropping not allowed.
				e.Effect = DragDropEffects.None;
				return;
			}

			if (!txtPattern.Focused)
			{
				txtPattern.Focus();

				// Make these visible during dragging.
				lblDown.Visible = lblUp.Visible = true;
			}

			Point pt = txtPattern.PointToClient(new Point(e.X, e.Y));
			if (pt.X >= 0)
			{
				txtPattern.SelectionStart = (pt.X >= EndOfTextLocation.X ?
					txtPattern.Text.Length : txtPattern.GetCharIndexFromPosition(pt));
			}

			LocateArrows();
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
			lblDown.Visible = lblUp.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_Click(object sender, EventArgs e)
		{
			LocateArrows();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_Enter(object sender, EventArgs e)
		{
			LocateArrows();
			lblDown.Visible = lblUp.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_Leave(object sender, EventArgs e)
		{
			lblDown.Visible = lblUp.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtPattern_SizeChanged(object sender, EventArgs e)
		{
			Height = txtPattern.Height + 10;
			LocateArrows();
		}
		
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Aligns the arrows with where the insertion point.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LocateArrows()
		{
			// Get the IP's location and make it relative to the
			// user control rather than relative to the text box.
			Point pt = InsertionPointLocation;
			pt = txtPattern.PointToScreen(pt);
			pt = PointToClient(pt);

			// Calculate where the arrow labels should be
			// so they're centered over and under the IP.
			pt.X -= (lblDown.Width / 2);

			// Move the arrows if necessary.
			if (lblDown.Left != pt.X)
				lblUp.Left = lblDown.Left = pt.X;

			if (lblUp.Top != txtPattern.Bottom - 5)
				lblUp.Top = txtPattern.Bottom - 5;

			if (lblDown.Top != -5)
				lblDown.Top = -5;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clear the pattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			m_ignoreTextChange = true;
			txtPattern.Text = EmptyPattern;
			m_searchQuery = new SearchQuery();
			m_srchQryCategory = null;
			m_ignoreTextChange = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts the specified text in the current insertion point in the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Insert(string text)
		{
			if (string.IsNullOrEmpty(text) /*|| (text == "#" && !m_allowFullSearchPattern)*/)
				return;

			int selstart = txtPattern.SelectionStart;
			string newText = txtPattern.Text.Trim();

			// Make the arrows invisible if they're not already since changing the
			// text in the text box will make them jump around. It's unsightly.
			bool arrowsWereVisible = lblDown.Visible;
			lblDown.Visible = lblUp.Visible = false;

			// First, remove any selected text.
			if (txtPattern.SelectionLength > 0)
				newText = newText.Remove(selstart, txtPattern.SelectionLength);

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
			txtPattern.Text = newText;
			txtPattern.SelectionStart = selstart + (text == "{}" || text == "[]"
				|| text == DataUtils.kDiacriticPlaceholder ? text.Length - 1 : text.Length);

			Application.DoEvents();
			LocateArrows();

			lblDown.Visible = lblUp.Visible = arrowsWereVisible;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure text with a dotted circle is not being inserted next to another and if
		/// not, make sure it is surrounded by square brackets.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ProcessTextWithDottedCircle(string newText, int selstart, ref string text)
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
			if (dottedCircle < 0)
				text = "[" + text + "]";
			else
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
			LocateArrows();

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
			if (!PaApp.IsFormActive(m_owningForm))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = !IsPatternEmpty;
			itemProps.Update = true;
			return true;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
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

			Microsoft.VisualBasic.Devices.Keyboard kb = new Microsoft.VisualBasic.Devices.Keyboard();

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