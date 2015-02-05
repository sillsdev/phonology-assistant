<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
	# Name:        MigratePaUI0350.xsl
	# Purpose:     Updates to UI for 3.5.1
	#
	# Author:      Greg Trihus <greg_trihus@sil.org>
	#
	# Created:     2015/2/5
	# Copyright:   (c) 2015 SIL International
	# Licence:     <MIT>
	################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0">

	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:strip-space elements="*"/>

  <xsl:template match="node()|@*">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='MainWindow.ViewTabs.DistributionChartsTab_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Distribution Charts View (Ctrl+Alt+B)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.BeginSearch_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Search (Alt+S)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.ClearChart_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Clear Chart (Alt+C)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.DeleteChartColumn_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Delete Column (Alt+O)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.DeleteChartRow_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Delete Row (Alt+R)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.EditSourceRecord_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Edit source record (Shift+F2)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.InsertIntoChart_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Insert Element into Current Chart Cell (Alt+I)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.Playback_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Playback (F5)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.SaveChart_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Save Current Chart (Ctrl+S)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.SearchOptions_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Search Options for Current Chart Column (Alt+O)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.StopPlayback_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Stop Playback (F8)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.EditMenu.EditSourceRecord_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Edit source record (Shift+F2)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.EditMenu.Find_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Find (Ctrl+F)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.EditMenu.FindNext_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Find Next (F3)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.EditMenu.FindPrevious_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Find previous (Shift+F3)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.FileMenu.New_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Create New Project (Ctrl+N)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.FileMenu.Open_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Open Project (Ctrl+O)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.FileMenu.Playback_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Playback (F5)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.FileMenu.PlaybackRepeatedly_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Playback Repeatedly (Ctrl+F5)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.FileMenu.StopPlayback_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Stop Playback (F8)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.HelpMenu.PA_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Phonology Assistant Help (F1)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults_ShortcutKeys_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Ctrl+Alt+N</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Similar Environments (Ctrl+Alt+N)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.CIESimilarResults']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Similar E&amp;nvironments</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.CollapseAllGroups_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Collapse All Groups (Ctrl+Up)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts_ShortcutKeys_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Ctrl+Alt+B</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Distribution Charts View (Ctrl+Alt+B)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.DistributionCharts']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Distri&amp;bution Charts</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.ExpandAllGroups_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Expand All Groups (Ctrl+Down)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.GroupByPrimarySortField_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Group by Primary Sort Field (Ctrl+G)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.MinimalPairs_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Minimal Pairs (Ctrl+M)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.Search']">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
		<xsl:if test="not(following-sibling::*[1]/@tuid = 'Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane_ShortcutKeys_')">
			<tu tuid="Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane_ShortcutKeys_">
				<tuv xml:lang="en">
					<seg>F2</seg>
				</tuv>
			</tu>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.ViewMenu.ShowRecordPane_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Record View (F2)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.BeginSearch_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Search (Alt+S)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ClearPattern_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Clear Current Search Pattern and Results (Alt+C)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.CollapseAllGroups_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Collapse All Groups (Ctrl+Up)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ExpandAllGroups_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Expand All Groups (Ctrl+Down)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.Find_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Find (Ctrl+F)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.FindNext_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Find Next (F3)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.GroupByPrimarySortField_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Group by Primary Sort Field (Ctrl+G)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.InsertIntoChart_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Insert Element into Current Chart Cell (Alt+I)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.InsertIntoPattern_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Insert Element into Current Search Pattern (Alt+I)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.Playback_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Playback (F5)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.PlaybackOnMenu_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Playback (F5)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.PlaybackRepeatedly_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Playback Repeatedly (Ctrl+F5)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.PlaybackRepeatedly']">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
		<xsl:if test="not(following-sibling::*[1]/@tuid = 'Menus and Toolbars.ToolbarItems.ResetChart_ToolTip_')">
			<tu tuid="Menus and Toolbars.ToolbarItems.ResetChart_ToolTip_">
				<tuv xml:lang="en">
					<seg>Reset Chart</seg>
				</tuv>
			</tu>
			<tu tuid="Menus and Toolbars.ToolbarItems.ResetChart">
				<tuv xml:lang="en">
					<seg>Reset Chart</seg>
				</tuv>
			</tu>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.RunChartSearch_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Fill Chart with Results (Alt+L)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.SaveChart_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Save Current Chart (Ctrl+S)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.SavePatternOnMenu_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Save Current Search Pattern (Ctrl+S)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ShowCIEResults_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Minimal Pairs (Ctrl+M)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ShowCIESimilarResults_ShortcutKeys_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Ctrl+Alt+N</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ShowCIESimilarResults_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Similar Environments (Ctrl+Alt+N)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ShowCIESimilarResults']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Similar E&amp;nvironments</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ShowHtmlChart']">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
		<xsl:if test="not(following-sibling::*[1]/@tuid = 'Menus and Toolbars.ToolbarItems.ShowRecordPane_ShortcutKeys_')">
			<tu tuid="Menus and Toolbars.ToolbarItems.ShowRecordPane_ShortcutKeys_">
				<tuv xml:lang="en">
					<seg>F2</seg>
				</tuv>
			</tu>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ShowRecordPane_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Record View (F2)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.ShowResults_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Show Search Results (Alt+S)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ToolbarItems.StopPlayback_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Stop Playback (F8)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Views.DistributionChart.ChartNameLabel']">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
		<xsl:if test="not(following-sibling::*[1]/@tuid = 'Views.DistributionChart.ConfirmResetChartMsg')">
			<tu tuid="Views.DistributionChart.ConfirmResetChartMsg">
				<tuv xml:lang="en">
					<seg>Are you sure you want to reset the charts?</seg>
				</tuv>
			</tu>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*[@tuid='Views.WordLists.SearchResults.SimilarEnvironmentsButtonToolTipText']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Similar Environments Options (Ctrl+Alt+N)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='Menus and Toolbars.ContextMenuItems.RunChartSearch_ToolTip_']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<tuv xml:lang="en">
				<seg>Fill Chart with Results (Alt+L)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>
