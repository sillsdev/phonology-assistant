<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_2a.xsl 2010-04-03 -->
	<!-- Convert from list to table with column groups, columns, row groups, and rows. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="hyphenateColumnHeadings" select="$options/xhtml:li[@class = 'hyphenatedColumnHeadings']" />

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />

	<xsl:variable name="colA">
    <xsl:choose>
      <xsl:when test="$view = 'Consonant Chart'">
        <xsl:value-of select="'Voiceless'" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'Unrounded'" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name="colB">
    <xsl:choose>
      <xsl:when test="$view = 'Consonant Chart'">
        <xsl:value-of select="'Voiced'" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'Rounded'" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:body/xhtml:ul[@class = 'CV chart']">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <xsl:apply-templates select="@*" />
      <colgroup>
        <col />
      </colgroup>
      <xsl:for-each select="//xhtml:ul[@class = 'colgroup features']/xhtml:li">
        <colgroup>
          <col />
          <col />
        </colgroup>
      </xsl:for-each>
      <thead>
        <tr>
          <th />
          <xsl:for-each select="//xhtml:ul[@class = 'colgroup features']/xhtml:li">
            <xsl:call-template name="thColumnGroup">
              <xsl:with-param name="feature" select="." />
            </xsl:call-template>
          </xsl:for-each>
        </tr>
      </thead>
      <xsl:for-each select="//xhtml:ul[@class = 'rowgroup features']/xhtml:li">
        <xsl:variable name="rowgroup" select="xhtml:span" />
        <xsl:variable name="rowspan" select="count(xhtml:ul/xhtml:li)" />
        <tbody>
          <xsl:for-each select="xhtml:ul/xhtml:li">
            <xsl:variable name="row" select="." />
            <tr>
              <xsl:if test="position() = 1">
                <xsl:call-template name="thRowGroup">
                  <xsl:with-param name="feature" select="$rowgroup" />
                  <xsl:with-param name="rowspan" select="$rowspan" />
                </xsl:call-template>
              </xsl:if>
              <xsl:for-each select="//xhtml:ul[@class = 'colgroup features']/xhtml:li">
                <xsl:variable name="colgroup" select="." />
                <xsl:call-template name="cell">
                  <xsl:with-param name="colgroup" select="$colgroup" />
                  <xsl:with-param name="rowgroup" select="$rowgroup" />
                  <xsl:with-param name="row" select="$row" />
                  <xsl:with-param name="col" select="$colA" />
                </xsl:call-template>
                <xsl:call-template name="cell">
                  <xsl:with-param name="colgroup" select="$colgroup" />
                  <xsl:with-param name="rowgroup" select="$rowgroup" />
                  <xsl:with-param name="row" select="$row" />
                  <xsl:with-param name="col" select="$colB" />
                </xsl:call-template>
              </xsl:for-each>
            </tr>
          </xsl:for-each>
        </tbody>
      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template name="thColumnGroup">
    <xsl:param name="feature" />
    <th scope="colgroup" colspan="2" title="{$feature}" xmlns="http://www.w3.org/1999/xhtml">
      <xsl:choose>
        <xsl:when test="not($hyphenateColumnHeadings = 'true')">
          <xsl:value-of select="$feature" />
        </xsl:when>
				<xsl:when test="$feature = 'Front' and not(following-sibling::*[. = 'Near-front']) and //xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:div[@class = 'features']/xhtml:ul[@class = 'articulatory']/xhtml:li = 'Near-front']">
					<xsl:value-of select="'Front&#xA0;or'" />
					<br />
					<xsl:value-of select="'Near-front'" />
				</xsl:when>
				<xsl:when test="$feature = 'Back' and not(following-sibling::*[. = 'Near-back']) and //xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:div[@class = 'features']/xhtml:ul[@class = 'articulatory']/xhtml:li = 'Near-back']">
					<xsl:value-of select="'Back&#xA0;or'" />
					<br />
					<xsl:value-of select="'Near-back'" />
				</xsl:when>
				<xsl:when test="$feature = 'Labiodental'">
          <xsl:value-of select="'Labio-'" />
          <br />
          <xsl:value-of select="'dental'" />
        </xsl:when>
				<xsl:when test="$feature = 'Linguolabial'">
					<xsl:value-of select="'Linguo-'" />
					<br />
					<xsl:value-of select="'labial'" />
				</xsl:when>
				<xsl:when test="$feature = 'Postalveolar'">
          <xsl:value-of select="'Post-'" />
          <br />
          <xsl:value-of select="'alveolar'" />
        </xsl:when>
        <xsl:when test="$feature = 'Retroflex'">
          <xsl:value-of select="'Retro-'" />
          <br />
          <xsl:value-of select="'flex'" />
        </xsl:when>
				<xsl:when test="$feature = 'Alveolo-palatal'">
					<xsl:value-of select="'Alveolo-'" />
					<br />
					<xsl:value-of select="'palatal'" />
				</xsl:when>
				<xsl:when test="$feature = 'Pharyngeal'">
          <xsl:value-of select="'Pharyn-'" />
          <br />
          <xsl:value-of select="'geal'" />
        </xsl:when>
        <xsl:when test="$feature = 'Epiglottal'">
          <xsl:value-of select="'Epi-'" />
          <br />
          <xsl:value-of select="'glottal'" />
        </xsl:when>
        <xsl:when test="$feature = 'Near-front'">
          <xsl:value-of select="'Near-'" />
          <br />
          <xsl:value-of select="'front'" />
        </xsl:when>
        <xsl:when test="$feature = 'Near-back'">
          <xsl:value-of select="'Near-'" />
          <br />
          <xsl:value-of select="'back'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$feature" />
        </xsl:otherwise>
      </xsl:choose>
    </th>
  </xsl:template>

  <xsl:template name="thRowGroup">
    <xsl:param name="feature" />
    <xsl:param name="rowspan" />
    <th scope="rowgroup" rowspan="{$rowspan}" title="{$feature}" xmlns="http://www.w3.org/1999/xhtml">
      <xsl:choose>
        <xsl:when test="not($hyphenateColumnHeadings = 'true')">
          <xsl:value-of select="$feature" />
        </xsl:when>
        <xsl:when test="$feature = 'Lateral Affricate'">
          <xsl:choose>
            <xsl:when test="$rowspan = 1">
              <xsl:value-of select="'Lateral affricate'" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Lateral'" />
              <br />
              <xsl:value-of select="'affricate'" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="$feature = 'Lateral Fricative'">
          <xsl:choose>
            <xsl:when test="$rowspan = 1">
              <xsl:value-of select="'Lateral fricative'" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Lateral'" />
              <br />
              <xsl:value-of select="'fricative'" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="$feature = 'Lateral Approximant'">
          <xsl:choose>
            <xsl:when test="$rowspan = 1">
              <xsl:value-of select="'Lateral approximant'" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Lateral'" />
              <br />
              <xsl:value-of select="'approximant'" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$feature" />
        </xsl:otherwise>
      </xsl:choose>
    </th>
  </xsl:template>

  <xsl:template name="cell">
    <xsl:param name="colgroup" />
    <xsl:param name="rowgroup" />
    <xsl:param name="row" />
    <xsl:param name="col" />
    <td class="Phonetic" xmlns="http://www.w3.org/1999/xhtml">
      <ul>
        <xsl:for-each select="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'colgroup'] = $colgroup][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'rowgroup'] = $rowgroup][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'row'] = $row][xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'col'] = $col]">
          <xsl:copy>
            <xsl:apply-templates select="@*|node()" />
          </xsl:copy>
        </xsl:for-each>
      </ul>
    </td>
  </xsl:template>

  <xsl:template match="xhtml:ul[@class = 'chart features']" />
  <xsl:template match="xhtml:ul[@class = 'colgroup features']" />
  <xsl:template match="xhtml:ul[@class = 'rowgroup features']" />

</xsl:stylesheet>