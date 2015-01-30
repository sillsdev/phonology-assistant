using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Localization;
using Microsoft.Win32;
using SIL.Pa.DataSource.Sa;
using SIL.Pa.UI.Dialogs;
using SilTools;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
// FIXME Linux - make this work in Linux (really only useful once Speech Analyzer is ported)
#if !__MonoCS__
	public class AudioPlayer
	{
		[DllImport("winmm.dll")]
		private extern static int mciSendString(string command, IntPtr responseBuffer,
			int responseBufferLength, IntPtr hwndCallback);

		[DllImport("winmm.dll")]
		private extern static int mciSendString(string command, StringBuilder responseBuffer,
			int responseBufferLength, IntPtr hwndCallback);

		private const string kDeviceName = "SILAudio";
		private static bool s_playbackInProgress;

		private string m_lstFile;
		private const string kSaListFileContentFmt = "[Settings]\nCallingApp={0}\nShowWindow=Hide\n" +
			"[AudioFiles]\nFile0={1}\n[Commands]\nCommand0=SelectFile(0)\n" +
			"Command1=Playback({2},,{3},{4})\nCommand2=Return(1)";

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint GetShortPathName(
           [MarshalAs(UnmanagedType.LPTStr)]string lpszLongPath,
           [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath,
           uint cchBuffer);


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not audio is being played back.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsPlaybackInProgress
		{
			get { return s_playbackInProgress; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Plays the specified audio file beginning with the specified time in the file
		/// and ending with the specified end time.
		/// </summary>
		/// <param name="soundFile">Full name and path to the audio file to play.</param>
		/// <param name="from">Starting point, in milliseconds, for playback.</param>
		/// <param name="to">Ending point, in milliseconds, for playback.</param>
		/// ------------------------------------------------------------------------------------
		public void Play(string soundFile, long from, long to)
		{
			if (!File.Exists(soundFile))
				return;

			// First, make sure playback is stopped.
			Stop();

			StringBuilder buffer = new StringBuilder(128);

            // need to get the old DOS 8.3 form of the file name so that Unicode characters in the name won't
            // prevent the file from playing
            soundFile = GetShortName(soundFile);

			// Open audio device
			string command = "open \"" + soundFile + "\" type mpegvideo alias " + kDeviceName;
			int err = mciSendString(command, IntPtr.Zero, 0, IntPtr.Zero);

			if (err != 0)
				return;

			// Build the command string for playback
			command = "play " + kDeviceName;
			if (from >= 0)
			{
				command += string.Format(" from {0}", from);
				if (to > from)
					command += string.Format(" to {0}", to);
			}

			// Play audio
			err = mciSendString(command, IntPtr.Zero, 0, IntPtr.Zero);

			if (err == 0)
			{
				s_playbackInProgress = true;

				// Monitor the playback status.
				command = string.Format("status {0} mode", kDeviceName);
				do
				{
					Application.DoEvents();
					mciSendString(command, buffer, buffer.Capacity, IntPtr.Zero);
				}
				while (buffer.ToString().ToLower() == "playing");

				s_playbackInProgress = false;
			}

			// Close the audio device.
			command = string.Format("close {0}", kDeviceName);
			mciSendString(command, IntPtr.Zero, 0, IntPtr.Zero);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If there is an audio file being played back, this method stops the playback.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (s_playbackInProgress)
				mciSendString("stop " + kDeviceName, IntPtr.Zero, 0, IntPtr.Zero);

			s_playbackInProgress = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates a byte value (assumed to be within the audio data portion of a wave
		/// file) to its equivalent value in milliseconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static long ByteValueToMilliseconds(long byteVal, int channels,
			long samplesPerSec, int bitsPerSample)
		{
			int bytesPerSample = (bitsPerSample / 8) * channels;
			long bytesPerSecond = bytesPerSample * samplesPerSec;

			return (long)((byteVal / (double)bytesPerSecond) * 1000f);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the specified millisecond value to a byte offset used (and only understood
		/// by SA) for altered speed playback in SA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static long MillisecondValueToBytes(long millisecondVal, string filename)
		{
			using (var reader = new AudioReader())
			{
				if (reader.Initialize(filename) == AudioReader.InitResult.FileNotFound)
					return 0;

				// Assume the bytes per second is 44100, which it will be if the audio file
				// is not a wave file.
				long bytesPerSecond = 44100;

				if (reader.IsValidWaveFile())
					bytesPerSecond = reader.BytesPerSecond;

				return (millisecondVal * bytesPerSecond) / 1000;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Plays back an utterance at slowed or increased speed using Speech Analyzer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Process AlteredSpeedPlayback(string callingApp, string soundFile, long from,
			long to, int speed)
		{
			// Make sure the wave file exists. If not, don't return false since
			// returning false is reserved for the condition when SA cannot be found.
			if (!File.Exists(soundFile))
				return null;

			// Make sure SA can be found.
			string saLoc = GetSaPath();
			if (saLoc == null)
			{
				var msg = LocalizationManager.GetString("Miscellaneous.Messages.ProblemFindingSAForPlaybackMsg",
					"Speech Analyzer 3.0.1 is required to playback utterances at speeds other than " +
					"100%, but it is not installed. Please install Speech Analyzer 3.0.1 and try again.",
					"Message displayed when SA 3.0.1 is not installed and the user is attempting to playback audio at a speed other than 100 percent.");

				using (var dlg = new DownloadSaDlg(msg))
					dlg.ShowDialog();

				return null;
			}

			// Create the contents for the SA list file.
			string saListFileContent = string.Format(kSaListFileContentFmt,
				new object[] { callingApp, soundFile, speed,
					(from >= 0 ? from.ToString() : string.Empty),
					(to >= 0 && to > from ? to.ToString() : string.Empty)});

			saListFileContent = Utils.ConvertLiteralNewLines(saListFileContent);

			// Write the list file.
			m_lstFile = Path.GetTempFileName();
			File.AppendAllText(m_lstFile, saListFileContent);
			
			// Start SA.
			Process prs = new Process();
			prs.StartInfo.FileName = "\"" + saLoc + "\"";
			prs.StartInfo.Arguments = "-l " + m_lstFile;
			prs.EnableRaisingEvents = true;
			prs.Exited += SA_Exited;
			prs.Start();

			return prs;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the list file gets deleted when the SA process exits.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void SA_Exited(object sender, EventArgs e)
		{
			if (File.Exists(m_lstFile))
				File.Delete(m_lstFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Looks in the registry to find the path to sa.exe.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetSaPath()
		{
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(@"Record\{6EAC0C98-FF43-3780-AE79-B291998FED12}\4.0.0.0");
            string saLoc = (regKey == null ? null : regKey.GetValue("CodeBase", null) as string);
            if (saLoc != null)
            {
                saLoc = (saLoc.Substring(0, saLoc.LastIndexOf("\\")) + "\\SA.exe").Replace(@"file:///", "");
            }
            else//Old version(3.0.1)
            {
                regKey = Registry.LocalMachine.OpenSubKey(@"Software\SIL\Speech Analyzer");
                saLoc = (regKey == null ? null : regKey.GetValue("Location", null) as string);
            }
			return (string.IsNullOrEmpty(saLoc) || !File.Exists(saLoc) ? null : saLoc);
		}

        private static string GetShortName(string path)
        {
            var shortBuilder = new StringBuilder(300);
            GetShortPathName(path, shortBuilder, (uint)shortBuilder.Capacity);
            return shortBuilder.ToString();
        }


		#region MCI Error codes to use some day... maybe
		// ENHANCE: Some day, check the error code returned by mciSendString against this list.
		/* MCI error return values */
		//#define MCIERR_BASE 256
		//#define MCIERR_INVALID_DEVICE_ID        (MCIERR_BASE + 1)
		//#define MCIERR_UNRECOGNIZED_KEYWORD     (MCIERR_BASE + 3)
		//#define MCIERR_UNRECOGNIZED_COMMAND     (MCIERR_BASE + 5)
		//#define MCIERR_HARDWARE                 (MCIERR_BASE + 6)
		//#define MCIERR_INVALID_DEVICE_NAME      (MCIERR_BASE + 7)
		//#define MCIERR_OUT_OF_MEMORY            (MCIERR_BASE + 8)
		//#define MCIERR_DEVICE_OPEN              (MCIERR_BASE + 9)
		//#define MCIERR_CANNOT_LOAD_DRIVER       (MCIERR_BASE + 10)
		//#define MCIERR_MISSING_COMMAND_STRING   (MCIERR_BASE + 11)
		//#define MCIERR_PARAM_OVERFLOW           (MCIERR_BASE + 12)
		//#define MCIERR_MISSING_STRING_ARGUMENT  (MCIERR_BASE + 13)
		//#define MCIERR_BAD_INTEGER              (MCIERR_BASE + 14)
		//#define MCIERR_PARSER_INTERNAL          (MCIERR_BASE + 15)
		//#define MCIERR_DRIVER_INTERNAL          (MCIERR_BASE + 16)
		//#define MCIERR_MISSING_PARAMETER        (MCIERR_BASE + 17)
		//#define MCIERR_UNSUPPORTED_FUNCTION     (MCIERR_BASE + 18)
		//#define MCIERR_FILE_NOT_FOUND           (MCIERR_BASE + 19)
		//#define MCIERR_DEVICE_NOT_READY         (MCIERR_BASE + 20)
		//#define MCIERR_INTERNAL                 (MCIERR_BASE + 21)
		//#define MCIERR_DRIVER                   (MCIERR_BASE + 22)
		//#define MCIERR_CANNOT_USE_ALL           (MCIERR_BASE + 23)
		//#define MCIERR_MULTIPLE                 (MCIERR_BASE + 24)
		//#define MCIERR_EXTENSION_NOT_FOUND      (MCIERR_BASE + 25)
		//#define MCIERR_OUTOFRANGE               (MCIERR_BASE + 26)
		//#define MCIERR_FLAGS_NOT_COMPATIBLE     (MCIERR_BASE + 28)
		//#define MCIERR_FILE_NOT_SAVED           (MCIERR_BASE + 30)
		//#define MCIERR_DEVICE_TYPE_REQUIRED     (MCIERR_BASE + 31)
		//#define MCIERR_DEVICE_LOCKED            (MCIERR_BASE + 32)
		//#define MCIERR_DUPLICATE_ALIAS          (MCIERR_BASE + 33)
		//#define MCIERR_BAD_CONSTANT             (MCIERR_BASE + 34)
		//#define MCIERR_MUST_USE_SHAREABLE       (MCIERR_BASE + 35)
		//#define MCIERR_MISSING_DEVICE_NAME      (MCIERR_BASE + 36)
		//#define MCIERR_BAD_TIME_FORMAT          (MCIERR_BASE + 37)
		//#define MCIERR_NO_CLOSING_QUOTE         (MCIERR_BASE + 38)
		//#define MCIERR_DUPLICATE_FLAGS          (MCIERR_BASE + 39)
		//#define MCIERR_INVALID_FILE             (MCIERR_BASE + 40)
		//#define MCIERR_NULL_PARAMETER_BLOCK     (MCIERR_BASE + 41)
		//#define MCIERR_UNNAMED_RESOURCE         (MCIERR_BASE + 42)
		//#define MCIERR_NEW_REQUIRES_ALIAS       (MCIERR_BASE + 43)
		//#define MCIERR_NOTIFY_ON_AUTO_OPEN      (MCIERR_BASE + 44)
		//#define MCIERR_NO_ELEMENT_ALLOWED       (MCIERR_BASE + 45)
		//#define MCIERR_NONAPPLICABLE_FUNCTION   (MCIERR_BASE + 46)
		//#define MCIERR_ILLEGAL_FOR_AUTO_OPEN    (MCIERR_BASE + 47)
		//#define MCIERR_FILENAME_REQUIRED        (MCIERR_BASE + 48)
		//#define MCIERR_EXTRA_CHARACTERS         (MCIERR_BASE + 49)
		//#define MCIERR_DEVICE_NOT_INSTALLED     (MCIERR_BASE + 50)
		//#define MCIERR_GET_CD                   (MCIERR_BASE + 51)
		//#define MCIERR_SET_CD                   (MCIERR_BASE + 52)
		//#define MCIERR_SET_DRIVE                (MCIERR_BASE + 53)
		//#define MCIERR_DEVICE_LENGTH            (MCIERR_BASE + 54)
		//#define MCIERR_DEVICE_ORD_LENGTH        (MCIERR_BASE + 55)
		//#define MCIERR_NO_INTEGER               (MCIERR_BASE + 56)

		//#define MCIERR_WAVE_OUTPUTSINUSE        (MCIERR_BASE + 64)
		//#define MCIERR_WAVE_SETOUTPUTINUSE      (MCIERR_BASE + 65)
		//#define MCIERR_WAVE_INPUTSINUSE         (MCIERR_BASE + 66)
		//#define MCIERR_WAVE_SETINPUTINUSE       (MCIERR_BASE + 67)
		//#define MCIERR_WAVE_OUTPUTUNSPECIFIED   (MCIERR_BASE + 68)
		//#define MCIERR_WAVE_INPUTUNSPECIFIED    (MCIERR_BASE + 69)
		//#define MCIERR_WAVE_OUTPUTSUNSUITABLE   (MCIERR_BASE + 70)
		//#define MCIERR_WAVE_SETOUTPUTUNSUITABLE (MCIERR_BASE + 71)
		//#define MCIERR_WAVE_INPUTSUNSUITABLE    (MCIERR_BASE + 72)
		//#define MCIERR_WAVE_SETINPUTUNSUITABLE  (MCIERR_BASE + 73)

		//#define MCIERR_SEQ_DIV_INCOMPATIBLE     (MCIERR_BASE + 80)
		//#define MCIERR_SEQ_PORT_INUSE           (MCIERR_BASE + 81)
		//#define MCIERR_SEQ_PORT_NONEXISTENT     (MCIERR_BASE + 82)
		//#define MCIERR_SEQ_PORT_MAPNODEVICE     (MCIERR_BASE + 83)
		//#define MCIERR_SEQ_PORT_MISCERROR       (MCIERR_BASE + 84)
		//#define MCIERR_SEQ_TIMER                (MCIERR_BASE + 85)
		//#define MCIERR_SEQ_PORTUNSPECIFIED      (MCIERR_BASE + 86)
		//#define MCIERR_SEQ_NOMIDIPRESENT        (MCIERR_BASE + 87)

		//#define MCIERR_NO_WINDOW                (MCIERR_BASE + 90)
		//#define MCIERR_CREATEWINDOW             (MCIERR_BASE + 91)
		//#define MCIERR_FILE_READ                (MCIERR_BASE + 92)
		//#define MCIERR_FILE_WRITE               (MCIERR_BASE + 93)

		//#define MCIERR_NO_IDENTITY              (MCIERR_BASE + 94)
		#endregion
	}
#else
	public class AudioPlayer
	{
		private static int mciSendString (string command, IntPtr responseBuffer,
			int responseBufferLength, IntPtr hwndCallback) { return 0; }
		private static int mciSendString (string command, StringBuilder responseBuffer,
			int responseBufferLength, IntPtr hwndCallback) { return 0; }
		private const string kDeviceName = "SILAudio";
		private const string kSaListFileContentFmt = "";
		public static bool IsPlaybackInProgress {
			get { return false; }
		}
		public void Play(string soundFile, long from, long to) {}
		public void Stop() {}
		public static long ByteValueToMilliseconds (long byteVal, int channels,
			long samplesPerSec, int bitsPerSample) { return 0; }
		public static long MillisecondValueToBytes(long millisecondVal, string filename) { return 0; }
		public Process AlteredSpeedPlayback(string callingApp, string soundFile, long from,
			long to, int speed) { return null; }
		void SA_Exited (object sender, EventArgs e) {}
		public static string GetSaPath() { return ""; }
	}
#endif
}
