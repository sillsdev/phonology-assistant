<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_1b_minimal_pairs_split.xsl 2010-04-05 -->
  <!-- If there are minimal pairs, optionally split groups into all combinations of pairs. -->
	<!-- TO DO: Correct for Both Environments Identical, but must adapt enumeration for Before or After. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="minimalPairs" select="$details/xhtml:li[@class = 'minimalPairs']" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="oneMinimalPairPerGroup" select="$options/xhtml:li[@class = 'oneMinimalPairPerGroup']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Apply or ignore transformations. -->
	<xsl:template match="xhtml:table[@class = 'list']">
		<xsl:choose>
			<xsl:when test="$minimalPairs and $oneMinimalPairPerGroup = 'true'">
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

  <xsl:template match="xhtml:table[@class = 'list']/xhtml:tbody[@class = 'group']">
    <xsl:apply-templates select="xhtml:tr[@class = 'data'][1]" mode="enumerate1" />
  </xsl:template>

  <xsl:template match="xhtml:tr" mode="enumerate1">
    <xsl:variable name="PhoneticItem1" select="xhtml:td[@class = 'Phonetic item']" />
		<xsl:apply-templates select="following-sibling::xhtml:tr[xhtml:td[@class = 'Phonetic item'] != $PhoneticItem1][1]" mode="enumerate2">
			<xsl:with-param name="PhoneticItem1" select="$PhoneticItem1" />
		</xsl:apply-templates>
		<xsl:apply-templates select="following-sibling::xhtml:tr[xhtml:td[@class = 'Phonetic item'] != $PhoneticItem1][1]" mode="enumerate1" />
	</xsl:template>

  <xsl:template match="xhtml:tr" mode="enumerate2">
    <xsl:param name="PhoneticItem1" />
    <xsl:variable name="PhoneticItem2" select="xhtml:td[@class = 'Phonetic item']" />
    <xsl:apply-templates select="ancestor::xhtml:tbody[@class = 'group']" mode="minimalPair">
      <xsl:with-param name="PhoneticItem1" select="$PhoneticItem1" />
      <xsl:with-param name="PhoneticItem2" select="$PhoneticItem2" />
    </xsl:apply-templates>
		<xsl:apply-templates select="following-sibling::xhtml:tr[xhtml:td[@class = 'Phonetic item'] != $PhoneticItem2][1]" mode="enumerate2">
			<xsl:with-param name="PhoneticItem1" select="$PhoneticItem1" />
		</xsl:apply-templates>
	</xsl:template>

  <xsl:template match="xhtml:tbody" mode="minimalPair">
    <xsl:param name="PhoneticItem1" />
    <xsl:param name="PhoneticItem2" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<!-- In the group heading, replace count (that is, number of records) with the pair of units. -->
			<tr class="heading" xmlns="http://www.w3.org/1999/xhtml">
				<th class="Phonetic pair">
					<ul>
						<li>
							<span>
								<xsl:value-of select="$PhoneticItem1" />
							</span>
						</li>
						<li>
							<span>
								<xsl:value-of select="$PhoneticItem2" />
							</span>
						</li>
					</ul>
				</th>
				<xsl:apply-templates select="xhtml:tr[@class = 'heading']/xhtml:th[not(@class = 'count')]" />
			</tr>
      <xsl:apply-templates select="xhtml:tr[@class = 'data'][xhtml:td[@class = 'Phonetic item'] = $PhoneticItem1][1]" />
      <xsl:apply-templates select="xhtml:tr[@class = 'data'][xhtml:td[@class = 'Phonetic item'] = $PhoneticItem2][1]" />
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>