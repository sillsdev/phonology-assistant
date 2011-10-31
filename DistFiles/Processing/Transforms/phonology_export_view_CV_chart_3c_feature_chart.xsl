<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_3c_feature_chart.xsl 2011-10-17 -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="@* | node()" mode="transpose">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="transpose" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive-segment values chart') or starts-with(@class, 'distinctive-descriptive values chart')]">
		<xsl:choose>
			<xsl:when test="(contains(@class, 'place') or contains(@class, 'backness')) and count(xhtml:thead/xhtml:tr/xhtml:th[starts-with(@class, 'Phonetic')]) &lt; 40">
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@*" mode="transpose" />
					<colgroup xmlns="http://www.w3.org/1999/xhtml">
						<xsl:for-each select="xhtml:thead/xhtml:tr">
							<col xmlns="http://www.w3.org/1999/xhtml" />
						</xsl:for-each>
					</colgroup>
					<xsl:for-each select="xhtml:tbody">
						<colgroup xmlns="http://www.w3.org/1999/xhtml">
							<xsl:for-each select="xhtml:tr">
								<col />
							</xsl:for-each>
						</colgroup>
					</xsl:for-each>
					<thead xmlns="http://www.w3.org/1999/xhtml">
						<tr>
							<th />
							<xsl:apply-templates select="xhtml:tbody/xhtml:tr/xhtml:th" mode="transpose" />
						</tr>
					</thead>
					<xsl:variable name="table" select="." />
					<xsl:variable name="tr" select="xhtml:thead/xhtml:tr[1]" />
					<xsl:for-each select="xhtml:colgroup[preceding-sibling::*[1][self::xhtml:colgroup]]">
						<xsl:variable name="colgroup" select="." />
						<xsl:variable name="n" select="count($colgroup/preceding-sibling::xhtml:colgroup[preceding-sibling::*[1][self::xhtml:colgroup]]/xhtml:col)" />
						<tbody xmlns="http://www.w3.org/1999/xhtml">
							<xsl:for-each select="xhtml:col">
								<xsl:variable name="i" select="$n + position()" />
								<tr>
									<xsl:apply-templates select="$tr/xhtml:th[node()][$i]" mode="transpose" />
									<xsl:apply-templates select="$table/xhtml:tbody/xhtml:tr/xhtml:td[$i]" mode="transpose" />
								</tr>
							</xsl:for-each>
						</tbody>
					</xsl:for-each>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>		
	</xsl:template>

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive-segment values chart')]/@class" mode="transpose">
		<xsl:attribute name="class">
			<xsl:value-of select="'segment-distinctive values chart'" />
			<xsl:value-of select="substring-after(., 'distinctive-segment values chart')" />
		</xsl:attribute>
	</xsl:template>

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive-descriptive values chart')]/@class" mode="transpose">
		<xsl:attribute name="class">
			<xsl:value-of select="'descriptive-distinctive values chart'" />
			<xsl:value-of select="substring-after(., 'distinctive-descriptive values chart')" />
		</xsl:attribute>
	</xsl:template>

	<xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th" mode="transpose">
		<xsl:copy>
			<xsl:attribute name="class">
				<xsl:value-of select="'rotate'" />
				<xsl:if test="../@class = 'redundant'">
					<xsl:value-of select="' redundant'" />
				</xsl:if>
			</xsl:attribute>
			<xsl:apply-templates select="@*" mode="transpose" />
			<div xmlns="http://www.w3.org/1999/xhtml"><span><xsl:value-of select="." /></span></div>
		</xsl:copy>
	</xsl:template>

  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th/@scope" mode="transpose">
    <xsl:attribute name="scope">
      <xsl:value-of select="'col'" />
    </xsl:attribute>
  </xsl:template>

	<xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[@class = 'rotate'][xhtml:div/xhtml:span]" mode="transpose">
		<xsl:copy>
			<xsl:apply-templates select="@scope" mode="transpose" />
			<xsl:value-of select="xhtml:div/xhtml:span" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:thead/xhtml:tr/xhtml:th/@scope" mode="transpose">
    <xsl:attribute name="scope">
      <xsl:value-of select="'row'" />
    </xsl:attribute>
  </xsl:template>

	<!-- Add redundant class to non-empty data cells only. -->
	<xsl:template match="xhtml:tr[@class = 'redundant']/xhtml:td[text()]" mode="transpose">
		<xsl:copy>
			<xsl:copy-of select="../@class" />
			<xsl:value-of select="." />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>