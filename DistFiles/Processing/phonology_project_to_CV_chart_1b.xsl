<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_to_CV_chart_1b.xsl 2010-03-23 -->
	<!-- Insert lists of feature names. -->
	<!-- Number the articulatory features. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="units" select="/inventory/units" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="colgroupFeatures">
		<xsl:copy>
			<xsl:for-each select="feature">
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:attribute name="column">
						<xsl:value-of select="2 * (position() - 1)" />
					</xsl:attribute>
					<xsl:apply-templates />
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="rowgroupFeatures">
		<xsl:copy>
			<xsl:for-each select="feature">
				<xsl:variable name="featureName" select="." />
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:attribute name="group">
						<xsl:value-of select="position() - 1" />
					</xsl:attribute>
					<name>
						<xsl:value-of select="$featureName" />
					</name>
					<xsl:for-each select="$units/unit[keys/chartKey[@class = 'rowgroup'] = $featureName]">
						<xsl:sort select="keys/chartKey[@class = 'row']" />
						<xsl:variable name="rowKey" select="keys/chartKey[@class = 'row']" />
						<xsl:if test="not(preceding-sibling::unit[keys/chartKey[@class = 'rowgroup'] = $featureName][keys/chartKey[@class = 'row'] = $rowKey])">
							<row>
								<xsl:value-of select="$rowKey" />
							</row>
						</xsl:if>
					</xsl:for-each>
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>