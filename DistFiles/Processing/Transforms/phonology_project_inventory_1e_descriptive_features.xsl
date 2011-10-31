<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_1e_descriptive_features.xsl 2011-08-26 -->
	<!-- Sort descriptive features in order. -->
	<!-- Select value of class attribute for consonant or vowel. -->
	<!-- If feature overrides differ from default features, keep a copy of the default features. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Remove features from symbols. -->
	<xsl:template match="symbol/features[@class = 'descriptive']" />

	<xsl:template match="segment[not(features[@class = 'descriptive'])]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:call-template name="default">
				<xsl:with-param name="segment" select="." />
			</xsl:call-template>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Copy feature overrides of segment. -->
	<xsl:template match="segment/features[@class = 'descriptive']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:call-template name="features">
				<xsl:with-param name="features" select="." />
			</xsl:call-template>
		</xsl:copy>
		<xsl:call-template name="default">
			<xsl:with-param name="segment" select=".." />
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="default">
		<xsl:param name="segment" />
		<xsl:variable name="symbol" select="$segment/symbols/symbol[@base = 'true'][not(@primary = 'false')][features[@class = 'descriptive']]" />
		<xsl:variable name="featuresDefault" select="$symbol/features[@class = 'descriptive']" />
		<xsl:variable name="featuresSelected" select="$segment/features[@class = 'descriptive']" />
		<xsl:variable name="class">
			<xsl:choose>
				<xsl:when test="$featuresSelected">
					<xsl:variable name="booleanDifferences">
						<xsl:call-template name="booleanFeatureDifferences">
							<xsl:with-param name="featuresA" select="$featuresSelected" />
							<xsl:with-param name="featuresB" select="$featuresDefault" />
						</xsl:call-template>
					</xsl:variable>
					<!-- If there are any differences between selected and default features, copy the default features. -->
					<xsl:if test="$booleanDifferences = 'true'">
						<xsl:value-of select="'descriptive default'" />
					</xsl:if>
				</xsl:when>
				<xsl:when test="$featuresDefault">
					<xsl:value-of select="'descriptive'" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="string-length($class) != 0">
			<features class="{$class}">
				<xsl:call-template name="features">
					<xsl:with-param name="features" select="$featuresDefault" />
				</xsl:call-template>
			</features>
		</xsl:if>
	</xsl:template>

	<!-- Sort features by order attribute. -->
	<!-- Remove duplicate features (not including diphthongs). -->
	<!-- For example; -->
	<!-- * advanced or retracted from diacritics under both base symbols in velar affricate. -->
	<!-- * prenasalized from two superscript nasal symbols preceding labial-velar plosive. -->
	<xsl:template name="features">
		<xsl:param name="features" />
		<xsl:for-each select="$features/feature">
			<xsl:sort select="@order" />
			<xsl:variable name="name" select="." />
			<xsl:if test="not(preceding-sibling::feature[. = $name][not(@position)])">
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<!-- Some classes depend on the type of segment: -->
	<!-- * col-row: voicing is col for consonant, row for vowel -->
	<!-- * colgroup-col: rounding is colgroup for consonant, col for vowel -->
	<xsl:template match="features[@class = 'descriptive']/feature/@class[contains(., '-')]">
		<xsl:variable name="features" select="../.." />
		<xsl:attribute name="class">
			<xsl:choose>
				<xsl:when test="$features/feature[. = 'consonant']">
					<xsl:value-of select="substring-before(., '-')" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-after(., '-')" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>

</xsl:stylesheet>