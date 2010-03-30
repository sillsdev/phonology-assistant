<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_2c.xsl 2010-03-08 -->
  <!-- If there are no phones or one phone in a data cell, remove the list. -->
  <!-- If any column heading cell contains line breaks, insert a break in the heading cells of the feature tables. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>
  
  <xsl:template match="xhtml:table[@class = 'CV chart']//xhtml:th[. = @title]/@title" />

  <xsl:template match="xhtml:table[@class = 'CV chart']//xhtml:td[@class = 'Phonetic']">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:variable name="numberOfPhones" select="count(xhtml:ul/xhtml:li)" />
      <xsl:choose>
        <xsl:when test="$numberOfPhones = 0" />
        <xsl:when test="$numberOfPhones = 1">
          <xsl:attribute name="title">
            <xsl:value-of select="xhtml:ul/xhtml:li/@title" />
          </xsl:attribute>
          <xsl:apply-templates select="xhtml:ul/xhtml:li/node()" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="xhtml:ul" />
          <xsl:copy-of select="xhtml:ul/xhtml:li[1]/xhtml:div[@class = 'features']" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:table[@class = 'CV chart']//xhtml:td[@class = 'Phonetic']/xhtml:ul[count(xhtml:li) != 1]/xhtml:li/xhtml:div[@class = 'features']" />

  <xsl:template match="xhtml:table[@class = 'articulatory features' or @class = 'binary features' or @class = 'hierarchical features']/xhtml:thead/xhtml:tr/xhtml:th">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <!-- If this heading cell does not contain a break but a chart heading cell does, insert it. -->
      <xsl:if test="not(xhtml:br) and ../../../../xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th/xhtml:br">
        <br xmlns="http://www.w3.org/1999/xhtml" />
      </xsl:if>
      <xsl:apply-templates />
    </xsl:copy>
  </xsl:template>

	<!-- Remove an empty tbody element (that is, if there are no distinguishing features in that class). -->
	<xsl:template match="xhtml:table[@class = 'articulatory features']/xhtml:tbody[not(xhtml:tr)]" />

	<!-- Remove an empty tbody element (that is, if there are no distinguishing features in that class). -->
	<xsl:template match="xhtml:table[@class = 'binary features']/xhtml:tbody[not(xhtml:tr)]" />

	<xsl:template match="xhtml:table[@class = 'binary features']/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'name'] = 'LAB' or xhtml:td[@class = 'name'] = 'COR' or xhtml:td[@class = 'name'] = 'DORS']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<td class="univalent" colspan="3" xmlns="http://www.w3.org/1999/xhtml">
				<div>
					<xsl:apply-templates select="xhtml:td[@class = 'name']/@*" />
					<xsl:value-of select="xhtml:td[@class = 'name']" />
				</div>
			</td>
		</xsl:copy>
	</xsl:template>

	<!-- Remove a row that contains an empty div (that is, if that class node does not occur). -->
	<!-- For example, coronal for vowels. -->
	<xsl:template match="xhtml:table[@class = 'hierarchical features']//xhtml:tr[xhtml:td[@class = 'univalent' and not(xhtml:div)]]" />

</xsl:stylesheet>