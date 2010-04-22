<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_0c_from_distribution_chart.xsl 2010-04-21 -->
	<!-- Make a table from the lists of colgroups, rowgroups, and units. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="xhtml:ul[@class = 'CV chart']">
		<xsl:variable name="units" select="." />
		<xsl:variable name="colgroupFeatures" select="../xhtml:ul[@class = 'colgroup features']" />
		<xsl:variable name="rowgroupFeatures" select="../xhtml:ul[@class = 'rowgroup features']" />
		<table xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="@*" />
			<colgroup>
				<col />
			</colgroup>
			<xsl:for-each select="$colgroupFeatures/xhtml:li">
				<colgroup>
					<col />
					<col />
				</colgroup>
			</xsl:for-each>
			<thead>
				<tr>
					<th />
					<xsl:for-each select="$colgroupFeatures/xhtml:li">
						<th scope="colgroup" colspan="2">
							<xsl:value-of select="." />
						</th>
					</xsl:for-each>
				</tr>
			</thead>
			<xsl:for-each select="$rowgroupFeatures/xhtml:li">
				<xsl:variable name="rowgroup" select="xhtml:span" />
				<xsl:variable name="rowspan" select="count(xhtml:ul/xhtml:li)" />
				<tbody>
					<xsl:for-each select="xhtml:ul/xhtml:li">
						<xsl:variable name="row" select="." />
						<tr>
							<xsl:if test="position() = 1">
								<th scope="rowgroup" rowspan="{$rowspan}">
									<xsl:value-of select="$rowgroup" />
								</th>
							</xsl:if>
							<xsl:for-each select="$colgroupFeatures/xhtml:li">
								<xsl:variable name="colgroup" select="." />
								<td class="Phonetic" xmlns="http://www.w3.org/1999/xhtml">
									<xsl:apply-templates select="$units/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'colgroup'] = $colgroup][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'rowgroup'] = $rowgroup][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'row'] = $row][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'col'] = 'Voiceless' or xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'col'] = 'Unrounded']/*" />
								</td>
								<td class="Phonetic" xmlns="http://www.w3.org/1999/xhtml">
									<xsl:apply-templates select="$units/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'colgroup'] = $colgroup][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'rowgroup'] = $rowgroup][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'row'] = $row][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'col'] = 'Voiced' or xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'col'] = 'Rounded']/*" />
								</td>
							</xsl:for-each>
						</tr>
					</xsl:for-each>
				</tbody>
			</xsl:for-each>
		</table>
	</xsl:template>

	<xsl:template match="xhtml:ul[@class = 'chart features']" />
	<xsl:template match="xhtml:ul[@class = 'colgroup features']" />
	<xsl:template match="xhtml:ul[@class = 'rowgroup features']" />

</xsl:stylesheet>