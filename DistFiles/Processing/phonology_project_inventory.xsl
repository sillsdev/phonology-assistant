<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory.xsl 2010-03-25 -->
	<!-- For each unit that is in the program phonetic inventory, insert articulatory and binary features. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />

  <xsl:variable name="programConfigurationFolder" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programPhoneticInventoryFile']" />

  <xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="symbols" select="document($programPhoneticInventoryXML)/inventory/symbols" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

  <xsl:template match="unit">
		<xsl:variable name="literal" select="@literal" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:variable name="symbol" select="$symbols/symbol[@literal = $literal]" />
			<xsl:if test="$symbol">
				<xsl:apply-templates select="$symbol/articulatoryFeatures" />
				<xsl:apply-templates select="$symbol/binaryFeatures" />
			</xsl:if>
		</xsl:copy>
  </xsl:template>

</xsl:stylesheet>