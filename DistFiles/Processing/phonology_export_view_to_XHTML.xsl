<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_to_XHTML.xsl 2010-05-13 -->
  <!-- Converts any exported view to XHTML. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="yes" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd" />

	<!-- Phonology Assistant interprets indent="yes" to mean insert line breaks in the output but omit indentation. -->
	<!-- Assume that the input contains no unnecessary white space. If it might, uncomment strip-space and preserve-space.  -->
  <!--
  <xsl:strip-space elements="*" />
  <xsl:preserve-space elements="xhtml:h1 xhtml:h2 xhtml:h3 xhtml:h4 xhtml:p xhtml:span xhtml:a xhtml:cite xhtml:em xhtml:strong" />
  -->

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="minimalPairs" select="$details/xhtml:li[@class = 'minimalPairs']" />
	<xsl:variable name="researcher" select="$details/xhtml:li[@class = 'researcher']" />

	<xsl:variable name="formatting" select="$metadata/xhtml:table[@class = 'formatting']" />
	<xsl:variable name="sorting" select="$metadata/xhtml:ul[@class = 'sorting']" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="interactiveWebPage" select="$options/xhtml:li[@class = 'interactiveWebPage']" />
	<xsl:variable name="genericStylesheetForIE7" select="$options/xhtml:li[@class = 'genericStylesheetForIE7']" />
	<xsl:variable name="genericStylesheetForIE6" select="$options/xhtml:li[@class = 'genericStylesheetForIE6']" />
	<xsl:variable name="tableOfDetails" select="$options/xhtml:li[@class = 'tableOfDetails']" />
	<xsl:variable name="hyperlinkToEthnologue" select="$options/xhtml:li[@class = 'hyperlinkToEthnologue']" />
	<xsl:variable name="dateAndTime" select="$options/xhtml:li[@class = 'dateAndTime']" />
	<xsl:variable name="oneMinimalPairPerGroup" select="$options/xhtml:li[@class = 'oneMinimalPairPerGroup']" />
	<xsl:variable name="genericRelativePath" select="$options/xhtml:li[@class = 'genericRelativePath']" />
	<xsl:variable name="specificRelativePath" select="$options/xhtml:li[@class = 'specificRelativePath']" />
  <xsl:variable name="specificStylesheetFile" select="$options/xhtml:li[@class = 'specificStylesheetFile']" />

	<xsl:variable name="nonBreakingSpaceInEmptyTableCell" select="'true'" />
	<!-- TO DO: title, heading, p elements also? -->

	<xsl:param name="langDefault" select="'en'" />
	<!-- For Internet Explorer, specify Internet security zone instead of Local Machine. -->
	<xsl:param name="markOfTheWeb" select="'about:internet'" />
	<!-- For Internet Explorer, use the most recent installed version, instead of version 7. -->
	<!-- Necessary for the HTML view of CV chart in Phonology Assistant. -->
	<!-- Might also help with exported files on an intranet, including a file server. -->
	<xsl:param name="X-UA-Compatible" select="'IE=edge'" />
	<xsl:param name="genericStylesheetFile" select="'phonology.css'" />
	<xsl:param name="genericStylesheetFilePrint" select="'phonology_print.css'" />
	<xsl:param name="genericStylesheetFileIE7" select="'phonology_IE7.css'" />
	<xsl:param name="genericStylesheetFileIE6" select="'phonology_IE6.css'" />
	<xsl:param name="jqueryScriptFile" select="'jquery.js'" />
  <xsl:param name="phonologyScriptFile" select="'phonology.js'" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
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
				<meta http-equiv="content-type" content="text/html; charset=utf-8" />
				<xsl:if test="string-length($X-UA-Compatible) != 0">
					<meta http-equiv="X-UA-Compatible" content="{$X-UA-Compatible}" />
				</xsl:if>
				<title>
          <xsl:value-of select="xhtml:head/xhtml:title" />
        </title>
				<xsl:if test="string-length($researcher) != 0">
					<meta name="author" content="{$researcher}" />
				</xsl:if>
				<link rel="stylesheet" type="text/css" href="{concat($genericRelativePath, $genericStylesheetFile)}" media="all" />
				<link rel="stylesheet" type="text/css" href="{concat($genericRelativePath, $genericStylesheetFilePrint)}" media="print" />
				<xsl:if test="$genericStylesheetForIE7 = 'true'">
					<!-- Here is an example of a conditional comment containing a style sheet: -->
					<!--[if lte IE 7]><link rel="stylesheet" type="text/css" href="../phonology_IE7.css" /><![endif]-->
					<xsl:comment>
						<xsl:value-of select="'[if lte IE 7]&gt;'" />
						<xsl:value-of select="concat('&lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;', $genericRelativePath, $genericStylesheetFileIE7, '&quot; /&gt;')" />
						<xsl:value-of select="'&lt;![endif]'" />
					</xsl:comment>
					<xsl:if test="$genericStylesheetForIE6 = 'true'">
						<!-- The IE6.css file assumes that the IE7.css file precedes it. -->
						<!-- That is, you can support IE7 but not IE6; but to support IE6, you must support IE7. -->
						<!--[if lte IE 6]><link rel="stylesheet" type="text/css" href="../phonology_IE6.css" /><![endif]-->
						<xsl:comment>
							<xsl:value-of select="'[if lte IE 6]&gt;'" />
							<xsl:value-of select="concat('&lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;', $genericRelativePath, $genericStylesheetFileIE6, '&quot; /&gt;')" />
							<xsl:value-of select="'&lt;![endif]'" />
						</xsl:comment>
					</xsl:if>
				</xsl:if>
				<xsl:choose>
          <xsl:when test="string-length($specificStylesheetFile) != 0">
            <link rel="stylesheet" type="text/css" href="{concat($specificRelativePath, $specificStylesheetFile)}" />
          </xsl:when>
					<xsl:when test="$formatting">
						<xsl:call-template name="internalStylesheet">
              <xsl:with-param name="formatting" select="$formatting" />
            </xsl:call-template>
          </xsl:when>
        </xsl:choose>
      </head>
			<body>
				<xsl:apply-templates select="xhtml:body/*" />
				<!-- To reduce potential delay in loading content, script elements are at the end of the body. -->
				<!-- The comments hide the script from Internet Explorer 6 and earlier. -->
				<xsl:comment>
					<xsl:value-of select="'[if gte IE 7]&gt;&lt;!'" />
				</xsl:comment>
				<!-- Newline necessary to force start and end tags on separate lines. -->
				<xsl:if test="$interactiveWebPage = 'true'">
					<script type="text/javascript" src="{concat($genericRelativePath, $jqueryScriptFile)}">
						<xsl:value-of select="'&#xA;'" />
					</script>
					<script type="text/javascript" src="{concat($genericRelativePath, $phonologyScriptFile)}">
						<xsl:value-of select="'&#xA;'" />
					</script>
					<xsl:comment>
						<xsl:value-of select="'&lt;![endif]'" />
					</xsl:comment>
				</xsl:if>
			</body>
    </html>
  </xsl:template>

	<!-- Internet Explorer 7 and earlier require a cellspacing attribute for tables, -->
	<!-- because they do not implement the border-spacing property in CSS. -->
  <xsl:template match="xhtml:table">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="not(@cellspacing)">
				<xsl:attribute name="cellspacing">
					<xsl:value-of select="0" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

  <!-- Insert a class for sorting in column headings of some lists. -->
  <xsl:template match="xhtml:table[@class = 'list']/xhtml:thead//xhtml:th">
    <xsl:variable name="fieldName" select="." />
    <xsl:variable name="primarySortFieldName" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[1]/@title" />
		<xsl:variable name="class">
			<xsl:if test="$fieldName = $primarySortFieldName">
				<xsl:value-of select="'sortField'" />
			</xsl:if>
			<xsl:if test="$interactiveWebPage = 'true'">
				<!-- Phonetic sort options for Phonetic, except when there is one minimal pair per group. -->
				<xsl:if test="$fieldName = 'Phonetic' and not($minimalPairs and $oneMinimalPairPerGroup = 'true')">
					<xsl:if test="$fieldName = $primarySortFieldName">
						<xsl:value-of select="' '" />
					</xsl:if>
					<xsl:value-of select="'sortOptions'" />
				</xsl:if>
			</xsl:if>
		</xsl:variable>
		<xsl:copy>
      <xsl:if test="string-length($class) != 0">
        <xsl:attribute name="class">
          <xsl:value-of select="$class" />
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$fieldName = $primarySortFieldName">
					<span xmlns="http://www.w3.org/1999/xhtml">
						<xsl:value-of select="." />
					</span>
					<xsl:value-of select="'&#xA;'" />
					<ins xmlns="http://www.w3.org/1999/xhtml">
						<xsl:variable name="fieldOrder" select="$sorting/xhtml:li[@class = 'fieldOrder']/xhtml:ol/xhtml:li[@title = $fieldName]" />
						<xsl:choose>
							<xsl:when test="$fieldName = 'Phonetic'">
								<!-- Phonetic or (someday) Phonemic. -->
								<xsl:choose>
									<xsl:when test="$sorting/xhtml:li[@class = 'phoneticSortOption'] = 'mannerOfArticulation'">
										<xsl:choose>
											<xsl:when test="$fieldOrder = 'descending'">
												<!-- black down-pointing triangle -->
												<xsl:value-of select="'&#x25BC;'" />
											</xsl:when>
											<xsl:otherwise>
												<!-- black up-pointing triangle -->
												<xsl:value-of select="'&#x25B2;'" />
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<!-- placeOfArticulation -->
										<xsl:choose>
											<xsl:when test="$fieldOrder = 'descending'">
												<!-- black right-pointing triangle -->
												<xsl:value-of select="'&#x25B6;'" />
											</xsl:when>
											<xsl:otherwise>
												<!-- black left-pointing triangle -->
												<xsl:value-of select="'&#x25C0;'" />
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$fieldOrder = 'descending'">
										<!-- black down-pointing small triangle -->
										<xsl:value-of select="'&#x25BE;'" />
									</xsl:when>
									<xsl:otherwise>
										<!-- black up-pointing small triangle -->
										<xsl:value-of select="'&#x25B4;'" />
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</ins>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="." />
				</xsl:otherwise>
			</xsl:choose>
    </xsl:copy>
  </xsl:template>

  <!-- For interactive :hover formatting, remove Phonetic class from empty data cells in CV charts. -->
  <xsl:template match="xhtml:table[@class = 'CV chart']//xhtml:td[not(node())][@class = 'Phonetic']/@class" />

	<!-- Temporarily provide class="zero" until the Phonology Assistant program can. -->
	<xsl:template match="xhtml:table[@class = 'distribution chart']//xhtml:td[. = '0'][not(@class)]">
		<xsl:copy>
			<xsl:attribute name="class">
				<xsl:value-of select="'zero'" />
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- For borders in Internet Explorer 7 and earlier, insert a non-breaking space in most empty table cells. -->
	<!-- This replacement requires a special case to sort non-Phonetic columns in the phonology.js file. -->
	<xsl:template match="xhtml:tr/xhtml:*[not(node())]">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:if test="$nonBreakingSpaceInEmptyTableCell = 'true'">
        <xsl:value-of select="'&#xA0;'" />
      </xsl:if>
    </xsl:copy>
  </xsl:template>
  
  <!-- Exceptions: In charts, the upper-left cell can be empty. -->
  <xsl:template match="xhtml:table[contains(@class, 'chart')]/xhtml:thead/xhtml:tr[1]/xhtml:th[1]">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <!-- At the beginning or end of the text in a table cell, replace space with non-breaking space. -->
	<!-- This replacement requires a special case to sort non-Phonetic columns in the phonology.js file. -->
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

  <!-- Convert table of field formatting properties to CSS style rules. -->
	<!-- Similar to phonology_export_view_to_CSS.xsl for an external style sheet file. -->
  <xsl:template name="internalStylesheet">
    <xsl:param name="formatting" />
    <style type="text/css" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="'&#xA;'" />
      <xsl:for-each select="$formatting/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'class']/text()]">
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
		<xsl:if test="$tableOfDetails = 'true'">
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
			<xsl:variable name="phoneticSearchSubfieldOrder" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol" />
			<xsl:variable name="view" select="$details//xhtml:li[@class = 'view']" />
			<xsl:variable name="searchPattern" select="$details/xhtml:li[@class = 'searchPattern']" />
			<xsl:variable name="filter" select="$details/xhtml:li[@class = 'filter']" />
			<xsl:variable name="numberOfPhones" select="$details/xhtml:li[@class = 'numberOfPhones']" />
			<xsl:variable name="numberOfPhonemes" select="$details/xhtml:li[@class = 'numberOfPhonemes']" />
			<xsl:variable name="numberOfRecords" select="$details/xhtml:li[@class = 'numberOfRecords']" />
			<xsl:variable name="numberOfGroups" select="$details/xhtml:li[@class = 'numberOfGroups']" />
			<xsl:variable name="minimalPairs" select="$details/xhtml:li[@class = 'minimalPairs']" />
			<xsl:variable name="projectName" select="$details/xhtml:li[@class = 'projectName']" />
			<xsl:variable name="languageName" select="$details/xhtml:li[@class = 'languageName']" />
			<xsl:variable name="languageCode" select="$details/xhtml:li[@class = 'languageCode']" />
			<xsl:variable name="date" select="$details/xhtml:li[@class = 'date']" />
			<xsl:variable name="time" select="$details/xhtml:li[@class = 'time']" />
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
									<xsl:value-of select="'Number of phones:'" />
								</th>
								<td>
									<xsl:value-of select="$numberOfPhones" />
								</td>
							</tr>
						</xsl:when>
						<xsl:when test="$numberOfPhonemes">
							<tr class="numberOfPhonemes" xmlns="http://www.w3.org/1999/xhtml">
								<th scope="row">
									<xsl:value-of select="'Number of Phonemes:'" />
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
					<xsl:if test="$minimalPairs">
						<tr class="minimalPairs" xmlns="http://www.w3.org/1999/xhtml">
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
						<tr class="sortField" xmlns="http://www.w3.org/1999/xhtml">
							<th scope="row">
								<xsl:value-of select="'Sort field:'" />
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
						<xsl:if test="$view = 'Search' and $phoneticSearchSubfieldOrder">
							<tr class="phoneticSortOptions" xmlns="http://www.w3.org/1999/xhtml">
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
					<xsl:if test="string-length($languageCode) != 0">
						<tr class="languageCode">
							<th scope="row">
								<xsl:value-of select="'ISO 639-3 code:'" />
							</th>
							<td>
								<xsl:choose>
									<xsl:when test="$hyperlinkToEthnologue and string-length($languageCode) = 3 and string-length(translate($languageCode, 'abcdefghijklmnopqrstuvwxyz', '')) = 0">
										<a href="{concat('http://www.ethnologue.com/show_language.asp?code=', $languageCode)}" title="www.ethnologue.com">
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