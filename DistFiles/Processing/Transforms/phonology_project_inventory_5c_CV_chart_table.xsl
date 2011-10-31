<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_project_inventory_5c_CV_chart_table.xsl 2011-08-18 -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:param name="base-file-level" select="'0'" />

	<xsl:variable name="titlePrefix" select="'Features of consonants and vowels'" />
	<xsl:variable name="classPrefix" select="'CV chart'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:div[starts-with(@class, 'CV chart')]">
		<xsl:variable name="ulColgroup" select="xhtml:ul[@class = 'colgroup']" />
		<xsl:variable name="ulRowgroup" select="xhtml:ul[@class = 'rowgroup']" />
		<xsl:variable name="ulSegment" select="xhtml:ul[@class = 'segment']" />
		<table xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="@*" />
			<colgroup>
				<col />
			</colgroup>
			<xsl:for-each select="$ulColgroup/xhtml:li">
				<colgroup>
					<col />
					<col />
				</colgroup>
			</xsl:for-each>
			<thead>
				<tr>
					<th />
					<xsl:for-each select="$ulColgroup/xhtml:li">
						<th scope="colgroup" colspan="2">
							<xsl:value-of select="." />
						</th>
					</xsl:for-each>
				</tr>
			</thead>
			<xsl:for-each select="$ulRowgroup/xhtml:li">
				<xsl:variable name="chartKeyRowgroup" select="xhtml:span" />
				<tbody>
					<tr>
						<th scope="row">
							<xsl:value-of select="$chartKeyRowgroup" />
						</th>
						<xsl:call-template name="row">
							<xsl:with-param name="ulColgroup" select="$ulColgroup" />
							<xsl:with-param name="chartKeyRowgroup" select="$chartKeyRowgroup" />
							<xsl:with-param name="chartKeyRow" select="xhtml:ul/xhtml:li[1]" />
							<xsl:with-param name="ulSegment" select="$ulSegment" />
						</xsl:call-template>
					</tr>
					<xsl:for-each select="xhtml:ul/xhtml:li[preceding-sibling::xhtml:li]">
						<tr>
							<th scope="row" />
							<xsl:call-template name="row">
								<xsl:with-param name="ulColgroup" select="$ulColgroup" />
								<xsl:with-param name="chartKeyRowgroup" select="$chartKeyRowgroup" />
								<xsl:with-param name="chartKeyRow" select="." />
								<xsl:with-param name="ulSegment" select="$ulSegment" />
							</xsl:call-template>
						</tr>
					</xsl:for-each>
				</tbody>
			</xsl:for-each>
		</table>
	</xsl:template>

	<xsl:template name="row">
		<xsl:param name="ulColgroup" />
		<xsl:param name="chartKeyRowgroup" />
		<xsl:param name="chartKeyRow" />
		<xsl:param name="ulSegment" />
		<xsl:for-each select="$ulColgroup/xhtml:li">
			<xsl:variable name="chartKeyColgroup" select="." />
			<xsl:call-template name="col">
				<xsl:with-param name="chartKeyRowgroup" select="$chartKeyRowgroup" />
				<xsl:with-param name="chartKeyColgroup" select="$chartKeyColgroup" />
				<xsl:with-param name="chartKeyCol" select="'0'" />
				<xsl:with-param name="chartKeyRow" select="$chartKeyRow" />
				<xsl:with-param name="ulSegment" select="$ulSegment" />
			</xsl:call-template>
			<xsl:call-template name="col">
				<xsl:with-param name="chartKeyRowgroup" select="$chartKeyRowgroup" />
				<xsl:with-param name="chartKeyColgroup" select="$chartKeyColgroup" />
				<xsl:with-param name="chartKeyCol" select="'1'" />
				<xsl:with-param name="chartKeyRow" select="$chartKeyRow" />
				<xsl:with-param name="ulSegment" select="$ulSegment" />
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="col">
		<xsl:param name="chartKeyColgroup" />
		<xsl:param name="chartKeyRowgroup" />
		<xsl:param name="chartKeyCol" />
		<xsl:param name="chartKeyRow" />
		<xsl:param name="ulSegment" />
		<xsl:variable name="li" select="$ulSegment/xhtml:li[xhtml:ul[@class = 'chartKey'][xhtml:li[@class = 'rowgroup'] = $chartKeyRowgroup][xhtml:li[@class = 'colgroup'] = $chartKeyColgroup][xhtml:li[@class = 'col'] = $chartKeyCol][xhtml:li[@class = 'row'] = $chartKeyRow]]" />
		<td class="Phonetic" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:if test="$li">
				<xsl:value-of select="$li/xhtml:span" />
			</xsl:if>
		</td>
	</xsl:template>

</xsl:stylesheet>