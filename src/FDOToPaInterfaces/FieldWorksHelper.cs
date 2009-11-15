// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace SIL.FdoToPaInterfaces
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldWorksHelper
	{
		private static string s_fwInstallPath;
		private static IPaLexicalInfo s_lexEntrySvr;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether FieldWorks is installed on the computer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsFwLoaded
		{
			get
			{
				if (s_fwInstallPath == null)
				{
					RegistryKey regkey = Registry.LocalMachine.OpenSubKey(@"Software\SIL\FieldWorks");
					if (regkey != null)
						s_fwInstallPath = regkey.GetValue("RootCodeDir", null) as string;

					if (s_fwInstallPath != null)
					{
						// On a development machine, this points to distfiles so
						// modify the path to point to the output\debug folder.
						if (s_fwInstallPath.ToLower().EndsWith("\\distfiles"))
							s_fwInstallPath += @"\..\output\debug";
					}
				}

				return (s_fwInstallPath != null && Directory.Exists(s_fwInstallPath));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an instance of a class that serves up lexical entries from a FW database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IPaLexicalInfo LexEntryServer
		{
			get
			{
				if (!IsFwLoaded)
					return null;

				if (s_lexEntrySvr == null)
				{
					Assembly asm;

					try
					{
						// Load the assembly that links us to FieldWorks
						asm = Assembly.LoadFrom(Path.Combine(s_fwInstallPath, "FDOToPa.dll"));
					}
					catch
					{
						s_fwInstallPath = null;
						return null;
					}

					// Find a class type in the assembly that implements our desired interface.
					var type = asm.GetTypes().SingleOrDefault(x => x.GetInterface("IPaLexicalInfo") != null);
					s_lexEntrySvr = (IPaLexicalInfo)asm.CreateInstance(type.FullName);
				}

				return s_lexEntrySvr;
			}
		}
	}
}
