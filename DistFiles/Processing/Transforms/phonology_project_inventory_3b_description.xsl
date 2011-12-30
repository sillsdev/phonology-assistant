<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<!-- phonology_project_inventory_3b_description.xsl 2011-10-06 -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<!-- Assume that the project inventory has description hyphens. -->
	<xsl:variable name="descriptionHyphens" select="/inventory/descriptionFormat/descriptionHyphens" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Remove descriptionFormat from the project inventory file. -->
	<xsl:template match="/inventory/descriptionFormat" />

	<xsl:template match="segment/description">
		<xsl:variable name="features" select="../features[@class = 'descriptive']" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="*">
				<xsl:variable name="text" select="." />
				<xsl:if test="position() != 1">
					<xsl:variable name="elementPreceding" select="preceding-sibling::*[1]" />
					<xsl:variable name="boolean">
						<xsl:apply-templates select="$descriptionHyphens/descriptionHyphen[1]" mode="descriptionHyphen">
							<xsl:with-param name="features" select="$features" />
							<xsl:with-param name="elementPreceding" select="$elementPreceding" />
							<xsl:with-param name="elementCurrent" select="." />
						</xsl:apply-templates>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$boolean = 'true'">
							<xsl:value-of select="'-'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="' '" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
				<xsl:value-of select="." />
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="descriptionHyphen" mode="descriptionHyphen">
		<xsl:param name="features" />
		<xsl:param name="elementPreceding" />
		<xsl:param name="elementCurrent" />
		<xsl:variable name="boolean">
			<xsl:call-template name="booleanAnd">
				<xsl:with-param name="booleanSequence">
					<xsl:if test="forAnyOf">
						<xsl:call-template name="booleanOr">
							<xsl:with-param name="booleanSequence">
								<xsl:apply-templates select="forAnyOf/*" mode="booleanFeatures">
									<xsl:with-param name="features" select="$features" />
								</xsl:apply-templates>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
					<xsl:apply-templates select="*[not(self::forAnyOf)][1]" mode="descriptionHyphen">
						<xsl:with-param name="element" select="$elementPreceding" />
					</xsl:apply-templates>
					<xsl:apply-templates select="*[not(self::forAnyOf)][2]" mode="descriptionHyphen">
						<xsl:with-param name="element" select="$elementCurrent" />
					</xsl:apply-templates>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$boolean = 'true'">
				<xsl:value-of select="$boolean" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="following-sibling::descriptionHyphen[1]" mode="descriptionHyphen">
					<xsl:with-param name="features" select="$features" />
					<xsl:with-param name="elementPreceding" select="$elementPreceding" />
					<xsl:with-param name="elementCurrent" select="$elementCurrent" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="feature[@category]" mode="descriptionHyphen">
		<xsl:param name="element" />
		<xsl:variable name="category" select="@category" />
		<xsl:choose>
			<xsl:when test="$element[self::feature] and $element/@category = $category">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="text" mode="descriptionHyphen">
		<xsl:param name="element" />
		<xsl:variable name="text" select="." />
		<xsl:choose>
			<xsl:when test="$element[self::text] and $element = $text">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>