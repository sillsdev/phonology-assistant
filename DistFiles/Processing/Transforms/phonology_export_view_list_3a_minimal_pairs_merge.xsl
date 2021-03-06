﻿<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_3a_minimal_pairs_merge.xsl 2011-09-28 -->
  <!-- Merge groups corresponding to the same pair of phones. -->
	<!-- This step must follow the list sorting steps, because the order of pairs depends on -->
	<!-- phonetic sort option (that is, place_or_backness versus manner_or_height). -->

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

	<!-- Copy the first group for each minimal pair. -->
	<xsl:template match="xhtml:table[contains(@class, 'list')]/xhtml:tbody[contains(@class, 'group')][xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')]]">
		<xsl:variable name="ul" select="xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')]/xhtml:ul" />
		<xsl:variable name="literal1" select="$ul/xhtml:li[1]" />
		<xsl:variable name="literal2" select="$ul/xhtml:li[2]" />
		<xsl:if test="not(preceding-sibling::xhtml:tbody[1][xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')]/xhtml:ul/xhtml:li[1] = $literal1 and xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')]/xhtml:ul/xhtml:li[2] = $literal2])">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
				<xsl:apply-templates select="following-sibling::xhtml:tbody[xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')]/xhtml:ul/xhtml:li[1] = $literal1 and xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')]/xhtml:ul/xhtml:li[2] = $literal2]" mode="merge" />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

	<!-- Merge any additional groups for a minimal pair. -->
	<xsl:template match="xhtml:tbody[contains(@class, 'group')]" mode="merge">
		<!-- Change the heading row to a subheading. -->
		<!-- Change the phonetic pair and count heading cells to an empty data cell. -->
		<tr class="subheading" xmlns="http://www.w3.org/1999/xhtml">
			<td colspan="2" />
			<xsl:apply-templates select="xhtml:tr[@class = 'heading']/xhtml:th[not(starts-with(@class, 'Phonetic pair') or @class = 'count')]" />
		</tr>
		<xsl:apply-templates select="xhtml:tr[@class = 'data']" />
	</xsl:template>

</xsl:stylesheet>