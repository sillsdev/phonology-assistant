using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using SIL.Localize.LocalizingUtils;

namespace SIL.Localize.CreateResourceCatalog
{
	static class Program
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// ------------------------------------------------------------------------------------
		static void Main(string[] args)
		{
			Console.WriteLine();
			
			if (args.Length != 3)
			{
				ShowUsage();
				return;
			}

			string folder = null;
			string appName = null;
			string outputFile = null;

			foreach (string arg in args)
			{
				if (arg.ToLower().StartsWith("/out:"))
					outputFile = ParseArg(arg);
				else if (arg.ToLower().StartsWith("/scan:"))
					folder = ParseArg(arg);
				else if (arg.ToLower().StartsWith("/app:"))
					appName = ParseArg(arg);
			}

			if (!VerifyArgs(outputFile, folder, appName))
				return;

			string[] files = Directory.GetFiles(folder, "*.csproj", SearchOption.AllDirectories);
			if (files == null || files.Length == 0)
			{
				Console.WriteLine("There are no C# project files found in '" + folder + "'.");
				return;
			}

			try
			{
				ResourceCatalog list = ResourceCatalogCreator.Create(new List<string>(files), null);
				list.ApplicationName = appName;

				if (list != null && list.Count > 0)
				{
					// Create the output directory if it doesn't exist.
					string outputFolder = Path.GetDirectoryName(outputFile);
					if (!string.IsNullOrEmpty(outputFolder) && !Directory.Exists(outputFolder))
						Directory.CreateDirectory(outputFolder);

					LocalizingHelper.SerializeData(outputFile, list);
					Console.WriteLine("The resource catalog '" + outputFile + "' has been created.");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				while (e.InnerException != null)
				{
					e = e.InnerException;
					Console.WriteLine(e.Message);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string ParseArg(string arg)
		{
			int i = arg.IndexOf(':');
			arg = arg.Substring(i + 1);
			return arg.Replace("\"", string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool VerifyArgs(string outputFile, string folder, string appName)
		{
			if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(appName) ||
				string.IsNullOrEmpty(outputFile))
			{
				ShowUsage();
				return false;
			}

			if (!Directory.Exists(folder))
			{
				Console.WriteLine("The folder '" + folder + "' does not exist.");
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ShowUsage()
		{
			string progName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);

			Console.WriteLine("Usage:");
			Console.WriteLine();
			Console.WriteLine(string.Format("   {0} /out:outputfile /scan:folder /app:name", progName));
			Console.WriteLine();
			Console.WriteLine("   /out:outputfile   outputfile is the XML resource cataglog output file.");
			Console.WriteLine("   /scan:folder      folder is the root folder where scanning for C# projects begins. Scanning covers sub folders.");
			Console.WriteLine("   /app:name         name is the application name (e.g. FieldWorks, Phonology Assistant, etc.)");
			Console.WriteLine();
			Console.WriteLine();
		}
	}
}
