<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_to_CV_chart_1a.xsl 2010-04-20 -->
	<!-- Insert features for column and row groups of a chart. -->
	<!-- Insert chart keys to place units in cells of a chart. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<!-- Assume that Phonology Assistant adds a view attribute to the inventory element. -->
	<xsl:variable name="view" select="/inventory/@view" />
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

	<!-- Assume that Phonology Assistant provides articulatory features from the program phonetic inventory. -->
	<xsl:variable name="programArticulatoryFeatures" select="/inventory/articulatoryFeatures" />
	<xsl:variable name="subKeyFormat" select="translate(count($programArticulatoryFeatures/feature), '0123456789', '0000000000')" />

	<!-- Assume that Phonology Assistant provides units from the phonetic inventory, according to the active filter. -->
	<xsl:variable name="units" select="/inventory/units" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="inventory">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<colgroupFeatures>
				<xsl:apply-templates select="$programArticulatoryFeatures/feature[@class = 'colgroup'][@type = $type]" />
			</colgroupFeatures>
			<rowgroupFeatures>
				<xsl:apply-templates select="$programArticulatoryFeatures/feature[@class = 'rowgroup'][@type = $type]" />
			</rowgroupFeatures>
			<xsl:apply-templates select="units" />
		</xsl:copy>
	</xsl:template>

	<!-- Keep only the units for either Consonant Chart or Vowel Chart view. -->
	<xsl:template match="unit">
		<xsl:if test="articulatoryFeatures/feature[. = $type]">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:apply-templates select="keys | articulatoryFeatures" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>
	
	<!-- Add combined order attributes for row features and all features. -->
	<xsl:template match="unit/keys">
		<xsl:variable name="articulatoryFeatures" select="../articulatoryFeatures" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<!-- For row features, use the position as the key. -->
			<!-- Make sure that the keys are in the correct order. -->
			<chartKeys class="row">
				<xsl:attribute name="order">
					<xsl:for-each select="$programArticulatoryFeatures/feature">
						<xsl:variable name="featureName" select="name" />
						<!-- TO DO: Can colgroup or rowgroup features can have a primary attribute at this step? I doubt it. -->
						<xsl:if test="$articulatoryFeatures/feature[@class = 'row'][. = $featureName] or $articulatoryFeatures/feature[@class = 'colgroup'][@primary = 'false'][. = $featureName]">
							<xsl:value-of select="format-number(position(), $subKeyFormat)" />
						</xsl:if>
					</xsl:for-each>
				</xsl:attribute>
			</chartKeys>
			<!-- The next step uses this key to determine whether any units have identical features. -->
			<!-- Make sure that the keys are in the correct order. -->
			<chartKeys class="all">
				<xsl:attribute name="order">
					<xsl:for-each select="$programArticulatoryFeatures/feature">
						<xsl:variable name="featureName" select="name" />
						<xsl:if test="$articulatoryFeatures/feature[. = $featureName]">
							<xsl:value-of select="format-number(position(), $subKeyFormat)" />
						</xsl:if>
					</xsl:for-each>
				</xsl:attribute>
			</chartKeys>
		</xsl:copy>
	</xsl:template>

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

</xsl:stylesheet>