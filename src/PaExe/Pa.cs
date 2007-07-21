using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
				List<string> args = new List<string>(commandLineArgs);

				for (int i = 0; i < args.Count; i++)
				{
					if (args[i].ToLower().StartsWith("/uilang:"))
					{
						string lang = args[i].Substring(8);
						try
						{
							CultureInfo ci = CultureInfo.GetCultureInfo(lang);
							Thread.CurrentThread.CurrentUICulture = ci;
						}
						catch (Exception e)
						{
							string msg = e.Message + "\n\nUsing default culture.";
							STUtils.STMsgBox(msg, MessageBoxButtons.OK);
						}

						args.RemoveAt(i);
						break;
					}
				}

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new PaMainWnd(args.ToArray()));
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
