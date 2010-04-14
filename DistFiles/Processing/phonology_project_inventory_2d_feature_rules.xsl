<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2d_feature_rules.xsl 2010-04-09 -->
  <!-- Convert from articulatory to binary abd hierarchical features according to rules. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="programBinaryFeatures" select="document($programPhoneticInventoryXML)/inventory/binaryFeatures" />
	<xsl:variable name="programHierarchicalFeatures" select="document($programPhoneticInventoryXML)/inventory/hierarchicalFeatures" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="unit/articulatoryFeatures">
		<xsl:copy-of select="." />
		<binaryFeatures>
			<xsl:apply-templates select="$programBinaryFeatures/feature[@class]/featureRule[not(@class = 'UA')]">
				<xsl:with-param name="articulatoryFeatures" select="." />
			</xsl:apply-templates>
		</binaryFeatures>
		<root>
			<xsl:apply-templates select="$programHierarchicalFeatures/feature[@parent = 'root']">
				<xsl:with-param name="articulatoryFeatures" select="." />
			</xsl:apply-templates>
		</root>
	</xsl:template>

	<xsl:template match="hierarchicalFeatures/feature">
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<xsl:when test="featureRule">
				<xsl:apply-templates select="featureRule">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="@class = 'nonTerminal'">
				<xsl:call-template name="class">
					<xsl:with-param name="name" select="name" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- TO DO: Generalize dependency between when and for according to subclass? -->
	<!--
	-->
	<xsl:template match="hierarchicalFeatures/feature[name = 'soft palate']">
		<xsl:param name="articulatoryFeatures" />
		<xsl:apply-templates select="featureRule">
			<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
		</xsl:apply-templates>
		<xsl:if test="$articulatoryFeatures/feature[. = 'Vowel'] and not($articulatoryFeatures/feature[. = 'Nasalized'])">
			<xsl:if test="$articulatoryFeatures/ancestor::units/unit[articulatoryFeatures/feature[. = 'Vowel'] and articulatoryFeatures/feature[. = 'Nasalized']]">
				<xsl:call-template name="class">
					<xsl:with-param name="name" select="name" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
		<xsl:if test="$articulatoryFeatures/feature[. = 'Consonant'] and not($articulatoryFeatures/feature[. = 'Nasalized'] or $articulatoryFeatures/feature[. = 'Prenasalized'])">
			<xsl:variable name="mannerOfArticulation" select="articulatoryFeatures/feature[@subclass = 'mannerOfArticulation']" />
			<xsl:if test="$articulatoryFeatures/ancestor::units/unit[articulatoryFeatures/feature[@subclass = $mannerOfArticulation] and ($articulatoryFeatures/feature[. = 'Nasalized'] or $articulatoryFeatures/feature[. = 'Prenasalized'])]">
				<xsl:call-template name="class">
					<xsl:with-param name="name" select="name" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="featureRule">
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<xsl:when test="for">
				<xsl:apply-templates select="for/*[1]">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
					<xsl:with-param name="featureRule" select="." />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="when">
				<xsl:call-template name="when">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
					<xsl:with-param name="featureRule" select="." />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="if">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
					<xsl:with-param name="featureRule" select="." />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="when">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="featureRule" />
		<xsl:choose>
			<xsl:when test="$featureRule/when/articulatoryFeature">
				<xsl:variable name="feature" select="$featureRule/when/articulatoryFeature[1]" />
				<xsl:choose>
					<xsl:when test="$featureRule/for/articulatoryFeature">
						<xsl:variable name="forFeature" select="$featureRule/for/articulatoryFeature" />
						<xsl:if test="$articulatoryFeatures/ancestor::inventory[1]//articulatoryFeatures[feature[. = $forFeature]]/feature[. = $feature]">
							<xsl:call-template name="if">
								<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
								<xsl:with-param name="featureRule" select="$featureRule" />
							</xsl:call-template>
						</xsl:if>
					</xsl:when>
					<xsl:otherwise>
						<xsl:if test="$articulatoryFeatures/ancestor::inventory[1]//articulatoryFeatures/feature[. = $feature]">
							<xsl:call-template name="if">
								<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
								<xsl:with-param name="featureRule" select="$featureRule" />
							</xsl:call-template>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="if">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
					<xsl:with-param name="featureRule" select="$featureRule" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="if">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="featureRule" />
		<xsl:apply-templates select="$featureRule/if/*[1]">
			<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
			<xsl:with-param name="featureRule" select="$featureRule" />
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="featureRule//anyOf">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="featureRule" />
		<xsl:variable name="anyOf">
			<xsl:apply-templates select="*[1]">
				<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="string-length($anyOf) != 0">
				<xsl:choose>
					<xsl:when test="following-sibling::*">
						<xsl:apply-templates select="following-sibling::*[1]">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="$featureRule/then">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="$featureRule/else">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="featureRule//anyOf/allOf">
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="allOfFollowingSibling" select="following-sibling::*[1]" />
		<xsl:apply-templates select="*[1]">
			<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
			<xsl:with-param name="allOfFollowingSibling" select="$allOfFollowingSibling" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- Depending on whether the last feature matches, then or else. -->
	<xsl:template match="featureRule//articulatoryFeature">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="featureRule" />
		<xsl:variable name="feature" select="." />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $feature]">
				<xsl:choose>
					<xsl:when test="following-sibling::*">
						<xsl:apply-templates select="following-sibling::*[1]">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="$featureRule/then">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="$featureRule/else">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Depending on whether the last feature matches, then or else. -->
	<xsl:template match="featureRule/if/not[articulatoryFeature]">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="featureRule" />
		<xsl:variable name="feature" select="articulatoryFeature[1]" />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $feature]">
				<xsl:apply-templates select="$featureRule/else">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="following-sibling::*">
						<xsl:apply-templates select="following-sibling::*[1]">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="$featureRule/then">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Return the first feature that does match. -->
	<xsl:template match="featureRule//anyOf/articulatoryFeature">
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="feature" select="." />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $feature]">
				<xsl:value-of select="$feature" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="following-sibling::*[1]">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Return the last feature if all of the features match. -->
	<xsl:template match="featureRule//anyOf/allOf/articulatoryFeature">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="allOfFollowingSibling" />
		<xsl:variable name="feature" select="." />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $feature]">
				<xsl:choose>
					<xsl:when test="following-sibling::*">
						<xsl:apply-templates select="following-sibling::*[1]">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="allOfFollowingSibling" select="$allOfFollowingSibling" />
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$feature" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="$allOfFollowingSibling">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- If the last feature matches, do the rule. -->
	<xsl:template match="featureRule/for/articulatoryFeature">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="featureRule" />
		<xsl:variable name="feature" select="." />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $feature]">
				<xsl:choose>
					<xsl:when test="following-sibling::*">
						<xsl:apply-templates select="following-sibling::*[1]">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="when">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- If the last feature matches, do the rule. -->
	<xsl:template match="featureRule/for/not[articulatoryFeature]">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="featureRule" />
		<xsl:variable name="feature" select="articulatoryFeature[1]" />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $feature]" />
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="following-sibling::*">
						<xsl:apply-templates select="following-sibling::*[1]">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="when">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- If the last feature matches, do the rule. -->
	<xsl:template match="featureRule/for/anyOf">
		<xsl:param name="articulatoryFeatures" />
		<xsl:param name="featureRule" />
		<xsl:variable name="anyOf">
			<xsl:apply-templates select="*[1]">
				<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="string-length($anyOf) != 0">
				<xsl:choose>
					<xsl:when test="following-sibling::*">
						<xsl:apply-templates select="following-sibling::*[1]">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="when">
							<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
							<xsl:with-param name="featureRule" select="$featureRule" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="featureRule/then">
		<xsl:param name="articulatoryFeatures" />
		<xsl:apply-templates>
			<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="featureRule/else">
		<xsl:param name="articulatoryFeatures" />
		<xsl:apply-templates>
			<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="featureRule//featureValue">
		<feature>
			<xsl:value-of select="concat(., ancestor::feature[1]/name)" />
		</feature>
	</xsl:template>

	<xsl:template match="featureRule//featureClass">
		<xsl:param name="articulatoryFeatures" />
		<xsl:call-template name="class">
			<xsl:with-param name="name" select="ancestor::feature[1]/name" />
			<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="class">
		<xsl:param name="name" />
		<xsl:param name="articulatoryFeatures" />
		<class name="{$name}">
			<xsl:apply-templates select="$programHierarchicalFeatures/feature[@parent = $name]">
				<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
			</xsl:apply-templates>
		</class>
	</xsl:template>

</xsl:stylesheet>