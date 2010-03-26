<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2c_feature_overrides.xsl 2010-03-22 -->
  <!-- Override features of units (phonetic or phonological). -->
  <!-- Otherwise, move features of remaining base symbol to the unit itself. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="unit">
    <xsl:variable name="literal" select="@literal" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:if test="not(articulatoryFeatures[@changed = 'true'])">
				<xsl:copy-of select="sequence/symbol/articulatoryFeatures" />
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
  </xsl:template>

</xsl:stylesheet>