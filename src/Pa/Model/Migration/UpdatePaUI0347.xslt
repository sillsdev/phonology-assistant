<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
	# Name:        MigratePaUI0347.xsl
	# Purpose:     Remove 2003 from Word 2003 XML option
	#
	# Author:      Greg Trihus <greg_trihus@sil.org>
	#
	# Created:     2014/12/10
	# Copyright:   (c) 2014 SIL International
	# Licence:     <MIT>
	################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0">
	
	<xsl:output method="xml"/>
	
	<xsl:template match="node()|@*">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="*[@tuid='Menus and Toolbars.MenuItems.FileMenu.ExportWordXml']//seg">
		<xsl:choose>
			<xsl:when test="contains(text(), '2003')">
				<xsl:copy>
					<xsl:text>&amp;Word XML...</xsl:text>
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="*[@tuid='Miscellaneous.FileTypes.Word2003XmlFileType']//seg">
		<xsl:choose>
			<xsl:when test="contains(text(), '2003')">
				<xsl:copy>
					<xsl:text>Word XML Files (*.xml)|*.xml</xsl:text>
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="*[@tuid='MainWindow.ViewTabs.DistributionChartTab']">
		<xsl:copy>
			<xsl:attribute name="tuid">MainWindow.ViewTabs.DistributionChartsTab</xsl:attribute>
			<tuv xml:lang="en">
				<seg>Distribution Charts</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*[@tuid='MainWindow.ViewTabs.DistributionChartTab_ToolTip_']">
		<xsl:copy>
			<xsl:attribute name="tuid">MainWindow.ViewTabs.DistributionChartsTab_ToolTip_</xsl:attribute>
			<tuv xml:lang="en">
				<seg>Distribution Charts View (Ctrl+Alt+B)</seg>
			</tuv>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>