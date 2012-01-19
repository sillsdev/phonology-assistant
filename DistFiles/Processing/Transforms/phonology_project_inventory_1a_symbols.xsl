<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_1a_symbols.xsl 2011-10-28 -->
	<!-- For each segment, copy information about its symbols from the program phonetic inventory. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="symbolDefinitions" select="document($programPhoneticInventoryXML)/inventory/symbolDefinitions" />

	<!-- Assume that the project inventory has descriptive feature definitions with class, category, type, and order attributes. -->
	<xsl:variable name="projectDescriptiveFeatures" select="/inventory/featureDefinitions[@class = 'descriptive']" />

	<!-- Assume that the project inventory has distinctive feature definitions with order attribute. -->
	<xsl:variable name="projectDistinctiveFeatures" select="/inventory/featureDefinitions[@class = 'distinctive']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="segments">
		<xsl:variable name="segments" select="." />
		<xsl:variable name="symbolsIgnored" select="../symbols[@class = 'ignoredInChart']" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates select="segment">
				<xsl:with-param name="segments" select="$segments" />
				<xsl:with-param name="symbolsIgnored" select="$symbolsIgnored" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

  <xsl:template match="segment[@literal]">
		<xsl:param name="segments" />
		<xsl:param name="symbolsIgnored" />
		<xsl:variable name="literal" select="@literal" />
		<xsl:variable name="literalInChart">
			<xsl:choose>
				<xsl:when test="$symbolsIgnored/@literals">
					<xsl:value-of select="translate($literal, $symbolsIgnored/@literals, '')" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$literal" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<!-- If a segment contains symbols to ignore: -->
		<!-- * If there is no segment without the symbols, add the symbol. -->
		<!--   The next step will remove added segments which are duplicates. -->
		<!-- * Add an attribute which refers to the segment without the symbols. -->
		<!--   The charts will omit segments which have the attribute. -->
		<xsl:if test="$literalInChart != $literal">
			<xsl:if test="not($segments/segment[@literal = $literalInChart])">
				<segment literal="{$literalInChart}" addedForChart="true">
					<xsl:apply-templates>
						<xsl:with-param name="symbolsIgnored" select="$symbolsIgnored" />
						<xsl:with-param name="literal" select="$literal" />
					</xsl:apply-templates>
					<xsl:call-template name="symbols">
						<xsl:with-param name="literal" select="$literalInChart" />
						<xsl:with-param name="segments" select="$segments" />
					</xsl:call-template>
				</segment>
			</xsl:if>
		</xsl:if>
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="$literalInChart != $literal">
				<xsl:attribute name="literalInChart">
					<xsl:value-of select="$literalInChart" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
			<xsl:call-template name="symbols">
				<xsl:with-param name="literal" select="$literal" />
				<xsl:with-param name="segments" select="$segments" />
			</xsl:call-template>
		</xsl:copy>
  </xsl:template>

	<!-- Segment has descriptive feature overrides. -->
	<xsl:template match="features[@class = 'descriptive']">
		<xsl:param name="symbolsIgnored" />
		<xsl:param name="literal" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates select="feature" mode="descriptive">
				<xsl:with-param name="symbolsIgnored" select="$symbolsIgnored" />
				<xsl:with-param name="literal" select="$literal" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<!-- Segment has distinctive feature overrides. -->
	<xsl:template match="features[@class = 'distinctive']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="feature">
				<xsl:variable name="name" select="substring(., 2)" />
				<xsl:variable name="featureDefinition" select="$projectDistinctiveFeatures/featureDefinition[name = $name]" />
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:attribute name="order">
						<xsl:value-of select="$featureDefinition/@order" />
					</xsl:attribute>
					<xsl:apply-templates />
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="symbols">
		<xsl:param name="literal" />
		<xsl:param name="segments" />
		<symbols>
			<xsl:call-template name="literal">
				<xsl:with-param name="text" select="$literal" />
				<xsl:with-param name="segments" select="$segments" />
			</xsl:call-template>
		</symbols>
	</xsl:template>

	<xsl:template name="literal">
    <xsl:param name="text" />
		<xsl:param name="segments" />
    <xsl:if test="string-length($text) != 0">
      <xsl:variable name="char" select="substring($text, 1, 1)" />
      <xsl:apply-templates select="$symbolDefinitions/symbolDefinition[@literal = $char]" mode="symbolDefinition">
				<xsl:with-param name="segments" select="$segments" />
			</xsl:apply-templates>
      <xsl:call-template name="literal">
        <xsl:with-param name="text" select="substring($text, 2)" />
				<xsl:with-param name="segments" select="$segments" />
			</xsl:call-template>
    </xsl:if>
  </xsl:template>

  <!-- Copy features of symbol. -->
	<!-- For base symbols, apply any feature rules. -->
	<!-- For diacritic symbols, copy any feature rules. -->
	<xsl:template match="symbolDefinition" mode="symbolDefinition">
		<xsl:param name="segments" />
		<xsl:choose>
			<xsl:when test="usage/@replaceWith">
				<xsl:call-template name="literal">
					<xsl:with-param name="text" select="usage/@replaceWith" />
					<xsl:with-param name="segments" select="$segments" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="literal" select="@literal" />
				<xsl:variable name="articulatoryFeaturesChanged" select="$segments/segment[@literal = $literal]/articulatoryFeatures[@changed = 'true']" />
				<xsl:variable name="base" select="isBase" />
				<symbol literal="{@literal}" code="{@code}" base="{$base}">
					<xsl:choose>
						<xsl:when test="$articulatoryFeaturesChanged">
							<xsl:copy-of select="$articulatoryFeaturesChanged" />
						</xsl:when>
						<xsl:when test="$base = 'true'">
							<features class="descriptive">
								<xsl:apply-templates select="features[@class = 'descriptive']/feature" mode="feature" />
								<xsl:apply-templates select="featureRule[@class = 'descriptive'][@featureOption]/add[feature]" mode="add" />
							</features>
						</xsl:when>
						<xsl:otherwise>
							<features class="descriptive">
								<xsl:apply-templates select="features[@class = 'descriptive']/feature" mode="feature" />
							</features>
							<xsl:apply-templates select="featureRule[@class = 'descriptive']" />
						</xsl:otherwise>
					</xsl:choose>
				</symbol>
			</xsl:otherwise>
		</xsl:choose>
  </xsl:template>

	<!-- If an option is selected, change a feature of the symbol. -->
	<!-- For example, change front to central. -->
	<xsl:template match="feature" mode="feature">
		<xsl:variable name="feature" select="." />
		<xsl:variable name="change" select="../../featureRule[@class = 'descriptive'][@featureOption]/change[find[feature[. = $feature]]]" />
		<xsl:choose>
			<xsl:when test="$change">
				<xsl:variable name="boolean">
					<xsl:call-template name="featureOption">
						<xsl:with-param name="class" select="'descriptive'" />
						<xsl:with-param name="name" select="$change/../@featureOption" />
					</xsl:call-template>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$boolean = 'true'">
						<xsl:apply-templates select="$change/replace/feature" mode="descriptive" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="." mode="descriptive" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="." mode="descriptive" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- If an option is selected, add a feature to the symbol. -->
	<!-- For example, advanced tongue root. -->
	<xsl:template match="add" mode="add">
		<xsl:variable name="boolean">
			<xsl:call-template name="featureOption">
				<xsl:with-param name="class" select="'descriptive'" />
				<xsl:with-param name="name" select="../@featureOption" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$boolean = 'true'">
			<xsl:apply-templates select="feature" mode="descriptive" />
		</xsl:if>
	</xsl:template>

	<!-- Copy attributes from featureDefinition to feature in rule for diacritic symbol. -->
	<xsl:template match="featureRule//feature">
		<xsl:apply-templates select="." mode="descriptive" /> 
	</xsl:template>
	
	<!-- Copy attributes from featureDefinition to feature. -->
	<xsl:template match="feature" mode="descriptive">
		<xsl:param name="symbolsIgnored" />
		<xsl:param name="literal" />
		<xsl:variable name="name" select="." />
		<xsl:choose>
			<xsl:when test="$symbolsIgnored and $symbolsIgnored/symbol[features[@class = 'descriptive']/feature[. = $name]][contains($literal, @literal)]" />
			<xsl:otherwise>
				<xsl:variable name="featureDefinition" select="$projectDescriptiveFeatures/featureDefinition[name = $name]" />
				<xsl:copy>
					<!-- Copy attributes from features definition: class, category, order. -->
					<xsl:copy-of select="$featureDefinition/@class" />
					<xsl:copy-of select="$featureDefinition/@category" />
					<xsl:copy-of select="$featureDefinition/@order" />
					<!-- Copy primary attribute from feature in symbol definition. -->
					<xsl:copy-of select="@primary" />
					<xsl:apply-templates />
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>