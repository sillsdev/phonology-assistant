using System;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	/// <summary>
	/// The only method in this class is <see cref="Main"/>. All other methods should go
	/// in a Dll, so that NUnit tests can be written.
	/// </summary>
	public class PA
	{
		[STAThread]
		static void Main(string[] commandLineArgs) 
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new PaMainWnd(commandLineArgs));
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
