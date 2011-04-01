using System;
using System.Windows.Forms;
using Palaso.Reporting;
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
			ErrorReport.AddProperty("EmailAddress", "PaFeedback@sil.org");
			ErrorReport.AddStandardProperties();
			ExceptionHandler.Init();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new PaMainWnd(true));
		}
	}
}
