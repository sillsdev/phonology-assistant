﻿<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_list_2c_sort_place.xsl 2011-10-14 -->
  <!-- Make it possible to sort an interactive list by the Phonetic column: -->
	<!-- * consonant: place of articulation -->
	<!-- * vowel: backness -->

	<!-- Important: If table is neither Data Corpus nor Search view, copy it with no changes. -->

	<!-- Important: In case Phonology Assistant exports collapsed in class attributes, test: -->
	<!-- * xhtml:table[contains(@class, 'list')] instead of @class = 'list' -->
	<!-- * xhtml:tbody[contains(@class, 'group')] instead of @class = 'group' -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="phoneticSortClass" select="'place_or_backness'" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="sorting" select="$metadata/xhtml:ul[@class = 'sorting']" />
	<xsl:variable name="phoneticSortOrder" select="$sorting/xhtml:li[@class = 'phoneticSortOrder']" />
	<xsl:variable name="phoneticSortClassActive" select="$sorting/xhtml:li[@class = 'phoneticSortClass']" />

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:param name="position" />
    <xsl:param name="sortOrderFormat" />
    <xsl:copy>
      <xsl:apply-templates select="@* | node()">
        <xsl:with-param name="position" select="$position" />
        <xsl:with-param name="sortOrderFormat" select="$sortOrderFormat" />
      </xsl:apply-templates>
    </xsl:copy>
  </xsl:template>

  <!-- Apply or ignore this transformation. -->
	<xsl:template match="xhtml:table">
		<xsl:choose>
			<xsl:when test="contains(@class, 'list') and $phoneticSortOrder = 'true'">
				<xsl:choose>
					<!-- If one minimal pair per group, do not sort groups by the non-active sort option. -->
					<xsl:when test="xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')] and $phoneticSortClass != $phoneticSortClassActive">
						<!-- To ignore all of the following rules, copy instead of apply templates. -->
						<xsl:copy-of select="." />
					</xsl:when>
					<xsl:when test="xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic')]">
						<xsl:copy>
							<xsl:apply-templates select="@*" />
							<xsl:apply-templates select="xhtml:colgroup" />
							<xsl:apply-templates select="xhtml:thead" />
							<xsl:variable name="sortOrderFormat" select="translate(count(xhtml:tbody), '0123456789', '0000000000')" />
							<xsl:choose>
								<xsl:when test="xhtml:tbody[contains(@class, 'group')]/xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic']">
									<xsl:for-each select="xhtml:tbody">
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic']//xhtml:li[@class = $phoneticSortClass]" />
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = 'Phonetic']//xhtml:li[@class = 'secondary']" />
										<xsl:call-template name="tbody">
											<xsl:with-param name="position" select="position()" />
											<xsl:with-param name="sortOrderFormat" select="$sortOrderFormat" />
										</xsl:call-template>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise>
									<xsl:variable name="subfieldClass1" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol/xhtml:li[1]/@class" />
									<xsl:variable name="subfieldClass2" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol/xhtml:li[2]/@class" />
									<xsl:variable name="subfieldClass3" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol/xhtml:li[3]/@class" />
									<xsl:for-each select="xhtml:tbody">
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')]//xhtml:li[@class = $phoneticSortClass]" />
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[starts-with(@class, 'Phonetic pair')]//xhtml:li[@class = 'secondary']" />
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = $subfieldClass1]//xhtml:li[@class = $phoneticSortClass]" />
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = $subfieldClass1]//xhtml:li[@class = 'secondary']" />
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = $subfieldClass2]//xhtml:li[@class = $phoneticSortClass]" />
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = $subfieldClass2]//xhtml:li[@class = 'secondary']" />
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = $subfieldClass3]//xhtml:li[@class = $phoneticSortClass]" />
										<xsl:sort select="xhtml:tr[@class = 'heading']/xhtml:th[@class = $subfieldClass3]//xhtml:li[@class = 'secondary']" />
										<xsl:call-template name="tbody">
											<xsl:with-param name="position" select="position()" />
											<xsl:with-param name="sortOrderFormat" select="$sortOrderFormat" />
										</xsl:call-template>
									</xsl:for-each>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:copy>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy>
							<xsl:apply-templates select="@* | node()"/>
						</xsl:copy>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<!-- To ignore all of the following rules, copy instead of apply templates. -->
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Sort the records. -->
  <xsl:template match="xhtml:tbody">
    <xsl:call-template name="tbody">
			<xsl:with-param name="sortOrderFormat" select="translate(count(xhtml:tr[not(@class = 'heading')]), '0123456789', '0000000000')" />
		</xsl:call-template>
  </xsl:template>

  <xsl:template name="tbody">
    <xsl:param name="position" />
    <xsl:param name="sortOrderFormat" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:apply-templates select="xhtml:tr[@class = 'heading']">
        <xsl:with-param name="position" select="$position" />
        <xsl:with-param name="sortOrderFormat" select="$sortOrderFormat" />
      </xsl:apply-templates>
      <xsl:choose>
        <xsl:when test="$view = 'Search'">
          <!-- In Search view, sort the three subfields according to advanced phonetic sort options. -->
          <xsl:variable name="subfieldClass1" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol/xhtml:li[1]/@class" />
          <xsl:variable name="subfieldClass2" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol/xhtml:li[2]/@class" />
          <xsl:variable name="subfieldClass3" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol/xhtml:li[3]/@class" />
          <xsl:for-each select="xhtml:tr[not(@class = 'heading')]">
            <xsl:sort select="xhtml:td[@class = $subfieldClass1]//xhtml:li[@class = $phoneticSortClass]" />
						<xsl:sort select="xhtml:td[@class = $subfieldClass1]//xhtml:li[@class = 'secondary']" />
						<xsl:sort select="xhtml:td[@class = $subfieldClass2]//xhtml:li[@class = $phoneticSortClass]" />
						<xsl:sort select="xhtml:td[@class = $subfieldClass2]//xhtml:li[@class = 'secondary']" />
						<xsl:sort select="xhtml:td[@class = $subfieldClass3]//xhtml:li[@class = $phoneticSortClass]" />
						<xsl:sort select="xhtml:td[@class = $subfieldClass3]//xhtml:li[@class = 'secondary']" />
						<xsl:sort select="xhtml:td[@class = 'Phonetic item']//xhtml:li[@class = 'exported']" />
            <xsl:copy>
              <xsl:apply-templates select="@*" />
              <xsl:apply-templates>
                <xsl:with-param name="position" select="position()" />
                <xsl:with-param name="sortOrderFormat" select="$sortOrderFormat" />
              </xsl:apply-templates>
            </xsl:copy>
          </xsl:for-each>
        </xsl:when>
        <xsl:otherwise>
          <!-- In Data Corpus view, sort by the Phonetic field. -->
          <xsl:for-each select="xhtml:tr[not(@class = 'heading')]">
            <xsl:sort select="xhtml:td[@class = 'Phonetic']//xhtml:li[@class = $phoneticSortClass]" />
						<xsl:sort select="xhtml:td[@class = 'Phonetic']//xhtml:li[@class = 'secondary']" />
						<xsl:sort select="xhtml:td[@class = 'Phonetic']//xhtml:li[@class = 'exported']" />
            <xsl:copy>
              <xsl:apply-templates select="@*" />
              <xsl:apply-templates>
                <xsl:with-param name="position" select="position()" />
                <xsl:with-param name="sortOrderFormat" select="$sortOrderFormat" />
              </xsl:apply-templates>
            </xsl:copy>
          </xsl:for-each>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>

  <!-- Replace the sort key with the sort order. -->
  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th[@class = 'Phonetic' or @class = 'Phonetic item']/xhtml:ul[@class = 'sorting_order']/xhtml:li">
    <xsl:param name="position" />
    <xsl:param name="sortOrderFormat" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:choose>
        <xsl:when test="@class = $phoneticSortClass">
					<xsl:value-of select="format-number($position, $sortOrderFormat)" />
				</xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>

  <!-- Replace the sort key with the sort order. -->
  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:td[starts-with(@class, 'Phonetic')]/xhtml:ul[@class = 'sorting_order']/xhtml:li">
    <xsl:param name="position" />
    <xsl:param name="sortOrderFormat" />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:choose>
        <xsl:when test="@class = $phoneticSortClass">
          <xsl:value-of select="format-number($position, $sortOrderFormat)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>