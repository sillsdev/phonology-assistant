<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

	<!-- phonology_export_view_list_3b_minimal_pairs_difference.xsl 2010-04-05 -->
	<!-- If there are minimal pairs, include lists of feature differences in XHTML files. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />

	<!-- A project phonetic inventory file contains features of phonetic or phonological units, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="units" select="document($projectPhoneticInventoryXML)/inventory/units[@type = 'phonetic']" />

	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="interactiveWebPage">
		<xsl:if test="$format = 'XHTML'">
			<xsl:value-of select="$options/xhtml:li[@class = 'interactiveWebPage']" />
		</xsl:if>
	</xsl:variable>

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Apply or ignore transformations. -->
	<xsl:template match="xhtml:table[@class = 'list']">
		<xsl:choose>
			<xsl:when test="$interactiveWebPage = 'true'">
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<!-- To ignore the following rules, copy instead of apply-templates. -->
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'list']/xhtml:tbody[@class = 'group']/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']/xhtml:ul[count(xhtml:li[xhtml:span]) = 2]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates select="xhtml:li[xhtml:span][1]" mode="pair">
				<xsl:with-param name="literalOther" select="xhtml:li[xhtml:span][2]/xhtml:span" />
			</xsl:apply-templates>
			<xsl:apply-templates select="xhtml:li[xhtml:span][2]" mode="pair">
				<xsl:with-param name="literalOther" select="xhtml:li[xhtml:span][1]/xhtml:span" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'list']/xhtml:tbody[@class = 'group']/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']/xhtml:ul/xhtml:li" mode="pair">
		<xsl:param name="literalOther" />
		<xsl:variable name="literal" select="xhtml:span" />
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<div class="differences" xmlns="http://www.w3.org/1999/xhtml">
				<ul class="articulatory">
					<xsl:apply-templates select="$units/unit[@literal = $literal]/articulatoryFeatures/feature" mode="differences">
						<xsl:with-param name="featuresOther" select="$units/unit[@literal = $literalOther]/articulatoryFeatures" />
					</xsl:apply-templates>
				</ul>
			</div>
		</xsl:copy>
	</xsl:template>

  <xsl:template match="feature" mode="differences">
    <xsl:param name="featuresOther" />
    <xsl:variable name="feature" select="." />
    <xsl:if test="not($featuresOther/feature[. = $feature])">
      <li xmlns="http://www.w3.org/1999/xhtml">
        <xsl:value-of select="." />
      </li>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>