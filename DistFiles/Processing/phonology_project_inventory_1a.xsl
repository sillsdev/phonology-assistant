<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_1a.xsl 2010-05-20 -->
	<!-- For each phone, copy information about its symbols from the program phonetic inventory. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="symbols" select="document($programPhoneticInventoryXML)/inventory/symbols" />

	<xsl:variable name="languageName" select="/inventory/@languageName" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

  <xsl:template match="unit">
		<xsl:variable name="units" select=".." />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<sequence>
				<xsl:call-template name="sequence">
					<xsl:with-param name="text" select="@literal" />
					<xsl:with-param name="units" select="$units" />
				</xsl:call-template>
			</sequence>
		</xsl:copy>
  </xsl:template>

  <xsl:template name="sequence">
    <xsl:param name="text" />
		<xsl:param name="units" />
    <xsl:if test="string-length($text) != 0">
      <xsl:variable name="char" select="substring($text, 1, 1)" />
      <xsl:apply-templates select="$symbols/symbol[@literal = $char]">
				<xsl:with-param name="units" select="$units" />
			</xsl:apply-templates>
      <xsl:call-template name="sequence">
        <xsl:with-param name="text" select="substring($text, 2)" />
				<xsl:with-param name="units" select="$units" />
			</xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="symbol">
		<xsl:param name="units" />
		<xsl:variable name="literal" select="@literal" />
		<xsl:variable name="articulatoryFeaturesChanged" select="$units/unit[@literal = $literal]/articulatoryFeatures[@changed = 'true']" />
    <symbol literal="{@literal}">
      <xsl:copy-of select="description" />
      <xsl:copy-of select="type" />
      <xsl:copy-of select="subType" />
      <xsl:copy-of select="isBase" />
      <xsl:copy-of select="canPrecedeBase" />
			<xsl:choose>
				<xsl:when test="$articulatoryFeaturesChanged">
		      <xsl:copy-of select="$articulatoryFeaturesChanged" />
				</xsl:when>
				<xsl:otherwise>
				  <xsl:copy-of select="articulatoryFeatures" />
				</xsl:otherwise>
			</xsl:choose>
			<xsl:copy-of select="articulatoryFeatureRule" />
			<xsl:copy-of select="binaryFeatures" />
    </symbol>
  </xsl:template>

</xsl:stylesheet>