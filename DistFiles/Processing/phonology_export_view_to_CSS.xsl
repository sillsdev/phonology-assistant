<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_to_CSS.xsl 2010-04-01 -->
  <!-- Convert project-specific formatting to an external CSS file for exported XHTML files. -->

	<xsl:output method="text" version="1.0" encoding="UTF-8" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="projectName" select="$details/xhtml:li[@class = 'projectName']" />
	<xsl:variable name="languageName" select="$details/xhtml:li[@class = 'languageName']" />
	<xsl:variable name="languageCode" select="$details/xhtml:li[@class = 'languageCode']" />
	<xsl:variable name="formatting" select="$metadata/xhtml:table[@class = 'formatting']" />

	<xsl:template match="/">
		<xsl:if test="string-length($projectName) != 0">
			<xsl:value-of select="concat('/* Project name: ', $projectName, ' */&#xA;')" />
		</xsl:if>
		<xsl:if test="string-length($languageName) != 0">
			<xsl:value-of select="concat('/* Language name: ', $languageName, ' */&#xA;')" />
		</xsl:if>
		<xsl:if test="string-length($languageCode) != 0">
			<xsl:value-of select="concat('/* ISO 639-3 code: ', $languageCode, ' */&#xA;')" />
		</xsl:if>
		<xsl:for-each select="$formatting/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'class']/text()]">
			<xsl:value-of select="concat('.', xhtml:td[@class = 'class']/text(), '&#xA;{&#xA;')" />
			<xsl:if test="xhtml:td[@class = 'font-family']/text()">
				<xsl:variable name="font-family" select="xhtml:td[@class = 'font-family']/text()" />
				<xsl:choose>
					<xsl:when test="contains($font-family, ' ')">
						<xsl:value-of select="concat('font-family: &quot;', $font-family, '&quot;;&#xA;')" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat('font-family: ', $font-family, ';&#xA;')" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
			<xsl:if test="xhtml:td[@class = 'font-size']/text()">
				<xsl:variable name="font-size" select="xhtml:td[@class = 'font-size']/text()" />
				<xsl:value-of select="concat('font-size: ', format-number(100 * $font-size div 12, '.00'), '%; /* ', $font-size, 'pt */&#xA;')" />
			</xsl:if>
			<xsl:if test="xhtml:td[@class = 'font-weight']/text()">
				<xsl:value-of select="concat('font-weight: ', xhtml:td[@class = 'font-weight']/text(), ';&#xA;')" />
			</xsl:if>
			<xsl:if test="xhtml:td[@class = 'font-style']/text()">
				<xsl:value-of select="concat('font-style: ', xhtml:td[@class = 'font-style']/text(), ';&#xA;')" />
			</xsl:if>
			<xsl:value-of select="'}&#xA;'" />
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>