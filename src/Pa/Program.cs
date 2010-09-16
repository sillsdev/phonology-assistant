using System;
using System.Windows.Forms;
using SIL.Pa.UI;
using SilUtils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Program
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
