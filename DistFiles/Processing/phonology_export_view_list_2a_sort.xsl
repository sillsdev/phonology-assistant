<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_2a_sort.xsl 2010-05-13 -->
  <!-- Insert keys to sort by the Phonetic column. -->
	
	<!-- Differences between views in the Phonology Assistant program -->
	<!-- and interactive tables in XHTML files exported from Phonology Assistant: -->
	<!-- * If the list is grouped, a view regroups according to the column, -->
	<!--   but a table resorts within the same groups. -->
	<!-- * If the list has minimal groups, both resort within the same groups, -->
	<!--   but if one minimal pair per group, a table has no sortable columns. -->
	<!-- For more information, see the phonology.js file. -->

	<!-- Important: If table is neither Data Corpus nor Search view, copy it with no changes. -->

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

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
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

	<!-- For all interactive Web pages, include sort order list in the Phonetic field. -->
	<xsl:variable name="phoneticSortOrder">
		<xsl:choose>
			<xsl:when test="$interactiveWebPage = 'true' and //xhtml:table[contains(@class, 'list')]//xhtml:td[starts-with(@class, 'Phonetic')]">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<!-- If one minimal pair per group, sort regardless of format option. -->
			<xsl:when test="//xhtml:table[contains(@class, 'list')]//xhtml:th[@class = 'Phonetic pair']">
				<xsl:value-of select="'true'" />
			</xsl:when>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="phoneticSortOption" select="$sorting/xhtml:li[@class = 'phoneticSortOption']" />

  <!-- A project phonetic inventory file contains digits for sort keys. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="units" select="document($projectPhoneticInventoryXML)/inventory/units[@type = $typeOfUnits]" />
	
  <xsl:variable name="maxUnitLength">
    <xsl:for-each select="$units/unit">
      <xsl:sort select="string-length(@literal)" order="descending" data-type="number" />
      <xsl:if test="position() = 1">
        <xsl:value-of select="string-length(@literal)" />
      </xsl:if>
    </xsl:for-each>
  </xsl:variable>

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <!-- Insert a metadata setting for the rest of the transformations. -->
  <xsl:template match="xhtml:div[@id = 'metadata']/xhtml:ul[@class = 'sorting']">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
      <xsl:if test="$phoneticSortOrder = 'true'">
        <li class="phoneticSortOrder" xmlns="http://www.w3.org/1999/xhtml">
          <xsl:value-of select="$phoneticSortOrder" />
        </li>
      </xsl:if>
    </xsl:copy>
  </xsl:template>

  <!-- Apply or ignore transformations. -->
  <xsl:template match="xhtml:table">
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

  <!-- Phonetic pair heading cells contain a list of two units. -->
	<!-- Although researchers cannot resort the result, the pipeline sorts -->
	<!-- groups that were split in step 1b, and then merges them in step 3a. -->
	<xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th[@class = 'Phonetic pair']/xhtml:ul">
		<xsl:variable name="unit1" select="xhtml:li[1]/xhtml:span" />
		<xsl:variable name="unit2" select="xhtml:li[2]/xhtml:span" />
		<xsl:variable name="sortKey1">
			<xsl:call-template name="phoneticTextToSortKey">
				<xsl:with-param name="text" select="$unit1" />
				<xsl:with-param name="phoneticSortOption" select="$phoneticSortOption" />
				<xsl:with-param name="direction" select="'leftToRight'" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="sortKey2">
			<xsl:call-template name="phoneticTextToSortKey">
				<xsl:with-param name="text" select="$unit2" />
				<xsl:with-param name="phoneticSortOption" select="$phoneticSortOption" />
				<xsl:with-param name="direction" select="'leftToRight'" />
			</xsl:call-template>
		</xsl:variable>
		<!-- Make sure the pair of units are in the correct relative order. -->
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
						<xsl:value-of select="concat($unit2, $unit1)" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat($unit1, $unit2)" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
			<xsl:with-param name="direction" select="'leftToRight'" />
		</xsl:call-template>
  </xsl:template>

	<!-- Phonetic in heading rows unless one minimal pair per group. -->
	<xsl:template match="xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic') and @class != 'Phonetic pair']">
		<xsl:variable name="class" select="@class" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<!-- Enclose the text in a span to separate it from the sort order list. -->
			<span xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="text()" />
			</span>
			<xsl:call-template name="sortOrder">
				<xsl:with-param name="text">
					<xsl:choose>
						<!-- Group by Phonetic in Data Corpus view. -->
						<xsl:when test="$class = 'Phonetic'">
							<xsl:value-of select="text()" />
						</xsl:when>
						<!-- One minimal pair per group in Search view. -->
						<xsl:when test="../xhtml:th[@class = 'Phonetic pair']">
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

	<!-- Phonetic cells in data rows. -->
  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:td[starts-with(@class, 'Phonetic')]">
    <xsl:param name="position" />
    <xsl:param name="sortOrderFormat" />
    <xsl:variable name="class" select="@class" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<!-- Text is already in a span (for example, because there is a transcription list). -->
				<xsl:when test="xhtml:span">
					<xsl:apply-templates />
				</xsl:when>
				<!-- Enclose the text in a span to separate it from the sort order list. -->
				<xsl:otherwise>
					<span xmlns="http://www.w3.org/1999/xhtml">
						<xsl:value-of select="text()" />
					</span>
				</xsl:otherwise>
			</xsl:choose>
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
		<ul class="sortOrder" xmlns="http://www.w3.org/1999/xhtml">
			<li class="placeOfArticulation">
				<xsl:call-template name="phoneticTextToSortKey">
					<xsl:with-param name="text" select="$text" />
					<xsl:with-param name="phoneticSortOption" select="'placeOfArticulation'" />
					<xsl:with-param name="direction" select="$direction" />
				</xsl:call-template>
			</li>
			<li class="mannerOfArticulation">
				<xsl:call-template name="phoneticTextToSortKey">
					<xsl:with-param name="text" select="$text" />
					<xsl:with-param name="phoneticSortOption" select="'mannerOfArticulation'" />
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
		<xsl:param name="phoneticSortOption" />
		<xsl:param name="direction" />
		<xsl:param name="unitLength" select="$maxUnitLength" />
		<xsl:variable name="textLength" select="string-length($text)" />
		<xsl:if test="$textLength != 0">
			<xsl:choose>
				<xsl:when test="$unitLength = 0">
					<!-- If no units matched, skip a character, and then continue matching. -->
					<xsl:call-template name="phoneticTextToSortKey">
						<xsl:with-param name="text" select="substring($text, 2)" />
						<xsl:with-param name="phoneticSortOption" select="$phoneticSortOption" />
						<xsl:with-param name="direction" select="$direction" />
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="$unitLength &gt; $textLength">
					<!-- If the current unit length is less than the remaining text length, decrease the length. -->
					<xsl:call-template name="phoneticTextToSortKey">
						<xsl:with-param name="text" select="$text" />
						<xsl:with-param name="phoneticSortOption" select="$phoneticSortOption" />
						<xsl:with-param name="direction" select="$direction" />
						<xsl:with-param name="unitLength" select="$unitLength - 1" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<!-- Attempt to match a unit of the current length in the phonetic inventory. -->
					<xsl:variable name="textUnit" select="substring($text, 1, $unitLength)" />
					<xsl:variable name="sortKey" select="$units/unit[@literal = $textUnit]/keys/sortKey[@class = $phoneticSortOption]" />
					<xsl:choose>
						<xsl:when test="string-length($sortKey) != 0">
							<!-- If a unit matches, return its sort key, and then continue matching. -->
							<xsl:if test="$direction != 'rightToLeft'">
								<xsl:value-of select="$sortKey" />
							</xsl:if>
							<xsl:call-template name="phoneticTextToSortKey">
								<xsl:with-param name="text" select="substring($text, $unitLength + 1)" />
								<xsl:with-param name="phoneticSortOption" select="$phoneticSortOption" />
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
								<xsl:with-param name="phoneticSortOption" select="$phoneticSortOption" />
								<xsl:with-param name="direction" select="$direction" />
								<xsl:with-param name="unitLength" select="$unitLength - 1" />
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>