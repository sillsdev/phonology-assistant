using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Defines a text box control for entering find phone search patterns.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class PatternTextBox : TextBox, IxCoreColleague
    {
        public event EventHandler SearchQueryChanged;
        public event EventHandler PatternTextChanged;
        public event EventHandler SearchOptionsChanged;

        private const char kEmptyPatternChar = '\u25CA';
        private readonly Label _insertionLine;
        private SearchOptionsDropDown _searchOptionsDropDown;

        /// ------------------------------------------------------------------------------------
        public PatternTextBox()
        {
            HideSelection = false;
            AllowDrop = true;
            AcceptsReturn = true;
            DoubleBuffered = true;

            if (App.DesignMode)
                return;

            Font = App.PhoneticFont;
            TextChanged += HandlePatternTextBoxTextChanged;
            KeyPress += HandlePatternTextBoxKeyPress;

            // Use a thin label as the insertion point when the text box does not have focus.
            _insertionLine = new Label();
            _insertionLine.AutoSize = false;
            _insertionLine.BackColor = SystemColors.GrayText;
            _insertionLine.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom);
            _insertionLine.Bounds = new Rectangle(0, 1, 1, ClientSize.Height - 4);
            Controls.Add(_insertionLine);

            SearchQuery = new SearchQuery();
            App.AddMediatorColleague(this);
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
            get
            {
                if (_searchOptionsDropDown == null)
                {
                    _searchOptionsDropDown = new SearchOptionsDropDown();
                    _searchOptionsDropDown.Disposed += HandleSearchOptionsDropDownDisposed;
                }

                return _searchOptionsDropDown;
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// The only time this will be disposed before the program terminates is when the
        /// view is redocked after being undocked. That is because the toolbar/menu adapter
        /// is disposed and recreated when the view is being redocked. And when the TMAdapter
        /// is disposed, so are the custom controls it hosts in drop-downs.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleSearchOptionsDropDownDisposed(object sender, EventArgs e)
        {
            _searchOptionsDropDown.Disposed -= HandleSearchOptionsDropDownDisposed;
            _searchOptionsDropDown = null;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the current search query. Use SetSearchQuery to set the current search query.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SearchQuery SearchQuery { get; private set; }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the pattern text box's owning form.
        /// </summary>s
        /// ------------------------------------------------------------------------------------
        public ITabView OwningView { get; set; }

        /// ------------------------------------------------------------------------------------
        public string SearchQueryCategory { get; set; }

        /// ------------------------------------------------------------------------------------
        public bool ClassDisplayBehaviorChanged { get; set; }

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
                return (/* m_allowFullSearchPattern && */ !App.DesignMode &&
                    Settings.Default.ShowClassNamesInSearchPatterns ?
                    App.kEmptyDiamondPattern : string.Empty);
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
                int textLen = Text.Length;

                if (textLen == 0)
                    return new Point(0, 0);

                // Get the location of the last character.
                Point ptLastChar = GetPositionFromCharIndex(textLen - 1);

                using (Graphics g = CreateGraphics())
                {
                    string lastChar = Text.Substring(textLen - 1);

                    // Measure the width of the last character;
                    int lastCharWidth = TextRenderer.MeasureText(g, lastChar,
                        Font, Size.Empty, TextFormatFlags.NoPadding |
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
            get { return (Text == EmptyPattern); }
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
                string text = Text.Trim();

                // TODO: should I care if the / and _ have stuff around them?
                return (text.IndexOf(kEmptyPatternChar) < 0 &&
                    text.IndexOf('/') >= 0 && text.IndexOf('_') >= 0);
            }
        }

        #endregion

        #region Misc. methods
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Remove ourselves from the colleague list.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            App.RemoveMediatorColleague(this);
        }

        /// ------------------------------------------------------------------------------------
        private void SetInsertionLinePosition()
        {
            if (_insertionLine == null)
                return;

            int selStart = SelectionStart;

            if (Text == string.Empty || SelectionLength > 0)
            {
                _insertionLine.Left = 1;
            }
            else
            {
                _insertionLine.Left = ((selStart < Text.Length ?
                    GetPositionFromCharIndex(selStart) : EndOfTextLocation).X);
            }
        }

        #endregion

        #region Public methods (some are static and used by multiple views)
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Set's the pattern text box's search query, cloning it if specified by the
        /// cloneQuery flag.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public void SetSearchQuery(SearchQuery query)
        {
            SetSearchQuery(query, false);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Set's the pattern text box's search query, cloning it if specified by the
        /// cloneQuery flag.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public void SetSearchQuery(SearchQuery query, bool allowTextChangedEvent)
        {
            if (query == SearchQuery || (query != null && query.IsPatternRegExpression))
                return;

            if (!allowTextChangedEvent)
                TextChanged -= HandlePatternTextBoxTextChanged;

            if (query == null)
                Clear();
            else
            {
                Text = (string.IsNullOrEmpty(query.Pattern) ? EmptyPattern : query.Pattern);
                SearchQuery = query.Clone();
            }

            if (!allowTextChangedEvent)
                TextChanged += HandlePatternTextBoxTextChanged;

            if (SearchQueryChanged != null)
                SearchQueryChanged(this, EventArgs.Empty);

            SetInsertionLinePosition();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Clear the pattern
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public new void Clear()
        {
            TextChanged -= HandlePatternTextBoxTextChanged;
            Text = EmptyPattern;
            SearchQuery = new SearchQuery();
            SearchQueryCategory = null;
            TextChanged += HandlePatternTextBoxTextChanged;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Inserts the specified text in the current insertion point in the search pattern.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public void Insert(string text)
        {
            Insert(this, text);
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
            if (text.IndexOf(App.DottedCircleC) >= 0)
                ProcessTextWithDottedCircle(newText, selstart, ref text);

            newText = newText.Insert(selstart, text);
            txt.Text = newText;
            txt.SelectionStart = selstart + (text == "{}" || text == "[]"
                || text == App.DiacriticPlaceholder ? text.Length - 1 : text.Length);



            // Argh, I know.
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
            string nonDottedCirclePart = text.Replace(App.DottedCircle, string.Empty);
            if (nonDottedCirclePart.Length == 1)
            {
                var charInfo = App.IPASymbolCache[nonDottedCirclePart];
                if (charInfo != null && charInfo.CanPrecedeBase)
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
                    if (newText[i] == App.DottedCircleC)
                        dottedCircle = i;
                    else if (newText[i] == '[' || (newText[i] == ']' && i < selstart))
                        break;
                }
            }

            // Go forward, looking for a dotted circle or open/close brackets.
            for (int i = selstart; i < newText.Length && newText.Length > 0 && dottedCircle < 0; i++)
            {
                if (newText[i] == App.DottedCircleC)
                    dottedCircle = i;
                else if (newText[i] == ']' || newText[i] == '[')
                    break;
            }

            // Did we find a dotted circle. If not, then surround the
            // dotted circle and its diacritic in square brackets.
            if (dottedCircle >= 0)
            {
                // Remove the dotted circle.
                int i = text.IndexOf(App.DottedCircleC);
                text = text.Substring(0, i) + text.Substring(i + 1);
            }
        }

        #endregion

        #region Message handlers for Inserting
        /// ------------------------------------------------------------------------------------
        protected bool OnInsertConsonant(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert("[C]");
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnInsertVowel(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert("[V]");
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnInsertZeroOrMore(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert("*");
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnInsertOneOrMore(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert("+");
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnInsertWordBoundary(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert("#");
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnInsertDiacriticPlaceholder(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert(App.DiacriticPlaceholder);
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnInsertSyllableBoundary(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert(".");
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnInsertANDGroup(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert("[]");
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnInsertORGroup(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            Insert("{}");
            return true;
        }

        #endregion

        #region Message handlers for non inserting actions
        /// ------------------------------------------------------------------------------------
        protected bool OnDropDownSearchOptions(object args)
        {
            if (!OwningView.ActiveView)
                return false;

            _searchOptionsDropDown.SearchQuery = SearchQuery;
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnDropDownClosedSearchOptions(object args)
        {
            var itemProps = args as TMItemProperties;
            if (itemProps == null || (itemProps.ParentControl != OwningView))
                return false;

            if (_searchOptionsDropDown.OptionsChanged)
            {
                SearchQuery = _searchOptionsDropDown.SearchQuery;

                if (SearchOptionsChanged != null)
                    SearchOptionsChanged(this, EventArgs.Empty);

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
            if (!OwningView.ActiveView)
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
            Font = App.PhoneticFont;
            SetInsertionLinePosition();

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
            if (Text == App.kEmptyDiamondPattern || Text == string.Empty)
                Text = EmptyPattern;

            SelectionStart = 0;
            SelectionLength = 0;
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
            string replacedText = App.Project.SearchClasses.ModifyPatternText(Text);

            if (replacedText != string.Empty)
            {
                ClassDisplayBehaviorChanged = true;
                Text = replacedText;
                ClassDisplayBehaviorChanged = false;
            }

            return false;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnUpdateBeginSearch(object args)
        {
            TMItemProperties itemProps = args as TMItemProperties;
            if (!OwningView.ActiveView || itemProps == null ||
                itemProps.Name.StartsWith("cmnu", StringComparison.Ordinal))
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
        protected bool OnUpdateSavePattern(object args)
        {
            TMItemProperties itemProps = args as TMItemProperties;
            if (!OwningView.ActiveView || itemProps == null)
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
        protected bool OnUpdateVerifyPattern(object args)
        {
            TMItemProperties itemProps = args as TMItemProperties;
            if (itemProps == null || !OwningView.ActiveView)
                return false;

            itemProps.Visible = true;
            itemProps.Enabled = !IsPatternEmpty;
            itemProps.Update = true;
            return true;
        }

        #endregion

        #region Overridden event handlers
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Do this on the up stroke since the insertion point will have been moved by that
        /// time, thus causing the arrows to be placed in the correct location.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            SetInsertionLinePosition();
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

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

            if (!Focused)
                Focus();

            Point pt = PointToClient(new Point(e.X, e.Y));
            if (pt.X >= 0)
            {
                SelectionStart = (pt.X >= EndOfTextLocation.X ?
                    Text.Length : GetCharIndexFromPosition(pt));
            }

            SetInsertionLinePosition();

            e.Effect = DragDropEffects.Copy;
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            SearchQuery query = e.Data.GetData(typeof(SearchQuery)) as SearchQuery;

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
                    SetSearchQuery(query, true);
                }
            }

            SetInsertionLinePosition();
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            SetInsertionLinePosition();
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            SetInsertionLinePosition();
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetInsertionLinePosition();
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            _insertionLine.Visible = false;
            SetInsertionLinePosition();
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            _insertionLine.Visible = true;
            SetInsertionLinePosition();
        }

        /// ------------------------------------------------------------------------------------
        public override bool PreProcessMessage(ref Message m)
        {
            if (m.Msg != 0x0100)
                return base.PreProcessMessage(ref m);

            // Handle some special cases when the Ctrl key is down. Except for Ctrl0, the
            // reason we handle {}{} and - (the dash is treated as an underscore) specially
            // is because the KeyMan IPA keyboard intercepts them for its purposes.
            if ((ModifierKeys & Keys.Control) != Keys.Control)
                return base.PreProcessMessage(ref m);

            bool shiftDown = ((ModifierKeys & Keys.Shift) == Keys.Shift);
            string toInsert = null;
            int keyCode = m.WParam.ToInt32();

            if (keyCode == (int)Keys.OemCloseBrackets)
                toInsert = (shiftDown ? "}" : "]");
            else if (keyCode == (int)Keys.OemOpenBrackets)
                toInsert = (shiftDown ? "{" : "[");
            else if (keyCode == (int)Keys.OemMinus)
                toInsert = "_";
            else if (keyCode == (int)Keys.D0)
                toInsert = App.DiacriticPlaceholder;

            if (toInsert != null)
            {
                Insert(toInsert);
                return true;
            }

            return base.PreProcessMessage(ref m);
        }

        #endregion

        #region Static methods used by multiple views
        /// ------------------------------------------------------------------------------------
        public static void HandlePatternTextBoxTextChanged(object sender, EventArgs e)
        {
            var txt = sender as PatternTextBox;

            if (txt == null /* || m_ignoreTextChange */)
                return;

            int selstart = txt.SelectionStart;
            int sellen = txt.SelectionLength;

            // Force all consonant class to be uppercase.
            if (txt.Text.IndexOf("[c]", StringComparison.Ordinal) >= 0)
                txt.Text = txt.Text.Replace("[c]", "[C]");

            // Force all vowel class to be uppercase.
            if (txt.Text.IndexOf("[v]", StringComparison.Ordinal) >= 0)
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

            if (txt.SearchQuery.Pattern != txt.Text)
            {
                txt.SearchQuery.Pattern = txt.Text;
                if (txt.PatternTextChanged != null)
                    txt.PatternTextChanged(txt, EventArgs.Empty);
            }

            txt.Invalidate();
        }

        /// ------------------------------------------------------------------------------------
        /// <remarks>
        /// Ctrl-A: KeyChar = 1
        /// Ctrl-C: KeyChar = 3
        /// Ctrl-V: KeyChar = 22
        /// Ctrl-X: KeyChar = 24
        /// Ctrl-Y: KeyChar = 25
        /// Ctrl-Z: KeyChar = 26
        /// </remarks>
        /// ------------------------------------------------------------------------------------
        public static void HandlePatternTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            var txt = sender as PatternTextBox;

            // Let ctrl sequences take their normal course.
            if (txt == null)
                return;

            int selStart = txt.SelectionStart;

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

            if (e.KeyChar == (char)Keys.Back)
            {
                if (selStart > 0)
                {
                    txt.Text = txt.Text.Remove(selStart, 0);
                    txt.SelectionStart = selStart;
                    return;
                }
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
            if (e.KeyChar == (char)Keys.Enter && txt.IsPatternFull)
            {
                App.MsgMediator.SendMessage("EnterPressedInSearchPatternTextBox", txt.SearchQuery);
                e.Handled = true;
                return;
            }



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
            // But only when the insertion point is not in a class name or
            // binary or articulatory feature.
            if ((e.KeyChar == 'C' || e.KeyChar == 'V') && SurroundCVInBrackets(txt))
            {
                txt.Text = txt.Text.Insert(selStart, ("[" + e.KeyChar + "]"));
                e.KeyChar = (char)0;
                e.Handled = true;
                txt.SelectionStart = selStart + 3;
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

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if the C or V the user just typed should be surrounded by square brackets.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private static bool SurroundCVInBrackets(TextBoxBase txt)
        {
            if (!Settings.Default.AssumeCVKeysArePhoneClassWhileTyping)
                return false;

            return (!InsideBraketedGroup(txt, '[', ']') && !InsideBraketedGroup(txt, '<', '>'));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Determines whether or not the insertion point in the specified text box is inside
        /// the set of specified brackets.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private static bool InsideBraketedGroup(TextBoxBase txt, char openBracket, char closeBracket)
        {
            for (int i = txt.SelectionStart - 1; i >= 0 && i < txt.Text.Length; i--)
            {
                if (txt.Text[i] == closeBracket)
                    break;

                if (txt.Text[i] == openBracket)
                    return true;
            }

            return false;
        }

        #endregion

        #region IxCoreColleague Members
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
}
