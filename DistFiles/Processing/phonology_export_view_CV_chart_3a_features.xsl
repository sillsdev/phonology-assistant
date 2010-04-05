<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

	<!-- phonology_export_view_CV_chart_3a_features.xsl 2010-04-05 -->
  <!-- From consonant or vowel chart, make charts of binary features, or hierarchical features, or both. -->
	<!-- For development, highlight differences between project phones and program symbols. -->

	<!-- TO DO: Select phonetic/phonological units. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />

	<!-- The program phonetic character inventory file contains the features, symbols, and so on. -->
	<xsl:variable name="programConfigurationFolder" select="$settings/xhtml:li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'programPhoneticInventoryFile']" />
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="articulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />
	<xsl:variable name="binaryFeatures" select="document($programPhoneticInventoryXML)/inventory/binaryFeatures" />
	<xsl:variable name="hierarchicalFeatures" select="document($programPhoneticInventoryXML)/inventory/hierarchicalFeatures" />
	<xsl:variable name="symbols" select="document($programPhoneticInventoryXML)/inventory/symbols" />

	<!-- A project phonetic inventory file contains features of phonetic or phonological units, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="units" select="document($projectPhoneticInventoryXML)/inventory/units[@type = 'phonetic']" />

	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="interactiveWebPage">
		<xsl:if test="$format = 'XHTML'">
			<xsl:value-of select="$options/xhtml:li[@class = 'interactiveWebPage']" />
		</xsl:if>
	</xsl:variable>
	<!-- TO DO: Also for Word? -->
	<xsl:variable name="featureChartByPlaceOfArticulation">
		<xsl:if test="$format = 'XHTML' and //xhtml:table[@class = 'CV chart'] and not(xhtml:table[@class = 'distribution chart'])">
			<xsl:value-of select="$options/xhtml:li[@class = 'featureChartByPlaceOfArticulation']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="featureChartByMannerOfArticulation">
		<xsl:if test="$format = 'XHTML' and //xhtml:table[@class = 'CV chart'] and not(xhtml:table[@class = 'distribution chart'])">
			<xsl:value-of select="$options/xhtml:li[@class = 'featureChartByMannerOfArticulation']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="featureCharts">
		<xsl:if test="$featureChartByPlaceOfArticulation = 'true' or $featureChartByMannerOfArticulation = 'true'">
			<xsl:value-of select="'true'" />
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />
	<xsl:variable name="differences">
		<xsl:choose>
			<!-- IPA -->
			<xsl:when test="$details/xhtml:li[@class = 'languageName'] = ''">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />
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

	<xsl:variable name="classOfTable" select="'feature chart'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:body">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates />
			<!-- Insert one or more feature charts following the CV chart and any feature tables. -->
			<xsl:if test="$featureCharts = 'true'">
				<xsl:if test="$featureChartByPlaceOfArticulation = 'true'">
					<xsl:call-template name="featureChart">
						<xsl:with-param name="heading">
							<xsl:choose>
								<xsl:when test="$type = 'Consonant'">
									<xsl:value-of select="'Distinctive features of consonants by place of articulation'" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Distinctive features of vowels by backness'" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:with-param>
						<xsl:with-param name="classOfArticulatoryFeatures" select="'colgroup'" />
						<xsl:with-param name="subclassOfArticulatoryFeatures">
							<xsl:choose>
								<xsl:when test="$type = 'Consonant'">
									<xsl:value-of select="'placeOfArticulation'" />
								</xsl:when>
								<xsl:when test="$type = 'Vowel'">
									<xsl:value-of select="'backness'" />
								</xsl:when>
							</xsl:choose>
						</xsl:with-param>
						<xsl:with-param name="classOfSortKey" select="'placeOfArticulation'" />
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$featureChartByMannerOfArticulation = 'true'">
					<xsl:call-template name="featureChart">
						<xsl:with-param name="heading">
							<xsl:choose>
								<xsl:when test="$type = 'Consonant'">
									<xsl:value-of select="'Distinctive features of consonants by manner of articulation'" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Distinctive features of vowels by height'" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:with-param>
						<xsl:with-param name="classOfArticulatoryFeatures" select="'rowgroup'" />
						<xsl:with-param name="subclassOfArticulatoryFeatures">
							<xsl:choose>
								<xsl:when test="$type = 'Consonant'">
									<xsl:value-of select="'mannerOfArticulation'" />
								</xsl:when>
								<xsl:when test="$type = 'Vowel'">
									<xsl:value-of select="'height'" />
								</xsl:when>
							</xsl:choose>
						</xsl:with-param>
						<xsl:with-param name="classOfSortKey" select="'mannerOfArticulation'" />
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<!-- Feature chart, preceded by heading. -->

	<xsl:template name="featureChart">
		<xsl:param name="heading" />
		<xsl:param name="classOfArticulatoryFeatures" />
		<xsl:param name="subclassOfArticulatoryFeatures" />
		<xsl:param name="classOfSortKey" />
		<!-- Enclose in a report division, which is collapsed initially. -->
		<div xmlns="http://www.w3.org/1999/xhtml">
			<xsl:attribute name="class">
				<xsl:value-of select="'report'" />
				<xsl:if test="$interactiveWebPage = 'true'">
					<!-- Division is collapsed initially. -->
					<xsl:value-of select="' collapsed'" />
				</xsl:if>
			</xsl:attribute>
			<h4 xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="$heading" />
			</h4>
			<table class="{$classOfTable}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:call-template name="colgroups">
					<xsl:with-param name="class" select="$classOfArticulatoryFeatures" />
					<xsl:with-param name="subclass" select="$subclassOfArticulatoryFeatures" />
				</xsl:call-template>
				<!-- binary features -->
				<thead>
					<xsl:call-template name="row">
						<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
					</xsl:call-template>
				</thead>
				<xsl:for-each select="$binaryFeatures/feature[@class]">
					<xsl:variable name="class" select="@class" />
					<xsl:if test="not(preceding-sibling::feature[@class = $class])">
						<tbody>
							<xsl:for-each select="$binaryFeatures/feature[@class = $class]">
								<xsl:call-template name="row">
									<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
									<xsl:with-param name="feature" select="." />
								</xsl:call-template>
							</xsl:for-each>
						</tbody>
					</xsl:if>
				</xsl:for-each>
				<!-- hierarchical features -->
				<tbody>
					<xsl:call-template name="row">
						<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
					</xsl:call-template>
				</tbody>
				<tbody>
					<xsl:for-each select="$hierarchicalFeatures/feature[@class = 'terminal'][@parent = 'root']">
						<xsl:call-template name="row">
							<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
							<xsl:with-param name="feature" select="." />
						</xsl:call-template>
					</xsl:for-each>
				</tbody>
				<xsl:for-each select="$hierarchicalFeatures/feature[@class = 'nonTerminal'][@parent = 'root']">
					<tbody>
						<xsl:call-template name="row">
							<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
							<xsl:with-param name="feature" select="." />
						</xsl:call-template>
						<xsl:call-template name="descendantFeatures">
							<xsl:with-param name="name" select="name" />
							<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
						</xsl:call-template>
					</tbody>
				</xsl:for-each>
			</table>
		</div>
	</xsl:template>

	<xsl:template name="descendantFeatures">
		<xsl:param name="name" />
		<xsl:param name="classOfSortKey" />
		<xsl:for-each select="$hierarchicalFeatures/feature[@parent = $name]">
			<xsl:call-template name="row">
				<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
				<xsl:with-param name="feature" select="." />
			</xsl:call-template>
			<xsl:if test="@class = 'nonTerminal'">
				<xsl:call-template name="descendantFeatures">
					<xsl:with-param name="name" select="name" />
					<xsl:with-param name="classOfSortKey" select="$classOfSortKey" />
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	
	<!-- colgroups for-each -->
	<!-- feature[@type = $type][@class = $class] -->
	<!-- type: Consonant/Vowel -->
	<!-- class: colgroup/rowgroup -->
	<!-- if any units have the feature, one col per phone -->
	
	<xsl:template name="colgroups">
		<xsl:param name="class" />
		<xsl:param name="subclass" />
		<colgroup xmlns="http://www.w3.org/1999/xhtml">
			<col />
		</colgroup>
		<xsl:for-each select="$articulatoryFeatures/feature[@class = $class][@subclass = $subclass][@type = $type]">
			<xsl:variable name="name" select="name" />
			<xsl:choose>
				<!-- If Front/Near-Front, or Back/Near-back column groups were merged in the vowel chart, merge them in the feature chart by backness. -->
				<xsl:when test="$name = 'Front' and $units/unit[articulatoryFeatures/feature[. = $name]] and $units/unit[articulatoryFeatures/feature[. = 'Near-front']]">
					<colgroup xmlns="http://www.w3.org/1999/xhtml">
						<xsl:for-each select="$units/unit[articulatoryFeatures/feature[. = $name] or articulatoryFeatures/feature[. = 'Near-front']]">
							<col />
						</xsl:for-each>
					</colgroup>
				</xsl:when>
				<xsl:when test="$name = 'Near-front' and $units/unit[articulatoryFeatures/feature[. = $name]] and $units/unit[articulatoryFeatures/feature[. = 'Front']]" />
				<xsl:when test="$name = 'Back' and $units/unit[articulatoryFeatures/feature[. = $name]] and $units/unit[articulatoryFeatures/feature[. = 'Near-back']]">
					<colgroup xmlns="http://www.w3.org/1999/xhtml">
						<xsl:for-each select="$units/unit[articulatoryFeatures/feature[. = $name] or articulatoryFeatures/feature[. = 'Near-back']]">
							<col />
						</xsl:for-each>
					</colgroup>
				</xsl:when>
				<xsl:when test="$name = 'Near-back' and $units/unit[articulatoryFeatures/feature[. = $name]] and $units/unit[articulatoryFeatures/feature[. = 'Back']]" />
				<xsl:otherwise>
					<xsl:if test="$units/unit[articulatoryFeatures/feature[. = $name]]">
						<colgroup xmlns="http://www.w3.org/1999/xhtml">
							<xsl:for-each select="$units/unit[articulatoryFeatures/feature[. = $name]]">
								<col />
							</xsl:for-each>
						</colgroup>
					</xsl:if>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
	
	<!-- row for-each -->
	<!-- unit[articulatoryFeatures/feature[. = $type] -->
	<!-- sort select="keys/sortKey[@class = $classOfSortKey]" -->

	<xsl:template name="row">
		<xsl:param name="classOfSortKey" />
		<xsl:param name="feature" />
		<tr xmlns="http://www.w3.org/1999/xhtml">
			<th>
				<xsl:if test="$feature">
					<xsl:attribute name="scope">
						<xsl:value-of select="'row'" />
					</xsl:attribute>
					<xsl:if test="$feature/fullname and $feature/fullname != $feature/name">
						<xsl:attribute name="title">
							<xsl:value-of select="$feature/fullname" />
						</xsl:attribute>
					</xsl:if>
					<xsl:value-of select="$feature/name" />
				</xsl:if>
			</th>
			<xsl:for-each select="$units/unit[articulatoryFeatures/feature[. = $type]]">
				<xsl:sort select="keys/sortKey[@class = $classOfSortKey]" />
				<xsl:call-template name="cell">
					<xsl:with-param name="unit" select="." />
					<xsl:with-param name="feature" select="$feature" />
				</xsl:call-template>
			</xsl:for-each>
		</tr>
	</xsl:template>

	<!-- cell -->

	<xsl:template name="cell">
		<xsl:param name="unit" />
		<xsl:param name="feature" />
		<xsl:variable name="literal" select="$unit/@literal" />
		<xsl:choose>
			<xsl:when test="$feature">
				<xsl:variable name="featureName" select="$feature/name" />
				<xsl:variable name="projectValue">
					<xsl:choose>
						<xsl:when test="$feature/@class = 'terminal'">
							<xsl:apply-templates select="$unit/hierarchicalFeatures/feature[substring(., 2) = $featureName]" mode="terminal" />
						</xsl:when>
						<xsl:when test="$feature/@class = 'nonTerminal'">
							<xsl:if test="$unit/hierarchicalFeatures/feature[. = $featureName]">
								<xsl:value-of select="'&#x2022;'" />
							</xsl:if>
						</xsl:when>
						<xsl:when test="$featureName = 'LAB' or $featureName = 'COR' or $featureName = 'DORS'">
							<xsl:if test="substring($unit/binaryFeatures/feature[substring(., 2) = $featureName], 1, 1) = '+'">
								<xsl:value-of select="'&#x2022;'" />
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring($unit/binaryFeatures/feature[substring(., 2) = $featureName], 1, 1)" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<td xmlns="http://www.w3.org/1999/xhtml">
					<xsl:if test="$differences = 'true'">
						<xsl:if test="string-length($literal) = 1">
							<xsl:variable name="symbol" select="$symbols/symbol[@literal = $literal]" />
							<xsl:if test="$symbol">
								<xsl:variable name="programValue" select="substring($symbol/binaryFeatures/feature[substring(., 2) = $featureName], 1, 1)" />
								<xsl:if test="$projectValue != $programValue">
									<xsl:attribute name="class">
										<xsl:value-of select="'different'" />
									</xsl:attribute>
								</xsl:if>
							</xsl:if>
						</xsl:if>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="$projectValue = '-'">
							<xsl:value-of select="'&#x2013;'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$projectValue" />
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="$differences = 'true'">
						<xsl:if test="string-length($literal) = 1">
							<xsl:variable name="symbol" select="$symbols/symbol[@literal = $literal]" />
							<xsl:if test="$symbol">
								<xsl:variable name="programValue" select="substring($symbol/binaryFeatures/feature[substring(., 2) = $featureName], 1, 1)" />
								<xsl:if test="$projectValue != $programValue">
									<xsl:if test="string-length($projectValue) = 0">
										<xsl:value-of select="'&#x2022;'" />
									</xsl:if>
									<span class="different">
										<xsl:value-of select="'/'" />
										<xsl:choose>
											<xsl:when test="string-length($programValue) = 0">
												<xsl:value-of select="'&#x2022;'" />
											</xsl:when>
											<xsl:when test="$programValue = '-'">
												<xsl:value-of select="'&#x2013;'" />
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$programValue" />
											</xsl:otherwise>
										</xsl:choose>
									</span>
								</xsl:if>
							</xsl:if>
						</xsl:if>
					</xsl:if>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="title" select="$unit/description" />
				<th class="Phonetic" scope="col" title="{$title}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="$literal" />
				</th>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Hierarchical terminal features can have both binary values. -->
	<!-- For example, an affricate has [=cont][+cont], a prenasalized consonant has [+nas][-nas]. -->
	<xsl:template match="feature" mode="terminal">
		<xsl:value-of select="substring(., 1, 1)" />
	</xsl:template>

</xsl:stylesheet>