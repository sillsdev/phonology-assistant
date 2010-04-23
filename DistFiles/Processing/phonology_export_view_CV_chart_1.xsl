<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_1.xsl 2010-04-22 -->
	<!-- Export to XHTML: title attribute of phonetic cell contains description of unit. -->
  <!-- In several column heading cells, optionally insert hyphens and line breaks. -->
	<!-- In two-word row heading cell, change articulatory feature to sentence case. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />

	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="hyphenateColumnHeadings" select="$options/xhtml:li[@class = 'hyphenatedColumnHeadings']" />

	<xsl:variable name="typeOfUnits">
		<xsl:choose>
			<xsl:when test="string-length($details/xhtml:li[@class = 'typeOfUnits']) != 0">
				<xsl:value-of select="$details/xhtml:li[@class = 'typeOfUnits']" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'phonetic'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<!-- A project phonetic inventory file contains features of phonetic or phonological units, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="units" select="document($projectPhoneticInventoryXML)/inventory/units[@type = $typeOfUnits]" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Export to XHTML: title attribute of phonetic cell contains description of unit. -->
	<xsl:template match="xhtml:table[@class = 'CV chart']//xhtml:td[@class = 'Phonetic'][node()]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="$format = 'XHTML'">
				<xsl:variable name="literal">
					<xsl:choose>
						<xsl:when test="xhtml:span">
							<xsl:value-of select="xhtml:span" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="." />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="description" select="$units/unit[@literal = $literal]/description" />
				<xsl:if test="string-length($description) != 0">
					<xsl:attribute name="title">
						<xsl:value-of select="$description" />
					</xsl:attribute>
				</xsl:if>
			</xsl:if>
			<xsl:apply-templates />
    </xsl:copy>
  </xsl:template>

	<!-- In the following column heading cells, optionally insert hyphens and line breaks. -->
	<!-- The title attribute consists of the articulatory feature in square brackets. -->

	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[. = 'Labiodental']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$hyphenateColumnHeadings = 'true'">
					<xsl:attribute name="title">
						<xsl:value-of select="concat('[', ., ']')" />
					</xsl:attribute>
					<xsl:value-of select="'Labio-'" />
					<br xmlns="http://www.w3.org/1999/xhtml" />
					<xsl:value-of select="'dental'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[. = 'Linguolabial']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$hyphenateColumnHeadings = 'true'">
					<xsl:attribute name="title">
						<xsl:value-of select="concat('[', ., ']')" />
					</xsl:attribute>
					<xsl:value-of select="'Linguo-'" />
					<br xmlns="http://www.w3.org/1999/xhtml" />
					<xsl:value-of select="'labial'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[. = 'Postalveolar']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$hyphenateColumnHeadings = 'true'">
					<xsl:attribute name="title">
						<xsl:value-of select="concat('[', ., ']')" />
					</xsl:attribute>
					<xsl:value-of select="'Post-'" />
					<br xmlns="http://www.w3.org/1999/xhtml" />
					<xsl:value-of select="'alveolar'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[. = 'Retroflex']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$hyphenateColumnHeadings = 'true'">
					<xsl:attribute name="title">
						<xsl:value-of select="concat('[', ., ']')" />
					</xsl:attribute>
					<xsl:value-of select="'Retro-'" />
					<br xmlns="http://www.w3.org/1999/xhtml" />
					<xsl:value-of select="'flex'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[. = 'Pharyngeal']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$hyphenateColumnHeadings = 'true'">
					<xsl:attribute name="title">
						<xsl:value-of select="concat('[', ., ']')" />
					</xsl:attribute>
					<xsl:value-of select="'Pharyn-'" />
					<br xmlns="http://www.w3.org/1999/xhtml" />
					<xsl:value-of select="'geal'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[. = 'Epiglottal']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$hyphenateColumnHeadings = 'true'">
					<xsl:attribute name="title">
						<xsl:value-of select="concat('[', ., ']')" />
					</xsl:attribute>
					<xsl:value-of select="'Epi-'" />
					<br xmlns="http://www.w3.org/1999/xhtml" />
					<xsl:value-of select="'glottal'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<!-- Some articulatory features are hyphenated: Alveolo-palatal, Near-front, Near-back. -->
	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[contains(., '-')]">
		<xsl:variable name="before" select="substring-before(., ' ')" />
		<xsl:variable name="after" select="substring-after(., ' ')" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$hyphenateColumnHeadings = 'true'">
					<xsl:attribute name="title">
						<xsl:value-of select="concat('[', ., ']')" />
					</xsl:attribute>
					<xsl:value-of select="concat($before, '-')" />
					<br xmlns="http://www.w3.org/1999/xhtml" />
					<xsl:value-of select="$after" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[. = 'Front or Near-front']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="title">
				<xsl:value-of select="'{[Front],[Near-front]}'" />
			</xsl:attribute>
			<xsl:value-of select="'Front&#xA0;or'" />
			<br xmlns="http://www.w3.org/1999/xhtml" />
			<xsl:value-of select="'Near-front'" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:thead/xhtml:tr/xhtml:th[. = 'Back or Near-back']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="title">
				<xsl:value-of select="'{[Back],[Near-back]}'" />
			</xsl:attribute>
			<xsl:value-of select="'Back&#xA0;or'" />
			<br xmlns="http://www.w3.org/1999/xhtml" />
			<xsl:value-of select="'Near-back'" />
		</xsl:copy>
	</xsl:template>

	<!-- In two-word row heading cell, change articulatory feature to sentence case. -->
	<!-- For example, change Lateral Approximant to Lateral approximant. -->
	<xsl:template match="xhtml:table[@class = 'CV chart']/xhtml:tbody/xhtml:tr/xhtml:th[contains(., ' ')]">
		<xsl:variable name="before" select="substring-before(., ' ')" />
		<xsl:variable name="after" select="translate(substring-after(., ' '), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')" />
		<xsl:variable name="text" select="concat($before, ' ', $after)" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="title">
				<xsl:value-of select="concat('[', ., ']')" />
			</xsl:attribute>
			<xsl:value-of select="$text" />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>