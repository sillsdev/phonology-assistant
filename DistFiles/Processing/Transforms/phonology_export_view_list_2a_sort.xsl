<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_2a_sort.xsl 2011-11-04 -->
  <!-- Insert keys to sort by the Phonetic column. -->
	
	<!-- Differences between views in the Phonology Assistant program -->
	<!-- and interactive tables in XHTML files exported from Phonology Assistant: -->
	<!-- * If the list is grouped, a view regroups according to the column, -->
	<!--   but a table resorts within the same groups. -->
	<!-- * If the list has minimal groups, both resort within the same groups, -->
	<!--   but if one minimal pair per group, a table has no sortable columns. -->
	<!-- For more information, see the phonology.js file. -->

	<!-- Important: If table is neither Data nor Search view, copy it with no changes. -->

	<!-- Important: In case Phonology Assistant exports collapsed in class attributes, test: -->
	<!-- * xhtml:table[contains(@class, 'list')] instead of @class = 'list' -->
	<!-- * xhtml:tbody[contains(@class, 'group')] instead of @class = 'group' -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="sorting" select="$metadata/xhtml:ul[@class = 'sorting']" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="interactiveWebPage">
		<xsl:if test="$format = 'XHTML'">
			<xsl:value-of select="$options/xhtml:li[@class = 'interactiveWebPage']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="oneMinimalPairPerGroup" select="$options/xhtml:li[@class = 'oneMinimalPairPerGroup']" />
	<xsl:variable name="languageIdentifiers" select="$metadata/xhtml:table[@class = 'language identifiers']" />

	<!-- For all interactive Web pages, include sort order list in the Phonetic field. -->
	<xsl:variable name="phoneticSortOrder">
		<xsl:choose>
			<xsl:when test="$interactiveWebPage = 'true' and //xhtml:table[contains(@class, 'list')]//xhtml:td[starts-with(@class, 'Phonetic')]">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<!-- If one minimal pair per group, sort regardless of format option. -->
			<xsl:when test="//xhtml:table[contains(@class, 'list')]//xhtml:th[starts-with(@class, 'Phonetic pair')]">
				<xsl:value-of select="'true'" />
			</xsl:when>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="phoneticSortOption" select="$sorting/xhtml:li[@class = 'phoneticSortOption']" />
	<xsl:variable name="phoneticSortClass">
		<xsl:choose>
			<xsl:when test="$phoneticSortOption = 'placeOfArticulation'">
				<xsl:value-of select="'place_or_backness'" />
			</xsl:when>
			<xsl:when test="$phoneticSortOption = 'mannerOfArticulation'">
				<xsl:value-of select="'manner_or_height'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$phoneticSortOption" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />
	<xsl:variable name="pairs" select="$details/xhtml:li[@class = 'pairs']" />

	<!-- A project phonetic inventory file contains digits for sort keys. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="languageIdentifier" select="document($projectPhoneticInventoryXML)/inventory/@languageIdentifier" />
	<xsl:variable name="segments" select="document($projectPhoneticInventoryXML)/inventory/segments" />
	
  <xsl:variable name="maxUnitLength">
    <xsl:for-each select="$segments/segment">
      <xsl:sort select="string-length(@literal)" order="descending" data-type="number" />
      <xsl:if test="position() = 1">
        <xsl:value-of select="string-length(@literal)" />
      </xsl:if>
    </xsl:for-each>
  </xsl:variable>

	<!--
	<xsl:variable name="primarySortKeyFormat" select="translate(count($segments/segment), '0123456789', '0000000000')" />
	-->
	<xsl:variable name="secondarySortKeyFormat" select="translate($segments/segment/keys/sortKey[@class = 'secondary'], '0123456789', '0000000000')" />

	<!-- Adjust title and heading for Search view. -->
	<xsl:variable name="heading">
		<xsl:variable name="title" select="/xhtml:html/xhtml:head/xhtml:title" />
		<xsl:choose>
			<xsl:when test="$view = 'Search'">
				<xsl:choose>
					<xsl:when test="$pairs">
						<xsl:value-of select="'Minimal pairs'" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Search results'" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$title" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<xsl:variable name="subheading">
		<xsl:if test="$view = 'Search'">
			<xsl:if test="not($pairs)">
				<xsl:value-of select="$details/xhtml:li[@class = 'pattern name']" />
			</xsl:if>
		</xsl:if>
	</xsl:variable>

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <!-- Insert two metadata settings for the rest of the transformations. -->
  <xsl:template match="xhtml:div[@id = 'metadata']/xhtml:ul[@class = 'sorting']">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
      <xsl:if test="$phoneticSortOrder = 'true'">
        <li class="phoneticSortOrder" xmlns="http://www.w3.org/1999/xhtml">
          <xsl:value-of select="$phoneticSortOrder" />
        </li>
      </xsl:if>
			<xsl:if test="string-length($phoneticSortClass) != 0">
				<li class="phoneticSortClass" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="$phoneticSortClass" />
				</li>
			</xsl:if>
		</xsl:copy>
  </xsl:template>

	<!-- Append language name to title (but not heading). -->
	<xsl:template match="/xhtml:html/xhtml:head/xhtml:title">
		<xsl:variable name="languageName" select="$details/xhtml:li[@class = 'language name']" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="$heading" />
			<xsl:if test="string-length($languageName) != 0 and $languageName != 'Undetermined'">
				<xsl:value-of select="' for '" />
				<xsl:value-of select="$languageName" />
			</xsl:if>
			<xsl:if test="string-length($subheading) != 0">
				<xsl:value-of select="': '" />
				<xsl:value-of select="$subheading" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	
  <!-- Apply or ignore transformations. -->
  <xsl:template match="xhtml:table">
		<h3 xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="$heading" />
			<xsl:if test="string-length($subheading) != 0">
				<xsl:value-of select="': '" />
				<xsl:value-of select="$subheading" />
			</xsl:if>
		</h3>
    <xsl:choose>
      <xsl:when test="contains(@class, 'list') and $phoneticSortOrder = 'true'">
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

  <!-- Remember original order of rows in case Phonetic is not the primary sort field. -->
  <xsl:template match="xhtml:tbody">
    <xsl:variable name="sortOrderFormat" select="translate(count(xhtml:tr[not(@class = 'heading')]), '0123456789', '0000000000')" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:apply-templates select="xhtml:tr[@class = 'heading']" />
      <xsl:for-each select="xhtml:tr[not(@class = 'heading')]">
        <xsl:copy>
          <xsl:apply-templates select="@*" />
          <xsl:apply-templates>
            <xsl:with-param name="position" select="position()" />
            <xsl:with-param name="sortOrderFormat" select="$sortOrderFormat" />
          </xsl:apply-templates>
        </xsl:copy>        
      </xsl:for-each>
    </xsl:copy>
  </xsl:template>

  <!-- Phonetic pair heading cells contain a list of two segments. -->
	<!-- Although researchers cannot resort the result, the pipeline sorts -->
	<!-- groups that were split in step 1b, and then merges them in step 3a. -->
	<xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th[starts-with(@class, 'Phonetic pair')]/xhtml:ul">
		<xsl:variable name="text1" select="xhtml:li[1]" />
		<xsl:variable name="text2" select="xhtml:li[2]" />
		<xsl:variable name="sortKey1">
			<xsl:call-template name="phoneticTextToSortKey">
				<xsl:with-param name="text" select="$text1" />
				<xsl:with-param name="phoneticSortClass" select="$phoneticSortClass" />
				<xsl:with-param name="direction" select="'leftToRight'" />
			</xsl:call-template>
			<xsl:call-template name="phoneticTextToSortKey">
				<xsl:with-param name="text" select="$text1" />
				<xsl:with-param name="phoneticSortClass" select="'secondary'" />
				<xsl:with-param name="direction" select="'leftToRight'" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="sortKey2">
			<xsl:call-template name="phoneticTextToSortKey">
				<xsl:with-param name="text" select="$text2" />
				<xsl:with-param name="phoneticSortClass" select="$phoneticSortClass" />
				<xsl:with-param name="direction" select="'leftToRight'" />
			</xsl:call-template>
			<xsl:call-template name="phoneticTextToSortKey">
				<xsl:with-param name="text" select="$text2" />
				<xsl:with-param name="phoneticSortClass" select="'secondary'" />
				<xsl:with-param name="direction" select="'leftToRight'" />
			</xsl:call-template>
		</xsl:variable>
		<!-- Make sure the pair of segments are in the correct relative order. -->
		<xsl:copy>
			<xsl:choose>
				<xsl:when test="$sortKey2 &lt; $sortKey1">
					<xsl:apply-templates select="xhtml:li[2]" />
					<xsl:apply-templates select="xhtml:li[1]" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="xhtml:li[1]" />
					<xsl:apply-templates select="xhtml:li[2]" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
		<!-- Insert a sort order list for the group. Step 2d will remove it. -->
		<xsl:call-template name="sortOrder">
			<xsl:with-param name="text">
				<xsl:choose>
					<xsl:when test="$sortKey2 &lt; $sortKey1">
						<xsl:value-of select="concat($text2, $text1)" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat($text1, $text2)" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
			<xsl:with-param name="direction" select="'leftToRight'" />
		</xsl:call-template>
  </xsl:template>

	<xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th[starts-with(@class, 'Phonetic pair')]/xhtml:ul/xhtml:li">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
			<xsl:if test="not(@lang)">
				<xsl:attribute name="lang">
					<xsl:value-of select="$languageIdentifier" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
    </xsl:copy>
	</xsl:template>

	<!-- Phonetic in heading rows unless one minimal pair per group. -->
	<xsl:template match="xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic') and not(starts-with(@class, 'Phonetic pair'))]">
		<xsl:variable name="class" select="@class" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<!-- Enclose the text in a span to separate it from the sort order list. -->
			<span lang="{$languageIdentifier}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="text()" />
			</span>
			<xsl:call-template name="sortOrder">
				<xsl:with-param name="text">
					<xsl:choose>
						<!-- Group by Phonetic in Data view. -->
						<xsl:when test="$class = 'Phonetic'">
							<xsl:value-of select="text()" />
						</xsl:when>
						<!-- One minimal pair per group in Search view. -->
						<xsl:when test="../xhtml:th[starts-with(@class, 'Phonetic pair')]">
							<xsl:if test="$class = 'Phonetic preceding' or $class = 'Phonetic following'">
								<xsl:value-of select="text()" />
							</xsl:if>
						</xsl:when>
						<!-- Group by Phonetic in Search view: the search item is part of the preceding field. -->
						<xsl:when test="$class = 'Phonetic item' and ../xhtml:th[@class = 'Phonetic preceding']">
							<xsl:value-of select="substring-before(../xhtml:th[@class = 'Phonetic preceding'], '/')" />
						</xsl:when>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="direction">
					<xsl:choose>
						<xsl:when test="$class = 'Phonetic'">
							<xsl:value-of select="'leftToRight'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol/xhtml:li[@class = $class]" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:tbody/xhtml:tr/xhtml:td">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="not(@lang)">
				<xsl:variable name="class" select="@class" />
				<xsl:variable name="languageIdentifier" select="$languageIdentifiers/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'fields']/xhtml:ul/xhtml:li[@class = $class]]/xhtml:td[@class = 'BCP_47']" />
				<xsl:if test="$languageIdentifier">
					<xsl:attribute name="lang">
						<xsl:value-of select="$languageIdentifier" />
					</xsl:attribute>
				</xsl:if>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Phonetic cells in data rows. -->
  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:td[starts-with(@class, 'Phonetic')]">
    <xsl:param name="position" />
    <xsl:param name="sortOrderFormat" />
    <xsl:variable name="class" select="@class" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<!-- Enclose the text in a span to separate it from the sort order list. -->
			<span lang="{$languageIdentifier}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="text()" />
			</span>
			<!-- Text is already in a span (for example, because there is a transcription list). -->
			<xsl:if test="xhtml:span">
				<xsl:apply-templates select="*[not(self::xhtml:span)]" />
			</xsl:if>
			<xsl:call-template name="sortOrder">
				<xsl:with-param name="text">
					<xsl:choose>
						<xsl:when test="xhtml:span">
							<xsl:value-of select="xhtml:span" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="text()" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="direction">
					<xsl:choose>
						<xsl:when test="$class = 'Phonetic'">
							<xsl:value-of select="'leftToRight'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol/xhtml:li[@class = $class]" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="position" select="$position" />
				<xsl:with-param name="sortOrderFormat" select="$sortOrderFormat" />
			</xsl:call-template>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="sortOrder">
		<xsl:param name="text" />
		<xsl:param name="direction" />
		<xsl:param name="position" />
		<xsl:param name="sortOrderFormat" />
		<ul class="sorting_order" xmlns="http://www.w3.org/1999/xhtml">
			<li class="place_or_backness">
				<xsl:call-template name="phoneticTextToSortKey">
					<xsl:with-param name="text" select="$text" />
					<xsl:with-param name="phoneticSortClass" select="'place_or_backness'" />
					<xsl:with-param name="direction" select="$direction" />
				</xsl:call-template>
			</li>
			<li class="manner_or_height">
				<xsl:call-template name="phoneticTextToSortKey">
					<xsl:with-param name="text" select="$text" />
					<xsl:with-param name="phoneticSortClass" select="'manner_or_height'" />
					<xsl:with-param name="direction" select="$direction" />
				</xsl:call-template>
			</li>
			<li class="secondary">
				<xsl:call-template name="phoneticTextToSortKey">
					<xsl:with-param name="text" select="$text" />
					<xsl:with-param name="phoneticSortClass" select="'secondary'" />
					<xsl:with-param name="direction" select="$direction" />
				</xsl:call-template>
			</li>
			<xsl:if test="$position">
				<li class="exported">
					<xsl:value-of select="format-number($position, $sortOrderFormat)" />
				</li>
			</xsl:if>
		</ul>
	</xsl:template>

	<xsl:template name="phoneticTextToSortKey">
		<xsl:param name="text" />
		<xsl:param name="phoneticSortClass" />
		<xsl:param name="direction" />
		<xsl:param name="segmentLength" select="$maxUnitLength" />
		<xsl:variable name="textLength" select="string-length($text)" />
		<xsl:if test="$textLength != 0">
			<xsl:choose>
				<xsl:when test="$segmentLength = 0">
					<!-- If no segments matched, skip a character, and then continue matching. -->
					<xsl:call-template name="phoneticTextToSortKey">
						<xsl:with-param name="text" select="substring($text, 2)" />
						<xsl:with-param name="phoneticSortClass" select="$phoneticSortClass" />
						<xsl:with-param name="direction" select="$direction" />
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="$segmentLength &gt; $textLength">
					<!-- If the current segment length is less than the remaining text length, decrease the length. -->
					<xsl:call-template name="phoneticTextToSortKey">
						<xsl:with-param name="text" select="$text" />
						<xsl:with-param name="phoneticSortClass" select="$phoneticSortClass" />
						<xsl:with-param name="direction" select="$direction" />
						<xsl:with-param name="segmentLength" select="$segmentLength - 1" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<!-- Attempt to match a segment of the current length in the phonetic inventory. -->
					<xsl:variable name="literal" select="substring($text, 1, $segmentLength)" />
					<xsl:variable name="segment" select="$segments/segment[@literal = $literal]" />
					<xsl:choose>
						<xsl:when test="$segment">
							<xsl:variable name="sortKey">
								<xsl:choose>
									<xsl:when test="$segment/keys/sortKey[@class = $phoneticSortClass]">
										<xsl:value-of select="$segment/keys/sortKey[@class = $phoneticSortClass]" />
									</xsl:when>
									<xsl:when test="$phoneticSortClass = 'secondary'">
										<xsl:value-of select="$secondarySortKeyFormat" />
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							<!-- If a segment matches, return its sort key, and then continue matching. -->
							<xsl:if test="$direction != 'rightToLeft'">
								<xsl:value-of select="$sortKey" />
							</xsl:if>
							<xsl:call-template name="phoneticTextToSortKey">
								<xsl:with-param name="text" select="substring($text, $segmentLength + 1)" />
								<xsl:with-param name="phoneticSortClass" select="$phoneticSortClass" />
								<xsl:with-param name="direction" select="$direction" />
							</xsl:call-template>
							<xsl:if test="$direction = 'rightToLeft'">
								<xsl:value-of select="$sortKey" />
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<!-- If no phone matched, decrease the length. -->
							<xsl:call-template name="phoneticTextToSortKey">
								<xsl:with-param name="text" select="$text" />
								<xsl:with-param name="phoneticSortClass" select="$phoneticSortClass" />
								<xsl:with-param name="direction" select="$direction" />
								<xsl:with-param name="segmentLength" select="$segmentLength - 1" />
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>