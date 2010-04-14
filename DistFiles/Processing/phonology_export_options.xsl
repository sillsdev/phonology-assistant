<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>
	<!-- phonology_export_options.xsl 2010-04-02 -->
	<!-- Insert options for other XSL Transformations in the pipeline. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<!-- Help Converter provides the following options. -->
	<xsl:param name="optionsFile" select="''" />
	<xsl:param name="format" select="'XHTML'" />
	<xsl:param name="file-name-without-extension" select="''" />

	<!-- Assume that the program has inserted a metadata div with a settings list. -->
	<xsl:variable name="metadata" select="/xhtml:html/xhtml:body/xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="userFolder" select="$settings/xhtml:li[@class = 'userFolder']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Insert additional options. -->
	<xsl:template match="/xhtml:html/xhtml:body/xhtml:div[@id = 'metadata']/*[@class = 'options']">
		<ul class="options" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:if test="string-length($format) != 0 and not($metadata//xhtml:li[@class = 'format'])">
				<li class="format">
					<xsl:value-of select="$format" />
				</li>
			</xsl:if>
			<xsl:if test="$format = 'Word 2003 XML' and string-length($file-name-without-extension) != 0">
				<li class="fileName">
					<xsl:value-of select="$format" />
				</li>
			</xsl:if>
			<!-- Copy any options exported by the program. -->
			<!-- Ignore any old-style upper-level list items that contained lists. -->
			<xsl:apply-templates select="$metadata/xhtml:*[@class = 'options']//xhtml:li[not(xhtml:ul)]" />
			<!-- For now, assume that subsequent steps can ignore irrelevant options. -->
			<xsl:choose>
				<!-- In Help Converter, get the options from a file in the user folder. -->
				<xsl:when test="string-length($optionsFile) != 0">
					<xsl:apply-templates select="document(concat($userFolder, $optionsFile))//xhtml:ul[@class = 'options']/xhtml:li" />
				</xsl:when>
				<!-- In Phonology Assistant, initialize the options to default values. -->
				<xsl:otherwise>
					<li class="interactiveWebPage">true</li>
					<li class="tableOfDetails">true</li>
					<li class="hyperlinkToEthnologue">true</li>
					<li class="dateAndTime">true</li>
					<li class="oneMinimalPairPerGroup">true</li>
					<li class="moreSimilarVersusLessSimilarPairs">false</li>
					<!--
					<li class="textFlowOfColumnHeadings">horizontal</li>
					<li class="textFlowOfColumnHeadings">verticalCounterClockwise</li>
					<li class="textFlowOfColumnHeadings">verticalClockwise</li>
					-->
					<!--
					<li class="chart-tblHeader-textFlow">bt-lr</li>
					-->
					<li class="textFlowOfColumnHeadings">horizontal</li>
					<li class="hyphenatedColumnHeadings">true</li>
					<li class="articulatoryFeatureTable">true</li>
					<li class="binaryFeatureTable">true</li>
					<li class="hierarchicalFeatureTable">true</li>
					<li class="featureChartByPlaceOfArticulation">true</li>
					<li class="featureChartByMannerOfArticulation">false</li>
					<li class="generalizeEnvironments">true</li>
					<li class="generalizeItems">true</li>
					<li class="transposeColumnsAndRows">false</li>
					<!--
					<li class="withCVchart">true</li>
					-->
					<!--
					<li class="orientation">Portrait</li>
					<li class="orientation">Landscape</li>
					-->
					<li class="orientation">Portrait</li>
					<!--
					<li class="paperSize">A4</li>
					<li class="paperSize">Letter</li>
					<li class="paperSize">Legal</li>
					-->
					<li class="paperSize">Letter</li>
				</xsl:otherwise>
			</xsl:choose>
		</ul>
	</xsl:template>

</xsl:stylesheet>