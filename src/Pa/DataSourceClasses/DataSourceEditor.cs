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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using L10NSharp;
using Microsoft.Win32;
using Palaso.Reporting;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Dialogs;
using SilTools;

namespace SIL.Pa.DataSource
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class that encapsulates methods for calling programs that own a particular data source.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DataSourceEditor
	{
		#region OS-specific stuff
		private struct POINTAPI
		{
			public int x;
			public int y;
		}

		private struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		private struct WINDOWPLACEMENT
		{
			public int length;
			public int flags;
			public int showCmd;
			public POINTAPI ptMinPosition;
			public POINTAPI ptMaxPosition;
			public RECT rcNormalPosition;
		}

		private const int RestoreToMaximized = 2;
		private const int Minimized = 2;

#if !__MonoCS__
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
#else
		private static int ShowWindow(IntPtr hwnd, int nCmdShow)
		{
			Console.WriteLine("Warning--using unimplemented method ShowWindow"); // FIXME Linux
			return(0);
		}
#endif
		
#if !__MonoCS__
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
#else
		private static bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl)
		{
			Console.WriteLine("Warning--using unimplemented method GetWindowPlacement"); // FIXME Linux
			return(false);
		}
#endif

		#endregion

		private static List<Process> s_saProcesses;
		
		private readonly bool m_showFwJumpUrlDlg;
		private const string kSaListFileContentFmt = "[Settings]\nCallingApp={0}\n" +
			"[AudioFiles]\nFile0={1}\n[BeginningWAVOffsets]\nOffset0={2}\n" +
			"[EndingWAVOffsets]\nOffset0={3}\n[Commands]\nCommand0=SelectFile(0)";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a DataSourceEditor object and calls the "owning" application to edit the
		/// data source from which the specified record originated (e.g. Toolbox, Flex, SA).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataSourceEditor(WordCacheEntry wcentry, string callingApp) :
			this(wcentry, callingApp, false)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a DataSourceEditor object and calls the "owning" application to edit the
		/// data source from which the specified record originated (e.g. Toolbox, Flex, SA).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataSourceEditor(WordCacheEntry wcentry, string callingApp, bool showFwUrlDialog)
		{
			m_showFwJumpUrlDlg = showFwUrlDialog;
		
			var sourceType = wcentry.RecordEntry.DataSource.Type;

			switch (sourceType)
			{
				case DataSourceType.SFM: EditRecordInSFMEditor(wcentry.RecordEntry); break;
				case DataSourceType.Toolbox: EditRecordInToolbox(wcentry.RecordEntry); break;
				case DataSourceType.FW:
				case DataSourceType.FW7: EditRecordInFieldWorks(wcentry.RecordEntry); break;
				case DataSourceType.SA: EditRecordInSA(wcentry, callingApp); break;
				default:
					ErrorReport.NotifyUserOfProblem(LocalizationManager.GetString(
						"Miscellaneous.Messages.DataSourceEditing.UnableToEditSourceRecordMsg",
						"There is no source record editor associated with this record."));
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not Toolbox is a running process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsToolboxRunning
		{
            get { return Process.GetProcesses().Any(prs => prs.ProcessName.ToLower().StartsWith("toolbox", StringComparison.Ordinal)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to edit the specified record in the editor specified for its data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EditRecordInSFMEditor(RecordCacheEntry recEntry)
		{
			if (recEntry == null || recEntry.DataSource == null)
				return;

			string msg = null;
			string arg = null;

			// Make sure an editor has been specified.
			if (string.IsNullOrEmpty(recEntry.DataSource.Editor))
			{
				arg = recEntry.DataSource.SourceFile;
				msg = LocalizationManager.GetString("Miscellaneous.Messages.DataSourceEditing.NoDataSourceEditorSpecifiedMsg",
					"No editor has been specified in the project settings for the following data source: '{0}'. See the help file for more information.",
					"Displayed when no editor has been specified and the user chooses 'Edit Source Record' from the edit menu.");
			}

			// Make sure editor exists.
			if (msg == null && !File.Exists(recEntry.DataSource.Editor))
			{
				arg = recEntry.DataSource.Editor;
				msg = LocalizationManager.GetString("Miscellaneous.Messages.DataSourceEditing.DataSourceEditorMissingMsg", "The editor '{0}' cannot be found.",
					"Displayed when specified editor cannot be found when the user chooses 'Edit Source Record' from the edit menu.");
			}

			if (msg != null)
			{
				ErrorReport.NotifyUserOfProblem(msg, arg);
				return;
			}

			// Start editor.
			var prs = new Process();
			prs.StartInfo.UseShellExecute = true;
			prs.StartInfo.FileName = "\"" + recEntry.DataSource.Editor + "\"";
			prs.StartInfo.Arguments = " \"" + recEntry.DataSource.SourceFile + "\"";
			prs.Start();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to edit the specified record in Toolbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EditRecordInToolbox(RecordCacheEntry recEntry)
		{
			if (!IsToolboxRunning)
			{
				var msg = LocalizationManager.GetString("Miscellaneous.Messages.DataSourceEditing.ToolboxNotRunningMsg",
					"For this feature to work, you must have Toolbox running with the following database opened:\n\n {0}\n\nSee the help file for more information.");
				Utils.MsgBox(string.Format(msg, Utils.PrepFilePathForMsgBox(recEntry.DataSource.SourceFile)));
			    return;
			}

			string sortFieldName = recEntry.DataSource.ToolboxSortField;

			// Get the record field whose value will tell us what record to jump to.
			if (string.IsNullOrEmpty(sortFieldName))
			{
				var msg = LocalizationManager.GetString("Miscellaneous.Messages.DataSourceEditing.NoToolboxSortFieldSpecified",
					"The first Toolbox sort field for this record's data source has not been specified. " +
					"To specify the first Toolbox sort field, go to the data source's properties from " +
					"the project properties dialog.");
				ErrorReport.NotifyUserOfProblem(msg);
				return;
			}

			// Find the field information for the specified sort field.
			var field = recEntry.Project.GetFieldForName(sortFieldName);
			if (field == null)
			{
				var msg = LocalizationManager.GetString(
					"Miscellaneous.Messages.DataSourceEditing.InvalidToolboxSortField",
					"The field '{0}' is invalid.");
				
				ErrorReport.NotifyUserOfProblem(msg, sortFieldName);
				return;
			}

			// Get the value indicating what record to jump to.
			var jumpValue = (field.Type == FieldType.Phonetic ?
				GetPhoneticJumpWord(recEntry) : recEntry[sortFieldName]);

			if (string.IsNullOrEmpty(jumpValue))
				return;

			// Write the value to the registry.
			RegistryKey regKey =
			    Registry.CurrentUser.CreateSubKey(@"Software\SantaFe\Focus\Word");

			regKey.SetValue(null, jumpValue, RegistryValueKind.String);

			// Inform anyone who cares (namely Toolbox) that a jump is being requested.
			uint WM_SANTA_FE_FOCUS = Utils.RegisterWindowMessage("SantaFeFocus");
			Utils.PostMessage(Utils.HWND_BROADCAST, WM_SANTA_FE_FOCUS, 4, 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the phonetic text used to tell Toolbox what record to jump to.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetPhoneticJumpWord(RecordCacheEntry recEntry)
		{
			// Because phonetic data may contain converted ambiguous items, we need to make
			// sure the original phonetic data is used for the jump value. Therefore, we'll
			// pull the original phonetic data from a temporary file containing all the
			// original phonetic data for each data source record.
			var tempCache = TempRecordCache.Load();
			var jumpValue = tempCache[recEntry.Id];
			tempCache.Clear();
			return jumpValue;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to edit the specified record in FieldWorks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EditRecordInFieldWorks(RecordCacheEntry recEntry)
		{
			var url = (recEntry.DataSource.Type == DataSourceType.FW ?
				FwDBAccessInfo.JumpUrl : Settings.Default.Fw7JumpUrlFormat);

			if (recEntry.DataSource.Type == DataSourceType.FW)
			{
				url = string.Format(url, recEntry.Guid,
					recEntry.DataSource.FwDataSourceInfo.Server,
					recEntry.DataSource.FwDataSourceInfo.Name);
			}
			else
			{
				url = string.Format(url, recEntry.DataSource.FwDataSourceInfo.Name,
					recEntry.DataSource.FwDataSourceInfo.Server, recEntry.Guid);
			}

			// Spaces aren't allowed in the URL. They should be converted to '+'.
			url = url.Trim().Replace(' ', '+');

			try
			{
				if (m_showFwJumpUrlDlg)
				{
					using (var dlg = new EditFwUrlDlg(url))
					{
						if (dlg.ShowDialog() == DialogResult.Cancel)
							return;

						url = dlg.Url;
					}
				}

				RestoreAppIfRunning("Flex");
				Process.Start(url);
			}
			catch
			{
				// TODO: Should this be a message box?
				Clipboard.SetText(url);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to edit the specified record in Speech Analyzer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EditRecordInSA(WordCacheEntry wcentry, string callingApp)
		{
			// Get the audio file field.
			var field = wcentry.Project.GetAudioFileField();
			if (field == null)
			{
				ErrorReport.NotifyUserOfProblem(LocalizationManager.GetString(
					"Miscellaneous.Messages.DataSourceEditing.NoAudioFileFieldMissingMsg",
					"This project doesn't contain a field definition for an audio file path."));
				
				return;
			}

			// Get the audio file and make sure it exists.
			var audioFile = wcentry[field.Name];
			if (string.IsNullOrEmpty(audioFile) || !File.Exists(audioFile))
			{
				ErrorReport.NotifyUserOfProblem(LocalizationManager.GetString(
					"Miscellaneous.Messages.DataSourceEditing.AudioFileMissingMsg",
					"The audio file '{0}' is cannot be found."), audioFile);
				return;
			}

			// Make sure SA exists.
			var saPath = AudioPlayer.GetSaPath();
			if (saPath == null || !File.Exists(saPath))
			{
				var msg = LocalizationManager.GetString("Miscellaneous.Messages.DataSourceEditing.AudioEditProblemMsg",
					"Speech Analyzer 3.0.1 is required to edit audio data sources, but it " +
					"is not installed. Please install Speech Analyzer 3.0.1 and try again.",
					"Message displayed when SA 3.0.1 is not installed and the user is attempting to edit an audio file.");

				using (var dlg = new DownloadSaDlg(msg))
					dlg.ShowDialog();

				return;
			}

			var lstFile = GetSaListFile(wcentry, audioFile, callingApp);

			// Start SA.
			var prs = new Process();
			prs.StartInfo.UseShellExecute = true;
			prs.StartInfo.FileName = "\"" + saPath + "\"";
			prs.StartInfo.Arguments = "-l " + lstFile;
			prs.EnableRaisingEvents = true;
			prs.Exited += SA_Exited;

			// Create a new collection to hold the new process.
			if (s_saProcesses == null)
				s_saProcesses = new List<Process>();

			// Save the process so PA has a record of it. (See CloseSAInstances, below)
			s_saProcesses.Add(prs);
			prs.Start();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When an SA process quites, then remove it from our collection of SA process PA
		/// started.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SA_Exited(object sender, EventArgs e)
		{
			var prs = sender as Process;
			if (prs != null)
			{
				prs.Exited -= SA_Exited;
				s_saProcesses.Remove(prs);

				if (s_saProcesses.Count == 0)
					s_saProcesses = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes all instances of SA that PA started.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void CloseSAInstances()
		{
			if (s_saProcesses != null)
			{
				foreach (var prs in s_saProcesses)
				{
					prs.Exited -= SA_Exited;
					prs.CloseMainWindow();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a temporary file that contains commands for SA to use for opening
		/// the specified wave file and zooming to a particular start and stop offset.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetSaListFile(WordCacheEntry wcentry, string audioFile, string callingApp)
		{
			// Get the utterance's offset.
			var offset = wcentry.AudioOffset;
			var length = wcentry.AudioLength;

			// Create the contents for the SA list file.
			var saListFileContent = string.Format(kSaListFileContentFmt,
				new object[] { callingApp, audioFile, offset, offset + length });
			
			saListFileContent = Utils.ConvertLiteralNewLines(saListFileContent);

			// Write the list file.
			var lstFile = Path.GetTempFileName();
			File.AppendAllText(lstFile, saListFileContent);

			return lstFile;
		}

		/// ------------------------------------------------------------------------------------
		private void RestoreAppIfRunning(string processName)
		{
			var prs = Process.GetProcessesByName(processName);

			if (prs.Length > 0)
			{
				var placement = new WINDOWPLACEMENT();
				placement.length = Marshal.SizeOf(placement);
				GetWindowPlacement(prs[0].MainWindowHandle, ref placement);

				if (placement.showCmd != Minimized)
					return;

				bool gotoMax = (placement.flags & RestoreToMaximized) > 0;
				ShowWindow(prs[0].MainWindowHandle, gotoMax ? 3 : 9);
			}
		}
	}
}
