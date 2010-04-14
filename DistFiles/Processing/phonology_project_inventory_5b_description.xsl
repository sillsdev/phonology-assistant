<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<!-- phonology_project_inventory_5b_description.xsl 2010-04-09 -->

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="unit/description">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="*">
				<xsl:variable name="subclass" select="@subclass" />
				<xsl:choose>
					<xsl:when test="position() = 1" />
					<xsl:when test="@subclass = 'placeOfArticulation' and preceding-sibling::*[1][self::feature][@subclass = $subclass]">
						<xsl:value-of select="'-'" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="' '" />
					</xsl:otherwise>
				</xsl:choose>
				<xsl:value-of select="translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxya')" />
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>