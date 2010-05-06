<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2c_feature_overrides.xsl 2010-04-24 -->
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
			<xsl:choose>
				<xsl:when test="articulatoryFeatures[@changed = 'true']">
					<xsl:apply-templates select="articulatoryFeatures[@changed = 'true']" />
				</xsl:when>
				<xsl:otherwise>
					<!-- If the phone contains only one base symbol, there is only one sequence. -->
					<!-- If the phone is an ambiguous sequence, select the first sequence pattern that matched its base symbols. -->
					<xsl:variable name="sequence" select="sequence[not(symbol/@primary = 'unmatched' or symbol/if/feature[. = 'unmatched'] or symbol/if/anyOf[not(feature)])][1]" />
					<articulatoryFeatures>
						<xsl:apply-templates select="$sequence/symbol/articulatoryFeatures/feature" />
					</articulatoryFeatures>
					<xsl:apply-templates select="$sequence" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
  </xsl:template>

</xsl:stylesheet>