<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_5a_description.xsl 2010-03-26 -->

	<xsl:variable name="programConfigurationFolder" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="//div[@id = 'metadata']/ul[@id = 'settings']/li[@class = 'programPhoneticInventoryFile']" />
	
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="descriptions" select="document($programPhoneticInventoryXML)/inventory/descriptions" />
	<xsl:variable name="programArticulatoryFeatures" select="document($programPhoneticInventoryXML)/inventory/articulatoryFeatures" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

	<!-- For each phone, select features in the correct order for its description. -->
	<xsl:template match="unit">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<description>
				<xsl:variable name="type" select="articulatoryFeatures/feature[@class = 'type']" />
				<xsl:apply-templates select="$descriptions/patterns/pattern[@type = $type]/feature" mode="description">
					<xsl:with-param name="articulatoryFeatures" select="articulatoryFeatures" />
				</xsl:apply-templates>
			</description>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Select the literal feature if the phone has it. -->
	<xsl:template match="pattern/feature" mode="description">
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="feature" select="." />
		<xsl:apply-templates select="$articulatoryFeatures/feature[. = $feature]" mode="description">
			<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
		</xsl:apply-templates>
	</xsl:template>

	<!-- Select any features that have the subclass. -->
	<xsl:template match="pattern/feature[@subclass]" mode="description">
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="subclass" select="@subclass" />
		<!-- Iterate through features in program inventory in case features of the phone are not in order. -->
		<!-- For example, Labial follows Velar instead of preceding it. -->
		<xsl:for-each select="$programArticulatoryFeatures/feature[@subclass = $subclass]">
			<xsl:variable name="feature" select="name" />
			<xsl:if test="$articulatoryFeatures/feature[. = $feature]">
				<xsl:apply-templates select="$articulatoryFeatures/feature[. = $feature]" mode="description">
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<!-- If there are any rules, apply them; otherwise just copy the feature. -->
	<xsl:template match="articulatoryFeatures/feature" mode="description">
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<xsl:when test="$descriptions/changes/change">
				<xsl:apply-templates select="$descriptions/changes/change[1]" mode="description">
					<xsl:with-param name="feature" select="." />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Process a potential change. -->
	<xsl:template match="change" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<!-- If this change has a well-formed find condition, test it. -->
			<xsl:when test="count(find) = 1 and count(find/feature) = 1">
				<xsl:apply-templates select="find/feature[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- If there are any more changes, process the next change. -->
			<xsl:when test="following-sibling::change">
				<xsl:apply-templates select="following-sibling::change[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- Because there was no change, copy the feature. -->
			<xsl:otherwise>
				<xsl:copy-of select="$feature" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<!-- find -->

	<!-- Test a find condition: feature name. -->
	<xsl:template match="find/feature" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<xsl:when test=". = $feature">
				<xsl:call-template name="findTrue">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="findFalse">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Test a find condition: feature subclass. -->
	<xsl:template match="find/feature[@subclass]" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<xsl:when test="@subclass = $feature/@subclass">
				<xsl:call-template name="findTrue">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="findFalse">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="findTrue">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<!-- If this change has a well-formed if condition, test it. -->
			<!-- count(ancestor::change[1]/if) = 1 and ancestor::change[1]/if/* -->
			<xsl:when test="count(ancestor::change[1]/if) = 1 and ancestor::change[1]/if/*">
				<xsl:apply-templates select="ancestor::change[1]/if/*[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- Replace the feature. -->
			<xsl:otherwise>
				<xsl:apply-templates select="ancestor::change[1]/replace" mode="description" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- TO DO: Rename findFalse/ifFalse as "change does not apply"? -->
	<xsl:template name="findFalse">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<!-- If there are any more rules. -->
			<xsl:when test="ancestor::change[1]/following-sibling::change">
				<xsl:apply-templates select="ancestor::change[1]/following-sibling::change[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- Because no rules applied, copy the feature. -->
			<xsl:otherwise>
				<xsl:copy-of select="$feature" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<!-- if -->

	<xsl:template name="ifTrue">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<!-- If there is another if condition, test it. -->
			<xsl:when test="following-sibling::*">
				<xsl:apply-templates select="following-sibling::*[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- Replace the feature. -->
			<xsl:otherwise>
				<xsl:apply-templates select="ancestor::change[1]/replace" mode="description" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- TO DO: Rename findFalse/ifFalse as "change does not apply"? -->
	<xsl:template name="ifFalse">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<!-- If there are any more rules. -->
			<xsl:when test="ancestor::change[1]/following-sibling::change">
				<xsl:apply-templates select="ancestor::change[1]/following-sibling::change[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- Because no rules applied, copy the feature. -->
			<xsl:otherwise>
				<xsl:copy-of select="$feature" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="if/feature" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="this" select="." />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $this]">
				<xsl:call-template name="ifTrue">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="ifFalse">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="if/feature[@subclass]" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[@subclass = $subclass]">
				<xsl:call-template name="ifTrue">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="ifFalse">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="if/feature[@subclass][@count]" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:variable name="count" select="@count" />
		<xsl:choose>
			<xsl:when test="count($articulatoryFeatures/feature[@subclass = $subclass]) = $count">
				<xsl:call-template name="ifTrue">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="ifFalse">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<!-- anyOf -->

	<xsl:template match="if/anyOf" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<!-- If there is at least one alternative, test it. -->
			<xsl:when test="*">
				<xsl:apply-templates select="*[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="anyOfTrue">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="if/anyOf/feature" mode="description">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:variable name="this" select="." />
		<xsl:choose>
			<xsl:when test="$articulatoryFeatures/feature[. = $this]">
				<xsl:call-template name="anyOfTrue">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="anyOfFalse">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="anyOfTrue">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<!-- If there is another if condition, test it. -->
			<xsl:when test="ancestor::anyOf[1]/following-sibling::*">
				<xsl:apply-templates select="ancestor::anyOf[1]/following-sibling::*[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- Replace the feature. -->
			<xsl:otherwise>
				<xsl:apply-templates select="ancestor::change[1]/replace" mode="description" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="anyOfFalse">
		<xsl:param name="feature" />
		<xsl:param name="articulatoryFeatures" />
		<xsl:choose>
			<!-- If there are any more anyOf. -->
			<xsl:when test="following-sibling::*">
				<xsl:apply-templates select="following-sibling::*[1]" mode="description">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<!-- TO DO: Rename findFalse/ifFalse as "change does not apply"? -->
				<xsl:call-template name="ifFalse">
					<xsl:with-param name="feature" select="$feature" />
					<xsl:with-param name="articulatoryFeatures" select="$articulatoryFeatures" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- replace -->

	<xsl:template match="replace" mode="description">
		<xsl:param name="feature" />
		<xsl:apply-templates mode="description">
			<xsl:with-param name="feature" select="$feature" />
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="replace/feature" mode="description">
		<xsl:param name="feature" />
		<xsl:copy-of select="$feature" />
	</xsl:template>

	<xsl:template match="replace/feature[text()]" mode="description">
		<xsl:param name="feature" />
		<xsl:copy-of select="." />
	</xsl:template>

	<xsl:template match="replace/text" mode="description">
		<xsl:param name="feature" />
		<xsl:copy-of select="." />
	</xsl:template>

</xsl:stylesheet>