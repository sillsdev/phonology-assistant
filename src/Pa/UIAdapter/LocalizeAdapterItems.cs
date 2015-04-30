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
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.Playback", "&Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.Playback", "&Playback", null, "Playback (F5)", "F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.StopPlayback":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.StopPlayback", "&Stop Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.StopPlayback", "&Stop Playback", null, "Stop Playback (F8)", "F8", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.EditSourceRecord":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.EditSourceRecord", "&Edit Source Record", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.EditSourceRecord", "&Edit Source Record", null, "Edit source record (Shift+F2)", "Shift+F2", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.File":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.File", "&File", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.File", "&File", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileNew":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.New", "&New Project...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.New", "&New Project...", null, "Create New Project (Ctrl+N)", "Ctrl+N", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileOpen":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Open", "&Open Project...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Open", "&Open Project...", null, "Open Project (Ctrl+O)", "Ctrl+O", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileClose":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Close", "&Close Project", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Close", "&Close Project", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ProjectSettings":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ProjectSettings", "P&roject Settings...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ProjectSettings", "P&roject Settings...", null, null, "Ctrl+R", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.BackupRestore":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.BackupRestore", "&Backup && Restore", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.BackupRestore", "&Backup && Restore", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileBackup":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Backup", "&Backup...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Backup", "&Backup...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileRestore":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Restore", "&Restore...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Restore", "&Restore...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportAs":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Export", "&Export", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Export", "&Export", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportHTML":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportHTML", "&HTML...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportHTML", "&HTML...", null, "Export to HTML File", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportWordXml":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportWordXml", "&Word XML...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportWordXml", "&Word XML...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportRTF":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportRTF", "&Rich Text Format (RTF)...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportRTF", "&Rich Text Format (RTF)...", null, "Export to RTF File", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportXLingPaper":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportXLingPaper", "X&LingPaper...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportXLingPaper", "X&LingPaper...", null, "Export to XLingPaper File", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ExportAsPAXML":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportPAXML", "{0} &XML...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportPAXML", "{0} &XML...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Playback":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Playback", "&Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Playback", "&Playback", null, "Playback (F5)", "F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.PlaybackRepeatedly":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.PlaybackRepeatedly", "Playback &Repeatedly", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.PlaybackRepeatedly", "Playback &Repeatedly", null, "Playback Repeatedly (Ctrl+F5)", "Ctrl+F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.StopPlayback":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.StopPlayback", "&Stop Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.StopPlayback", "&Stop Playback", null, "Stop Playback (F8)", "F8", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExit":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Exit", "E&xit", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Exit", "E&xit", null, "Exit Phonology Assistant", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Edit":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Edit", "&Edit", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Edit", "&Edit", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Find":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Find", "&Find", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Find", "&Find", null, "Find (Ctrl+F)", "Ctrl+F", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FindNext":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindNext", "Find &Next", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindNext", "Find &Next", null, "Find Next (F3)", "F3", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FindPrevious":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindPrevious", "Find &Previous", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindPrevious", "Find &Previous", null, "Find previous (Shift+F3)", "Shift+F3", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.EditSourceRecord":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.EditSourceRecord", "&Edit Source Record", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.EditSourceRecord", "&Edit Source Record", null, "Edit source record (Shift+F2)", "Shift+F2", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ReloadProject":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.ReloadProject", "&Reload Project Data Sources", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.ReloadProject", "&Reload Project Data Sources", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.View":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.View", "&View", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.View", "&View", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.DataCorpus":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DataCorpus", "&Data Corpus", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DataCorpus", "&Data Corpus", null, "Data Corpus View (Ctrl+Alt+D)", "Ctrl+Alt+D", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FindPhones":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.Search", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.Search", "&Search", null, "Search View (Ctrl+Alt+S)", "Ctrl+Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ConsonantChart":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ConsonantChart", "&Consonant Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ConsonantChart", "&Consonant Chart", null, "Consonant Chart View (Ctrl+Alt+C)", "Ctrl+Alt+C", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.VowelChart":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.VowelChart", "&Vowel Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.VowelChart", "&Vowel Chart", null, "Vowel Chart View (Ctrl+Alt+V)", "Ctrl+Alt+V", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.XYChart":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts", "Distri&bution Charts", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts", "Distri&bution Charts", null, "Distribution Charts View (Ctrl+Alt+B)", "Ctrl+Alt+B", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ShowCIEResults":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.MinimalPairs", "&Minimal Pairs", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.MinimalPairs", "&Minimal Pairs", null, "Minimal Pairs (Ctrl+M)", "Ctrl+M", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ShowCIESimilarResults":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults", "Similar E&nvironments", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults", "Similar E&nvironments", null, "Similar Environments (Ctrl+Alt+N)", "Ctrl+Alt+N", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.GroupBySortedField":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.GroupByPrimarySortField", "&Group by Primary Sort Field", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.GroupByPrimarySortField", "&Group by Primary Sort Field", null, "Group by Primary Sort Field (Ctrl+G)", "Ctrl+G", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.CollapseAllGroups":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CollapseAllGroups", "C&ollapse All Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CollapseAllGroups", "C&ollapse All Groups", null, "Collapse All Groups (Ctrl+Up)", "Ctrl+Up", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ExpandAllGroups":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ExpandAllGroups", "&Expand All Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ExpandAllGroups", "&Expand All Groups", null, "Expand All Groups (Ctrl+Down)", "Ctrl+Down", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ShowRecordPane":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane", "&Record View", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane", "&Record View", null, "Record View (F2)", "F2", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "MenuItems.ShowHtmlChart":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHtmlChart", "Toggle HTML Vie&w", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHtmlChart", "Toggle HTML Vie&w", null, "Toggle HTML View (F4)", "F4", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "MenuItems.ShowBackToEthnologue":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowBackToEthnologue", "Back (from Ethnologue)", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowBackToEthnologue", "Back (from Ethnologue)", null, "Back (from Ethnologue)", null, ((System.ComponentModel.IComponent)(item)));
                    break;
                case "MenuItems.ShowHistogram":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHistogram", "&Histogram", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHistogram", "&Histogram", null, "Show Histogram (Alt+H)", "Alt+H", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "MenuItems.Tools":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Tools", "&Tools", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Tools", "&Tools", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FiltersOnViewMenu":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.FiltersOnMenu", "&Filters...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.FiltersOnMenu", "&Filters...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ExperimentalTranscriptions":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.TranscriptionChanges", "&Transcription Changes...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.TranscriptionChanges", "&Transcription Changes...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.AmbiguousSequences":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.AmbiguousSequences", "&Ambiguous Sequences...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.AmbiguousSequences", "&Ambiguous Sequences...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.DescriptiveFeatures":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DescriptiveFeatures", "D&escriptive Features...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DescriptiveFeatures", "D&escriptive Features...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.DistinctiveFeatures":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DistinctiveFeatures", "D&istinctive Features...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DistinctiveFeatures", "D&istinctive Features...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Classes":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Classes", "&Classes...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Classes", "&Classes...", null, "Classes", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.UndefinedCharacters":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.UndefinedCharacters", "&Undefined Phonetic Characters...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.UndefinedCharacters", "&Undefined Phonetic Characters...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Options":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Options", "&Options...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Options", "&Options...", null, "Options", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Help":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Help", "&Help", "Text on main menu (i.e. menu across top of application window).", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Help", "&Help", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.HelpPA":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.PA", "Phonology Assistant...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.PA", "Phonology Assistant...", null, "Phonology Assistant Help (F1)", "F1", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.HelpTraining":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Training", "&Training", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Training", "&Training", null, "Training", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.StudentManual":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingStudentManual", "&Student Manual...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingStudentManual", "&Student Manual...", null, "Student Manual", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.InstructorGuide":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingInstructorGuide", "&Instructor Guide...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingInstructorGuide", "&Instructor Guide...", null, "Instructor Guide", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.HelpAbout":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.About", "&About Phonology Assistant...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.About", "&About Phonology Assistant...", null, "About Phonolgy Assistant...", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.UnDockView":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.UnDockView", "Undock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.UnDockView", "Undock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.", "Undock View", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.DockView":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.DockView", "Dock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.DockView", "Dock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.", "Dock View", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.OptionsMain":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.OptionsMain", "&Options...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.OptionsMain", "&Options...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.", "Options", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Filters":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.Filters", "&Filters...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.Filters", "&Filters...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.", "Filters", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.NoFilter":
                    LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.FiltersDropDown.NoFilter", "(&No Filter)", "Displayed on the drop-down of the filters button on the far right of the main menu");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.FiltersDropDown.NoFilter", "(&No Filter)", "Displayed on the drop-down of the filters button on the far right of the main menu", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.GroupBySortedField":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.GroupByPrimarySortField", "&Group by Primary Sort Field", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.GroupByPrimarySortField", "&Group by Primary Sort Field", null, "Group by Primary Sort Field (Ctrl+G)", "Ctrl+G", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.CollapseAllGroups":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.CollapseAllGroups", "C&ollapse All Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.CollapseAllGroups", "C&ollapse All Groups", null, "Collapse All Groups (Ctrl+Up)", "Ctrl+Up", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ExpandAllGroups":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ExpandAllGroups", "&Expand All Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ExpandAllGroups", "&Expand All Groups", null, "Expand All Groups (Ctrl+Down)", "Ctrl+Down", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowRecordPane":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowRecordPane", "&Record View", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowRecordPane", "&Record View", null, "Record View (F2)", "F2", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.Find":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Find", "&Find", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Find", "&Find", null, "Find (Ctrl+F)", "Ctrl+F", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.FindNext":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.FindNext", "Find &Next", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.FindNext", "Find &Next", null, "Find Next (F3)", "F3", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.Playback":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Playback", "&Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Playback", "&Playback", null, "Playback (F5)", "F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.PlaybackOnMenu":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackOnMenu", "&Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackOnMenu", "&Playback", null, "Playback (F5)", "F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.PlaybackRepeatedly":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackRepeatedly", "Playback &Repeatedly", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackRepeatedly", "Playback &Repeatedly", null, "Playback Repeatedly (Ctrl+F5)", "Ctrl+F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.StopPlayback":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.StopPlayback", "&Stop Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.StopPlayback", "&Stop Playback", null, "Stop Playback (F8)", "F8", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AdjustPlaybackSpeedParent":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeedParent", "Playback &Speed", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeedParent", "Playback &Speed", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AdjustPlaybackSpeed":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeed", "Playback &Speed", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeed", "Playback &Speed", null, "Playback Speed", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.PhoneticSort":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PhoneticSort", "Phonetic Sort Options", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PhoneticSort", "Phonetic Sort Options", null, "Phonetic Sort Options", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CutSavedPattern":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CutSavedPattern", "Cu&t", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CutSavedPattern", "Cu&t", null, "Cut saved pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CopySavedPattern":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopySavedPattern", "&Copy", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopySavedPattern", "&Copy", null, "Copy Saved pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.PasteSavedPattern":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.PasteSavedPattern", "&Paste", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.PasteSavedPattern", "&Paste", null, "Paste Saved Pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList": 
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList", "Show Results in &Active Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList", "Show Results in &Active Tab Group", null, "Show results in active tab group", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList", "Show Results in New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList", "Show Results in New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList", "Show Results in New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList", "Show Results in New Stac&ked Tab Group", null, "Show Results in New Stacked Tab Group", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CopyToCurrentPattern-FromSavedList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromSavedList", "C&opy Selected Pattern to Current Search Pattern", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromSavedList", "C&opy Selected Pattern to Current Search Pattern", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.RemovePattern-FromSavedList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromSavedList", "&Remove", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromSavedList", "&Remove", null, "Remove Pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.MoveToNewSideBySideTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewSideBySideTabGroup", "Move to New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewSideBySideTabGroup", "Move to New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.MoveToNewStackedTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewStackedTabGroup", "Move to New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewStackedTabGroup", "Move to New Stac&ked Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CloseTab":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTab", "Close Tab", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTab", "Close Tab", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CloseTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTabGroup", "Close Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTabGroup", "Close Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CloseAllTabGroups":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseAllTabGroups", "Close All Tab Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseAllTabGroups", "Close All Tab Groups", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList", "Show Results in &Active Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList", "Show Results in &Active Tab Group", null, "Show results in active tab group", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList", "Show Results in New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList", "Show Results in New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList", "Show Results in New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList", "Show Results in New Stac&ked Tab Group", null, "Show Results in New Stacked Tab Group", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CopyToCurrentPattern-FromRecentList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromRecentList", "C&opy Selected Pattern to Current Search Pattern", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromRecentList", "C&opy Selected Pattern to Current Search Pattern", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.RemovePattern-FromRecentList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromRecentList", "&Remove", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromRecentList", "&Remove", null, "Remove Pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ClearRecentPatternList-FromRecentList":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearRecentPatternList-FromRecentList", "Clear &List", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearRecentPatternList-FromRecentList", "Clear &List", null, "Clear List", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowResults":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowResults", "&Show Results", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowResults", "&Show Results", null, "Show Search Results (Alt+S)", "Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertIntoPattern":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoPattern", "&Insert", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoPattern", "&Insert", null, "Insert Element into Current Search Pattern (Alt+I)", "Alt+I", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertConsonant":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertConsonant", "[C] Any &Consonant", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertConsonant", "[C] Any &Consonant", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertVowel":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertVowel", "[V] Any &Vowel", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertVowel", "[V] Any &Vowel", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertZeroOrMore":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertOneOrMore":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertWordBoundary":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordBoundary", "# Space/&Word Boundary", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordBoundary", "# Space/&Word Boundary", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertDiacriticPlaceholder":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null, null, "Ctrl+0", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertANDGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertANDGroup", "[  ] A&ND Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertANDGroup", "[  ] A&ND Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertORGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertORGroup", "{  } O&R Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertORGroup", "{  } O&R Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.InsertWordInitial":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordInitial", "/#_* Word Initial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordInitial", "/#_* Word Initial", null, null, "Ctrl+1", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.InsertWordFinal":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordFinal", "/*_# Word final", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordFinal", "/*_# Word final", null, null, "Ctrl+9", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.InsertAnywhere":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertAnywhere", "/*_* Anywhere", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertAnywhere", "/*_* Anywhere", null, null, "Ctrl+5", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.InsertWordMedial":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null, null, "Ctrl+2", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.SearchOptions":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SearchOptions", "&Options", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SearchOptions", "&Options", null, "Search Options (Alt+O)", "Alt+O", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SavePattern":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePattern", "Save", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePattern", "Save", null, "Save Search Pattern (Ctrl+S)", "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SavePatternOnMenu":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternOnMenu", "&Save", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternOnMenu", "&Save", null, "Save Current Search Pattern (Ctrl+S)", "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SavePatternAs":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternAs", "Save &As...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternAs", "Save &As...", null, "Save Current Search Pattern As...", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ClearPattern":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearPattern", "&Clear", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearPattern", "&Clear", null, "Clear Current Search Pattern and Results (Alt+C)", "Alt+C", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowCIEResults":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIEResults", "&Minimal Pairs", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIEResults", "&Minimal Pairs", null, "Minimal Pairs (Ctrl+M)", "Ctrl+M", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.ShowCIESimilarResults":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIESimilarResults", "Similar E&nvironments", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIESimilarResults", "Similar E&nvironments", null, "Similar Environments (Ctrl+Alt+N)", "Ctrl+Alt+N", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CharChartSearchContextMenu":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CharChartSearchContextMenu", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CharChartSearchContextMenu", "&Search", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchAnywhere":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAnywhere", "Anywhere", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAnywhere", "Anywhere", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchInitial":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchInitial", "Word Initially", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchInitial", "Word Initially", null, "Search for phone word initially", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchMedial":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchMedial", "Word Medial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchMedial", "Word Medial", null, "Search for phone word medial", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchFinal":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchFinal", "Word Finally", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchFinal", "Word Finally", null, "Search for phone word finally", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchAlone":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAlone", "Alone", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAlone", "Alone", null, "Search for phone alone", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearch":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearch", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearch", "&Search", null, "Search for Phone (Alt+S)", "Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchAnywhere":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAnywhere", "Anywhere", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAnywhere", "Anywhere", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchInitial": 
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchInitial", "Word Initially", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchInitial", "Word Initially", null, "Search for phone word initially", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchMedial":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchMedial", "Word Medial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchMedial", "Word Medial", null, "Search for phone word medial", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchFinal":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchFinal", "Word Finally", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchFinal", "Word Finally", null, "Search for phone word finally", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchAlone":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAlone", "Alone", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAlone", "Alone", null, "Search for phone alone", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.IgnoredSymbols":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.IgnoredSymbols", "Ignore", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.IgnoredSymbols", "Ignore", null, "Select Ignored Symbols", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowHistogram":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHistogram", "&Histogram", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHistogram", "&Histogram", null, "Show Histogram (Alt+H)", "Alt+H", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowHtmlChart":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHtmlChart", "Toggle HTML Vie&w", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHtmlChart", "Toggle HTML Vie&w", null, "Toggle HTML View (F4)", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowBackToEthnologue":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowBackToEthnologue", "Back (from Ethnologue)", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowBackToEthnologue", "Back (from Ethnologue)", null, "Back (from Ethnologue)", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertIntoChart":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertIntoChart", "&Insert", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertIntoChart", "&Insert", null, "Insert Element into Current Chart Cell (Alt+I)", "Alt+I", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertConsonant":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertConsonant", "[C] Any &Consonant", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertConsonant", "[C] Any &Consonant", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertVowel":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertVowel", "[V] Any &Vowel", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertVowel", "[V] Any &Vowel", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertZeroOrMore":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertOneOrMore":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertWordBoundary":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordBoundary", "# Space/&Word Boundary", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordBoundary", "# Space/&Word Boundary", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertDiacriticPlaceholder":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null, null, "Ctrl+0", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertWordInitial":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordInitial", "/#_* Word Initial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordInitial", "/#_* Word Initial", null, null, "Ctrl+1", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertWordFinal":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordFinal", "/*_# Word final", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordFinal", "/*_# Word final", null, null, "Ctrl+9", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertAnywhere":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertAnywhere", "/*_* Anywhere", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertAnywhere", "/*_* Anywhere", null, null, "Ctrl+5", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertWordMedial":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null, null, "Ctrl+2", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertANDGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertANDGroup", "[  ] A&ND Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertANDGroup", "[  ] A&ND Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertORGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertORGroup", "{  } O&R Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertORGroup", "{  } O&R Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.SearchOptions":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SearchOptions", "Search &Options", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SearchOptions", "Search &Options", null, "Search Options for Current Chart Column (Alt+O)", "Alt+O", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.RunChartSearch":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RunChartSearch", "Fi&ll Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RunChartSearch", "Fi&ll Chart", null, "Fill Chart with Results (Alt+L)", "Alt+L", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.BeginSearch":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.BeginSearch", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.BeginSearch", "&Search", null, "Search (Alt+S)", "Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.AddResultsToCurrentTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.AddResultsToNewSideBySideTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.AddResultsToNewStackedTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.SaveChart":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SaveChart", "S&ave", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SaveChart", "S&ave", null, "Save Current Chart (Ctrl+S)", "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ClearChart":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearChart", "&Clear", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearChart", "&Clear", null, "Clear Chart (Alt+C)", "Alt+C", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.DeleteChartRow":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartRow", "Delete &Row", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartRow", "Delete &Row", null, "Delete Row (Alt+R)", "Alt+R", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.DeleteChartColumn":
                    LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartColumn", "Delete C&olumn", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartColumn", "Delete C&olumn", null, "Delete Column (Alt+O)", "Alt+O", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.BeginSearch":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.BeginSearch", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.BeginSearch", "&Search", null, "Search (Alt+S)", "Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AddResultsToCurrentTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AddResultsToNewSideBySideTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AddResultsToNewStackedTabGroup":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.RunChartSearch":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.RunChartSearch", "Fi&ll Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.RunChartSearch", "Fi&ll Chart", null, "Fill Chart with Results (Alt+L)", "Alt+L", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertIntoChart":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoChart", "&Insert", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoChart", "&Insert", null, "Insert Element into Current Chart Cell (Alt+I)", "Alt+I", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SaveChart":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChart", "S&ave", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChart", "S&ave", null, "Save Current Chart (Ctrl+S)", "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SaveChartOnMenu":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartOnMenu", "&Save", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartOnMenu", "&Save", null, null, "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SaveChartAs":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartAs", "Save &As...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartAs", "Save &As...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ResetChart":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ResetChart", "Reset Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ResetChart", "Reset Chart", null, "Reset Chart", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ClearChart":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearChart", "&Clear", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearChart", "&Clear", null, "Clear Chart (Alt+C)", "Alt+C", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.DeleteChartRow":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartRow", "Delete &Row", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartRow", "Delete &Row", null, "Delete Row (Alt+R)", "Alt+R", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.DeleteChartColumn":
                    LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartColumn", "Delete C&olumn", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartColumn", "Delete C&olumn", null, "Delete Column (Alt+O)", "Alt+O", ((System.ComponentModel.IComponent)(item)));
                    break;
            }
        }
    }
}