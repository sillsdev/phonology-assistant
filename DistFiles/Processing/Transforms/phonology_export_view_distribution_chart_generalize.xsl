<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_distribution_chart_generalize.xsl 2011-11-03 -->
	<!-- Assuming that the researcher has manually generalized a distribution chart, -->
	<!-- separate general and individual elements if either or both of the following are true: -->
	<!-- * items: the chart name contains [gi], [giC], or [giV]. -->
	<!--          C or V means insert a consonant or vowel chart with environment tabs. -->
	<!-- * environments: the chart name contains [ge]. -->

	<!-- Important: If format is not XHTML, copy the table with no changes. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />

	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />

	<xsl:variable name="view">
		<xsl:choose>
			<xsl:when test="$details/xhtml:li[@class = 'view'] = 'Distribution Chart'">
				<xsl:value-of select="'Distribution'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$details/xhtml:li[@class = 'view']" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<xsl:variable name="title" select="/xhtml:html/xhtml:head/xhtml:title" />

	<!-- Simplify title and heading text if it is a two-word view in Phonology Assistant. -->
	<xsl:variable name="heading" select="'Distribution'" />

	<xsl:variable name="subheading">
		<xsl:if test="$view = 'Distribution'">
			<xsl:variable name="name" select="$details/xhtml:li[@class = 'chart name' or @class = 'name']" />
			<xsl:if test="$name">
				<xsl:variable name="gi">
					<xsl:choose>
						<xsl:when test="contains($name, ' [gi')">
							<xsl:value-of select="substring-before($name, ' [gi')" />
						</xsl:when>
						<xsl:when test="contains($name, '[gi')">
							<xsl:value-of select="substring-before($name, '[gi')" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$name" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="contains($gi, ' [ge')">
						<xsl:value-of select="substring-before($gi, ' [ge')" />
					</xsl:when>
					<xsl:when test="contains($gi, '[ge')">
						<xsl:value-of select="substring-before($gi, '[ge')" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$gi" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="generalizeEnvironments">
		<xsl:choose>
			<xsl:when test="$view = 'Distribution' and contains($title, '[ge]')">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="generalizeItems">
		<xsl:choose>
			<xsl:when test="$view = 'Distribution' and contains($title, '[gi]') or contains($title, '[giC]') or contains($title, '[giV]')">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<!-- TO DO: In the metadata, these will become properties of the individual chart. -->

	<xsl:variable name="generalEnvironments">
    <xsl:choose>
      <xsl:when test="$generalizeEnvironments = 'true' and //xhtml:table[@class = 'distribution chart']/xhtml:thead/xhtml:tr/xhtml:th[contains(., '[')][following-sibling::xhtml:th[1][not(contains(., '['))][string-length(translate(., '*+#_', '')) != 0]]">
        <xsl:value-of select="'true'" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'false'" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="generalItems">
    <xsl:choose>
      <xsl:when test="$generalizeItems = 'true' and //xhtml:table[@class = 'distribution chart']/xhtml:tbody/xhtml:tr[xhtml:th[contains(., '[')]][following-sibling::xhtml:tr[1][xhtml:th[not(contains(., '['))][string-length(translate(., '*+#_', '')) != 0]]]">
        <xsl:value-of select="'true'" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'false'" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="@* | node()" mode="generalize">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" mode="generalize" />
    </xsl:copy>
  </xsl:template>

	<!-- Replace obsolete title in metadata. -->
	<xsl:template match="xhtml:div[@id = 'metadata']/xhtml:ul[@class = 'details']/xhtml:li[@class = 'view'][. = 'Distribution Chart']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="'Distribution'" />
		</xsl:copy>
	</xsl:template>

	<!-- Append language name to title (but not heading). -->
	<xsl:template match="/xhtml:html/xhtml:head/xhtml:title">
		<xsl:variable name="languageName" select="$details/xhtml:li[@class = 'languageName']" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="$heading" />
			<xsl:if test="string-length($languageName) != 0 and $languageName != 'Undetermined'">
				<xsl:value-of select="' for '" />
				<xsl:value-of select="$languageName" />
			</xsl:if>
			<xsl:if test="string-length($subheading) != 0">
				<xsl:value-of select="': '" />
				<xsl:value-of select="$subheading" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'distribution chart']">
		<h3 xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="$heading" />
			<xsl:if test="string-length($subheading) != 0">
				<xsl:value-of select="': '" />
				<xsl:value-of select="$subheading" />
			</xsl:if>
		</h3>
		<xsl:choose>
			<xsl:when test="$format = 'XHTML' and ($generalEnvironments = 'true' or $generalItems = 'true')">
				<xsl:copy>
					<xsl:apply-templates select="@*" mode="generalize" />
					<!-- Replace original colgroups according to the heading row. -->
					<xsl:apply-templates select="xhtml:thead/xhtml:tr/xhtml:th[1]" mode="colgroup" />
					<xsl:choose>
						<xsl:when test="$generalEnvironments = 'true'">
							<xsl:apply-templates select="xhtml:thead/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')][1]" mode="colgroup" />
						</xsl:when>
						<xsl:otherwise>
							<colgroup xmlns="http://www.w3.org/1999/xhtml">
								<xsl:for-each select="xhtml:thead/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')]">
									<col />
								</xsl:for-each>
							</colgroup>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:apply-templates select="xhtml:thead" mode="generalize" />
					<xsl:apply-templates select="xhtml:tbody" mode="generalize" />
				</xsl:copy>				
			</xsl:when>
			<!-- To ignore the following rules, copy instead of apply-templates. -->
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[1]" mode="colgroup">
    <colgroup xmlns="http://www.w3.org/1999/xhtml">
      <col />
      <xsl:if test="$generalItems = 'true'">
        <col />
      </xsl:if>
    </colgroup>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')]" mode="colgroup">
    <xsl:choose>
      <xsl:when test="$generalEnvironments = 'true' and contains(., '[')">
        <colgroup xmlns="http://www.w3.org/1999/xhtml">
          <col />
          <xsl:apply-templates select="following-sibling::xhtml:th[1][not(contains(., '['))][string-length(translate(., '*+#_', '')) != 0]" mode="col" />
        </colgroup>
        <xsl:apply-templates select="following-sibling::xhtml:th[contains(., '[') or string-length(translate(., '*+#_', '')) = 0][1]" mode="colgroup" />
      </xsl:when>
      <xsl:otherwise>
        <colgroup xmlns="http://www.w3.org/1999/xhtml">
          <col />
        </colgroup>
        <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="colgroup" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th" mode="col">
    <col xmlns="http://www.w3.org/1999/xhtml" />
    <xsl:apply-templates select="following-sibling::xhtml:th[1][not(contains(., '['))][string-length(translate(., '*+#_', '')) != 0]" mode="col" />
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr" mode="generalize">
    <xsl:choose>
      <xsl:when test="$generalEnvironments = 'true'">
        <xsl:apply-templates select="." mode="general" />
        <xsl:apply-templates select="." mode="individual" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy>
          <xsl:apply-templates select="xhtml:th" mode="generalize" />
        </xsl:copy>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[1]" mode="generalize">
    <xsl:copy>
      <xsl:attribute name="colspan">
        <xsl:value-of select="2" />
      </xsl:attribute>
      <xsl:attribute name="rowspan">
        <xsl:value-of select="1" />
      </xsl:attribute>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')]" mode="generalize">
    <xsl:copy>
      <xsl:apply-templates select="@class" mode="generalize" />
      <xsl:attribute name="scope">
        <xsl:value-of select="'col'" />
      </xsl:attribute>
      <xsl:apply-templates mode="generalize" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr" mode="general">
    <xsl:copy>
      <xsl:attribute name="class">
        <xsl:value-of select="'general'" />
      </xsl:attribute>
      <xsl:apply-templates select="xhtml:th[1]" mode="general" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[1]" mode="general">
    <xsl:copy>
      <xsl:attribute name="colspan">
        <xsl:choose>
          <xsl:when test="$generalItems = 'true'">
            <xsl:value-of select="2" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="1" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="rowspan">
        <xsl:value-of select="2" />
      </xsl:attribute>
    </xsl:copy>
    <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="general" />
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th[contains(@class, 'Phonetic')]" mode="general">
    <xsl:copy>
      <xsl:apply-templates select="@class" mode="generalize" />
      <xsl:attribute name="scope">
        <xsl:value-of select="'colgroup'" />
      </xsl:attribute>
      <xsl:attribute name="colspan">
        <xsl:choose>
          <xsl:when test="contains(., '[') and following-sibling::xhtml:th">
            <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="colspan" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="1" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates mode="generalize" />
    </xsl:copy>
    <xsl:choose>
      <xsl:when test="contains(., '[')">
        <!-- If this is a generalized search environment, the next non-individual item is the next colgroup. -->
        <xsl:apply-templates select="following-sibling::xhtml:th[contains(., '[') or string-length(translate(., '*+#_', '')) = 0][1]" mode="general" />
      </xsl:when>
      <xsl:otherwise>
        <!-- If this is not a generalized search item, the next item is the next colgroup. -->
        <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="general" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:th" mode="colspan">
    <xsl:param name="colspan" select="1" />
    <xsl:choose>
      <xsl:when test="contains(., '[') or string-length(translate(., '*+#_', '')) = 0">
        <xsl:value-of select="$colspan" />
      </xsl:when>
      <xsl:when test="following-sibling::xhtml:th">
        <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="colspan">
          <xsl:with-param name="colspan" select="$colspan + 1" />
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$colspan + 1" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="xhtml:thead/xhtml:tr" mode="individual">
    <xsl:copy>
      <xsl:attribute name="class">
        <xsl:value-of select="'individual'" />
      </xsl:attribute>
      <xsl:apply-templates select="xhtml:th[contains(@class, 'Phonetic')][1]" mode="individual-colgroup" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th" mode="individual-colgroup">
    <!-- In the individual heading row, the first cell of each colgroup is empty. -->
    <xsl:copy />
    <xsl:choose>
      <xsl:when test="contains(., '[') and following-sibling::xhtml:th[1][not(contains(., '['))][string-length(translate(., '*+#_', '')) != 0]">
        <!-- This colgroup contains one or more individual cells. -->
        <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="individual-col" />
      </xsl:when>
      <xsl:otherwise>
        <!-- This colgroup consists of only one cell. -->
        <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="individual-colgroup" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:thead/xhtml:tr/xhtml:th" mode="individual-col">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" mode="generalize" />
    </xsl:copy>
    <xsl:choose>
      <xsl:when test="following-sibling::xhtml:th[1][not(contains(., '['))][string-length(translate(., '*+#_', '')) != 0]">
        <!-- This colgroup contains more individual cells. -->
        <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="individual-col" />
      </xsl:when>
      <xsl:otherwise>
        <!-- This colgroup does not contain any more cells. -->
        <xsl:apply-templates select="following-sibling::xhtml:th[1]" mode="individual-colgroup" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:tbody" mode="generalize">
    <xsl:choose>
      <xsl:when test="$generalItems = 'true'">
        <xsl:apply-templates select="xhtml:tr[1]" mode="general" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy>
          <xsl:apply-templates mode="generalize" />
        </xsl:copy>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:tbody/xhtml:tr" mode="general">
    <tbody xmlns="http://www.w3.org/1999/xhtml">
      <xsl:copy>
        <xsl:attribute name="class">
          <xsl:value-of select="'general'" />
        </xsl:attribute>
        <xsl:apply-templates select="xhtml:th" mode="general">
          <xsl:with-param name="rowspan">
            <xsl:choose>
              <xsl:when test="contains(xhtml:th, '[') and following-sibling::xhtml:tr">
                <xsl:apply-templates select="following-sibling::xhtml:tr[1]" mode="rowspan" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:with-param>
        </xsl:apply-templates>
        <xsl:apply-templates select="xhtml:td" mode="generalize" />
      </xsl:copy>
      <!-- If this is a generalized search item, any following individuals are in its group. -->
      <xsl:if test="contains(xhtml:th, '[')">
        <xsl:apply-templates select="following-sibling::xhtml:tr[1][not(contains(xhtml:th, '['))][string-length(translate(xhtml:th, '*+#_', '')) != 0]" mode="individual" />
      </xsl:if>
    </tbody>
    <xsl:choose>
      <xsl:when test="contains(xhtml:th, '[')">
        <!-- If this is a generalized search item, the next non-individual item is in another group. -->
        <xsl:apply-templates select="following-sibling::xhtml:tr[contains(xhtml:th, '[') or string-length(translate(xhtml:th, '*+#_', '')) = 0][1]" mode="general" />
      </xsl:when>
      <xsl:otherwise>
        <!-- If this is not a generalized search item, the following row is in another group. -->
        <xsl:apply-templates select="following-sibling::xhtml:tr[1]" mode="general" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:tbody/xhtml:tr" mode="individual">
    <xsl:copy>
      <xsl:attribute name="class">
        <xsl:value-of select="'individual'" />
      </xsl:attribute>
      <xsl:apply-templates mode="generalize" />
    </xsl:copy>
    <xsl:apply-templates select="following-sibling::xhtml:tr[1][not(contains(xhtml:th, '['))][string-length(translate(xhtml:th, '*+#_', '')) != 0]" mode="individual" />
  </xsl:template>

  <xsl:template match="xhtml:tr" mode="rowspan">
    <xsl:param name="rowspan" select="1" />
    <xsl:choose>
      <xsl:when test="contains(xhtml:th, '[') or string-length(translate(., '*+#_', '')) = 0">
        <xsl:value-of select="$rowspan" />
      </xsl:when>
      <xsl:when test="following-sibling::xhtml:tr">
        <xsl:apply-templates select="following-sibling::xhtml:tr[1]" mode="rowspan">
          <xsl:with-param name="rowspan" select="$rowspan + 1" />
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$rowspan + 1" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:tbody/xhtml:tr/xhtml:th" mode="general">
    <xsl:param name="rowspan" />
    <xsl:copy>
      <xsl:apply-templates select="@class" mode="generalize" />
      <xsl:attribute name="scope">
        <xsl:value-of select="'rowgroup'" />
      </xsl:attribute>
      <xsl:attribute name="rowspan">
        <xsl:value-of select="$rowspan" />
      </xsl:attribute>
      <xsl:apply-templates mode="generalize" />
    </xsl:copy>
    <th xmlns="http://www.w3.org/1999/xhtml" />
  </xsl:template>

</xsl:stylesheet>