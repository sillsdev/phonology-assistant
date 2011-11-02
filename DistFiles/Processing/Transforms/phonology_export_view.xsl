<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
xmlns:svg="http://www.w3.org/2000/svg"
exclude-result-prefixes="xhtml svg"
>

  <!-- phonology_export_view.xsl 2011-11-02 -->
  <!-- Adds details and heading to exported view before conversion to XHTML to Word 2003 XML. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<!-- Insert CDATA sections within style elements. -->
	<xsl:output cdata-section-elements="svg:style xhtml:style" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="languageName" select="$details/xhtml:li[@class = 'languageName']" />
	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />
	<xsl:variable name="minimalPairs" select="$details/xhtml:li[@class = 'minimalPairs']" />

	<xsl:variable name="formatting" select="$metadata/xhtml:table[@class = 'formatting']" />
	<xsl:variable name="sorting" select="$metadata/xhtml:ul[@class = 'sorting']" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="tableOfDetails" select="$options/xhtml:li[@class = 'tableOfDetails']" />
	<xsl:variable name="hyperlinkToEthnologue" select="$options/xhtml:li[@class = 'hyperlinkToEthnologue']" />
	<xsl:variable name="dateAndTime" select="$options/xhtml:li[@class = 'dateAndTime']" />
	<xsl:variable name="oneMinimalPairPerGroup" select="$options/xhtml:li[@class = 'oneMinimalPairPerGroup']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Append language name to title. -->
	<xsl:template match="/xhtml:html/xhtml:head/xhtml:title">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="." />
			<xsl:if test="string-length($languageName) != 0 and $languageName != 'Undetermined'">
				<xsl:value-of select="' for '" />
				<xsl:value-of select="$languageName" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>
  
  <!-- Table of details. -->
	<xsl:template match="xhtml:div[@id = 'metadata']">
		<xsl:copy-of select="." />
		<xsl:if test="$tableOfDetails = 'true'">
			<xsl:variable name="primarySortFieldName" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]/@title" />
			<xsl:variable name="primarySortFieldDirection" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]" />
			<xsl:variable name="phoneticSortClass" select="$sorting/xhtml:li[@class = 'phoneticSortClass']" />
			<xsl:variable name="phoneticSortClassName">
				<xsl:choose>
					<xsl:when test="$phoneticSortClass = 'place_or_backness'">
						<xsl:value-of select="'place or backness'" />
					</xsl:when>
					<xsl:when test="$phoneticSortClass = 'manner_or_height'">
						<xsl:value-of select="'manner or height'" />
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="phoneticSearchSubfieldOrder" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol" />
			<xsl:variable name="view" select="$details//xhtml:li[@class = 'view']" />
			<xsl:variable name="searchPattern" select="$details/xhtml:li[@class = 'searchPattern']" />
			<xsl:variable name="filter" select="$details/xhtml:li[@class = 'filter']" />
			<xsl:variable name="numberOfMonophthongs" select="count(//xhtml:div[starts-with(@class, 'quadrilateral')]//svg:g[@class = 'data']/svg:g[@class = 'monophthong'])" />
			<xsl:variable name="numberOfDiphthongs" select="count(//xhtml:div[starts-with(@class, 'quadrilateral')]//svg:g[@class = 'data']/svg:g[@class = 'diphthong'])" />
			<xsl:variable name="numberOfPhones" select="$details/xhtml:li[@class = 'numberOfPhones']" />
			<xsl:variable name="numberOfPhonemes" select="$details/xhtml:li[@class = 'numberOfPhonemes']" />
			<xsl:variable name="numberOfRecords" select="$details/xhtml:li[@class = 'numberOfRecords']" />
			<xsl:variable name="numberOfGroups" select="$details/xhtml:li[@class = 'numberOfGroups']" />
			<xsl:variable name="numberOfPairsMoreSimilar" select="$details/xhtml:li[@class = 'number pairs more-similar']" />
			<xsl:variable name="numberOfPairsLessSimilar" select="$details/xhtml:li[@class = 'number pairs less-similar']" />
			<xsl:variable name="numberOfPairsLeastSimilar" select="$details/xhtml:li[@class = 'number pairs least-similar']" />
			<xsl:variable name="minimalPairs" select="$details/xhtml:li[@class = 'minimalPairs']" />
			<xsl:variable name="projectName" select="$details/xhtml:li[@class = 'projectName']" />
			<xsl:variable name="languageName" select="$details/xhtml:li[@class = 'languageName']" />
			<xsl:variable name="languageCode" select="$details/xhtml:li[@class = 'languageCode']" />
			<xsl:variable name="date" select="$details/xhtml:li[@class = 'date']" />
			<xsl:variable name="time" select="$details/xhtml:li[@class = 'time']" />
			<table class="details" xmlns="http://www.w3.org/1999/xhtml">
				<col />
				<col />
				<tbody>
					<xsl:if test="$projectName != $languageName">
						<tr class="project name">
							<th scope="row">
								<xsl:value-of select="'Project name:'" />
							</th>
							<td>
								<xsl:value-of select="$projectName" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$languageName">
						<tr class="language name">
							<th scope="row">
								<xsl:value-of select="'Language name:'" />
							</th>
							<td>
								<xsl:value-of select="$languageName" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="string-length($languageCode) != 0">
						<tr class="language code">
							<th scope="row">
								<xsl:value-of select="'ISO 639-3 code:'" />
							</th>
							<td>
								<xsl:choose>
									<xsl:when test="$hyperlinkToEthnologue and string-length($languageCode) = 3 and string-length(translate($languageCode, 'abcdefghijklmnopqrstuvwxyz', '')) = 0 and $languageCode != 'und' and $languageCode != 'mul'">
										<a href="{concat('http://www.ethnologue.com/show_language.asp?code=', $languageCode)}" title="ethnologue.com">
											<xsl:value-of select="$languageCode" />
										</a>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$languageCode" />
									</xsl:otherwise>
								</xsl:choose>
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$searchPattern">
						<tr class="searchPattern">
							<th scope="row">
								<xsl:value-of select="'Search pattern:'" />
							</th>
							<td class="Phonetic">
								<xsl:value-of select="$searchPattern" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$filter">
						<tr class="filter">
							<th scope="row">
								<xsl:value-of select="'Filter:'" />
							</th>
							<td>
								<strong>
									<xsl:value-of select="$filter" />
								</strong>
							</td>
						</tr>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="$view = 'Vowel quadrilateral'">
							<xsl:if test="$numberOfMonophthongs != 0">
								<tr class="number monophthong" xmlns="http://www.w3.org/1999/xhtml">
									<th scope="row">
										<xsl:value-of select="'Number of monophthongs:'" />
									</th>
									<td>
										<xsl:value-of select="$numberOfMonophthongs" />
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="$numberOfDiphthongs != 0">
								<tr class="number diphthong" xmlns="http://www.w3.org/1999/xhtml">
									<th scope="row">
										<xsl:value-of select="'Number of diphthongs:'" />
									</th>
									<td>
										<xsl:value-of select="$numberOfDiphthongs" />
									</td>
								</tr>
							</xsl:if>
						</xsl:when>
						<xsl:when test="$numberOfPhones">
							<xsl:choose>
								<xsl:when test="$view = 'Consonant Chart'">
									<xsl:call-template name="number">
										<xsl:with-param name="type" select="'consonant'" />
										<xsl:with-param name="value" select="$numberOfPhones" />
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$view = 'Vowel Chart'">
									<xsl:call-template name="number">
										<xsl:with-param name="type" select="'vowel'" />
										<xsl:with-param name="value" select="$numberOfPhones" />
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="numberCV">
										<xsl:with-param name="type" select="'consonant'" />
									</xsl:call-template>
									<xsl:call-template name="numberCV">
										<xsl:with-param name="type" select="'vowel'" />
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:when test="$numberOfPhonemes">
							<tr class="number segment" xmlns="http://www.w3.org/1999/xhtml">
								<th scope="row">
									<xsl:value-of select="'Number of segments:'" />
								</th>
								<td>
									<xsl:value-of select="$numberOfPhonemes" />
								</td>
							</tr>
						</xsl:when>
						<xsl:when test="$numberOfRecords">
							<tr class="number record">
								<th scope="row">
									<xsl:value-of select="'Number of records:'" />
								</th>
								<td>
									<xsl:value-of select="$numberOfRecords" />
								</td>
							</tr>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="$numberOfGroups">
						<tr class="number group">
							<th scope="row">
								<xsl:value-of select="'Number of groups:'" />
							</th>
							<td>
								<xsl:value-of select="$numberOfGroups" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$numberOfPairsMoreSimilar">
						<tr class="number pairs more-similar">
							<th scope="row">
								<xsl:value-of select="'Number of '" />
								<span class="more-similar">
									<xsl:value-of select="'more-similar'" />
								</span>
								<xsl:value-of select="' pairs:'" />
							</th>
							<td>
								<xsl:value-of select="$numberOfPairsMoreSimilar" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$numberOfPairsLessSimilar">
						<tr class="number pairs less-similar">
							<th scope="row">
								<xsl:value-of select="'Number of '" />
								<span class="less-similar">
									<xsl:value-of select="'less-similar'" />
								</span>
								<xsl:value-of select="' pairs:'" />
							</th>
							<td>
								<xsl:value-of select="$numberOfPairsLessSimilar" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$numberOfPairsLeastSimilar">
						<tr class="number pairs least-similar">
							<th scope="row">
								<xsl:value-of select="'Number of '" />
								<span class="least-similar">
									<xsl:value-of select="'least-similar'" />
								</span>
								<xsl:value-of select="' pairs:'" />
							</th>
							<td>
								<xsl:value-of select="$numberOfPairsLeastSimilar" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$minimalPairs">
						<tr class="minimal_pairs" xmlns="http://www.w3.org/1999/xhtml">
							<th scope="row">
								<xsl:value-of select="'Minimal pairs:'" />
							</th>
							<td>
								<xsl:choose>
									<xsl:when test="$minimalPairs = 'Before'">
										<xsl:value-of select="'Identical preceding environment'" />
									</xsl:when>
									<xsl:when test="$minimalPairs = 'After'">
										<xsl:value-of select="'Identical following environment'" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'Both environments identical'" />
									</xsl:otherwise>
								</xsl:choose>
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$sorting">
						<tr class="sorting_field" xmlns="http://www.w3.org/1999/xhtml">
							<th scope="row">
								<xsl:value-of select="'Sort field:'" />
							</th>
							<td>
								<xsl:value-of select="$primarySortFieldName" />
								<xsl:if test="$primarySortFieldName = 'Phonetic' and $phoneticSortClassName">
									<xsl:value-of select="concat(', ', $phoneticSortClassName)" />
								</xsl:if>
								<xsl:if test="$primarySortFieldDirection = 'descending'">
									<xsl:value-of select="concat(', ', $primarySortFieldDirection)" />
								</xsl:if>
							</td>
						</tr>
						<xsl:if test="$view = 'Search' and $phoneticSearchSubfieldOrder">
							<tr class="sorting_options" xmlns="http://www.w3.org/1999/xhtml">
								<th scope="row">
									<xsl:value-of select="'Phonetic sort options:'" />
								</th>
								<td>
									<xsl:call-template name="phoneticSearchSubfieldOrder">
										<xsl:with-param name="subfield" select="$phoneticSearchSubfieldOrder/xhtml:li[1]" />
									</xsl:call-template>
									<xsl:call-template name="phoneticSearchSubfieldOrder">
										<xsl:with-param name="subfield" select="$phoneticSearchSubfieldOrder/xhtml:li[2]" />
									</xsl:call-template>
									<xsl:call-template name="phoneticSearchSubfieldOrder">
										<xsl:with-param name="subfield" select="$phoneticSearchSubfieldOrder/xhtml:li[3]" />
									</xsl:call-template>
								</td>
							</tr>
						</xsl:if>
					</xsl:if>
					<xsl:if test="$dateAndTime = 'true' and $date and $time">
						<tr class="datetime">
							<th scope="row">
								<xsl:value-of select="'Date and time:'" />
							</th>
							<td>
								<!-- TO DO: Can the cell have the class instead of the span? -->
								<span class="dtstart">
									<span class="value">
										<xsl:value-of select="$date" />
									</span>
									<xsl:value-of select="' at '" />
									<span class="value">
										<xsl:value-of select="$time" />
									</span>
								</span>
							</td>
						</tr>
					</xsl:if>
				</tbody>
			</table>
		</xsl:if>
	</xsl:template>

	<xsl:template name="number">
		<xsl:param name="type" />
		<xsl:param name="value" />
		<tr class="{concat('number ', $type)}" xmlns="http://www.w3.org/1999/xhtml">
			<th scope="row">
				<xsl:value-of select="concat('Number of ', $type, 's:')" />
			</th>
			<td>
				<xsl:value-of select="$value" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template name="numberCV">
		<xsl:param name="type" />
		<xsl:variable name="class" select="concat('CV chart ', $type)" />
		<xsl:variable name="tableCV" select="//xhtml:table[@class = $class]" />
		<xsl:if test="$tableCV">
			<xsl:call-template name="number">
				<xsl:with-param name="type" select="$type" />
				<xsl:with-param name="value" select="count($tableCV//xhtml:td/xhtml:span)" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="phoneticSearchSubfieldOrder">
		<xsl:param name="subfield" />
		<xsl:if test="$subfield">
			<xsl:variable name="class" select="$subfield/@class" />
			<xsl:if test="$subfield/preceding-sibling::*">
				<xsl:value-of select="', '" />
			</xsl:if>
			<xsl:choose>
				<xsl:when test="contains($class, 'preceding')">
					<xsl:value-of select="'Preceding'" />
				</xsl:when>
				<xsl:when test="contains($class, 'following')">
					<xsl:value-of select="'Following'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'Item'" />
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="$subfield = 'rightToLeft'">
				<xsl:value-of select="' r-to-l'" />
			</xsl:if>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>