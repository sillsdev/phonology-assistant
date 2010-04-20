<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

	<!-- phonology_export_view_CV_chart_3b_feature_chart.xsl 2010-04-20 -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- In a feature chart, remove any feature rows that contain only empty data cells. -->
	<xsl:template match="xhtml:table[@class = 'feature chart']/xhtml:tbody/xhtml:tr[xhtml:td and not(xhtml:td[text()])]" />

</xsl:stylesheet>