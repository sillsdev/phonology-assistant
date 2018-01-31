// Copyright (c) 2009-2017 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SIL.PaToFdoInterfaces;
using SIL.Xml;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaLexicalInfo : IPaLexicalInfo, IDisposable
	{
		private List<PaWritingSystem> m_writingSystems;
		private List<PaLexEntry> m_lexEntries;

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
			LCModel.Core.Text.Icu.InitIcuDataDir();
			//FwRegistryHelper.Initialize(); // inorder to find correct Registry keys

			//using (var dlg = new ChooseLangProjectDialog(dialogBounds, dialogSplitterPos))
			//{
			//	if (dlg.ShowDialog(owner) == DialogResult.OK)
			//	{
			//		name = dlg.Project;
			//		server = null;
			//		dialogBounds = dlg.Bounds;
			//		dialogSplitterPos = dlg.SplitterPosition;
			//		return true;
			//	}
			//}

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
			var timeToWaitTotalMillis = timeToWaitForLoadingData + timeToWaitForProcessStart;

			//try
			//{
			//	do
			//	{
			//		FieldWorks.RunOnRemoteClients(FieldWorks.kPaRemoteRequest, requestor =>
			//		{
			//			return LoadFwDataForPa((PaRemoteRequest)requestor, name, server, loadOnlyWs,
			//				timeToWaitForLoadingData, newProcessStarted, out foundFwProcess);
			//		});

			//		if (foundFwProcess)
			//			return true;

			//		if (!newProcessStarted)
			//		{
			//			newProcessStarted = true;
			//			newFwInstance = FieldWorks.OpenProjectWithNewProcess(name, "-" + FwAppArgs.kNoUserInterface);

			//			// TODO-Linux: WaitForInputIdle isn't fully implemented on Linux.
			//			if (!newFwInstance.WaitForInputIdle(timeToWaitForProcessStart))
			//				return false;
			//		}
			//	} while ((DateTime.Now - start).TotalMilliseconds <= timeToWaitTotalMillis);
			//}
			//finally
			//{
			//	if (newFwInstance != null)
			//	{
			//		if (!newFwInstance.HasExited)
			//			newFwInstance.Kill();
			//		newFwInstance.Dispose();
			//	}

			//	Debug.WriteLine((DateTime.Now - start).TotalMilliseconds);
			//}

			// this line is reached only on startup timeout.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		private bool LoadFwDataForPa(PaRemoteRequest requestor, string name, string server,
			bool loadOnlyWs, int timeToWaitForLoadingData,
			bool newProcessStarted, out bool foundFwProcess)
		{
			foundFwProcess = false;

			Func<string, string, bool> invoker = requestor.ShouldWait;
			DateTime endTime = DateTime.Now.AddMilliseconds(timeToWaitForLoadingData);

			// Wait until this process knows which project it is loading.
			bool shouldWait;
			do
			{
				IAsyncResult ar = invoker.BeginInvoke(name, server, null, null);
				if (!ar.AsyncWaitHandle.WaitOne(endTime - DateTime.Now, false))
					return false;

				// Get the return value of the ShouldWait method.
				shouldWait = invoker.EndInvoke(ar);
				if (shouldWait)
				{
					if (timeToWaitForLoadingData > 0 && DateTime.Now > endTime)
						return false;

					Thread.Sleep(100);
				}
			} while (shouldWait);

			if (!requestor.IsMyProject(name, server))
				return false;

			string xml = requestor.GetWritingSystems();
			m_writingSystems = XmlSerializationHelper.DeserializeFromString<List<PaWritingSystem>>(xml);

			if (!loadOnlyWs)
			{
				xml = requestor.GetLexEntries();
				m_lexEntries = XmlSerializationHelper.DeserializeFromString<List<PaLexEntry>>(xml);
			}

			if (newProcessStarted)
				requestor.ExitProcess();

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
