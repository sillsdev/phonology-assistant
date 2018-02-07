// Copyright (c) 2009-2017 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SIL.LCModel;
using SIL.WritingSystems;
using SIL.LCModel.Utils;
using SIL.Xml;

namespace SIL.PaToFdoInterfaces
{
	/// ----------------------------------------------------------------------------------------
	public class PaLexicalInfo : IPaLexicalInfo, IDisposable
	{
		private List<PaWritingSystem> m_writingSystems;
		private List<PaLexEntry> m_lexEntries;
	    public string SelectedProject { get; set; }
	    public LcmCache m_cache;
        #region Disposable stuff
#if DEBUG
        /// <summary/>
        ~PaLexicalInfo()
		{
			Dispose(false);
		}
		#endif

		/// <summary/>
		public bool IsDisposed { get; private set; }

		/// <summary/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary/>
		protected virtual void Dispose(bool fDisposing)
		{
			Debug.WriteLineIf(!fDisposing, "****** Missing Dispose() call for " + GetType() + " *******");
			if (fDisposing && !IsDisposed)
			{
				// dispose managed and unmanaged objects
				if (m_lexEntries != null)
					m_lexEntries.Clear();

				if (m_writingSystems != null)
					m_writingSystems.Clear();
			}
			m_lexEntries = null;
			m_writingSystems = null;
			IsDisposed = true;
		}
		#endregion

		#region IPaLexicalInfo Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a dialog that allows the user to choose an FW language project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowOpenProject(Form owner, ref Rectangle dialogBounds,
			ref int dialogSplitterPos, out string name, out string server)
		{
            using (var dlg = new ChooseProject())
            {
            	if (dlg.ShowDialog(owner) == DialogResult.OK)
            	{
            		name = dlg.SelectedProject;
            		server = null;
            		dialogBounds = dlg.Bounds;
                    //dialogSplitterPos = dlg.SplitterPosition;
	                
	                return true;
	            }
		    }

            name = null;
			server = null;
			return false;
	    }

	    public void LoadProject(string projectName)
	    {
	        SIL.LCModel.Core.Text.Icu.InitIcuDataDir();
	        if (!Sldr.IsInitialized) Sldr.Initialize(true);
	        var dirs = new PaFieldWorksHelper();
	        var projectPath = string.Empty;

            if (File.Exists(projectName))
                projectPath = Path.Combine(dirs.ProjectsDirectory, projectName, projectName);
            else
	            projectPath = Path.Combine(dirs.ProjectsDirectory, projectName, projectName + LcmFileHelper.ksFwDataXmlFileExtension);

            var ui = new SilentLcmUI(SynchronizeInvoke);
	        var settings = new LcmSettings {DisableDataMigration = false, UpdateGlobalWSStore = false};
	        SilTools.Utils.WaitCursors(true);
	        using (var progressDlg = new PAProgress())
	        {
	            m_cache = LcmCache.CreateCacheFromExistingData(new FWProjectId(BackendProviderType.kSharedXML, projectPath), Thread.CurrentThread.CurrentUICulture.Name, ui, dirs, settings, progressDlg);
	        }
	        SilTools.Utils.WaitCursors(false);
        }

	    public ISynchronizeInvoke SynchronizeInvoke
	    {
	        get
	        {
	            Form form = Form.ActiveForm;
	            if (form != null)
	                return form;

	            if (Application.OpenForms.Count > 0)
	                return Application.OpenForms[0];

	            return null;
	        }
	    }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the FDO repositories from the specified project and server but only
        /// loads the writing systems. Initialize must be called to get the rest of the data.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public bool LoadOnlyWritingSystems(string name, string server,
			int timeToWaitForProcessStart, int timeToWaitForLoadingData)
		{
			return InternalInitialize(name, server, true,
				timeToWaitForProcessStart, timeToWaitForLoadingData);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the FDO repositories from the specified project and server.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Initialize(string name, string server, int timeToWaitForProcessStart,
			int timeToWaitForLoadingData)
		{
			return InternalInitialize(name, server, false,
				timeToWaitForProcessStart, timeToWaitForLoadingData);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the FDO repositories from the specified project and server.
		///
		/// <returns>
		/// true if the repositories are successfully initialized and FieldWorks started;
		/// otherwise, false.
		/// </returns>
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool InternalInitialize(string name, string server, bool loadOnlyWs,
			int timeToWaitForProcessStart, int timeToWaitForLoadingData)
		{
			bool foundFwProcess = false;
			bool newProcessStarted = false;
			Process newFwInstance = null;

		    var start = DateTime.Now;
		    
            try
		    {
		        return LoadFwDataForPa(name, server, loadOnlyWs, timeToWaitForLoadingData, newProcessStarted,
		            out foundFwProcess);
		    }
		    finally 
		    {
		        Debug.WriteLine((DateTime.Now - start).TotalMilliseconds);
            }

			// this line is reached only on startup timeout.
			return false;
		}

        private Process OpenProjectWithNewProcess(string name, object p)
        {
            throw new NotImplementedException();
        }

        /// ------------------------------------------------------------------------------------
        private bool LoadFwDataForPa(string name, string server, bool loadOnlyWs, int timeToWaitForLoadingData, bool newProcessStarted, out bool foundFwProcess)
		{

		    foundFwProcess = false;
		    PaRemoteRequest requestor = new PaRemoteRequest();
		    LoadProject(name);
		    requestor.cache = m_cache;

		    m_writingSystems = requestor.GetWritingSystems(); ;

		    if (!loadOnlyWs)
		    {
				List<PaLexEntry> entries = requestor.GetLexEntries();
			    if (entries != null && entries.Count > 0)
				    m_lexEntries = entries;
			    else
				    m_lexEntries = new List<PaLexEntry>();
		    }

		    foundFwProcess = true;
		    return true;
        }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the lexical entries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<IPaLexEntry> LexEntries
		{
			get { return m_lexEntries; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the writing systems.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<IPaWritingSystem> WritingSystems
		{
			get { return m_writingSystems; }
		}

		#endregion
	}
}
