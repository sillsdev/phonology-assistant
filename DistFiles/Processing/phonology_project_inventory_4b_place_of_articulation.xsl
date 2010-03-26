<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_4b_place_of_articulation.xsl 2010-03-22 -->
  <!-- Determine sort order by place of articulation. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <xsl:variable name="chartClass1" select="'colgroup'" />
  <xsl:variable name="chartClass2" select="'rowgroup'" />
  <xsl:variable name="sortKeyClass" select="'placeOfArticulation'" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:param name="sortKey" />
    <xsl:copy>
      <xsl:apply-templates select="@*|node()">
        <xsl:with-param name="sortKey" select="$sortKey" />
      </xsl:apply-templates>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="units">
    <xsl:variable name="sortKeyFormat" select="translate(count(unit), '0123456789', '0000000000')" />
    <xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="unit">
				<xsl:sort select="keys/chartKey[@class = $chartClass1]" />
				<xsl:sort select="keys/chartKey[@class = $chartClass2]" />
				<xsl:sort select="keys/chartKey[@class = 'col']" />
				<xsl:sort select="keys/chartKey[@class = 'row']" />
				<xsl:copy>
					<xsl:apply-templates select="@*|node()">
						<xsl:with-param name="sortKey" select="format-number(position(), $sortKeyFormat)" />
					</xsl:apply-templates>
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
  </xsl:template>

  <xsl:template match="unit/keys/sortKey">
    <xsl:param name="sortKey" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:choose>
        <xsl:when test="@class = $sortKeyClass">
          <xsl:value-of select="$sortKey" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>