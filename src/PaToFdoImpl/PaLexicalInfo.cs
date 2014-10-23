// Copyright (c) 2009-2013 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)
//
// File: PaLexicalInfo.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using SIL.PaToFdoInterfaces;
using SIL.Utils;
using System.Runtime.InteropServices;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaLexicalInfo : IPaLexicalInfo, IDisposable
	{
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        private static Assembly fwAssembly;
        private static string fwInstallDir;
        private static Assembly fdoAssembly;
        private static Assembly basicUtilsAssembly;
        private static Assembly fwUtilsAssembly;
        private static Assembly fdoUiAssembly;
        private List<PaWritingSystem> m_writingSystems;
		private List<PaLexEntry> m_lexEntries;
        private dynamic fdoCache;

		#region constructors
		/// <summary>
		/// Contstructor is required to initialize ClientServerServices
		/// </summary>
		public PaLexicalInfo(Assembly fwAssembly, string fwInstallDir)
		{
            PaLexicalInfo.fwAssembly = fwAssembly;
            PaLexicalInfo.fwInstallDir = fwInstallDir;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromFieldWorksFolder);
            fdoAssembly = Assembly.LoadFile(Path.Combine(fwInstallDir, "FDO.dll"));
            fdoUiAssembly = Assembly.LoadFile(Path.Combine(fwInstallDir, "FdoUi.dll"));
            basicUtilsAssembly = Assembly.LoadFile(Path.Combine(fwInstallDir, "BasicUtils.dll"));
            fwUtilsAssembly = Assembly.LoadFile(Path.Combine(fwInstallDir, "FwUtils.dll"));

            // Load native DLL's with COM classes
            CheckError(LoadLibrary(Path.Combine(fwInstallDir, "icudt50.dll")), "icudt50.dll");
            CheckError(LoadLibrary(Path.Combine(fwInstallDir, "icuuc50.dll")), "icuuc50.dll");
            CheckError(LoadLibrary(Path.Combine(fwInstallDir, "icuin50.dll")), "icuin50.dll");
            LoadLibrary(Path.Combine(fwInstallDir, "DebugProcs.dll"));  // don't check for error since it won't exist in a relese build
            CheckError(LoadLibrary(Path.Combine(fwInstallDir, "FwKernel.dll")), "FwKernel.dll");

            InitializeClientServices();
		}

        private void CheckError(IntPtr intPtr, string libraryName)
        {
            if (intPtr == IntPtr.Zero)
                throw new ArgumentException("Could not load one of the required Fieldworks libraries: {0}", libraryName);
        }

        private Assembly LoadFromFieldWorksFolder(object sender, ResolveEventArgs args)
        {
            string assemblyPath = Path.Combine(fwInstallDir, new AssemblyName(args.Name).Name + ".dll");
            if (File.Exists(assemblyPath) == false) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
		#endregion

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
			InitIcuDataDir();
			RegistryHelper.ProductName = "FieldWorks"; // inorder to find correct Registry keys

			using (dynamic dlg = CreateChooseLangProjectDialog(dialogBounds, dialogSplitterPos))
			{
				if (dlg.ShowDialog(owner) == DialogResult.OK)
				{
					name = dlg.Project;
					server = dlg.Server;
					dialogBounds = dlg.Bounds;
					dialogSplitterPos = dlg.SplitterPosition;
					return true;
				}
			}

			name = null;
			server = null;
			return false;
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the FDO repositories from the specified project and server but only
		/// loads the writing systems. Initialize must be called to get the rest of the data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool LoadOnlyWritingSystems(string name, string server)
		{
			return InternalInitialize(name, server, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the FDO repositories from the specified project and server.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Initialize(string name, string server)
		{
			return InternalInitialize(name, server, false);
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
		[SuppressMessage("Gendarme.Rules.Portability", "MonoCompatibilityReviewRule",
			Justification="See TODO-Linux comment")]
		private bool InternalInitialize(string name, string server, bool loadOnlyWs)
		{

			var start = DateTime.Now;

			return LoadFwDataForPa(name, server, loadOnlyWs);
		}

		/// ------------------------------------------------------------------------------------
		private bool LoadFwDataForPa(string name, string server,
			bool loadOnlyWs)
		{
            OpenProject(name, server);
            m_writingSystems = PaWritingSystem.GetWritingSystems(GetWritingSystems(), ServiceLocator());
			if (!loadOnlyWs)
				m_lexEntries = PaLexEntry.GetAll(GetLexicalEntries());

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

        #region Static helper methods
        public static bool IsComplexForm(dynamic lxEntryRef)
        {
            // lxEntryRef.RefType != LexEntryRefTags.krtComplexForm
            return false;
        }

        public static bool IsLexEntry(object obj)
        {
            // component is ILexEntry
            return false;
        }

        public static bool IsLexSense(object obj)
        {
            // component is ILexSense
            return false;
        }

        internal static dynamic GetLexEntryRefOwner(dynamic lxEntryRef)
        {
            // var lx = lxEntryRef.OwnerOfClass<ILexEntry>();
            throw new NotImplementedException();
        }
        #endregion

        #region Private helper methods
        private void InitializeClientServices()
        {
            // TODO: Needed for dialog to open
            // need to call this iniitialization routine to allow the ChooseLangProjectDialog can be used
            //ClientServerServices.SetCurrentToDb4OBackend(null, FwDirectoryFinder.FdoDirectories,
            //    () => FwDirectoryFinder.ProjectsDirectory == FwDirectoryFinder.ProjectsDirectoryLocalMachine);

        }

        private void InitIcuDataDir()
        {
            Assembly comAssembly = Assembly.LoadFile(Path.Combine(fwInstallDir, "COMInterfaces.dll"));
            Type icu = comAssembly.GetType("SIL.FieldWorks.Common.COMInterfaces.Icu");
            MethodInfo initMethod = icu.GetMethod("InitIcuDataDir");
            initMethod.Invoke(null, null);
        }

        private dynamic CreateChooseLangProjectDialog(Rectangle dialogBounds, int dialogSplitterPos)
        {
            throw new NotImplementedException();
        }

        private void OpenProject(string name, string server)
        {
            // create project id
            dynamic projId = SilTools.ReflectionHelper.CreateClassInstance(fwAssembly, "SIL.FieldWorks.ProjectId", new object[] { "xml", name, server });
            dynamic threadHelper = SilTools.ReflectionHelper.CreateClassInstance(basicUtilsAssembly, "SIL.Utils.ThreadHelper", null);
            dynamic fwFdoUi = SilTools.ReflectionHelper.CreateClassInstance(fdoUiAssembly, "SIL.FieldWorks.FdoUi.FwFdoUI", new object[] { null, threadHelper });

            Type directoryFinderClass = fwUtilsAssembly.GetType("SIL.FieldWorks.Common.FwUtils.FwDirectoryFinder");
            dynamic fdoDirs = SilTools.ReflectionHelper.GetProperty(directoryFinderClass, "FdoDirectories");

            Type cacheClass = fdoAssembly.GetType("SIL.FieldWorks.FDO.FdoCache");
            fdoCache = SilTools.ReflectionHelper.GetResult(cacheClass, "CreateCacheFromExistingData", new object[] { projId, "en",
                fwFdoUi, fdoDirs, null /* progress dlg */, true /* forbid migration */});
        }

        private IEnumerable<dynamic> GetWritingSystems()
        {
            return fdoCache.LanguageProject.AllWritingSystems;
        }

        private IEnumerable<dynamic> GetLexicalEntries()
        {
            Type lexRepositoryClass = fdoAssembly.GetType("SIL.FieldWorks.FDO.ILexEntryRepository");
            dynamic repository = ServiceLocator().GetInstance();
            return repository.AllInstances();
        }

        private dynamic ServiceLocator()
        {
            return fdoCache.ServiceLocator;
        }
        #endregion
    }
}
