using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.Pa.Controls;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;
using System.Drawing.Drawing2D;

// I don't want to use a custom attribute, so I'm
// kludging what I want by using this attribute.
[assembly: System.Reflection.AssemblyDefaultAlias("CanBeDisabled")]

namespace SIL.Pa.FiltersAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Add-On for filtering feature.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		private Dictionary<Form, FilterGUIComponent> m_guiComponents;
		private PaFiltersList m_filters;
		private bool m_fDataFilterAfterReading = false;

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMainViewOpened(object args)
		{
			if (args is PaMainWnd)
			{
				m_guiComponents = new Dictionary<Form, FilterGUIComponent>();
				OnViewUndocked(args);
			}
			
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewUndocked(object args)
		{
			if (!(args is Control) || m_guiComponents == null)
				return false;

			Form frm = args as Form;
			if (frm == null)
			{
				frm = (args as Control).FindForm();
				if (frm == null)
					return false;
			}

			m_guiComponents[frm] = new FilterGUIComponent(frm);
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

			FilterGUIComponent fgc;
			if (m_guiComponents.TryGetValue(frm, out fgc))
			{
				fgc.Dispose();
				m_guiComponents.Remove(frm);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeforeLoadingDataSources(object args)
		{
			m_fDataFilterAfterReading = false;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAfterLoadingDataSources(object args)
		{
			// The only time we get here and this flag is true, should be when the SA data
			// source add-on is enabled and has already done its thing and sent the
			// AfterSaDataSourceAddOnHandledLoadedingDataSources message, in which case
			// we know we've already filtered the data since we handle that message.
			if (m_fDataFilterAfterReading)
				return false;

			// If we've gotten to this point, we know one of two things. 1) either the
			// SA data source add-on is not loaded or enabled, or 2) it is loaded, but
			// our handler for the AfterLoadingDataSources message got called before the
			// SA data source add-on's handler for the AfterLoadingDataSources message.
			// In the case of the latter, we need to check if the if the SA data source
			// add-on is loaded and enabled, and if it is, do nothing here because it
			// will get done in our handler for the
			// AfterSaDataSourceAddOnHandledLoadedingDataSources message. Otherwise,
			// go ahead and filter the data.
			bool fSaAddOnEnabled = false;
			foreach (Assembly assembly in PaApp.AddOnAssemblys)
			{
				string filename = Path.GetFileName(assembly.CodeBase);
				filename = Path.GetFileNameWithoutExtension(filename);
				if (filename.ToLower() == "pasadatasourceaddon")
				{
					fSaAddOnEnabled = PaApp.SettingsHandler.GetBoolSettingsValue(
						filename, "Enabled", true);

					break;
				}
			}

			if (!fSaAddOnEnabled)
				FilterData(args as PaProject);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAfterSaDataSourceAddOnHandledLoadedingDataSources(object args)
		{
			FilterData(args as PaProject);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FilterData(PaProject project)
		{
			m_fDataFilterAfterReading = true;

			if (project == null)
				return;

			if (m_filters == null)
				m_filters = PaFiltersList.Load(project);

			FilterHelper.FilterList = m_filters;

			// TODO: Filter data
			// PaApp.SettingsHandler.GetStringSettingsValue("filters", "current", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnFilterApplied(object args)
		{
			PaFilter filter = args as PaFilter;
			int imageWidth = Properties.Resources.kimidFilterSmall.Width;

			// Make sure all the filter controls on each window are updated with the
			// filter just applied.
			foreach (FilterGUIComponent fgc in m_guiComponents.Values)
			{
				fgc.DropDownCtrl.CurrentFilter = filter;
				fgc.FilterStatusStripLabel.Visible = (filter != null);

				if (filter == null)
					continue;

				using (Graphics g = fgc.StatusStrip.CreateGraphics())
				{
					fgc.FilterStatusStripLabel.Text = filter.Name;

					int desiredWidth = TextRenderer.MeasureText(filter.Name,
						fgc.FilterStatusStripLabel.Font).Width + imageWidth + 4;

					fgc.FilterStatusStripLabel.Width =
						Math.Min(desiredWidth, fgc.StatusStrip.Width / 2);
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnFilterListUpdated(object args)
		{
			m_filters = PaFiltersList.Load();
			FilterHelper.FilterList = m_filters;

			foreach (FilterGUIComponent fgc in m_guiComponents.Values)
				fgc.RefreshFilterList();

			// TODO: Reapply the filter that's currently active.
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
			set { FilterHelper.s_filters = value; }
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
		public static void FilterApplied(PaFilter filter)
		{
			s_currFilter = filter;
			PaApp.BuildPhoneCache();
			PaApp.MsgMediator.SendMessage("DataSourcesModified", PaApp.Project.ProjectFileName);
		}
	}
}
