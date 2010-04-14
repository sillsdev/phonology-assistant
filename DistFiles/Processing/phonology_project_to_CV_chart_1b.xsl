<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_to_CV_chart_1b.xsl 2010-04-09 -->
	<!-- Insert column and group attributes. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="units" select="/inventory/units" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
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
						<xsl:variable name="allKey" select="keys/chartKey[@class = 'all']" />
						<xsl:variable name="countPrecedingUnitsWithSameFeatures" select="count(preceding-sibling::unit[keys/chartKey[@class = 'all'] = $allKey])" />
						<xsl:choose>
							<!-- If a unit has the same features as a preceding unit, put it in a separate row. -->
							<xsl:when test="$countPrecedingUnitsWithSameFeatures != 0">
								<row literal="{@literal}">
									<xsl:value-of select="$rowKey" />
								</row>
							</xsl:when>
							<xsl:when test="not(preceding-sibling::unit[keys/chartKey[@class = 'rowgroup'] = $featureName][keys/chartKey[@class = 'row'] = $rowKey])">
								<row>
									<xsl:value-of select="$rowKey" />
								</row>
							</xsl:when>
						</xsl:choose>
					</xsl:for-each>
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>