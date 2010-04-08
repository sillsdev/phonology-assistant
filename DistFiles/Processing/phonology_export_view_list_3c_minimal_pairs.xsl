<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_3c_minimal_pairs.xsl 2010-04-06 -->
  <!-- If there are minimal pairs, count the number of records and groups. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- If there are minimal pairs, update the total number of records. -->
	<xsl:template match="//xhtml:div[@id = 'metadata']/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfRecords']">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="../xhtml:li[@class = 'minimalPairs']">
					<xsl:value-of select="count(//xhtml:table[@class = 'list']/xhtml:tbody[@class = 'group']/xhtml:tr[@class = 'data'])" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
    </xsl:copy>
  </xsl:template>

	<!-- If there are minimal pairs, update the total number of groups. -->
	<xsl:template match="//xhtml:div[@id = 'metadata']/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfGroups']">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="../xhtml:li[@class = 'minimalPairs']">
					<xsl:value-of select="count(//xhtml:table[@class = 'list']/xhtml:tbody[@class = 'group'])" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
    </xsl:copy>
  </xsl:template>

	<!-- If a list contains at least one feature, copy the div containing feature differences. -->
  <xsl:template match="xhtml:table[@class = 'list']/xhtml:tbody[@class = 'group']/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']/xhtml:ul/xhtml:li/xhtml:div[@class = 'differences']">
    <xsl:if test="xhtml:ul[xhtml:li]">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

	<!-- If a list of differences contains at least one feature, copy it. -->
  <xsl:template match="xhtml:div[@class = 'differences']/xhtml:ul">
    <xsl:if test="xhtml:li">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

	<!-- If not one minimal pair per group, update the count of records. -->
  <xsl:template match="xhtml:table[@class = 'list']/xhtml:tbody[@class = 'group']/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'count']">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:value-of select="count(../../xhtml:tr[@class = 'data'])" />
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>