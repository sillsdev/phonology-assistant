<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_distribution_chart_transpose.xsl 2011-05-27 -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="distributionChartTransposed" select="$options/xhtml:li[@class = 'distributionChartTransposed']" />
	<!-- TO DO: If this becomes a property of the individual chart, where would it go in the metadata? -->

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:table[@class = 'distribution chart']">
    <xsl:choose>
      <xsl:when test="$distributionChartTransposed = 'true'">
        <xsl:variable name="generalChart" select="xhtml:*/xhtml:tr[@class = 'general']" />
        <xsl:variable name="generalEnvironmentsTransposed" select="xhtml:tbody/xhtml:tr[@class = 'general']" />
        <xsl:variable name="generalItemsTransposed" select="xhtml:thead/xhtml:tr[@class = 'general']" />
        <xsl:copy>
          <xsl:apply-templates select="@*" />
          <xsl:choose>
            <xsl:when test="$generalChart">
              <colgroup xmlns="http://www.w3.org/1999/xhtml">
                <xsl:for-each select="xhtml:thead/xhtml:tr">
                  <col />
                </xsl:for-each>
              </colgroup>
              <xsl:choose>
                <xsl:when test="$generalEnvironmentsTransposed">
                  <xsl:for-each select="xhtml:tbody">
                    <colgroup xmlns="http://www.w3.org/1999/xhtml">
                      <xsl:for-each select="xhtml:tr">
                        <col />
                      </xsl:for-each>
                    </colgroup>
                  </xsl:for-each>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:for-each select="xhtml:tbody/xhtml:tr">
                    <colgroup xmlns="http://www.w3.org/1999/xhtml">
                      <col />
                    </colgroup>
                  </xsl:for-each>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
							<colgroup xmlns="http://www.w3.org/1999/xhtml">
								<xsl:for-each select="xhtml:thead/xhtml:tr">
									<col xmlns="http://www.w3.org/1999/xhtml" />
								</xsl:for-each>
							</colgroup>
							<colgroup xmlns="http://www.w3.org/1999/xhtml">
								<xsl:for-each select="xhtml:tbody/xhtml:tr">
									<col xmlns="http://www.w3.org/1999/xhtml" />
								</xsl:for-each>
							</colgroup>
            </xsl:otherwise>
          </xsl:choose>
          <thead xmlns="http://www.w3.org/1999/xhtml">
            <xsl:choose>
              <xsl:when test="$generalChart">
                <xsl:choose>
                  <xsl:when test="$generalEnvironmentsTransposed">
                    <tr class="general">
                      <th>
                        <xsl:attribute name="colspan">
                          <xsl:value-of select="count(xhtml:thead/xhtml:tr)" />
                        </xsl:attribute>
                        <xsl:attribute name="rowspan">
                          <xsl:value-of select="count(xhtml:tbody[1]/xhtml:tr[1]/xhtml:th)" />
                        </xsl:attribute>
                      </th>
                      <xsl:apply-templates select="xhtml:tbody/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')]" mode="transposeGeneral" />
                    </tr>
                    <tr class="individual">
                      <xsl:apply-templates select="xhtml:tbody/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')]" mode="transposeIndividual" />
                    </tr>
                  </xsl:when>
                  <xsl:otherwise>
                    <th>
                      <xsl:attribute name="colspan">
                        <xsl:value-of select="count(xhtml:thead/xhtml:tr)" />
                      </xsl:attribute>
                      <xsl:attribute name="rowspan">
                        <xsl:value-of select="1" />
                      </xsl:attribute>
                    </th>
                    <tr class="individual">
                      <xsl:apply-templates select="xhtml:tbody/xhtml:tr/xhtml:th" mode="transposeGeneralized" />
                    </tr>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <tr>
                  <th />
                  <xsl:apply-templates select="xhtml:tbody/xhtml:tr/xhtml:th" mode="transpose" />
                </tr>
              </xsl:otherwise>
            </xsl:choose>
          </thead>
          <xsl:choose>
            <xsl:when test="$generalItemsTransposed">
              <xsl:for-each select="xhtml:thead/xhtml:tr[@class = 'general']/xhtml:th[contains(@class, 'Phonetic')]">
                <xsl:variable name="position" select="position()" />
                <xsl:variable name="positionInData">
                  <xsl:apply-templates select="../../xhtml:tr[@class = 'individual']/xhtml:th[not(@class)][$position]" mode="positionInData" />
                </xsl:variable>
                <tbody xmlns="http://www.w3.org/1999/xhtml">
                  <tr class="general">
                    <xsl:apply-templates select="." mode="transposeGeneral" />
                    <th />
                    <xsl:apply-templates select="../../../xhtml:tbody/xhtml:tr/xhtml:td[number($positionInData)]" />
                  </tr>
                  <xsl:apply-templates select="../../xhtml:tr[@class = 'individual']/xhtml:th[not(@class)][$position]" mode="transposeIndividual" />
                </tbody>
              </xsl:for-each>
            </xsl:when>
            <xsl:otherwise>
              <tbody xmlns="http://www.w3.org/1999/xhtml">
                <xsl:variable name="tbody" select="xhtml:tbody" />
                <xsl:for-each select="xhtml:thead/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')]">
                  <tr>
                    <xsl:apply-templates select="." mode="transpose" />
                    <xsl:variable name="position" select="position()" />
                    <xsl:apply-templates select="$tbody/xhtml:tr/xhtml:td[$position]" />
                  </tr>
                </xsl:for-each>
              </tbody>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:copy>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy>
          <xsl:apply-templates select="@* | node()" />
        </xsl:copy>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Column headings in generalized distribution charts. -->

  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th" mode="transposeGeneral">
    <xsl:if test="../@class = 'general'">
      <xsl:copy>
        <xsl:apply-templates select="@class" />
        <xsl:attribute name="scope">
          <xsl:value-of select="'colgroup'" />
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select="@rowspan" />
        </xsl:attribute>
        <xsl:apply-templates />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th" mode="transposeIndividual">
    <xsl:copy>
      <xsl:if test="../@class = 'individual'">
        <xsl:apply-templates select="@class" />
      </xsl:if>
      <xsl:attribute name="scope">
        <xsl:value-of select="'col'" />
      </xsl:attribute>
      <xsl:if test="../@class = 'individual'">
        <xsl:apply-templates />
      </xsl:if>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th" mode="transposeGeneralized">
    <xsl:copy>
      <xsl:apply-templates select="@class" />
      <xsl:attribute name="scope">
        <xsl:value-of select="'col'" />
      </xsl:attribute>
      <xsl:apply-templates />
    </xsl:copy>
  </xsl:template>

  <!-- Row headings in generalized distribution charts. -->

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th" mode="transposeGeneral">
    <xsl:if test="../@class = 'general'">
      <xsl:copy>
        <xsl:apply-templates select="@class" />
        <xsl:attribute name="scope">
          <xsl:value-of select="'rowgroup'" />
        </xsl:attribute>
        <xsl:attribute name="rowspan">
          <xsl:value-of select="@colspan" />
        </xsl:attribute>
        <xsl:apply-templates />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[not(@class)]" mode="transposeIndividual">
    <xsl:apply-templates select="following-sibling::xhtml:th[1][contains(@class, 'Phonetic')][not(contains(., '['))][string-length(translate(., '*+#_', '')) != 0]" mode="transposeIndividual" />
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')]" mode="transposeIndividual">
    <xsl:variable name="positionInData">
      <xsl:apply-templates select="." mode="positionInData" />
    </xsl:variable>
    <tr class="individual" xmlns="http://www.w3.org/1999/xhtml">
      <xsl:copy>
        <xsl:apply-templates select="@class" />
        <xsl:attribute name="scope">
          <xsl:value-of select="'row'" />
        </xsl:attribute>
        <xsl:apply-templates />
      </xsl:copy>
      <xsl:apply-templates select="../../../xhtml:tbody/xhtml:tr/xhtml:td[number($positionInData)]" />
    </tr>
    <xsl:apply-templates select="following-sibling::xhtml:th[1][contains(@class, 'Phonetic')][not(contains(., '['))][string-length(translate(., '*+#_', '')) != 0]" mode="transposeIndividual" />
  </xsl:template>

  <xsl:template match="xhtml:th" mode="positionInData">
    <xsl:value-of select="count(preceding-sibling::xhtml:th) + 1" />
  </xsl:template>


  <!-- Headings in non-generalized distribution charts. -->
  
  <xsl:template match="xhtml:th" mode="transpose">
    <xsl:copy>
      <xsl:apply-templates select="@*" mode="transpose" />
      <xsl:apply-templates />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="@*" mode="transpose">
    <xsl:copy-of select="." />
  </xsl:template>

  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th/@scope" mode="transpose">
    <xsl:attribute name="scope">
      <xsl:value-of select="'col'" />
    </xsl:attribute>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th/@scope" mode="transpose">
    <xsl:attribute name="scope">
      <xsl:value-of select="'row'" />
    </xsl:attribute>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th/@colspan" mode="transpose" />

</xsl:stylesheet>