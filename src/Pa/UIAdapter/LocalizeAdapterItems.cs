// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.ComponentModel;
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
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.Playback", "&Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.Playback", "&Playback", null, "Playback (F5)", "F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.StopPlayback":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.StopPlayback", "&Stop Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.StopPlayback", "&Stop Playback", null, "Stop Playback (F8)", "F8", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.EditSourceRecord":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.EditSourceRecord", "&Edit Source Record", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.EditSourceRecord", "&Edit Source Record", null, "Edit source record (Shift+F2)", "Shift+F2", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.File":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.File", "&File", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.File", "&File", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileNew":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.New", "&New Project...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.New", "&New Project...", null, "Create New Project (Ctrl+N)", "Ctrl+N", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileOpen":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.Open", "&Open Project...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Open", "&Open Project...", null, "Open Project (Ctrl+O)", "Ctrl+O", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileClose":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.Close", "&Close Project", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Close", "&Close Project", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ProjectSettings":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.ProjectSettings", "P&roject Settings...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ProjectSettings", "P&roject Settings...", null, null, "Ctrl+R", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.BackupRestore":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.BackupRestore", "&Backup && Restore", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.BackupRestore", "&Backup && Restore", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileBackup":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.Backup", "&Backup...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Backup", "&Backup...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileRestore":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.Restore", "&Restore...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Restore", "&Restore...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportAs":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.Export", "&Export", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Export", "&Export", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportHTML":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.ExportHTML", "&HTML...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportHTML", "&HTML...", null, "Export to HTML File", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportWordXml":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.ExportWordXml", "&Word XML...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportWordXml", "&Word XML...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportRTF":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.ExportRTF", "&Rich Text Format (RTF)...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportRTF", "&Rich Text Format (RTF)...", null, "Export to RTF File", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExportXLingPaper":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.ExportXLingPaper", "X&LingPaper...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportXLingPaper", "X&LingPaper...", null, "Export to XLingPaper File", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ExportAsPAXML":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.ExportPAXML", "{0} &XML...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.ExportPAXML", "{0} &XML...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Playback":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.Playback", "&Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Playback", "&Playback", null, "Playback (F5)", "F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.PlaybackRepeatedly":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.PlaybackRepeatedly", "Playback &Repeatedly", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.PlaybackRepeatedly", "Playback &Repeatedly", null, "Playback Repeatedly (Ctrl+F5)", "Ctrl+F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.StopPlayback":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.StopPlayback", "&Stop Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.StopPlayback", "&Stop Playback", null, "Stop Playback (F8)", "F8", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FileExit":
                    Item(itemProps, "Menus and Toolbars.MenuItems.FileMenu.Exit", "E&xit", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileMenu.Exit", "E&xit", null, "Exit Phonology Assistant", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Edit":
                    Item(itemProps, "Menus and Toolbars.MenuItems.EditMenu.Edit", "&Edit", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Edit", "&Edit", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Find":
                    Item(itemProps, "Menus and Toolbars.MenuItems.EditMenu.Find", "&Find", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.Find", "&Find", null, "Find (Ctrl+F)", "Ctrl+F", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FindNext":
                    Item(itemProps, "Menus and Toolbars.MenuItems.EditMenu.FindNext", "Find &Next", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindNext", "Find &Next", null, "Find Next (F3)", "F3", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FindPrevious":
                    Item(itemProps, "Menus and Toolbars.MenuItems.EditMenu.FindPrevious", "Find &Previous", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.FindPrevious", "Find &Previous", null, "Find previous (Shift+F3)", "Shift+F3", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.EditSourceRecord":
                    Item(itemProps, "Menus and Toolbars.MenuItems.EditMenu.EditSourceRecord", "&Edit Source Record", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.EditSourceRecord", "&Edit Source Record", null, "Edit source record (Shift+F2)", "Shift+F2", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ReloadProject":
                    Item(itemProps, "Menus and Toolbars.MenuItems.EditMenu.ReloadProject", "&Reload Project Data Sources", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditMenu.ReloadProject", "&Reload Project Data Sources", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.View":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.View", "&View", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.View", "&View", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.DataCorpus":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.DataCorpus", "&Data Corpus", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DataCorpus", "&Data Corpus", null, "Data Corpus View (Ctrl+Alt+D)", "Ctrl+Alt+D", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FindPhones":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.Search", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.Search", "&Search", null, "Search View (Ctrl+Alt+S)", "Ctrl+Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ConsonantChart":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.ConsonantChart", "&Consonant Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ConsonantChart", "&Consonant Chart", null, "Consonant Chart View (Ctrl+Alt+C)", "Ctrl+Alt+C", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.VowelChart":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.VowelChart", "&Vowel Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.VowelChart", "&Vowel Chart", null, "Vowel Chart View (Ctrl+Alt+V)", "Ctrl+Alt+V", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.XYChart":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts", "Distri&bution Charts", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts", "Distri&bution Charts", null, "Distribution Charts View (Ctrl+Alt+B)", "Ctrl+Alt+B", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ShowCIEResults":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.MinimalPairs", "&Minimal Pairs", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.MinimalPairs", "&Minimal Pairs", null, "Minimal Pairs (Ctrl+M)", "Ctrl+M", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ShowCIESimilarResults":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults", "Similar E&nvironments", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults", "Similar E&nvironments", null, "Similar Environments (Ctrl+Alt+N)", "Ctrl+Alt+N", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.GroupBySortedField":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.GroupByPrimarySortField", "&Group by Primary Sort Field", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.GroupByPrimarySortField", "&Group by Primary Sort Field", null, "Group by Primary Sort Field (Ctrl+G)", "Ctrl+G", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.CollapseAllGroups":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.CollapseAllGroups", "C&ollapse All Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.CollapseAllGroups", "C&ollapse All Groups", null, "Collapse All Groups (Ctrl+Up)", "Ctrl+Up", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ExpandAllGroups":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.ExpandAllGroups", "&Expand All Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ExpandAllGroups", "&Expand All Groups", null, "Expand All Groups (Ctrl+Down)", "Ctrl+Down", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ShowRecordPane":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane", "&Record View", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane", "&Record View", null, "Record View (F2)", "F2", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "MenuItems.ShowHtmlChart":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.ShowHtmlChart", "Toggle HTML Vie&w", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHtmlChart", "Toggle HTML Vie&w", null, "Toggle HTML View (F4)", "F4", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "MenuItems.ShowBackToEthnologue":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.ShowBackToEthnologue", "Back (from Ethnologue)", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowBackToEthnologue", "Back (from Ethnologue)", null, "Back (from Ethnologue)", null, ((System.ComponentModel.IComponent)(item)));
                    break;
                case "MenuItems.ShowHistogram":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ViewMenu.ShowHistogram", "&Histogram", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ViewMenu.ShowHistogram", "&Histogram", null, "Show Histogram (Alt+H)", "Alt+H", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "MenuItems.Tools":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.Tools", "&Tools", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Tools", "&Tools", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.FiltersOnViewMenu":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.FiltersOnMenu", "&Filters...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.FiltersOnMenu", "&Filters...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.ExperimentalTranscriptions":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.TranscriptionChanges", "&Transcription Changes...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.TranscriptionChanges", "&Transcription Changes...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.AmbiguousSequences":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.AmbiguousSequences", "&Ambiguous Sequences...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.AmbiguousSequences", "&Ambiguous Sequences...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.DescriptiveFeatures":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.DescriptiveFeatures", "D&escriptive Features...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DescriptiveFeatures", "D&escriptive Features...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.DistinctiveFeatures":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.DistinctiveFeatures", "D&istinctive Features...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.DistinctiveFeatures", "D&istinctive Features...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Classes":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.Classes", "&Classes...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Classes", "&Classes...", null, "Classes", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.UndefinedCharacters":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.UndefinedCharacters", "&Undefined Phonetic Characters...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.UndefinedCharacters", "&Undefined Phonetic Characters...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Options":
                    Item(itemProps, "Menus and Toolbars.MenuItems.ToolsMenu.Options", "&Options...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.ToolsMenu.Options", "&Options...", null, "Options", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Help":
                    Item(itemProps, "Menus and Toolbars.MenuItems.HelpMenu.Help", "&Help", "Text on main menu (i.e. menu across top of application window).");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Help", "&Help", "Text on main menu (i.e. menu across top of application window).", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.HelpPA":
                    Item(itemProps, "Menus and Toolbars.MenuItems.HelpMenu.PA", "Phonology Assistant...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.PA", "Phonology Assistant...", null, "Phonology Assistant Help (F1)", "F1", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.HelpTraining":
                    Item(itemProps, "Menus and Toolbars.MenuItems.HelpMenu.Training", "&Training", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.Training", "&Training", null, "Training", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.StudentManual":
                    Item(itemProps, "Menus and Toolbars.MenuItems.HelpMenu.TrainingStudentManual", "&Student Manual...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingStudentManual", "&Student Manual...", null, "Student Manual", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.InstructorGuide":
                    Item(itemProps, "Menus and Toolbars.MenuItems.HelpMenu.TrainingInstructorGuide", "&Instructor Guide...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.TrainingInstructorGuide", "&Instructor Guide...", null, "Instructor Guide", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.HelpAbout":
                    Item(itemProps, "Menus and Toolbars.MenuItems.HelpMenu.About", "&About Phonology Assistant...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpMenu.About", "&About Phonology Assistant...", null, "About Phonolgy Assistant...", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.UnDockView":
                    Item(itemProps, "Menus and Toolbars.MenuItems.MainMenuButtons.UnDockView", "Undock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.UnDockView", "Undock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.", "Undock View", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.DockView":
                    Item(itemProps, "Menus and Toolbars.MenuItems.MainMenuButtons.DockView", "Dock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.DockView", "Dock View", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.", "Dock View", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.OptionsMain":
                    Item(itemProps, "Menus and Toolbars.MenuItems.MainMenuButtons.OptionsMain", "&Options...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.OptionsMain", "&Options...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.", "Options", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.Filters":
                    Item(itemProps, "Menus and Toolbars.MenuItems.MainMenuButtons.Filters", "&Filters...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.Filters", "&Filters...", "Normally, the text of this is not displayed. The tool tip is, however. This is for the filters button on the far right of the main menu.", "Filters", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "MenuItems.NoFilter":
                    Item(itemProps, "Menus and Toolbars.MenuItems.MainMenuButtons.FiltersDropDown.NoFilter", "(&No Filter)", "Displayed on the drop-down of the filters button on the far right of the main menu");
                    //LocalizationManager.GetString("Menus and Toolbars.MenuItems.MainMenuButtons.FiltersDropDown.NoFilter", "(&No Filter)", "Displayed on the drop-down of the filters button on the far right of the main menu", null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.GroupBySortedField":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.GroupByPrimarySortField", "&Group by Primary Sort Field", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.GroupByPrimarySortField", "&Group by Primary Sort Field", null, "Group by Primary Sort Field (Ctrl+G)", "Ctrl+G", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.CollapseAllGroups":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.CollapseAllGroups", "C&ollapse All Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.CollapseAllGroups", "C&ollapse All Groups", null, "Collapse All Groups (Ctrl+Up)", "Ctrl+Up", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ExpandAllGroups":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ExpandAllGroups", "&Expand All Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ExpandAllGroups", "&Expand All Groups", null, "Expand All Groups (Ctrl+Down)", "Ctrl+Down", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowRecordPane":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ShowRecordPane", "&Record View", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowRecordPane", "&Record View", null, "Record View (F2)", "F2", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.Find":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.Find", "&Find", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Find", "&Find", null, "Find (Ctrl+F)", "Ctrl+F", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.FindNext":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.FindNext", "Find &Next", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.FindNext", "Find &Next", null, "Find Next (F3)", "F3", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.Playback":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.Playback", "&Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Playback", "&Playback", null, "Playback (F5)", "F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.PlaybackOnMenu":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.PlaybackOnMenu", "&Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackOnMenu", "&Playback", null, "Playback (F5)", "F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.PlaybackRepeatedly":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.PlaybackRepeatedly", "Playback &Repeatedly", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackRepeatedly", "Playback &Repeatedly", null, "Playback Repeatedly (Ctrl+F5)", "Ctrl+F5", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.StopPlayback":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.StopPlayback", "&Stop Playback", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.StopPlayback", "&Stop Playback", null, "Stop Playback (F8)", "F8", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AdjustPlaybackSpeedParent":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeedParent", "Playback &Speed", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeedParent", "Playback &Speed", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AdjustPlaybackSpeed":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeed", "Playback &Speed", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeed", "Playback &Speed", null, "Playback Speed", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.PhoneticSort":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.PhoneticSort", "Phonetic Sort Options", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PhoneticSort", "Phonetic Sort Options", null, "Phonetic Sort Options", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CutSavedPattern":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.CutSavedPattern", "Cu&t", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CutSavedPattern", "Cu&t", null, "Cut saved pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CopySavedPattern":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.CopySavedPattern", "&Copy", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopySavedPattern", "&Copy", null, "Copy Saved pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.PasteSavedPattern":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.PasteSavedPattern", "&Paste", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.PasteSavedPattern", "&Paste", null, "Paste Saved Pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList", "Show Results in &Active Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList", "Show Results in &Active Tab Group", null, "Show results in active tab group", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList", "Show Results in New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList", "Show Results in New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList", "Show Results in New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList", "Show Results in New Stac&ked Tab Group", null, "Show Results in New Stacked Tab Group", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CopyToCurrentPattern-FromSavedList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromSavedList", "C&opy Selected Pattern to Current Search Pattern", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromSavedList", "C&opy Selected Pattern to Current Search Pattern", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.RemovePattern-FromSavedList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.RemovePattern-FromSavedList", "&Remove", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromSavedList", "&Remove", null, "Remove Pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.MoveToNewSideBySideTabGroup":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.MoveToNewSideBySideTabGroup", "Move to New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewSideBySideTabGroup", "Move to New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.MoveToNewStackedTabGroup":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.MoveToNewStackedTabGroup", "Move to New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewStackedTabGroup", "Move to New Stac&ked Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CloseTab":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.CloseTab", "Close Tab", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTab", "Close Tab", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CloseTabGroup":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.CloseTabGroup", "Close Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTabGroup", "Close Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CloseAllTabGroups":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.CloseAllTabGroups", "Close All Tab Groups", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseAllTabGroups", "Close All Tab Groups", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList", "Show Results in &Active Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList", "Show Results in &Active Tab Group", null, "Show results in active tab group", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList", "Show Results in New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList", "Show Results in New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList", "Show Results in New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList", "Show Results in New Stac&ked Tab Group", null, "Show Results in New Stacked Tab Group", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CopyToCurrentPattern-FromRecentList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromRecentList", "C&opy Selected Pattern to Current Search Pattern", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromRecentList", "C&opy Selected Pattern to Current Search Pattern", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.RemovePattern-FromRecentList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.RemovePattern-FromRecentList", "&Remove", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromRecentList", "&Remove", null, "Remove Pattern", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ClearRecentPatternList-FromRecentList":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ClearRecentPatternList-FromRecentList", "Clear &List", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearRecentPatternList-FromRecentList", "Clear &List", null, "Clear List", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowResults":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ShowResults", "&Show Results", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowResults", "&Show Results", null, "Show Search Results (Alt+S)", "Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertIntoPattern":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertIntoPattern", "&Insert", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoPattern", "&Insert", null, "Insert Element into Current Search Pattern (Alt+I)", "Alt+I", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertConsonant":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertConsonant", "[C] Any &Consonant", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertConsonant", "[C] Any &Consonant", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertVowel":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertVowel", "[V] Any &Vowel", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertVowel", "[V] Any &Vowel", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertZeroOrMore":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertOneOrMore":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertWordBoundary":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertWordBoundary", "# Space/&Word Boundary", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordBoundary", "# Space/&Word Boundary", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertDiacriticPlaceholder":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null, null, "Ctrl+0", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertANDGroup":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertANDGroup", "[  ] A&ND Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertANDGroup", "[  ] A&ND Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertORGroup":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertORGroup", "{  } O&R Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertORGroup", "{  } O&R Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.InsertWordInitial":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertWordInitial", "/#_* Word Initial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordInitial", "/#_* Word Initial", null, null, "Ctrl+1", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.InsertWordFinal":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertWordFinal", "/*_# Word final", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordFinal", "/*_# Word final", null, null, "Ctrl+9", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.InsertAnywhere":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertAnywhere", "/*_* Anywhere", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertAnywhere", "/*_* Anywhere", null, null, "Ctrl+5", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.InsertWordMedial":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null, null, "Ctrl+2", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.SearchOptions":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.SearchOptions", "&Options", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SearchOptions", "&Options", null, "Search Options (Alt+O)", "Alt+O", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SavePattern":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.SavePattern", "Save", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePattern", "Save", null, "Save Search Pattern (Ctrl+S)", "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SavePatternOnMenu":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.SavePatternOnMenu", "&Save", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternOnMenu", "&Save", null, "Save Current Search Pattern (Ctrl+S)", "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SavePatternAs":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.SavePatternAs", "Save &As...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternAs", "Save &As...", null, "Save Current Search Pattern As...", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ClearPattern":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ClearPattern", "&Clear", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearPattern", "&Clear", null, "Clear Current Search Pattern and Results (Alt+C)", "Alt+C", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowCIEResults":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ShowCIEResults", "&Minimal Pairs", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIEResults", "&Minimal Pairs", null, "Minimal Pairs (Ctrl+M)", "Ctrl+M", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ToolbarItems.ShowCIESimilarResults":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ShowCIESimilarResults", "Similar E&nvironments", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIESimilarResults", "Similar E&nvironments", null, "Similar Environments (Ctrl+Alt+N)", "Ctrl+Alt+N", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.CharChartSearchContextMenu":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.CharChartSearchContextMenu", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CharChartSearchContextMenu", "&Search", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchAnywhere":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAnywhere", "Anywhere", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAnywhere", "Anywhere", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchInitial":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ChartPhoneSearchInitial", "Word Initially", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchInitial", "Word Initially", null, "Search for phone word initially", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchMedial":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ChartPhoneSearchMedial", "Word Medial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchMedial", "Word Medial", null, "Search for phone word medial", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchFinal":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ChartPhoneSearchFinal", "Word Finally", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchFinal", "Word Finally", null, "Search for phone word finally", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ChartPhoneSearchAlone":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAlone", "Alone", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAlone", "Alone", null, "Search for phone alone", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearch":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ChartPhoneSearch", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearch", "&Search", null, "Search for Phone (Alt+S)", "Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchAnywhere":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ChartPhoneSearchAnywhere", "Anywhere", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAnywhere", "Anywhere", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchInitial":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ChartPhoneSearchInitial", "Word Initially", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchInitial", "Word Initially", null, "Search for phone word initially", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchMedial":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ChartPhoneSearchMedial", "Word Medial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchMedial", "Word Medial", null, "Search for phone word medial", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchFinal":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ChartPhoneSearchFinal", "Word Finally", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchFinal", "Word Finally", null, "Search for phone word finally", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ChartPhoneSearchAlone":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ChartPhoneSearchAlone", "Alone", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAlone", "Alone", null, "Search for phone alone", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.IgnoredSymbols":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.IgnoredSymbols", "Ignore", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.IgnoredSymbols", "Ignore", null, "Select Ignored Symbols", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowHistogram":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ShowHistogram", "&Histogram", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHistogram", "&Histogram", null, "Show Histogram (Alt+H)", "Alt+H", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowHtmlChart":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ShowHtmlChart", "Toggle HTML Vie&w", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHtmlChart", "Toggle HTML Vie&w", null, "Toggle HTML View (F4)", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ShowBackToEthnologue":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ShowBackToEthnologue", "Back (from Ethnologue)", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowBackToEthnologue", "Back (from Ethnologue)", null, "Back (from Ethnologue)", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertIntoChart":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertIntoChart", "&Insert", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertIntoChart", "&Insert", null, "Insert Element into Current Chart Cell (Alt+I)", "Alt+I", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertConsonant":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertConsonant", "[C] Any &Consonant", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertConsonant", "[C] Any &Consonant", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertVowel":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertVowel", "[V] Any &Vowel", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertVowel", "[V] Any &Vowel", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertZeroOrMore":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertZeroOrMore", "* &Zero or More Phones or Diacritics", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertOneOrMore":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertOneOrMore", "+ &One or More Phones or Diacritics", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertWordBoundary":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertWordBoundary", "# Space/&Word Boundary", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordBoundary", "# Space/&Word Boundary", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertDiacriticPlaceholder":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertDiacriticPlaceholder", "&Diacritic Placeholder", null, null, "Ctrl+0", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertWordInitial":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertWordInitial", "/#_* Word Initial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordInitial", "/#_* Word Initial", null, null, "Ctrl+1", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertWordFinal":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertWordFinal", "/*_# Word final", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordFinal", "/*_# Word final", null, null, "Ctrl+9", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertAnywhere":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertAnywhere", "/*_* Anywhere", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertAnywhere", "/*_* Anywhere", null, null, "Ctrl+5", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertWordMedial":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordMedial", "/{[C],[V]}_{[C],[V]} Word Medial", null, null, "Ctrl+2", ((System.ComponentModel.IComponent)(item)));
                    break;
                case "ContextMenuItems.InsertANDGroup":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertANDGroup", "[  ] A&ND Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertANDGroup", "[  ] A&ND Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.InsertORGroup":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.InsertORGroup", "{  } O&R Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertORGroup", "{  } O&R Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.SearchOptions":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.SearchOptions", "Search &Options", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SearchOptions", "Search &Options", null, "Search Options for Current Chart Column (Alt+O)", "Alt+O", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.RunChartSearch":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.RunChartSearch", "Fi&ll Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RunChartSearch", "Fi&ll Chart", null, "Fill Chart with Results (Alt+L)", "Alt+L", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.BeginSearch":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.BeginSearch", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.BeginSearch", "&Search", null, "Search (Alt+S)", "Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.AddResultsToCurrentTabGroup":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.AddResultsToNewSideBySideTabGroup":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.AddResultsToNewStackedTabGroup":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.SaveChart":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.SaveChart", "S&ave", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SaveChart", "S&ave", null, "Save Current Chart (Ctrl+S)", "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.ClearChart":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.ClearChart", "&Clear", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearChart", "&Clear", null, "Clear Chart (Alt+C)", "Alt+C", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.DeleteChartRow":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.DeleteChartRow", "Delete &Row", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartRow", "Delete &Row", null, "Delete Row (Alt+R)", "Alt+R", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ContextMenuItems.DeleteChartColumn":
                    Item(itemProps, "Menus and Toolbars.ContextMenuItems.DeleteChartColumn", "Delete C&olumn", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.DeleteChartColumn", "Delete C&olumn", null, "Delete Column (Alt+O)", "Alt+O", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.BeginSearch":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.BeginSearch", "&Search", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.BeginSearch", "&Search", null, "Search (Alt+S)", "Alt+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AddResultsToCurrentTabGroup":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToCurrentTabGroup", "Show Results in &Active Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AddResultsToNewSideBySideTabGroup":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewSideBySideTabGroup", "Show Results in New Side-&By-Side Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.AddResultsToNewStackedTabGroup":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewStackedTabGroup", "Show Results in New Stac&ked Tab Group", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.RunChartSearch":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.RunChartSearch", "Fi&ll Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.RunChartSearch", "Fi&ll Chart", null, "Fill Chart with Results (Alt+L)", "Alt+L", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.InsertIntoChart":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.InsertIntoChart", "&Insert", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoChart", "&Insert", null, "Insert Element into Current Chart Cell (Alt+I)", "Alt+I", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SaveChart":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.SaveChart", "S&ave", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChart", "S&ave", null, "Save Current Chart (Ctrl+S)", "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SaveChartOnMenu":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.SaveChartOnMenu", "&Save", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartOnMenu", "&Save", null, null, "Ctrl+S", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.SaveChartAs":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.SaveChartAs", "Save &As...", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartAs", "Save &As...", null, null, null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ResetChart":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ResetChart", "Reset Chart", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ResetChart", "Reset Chart", null, "Reset Chart", null, ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.ClearChart":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.ClearChart", "&Clear", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearChart", "&Clear", null, "Clear Chart (Alt+C)", "Alt+C", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.DeleteChartRow":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.DeleteChartRow", "Delete &Row", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartRow", "Delete &Row", null, "Delete Row (Alt+R)", "Alt+R", ((System.ComponentModel.IComponent)(item)));
                    break;

                case "ToolbarItems.DeleteChartColumn":
                    Item(itemProps, "Menus and Toolbars.ToolbarItems.DeleteChartColumn", "Delete C&olumn", null);
                    //LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.DeleteChartColumn", "Delete C&olumn", null, "Delete Column (Alt+O)", "Alt+O", ((System.ComponentModel.IComponent)(item)));
                    break;
            }
        }

        private static void Item(TMItemProperties itemProps, string id, string englishText, string comment)
        {
            itemProps.Text = LocalizationManager.GetString(id, englishText, comment);
        }
    }
}