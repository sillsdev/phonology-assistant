<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_to_CV_chart_1a.xsl 2010-03-26 -->
	<!-- Insert lists of feature names. -->
	<!-- Number the articulatory features. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="programConfigurationFolder" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="programArticulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />

	<xsl:variable name="subKeyFormat" select="translate(count($programArticulatoryFeatures/feature), '0123456789', '0000000000')" />

	<xsl:variable name="units" select="/inventory/units" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="inventory">
		<xsl:variable name="view" select="@view" />
		<xsl:variable name="type">
			<xsl:choose>
				<xsl:when test="$view = 'Consonant Chart'">
					<xsl:value-of select="'Consonant'" />
				</xsl:when>
				<xsl:when test="$view = 'Vowel Chart'">
					<xsl:value-of select="'Vowel'" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<colgroupFeatures>
				<xsl:apply-templates select="$programArticulatoryFeatures/feature[@class = 'colgroup'][@type = $type]" />
			</colgroupFeatures>
			<rowgroupFeatures>
				<xsl:apply-templates select="$programArticulatoryFeatures/feature[@class = 'rowgroup'][@type = $type]" />
			</rowgroupFeatures>
		</xsl:copy>
	</xsl:template>

	<!-- Select features which determine the chart cell for a unit. -->
	<!-- Omit the sortKey elements. -->
	<xsl:template match="unit/keys">
		<xsl:variable name="articulatoryFeatures" select="../articulatoryFeatures" />
		<xsl:copy>
			<chartKey class="colgroup">
				<xsl:variable name="colgroupFeature" select="$articulatoryFeatures/feature[@class = 'colgroup'][not(@primary = 'false')]" />
				<xsl:choose>
					<xsl:when test="$colgroupFeature = 'Near-front'">
						<xsl:value-of select="'Front'" />
					</xsl:when>
					<xsl:when test="$colgroupFeature = 'Near-back'">
						<xsl:value-of select="'Back'" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$articulatoryFeatures/feature[@class = 'colgroup'][not(@primary = 'false')]" />
					</xsl:otherwise>
				</xsl:choose>
			</chartKey>
			<chartKey class="rowgroup">
				<xsl:value-of select="$articulatoryFeatures/feature[@class = 'rowgroup']" />
			</chartKey>
			<chartKey class="col">
				<xsl:choose>
					<xsl:when test="$articulatoryFeatures/feature[@class = 'col']">
						<xsl:variable name="colFeature" select="$articulatoryFeatures/feature[@class = 'col']" />
						<xsl:choose>
							<xsl:when test="$colFeature = 'Voiceless' or $colFeature = 'Unrounded'">
								<xsl:value-of select="0" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<!-- Provide a pseudo-feature for the chart cell in the following special cases: -->
					<xsl:when test="$articulatoryFeatures/feature[@subclass = 'stateOfGlottis']">
						<xsl:value-of select="1" />
					</xsl:when>
					<xsl:when test="$articulatoryFeatures/feature[. = 'Click' or . = 'Lateral Click']">
						<xsl:value-of select="0" />
					</xsl:when>
				</xsl:choose>
			</chartKey>
			<chartKey class="row">
				<!-- Make sure that the keys are in the correct order. -->
				<xsl:for-each select="$programArticulatoryFeatures/feature">
					<xsl:variable name="featureName" select="name" />
					<xsl:if test="$articulatoryFeatures/feature[@class = 'row'][. = $featureName] or $articulatoryFeatures/feature[@class = 'colgroup'][@primary = 'false'][. = $featureName]">
						<!-- For a row feature, use the number of preceding siblings as the key. -->
						<xsl:value-of select="format-number(count(preceding-sibling::feature), $subKeyFormat)" />
					</xsl:if>
				</xsl:for-each>
			</chartKey>
		</xsl:copy>
	</xsl:template>

	<!-- Omit the hierarchical structure. -->
	<xsl:template match="unit/root" />

	<!-- If at least one unit has this feature, copy it. -->
	<xsl:template match="inventory/articulatoryFeatures/feature">
		<xsl:variable name="featureName" select="name" />
		<xsl:if test="$units/unit[articulatoryFeatures/feature[. = $featureName]]">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:value-of select="$featureName" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>
	
	<!-- Put Near-front in the Front colgroup and Near-back in the Back colgroup. -->

	<xsl:template match="inventory/articulatoryFeatures/feature[name = 'Front']">
		<xsl:variable name="featureName" select="name" />
		<xsl:if test="$units/unit[articulatoryFeatures/feature[. = $featureName or . = 'Near-front']]">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:value-of select="$featureName" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template match="inventory/articulatoryFeatures/feature[name = 'Near-front']" />

	<xsl:template match="inventory/articulatoryFeatures/feature[name = 'Back']">
		<xsl:variable name="featureName" select="name" />
		<xsl:if test="$units/unit[articulatoryFeatures/feature[. = $featureName or . = 'Near-back']]">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:value-of select="$featureName" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template match="inventory/articulatoryFeatures/feature[name = 'Near-back']" />

	<!-- If at least one unit has this feature, copy it. -->
	<xsl:template match="inventory/binaryFeatures/feature">
		<xsl:variable name="featureName" select="name" />
		<xsl:if test="$units/unit[binaryFeatures/feature[substring(., 2) = $featureName]]">
			<xsl:copy>
				<xsl:apply-templates select="@*|node()" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<!-- If at least one unit has this feature, copy it. -->
	<xsl:template match="inventory/hierarchicalFeatures/feature">
		<xsl:variable name="featureName" select="name" />
		<xsl:choose>
			<xsl:when test="@class = 'terminal'">
				<xsl:if test="$units/unit[hierarchicalFeatures/feature[substring(., 2) = $featureName]]">
					<xsl:copy>
						<xsl:apply-templates select="@*|node()" />
					</xsl:copy>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="feature" select="." />
				<xsl:if test="$units/unit[hierarchicalFeatures/feature[. = $featureName]]">
					<xsl:copy>
						<xsl:apply-templates select="@*|node()" />
					</xsl:copy>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- For an inventory feature (versus a unit feature), copy only its name or fullname. -->
	
	<xsl:template match="inventory/*/feature/*" />
	
	<xsl:template match="inventory/*/feature/name | inventory/*/feature/fullname">
		<xsl:copy-of select="." />
	</xsl:template>

</xsl:stylesheet>