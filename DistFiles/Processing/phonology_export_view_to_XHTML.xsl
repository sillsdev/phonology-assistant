<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_to_XHTML.xsl 2010-03-29 -->
  <!-- Converts any exported view to XHTML. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />
  <!-- Instead of specifying the !DOCTYPE declaration in xsl:output, -->
  <!-- specify output_to_XHTML_Strict.tidy or output_to_XHTML_Transitional.tidy in the conversion process. -->

  <!--
  <xsl:strip-space elements="*" />
  <xsl:preserve-space elements="xhtml:h1 xhtml:h2 xhtml:h3 xhtml:h4 xhtml:p xhtml:span xhtml:a xhtml:cite xhtml:em xhtml:strong" />
  -->

  <xsl:param name="langDefault" select="'en'" />
  <xsl:param name="meta-http-equiv-X-UA-Compatible" select="''" />
  <!-- Test compatibility with Internet Explorer 7. -->
  <!--
  <xsl:param name="meta-http-equiv-X-UA-Compatible" select="'IE=7'" />
  -->

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="options" select="$metadata/xhtml:div[@class = 'options']" />
	<xsl:variable name="details" select="$options/xhtml:ul[@class = 'view']/xhtml:li/xhtml:ul/xhtml:li[@class = 'details']" />
	<xsl:variable name="dateAndTime" select="$options/xhtml:ul[@class = 'view']/xhtml:li/xhtml:ul/xhtml:li[@class = 'dateAndTime']" />
  <xsl:variable name="genericRelativePathXHTML" select="$options/xhtml:ul[@class = 'format']/xhtml:li[@class = 'XHTML']/xhtml:ul/xhtml:li[@class = 'genericRelativePath']" />
  <xsl:variable name="specificRelativePathXHTML" select="$options/xhtml:ul[@class = 'format']/xhtml:li[@class = 'XHTML']/xhtml:ul/xhtml:li[@class = 'specificRelativePath']" />
  <xsl:variable name="specificStylesheetFileXHTML" select="$options/xhtml:ul[@class = 'format']/xhtml:li[@class = 'XHTML']/xhtml:ul/xhtml:li[@class = 'specificStylesheetFile']" />
  <xsl:variable name="nonBreakingSpaceInEmptyTableCellsXHTML" select="$options/xhtml:ul[@class = 'format']/xhtml:li[@class = 'XHTML']/xhtml:ul/xhtml:li[@class = 'nonBreakingSpaceInEmptyTableCell']" />
  <xsl:variable name="interactiveXHTML" select="$options/xhtml:ul[@class = 'format']/xhtml:li[@class = 'XHTML']/xhtml:ul/xhtml:li[@class = 'interactive']" />

	<xsl:variable name="markOfTheWeb" select="'about:internet'" />
	<xsl:param name="genericStylesheetFileXHTML" select="'phonology.css'" />
	<xsl:param name="genericStylesheetPrintFileXHTML" select="'phonology_print.css'" />
	<xsl:param name="jqueryScriptFile" select="'jquery.js'" />
  <xsl:param name="phonologyScriptFile" select="'phonology.js'" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="/xhtml:html">
    <xsl:variable name="lang" select="$langDefault" />
    <html xml:lang="{$lang}" lang="{$lang}" xmlns="http://www.w3.org/1999/xhtml">
      <!-- For more information about the mark of the Web, see http://msdn.microsoft.com/en-us/library/ms537628.aspx. -->
      <!-- Important: Here is the reason for inserting the mark of the Web following the html tag. -->
      <!-- MSXML XSL Transformations and HTML Tidy both move comments preceding the html tag, -->
      <!-- which is the default location of the mark, to the beginning of the file preceding the !DOCTYPE declaration. -->
      <!-- However, the !DOCTYPE declaration must be first to enable Standards mode in Internet Explorer. -->
      <xsl:variable name="markOfTheWebLength" select="string-length($markOfTheWeb)" />
      <xsl:if test="$markOfTheWebLength != 0">
        <xsl:comment>
          <xsl:value-of select="concat(' saved from url=(', format-number($markOfTheWebLength, '0000'), ')', $markOfTheWeb, ' ')" />
        </xsl:comment>
      </xsl:if>
      <head>
        <title>
          <xsl:value-of select="xhtml:head/xhtml:title" />
        </title>
        <meta http-equiv="content-type" content="text/html; charset=utf-8" />
        <xsl:if test="$meta-http-equiv-X-UA-Compatible != ''">
          <meta http-equiv="X-UA-Compatible" content="{$meta-http-equiv-X-UA-Compatible}" />
        </xsl:if>
				<!--
        <xsl:if test="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'researcher']">
          <meta name="author" content="{$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'researcher']}" />
        </xsl:if>
				-->
        <link rel="stylesheet" type="text/css" href="{concat($genericRelativePathXHTML, $genericStylesheetFileXHTML)}" media="all" />
				<link rel="stylesheet" type="text/css" href="{concat($genericRelativePathXHTML, $genericStylesheetPrintFileXHTML)}" media="print" />
				<xsl:choose>
          <xsl:when test="$specificStylesheetFileXHTML != ''">
            <link rel="stylesheet" type="text/css" href="{concat($specificRelativePathXHTML, $specificStylesheetFileXHTML)}" />
          </xsl:when>
          <xsl:when test="$metadata/xhtml:table[@class = 'formatting']">
            <xsl:call-template name="internalStylesheet">
              <xsl:with-param name="tableFormatting" select="$metadata/xhtml:table[@class = 'formatting']" />
            </xsl:call-template>
          </xsl:when>
        </xsl:choose>
        <xsl:if test="$interactiveXHTML = 'true'">
          <script type="text/javascript" src="{concat($genericRelativePathXHTML, $jqueryScriptFile)}"></script>
          <script type="text/javascript" src="{concat($genericRelativePathXHTML, $phonologyScriptFile)}"></script>
        </xsl:if>
      </head>
			<xsl:apply-templates select="xhtml:body" />
    </html>
  </xsl:template>

  <!-- Add cellspacing attribute to tables for Internet Explorer 7 and earlier. -->
  <xsl:template match="xhtml:table">
    <xsl:choose>
      <!-- If not interactive, omit tables of features. -->
      <xsl:when test="($interactiveXHTML = 'false') and (@class = 'articulatory features' or @class = 'binary features' or @class = 'hierarchical features')" />
      <xsl:otherwise>
        <xsl:copy>
          <xsl:apply-templates select="@*" />
          <xsl:attribute name="cellspacing">
            <xsl:value-of select="0" />
          </xsl:attribute>
          <xsl:apply-templates />
        </xsl:copy>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Insert a class for sorting in column headings of lists. -->
  <xsl:template match="xhtml:table[@class = 'list']/xhtml:thead//xhtml:th">
    <xsl:variable name="fieldName" select="." />
    <xsl:variable name="primarySortFieldName" select="$metadata/xhtml:ul[@class = 'sorting']/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]/@title" />
    <xsl:copy>
      <xsl:variable name="classThatIsTheSumOfTheParts">
        <xsl:if test="$fieldName = $primarySortFieldName">
          <xsl:value-of select="'sortField'" />
        </xsl:if>
        <xsl:if test="$interactiveXHTML = 'true'">
          <xsl:if test="$fieldName = 'Phonetic'">
            <!-- Phonetic sort options apply to whenever Phonetic is the primary sort field; but even if not, in ungrouped lists, . -->
            <!-- That is, omit the options in a list of minimal pairs or a list with generic groups for which Phonetic is not the primary sort field. -->
            <xsl:if test="$primarySortFieldName = 'Phonetic' or ancestor::xhtml:table[@class = 'list'][not(xhtml:tbody[@class = 'group'])]">
							<xsl:if test="not($metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'minimalPairs'])">
								<xsl:value-of select="' sortOptions'" />
								<xsl:if test="$metadata/xhtml:ul[@class = 'sorting']/xhtml:li[@class = 'phoneticSortOption'] = 'mannerOfArticulation'">
									<xsl:value-of select="' mannerOfArticulation'" />
								</xsl:if>
							</xsl:if>
            </xsl:if>
          </xsl:if>
          <xsl:if test="$metadata/xhtml:ul[@class = 'sorting']/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[@title = $fieldName] = 'descending'">
            <xsl:value-of select="' descending'" />
          </xsl:if>
        </xsl:if>
      </xsl:variable>
      <xsl:variable name="class">
        <xsl:choose>
          <xsl:when test="starts-with($classThatIsTheSumOfTheParts, ' ')">
            <xsl:value-of select="substring($classThatIsTheSumOfTheParts, 2)" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$classThatIsTheSumOfTheParts" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:if test="string-length($class) != 0">
        <xsl:attribute name="class">
          <xsl:value-of select="$class" />
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:ul[@class = 'sortOrder' or @class = 'articulatory' or @class = 'binary']">
    <!-- If interactive, include lists of numbers for sorting in lists or features for phones in charts. -->
    <xsl:if test="$interactiveXHTML = 'true'">
      <xsl:copy>
        <xsl:apply-templates select="@*|node()" />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

  <xsl:template match="xhtml:span[following-sibling::xhtml:ul[@class = 'sortOrder'] or xhtml:div[@class = 'differences' or @class = 'features']]">
    <xsl:choose>
      <xsl:when test="$interactiveXHTML = 'true'">
        <!-- If interactive, include span enclosing text followed by lists of numbers for sorting in lists or features for phones in charts. -->
        <xsl:copy>
          <xsl:apply-templates select="@*|node()" />
        </xsl:copy>
      </xsl:when>
      <xsl:otherwise>
        <!-- Omit the span. -->
        <xsl:apply-templates />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- For :hover formatting, remove Phonetic class from empty cells in CV chart. -->
  <xsl:template match="xhtml:table[@class = 'CV chart']//xhtml:td[not(node())][@class = 'Phonetic']/@class" />

  <!-- There is an option to insert a non-breaking space in empty table cells -->
  <!-- for the borders in Internet Explorer 7 and earlier. -->
  <xsl:template match="xhtml:table//xhtml:tr/xhtml:*[not(node())]">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:if test="$nonBreakingSpaceInEmptyTableCellsXHTML = 'true'">
        <xsl:value-of select="'&#xA0;'" />
      </xsl:if>
    </xsl:copy>
  </xsl:template>
  
  <!-- Exception: In charts or grouped lists, the upper-left cell can be empty. -->
  <xsl:template match="xhtml:table[contains(@class, 'chart') or (@class = 'list' and xhtml:tbody[@class = 'group'])]/xhtml:thead/xhtml:tr[1]/xhtml:th[1]">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <!-- At the beginning or end of the text in a table cell, replace space with non-breaking space. -->
  <xsl:template match="xhtml:tr/*//text()">
    <xsl:variable name="text" select="." />
    <xsl:variable name="text-replace-start">
      <xsl:choose>
        <xsl:when test="starts-with($text, ' ')">
          <xsl:value-of select="concat('&#xA0;', substring($text, 2))" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$text" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="substring($text-replace-start, string-length($text-replace-start) ,1) = ' '">
        <xsl:value-of select="concat(substring($text-replace-start, 1, string-length($text-replace-start) - 1), '&#xA0;')" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text-replace-start" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Convert table of field formatting properties to style rules. -->
  
  <xsl:template name="internalStylesheet">
    <xsl:param name="tableFormatting" />
    <style type="text/css" xmlns="http://www.w3.org/1999/xhtml">
      <xsl:for-each select="$tableFormatting/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'class']/text()]">
        <xsl:value-of select="concat('.', xhtml:td[@class = 'class']/text(), '&#xA;{&#xA;')" />
        <xsl:if test="xhtml:td[@class = 'font-family']/text()">
          <xsl:variable name="font-family" select="xhtml:td[@class = 'font-family']/text()" />
          <xsl:choose>
            <xsl:when test="contains($font-family, ' ')">
              <xsl:value-of select="concat('font-family: &quot;', $font-family, '&quot;;&#xA;')" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="concat('font-family: ', $font-family, ';&#xA;')" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
        <xsl:if test="xhtml:td[@class = 'font-size']/text()">
          <xsl:variable name="font-size" select="xhtml:td[@class = 'font-size']/text()" />
          <xsl:value-of select="concat('font-size: ', format-number(100 * $font-size div 12, '.00'), '%; /* ', $font-size, 'pt */&#xA;')" />
        </xsl:if>
        <xsl:if test="xhtml:td[@class = 'font-weight']/text()">
          <xsl:value-of select="concat('font-weight: ', xhtml:td[@class = 'font-weight']/text(), ';&#xA;')" />
        </xsl:if>
        <xsl:if test="xhtml:td[@class = 'font-style']/text()">
          <xsl:value-of select="concat('font-style: ', xhtml:td[@class = 'font-style']/text(), ';&#xA;')" />
        </xsl:if>
        <xsl:value-of select="'}&#xA;'" />
      </xsl:for-each>
    </style>
  </xsl:template>
  
  <!-- Table of details. -->

	<xsl:template match="xhtml:div[@id = 'metadata']">
		<xsl:if test="$details = 'true'">
			<xsl:variable name="sorting" select="$metadata/xhtml:ul[@class = 'sorting']" />
			<xsl:variable name="primarySortFieldName" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]/@title" />
			<xsl:variable name="primarySortFieldDirection" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]" />
			<xsl:variable name="phoneticSortOption" select="$sorting/xhtml:li[@class = 'phoneticSortOption']" />
			<xsl:variable name="phoneticSortOptionName">
				<xsl:choose>
					<xsl:when test="$phoneticSortOption = 'placeOfArticulation'">
						<xsl:value-of select="'place of articulation'" />
					</xsl:when>
					<xsl:when test="$phoneticSortOption = 'mannerOfArticulation'">
						<xsl:value-of select="'manner of articulation'" />
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="searchPattern" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'searchPattern']" />
			<xsl:variable name="filter" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'filter']" />
			<xsl:variable name="numberOfPhones" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfPhones']" />
			<xsl:variable name="numberOfPhonemes" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfPhonemes']" />
			<xsl:variable name="numberOfRecords" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfRecords']" />
			<xsl:variable name="numberOfGroups" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfGroups']" />
			<xsl:variable name="projectName" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'projectName']" />
			<xsl:variable name="languageName" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'languageName']" />
			<xsl:variable name="languageCode" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'languageCode']" />
			<xsl:variable name="date" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'date']" />
			<xsl:variable name="time" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'time']" />
			<h4 xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="/xhtml:html/xhtml:head/xhtml:title" />
			</h4>
			<table class="details" cellspacing="0" xmlns="http://www.w3.org/1999/xhtml">
				<col />
				<col />
				<tbody>
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
						<xsl:when test="$numberOfPhones">
							<tr class="numberOfPhones" xmlns="http://www.w3.org/1999/xhtml">
								<th scope="row">
									<xsl:value-of select="'Number of phones'" />
								</th>
								<td>
									<xsl:value-of select="$numberOfPhones" />
								</td>
							</tr>
						</xsl:when>
						<xsl:when test="$numberOfPhonemes">
							<tr class="numberOfPhonemes" xmlns="http://www.w3.org/1999/xhtml">
								<th scope="row">
									<xsl:value-of select="'Number of Phonemes'" />
								</th>
								<td>
									<xsl:value-of select="$numberOfPhonemes" />
								</td>
							</tr>
						</xsl:when>
						<xsl:when test="$numberOfRecords">
							<tr class="numberOfRecords">
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
						<tr class="numberOfGroups">
							<th scope="row">
								<xsl:value-of select="'Number of groups:'" />
							</th>
							<td>
								<xsl:value-of select="$numberOfGroups" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$sorting">
						<tr class="primarySortField" xmlns="http://www.w3.org/1999/xhtml">
							<th scope="row">
								<xsl:value-of select="'Primary sort field:'" />
							</th>
							<td>
								<xsl:value-of select="$primarySortFieldName" />
								<xsl:if test="$primarySortFieldName = 'Phonetic' and $phoneticSortOptionName">
									<xsl:value-of select="concat(', ', $phoneticSortOptionName)" />
								</xsl:if>
								<xsl:if test="$primarySortFieldDirection = 'descending'">
									<xsl:value-of select="concat(', ', $primarySortFieldDirection)" />
								</xsl:if>
							</td>
						</tr>
						<!-- TO DO: Minimal pairs options. -->
					</xsl:if>
					<!-- TO DO: Researcher? -->
					<xsl:if test="$projectName != $languageName">
						<tr class="projectName">
							<th scope="row">
								<xsl:value-of select="'Project name:'" />
							</th>
							<td>
								<xsl:value-of select="$projectName" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$languageName">
						<tr class="languageName">
							<th scope="row">
								<xsl:value-of select="'Language name:'" />
							</th>
							<td>
								<xsl:value-of select="$languageName" />
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="$languageCode">
						<tr class="languageCode">
							<th scope="row">
								<xsl:value-of select="'ISO 639-3 code:'" />
							</th>
							<td>
								<xsl:choose>
									<xsl:when test="string-length($languageCode) = 3 and string-length(translate($languageCode, 'abcdefghijklmnopqrstuvwxyz', '')) = 0">
										<a href="{concat('http://www.ethnologue.com/show_language.asp?code=', $languageCode)}" title="http://www.ethnologue.com">
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
					<xsl:if test="$dateAndTime = 'true' and $date and $time">
						<tr class="dateAndTime">
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

</xsl:stylesheet>