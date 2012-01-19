<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_4b_distinctive_features.xsl 2011-09-21 -->
  <!-- Convert from descriptive to distinctive features according to rules. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />
	
	<xsl:variable name="projectDistinctiveFeatures" select="/inventory/featureDefinitions[@class = 'distinctive']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Remove the copy of distinctive feature definitions from the project inventory file. -->
	<xsl:template match="/inventory/featureDefinitions[@class = 'distinctive']" />

	<!-- For each segment, insert distinctive features -->
	<!-- following descriptive features and any distinctive feature (overrides). -->

	<xsl:template match="segment/features[starts-with(@class, 'descriptive')][not(following-sibling::features[starts-with(@class, 'descriptive') or @class = 'distinctive'])]">
		<xsl:copy-of select="." />
		<xsl:call-template name="distinctive" />
	</xsl:template>

	<xsl:template match="segment/features[@class = 'distinctive']">
		<xsl:copy-of select="." />
		<xsl:call-template name="distinctive">
			<xsl:with-param name="classSuffix" select="'default'" />
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="distinctive">
		<xsl:param name="classSuffix" />
		<xsl:variable name="class">
			<xsl:value-of select="'distinctive'" />
			<xsl:if test="string-length($classSuffix) != 0">
				<xsl:value-of select="' '" />
				<xsl:value-of select="$classSuffix" />
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="features" select="../features[@class = 'descriptive']" />
		<features class="{$class}">
			<xsl:if test="$features/feature">
				<!-- Evaluate only if there is at least one descriptive feature, -->
				<!-- because a project inventory can contain bogus segments -->
				<!-- which consist of independent diacritic symbols -->
				<!-- if the ambiguous sequences are incompletely specified. -->
				<!-- Evaluate distinctive feature rules independently. -->
				<!-- That is, multiple rules do not mean switch-case-otherwise or if-else-if. -->
				<xsl:apply-templates select="$projectDistinctiveFeatures/featureDefinition/rule" mode="feature">
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
			</xsl:if>
		</features>
	</xsl:template>

	<!-- Evaluate for and if conditions. -->
	<xsl:template match="rule" mode="feature">
		<xsl:param name="features" />
		<xsl:variable name="booleanFor">
			<xsl:choose>
				<xsl:when test="forAnyOf">
					<xsl:call-template name="booleanOr">
						<xsl:with-param name="booleanSequence">
							<xsl:apply-templates select="forAnyOf/*" mode="booleanFeatures">
								<xsl:with-param name="features" select="$features" />
							</xsl:apply-templates>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'true'" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="contains($booleanFor, 'true')">
			<xsl:variable name="category" select="contour/@category" />
			<xsl:choose>
				<xsl:when test="$category and $features/feature[@category = $category][@position]">
					<xsl:variable name="rule" select="." />
					<xsl:for-each select="$features/feature[@category = $category][@position]">
						<xsl:sort select="@position" />
						<xsl:call-template name="ruleIfThenElse">
							<xsl:with-param name="rule" select="$rule" />
							<xsl:with-param name="features" select="$features" />
							<xsl:with-param name="position" select="@position" />
						</xsl:call-template>
					</xsl:for-each>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="ruleIfThenElse">
						<xsl:with-param name="rule" select="." />
						<xsl:with-param name="features" select="$features" />
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="ruleIfThenElse">
		<xsl:param name="rule" />
		<xsl:param name="features" />
		<xsl:param name="position" />
		<xsl:variable name="feature" select="$rule/.." />
		<xsl:variable name="booleanIf">
			<xsl:call-template name="booleanOr">
				<xsl:with-param name="booleanSequence">
					<xsl:apply-templates select="$rule/ifAnyOf/*" mode="booleanFeatures">
						<xsl:with-param name="features" select="$features" />
						<xsl:with-param name="position" select="$position" />
					</xsl:apply-templates>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($booleanIf, 'true')">
				<xsl:apply-templates select="$rule/then/*" mode="feature">
					<xsl:with-param name="feature" select="$feature" />
					<!--
					<xsl:with-param name="position" select="$position" />
					-->
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="$rule/else/*" mode="feature">
					<xsl:with-param name="feature" select="$feature" />
					<!--
					<xsl:with-param name="position" select="$position" />
					-->
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- bivalent -->
	<xsl:template match="value" mode="feature">
		<xsl:param name="feature" />
		<xsl:param name="position" />
		<feature>
			<xsl:if test="$position">
				<xsl:attribute name="position">
					<xsl:value-of select="$position" />
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="concat(., $feature/name)" />
		</feature>
	</xsl:template>

	<!-- univalent -->
	<xsl:template match="name" mode="feature">
		<xsl:param name="feature" />
		<xsl:param name="position" />
		<feature>
			<xsl:if test="$position">
				<xsl:attribute name="position">
					<xsl:value-of select="$position" />
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="$feature/name" />
		</feature>
	</xsl:template>

</xsl:stylesheet>