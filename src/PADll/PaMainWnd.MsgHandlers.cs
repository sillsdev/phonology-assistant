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
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic.Devices;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.Pa.Dialogs;
using SIL.Pa.FFSearchEngine;
using SIL.Pa.Resources;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for PaMainWnd.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PaMainWnd
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRecentlyUsedProjectChosen(object args)
		{
			string filename = args as string;

			if (!File.Exists(filename))
			{
				string msg = Properties.Resources.kstidRecentlyUsedProjectMissingMsg;
				STUtils.STMsgBox(string.Format(msg, filename), MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
			else if (PaApp.Project == null || PaApp.Project.ProjectFileName != filename)
			{
				LoadProject(filename);
				UndefinedPhoneticCharactersDlg.Show(PaApp.Project == null ?
					string.Empty : PaApp.Project.ProjectName);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			PaProject project = args as PaProject;
			if (project != null)
			{
				Text = string.Format(Properties.Resources.kstidMainWindowCaption,
					project.ProjectName, Application.ProductName);
				Invalidate();
			}

			UndefinedPhoneticCharactersDlg.Show(args as string);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnHelpPA(object args)
		{
			PaApp.ShowHelpTopic("hidGettingStarted");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnHelpAbout(object args)
		{
			using (AboutDlg dlg = new AboutDlg(true, false))
			{
				((IAboutDlg)dlg).ProdVersion = PaApp.ProdVersion;
				dlg.ShowDialog(this);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnStudentManual(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingStudentManualDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnExercises(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingExercisesDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInstructorGuide(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingInstructorGuideDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OpenTrainingDocument(string docName)
		{
			string path = Path.Combine(Application.StartupPath, PaApp.kTrainingSubFolder);
			path = Path.Combine(path, docName);

			if (!File.Exists(path))
			{
				string msg = string.Format(Properties.Resources.kstidMissingTrainingFileMsg,
					STUtils.PrepFilePathForSTMsgBox(path));

				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			Process prs = new Process();
			prs.StartInfo.UseShellExecute = true;
			prs.StartInfo.FileName = "\"" + path + "\"";
			prs.Start();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnFileExit(object args)
		{
			Close();
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnNewProject(object args)
		{
			ProjectSettingsDlg dlg = new ProjectSettingsDlg();

			if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Project != null)
			{
				if (STUtils.STMsgBox(
					string.Format(Properties.Resources.kstidLoadNewProjectQuestion,
					dlg.Project.ProjectName), MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					LoadProject(dlg.Project.ProjectFileName);
					UndefinedPhoneticCharactersDlg.Show(dlg.Project.ProjectName, true);
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnOpenProject(object args)
		{
			int filterindex = 0;

			string filter = string.Format(ResourceHelper.GetString("kstidFileTypePAProject"),
				Application.ProductName) + "|" + ResourceHelper.GetString("kstidFileTypeAllFiles");

			string dlgTitle =
				string.Format(Properties.Resources.kstidPAFilesCaptionOFD, Application.ProductName);
			
			string initialDir = PaApp.SettingsHandler.GetStringSettingsValue(Name, "lastprojdir",
				PaApp.DefaultProjectFolder);

			string[] filenames = PaApp.OpenFileDialog("pap", filter, ref filterindex,
				dlgTitle, false, initialDir);

			if (filenames.Length > 0 && File.Exists(filenames[0]))
			{
				PaApp.SettingsHandler.SaveSettingsValue(Name, "lastprojdir",
					Path.GetDirectoryName(filenames[0]));
				
				LoadProject(filenames[0]);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnProjectSettings(object args)
		{
			using (ProjectSettingsDlg dlg = new ProjectSettingsDlg(PaApp.Project))
			{
				dlg.ShowDialog(this);
				if (dlg.ChangesWereMade)
					FullProjectReload();
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads the project fully. I.e. blows away the previous project and starts reload
		/// from scratch.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FullProjectReload()
		{
			STUtils.WaitCursors(false);

			PaProject project = PaProject.Load(PaApp.Project.ProjectFileName, this);
			if (project != null)
			{
				// If there was a project loaded before this,
				// then get rid of it to make way for the new one.
				if (PaApp.Project != null)
				{
					PaApp.Project.Dispose();
					PaApp.Project = null;
				}

				PaApp.Project = project;
				PaApp.MsgMediator.SendMessage("DataSourcesModified", project);
			}

			STUtils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateProjectSettings(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (PaApp.Project != null);
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if message was handled</returns>
		///----------------------------------------------------------------------------------
		protected bool OnExportAsPAXML(object args)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.OverwritePrompt = true;
			dlg.CheckFileExists = false;
			dlg.CheckPathExists = true;
			dlg.AddExtension = true;
			dlg.ShowHelp = false;
			dlg.RestoreDirectory = false;
			dlg.InitialDirectory = Environment.CurrentDirectory;
			dlg.DefaultExt = "paxml";
			dlg.Title = string.Format(Properties.Resources.kstidPAXMLExportCaptionSFD, Application.ProductName);
			dlg.FileName = PaApp.Project.ProjectName + ".paxml";
			dlg.FilterIndex = 0;
			dlg.Filter = string.Format(ResourceHelper.GetString("kstidFileTypePAXML"),
				Application.ProductName) + "|" + ResourceHelper.GetString("kstidFileTypeAllFiles");

			if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
				PaApp.RecordCache.Save(dlg.FileName);
			
			return true;
		}

		///----------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if message was handled</returns>
		///----------------------------------------------------------------------------------
		protected bool OnUpdateExportAsPAXML(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;
			
			itemProps.Update = true;
			itemProps.Visible = true;
			itemProps.Text = string.Format(itemProps.OriginalText, Application.ProductName);
			itemProps.Enabled = (PaApp.Project != null && PaApp.RecordCache != null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnToolsOptions(object args)
		{
			using (OptionsDlg optionsDlg = new OptionsDlg())
			{
				// TODO: Send a message indicating the options were changed.
				if (optionsDlg.ShowDialog(this) == DialogResult.OK)
				{
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateToolsOptions(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Update = true;
			itemProps.Visible = true;
			itemProps.Enabled = (PaApp.Project != null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUndefinedCharacters(object args)
		{
			UndefinedPhoneticCharactersDlg.Show(PaApp.Project.ProjectName, true);
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateUndefinedCharacters(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Update = true;
			itemProps.Enabled = (PaApp.Project != null &&
				DataUtils.IPACharCache.UndefinedCharacters != null &&
				DataUtils.IPACharCache.UndefinedCharacters.Count > 0);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnReloadProject(object args)
		{
			PaApp.Project.ReloadDataSources();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateReloadProject(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled =
				(PaApp.Project != null && PaApp.Project.DataSources != null && PaApp.Project.DataSources.Count > 0);
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

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// The drop-down portion of the button was clicked so show the list of saved patterns
		///// in a popup.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public bool OnDropDownViewFindPhones(object args)
		//{
		//    // TODO: What should be done if there are no saved patterns to show?

		//    ToolBarPopupInfo itemProps = args as ToolBarPopupInfo;
		//    if (itemProps == null)
		//        return false;

		//    SearchPatternTreeView sptv = new SearchPatternTreeView();
		//    sptv.IsForToolbarPopup = true;
		//    sptv.AllowDataModifications = false;
		//    sptv.BackColor = SystemColors.Menu;
		//    sptv.Dock = DockStyle.Fill;
		//    sptv.Load();

		//    SizableDropDownPanel sptvHost =
		//        new SizableDropDownPanel("savedpatterndropdownsize", new Size(300, 350));

		//    // When there are not patterns or categories in the list, then eliminate
		//    // all but the bottom padding in the host so the "No saved search patterns"
		//    // message is centered.
		//    if (sptv.Nodes.Count == 0)
		//        sptvHost.Padding = new Padding(0, 0, 0, sptvHost.Padding.Bottom);
			
		//    sptvHost.BackColor = SystemColors.Menu;
		//    sptvHost.Controls.Add(sptv);
		//    itemProps.Control = sptvHost;
		//    return true;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enable / disable the Edit Source Record menu selection and toolbar button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditSourceRecord(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
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
				else if (vwTabGroup.CurrentTab.View is XYChartVw)
				{
					XYChartVw view = vwTabGroup.CurrentTab.View as XYChartVw;
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
			itemProps.Enabled = (PaApp.Project != null && grid != null && enabled && PaApp.WordCache.Count != 0);
			return true;
		}

		#region Message handlers for Find
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the logic for all methods OnUpdateEditFind(Next/Previous)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool HandleFindItemUpdate(TMItemProperties itemProps, bool enableAllow)
		{
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
				else if (vwTabGroup.CurrentTab.View is XYChartVw)
				{
					XYChartVw view = vwTabGroup.CurrentTab.View as XYChartVw;
					grid = view.ResultViewManger.CurrentViewsGrid;
				}
				else
					enableAllow = false;
			}

			bool enable = (enableAllow && PaApp.Project != null && grid != null && grid.RowCount > 0);

			if (itemProps.Enabled != enable)
			{
				itemProps.Enabled = enable;
				itemProps.Visible = true;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle OnUpdateEditFind.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFind(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle OnUpdateEditFindNext.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFindNext(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, FindInfo.CanFindAgain);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle OnUpdateEditFindPrevious.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFindPrevious(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, FindInfo.CanFindAgain);
		}

		#endregion

		#region Define Features and Classes Message Handlers
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnDefineArticulatoryFeatures(object args)
		//{
		//    using (ArticulatoryFeaturesDlg dlg = new ArticulatoryFeaturesDlg())
		//        dlg.ShowDialog(this);

		//    return true;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateDefineArticulatoryFeatures(object args)
		//{
		//    PaApp.EnableWhenProjectOpen(args as TMItemProperties);
		//    return true;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnDefineBinaryFeatures(object args)
		//{
		//    using (BinaryFeaturesDlg dlg = new BinaryFeaturesDlg())
		//        dlg.ShowDialog(this);

		//    return true;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateDefineBinaryFeatures(object args)
		//{
		//    PaApp.EnableWhenProjectOpen(args as TMItemProperties);
		//    return true;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnDefineClasses(object args)
		{
			using (ClassesDlg dlg = new ClassesDlg())
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateDefineClasses(object args)
		{
			PaApp.EnableWhenProjectOpen(args as TMItemProperties);
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
			PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(DataCorpusVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewFindPhones(object args)
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewFindPhones(object args)
		{
			PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(SearchVw));
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
			PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(ConsonantChartVw));
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
			PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(VowelChartVw));
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
			vwTabGroup.ActivateView(typeof(XYChartVw));
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
			PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(XYChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the phone inventory view.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewPhoneInventory(object args)
		{
			vwTabGroup.ActivateView(typeof(PhoneInventoryVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the phone inventory view menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewPhoneInventory(object args)
		{
			PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(PhoneInventoryVw));
			return true;
		}

		#endregion

		#region CurrentViewsGrid stuff
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateShowGridLines(object args)
		//{
		//    return false;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateChartPhoneSearch(object args)
		//{
		//    TMItemProperties itemProps = args as TMItemProperties;
		//    if (itemProps == null)
		//        return false;

		//    itemProps.VisibleInGrid = true;
		//    itemProps.Enabled = false;
		//    itemProps.Update = true;
		//    return true;
		//}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnWriteIPACache(object args)
		{
			DataUtils.IPACharCache.Save();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only show this menu if the CTRL and SHIFT key are down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateWriteIPACache(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null)
			{
				Keyboard kb =
					new Keyboard();

				itemProps.Visible = (kb.CtrlKeyDown && kb.ShiftKeyDown);
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the default field information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnWriteDefaultFieldInfo(object args)
		{
			PaApp.FieldInfo.Save();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only show this menu if the CTRL and SHIFT key are down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateWriteDefaultFieldInfo(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null)
			{
				Keyboard kb =
					new Keyboard();

				itemProps.Visible = (kb.CtrlKeyDown && kb.ShiftKeyDown);
				itemProps.Update = true;
			}

			return true;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// <param name="args"></param>
		///// <returns>true if the message was handled</returns>
		///// ------------------------------------------------------------------------------------
		//protected bool OnFiltersDialog(object args)
		//{
		//    using (FiltersDialog filtersDlg = new FiltersDialog())
		//    {
		//        // TODO: Send a message indicating the filters were changed.
		//        if (filtersDlg.ShowDialog(this) == DialogResult.OK)
		//        {
		//        }
		//    }

		//    return true;
		//}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="configurationParameters"></param>
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
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}