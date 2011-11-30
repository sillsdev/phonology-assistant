using System;
using System.Collections.Generic;
using System.Text;

namespace System.Drawing.Html
{
    /// <summary>
    /// Used to mark a property as a Css property.
    /// The <see cref="Name"/> property is used to specify the oficial CSS name
    /// </summary>
    public class CssPropertyAttribute : Attribute
    {
        #region Fields
        private string _name;
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new CssPropertyAttribute
        /// </summary>
        /// <param name="name">Name of the Css property</param>
        public CssPropertyAttribute(string name)
        {
            Name = name;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the CSS property
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
	

        #endregion
    }
}
