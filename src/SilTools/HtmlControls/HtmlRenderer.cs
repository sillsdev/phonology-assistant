using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace System.Drawing.Html
{
    public static class HtmlRenderer
    {
        #region References

        /// <summary>
        /// List of assembly references
        /// </summary>
        private static List<Assembly> _references;

        /// <summary>
        /// Gets a list of Assembly references used to search for external references
        /// </summary>
        /// <remarks>
        /// This references are used when loading images and other content, when
        /// rendering a piece of HTML/CSS
        /// </remarks>
        public static List<Assembly> References
        {
            get { return _references; }
        }

        /// <summary>
        /// Adds a reference to the References list if not yet listed
        /// </summary>
        /// <param name="assembly"></param>
        internal static void AddReference(Assembly assembly)
        {
            if (!References.Contains(assembly))
            {
                References.Add(assembly);
            }
        }

        static HtmlRenderer()
        {
            //Initialize references list
            _references = new List<Assembly>();

            //Add this assembly as a reference
            References.Add(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the HTML on the specified point using the specified width.
        /// </summary>
        /// <param name="g">Device to draw</param>
        /// <param name="html">HTML source</param>
        /// <param name="location">Point to start drawing</param>
        /// <param name="width">Width to fit HTML drawing</param>
        public static void Render(Graphics g, string html, PointF location, float width)
        {
            Render(g, html, new RectangleF(location, new SizeF(width, 0)), false);
        }

        /// <summary>
        /// Renders the specified HTML source on the specified area clipping if specified
        /// </summary>
        /// <param name="g">Device to draw</param>
        /// <param name="html">HTML source</param>
        /// <param name="area">Area where HTML should be drawn</param>
        /// <param name="clip">If true, it will only paint on the specified area</param>
        public static void Render(Graphics g, string html, RectangleF area, bool clip)
        {
            InitialContainer container = new InitialContainer(html);
            Region prevClip = g.Clip;

            if (clip) g.SetClip(area);

            container.SetBounds(area);
            container.MeasureBounds(g);
            container.Paint(g);

            if (clip) g.SetClip(prevClip, System.Drawing.Drawing2D.CombineMode.Replace);
        }

        #endregion
    }
}
