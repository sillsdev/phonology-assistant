<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_5a_CV_chart_order.xsl 2011-10-28 -->
	<!-- Add an order attribute to chart keys and subkeys. -->
	<!-- At the end of the project inventory, order attributes were removed: -->
	<!-- * To reduce the size of the project phonetic inventory file. -->
	<!-- * To allow external editing or processing of the features and keys of segments. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- This pipeline assumes that segments have a column group, row group, and column key. -->
	<xsl:template match="segment">
		<xsl:if test="keys/chartKey[@class = 'colgroup'] and keys/chartKey[@class = 'rowgroup'] and keys/chartKey[@class = 'col']">
			<!-- Omit segments for which another segment represents it in the chart. -->
			<xsl:if test="not(@literalInChart)">
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:apply-templates select="features[@class = 'descriptive']" />
					<xsl:apply-templates select="keys" />
				</xsl:copy>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<!-- Make sure that chartKey elements exist for rows. -->
	<xsl:template match="segment/keys">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates />
			<xsl:if test="not(chartKey[@class = 'rowConditional'])">
				<chartKey class="rowConditional" />
			</xsl:if>
			<xsl:if test="not(chartKey[@class = 'rowGeneral'])">
				<chartKey class="rowGeneral" />
			</xsl:if>
			<xsl:if test="not(chartKey[@class = 'tone'])">
				<chartKey class="tone" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	
</xsl:stylesheet>