<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2c_feature_overrides.xsl 2010-04-09 -->
  <!-- Override features of units (phonetic or phonological). -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="unit">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:if test="not(articulatoryFeatures[@changed = 'true'])">
				<!-- Unless the researcher has made explicit features changes to a unit, merge features from the first sequence which is not unmatched. -->
				<articulatoryFeatures>
					<xsl:apply-templates select="sequence[not(symbol/@primary = 'unmatched' or symbol/if/feature[. = 'unmatched'] or symbol/if/anyOf[not(feature)])][1]/symbol/articulatoryFeatures/feature" />
				</articulatoryFeatures>
			</xsl:if>
			<!-- Keep the first sequence which is not unmatched. -->
			<xsl:apply-templates select="sequence[not(symbol/@primary = 'unmatched' or symbol/if/feature[. = 'unmatched'] or symbol/if/anyOf[not(feature)])][1]" />
		</xsl:copy>
  </xsl:template>

</xsl:stylesheet>