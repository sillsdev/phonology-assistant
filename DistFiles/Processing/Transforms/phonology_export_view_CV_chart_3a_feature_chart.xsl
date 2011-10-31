<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

	<!-- phonology_export_view_CV_chart_3a_feature_chart.xsl 2011-10-28 -->
  <!-- Following the section which contains a consonant chart or vowel chart, or both, -->
	<!-- append sections which contain corresponding charts of distinctive features. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />

	<xsl:variable name="languageCode3" select="$details/xhtml:li[@class = 'languageCode']" />
	<xsl:variable name="languageCode1">
		<xsl:if test="string-length($languageCode3) != 0">
			<xsl:value-of select="document('ISO_639.xml')//xhtml:tr[xhtml:td[@class = 'ISO_639-3'] = $languageCode3]/xhtml:td[@class = 'ISO_639-1']" />
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

	<!-- A project phonetic inventory file contains features of phonetic or phonological segments, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="segments" select="document($projectPhoneticInventoryXML)/inventory/segments" />

	<!-- The program phonetic character inventory file contains the features, symbols, and so on. -->
	<xsl:variable name="programConfigurationFolder" select="$settings/xhtml:li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'programPhoneticInventoryFile']" />
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="programDescriptiveFeatures" select="document($programPhoneticInventoryXML)/inventory/featureDefinitions[@class = 'descriptive']" />

	<xsl:variable name="programDistinctiveFeaturesFile" select="concat($settings/xhtml:li[@class = 'programDistinctiveFeaturesName'], '.DistinctiveFeatures.xml')" />
	<xsl:variable name="programDistinctiveFeaturesXML" select="concat($programConfigurationFolder, $programDistinctiveFeaturesFile)" />
	<xsl:variable name="programDistinctiveFeatures" select="document($programDistinctiveFeaturesXML)/inventory/featureDefinitions[@class = 'distinctive']" />

	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="interactiveWebPage">
		<xsl:if test="$format = 'XHTML'">
			<xsl:value-of select="$options/xhtml:li[@class = 'interactiveWebPage']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="distinctiveFeatureTable" select="$options/xhtml:li[@class = 'distinctiveFeatureTable']" />
	<xsl:variable name="featureChartByPlaceOrBackness">
		<xsl:if test="$format = 'XHTML' and //xhtml:table[starts-with(@class, 'CV chart')] and not(xhtml:table[@class = 'distribution chart'])">
			<xsl:if test="$distinctiveFeatureTable = 'true'">
				<xsl:value-of select="$options/xhtml:li[@class = 'featureChartByPlaceOrBackness']" />
			</xsl:if>
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="featureChartByMannerOrHeight">
		<xsl:if test="$format = 'XHTML' and //xhtml:table[starts-with(@class, 'CV chart')] and not(xhtml:table[@class = 'distribution chart'])">
			<xsl:if test="$distinctiveFeatureTable = 'true'">
				<xsl:value-of select="$options/xhtml:li[@class = 'featureChartByMannerOrHeight']" />
			</xsl:if>
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="featureCharts">
		<xsl:if test="$featureChartByPlaceOrBackness = 'true' or $featureChartByMannerOrHeight = 'true'">
			<xsl:value-of select="'true'" />
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="classPrefix" select="'distinctive-segment values chart'" />
	<xsl:variable name="distinctiveFeaturesHeadingPrefix" select="'Distinctive features of '" />

	<xsl:variable name="plusMinusSign" select="'&#x00B1;'" />
	<xsl:variable name="middleDot" select="'&#x00B7;'" />
	<xsl:variable name="enDash" select="'&#x2013;'" />
	<xsl:variable name="bullet" select="'&#x2022;'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:div[xhtml:h3][.//xhtml:table[starts-with(@class, 'CV chart')]]">
		<xsl:variable name="ancestor" select="." />
		<xsl:copy>
			<xsl:if test="not(@class) and $featureCharts = 'true'">
				<xsl:attribute name="class">
					<xsl:value-of select="'section'" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
		<xsl:if test="$featureCharts = 'true'">
			<xsl:for-each select=".//xhtml:table[starts-with(@class, 'CV chart')]">
				<xsl:variable name="type" select="substring-after(@class, 'CV chart ')" />
				<xsl:variable name="tableCV" select="." />
				<xsl:variable name="tableDistinctiveFeatures" select="../xhtml:table[starts-with(@class, 'distinctive features')]" />
				<xsl:choose>
					<!-- If a table of distinctive features corresponds to this CV chart, -->
					<!-- the feature charts use its redundant features. -->
					<xsl:when test="$tableDistinctiveFeatures">
						<xsl:call-template name="featureCharts">
							<xsl:with-param name="type" select="$type" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
						</xsl:call-template>
					</xsl:when>
					<!-- If a table of distinctive features is shared by multiple CV charts, -->
					<!-- the feature charts can use its redundant features, -->
					<!-- but must also determine redundant features for the (subset of) segments in this CV chart. -->
					<xsl:otherwise>
						<xsl:call-template name="featureCharts">
							<xsl:with-param name="type" select="$type" />
							<xsl:with-param name="tableCV" select="$tableCV" />
							<xsl:with-param name="positionKeyFormat" select="translate(count($tableCV//xhtml:ul[@class = 'distinctive features']), '0123456789', '0000000000')" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$ancestor/xhtml:div/xhtml:table[starts-with(@class, 'distinctive features')]" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>

	<!-- Insert one or more feature charts following the CV chart and any feature tables. -->
	<xsl:template name="featureCharts">
		<xsl:param name="type" />
		<xsl:param name="tableCV" />
		<xsl:param name="positionKeyFormat" />
		<xsl:param name="tableDistinctiveFeatures" />
		<xsl:if test="$featureChartByPlaceOrBackness = 'true'">
			<xsl:call-template name="featureChart">
				<xsl:with-param name="type" select="$type" />
				<xsl:with-param name="tableCV" select="$tableCV" />
				<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
				<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
				<xsl:with-param name="heading">
					<xsl:choose>
						<xsl:when test="$type = 'consonant'">
							<xsl:value-of select="concat($distinctiveFeaturesHeadingPrefix, 'consonants by place of articulation')" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat($distinctiveFeaturesHeadingPrefix, 'vowels by backness')" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="classDescriptive" select="'colgroup'" />
				<xsl:with-param name="categoryDescriptive">
					<xsl:choose>
						<xsl:when test="$type = 'consonant'">
							<xsl:value-of select="'place'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'backness'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="classOfSortKey" select="'place_or_backness'" />
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$featureChartByMannerOrHeight = 'true'">
			<xsl:call-template name="featureChart">
				<xsl:with-param name="type" select="$type" />
				<xsl:with-param name="tableCV" select="$tableCV" />
				<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
				<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
				<xsl:with-param name="heading">
					<xsl:choose>
						<xsl:when test="$type = 'consonant'">
							<xsl:value-of select="concat($distinctiveFeaturesHeadingPrefix, 'consonants by manner of articulation')" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat($distinctiveFeaturesHeadingPrefix, 'vowels by height')" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="classDescriptive" select="'rowgroup'" />
				<xsl:with-param name="categoryDescriptive">
					<xsl:choose>
						<xsl:when test="$type = 'consonant'">
							<xsl:value-of select="'manner'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'height'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="classOfSortKey" select="'manner_or_height'" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="featureChart">
		<xsl:param name="type" />
		<xsl:param name="tableCV" />
		<xsl:param name="positionKeyFormat" />
		<xsl:param name="tableDistinctiveFeatures" />
		<xsl:param name="heading" />
		<xsl:param name="classDescriptive" />
		<xsl:param name="categoryDescriptive" />
		<xsl:param name="classOfSortKey" />
		<!-- Enclose in a section division, which is collapsed initially. -->
		<div xmlns="http://www.w3.org/1999/xhtml">
			<xsl:attribute name="class">
				<xsl:value-of select="'section'" />
				<xsl:if test="$interactiveWebPage = 'true'">
					<!-- Division is collapsed initially. -->
					<xsl:value-of select="' collapsed'" />
				</xsl:if>
			</xsl:attribute>
			<h3 xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="$heading" />
			</h3>
			<table class="{concat($classPrefix, ' ', $type, ' ', $categoryDescriptive)}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:call-template name="colgroupsSegments">
					<xsl:with-param name="classChartKey" select="$classDescriptive" />
					<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
				</xsl:call-template>
				<thead>
					<xsl:call-template name="row">
						<xsl:with-param name="type" select="$type" />
						<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
					</xsl:call-template>
				</thead>
				<xsl:for-each select="$programDistinctiveFeatures/featureDefinition[@category][not(@type != $type)]">
					<xsl:variable name="category" select="@category" />
					<xsl:if test="not(preceding-sibling::featureDefinition[@category = $category])">
						<tbody>
							<xsl:for-each select="$programDistinctiveFeatures/featureDefinition[@category = $category][not(@type != $type)]">
								<xsl:call-template name="row">
									<xsl:with-param name="type" select="$type" />
									<xsl:with-param name="tableCV" select="$tableCV" />
									<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
									<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
									<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
									<xsl:with-param name="feature" select="." />
								</xsl:call-template>
							</xsl:for-each>
						</tbody>
					</xsl:if>
				</xsl:for-each>
			</table>
			<xsl:choose>
				<xsl:when test="$categoryDescriptive = 'place'">
					<table class="{concat('distinctive-descriptive values chart', ' ', $type, ' ', $categoryDescriptive)}" xmlns="http://www.w3.org/1999/xhtml">
						<xsl:call-template name="colgroupsFeatures">
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
						</xsl:call-template>
						<xsl:call-template name="theadFeatures">
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
						</xsl:call-template>
						<xsl:call-template name="tbodyFeatures">
							<xsl:with-param name="categoryDistinctive" select="'labial'" />
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
							<xsl:with-param name="tableCV" select="$tableCV" />
							<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
						</xsl:call-template>
						<xsl:call-template name="tbodyFeatures">
							<xsl:with-param name="categoryDistinctive" select="'coronal'" />
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
							<xsl:with-param name="tableCV" select="$tableCV" />
							<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
						</xsl:call-template>
						<xsl:call-template name="tbodyFeatures">
							<xsl:with-param name="categoryDistinctive" select="'dorsal'" />
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
							<xsl:with-param name="tableCV" select="$tableCV" />
							<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
						</xsl:call-template>
						<xsl:call-template name="tbodyFeatures">
							<xsl:with-param name="categoryDistinctive" select="'radical'" />
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
							<xsl:with-param name="tableCV" select="$tableCV" />
							<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
						</xsl:call-template>
					</table>
				</xsl:when>
				<xsl:when test="$categoryDescriptive = 'manner'">
					<table class="{concat('distinctive-descriptive values chart', ' ', $type, ' ', $categoryDescriptive)}" xmlns="http://www.w3.org/1999/xhtml">
						<xsl:call-template name="colgroupsFeatures">
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
						</xsl:call-template>
						<xsl:call-template name="theadFeatures">
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
						</xsl:call-template>
						<xsl:call-template name="tbodyFeatures">
							<xsl:with-param name="categoryDistinctive" select="'major'" />
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
							<xsl:with-param name="tableCV" select="$tableCV" />
							<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
						</xsl:call-template>
						<xsl:call-template name="tbodyFeatures">
							<xsl:with-param name="categoryDistinctive" select="'manner'" />
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
							<xsl:with-param name="tableCV" select="$tableCV" />
							<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
						</xsl:call-template>
						<xsl:call-template name="tbodyFeatures">
							<xsl:with-param name="nameDistinctive" select="'implosive'" />
							<xsl:with-param name="classChartKey" select="$classDescriptive" />
							<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
							<xsl:with-param name="tableCV" select="$tableCV" />
							<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
							<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
						</xsl:call-template>
					</table>
				</xsl:when>
			</xsl:choose>
		</div>
	</xsl:template>
	
	<!-- colgroups -->
	<!-- classChartKey: colgroup or rowgroup -->
	<!-- categoryDescriptive: place/backness or manner/height -->

	<!-- if any segments have the feature, one col per segment -->
	<xsl:template name="colgroupsSegments">
		<xsl:param name="classChartKey" />
		<xsl:param name="categoryDescriptive" />
		<colgroup xmlns="http://www.w3.org/1999/xhtml">
			<col />
		</colgroup>
		<xsl:for-each select="$programDescriptiveFeatures/featureDefinition[@category = $categoryDescriptive]">
			<xsl:variable name="name" select="name" />
			<xsl:if test="$segments/segment[keys/chartKey[@class = $classChartKey][. = $name]]">
				<colgroup xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="$segments/segment[not(@literalInChart)][keys/chartKey[@class = $classChartKey][. = $name]]">
						<col />
					</xsl:for-each>
				</colgroup>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<!-- if any segments have the feature, one col per feature -->
	<xsl:template name="colgroupsFeatures">
		<xsl:param name="classChartKey" />
		<xsl:param name="categoryDescriptive" />
		<colgroup xmlns="http://www.w3.org/1999/xhtml">
			<col />
		</colgroup>
		<xsl:if test="$segments/segment[keys/chartKey[@class = $classChartKey]]">
			<colgroup xmlns="http://www.w3.org/1999/xhtml">
				<xsl:for-each select="$programDescriptiveFeatures/featureDefinition[@category = $categoryDescriptive]">
					<xsl:variable name="name" select="name" />
					<xsl:if test="$segments/segment[keys/chartKey[@class = $classChartKey][. = $name]]">
						<col />
					</xsl:if>
				</xsl:for-each>
			</colgroup>
		</xsl:if>
	</xsl:template>

	<!-- if any segments have the feature, one col per feature -->
	<xsl:template name="theadFeatures">
		<xsl:param name="classChartKey" />
		<xsl:param name="categoryDescriptive" />
		<thead xmlns="http://www.w3.org/1999/xhtml">
			<tr>
				<th />
				<xsl:for-each select="$programDescriptiveFeatures/featureDefinition[@category = $categoryDescriptive]">
					<xsl:variable name="name" select="name" />
					<xsl:if test="$segments/segment[keys/chartKey[@class = $classChartKey][. = $name]]">
						<th class="rotate" scope="col">
							<div>
								<span>
									<xsl:value-of select="$name" />
								</span>
							</div>
						</th>
					</xsl:if>
				</xsl:for-each>
			</tr>
		</thead>
	</xsl:template>

	<xsl:template name="tbodyFeatures">
		<xsl:param name="categoryDistinctive" />
		<xsl:param name="nameDistinctive" />
		<xsl:param name="classChartKey" />
		<xsl:param name="categoryDescriptive" />
		<xsl:param name="tableCV" />
		<xsl:param name="positionKeyFormat" />
		<xsl:param name="tableDistinctiveFeatures" />
		<tbody xmlns="http://www.w3.org/1999/xhtml">
			<xsl:choose>
				<xsl:when test="$categoryDistinctive">
					<xsl:for-each select="$programDistinctiveFeatures/featureDefinition[@category = $categoryDistinctive]">
					<xsl:call-template name="trFeature">
						<xsl:with-param name="featureDefinition" select="." />
						<xsl:with-param name="classChartKey" select="$classChartKey" />
						<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
						<xsl:with-param name="tableCV" select="$tableCV" />
						<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
						<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
					</xsl:call-template>
					</xsl:for-each>
				</xsl:when>
				<xsl:when test="$nameDistinctive">
					<xsl:call-template name="trFeature">
						<xsl:with-param name="featureDefinition" select="$programDistinctiveFeatures/featureDefinition[name = $nameDistinctive]" />
						<xsl:with-param name="classChartKey" select="$classChartKey" />
						<xsl:with-param name="categoryDescriptive" select="$categoryDescriptive" />
						<xsl:with-param name="tableCV" select="$tableCV" />
						<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
						<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
					</xsl:call-template>
				</xsl:when>
			</xsl:choose>
		</tbody>
	</xsl:template>

	<xsl:template name="trFeature">
		<xsl:param name="featureDefinition" />
		<xsl:param name="classChartKey" />
		<xsl:param name="categoryDescriptive" />
		<xsl:param name="tableCV" />
		<xsl:param name="positionKeyFormat" />
		<xsl:param name="tableDistinctiveFeatures" />
		<xsl:variable name="nameDistinctive" select="$featureDefinition/name" />
		<xsl:variable name="fullname" select="$featureDefinition/fullname" />
		<xsl:variable name="featurePlus" select="concat('+', $nameDistinctive)" />
		<xsl:variable name="featureMinus" select="concat('-', $nameDistinctive)" />
		<tr xmlns="http://www.w3.org/1999/xhtml">
			<xsl:call-template name="trClassTitleRedundant">
				<xsl:with-param name="tableCV" select="$tableCV" />
				<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
				<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
				<xsl:with-param name="featureName" select="$nameDistinctive" />
			</xsl:call-template>
			<th scope="row">
				<xsl:if test="$fullname and $fullname != nameDistinctive">
					<xsl:attribute name="title">
						<xsl:value-of select="$fullname" />
					</xsl:attribute>
				</xsl:if>
				<xsl:value-of select="$nameDistinctive" />
			</th>
			<xsl:for-each select="$programDescriptiveFeatures/featureDefinition[@category = $categoryDescriptive]">
				<xsl:variable name="nameDescriptive" select="name" />
				<xsl:variable name="valuesDistinctive">
					<xsl:for-each select="$segments/segment[keys/chartKey[@class = $classChartKey][. = $nameDescriptive]]">
						<xsl:variable name="featuresDistinctive" select="features[@class = 'distinctive']" />
						<xsl:if test="$featuresDistinctive/feature[. = $featurePlus]">
							<xsl:value-of select="'+'" />
						</xsl:if>
						<xsl:if test="$featuresDistinctive/feature[. = $featureMinus]">
							<xsl:value-of select="'-'" />
						</xsl:if>
						<xsl:if test="not($featuresDistinctive/feature[. = $featurePlus or . = $featureMinus])">
							<xsl:value-of select="'0'" />
						</xsl:if>
					</xsl:for-each>
				</xsl:variable>
				<xsl:if test="string-length($valuesDistinctive) != 0">
					<td>
						<xsl:if test="contains($valuesDistinctive, '+') or contains($valuesDistinctive, '-')">
							<xsl:if test="contains($valuesDistinctive, '0')">
								<xsl:value-of select="$middleDot" />
							</xsl:if>
							<xsl:choose>
								<xsl:when test="contains($valuesDistinctive, '+')">
									<xsl:choose>
										<xsl:when test="contains($valuesDistinctive, '-')">
											<xsl:value-of select="$plusMinusSign" />
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'+'" />
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$enDash" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
					</td>
				</xsl:if>
			</xsl:for-each>
		</tr>
	</xsl:template>

	<!-- The following logic is similar to step 2b, but for the (subset of) segments in the CV chart. -->
	<!-- * If there is a table of distinctive features and it does not contain this feature, -->
	<!--   then this feature is redundant in this chart too. -->
	<!-- * If the table contains this feature indicated as redundant, it is redundant here too; -->
	<!--   however another feature in this chart might be redundant with respect to it here (which was not there). -->
	<xsl:template name="trClassTitleRedundant">
		<xsl:param name="tableCV" />
		<xsl:param name="positionKeyFormat" />
		<xsl:param name="tableDistinctiveFeatures" />
		<xsl:param name="featureName" />
		<xsl:variable name="featurePlus" select="concat('+', $featureName)" />
		<!-- Important: Although cells for minus values might contain en-dash, list items have minus. -->
		<xsl:variable name="featureMinus" select="concat('-', $featureName)" />
		<xsl:variable name="positionKey">
			<xsl:if test="(not($tableDistinctiveFeatures) or $tableDistinctiveFeatures/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'name'] = $featureName]) and $tableCV and (($tableCV//xhtml:ul[@class = 'distinctive features'][xhtml:li[. = $featurePlus]] and $tableCV//xhtml:ul[@class = 'distinctive features'][not(xhtml:li[. = $featurePlus])]) or ($tableCV//xhtml:ul[@class = 'distinctive features'][xhtml:li[. = $featureMinus]] and $tableCV//xhtml:ul[@class = 'distinctive features'][not(xhtml:li[. = $featureMinus])]))">
				<xsl:for-each select="$tableCV//xhtml:ul[@class = 'distinctive features']">
					<xsl:if test="xhtml:li[. = $featurePlus]">
						<xsl:value-of select="format-number(position(), $positionKeyFormat)" />
					</xsl:if>
				</xsl:for-each>
				<xsl:value-of select="' '" />
				<xsl:for-each select="$tableCV//xhtml:ul[@class = 'distinctive features']">
					<xsl:if test="xhtml:li[. = $featureMinus]">
						<xsl:value-of select="format-number(position(), $positionKeyFormat)" />
					</xsl:if>
				</xsl:for-each>
			</xsl:if>
		</xsl:variable>
		<xsl:if test="$tableDistinctiveFeatures/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'name'] = $featureName][contains(@class, 'redundant')] or ($tableDistinctiveFeatures and not($tableDistinctiveFeatures/xhtml:tbody/xhtml:tr[xhtml:td[@class = 'name'] = $featureName])) or ($tableCV and string-length($positionKey) = 0)">
			<xsl:attribute name="class">
				<xsl:value-of select="'redundant'" />
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="string-length($positionKey) != 0">
			<xsl:attribute name="title">
				<xsl:value-of select="$positionKey" />
			</xsl:attribute>
		</xsl:if>
	</xsl:template>

	<!-- row for-each -->
	<!-- segment[features[@class = 'descriptive']/feature[. = $type] -->
	<!-- sort select="keys/sortKey[@class = $classOfSortKey]" -->

	<xsl:template name="row">
		<xsl:param name="type" />
		<xsl:param name="tableCV" />
		<xsl:param name="positionKeyFormat" />
		<xsl:param name="tableDistinctiveFeatures" />
		<xsl:param name="feature" />
		<xsl:param name="classOfSortKey" />
		<tr xmlns="http://www.w3.org/1999/xhtml">
			<xsl:if test="$feature">
				<xsl:variable name="featureName" select="$feature/name" />
				<xsl:call-template name="trClassTitleRedundant">
					<xsl:with-param name="tableCV" select="$tableCV" />
					<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
					<xsl:with-param name="tableDistinctiveFeatures" select="$tableDistinctiveFeatures" />
					<xsl:with-param name="featureName" select="$featureName" />
				</xsl:call-template>
			</xsl:if>
			<th>
				<xsl:choose>
					<xsl:when test="$feature">
						<xsl:attribute name="scope">
							<xsl:value-of select="'row'" />
						</xsl:attribute>
						<xsl:if test="$feature/fullname and $feature/fullname != $feature/name">
							<xsl:attribute name="title">
								<xsl:value-of select="$feature/fullname" />
							</xsl:attribute>
						</xsl:if>
						<xsl:value-of select="$feature/name" />
					</xsl:when>
				</xsl:choose>
			</th>
			<xsl:for-each select="$segments/segment[not(@literalInChart)][features[@class = 'descriptive']/feature[. = $type]]">
				<xsl:sort select="keys/sortKey[@class = $classOfSortKey]" />
				<xsl:call-template name="cell">
					<xsl:with-param name="segment" select="." />
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="type" select="$type" />
				</xsl:call-template>
			</xsl:for-each>
		</tr>
	</xsl:template>

	<!-- cell -->

	<xsl:template name="cell">
		<xsl:param name="segment" />
		<xsl:param name="feature" />
		<xsl:param name="type" />
		<xsl:variable name="literal" select="$segment/@literal" />
		<xsl:choose>
			<xsl:when test="$feature">
				<xsl:variable name="featureName" select="$feature/name" />
				<td xmlns="http://www.w3.org/1999/xhtml">
					<xsl:choose>
						<!-- Distinctive bivalent features can have both binary values. -->
						<!-- For example, an affricate has [=cont][+cont], a prenasalized consonant has [+nas][-nas]. -->
						<xsl:when test="$feature/@class = 'bivalent'">
							<xsl:apply-templates select="$segment/features[@class = 'distinctive']/feature[substring(., 2) = $featureName]" mode="bivalent" />
						</xsl:when>
						<xsl:when test="$feature/@class = 'univalent'">
							<xsl:if test="$segment/features[@class = 'distinctive']/feature[. = $featureName]">
								<xsl:value-of select="$bullet" />
							</xsl:if>
						</xsl:when>
					</xsl:choose>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="indistinguishableThisFromThat">
					<xsl:call-template name="booleanOr">
						<xsl:with-param name="booleanSequence">
							<xsl:for-each select="$segment/preceding-sibling::segment[not(@literalInChart)][features[@class = 'descriptive']/feature[. = $type]]">
								<xsl:call-template name="indistinguishableThisFromThat">
									<xsl:with-param name="segmentThis" select="$segment" />
									<xsl:with-param name="segmentThat" select="." />
								</xsl:call-template>
							</xsl:for-each>
							<xsl:for-each select="$segment/following-sibling::segment[not(@literalInChart)][features[@class = 'descriptive']/feature[. = $type]]">
								<xsl:call-template name="indistinguishableThisFromThat">
									<xsl:with-param name="segmentThis" select="$segment" />
									<xsl:with-param name="segmentThat" select="." />
								</xsl:call-template>
							</xsl:for-each>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="indistinguishableThatFromThis">
					<xsl:call-template name="booleanOr">
						<xsl:with-param name="booleanSequence">
							<xsl:for-each select="$segment/preceding-sibling::segment[not(@literalInChart)][features[@class = 'descriptive']/feature[. = $type]]">
								<xsl:call-template name="indistinguishableThisFromThat">
									<xsl:with-param name="segmentThis" select="." />
									<xsl:with-param name="segmentThat" select="$segment" />
								</xsl:call-template>
							</xsl:for-each>
							<xsl:for-each select="$segment/following-sibling::segment[not(@literalInChart)][features[@class = 'descriptive']/feature[. = $type]]">
								<xsl:call-template name="indistinguishableThisFromThat">
									<xsl:with-param name="segmentThis" select="." />
									<xsl:with-param name="segmentThat" select="$segment" />
								</xsl:call-template>
							</xsl:for-each>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="class">
					<xsl:value-of select="'Phonetic'" />
					<xsl:if test="$indistinguishableThisFromThat = 'true' or $indistinguishableThatFromThis = 'true'">
						<xsl:value-of select="' indistinguishable'" />
						<xsl:if test="$indistinguishableThisFromThat = 'true'">
							<xsl:value-of select="' this_from_that'" />
						</xsl:if>
						<xsl:if test="$indistinguishableThatFromThis = 'true'">
							<xsl:value-of select="' that_from_this'" />
						</xsl:if>
					</xsl:if>
				</xsl:variable>
				<xsl:variable name="title" select="$segment/description" />
				<th class="{$class}" scope="col" title="{$title}" xmlns="http://www.w3.org/1999/xhtml">
					<span lang="{$langPhonetic}">
						<xsl:value-of select="$literal" />
					</span>
				</th>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="feature" mode="bivalent">
		<xsl:value-of select="substring(., 1, 1)" />
	</xsl:template>

	<!-- If segmentThat has every feature of segmentThis, -->
	<!-- then you cannot distinguish segmentThis from segmentThat. -->
	<xsl:template name="indistinguishableThisFromThat">
		<xsl:param name="segmentThis" />
		<xsl:param name="segmentThat" />
		<xsl:call-template name="booleanAnd">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates select="$segmentThis/features[@class='distinctive']/feature" mode="booleanFeatures">
					<xsl:with-param name="features" select="$segmentThat/features[@class='distinctive']" />
				</xsl:apply-templates>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

</xsl:stylesheet>