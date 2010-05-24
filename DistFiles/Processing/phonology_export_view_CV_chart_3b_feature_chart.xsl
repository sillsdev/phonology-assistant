<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

	<!-- phonology_export_view_CV_chart_3b_feature_chart.xsl 2010-05-24 -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- In a feature chart, remove any feature rows that contain only empty data cells. -->
	<xsl:template match="xhtml:table[@class = 'feature chart']/xhtml:tbody/xhtml:tr[xhtml:td and not(xhtml:td[node()])]" />

	<xsl:template match="xhtml:table[@class = 'feature chart']//xhtml:td">
		<xsl:copy>
			<xsl:if test="xhtml:span[@class = 'unmarked'] and not(xhtml:span[@class = 'marked'])">
				<xsl:attribute name="class">
					<xsl:value-of select="'unmarked'" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'feature chart']//xhtml:td/xhtml:span">
		<xsl:apply-templates />
	</xsl:template>

	<!--
	<xsl:template match="xhtml:table[@class = 'feature chart']//xhtml:td[count(xhtml:span) = 2 and xhtml:span[@class = 'marked']]/xhtml:span[@class = 'unmarked']">
		<xsl:copy-of select="." />
	</xsl:template>
	-->

</xsl:stylesheet>