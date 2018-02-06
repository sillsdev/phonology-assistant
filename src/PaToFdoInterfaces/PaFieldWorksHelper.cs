// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
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
using SIL.LCModel;

namespace SIL.PaToFdoInterfaces
{
	/// ----------------------------------------------------------------------------------------
	public class PaFieldWorksHelper : IDisposable, ILcmDirectories
    {
		private static string s_fwInstallPath;
		private static Assembly s_assembly;
		private IPaLexicalInfo _lexEntryServer;

		#region Construction and disposal Members
		/// ------------------------------------------------------------------------------------
		public PaFieldWorksHelper()
		{
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
	    public string ProjectsDirectory
	    {
	        get
	        {
	            var dataFolder = RegistrySettings.FallbackStringValue(@"SIL\FieldWorks\9", "ProjectsDir");
	            if (!string.IsNullOrEmpty(dataFolder)) return dataFolder;
	            dataFolder = RegistrySettings.FallbackStringValue(@"SIL\FieldWorks\8", "ProjectsDir");
	            return dataFolder ?? @"C:\ProgramData\SIL\FieldWorks\Projects";
	        }
	    }

	    const string TemplateFolder = "Templates";
	    public string TemplateDirectory
	    {
	        get
	        {
	            var dataFolder = RegistrySettings.FallbackStringValue(@"SIL\FieldWorks\9", "RootCodeDir");
	            if (!string.IsNullOrEmpty(dataFolder)) return Path.Combine(dataFolder, TemplateFolder);
	            dataFolder = RegistrySettings.FallbackStringValue(@"SIL\FieldWorks\8", "RootCodeDir");
	            return Path.Combine(dataFolder ?? @"C:\ProgramData\SIL\FieldWorks\Projects", TemplateFolder);
	        }
	    }

	    public string BackupDirectory
	    {
	        get
	        {
	            var backupFolder = RegistrySettings.FallbackStringValue(@"SIL\FirleWorks\9\ProjectBackup", "DefaultBackupDirectory");
	            if (!string.IsNullOrEmpty(backupFolder)) return backupFolder;
	            backupFolder = RegistrySettings.FallbackStringValue(@"SIL\FirleWorks\8\ProjectBackup", "DefaultBackupDirectory");
	            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
	            return backupFolder ?? Path.Combine(documents, "My FieldWorks", "Backups");
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
		    s_assembly = Assembly.GetExecutingAssembly();

            // Find a class type in the assembly that implements our desired interface.
            var type = s_assembly.GetTypes().SingleOrDefault(x => x.GetInterface("IPaLexicalInfo") != null);
			_lexEntryServer = (IPaLexicalInfo)s_assembly.CreateInstance(type.FullName);
		
			if (_lexEntryServer == null)
				throw new Exception("Error creating instance of IPaLexicalInfo.");
		}
	}
}
