<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_1d_base_sequences.xsl 2011-09-21 -->
  <!-- Merge descriptive features of base symbol sequences. -->
	<!-- General rule: Keep features of the primary base symbol; omit features of non-primary base symbols. -->
	<!-- Specific rules might do any of the following: -->
	<!-- * Retain features (by category) from a non-primary base symbol. -->
	<!--   If it has a primary="false" attribute, add a primary="true" attribute to the corresponding feature of its category. -->
	<!--   If any non-primary symbol of the sequencePattern is secondary (that is, in a diphthong), -->
	<!--   add primary and position attributes to distinguish this feature. -->
	<!-- * Remove a feature (by category) from the primary base symbol. -->
	<!-- * Add a feature to the primary base symbol. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />
	
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="sequencePatterns" select="document($programPhoneticInventoryXML)/inventory/sequencePatterns" />

	<!-- Assume that the project inventory has descriptive feature definitions with class, category, and order attributes. -->
	<xsl:variable name="projectDescriptiveFeatures" select="/inventory/featureDefinitions[@class = 'descriptive']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="@* | node()" mode="long">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Merge descriptive features of symbols according to the first sequencePattern that matches the base symbols. -->
	<xsl:template match="symbols">
		<xsl:variable name="countBaseSymbolOfSegment" select="count(symbol[@base = 'true'])" />
		<xsl:choose>
			<xsl:when test="$countBaseSymbolOfSegment = 1">
				<xsl:copy-of select="." />
			</xsl:when>
			<xsl:when test="$countBaseSymbolOfSegment = 2 and symbol[@base = 'true'][1]/@literal = symbol[@base = 'true'][2]/@literal">
				<xsl:copy>
					<xsl:for-each select="symbol">
						<xsl:copy>
							<xsl:apply-templates select="@*" mode="long" />
							<xsl:if test="@base = 'true' and not(preceding-sibling::symbol[@base = 'true'])">
								<xsl:apply-templates mode="long" />
							</xsl:if>
						</xsl:copy>
					</xsl:for-each>
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="sequencePatterns">
					<xsl:with-param name="sequencePattern" select="$sequencePatterns/sequencePattern[count(symbol) = $countBaseSymbolOfSegment][1]" />
					<xsl:with-param name="symbolsOfSegment" select="." />
					<xsl:with-param name="countBaseSymbolOfSegment" select="$countBaseSymbolOfSegment" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Add long to first base symbol of an identical pair. -->
	<xsl:template match="symbol[@base = 'true'][not(preceding-sibling::symbol[@base = 'true'])]/features[@class = 'descriptive']" mode="long">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:call-template name="feature">
				<xsl:with-param name="name" select="'long'" />
			</xsl:call-template>
		</xsl:copy>
	</xsl:template>

	<!-- Replace primary='true' with primary='false' in second base symbol of an identical pair. -->
	<xsl:template match="symbol[@base = 'true'][preceding-sibling::symbol[@base = 'true']]/@primary" mode="long">
		<xsl:attribute name="primary">
			<xsl:value-of select="'false'" />
		</xsl:attribute>
	</xsl:template>

	<xsl:template name="sequencePatterns">
		<xsl:param name="sequencePattern" />
		<xsl:param name="symbolsOfSegment" />
		<xsl:param name="countBaseSymbolOfSegment" />
		<xsl:choose>
			<xsl:when test="$sequencePattern">
				<xsl:variable name="boolean">
					<xsl:call-template name="booleanAnd">
						<xsl:with-param name="booleanSequence">
							<xsl:for-each select="$symbolsOfSegment/symbol[@base = 'true']">
								<xsl:variable name="symbolOfSegment" select="." />
								<xsl:variable name="primaryOfSegment" select="$symbolOfSegment/@primary" />
								<xsl:variable name="position" select="position()" />
								<xsl:variable name="symbolOfPattern" select="$sequencePattern/symbol[$position]" />
								<xsl:choose>
									<xsl:when test="$primaryOfSegment = 'true' and $primaryOfSegment != $symbolOfPattern/@primary">
										<xsl:value-of select="'false'" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="booleanOr">
											<xsl:with-param name="booleanSequence">
												<xsl:apply-templates select="$symbolOfPattern/ifAnyOf/*" mode="booleanFeatures">
													<xsl:with-param name="features" select="$symbolOfSegment/features[@class = 'descriptive']" />
												</xsl:apply-templates>
											</xsl:with-param>
										</xsl:call-template>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:variable>
				<xsl:choose>
					<!-- Apply the matched sequencePattern to the base symbols. -->
					<!-- Merge features according to add, replace, and retain elements in the sequencePattern. -->
					<xsl:when test="$boolean = 'true'">
						<xsl:copy>
							<xsl:for-each select="$symbolsOfSegment/symbol">
								<xsl:copy>
									<xsl:apply-templates select="@*" />
									<xsl:if test="@base = 'true' and @primary = 'true'">
										<features class="descriptive">
											<xsl:for-each select="$symbolsOfSegment/symbol[@base = 'true']">
												<xsl:variable name="position" select="position()" />
												<xsl:variable name="symbolOfPattern" select="$sequencePattern/symbol[$position]" />
												<xsl:apply-templates select="." mode="changes">
													<xsl:with-param name="sequencePattern" select="$sequencePattern" />
													<xsl:with-param name="position" select="$position" />
												</xsl:apply-templates>
											</xsl:for-each>
											<xsl:apply-templates select="$sequencePattern//changes/add/feature" mode="featureDefinition" />
										</features>
									</xsl:if>
								</xsl:copy>
							</xsl:for-each>
						</xsl:copy>
					</xsl:when>
					<!-- Recursive call for the next sequencePattern with the correct number of symbols, if any. -->
					<xsl:otherwise>
						<xsl:call-template name="sequencePatterns">
							<xsl:with-param name="sequencePattern" select="$sequencePattern/following-sibling::sequencePattern[count(symbol) = $countBaseSymbolOfSegment][1]" />
							<xsl:with-param name="symbolsOfSegment" select="$symbolsOfSegment" />
							<xsl:with-param name="countBaseSymbolOfSegment" select="$countBaseSymbolOfSegment" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<!-- Because no patterns matched, apply the default sequencePattern to the symbols. -->
			<!-- The keep the features of the base symbol which is not non-primary -->
			<!-- for segments consisting of multiple base symbols, in case no sequence patterns match. -->
			<xsl:otherwise>
				<xsl:copy>
					<xsl:for-each select="symbol">
						<xsl:copy>
							<xsl:copy-of select="@*" />
							<xsl:if test="@base = 'true' and not(@primary = 'false')">
								<xsl:apply-templates />
							</xsl:if>
						</xsl:copy>
					</xsl:for-each>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="symbol[@primary = 'true']" mode="changes">
		<xsl:param name="sequencePattern" />
		<xsl:param name="position" />
		<xsl:variable name="symbolOfPattern" select="$sequencePattern/symbol[$position]" />
		<xsl:choose>
			<xsl:when test="$sequencePattern/changes">
				<xsl:apply-templates select="features[@class = 'descriptive']/feature" mode="mergeFeatures">
					<xsl:with-param name="symbolOfPattern" select="$symbolOfPattern" />
					<xsl:with-param name="position" select="$position" />
					<xsl:with-param name="changes" select="$sequencePattern/changes[@primary = 'true']" />
					<xsl:with-param name="changesCounterpart" select="$sequencePattern/changes[@primary = 'false']" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="features[@class = 'descriptive']/feature" mode="mergeFeatures">
					<xsl:with-param name="symbolOfPattern" select="$symbolOfPattern" />
					<xsl:with-param name="position" select="$position" />
					<xsl:with-param name="changes" select="$symbolOfPattern/changes" />
					<xsl:with-param name="changesCounterpart" select="$sequencePattern/symbol[@primary = 'false'][changes[replace or retain]]/changes" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="symbol[@primary = 'false']" mode="changes">
		<xsl:param name="sequencePattern" />
		<xsl:param name="position" />
		<xsl:variable name="symbolOfPattern" select="$sequencePattern/symbol[$position]" />
		<xsl:choose>
			<xsl:when test="$symbolOfPattern/@primary = 'true' and $sequencePattern/changes">
				<xsl:apply-templates select="features[@class = 'descriptive']/feature" mode="mergeFeatures">
					<xsl:with-param name="symbolOfPattern" select="$symbolOfPattern" />
					<xsl:with-param name="position" select="$position" />
					<xsl:with-param name="changes" select="$sequencePattern/changes[@primary = 'false']" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="features[@class = 'descriptive']/feature" mode="mergeFeatures">
					<xsl:with-param name="symbolOfPattern" select="$symbolOfPattern" />
					<xsl:with-param name="position" select="$position" />
					<xsl:with-param name="changes" select="$symbolOfPattern/changes" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Merge features according to add, remove, and retain elements in the sequencePattern. -->

	<!-- Copy a descriptive feature of the primary base symbol of the segment, -->
	<!-- unless the corresponding symbol of the sequencePattern removes it (by category). -->
	<xsl:template match="symbol[@primary = 'true']/features[@class = 'descriptive']/feature" mode="mergeFeatures">
		<xsl:param name="symbolOfPattern" />
		<xsl:param name="position" />
		<xsl:param name="changes" />
		<xsl:param name="changesCounterpart" />
		<xsl:variable name="feature" select="." />
		<xsl:variable name="category" select="@category" />
		<xsl:variable name="featureReplace" select="$changes/replace/feature[@category = $category][text()]" />
		<xsl:choose>
			<!-- Replace any feature of a category with a feature in the primary symbol of the pattern. -->
			<!-- For example, affricate. -->
			<xsl:when test="$featureReplace">
				<xsl:apply-templates select="$featureReplace" mode="featureDefinition" />
			</xsl:when>
			<!-- Replace any feature of a category with a feature in a non-primary symbol of the segment. -->
			<!-- For example, voicing of a click accompaniment. -->
			<xsl:when test="$changesCounterpart/replace/feature[@category = $category]" />
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:if test="not($category = 'type')">
						<xsl:choose>
							<!-- If a vowel sequence, add primary and position attributes to distinguish this feature. -->
							<xsl:when test="$symbolOfPattern/ifAnyOf/feature[. = 'vowel']">
								<xsl:attribute name="primary">
									<xsl:value-of select="'true'" />
								</xsl:attribute>
								<xsl:attribute name="position">
									<xsl:value-of select="$position" />
								</xsl:attribute>
							</xsl:when>
							<!-- If any non-primary symbol of sequencePattern retains a feature with the same category, -->
							<!-- add a primary attribute to distinguish this feature. -->
							<xsl:when test="$changesCounterpart/retain/feature[@category = $category]">
								<xsl:attribute name="primary">
									<xsl:value-of select="'true'" />
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
					</xsl:if>
					<xsl:apply-templates />
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Copy a descriptive feature from a non-primary base symbol of the segment, -->
	<!-- only if the corresponding symbol of the sequencePattern retains it (by category). -->
	<!-- Copy any primary="false" attribute in features of the sequencePattern: -->
	<!-- * Secondary place of articulation for consonants (for example, labial-velar). -->
	<!-- * Secondary height, backness, or rounding for vowel dipthongs. -->
	<xsl:template match="symbol[@primary = 'false']/features[@class = 'descriptive']/feature" mode="mergeFeatures">
		<xsl:param name="symbolOfPattern" />
		<xsl:param name="position" />
		<xsl:param name="changes" />
		<xsl:variable name="category" select="@category" />
		<xsl:if test="not($category = 'type' or $category = 'sequence')">
			<xsl:choose>
				<!-- If a vowel sequence, add primary and position attributes to distinguish this feature. -->
				<xsl:when test="$symbolOfPattern/ifAnyOf/feature[. = 'vowel']">
					<xsl:copy>
						<xsl:apply-templates select="@*" />
						<xsl:attribute name="primary">
							<xsl:value-of select="'false'" />
						</xsl:attribute>
						<xsl:attribute name="position">
							<xsl:value-of select="$position" />
						</xsl:attribute>
						<xsl:apply-templates />
					</xsl:copy>
				</xsl:when>
				<xsl:when test="$changes/retain/feature[@category = $category]">
					<xsl:copy>
						<xsl:apply-templates select="@*" />
						<xsl:attribute name="primary">
							<xsl:value-of select="'false'" />
						</xsl:attribute>
						<xsl:apply-templates />
					</xsl:copy>
				</xsl:when>
				<xsl:when test="$changes/replace/feature[@category = $category][not(text())] or ($category != 'voicing' and $category != 'place' and $category != 'manner')">
					<xsl:copy>
						<xsl:apply-templates select="@* | node()" />
					</xsl:copy>
				</xsl:when>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Copy attributes from featureDefinition to feature. -->
	<xsl:template match="feature" mode="featureDefinition">
		<xsl:call-template name="feature">
			<xsl:with-param name="name" select="." />
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="feature">
		<xsl:param name="name" />
		<xsl:variable name="featureDefinition" select="$projectDescriptiveFeatures/featureDefinition[name = $name]" />
		<feature>
			<!-- Copy attributes from features definition: class, category, order. -->
			<xsl:copy-of select="$featureDefinition/@class" />
			<xsl:copy-of select="$featureDefinition/@category" />
			<xsl:copy-of select="$featureDefinition/@order" />
			<xsl:value-of select="$name" />
		</feature>
	</xsl:template>

</xsl:stylesheet>