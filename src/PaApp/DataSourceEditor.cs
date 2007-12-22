using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using SIL.SpeechTools.AudioUtils;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
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
		
		private readonly bool m_showFwJumpUrlDlg = false;
		private readonly string m_saListFileContentFmt = "[Settings]\nCallingApp={0}\n" +
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
		
			DataSourceType sourceType = wcentry.RecordEntry.DataSource.DataSourceType;

			if (sourceType == DataSourceType.SFM)
				EditRecordInSFMEditor(wcentry.RecordEntry);
			else if (sourceType == DataSourceType.Toolbox)
				EditRecordInToolbox(wcentry.RecordEntry);
			else if (sourceType == DataSourceType.FW)
				EditRecordInFieldWorks(wcentry.RecordEntry);
			else if (sourceType == DataSourceType.SA)
				EditRecordInSA(wcentry, callingApp);
			else
				STUtils.STMsgBox(Properties.Resources.kstidUnableToEditSourceRecordMsg);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not Toolbox is a running process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsToolboxRunning
		{
			get
			{
				Process[] processes = Process.GetProcesses();
				foreach (Process prs in processes)
				{
					if (prs.ProcessName.ToLower().StartsWith("toolbox"))
						return true;
				}

				return false;
			}
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
				msg = string.Format(Properties.Resources.kstidNoDataSourceEditorSpecifiedMsg,
					STUtils.PrepFilePathForSTMsgBox(recEntry.DataSource.DataSourceFile));
			}

			// Make sure editor exists.
			if (msg == null && !File.Exists(recEntry.DataSource.Editor))
			{
				msg = string.Format(Properties.Resources.kstidDataSourceEditorMissingMsg,
					STUtils.PrepFilePathForSTMsgBox(recEntry.DataSource.Editor));
			}

			if (msg != null)
			{
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// Start SA.
			Process prs = new Process();
			prs.StartInfo.UseShellExecute = true;
			prs.StartInfo.FileName = "\"" + recEntry.DataSource.Editor + "\"";
			prs.StartInfo.Arguments = " \"" + recEntry.DataSource.DataSourceFile + "\"";
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
					STUtils.PrepFilePathForSTMsgBox(recEntry.DataSource.DataSourceFile));
				
				STUtils.STMsgBox(msg);
			    return;
			}

			string sortField = recEntry.DataSource.ToolboxSortField;

			// Get the record field whose value will tell us what record to jump to.
			if (string.IsNullOrEmpty(sortField))
			{
				STUtils.STMsgBox(Properties.Resources.kstidNoToolboxSortFieldSpecified);
				return;
			}

			// Find the field information for the specified sort field.
			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[sortField];
			if (fieldInfo == null)
			{
				string msg = Properties.Resources.kstidInvalidToolboxSortField;
				STUtils.STMsgBox(string.Format(msg, sortField));
				return;
			}

			// Get the value indicating what record to jump to.
			string jumpValue = (fieldInfo.IsPhonetic ?
				GetPhoneticJumpWord(recEntry) : recEntry[sortField]);

			if (string.IsNullOrEmpty(jumpValue))
				return;

			// Write the value to the registry.
			RegistryKey regKey =
			    Registry.CurrentUser.CreateSubKey(@"Software\SantaFe\Focus\Word");

			regKey.SetValue(null, jumpValue, RegistryValueKind.String);

			// Inform anyone who cares (namely Toolbox) that a jump is being requested.
			uint WM_SANTA_FE_FOCUS = STUtils.RegisterWindowMessage("SantaFeFocus");
			STUtils.PostMessage(STUtils.HWND_BROADCAST, WM_SANTA_FE_FOCUS, 4, 0);
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
			Dictionary<int, string> tempCache = TempRecordCache.Load();
			string jumpValue = tempCache[recEntry.Id];
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
			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo.GuidField;
			string url = SIL.Pa.Data.FwDBAccessInfo.JumpUrl;

			if (fieldInfo != null && !string.IsNullOrEmpty(url))
			{
				url = string.Format(url, recEntry[fieldInfo.FieldName],
					recEntry.DataSource.FwDataSourceInfo.MachineName,
					recEntry.DataSource.FwDataSourceInfo.DBName);

				// Spaces aren't allowed in the URL. They should be converted to '+'.
				url = url.Trim().Replace(' ', '+');

				try
				{
					if (m_showFwJumpUrlDlg)
					{
						using (EditFwUrlDlg dlg = new EditFwUrlDlg(url))
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
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to edit the specified record in Speech Analyzer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EditRecordInSA(WordCacheEntry wcentry, string callingApp)
		{
			// Get the audio file field.
			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo.AudioFileField;
			if (fieldInfo == null)
			{
				STUtils.STMsgBox(Properties.Resources.kstidNoAudioField);
				return;
			}

			// Get the audio file and make sure it exists.
			string audioFile = wcentry[fieldInfo.FieldName];
			if (string.IsNullOrEmpty(audioFile) || !File.Exists(audioFile))
			{
				STUtils.STMsgBox(Properties.Resources.kstidAudioFileMissingMsg);
				return;
			}

			// Make sure SA exists.
			string saPath = AudioPlayer.GetSaPath();
			if (saPath == null || !File.Exists(saPath))
			{
				STUtils.STMsgBox(Properties.Resources.kstidSAMissingMsg);
				return;
			}

			string lstFile = GetSaListFile(wcentry, audioFile, callingApp);

			// Start SA.
			Process prs = new Process();
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
			Process prs = sender as Process;
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
				foreach (Process prs in s_saProcesses)
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
			PaFieldInfo fieldInfo = PaApp.FieldInfo.AudioFileOffsetField;
			if (fieldInfo == null || !ulong.TryParse(wcentry[fieldInfo.FieldName], out offset))
				offset = 0;

			// Get the utterance's length.
			ulong length;
			fieldInfo = PaApp.FieldInfo.AudioFileLengthField;
			if (fieldInfo == null || !ulong.TryParse(wcentry[fieldInfo.FieldName], out length))
				length = 0;

			// Create the contents for the SA list file.
			string saListFileContent = string.Format(m_saListFileContentFmt,
				new object[] { callingApp, audioFile, offset, offset + length });
			saListFileContent = STUtils.ConvertLiteralNewLines(saListFileContent);

			// Write the list file.
			string lstFile = Path.GetTempFileName();
			File.AppendAllText(lstFile, saListFileContent);

			return lstFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RestoreAppIfRunning(string processName)
		{
			Process[] prs = Process.GetProcessesByName(processName);

			if (prs != null && prs.Length > 0)
			{
				WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
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
