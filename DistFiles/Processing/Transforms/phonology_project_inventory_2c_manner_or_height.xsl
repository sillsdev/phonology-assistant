<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2c_manner_or_height.xsl 2011-08-22 -->
  <!-- Determine sort order by manner of articulation or height. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <xsl:variable name="chartClass1" select="'rowgroup'" />
  <xsl:variable name="chartClass2" select="'colgroup'" />
  <xsl:variable name="sortKeyClass" select="'manner_or_height'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:param name="sortKey" />
    <xsl:copy>
      <xsl:apply-templates select="@* | node()">
        <xsl:with-param name="sortKey" select="$sortKey" />
      </xsl:apply-templates>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="segments">
		<xsl:variable name="sortKeyFormat" select="translate(count(segment), '0123456789', '0000000000')" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="segment">
				<xsl:sort select="keys/chartKey[@class = $chartClass1][not(@primary = 'false')]/@order" data-type="text" />
				<xsl:sort select="keys/chartKey[@class = $chartClass2][not(@primary = 'false')]/@order" data-type="text" />
				<xsl:sort select="keys/chartKey[@class = 'col']/@order" data-type="text" />
				<xsl:sort select="keys/chartKey[@class = 'rowConditional']/@order" data-type="text" />
				<xsl:sort select="keys/chartKey[@class = 'rowGeneral']/@order" data-type="text" />
				<xsl:sort select="keys/chartKey[@class = 'tone']/@order" data-type="text" />
				<xsl:sort select="keys/chartKey[@class = 'tiebreaker']" data-type="text" />
				<xsl:copy>
          <xsl:apply-templates select="@* | node()">
            <xsl:with-param name="sortKey" select="format-number(position(), $sortKeyFormat)" />
          </xsl:apply-templates>
        </xsl:copy>
      </xsl:for-each>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="segment/keys/sortKey">
    <xsl:param name="sortKey" />
		<xsl:choose>
			<xsl:when test="@class = $sortKeyClass">
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:value-of select="$sortKey" />
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
  </xsl:template>

	<!-- Replace order attribute with numbers according to tones which occur in this inventory. -->
	<xsl:template match="/inventory/tones">
		<xsl:variable name="sortKeyFormat" select="translate(count(feature), '0123456789', '0000000000')" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="feature">
				<xsl:sort select="@order" data-type="text" />
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:attribute name="order">
						<xsl:value-of select="format-number(position(), $sortKeyFormat)" />
					</xsl:attribute>
					<xsl:apply-templates />
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>