<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_3b_minimal_pairs_count.xsl 2011-11-04 -->
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
	<xsl:template match="//xhtml:div[@id = 'metadata']/xhtml:ul[@class = 'details'][xhtml:li[@class = 'pairs']]/xhtml:li[@class = 'number record']">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:value-of select="count(//xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'data'])" />
		</xsl:copy>
  </xsl:template>

	<!-- If there are minimal pairs, update the total number of groups. -->
	<xsl:template match="//xhtml:div[@id = 'metadata']/xhtml:ul[@class = 'details'][xhtml:li[@class = 'pairs']]/xhtml:li[@class = 'number group']">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:value-of select="count(//xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')])" />
		</xsl:copy>
		<li class="number pairs more-similar" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="count(//xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')][xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair more-similar')]])" />
		</li>
		<li class="number pairs less-similar" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="count(//xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')][xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair less-similar')]])" />
		</li>
		<li class="number pairs least-similar" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="count(//xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')][xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair least-similar')]])" />
		</li>
	</xsl:template>

	<!-- Update the count of records in groups of minimal pairs. -->
  <xsl:template match="xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading'][xhtml:th[starts-with(@class, 'Phonetic pair')]]/xhtml:th[@class = 'count']">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:choose>
				<!-- A more-similar pair that does not occur in the data. -->
				<xsl:when test="count(../../xhtml:tr[@class = 'data']) = 0">
					<xsl:value-of select="0" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="count(../../xhtml:tr[@class = 'heading' or @class = 'subheading'])" />
				</xsl:otherwise>
			</xsl:choose>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>