<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_1a.xsl 2010-04-03 -->
  <!-- Temporarily convert the table of phones to a list of phones. -->
  <!-- From the project phonetic inventory file, for each phone: -->
  <!-- * Get the lists of articulatory and binary features. -->
  <!-- * Get the colgroup, rowgroup, and col features, and the row sort key. -->
  <!-- From the PhoneticCharacterInventory.xml file for the program: -->
  <!-- * Add lists of colgroup and rowgroup articulatory features for chart headings. -->
  <!-- * Add auxilliary tables of non-heading articulatory features and all binary features. -->

	<!-- TO DO: Select phonetic/phonological units. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />

	<!-- A project phonetic inventory file contains features of phonetic or phonological units, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="units" select="document($projectPhoneticInventoryXML)/inventory/units" />

	<!-- The program phonetic character inventory file contains the features, symbols, and so on. -->
	<xsl:variable name="programConfigurationFolder" select="$settings/xhtml:li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'programPhoneticInventoryFile']" />
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="articulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />
	<xsl:variable name="binaryFeatures" select="document($programPhoneticInventoryXML)/inventory/binaryFeatures" />
	<xsl:variable name="hierarchicalFeatures" select="document($programPhoneticInventoryXML)/inventory/hierarchicalFeatures" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="interactiveWebPage">
		<xsl:if test="$format = 'XHTML'">
			<xsl:value-of select="$options/xhtml:li[@class = 'interactiveWebPage']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="articulatoryFeatureTable">
		<xsl:if test="$interactiveWebPage = 'true'">
			<xsl:value-of select="$options/xhtml:li[@class = 'articulatoryFeatureTable']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="binaryFeatureTable">
		<xsl:if test="$interactiveWebPage = 'true'">
			<xsl:value-of select="$options/xhtml:li[@class = 'binaryFeatureTable']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="hierarchicalFeatureTable">
		<xsl:if test="$interactiveWebPage = 'true'">
			<xsl:value-of select="$options/xhtml:li[@class = 'hierarchicalFeatureTable']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="features">
		<xsl:if test="$articulatoryFeatureTable = 'true' or $binaryFeatureTable = 'true' or $hierarchicalFeatureTable = 'true'">
			<xsl:value-of select="'true'" />
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />
	
	<xsl:variable name="rowKeyFormat" select="translate(count($articulatoryFeatures/feature), '0123456789', '0000000000')" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:table[@class = 'CV chart']">
    <xsl:variable name="type">
      <xsl:choose>
        <xsl:when test="$view = 'Consonant Chart'">
          <xsl:value-of select="'Consonant'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'Vowel'" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <ul xmlns="http://www.w3.org/1999/xhtml">
      <xsl:apply-templates select="@*" />
      <xsl:for-each select="xhtml:tbody/xhtml:tr/xhtml:td[@class = 'Phonetic'][text()]">
        <xsl:variable name="text" select="." />
        <xsl:apply-templates select="$units/unit[@literal = $text]" />
      </xsl:for-each>
    </ul>
    <ul class="colgroup features" xmlns="http://www.w3.org/1999/xhtml">
      <xsl:apply-templates select="$articulatoryFeatures/feature[@class = 'colgroup'][@type = $type]" mode="colgroup" />
    </ul>
    <ul class="rowgroup features" xmlns="http://www.w3.org/1999/xhtml">
      <xsl:apply-templates select="$articulatoryFeatures/feature[@class = 'rowgroup'][@type = $type]" mode="rowgroup" />
    </ul>
		<xsl:if test="$articulatoryFeatureTable = 'true'">
			<table class="articulatory features" xmlns="http://www.w3.org/1999/xhtml">
				<col />
				<thead>
					<tr>
						<th scope="col">
							<xsl:value-of select="'Articulatory'" />
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="$articulatoryFeatures/feature[@class = 'col-row' or @class = 'row-col']" mode="auxilliary" />
				</tbody>
				<tbody>
					<xsl:apply-templates select="$articulatoryFeatures/feature[@class = 'row']" mode="auxilliary" />
				</tbody>
			</table>
		</xsl:if>
		<xsl:if test="$binaryFeatureTable = 'true'">
			<table class="binary features" xmlns="http://www.w3.org/1999/xhtml">
				<colgroup>
					<col />
					<col />
					<col />
				</colgroup>
				<thead>
					<tr>
						<th scope="colgroup" colspan="3">
							<xsl:value-of select="'binary'" />
						</th>
					</tr>
				</thead>
				<xsl:for-each select="$binaryFeatures/feature[@class]">
					<xsl:variable name="class" select="@class" />
					<xsl:if test="not(preceding-sibling::feature[@class = $class])">
						<tbody>
							<xsl:apply-templates select="$binaryFeatures/feature[@class = $class]" mode="auxilliary" />
						</tbody>
					</xsl:if>
				</xsl:for-each>
			</table>
		</xsl:if>
		<xsl:if test="$hierarchicalFeatureTable = 'true'">
			<table class="hierarchical features" xmlns="http://www.w3.org/1999/xhtml">
				<colgroup>
					<col />
					<col />
					<col />
				</colgroup>
				<thead>
					<tr>
						<th scope="colgroup" colspan="3">
							<xsl:value-of select="'hierarchical'" />
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="$hierarchicalFeatures/feature[@parent = 'root']" mode="auxilliary" />
				</tbody>
			</table>
		</xsl:if>
	</xsl:template>

  <xsl:template match="feature" mode="colgroup">
    <li xmlns="http://www.w3.org/1999/xhtml">
      <xsl:value-of select="name" />
    </li>
  </xsl:template>

  <xsl:template match="feature" mode="rowgroup">
    <li xmlns="http://www.w3.org/1999/xhtml">
      <span>
        <xsl:value-of select="name" />
      </span>
    </li>
  </xsl:template>

  <xsl:template match="inventory/articulatoryFeatures/feature" mode="auxilliary">
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td class="name">
        <xsl:value-of select="name" />
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="inventory/binaryFeatures/feature | inventory/hierarchicalFeatures/feature[@class = 'terminal']" mode="auxilliary">
    <tr xmlns="http://www.w3.org/1999/xhtml">
			<td class="plus">
				<xsl:value-of select="'+'" />
			</td>
			<td class="minus">
				<xsl:value-of select="'-'" />
			</td>
			<td class="name">
        <xsl:if test="fullname and fullnam != name">
          <xsl:attribute name="title">
            <xsl:value-of select="fullname" />
          </xsl:attribute>
        </xsl:if>
        <xsl:value-of select="name" />
      </td>
    </tr>
  </xsl:template>

	<xsl:template match="inventory/hierarchicalFeatures/feature[@class = 'nonTerminal']" mode="auxilliary">
		<tr xmlns="http://www.w3.org/1999/xhtml">
			<td class="univalent" colspan="3">
				<div class="name">
					<xsl:if test="fullname and fullname != name">
						<xsl:attribute name="title">
							<xsl:value-of select="fullname" />
						</xsl:attribute>
					</xsl:if>
					<xsl:value-of select="name" />
				</div>
				<table>
					<tbody>
						<xsl:variable name="name" select="name" />
						<xsl:apply-templates select="$hierarchicalFeatures/feature[@parent = $name]" mode="auxilliary" />
					</tbody>
				</table>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="unit">
    <li title="{description}" xmlns="http://www.w3.org/1999/xhtml">
      <span>
        <xsl:value-of select="@literal" />
      </span>
			<xsl:if test="$features = 'true'">
				<div class="features">
					<xsl:if test="$articulatoryFeatureTable = 'true'">
						<ul class="articulatory">
							<xsl:apply-templates select="articulatoryFeatures/feature" />
						</ul>
					</xsl:if>
					<xsl:if test="$binaryFeatureTable = 'true'">
						<ul class="binary">
							<xsl:apply-templates select="binaryFeatures/feature" />
						</ul>
					</xsl:if>
					<xsl:if test="$hierarchicalFeatureTable = 'true'">
						<ul class="hierarchical">
							<xsl:apply-templates select="hierarchicalFeatures/feature" />
						</ul>
					</xsl:if>
				</div>
			</xsl:if>
      <ul class="chart features">
        <li class="colgroup">
					<xsl:variable name="colgroupFeature" select="articulatoryFeatures/feature[@class = 'colgroup'][not(@primary = 'false')]" />
					<xsl:choose>
						<xsl:when test="$colgroupFeature = 'Near-front'">
							<xsl:value-of select="'Front'" />
						</xsl:when>
						<xsl:when test="$colgroupFeature = 'Near-back'">
							<xsl:value-of select="'Back'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="articulatoryFeatures/feature[@class = 'colgroup'][not(@primary = 'false')]" />
						</xsl:otherwise>
					</xsl:choose>
        </li>
        <li class="rowgroup">
          <xsl:value-of select="articulatoryFeatures/feature[@class = 'rowgroup']" />
        </li>
				<xsl:if test="articulatoryFeatures/feature[@class = 'colgroup'][@primary = 'false']">
					<xsl:call-template name="row">
						<xsl:with-param name="feature" select="articulatoryFeatures/feature[@class = 'colgroup'][@primary = 'false']" />
					</xsl:call-template>
				</xsl:if>
        <xsl:for-each select="articulatoryFeatures/feature[@class = 'row']">
					<xsl:call-template name="row">
						<xsl:with-param name="feature" select="." />
					</xsl:call-template>
        </xsl:for-each>
        <li class="col">
					<xsl:choose>
						<xsl:when test="articulatoryFeatures/feature[@class = 'col']">
							<xsl:value-of select="articulatoryFeatures/feature[@class = 'col']" />
						</xsl:when>
						<!-- Provide a pseudo-feature for the chart cell in the following special cases: -->
						<xsl:when test="articulatoryFeatures/feature[@subclass = 'stateOfGlottis']">
							<xsl:value-of select="'Voiced'" />
						</xsl:when>
						<xsl:when test="articulatoryFeatures/feature[. = 'Click' or . = 'Lateral Click']">
							<xsl:value-of select="'Voiceless'" />
						</xsl:when>
					</xsl:choose>
        </li>
      </ul>
    </li>    
  </xsl:template>

	<xsl:template match="feature">
    <li xmlns="http://www.w3.org/1999/xhtml">
      <xsl:value-of select="." />
    </li>
  </xsl:template>

	<xsl:template name="row">
		<xsl:param name="feature" />
		<li class="row" title="{$feature}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="$articulatoryFeatures/feature[name = $feature]" mode="row" />
		</li>
	</xsl:template>

	<xsl:template match="feature" mode="row">
		<xsl:value-of select="format-number(count(preceding-sibling::feature) + 1, $rowKeyFormat)" />
	</xsl:template>

</xsl:stylesheet>