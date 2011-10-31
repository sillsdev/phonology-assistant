<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_3a_description.xsl 2011-10-06 -->
	<!-- Segments get descriptive names from descriptive features according to patterns and changes. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="descriptionFormat" select="document($programPhoneticInventoryXML)/inventory/descriptionFormat" />

	<!-- Assume that the project inventory has descriptive feature definitions with class, category, and order attributes. -->
	<xsl:variable name="projectDescriptiveFeatures" select="/inventory/featureDefinitions[@class = 'descriptive']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Remove descriptive feature definitions from the project inventory file. -->
	<xsl:template match="/inventory/featureDefinitions[@class = 'descriptive']" />

	<!-- Remove secondary feature elements of diphthongs. -->
	<xsl:template match="feature[@position][@primary = 'false']" />
	<xsl:template match="feature[@position][@primary = 'true']/@position" />
	<xsl:template match="feature[@position][@primary = 'true']/@primary" />

	<!-- For the next step, add rules for hyphens in descriptions. -->
	<xsl:template match="/inventory">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<descriptionFormat>
				<xsl:copy-of select="$descriptionFormat/descriptionHyphens" />
			</descriptionFormat>
		</xsl:copy>
	</xsl:template>

	<!-- For each segment, select features in the correct order for its description. -->
	<xsl:template match="segment/features[@class = 'descriptive']">
		<description>
			<xsl:call-template name="descriptionPatterns">
				<xsl:with-param name="descriptionPattern" select="$descriptionFormat/descriptionPatterns/descriptionPattern[1]" />
				<xsl:with-param name="features" select="." />
			</xsl:call-template>
		</description>
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Use the first description pattern that matches the features of the segment. -->
	<xsl:template name="descriptionPatterns">
		<xsl:param name="descriptionPattern" />
		<xsl:param name="features" />
		<xsl:if test="$descriptionPattern">
			<xsl:variable name="boolean">
				<xsl:call-template name="booleanAnd">
					<xsl:with-param name="booleanSequence">
						<xsl:apply-templates select="$descriptionPattern/if/*" mode="booleanFeatures">
							<xsl:with-param name="features" select="$features" />
						</xsl:apply-templates>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$boolean = 'true'">
					<xsl:apply-templates select="$descriptionPattern/then/*" mode="description">
						<xsl:with-param name="features" select="$features" />
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="descriptionPatterns">
						<xsl:with-param name="descriptionPattern" select="$descriptionPattern/following-sibling::descriptionPattern[1]" />
						<xsl:with-param name="features" select="$features" />
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Select the literal feature if the segment has it. -->
	<xsl:template match="descriptionPattern/then/feature" mode="description">
		<xsl:param name="features" />
		<xsl:variable name="text" select="." />
		<xsl:apply-templates select="$features/feature[. = $text]" mode="description">
			<xsl:with-param name="features" select="$features" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- Select any features that have the category. -->
	<xsl:template match="descriptionPattern/then/feature[@category]" mode="description">
		<xsl:param name="features" />
		<xsl:variable name="category" select="@category" />
		<xsl:choose>
			<!-- Regardless of which is primary, in order by place (for example, labial-veiar, coronal-velar). -->
			<xsl:when test="@category = 'place'">
				<xsl:for-each select="$projectDescriptiveFeatures/featureDefinition[@category = $category]">
					<xsl:variable name="name" select="name" />
					<xsl:apply-templates select="$features/feature[. = $name]" mode="description">
						<xsl:with-param name="features" select="$features" />
					</xsl:apply-templates>
				</xsl:for-each>
			</xsl:when>
			<!-- Any non-primary precedes the primary feature (for example, nasal click). -->
			<xsl:otherwise>
				<xsl:apply-templates select="$features/feature[@category = $category][@primary = 'false']" mode="description">
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
				<xsl:apply-templates select="$features/feature[@category = $category][not(@primary = 'false')]" mode="description">
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Select any features that have the category. -->
	<!-- In a diphthong, order by position (not by primary/non-primary). -->
	<xsl:template match="descriptionPattern/then/feature[@category][@position]" mode="description">
		<xsl:param name="features" />
		<xsl:variable name="category" select="@category" />
		<xsl:variable name="position" select="@position" />
		<xsl:apply-templates select="$features/feature[@category = $category][@position = $position]" mode="description">
			<xsl:with-param name="features" select="$features" />
		</xsl:apply-templates>
	</xsl:template>
	
	<!-- Copy the text (for example, to in the description of a diphthong). -->
	<xsl:template match="descriptionPattern/then/text" mode="description">
		<xsl:copy-of select="." />
	</xsl:template>

	<!-- If there are any changes, apply them; otherwise just copy the feature. -->
	<xsl:template match="features[@class = 'descriptive']/feature" mode="description">
		<xsl:param name="features" />
		<xsl:choose>
			<xsl:when test="$descriptionFormat/descriptionChanges/descriptionChange">
				<xsl:apply-templates select="$descriptionFormat/descriptionChanges/descriptionChange[1]" mode="description">
					<xsl:with-param name="feature" select="." />
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Make a change if it applies to this feature of the segment. -->
	<xsl:template match="descriptionChange" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="features" />
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
			<xsl:choose>
				<xsl:when test="contains($booleanFind, 'true') and forAnyOf">
					<xsl:call-template name="booleanOr">
						<xsl:with-param name="booleanSequence">
							<xsl:apply-templates select="forAnyOf/*" mode="booleanFeatures">
								<xsl:with-param name="features" select="$features" />
							</xsl:apply-templates>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$booleanFind" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<!-- This change applies, so make it. -->
			<xsl:when test="contains($boolean, 'true')">
				<xsl:apply-templates select="replace/*" mode="replace">
					<xsl:with-param name="feature" select="$feature" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- If there are any more changes, go to the next change. -->
			<xsl:when test="following-sibling::descriptionChange">
				<xsl:apply-templates select="following-sibling::descriptionChange[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="features" select="$features" />
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
	<xsl:template match="find/feature" mode="find">
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

	<!-- Does this feature have the category? -->
	<xsl:template match="find/feature[@category]" mode="find">
		<xsl:param name="feature" />
		<xsl:variable name="category" select="@category" />
		<xsl:choose>
			<xsl:when test="$feature/@category = $category">
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