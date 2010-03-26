<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2b_base_sequences.xsl 2010-03-26 -->
  <!-- Merge articulatory features of base character sequences. -->
	<!-- General rule: Keep features of the primary base symbol; omit features of non-primary base symbols. -->
	<!-- Specific rules might do any of the following: -->
	<!-- * Retain features (by subclass) from a non-primary base symbol. -->
	<!--   If it has a primary="false" attribute, add a primary="true" attribute to the corresponding feature of its subclass. -->
	<!-- * Remove a feature (by subclass) from the primary base symbol. -->
	<!-- * Add a feature to the primary base symbol. -->
  <!-- TO DO: Determine binary features from articulatory features according to rules. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="programConfigurationFolder" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programPhoneticInventoryFile']" />
	
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="sequences" select="document($programPhoneticInventoryXML)/inventory/sequences" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="@*|node()" mode="sequence">
		<xsl:param name="sequence" />
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" mode="sequence">
				<xsl:with-param name="sequence" select="$sequence" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="unit/sequence[symbol[isBase = 'true' and isBase/@primary = 'true']][symbol[isBase = 'true' and isBase/@primary = 'false']]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$sequences/sequence">
					<xsl:apply-templates select="$sequences/sequence[1]" mode="sequence">
						<xsl:with-param name="sequence" select="." />
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="symbol[isBase = 'true' and isBase/@primary = 'true']" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="inventory/sequences/sequence" mode="sequence">
		<xsl:param name="sequence" />
		<xsl:variable name="featurePrimaryTrue">
			<xsl:apply-templates select="symbol[@primary = 'true']/if/*[1]" mode="sequence">
				<xsl:with-param name="symbol" select="$sequence/symbol[isBase = 'true' and isBase/@primary = 'true']" />
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="featurePrimaryFalse">
			<xsl:apply-templates select="symbol[@primary = 'false']/if/*[1]" mode="sequence">
				<xsl:with-param name="symbol" select="$sequence/symbol[isBase = 'true' and isBase/@primary = 'false']" />
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="string-length($featurePrimaryTrue) != 0 and string-length($featurePrimaryFalse) != 0">
				<xsl:apply-templates select="$sequence/symbol[isBase = 'true' and isBase/@primary = 'true']" mode="sequence">
					<xsl:with-param name="sequence" select="." />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="following-sibling::sequence">
				<xsl:apply-templates select="following-sibling::sequence[1]" mode="sequence">
					<xsl:with-param name="sequence" select="$sequence" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="$sequence/symbol[isBase = 'true' and isBase/@primary = 'true']" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="inventory/sequences/sequence/symbol/if/feature" mode="sequence">
		<xsl:param name="symbol" />
		<xsl:variable name="feature" select="." />
		<xsl:value-of select="$symbol/articulatoryFeatures/feature[. = $feature]" />
	</xsl:template>

	<xsl:template match="inventory/sequences/sequence/symbol/if/anyOf" mode="sequence">
		<xsl:param name="symbol" />
		<xsl:apply-templates select="feature[1]" mode="sequence">
			<xsl:with-param name="symbol" select="$symbol" />
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="inventory/sequences/sequence/symbol/if/anyOf/feature" mode="sequence">
		<xsl:param name="symbol" />
		<xsl:variable name="feature" select="." />
		<xsl:choose>
			<xsl:when test="$symbol/articulatoryFeatures/feature[. = $feature]">
				<xsl:value-of select="$symbol/articulatoryFeatures/feature[. = $feature]" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="following-sibling::feature[1]" mode="sequence">
					<xsl:with-param name="symbol" select="$symbol" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="unit/sequence/symbol[isBase = 'true' and isBase/@primary = 'true']/articulatoryFeatures" mode="sequence">
		<xsl:param name="sequence" />
		<xsl:copy>
			<xsl:for-each select="ancestor::sequence[1]/symbol[isBase = 'true' and isBase/@primary = 'false']/articulatoryFeatures/feature">
				<xsl:variable name="subclass" select="@subclass" />
				<xsl:if test="$sequence/symbol[@primary = 'false']/retain/feature[@subclass = $subclass]">
					<xsl:copy>
						<xsl:apply-templates select="@*" />
						<xsl:copy-of select="$sequence/symbol[@primary = 'false']/retain/feature[@subclass = $subclass]/@primary" />
						<xsl:apply-templates />
					</xsl:copy>
				</xsl:if>
			</xsl:for-each>
			<xsl:apply-templates select="feature" mode="sequence">
				<xsl:with-param name="sequence" select="$sequence" />
			</xsl:apply-templates>
			<xsl:apply-templates select="$sequence/symbol[@primary = 'true']/add/feature" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="unit/sequence/symbol[isBase = 'true' and isBase/@primary = 'true']/articulatoryFeatures/feature" mode="sequence">
		<xsl:param name="sequence" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:choose>
			<xsl:when test="$sequence/symbol[@primary = 'true']/remove/feature[@subclass = $subclass]" />
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:if test="$sequence/symbol[@primary = 'false']/retain/feature[@subclass = $subclass][@primary = 'false']">
						<xsl:attribute name="primary">
							<xsl:value-of select="'true'" />
						</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates />
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>