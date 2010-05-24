<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2d_feature_rules.xsl 2010-05-24 -->
  <!-- Remove binary and hierarchical feature rules that do not apply to the project phonetic inventory. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="programArticulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />
	<xsl:variable name="programBinaryFeatures" select="document($programPhoneticInventoryXML)/inventory/binaryFeatures" />
	<xsl:variable name="programHierarchicalFeatures" select="document($programPhoneticInventoryXML)/inventory/hierarchicalFeatures" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@* | node()" mode="units">
		<xsl:param name="units" />
    <xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="units">
				<xsl:with-param name="units" select="$units" />
			</xsl:apply-templates>
    </xsl:copy>
  </xsl:template>

	<!-- Following units, insert binary and hierarchical features from the program inventory. -->
  <xsl:template match="units">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
		<xsl:apply-templates select="$programBinaryFeatures" mode="units">
			<xsl:with-param name="units" select="." />
		</xsl:apply-templates>
		<xsl:apply-templates select="$programHierarchicalFeatures" mode="units">
			<xsl:with-param name="units" select="." />
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="units/unit/articulatoryFeatures/feature">
		<xsl:variable name="feature" select="." />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates select="$programArticulatoryFeatures/feature[name = $feature]/rule">
				<xsl:with-param name="unit" select="../.." />
				<xsl:with-param name="feature" select="." />
			</xsl:apply-templates>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="inventory/articulatoryFeatures/feature/rule">
		<xsl:param name="unit" />
		<xsl:param name="feature" />
		<xsl:variable name="booleanFor">
			<xsl:call-template name="booleanAnd">
				<xsl:with-param name="booleanSequence">
					<xsl:apply-templates select="for/*" mode="boolean">
						<xsl:with-param name="feature" select="$feature" />
					</xsl:apply-templates>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$booleanFor = 'true'">
			<xsl:variable name="booleanIf">
				<xsl:call-template name="booleanAnd">
					<xsl:with-param name="booleanSequence">
						<xsl:apply-templates select="if/*" mode="boolean">
							<xsl:with-param name="unit" select="$unit" />
						</xsl:apply-templates>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$booleanIf = 'true'">
					<xsl:apply-templates select="then/*" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="else/*" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template match="attribute[@marked]">
		<xsl:attribute name="marked">
			<xsl:value-of select="@marked" />
		</xsl:attribute>
	</xsl:template>

	<!-- Does a when condition match any units in the project phonetic inventory? -->
	<xsl:template match="feature/rule[when]" mode="units">
		<xsl:param name="units" />
		<xsl:variable name="for" select="for" />
		<xsl:variable name="when" select="when" />
		<xsl:variable name="booleanWhen">
			<xsl:call-template name="booleanOr">
				<xsl:with-param name="booleanSequence">
					<xsl:for-each select="$units/unit">
						<xsl:variable name="unit" select="." />
						<xsl:variable name="booleanFor">
							<xsl:call-template name="booleanAnd">
								<xsl:with-param name="booleanSequence">
									<xsl:apply-templates select="$for/*" mode="boolean">
										<xsl:with-param name="unit" select="$unit" />
									</xsl:apply-templates>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:variable>
						<xsl:if test="$booleanFor = 'true'">
							<xsl:call-template name="booleanAnd">
								<xsl:with-param name="booleanSequence">
									<xsl:apply-templates select="$when/*" mode="boolean">
										<xsl:with-param name="unit" select="$unit" />
									</xsl:apply-templates>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
					</xsl:for-each>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:copy>
			<xsl:if test="$booleanWhen = 'true'">
				<xsl:apply-templates select="@* | node()" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<!-- Does a generalized for-when condition match any units in the project phonetic inventory? -->
	<xsl:template match="feature/rule[for/articulatoryFeature/@subclass][when]" mode="units">
		<xsl:param name="units" />
		<xsl:variable name="rule" select="." />
		<xsl:variable name="subclass" select="for/articulatoryFeature/@subclass" />
		<xsl:variable name="when" select="when" />
		<xsl:for-each select="$programArticulatoryFeatures/feature[@subclass = $subclass]">
			<xsl:variable name="featureName" select="name" />
			<xsl:variable name="booleanWhen">
				<xsl:call-template name="booleanOr">
					<xsl:with-param name="booleanSequence">
						<xsl:for-each select="$units/unit[articulatoryFeatures/feature[. = $featureName]]">
							<xsl:call-template name="booleanAnd">
								<xsl:with-param name="booleanSequence">
									<xsl:apply-templates select="$when/*" mode="boolean">
										<xsl:with-param name="unit" select="." />
									</xsl:apply-templates>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:variable>
			<rule>
				<xsl:if test="$booleanWhen = 'true'">
					<xsl:apply-templates select="$rule/@*" />
					<for>
						<articulatoryFeature>
							<xsl:value-of select="$featureName" />
						</articulatoryFeature>
					</for>
					<xsl:apply-templates select="$rule/*[not(self::for)]" />
				</xsl:if>
			</rule>
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>