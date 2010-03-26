<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- PA_PhoneInventory_2a_diacritics.xsl 2010-03-19 -->
  <!-- Merge articulatory and binary features of each diacritic into the preceding base character. -->
	<!-- TO DO: What if there are repeated diacritics by mistake? -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="unit/sequence/symbol[isBase = 'true']/articulatoryFeatures">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:for-each select="feature">
        <xsl:variable name="feature">
          <xsl:choose>
            <xsl:when test="../../following-sibling::symbol[1][isBase = 'false']">
              <xsl:apply-templates select="../../following-sibling::symbol[1][isBase = 'false']" mode="articulatoryFeature">
                <xsl:with-param name="feature" select="." />
              </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="." />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <!-- If no diacritic cancels out the feature, copy it. -->
        <xsl:if test="string-length($feature) != 0">
          <xsl:apply-templates select="." />
        </xsl:if>
      </xsl:for-each>
			<xsl:apply-templates select="../following-sibling::symbol[1][isBase = 'false']" mode="articulatoryFeatures">
				<xsl:with-param name="articulatoryFeatures" select="." />
			</xsl:apply-templates>
    </xsl:copy>
  </xsl:template>

  <!-- Return the articulatory feature for the base unless a diacritic has either of the following: -->
	<!-- 1. a feature from its subclass. -->
	<!-- 2. a change rule for it. -->
  <xsl:template match="symbol" mode="articulatoryFeature">
    <xsl:param name="feature" />
		<xsl:variable name="subclass" select="$feature/@subclass" />
    <xsl:choose>
      <xsl:when test="articulatoryFeatures/feature[@subclass = $subclass]" />
      <xsl:when test="articulatoryFeatureRule/change/find/feature[. = $feature]" />
      <xsl:when test="following-sibling::symbol[1][isBase = 'false']">
        <xsl:apply-templates select="following-sibling::symbol[1][isBase = 'false']" mode="articulatoryFeature">
          <xsl:with-param name="feature" select="$feature" />
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$feature" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Copy articulatory features from diacritics. -->
  <xsl:template match="symbol" mode="articulatoryFeatures">
		<xsl:param name="articulatoryFeatures" />
    <xsl:apply-templates select="articulatoryFeatures/feature" />
		<xsl:apply-templates select="articulatoryFeatureRule/*[1]" mode="articulatoryFeatures">
			<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
		</xsl:apply-templates>
    <xsl:apply-templates select="following-sibling::symbol[1][isBase = 'false']" mode="articulatoryFeatures" />
  </xsl:template>

	<xsl:template match="articulatoryFeatureRule/change" mode="articulatoryFeatures">
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="feature" select="find/feature" />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $feature]">
				<xsl:copy-of select="replace/feature" />
			</xsl:when>
			<xsl:when test="following-sibling::*">
				<xsl:apply-templates select="following-sibling::*[1]" mode="articulatoryFeatures">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="articulatoryFeatureRule/add[if/precedesBase]" mode="articulatoryFeatures">
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<xsl:when test="ancestor::symbol/isBase[. = 'false'][@precedesBase = 'true']">
				<xsl:copy-of select="then/feature" />
			</xsl:when>
			<xsl:when test="following-sibling::*">
				<xsl:apply-templates select="following-sibling::*[1]" mode="articulatoryFeatures">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="articulatoryFeatureRule/otherwise" mode="articulatoryFeatures">
		<xsl:copy-of select="feature" />
	</xsl:template>

	<xsl:template match="unit/sequence/symbol[isBase = 'true']/binaryFeatures">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:for-each select="feature">
        <xsl:variable name="feature">
          <xsl:choose>
            <xsl:when test="../../following-sibling::symbol[1][isBase = 'false']">
              <xsl:apply-templates select="../../following-sibling::symbol[1][isBase = 'false']" mode="binaryFeature">
                <xsl:with-param name="feature" select="." />
              </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="." />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <!-- If no diacritic cancels out the feature, copy it. -->
        <xsl:if test="string-length($feature) != 0">
          <xsl:apply-templates select="." />
        </xsl:if>
      </xsl:for-each>
      <xsl:apply-templates select="../following-sibling::symbol[1][isBase = 'false']" mode="binaryFeatures" />
    </xsl:copy>
  </xsl:template>

  <!-- Return the value of the binary feature for the base unless a diacritic has a value. -->
  <xsl:template match="symbol" mode="binaryFeature">
    <xsl:param name="feature" />
    <xsl:choose>
      <xsl:when test="binaryFeatures/feature[substring(., 2) = substring($feature, 2)]" />
      <xsl:when test="following-sibling::symbol[1][isBase = 'false']">
        <xsl:apply-templates select="following-sibling::symbol[1][isBase = 'false']" mode="binaryFeature">
          <xsl:with-param name="feature" select="$feature" />
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$feature" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Copy binary features from diacritics. -->
  <xsl:template match="symbol" mode="binaryFeatures">
    <xsl:apply-templates select="binaryFeatures/feature" />
    <xsl:apply-templates select="following-sibling::symbol[1][isBase = 'false']" mode="binaryFeatures" />
  </xsl:template>

  <xsl:template match="unit/sequence/symbol[isBase = 'false']" />

</xsl:stylesheet>