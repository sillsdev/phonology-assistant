// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.   
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: PaMainWnd.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Filters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.Resources;
using SIL.Pa.UI.Controls;
using SIL.Pa.UI.Dialogs;
using SIL.Pa.UI.Views;
using SilTools;

namespace SIL.Pa.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for PaMainWnd.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PaMainWnd
	{
		/// ------------------------------------------------------------------------------------
		protected bool OnUserInterfaceLangaugeChanged(object args)
		{
			App.ReapplyLocalizationsToAllObjects();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnRecentlyUsedProjectChosen(object args)
		{
			string filename = args as string;

			if (!File.Exists(filename))
			{
				var fmt = App.LocalizeString("RecentlyOpenedProjectMissingMsg",
					"The project file '{0}' is missing.", App.kLocalizationGroupInfoMsg);
				
				Utils.MsgBox(string.Format(fmt, filename), MessageBoxIcon.Exclamation);
			}
			else if (m_project == null || m_project.FileName != filename)
			{
				LoadProject(filename);
				UndefinedPhoneticCharactersDlg.Show(m_project);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			var project = args as PaProject;
			if (project != null)
			{
				SetWindowText(project);
				Invalidate();
			}

			UndefinedPhoneticCharactersDlg.Show(project);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnHelpPA(object args)
		{
			App.ShowHelpTopic("hidGettingStarted");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnHelpAbout(object args)
		{
			using (var dlg = new AboutDlg(true, false))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnStudentManual(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingStudentManualDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExercises(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingExercisesDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInstructorGuide(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingInstructorGuideDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private static void OpenTrainingDocument(string docName)
		{
			string path = Path.Combine(App.AssemblyPath, App.kTrainingSubFolder);
			path = Path.Combine(path, docName);

			if (!File.Exists(path))
			{
				var fmt = App.LocalizeString("TrainingFileMissingMsg",
					"The training file '{0}' is missing.", App.kLocalizationGroupInfoMsg);

				var msg = string.Format(fmt, Utils.PrepFilePathForMsgBox(path));
				Utils.MsgBox(msg, MessageBoxIcon.Exclamation);
				return;
			}

			var prs = new Process();
			prs.StartInfo.UseShellExecute = true;
			prs.StartInfo.FileName = "\"" + path + "\"";
			prs.Start();
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFileExit(object args)
		{
			Close();
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		protected bool OnNewProject(object args)
		{
			var dlg = new ProjectSettingsDlg(null);

			if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Project != null)
			{
				var fmt = App.LocalizeString("LoadNewlyCreatedProjectQuestion",
					"Would you like to load the '{0}' project?", App.kLocalizationGroupInfoMsg);

				var msg = string.Format(fmt, dlg.Project.Name);

				if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					LoadProject(dlg.Project.FileName);
					UndefinedPhoneticCharactersDlg.Show(dlg.Project, true);
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnOpenProject(object args)
		{
			int filterindex = 0;

			string filter = string.Format(App.kstidFileTypePAProject,
				Application.ProductName) + "|" + App.kstidFileTypeAllFiles;

			var fmt = App.LocalizeString("ProjectOpenFileDlg.WindowText",
				"Open {0} Project File", App.kLocalizationGroupDialogs);
			
			string initialDir =
				(Settings.Default.LastFolderForOpenProjectDlg ?? App.DefaultProjectFolder);

			string[] filenames = App.OpenFileDialog("pap", filter, ref filterindex,
				string.Format(fmt, Application.ProductName), false, initialDir);

			if (filenames.Length > 0 && File.Exists(filenames[0]))
			{
				Settings.Default.LastFolderForOpenProjectDlg = Path.GetDirectoryName(filenames[0]);
				LoadProject(filenames[0]);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnProjectSettings(object args)
		{
			using (var dlg = new ProjectSettingsDlg(m_project))
			{
				if (dlg.ShowDialog(this) != DialogResult.OK || !dlg.ChangesWereMade)
					return true;

				Utils.WaitCursors(false);

				// Fully reload the project and blow away the previous project instance.
				var project = PaProject.Load(m_project.FileName, this);
				if (project != null)
				{
					// If there was a project loaded before this,
					// then get rid of it to make way for the new one.
					if (m_project != null)
					{
						m_project.Dispose();
						m_project = null;
					}

					project.LastNewlyMappedFields = dlg.NewlyMappedFields;
					App.Project = m_project = project;
					App.MsgMediator.SendMessage("DataSourcesModified", project);
				}

				Utils.WaitCursors(false);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateProjectSettings(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (m_project != null);
			itemProps.Update = true;
			return true;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateDefineAmbiguousItems(object args)
		//{
		//    PaApp.EnableWhenProjectOpen(args as TMItemProperties);
		//    return true;
		//}

		///----------------------------------------------------------------------------------
		protected bool OnExportAsPAXML(object args)
		{
			var dlg = new SaveFileDialog();
			dlg.OverwritePrompt = true;
			dlg.CheckFileExists = false;
			dlg.CheckPathExists = true;
			dlg.AddExtension = true;
			dlg.ShowHelp = false;
			dlg.RestoreDirectory = false;
			dlg.InitialDirectory = Environment.CurrentDirectory;
			dlg.DefaultExt = "paxml";

			var fmt = App.LocalizeString("PaXmlExportSaveFileDlg.WindowText",
				"Export to {0} XML", App.kLocalizationGroupDialogs);

			dlg.Title = string.Format(fmt, Application.ProductName);
			dlg.FileName = m_project.Name + ".paxml";
			dlg.FilterIndex = 0;
			dlg.Filter = string.Format(App.kstidFileTypePAXML, Application.ProductName) +
				"|" + App.kstidFileTypeAllFiles;

			if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
				m_project.RecordCache.Save(dlg.FileName);
			
			return true;
		}

		///----------------------------------------------------------------------------------
		protected bool OnUpdateExportAsPAXML(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;
			
			itemProps.Update = true;
			itemProps.Visible = true;
			itemProps.Text = string.Format(itemProps.OriginalText, Application.ProductName);
			itemProps.Enabled = (m_project != null && m_project.RecordCache != null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnToolsOptions(object args)
		{
			using (var optionsDlg = new OptionsDlg(m_project))
			{
				// TODO: Send a message indicating the options were changed.
				if (optionsDlg.ShowDialog(this) == DialogResult.OK)
				{
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateToolsOptions(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Update = true;
			itemProps.Visible = true;
			itemProps.Enabled = (m_project != null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUndefinedCharacters(object args)
		{
			UndefinedPhoneticCharactersDlg.Show(m_project, true);
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateUndefinedCharacters(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Update = true;
			itemProps.Enabled = (m_project != null &&
				App.IPASymbolCache.UndefinedCharacters != null &&
				App.IPASymbolCache.UndefinedCharacters.Count > 0);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnReloadProject(object args)
		{
			m_project.ReloadDataSources();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateReloadProject(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled =
				(m_project != null && m_project.DataSources != null && m_project.DataSources.Count > 0);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disable the Undock Menu when there is only one docked view remaining in
		/// the main window.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnEndViewChangingStatus(object args)
		{
			EnableUndockMenu(vwTabGroup.DockedTabCount != 1);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enable / disable the Edit Source Record menu selection and toolbar button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditSourceRecord(object args)
		{
			var itemProps = args as TMItemProperties;
			bool enabled = true;

			if (itemProps == null)
				return false;

			PaWordListGrid grid = null;

			if (vwTabGroup != null && vwTabGroup.CurrentTab != null)
			{
				if (vwTabGroup.CurrentTab.View is DataCorpusVw)
					grid = (vwTabGroup.CurrentTab.View as DataCorpusVw).WordListGrid;
				else if (vwTabGroup.CurrentTab.View is SearchVw)
				{
					SearchVw view = vwTabGroup.CurrentTab.View as SearchVw;
					grid = view.ResultViewManger.CurrentViewsGrid;
				}
				else if (vwTabGroup.CurrentTab.View is DistributionChartVw)
				{
					DistributionChartVw view = vwTabGroup.CurrentTab.View as DistributionChartVw;
					grid = view.ResultViewManger.CurrentViewsGrid;
				}
				else
					enabled = false;
			}

			// Disable UpdateEditSourceRecord if the current row is a hierarchical group row
			if (grid != null && grid.CurrentRow is SilHierarchicalGridRow)
				enabled = false;

			itemProps.Visible = true;
			itemProps.Update = true;
			itemProps.Enabled = (m_project != null && grid != null && enabled && m_project.WordCache.Count != 0);
			return true;
		}

		#region Define Features and Classes Message Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hides the menu item that's a place holder for adding the menu items for each
		/// filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateFiltersPlaceholder(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;

			if (itemProps != null)
			{
				itemProps.Visible = false;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a menu item for each filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownFiltersParent(object args)
		{
			ToolBarPopupInfo tbpi = args as ToolBarPopupInfo;
			if (tbpi == null)
				return false;

			const string cmdId = "CmdExecuteFilter";
			tbpi.Adapter.AddCommandItem(cmdId, "EnableFilter");

			bool firstItem = true;
			foreach (var filter in m_project.FilterHelper.Filters.OrderBy(x => x.Name))
			{
				if (filter.ShowInToolbarList)
				{
					var props = new TMItemProperties();
					props.BeginGroup = firstItem;
					props.Text = filter.Name;
					props.CommandId = cmdId;
					props.Name = "FILTER:" + filter.Name;
					props.Tag = filter;
					props.Checked = (m_project.FilterHelper.CurrentFilter == filter);
					props.Visible = true;
					props.Update = true;
					tbpi.Adapter.AddMenuItem(props, "mnuFiltersMain", "mnuFilterPlaceholder");
					firstItem = false;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the filter menu items added in OnDropDownFiltersParent().
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedFiltersParent(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			foreach (var filter in m_project.FilterHelper.Filters)
			{
				if (filter.ShowInToolbarList)
					itemProps.Adapter.RemoveItem("FILTER:" + filter.Name);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnEnableFilter(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			m_project.FilterHelper.ApplyFilter(itemProps.Tag as Filter);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFilters(object args)
		{
			using (var dlg = new FiltersDlg(m_project))
				dlg.ShowDialog(this);
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateFilters(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnNoFilter(object args)
		{
			OnFilterTurnedOff(null);
			m_project.FilterHelper.TurnOffCurrentFilter();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateNoFilter(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Enabled = (m_project.FilterHelper.CurrentFilter != null);
			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFilterChanged(object args)
		{
			var filter = args as Filter;
			sblblFilter.Visible = (filter != null);
			if (filter != null)
			{
				sblblFilter.Text = filter.Name;
				var constraint = new Size(statusStrip.Width / 3, 0);
				sblblFilter.Width = sblblFilter.GetPreferredSize(constraint).Width + 20;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFilterTurnedOff(object args)
		{
			sblblFilter.Visible = false;
			sblblFilter.Text = string.Empty;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFeatures(object args)
		{
			using (var dlg = new FeaturesDlg(m_project))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateFeatures(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnAmbiguousSequences(object args)
		{
			using (var dlg = new AmbiguousSequencesDlg(m_project))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAmbiguousSequences(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExperimentalTranscriptions(object args)
		{
			using (var dlg = new TranscriptionChangesDlg())
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExperimentalTranscriptions(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnClasses(object args)
		{
			using (ClassesDlg dlg = new ClassesDlg(m_project))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateClasses(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		#endregion

		#region Message handlers for Views
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the query corpus view.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewDataCorpus(object args)
		{
			vwTabGroup.ActivateView(typeof(DataCorpusVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the query corpus view menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewDataCorpus(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(DataCorpusVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnViewSearch(object args)
		{
			SearchVw vw = vwTabGroup.ActivateView(typeof(SearchVw)) as SearchVw;

			if (vw == null)
				return true;
			
			if (args is SearchQuery)
				vw.PerformSearch(args as SearchQuery, SearchResultLocation.CurrentTabGroup);
			else if (args is List<SearchQuery>)
			{
				foreach (SearchQuery query in (args as List<SearchQuery>))
					vw.PerformSearch(query, SearchResultLocation.CurrentTabGroup);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewFindPhones(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(SearchVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the consonant chart.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewConsonantChart(object args)
		{
			vwTabGroup.ActivateView(typeof(ConsonantChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the Consonant chart menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewConsonantChart(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(ConsonantChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the vowel chart.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewVowelChart(object args)
		{
			vwTabGroup.ActivateView(typeof(VowelChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the vowel chart menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewVowelChart(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(VowelChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the xy chart.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewXYChart(object args)
		{
			vwTabGroup.ActivateView(typeof(DistributionChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the xy chart menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewXYChart(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(DistributionChartVw));
			return true;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}