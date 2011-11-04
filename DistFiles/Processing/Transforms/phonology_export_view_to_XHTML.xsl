<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
xmlns:svg="http://www.w3.org/2000/svg"
exclude-result-prefixes="xhtml svg"
>

  <!-- phonology_export_view_to_XHTML.xsl 2011-11-04 -->
  <!-- Converts any exported view to XHTML. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="yes" />

	<!-- Insert CDATA sections within style elements. -->
	<xsl:output cdata-section-elements="svg:style xhtml:style" />

	<!-- Phonology Assistant interprets indent="yes" to mean insert line breaks in the output but omit indentation. -->
	<!-- Assume that the input contains no unnecessary white space. If it might, uncomment strip-space and preserve-space.  -->
  <!--
  <xsl:strip-space elements="*" />
  <xsl:preserve-space elements="xhtml:h1 xhtml:h2 xhtml:h3 xhtml:h4 xhtml:p xhtml:span xhtml:a xhtml:cite xhtml:em xhtml:strong" />
  -->

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="pairs" select="$details/xhtml:li[@class = 'pairs']" />
	<xsl:variable name="researcher" select="$details/xhtml:li[@class = 'researcher']" />

	<xsl:variable name="formatting" select="$metadata/xhtml:table[@class = 'formatting']" />
	<xsl:variable name="sorting" select="$metadata/xhtml:ul[@class = 'sorting']" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="interactiveWebPage" select="$options/xhtml:li[@class = 'interactiveWebPage']" />
	<xsl:variable name="genericStylesheetForCSS3" select="$options/xhtml:li[@class = 'genericStylesheetForCSS3']" />
	<xsl:variable name="genericStylesheetForIE8" select="$options/xhtml:li[@class = 'genericStylesheetForIE8']" />
	<xsl:variable name="genericStylesheetForIE7" select="$options/xhtml:li[@class = 'genericStylesheetForIE7']" />
	<xsl:variable name="genericStylesheetForIE6" select="$options/xhtml:li[@class = 'genericStylesheetForIE6']" />
	<xsl:variable name="oneMinimalPairPerGroup" select="$options/xhtml:li[@class = 'oneMinimalPairPerGroup']" />
	<xsl:variable name="genericRelativePath" select="$options/xhtml:li[@class = 'genericRelativePath']" />
	<xsl:variable name="specificRelativePath" select="$options/xhtml:li[@class = 'specificRelativePath']" />
  <xsl:variable name="specificStylesheetFile" select="$options/xhtml:li[@class = 'specificStylesheetFile']" />

	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />

	<xsl:param name="langDefault" select="'en'" />
	<!-- For Internet Explorer, specify Internet security zone instead of Local Machine. -->
	<xsl:param name="markOfTheWeb" select="'about:internet'" />
	<!-- For Internet Explorer, use the most recent installed version, instead of version 7. -->
	<!-- Necessary for the HTML view of CV chart in Phonology Assistant. -->
	<!-- Might also help with exported files on an intranet, including a file server. -->
	<xsl:param name="X-UA-Compatible" select="'IE=edge'" />
	<xsl:param name="genericStylesheetFile" select="'phonology.css'" />
	<xsl:param name="genericStylesheetFilePrint" select="'phonology_print.css'" />
	<xsl:param name="genericStylesheetFileCSS3" select="'phonology_CSS3.css'" />
	<xsl:param name="genericStylesheetFileIE8" select="'phonology_IE8.css'" />
	<xsl:param name="genericStylesheetFileIE7" select="'phonology_IE7.css'" />
	<xsl:param name="genericStylesheetFileIE6" select="'phonology_IE6.css'" />
	<xsl:param name="jqueryScriptFile" select="'jquery.js'" />
	<xsl:param name="jqueryuiScriptFile" select="'jquery-ui.js'" />
	<xsl:param name="phonologyScriptFile" select="'phonology.js'" />
	<xsl:param name="phonologyScriptFileQuadrilateralIE6" select="'phonology_quadrilateral_IE6.js'" />
	<xsl:param name="phonologyScriptFileQuadrilateral" select="'phonology_quadrilateral_interactive.js'" />

	<xsl:variable name="numberOfMonophthongs" select="count(//xhtml:div[starts-with(@class, 'quadrilateral')]//svg:g[@class = 'data']/svg:g[@class = 'monophthong'])" />
	<xsl:variable name="numberOfDiphthongs" select="count(//xhtml:div[starts-with(@class, 'quadrilateral')]//svg:g[@class = 'data']/svg:g[@class = 'diphthong'])" />

	<xsl:param name="srcQuadrilateralGrid"	select="concat($genericRelativePath, 'phonology_quadrilateral_grid.png')" />
	<xsl:param name="srcQuadrilateralPoint"	select="concat($genericRelativePath, 'phonology_quadrilateral_point.png')" />
	<xsl:param name="srcQuadrilateralEllipse"	select="concat($genericRelativePath, 'phonology_quadrilateral_ellipse.png')" />
	<xsl:param name="srcQuadrilateralDiphthongEnd"	select="concat($genericRelativePath, 'phonology_quadrilateral_diphthong_end.png')" />

	<xsl:param name="dxQuadrilateral" select="'0.25'" />
	<xsl:param name="dyQuadrilateral" select="'-0.625'" />
	<xsl:variable name="styleLeft" select="concat('right:', $dxQuadrilateral, 'em;top:', $dyQuadrilateral, 'em;')" />
	<xsl:variable name="styleRight" select="concat('left:', $dxQuadrilateral, 'em;top:', $dyQuadrilateral, 'em;')" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>
	
	<!-- Vowel quadrilaterals -->

	<!-- Copy parent div only if there are two quadrilaterals (that is, monophthongs and diphthongs). -->
	<xsl:template match="xhtml:div[@class = 'quadrilaterals']">
		<xsl:choose>
			<xsl:when test="xhtml:div[@class = 'quadrilateral left']//svg:g[@class = 'data']/svg:g and xhtml:div[@class = 'quadrilateral right']//svg:g[@class = 'data']/svg:g">
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="xhtml:div[starts-with(@class, 'quadrilateral')]" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Convert child div only if it contains vowel segments. -->
	<xsl:template match="xhtml:div[@class = 'quadrilaterals']/xhtml:div[starts-with(@class, 'quadrilateral')]">
		<xsl:if test=".//svg:g[@class = 'data']/svg:g">
			<xsl:variable name="classInput" select="@class" />
			<xsl:variable name="classOutput">
				<xsl:choose>
					<xsl:when test="../xhtml:div[starts-with(@class, 'quadrilateral') and @class != $classInput]//svg:g[@class = 'data']/svg:g">
						<xsl:value-of select="$classInput" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'quadrilateral'" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<div class="{$classOutput}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:comment>
					<xsl:value-of select="'[if lt IE 9]&gt;&#xA;'" />
					<xsl:value-of select="'&lt;p&gt;To display vowel quadrilaterals in Phonology Assistant, you need Internet Explorer 9 or later.&lt;/p&gt;&#xA;'" />
					<xsl:value-of select="'&lt;p&gt;You can display exported vowel quadrilateral files in any of the following Web browsers: Chrome, Firefox 1.5 or later, Internet Explorer 9 or later, Opera 9 or later, Safari 3 or later.&lt;/p&gt;&#xA;'" />
					<xsl:value-of select="'&lt;![endif]'" />
				</xsl:comment>
				<!-- Internet Explorer 9 supports SVG. -->
				<!-- The following comments hide the script from Internet Explorer 8 and earlier. -->
				<!--
				<xsl:comment>
					<xsl:value-of select="'[if ge IE 9]&gt;&lt;!'" />
				</xsl:comment>
				-->
				<xsl:apply-templates />
				<!--
				<xsl:comment>
					<xsl:value-of select="'&lt;![endif]'" />
				</xsl:comment>
				-->
				<!-- Force line break between comment and end tag of div. -->
				<!--
				<xsl:value-of select="'&#xA;'" />
				-->
				<!--
					<img class="grid" src="{$srcQuadrilateralGrid}" alt="grid" title="" />
					<xsl:apply-templates select="xhtml:ul[@class = 'data']" />
					-->
					<!-- Internet Explorer 9 supports SVG. -->
					<!-- The comments hide the script from Internet Explorer 8 and earlier. -->
				<!--
					<xsl:comment>
						<xsl:value-of select="'[if gt IE 8]&gt;&lt;!'" />
					</xsl:comment>
				<xsl:comment>
					<xsl:value-of select="'&lt;![endif]'" />
				</xsl:comment>
				-->
			</div>
		</xsl:if>
	</xsl:template>

	<!-- Convert descriptive features of vowel segment to attributes and one or more img elements. -->
	<xsl:template match="xhtml:div[starts-with(@class, 'quadrilateral')]/xhtml:ul[@class = 'data']/xhtml:li">
		<xsl:variable name="classItem" select="@class" />
		<xsl:variable name="title" select="@title" />
		<xsl:variable name="literal" select="xhtml:span" />
		<xsl:variable name="lang" select="xhtml:span/@lang" />
		<xsl:variable name="height" select="xhtml:ul/xhtml:li[@class = 'height']" />
		<xsl:variable name="backness" select="xhtml:ul/xhtml:li[@class = 'backness']" />
		<xsl:variable name="rounding" select="xhtml:ul/xhtml:li[@class = 'rounding']" />
		<xsl:variable name="sequence" select="xhtml:ul/xhtml:li[@class = 'sequence']" />
		<xsl:variable name="classSpan">
			<xsl:choose>
				<xsl:when test="$rounding = 'unrounded'">
					<xsl:value-of select="'left'" />
				</xsl:when>
				<xsl:when test="$rounding = 'rounded'">
					<xsl:value-of select="'right'" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="y">
			<xsl:call-template name="y">
				<xsl:with-param name="height" select="$height" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="x">
			<xsl:call-template name="x">
				<xsl:with-param name="backness" select="$backness" />
				<xsl:with-param name="y" select="$y" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="styleItem">
			<xsl:call-template name="formatPropertyValue">
				<xsl:with-param name="property" select="'left'" />
				<xsl:with-param name="value" select="$x" />
			</xsl:call-template>
			<xsl:call-template name="formatPropertyValue">
				<xsl:with-param name="property" select="'top'" />
				<xsl:with-param name="value" select="$y" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="styleSpan">
			<xsl:choose>
				<xsl:when test="$rounding = 'unrounded'">
					<xsl:value-of select="$styleLeft" />
				</xsl:when>
				<xsl:when test="$rounding = 'rounded'">
					<xsl:value-of select="$styleRight" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy>
			<xsl:if test="string-length($classItem) != 0">
				<xsl:attribute name="class">
					<xsl:value-of select="$classItem" />
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="style">
				<xsl:value-of select="$styleItem" />
			</xsl:attribute>
			<xsl:if test="string-length($title) != 0">
				<xsl:attribute name="title">
					<xsl:value-of select="$title" />
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$sequence = 'diphthong'">
					<xsl:variable name="y2">
						<xsl:call-template name="y">
							<xsl:with-param name="height" select="xhtml:ul/xhtml:li[@class = 'height secondary']" />
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="x2">
						<xsl:call-template name="x">
							<xsl:with-param name="backness" select="xhtml:ul/xhtml:li[@class = 'backness secondary']" />
							<xsl:with-param name="y" select="$y2" />
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="styleDiphthongEnd">
						<xsl:call-template name="formatPropertyValue">
							<xsl:with-param name="property" select="'left'" />
							<xsl:with-param name="value" select="$x2 - $x" />
						</xsl:call-template>
						<xsl:call-template name="formatPropertyValue">
							<xsl:with-param name="property" select="'top'" />
							<xsl:with-param name="value" select="$y2 - $y" />
						</xsl:call-template>
					</xsl:variable>
					<img class="beginning" src="{$srcQuadrilateralPoint}" alt="{concat('beginning of ', $title)}" xmlns="http://www.w3.org/1999/xhtml" />
					<img class="end" style="{$styleDiphthongEnd}" src="{$srcQuadrilateralDiphthongEnd}" alt="{concat('end of ', $title)}" xmlns="http://www.w3.org/1999/xhtml" />
				</xsl:when>
				<xsl:otherwise>
					<img class="point" src="{$srcQuadrilateralPoint}" alt="{concat('point for ', $title)}" xmlns="http://www.w3.org/1999/xhtml" />
				</xsl:otherwise>
			</xsl:choose>
			<span class="{$classSpan}" style="{$styleSpan}" lang="{$lang}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="$literal" />
			</span>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="x">
		<xsl:param name="backness" />
		<xsl:param name="y" />
		<xsl:choose>
			<xsl:when test="$backness = 'front'">
				<xsl:value-of select="2 * $y div 3" />
			</xsl:when>
			<xsl:when test="$backness = 'near-front'">
				<xsl:value-of select="5 div 6" />
			</xsl:when>
			<xsl:when test="$backness = 'central'">
				<xsl:value-of select="2 + $y div 3" />
			</xsl:when>
			<xsl:when test="$backness = 'near-back'">
				<xsl:value-of select="4 - 5 div 6" />
			</xsl:when>
			<xsl:when test="$backness = 'back'">
				<xsl:value-of select="4" />
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="y">
		<xsl:param name="height" />
		<xsl:choose>
			<xsl:when test="$height = 'close'">
				<xsl:value-of select="0" />
			</xsl:when>
			<xsl:when test="$height = 'near-close'">
				<xsl:value-of select="0.5" />
			</xsl:when>
			<xsl:when test="$height = 'close-mid'">
				<xsl:value-of select="1" />
			</xsl:when>
			<xsl:when test="$height = 'mid'">
				<xsl:value-of select="1.5" />
			</xsl:when>
			<xsl:when test="$height = 'open-mid'">
				<xsl:value-of select="2" />
			</xsl:when>
			<xsl:when test="$height = 'near-open'">
				<xsl:value-of select="2.5" />
			</xsl:when>
			<xsl:when test="$height = 'open'">
				<xsl:value-of select="3" />
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="formatPropertyValue">
		<xsl:param name="property" />
		<xsl:param name="value" />
		<xsl:value-of select="$property" />
		<xsl:value-of select="':'" />
		<xsl:choose>
			<xsl:when test="$value = 0">
				<xsl:value-of select="$value" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="format-number($value, '0.000')" />
			</xsl:otherwise>
		</xsl:choose>
		<xsl:value-of select="'em;'" />
	</xsl:template>

	<xsl:template match="/xhtml:html">
    <xsl:variable name="lang" select="$langDefault" />
    <html lang="{$lang}" xmlns="http://www.w3.org/1999/xhtml">
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
				<xsl:if test="$genericStylesheetForCSS3 = 'true'">
					<!-- Internet Explorer 9 supports rounded borders. -->
					<!-- The comments hide the script from Internet Explorer 8 and earlier. -->
					<xsl:comment>
						<xsl:value-of select="'[if gte IE 9]&gt;&lt;!'" />
					</xsl:comment>
					<link rel="stylesheet" type="text/css" href="{concat($genericRelativePath, $genericStylesheetFileCSS3)}" class="CSS3" media="all" />
					<xsl:comment>
						<xsl:value-of select="'&lt;![endif]'" />
					</xsl:comment>
				</xsl:if>
				<xsl:if test="$genericStylesheetForIE8 = 'true' and $view != 'Vowel quadrilateral'">
					<!-- Here is an example of a conditional comment containing a style sheet: -->
					<!--[if lte IE 8]><link rel="stylesheet" type="text/css" href="../phonology_IE8.css" class="IE8" media="all" /><![endif]-->
					<xsl:comment>
						<xsl:value-of select="'[if lte IE 8]&gt;'" />
						<xsl:value-of select="concat('&lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;', $genericRelativePath, $genericStylesheetFileIE8, '&quot; class=&quot;IE8&quot; media=&quot;all&quot; /&gt;')" />
						<xsl:value-of select="'&lt;![endif]'" />
					</xsl:comment>
					<xsl:if test="$genericStylesheetForIE7 = 'true'">
						<!-- Here is an example of a conditional comment containing a style sheet: -->
						<!--[if lte IE 7]><link rel="stylesheet" type="text/css" href="../phonology_IE7.css" class="IE7" media="all" /><![endif]-->
						<xsl:comment>
							<xsl:value-of select="'[if lte IE 7]&gt;'" />
							<xsl:value-of select="concat('&lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;', $genericRelativePath, $genericStylesheetFileIE7, '&quot; class=&quot;IE7&quot; media=&quot;all&quot; /&gt;')" />
							<xsl:value-of select="'&lt;![endif]'" />
						</xsl:comment>
						<xsl:if test="$genericStylesheetForIE6 = 'true'">
							<!-- The IE6.css file assumes that the IE7.css file precedes it. -->
							<!-- That is, you can support IE7 but not IE6; but to support IE6, you must support IE7. -->
							<!--[if lte IE 6]><link rel="stylesheet" type="text/css" href="../phonology_IE6.css" class="IE6" media="all" /><![endif]-->
							<xsl:comment>
								<xsl:value-of select="'[if lte IE 6]&gt;'" />
								<xsl:value-of select="concat('&lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;', $genericRelativePath, $genericStylesheetFileIE6, '&quot; class=&quot;IE6&quot; media=&quot;all&quot; /&gt;')" />
								<xsl:value-of select="'&lt;![endif]'" />
							</xsl:comment>
						</xsl:if>
					</xsl:if>
				</xsl:if>
				<xsl:if test="$view = 'Data' or $view = 'Search'">
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
				</xsl:if>
			</head>
			<body>
				<xsl:apply-templates select="xhtml:body/*" />
				<!-- To reduce potential delay in loading content, script elements are at the end of the body. -->
				<!-- The comments hide the script from Internet Explorer 6 and earlier. -->
				<!-- Newline necessary to force start and end tags on separate lines. -->
				<xsl:if test="$interactiveWebPage = 'true'">
					<xsl:choose>
						<xsl:when test="$view = 'Vowel quadrilateral'">
							<script type="text/javascript" src="{concat($genericRelativePath, $jqueryScriptFile)}">
								<xsl:value-of select="'&#xA;'" />
							</script>
							<script type="text/javascript" src="{concat($genericRelativePath, $jqueryuiScriptFile)}">
								<xsl:value-of select="'&#xA;'" />
							</script>
							<script type="text/javascript" src="{concat($genericRelativePath, $phonologyScriptFileQuadrilateral)}">
								<xsl:value-of select="'&#xA;'" />
							</script>
							<xsl:if test="$numberOfMonophthongs != 0 and $numberOfDiphthongs != 0">
								<script type="text/javascript" src="{concat($genericRelativePath, $phonologyScriptFile)}">
									<xsl:value-of select="'&#xA;'" />
								</script>
								<xsl:comment>
									<xsl:value-of select="'[if IE 6]&gt;&#xA;'" />
									<xsl:value-of select="concat('&lt;script type=&quot;text/javascript&quot; src=&quot;', $genericRelativePath, $phonologyScriptFileQuadrilateralIE6, '&quot;&gt;&#xA;')" />
									<xsl:value-of select="'&lt;/script&gt;&#xA;'" />
									<xsl:value-of select="'&lt;![endif]'" />
								</xsl:comment>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<xsl:comment>
								<xsl:value-of select="'[if gte IE 7]&gt;&lt;!'" />
							</xsl:comment>
							<script type="text/javascript" src="{concat($genericRelativePath, $jqueryScriptFile)}">
								<xsl:value-of select="'&#xA;'" />
							</script>
							<script type="text/javascript" src="{concat($genericRelativePath, $jqueryuiScriptFile)}">
								<xsl:value-of select="'&#xA;'" />
							</script>
							<script type="text/javascript" src="{concat($genericRelativePath, $phonologyScriptFile)}">
								<xsl:value-of select="'&#xA;'" />
							</script>
							<xsl:comment>
								<xsl:value-of select="'&lt;![endif]'" />
							</xsl:comment>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</body>
    </html>
  </xsl:template>

	<xsl:template match="xhtml:div[@id = 'metadata']" />

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
				<xsl:value-of select="'sorting_field'" />
			</xsl:if>
			<xsl:if test="$interactiveWebPage = 'true'">
				<!-- Phonetic sort options for Phonetic, except when there is one minimal pair per group. -->
				<xsl:if test="$fieldName = 'Phonetic' and not($pairs and $oneMinimalPairPerGroup = 'true')">
					<xsl:if test="$fieldName = $primarySortFieldName">
						<xsl:value-of select="' '" />
					</xsl:if>
					<xsl:value-of select="'sorting_options'" />
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
									<xsl:when test="$sorting/xhtml:li[@class = 'phoneticSortClass'] = 'manner_or_height'">
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
										<!-- place_or_backness -->
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
  <xsl:template match="xhtml:table[starts-with(@class, 'CV chart')]//xhtml:td[not(node())][@class = 'Phonetic']/@class" />

	<!-- Temporarily provide class="zero" until the Phonology Assistant program can. -->
	<xsl:template match="xhtml:table[@class = 'distribution chart']//xhtml:td[. = '0'][not(@class)]">
		<xsl:copy>
			<xsl:attribute name="class">
				<xsl:value-of select="'zero'" />
			</xsl:attribute>
			<xsl:apply-templates />
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

</xsl:stylesheet>