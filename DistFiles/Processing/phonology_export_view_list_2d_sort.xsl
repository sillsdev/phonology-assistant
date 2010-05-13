<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_2d_sort.xsl 2010-05-13 -->
  <!-- Make it possible to sort an interactive list by the Phonetic column. -->

	<!-- Important: If table is neither Data Corpus nor Search view, copy it with no changes. -->

	<!-- Important: In case Phonology Assistant exports collapsed in class attributes, test: -->
	<!-- * xhtml:table[contains(@class, 'list')] instead of @class = 'list' -->
	<!-- * xhtml:tbody[contains(@class, 'group')] instead of @class = 'group' -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="sorting" select="$metadata/xhtml:ul[@class = 'sorting']" />
	<xsl:variable name="phoneticSortOrder" select="$sorting/xhtml:li[@class = 'phoneticSortOrder']" />
	<xsl:variable name="phoneticSortOption" select="$sorting/xhtml:li[@class = 'phoneticSortOption']" />
	<xsl:variable name="fieldOrderPhonetic" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[@title = 'Phonetic']" />

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <!-- Apply or ignore this transformation. -->
  <xsl:template match="xhtml:table">
    <xsl:choose>
      <xsl:when test="contains(@class, 'list') and $phoneticSortOrder = 'true'">
        <xsl:copy>
          <xsl:choose>
            <xsl:when test="$fieldOrderPhonetic = 'descending' and xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']">
							<!-- If one minimal pair per group, resort the split groups before merging them in the next step. -->
              <xsl:apply-templates select="@*" />
              <xsl:apply-templates select="xhtml:colgroup" />
              <xsl:apply-templates select="xhtml:thead" />
							<xsl:apply-templates select="xhtml:tbody">
								<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic item']//xhtml:li[@class = phoneticSortOption]" order="descending" />
							</xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="@* | node()" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:copy>
      </xsl:when>
      <xsl:otherwise>
        <!-- To ignore all of the following rules, copy instead of apply templates. -->
        <xsl:copy-of select="." />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Sort the records. -->
  <xsl:template match="xhtml:tbody">
    <xsl:variable name="phoneticFieldClass">
      <xsl:choose>
        <xsl:when test="$view = 'Search'">
          <xsl:value-of select="'Phonetic item'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'Phonetic'" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="order" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:apply-templates select="xhtml:tr[@class = 'heading']" />
      <xsl:choose>
        <xsl:when test="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]/@title != 'Phonetic'">
          <!-- If primary sort order was not Phonetic, sort by exported order. -->
          <xsl:apply-templates select="xhtml:tr[not(@class = 'heading')]">
            <xsl:sort select="xhtml:td[@class = $phoneticFieldClass]//xhtml:li[@class = 'exported']" />
          </xsl:apply-templates>
        </xsl:when>
        <xsl:when test="$sorting/xhtml:li[@class = 'phoneticSortOption'] = 'mannerOfArticulation'">
          <!-- If the primary sort order was Phonetic, manner of articulation. -->
          <xsl:choose>
            <xsl:when test="$order = 'descending'">
              <xsl:apply-templates select="xhtml:tr[not(@class = 'heading')]">
                <xsl:sort select="xhtml:td[@class = $phoneticFieldClass]//xhtml:li[@class = 'mannerOfArticulation']" order="descending" />
                <xsl:sort select="xhtml:td[@class = $phoneticFieldClass]//xhtml:li[@class = 'exported']" />
              </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="xhtml:tr[not(@class = 'heading')]">
                <xsl:sort select="xhtml:td[@class = $phoneticFieldClass]//xhtml:li[@class = 'mannerOfArticulation']" order="ascending" />
                <xsl:sort select="xhtml:td[@class = $phoneticFieldClass]//xhtml:li[@class = 'exported']" />
              </xsl:apply-templates>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="$order = 'descending'">
          <!-- If the primary sort order was Phonetic, place of articulation, descending. -->
          <xsl:apply-templates select="xhtml:tr[not(@class = 'heading')]">
            <xsl:sort select="xhtml:td[@class = $phoneticFieldClass]//xhtml:li[@class = 'placeOfArticulation']" order="descending" />
            <xsl:sort select="xhtml:td[@class = $phoneticFieldClass]//xhtml:li[@class = 'exported']" />
          </xsl:apply-templates>
        </xsl:when>
        <xsl:otherwise>
          <!-- Already sorted because the primary sort order was Phonetic, place of articulation, ascending. -->
          <xsl:apply-templates select="xhtml:tr[not(@class = 'heading')]" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>

  <!-- Delete lists in Phonetic pair headings of Search view. -->
	<xsl:template match="xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']/xhtml:ul[@class = 'sortOrder']" />

	<!-- Remove spans and delete lists in Phonetic item headings of Search view only if one minimal pair per group. -->
	<xsl:template match="xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading'][xhtml:th[@class = 'Phonetic pair']]/xhtml:th[@class = 'Phonetic item']/xhtml:span">
		<xsl:apply-templates />
	</xsl:template>
	<xsl:template match="xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading'][xhtml:th[@class = 'Phonetic pair']]/xhtml:th[@class = 'Phonetic item']/xhtml:ul[@class = 'sortOrder']" />

	<!-- Remove spans and delete lists in Phonetic item data of Search view only if one minimal pair per group. -->
	<xsl:template match="xhtml:tbody[contains(@class, 'group')][xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']]/xhtml:tr[@class = 'data']/xhtml:td[@class = 'Phonetic item']/xhtml:span">
		<xsl:apply-templates />
	</xsl:template>
	<xsl:template match="xhtml:tbody[contains(@class, 'group')][xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic pair']]/xhtml:tr[@class = 'data']/xhtml:td[@class = 'Phonetic item']/xhtml:ul[@class = 'sortOrder']" />

	<!-- Remove spans and delete lists in Phonetic preceding and following fields of Search view. -->
	<!-- However, keep the span if the cell contains a list other than sortOrder (for example, transcription). -->
  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:*[@class = 'Phonetic preceding' or @class = 'Phonetic following'][not(xhtml:ul[@class != 'sortOrder'])]/xhtml:span">
		<xsl:apply-templates />
	</xsl:template>
  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:*[@class = 'Phonetic preceding' or @class = 'Phonetic following']/xhtml:ul[@class = 'sortOrder']" />

  <!-- Delete the exported order of rows in lists. -->
  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:td/xhtml:ul[@class = 'sortOrder']/xhtml:li[@class = 'exported']" />

</xsl:stylesheet>