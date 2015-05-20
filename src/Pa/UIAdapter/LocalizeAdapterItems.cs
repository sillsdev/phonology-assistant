// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
//
using System.Windows.Forms;
using L10NSharp;
using SIL.FieldWorks.Common.UIAdapters;

namespace SIL.Pa
{
    public static class LocalizeAdapterItems
    {
        public static void LocalizeItem(object item, string id, TMItemProperties itemProps)
        {
            switch (id)
            {
                case "ContextMenuItems.Playback":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.Playback", "&Playback", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.Playback" + "_ToolTip_", "Playback (F5)", null);
                    break;

                case "ContextMenuItems.StopPlayback":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.StopPlayback", "&Stop Playback", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.StopPlayback" + "_ToolTip_", "Stop Playback (F8)", null);
                    break;

                case "ContextMenuItems.EditSourceRecord":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.EditSourceRecord", "&Edit Source Record", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.EditSourceRecord" + "_ToolTip_", "Edit source record (Shift+F2)", null);
                    break;

                case "MenuItems.File":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.File", "&File", "Text on main menu (i.e. menu across top of application window).");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.File" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.FileNew":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.New", "&New Project...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.New" + "_ToolTip_", "Create New Project (Ctrl+N)", null);
                    break;

                case "MenuItems.FileOpen":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Open", "&Open Project...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Open" + "_ToolTip_", "Open Project (Ctrl+O)", null);
                    break;

                case "MenuItems.FileClose":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Close", "&Close Project", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Close" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.ProjectSettings":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ProjectSettings", "P&roject Settings...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ProjectSettings" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.BackupRestore":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.BackupRestore", "&Backup && Restore", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.BackupRestore" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.FileBackup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Backup", "&Backup...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Backup" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.FileRestore":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Restore", "&Restore...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Restore" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.FileExportAs":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Export", "&Export", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Export" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.FileExportHTML":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportHTML", "&HTML...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportHTML" + "_ToolTip_", "Export to HTML File", null);
                    break;

                case "MenuItems.FileExportWordXml":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportWordXml", "&Word XML...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportWordXml" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.FileExportRTF":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportRTF", "&Rich Text Format (RTF)...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportRTF" + "_ToolTip_", "Export to RTF File", null);
                    break;

                case "MenuItems.FileExportXLingPaper":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportXLingPaper", "X&LingPaper...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportXLingPaper" + "_ToolTip_", "Export to XLingPaper File", null);
                    break;

                case "MenuItems.ExportAsPAXML":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportPAXML", "{0} &XML...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportPAXML" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.Playback":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Playback", "&Playback", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Playback" + "_ToolTip_", "Playback (F5)", null);
                    break;

                case "MenuItems.PlaybackRepeatedly":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.PlaybackRepeatedly", "Playback &Repeatedly", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.PlaybackRepeatedly" + "_ToolTip_", "Playback Repeatedly (Ctrl+F5)", null);
                    break;

                case "MenuItems.StopPlayback":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.StopPlayback", "&Stop Playback", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.StopPlayback" + "_ToolTip_", "Stop Playback (F8)", null);
                    break;

                case "MenuItems.FileExit":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Exit", "E&xit", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Exit" + "_ToolTip_", "Exit Phonology Assistant", null);
                    break;

                case "MenuItems.Edit":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Edit", "&Edit", "Text on main menu (i.e. menu across top of application window).");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Edit" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.Find":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Find", "&Find", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Find" + "_ToolTip_", "Find (Ctrl+F)", null);
                    break;

                case "MenuItems.FindNext":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindNext", "Find &Next", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindNext" + "_ToolTip_", "Find Next (F3)", null);
                    break;

                case "MenuItems.FindPrevious":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindPrevious", "Find &Previous", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindPrevious" + "_ToolTip_", "Find previous (Shift+F3)", null);
                    break;

                case "MenuItems.EditSourceRecord":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.EditSourceRecord", "&Edit Source Record", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.EditSourceRecord" + "_ToolTip_", "Edit source record (Shift+F2)", null);
                    break;

                case "MenuItems.ReloadProject":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.ReloadProject", "&Reload Project Data Sources", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.ReloadProject" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.View":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.View", "&View", "Text on main menu (i.e. menu across top of application window).");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.View" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.DataCorpus":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DataCorpus", "&Data Corpus", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DataCorpus" + "_ToolTip_", "Data Corpus View (Ctrl+Alt+D)", null);
                    break;

                case "MenuItems.FindPhones":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.Search", "&Search", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.Search" + "_ToolTip_", "Search View (Ctrl+Alt+S)", null);
                    break;

                case "MenuItems.ConsonantChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ConsonantChart", "&Consonant Chart", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ConsonantChart" + "_ToolTip_", "Consonant Chart View (Ctrl+Alt+C)", null);
                    break;

                case "MenuItems.VowelChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.VowelChart", "&Vowel Chart", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.VowelChart" + "_ToolTip_", "Vowel Chart View (Ctrl+Alt+V)", null);
                    break;

                case "MenuItems.XYChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts", "Distri&bution Charts", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts" + "_ToolTip_", "Distribution Charts View (Ctrl+Alt+B)", null);
                    break;

                case "MenuItems.ShowCIEResults":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.MinimalPairs", "&Minimal Pairs", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.MinimalPairs" + "_ToolTip_", "Minimal Pairs (Ctrl+M)", null);
                    break;

                case "MenuItems.ShowCIESimilarResults":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults", "Similar E&nvironments", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults" + "_ToolTip_", "Similar Environments (Ctrl+Alt+N)", null);
                    break;

                case "MenuItems.GroupBySortedField":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.GroupByPrimarySortField", "&Group by Primary Sort Field", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.GroupByPrimarySortField" + "_ToolTip_", "Group by Primary Sort Field (Ctrl+G)", null);
                    break;

                case "MenuItems.CollapseAllGroups":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CollapseAllGroups", "C&ollapse All Groups", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CollapseAllGroups" + "_ToolTip_", "Collapse All Groups (Ctrl+Up)", null);
                    break;

                case "MenuItems.ExpandAllGroups":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ExpandAllGroups", "&Expand All Groups", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ExpandAllGroups" + "_ToolTip_", "Expand All Groups (Ctrl+Down)", null);
                    break;

                case "MenuItems.ShowRecordPane":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane", "&Record View", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane" + "_ToolTip_", "Record View (F2)", null);
                    break;
                case "MenuItems.ShowHtmlChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHtmlChart", "Toggle HTML Vie&w", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHtmlChart" + "_ToolTip_", "Toggle HTML View (F4)", null);
                    break;
                case "MenuItems.ShowBackToEthnologue":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowBackToEthnologue", "Back (from Ethnologue)", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowBackToEthnologue" + "_ToolTip_", "Back (from Ethnologue)", null);
                    break;
                case "MenuItems.ShowHistogram":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHistogram", "&Histogram", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHistogram" + "_ToolTip_", "Show Histogram (Alt+H)", null);
                    break;
                case "MenuItems.Tools":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Tools", "&Tools", "Text on main menu (i.e. menu across top of application window).");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Tools" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.FiltersOnViewMenu":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.FiltersOnMenu", "&Filters...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.FiltersOnMenu" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.ExperimentalTranscriptions":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.TranscriptionChanges", "&Transcription Changes...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.TranscriptionChanges" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.AmbiguousSequences":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.AmbiguousSequences", "&Ambiguous Sequences...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.AmbiguousSequences" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.DescriptiveFeatures":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DescriptiveFeatures", "D&escriptive Features...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DescriptiveFeatures" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.DistinctiveFeatures":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DistinctiveFeatures", "D&istinctive Features...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DistinctiveFeatures" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.Classes":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Classes", "&Classes...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Classes" + "_ToolTip_", "Classes", null);
                    break;

                case "MenuItems.UndefinedCharacters":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.UndefinedCharacters", "&Undefined Phonetic Characters...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.UndefinedCharacters" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.Options":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Options", "&Options...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Options" + "_ToolTip_", "Options", null);
                    break;

                case "MenuItems.Help":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Help", "&Help", "Text on main menu (i.e. menu across top of application window).");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Help" + "_ToolTip_", null, null);
                    break;

                case "MenuItems.HelpPA":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.PA", "Phonology Assistant...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.PA" + "_ToolTip_", "Phonology Assistant Help (F1)", null);
                    break;

                case "MenuItems.HelpTraining":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Training", "&Training", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Training" + "_ToolTip_", "Training", null);
                    break;

                case "MenuItems.StudentManual":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingStudentManual", "&Student Manual...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingStudentManual" + "_ToolTip_", "Student Manual", null);
                    break;

                case "MenuItems.InstructorGuide":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingInstructorGuide", "&Instructor Guide...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingInstructorGuide" + "_ToolTip_", "Instructor Guide", null);
                    break;

                case "MenuItems.HelpAbout":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.About", "&About Phonology Assistant...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.About" + "_ToolTip_", "About Phonolgy Assistant...", null);
                    break;

                case "MenuItems.UnDockView":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.UnDockView", "Undock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.UnDockView" + "_ToolTip_", "Undock View", null);
                    break;

                case "MenuItems.DockView":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.DockView", "Dock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.DockView" + "_ToolTip_", "Dock View", null);
                    break;

                case "MenuItems.OptionsMain":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.OptionsMain", "&Options...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.OptionsMain" + "_ToolTip_", "Options", null);
                    break;

                case "MenuItems.Filters":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.Filters", "&Filters...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.Filters" + "_ToolTip_", "Filters", null);
                    break;

                case "MenuItems.NoFilter":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.FiltersDropDown.NoFilter", "(&No Filter)", "Displayed on the drop-down of the filters button on the far right of the main menu");
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.FiltersDropDown.NoFilter" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.GroupBySortedField":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.GroupByPrimarySortField", "&Group by Primary Sort Field", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.GroupByPrimarySortField" + "_ToolTip_", "Group by Primary Sort Field (Ctrl+G)", null);
                    break;

                case "ToolbarItems.CollapseAllGroups":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.CollapseAllGroups", "C&ollapse All Groups", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.CollapseAllGroups" + "_ToolTip_", "Collapse All Groups (Ctrl+Up)", null);
                    break;

                case "ToolbarItems.ExpandAllGroups":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ExpandAllGroups", "&Expand All Groups", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ExpandAllGroups" + "_ToolTip_", "Expand All Groups (Ctrl+Down)", null);
                    break;

                case "ToolbarItems.ShowRecordPane":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowRecordPane", "&Record View", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowRecordPane" + "_ToolTip_", "Record View (F2)", null);
                    break;

                case "ToolbarItems.Find":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Find", "&Find", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Find" + "_ToolTip_", "Find (Ctrl+F)", null);
                    break;

                case "ToolbarItems.FindNext":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.FindNext", "Find &Next", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.FindNext" + "_ToolTip_", "Find Next (F3)", null);
                    break;

                case "ToolbarItems.Playback":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Playback", "&Playback", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Playback" + "_ToolTip_", "Playback (F5)", null);
                    break;

                case "ToolbarItems.PlaybackOnMenu":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackOnMenu", "&Playback", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackOnMenu" + "_ToolTip_", "Playback (F5)", null);
                    break;

                case "ToolbarItems.PlaybackRepeatedly":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackRepeatedly", "Playback &Repeatedly", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackRepeatedly" + "_ToolTip_", "Playback Repeatedly (Ctrl+F5)", null);
                    break;

                case "ToolbarItems.StopPlayback":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.StopPlayback", "&Stop Playback", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.StopPlayback" + "_ToolTip_", "Stop Playback (F8)", null);
                    break;

                case "ToolbarItems.AdjustPlaybackSpeedParent":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeedParent", "Playback &Speed", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeedParent" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.AdjustPlaybackSpeed":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeed", "Playback &Speed", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeed" + "_ToolTip_", "Playback Speed", null);
                    break;

                case "ToolbarItems.PhoneticSort":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PhoneticSort", "Phonetic Sort Options", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PhoneticSort" + "_ToolTip_", "Phonetic Sort Options", null);
                    break;

                case "ContextMenuItems.CutSavedPattern":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CutSavedPattern", "Cu&t", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CutSavedPattern" + "_ToolTip_", "Cut saved pattern", null);
                    break;

                case "ContextMenuItems.CopySavedPattern":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopySavedPattern", "&Copy", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopySavedPattern" + "_ToolTip_", "Copy Saved pattern", null);
                    break;

                case "ContextMenuItems.PasteSavedPattern":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.PasteSavedPattern", "&Paste", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.PasteSavedPattern" + "_ToolTip_", "Paste Saved Pattern", null);
                    break;

                case "ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList": 
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList", "Show Results in &Active Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList" + "_ToolTip_", "Show results in active tab group", null);
                    break;

                case "ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList", "Show Results in New Side-&By-Side Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList", "Show Results in New Stac&ked Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList" + "_ToolTip_", "Show Results in New Stacked Tab Group", null);
                    break;

                case "ContextMenuItems.CopyToCurrentPattern-FromSavedList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromSavedList", "C&opy Selected Pattern to Current Search Pattern", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromSavedList" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.RemovePattern-FromSavedList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromSavedList", "&Remove", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromSavedList" + "_ToolTip_", "Remove Pattern", null);
                    break;

                case "ContextMenuItems.MoveToNewSideBySideTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewSideBySideTabGroup", "Move to New Side-&By-Side Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewSideBySideTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.MoveToNewStackedTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewStackedTabGroup", "Move to New Stac&ked Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewStackedTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.CloseTab":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTab", "Close Tab", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTab" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.CloseTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTabGroup", "Close Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.CloseAllTabGroups":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseAllTabGroups", "Close All Tab Groups", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseAllTabGroups" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList", "Show Results in &Active Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList" + "_ToolTip_", "Show results in active tab group", null);
                    break;

                case "ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList", "Show Results in New Side-&By-Side Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList", "Show Results in New Stac&ked Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList" + "_ToolTip_", "Show Results in New Stacked Tab Group", null);
                    break;

                case "ContextMenuItems.CopyToCurrentPattern-FromRecentList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromRecentList", "C&opy Selected Pattern to Current Search Pattern", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromRecentList" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.RemovePattern-FromRecentList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromRecentList", "&Remove", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromRecentList" + "_ToolTip_", "Remove Pattern", null);
                    break;

                case "ContextMenuItems.ClearRecentPatternList-FromRecentList":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearRecentPatternList-FromRecentList", "Clear &List", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearRecentPatternList-FromRecentList" + "_ToolTip_", "Clear List", null);
                    break;

                case "ToolbarItems.ShowResults":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowResults", "&Show Results", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowResults" + "_ToolTip_", "Show Search Results (Alt+S)", null);
                    break;

                case "ToolbarItems.InsertIntoPattern":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoPattern", "&Insert", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoPattern" + "_ToolTip_", "Insert Element into Current Search Pattern (Alt+I)", null);
                    break;

                case "ToolbarItems.InsertConsonant":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertConsonant", "[C] Any &Consonant", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertConsonant" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.InsertVowel":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertVowel", "[V] Any &Vowel", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertVowel" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.InsertZeroOrMore":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertZeroOrMore" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.InsertOneOrMore":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertOneOrMore" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.InsertWordBoundary":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordBoundary", "# Space/&Word Boundary", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordBoundary" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.InsertDiacriticPlaceholder":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertDiacriticPlaceholder" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.InsertANDGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertANDGroup", "[  ] A&ND Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertANDGroup" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.InsertORGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertORGroup", "{  } O&R Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertORGroup" + "_ToolTip_", null, null);
                    break;
                case "ToolbarItems.InsertWordInitial":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordInitial", "/#_* Word Initial", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordInitial" + "_ToolTip_", null, null);
                    break;
                case "ToolbarItems.InsertWordFinal":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordFinal", "/*_# Word final", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordFinal" + "_ToolTip_", null, null);
                    break;
                case "ToolbarItems.InsertAnywhere":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertAnywhere", "/*_* Anywhere", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertAnywhere" + "_ToolTip_", null, null);
                    break;
                case "ToolbarItems.InsertWordMedial":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordMedial" + "_ToolTip_", null, null);
                    break;
                case "ToolbarItems.SearchOptions":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SearchOptions", "&Options", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SearchOptions" + "_ToolTip_", "Search Options (Alt+O)", null);
                    break;

                case "ToolbarItems.SavePattern":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePattern", "Save", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePattern" + "_ToolTip_", "Save Search Pattern (Ctrl+S)", null);
                    break;

                case "ToolbarItems.SavePatternOnMenu":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternOnMenu", "&Save", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternOnMenu" + "_ToolTip_", "Save Current Search Pattern (Ctrl+S)", null);
                    break;

                case "ToolbarItems.SavePatternAs":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternAs", "Save &As...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternAs" + "_ToolTip_", "Save Current Search Pattern As...", null);
                    break;

                case "ToolbarItems.ClearPattern":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearPattern", "&Clear", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearPattern" + "_ToolTip_", "Clear Current Search Pattern and Results (Alt+C)", null);
                    break;

                case "ToolbarItems.ShowCIEResults":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIEResults", "&Minimal Pairs", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIEResults" + "_ToolTip_", "Minimal Pairs (Ctrl+M)", null);
                    break;
                case "ToolbarItems.ShowCIESimilarResults":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIESimilarResults", "Similar E&nvironments", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIESimilarResults" + "_ToolTip_", "Similar Environments (Ctrl+Alt+N)", null);
                    break;

                case "ContextMenuItems.CharChartSearchContextMenu":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CharChartSearchContextMenu", "&Search", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CharChartSearchContextMenu" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.ChartPhoneSearchAnywhere":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAnywhere", "Anywhere", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAnywhere" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.ChartPhoneSearchInitial":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchInitial", "Word Initially", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchInitial" + "_ToolTip_", "Search for phone word initially", null);
                    break;

                case "ContextMenuItems.ChartPhoneSearchMedial":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchMedial", "Word Medial", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchMedial" + "_ToolTip_", "Search for phone word medial", null);
                    break;

                case "ContextMenuItems.ChartPhoneSearchFinal":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchFinal", "Word Finally", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchFinal" + "_ToolTip_", "Search for phone word finally", null);
                    break;

                case "ContextMenuItems.ChartPhoneSearchAlone":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAlone", "Alone", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAlone" + "_ToolTip_", "Search for phone alone", null);
                    break;

                case "ToolbarItems.ChartPhoneSearch":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearch", "&Search", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearch" + "_ToolTip_", "Search for Phone (Alt+S)", null);
                    break;

                case "ToolbarItems.ChartPhoneSearchAnywhere":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAnywhere", "Anywhere", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAnywhere" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.ChartPhoneSearchInitial": 
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchInitial", "Word Initially", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchInitial" + "_ToolTip_", "Search for phone word initially", null);
                    break;

                case "ToolbarItems.ChartPhoneSearchMedial":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchMedial", "Word Medial", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchMedial" + "_ToolTip_", "Search for phone word medial", null);
                    break;

                case "ToolbarItems.ChartPhoneSearchFinal":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchFinal", "Word Finally", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchFinal" + "_ToolTip_", "Search for phone word finally", null);
                    break;

                case "ToolbarItems.ChartPhoneSearchAlone":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAlone", "Alone", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAlone" + "_ToolTip_", "Search for phone alone", null);
                    break;

                case "ToolbarItems.IgnoredSymbols":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.IgnoredSymbols", "Ignore", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.IgnoredSymbols" + "_ToolTip_", "Select Ignored Symbols", null);
                    break;

                case "ToolbarItems.ShowHistogram":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHistogram", "&Histogram", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHistogram" + "_ToolTip_", "Show Histogram (Alt+H)", null);
                    break;

                case "ToolbarItems.ShowHtmlChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHtmlChart", "Toggle HTML Vie&w", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHtmlChart" + "_ToolTip_", "Toggle HTML View (F4)", null);
                    break;

                case "ToolbarItems.ShowBackToEthnologue":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowBackToEthnologue", "Back (from Ethnologue)", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowBackToEthnologue" + "_ToolTip_", "Back (from Ethnologue)", null);
                    break;

                case "ContextMenuItems.InsertIntoChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertIntoChart", "&Insert", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertIntoChart" + "_ToolTip_", "Insert Element into Current Chart Cell (Alt+I)", null);
                    break;

                case "ContextMenuItems.InsertConsonant":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertConsonant", "[C] Any &Consonant", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertConsonant" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.InsertVowel":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertVowel", "[V] Any &Vowel", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertVowel" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.InsertZeroOrMore":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertZeroOrMore" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.InsertOneOrMore":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertOneOrMore" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.InsertWordBoundary":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordBoundary", "# Space/&Word Boundary", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordBoundary" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.InsertDiacriticPlaceholder":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertDiacriticPlaceholder" + "_ToolTip_", null, null);
                    break;
                case "ContextMenuItems.InsertWordInitial":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordInitial", "/#_* Word Initial", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordInitial" + "_ToolTip_", null, null);
                    break;
                case "ContextMenuItems.InsertWordFinal":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordFinal", "/*_# Word final", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordFinal" + "_ToolTip_", null, null);
                    break;
                case "ContextMenuItems.InsertAnywhere":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertAnywhere", "/*_* Anywhere", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertAnywhere" + "_ToolTip_", null, null);
                    break;
                case "ContextMenuItems.InsertWordMedial":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordMedial" + "_ToolTip_", null, null);
                    break;
                case "ContextMenuItems.InsertANDGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertANDGroup", "[  ] A&ND Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertANDGroup" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.InsertORGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertORGroup", "{  } O&R Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertORGroup" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.SearchOptions":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SearchOptions", "Search &Options", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SearchOptions" + "_ToolTip_", "Search Options for Current Chart Column (Alt+O)", null);
                    break;

                case "ContextMenuItems.RunChartSearch":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RunChartSearch", "Fi&ll Chart", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RunChartSearch" + "_ToolTip_", "Fill Chart with Results (Alt+L)", null);
                    break;

                case "ContextMenuItems.BeginSearch":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.BeginSearch", "&Search", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.BeginSearch" + "_ToolTip_", "Search (Alt+S)", null);
                    break;

                case "ContextMenuItems.AddResultsToCurrentTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToCurrentTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.AddResultsToNewSideBySideTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewSideBySideTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.AddResultsToNewStackedTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewStackedTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ContextMenuItems.SaveChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SaveChart", "S&ave", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SaveChart" + "_ToolTip_", "Save Current Chart (Ctrl+S)", null);
                    break;

                case "ContextMenuItems.ClearChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearChart", "&Clear", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearChart" + "_ToolTip_", "Clear Chart (Alt+C)", null);
                    break;

                case "ContextMenuItems.DeleteChartRow":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartRow", "Delete &Row", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartRow" + "_ToolTip_", "Delete Row (Alt+R)", null);
                    break;

                case "ContextMenuItems.DeleteChartColumn":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartColumn", "Delete C&olumn", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartColumn" + "_ToolTip_", "Delete Column (Alt+O)", null);
                    break;

                case "ToolbarItems.BeginSearch":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.BeginSearch", "&Search", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.BeginSearch" + "_ToolTip_", "Search (Alt+S)", null);
                    break;

                case "ToolbarItems.AddResultsToCurrentTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToCurrentTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.AddResultsToNewSideBySideTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewSideBySideTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.AddResultsToNewStackedTabGroup":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewStackedTabGroup" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.RunChartSearch":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.RunChartSearch", "Fi&ll Chart", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.RunChartSearch" + "_ToolTip_", "Fill Chart with Results (Alt+L)", null);
                    break;

                case "ToolbarItems.InsertIntoChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoChart", "&Insert", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoChart" + "_ToolTip_", "Insert Element into Current Chart Cell (Alt+I)", null);
                    break;

                case "ToolbarItems.SaveChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChart", "S&ave", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChart" + "_ToolTip_", "Save Current Chart (Ctrl+S)", null);
                    break;

                case "ToolbarItems.SaveChartOnMenu":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartOnMenu", "&Save", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartOnMenu" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.SaveChartAs":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartAs", "Save &As...", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartAs" + "_ToolTip_", null, null);
                    break;

                case "ToolbarItems.ResetChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ResetChart", "Reset Chart", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ResetChart" + "_ToolTip_", "Reset Chart", null);
                    break;

                case "ToolbarItems.ClearChart":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearChart", "&Clear", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearChart" + "_ToolTip_", "Clear Chart (Alt+C)", null);
                    break;

                case "ToolbarItems.DeleteChartRow":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartRow", "Delete &Row", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartRow" + "_ToolTip_", "Delete Row (Alt+R)", null);
                    break;

                case "ToolbarItems.DeleteChartColumn":
                    itemProps.Text = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartColumn", "Delete C&olumn", null);
                    itemProps.Tooltip = LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartColumn" + "_ToolTip_", "Delete Column (Alt+O)", null);
                    break;
            }
        }
    }
}