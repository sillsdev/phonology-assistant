<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2e_hierarchical_features.xsl 2010-03-13 -->
	<!-- Simple list of hierarchical features. -->

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="root">
		<xsl:copy-of select="." />
		<hierarchicalFeatures>
			<xsl:apply-templates mode="hierarchicalFeatures" />
		</hierarchicalFeatures>
	</xsl:template>

	<xsl:template match="feature" mode="hierarchicalFeatures">
		<xsl:copy-of select="." />
	</xsl:template>

	<xsl:template match="class" mode="hierarchicalFeatures">
		<feature>
			<xsl:value-of select="@name" />
		</feature>
		<xsl:apply-templates mode="hierarchicalFeatures" />
	</xsl:template>

</xsl:stylesheet>