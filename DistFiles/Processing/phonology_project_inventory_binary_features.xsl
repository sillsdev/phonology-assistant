<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_binary_features.xsl 2010-03-26 -->
  <!-- Omit questionable binary features of units. -->
  <!-- TO DO: Include an attribute for the hierarchical order? -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="programConfigurationFolder" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programPhoneticInventoryFile']" />
	
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="programBinaryFeatures" select="document($programPhoneticInventoryXML)/inventory/binaryFeatures" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Copy features in the standard order. -->
  <xsl:template match="binaryFeatures">
		<xsl:variable name="projectArticulatoryFeatures" select="../articulatoryFeatures" />
		<xsl:variable name="projectBinaryFeatures" select="." />
		<xsl:copy>
			<xsl:for-each select="$programBinaryFeatures/feature">
				<xsl:variable name="featureName" select="name" />
				<xsl:if test="$projectBinaryFeatures/feature[substring(., 2) = $featureName]">
					<xsl:choose>
						<!-- Features that might be removed. -->
						<xsl:when test="$featureName = 'approx'" />
						<xsl:when test="$featureName = 'Continuant articulatory'" />
						<xsl:when test="$featureName = 'Epiglottal'" />
						<xsl:when test="$featureName = 'Hyper anterior'" />
						<xsl:when test="$featureName = 'Implosive'" />
						<xsl:when test="$featureName = 'Labio-dental'" />
						<!-- Features that might not be specified for vowels. -->
						<!--
						<xsl:when test="$featureName = 'Front'" />
						<xsl:when test="($featureName = 'ant' or $featureName = 'COR' or $featureName = 'distr') and $projectArticulatoryFeatures/feature[. = 'Vowel']" />
						-->
						<xsl:otherwise>
							<xsl:copy-of select="$projectBinaryFeatures/feature[substring(., 2) = $featureName]" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>