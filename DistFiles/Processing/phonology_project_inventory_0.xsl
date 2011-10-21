<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_0.xsl 2011-10-21 -->
	<!-- Make sure that a .tmp file for project phonetic inventory or CV chart contains -->
	<!-- essential information from the program phonetic inventory and distinctive features files. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Make sure that a .tmp file for project phonetic inventory or CV chart contains: -->
	<!-- * featureDefinitions[@class = 'descriptive'] from program phonetic inventory -->
	<!-- * chartKeyPatterns from program phonetic inventory -->
	<!-- * featureDefinitions[@class = 'distinctive'] from selected program distinctive features -->
	<xsl:template match="/inventory">
		<xsl:variable name="metadata" select="div[@id = 'metadata']" />
		<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
		<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
		<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />
		<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates />
			<xsl:if test="not(featureDefinitions[@class = 'descriptive'])">
				<xsl:apply-templates select="document($programPhoneticInventoryXML)/inventory/featureDefinitions[@class = 'descriptive']" />
			</xsl:if>
			<xsl:if test="not(chartKeyPatterns)">
				<xsl:apply-templates select="document($programPhoneticInventoryXML)/inventory/chartKeyPatterns" />
			</xsl:if>
			<xsl:if test="not(featureDefinitions[@class = 'distinctive'])">
				<xsl:variable name="programDistinctiveFeaturesFile" select="concat($settings/li[@class = 'programDistinctiveFeaturesName'], '.DistinctiveFeatures.xml')" />
				<xsl:variable name="programDistinctiveFeaturesXML" select="concat($programConfigurationFolder, $programDistinctiveFeaturesFile)" />
				<xsl:apply-templates select="document($programDistinctiveFeaturesXML)/inventory/featureDefinitions[@class = 'distinctive']" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<!-- Add order attributes to featureDefinition elements. -->
	<xsl:template match="/inventory/featureDefinitions">
		<xsl:variable name="orderFormat" select="translate(count(featureDefinition), '0123456789', '0000000000')" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="featureDefinition">
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:if test="not(@order)">
						<xsl:attribute name="order">
							<xsl:value-of select="format-number(position(), $orderFormat)" />
						</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates />
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>