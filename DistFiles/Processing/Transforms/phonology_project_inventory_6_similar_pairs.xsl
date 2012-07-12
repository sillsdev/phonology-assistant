<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_6_similar_pairs.xsl 2012-05-24 -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<xsl:variable name="classOfSortKey" select="'place_or_backness'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="inventory">
		<xsl:variable name="segments" select="segments" />
		<xsl:variable name="metadata" select="div[@id = 'metadata']" />
		<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
		<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
		<xsl:variable name="programDistinctiveFeaturesFile" select="concat($settings/li[@class = 'programDistinctiveFeaturesName'], '.DistinctiveFeatures.xml')" />
		<xsl:variable name="programDistinctiveFeaturesXML" select="concat($programConfigurationFolder, $programDistinctiveFeaturesFile)" />
		<xsl:variable name="similarityMetric" select="document($programDistinctiveFeaturesXML)/inventory/similarityMetric" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:if test="$similarityMetric">
				<pairs type="consonant">
					<xsl:call-template name="pairs">
						<xsl:with-param name="segments" select="$segments" />
            <xsl:with-param name="type" select="'consonant'" />
            <xsl:with-param name="similarityMetric" select="$similarityMetric" />
          </xsl:call-template>
				</pairs>
				<pairs type="vowel">
					<xsl:call-template name="pairs">
						<xsl:with-param name="segments" select="$segments" />
            <xsl:with-param name="type" select="'vowel'" />
            <xsl:with-param name="similarityMetric" select="$similarityMetric" />
          </xsl:call-template>
				</pairs>
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="pairs">
		<xsl:param name="segments" />
    <xsl:param name="type" />
    <xsl:param name="similarityMetric" />
    <xsl:for-each select="$segments/segment[features[@class = 'descriptive']/feature[. = $type]]">
			<xsl:sort select="keys/sortKey[@class = $classOfSortKey]" />
			<xsl:variable name="segment1" select="." />
			<xsl:for-each select="$segments/segment[features[@class = 'descriptive']/feature[. = $type]]">
				<xsl:sort select="keys/sortKey[@class = $classOfSortKey]" />
				<xsl:variable name="segment2" select="." />
				<xsl:if test="$segment1/keys/sortKey[@class = $classOfSortKey] &lt; $segment2/keys/sortKey[@class = $classOfSortKey]">
					<xsl:call-template name="pair">
						<xsl:with-param name="segment1" select="$segment1" />
						<xsl:with-param name="segment2" select="$segment2" />
						<xsl:with-param name="similarityMetric" select="$similarityMetric" />
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="pair">
		<xsl:param name="segment1" />
		<xsl:param name="segment2" />
		<xsl:param name="similarityMetric" />
		<xsl:variable name="features1" select="$segment1/features[@class = 'distinctive']" />
		<xsl:variable name="features2" select="$segment2/features[@class = 'distinctive']" />
    <xsl:variable name="value">
      <xsl:call-template name="value">
        <xsl:with-param name="features1" select="$features1" />
        <xsl:with-param name="features2" select="$features2" />
        <xsl:with-param name="featureMetrics" select="$similarityMetric/featureMetric" />
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="similarity">
      <xsl:choose>
        <xsl:when test="$value &lt;= 1">
          <xsl:value-of select="'more'" />
        </xsl:when>
        <xsl:when test="$value &lt;= 2">
          <xsl:variable name="articulators">
            <xsl:call-template name="articulators">
              <xsl:with-param name="features1" select="$features1" />
              <xsl:with-param name="features2" select="$features2" />
              <xsl:with-param name="featureMetrics" select="$similarityMetric/featureMetric[@articulator = 'true']" />
            </xsl:call-template>
          </xsl:variable>
          <xsl:if test="$articulators &lt;= 1">
            <xsl:value-of select="'less'" />
          </xsl:if>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="string-length($similarity) != 0">
      <pair similarity="{$similarity}">
        <segment literal="{$segment1/@literal}" />
        <segment literal="{$segment2/@literal}" />
      </pair>
    </xsl:if>
	</xsl:template>

  <!-- The sum of the values of the feature differences might be a fraction. -->
  <!-- For information about computing sums, see pages 54-57 in XSLT Cookbook. -->
  <xsl:template name="value">
    <xsl:param name="features1" />
    <xsl:param name="features2" />
    <xsl:param name="featureMetrics" />
    <xsl:param name="result" select="0" />
    <xsl:choose>
      <xsl:when test="not($featureMetrics)">
        <xsl:value-of select="$result" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="featureMetric" select="$featureMetrics[1]" />
        <xsl:variable name="boolean">
          <xsl:call-template name="difference">
            <xsl:with-param name="features1" select="$features1" />
            <xsl:with-param name="features2" select="$features2" />
            <xsl:with-param name="name" select="$featureMetric/name" />
            <xsl:with-param name="forAnyOf" select="$featureMetric/forAnyOf" />
          </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="value">
          <xsl:choose>
            <xsl:when test="$boolean = 'true'">
              <xsl:value-of select="$featureMetric/@value" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="0" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:call-template name="value">
          <xsl:with-param name="features1" select="$features1" />
          <xsl:with-param name="features2" select="$features2" />
          <xsl:with-param name="featureMetrics" select="$featureMetrics[position() != 1]" />
          <xsl:with-param name="result" select="$result + $value" />
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- The sum of the articulator differences is an integer. -->
  <!-- For information about computing sums, see pages 54-57 in XSLT Cookbook. -->
  <xsl:template name="articulators">
    <xsl:param name="features1" />
    <xsl:param name="features2" />
    <xsl:param name="featureMetrics" />
    <xsl:param name="result" select="0" />
    <xsl:choose>
      <xsl:when test="not($featureMetrics)">
        <xsl:value-of select="$result" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="featureMetric" select="$featureMetrics[1]" />
        <xsl:variable name="boolean">
          <xsl:call-template name="difference">
            <xsl:with-param name="features1" select="$features1" />
            <xsl:with-param name="features2" select="$features2" />
            <xsl:with-param name="name" select="$featureMetric/name" />
            <xsl:with-param name="forAnyOf" select="$featureMetric/forAnyOf" />
          </xsl:call-template>
        </xsl:variable>
        <xsl:call-template name="articulators">
          <xsl:with-param name="features1" select="$features1" />
          <xsl:with-param name="features2" select="$features2" />
          <xsl:with-param name="featureMetrics" select="$featureMetrics[position() != 1]" />
          <xsl:with-param name="result" select="$result + ($boolean = 'true')" />
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="difference">
    <xsl:param name="features1" />
    <xsl:param name="features2" />
    <xsl:param name="name" />
    <xsl:param name="forAnyOf" />
    <xsl:variable name="feature1" select="$features1/feature[substring(., 2) = $name]" />
    <xsl:variable name="feature2" select="$features2/feature[substring(., 2) = $name]" />
    <xsl:choose>
      <xsl:when test="substring($feature1, 1, 1) = substring($feature2, 1, 1)">
        <xsl:value-of select="'false'" />
      </xsl:when>
      <xsl:when test="$forAnyOf">
        <xsl:variable name="booleanSequence">
          <xsl:call-template name="booleanOr">
            <xsl:with-param name="booleanSequence">
              <xsl:apply-templates select="$forAnyOf/*" mode="booleanFeatures">
                <xsl:with-param name="features" select="$features1" />
              </xsl:apply-templates>
            </xsl:with-param>
          </xsl:call-template>
          <xsl:call-template name="booleanOr">
            <xsl:with-param name="booleanSequence">
              <xsl:apply-templates select="$forAnyOf/*" mode="booleanFeatures">
                <xsl:with-param name="features" select="$features2" />
              </xsl:apply-templates>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$forAnyOf/@segments = 'either'">
            <xsl:call-template name="booleanOr">
              <xsl:with-param name="booleanSequence" select="$booleanSequence" />
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="booleanAnd">
              <xsl:with-param name="booleanSequence" select="$booleanSequence" />
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'true'" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>