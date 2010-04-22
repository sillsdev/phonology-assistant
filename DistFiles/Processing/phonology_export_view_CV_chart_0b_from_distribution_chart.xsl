<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_0b_from_distribution_chart.xsl 2010-04-21 -->
	<!-- Make sure that there are no duplicate units from the search items of the distribution chart. -->
	<!-- Keep a colgroup or rowgroup feature only if at least one unit has it. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Make sure that there are no duplicate units from the search items of the distribution chart. -->
	<xsl:template match="xhtml:ul[@class = 'CV chart']/xhtml:li">
		<xsl:variable name="literal" select="xhtml:span" />
		<xsl:if test="not(preceding-sibling::xhtml:li[xhtml:span = $literal])">
			<xsl:copy>
				<xsl:apply-templates select="@* | node()" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<!-- Keep a colgroup feature only if at least one unit has it. -->
	<xsl:template match="xhtml:ul[@class = 'colgroup features']/xhtml:li">
		<xsl:variable name="feature" select="." />
		<xsl:if test="../../xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'colgroup'][. = $feature]]">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

	<!-- Keep a rowgroup feature only if at least one unit has it. -->
	<xsl:template match="xhtml:ul[@class = 'rowgroup features']/xhtml:li">
    <xsl:variable name="feature" select="xhtml:span" />
    <!-- Keep this chart heading feature only if at least one phone has it. -->
    <xsl:if test="../../xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'rowgroup'][. = $feature]]">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
				<ul xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="../../xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'rowgroup'][. = $feature]]">
						<xsl:sort select="xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'row']" />
						<xsl:variable name="rowKey" select="xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'row']" />
						<xsl:if test="not(preceding-sibling::xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'rowgroup'][. = $feature]][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'row'][. = $rowKey]])">
							<li>
								<xsl:value-of select="$rowKey" />
							</li>
						</xsl:if>
					</xsl:for-each>
				</ul>
			</xsl:copy>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>