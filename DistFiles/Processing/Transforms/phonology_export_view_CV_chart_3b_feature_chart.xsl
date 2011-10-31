<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

	<!-- phonology_export_view_CV_chart_3b_feature_chart.xsl 2011-10-21 -->
	<!-- If two CV charts share the distinctive features table, -->
	<!-- indicate features that are redundant for the type of segments in the feature chart. -->
	<!-- This logic is similar to step 2c. -->
	<!-- If a feature value is hyphen-minus, replace with en dash. Do not change +- or -+ contour values.  -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />

	<!-- A project phonetic inventory file contains features of phonetic or phonological segments, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />

	<!-- The program phonetic character inventory file contains the features, symbols, and so on. -->
	<xsl:variable name="programConfigurationFolder" select="$settings/xhtml:li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programDistinctiveFeaturesFile" select="concat($settings/xhtml:li[@class = 'programDistinctiveFeaturesName'], '.DistinctiveFeatures.xml')" />
	<xsl:variable name="programDistinctiveFeaturesXML" select="concat($programConfigurationFolder, $programDistinctiveFeaturesFile)" />
	<xsl:variable name="programDistinctiveFeatures" select="document($programDistinctiveFeaturesXML)/inventory/featureDefinitions[@class = 'distinctive']" />

	<xsl:variable name="enDash" select="'&#x2013;'" />
	<xsl:variable name="middleDot" select="'&#x00B7;'" />
	<xsl:variable name="bullet" select="'&#x2022;'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive-descriptive values chart')]">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
		<xsl:if test="xhtml:tbody/xhtml:tr/xhtml:td[contains(., $middleDot)]">
			<p xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="concat('A cell contains a ', $middleDot, '&#xA0;dot if the distinctive feature is unspecified for some (but not all) segments which have the descriptive feature.')" />
			</p>
		</xsl:if>
	</xsl:template>

	<!-- If the previous step added a temporary title attribute to a redundant feature, remove it. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive-segment values chart') or starts-with(@class, 'distinctive-descriptive values chart')]/xhtml:tbody/xhtml:tr/@title" />
	
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive-segment values chart') or starts-with(@class, 'distinctive-descriptive values chart')]/xhtml:tbody/xhtml:tr[not(@class = 'redundant')][@title]">
		<xsl:variable name="nameThis" select="xhtml:th" />
		<xsl:variable name="nameThat" select="$programDistinctiveFeatures/featureDefinition[name = $nameThis]/followingRelationship/@name" />
		<xsl:variable name="table" select="../.." />
		<xsl:variable name="redundant">
			<xsl:choose>
				<xsl:when test="$nameThat and $table/xhtml:tbody/xhtml:tr[xhtml:th = $nameThat][@title]">
					<xsl:call-template name="redundant">
						<xsl:with-param name="positionKeyThis" select="@title" />
						<xsl:with-param name="positionKeyThat" select="$table/xhtml:tbody/xhtml:tr[xhtml:th = $nameThat]/@title" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="preceding-sibling::xhtml:tr[@title][1]" mode="redundant">
						<xsl:with-param name="nameThis" select="$nameThis" />
						<xsl:with-param name="positionKeyThis" select="@title" />
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy>
			<xsl:if test="$redundant = 'true'">
				<xsl:attribute name="class">
					<xsl:value-of select="'redundant'" />
				</xsl:attribute>
			</xsl:if>
			<!-- The previous step added a temporary title attribute, which is omitted here. -->
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Return whether the feature is redundant with a preceding sibling. -->
	<xsl:template match="xhtml:tr[@title]" mode="redundant">
		<xsl:param name="nameThis" />
		<xsl:param name="positionKeyThis" />
		<xsl:variable name="nameThat" select="xhtml:th" />
		<xsl:variable name="redundant">
			<xsl:if test="not($programDistinctiveFeatures/featureDefinition[name = $nameThat]/followingRelationship/@name = $nameThis)">
				<xsl:call-template name="redundant">
					<xsl:with-param name="positionKeyThis" select="$positionKeyThis" />
					<xsl:with-param name="positionKeyThat" select="@title" />
				</xsl:call-template>
			</xsl:if>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$redundant = 'true'">
				<xsl:value-of select="$redundant" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="preceding-sibling::xhtml:tr[@title][1]" mode="redundant">
					<xsl:with-param name="nameThis" select="$nameThis" />
					<xsl:with-param name="positionKeyThis" select="$positionKeyThis" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Return true if either: -->
	<!-- * segments have the same positive and negative values for the two features -->
	<!-- * segments have the opposite positive and negative values for the two features -->
	<xsl:template name="redundant">
		<xsl:param name="positionKeyThis" />
		<xsl:param name="positionKeyThat" />
		<xsl:choose>
			<xsl:when test="$positionKeyThis = $positionKeyThat">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:when test="substring-before($positionKeyThis, ' ') = substring-after($positionKeyThat, ' ') and substring-after($positionKeyThis, ' ') = substring-before($positionKeyThat, ' ')">
				<xsl:value-of select="'true'" />
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- If the value is hyphen-minus, replace with en dash. However, do not change +- or -+ contour values.  -->
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive-segment values chart') or starts-with(@class, 'distinctive-descriptive values chart')]//xhtml:td/text()[. = '-']">
		<xsl:value-of select="$enDash" />
	</xsl:template>

</xsl:stylesheet>