<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_1a_minimal_pairs_omit.xsl 2010-04-14 -->
  <!-- If there are minimal pairs, omit groups that do not contain contrasting phones. -->

	<!-- Important: If table is not Search view, copy it with no changes. -->

	<!-- Important: In case Phonology Assistant exports collapsed in class attributes, test: -->
	<!-- * xhtml:table[contains(@class, 'list')] instead of @class = 'list' -->
	<!-- * xhtml:tbody[contains(@class, 'group')] instead of @class = 'group' -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="minimalPairs" select="$details/xhtml:li[@class = 'minimalPairs']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Apply or ignore transformations. -->
	<xsl:template match="xhtml:table">
		<xsl:choose>
			<xsl:when test="contains(@class, 'list') and $minimalPairs">
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<!-- To ignore the following rules, copy instead of apply-templates. -->
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Remove any groups in which the phonetic item is always the same. -->
  <xsl:template match="xhtml:tbody[contains(@class, 'group')]">
    <xsl:variable name="PhoneticItem1" select="xhtml:tr[@class = 'data'][1]/xhtml:td[@class = 'Phonetic item']" />
    <xsl:if test="xhtml:tr[@class = 'data']/xhtml:td[@class = 'Phonetic item'] != $PhoneticItem1">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>