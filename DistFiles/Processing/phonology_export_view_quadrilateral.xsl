<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
xmlns:svg="http://www.w3.org/2000/svg"
exclude-result-prefixes="xhtml svg"
>

  <!-- phonology_export_view_quadrilateral.xsl 2011-05-10 -->
	<!-- Export to SVG or XHTML -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<!-- Insert CDATA sections within style elements. -->
	<xsl:output cdata-section-elements="svg:style xhtml:style" />

	<xsl:param name="langDefault" select="'en'" />
	<xsl:param name="viewBox" select="'0 0 6000 4000'" />
	<xsl:param name="preserveAspectRatio" select="'xMinYMin meet'" />
	<xsl:param name="paddingLeft" select="'1000'" />
	<xsl:param name="paddingTop" select="'500'" />
	<xsl:param name="width" select="'4000'" />
	<xsl:param name="height" select="'3000'" />
	<xsl:param name="dx" select="'100'" />
	<xsl:param name="dy" select="'100'" />
	<xsl:param name="r" select="'30'" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="languageName" select="$details/xhtml:li[@class = 'languageName']" />
	<xsl:variable name="languageCode3" select="$details/xhtml:li[@class = 'languageCode']" />
	<xsl:variable name="languageCode1">
		<xsl:if test="string-length($languageCode3) != 0">
			<xsl:value-of select="document('ISO_639.html')//xhtml:tr[xhtml:td[@class = 'ISO_639-3'] = $languageCode3]/xhtml:td[@class = 'ISO_639-1']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="languageCode">
		<xsl:choose>
			<xsl:when test="string-length($languageCode1) = 2">
				<xsl:value-of select="$languageCode1" />
			</xsl:when>
			<xsl:when test="string-length($languageCode3) != 0">
				<xsl:value-of select="$languageCode3" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'und'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="langPhonetic">
		<xsl:value-of select="$languageCode" />
		<xsl:value-of select="'-fonipa'" />
	</xsl:variable>

	<!-- A project phonetic inventory file contains features of phonetic or phonological units, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="units" select="document($projectPhoneticInventoryXML)/inventory/units" />

	<xsl:variable name="upperAlpha" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />
	<xsl:variable name="lowerAlpha" select="'abcdefghijklmnopqrstuvwxyz'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="/xhtml:html/xhtml:head/xhtml:title">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="'Vowel quadrilateral'" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="//xhtml:div[@id = 'metadata']/xhtml:ul[@class = 'details']/xhtml:li[@class = 'view']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="'Vowel quadrilateral'" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']">
		<xsl:if test="xhtml:tbody/xhtml:tr/xhtml:td[@class = 'Phonetic'][node()]">
			<div class="quadrilaterals" xmlns="http://www.w3.org/1999/xhtml">
				<div class="quadrilateral left">
					<xsl:call-template name="quadrilateral">
						<xsl:with-param name="table" select="." />
						<xsl:with-param name="subtype" select="'monophthong'" />
					</xsl:call-template>
				</div>
				<div class="quadrilateral right">
					<xsl:call-template name="quadrilateral">
						<xsl:with-param name="table" select="." />
						<xsl:with-param name="subtype" select="'diphthong'" />
					</xsl:call-template>
				</div>
			</div>
		</xsl:if>
	</xsl:template>

	<xsl:template name="quadrilateral">
		<xsl:param name="table" />
		<xsl:param name="subtype" />
		<svg version="1.1" viewBox="{$viewBox}" preserveAspectRatio="{$preserveAspectRatio}" xml:lang="{$langDefault}" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">
			<title>
				<xsl:value-of select="'Vowel quadrilateral for '" />
				<xsl:choose>
					<xsl:when test="string-length($languageName) != 0">
						<xsl:value-of select="$languageName" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Languaage'" />
					</xsl:otherwise>
				</xsl:choose>
			</title>
			<defs>
				<marker id="quadrilateralLineArrowheadOpen" markerUnits="strokeWidth" viewBox="-8 -3 8 6" markerWidth="8" markerHeight="6" orient="auto">
					<desc>end marker for diphthong lines</desc>
					<path d="M 0 0 l -8 3 M 0 0 l -8 -3" />
				</marker>
				<marker id="quadrilateralLineArrowheadFilled" markerUnits="strokeWidth" viewBox="-8 -3 8 6" markerWidth="8" markerHeight="6" orient="auto">
					<desc>end marker for diphthong lines</desc>
					<path d="M 0 0 l -8 3 v -6 z" />
				</marker>
				<clipPath id="quadrilateralGridBoundary">
					<desc>boundary for clipped shapes (for example, filled ellipses)</desc>
					<path d="M 0 0 h 4000 v 3000 h -2000 z" />
				</clipPath>
			</defs>
			<style type="text/css"><![CDATA[
svg { fill-opacity: 0; } /* equivalent to background-color: transparent */
svg.container { overflow: visible; }
/* text font-size means 400 milliunits, which allows two rows per vertical unit (for example, near-close between close and close-mid) */
text { fill-opacity: 1; fill: currentColor; stroke: none; font-size: 400px; font-family: "Charis SIL", "Doulos SIL", "IPAKielSeven", "IPAKiel", serif; }
text.left { text-anchor: end; } /* the text is to the left of (x,y)  */
text.center { text-anchor: middle; } /* the text is centered horizontally at (x,y)  */
text.right { text-anchor: start; } /* the text is to the right of (x,y) */
circle, ellipse, line, path { fill: none; stroke: currentColor; stroke-width: 10; } /* for example, stroke-width is 1px if 1000 milliunits = 96 pixels */
marker path { fill-opacity: 1; fill: currentColor; stroke: currentColor; stroke-width: 1; }
.filled { fill: currentColor; fill-opacity: 1; }
.clipped { clip-path: url(#quadrilateralGridBoundary); }
g.grid line { stroke: #999999; } /* border */
g.data line { stroke-width: 20; } /* for example, stroke-width is 2px if 1000 milliunits = 96 pixels */
g.data line.dashed { stroke-dasharray: 50, 50; }
g.data line.arrowhead { marker-end: url(#quadrilateralLineArrowheadOpen); }
g.data line.arrowhead.filled { marker-end: url(#quadrilateralLineArrowheadFilled); }
g.data g.crosshairs line { stroke-width: 10; } /* for example, stroke-width is 1px if 1000 milliunits = 96 pixels */
g.data g.handles circle { stroke-opacity: 0.6; fill-opacity: 0.6; fill: currentColor; }
g.data g.handles ellipse { stroke-opacity: 0.6; stroke-width: 30; } /* for example, stroke-width is 3px if 1000 milliunits = 96 pixels */
g.data g.handles line { stroke-opacity: 0.6; stroke: #dddddd; stroke-width: 10; }
]]></style>
			<xsl:comment> x=padding-left and y=padding-top so that close front is at (0,0) and open back is at (4000,3000) </xsl:comment>
			<svg class="container" x="{$paddingLeft}" y="{$paddingTop}" width="{$width}" height="{$height}">
				<g class="grid">
					<g class="lines">
						<desc>dimensions of a vowel quadrilateral are in milliunits: 4000 at the top, 3000 at the right, 2000 at the bottom</desc>
						<line x1="0" y1="0" x2="4000" y2="0">
							<title>close</title>
						</line>
						<line x1="667" y1="1000" x2="4000" y2="1000">
							<title>close-mid</title>
						</line>
						<line x1="1333" y1="2000" x2="4000" y2="2000">
							<title>open-mid</title>
						</line>
						<line x1="2000" y1="3000" x2="4000" y2="3000">
							<title>open</title>
						</line>
						<line x1="0" y1="0" x2="2000" y2="3000">
							<title>front</title>
							<desc>equation for front line: x = y * 2 / 3</desc>
						</line>
						<line x1="2000" y1="0" x2="3000" y2="3000">
							<title>central</title>
							<desc>equation for central line: x = 2000 + y / 3</desc>
						</line>
						<line x1="4000" y1="0" x2="4000" y2="3000">
							<title>back</title>
						</line>
					</g>
				</g>
				<g class="data">
					<xsl:apply-templates select="$table/xhtml:tbody/xhtml:tr/xhtml:td[@class = 'Phonetic'][node()]">
						<xsl:with-param name="subtype" select="$subtype" />
					</xsl:apply-templates>
				</g>
			</svg>
		</svg>
	</xsl:template>

	<!-- Export to XHTML: title attribute of phonetic cell contains description of unit. -->
	<xsl:template match="xhtml:table[@class = 'CV chart']//xhtml:td[@class = 'Phonetic'][node()]">
		<xsl:param name="subtype" />
		<xsl:variable name="literal">
			<xsl:choose>
				<xsl:when test="xhtml:span">
					<xsl:value-of select="xhtml:span" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="." />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="unit" select="$units/unit[@literal = $literal]" />
		<xsl:variable name="description" select="$unit/description" />
		<xsl:variable name="height" select="translate($unit/keys/chartKey[@class = 'rowgroup'], $upperAlpha, $lowerAlpha)" />
		<xsl:variable name="backness">
			<xsl:choose>
				<xsl:when test="$unit/features[@class = 'descriptive']/feature[@category = 'backness']">
					<xsl:value-of select="$unit/features[@class = 'descriptive']/feature[@category = 'backness']" />
				</xsl:when>
				<xsl:when test="$unit/articulatoryFeatures/feature[@subclass= 'backness']">
					<xsl:value-of select="translate($unit/articulatoryFeatures/feature[@subclass= 'backness'], $upperAlpha, $lowerAlpha)" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="rounding" select="translate($unit/keys/chartKey[@class = 'col'], $upperAlpha, $lowerAlpha)" />
		<xsl:variable name="textClass">
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
		<xsl:variable name="xFormatted" select="round($x)" />
		<xsl:variable name="transform">
			<xsl:value-of select="'translate('" />
			<xsl:if test="$rounding = 'unrounded'">
				<xsl:value-of select="'-'" />
			</xsl:if>
			<xsl:value-of select="$dx" />
			<xsl:value-of select="','" />
			<xsl:value-of select="$dy" />
			<xsl:value-of select="')'" />
		</xsl:variable>
		<!-- TO DO for diphthongs: height, backness, and rounding features of second base symbol. -->
		<xsl:variable name="height2">
			<xsl:choose>
				<xsl:when test="$unit/keys/chartKeySecondary/chartKey[@class = 'rowgroup']">
					<xsl:value-of select="translate($unit/keys/chartKeySecondary/chartKey[@class = 'rowgroup'], $upperAlpha, $lowerAlpha)" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="backness2">
			<xsl:choose>
				<xsl:when test="$unit/keys/chartKeySecondary/chartKey[@class = 'colgroup']">
					<xsl:value-of select="translate($unit/keys/chartKeySecondary/chartKey[@class = 'colgroup'], $upperAlpha, $lowerAlpha)" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="rounding2">
			<xsl:choose>
				<xsl:when test="$unit/keys/chartKeySecondary/chartKey[@class = 'col']">
					<xsl:value-of select="translate($unit/keys/chartKeySecondary/chartKey[@class = 'col'], $upperAlpha, $lowerAlpha)" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="$unit/features[@class = 'descriptive']/feature[. = $subtype]">
			<g class="{$subtype}" xmlns="http://www.w3.org/2000/svg">
				<title>
					<xsl:value-of select="$description" />
				</title>
				<xsl:if test="$subtype = 'diphthong'">
					<xsl:variable name="y2">
						<xsl:call-template name="y">
							<xsl:with-param name="height" select="$height2" />
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="x2">
						<xsl:call-template name="x">
							<xsl:with-param name="backness" select="$backness2" />
							<xsl:with-param name="y" select="$y2" />
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="x2Formatted" select="round($x2)" />
					<line class="arrowhead filled" x1="{$xFormatted}" y1="{$y}" x2="{$x2Formatted}" y2="{$y2}" />
				</xsl:if>
				<circle class="filled" cx="{$xFormatted}" cy="{$y}" r="{$r}" xmlns:xlink="http://www.w3.org/1999/xlink" />
				<text class="{$textClass}" x="{$xFormatted}" y="{$y}" transform="{$transform}" xml:lang="{$langPhonetic}">
					<xsl:value-of select="$literal" />
				</text>
			</g>
		</xsl:if>
	</xsl:template>

	<xsl:template name="x">
		<xsl:param name="backness" />
		<xsl:param name="y" />
		<xsl:choose>
			<xsl:when test="$backness = 'front'">
				<xsl:value-of select="2 * $y div 3" />
			</xsl:when>
			<xsl:when test="$backness = 'near-front'">
				<xsl:value-of select="5000 div 6" />
			</xsl:when>
			<xsl:when test="$backness = 'central'">
				<xsl:value-of select="2000 + $y div 3" />
			</xsl:when>
			<xsl:when test="$backness = 'near-back'">
				<xsl:value-of select="4000 - 5000 div 6" />
			</xsl:when>
			<xsl:when test="$backness = 'back'">
				<xsl:value-of select="4000" />
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
				<xsl:value-of select="500" />
			</xsl:when>
			<xsl:when test="$height = 'close-mid'">
				<xsl:value-of select="1000" />
			</xsl:when>
			<xsl:when test="$height = 'mid'">
				<xsl:value-of select="1500" />
			</xsl:when>
			<xsl:when test="$height = 'open-mid'">
				<xsl:value-of select="2000" />
			</xsl:when>
			<xsl:when test="$height = 'near-open'">
				<xsl:value-of select="2500" />
			</xsl:when>
			<xsl:when test="$height = 'open'">
				<xsl:value-of select="3000" />
			</xsl:when>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>