<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2e_feature_rules.xsl 2010-05-24 -->
  <!-- Convert from articulatory to binary abd hierarchical features according to rules. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />
	
	<xsl:variable name="projectBinaryFeatures" select="/inventory/binaryFeatures" />
	<xsl:variable name="projectHierarchicalFeatures" select="/inventory/hierarchicalFeatures" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="/inventory/binaryFeatures" />
	<!--
	-->
	<xsl:template match="/inventory/hierarchicalFeatures" />

	<!-- Following articulatory features, insert binary and hierarchical features. -->
  <xsl:template match="unit/articulatoryFeatures">
		<xsl:copy-of select="." />
		<binaryFeatures>
			<xsl:apply-templates select="$projectBinaryFeatures/feature[@class]/rule[not(@class = 'UA')]" mode="feature">
				<xsl:with-param name="unit" select=".." />
			</xsl:apply-templates>
		</binaryFeatures>
		<root>
			<xsl:apply-templates select="$projectHierarchicalFeatures/feature[@parent = 'root']" mode="feature">
				<xsl:with-param name="unit" select=".." />
			</xsl:apply-templates>
		</root>
	</xsl:template>

	<xsl:template match="rule" mode="feature">
		<xsl:param name="unit" />
		<xsl:variable name="feature" select=".." />
		<xsl:variable name="booleanFor">
			<xsl:call-template name="booleanAnd">
				<xsl:with-param name="booleanSequence">
					<xsl:apply-templates select="for/*" mode="boolean">
						<xsl:with-param name="unit" select="$unit" />
					</xsl:apply-templates>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="contains($booleanFor, 'true')">
			<xsl:variable name="booleanIf">
				<xsl:call-template name="booleanAnd">
					<xsl:with-param name="booleanSequence">
						<xsl:apply-templates select="if/*" mode="boolean">
							<xsl:with-param name="unit" select="$unit" />
						</xsl:apply-templates>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="contains($booleanIf, 'true')">
					<xsl:apply-templates select="then/*" mode="feature">
						<xsl:with-param name="feature" select="$feature" />
						<xsl:with-param name="boolean" select="$booleanIf" />
						<xsl:with-param name="unit" select="$unit" />
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="else/*" mode="feature">
						<xsl:with-param name="feature" select="$feature" />
						<xsl:with-param name="boolean" select="$booleanIf" />
						<xsl:with-param name="unit" select="$unit" />
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template match="value" mode="feature">
		<xsl:param name="feature" />
		<xsl:param name="boolean" />
		<feature>
			<xsl:if test="contains($boolean, 'marked')">
				<xsl:attribute name="marked">
					<xsl:choose>
						<xsl:when test="contains($boolean, 'unmarked')">
							<xsl:value-of select="'false'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'true'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="concat(., $feature/name)" />
		</feature>
	</xsl:template>

	
	<xsl:template match="class" mode="feature">
		<xsl:param name="feature" />
		<xsl:param name="unit" />
		<xsl:call-template name="class">
			<xsl:with-param name="feature" select="$feature" />
			<xsl:with-param name="unit" select="$unit" />
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="hierarchicalFeatures/feature" mode="feature">
		<xsl:param name="unit" />
		<xsl:choose>
			<!-- If a rule did not meet its when condition and there is other rule. -->
			<xsl:when test="rule[not(*)] and not(rule[*])" />
			<xsl:when test="rule">
				<xsl:apply-templates select="rule[*]" mode="feature">
					<xsl:with-param name="unit" select="$unit" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="@class = 'nonTerminal'">
				<xsl:call-template name="class">
					<xsl:with-param name="feature" select="." />
					<xsl:with-param name="unit" select="$unit" />
				</xsl:call-template>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="class">
		<xsl:param name="feature" />
		<xsl:param name="unit" />
		<xsl:variable name="featureName" select="$feature/name" />
		<class name="{$featureName}">
			<xsl:apply-templates select="$projectHierarchicalFeatures/feature[@parent = $featureName]" mode="feature">
				<xsl:with-param name="unit" select="$unit" />
			</xsl:apply-templates>
		</class>
	</xsl:template>

</xsl:stylesheet>