<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_1c_diacritics.xsl 2011-08-02 -->
  <!-- Merge descriptive features of each diacritic symbol into the base symbol. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />
	
  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Remove descriptive features or feature rules from diacritic symbols in the project inventory file. -->
	<xsl:template match="symbol[@base = 'false']/*" />

	<xsl:template match="symbol[@base = 'true']/features[@class = 'descriptive']">
		<xsl:variable name="symbol" select=".." />
		<xsl:variable name="features" select="." />
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:for-each select="feature">
        <xsl:variable name="boolean">
					<xsl:call-template name="booleanOr">
						<xsl:with-param name="booleanSequence">
							<xsl:apply-templates select="$symbol/preceding-sibling::symbol[1][@base = 'false'][@precedesBase = 'true']" mode="removeFeaturePreceding">
								<xsl:with-param name="feature" select="." />
							</xsl:apply-templates>
							<xsl:apply-templates select="$symbol/following-sibling::symbol[1][@base = 'false']" mode="removeFeatureFollowing">
								<xsl:with-param name="feature" select="." />
							</xsl:apply-templates>
						</xsl:with-param>
					</xsl:call-template>
        </xsl:variable>
        <!-- If no diacritic cancels out the feature, copy it. -->
        <xsl:if test="$boolean != 'true'">
          <xsl:apply-templates select="." />
        </xsl:if>
      </xsl:for-each>
			<xsl:apply-templates select="$symbol/preceding-sibling::symbol[1][@base = 'false'][@precedesBase = 'true']" mode="addFeaturesPreceding">
				<xsl:with-param name="features" select="$features" />
			</xsl:apply-templates>
			<xsl:apply-templates select="$symbol/following-sibling::symbol[1][@base = 'false']" mode="addFeaturesFollowing">
				<xsl:with-param name="features" select="$features" />
			</xsl:apply-templates>
    </xsl:copy>
  </xsl:template>

	<!-- Iterator for diacritic symbols which precede the base symbol. -->
	<xsl:template match="symbol" mode="removeFeaturePreceding">
		<xsl:param name="feature" />
		<xsl:call-template name="removeFeature">
			<xsl:with-param name="feature" select="$feature" />
		</xsl:call-template>
		<xsl:apply-templates select="preceding-sibling::symbol[1][@base = 'false'][@precedesBase = 'true']" mode="removeFeaturePreceding">
			<xsl:with-param name="feature" select="$feature" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- Iterator for diacritic symbols which follow the base symbol. -->
	<xsl:template match="symbol" mode="removeFeatureFollowing">
    <xsl:param name="feature" />
		<xsl:call-template name="removeFeature">
			<xsl:with-param name="feature" select="$feature" />
		</xsl:call-template>
		<xsl:apply-templates select="following-sibling::symbol[1][@base = 'false']" mode="removeFeatureFollowing">
			<xsl:with-param name="feature" select="$feature" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- Return true if a diacritic symbol has either of the following for a feature of the base symbol: -->
	<!-- 1. a feature from its category. -->
	<!-- 2. a change rule for it. -->
	<xsl:template name="removeFeature">
		<xsl:param name="feature" />
		<xsl:variable name="category" select="$feature/@category" />
		<xsl:choose>
			<xsl:when test="features[@class = 'descriptive']/feature[@category = $category]">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:when test="featureRule[@class = 'descriptive']/change/find/feature[. = $feature]">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Iterator for diacritic symbols which precede the base symbol. -->
	<xsl:template match="symbol" mode="addFeaturesPreceding">
		<xsl:param name="features" />
		<xsl:call-template name="addFeatures">
			<xsl:with-param name="features" select="$features" />
		</xsl:call-template>
		<xsl:apply-templates select="preceding-sibling::symbol[1][@base = 'false'][@precedesBase = 'true']" mode="addFeaturesPreceding">
			<xsl:with-param name="features" select="$features" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- Iterator for diacritic symbols which follow the base symbol. -->
	<xsl:template match="symbol" mode="addFeaturesFollowing">
		<xsl:param name="features" />
		<xsl:call-template name="addFeatures">
			<xsl:with-param name="features" select="$features" />
		</xsl:call-template>
		<xsl:apply-templates select="following-sibling::symbol[1][@base = 'false']" mode="addFeaturesFollowing">
			<xsl:with-param name="features" select="$features" />
		</xsl:apply-templates>
  </xsl:template>

	<!-- Copy descriptive features from diacritic symbol. -->
	<xsl:template name="addFeatures">
		<xsl:param name="features" />
		<xsl:apply-templates select="features[@class = 'descriptive']/feature" />
		<xsl:apply-templates select="featureRule[@class = 'descriptive']/*[1]" mode="addFeatures">
			<xsl:with-param name="features" select="$features" />
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="symbol/featureRule[@class = 'descriptive']/add[if/precedesBase]" mode="addFeatures">
		<xsl:param name="features" />
		<xsl:variable name="symbol" select="../.." />
		<xsl:choose>
			<xsl:when test="$symbol[@precedesBase = 'true']">
				<xsl:copy-of select="then/feature" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="following-sibling::*[1]" mode="addFeatures">
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="symbol/featureRule[@class = 'descriptive']/change" mode="addFeatures">
		<xsl:param name="features" />
		<xsl:variable name="feature" select="find/feature" />
		<xsl:choose>
			<xsl:when test="$features/feature[. = $feature]">
				<xsl:copy-of select="replace/feature" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="following-sibling::*[1]" mode="addFeatures">
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="symbol/featureRule[@class = 'descriptive']/otherwise" mode="addFeatures">
		<xsl:copy-of select="feature" />
	</xsl:template>

</xsl:stylesheet>