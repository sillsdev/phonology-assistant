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
using System.Windows.Forms;
using SIL.Reporting;
using SIL.Pa.UI;
using SilTools;

namespace SIL.Pa
{
    /// ----------------------------------------------------------------------------------------
    public class Program
    {
        /// ------------------------------------------------------------------------------------
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetCompatibleTextRenderingDefault(false);
            }
            catch
            {
                // The compatible rendering system may default to false anyway.
            }
            ErrorReport.EmailAddress = "PaFeedback@sil.org";
			ErrorReport.AddStandardProperties();
			ExceptionHandler.Init();

            Application.EnableVisualStyles();
			Application.Run(new PaMainWnd(true));
		}
	}
}
