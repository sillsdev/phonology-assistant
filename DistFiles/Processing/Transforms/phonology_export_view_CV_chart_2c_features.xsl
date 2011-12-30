<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_2c_features.xsl 2011-10-21 -->
	<!-- Remove empty tbody elements (that is, if they contains no features that distinguish segments). -->
	<!-- Indicate features which distinguish segments but are redundant with other features. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />

	<!-- The program phonetic character inventory file contains the features, symbols, and so on. -->
	<xsl:variable name="programConfigurationFolder" select="$settings/xhtml:li[@class = 'programConfigurationFolder']" />

	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />

	<xsl:variable name="programDistinctiveFeaturesFile" select="concat($settings/xhtml:li[@class = 'programDistinctiveFeaturesName'], '.DistinctiveFeatures.xml')" />
	<xsl:variable name="programDistinctiveFeaturesXML" select="concat($programConfigurationFolder, $programDistinctiveFeaturesFile)" />
	<xsl:variable name="programDistinctiveFeatures" select="document($programDistinctiveFeaturesXML)/inventory/featureDefinitions[@class = 'distinctive']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Remove empty tbody elements (that is, if they contains no features that distinguish segments). -->
	<xsl:template match="xhtml:table[contains(@class, 'features')]/xhtml:tbody[not(xhtml:tr)]" />

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'univalent']">
		<xsl:variable name="colspan" select="xhtml:td[1]/@colspan" />
		<xsl:variable name="ancestor" select="../../.." />
		<xsl:variable name="feature" select="xhtml:td[@class = 'name']" />
		<xsl:if test="following-sibling::xhtml:tr[1][@class = 'bivalent'] or following-sibling::xhtml:tr[1]/xhtml:td[1]/@colspan != $colspan or ($ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features'][xhtml:li[. = $feature]] and $ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features'][not(xhtml:li[. = $feature])])">
			<xsl:copy>
				<xsl:apply-templates select="@* | node()" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>
	
	<!-- Indicate bivalent distinctive features that are redundant. -->

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'bivalent']">
		<xsl:variable name="nameThis" select="xhtml:td[@class = 'name']" />
		<xsl:variable name="positionKeyPlusThis" select="xhtml:td[. = '+']/@title" />
		<xsl:variable name="positionKeyMinusThis" select="xhtml:td[. = '&#x2013;' or . = '-']/@title" />
		<xsl:variable name="trUnivalent" select="preceding-sibling::xhtml:tr[@class = 'univalent'][1]" />
		<xsl:variable name="redundantWithUnivalent">
			<xsl:if test="$trUnivalent">
				<xsl:variable name="univalent" select="$trUnivalent/@title" />
				<xsl:choose>
					<xsl:when test="$positionKeyPlusThis = $univalent and $positionKeyMinusThis = $univalent">
						<xsl:value-of select="'true'" />
					</xsl:when>
					<xsl:when test="$positionKeyPlusThis = $univalent and string-length($positionKeyMinusThis) = 0">
						<xsl:value-of select="'true'" />
					</xsl:when>
					<xsl:when test="$positionKeyMinusThis = $univalent and string-length($positionKeyPlusThis) = 0">
						<xsl:value-of select="'true'" />
					</xsl:when>
				</xsl:choose>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="redundant">
			<xsl:choose>
				<xsl:when test="$redundantWithUnivalent = 'true'">
					<xsl:value-of select="$redundantWithUnivalent" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="followingRelationship" select="$programDistinctiveFeatures/featureDefinition[name = $nameThis]/followingRelationship[@name]" />
					<xsl:choose>
						<xsl:when test="$followingRelationship">
							<xsl:variable name="nameThat" select="$followingRelationship/@name" />
							<xsl:variable name="table" select="../.." />
							<xsl:call-template name="redundant">
								<xsl:with-param name="tr" select="$table/xhtml:tbody/xhtml:tr[@class = 'bivalent'][xhtml:td[@class = 'name'] = $nameThat]" />
								<xsl:with-param name="positionKeyPlusThis" select="$positionKeyPlusThis" />
								<xsl:with-param name="positionKeyMinusThis" select="$positionKeyMinusThis" />
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="preceding-sibling::xhtml:tr[@class = 'bivalent'][1]" mode="redundant">
								<xsl:with-param name="nameThis" select="$nameThis" />
								<xsl:with-param name="positionKeyPlusThis" select="$positionKeyPlusThis" />
								<xsl:with-param name="positionKeyMinusThis" select="$positionKeyMinusThis" />
							</xsl:apply-templates>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@* | node()">
				<xsl:with-param name="redundant" select="$redundant" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'bivalent']/@class">
		<xsl:param name="redundant" />
		<xsl:choose>
			<xsl:when test="$redundant = 'true'">
				<xsl:attribute name="class">
					<xsl:value-of select="." />
					<xsl:value-of select="' redundant'" />
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="xhtml:tr" mode="redundant" />

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'bivalent']" mode="redundant">
		<xsl:param name="nameThis" />
		<xsl:param name="positionKeyPlusThis" />
		<xsl:param name="positionKeyMinusThis" />
		<xsl:variable name="nameThat" select="xhtml:td[@class = 'name']" />
		<xsl:variable name="redundant">
			<xsl:if test="not($programDistinctiveFeatures/featureDefinition[name = $nameThat]/followingRelationship/@name = $nameThis)">
				<xsl:call-template name="redundant">
					<xsl:with-param name="tr" select="." />
					<xsl:with-param name="positionKeyPlusThis" select="$positionKeyPlusThis" />
					<xsl:with-param name="positionKeyMinusThis" select="$positionKeyMinusThis" />
				</xsl:call-template>
			</xsl:if>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$redundant = 'true'">
				<xsl:value-of select="$redundant" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="preceding-sibling::xhtml:tr[@class = 'bivalent'][1]" mode="redundant">
					<xsl:with-param name="nameThis" select="$nameThis" />
					<xsl:with-param name="positionKeyPlusThis" select="$positionKeyPlusThis" />
					<xsl:with-param name="positionKeyMinusThis" select="$positionKeyMinusThis" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="redundant">
		<xsl:param name="tr" />
		<xsl:param name="positionKeyPlusThis" />
		<xsl:param name="positionKeyMinusThis" />
		<xsl:variable name="positionKeyPlusThat" select="$tr/xhtml:td[. = '+']/@title" />
		<xsl:variable name="positionKeyMinusThat" select="$tr/xhtml:td[. = '&#x2013;' or . = '-']/@title" />
		<xsl:choose>
			<xsl:when test="$positionKeyPlusThis = $positionKeyPlusThat and $positionKeyMinusThis = $positionKeyMinusThat">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:when test="$positionKeyPlusThis = $positionKeyMinusThat and $positionKeyMinusThis = $positionKeyPlusThat">
				<xsl:value-of select="'true'" />
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- Remove contents of table cells for distinctive feature values which do not occur. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'bivalent']/xhtml:td[. = '+' or . = '&#x2013;' or . = '-'][string-length(@title) = 0]">
		<xsl:copy />
	</xsl:template>

	<!-- Remove title attributes. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'univalent']/@title" />
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'bivalent']/xhtml:td[. = '+' or . = '&#x2013;' or . = '-']/@title" />

</xsl:stylesheet>