using System.Collections.Generic;

namespace System.Drawing.Html
{
    /// <summary>
    /// Splits text on words for a box
    /// </summary>
    internal class CssBoxWordSplitter
    {
        #region Static

        /// <summary>
        /// Returns a bool indicating if the specified box white-space processing model specifies
        /// that sequences of white spaces should be collapsed on a single whitespace
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CollapsesWhiteSpaces(CssBox b)
        {
            return b.WhiteSpace == CssConstants.Normal ||
                b.WhiteSpace == CssConstants.Nowrap ||
                b.WhiteSpace == CssConstants.PreLine;
        }

        /// <summary>
        /// Returns a bool indicating if line breaks at the source should be eliminated
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EliminatesLineBreaks(CssBox b)
        {
            return b.WhiteSpace == CssConstants.Normal || b.WhiteSpace == CssConstants.Nowrap;
        }

        #endregion

        #region Fields
        private CssBox _box;
        private string _text;
        private List<CssBoxWord> _words;
        private CssBoxWord _curword;

        #endregion

        #region Ctor

        private CssBoxWordSplitter()
        {
            _words = new List<CssBoxWord>();
            _curword = null;
        }

        public CssBoxWordSplitter(CssBox box, string text)
            : this()
        {
            _box = box;
            _text = text.Replace("\r", string.Empty); ;
        }

        #endregion

        #region Props


        public List<CssBoxWord> Words
        {
            get { return _words; }
        }


        public string Text
        {
            get { return _text; }
        }


        public CssBox Box
        {
            get { return _box; }
        }


        #endregion

        #region Public Metods

        /// <summary>
        /// Splits the text on words using rules of the specified box
        /// </summary>
        public void SplitWords()
        {

            if (string.IsNullOrEmpty(Text)) return;

            _curword = new CssBoxWord(Box);

            bool onspace = IsSpace(Text[0]);

            for (int i = 0; i < Text.Length; i++)
            {
                if (IsSpace(Text[i]))
                {
                    if (!onspace) CutWord();

                    if (IsLineBreak(Text[i]))
                    {
                        _curword.AppendChar('\n');
                        CutWord();
                    }
                    else if (IsTab(Text[i]))
                    {
                        _curword.AppendChar('\t');
                        CutWord();
                    }
                    else
                    {
                        _curword.AppendChar(' ');
                    }

                    onspace = true;
                }
                else
                {
                    if (onspace) CutWord();
                    _curword.AppendChar(Text[i]);

                    onspace = false;
                }
            }

            CutWord();
        }

        private void CutWord()
        {
            if(_curword.Text.Length > 0)
                Words.Add(_curword);
            _curword = new CssBoxWord(Box);
        }

        private bool IsSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }

        private bool IsLineBreak(char c)
        {
            return c == '\n' || c == '\a';
        }

        private bool IsTab(char c)
        {
            return c == '\t';
        }

        #endregion
    }

}
