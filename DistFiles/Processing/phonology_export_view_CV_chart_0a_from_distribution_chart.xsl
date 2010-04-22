<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_0a_from_distribution_chart.xsl 2010-04-21 -->
  <!-- Following a distribution chart with generalized items, -->
	<!-- insert a consonant or vowel chart with environment tabs. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />

	<!-- The option must be selected and all other criteria. -->
	<xsl:variable name="withCVchart" select="$options/xhtml:li[@class = 'withCVchart']" />

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
	<xsl:variable name="programArticulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />

	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="interactiveWebPage">
		<xsl:if test="$format = 'XHTML'">
			<xsl:value-of select="$options/xhtml:li[@class = 'interactiveWebPage']" />
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="subKeyFormat" select="translate(count($programArticulatoryFeatures/feature), '0123456789', '0000000000')" />

	<xsl:variable name="title" select="/xhtml:html/xhtml:head/xhtml:title" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="xhtml:table[@class = 'distribution chart']">
		<!-- Copy the distribution chart. -->
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
		<xsl:if test="$interactiveWebPage = 'true'">
			<xsl:if test="contains($title, '[giC]') or contains($title, '[giV]')">
				<xsl:variable name="type">
					<xsl:choose>
						<xsl:when test="contains($title, '[giC]')">
							<xsl:value-of select="'Consonant'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'Vowel'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<!-- Tabs of the CV chart correspond to non-individual environments of the distribution chart. -->
				<xsl:variable name="headingRow" select="xhtml:thead/xhtml:tr[not(@class = 'individual')]" />
				<ul class="environments" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="$headingRow/xhtml:th[@class = 'Phonetic']">
						<li class="Phonetic">
							<xsl:value-of select="." />
						</li>
					</xsl:for-each>
				</ul>
				<!-- List of individual items from the distribution chart which are units in the project inventory. -->
				<ul class="CV chart" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="xhtml:tbody/xhtml:tr[@class = 'individual']">
						<xsl:variable name="text" select="xhtml:th[@class = 'Phonetic']" />
						<xsl:apply-templates select="$units/unit[@literal = $text]">
							<xsl:with-param name="headingRow" select="$headingRow" />
							<xsl:with-param name="individualRow" select="." />
						</xsl:apply-templates>
					</xsl:for-each>
				</ul>
				<ul class="colgroup features" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="$programArticulatoryFeatures/feature[@class = 'colgroup'][@type = $type]">
						<li>
							<xsl:value-of select="name" />
						</li>
					</xsl:for-each>
				</ul>
				<ul class="rowgroup features" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="$programArticulatoryFeatures/feature[@class = 'rowgroup'][@type = $type]">
						<li xmlns="http://www.w3.org/1999/xhtml">
							<!-- Wrap the feature name in a span because the next step will add a list of rows. -->
							<span>
								<xsl:value-of select="name" />
							</span>
						</li>
					</xsl:for-each>
				</ul>
			</xsl:if>
		</xsl:if>
  </xsl:template>

  <xsl:template match="unit">
		<xsl:param name="headingRow" />
		<xsl:param name="individualRow" />
		<xsl:variable name="unitArticulatoryFeatures" select="articulatoryFeatures" />
    <li xmlns="http://www.w3.org/1999/xhtml">
      <span>
        <xsl:value-of select="@literal" />
      </span>
			<ul class="environments">
				<xsl:for-each select="$headingRow/xhtml:th[@class = 'Phonetic']">
					<xsl:variable name="position" select="position()" />
					<xsl:variable name="data">
						<xsl:choose>
							<xsl:when test="$headingRow/@class = 'general'">
								<xsl:variable name="positionOfGeneralColumn" select="count($headingRow/following-sibling::xhtml:tr/xhtml:th[not(@class)][$position]/preceding-sibling::xhtml:th) + 1" />
								<xsl:value-of select="$individualRow/xhtml:td[$positionOfGeneralColumn]" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$individualRow/xhtml:td[$position]" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:if test="string-length($data) != 0 and $data != '0'">
						<li>
							<xsl:value-of select="." />
						</li>
					</xsl:if>
				</xsl:for-each>
			</ul>
			<!-- Copy keys from project phonetic inventory, and then add two. -->
			<ul class="chart features">
				<xsl:for-each select="keys/*[@class != 'row']">
					<li class="{@class}">
						<xsl:apply-templates />
					</li>
				</xsl:for-each>
				<li class="row">
					<xsl:for-each select="$programArticulatoryFeatures/feature">
						<xsl:variable name="featureName" select="name" />
						<!-- TO DO: Can colgroup or rowgroup features can have a primary attribute at this step? I doubt it. -->
						<xsl:if test="$unitArticulatoryFeatures/feature[@class = 'row'][. = $featureName] or articulatoryFeatures/feature[@class = 'colgroup'][@primary = 'false'][. = $featureName]">
							<xsl:value-of select="format-number(position(), $subKeyFormat)" />
						</xsl:if>
					</xsl:for-each>
				</li>
				<li class="all">
					<xsl:for-each select="$programArticulatoryFeatures/feature">
						<xsl:variable name="featureName" select="name" />
						<xsl:if test="$unitArticulatoryFeatures/feature[. = $featureName]">
							<xsl:value-of select="format-number(position(), $subKeyFormat)" />
						</xsl:if>
					</xsl:for-each>
				</li>
			</ul>
		</li>    
  </xsl:template>

	<!-- Remove any generalized CV chart options from the title. -->
	<xsl:template match="/xhtml:html/xhtml:head/xhtml:title">
		<xsl:variable name="title">
			<xsl:choose>
				<xsl:when test="contains(., ' [giC]')">
					<xsl:value-of select="substring-before(., ' [giC]')" />
				</xsl:when>
				<xsl:when test="contains(., '[giC]')">
					<xsl:value-of select="substring-before(., '[giC]')" />
				</xsl:when>
				<xsl:when test="contains(., ' [giV]')">
					<xsl:value-of select="substring-before(., ' [giV]')" />
				</xsl:when>
				<xsl:when test="contains(., '[giV]')">
					<xsl:value-of select="substring-before(., '[giV]')" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="." />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="$title" />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>