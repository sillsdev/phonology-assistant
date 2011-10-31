<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_chartKey_order_1.xsl 2011-10-28 -->
  <!-- Add an order attribute to chartKey elements. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<!-- Assume that the project inventory has descriptive feature definitions with an order attribute. -->
	<xsl:variable name="projectDescriptiveFeatures" select="/inventory/featureDefinitions[@class = 'descriptive']" />
	<xsl:variable name="orderFormat" select="translate(count($projectDescriptiveFeatures/featureDefinition), '0123456789', '0000000000')" />
	<xsl:variable name="orderZero" select="format-number(0, $orderFormat)" />

	<!-- Assume that the project inventory has chart key patterns. -->
	<xsl:variable name="chartKeyPatterns" select="/inventory/chartKeyPatterns[@class = 'rowGeneral']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Chart key is the order of the feature. -->
	<xsl:template match="keys/chartKey">
		<xsl:variable name="featureName" select="." />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="not(@order)">
				<xsl:copy-of select="$projectDescriptiveFeatures/featureDefinition[name = $featureName]/@order" />
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Chart key is the order of the feature. -->
	<xsl:template match="keys/chartKey[@class = 'tone']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="not(@order)">
				<xsl:attribute name="order">
					<xsl:choose>
						<xsl:when test="feature">
							<xsl:apply-templates select="feature[1]" mode="order" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$orderZero" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Assume at most one conditional feature per segment. -->
	<!-- For a consonant chart, a step in the pipeline will replace 1 with 0 -->
	<!-- if the feature doesn't distinguish segments in any cell of the rowgroup. -->
	<xsl:template match="keys/chartKey[@class = 'rowConditional']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="not(@order)">
				<xsl:attribute name="order">
					<xsl:choose>
						<xsl:when test="feature">
							<xsl:apply-templates select="feature[1]" mode="order" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$orderZero" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Chart key for row is based on a pattern. -->
  <xsl:template match="keys/chartKey[@class = 'rowGeneral']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="not(@order)">
				<xsl:attribute name="order">
					<xsl:call-template name="chartKeyPatterns">
						<xsl:with-param name="chartKeyPattern" select="$chartKeyPatterns/chartKeyPattern[1]" />
						<xsl:with-param name="segment" select="../.." />
						<xsl:with-param name="features" select="." />
					</xsl:call-template>
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
  </xsl:template>

	<!-- Chart key for row is based on the first pattern that matches the features of the segment. -->
	<xsl:template name="chartKeyPatterns">
		<xsl:param name="chartKeyPattern" />
		<xsl:param name="segment" />
		<xsl:param name="features" />
		<xsl:variable name="boolean">
			<xsl:choose>
				<xsl:when test="$chartKeyPattern">
					<xsl:call-template name="booleanAnd">
						<xsl:with-param name="booleanSequence">
							<xsl:apply-templates select="$chartKeyPattern/if/*" mode="booleanFeatures">
								<xsl:with-param name="features" select="$segment/features[@class = 'descriptive']" />
							</xsl:apply-templates>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'false'" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$boolean = 'true'">
				<xsl:apply-templates select="$chartKeyPattern/then/*" mode="chartKeyPatterns">
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$chartKeyPattern/following-sibling::chartKeyPattern">
				<xsl:call-template name="chartKeyPatterns">
					<xsl:with-param name="chartKeyPattern" select="$chartKeyPattern/following-sibling::chartKeyPattern[1]" />
					<xsl:with-param name="segment" select="$segment" />
					<xsl:with-param name="features" select="$features" />
				</xsl:call-template>
			</xsl:when>
		</xsl:choose>	
	</xsl:template>

	<!-- The choose element contains mutually exclusive features. -->
	<xsl:template match="choose" mode="chartKeyPatterns">
		<xsl:param name="features" />
		<xsl:apply-templates select="*[1]" mode="chartKeyPatterns">
			<xsl:with-param name="features" select="$features" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- The diphthong element contains the non-primary features. -->
	<xsl:template match="diphthong" mode="chartKeyPatterns">
		<xsl:param name="features" />
		<xsl:apply-templates select="*" mode="chartKeyPatterns">
			<xsl:with-param name="features" select="$features/diphthong" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- Chart key includes a specific feature, otherwise zero. -->
	<xsl:template match="feature" mode="chartKeyPatterns">
		<xsl:param name="features" />
		<xsl:variable name="featureOfPattern" select="." />
		<xsl:variable name="featureOfSegment" select="$features/feature[. = $featureOfPattern]" />
		<xsl:choose>
			<xsl:when test="$featureOfSegment">
				<xsl:apply-templates select="$featureOfSegment" mode="order" />
			</xsl:when>
			<xsl:when test="../self::choose and following-sibling::feature[1]">
				<xsl:apply-templates select="following-sibling::feature[1]" mode="chartKeyPatterns">
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$orderZero" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Chart key includes a category of features, otherwise zero. -->
	<xsl:template match="feature[@category]" mode="chartKeyPatterns">
		<xsl:param name="features" />
		<xsl:variable name="category" select="@category" />
		<xsl:variable name="order">
			<xsl:for-each select="$projectDescriptiveFeatures/featureDefinition[@category = $category]">
				<xsl:variable name="name" select="name" />
				<xsl:if test="$features/feature[. = $name]">
					<!-- If the segment has the feature, get the order attribute from the feature definition. -->
					<xsl:value-of select="@order" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="maxOccurs">
			<xsl:choose>
				<xsl:when test="@maxOccurs">
					<xsl:value-of select="number(@maxOccurs)" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="1" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="$order" />
		<xsl:call-template name="orderZero">
			<xsl:with-param name="n" select="$maxOccurs - (string-length($order) div string-length($orderFormat))" />
		</xsl:call-template>
	</xsl:template>

	<!-- If the feature has an order attribute, use it; otherwise get it from the feature definition. -->
	<xsl:template match="feature" mode="order">
		<xsl:choose>
			<xsl:when test="@order">
				<xsl:value-of select="@order" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="name" select="." />
				<xsl:value-of select="$projectDescriptiveFeatures/featureDefinition[name = $name]/@order" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Order attribute of key includes zero if the segment does not have a feature. -->
	<xsl:template name="orderZero">
		<xsl:param name="n" />
		<xsl:if test="$n &gt; 0">
			<xsl:value-of select="$orderZero" />
			<xsl:call-template name="orderZero">
				<xsl:with-param name="n" select="$n - 1" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>