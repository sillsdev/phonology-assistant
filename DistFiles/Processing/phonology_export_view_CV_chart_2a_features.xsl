<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_2a_features.xsl 2010-04-21 -->
	<!-- Export to XHTML, Interactive Web page, and at least one feature table. -->
  <!-- For each Phonetic data cell: -->
  <!-- * Wrap the literal unit in a span. -->
	<!-- * Copy lists of features from the project phonetic inventory file. -->
	<!-- Following the CV chart, insert feature tables. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />

	<xsl:variable name="typeOfUnits">
		<xsl:choose>
			<xsl:when test="string-length($details/xhtml:li[@class = 'typeOfUnits']) != 0">
				<xsl:value-of select="$details/xhtml:li[@class = 'typeOfUnits']" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'phonetic'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<!-- A project phonetic inventory file contains features of phonetic or phonological units, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="units" select="document($projectPhoneticInventoryXML)/inventory/units[@type = $typeOfUnits]" />

	<!-- The program phonetic character inventory file contains the features, symbols, and so on. -->
	<xsl:variable name="programConfigurationFolder" select="$settings/xhtml:li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'programPhoneticInventoryFile']" />
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="articulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />
	<xsl:variable name="binaryFeatures" select="document($programPhoneticInventoryXML)/inventory/binaryFeatures" />
	<xsl:variable name="hierarchicalFeatures" select="document($programPhoneticInventoryXML)/inventory/hierarchicalFeatures" />

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

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="xhtml:table[@class = 'CV chart']">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
		<xsl:variable name="br" select="count(xhtml:thead/xhtml:tr/xhtml:th/xhtml:br)" />
		<xsl:if test="$articulatoryFeatureTable = 'true'">
			<table class="articulatory features" xmlns="http://www.w3.org/1999/xhtml">
				<col />
				<thead>
					<tr>
						<th scope="col">
							<xsl:if test="$br != 0">
								<br />
							</xsl:if>
							<span>
								<xsl:value-of select="'Articulatory'" />
							</span>
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="$articulatoryFeatures/feature[@class = 'col-row' or @class = 'row-col']" mode="table" />
				</tbody>
				<tbody>
					<xsl:apply-templates select="$articulatoryFeatures/feature[@class = 'row']" mode="table" />
				</tbody>
			</table>
		</xsl:if>
		<xsl:if test="$binaryFeatureTable = 'true'">
			<table class="binary features" xmlns="http://www.w3.org/1999/xhtml">
				<colgroup>
					<col />
					<col />
				</colgroup>
				<colgroup>
					<col />
				</colgroup>
				<thead>
					<tr>
						<th scope="colgroup" colspan="3">
							<xsl:if test="$br != 0">
								<br />
							</xsl:if>
							<span>
								<xsl:value-of select="'Binary'" />
							</span>
						</th>
					</tr>
				</thead>
				<xsl:for-each select="$binaryFeatures/feature[@class]">
					<xsl:variable name="class" select="@class" />
					<xsl:if test="not(preceding-sibling::feature[@class = $class])">
						<tbody>
							<xsl:apply-templates select="$binaryFeatures/feature[@class = $class]" mode="table" />
						</tbody>
					</xsl:if>
				</xsl:for-each>
			</table>
		</xsl:if>
		<xsl:if test="$hierarchicalFeatureTable = 'true'">
			<!-- After step 2b removes any rows, step 2c inserts colgroup elements and colspan attributes. -->
			<table class="hierarchical features" xmlns="http://www.w3.org/1999/xhtml">
				<thead>
					<tr>
						<th scope="colgroup">
							<xsl:if test="$br != 0">
								<br />
							</xsl:if>
							<span>
								<xsl:value-of select="'Hierarchical'" />
							</span>
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="$hierarchicalFeatures/feature[@parent = 'root']" mode="table" />
				</tbody>
			</table>
		</xsl:if>
	</xsl:template>

	<xsl:template match="xhtml:table[@class = 'CV chart']//xhtml:td[@class = 'Phonetic'][node()]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$features = 'true'">
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
					<xsl:choose>
						<xsl:when test="xhtml:span">
							<xsl:apply-templates />
						</xsl:when>
						<xsl:otherwise>
							<span xmlns="http://www.w3.org/1999/xhtml">
								<xsl:value-of select="$literal" />
							</span>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="$unit">
						<xsl:if test="$articulatoryFeatureTable = 'true'">
							<xsl:apply-templates select="$unit/articulatoryFeatures" mode="list" />
						</xsl:if>
						<xsl:if test="$binaryFeatureTable = 'true'">
							<xsl:apply-templates select="$unit/binaryFeatures" mode="list" />
						</xsl:if>
						<xsl:if test="$hierarchicalFeatureTable = 'true'">
							<xsl:apply-templates select="$unit/hierarchicalFeatures" mode="list" />
						</xsl:if>
					</xsl:if>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="articulatoryFeatures" mode="list">
		<ul class="articulatory features" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="feature" mode="list" />
		</ul>
	</xsl:template>

	<xsl:template match="binaryFeatures" mode="list">
		<ul class="binary features" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="feature" mode="list" />
		</ul>
	</xsl:template>

	<xsl:template match="hierarchicalFeatures" mode="list">
		<ul class="hierarchical features" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="feature" mode="list" />
		</ul>
	</xsl:template>

	<xsl:template match="feature" mode="list">
		<xsl:if test=". != '-LAB' and . != '-COR' and . != '-DORS' and . != '-PHAR'">
			<li xmlns="http://www.w3.org/1999/xhtml">
				<xsl:choose>
					<xsl:when test=". = '+LAB' or . = '+COR' or . = '+DORS' or . = '+PHAR'">
						<xsl:value-of select="substring(., 2)" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="." />
					</xsl:otherwise>
				</xsl:choose>
			</li>
		</xsl:if>
	</xsl:template>

	<xsl:template match="articulatoryFeatures/feature" mode="table">
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td class="name">
        <xsl:value-of select="name" />
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="binaryFeatures/feature" mode="table">
		<tr class="bivalent" xmlns="http://www.w3.org/1999/xhtml">
			<td class="plus">
				<xsl:value-of select="'+'" />
			</td>
			<!-- en-dash -->
			<td class="minus">
				<xsl:value-of select="'&#x2013;'" />
			</td>
			<td class="name">
				<xsl:if test="fullname and fullname != name">
					<xsl:attribute name="title">
						<xsl:value-of select="fullname" />
					</xsl:attribute>
				</xsl:if>
				<xsl:value-of select="name" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="binaryFeatures/feature[name = 'LAB' or name = 'COR' or name = 'DORS' or name = 'PHAR']" mode="table">
		<tr class="univalent" xmlns="http://www.w3.org/1999/xhtml">
			<td colspan="2" />
			<td class="name">
				<xsl:if test="fullname and fullname != name">
					<xsl:attribute name="title">
						<xsl:value-of select="fullname" />
					</xsl:attribute>
				</xsl:if>
				<xsl:value-of select="name" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="hierarchicalFeatures/feature[@class = 'terminal']" mode="table">
		<xsl:param name="depth" select="0" />
		<tr class="bivalent" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:if test="$depth &gt; 0">
				<td colspan="{2 * $depth}" />
			</xsl:if>
			<td class="plus">
				<xsl:value-of select="'+'" />
			</td>
			<!-- en-dash -->
			<td class="minus">
				<xsl:value-of select="'&#x2013;'" />
			</td>
			<td class="name">
				<xsl:if test="fullname and fullname != name">
					<xsl:attribute name="title">
						<xsl:value-of select="fullname" />
					</xsl:attribute>
				</xsl:if>
				<xsl:value-of select="name" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="hierarchicalFeatures/feature[@class = 'nonTerminal']" mode="table">
		<xsl:param name="depth" select="0" />
		<xsl:variable name="name" select="name" />
		<tr class="univalent" xmlns="http://www.w3.org/1999/xhtml">
			<td colspan="{2 * ($depth + 1)}" />
			<td class="name">
				<xsl:if test="fullname and fullname != name">
					<xsl:attribute name="title">
						<xsl:value-of select="fullname" />
					</xsl:attribute>
				</xsl:if>
				<xsl:value-of select="name" />
			</td>
		</tr>
		<xsl:apply-templates select="$hierarchicalFeatures/feature[@parent = $name]" mode="table">
			<xsl:with-param name="depth" select="$depth + 1" />
		</xsl:apply-templates>
	</xsl:template>

</xsl:stylesheet>