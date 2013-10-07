// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2011, SIL International. All Rights Reserved.
// <copyright from='2009' to='2011' company='SIL International'>
//		Copyright (c) 2011, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FieldWorksHelper.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SIL.PaToFdoInterfaces
{
	/// ----------------------------------------------------------------------------------------
	public class PaFieldWorksHelper : IDisposable
	{
		private static string s_fwInstallPath;
		private static Assembly s_assembly;
		private static string[] s_regKeyPaths = new string[] { @"Software\SIL\FieldWorks\8",  @"Software\SIL\FieldWorks\7.0" };
		private IPaLexicalInfo _lexEntryServer;

		#region Construction and disposal Members
		/// ------------------------------------------------------------------------------------
		public PaFieldWorksHelper()
		{
			CreateLexEntryServer();
		}

		/// ------------------------------------------------------------------------------------
		public PaFieldWorksHelper(string regKeyPath) 
		{
			s_regKeyPaths = new string[] { regKeyPath };
			CreateLexEntryServer();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_lexEntryServer != null && _lexEntryServer is IDisposable)
			    ((IDisposable)_lexEntryServer).Dispose();
		}

		#endregion

		#region Static Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether FieldWorks is installed on the computer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsFwLoaded
		{
			get { return (FwInstallPath != null && Directory.Exists(FwInstallPath)); }
		}

		/// ------------------------------------------------------------------------------------
		public static string FwInstallPath
		{
			get
			{
				if (s_fwInstallPath == null)
				{
					RegistryKey regkey = null;
					foreach (var key in s_regKeyPaths)
					{
						try // exception on Linux because registry key tree does not exist
						{
							// FIXME Linux - allow working with FieldWorks for Linux
							regkey = Registry.LocalMachine.OpenSubKey(key);
							if (regkey != null)
								break;
						}
						catch (Exception) {}
					}

					if (regkey != null)
					{
						s_fwInstallPath = regkey.GetValue("FwExeDir", null) as string;
						if (s_fwInstallPath == null)
							s_fwInstallPath = regkey.GetValue("RootCodeDir", null) as string;
					}

					if (s_fwInstallPath != null)
					{
						// On a development machine, this points to distfiles so
						// modify the path to point to the output\debug folder.
                        if (s_fwInstallPath.ToLower().EndsWith("\\distfiles", StringComparison.Ordinal))
							s_fwInstallPath += @"\..\output\debug";
					}
				}

				return s_fwInstallPath;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static Assembly FieldWorksAssembly
		{
			get
			{
				if (s_assembly == null)
				{
					try
					{
						// Load the assembly that links us to FieldWorks
						s_assembly = Assembly.LoadFrom(Path.Combine(FwInstallPath, "FieldWorks.exe"));
					}
					catch
					{
						s_fwInstallPath = null;
					}
				}

				return s_assembly;
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a dialog that allows the user to choose an FW language project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowFwOpenProject(Form owner, ref Rectangle dialogBounds,
			ref int dialogSplitterPos, out string name, out string server)
		{
			return _lexEntryServer.ShowOpenProject(owner, ref dialogBounds,
				ref dialogSplitterPos, out name, out server);
		}

		/// ------------------------------------------------------------------------------------
		public bool Initialize(string name, string server, int timeToWaitForProcessStart,
			int timeToWaitForLoadingData)
		{
			return _lexEntryServer.Initialize(name, server, timeToWaitForProcessStart,
				timeToWaitForLoadingData);
		}

		/// ------------------------------------------------------------------------------------
		public bool LoadOnlyWritingSystems(string name, string server, int timeToWaitForProcessStart,
			int timeToWaitForLoadingData)
		{
			return _lexEntryServer.LoadOnlyWritingSystems(name, server, timeToWaitForProcessStart,
				timeToWaitForLoadingData);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<IPaLexEntry> LexEntries
		{
			get { return _lexEntryServer.LexEntries; }
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<IPaWritingSystem> WritingSystems
		{
			get { return _lexEntryServer.WritingSystems; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an instance of a class that serves up lexical entries from a FW database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CreateLexEntryServer()
		{
			if (!IsFwLoaded)
				throw new Exception("FieldWorks is not installed.");

			if (FieldWorksAssembly == null)
				throw new Exception("Error loading FieldWorks.exe");

			// Find a class type in the assembly that implements our desired interface.
			var type = s_assembly.GetTypes().SingleOrDefault(x => x.GetInterface("IPaLexicalInfo") != null);
			_lexEntryServer = (IPaLexicalInfo)s_assembly.CreateInstance(type.FullName);
		
			if (_lexEntryServer == null)
				throw new Exception("Error creating instance of IPaLexicalInfo.");
		}
	}
}
