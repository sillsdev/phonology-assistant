<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_6_similar_pairs.xsl 2011-10-21 -->

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
		<xsl:variable name="pairSimilarity" select="document($programDistinctiveFeaturesXML)/inventory/pairSimilarity" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:if test="$pairSimilarity">
				<pairs type="consonant">
					<xsl:call-template name="pairs">
						<xsl:with-param name="segments" select="$segments" />
						<xsl:with-param name="pairSimilarity" select="$pairSimilarity" />
						<xsl:with-param name="type" select="'consonant'" />
					</xsl:call-template>
				</pairs>
				<pairs type="vowel">
					<xsl:call-template name="pairs">
						<xsl:with-param name="segments" select="$segments" />
						<xsl:with-param name="pairSimilarity" select="$pairSimilarity" />
						<xsl:with-param name="type" select="'vowel'" />
					</xsl:call-template>
				</pairs>
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="pairs">
		<xsl:param name="segments" />
		<xsl:param name="pairSimilarity" />
		<xsl:param name="type" />
		<xsl:variable name="pairDifferences" select="$pairSimilarity/pairDifferences[@type = $type]" />
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
						<xsl:with-param name="pairDifferences" select="$pairDifferences" />
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="pair">
		<xsl:param name="segment1" />
		<xsl:param name="segment2" />
		<xsl:param name="pairDifferences" />
		<xsl:variable name="features1" select="$segment1/features[@class = 'distinctive']" />
		<xsl:variable name="features2" select="$segment2/features[@class = 'distinctive']" />
		<xsl:variable name="primaryDifferences">
			<xsl:call-template name="pairDifferences">
				<xsl:with-param name="features1" select="$features1" />
				<xsl:with-param name="features2" select="$features2" />
				<xsl:with-param name="pairDifferences" select="$pairDifferences" />
				<xsl:with-param name="primary" select="'true'" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="primaryLength" select="string-length($primaryDifferences)" />
		<xsl:if test="$primaryLength &lt;= 1">
			<xsl:variable name="secondaryDifferences">
				<xsl:call-template name="pairDifferences">
					<xsl:with-param name="features1" select="$features1" />
					<xsl:with-param name="features2" select="$features2" />
					<xsl:with-param name="pairDifferences" select="$pairDifferences" />
					<xsl:with-param name="primary" select="'false'" />
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="secondaryLength" select="string-length($secondaryDifferences)" />
			<xsl:variable name="differences" select="$primaryLength + $secondaryLength" />
			<xsl:if test="$differences &lt;= 2">
				<xsl:variable name="similarity">
					<xsl:choose>
						<xsl:when test="$differences &lt;= 1">
							<xsl:value-of select="'more'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'less'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<pair similarity="{$similarity}">
					<segment literal="{$segment1/@literal}" />
					<segment literal="{$segment2/@literal}" />
				</pair>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template name="pairDifferences">
		<xsl:param name="features1" />
		<xsl:param name="features2" />
		<xsl:param name="pairDifferences" />
		<xsl:param name="primary" />
		<xsl:for-each select="$pairDifferences/pairDifference[@primary = $primary]">
			<xsl:variable name="forAnyOf" select="forAnyOf" />
			<xsl:variable name="boolean">
				<xsl:choose>
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
			</xsl:variable>
			<xsl:if test="$boolean = 'true'">
				<xsl:call-template name="differenceDistinctive">
					<xsl:with-param name="features1" select="$features1" />
					<xsl:with-param name="features2" select="$features2" />
					<xsl:with-param name="name" select="name" />
					<xsl:with-param name="count" select="@count" />
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="differenceDistinctive">
		<xsl:param name="features1" />
		<xsl:param name="features2" />
		<xsl:param name="name" />
		<xsl:param name="count" />
		<xsl:variable name="feature1" select="$features1/feature[substring(., 2) = $name]" />
		<xsl:variable name="feature2" select="$features2/feature[substring(., 2) = $name]" />
		<xsl:choose>
			<xsl:when test="substring($feature1, 1, 1) = substring($feature2, 1, 1)" />
			<xsl:otherwise>
				<xsl:call-template name="dup">
					<xsl:with-param name="input" select="'*'" />
					<xsl:with-param name="count" select="$count" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- For information about duplicating a string, see pages 6-8 in XSLT Cookbook. -->
	<xsl:template name="dup">
		<xsl:param name="input" />
		<xsl:param name="count" select="1" />
		<xsl:choose>
			<xsl:when test="not($count) or not($input)" />
			<xsl:when test="$count = 1">
				<xsl:value-of select="$input" />
			</xsl:when>
			<xsl:otherwise>
				<!-- If $count is odd append an extra copy of input -->
				<xsl:if test="$count mod 2">
					<xsl:value-of select="$input" />
				</xsl:if>
				<!-- Recursively apply template after doubling input and halving count -->
				<xsl:call-template name="dup">
					<xsl:with-param name="input" select="concat($input,$input)" />
					<xsl:with-param name="count" select="floor($count div 2)" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>