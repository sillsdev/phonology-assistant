using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using SIL.Pa.UI;
using SilUtils;
using SIL.Pa.FFSearchEngine;
using SIL.Pa.AddOns;

// I don't want to use a custom attribute, so I'm
// kludging what I want by using this attribute.
[assembly: System.Reflection.AssemblyDefaultAlias("CanBeDisabled")]

namespace SIL.Pa.FiltersAddOn
{
	#region PaAddOnManager class for the Filter Add-On
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Add-On for filtering feature.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		private string m_startupFilterName = null;

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
				if (!VerifyAddOnMediatorExists(assembly))
					return;

				string settingName = Path.GetFileNameWithoutExtension(assembly.Location);
				if (PaApp.SettingsHandler.GetBoolSettingsValue(settingName, "Enabled", true))
				{
					PaApp.AddMediatorColleague(this);

					// Register to receive notification after data sources have been loaded.
					// Use the add-on mediator (as opposed to just responding to the
					// OnAfterDataSourcesLoaded message) because we want to make sure that
					// all other add-ons who want to have processed the message. That's
					// because if some other add-on changes the data in the cache
					// (i.e. PaSADataSourceAddOn), it may influence what records are
					// filtered out.
					AddOnMediator.RegisterForDataSourcesLoadedMsg(1000, this);

					m_startupFilterName = 
						PaApp.SettingsHandler.GetStringSettingsValue("PaFiltersAddOn", "currfilter", null);
				}
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the PaAddOnMediator.dll assembly exists in the add-on
		/// folder. Returns true if it does. This add-on depends on the existence of that
		/// assembly.
		/// </summary>
		/// <param name="assembly">This add-on's assembly object.</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private bool VerifyAddOnMediatorExists(Assembly assembly)
		{
			string assemblyPath = Path.GetDirectoryName(assembly.Location);
			if (!File.Exists(Path.Combine(assemblyPath, "PaAddOnMediator.dll")))
			{
				string msg = Properties.Resources.kstidAddOnMediatorMissingMsg;
				msg = string.Format(msg, Path.GetFileName(assembly.Location), assemblyPath);
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMainViewOpened(object args)
		{
			if (args is PaMainWnd)
				OnViewUndocked(args);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewUndocked(object args)
		{
			if (!(args is Control) || FilterHelper.GuiComponents == null)
				return false;

			Form frm = args as Form;
			if (frm == null)
			{
				frm = (args as Control).FindForm();
				if (frm == null)
					return false;
			}

			FilterHelper.GiveWindowFilterGuiComponents(frm);
			frm.FormClosed += HandleWindowClosed;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleWindowClosed(object sender, FormClosedEventArgs e)
		{
			Form frm = sender as Form;
			if (frm != null)
				frm.FormClosed -= HandleWindowClosed;

			FilterHelper.RemoveFilterGuiComponentsFromWindow(frm);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void AfterDataSourcesLoaded(object args)
		{
			PaProject project = args as PaProject;
			if (project != null)
			{
				FilterHelper.UpdateDisplayedFilterLists(PaFiltersList.Load(project), false);

				// The first time we read the data sources, check if there was a filter
				// applied when the user closed down PA the last time. If so, then apply
				// it now. (m_startupFilterName will only be non null the first time the
				// data sources are read after startup.
				if (!string.IsNullOrEmpty(m_startupFilterName))
				{
					PaFilter filter = FilterHelper.FilterList[m_startupFilterName];
					if (filter != null)
						FilterHelper.ApplyFilter(filter);
					m_startupFilterName = null;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDisplayFilterDlg(object args)
		{
			string filterName =
				(FilterHelper.CurrentFilter != null ? FilterHelper.CurrentFilter.Name : null);
			
			using (DefineFiltersDlg dlg = new DefineFiltersDlg(filterName))
				dlg.ShowDialog();

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
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}

	#endregion

	#region FilterHelper static class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ------------------------------------------------------------------------------------
	internal static class FilterHelper
	{
		private static PaFiltersList s_filters = null;
		private static WordCache s_unusedWordsCache = new WordCache();
		private static PaFilter s_currFilter = null;
		private static Dictionary<Form, FilterGUIComponent> s_guiComponents =
			new Dictionary<Form,FilterGUIComponent>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Dictionary<Form, FilterGUIComponent> GuiComponents
		{
			get { return FilterHelper.s_guiComponents; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFilter CurrentFilter
		{
			get { return FilterHelper.s_currFilter; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFiltersList FilterList
		{
			get { return FilterHelper.s_filters; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WordCache UnusedWordsCache
		{
			get { return FilterHelper.s_unusedWordsCache; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void GiveWindowFilterGuiComponents(Form frm)
		{
			s_guiComponents[frm] = new FilterGUIComponent(frm);
			s_guiComponents[frm].RefreshFilterList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void RemoveFilterGuiComponentsFromWindow(Form frm)
		{
			FilterGUIComponent fgc;
			if (s_guiComponents.TryGetValue(frm, out fgc))
			{
				fgc.Dispose();
				s_guiComponents.Remove(frm);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates each of the filter drop-downs created for the PA main window and all the
		/// undocked windows. When restorePrevFilter is true, if there is currently a filter
		/// applied, it will be reapplied after the lists are updated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UpdateDisplayedFilterLists(PaFiltersList filterList,
			bool restorePrevFilter)
		{
			PaFilter prevFilter = s_currFilter;
			s_filters = (filterList != null ? filterList : PaFiltersList.Load());

			foreach (FilterGUIComponent fgc in s_guiComponents.Values)
				fgc.RefreshFilterList();

			if (prevFilter != null)
			{
				// Because the filter list has been recreated, the previous filter was
				// orphaned by the recreation of the filter list, we need to find the
				// new instance of the filter with the same name. If that filter no
				// longer exists, then we must turn off filtering (i.e. Restore).
				ApplyFilter(s_filters[prevFilter.Name]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ApplyFilter(PaFilter filter)
		{
			Restore();
			s_currFilter = filter;

			if (filter != null)
			{
				filter.Apply();
				PaApp.BuildPhoneCache();
			}

			PaApp.MsgMediator.SendMessage("DataSourcesModified", PaApp.Project.ProjectFileName);
			UpdateFilterGuiComponents();

			PaApp.SettingsHandler.SaveSettingsValue("PaFiltersAddOn", "currfilter",
				(filter != null ? filter.Name : string.Empty));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void UpdateFilterGuiComponents()
		{
			int imageWidth = Properties.Resources.kimidFilterSmall.Width;

			// Make sure all the filter controls on each window are updated with the
			// filter just applied.
			foreach (FilterGUIComponent fgc in s_guiComponents.Values)
			{
				fgc.FilterStatusStripLabel.Visible = (s_currFilter != null);

				if (s_currFilter == null)
					continue;

				fgc.FilterStatusStripLabel.Text = s_currFilter.Name;
				
				// Uncomment this section to make the filter label on the status bar
				// auto-sized to the filter's name.
				//using (Graphics g = fgc.StatusStrip.CreateGraphics())
				//{
				//    int desiredWidth = TextRenderer.MeasureText(s_currFilter.Name,
				//        fgc.FilterStatusStripLabel.Font).Width + imageWidth + 4;

				//    fgc.FilterStatusStripLabel.Width =
				//        Math.Min(desiredWidth, fgc.StatusStrip.Width / 2);
				//}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the filtered out words (i.e. puts them back into the application's
		/// word cache).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Restore()
		{
			if (s_unusedWordsCache.Count > 0 && PaApp.WordCache != null)
			{
				PaApp.WordCache.AddRange(s_unusedWordsCache);
				PaApp.BuildPhoneCache();
				s_unusedWordsCache.Clear();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchEngine CheckSearchQuery(SearchQuery query, bool showErrMsg)
		{
			query.ErrorMessages.Clear();
			SearchQuery modifiedQuery;
			if (!PaApp.ConvertClassesToPatterns(query, out modifiedQuery, showErrMsg))
				return null;

			if (PaApp.Project != null)
				SearchEngine.IgnoreUndefinedCharacters = PaApp.Project.IgnoreUndefinedCharsInSearches;

			SearchEngine.ConvertPatternWithExperimentalTrans =
				PaApp.SettingsHandler.GetBoolSettingsValue("searchengine",
				"convertpatternswithexperimentaltrans", false);

			SearchEngine engine = new SearchEngine(modifiedQuery, PaApp.PhoneCache);

			string[] errors = modifiedQuery.ErrorMessages.ToArray();
			string msg = ReflectionHelper.GetStrResult(typeof(PaApp),
				"CombineErrorMessages", errors);

			if (!string.IsNullOrEmpty(msg))
			{
				if (showErrMsg)
					Utils.MsgBox(msg);

				query.ErrorMessages.AddRange(modifiedQuery.ErrorMessages);
				return null;
			}

			if (!ReflectionHelper.GetBoolResult(typeof(PaApp),
				"VerifyMiscPatternConditions", new object[] { engine, showErrMsg }))
			{
				query.ErrorMessages.AddRange(modifiedQuery.ErrorMessages);
				return null;
			}

			return engine;
		}
	}

	#endregion
}
