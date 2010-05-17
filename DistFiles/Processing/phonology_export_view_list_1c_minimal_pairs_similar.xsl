<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_1c_minimal_pairs_similar.xsl 2010-05-14 -->
  <!-- If the project inventory contains lists of similar pairs, -->
  <!-- partition the list of minimal pairs into more-similar and less-similar. -->
	<!-- Append empty groups for any similar pairs for which there are no minimal pairs. -->

	<!-- Important: In case Phonology Assistant exports collapsed in class attributes, test: -->
	<!-- * xhtml:table[contains(@class, 'list')] instead of @class = 'list' -->
	<!-- * xhtml:tbody[contains(@class, 'group')] instead of @class = 'group' -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />

	<!-- A project phonetic inventory file contains features of phonetic or phonological units, or both. -->
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="moreSimilarVersusLessSimilarPairs" select="$options/xhtml:li[@class = 'moreSimilarVersusLessSimilarPairs']" />

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="searchPattern" select="$details/xhtml:li[@class = 'searchPattern']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[contains(@class, 'list')][xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']]">
		<xsl:variable name="searchItem" select="substring-before($searchPattern, '/')" />
		<xsl:variable name="type">
			<xsl:choose>
				<xsl:when test="$searchItem = '[C]'">
					<xsl:value-of select="'Consonant'" />
				</xsl:when>
				<xsl:when test="$searchItem = '[V]'">
					<xsl:value-of select="'Vowel'" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="similarPhonePairs" select="document($projectPhoneticInventoryXML)/inventory/similarPhonePairs" />
		<xsl:choose>
			<xsl:when test="count($similarPhonePairs/pair[@type = $type]) != 0">
				<xsl:variable name="table" select="." />
				<xsl:variable name="colspan" select="count(xhtml:colgroup/xhtml:col) - 2" />
				<div class="report" xmlns="http://www.w3.org/1999/xhtml">
					<h4>More-similar pairs</h4>
					<xsl:copy>
						<xsl:apply-templates select="@* | node()" mode="similarPhonePairs">
							<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
							<xsl:with-param name="moreSimilar" select="'true'" />
						</xsl:apply-templates>
						<!-- For any similar pair of phones for which there are no minimal pairs, append an empty group. -->
						<xsl:for-each select="$similarPhonePairs/pair[@type = $type]">
							<xsl:variable name="literal1" select="phone[1]/@literal" />
							<xsl:variable name="literal2" select="phone[2]/@literal" />
							<xsl:choose>
								<xsl:when test="$table/xhtml:tbody/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair'][xhtml:ul/xhtml:li[1]/xhtml:span = $literal1 and xhtml:ul/xhtml:li[2]/xhtml:span = $literal2]" />
								<xsl:when test="$table/xhtml:tbody/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair'][xhtml:ul/xhtml:li[1]/xhtml:span = $literal2 and xhtml:ul/xhtml:li[2]/xhtml:span = $literal1]" />
								<xsl:otherwise>
									<tbody class="group">
										<tr class="heading">
											<th class="count">
												<xsl:value-of select="0" />
											</th>
											<th class="Phonetic pair">
												<ul>
													<li>
														<span>
															<xsl:value-of select="$literal1" />
														</span>
													</li>
													<li>
														<span>
															<xsl:value-of select="$literal2" />
														</span>
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
				</div>
				<div class="report" xmlns="http://www.w3.org/1999/xhtml">
					<h4>Less-similar pairs</h4>
					<xsl:copy>
						<xsl:apply-templates select="@* | node()" mode="similarPhonePairs">
							<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
							<xsl:with-param name="moreSimilar" select="'false'" />
						</xsl:apply-templates>
					</xsl:copy>
				</div>
			</xsl:when>
			<xsl:otherwise>
				<!-- To ignore all of the following rules, copy instead of apply templates. -->
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="xhtml:table[contains(@class, 'list')]/xhtml:tbody" mode="similarPhonePairs">
		<xsl:param name="similarPhonePairs" />
		<xsl:param name="moreSimilar" />
		<xsl:variable name="pair" select="xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']" />
		<xsl:variable name="literal1" select="$pair/xhtml:ul/xhtml:li[1]/xhtml:span" />
		<xsl:variable name="literal2" select="$pair/xhtml:ul/xhtml:li[2]/xhtml:span" />
		<xsl:variable name="similarPhonePair" select="$similarPhonePairs/pair[(phone[1]/@literal = $literal1 and phone[2]/@literal = $literal2) or (phone[1]/@literal = $literal2 and phone[2]/@literal = $literal1)]" />
		<xsl:choose>
			<xsl:when test="$moreSimilar = 'true'">
				<xsl:if test="$similarPhonePair">
					<xsl:copy-of select="." />
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="not($similarPhonePair)">
					<xsl:copy-of select="." />
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>