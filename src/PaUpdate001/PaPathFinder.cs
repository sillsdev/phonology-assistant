using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace SIL.Pa.Updates
{
	public static class PaPathFinder
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetPath()
		{
			string path = GetPaPathFromRegistry();
			if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
				path = GetPaPathFromUser();

			return (path == null || !Directory.Exists(path) ? null : path);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Looks in the registry for values that will provide the installation location of PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetPaPathFromRegistry()
		{
			try
			{
				// Get the association entry for .pap files. That value will be used for
				// getting another key value which contains the actual command-line.
				RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\.pap", false);
				string papAssoc = key.GetValue(string.Empty, null) as string;
				key.Close();

				if (string.IsNullOrEmpty(papAssoc))
					return null;

				// Get the command-line for opening .pap files.
				string keyPath = @"SOFTWARE\Classes\{0}\shell\open\command";
				key = Registry.LocalMachine.OpenSubKey(string.Format(keyPath, papAssoc));
				string paPath = key.GetValue(string.Empty, null) as string;
				key.Close();

				if (string.IsNullOrEmpty(paPath))
					return null;

				paPath = paPath.Replace("\"%1\"", string.Empty).Trim();
				return Path.GetDirectoryName(paPath);
			}
			catch { }

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetPaPathFromUser()
		{
			FolderBrowserDialog fldrBrowser = new FolderBrowserDialog();
			fldrBrowser.ShowNewFolderButton = false;
			fldrBrowser.Description = Properties.Resources.kstidInstallFolderMissing;
			fldrBrowser.SelectedPath =
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

			while (true)
			{
				if (fldrBrowser.ShowDialog() == DialogResult.Cancel)
					break;

				if (VerifyManuallyChosenFolder(fldrBrowser.SelectedPath))
					return fldrBrowser.SelectedPath;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool VerifyManuallyChosenFolder(string path)
		{
			if (File.Exists(Path.Combine("Pa.exe", path)) &&
				File.Exists(Path.Combine("PaDll.dll", path)))
			{
				return true;
			}

			string msg = Properties.Resources.kstidSuspectFolderMsg.Replace("\\n", "\n");
			return (MessageBox.Show(msg, Properties.Resources.kstidMsgBoxCaption,
				MessageBoxButtons.YesNo) == DialogResult.Yes);
		}
	}
}
