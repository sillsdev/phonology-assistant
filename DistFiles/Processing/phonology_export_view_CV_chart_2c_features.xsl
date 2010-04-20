<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_2c_features.xsl 2010-04-20 -->
	<!-- Remove empty tbody elements (that is, if they contains no features that distinguish units). -->
	<!-- In hierarchical features table: Add colgroup elements and colspan attributes. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
		<xsl:param name="colspanTotal" />
    <xsl:copy>
			<xsl:apply-templates select="@* | node()">
				<xsl:with-param name="colspanTotal" select="$colspanTotal" />
			</xsl:apply-templates>
    </xsl:copy>
  </xsl:template>

	<!-- Remove empty tbody elements (that is, if they contains no features that distinguish units). -->
	<xsl:template match="xhtml:table[contains(@class, 'features')]/xhtml:tbody[not(xhtml:tr)]" />

	<xsl:template match="xhtml:table[@class = 'hierarchical features']">
		<xsl:variable name="colspanUnivalentMax">
			<xsl:for-each select="xhtml:tbody/xhtml:tr[@class = 'univalent'][xhtml:td/@colspan]">
				<xsl:sort select="xhtml:td/@colspan" order="descending" data-type="number" />
				<xsl:if test="position() = 1">
					<xsl:value-of select="number(xhtml:td/@colspan) + 1" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="colspanBivalentMax">
			<xsl:for-each select="xhtml:tbody/xhtml:tr[@class = 'bivalent'][xhtml:td/@colspan]">
				<xsl:sort select="xhtml:td/@colspan" order="descending" data-type="number" />
				<xsl:if test="position() = 1">
					<xsl:value-of select="number(xhtml:td/@colspan) + 3" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="colspanTotal">
			<xsl:choose>
				<xsl:when test="$colspanUnivalentMax &gt; $colspanBivalentMax">
					<xsl:value-of select="$colspanUnivalentMax" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$colspanBivalentMax" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:call-template name="colgroup">
				<xsl:with-param name="colspanTotal" select="$colspanTotal" />
			</xsl:call-template>
			<colgroup xmlns="http://www.w3.org/1999/xhtml">
				<col />
			</colgroup>
			<xsl:apply-templates>
				<xsl:with-param name="colspanTotal" select="$colspanTotal" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="colgroup">
		<xsl:param name="colspanTotal" select="1" />
		<xsl:if test="$colspanTotal &gt; 1">
			<colgroup xmlns="http://www.w3.org/1999/xhtml">
				<col />
				<col />
			</colgroup>
			<xsl:call-template name="colgroup">
				<xsl:with-param name="colspanTotal" select="$colspanTotal - 2" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'hierarchical features']/xhtml:thead/xhtml:tr/xhtml:th">
		<xsl:param name="colspanTotal" select="1" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="colspan">
				<xsl:value-of select="$colspanTotal" />
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'hierarchical features']/xhtml:tbody/xhtml:tr/xhtml:td[@class = 'name']">
		<xsl:param name="colspanTotal" select="1" />
		<xsl:variable name="colspanPreceding">
			<xsl:choose>
				<xsl:when test="preceding-sibling::xhtml:td[@colspan]">
					<xsl:value-of select="number(preceding-sibling::xhtml:td[@colspan]/@colspan)" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="colspan">
			<xsl:choose>
				<xsl:when test="../@class = 'univalent'">
					<xsl:value-of select="$colspanTotal - $colspanPreceding" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="($colspanTotal - $colspanPreceding) - 2" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="colspan">
				<xsl:value-of select="$colspan" />
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Replace colspan with corresponding number of empty cells. -->
	<xsl:template match="xhtml:table[@class = 'hierarchical features']/xhtml:tbody/xhtml:tr/xhtml:td[1][@colspan]">
		<xsl:call-template name="colspan">
			<xsl:with-param name="colspan" select="@colspan" />
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="colspan">
		<xsl:param name="colspan" select="0" />
		<xsl:if test="$colspan &gt; 0">
			<td xmlns="http://www.w3.org/1999/xhtml" />
			<xsl:call-template name="colspan">
				<xsl:with-param name="colspan" select="$colspan - 1" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>