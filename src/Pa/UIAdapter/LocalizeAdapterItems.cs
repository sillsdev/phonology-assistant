using Localization;
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
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.Playback",
						"&Playback", null, "Playback", "F5", item);
					break;

				case "ContextMenuItems.StopPlayback":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.StopPlayback",
						"&Stop Playback", null, "Stop Playback", "F8", item);
					break;

				case "ContextMenuItems.EditSourceRecord":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.EditSourceRecord",
						"&Edit Source Record", null, "Edit source record", "Shift+F2", item);
					break;

				case "MenuItems.File":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.File",
					"&File", null, null, null, item);
					break;

				case "MenuItems.FileNew":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileNew",
						"&New Project...", null, "Create New Project", "Ctrl+N", item);
					break;

				case "MenuItems.FileOpen":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileOpen",
						"&Open Project...", null, "Open Project", "Ctrl+O", item);
					break;

				case "MenuItems.FileClose":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileClose",
						"&Close Project", null, null, null, item);
					break;

				case "MenuItems.ProjectSettings":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.ProjectSettings",
						"P&roject Settings...", null, null, "Ctrl+R", item);
					break;

				case "MenuItems.BackupRestore":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.BackupRestore",
						"&Backup && Restore", null, null, null, item);
					break;

				case "MenuItems.FileBackup":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileBackup",
						"&Backup...", null, null, null, item);
					break;

				case "MenuItems.FileRestore":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileRestore",
						"&Restore...", null, null, null, item);
					break;

				case "MenuItems.FileExportAs":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileExportAs",
						"&Export", null, null, null, item);
					break;

				case "MenuItems.FileExportHTML":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileExportHTML",
						"&HTML...", null, "Export to HTML File", null, item);
					break;

				case "MenuItems.FileExportWordXml":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileExportWordXml",
						"&Word 2003 XML...", null, null, null, item);
					break;

				case "MenuItems.FileExportRTF":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileExportRTF",
						"&Rich Text Format (RTF)...", null, "Export to RTF File", null, item);
					break;

				case "MenuItems.FileExportXLingPaper":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileExportXLingPaper",
						"X&LingPaper...", null, "Export to XLingPaper File", null, item);
					break;

				case "MenuItems.ExportAsPAXML":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.ExportAsPAXML",
						"{0} &XML...", null, null, null, item);
					break;

				case "MenuItems.Playback":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.Playback",
						"&Playback", null, "Playback", "F5", item);
					break;

				case "MenuItems.PlaybackRepeatedly":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.PlaybackRepeatedly",
						"Playback &Repeatedly", null, "Playback Repeatedly", "Ctrl+F5", item);
					break;

				case "MenuItems.StopPlayback":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.StopPlayback",
						"&Stop Playback", null, "Stop Playback", "F8", item);
					break;

				case "MenuItems.FileExit":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FileExit",
						"E&xit", null, "Exit Phonology Assistant", null, item);
					break;

				case "MenuItems.Edit":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.Edit",
						"&Edit", null, null, null, item);
					break;

				case "MenuItems.Find":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.Find",
						"&Find", null, "Find", "Ctrl+F", item);
					break;

				case "MenuItems.FindNext":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FindNext",
						"Find &Next", null, "Find Next", "F3", item);
					break;

				case "MenuItems.FindPrevious":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FindPrevious",
						"Find &Previous", null, "Find previous", "Shift+F3", item);
					break;

				case "MenuItems.EditSourceRecord":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.EditSourceRecord",
						"&Edit Source Record", null, "Edit source record", "Shift+F2", item);
					break;

				case "MenuItems.ReloadProject":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.ReloadProject",
						"&Reload Project Data Sources", null, null, null, item);
					break;

				case "MenuItems.View":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.View",
						"&View", null, null, null, item);
					break;

				case "MenuItems.DataCorpus":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.DataCorpus",
						"&Data Corpus", null, "Data Corpus View (Ctrl+Alt+D)", "Ctrl+Alt+D", item);
					break;

				case "MenuItems.FindPhones":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FindPhones",
						"&Search", null, "Search View (Ctrl+Alt+S)", "Ctrl+Alt+S", item);
					break;

				case "MenuItems.ConsonantChart":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.ConsonantChart",
						"&Consonant Chart", null, "Consonant Chart View (Ctrl+Alt+C)", "Ctrl+Alt+C", item);
					break;

				case "MenuItems.VowelChart":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.VowelChart",
						"&Vowel Chart", null, "Vowel Chart View (Ctrl+Alt+V)", "Ctrl+Alt+V", item);
					break;

				case "MenuItems.XYChart":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.XYChart",
						"&Distribution Charts", null, "Distribution Charts View (Ctrl+Alt+X)", "Ctrl+Alt+X", item);
					break;

				case "MenuItems.ShowCIEResults":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.ShowCIEResults",
						"&Minimal Pairs", null, "Minimal Pairs", "Ctrl+M", item);
					break;

				case "MenuItems.GroupBySortedField":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.GroupBySortedField",
						"&Group by Primary Sort Field", null, "Group by Primary Sort Field", "Ctrl+G", item);
					break;

				case "MenuItems.CollapseAllGroups":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.CollapseAllGroups",
						"C&ollapse All Groups", null, "Collapse All Groups", "Ctrl+Up", item);
					break;

				case "MenuItems.ExpandAllGroups":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.ExpandAllGroups",
						"&Expand All Groups", null, "Expand All Groups", "Ctrl+Down", item);
					break;

				case "MenuItems.ShowRecordPane":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.ShowRecordPane",
						"&Record View", null, "Record View", null, item);
					break;

				case "MenuItems.Tools":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.Tools",
						"&Tools", null, null, null, item);
					break;

				case "MenuItems.FiltersOnViewMenu":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.FiltersOnViewMenu",
						"&Filters...", null, null, null, item);
					break;

				case "MenuItems.ExperimentalTranscriptions":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.ExperimentalTranscriptions",
						"&Transcription Changes...", null, null, null, item);
					break;

				case "MenuItems.AmbiguousSequences":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.AmbiguousSequences",
						"&Ambiguous Sequences...", null, null, null, item);
					break;

				case "MenuItems.DescriptiveFeatures":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.DescriptiveFeatures",
						"D&escriptive Features...", null, null, null, item);
					break;

				case "MenuItems.DistinctiveFeatures":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.DistinctiveFeatures",
						"D&istinctive Features...", null, null, null, item);
					break;

				case "MenuItems.Classes":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.Classes",
					"&Classes...", null, "Classes", null, item);
					break;

				case "MenuItems.UndefinedCharacters":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.UndefinedCharacters",
						"&Undefined Phonetic Characters...", null, null, null, item);
					break;

				case "MenuItems.Options":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.Options",
						"&Options...", null, "Options", null, item);
					break;

				case "MenuItems.Help":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.Help",
						"&Help", null, null, null, item);
					break;

				case "MenuItems.HelpPA":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpPA",
						"Phonology Assistant...", null, "Phonology Assistant Help", "F1", item);
					break;

				case "MenuItems.HelpTraining":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpTraining",
						"&Training", null, "Training", null, item);
					break;

				case "MenuItems.StudentManual":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.StudentManual",
						"&Student Manual...", null, "Student Manual", null, item);
					break;

				case "MenuItems.InstructorGuide":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.InstructorGuide",
						"&Instructor Guide...", null, "Instructor Guide", null, item);
					break;

				case "MenuItems.HelpAbout":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.HelpAbout",
						"&About Phonology Assistant...", null, "About Phonolgy Assistant...", null, item);
					break;

				case "MenuItems.UnDockView":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.UnDockView",
						"Undock View", null, "Undock View", null, item);
					break;

				case "MenuItems.DockView":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.DockView",
						"Dock View", null, "Dock View", null, item);
					break;

				case "MenuItems.OptionsMain":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.OptionsMain",
						"&Options...", null, "Options", null, item);
					break;

				case "MenuItems.NoFilter":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.NoFilter",
						"(&No Filter)", null, null, null, item);
					break;

				case "MenuItems.Filters":
					LocalizationManager.GetString("Menus and Toolbars.MenuItems.Filters",
						"&Filters...", null, null, null, item);
					break;

				case "ToolbarItems.GroupBySortedField":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.GroupBySortedField",
						"&Group by Primary Sort Field", null, "Group by Primary Sort Field", "Ctrl+G", item);
					break;

				case "ToolbarItems.CollapseAllGroups":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.CollapseAllGroups",
						"C&ollapse All Groups", null, "Collapse All Groups", "Ctrl+Up", item);
					break;

				case "ToolbarItems.ExpandAllGroups":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ExpandAllGroups",
						"&Expand All Groups", null, "Expand All Groups", "Ctrl+Down", item);
					break;

				case "ToolbarItems.ShowRecordPane":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowRecordPane",
						"&Record View", null, "Record View", null, item);
					break;

				case "ToolbarItems.Find":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Find",
						"&Find", null, "Find", "Ctrl+F", item);
					break;

				case "ToolbarItems.FindNext":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.FindNext",
						"Find &Next", null, "Find Next", "F3", item);
					break;

				case "ToolbarItems.Playback":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.Playback",
						"&Playback", null, "Playback", "F5", item);
					break;

				case "ToolbarItems.PlaybackOnMenu":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackOnMenu",
						"&Playback", null, "Playback", "F5", item);
					break;

				case "ToolbarItems.PlaybackRepeatedly":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PlaybackRepeatedly",
						"Playback &Repeatedly", null, "Playback Repeatedly", "Ctrl+F5", item);
					break;

				case "ToolbarItems.StopPlayback":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.StopPlayback",
						"&Stop Playback", null, "Stop Playback", "F8", item);
					break;

				case "ToolbarItems.AdjustPlaybackSpeedParent":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeedParent",
						"Playback &Speed", null, null, null, item);
					break;

				case "ToolbarItems.AdjustPlaybackSpeed":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AdjustPlaybackSpeed",
						"Playback &Speed", null, "Playback Speed", null, item);
					break;

				case "ToolbarItems.PhoneticSort":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.PhoneticSort",
						"Phonetic Sort Options", null, "Phonetic Sort Options", null, item);
					break;

				case "ContextMenuItems.CutSavedPattern":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CutSavedPattern",
						"Cu&t", null, "Cut saved pattern", null, item);
					break;

				case "ContextMenuItems.CopySavedPattern":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopySavedPattern",
						"&Copy", null, "Copy Saved pattern", null, item);
					break;

				case "ContextMenuItems.PasteSavedPattern":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.PasteSavedPattern",
						"&Paste", null, "Paste Saved Pattern", null, item);
					break;

				case "ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromSavedList",
						"Show Results in &Active Tab Group", null, "Show results in active tab group", null, item);
					break;

				case "ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromSavedList",
						"Show Results in New Side-&By-Side Tab Group", null, null, null, item);
					break;

				case "ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromSavedList",
						"Show Results in New Stac&ked Tab Group", null, "Show Results in New Stacked Tab Group", null, item);
					break;

				case "ContextMenuItems.CopyToCurrentPattern-FromSavedList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromSavedList",
						"C&opy Selected Pattern to Current Search Pattern", null, null, null, item);
					break;

				case "ContextMenuItems.RemovePattern-FromSavedList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromSavedList",
						"&Remove", null, "Remove Pattern", null, item);
					break;

				case "ContextMenuItems.MoveToNewSideBySideTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewSideBySideTabGroup",
						"Move to New Side-&By-Side Tab Group", null, null, null, item);
					break;

				case "ContextMenuItems.MoveToNewStackedTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.MoveToNewStackedTabGroup",
						"Move to New Stac&ked Tab Group", null, null, null, item);
					break;

				case "ContextMenuItems.CloseTab":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTab",
						"Close Tab", null, null, null, item);
					break;

				case "ContextMenuItems.CloseTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseTabGroup",
						"Close Tab Group", null, null, null, item);
					break;

				case "ContextMenuItems.CloseAllTabGroups":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CloseAllTabGroups",
						"Close All Tab Groups", null, null, null, item);
					break;

				case "ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInCurrentTabGroup-FromRecentList",
						"Show Results in &Active Tab Group", null, "Show results in active tab group", null, item);
					break;

				case "ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewSideBySideTabGroup-FromRecentList",
						"Show Results in New Side-&By-Side Tab Group", null, null, null, item);
					break;

				case "ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ShowResultsInNewStackedTabGroup-FromRecentList",
						"Show Results in New Stac&ked Tab Group", null, "Show Results in New Stacked Tab Group", null, item);
					break;

				case "ContextMenuItems.CopyToCurrentPattern-FromRecentList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CopyToCurrentPattern-FromRecentList",
						"C&opy Selected Pattern to Current Search Pattern", null, null, null, item);
					break;

				case "ContextMenuItems.RemovePattern-FromRecentList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RemovePattern-FromRecentList",
						"&Remove", null, "Remove Pattern", null, item);
					break;

				case "ContextMenuItems.ClearRecentPatternList-FromRecentList":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearRecentPatternList-FromRecentList",
						"Clear &List", null, "Clear List", null, item);
					break;

				case "ToolbarItems.ShowResults":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowResults",
						"&Show Results", null, "Show Search Results", "Alt+S", item);
					break;

				case "ToolbarItems.InsertIntoPattern":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoPattern",
						"&Insert", null, "Insert Element into Current Search Pattern", "Alt+I", item);
					break;

				case "ToolbarItems.InsertConsonant":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertConsonant",
						"[C] Any &Consonant", null, null, null, item);
					break;

				case "ToolbarItems.InsertVowel":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertVowel",
						"[V] Any &Vowel", null, null, null, item);
					break;

				case "ToolbarItems.InsertZeroOrMore":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertZeroOrMore",
						"* &Zero or More Phones or Diacritics", null, null, null, item);
					break;

				case "ToolbarItems.InsertOneOrMore":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertOneOrMore",
						"+ &One or More Phones or Diacritics", null, null, null, item);
					break;

				case "ToolbarItems.InsertWordBoundary":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertWordBoundary",
						"# Space/&Word Boundary", null, null, null, item);
					break;

				case "ToolbarItems.InsertDiacriticPlaceholder":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertDiacriticPlaceholder",
						"&Diacritic Placeholder", null, null, "Ctrl+0", item);
					break;

				case "ToolbarItems.InsertANDGroup":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertANDGroup",
						"[  ] A&ND Group", null, null, null, item);
					break;

				case "ToolbarItems.InsertORGroup":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertORGroup",
						"{  } O&R Group", null, null, null, item);
					break;

				case "ToolbarItems.SearchOptions":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SearchOptions",
						"&Options", null, "Search Options", "Alt+O", item);
					break;

				case "ToolbarItems.SavePattern":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePattern",
						"Save", null, "Save Search Pattern", null, item);
					break;

				case "ToolbarItems.SavePatternOnMenu":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternOnMenu",
						"&Save", null, "Save Current Search Pattern", "Ctrl+S", item);
					break;

				case "ToolbarItems.SavePatternAs":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SavePatternAs",
						"Save &As...", null, "Save Current Search Pattern As...", null, item);
					break;

				case "ToolbarItems.ClearPattern":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearPattern",
						"&Clear", null, "Clear Current Search Pattern and Results", "Alt+C", item);
					break;

				case "ToolbarItems.ShowCIEResults":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowCIEResults",
						"&Minimal Pairs", null, "Minimal Pairs", "Ctrl+M", item);
					break;

				case "ContextMenuItems.CharChartSearchContextMenu":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.CharChartSearchContextMenu",
						"&Search", null, null, null, item);
					break;

				case "ContextMenuItems.ChartPhoneSearchAnywhere":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAnywhere",
						"&Anywhere", null, null, null, item);
					break;

				case "ContextMenuItems.ChartPhoneSearchInitial":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchInitial",
						"Word &Initially", null, "Search for phone word initially", null, item);
					break;

				case "ContextMenuItems.ChartPhoneSearchMedial":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchMedial",
						"Word &Medial", null, "Search for phone word medial", null, item);
					break;

				case "ContextMenuItems.ChartPhoneSearchFinal":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchFinal",
						"Word &Finally", null, "Search for phone word finally", null, item);
					break;

				case "ContextMenuItems.ChartPhoneSearchAlone":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ChartPhoneSearchAlone",
						"Al&one", null, "Search for phone alone", null, item);
					break;

				case "ToolbarItems.ChartPhoneSearch":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearch",
						"&Search", null, "Search for Phone", null, item);
					break;

				case "ToolbarItems.ChartPhoneSearchAnywhere":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAnywhere",
						"&Anywhere", null, null, null, item);
					break;

				case "ToolbarItems.ChartPhoneSearchInitial":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchInitial",
						"Word &Initially", null, "Search for phone word initially", null, item);
					break;

				case "ToolbarItems.ChartPhoneSearchMedial":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchMedial",
						"Word &Medial", null, "Search for phone word medial", null, item);
					break;

				case "ToolbarItems.ChartPhoneSearchFinal":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchFinal",
						"Word &Finally", null, "Search for phone word finally", null, item);
					break;

				case "ToolbarItems.ChartPhoneSearchAlone":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ChartPhoneSearchAlone",
						"Al&one", null, "Search for phone alone", null, item);
					break;

				case "ToolbarItems.IgnoredSymbols":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.IgnoredSymbols",
						"Ignore", null, "Select Ignored Symbols", null, item);
					break;

				case "ToolbarItems.ShowHistogram":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHistogram",
						"&Histogram", null, "Show Histogram", null, item);
					break;

				case "ToolbarItems.ShowHtmlChart":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ShowHtmlChart",
						"HTML Chart", null, "Show HTML Chart", null, item);
					break;

				case "ContextMenuItems.InsertIntoChart":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertIntoChart",
						"&Insert", null, "Insert Element into Current Chart Cell", "Alt+I", item);
					break;

				case "ContextMenuItems.InsertConsonant":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertConsonant",
						"[C] Any &Consonant", null, null, null, item);
					break;

				case "ContextMenuItems.InsertVowel":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertVowel",
						"[V] Any &Vowel", null, null, null, item);
					break;

				case "ContextMenuItems.InsertZeroOrMore":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertZeroOrMore",
						"* &Zero or More Phones or Diacritics", null, null, null, item);
					break;

				case "ContextMenuItems.InsertOneOrMore":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertOneOrMore",
						"+ &One or More Phones or Diacritics", null, null, null, item);
					break;

				case "ContextMenuItems.InsertWordBoundary":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertWordBoundary",
						"# Space/&Word Boundary", null, null, null, item);
					break;

				case "ContextMenuItems.InsertDiacriticPlaceholder":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertDiacriticPlaceholder",
						"&Diacritic Placeholder", null, null, "Ctrl+0", item);
					break;

				case "ContextMenuItems.InsertANDGroup":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertANDGroup",
						"[  ] A&ND Group", null, null, null, item);
					break;

				case "ContextMenuItems.InsertORGroup":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.InsertORGroup",
						"{  } O&R Group", null, null, null, item);
					break;

				case "ContextMenuItems.SearchOptions":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SearchOptions",
						"&Options", null, "Search Options for Current Chart Column", "Alt+O", item);
					break;

				case "ContextMenuItems.RunChartSearch":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.RunChartSearch",
						"Fi&ll Chart", null, "Fill Chart with Results", "Alt+L", item);
					break;

				case "ContextMenuItems.BeginSearch":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.BeginSearch",
						"&Search", null, "Search", "Alt+S", item);
					break;

				case "ContextMenuItems.AddResultsToCurrentTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToCurrentTabGroup",
						"Show Results in &Active Tab Group", null, null, null, item);
					break;

				case "ContextMenuItems.AddResultsToNewSideBySideTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewSideBySideTabGroup",
						"Show Results in New Side-&By-Side Tab Group", null, null, null, item);
					break;

				case "ContextMenuItems.AddResultsToNewStackedTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.AddResultsToNewStackedTabGroup",
						"Show Results in New Stac&ked Tab Group", null, null, null, item);
					break;

				case "ContextMenuItems.SaveChart":
					LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.SaveChart",
						"S&ave", null, "Save Current Chart", "Ctrl+S", item);
					break;

				case "ContextMenuItems.ClearChart":
						LocalizationManager.GetString("Menus and Toolbars.ContextMenuItems.ClearChart",
					"&Clear", null, "Clear Chart", "Alt+C", item);
					break;

				case "ToolbarItems.BeginSearch":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.BeginSearch",
						"&Search", null, "Search", "Alt+S", item);
					break;

				case "ToolbarItems.AddResultsToCurrentTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToCurrentTabGroup",
						"Show Results in &Active Tab Group", null, null, null, item);
					break;

				case "ToolbarItems.AddResultsToNewSideBySideTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewSideBySideTabGroup",
						"Show Results in New Side-&By-Side Tab Group", null, null, null, item);
					break;

				case "ToolbarItems.AddResultsToNewStackedTabGroup":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.AddResultsToNewStackedTabGroup",
						"Show Results in New Stac&ked Tab Group", null, null, null, item);
					break;

				case "ToolbarItems.RunChartSearch":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.RunChartSearch",
						"Fi&ll Chart", null, "Fill Chart with Results", "Alt+L", item);
					break;

				case "ToolbarItems.InsertIntoChart":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.InsertIntoChart",
						"&Insert", null, "Insert Element into Current Chart Cell", "Alt+I", item);
					break;

				case "ToolbarItems.SaveChart":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChart",
						"S&ave", null, "Save Current Chart", "Ctrl+S", item);
					break;

				case "ToolbarItems.SaveChartOnMenu":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartOnMenu",
						"&Save", null, null, "Ctrl+S", item);
					break;

				case "ToolbarItems.SaveChartAs":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.SaveChartAs",
						"Save &As...", null, null, null, item);
					break;

				case "ToolbarItems.ClearChart":
					LocalizationManager.GetString("Menus and Toolbars.ToolbarItems.ClearChart",
						"&Clear", null, "Clear Chart", "Alt+C", item);
					break;
			}		
		}
	}
}
