<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_3b_feature_changes.xsl 2010-03-22 -->
  <!-- Determine which articulatory features of units are overrides compared to the defaults. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="unit/articulatoryFeatures">
		<xsl:variable name="articulatoryFeatures" select="." />
		<xsl:variable name="sequence" select="../sequence" />
		<xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
		<articulatoryFeatureChanges>
			<add>
				<xsl:for-each select="$articulatoryFeatures/feature">
					<xsl:variable name="feature" select="." />
					<xsl:choose>
						<xsl:when test="$sequence/symbol[1]/articulatoryFeatures/feature[. = $feature]" />
						<xsl:otherwise>
							<xsl:copy-of select="." />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</add>
			<remove>
				<xsl:for-each select="$sequence/symbol[1]/articulatoryFeatures/feature">
					<xsl:variable name="feature" select="." />
					<xsl:choose>
						<xsl:when test="$articulatoryFeatures/feature[. = $feature]" />
						<xsl:otherwise>
							<xsl:copy-of select="." />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</remove>
		</articulatoryFeatureChanges>
  </xsl:template>

	<xsl:template match="unit[string-length(@literal) = 1]/binaryFeatures">
		<xsl:variable name="binaryFeatures" select="." />
		<xsl:variable name="sequence" select="../sequence" />
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
		<binaryFeatureChanges>
			<add>
				<xsl:for-each select="$binaryFeatures/feature">
					<xsl:variable name="feature" select="." />
					<xsl:choose>
						<xsl:when test="$sequence/symbol[1]/binaryFeatures/feature[. = $feature]" />
						<xsl:otherwise>
							<xsl:copy-of select="." />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</add>
			<remove>
				<xsl:for-each select="$sequence/symbol[1]/binaryFeatures/feature">
					<xsl:variable name="feature" select="." />
					<xsl:choose>
						<xsl:when test="$binaryFeatures/feature[. = $feature]" />
						<xsl:otherwise>
							<xsl:copy-of select="." />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</remove>
		</binaryFeatureChanges>
	</xsl:template>

	<xsl:template match="unit/sequence" />

</xsl:stylesheet>