<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2b_base_sequences.xsl 2010-04-09 -->
  <!-- Merge articulatory features of base character sequences. -->
	<!-- General rule: Keep features of the primary base symbol; omit features of non-primary base symbols. -->
	<!-- Specific rules might do any of the following: -->
	<!-- * Retain features (by subclass) from a non-primary base symbol. -->
	<!--   If it has a primary="false" attribute, add a primary="true" attribute to the corresponding feature of its subclass. -->
	<!-- * Remove a feature (by subclass) from the primary base symbol. -->
	<!-- * Add a feature to the primary base symbol. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />
	
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="sequencePatterns" select="document($programPhoneticInventoryXML)/inventory/sequencePatterns" />

	<xsl:variable name="matched" select="'matched'" />
	<xsl:variable name="unmatched" select="'unmatched'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Replace the original sequence of base symbols for a unit. The next step selects one sequence. -->
	<xsl:template match="unit/sequence">
		<xsl:variable name="sequenceOfUnit" select="." />
		<xsl:variable name="countSymbolOfUnit" select="count(symbol)" />
		<xsl:for-each select="$sequencePatterns/pattern[count(symbol) = $countSymbolOfUnit]">
			<!-- A sequence for each pattern with the correct number of symbols. -->
			<sequence title="{@title}">
				<xsl:for-each select="symbol">
					<xsl:variable name="symbolOfPattern" select="." />
					<xsl:variable name="position" select="position()" />
					<xsl:variable name="symbolOfUnit" select="$sequenceOfUnit/symbol[$position]" />
					<!-- The primary base symbol in the pattern must be in the same position as it is in the unit. -->
					<xsl:copy>
						<xsl:apply-templates select="$symbolOfUnit/@literal" />
						<xsl:attribute name="primary">
							<xsl:choose>
								<xsl:when test="@primary = $symbolOfUnit/isBase/@primary">
									<xsl:value-of select="$matched" />
								</xsl:when>
								<!-- A pattern for clicks has only one base symbol, therefore no primary attribute. -->
								<xsl:when test="not(@primary) and not($symbolOfUnit/isBase/@primary)">
									<xsl:value-of select="$matched" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$unmatched" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<!-- Match features according to the if element in the pattern. -->
						<xsl:apply-templates select="if" mode="matchPattern">
							<xsl:with-param name="articulatoryFeaturesOfUnit" select="$symbolOfUnit/articulatoryFeatures" />
						</xsl:apply-templates>
						<!-- Merge features according to add, remove, and retain elements in the pattern. -->
						<articulatoryFeatures>
							<xsl:apply-templates select="$symbolOfUnit/articulatoryFeatures/feature" mode="mergeFeatures">
								<xsl:with-param name="symbolOfPattern" select="$symbolOfPattern" />
							</xsl:apply-templates>
							<xsl:apply-templates select="add/feature" />
						</articulatoryFeatures>
					</xsl:copy>
				</xsl:for-each>
			</sequence>
		</xsl:for-each>
		<!-- The last sequence consists of the base symbol which is not non-primary: -->
		<!-- * For units consisting of only one base symbol. -->
		<!-- * For units consisting of multiple base symbols, in case no sequence patterns match. -->
		<xsl:copy>
			<xsl:apply-templates select="symbol[not(isBase/@primary = 'false')]" />
		</xsl:copy>
	</xsl:template>

	<!-- Match features in symbols of each pattern with corresponding symbols of unit. -->

	<!-- Default copy rule for if and anyOf elements. -->
	<xsl:template match="@* | node()" mode="matchPattern">
		<xsl:param name="articulatoryFeaturesOfUnit" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="matchPattern">
				<xsl:with-param name="articulatoryFeaturesOfUnit" select="$articulatoryFeaturesOfUnit" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<!-- Child of anyOf: keep matched feature; omit unmatched feature. -->
	<xsl:template match="if/anyOf/feature" mode="matchPattern">
		<xsl:param name="articulatoryFeaturesOfUnit" />
		<xsl:variable name="feature" select="." />
		<xsl:if test="$articulatoryFeaturesOfUnit/feature[. = $feature]">
			<xsl:copy>
				<xsl:value-of select="$matched" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<!-- Child of if: indicate whether feature is matched or unmatched. -->
	<xsl:template match="if/feature" mode="matchPattern">
		<xsl:param name="articulatoryFeaturesOfUnit" />
		<xsl:variable name="feature" select="." />
		<xsl:copy>
			<xsl:choose>
				<xsl:when test="$articulatoryFeaturesOfUnit/feature[. = $feature]">
					<xsl:value-of select="$matched" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$unmatched" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<!-- Merge features according to each pattern, in case one matches. -->

	<!-- Copy an articulatory feature of the primary base symbol of the unit, -->
	<!-- unless the corresponding symbol of the pattern removes it (by subclass). -->
	<xsl:template match="symbol[not(isBase/@primary = 'false')]/articulatoryFeatures/feature" mode="mergeFeatures">
		<xsl:param name="symbolOfPattern" />
		<xsl:variable name="pattern" select="$symbolOfPattern/.." />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:if test="not($symbolOfPattern/remove/feature[@subclass = $subclass])">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:if test="$pattern/symbol[@primary = 'false']/retain/feature[@subclass = $subclass][@primary = 'false']">
					<!-- If any non-primary symbol of the pattern retains a feature (with the same subclass) as non-primary, -->
					<!-- add a primary attribute to distinguish this feature. -->
					<xsl:attribute name="primary">
						<xsl:value-of select="'true'" />
					</xsl:attribute>
				</xsl:if>
				<xsl:apply-templates />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<!-- Copy an articulatory feature of a non-primary base symbol of the unit, -->
	<!-- only if the corresponding symbol of the pattern retains it (by subclass). -->
	<!-- Copy any primary="false" attribute in features of the pattern: -->
	<!-- * Secondary place of articulation for consonants (for example, labial-velar). -->
	<!-- * Secondary height, backness, or rounding for vowel dipthongs. -->
	<xsl:template match="symbol[isBase/@primary = 'false']/articulatoryFeatures/feature" mode="mergeFeatures">
		<xsl:param name="symbolOfPattern" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:variable name="featureOfPattern" select="$symbolOfPattern/retain/feature[@subclass = $subclass]" />
		<xsl:if test="$featureOfPattern">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:apply-templates select="$featureOfPattern/@primary" />
				<xsl:apply-templates />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>