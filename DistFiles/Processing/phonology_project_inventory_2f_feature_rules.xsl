<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2e_hierarchical_features.xsl 2010-05-26 -->
	<!-- Simple list of hierarchical features. -->

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="root">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
		<hierarchicalFeatures>
			<xsl:apply-templates mode="hierarchicalFeatures" />
		</hierarchicalFeatures>
	</xsl:template>

	<!-- Omit repeated bivalent values from diphthongs. -->
	<!-- For example, [-low] and [-low] from [Close] and [Close-mid] height for [ie]. -->
	<!-- Versus only one [-low] for [iu] because sequence pattern removes duplicate [Close]. -->
	<xsl:template match="root//feature">
		<xsl:variable name="feature" select="." />
		<xsl:if test="not(preceding-sibling::feature[. = $feature])">
			<xsl:copy-of select="." />
		</xsl:if>
	</xsl:template>

	<xsl:template match="feature" mode="hierarchicalFeatures">
		<xsl:variable name="feature" select="." />
		<xsl:if test="not(preceding-sibling::feature[. = $feature])">
			<xsl:copy-of select="." />
		</xsl:if>
	</xsl:template>

	<xsl:template match="class" mode="hierarchicalFeatures">
		<feature>
			<xsl:value-of select="@name" />
		</feature>
		<xsl:apply-templates mode="hierarchicalFeatures" />
	</xsl:template>

</xsl:stylesheet>