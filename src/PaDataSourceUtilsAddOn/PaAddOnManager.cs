using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;
using SIL.SpeechTools.Utils;
using SIL.Pa.Dialogs;
using System.IO;

// I don't want to use a custom attribute, so I'm
// kludging to get what I want by using this attribute.
[assembly: System.Reflection.AssemblyDefaultAlias("CanBeDisabled")]

namespace SIL.Pa.DataSourceUtilsAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		private SortedDictionary<int, PaDataSource> m_skippedDataSources;

		#region Constructing the Add-on class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaAddOnManager()
		{
			try
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				string settingName = Path.GetFileNameWithoutExtension(assembly.CodeBase);
				if (PaApp.SettingsHandler.GetBoolSettingsValue(settingName, "Enabled", true))
					PaApp.AddMediatorColleague(this);
			}
			catch { }
		}

		#endregion

		#region Message handlers for before and after loading data sources.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message (i.e. BeforeLoadingDataSources) gets called right before PA loads all
		/// the data sources in the current project. This specific version of the message
		/// handler will do is find all data sources that match the ones this add-on saves
		/// in the DataSourceLoadInfo list (saved in a file in the project folder) and remove
		/// them from the project's data source list. The data sources that are removed are
		/// stored in a temporary list so that they can be re-added to the project's list
		/// after the data sources are read.
		/// </summary>
		/// <remarks>
		/// Concerning the reason for setting the last modification date on each skipped
		/// data source to tomorrow (i.e. DateTime.Now.AddDays(1)), is so that every time
		/// PA gains focus and it determines if any of the data sources have been updated
		/// since it lost focus, PA will determine that all skipped data sources have not
		/// been updated. Normally, the last modification date on a data source is set when
		/// the data source is read. But if the data source is skipped, then the last
		/// modification date will not get set properly and will cause PA to keep re-reading
		/// data sources each time the program becomes active. Hence setting the data to
		/// tomorrow.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeforeLoadingDataSources(object args)
		{
			PaProject project = args as PaProject;
			if (project == null)
				return false;

			m_skippedDataSources = null;

			// Get the list of data sources that were marked for not loading.
			SkippedDataSourceList sdsl = SkippedDataSourceList.Load(project);
			if (sdsl == null || sdsl.Count == 0)
				return false;

			// Create our list to temporarily store the skipped data sources. Make it
			// a sorted list so we can insert them back in the list in the same place
			// they were when we removed them. This is so they will appear in the
			// project settings dialog in the same order in which the user added them.
			m_skippedDataSources = new SortedDictionary<int, PaDataSource>();
			
			// Go through the project's data sources and remove any that are to be skipped.
			for (int i = project.DataSources.Count - 1; i >= 0; i--)
			{
				PaDataSource ds = project.DataSources[i];
				if (sdsl.SkipDataSource(ds.ToString(true)))
				{
					// Set the skipped data sources date to tomorrow (see comment in 
					// header of this method), save it to our temp. list and remove
					// it from the project's list of data sources.
					ds.LastModification = DateTime.Now.AddDays(1);
					m_skippedDataSources[i] = ds;
					project.DataSources.RemoveAt(i);
				}
			}

			if (m_skippedDataSources.Count == 0)
				m_skippedDataSources = null;

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message (i.e. AfterLoadingDataSources) is called right after PA loads all the
		/// data sources in the current project. This specific version of the method adds back
		/// into the project's data source list those data sources that were removed in
		/// OnBeforeLoadingDataSources because they were marked not to be loaded in the
		/// ProjectSettingsDlg.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAfterLoadingDataSources(object args)
		{
			PaProject project = args as PaProject;
			if (project == null || m_skippedDataSources == null)
				return false;

			// Insert them back in in the project's data source list at the indexes from
			// which they were removed. This is so they will appear in the project settings
			// dialog in the same order in which the user added them.
			foreach (KeyValuePair<int, PaDataSource> kvp in m_skippedDataSources)
				project.DataSources.Insert(kvp.Key, kvp.Value);

			m_skippedDataSources = null;
			return false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnProjectSettingsDlgHandleCreated(object args)
		{
			ProjectSettingsDlgDataSourceSkippingHelper.Initialize(args as ProjectSettingsDlg);
			return false;
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] {this});
		}

		#endregion
	}
}
