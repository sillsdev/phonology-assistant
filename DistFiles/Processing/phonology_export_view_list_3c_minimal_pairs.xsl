<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_3c_minimal_pairs.xsl 2010-05-03 -->
  <!-- If there are minimal pairs, count the number of records and groups. -->

	<!-- Important: In case Phonology Assistant exports collapsed in class attributes, test: -->
	<!-- * xhtml:table[contains(@class, 'list')] instead of @class = 'list' -->
	<!-- * xhtml:tbody[contains(@class, 'group')] instead of @class = 'group' -->

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
					<xsl:value-of select="count(//xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'data'])" />
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
					<xsl:value-of select="count(//xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')])" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
    </xsl:copy>
  </xsl:template>

	<!-- If a list of differences contains at least one feature, copy it. -->
  <xsl:template match="xhtml:ul[@class = 'differences']">
    <xsl:if test="xhtml:li">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

	<!-- Update the count of records in groups. -->
  <xsl:template match="xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'count']">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:choose>
				<!-- One minimal pair per group. -->
				<xsl:when test="../xhtml:th[contains(@class, 'pair')]">
					<xsl:value-of select="count(../../xhtml:tr[@class = 'heading' or @class = 'subheading'])" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="count(../../xhtml:tr[@class = 'data'])" />
				</xsl:otherwise>
			</xsl:choose>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>