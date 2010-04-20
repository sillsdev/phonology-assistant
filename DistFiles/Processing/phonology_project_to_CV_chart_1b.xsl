<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_to_CV_chart_1b.xsl 2010-04-20 -->
	<!-- Compute column and group attributes. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="units" select="/inventory/units" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Compute the column attribute. -->
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

	<!-- Compute the group attribute and add row elements. -->
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
						<xsl:sort select="keys/chartKeys[@class = 'row']/@order" />
						<xsl:variable name="rowOrder" select="keys/chartKeys[@class = 'row']/@order" />
						<xsl:variable name="allOrder" select="keys/chartKeys[@class = 'all']/@order" />
						<xsl:variable name="countPrecedingUnitsWithSameFeatures" select="count(preceding-sibling::unit[keys/chartKeys[@class = 'all']/@order = $allOrder])" />
						<xsl:choose>
							<!-- If a unit has the same features as a preceding unit, put it in a separate row. -->
							<xsl:when test="$countPrecedingUnitsWithSameFeatures != 0">
								<row literal="{@literal}">
									<xsl:value-of select="$rowOrder" />
								</row>
							</xsl:when>
							<xsl:when test="not(preceding-sibling::unit[keys/chartKey[@class = 'rowgroup'] = $featureName][keys/chartKeys[@class = 'row']/@order = $rowOrder])">
								<row>
									<xsl:value-of select="$rowOrder" />
								</row>
							</xsl:when>
						</xsl:choose>
					</xsl:for-each>
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>