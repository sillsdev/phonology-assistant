<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_4c_distinctive_features.xsl 2011-09-21 -->
	<!-- If there are distinctive feature overrides: -->
	<!-- * Sort them in order. -->
	<!-- * If feature overrides differ from default features, keep a copy of default features. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Sort distinctive feature (overrides). -->
	<xsl:template match="features[@class = 'distinctive'][following-sibling::features[@class = 'distinctive default']]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="feature">
				<xsl:sort select="@order" />
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

	<!-- Remove order attribute from distinctive features in the project inventory file. -->
	<xsl:template match="features[@class = 'distinctive']/feature/@order" />

	<!-- If feature overrides differ from default features, keep a copy of default features. -->
	<xsl:template match="features[@class = 'distinctive default']">
		<xsl:variable name="boolean">
			<xsl:call-template name="booleanFeatureDifferences">
				<xsl:with-param name="featuresA" select="." />
				<xsl:with-param name="featuresB" select="preceding-sibling::features[@class = 'distinctive']" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$boolean = 'true'">
			<xsl:copy>
				<xsl:apply-templates select="@* | node()" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>