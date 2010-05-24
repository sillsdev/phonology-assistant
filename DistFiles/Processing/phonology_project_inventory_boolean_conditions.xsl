<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_boolean_conditions.xsl 2010-05-24 -->
  <!-- Evaluate boolean conditions for feature rules and description changes. -->

	<!-- Does the active feature have this class? -->
	<!-- Primary base of diphthongs: class="col" -->
	<!-- Secondary base of diphthongs: class="row" -->
	<xsl:template match="this[@class]" mode="boolean">
		<xsl:param name="feature" />
		<xsl:variable name="class" select="@class" />
		<xsl:choose>
			<xsl:when test="$feature/@class = $class">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<!-- Does the unit have this feature? -->
	<xsl:template match="articulatoryFeature" mode="boolean">
		<xsl:param name="unit" />
		<xsl:variable name="text" select="." />
		<xsl:call-template name="booleanOr">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates select="$unit/articulatoryFeatures/feature[. = $text]" mode="boolean" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the unit have the feature with this class? -->
	<!-- Primary base of diphthongs: class="col" or class="colgroup" or class="rowgroup" -->
	<!-- Secondary base of diphthongs: class="row" -->
	<xsl:template match="articulatoryFeature[@class]" mode="boolean">
		<xsl:param name="unit" />
		<xsl:variable name="class" select="@class" />
		<xsl:variable name="text" select="." />
		<xsl:call-template name="booleanOr">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates select="$unit/articulatoryFeatures/feature[@class = $class][. = $text]" mode="boolean" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the unit have any feature of the subclass? -->
	<xsl:template match="articulatoryFeature[@subclass]" mode="boolean">
		<xsl:param name="unit" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:call-template name="booleanOr">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates select="$unit/articulatoryFeatures/feature[@subclass = $subclass]" mode="boolean" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the unit have a marked feature of the subclass? -->
	<xsl:template match="articulatoryFeature[@marked][@subclass]" mode="boolean">
		<xsl:param name="unit" />
		<xsl:variable name="marked" select="@marked" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:call-template name="booleanOr">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates select="$unit/articulatoryFeatures/feature[@marked = $marked][@subclass = $subclass]" mode="boolean" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the unit have any position attributes for features of the subclass? -->
	<xsl:template match="articulatoryFeature[@position = ''][@subclass]" mode="boolean">
		<xsl:param name="unit" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:call-template name="booleanOr">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates select="$unit/articulatoryFeatures/feature[@subclass = $subclass][@position]" mode="boolean" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the unit have the feature at this position? -->
	<xsl:template match="articulatoryFeature[text()][@position]" mode="boolean">
		<xsl:param name="unit" />
		<xsl:variable name="position" select="@position" />
		<xsl:variable name="text" select="." />
		<xsl:call-template name="booleanOr">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates select="$unit/articulatoryFeatures/feature[@position = $position][. = $text]" mode="boolean" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the unit have this count of features for the subclass? -->
	<xsl:template match="articulatoryFeature[@count][@subclass]" mode="boolean">
		<xsl:param name="unit" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:variable name="count" select="@count" />
		<xsl:choose>
			<xsl:when test="count($unit/articulatoryFeatures/feature[@subclass = $subclass]) = $count">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	
	<!-- Does the unit have all of these articulatory features? -->
	<xsl:template match="allOf" mode="boolean">
		<xsl:param name="unit" />
		<xsl:call-template name="booleanAnd">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates mode="boolean">
					<xsl:with-param name="unit" select="$unit" />
				</xsl:apply-templates>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the unit have at least one of these articulatory features? -->
	<xsl:template match="anyOf" mode="boolean">
		<xsl:param name="unit" />
		<xsl:call-template name="booleanOr">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates mode="boolean">
					<xsl:with-param name="unit" select="$unit" />
				</xsl:apply-templates>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the unit not match the condition? -->
	<xsl:template match="not" mode="boolean">
		<xsl:param name="unit" />
		<xsl:call-template name="booleanNot">
			<xsl:with-param name="boolean">
				<xsl:call-template name="booleanAnd">
					<xsl:with-param name="booleanSequence">
						<xsl:apply-templates mode="boolean">
							<xsl:with-param name="unit" select="$unit" />
						</xsl:apply-templates>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>


	<!-- Return true, and also marked/unmarked if the feature has a marked attribute. -->
	<xsl:template match="unit/articulatoryFeatures/feature" mode="boolean">
		<xsl:value-of select="'true'" />
		<xsl:if test="@marked">
			<xsl:choose>
				<xsl:when test="@marked = 'true'">
					<xsl:value-of select="'marked'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'unmarked'" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>


	<!-- Return true if the sequence does not contain false. -->
	<xsl:template name="booleanAnd">
		<xsl:param name="booleanSequence" />
		<xsl:choose>
			<xsl:when test="contains($booleanSequence, 'false')">
				<xsl:value-of select="'false'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'true'" />
				<xsl:if test="contains($booleanSequence, 'marked')">
					<xsl:choose>
						<xsl:when test="contains($booleanSequence, 'unmarked')">
							<xsl:value-of select="'unmarked'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'marked'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Return true if the sequence contains true. -->
	<xsl:template name="booleanOr">
		<xsl:param name="booleanSequence" />
		<xsl:choose>
			<xsl:when test="contains($booleanSequence, 'true')">
				<xsl:value-of select="'true'" />
				<xsl:if test="contains($booleanSequence, 'marked')">
					<xsl:choose>
						<xsl:when test="contains($booleanSequence, 'unmarked')">
							<xsl:value-of select="'unmarked'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'marked'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Return the opposite value. -->
	<xsl:template name="booleanNot">
		<xsl:param name="boolean" />
		<xsl:choose>
			<xsl:when test="contains($boolean, 'true')">
				<xsl:value-of select="'false'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'true'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>