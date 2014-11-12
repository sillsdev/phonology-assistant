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
        private static Assembly fwCoreDlgsAssembly;
        private static Assembly fdoUiAssembly;
        private List<IPaWritingSystem> m_writingSystems;
        private List<IPaLexEntry> m_lexEntries;
        private List<PaCustomField> m_customFields;
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
            fwCoreDlgsAssembly = Assembly.LoadFile(Path.Combine(fwInstallDir, "FwCoreDlgs.dll"));

            // Load native DLL's with COM classes
            //CheckError(LoadLibrary(Path.Combine(fwInstallDir, "icudt50.dll")), "icudt50.dll");
            //CheckError(LoadLibrary(Path.Combine(fwInstallDir, "icuuc50.dll")), "icuuc50.dll");
            //CheckError(LoadLibrary(Path.Combine(fwInstallDir, "icuin50.dll")), "icuin50.dll");
            //LoadLibrary(Path.Combine(fwInstallDir, "DebugProcs.dll"));  // don't check for error since it won't exist in a relese build
            //CheckError(LoadLibrary(Path.Combine(fwInstallDir, "FwKernel.dll")), "FwKernel.dll");

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
            Justification = "See TODO-Linux comment")]
        private bool InternalInitialize(string name, string server, bool loadOnlyWs)
        {

            var start = DateTime.Now;

            return LoadFwDataForPa(name, server, loadOnlyWs);
        }

        /// ------------------------------------------------------------------------------------
        private bool LoadFwDataForPa(string name, string server,
            bool loadOnlyWs)
        {
            using (CreateComContext())
            {

                if (!OpenProject(name, server))
                    return false;

                m_writingSystems = PaWritingSystem.GetWritingSystems(GetWritingSystems(fdoCache.ServiceLocator), fdoCache.ServiceLocator);
                if (!loadOnlyWs)
                {
                    m_customFields = PaCustomField.GetCustomFields(fdoCache);
                    m_lexEntries = PaLexEntry.GetAll(GetLexicalEntries(), m_customFields, fdoCache.ServiceLocator);
                }

                ((IDisposable)fdoCache).Dispose();
                fdoCache = null;
            }

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

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets a collection of the custom fields defined in the project.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public IEnumerable<IPaCustomField> CustomFields
        {
            get { return m_customFields; }
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

        public static dynamic GetLexEntryRefOwner(dynamic lxEntryRef)
        {
            // var lx = lxEntryRef.OwnerOfClass<ILexEntry>();
            throw new NotImplementedException();
        }

        public static string GetTsStringText(dynamic obj, string propertyName)
        {
            dynamic property = SilTools.ReflectionHelper.GetProperty(obj, propertyName);
            return ((FieldWorks.Common.COMInterfaces.ITsString)property).Text;
        }

        public static void GetStringEntry(dynamic svcloc, dynamic msa, int i, out string wsId, out string tssText)
        {
            var msaCast = (FieldWorks.Common.COMInterfaces.ITsMultiString)msa;
            int wsHvo;
            var tsString = msaCast.GetStringFromIndex(i, out wsHvo);
            tssText = tsString.Text;
            wsId = null;
            foreach (dynamic ws in GetWritingSystems(svcloc))
            {
                int hvo = SilTools.ReflectionHelper.GetProperty(ws, "Handle");
                if (hvo == wsHvo)
                {
                    wsId = SilTools.ReflectionHelper.GetProperty(ws, "Id");
                    break;
                }
            }

        }

        private IDisposable CreateComContext()
        {
            dynamic activationContextHelper = SilTools.ReflectionHelper.CreateClassInstance(basicUtilsAssembly,
                "SIL.Utils.ActivationContextHelper", new object[] { "FDO.dll.manifest" });
            return SilTools.ReflectionHelper.GetResult(activationContextHelper, "Activate", new Object[] { });
        }
        #endregion

        #region Private helper methods
        private void InitializeClientServices()
        {
            // need to call this iniitialization routine to allow the ChooseLangProjectDialog can be used
            Type clientServerervices = fdoAssembly.GetType("SIL.FieldWorks.FDO.DomainServices.ClientServerServices");
            Type directoryFinderClass = fwUtilsAssembly.GetType("SIL.FieldWorks.Common.FwUtils.FwDirectoryFinder");
            dynamic fdoDirs = SilTools.ReflectionHelper.GetProperty(directoryFinderClass, "FdoDirectories");
            dynamic projectDirs = SilTools.ReflectionHelper.GetProperty(directoryFinderClass, "ProjectsDirectory");
            dynamic localProjectDirs = SilTools.ReflectionHelper.GetProperty(directoryFinderClass, "ProjectsDirectoryLocalMachine");

            Func<bool> comparer = delegate() { return projectDirs == localProjectDirs; };

            SilTools.ReflectionHelper.GetResult(clientServerervices, "SetCurrentToDb4OBackend", new object[] { null, fdoDirs, comparer  });

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
            Type chooseDlgClass = fwCoreDlgsAssembly.GetType();
            dynamic chooseDlg = SilTools.ReflectionHelper.CreateClassInstance(fwCoreDlgsAssembly, "SIL.FieldWorks.FwCoreDlgs.ChooseLangProjectDialog",
                new object[] { dialogBounds, dialogSplitterPos });
            return chooseDlg;
        }

        private bool OpenProject(string name, string server)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open project: " + ex.Message);
                fdoCache = null;
            }
            return fdoCache != null;
        }

        private static IEnumerable<dynamic> GetWritingSystems(dynamic svcloc)
        {
            dynamic wsRepository = SilTools.ReflectionHelper.GetProperty(svcloc, "WritingSystems");
            return SilTools.ReflectionHelper.GetProperty(wsRepository, "AllWritingSystems");
        }

        private IEnumerable<dynamic> GetLexicalEntries()
        {
            Type lexRepositoryClass = fdoAssembly.GetType("SIL.FieldWorks.FDO.ILexEntryRepository");
            dynamic repository = fdoCache.ServiceLocator.GetInstance(lexRepositoryClass);
            return repository.AllInstances();
        }
        #endregion

    }
}
