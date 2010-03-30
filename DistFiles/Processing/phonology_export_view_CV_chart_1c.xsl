<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_1c.xsl 2010-03-13 -->
  <!-- Keep only the sort keys for unique rows. Use the position in the program phonetic inventory. -->
  <!-- Keep only the auxilliary binary features which distinguish phones in this chart. -->
  <!-- Keep classes only for binary feature values that at least one phone has. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:ul[@class = 'rowgroup features']/xhtml:li">
    <xsl:copy>
      <xsl:apply-templates select="xhtml:span" />
      <ul xmlns="http://www.w3.org/1999/xhtml">
        <xsl:for-each select="xhtml:ul/xhtml:li">
					<xsl:sort select="." />
          <xsl:variable name="rowKey" select="." />
          <xsl:if test="not(preceding-sibling::xhtml:li[. = $rowKey])">
						<xsl:copy-of select="." />
          </xsl:if>
        </xsl:for-each>
      </ul>
    </xsl:copy>
  </xsl:template>

	<xsl:template match="xhtml:table[@class = 'binary features']/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'minus'][not(text())] and xhtml:td[@class = 'plus'][not(text())]]" />

  <xsl:template match="xhtml:table[contains(@class, 'features')]//xhtml:td[@class = 'minus' or @class = 'plus']">
    <xsl:copy>
      <xsl:if test="string-length(.) != 0">
				<xsl:apply-templates select="@*|node()" />
			</xsl:if>
    </xsl:copy>
  </xsl:template>

	<xsl:template match="xhtml:table[@class = 'hierarchical features']//xhtml:tbody/xhtml:tr[xhtml:td[@class = 'minus'][not(text())] and xhtml:td[@class = 'plus'][not(text())]]" />

</xsl:stylesheet>