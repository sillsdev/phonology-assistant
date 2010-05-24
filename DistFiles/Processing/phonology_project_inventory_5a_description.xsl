<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_5a_description.xsl 2010-05-24 -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="programArticulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />
	<xsl:variable name="descriptions" select="document($programPhoneticInventoryXML)/inventory/descriptions" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- For each phone, select features in the correct order for its description. -->
	<xsl:template match="unit">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<description>
				<xsl:variable name="type" select="articulatoryFeatures/feature[@class = 'type']" />
				<xsl:apply-templates select="$descriptions/patterns/pattern[@type = $type]/*" mode="description">
					<xsl:with-param name="unit" select="." />
				</xsl:apply-templates>
			</description>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Select the literal feature if the phone has it. -->
	<xsl:template match="pattern/articulatoryFeature" mode="description">
		<xsl:param name="unit" />
		<xsl:variable name="text" select="." />
		<xsl:apply-templates select="$unit/articulatoryFeatures/feature[. = $text]" mode="description">
			<xsl:with-param name="unit" select="$unit" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- Select any features that have the subclass. -->
	<xsl:template match="pattern/articulatoryFeature[@subclass]" mode="description">
		<xsl:param name="unit" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:choose>
			<xsl:when test="@subclass = 'placeOfArticulation'">
				<!-- Regardless of which is primary, in order by place (for example, labial-veiar, corono-velar). -->
				<xsl:for-each select="$programArticulatoryFeatures/feature[@subclass = $subclass]">
					<xsl:variable name="name" select="name" />
					<xsl:apply-templates select="$unit/articulatoryFeatures/feature[. = $name]" mode="description">
						<xsl:with-param name="unit" select="$unit" />
					</xsl:apply-templates>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<!-- Any non-primary precedes the primary feature (for example, nasal click). -->
				<xsl:apply-templates select="$unit/articulatoryFeatures/feature[@subclass = $subclass][@primary = 'false']" mode="description">
					<xsl:with-param name="unit" select="$unit" />
				</xsl:apply-templates>
				<xsl:apply-templates select="$unit/articulatoryFeatures/feature[@subclass = $subclass][not(@primary = 'false')]" mode="description">
					<!-- In a diphthong, order by position (not by primary/non-primary). -->
					<xsl:sort select="@position" data-type="number" />
					<xsl:with-param name="unit" select="$unit" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- If there are any changes, apply them; otherwise just copy the feature. -->
	<xsl:template match="unit/articulatoryFeatures/feature" mode="description">
		<xsl:param name="unit" />
		<xsl:choose>
			<xsl:when test="$descriptions/changes/change">
				<xsl:apply-templates select="$descriptions/changes/change[1]" mode="description">
					<xsl:with-param name="feature" select="." />
					<xsl:with-param name="unit" select="$unit" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Make a change if it applies to this feature of the unit. -->
	<xsl:template match="change" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="unit" />
		<xsl:variable name="booleanFind">
			<xsl:call-template name="booleanAnd">
				<xsl:with-param name="booleanSequence">
					<xsl:apply-templates select="find/*" mode="find">
						<xsl:with-param name="feature" select="$feature" />
					</xsl:apply-templates>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="boolean">
			<xsl:call-template name="booleanAnd">
				<xsl:with-param name="booleanSequence">
					<xsl:value-of select="$booleanFind" />
					<xsl:if test="contains($booleanFind, 'true') and for">
						<xsl:apply-templates select="for/*" mode="boolean">
							<xsl:with-param name="unit" select="$unit" />
						</xsl:apply-templates>
					</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:choose>
			<!-- This change applies, so make it. -->
			<xsl:when test="contains($boolean, 'true')">
				<xsl:apply-templates select="replace/*" mode="replace">
					<xsl:with-param name="feature" select="$feature" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- If there are any more changes, go to the next change. -->
			<xsl:when test="following-sibling::change">
				<xsl:apply-templates select="following-sibling::change[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="unit" select="$unit" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- Because no change applies, copy the feature. -->
			<xsl:otherwise>
				<xsl:copy-of select="$feature" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- find -->

	<!-- Is this the feature? -->
	<xsl:template match="find/articulatoryFeature" mode="find">
		<xsl:param name="feature" />
		<xsl:variable name="text" select="." />
		<xsl:choose>
			<xsl:when test="$feature = $text">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Does this feature have the subclass? -->
	<xsl:template match="find/articulatoryFeature[@subclass]" mode="find">
		<xsl:param name="feature" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:choose>
			<xsl:when test="$feature/@subclass = $subclass">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- replace -->

	<xsl:template match="replace/feature[not(node())]" mode="replace">
		<xsl:param name="feature" />
		<xsl:copy-of select="$feature" />
	</xsl:template>

	<xsl:template match="replace/feature[text()]" mode="replace">
		<xsl:copy-of select="." />
	</xsl:template>

	<xsl:template match="replace/text" mode="replace">
		<xsl:copy-of select="." />
	</xsl:template>

</xsl:stylesheet>