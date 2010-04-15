using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
	public class PaStartup
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
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
							Utils.MsgBox(e.Message + "\n\nUsing default culture.");
						}

						args.RemoveAt(i);
						break;
					}
				}

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
