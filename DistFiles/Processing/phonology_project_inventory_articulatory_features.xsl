<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_articulatory_features.xsl 2010-04-09 -->
  <!-- Provide classification and order of articulatory features. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="programArticulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />

	<xsl:variable name="featureOrderFormat" select="translate(count($programArticulatoryFeatures/feature), '0123456789', '0000000000')" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="articulatoryFeatures/feature">
    <xsl:variable name="featureName" select="." />
    <xsl:variable name="feature" select="$programArticulatoryFeatures/feature[name = $featureName]" />
    <xsl:copy>
			<xsl:attribute name="class">
				<xsl:variable name="featureClass" select="$feature/@class" />
				<xsl:choose>
					<xsl:when test="$featureClass = 'col-row' and ../feature[. = 'Consonant']">
						<xsl:value-of select="'col'" />
					</xsl:when>
					<xsl:when test="$featureClass = 'col-row' and ../feature[. = 'Vowel']">
						<xsl:value-of select="'row'" />
					</xsl:when>
					<xsl:when test="$featureClass = 'row-col' and ../feature[. = 'Consonant']">
						<xsl:value-of select="'row'" />
					</xsl:when>
					<xsl:when test="$featureClass = 'row-col' and ../feature[. = 'Vowel']">
						<xsl:value-of select="'col'" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$featureClass" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:copy-of select="$feature/@subclass" />
			<xsl:copy-of select="@primary" />
			<xsl:copy-of select="$feature/@type" />
			<xsl:attribute name="order">
				<xsl:apply-templates select="$feature" mode="order" />
			</xsl:attribute>
			<xsl:apply-templates />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="articulatoryFeatures/feature" mode="order">
		<xsl:value-of select="format-number(count(preceding-sibling::feature) + 1, $featureOrderFormat)" />
	</xsl:template>

</xsl:stylesheet>