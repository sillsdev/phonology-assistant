<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_1c_minimal_pairs_similar.xsl 2010-04-14 -->
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
		<xsl:param name="similarPhonePairs" />
		<xsl:param name="moreSimilar" />
    <xsl:copy>
			<xsl:apply-templates select="@* | node()">
				<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
				<xsl:with-param name="moreSimilar" select="$moreSimilar" />
			</xsl:apply-templates>
    </xsl:copy>
  </xsl:template>

	<xsl:template match="/xhtml:html">
		<xsl:choose>
			<xsl:when test="//xhtml:table[contains(@class, 'list')][xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']]">
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
				<xsl:variable name="similarPhonePairs" select="document($projectPhoneticInventoryXML)/inventory/similarPhonePairs[@type = $type]" />
				<xsl:choose>
					<xsl:when test="$similarPhonePairs">
						<xsl:copy>
							<xsl:apply-templates select="@* | node()">
								<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
							</xsl:apply-templates>
						</xsl:copy>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="." />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

  <xsl:template match="xhtml:table[contains(@class, 'list')][xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']]">
		<xsl:param name="similarPhonePairs" />
		<xsl:variable name="table" select="." />
		<xsl:variable name="colspan" select="count(xhtml:colgroup/xhtml:col) - 1" />
		<div class="report" xmlns="http://www.w3.org/1999/xhtml">
			<h4>More-similar pairs</h4>
			<xsl:copy>
				<xsl:apply-templates select="@* | node()">
					<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
					<xsl:with-param name="moreSimilar" select="'true'" />
				</xsl:apply-templates>
				<!-- For any similar pair of phones for which there are no minimal pairs, append an empty group. -->
				<xsl:for-each select="$similarPhonePairs/similarPhonePair[not(@features = 'false' and @chart = 'false')]">
					<xsl:variable name="literal1" select="similarPhone[1]/@literal" />
					<xsl:variable name="literal2" select="similarPhone[2]/@literal" />
					<xsl:choose>
						<xsl:when test="$table/xhtml:tbody/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair'][xhtml:ul/xhtml:li[1]/xhtml:span = $literal1 and xhtml:ul/xhtml:li[2]/xhtml:span = $literal2]" />
						<xsl:when test="$table/xhtml:tbody/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair'][xhtml:ul/xhtml:li[1]/xhtml:span = $literal2 and xhtml:ul/xhtml:li[2]/xhtml:span = $literal1]" />
						<xsl:otherwise>
							<tbody class="group">
								<tr class="heading">
									<th class="Phonetic pair">
										<xsl:call-template name="title">
											<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
											<xsl:with-param name="literal1" select="$literal1" />
											<xsl:with-param name="literal2" select="$literal2" />
										</xsl:call-template>
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
				<xsl:apply-templates select="@* | node()">
					<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
					<xsl:with-param name="moreSimilar" select="'false'" />
				</xsl:apply-templates>
			</xsl:copy>
		</div>
	</xsl:template>

	<xsl:template match="xhtml:table[contains(@class, 'list')]/xhtml:tbody">
		<xsl:param name="similarPhonePairs" />
		<xsl:param name="moreSimilar" />
		<xsl:variable name="pair" select="xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']" />
		<xsl:variable name="literal1" select="$pair/xhtml:ul/xhtml:li[1]/xhtml:span" />
		<xsl:variable name="literal2" select="$pair/xhtml:ul/xhtml:li[2]/xhtml:span" />
		<xsl:variable name="similarPhonePair" select="$similarPhonePairs/similarPhonePair[not(@features = 'false' and @chart = 'false')][(similarPhone[1]/@literal = $literal1 and similarPhone[2]/@literal = $literal2) or (similarPhone[1]/@literal = $literal2 and similarPhone[2]/@literal = $literal1)]" />
		<xsl:choose>
			<xsl:when test="$moreSimilar = 'true'">
				<xsl:if test="$similarPhonePair">
					<xsl:copy>
						<xsl:apply-templates select="@* | node()">
							<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
						</xsl:apply-templates>
					</xsl:copy>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="not($similarPhonePair)">
					<xsl:copy-of select="." />
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="xhtml:th[@class = 'Phonetic pair']">
		<xsl:param name="similarPhonePairs" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:call-template name="title">
				<xsl:with-param name="similarPhonePairs" select="$similarPhonePairs" />
				<xsl:with-param name="literal1" select="xhtml:ul/xhtml:li[1]/xhtml:span" />
				<xsl:with-param name="literal2" select="xhtml:ul/xhtml:li[2]/xhtml:span" />
			</xsl:call-template>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- For development only: indicate differences between feature rules/patterns and Pike's chart. -->
	<xsl:template name="title">
		<xsl:param name="similarPhonePairs" />
		<xsl:param name="literal1" />
		<xsl:param name="literal2" />
		<xsl:variable name="XOR">
			<xsl:choose>
				<xsl:when test="$similarPhonePairs/similarPhonePair[similarPhone[1]/@literal = $literal1 and similarPhone[2]/@literal = $literal2]">
					<xsl:variable name="similarPhonePair" select="$similarPhonePairs/similarPhonePair[similarPhone[1]/@literal = $literal1 and similarPhone[2]/@literal = $literal2]" />
					<xsl:choose>
						<xsl:when test="$similarPhonePair/@features = 'true' and $similarPhonePair/@chart = 'false'">
							<xsl:value-of select="'features'" />
						</xsl:when>
						<xsl:when test="$similarPhonePair/@features = 'false' and $similarPhonePair/@chart = 'true'">
							<xsl:value-of select="'chart'" />
						</xsl:when>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$similarPhonePairs/similarPhonePair[similarPhone[1]/@literal = $literal2 and similarPhone[2]/@literal = $literal1]">
					<xsl:variable name="similarPhonePair" select="$similarPhonePairs/similarPhonePair[similarPhone[1]/@literal = $literal2 and similarPhone[2]/@literal = $literal1]" />
					<xsl:choose>
						<xsl:when test="$similarPhonePair/@features = 'true' and $similarPhonePair/@chart = 'false'">
							<xsl:value-of select="'features'" />
						</xsl:when>
						<xsl:when test="$similarPhonePair/@features = 'false' and $similarPhonePair/@chart = 'true'">
							<xsl:value-of select="'chart'" />
						</xsl:when>
					</xsl:choose>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="string-length($XOR) != 0">
			<xsl:attribute name="title">
				<xsl:value-of select="$XOR" />
			</xsl:attribute>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>