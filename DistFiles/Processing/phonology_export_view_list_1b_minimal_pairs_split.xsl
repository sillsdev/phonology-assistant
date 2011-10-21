<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_1b_minimal_pairs_split.xsl 2011-10-21 -->
  <!-- If there are minimal pairs, optionally split groups into all combinations of pairs. -->

	<!-- Important: If table is not Search view, copy it with no changes. -->

	<!-- Important: In case Phonology Assistant exports collapsed in class attributes, test: -->
	<!-- * xhtml:table[contains(@class, 'list')] instead of @class = 'list' -->
	<!-- * xhtml:tbody[contains(@class, 'group')] instead of @class = 'group' -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="minimalPairs" select="$details/xhtml:li[@class = 'minimalPairs']" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="oneMinimalPairPerGroup" select="$options/xhtml:li[@class = 'oneMinimalPairPerGroup']" />

	<xsl:variable name="languageCode3" select="$details/xhtml:li[@class = 'languageCode']" />
	<xsl:variable name="languageCode1">
		<xsl:if test="string-length($languageCode3) != 0">
			<xsl:value-of select="document('ISO_639.xml')//xhtml:tr[xhtml:td[@class = 'ISO_639-3'] = $languageCode3]/xhtml:td[@class = 'ISO_639-1']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="languageCode">
		<xsl:choose>
			<xsl:when test="string-length($languageCode1) = 2">
				<xsl:value-of select="$languageCode1" />
			</xsl:when>
			<xsl:when test="string-length($languageCode3) != 0">
				<xsl:value-of select="$languageCode3" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'und'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="langPhonetic">
		<xsl:value-of select="$languageCode" />
		<xsl:value-of select="'-fonipa'" />
	</xsl:variable>

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Apply or ignore transformations. -->
	<xsl:template match="xhtml:table">
		<xsl:choose>
			<xsl:when test="contains(@class, 'list') and $minimalPairs and $oneMinimalPairPerGroup = 'true'">
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<!-- To ignore the following rules, copy instead of apply-templates. -->
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- In the first colgroup element, insert another col corresponding to the Phonetic pair column. -->
	<xsl:template match="xhtml:table/xhtml:colgroup[1]">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<col xmlns="http://www.w3.org/1999/xhtml" />
		</xsl:copy>
	</xsl:template>

	<!-- In group heading cell at the upper left, insert colspan attribute -->
	<!-- corresponding to the Phonetic pair and count columns. -->
	<xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[@class = 'group']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="colspan">
				<xsl:value-of select="2" />
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:tbody[contains(@class, 'group')]">
    <xsl:apply-templates select="xhtml:tr[@class = 'data'][1]" mode="enumerate1" />
  </xsl:template>

	<!-- Enumerate the first segment of pairs. -->
	<!-- That is, all segments in the group, except the last (but excluding any duplicate segments). -->
	<xsl:template match="xhtml:tr" mode="enumerate1">
		<xsl:variable name="PhoneticItem1" select="xhtml:td[@class = 'Phonetic item']" />
		<!-- Especially for contrast in analogous environments (that is, not Both Environments Identical), -->
		<!-- there might be multiple non-adjacent occurrences of segments. -->
		<xsl:if test="not(preceding-sibling::xhtml:tr[xhtml:td[@class = 'Phonetic item'] = $PhoneticItem1])">
			<xsl:apply-templates select="following-sibling::xhtml:tr[xhtml:td[@class = 'Phonetic item'] != $PhoneticItem1][1]" mode="enumerate2">
				<xsl:with-param name="PhoneticItem1" select="$PhoneticItem1" />
			</xsl:apply-templates>
		</xsl:if>
		<xsl:apply-templates select="following-sibling::xhtml:tr[xhtml:td[@class = 'Phonetic item'] != $PhoneticItem1][1]" mode="enumerate1" />
	</xsl:template>

	<!-- Enumerate the second segment of pairs. -->
	<!-- That is, all segments in the group following the first segment (but excluding any duplicate segments). -->
  <xsl:template match="xhtml:tr" mode="enumerate2">
    <xsl:param name="PhoneticItem1" />
    <xsl:variable name="PhoneticItem2" select="xhtml:td[@class = 'Phonetic item']" />
		<!-- Especially for contrast in analogous environments (that is, not Both Environments Identical), -->
		<!-- there might be multiple non-adjacent occurrences of segments. -->
		<xsl:if test="not(preceding-sibling::xhtml:tr[xhtml:td[@class = 'Phonetic item'] = $PhoneticItem2])">
			<xsl:apply-templates select="ancestor::xhtml:tbody[contains(@class, 'group')]" mode="minimalPair">
				<xsl:with-param name="PhoneticItem1" select="$PhoneticItem1" />
				<xsl:with-param name="PhoneticItem2" select="$PhoneticItem2" />
			</xsl:apply-templates>
		</xsl:if>
		<xsl:apply-templates select="following-sibling::xhtml:tr[xhtml:td[@class = 'Phonetic item'] != $PhoneticItem1][xhtml:td[@class = 'Phonetic item'] != $PhoneticItem2][1]" mode="enumerate2">
			<xsl:with-param name="PhoneticItem1" select="$PhoneticItem1" />
		</xsl:apply-templates>
	</xsl:template>

  <xsl:template match="xhtml:tbody" mode="minimalPair">
    <xsl:param name="PhoneticItem1" />
    <xsl:param name="PhoneticItem2" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<!-- In the group heading, insert the pair of segments following the count (that is, number of records). -->
			<tr class="heading" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:apply-templates select="xhtml:tr[@class = 'heading']/xhtml:th[@class = 'count']" />
				<th class="Phonetic pair">
					<ul>
						<li lang="{$langPhonetic}">
							<xsl:value-of select="$PhoneticItem1" />
						</li>
						<li lang="{$langPhonetic}">
							<xsl:value-of select="$PhoneticItem2" />
						</li>
					</ul>
				</th>
				<xsl:apply-templates select="xhtml:tr[@class = 'heading']/xhtml:th[not(@class = 'count')]" />
			</tr>
      <xsl:apply-templates select="xhtml:tr[@class = 'data'][xhtml:td[@class = 'Phonetic item'] = $PhoneticItem1]" />
      <xsl:apply-templates select="xhtml:tr[@class = 'data'][xhtml:td[@class = 'Phonetic item'] = $PhoneticItem2]" />
    </xsl:copy>
  </xsl:template>

	<!-- In the data cell at the left, insert colspan attribute -->
	<!-- corresponding to the Phonetic pair and count columns. -->
	<xsl:template match="xhtml:tbody/xhtml:tr[@class = 'data']/xhtml:td[1]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="colspan">
				<xsl:value-of select="2" />
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>