// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Drawing.Html
{
    /// <summary>
    /// Represents an anonymous inline box
    /// </summary>
    /// <remarks>
    /// To learn more about anonymous inline boxes visit:
    /// http://www.w3.org/TR/CSS21/visuren.html#anonymous
    /// </remarks>
    public class CssAnonymousBox
        : CssBox
    {
        #region Ctor

        public CssAnonymousBox(CssBox parentBox)
            : base(parentBox)
        {

        }

        #endregion
    }

    /// <summary>
    /// Represents an anonymous inline box which contains nothing but blank spaces
    /// </summary>
    public class CssAnonymousSpaceBox
        : CssAnonymousBox
    {
        public CssAnonymousSpaceBox(CssBox parentBox)
            : base(parentBox)
        {

        }
    }

}
