<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_1a.xsl 2010-03-29 -->
	<!-- For each phone, copy information about its symbols from the program phonetic inventory. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <xsl:variable name="programConfigurationFolder" select="//div[@id = 'metadata']/ul[@class = 'settings']/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="//div[@id = 'metadata']/ul[@class = 'settings']/li[@class = 'programPhoneticInventoryFile']" />

  <xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="symbols" select="document($programPhoneticInventoryXML)/inventory/symbols" />

	<xsl:variable name="languageName" select="/inventory/@languageName" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

  <xsl:template match="unit">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<sequence>
				<xsl:call-template name="sequence">
					<xsl:with-param name="text" select="@literal" />
				</xsl:call-template>
			</sequence>
		</xsl:copy>
  </xsl:template>

  <xsl:template name="sequence">
    <xsl:param name="text" />
    <xsl:if test="string-length($text) != 0">
      <xsl:variable name="char" select="substring($text, 1, 1)" />
      <xsl:apply-templates select="$symbols/symbol[@literal = $char]" />
      <xsl:call-template name="sequence">
        <xsl:with-param name="text" select="substring($text, 2)" />
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="symbol">
    <symbol literal="{@literal}">
      <xsl:copy-of select="description" />
      <xsl:copy-of select="type" />
      <xsl:copy-of select="subType" />
      <xsl:copy-of select="isBase" />
      <xsl:copy-of select="canPrecedeBase" />
      <xsl:copy-of select="articulatoryFeatures" />
			<xsl:copy-of select="articulatoryFeatureRule" />
			<xsl:copy-of select="binaryFeatures" />
    </symbol>
  </xsl:template>

</xsl:stylesheet>