<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:w="http://schemas.microsoft.com/office/word/2003/wordml"
xmlns:v="urn:schemas-microsoft-com:vml"
xmlns:w10="urn:schemas-microsoft-com:office:word"
xmlns:sl="http://schemas.microsoft.com/schemaLibrary/2003/core"
xmlns:aml="http://schemas.microsoft.com/aml/2001/core"
xmlns:wx="http://schemas.microsoft.com/office/word/2003/auxHint"
xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:dt="uuid:C2F41010-65B3-11d1-A29F-00AA00C14882"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>
  <!-- PA_Export_View_to_Word_2003_XML.xsl 2010-05-24 -->
	
  <!-- TO DO: No w:r and w:t in empty paragraphs? -->
  <!-- TO DO: Convert spaces and hyphens to non-breaking? -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />

  <!-- Parameters -->

  <xsl:param name="heading-rows-repeat" select="'true'" />
  <xsl:param name="cantSplit" select="$heading-rows-repeat = 'true'" />
  <xsl:param name="allow-rows-to-break-across-pages" select="'true'" />
  <xsl:param name="tblHeader" select="$allow-rows-to-break-across-pages = 'true'" />
  <xsl:param name="tc-tcPr-vAlign" select="'top'" />
  <xsl:param name="tc-tcPr-tcBorders-sz-thin" select="4" />
  <xsl:param name="tc-tcPr-tcBorders-sz-thick" select="12" />
  <xsl:param name="tc-tcPr-tcBorders-color" select="999999" />
	<xsl:param name="tc-tcPr-tcBorders-color-lighter" select="DDDDDD" />

	<!-- desaturated yellow rgb(255,255,178) hsv(60,30%,100%) -->
  <xsl:param name="Search-item-background-color" select="'ffffb2'" />
  <xsl:param name="distribution-chart-zero-background-color" select="'ffffb2'" />
	<!-- desaturated orange rgb(255,230,178) hsv(40,30%,100%) -->
	<xsl:param name="distribution-chart-caution-background-color" select="'ffe6b2'" />
	<!-- desaturated red rgb(255,178,178) hsv(0,30%,100%) -->
  <xsl:param name="distribution-chart-error-background-color" select="'ffb2b2'" />

  <!-- <xsl:param name="docPr_view" select="'normal'" /> -->
  <!-- Normal -->
  <!-- <xsl:param name="docPr_view" select="'web'" /> -->
  <!-- Web Layout -->
  <xsl:param name="docPr_view" select="'print'" />
  <!-- Print Layout -->
  <!-- <xsl:param name="docPr_view" select="'master-pages'" /> -->
  <!-- Outline -->
  <xsl:param name="docPr_zoom_percent" select="100" />
  <xsl:param name="docPr_attachedTemplate" select="''" />

  <xsl:param name="highlight_val" select="'yellow'" />
  <xsl:param name="non-breaking" select="true()" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="dateAndTime" select="$options/xhtml:li[@class = 'dateAndTime']" />
	<xsl:variable name="textFlowOfColumnHeadings" select="$options/xhtml:li[@class = 'textFlowOfColumnHeadings']" />
	<xsl:variable name="tblHeader-textFlow">
		<xsl:choose>
			<xsl:when test="$textFlowOfColumnHeadings = 'verticalCounterClockwise'">
				<xsl:value-of select="'bt-lr'" />
			</xsl:when>
			<xsl:when test="$textFlowOfColumnHeadings = 'verticalClockwise'">
				<xsl:value-of select="'tb-rl'" />
			</xsl:when>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="orientation">
		<xsl:choose>
			<xsl:when test="$options/xhtml:li[@class = 'orientation']">
				<xsl:value-of select="$options/xhtml:li[@class = 'orientation']" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'Portrait'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="paperSize">
		<xsl:choose>
			<xsl:when test="$options/xhtml:li[@class = 'paperSize']">
				<xsl:value-of select="$options/xhtml:li[@class = 'paperSize']" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'Letter'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<!-- Help Converter provides the file name for the footer. -->
	<!-- But get it from the metadata format options instead. -->
	<!--
  <xsl:param name="file-name-without-extension" select="''" />
	-->
	<xsl:variable name="fileName" select="$options/xhtml:li[@class = 'fileName']" />

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="date" select="$details/xhtml:li[@class = 'date']" />
	<xsl:variable name="time" select="$details/xhtml:li[@class = 'time']" />
	<xsl:variable name="researcher" select="$details/xhtml:li[@class = 'researcher']" />

	<xsl:template match="/xhtml:html">
    <xsl:processing-instruction name="mso-application">progid="Word.Document"</xsl:processing-instruction>
    <!-- Use the xsl:attribute element instead of a literal xml:space attribute in the w:wordDocument element -->
    <!-- so that white space is preserved from the input to the output, but not from the stylesheet itself. -->
    <!-- See Office 2003 XML, page 104 -->
    <w:wordDocument>
      <xsl:attribute name="xml:space">preserve</xsl:attribute>
			<xsl:variable name="formatting" select="xhtml:body/xhtml:div[@id = 'metadata']/xhtml:table[@class = 'formatting']" />
			<o:DocumentProperties>
        <o:Title>
          <xsl:value-of select="xhtml:head/xhtml:title" />
        </o:Title>
				<xsl:if test="string-length($researcher) != 0">
					<o:Author>
						<xsl:value-of select="$researcher" />
					</o:Author>
				</xsl:if>
			</o:DocumentProperties>
      <xsl:call-template name="fonts">
        <xsl:with-param name="formatting" select="$formatting" />
      </xsl:call-template>
      <w:styles>
        <xsl:call-template name="basicStyles" />
        <xsl:call-template name="projectCharacterStyles">
          <xsl:with-param name="formatting" select="$formatting" />
        </xsl:call-template>
      </w:styles>
      <w:docPr>
        <w:view w:val="{$docPr_view}" />
        <xsl:if test="$docPr_zoom_percent != 0">
          <w:zoom w:percent="{$docPr_zoom_percent}" />
        </xsl:if>
        <xsl:if test="string-length($docPr_attachedTemplate) != 0">
          <w:attachedTemplate w:val="{$docPr_attachedTemplate}" />
        </xsl:if>
        <!-- If a document contains tables, OpenOffice.org 3.1 requires the default tab stop. -->
        <w:defaultTabStop w:val="720" />
      </w:docPr>
      <w:body>
				<xsl:if test="$details">
					<xsl:apply-templates select="xhtml:body/xhtml:div[@id = 'metadata']" />
				</xsl:if>
				<xsl:apply-templates select="xhtml:body/xhtml:table" />
        <xsl:call-template name="pStyleText">
          <xsl:with-param name="pStyle" select="'SpaceSingle'" />
          <xsl:with-param name="text" select="''" />
        </xsl:call-template>
        <xsl:call-template name="sectPr" />
      </w:body>
    </w:wordDocument>
  </xsl:template>

  <xsl:template name="text">
    <xsl:variable name="text" select="." />
    <xsl:choose>

      <xsl:when test="$non-breaking = 'true()' and contains(., ' ') and ancestor::xhtml:th and ancestor::xhtml:thead">
        <!-- Convert spaces to non-breaking spaces in heading cells of table heads. -->
        <w:t>
          <xsl:value-of select="translate($text, ' ', '&#xA0;')" />
        </w:t>
      </xsl:when>
      <!--Strips out &nbsp for empty paragraphs -->
      <!--
      <xsl:when test="$text = '&#160;' and not(preceding-sibling::*) and not(following-sibling::*)">
        <w:t />
      </xsl:when>
      -->

      <!-- Remove the line break and indent that follows a br element. -->
      <!--
      <xsl:when test="starts-with($text,'&#10;')">
        <w:t>
          <xsl:call-template name="omit_leading_spaces">
            <xsl:with-param name="str" select="substring($text,2)" />
          </xsl:call-template>
        </w:t>
      </xsl:when>
      -->
      <!-- Remove the line break and indent that follows a br element. -->
      <!--
      <xsl:when test="starts-with($text,' ') and preceding-sibling::*[1][self::xhtml:br]">
        <w:t>
          <xsl:call-template name="omit_leading_spaces">
            <xsl:with-param name="str" select="$text" />
          </xsl:call-template>
        </w:t>
      </xsl:when>
      -->
      <xsl:otherwise>
        <w:t>
          <xsl:value-of select="$text" />
        </w:t>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="omit_leading_spaces">
    <xsl:param name="str" />
    <xsl:choose>
      <xsl:when test="starts-with($str, ' ')">
        <xsl:call-template name="omit_leading_spaces">
          <xsl:with-param name="str" select="substring($str, 2)" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$str" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="text()">
    <xsl:call-template name="text" />
  </xsl:template>

  <xsl:template match="xhtml:span/text()">
    <xsl:call-template name="text" />
  </xsl:template>

  <xsl:template match="xhtml:br">
    <w:br />
  </xsl:template>


  <xsl:template name="pStyleText">
    <xsl:param name="pStyle" select="'BodyText'" />
    <xsl:param name="rStyle" select="''" />
    <xsl:param name="text" select="''" />
    <w:p>
      <w:pPr>
        <w:pStyle w:val="{$pStyle}" />
      </w:pPr>
      <w:r>
        <xsl:if test="$rStyle != ''">
          <w:rPr>
            <w:rStyle w:val="{$rStyle}" />
          </w:rPr>
        </xsl:if>
        <w:t>
          <xsl:value-of select="$text" />
        </w:t>
      </w:r>
    </w:p>
  </xsl:template>

  <xsl:template name="p">
    <xsl:param name="pStyle_val" select="'BodyText'" />
    <w:p>
      <w:pPr>
        <w:pStyle w:val="{$pStyle_val}" />
      </w:pPr>
			<xsl:apply-templates />
		</w:p>
  </xsl:template>

  <!-- Block elements to paragraph styles -->

  <xsl:template match="xhtml:h1">
    <xsl:call-template name="p">
      <xsl:with-param name="pStyle_val" select="'Heading1'" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="xhtml:h2">
    <xsl:call-template name="p">
      <xsl:with-param name="pStyle_val" select="'Heading2'" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="xhtml:h3">
    <xsl:call-template name="p">
      <xsl:with-param name="pStyle_val" select="'Heading3'" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="xhtml:h4">
    <xsl:call-template name="p">
      <xsl:with-param name="pStyle_val" select="'Heading4'" />
    </xsl:call-template>
  </xsl:template>


  <xsl:template match="xhtml:p">
    <xsl:call-template name="p">
      <xsl:with-param name="pStyle_val" select="'BodyText'" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="xhtml:p[@class = 'SpaceSingle' or @class = 'SpaceHalf']">
    <xsl:call-template name="p">
      <xsl:with-param name="pStyle_val" select="@class" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="xhtml:th/xhtml:p">
    <xsl:call-template name="p">
      <xsl:with-param name="pStyle_val" select="'TableHeading'" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="xhtml:td/xhtml:p">
    <xsl:call-template name="p">
      <xsl:with-param name="pStyle_val" select="'TableData'" />
    </xsl:call-template>
  </xsl:template>

  <!-- Inline elements to character styles -->

  <xsl:template name="span-class-to-rStyle-val">
    <xsl:param name="class" select="''" />
    <xsl:value-of select="$class" />
  </xsl:template>
  
  <xsl:template name="span">
    <xsl:param name="class" select="''" />
    <xsl:choose>
      <xsl:when test="$class='highlighted'">
        <w:r>
          <w:rPr>
            <w:highlight w:val="{$highlight_val}" />
          </w:rPr>
          <xsl:apply-templates />
        </w:r>
      </xsl:when>
      <!-- If class ends with ' highlighted'. -->
      <xsl:when test="substring($class, (string-length($class) - string-length(' highlighted')) + 1) = ' highlighted'">
        <xsl:variable name="rStyle-val">
          <xsl:call-template name="span-class-to-rStyle-val">
            <xsl:with-param name="class" select="substring-before($class, ' highlighted')" />
          </xsl:call-template>
        </xsl:variable>
        <w:r>
          <w:rPr>
            <xsl:if test="$rStyle-val != ''">
              <w:rStyle w:val="{$rStyle-val}" />
            </xsl:if>
            <w:highlight w:val="{$highlight_val}" />
          </w:rPr>
          <xsl:apply-templates />
        </w:r>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="rStyle-val">
          <xsl:call-template name="span-class-to-rStyle-val">
            <xsl:with-param name="class" select="$class" />
          </xsl:call-template>
        </xsl:variable>
        <w:r>
          <w:rPr>
            <xsl:if test="$rStyle-val != ''">
              <w:rStyle w:val="{$rStyle-val}" />
            </xsl:if>
          </w:rPr>
          <xsl:apply-templates />
        </w:r>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:span">
    <xsl:call-template name="span">
      <xsl:with-param name="class" select="@class" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="xhtml:span/xhtml:span">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="xhtml:strong">
    <w:r>
      <w:rPr>
        <w:rStyle w:val="Strong" />
      </w:rPr>
      <xsl:call-template name="text" />
    </w:r>
  </xsl:template>

  <xsl:template match="xhtml:em">
    <w:r>
      <w:rPr>
        <w:rStyle w:val="Emphasis" />
      </w:rPr>
      <xsl:call-template name="text" />
    </w:r>
  </xsl:template>

  <!-- Tables -->

  <xsl:template match="xhtml:thead">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="xhtml:tbody">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="xhtml:table">
    <w:tbl>
      <w:tblPr>
        <xsl:choose>
          <xsl:when test="@class = 'list'">
            <w:tblStyle w:val="TableList" />
            <w:tblLook w:val="01E0" />
          </xsl:when>
          <xsl:when test="@class = 'CV chart' or @class = 'distribution chart'">
            <w:tblStyle w:val="TableChart" />
            <w:tblLook w:val="01E0" />
          </xsl:when>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="@width = '100%'">
            <w:tblW w:w="5000" w:type="pct" />
          </xsl:when>
          <xsl:otherwise>
            <w:tblW w:w="0" w:type="auto" />
          </xsl:otherwise>
        </xsl:choose>
      </w:tblPr>
      <xsl:if test=".//xhtml:col">
        <w:tblGrid>
          <!-- Some col elements might be contained in colgroup. -->
          <!-- TO DO: Make sure this visits them in the correct order! -->
          <xsl:for-each select=".//xhtml:col">
            <xsl:choose>
              <xsl:when test="@width and contains(@width, '%')">
                <!-- TO DO: Width percent? -->
                <xsl:variable name="columnPercentage" select="format-number(substring(@width, 1, string-length(@width)-1), '#') div 100" />
                <!-- xhtml:table may or may not have a width specified. -->
                <xsl:choose>
                  <xsl:when test="../@width">
                    <xsl:variable name="tableWidth" select="11220" />
                    <xsl:variable name="tcWidth" select="round($tableWidth * $columnPercentage)" />
                    <xsl:element name="w:gridCol">
                      <xsl:attribute name="w:w">
                        <xsl:value-of select="$tcWidth" />
                      </xsl:attribute>
                    </xsl:element>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:variable name="tableWidth" select="9576" />
                    <xsl:variable name="tcWidth" select="round($tableWidth * $columnPercentage)" />
                    <xsl:element name="w:gridCol">
                      <xsl:attribute name="w:w">
                        <xsl:value-of select="$tcWidth" />
                      </xsl:attribute>
                    </xsl:element>
                  </xsl:otherwise>
                </xsl:choose>
                <!-- debugging info
@width is <xsl:value-of select="@width" />
$tableWidth is <xsl:value-of select="$tableWidth" />
$columnPercentage is xsl:value-of select="$columnPercentage" />
-->
              </xsl:when>
              <xsl:when test="@width">
                <xsl:variable name="width" select="@width * 15" />
                <!-- 15 twips per pixel -->
                <w:gridCol w:w="{$width}" />
              </xsl:when>
              <xsl:when test="@style and starts-with(@style, 'width: ') and contains(@style, '%;')">
                <w:gridCol />
              </xsl:when>
              <xsl:otherwise>
                <w:gridCol />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
        </w:tblGrid>
      </xsl:if>
      <xsl:apply-templates />
    </w:tbl>
  </xsl:template>

  <xsl:template match="xhtml:tr">
    <w:tr>
      <xsl:if test="$cantSplit or $tblHeader or ($tblHeader-textFlow != '' and ancestor::xhtml:thead and ancestor::xhtml:table[contains(@class, 'chart')])">
        <w:trPr>
          <xsl:if test="$cantSplit">
            <w:cantSplit />
          </xsl:if>
          <xsl:if test="$tblHeader and $tblHeader-textFlow != '' and ancestor::xhtml:thead and ancestor::xhtml:table[contains(@class, 'chart')]">
            <xsl:variable name="max-length">
              <xsl:for-each select="xhtml:th/text()">
                <xsl:sort select="string-length(.)" data-type="number" order="descending" />
                <xsl:if test="position() = 1">
                  <xsl:value-of select="string-length(.)" />
                </xsl:if>
              </xsl:for-each>
            </xsl:variable>
            <!-- Estimate 0.833 inch per character. -->
            <xsl:variable name="max-length-in-twips" select="$max-length * 120" />
            <w:trHeight w:val="{$max-length-in-twips}" />
          </xsl:if>
          <xsl:if test="$tblHeader and ancestor::xhtml:thead">
            <w:tblHeader />
          </xsl:if>
        </w:trPr>
      </xsl:if>
      <!-- Insert empty cell corresponding to rowspan. -->
      <xsl:if test="not(xhtml:th) and ancestor::xhtml:tbody and ancestor::xhtml:table[@class = 'CV chart']">
        <w:tc>
          <w:tcPr>
            <w:tcW w:w="0" w:type="auto" />
            <w:vmerge />
            <w:tcBorders>
              <w:top w:val="nil" />
              <w:left w:val="nil" />
              <w:bottom w:val="nil" />
              <w:right w:val="single" w:sz="{$tc-tcPr-tcBorders-sz-thin}" w:space="0" w:color="auto" />
            </w:tcBorders>
          </w:tcPr>
          <w:p>
            <w:pPr>
              <w:pStyle w:val="TableHeading" />
            </w:pPr>
          </w:p>
        </w:tc>
      </xsl:if>
      <xsl:apply-templates />
    </w:tr>
  </xsl:template>
  
  <xsl:template match="xhtml:th | xhtml:td">
    <w:tc>
      <w:tcPr>
        <w:tcW w:w="0" w:type="auto" />
        <xsl:if test="@colspan">
          <w:gridSpan w:val="{@colspan}" />
        </xsl:if>
        <xsl:if test="@rowspan">
          <w:vmerge w:val="restart" />
        </xsl:if>
        <xsl:choose>
					<xsl:when test="@class = 'caution'">
						<w:shd w:val="clear" w:color="auto" w:fill="{$distribution-chart-caution-background-color}" />
					</xsl:when>
					<xsl:when test="@class = 'error'">
            <w:shd w:val="clear" w:color="auto" w:fill="{$distribution-chart-error-background-color}" />
          </xsl:when>
					<xsl:when test="@class = 'Phonetic item'">
            <w:shd w:val="clear" w:color="auto" w:fill="{$Search-item-background-color}" />
          </xsl:when>
          <xsl:when test="self::xhtml:td and text() = '0' and ancestor::xhtml:table[@class = 'distribution chart']">
            <w:shd w:val="clear" w:color="auto" w:fill="{$distribution-chart-zero-background-color}" />
          </xsl:when>
        </xsl:choose>
        <xsl:if test="starts-with(@class, 'Phonetic ')">
          <w:tcMar>
            <xsl:choose>
              <!-- In case the search environment begins or ends with narrow base and wide diacritic, -->
              <!-- provide one-fourth the default cell padding/margin at the edge by the item. -->
              <xsl:when test="@class = 'Phonetic preceding'">
                <w:right w:w="29" w:type="dxa" />
              </xsl:when>
              <xsl:when test="@class = 'Phonetic following'">
                <w:left w:w="29" w:type="dxa" />
              </xsl:when>
              <xsl:otherwise>
                <!-- Because the Phonetic item is centered, provide no cell padding/margin. -->
                <w:left w:w="0" w:type="dxa" />
                <w:right w:w="0" w:type="dxa" />
              </xsl:otherwise>
            </xsl:choose>
          </w:tcMar>
        </xsl:if>
        <xsl:if test="$tblHeader-textFlow != '' and ancestor::xhtml:thead and ancestor::xhtml:table[contains(@class, 'chart')]">
          <w:tcMar>
            <w:left w:w="0" w:type="dxa" />
            <w:bottom w:w="108" w:type="dxa" />
            <w:right w:w="0" w:type="dxa" />
          </w:tcMar>
          <w:textFlow w:val="{$tblHeader-textFlow}" />
        </xsl:if>
        <!-- Table style determines vertical alignment for table cells. -->
        <!-- Table style determines borders, except for groups. -->
        <xsl:if test="../@class = 'heading' or (../@class = 'subheading' and self::xhtml:th)">
          <w:tcBorders>
						<w:top w:val="single" w:sz="{$tc-tcPr-tcBorders-sz-thin}" w:space="0" w:color="auto" />
						<w:left w:val="nil" />
            <w:bottom w:val="nil" />
            <w:right w:val="nil" />
          </w:tcBorders>
        </xsl:if>
      </w:tcPr>
      <w:p>
        <w:pPr>
          <xsl:variable name="pStyle-val">
            <xsl:choose>
              <xsl:when test="@class = 'Phonetic preceding'">
                <xsl:value-of select="'TableDataRight'" />
              </xsl:when>
              <xsl:when test="@class = 'Phonetic item'">
                <xsl:value-of select="'TablePhoneticItem'" />
              </xsl:when>
              <xsl:when test="@class = 'count'">
                <xsl:value-of select="'TableGroupExpanded'" />
              </xsl:when>
              <!-- Heading cells in distribution charts are aligned at the left for counterclockwise text direction. -->
              <xsl:when test="../../../@class = 'distribution chart' and $tblHeader-textFlow = 'bt-lr' and ancestor::xhtml:thead">
                <xsl:value-of select="'TableData'" />
              </xsl:when>
              <!-- Otherwise cells in distribution charts are aligned at the right. -->
              <xsl:when test="../../../@class = 'distribution chart'">
                <xsl:value-of select="'TableDataRight'" />
              </xsl:when>
              <!-- To align the first column of a CV chart at the left, remove the following rule. -->
              <xsl:when test="self::xhtml:th and ancestor::xhtml:tbody and ../../../@class = 'CV chart'">
                <xsl:value-of select="'TableDataRight'" />
              </xsl:when>
              <!-- Heading cells in CV charts are aligned at the right for clockwise text direction. -->
              <xsl:when test="../../../@class = 'CV chart' and $tblHeader-textFlow = 'tb-rl' and ancestor::xhtml:thead">
                <xsl:value-of select="'TableDataRight'" />
              </xsl:when>
              <xsl:when test="self::xhtml:th">
                <xsl:value-of select="'TableHeading'" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'TableData'" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <w:pStyle w:val="{$pStyle-val}" />
        </w:pPr>
        <w:r>
          <xsl:variable name="rStyle-val">
            <xsl:choose>
							<xsl:when test="self::xhtml:th and ../../self::xhtml:tbody and ../../../@class = 'CV chart'">
								<xsl:value-of select="'Phonetic'" />
							</xsl:when>
							<xsl:when test="not(@class)">
                <xsl:value-of select="''" />
              </xsl:when>
              <xsl:when test="starts-with(@class, 'Phonetic')">
                <xsl:value-of select="'Phonetic'" />
              </xsl:when>
              <xsl:when test="@class = 'sorting_field'">
                <xsl:value-of select="'Strong'" />
              </xsl:when>
              <xsl:when test="self::xhtml:td and ../@class = 'filter'">
                <xsl:value-of select="'Strong'" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="@class" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:if test="$rStyle-val != ''">
            <w:rPr>
              <w:rStyle w:val="{$rStyle-val}" />
            </w:rPr>
          </xsl:if>
					<xsl:choose>
						<xsl:when test="@class = 'Phonetic pair'">
							<w:t>
								<xsl:value-of select="concat(xhtml:ul/xhtml:li[1]/xhtml:span, '&#xA0;', xhtml:ul/xhtml:li[2]/xhtml:span)" />
							</w:t>
						</xsl:when>
						<xsl:when test="xhtml:strong">
							<xsl:variable name="text" select="//text()" />
							<w:t>
								<xsl:value-of select="$text" />
							</w:t>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates />
						</xsl:otherwise>
					</xsl:choose>
				</w:r>
      </w:p>
    </w:tc>
  </xsl:template>

	<xsl:template match="xhtml:td/xhtml:ul[@class = 'transcription']" />
  
  <xsl:template name="basicStyles">
    <w:versionOfBuiltInStylenames w:val="4" />
    <w:latentStyles w:defLockedState="off" w:latentStyleCount="156" />
    <w:style w:type="paragraph" w:default="on" w:styleId="Normal">
      <w:name w:val="Normal" />
      <w:rPr>
        <wx:font wx:val="Times New Roman" />
        <w:sz w:val="22" />
        <w:sz-cs w:val="24" />
        <w:lang w:val="EN-US" w:fareast="EN-US" w:bidi="AR-SA" />
      </w:rPr>
    </w:style>
    <w:style w:type="paragraph" w:styleId="Heading4">
      <w:name w:val="heading 4" />
      <wx:uiName wx:val="Heading 4" />
      <w:basedOn w:val="Normal" />
      <w:next w:val="BodyText" />
      <w:pPr>
        <w:pStyle w:val="Heading4" />
        <w:keepNext />
        <w:keepLines />
        <w:spacing w:before="240" w:after="120" />
        <w:outlineLvl w:val="3" />
      </w:pPr>
      <w:rPr>
        <w:b />
        <w:b-cs />
        <w:sz w:val="24" />
        <w:sz-cs w:val="24" />
      </w:rPr>
    </w:style>
    <w:style w:type="character" w:default="on" w:styleId="DefaultParagraphFont">
      <w:name w:val="Default Paragraph Font" />
      <w:semiHidden />
    </w:style>
    <w:style w:type="table" w:default="on" w:styleId="TableNormal">
      <w:name w:val="Normal Table" />
      <wx:uiName wx:val="Table Normal" />
      <w:semiHidden />
      <w:tblPr>
        <w:tblInd w:w="0" w:type="dxa" />
        <w:tblBorders>
          <w:top w:val="nil" />
          <w:left w:val="nil" />
          <w:bottom w:val="nil" />
          <w:right w:val="nil" />
          <w:insideH w:val="nil" />
          <w:insideV w:val="nil" />
        </w:tblBorders>
        <w:tblCellMar>
          <w:top w:w="0" w:type="dxa" />
          <w:left w:w="108" w:type="dxa" />
          <w:bottom w:w="0" w:type="dxa" />
          <w:right w:w="108" w:type="dxa" />
        </w:tblCellMar>
      </w:tblPr>
    </w:style>
    <w:style w:type="list" w:default="on" w:styleId="NoList">
      <w:name w:val="No List" />
      <w:semiHidden />
    </w:style>
    <w:style w:type="paragraph" w:styleId="BodyText">
      <w:name w:val="Body Text" />
      <w:basedOn w:val="Normal" />
      <w:pPr>
        <w:pStyle w:val="BodyText" />
        <w:spacing w:after="120" />
      </w:pPr>
    </w:style>
    <w:style w:type="paragraph" w:styleId="Footer">
      <w:name w:val="footer" />
      <wx:uiName wx:val="Footer" />
      <w:basedOn w:val="Normal" />
      <w:pPr>
        <w:pStyle w:val="Footer" />
        <w:pBdr>
          <w:top w:val="single" w:sz="4" wx:bdrwidth="10" w:space="1" w:color="auto" />
        </w:pBdr>
        <w:tabs>
          <w:tab w:val="right" w:pos="9360" />
          <w:tab w:val="right" w:pos="14400" />
          <w:tab w:val="right" w:pos="18720" />
        </w:tabs>
      </w:pPr>
      <w:rPr>
        <w:sz w:val="18" />
      </w:rPr>
    </w:style>
    <w:style w:type="paragraph" w:styleId="Header">
      <w:name w:val="header" />
      <wx:uiName wx:val="Header" />
      <w:basedOn w:val="Normal" />
      <w:pPr>
        <w:pStyle w:val="Header" />
        <w:pBdr>
          <w:bottom w:val="single" w:sz="4" wx:bdrwidth="10" w:space="1" w:color="auto" />
        </w:pBdr>
        <w:tabs>
          <w:tab w:val="right" w:pos="9360" />
          <w:tab w:val="right" w:pos="14400" />
          <w:tab w:val="right" w:pos="18720" />
        </w:tabs>
      </w:pPr>
    </w:style>
    <w:style w:type="character" w:styleId="PageNumber">
      <w:name w:val="page number" />
      <wx:uiName wx:val="Page Number" />
      <w:basedOn w:val="DefaultParagraphFont" />
    </w:style>
    <w:style w:type="paragraph" w:styleId="SpaceSingle">
      <w:name w:val="Space Single" />
      <w:basedOn w:val="Normal" />
      <w:pPr>
        <w:pStyle w:val="SpaceSingle" />
        <w:spacing w:line="240" w:line-rule="exact" />
      </w:pPr>
    </w:style>
    <w:style w:type="character" w:styleId="Strong">
      <w:name w:val="Strong" />
      <w:basedOn w:val="DefaultParagraphFont" />
      <w:rPr>
        <w:b />
        <w:b-cs />
      </w:rPr>
    </w:style>
    <w:style w:type="table" w:styleId="TableChart">
      <w:name w:val="Table Chart" />
      <w:basedOn w:val="TableNormal" />
      <w:pPr>
        <w:spacing w:before="20" w:after="20" />
      </w:pPr>
      <w:tblPr>
        <w:tblInd w:w="0" w:type="dxa" />
        <w:tblCellMar>
          <w:top w:w="0" w:type="dxa" />
          <w:left w:w="108" w:type="dxa" />
          <w:bottom w:w="0" w:type="dxa" />
          <w:right w:w="108" w:type="dxa" />
        </w:tblCellMar>
      </w:tblPr>
      <w:tcPr>
        <w:vAlign w:val="top" />
      </w:tcPr>
      <w:tblStylePr w:type="firstRow">
        <w:pPr>
          <w:keepNext />
        </w:pPr>
        <w:tblPr />
        <w:tcPr>
          <w:tcBorders>
            <w:top w:val="nil" />
            <w:left w:val="nil" />
            <w:bottom w:val="single" w:sz="4" wx:bdrwidth="10" w:space="0" w:color="auto" />
            <w:right w:val="nil" />
            <w:insideH w:val="nil" />
            <w:insideV w:val="nil" />
            <w:tl2br w:val="nil" />
            <w:tr2bl w:val="nil" />
          </w:tcBorders>
        </w:tcPr>
      </w:tblStylePr>
      <w:tblStylePr w:type="firstCol">
        <w:tblPr />
        <w:tcPr>
          <w:tcBorders>
            <w:top w:val="nil" />
            <w:left w:val="nil" />
            <w:bottom w:val="nil" />
            <w:right w:val="single" w:sz="4" wx:bdrwidth="10" w:space="0" w:color="auto" />
            <w:insideH w:val="nil" />
            <w:insideV w:val="nil" />
            <w:tl2br w:val="nil" />
            <w:tr2bl w:val="nil" />
          </w:tcBorders>
        </w:tcPr>
      </w:tblStylePr>
      <w:tblStylePr w:type="nwCell">
        <w:tblPr />
        <w:tcPr>
          <w:tcBorders>
            <w:top w:val="nil" />
            <w:left w:val="nil" />
            <w:bottom w:val="nil" />
            <w:right w:val="nil" />
            <w:insideH w:val="nil" />
            <w:insideV w:val="nil" />
            <w:tl2br w:val="nil" />
            <w:tr2bl w:val="nil" />
          </w:tcBorders>
        </w:tcPr>
      </w:tblStylePr>
    </w:style>
    <w:style w:type="paragraph" w:styleId="TableData">
      <w:name w:val="Table Data" />
      <w:basedOn w:val="Normal" />
      <w:pPr>
        <w:pStyle w:val="TableData" />
      </w:pPr>
    </w:style>
    <w:style w:type="paragraph" w:styleId="TablePhoneticItem">
      <w:name w:val="Table Phonetic Item" />
      <w:basedOn w:val="TableData" />
      <w:pPr>
        <w:pStyle w:val="TablePhoneticItem" />
        <w:jc w:val="center" />
      </w:pPr>
      <w:rPr>
        <w:b />
      </w:rPr>
    </w:style>
    <w:style w:type="paragraph" w:styleId="TableDataRight">
      <w:name w:val="Table Data Right" />
      <w:basedOn w:val="TableData" />
      <w:pPr>
        <w:pStyle w:val="TableDataRight" />
        <w:jc w:val="right" />
      </w:pPr>
    </w:style>
    <w:style w:type="paragraph" w:styleId="TableHeading">
      <w:name w:val="Table Heading" />
      <w:basedOn w:val="TableData" />
      <w:next w:val="TableData" />
      <w:pPr>
        <w:pStyle w:val="TableHeading" />
        <w:keepNext />
      </w:pPr>
    </w:style>
    <w:style w:type="paragraph" w:styleId="TableGroupExpanded">
      <w:name w:val="Table Group Expanded" />
      <w:basedOn w:val="TableDataRight" />
      <w:next w:val="TableDataRight" />
      <w:pPr>
        <w:pStyle w:val="TableGroupExpanded" />
        <w:keepNext />
      </w:pPr>
      <w:rPr>
        <w:sz w:val="22" />
      </w:rPr>
    </w:style>
    <w:style w:type="table" w:styleId="TableList">
      <w:name w:val="Table List" />
      <w:basedOn w:val="TableNormal" />
      <w:pPr>
        <w:spacing w:before="20" w:after="20" />
      </w:pPr>
      <w:tblPr>
        <w:tblInd w:w="0" w:type="dxa" />
        <w:tblCellMar>
          <w:top w:w="0" w:type="dxa" />
          <w:left w:w="108" w:type="dxa" />
          <w:bottom w:w="0" w:type="dxa" />
          <w:right w:w="108" w:type="dxa" />
        </w:tblCellMar>
      </w:tblPr>
      <w:tcPr>
        <w:vAlign w:val="top" />
      </w:tcPr>
      <w:tblStylePr w:type="firstRow">
        <w:pPr>
          <w:keepNext />
        </w:pPr>
        <w:tblPr />
        <w:tcPr>
          <w:tcBorders>
            <w:top w:val="nil" />
            <w:left w:val="nil" />
            <w:bottom w:val="single" w:sz="12" wx:bdrwidth="30" w:space="0" w:color="auto" />
            <w:right w:val="nil" />
            <w:insideH w:val="nil" />
            <w:insideV w:val="nil" />
            <w:tl2br w:val="nil" />
            <w:tr2bl w:val="nil" />
          </w:tcBorders>
        </w:tcPr>
      </w:tblStylePr>
    </w:style>
    <w:style w:type="table" w:styleId="TableDetails">
      <w:name w:val="Table Details" />
      <w:basedOn w:val="TableNormal" />
      <w:pPr>
        <w:spacing w:before="20" w:after="20" />
      </w:pPr>
      <w:tblPr>
        <w:tblInd w:w="0" w:type="dxa" />
        <w:tblCellMar>
          <w:top w:w="0" w:type="dxa" />
          <w:left w:w="108" w:type="dxa" />
          <w:bottom w:w="0" w:type="dxa" />
          <w:right w:w="108" w:type="dxa" />
        </w:tblCellMar>
      </w:tblPr>
      <w:tcPr>
        <w:vAlign w:val="top" />
      </w:tcPr>
    </w:style>
  </xsl:template>

  <!-- Convert field properties in exported file to font elements. -->
  <xsl:template name="fonts">
    <xsl:param name="formatting" />
    <w:fonts>
      <w:defaultFonts w:ascii="Times New Roman" w:fareast="Times New Roman" w:h-ansi="Times New Roman" w:cs="Times New Roman" />
      <xsl:for-each select="$formatting/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'font-family'] != 'Times New Roman']">
        <xsl:variable name="name" select="xhtml:td[@class = 'font-family']" />
        <xsl:if test="not(preceding::xhtml:tr[xhtml:td[@class = 'font-family'] = $name])">
          <w:font w:name="{$name}" />
        </xsl:if>
      </xsl:for-each>
    </w:fonts>
  </xsl:template>
  
  <!-- Convert field properties in exported file to style rules. -->
  
  <xsl:template name="projectCharacterStyles">
    <xsl:param name="formatting" />
    <xsl:for-each select="$formatting/xhtml:tbody/xhtml:tr">
			<xsl:variable name="font-family" select="xhtml:td[@class = 'font-family']" />
			<xsl:variable name="font-size" select="xhtml:td[@class = 'font-size']" />
			<w:style w:type="character" w:styleId="{xhtml:td[@class = 'class']}">
        <w:name w:val="{xhtml:td[@class = 'name']}" />
        <w:basedOn w:val="DefaultParagraphFont" />
        <w:rPr>
          <xsl:if test="string-length($font-family) != 0">
            <w:rFonts w:ascii="{$font-family}" w:h-ansi="{$font-family}" />
          </xsl:if>
          <xsl:if test="string-length($font-size) != 0">
            <w:sz w:val="{2 * number($font-size)}" />
          </xsl:if>
          <xsl:if test="xhtml:td[@class = 'font-weight'] = 'bold'">
            <w:b />
          </xsl:if>
          <xsl:if test="xhtml:td[@class = 'font-style'] = 'italic'">
            <w:i />
          </xsl:if>
        </w:rPr>
      </w:style>
    </xsl:for-each>
  </xsl:template>

	<!-- Table of details in Word documents is similar XHTML files. -->

	<xsl:template match="xhtml:div[@id = 'metadata']">
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
		<xsl:variable name="phoneticSearchSubfieldOrder" select="$sorting/xhtml:li[@class = 'phoneticSearchSubfieldOrder']/xhtml:ol" />
		<xsl:variable name="view" select="$details//xhtml:li[@class = 'view']" />
		<xsl:variable name="searchPattern" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'searchPattern']" />
		<xsl:variable name="filter" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'filter']" />
		<xsl:variable name="numberOfPhones" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfPhones']" />
		<xsl:variable name="numberOfPhonemes" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfPhonemes']" />
		<xsl:variable name="numberOfRecords" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfRecords']" />
		<xsl:variable name="numberOfGroups" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'numberOfGroups']" />
		<xsl:variable name="minimalPairs" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'minimalPairs']" />
		<xsl:variable name="projectName" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'projectName']" />
		<xsl:variable name="languageName" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'languageName']" />
		<xsl:variable name="languageCode" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'languageCode']" />
		<xsl:variable name="date" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'date']" />
		<xsl:variable name="time" select="$metadata/xhtml:ul[@class = 'details']/xhtml:li[@class = 'time']" />
		<xsl:call-template name="pStyleText">
			<xsl:with-param name="pStyle" select="'Heading4'" />
			<xsl:with-param name="text" select="/xhtml:html/xhtml:head/xhtml:title" />
		</xsl:call-template>
		<w:tbl>
			<w:tblPr>
				<w:tblStyle w:val="TableDetails" />
				<w:tblW w:w="0" w:type="auto" />
				<w:tblLook w:val="01E0" />
			</w:tblPr>
			<w:tblGrid>
				<w:gridCol />
				<w:gridCol />
			</w:tblGrid>
			<xsl:if test="$searchPattern">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'Search pattern:'" />
					<xsl:with-param name="text" select="$searchPattern" />
					<xsl:with-param name="rStyle" select="'Phonetic'" />
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$filter">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'Filter:'" />
					<xsl:with-param name="text" select="$filter" />
					<xsl:with-param name="rStyle" select="'Strong'" />
				</xsl:call-template>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$numberOfPhones">
					<xsl:call-template name="detailsRow">
						<xsl:with-param name="label" select="'Number of phones:'" />
						<xsl:with-param name="text" select="$numberOfPhones" />
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="$numberOfPhonemes">
					<xsl:call-template name="detailsRow">
						<xsl:with-param name="label" select="'Number of Phonemes:'" />
						<xsl:with-param name="text" select="$numberOfPhonemes" />
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="$numberOfRecords">
					<xsl:call-template name="detailsRow">
						<xsl:with-param name="label" select="'Number of records:'" />
						<xsl:with-param name="text" select="$numberOfRecords" />
					</xsl:call-template>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="$numberOfGroups">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'Number of groups:'" />
					<xsl:with-param name="text" select="$numberOfGroups" />
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$minimalPairs">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'Minimal pairs:'" />
					<xsl:with-param name="text">
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
					</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$sorting">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'Primary sort field:'" />
					<xsl:with-param name="text">
						<xsl:value-of select="$primarySortFieldName" />
						<xsl:if test="$primarySortFieldName = 'Phonetic' and $phoneticSortOptionName">
							<xsl:value-of select="concat(', ', $phoneticSortOptionName)" />
						</xsl:if>
						<xsl:if test="$primarySortFieldDirection = 'descending'">
							<xsl:value-of select="concat(', ', $primarySortFieldDirection)" />
						</xsl:if>
					</xsl:with-param>
				</xsl:call-template>
				<xsl:if test="$view = 'Search' and $phoneticSearchSubfieldOrder">
					<xsl:call-template name="detailsRow">
						<xsl:with-param name="label" select="'Phonetic sort options:'" />
						<xsl:with-param name="text">
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
			<xsl:if test="$projectName != $languageName">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'Project name:'" />
					<xsl:with-param name="text" select="$projectName" />
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$languageName">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'Language name:'" />
					<xsl:with-param name="text" select="$languageName" />
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$languageCode">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'ISO 639-3 code:'" />
					<xsl:with-param name="text" select="$languageCode" />
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$dateAndTime = 'true' and $date and $time">
				<xsl:call-template name="detailsRow">
					<xsl:with-param name="label" select="'Date and time:'" />
					<xsl:with-param name="text" select="concat($date, ' at ', $time)" />
				</xsl:call-template>
			</xsl:if>
		</w:tbl>
		<xsl:call-template name="pStyleText">
			<xsl:with-param name="pStyle" select="'SpaceSingle'" />
			<xsl:with-param name="text" select="''" />
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="detailsRow">
		<xsl:param name="label" />
		<xsl:param name="text" />
		<xsl:param name="rStyle" />
		<w:tr>
			<w:tc>
				<w:tcPr>
					<w:tcW w:w="0" w:type="auto" />
				</w:tcPr>
				<xsl:call-template name="pStyleText">
					<xsl:with-param name="pStyle" select="'TableData'" />
					<xsl:with-param name="text" select="$label" />
				</xsl:call-template>
			</w:tc>
			<w:tc>
				<w:tcPr>
					<w:tcW w:w="0" w:type="auto" />
				</w:tcPr>
				<xsl:call-template name="pStyleText">
					<xsl:with-param name="pStyle" select="'TableData'" />
					<xsl:with-param name="rStyle" select="$rStyle" />
					<xsl:with-param name="text" select="$text" />
				</xsl:call-template>
			</w:tc>
		</w:tr>
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

	<!-- Section parameters -->

  <!-- Default parameters which the caller can override -->
	<!--
  <xsl:param name="orientation" select="'Portrait'"/>
  <xsl:param name="paperSize" select="'Letter'"/>
	-->
  <xsl:param name="sectPr_type" select="''"/>
  <!-- default (next page) -->
  <!-- <xsl:param name="sectPr_type" select="'continuous'"/> -->

  <xsl:template name="sectPr">
    <xsl:choose>
      <xsl:when test="$orientation='Portrait' and $paperSize='A4'">
        <xsl:call-template name="sectPr-param">
          <!-- Values in parameters are for Measurement units: Millimeters. Values in comments are for Measurement units: Inches -->
          <xsl:with-param name="sectPr_pgSz_w" select="11907"/>
          <!-- 11909 -->
          <xsl:with-param name="sectPr_pgSz_h" select="16840"/>
          <!-- 16834 -->
          <xsl:with-param name="sectPr_pgSz_orient" select="''"/>
          <!-- Portrait -->
          <xsl:with-param name="sectPr_pgSz_code" select="'9'"/>
          <!-- A4 -->
          <xsl:with-param name="sectPr_pgMar_top" select="1928"/>
          <!-- 1930 -->
          <xsl:with-param name="sectPr_pgMar_right" select="1264"/>
          <!-- 1267 -->
          <xsl:with-param name="sectPr_pgMar_bottom" select="1928"/>
          <!-- 1930 -->
          <xsl:with-param name="sectPr_pgMar_left" select="1264"/>
          <!-- 1267 -->
          <xsl:with-param name="sectPr_pgMar_header" select="1208"/>
          <!-- 1210 -->
          <xsl:with-param name="sectPr_pgMar_footer" select="1208"/>
          <!-- 1210 -->
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$orientation='Portrait' and $paperSize='Legal'">
        <xsl:call-template name="sectPr-param">
          <xsl:with-param name="sectPr_pgSz_w" select="12240"/>
          <xsl:with-param name="sectPr_pgSz_h" select="20160"/>
          <xsl:with-param name="sectPr_pgSz_orient" select="''"/>
          <!-- Portrait -->
          <xsl:with-param name="sectPr_pgSz_code" select="'5'"/>
          <!-- Legal -->
          <xsl:with-param name="sectPr_pgMar_top" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_right" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_bottom" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_left" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_header" select="720"/>
          <xsl:with-param name="sectPr_pgMar_footer" select="720"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$orientation='Landscape' and $paperSize='Letter'">
        <xsl:call-template name="sectPr-param">
          <xsl:with-param name="sectPr_pgSz_w" select="15840"/>
          <xsl:with-param name="sectPr_pgSz_h" select="12240"/>
          <xsl:with-param name="sectPr_pgSz_orient" select="'landscape'"/>
          <xsl:with-param name="sectPr_pgSz_code" select="'1'"/>
          <!-- Letter -->
          <xsl:with-param name="sectPr_pgMar_top" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_right" select="720"/>
          <xsl:with-param name="sectPr_pgMar_bottom" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_left" select="720"/>
          <xsl:with-param name="sectPr_pgMar_header" select="720"/>
          <xsl:with-param name="sectPr_pgMar_footer" select="720"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$orientation='Landscape' and $paperSize='A4'">
        <xsl:call-template name="sectPr-param">
          <xsl:with-param name="sectPr_pgSz_w" select="16840"/>
          <!-- 16834 -->
          <xsl:with-param name="sectPr_pgSz_h" select="11907"/>
          <!-- 11909 -->
          <xsl:with-param name="sectPr_pgSz_orient" select="'landscape'"/>
          <xsl:with-param name="sectPr_pgSz_code" select="'9'"/>
          <xsl:with-param name="sectPr_pgMar_top" select="1264"/>
          <!-- 1267 -->
          <xsl:with-param name="sectPr_pgMar_right" select="1208"/>
          <!-- 1210 -->
          <xsl:with-param name="sectPr_pgMar_bottom" select="1264"/>
          <!-- 1267 -->
          <xsl:with-param name="sectPr_pgMar_left" select="1208"/>
          <!-- 1210 -->
          <xsl:with-param name="sectPr_pgMar_header" select="544"/>
          <!-- 547 -->
          <xsl:with-param name="sectPr_pgMar_footer" select="544"/>
          <!-- 547 -->
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$orientation='Landscape' and $paperSize='Legal'">
        <xsl:call-template name="sectPr-param">
          <xsl:with-param name="sectPr_pgSz_w" select="20160"/>
          <xsl:with-param name="sectPr_pgSz_h" select="12240"/>
          <xsl:with-param name="sectPr_pgSz_orient" select="'landscape'"/>
          <xsl:with-param name="sectPr_pgSz_code" select="'5'"/>
          <!-- Legal -->
          <xsl:with-param name="sectPr_pgMar_top" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_right" select="720"/>
          <xsl:with-param name="sectPr_pgMar_bottom" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_left" select="720"/>
          <xsl:with-param name="sectPr_pgMar_header" select="720"/>
          <xsl:with-param name="sectPr_pgMar_footer" select="720"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <!-- Default orientation=Portrait paperSize=Letter -->
        <xsl:call-template name="sectPr-param">
          <xsl:with-param name="sectPr_pgSz_w" select="12240"/>
          <xsl:with-param name="sectPr_pgSz_h" select="15840"/>
          <xsl:with-param name="sectPr_pgSz_orient" select="''"/>
          <!-- Portrait -->
          <xsl:with-param name="sectPr_pgSz_code" select="'1'"/>
          <!-- Letter -->
          <xsl:with-param name="sectPr_pgMar_top" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_right" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_bottom" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_left" select="1440"/>
          <xsl:with-param name="sectPr_pgMar_header" select="720"/>
          <xsl:with-param name="sectPr_pgMar_footer" select="720"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="sectPr-param">
    <xsl:param name="sectPr_pgSz_w"/>
    <xsl:param name="sectPr_pgSz_h"/>
    <xsl:param name="sectPr_pgSz_orient"/>
    <!-- Portrait -->
    <xsl:param name="sectPr_pgSz_code"/>
    <!-- Letter -->
    <xsl:param name="sectPr_pgMar_top"/>
    <xsl:param name="sectPr_pgMar_right"/>
    <xsl:param name="sectPr_pgMar_bottom"/>
    <xsl:param name="sectPr_pgMar_left"/>
    <xsl:param name="sectPr_pgMar_header"/>
    <xsl:param name="sectPr_pgMar_footer"/>
    <xsl:variable name="sectPr_pgMar_gutter" select="0"/>
    <xsl:variable name="sectPr_cols_space" select="720"/>
    <w:sectPr>
      <w:hdr w:type="odd">
        <w:p>
          <w:pPr>
            <w:pStyle w:val="Header"/>
          </w:pPr>
          <w:fldSimple w:instr=" DOCPROPERTY  Title  \* MERGEFORMAT ">
            <w:r>
              <w:rPr>
                <w:noProof/>
              </w:rPr>
              <w:t>
                <xsl:value-of select="/xhtml:html/xhtml:head/xhtml:title"/>
              </w:t>
            </w:r>
          </w:fldSimple>
          <w:r>
            <w:tab/>
            <xsl:if test="$orientation='Landscape'">
              <w:tab/>
              <xsl:if test="$paperSize='Legal'">
                <w:tab/>
              </xsl:if>
            </xsl:if>
          </w:r>
          <w:r>
            <w:fldChar w:fldCharType="begin"/>
          </w:r>
          <w:r>
            <w:instrText> PAGE </w:instrText>
          </w:r>
          <w:r>
            <w:fldChar w:fldCharType="separate"/>
          </w:r>
          <w:r>
            <w:rPr>
              <w:noProof/>
            </w:rPr>
            <w:t>1</w:t>
          </w:r>
          <w:r>
            <w:fldChar w:fldCharType="end"/>
          </w:r>
        </w:p>
      </w:hdr>
      <w:ftr w:type="odd">
        <w:p>
          <w:pPr>
            <w:pStyle w:val="Footer"/>
          </w:pPr>
          <w:fldSimple w:instr=" FILENAME  \* MERGEFORMAT ">
            <w:r>
              <w:rPr>
                <w:noProof/>
              </w:rPr>
              <w:t>
                <xsl:value-of select="$fileName"/>
              </w:t>
            </w:r>
          </w:fldSimple>
          <w:r>
            <w:tab/>
            <xsl:if test="$orientation='Landscape'">
              <w:tab/>
              <xsl:if test="$paperSize='Legal'">
                <w:tab/>
              </xsl:if>
            </xsl:if>
            <w:t>Edited on </w:t>
          </w:r>
					<!--
          <w:fldSimple w:instr=" SAVEDATE  \* MERGEFORMAT ">
            <w:r>
              <w:rPr>
                <w:noProof/>
              </w:rPr>
              <w:t>0/0/00 0:00:00 AM</w:t>
            </w:r>
          </w:fldSimple>
					-->
					<w:r>
						<w:fldChar w:fldCharType="begin" />
					</w:r>
					<w:r>
						<w:instrText> SAVEDATE  \@ "yyyy-MM-dd HH:mm"  \* MERGEFORMAT </w:instrText>
					</w:r>
					<w:r>
						<w:fldChar w:fldCharType="separate" />
					</w:r>
					<w:r>
						<w:rPr>
							<w:noProof />
						</w:rPr>
						<w:t>
							<xsl:choose>
								<xsl:when test="$dateAndTime = 'true' and string-length($date) != 0 and string-length($time) != 0">
									<xsl:value-of select="concat($date, ' ', $time)" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'yyyy-mm-dd hh:mm'" />
								</xsl:otherwise>
							</xsl:choose>
						</w:t>
					</w:r>
					<w:r>
						<w:fldChar w:fldCharType="end" />
					</w:r>
				</w:p>
      </w:ftr>
      <xsl:if test="$sectPr_type != ''">
        <w:type w:val="{$sectPr_type}"/>
      </xsl:if>
      <xsl:choose>
        <xsl:when test="$sectPr_pgSz_orient != ''">
          <w:pgSz w:w="{$sectPr_pgSz_w}" w:h="{$sectPr_pgSz_h}" w:orient="{$sectPr_pgSz_orient}" w:code="{$sectPr_pgSz_code}"/>
        </xsl:when>
        <xsl:otherwise>
          <w:pgSz w:w="{$sectPr_pgSz_w}" w:h="{$sectPr_pgSz_h}" w:code="{$sectPr_pgSz_code}"/>
        </xsl:otherwise>
      </xsl:choose>
      <w:pgMar w:top="{$sectPr_pgMar_top}" w:right="{$sectPr_pgMar_right}" w:bottom="{$sectPr_pgMar_bottom}" w:left="{$sectPr_pgMar_left}" w:header="{$sectPr_pgMar_header}" w:footer="{$sectPr_pgMar_footer}" w:gutter="{$sectPr_pgMar_gutter}"/>
      <w:cols w:space="{$sectPr_cols_space}"/>
      <w:docGrid w:line-pitch="360"/>
    </w:sectPr>
  </xsl:template>

  <!-- Omit feature lists and tables in CV charts. -->
  <xsl:template match="xhtml:ul[@class = 'articulatory features']" />
  <xsl:template match="xhtml:ul[@class = 'binary features']" />
  <xsl:template match="xhtml:table[@class = 'articulatory features']" />
  <xsl:template match="xhtml:table[@class = 'binary features']" />

</xsl:stylesheet>