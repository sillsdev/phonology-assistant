<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
xmlns:svg="http://www.w3.org/2000/svg"
exclude-result-prefixes="xhtml svg"
>

  <!-- phonology_export_view_details.xsl 2011-11-07 -->
  <!-- Add table of details to exported view before conversion to XHTML or Word 2003 XML. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<!-- Insert CDATA sections within style elements. -->
	<xsl:output cdata-section-elements="svg:style xhtml:style" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Display selected metadata as a table of details. -->
	<xsl:template match="xhtml:div[@id = 'metadata']">
		<xsl:copy-of select="." />
		<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
		<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
		<xsl:if test="$options/xhtml:li[@class = 'tableOfDetails'] = 'true'">
			<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
			<xsl:variable name="projectName" select="$details/xhtml:li[@class = 'project name']" />
			<xsl:variable name="languageName" select="$details/xhtml:li[@class = 'language name']" />
			<xsl:variable name="languageCode" select="$details/xhtml:li[@class = 'language code']" />
			<xsl:variable name="filter" select="$details/xhtml:li[@class = 'filter']" />
			<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />
			<xsl:variable name="pairs" select="$details/xhtml:li[@class = 'pairs']" />
			<xsl:variable name="sorting" select="$metadata/xhtml:ul[@class = 'sorting']" />
			<table class="details" xmlns="http://www.w3.org/1999/xhtml">
				<col />
				<col />
				<tbody>
					<xsl:if test="$projectName != $languageName">
						<xsl:call-template name="tr">
							<xsl:with-param name="class" select="'project name'" />
							<xsl:with-param name="heading" select="'Project name:'" />
							<xsl:with-param name="value" select="$projectName" />
						</xsl:call-template>
					</xsl:if>
					<xsl:call-template name="tr">
						<xsl:with-param name="class" select="'language name'" />
						<xsl:with-param name="heading" select="'Language name:'" />
						<xsl:with-param name="value" select="$languageName" />
					</xsl:call-template>
					<xsl:if test="string-length($languageCode) = 3">
						<tr class="language code">
							<th scope="row">
								<xsl:value-of select="'ISO 639-3 code:'" />
							</th>
							<td>
								<xsl:choose>
									<xsl:when test="$options/xhtml:li[@class = 'hyperlinkToEthnologue'] = 'true' and string-length(translate($languageCode, 'abcdefghijklmnopqrstuvwxyz', '')) = 0 and $languageCode != 'und' and $languageCode != 'mul'">
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
					<xsl:call-template name="tr">
						<xsl:with-param name="class" select="'search pattern'" />
						<xsl:with-param name="heading" select="'Search pattern:'" />
						<xsl:with-param name="value" select="$details/xhtml:li[@class = 'search pattern']" />
					</xsl:call-template>
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
						<xsl:when test="$view = 'Data' or $view = 'Search' or $view = 'Distribution'">
							<xsl:call-template name="trNumber">
								<xsl:with-param name="key" select="'record'" />
								<xsl:with-param name="details" select="$details" />
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:if test="$view = 'Segments'">
								<xsl:call-template name="trNumber">
									<xsl:with-param name="key" select="'segment'" />
									<xsl:with-param name="details" select="$details" />
								</xsl:call-template>
							</xsl:if>
							<xsl:call-template name="trNumber">
								<xsl:with-param name="key" select="'consonant'" />
								<xsl:with-param name="details" select="$details" />
							</xsl:call-template>
							<xsl:call-template name="trNumber">
								<xsl:with-param name="key" select="'vowel'" />
								<xsl:with-param name="details" select="$details" />
							</xsl:call-template>
							<xsl:call-template name="trNumber">
								<xsl:with-param name="key" select="'monophthong'" />
								<xsl:with-param name="details" select="$details" />
							</xsl:call-template>
							<xsl:call-template name="trNumber">
								<xsl:with-param name="key" select="'diphthong'" />
								<xsl:with-param name="details" select="$details" />
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:call-template name="trNumber">
						<xsl:with-param name="key" select="'group'" />
						<xsl:with-param name="details" select="$details" />
					</xsl:call-template>
					<xsl:call-template name="trNumberPairs">
						<xsl:with-param name="key" select="'more-similar'" />
						<xsl:with-param name="details" select="$details" />
					</xsl:call-template>
					<xsl:call-template name="trNumberPairs">
						<xsl:with-param name="key" select="'less-similar'" />
						<xsl:with-param name="details" select="$details" />
					</xsl:call-template>
					<xsl:call-template name="trNumberPairs">
						<xsl:with-param name="key" select="'least-similar'" />
						<xsl:with-param name="details" select="$details" />
					</xsl:call-template>
					<xsl:if test="$pairs">
						<xsl:call-template name="tr">
							<xsl:with-param name="class" select="'pairs'" />
							<xsl:with-param name="heading" select="'Minimal pairs:'" />
							<xsl:with-param name="value">
								<xsl:choose>
									<xsl:when test="$pairs = 'Before'">
										<xsl:value-of select="'Identical preceding environment'" />
									</xsl:when>
									<xsl:when test="$pairs = 'After'">
										<xsl:value-of select="'Identical following environment'" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'Both environments identical'" />
									</xsl:otherwise>
								</xsl:choose>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="$sorting">
						<xsl:variable name="primarySortFieldName" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]/@title" />
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
						<xsl:variable name="primarySortFieldDirection" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]" />
						<xsl:call-template name="tr">
							<xsl:with-param name="class" select="'sorting_field'" />
							<xsl:with-param name="heading" select="'Sorting field:'" />
							<xsl:with-param name="value">
								<xsl:value-of select="$primarySortFieldName" />
								<xsl:if test="$primarySortFieldName = 'Phonetic' and $phoneticSortClassName">
									<xsl:value-of select="', '" />
									<xsl:value-of select="$phoneticSortClassName" />
								</xsl:if>
								<xsl:if test="$primarySortFieldDirection = 'descending'">
									<xsl:value-of select="', '" />
									<xsl:value-of select="$primarySortFieldDirection" />
								</xsl:if>
							</xsl:with-param>
						</xsl:call-template>
						<xsl:if test="$view = 'Search'">
							<xsl:variable name="phoneticSearchSubfieldOrder" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol" />
							<xsl:if test="$phoneticSearchSubfieldOrder">
								<xsl:call-template name="tr">
									<xsl:with-param name="class" select="'sorting_options'" />
									<xsl:with-param name="heading" select="'Phonetic sorting options:'" />
									<xsl:with-param name="value">
										<xsl:call-template name="phoneticSearchSubfieldOrder">
											<xsl:with-param name="subfield" select="$phoneticSearchSubfieldOrder/xhtml:li[1]" />
										</xsl:call-template>
										<xsl:call-template name="phoneticSearchSubfieldOrder">
											<xsl:with-param name="subfield" select="$phoneticSearchSubfieldOrder/xhtml:li[2]" />
										</xsl:call-template>
										<xsl:call-template name="phoneticSearchSubfieldOrder">
											<xsl:with-param name="subfield" select="$phoneticSearchSubfieldOrder/xhtml:li[3]" />
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:if>
						</xsl:if>
					</xsl:if>
					<xsl:if test="$options/xhtml:li[@class = 'dateAndTime'] = 'true'">
						<xsl:variable name="date" select="$details/xhtml:li[@class = 'date']" />
						<xsl:variable name="time" select="$details/xhtml:li[@class = 'time']" />
						<xsl:if test="$date and $time">
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
					</xsl:if>
				</tbody>
			</table>
		</xsl:if>
	</xsl:template>

	<xsl:template name="tr">
		<xsl:param name="class" />
		<xsl:param name="heading" />
		<xsl:param name="value" />
		<xsl:if test="$value">
			<xsl:if test="string-length($value) != 0">
				<tr class="{$class}" xmlns="http://www.w3.org/1999/xhtml">
					<th scope="row">
						<xsl:value-of select="$heading" />
					</th>
					<td>
						<xsl:value-of select="$value" />
					</td>
				</tr>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template name="trNumber">
		<xsl:param name="key" />
		<xsl:param name="details" />
		<xsl:variable name="class" select="concat('number ', $key)" />
		<xsl:variable name="value" select="$details/xhtml:li[@class = $class]" />
		<xsl:if test="$value">
			<xsl:if test="$value != '0'">
				<xsl:call-template name="tr">
					<xsl:with-param name="class" select="$class" />
					<xsl:with-param name="heading" select="concat('Number of ', $key, 's:')" />
					<xsl:with-param name="value" select="$value" />
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template name="trNumberPairs">
		<xsl:param name="key" />
		<xsl:param name="details" />
		<xsl:variable name="class" select="concat('number pairs ', $key)" />
		<xsl:variable name="value" select="$details/xhtml:li[@class = $class]" />
		<xsl:if test="$value">
			<xsl:if test="$value != '0'">
				<tr class="{$class}" xmlns="http://www.w3.org/1999/xhtml">
					<th scope="row">
						<xsl:value-of select="'Number of '" />
						<span class="{$key}">
							<xsl:value-of select="$key" />
						</span>
						<xsl:value-of select="' pairs:'" />
					</th>
					<td>
						<xsl:value-of select="$value" />
					</td>
				</tr>
			</xsl:if>
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