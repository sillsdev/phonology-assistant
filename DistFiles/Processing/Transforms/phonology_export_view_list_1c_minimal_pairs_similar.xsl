<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_1c_minimal_pairs_similar.xsl 2011-11-02 -->
  <!-- If the project inventory contains lists of similar pairs, -->
  <!-- classify the list of minimal pairs as more-similar, less-similar, least-similar. -->
	<!-- Append empty groups for any more-similar pairs for which there are no minimal pairs. -->

	<!-- Important: In case Phonology Assistant exports collapsed in class attributes, test: -->
	<!-- * xhtml:table[contains(@class, 'list')] instead of @class = 'list' -->
	<!-- * xhtml:tbody[contains(@class, 'group')] instead of @class = 'group' -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />

	<!-- A project phonetic inventory file contains features of phonetic or phonological segments, or both. -->
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="similarPairs" select="$options/xhtml:li[@class = 'similarPairs']" />

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="searchPattern" select="$details/xhtml:li[@class = 'searchPattern']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@* | node()" mode="pairs">
		<xsl:param name="pairs" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="pairs">
				<xsl:with-param name="pairs" select="$pairs" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[contains(@class, 'list')][xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']]">
		<xsl:variable name="searchItem" select="substring-before($searchPattern, '/')" />
		<xsl:variable name="type">
			<xsl:choose>
				<xsl:when test="$searchItem = '[C]'">
					<xsl:value-of select="'consonant'" />
				</xsl:when>
				<xsl:when test="$searchItem = '[V]'">
					<xsl:value-of select="'vowel'" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="pairs" select="document($projectPhoneticInventoryXML)/inventory/pairs[@type = $type]" />
		<xsl:choose>
			<xsl:when test="$pairs/pair">
				<xsl:variable name="table" select="." />
				<xsl:variable name="colspan" select="count(xhtml:colgroup/xhtml:col) - 2" />
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" mode="pairs">
						<xsl:with-param name="pairs" select="$pairs" />
					</xsl:apply-templates>
					<!-- For any more-similar pair of segments for which there are no minimal pairs, append an empty group. -->
					<xsl:for-each select="$pairs/pair[@similarity = 'more']">
						<xsl:variable name="literal1" select="segment[1]/@literal" />
						<xsl:variable name="literal2" select="segment[2]/@literal" />
						<xsl:choose>
							<xsl:when test="$table/xhtml:tbody/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair'][xhtml:ul/xhtml:li[1] = $literal1 and xhtml:ul/xhtml:li[2] = $literal2]" />
							<xsl:when test="$table/xhtml:tbody/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair'][xhtml:ul/xhtml:li[1] = $literal2 and xhtml:ul/xhtml:li[2] = $literal1]" />
							<xsl:otherwise>
								<tbody class="group" xmlns="http://www.w3.org/1999/xhtml">
									<tr class="heading">
										<th class="count">
											<xsl:value-of select="0" />
										</th>
										<th class="Phonetic pair more-similar">
											<ul>
												<li>
													<xsl:value-of select="$literal1" />
												</li>
												<li>
													<xsl:value-of select="$literal2" />
												</li>
											</ul>
										</th>
										<th colspan="{$colspan}" />
									</tr>
								</tbody>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<!-- To ignore all of the following rules, copy instead of apply templates. -->
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="xhtml:table[contains(@class, 'list')]/xhtml:tbody/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']/@class" mode="pairs">
		<xsl:param name="pairs" />
		<xsl:variable name="ul" select="../xhtml:ul" />
		<xsl:variable name="literal1" select="$ul/xhtml:li[1]" />
		<xsl:variable name="literal2" select="$ul/xhtml:li[2]" />
		<xsl:variable name="pair" select="$pairs/pair[(segment[1]/@literal = $literal1 and segment[2]/@literal = $literal2) or (segment[1]/@literal = $literal2 and segment[2]/@literal = $literal1)][@similarity]" />
		<xsl:attribute name="class">
			<xsl:value-of select="." />
			<xsl:value-of select="' '" />
			<xsl:choose>
				<xsl:when test="$pair">
					<xsl:value-of select="$pair/@similarity" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'least'" />
				</xsl:otherwise>
			</xsl:choose>
			<xsl:value-of select="'-similar'" />
		</xsl:attribute>
	</xsl:template>

</xsl:stylesheet>