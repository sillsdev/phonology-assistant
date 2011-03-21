using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
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
		#region Windows API stuff
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

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

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
					var msg = App.LocalizeString("UnableToEditSourceRecordMsg",
						"There is no source record editor associated with this record.",
						App.kLocalizationGroupInfoMsg);

					Utils.MsgBox(msg);
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
			get { return Process.GetProcesses().Any(prs => prs.ProcessName.ToLower().StartsWith("toolbox")); }
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

			// Make sure an editor has been specified.
			if (string.IsNullOrEmpty(recEntry.DataSource.Editor))
			{
				msg = App.LocalizeString("NoDataSourceEditorSpecifiedMsg",
					"No editor has been specified in the project settings for the following data source:\n\n{0}\n\nSee the help file for more information.",
					"Displayed when no editor has been specified and the user chooses 'Edit Source Record' from the edit menu.",
					App.kLocalizationGroupInfoMsg);

				msg = string.Format(msg, Utils.PrepFilePathForMsgBox(recEntry.DataSource.SourceFile));
			}

			// Make sure editor exists.
			if (msg == null && !File.Exists(recEntry.DataSource.Editor))
			{
				msg = App.LocalizeString("DataSourceEditorMissingMsg",
					"The editor '{0}' cannot be found.",
					"Displayed when specified editor cannot be found when the user chooses 'Edit Source Record' from the edit menu.",
					App.kLocalizationGroupInfoMsg);

				msg = string.Format(msg, Utils.PrepFilePathForMsgBox(recEntry.DataSource.Editor));
			}

			if (msg != null)
			{
				Utils.MsgBox(msg);
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
				string msg = string.Format(Properties.Resources.kstidToolboxNotRunningMsg,
					Utils.PrepFilePathForMsgBox(recEntry.DataSource.SourceFile));
				
				Utils.MsgBox(msg);
			    return;
			}

			string sortFieldName = recEntry.DataSource.ToolboxSortField;

			// Get the record field whose value will tell us what record to jump to.
			if (string.IsNullOrEmpty(sortFieldName))
			{
				Utils.MsgBox(Properties.Resources.kstidNoToolboxSortFieldSpecified);
				return;
			}

			// Find the field information for the specified sort field.
			var field = recEntry.Project.GetFieldForName(sortFieldName);
			if (field == null)
			{
				var msg = Properties.Resources.kstidInvalidToolboxSortField;
				Utils.MsgBox(string.Format(msg, sortFieldName));
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
			var field = recEntry.Project.GetFieldForName("GUID");
			var url = (recEntry.DataSource.Type == DataSourceType.FW ?
				FwDBAccessInfo.JumpUrl : Settings.Default.Fw7JumpUrlFormat);

			if (field == null || string.IsNullOrEmpty(url))
				return;

			if (recEntry.DataSource.Type == DataSourceType.FW)
			{
				url = string.Format(url, recEntry[field.Name],
					recEntry.DataSource.FwDataSourceInfo.Server,
					recEntry.DataSource.FwDataSourceInfo.Name);
			}
			else
			{
				url = string.Format(url, recEntry.DataSource.FwDataSourceInfo.Name,
					recEntry.DataSource.FwDataSourceInfo.Server, recEntry[field.Name]);
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
				Utils.MsgBox(App.LocalizeString("NoAudioFileFieldMissingMsg",
					"This project doesn't contain a field definition for an audio file path.",
					App.kLocalizationGroupInfoMsg));
				
				return;
			}

			// Get the audio file and make sure it exists.
			var audioFile = wcentry[field.Name];
			if (string.IsNullOrEmpty(audioFile) || !File.Exists(audioFile))
			{
				var msg = App.LocalizeString("AudioFileMissingMsg",
					"The audio file '{0}' is cannot be found.", App.kLocalizationGroupInfoMsg);
	
				Utils.MsgBox(string.Format(msg, audioFile));
				return;
			}

			// Make sure SA exists.
			var saPath = AudioPlayer.GetSaPath();
			if (saPath == null || !File.Exists(saPath))
			{
				var msg = App.LocalizeString("AudioEditProblemMsg",
					"Speech Analyzer 3.0.1 is required to edit audio data sources, but it " +
					"is not installed. Please install Speech Analyzer 3.0.1 and try again.",
					"Message displayed when SA 3.0.1 is not installed and the user is attempting to edit an audio file.",
					App.kLocalizationGroupInfoMsg);

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
			ulong offset;
			var field = wcentry.Project.GetAudioOffsetField();
			if (field == null || !ulong.TryParse(wcentry[field.Name], out offset))
				offset = 0;

			// Get the utterance's length.
			ulong length;
			field = wcentry.Project.GetAudioLengthField();
			if (field == null || !ulong.TryParse(wcentry[field.Name], out length))
				length = 0;

			// Create the contents for the SA list file.
			string saListFileContent = string.Format(kSaListFileContentFmt,
				new object[] { callingApp, audioFile, offset, offset + length });
			saListFileContent = Utils.ConvertLiteralNewLines(saListFileContent);

			// Write the list file.
			string lstFile = Path.GetTempFileName();
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
