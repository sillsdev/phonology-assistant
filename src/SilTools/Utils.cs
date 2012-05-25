using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace SilTools
{
	public static class Utils
	{
		public const char kObjReplacementChar = '\uFFFC';
		internal static ISplashScreen s_splashScreen;
		private static bool s_msgBoxJustShown;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not calls to MsgBox will actaully show
		/// a message box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool SuppressMsgBoxInteractions { get; set; }

		#region OS-specific stuff
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

#if !__MonoCS__
		[DllImport("user32")]
		public static extern int UpdateWindow(IntPtr hwnd);
#else
		public static int UpdateWindow(IntPtr hwnd)
		{
			Console.WriteLine("Warning--using unimplemented method UpdateWindow"); // FIXME Linux
			return(0);
		}
#endif

#if !__MonoCS__
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int FindWindowEx(IntPtr hWnd, int hwndChildAfter,
			string windowClass, string windowName);
#else
		public static int FindWindowEx(IntPtr hWnd, int hwndChildAfter,
			string windowClass, string windowName)
		{
			Console.WriteLine("Warning--using unimplemented method FindWindowEx"); // FIXME Linux
			return(0);
		}
#endif

#if !__MonoCS__
		[DllImport("User32.dll")]
		public extern static bool GetWindowRect(IntPtr hWnd, out RECT rect);
#else
		public static bool GetWindowRect(IntPtr hWnd, out RECT rect)
		{
			Console.WriteLine("Warning--using unimplemented method GetWindowRect"); // FIXME Linux
			rect.left = 0;
			rect.right = 0;
			rect.top = 0;
			rect.bottom = 0;
			return(false);
		}
#endif

#if !__MonoCS__
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
#else
		public static void SendMessage(IntPtr hWnd, int msg, int wParam, int lParam)
		{
			if(msg != PaintingHelper.WM_NCPAINT) { // repaint
				Console.WriteLine("Warning--using unimplemented method SendMessage"); // FIXME Linux
			}
			return;
		}
#endif

#if !__MonoCS__
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(int hWnd, uint msg, int wParam, int lParam);
#else
		public static bool PostMessage(int hWnd, uint msg, int wParam, int lParam)
		{
			Console.WriteLine("Warning--using unimplemented method PostMessage"); // FIXME Linux
			return(false);
		}
#endif

#if !__MonoCS__
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint RegisterWindowMessage(string msgName);
#else
		public static uint RegisterWindowMessage(string msgName)
		{
			Console.WriteLine("Warning--using unimplemented method RegisterWindowMessage"); // FIXME Linux
			return(0);
		}
#endif

		private const int WM_SETREDRAW = 0xB;
		public const int HWND_BROADCAST = 0xFFFF;

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
#if !__MonoCS__
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		extern public static void GlobalMemoryStatus(ref MemoryStatus ms);
#else
		public static void GlobalMemoryStatus(ref MemoryStatus ms)
		{
			Console.WriteLine("Warning--using unimplemented method GlobalMemoryStatus"); // FIXME Linux
			return;
		}
#endif

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the splash screen is showing, this will force it to be closed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ForceCloseOfSplashScreen()
		{
			if (s_splashScreen != null)
				s_splashScreen.Close();
		}

		/// ------------------------------------------------------------------------------------
		public static void CenterFormInScreen(Form frm)
		{
			Rectangle rc = Screen.GetWorkingArea(frm);
			if (rc == Rectangle.Empty)
				rc = Screen.PrimaryScreen.WorkingArea;

			if (frm.Width > rc.Width)
				frm.Width = rc.Width;

			if (frm.Height > rc.Height)
				frm.Height = rc.Height;

			frm.Location = new Point((rc.Width - frm.Width) / 2, (rc.Height - frm.Height) / 2);
		}

		/// ------------------------------------------------------------------------------------
		public static Image GetSilLogo()
		{
			return Properties.Resources.kimidSilLogo;
		}

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
			return (from ManagementObject mo in moc select (ulong)mo.Properties["FreeSpace"].Value).FirstOrDefault();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes relPath relative to fixedPath, but only if relPath is rooted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MakeRelativePath(string fixedPath, string relPath)
		{
			if (!Path.IsPathRooted(fixedPath))
				throw new ArgumentException("Fixed path is not rooted.", "fixedPath");

			if (!Path.IsPathRooted(relPath))
				throw new ArgumentException("Relative path is not rooted.", "relPath");

			if (relPath.IndexOf(fixedPath, StringComparison.Ordinal) != 0)
				return relPath;

			relPath = relPath.Remove(0, fixedPath.Length);
			return relPath.TrimStart(Path.DirectorySeparatorChar);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes the specified name safe to use as file name in the target OS. This is done
		/// by replacing all invalid file name characters found in the specified file name
		/// with the specified replacement character. Passing '\0' as the replacement character
		/// will remove invalid characters without replacing any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MakeSafeFileName(string fileName, char replacementChar)
		{
			string replacement = (replacementChar == '\0' ?
				string.Empty : replacementChar.ToString(CultureInfo.InvariantCulture));

			fileName = Path.GetInvalidFileNameChars()
				.Aggregate(fileName, (curr, c) => curr.Replace(c.ToString(CultureInfo.InvariantCulture), replacement));

			return fileName.Trim();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Since a file path may contain '\n' and passing a string to STMsgBox will convert
		/// those to a new line character (which would not be good for a file path), this
		/// method will go through the specified file path and prepare it to be properly
		/// displayed in a message box via the STMsgBox method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string PrepFilePathForMsgBox(string filepath)
		{
			if (filepath == null)
				return string.Empty;

			filepath = filepath.Replace(Environment.NewLine, kObjReplacementChar.ToString(CultureInfo.InvariantCulture));
			return filepath.Replace("\\n", kObjReplacementChar.ToString(CultureInfo.InvariantCulture));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a speech tools message box with just an OK button and an information icon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DialogResult MsgBox(string msg)
		{
			return MsgBox(msg, MessageBoxButtons.OK);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a speech tools message box with just an OK button and an information icon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DialogResult MsgBox(string msg, MessageBoxIcon icon)
		{
			return MsgBox(msg, MessageBoxButtons.OK, icon);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a speech tools message box with an icon that is determined by the buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DialogResult MsgBox(string msg, MessageBoxButtons buttons)
		{
			MessageBoxIcon icon;

			if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
				icon = MessageBoxIcon.Question;
			else if (buttons == MessageBoxButtons.AbortRetryIgnore || buttons == MessageBoxButtons.RetryCancel)
				icon = MessageBoxIcon.Exclamation;
			else
				icon = MessageBoxIcon.Information;

			return MsgBox(msg, buttons, icon);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a speech tools message box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DialogResult MsgBox(string msg, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			if (SuppressMsgBoxInteractions)
				return DialogResult.None;

			// If there a splash screen showing, then close it. Otherwise,
			// the message box will popup behind the splash screen.
			if (s_splashScreen != null)
				s_splashScreen.Close();

			s_msgBoxJustShown = true;
			msg = ConvertLiteralNewLines(msg);
			msg = msg.Replace(kObjReplacementChar.ToString(CultureInfo.InvariantCulture), Environment.NewLine);
			return MessageBox.Show(msg, Application.ProductName, buttons, icon);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a flag indicating whether or not the MsgBox method was just used
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
			text = text.Replace("&&", kObjReplacementChar.ToString(CultureInfo.InvariantCulture));
			text = text.Replace("&", string.Empty);
			return text.Replace(kObjReplacementChar.ToString(CultureInfo.InvariantCulture), "&");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the specified string for literal newline characters '\\n' and replaces
		/// them with real new line (i.e. '\n') characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ConvertLiteralNewLines(string text)
		{
			return text.Replace("\\n", Environment.NewLine);
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
				// FIXME Linux - fix MyDocuments here similar to way we did it in App.InitializeProjectFolder()
				string silSwPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				silSwPath = Path.Combine(silSwPath, @"SIL Software");

				// Check if an entry in the registry specifies the path. If not, create it.
				const string keyName = @"Software\SIL";
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
				const string keyName = @"Software\SIL";
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
		/// Gets the full path and filename for the specified file, first the application's
		/// startup directory is checked. If that fails, then the executing assemblies
		/// path is checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetLocalPath(string filename, bool mustExist)
		{
			return GetLocalPath(Assembly.GetCallingAssembly(), filename, mustExist);
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
			// look in the path where the specified assembly is located.
			if (!File.Exists(path) && mustExist)
			{
				// CodeBase prepends "file:/" (Win) or "file:" (Linux), which must be removed.
				int prefixLen = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? 5 : 6;
				string dir = Path.GetDirectoryName(assembly.CodeBase).Substring(prefixLen);
				path = Path.Combine(dir, filename);
			}

			return path;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of the assembly in which exists the method that called this
		/// method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetMyAssemblyPath()
		{
			string asmPath = Assembly.GetCallingAssembly().CodeBase;

			// CodeBase prepends "file:/" (Win) or "file:" (Linux), which must be removed.
			int prefixLen = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? 5 : 6;
			asmPath = asmPath.Substring(prefixLen);

			return Path.GetDirectoryName(asmPath);
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
		/// Serializes an object to an XML string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string SerializeToString<T>(T data)
		{
			try
			{
				StringBuilder output = new StringBuilder();
				using (StringWriter writer = new StringWriter(output))
				{
					XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
					nameSpace.Add(string.Empty, string.Empty);
					XmlSerializer serializer = new XmlSerializer(typeof(T));
					serializer.Serialize(writer, data, nameSpace);
					writer.Close();
				}

				return (output.Length == 0 ? null : output.ToString());
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.Fail(e.Message);
			}
			
			return null;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes XML from the specified string to an object of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static T DeserializeFromString<T>(string input) where T : class
		{
			Exception e;
			return (DeserializeFromString<T>(input, out e));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes XML from the specified string to an object of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static T DeserializeFromString<T>(string input, out Exception e) where T : class
		{
			e = null;

			try
			{
				if (string.IsNullOrEmpty(input))
					return null;

				// Whitespace is not allowed before the XML declaration,
				// so get rid of any that exists.
				input = input.TrimStart();

				using (TextReader reader = new StringReader(input))
				{
					XmlSerializer deserializer = new XmlSerializer(typeof(T));
					return (T)deserializer.Deserialize(reader);
				}
			}
			catch (Exception outEx)
			{
				e = outEx;
			}

			return null;
		}

		#endregion

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
		/// in the specified culture-specific format. 
		/// This will handle situations when a string containing a decimal point is being
		/// parsed on a computer whose locale is European (e.g. German) where they use a
		/// comma for the decimal separator.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool TryFloatParse(string input, CultureInfo ci, out float output)
		{
			if (ci == null)
			{
				if (float.TryParse(input, out output))
					return true;

				// The first attempt failed so now try parsing with a culture whose number
				// system decimal separator is known to be a period.
				ci = CultureInfo.InvariantCulture;
			}

			return float.TryParse(input, NumberStyles.Number, ci.NumberFormat, out output);
		}

		/// ------------------------------------------------------------------------------------
		public static void WaitCursors(bool turnOn)
		{
			Application.UseWaitCursor = turnOn;

			foreach (var frm in Application.OpenForms.Cast<Form>().Where(frm => !frm.InvokeRequired))
				frm.Cursor = (turnOn ? Cursors.WaitCursor : Cursors.Default);

			try
			{
				// I hate doing this, but setting the cursor property in .Net
				// often doesn't otherwise take effect until it's too late.
				Application.DoEvents();
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turns window redrawing on or off. After turning on, the window will be invalidated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void SetWindowRedraw(Control ctrl, bool turnOn)
		{
			SetWindowRedraw(ctrl, turnOn, true);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void SetWindowRedraw(Control ctrl, bool turnOn,
			bool invalidateAfterTurningOn)
		{
			if (ctrl != null && !ctrl.IsDisposed && ctrl.IsHandleCreated)
			{
#if !__MonoCS__
				SendMessage(ctrl.Handle, WM_SETREDRAW, (turnOn ? 1 : 0), 0);
#else
				if (turnOn)
					ctrl.ResumeLayout(invalidateAfterTurningOn);
				else
					ctrl.SuspendLayout();
#endif
				if (turnOn && invalidateAfterTurningOn)
					ctrl.Invalidate(true);
			}
		}
	}
}
