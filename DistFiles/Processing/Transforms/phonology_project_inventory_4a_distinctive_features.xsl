<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_4a_distinctive_features.xsl 2011-10-05 -->
	<!-- Assume that the project inventory has distinctive feature definitions. -->
	<!-- Remove distinctive feature rules that do not apply to this project. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<xsl:variable name="segments" select="/inventory/segments" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Remove a distinctive feature rule: -->
	<!-- * If it depends on an option that is not selected. -->
	<!-- * If a when condition fails for segments in the project phonetic inventory. -->
	<xsl:template match="featureDefinitions[@class = 'distinctive']/featureDefinition/rule">
		<xsl:variable name="booleanOption">
			<xsl:call-template name="featureOption">
				<xsl:with-param name="class" select="'distinctive'" />
				<xsl:with-param name="name" select="@featureOption" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$booleanOption != 'false'">
			<xsl:variable name="boolean">
				<xsl:variable name="forAnyOf" select="forAnyOf" />
				<xsl:call-template name="booleanAnd">
					<xsl:with-param name="booleanSequence">
						<xsl:if test="whenAnyOf">
							<xsl:variable name="whenAnyOf" select="whenAnyOf" />
							<xsl:call-template name="booleanOr">
								<xsl:with-param name="booleanSequence">
									<xsl:apply-templates select="$segments/segment" mode="segment">
										<xsl:with-param name="forAnyOf" select="$forAnyOf" />
										<xsl:with-param name="whenAnyOf" select="$whenAnyOf" />
									</xsl:apply-templates>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="whenNotAnyOf">
							<xsl:variable name="whenAnyOf" select="whenNotAnyOf" />
							<xsl:call-template name="booleanNot">
								<xsl:with-param name="boolean">
									<xsl:call-template name="booleanOr">
										<xsl:with-param name="booleanSequence">
											<xsl:apply-templates select="$segments/segment" mode="segment">
												<xsl:with-param name="forAnyOf" select="$forAnyOf" />
												<xsl:with-param name="whenAnyOf" select="$whenAnyOf" />
											</xsl:apply-templates>
										</xsl:with-param>
									</xsl:call-template>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:variable>
			<xsl:if test="$boolean = 'true'">
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<!-- Return segment has any features of the when condition -->
	<!-- but only if the segment satisfies the for condition. -->
	<xsl:template match="segment" mode="segment">
		<xsl:param name="forAnyOf" />
		<xsl:param name="whenAnyOf" />
		<xsl:variable name="features" select="features[@class = 'descriptive']" />
		<xsl:variable name="booleanFor">
			<xsl:choose>
				<xsl:when test="$forAnyOf">
					<xsl:call-template name="booleanOr">
						<xsl:with-param name="booleanSequence">
							<xsl:apply-templates select="$forAnyOf/*" mode="booleanFeatures">
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
		<xsl:if test="$booleanFor = 'true'">
			<xsl:call-template name="booleanOr">
				<xsl:with-param name="booleanSequence">
					<xsl:apply-templates select="$whenAnyOf/*" mode="booleanFeatures">
						<xsl:with-param name="features" select="$features" />
					</xsl:apply-templates>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<!-- Remove when conditions because they have been evaluated above. -->
	<xsl:template match="featureDefinitions[@class = 'distinctive']/featureDefinition/rule//whenAnyOf" />
	<xsl:template match="featureDefinitions[@class = 'distinctive']/featureDefinition/rule//whenNotAnyOf" />

	<!-- Remove contour values if the option is not selected. -->
	<xsl:template match="featureDefinitions[@class = 'distinctive']/featureDefinition/rule/contour[@featureOption]">
		<xsl:variable name="booleanOption">
			<xsl:call-template name="featureOption">
				<xsl:with-param name="class" select="'distinctive'" />
				<xsl:with-param name="name" select="@featureOption" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$booleanOption = 'true'">
			<xsl:copy-of select="." />
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>