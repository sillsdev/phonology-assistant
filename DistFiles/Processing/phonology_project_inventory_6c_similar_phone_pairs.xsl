<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_6c_similar_phone_pairs.xsl 2010-05-14 -->
  <!-- Delete pairs that are not similar according to features. -->
	<!-- Remove hierarchicalFeatures attribute from pair. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="similarPhonePairs/pair">
		<xsl:choose>
			<!-- Exclude the pair if it matches an anti-pattern. -->
			<xsl:when test="pattern[not(phone/*)][@action = 'exclude']" />
			<!-- Include the pair if either: -->
			<!-- * The hierarchical feature differences are within the threshold. -->
			<!-- * It matches a pattern. -->
			<xsl:when test="@hierarchicalFeatures = 'true' or pattern[not(phone/*)][@action = 'include']">
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- Remove temporary elements from steps 6a and 6b. -->
	<xsl:template match="similarPhonePairs/pair/@hierarchicalFeatures" />
	<xsl:template match="similarPhonePairs/pair/pattern" />

</xsl:stylesheet>