using System;
using System.Windows.Forms;
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
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new PaMainWnd(true));
			}
			catch (Exception e)
			{
				ExceptionViewer viewer = new ExceptionViewer(e);
				viewer.Show();

				while (viewer.Visible)
					Application.DoEvents();
			}
		}
	}
}
