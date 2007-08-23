using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;
using SilEncConverters22;

namespace SIL.SpeechTools.Utils
{
	public static class STUtils
	{
		public const char kObjReplacementChar = '\uFFFC';
		private static EncConverters s_ec = null;
		internal static ISplashScreen s_splashScreen = null;
		private static bool s_msgBoxJustShown = false;

		#region Windows 32 stuff
		/// <summary></summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct RECT
		{
			/// <summary></summary>
			public int left;
			/// <summary></summary>
			public int top;
			/// <summary></summary>
			public int right;
			/// <summary></summary>
			public int bottom;
		}
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int FindWindowEx(IntPtr hWnd, int hwndChildAfter,
			string windowClass, string windowName);

		/// <summary></summary>
		[DllImport("User32.dll")]
		public extern static bool GetWindowRect(IntPtr hWnd, out RECT rect);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		public const int HWND_BROADCAST = 0xFFFF;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(int hWnd, uint msg, int wParam, int lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint RegisterWindowMessage(string msgName);

		/// <summary>
		/// The <c>MemoryStatus</c> structure contains information about the current 
		/// state of both physical and virtual memory.
		/// </summary>
		public struct MemoryStatus
		{
			/// <summary>
			/// Size of the <c>MemoryStatus</c> data structure, in bytes. You do not 
			/// need to set this member before calling the <see cref="GlobalMemoryStatus"/> 
			/// function; the function sets it. 
			/// </summary>
			public uint dwLength;
			/// <summary>See MSDN documentation</summary>
			public uint dwMemoryLoad;
			/// <summary>Total size of physical memory, in bytes.</summary>
			public uint dwTotalPhys;
			/// <summary>Size of physical memory available, in bytes. </summary>
			public uint dwAvailPhys;
			/// <summary>Size of the committed memory limit, in bytes. </summary>
			public uint dwTotalPageFile;
			/// <summary>Size of available memory to commit, in bytes.</summary>
			public uint dwAvailPageFile;
			/// <summary>Total size of the user mode portion of the virtual address space of 
			/// the calling process, in bytes.</summary>
			public uint dwTotalVirtual;
			/// <summary>Size of unreserved and uncommitted memory in the user mode portion 
			/// of the virtual address space of the calling process, in bytes.</summary>
			public uint dwAvailVirtual;
		};

		/// <summary>
		/// The <c>GlobalMemoryStatus</c> function obtains information about the system's 
		/// current usage of both physical and virtual memory.
		/// </summary>
		/// <param name="ms">Pointer to a <see cref="MemoryStatus"/>  structure. The 
		/// <c>GlobalMemoryStatus</c> function stores information about current memory 
		/// availability into this structure.</param>
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		extern public static void GlobalMemoryStatus(ref MemoryStatus ms);

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the amount of free disk space on the specified drive.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ulong GetFreeDiskSpace(string drive)
		{
			string moQuery = "SELECT FreeSpace FROM Win32_LogicalDisk WHERE deviceID = '{0}'";
			moQuery = string.Format(moQuery, drive.Replace("\\", string.Empty));
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(moQuery);
			ManagementObjectCollection moc = searcher.Get();
			foreach (ManagementObject mo in moc)
				return (ulong)mo.Properties["FreeSpace"].Value;

			return 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Since a file path may contain '\n' and passing a string to STMsgBox will convert
		/// those to a new line character (which would not be good for a file path), this
		/// method will go through the specified file path and prepare it to be properly
		/// displayed in a message box via the STMsgBox method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string PrepFilePathForSTMsgBox(string filepath)
		{
			return filepath.Replace("\\n", kObjReplacementChar.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a speech tools message box with an icon that is determined by the buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DialogResult STMsgBox(string msg, MessageBoxButtons buttons)
		{
			MessageBoxIcon icon;

			if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
				icon = MessageBoxIcon.Question;
			else if (buttons == MessageBoxButtons.AbortRetryIgnore || buttons == MessageBoxButtons.RetryCancel)
				icon = MessageBoxIcon.Exclamation;
			else
				icon = MessageBoxIcon.Information;

			return STMsgBox(msg, buttons, icon);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a speech tools message box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DialogResult STMsgBox(string msg, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			// If there a splash screen showing, then close it. Otherwise,
			// the message box will popup behind the splash screen.
			if (s_splashScreen != null)
				s_splashScreen.Close();

			s_msgBoxJustShown = true;
			msg = ConvertLiteralNewLines(msg);
			msg = msg.Replace(kObjReplacementChar.ToString(), "\\n");
			return MessageBox.Show(msg, Application.ProductName, buttons, icon);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a flag indicating whether or not the STMsgBox method was just used
		/// to display a message box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool MessageBoxJustShown
		{
			get { return s_msgBoxJustShown; }
			set { s_msgBoxJustShown = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the ampersand accerlerator prefix from the specified text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string RemoveAcceleratorPrefix(string text)
		{
			text = text.Replace("&&", kObjReplacementChar.ToString());
			text = text.Replace("&", string.Empty);
			return text.Replace(kObjReplacementChar.ToString(), "&");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the specified string for literal newline characters '\\n' and replaces
		/// them with real new line (i.e. '\n') characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ConvertLiteralNewLines(string text)
		{
			return text.Replace("\\n", "\n");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a StringFormat object based on the GenericDefault string format but
		/// includes vertical centering, EllipsisCharacter trimming and the NoWrap format
		/// flag. Horizontal alignment is centered when horizontalCenter is true. Otherwise
		/// horizontal alignment is near.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static StringFormat GetStringFormat(bool horizontalCenter)
		{
			StringFormat sf = (StringFormat)StringFormat.GenericDefault.Clone();
			sf.Alignment = (horizontalCenter ? StringAlignment.Center : StringAlignment.Near);
			sf.LineAlignment = StringAlignment.Center;
			sf.Trimming = StringTrimming.EllipsisCharacter;
			sf.FormatFlags |= StringFormatFlags.NoWrap;
			return sf;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to the SIL software folder within the user's "My Documents" folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string SilSoftwarePath
		{
			get 
			{
				string silSwPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				silSwPath = Path.Combine(silSwPath, @"SIL Software");

				// Check if an entry in the registry specifies the path. If not, create it.
				string keyName = @"Software\SIL";
				RegistryKey key = Registry.CurrentUser.CreateSubKey(keyName);

				if (key != null)
				{
					string userDataRootPath = key.GetValue("UserDataLocationRoot") as string;

					// If the registry value was not found, then create it.
					if (string.IsNullOrEmpty(userDataRootPath))
						key.SetValue("UserDataLocationRoot", silSwPath);
					else
						silSwPath = userDataRootPath;

					key.Close();
				}

				// Create the folder if it doesn't exist.
				if (!Directory.Exists(silSwPath))
					Directory.CreateDirectory(silSwPath);

				return silSwPath;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to the common files within the user's "My Documents\SIL Software"
		/// folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string SilSoftwareCommonFilesPath
		{
			get 
			{
				string silSwCommonFilesPath = Path.Combine(SilSoftwarePath, "Common Files");

				// Check if an entry in the registry specifies the path. If not, create it.
				string keyName = @"Software\SIL";
				RegistryKey key = Registry.CurrentUser.CreateSubKey(keyName);

				if (key != null)
				{
					string userDataCommonPath = key.GetValue("UserDataCommonFilesLocation") as string;

					// If the registry value was not found, then create it.
					if (string.IsNullOrEmpty(userDataCommonPath))
						key.SetValue("UserDataCommonFilesLocation", silSwCommonFilesPath);
					else
						silSwCommonFilesPath = userDataCommonPath;

					key.Close();
				}

				// Create the folder if it doesn't exist.
				if (!Directory.Exists(silSwCommonFilesPath))
					Directory.CreateDirectory(silSwCommonFilesPath);

				return silSwCommonFilesPath;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to the SIL software folder within the user's "My Documents" folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string SASettingsPath
		{
			get
			{
				string saSettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
				saSettingsPath = Path.Combine(saSettingsPath, @"SIL\Speech Analyzer");

				return saSettingsPath;
			}
		}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path and filename for the specified file, first the application's
		/// startup directory is checked. If that fails, then the executing assemblies
		/// path is checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetLocalPath(string filename, bool mustExist)
		{
			return GetLocalPath(Assembly.GetExecutingAssembly(), filename, mustExist);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path and filename for the specified file, first the application's
		/// startup directory is checked. If that fails, then the executing assemblies
		/// path is checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetLocalPath(Assembly assembly, string filename, bool mustExist)
		{
			string path = Path.Combine(Application.StartupPath, filename);

			// If the file cannot be found in the application's startup
			// path (which is probably only the case when running tests),
			// look in the path where this class' assembly is located.
			if (!File.Exists(path) && mustExist)
			{
				// CodeBase prepends "file:/", which must be removed.
				string dir = Path.GetDirectoryName(assembly.CodeBase).Substring(6);
				path = Path.Combine(dir, filename);
			}

			return path;
		}

		#region Methods for XML serializing and deserializing data
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Serializes an object to the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool SerializeData(string filename, object data)
		{
			try
			{
				string filepath = GetLocalPath(filename, false);
				using (TextWriter writer = new StreamWriter(filepath))
				{
					XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
					nameSpace.Add(string.Empty, string.Empty);
					XmlSerializer serializer = new XmlSerializer(data.GetType());
					serializer.Serialize(writer, data, nameSpace);
					writer.Close();
				}

				return true;
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes data from the specified file to an object of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static object DeserializeData(string filename, Type type)
		{
			Exception e;
			return (DeserializeData(filename, type, out e));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes data from the specified file to an object of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static object DeserializeData(string filename, Type type, out Exception e)
		{
			object data;
			e = null;

			try
			{
				string filepath = GetLocalPath(filename, true);
				if (!File.Exists(filepath))
					return null;

				using (TextReader reader = new StreamReader(filepath))
				{
					XmlSerializer deserializer = new XmlSerializer(type);
					data = deserializer.Deserialize(reader);
					reader.Close();
				}
			}
			catch (Exception outEx)
			{
				data = null;
				e = outEx;
			}

			return data;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of encoding converters from the encoding converter repository
		/// installed on the computer. The setter is only for setting it to null;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static EncConverters EncodingConverters
		{
			get
			{
				if (s_ec == null)
					s_ec = new EncConverters();

				return s_ec;
			}
			set 
			{
				if (value == null)
					s_ec = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the encoding converter for the specified mapping (or encoding converter name).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static EncConverter GetConverter(string mapping)
		{
			if (EncodingConverters == null)
				return null;

			return (EncodingConverters[mapping] as EncConverter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method calls TryFloatParse with NO CultureInfo and returns the boolean result.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool TryFloatParse(string input, out float output)
		{
			return TryFloatParse(input, null, out output);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the CultureInfo is null, then this method will try a plain-vanilla float parse, 
		/// which uses the current culture's number system. If that fails, then it tries
		/// parsing using the English number system just in case the string contains a period
		/// for the decimal point.
		/// If the CultureInfo is NOT null, then this method will parse the input string
		/// in the specified  culture-specific format. 
		/// This will handle situations when a string containing a decimal point is being
		/// parsed on a computer whose locale is European (e.g. German) where they use a
		/// comma for the decimal separator.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool TryFloatParse(string input, 
			CultureInfo ci, out float output)
		{
			if (ci == null)
			{
				if (float.TryParse(input, out output))
					return true;

				// The first attempt failed so now try parsing with a culture whose number
				// system decimal separator is known to be a period.
				ci = CultureInfo.CreateSpecificCulture("en");
			}

			return float.TryParse(input, NumberStyles.Number,
					ci.NumberFormat, out output);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WaitCursors(bool turnOn)
		{
			Application.UseWaitCursor = turnOn;
			if (Application.OpenForms != null && Application.OpenForms.Count > 0)
			{
				foreach (Form frm in Application.OpenForms)
				{
					// Check if the form was created in the current thread.
					if (!frm.InvokeRequired)
						frm.Cursor = (turnOn ? Cursors.WaitCursor : Cursors.Default);
				}
			}

			Application.DoEvents();
		}
	}
}
