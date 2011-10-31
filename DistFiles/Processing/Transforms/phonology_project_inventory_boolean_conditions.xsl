<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_boolean_conditions.xsl 2011-10-05 -->
  <!-- Evaluate boolean conditions for feature rules and description changes. -->

	<!-- Is this option true? -->
	<xsl:template name="featureOption">
		<xsl:param name="class" />
		<xsl:param name="name" />
		<xsl:choose>
			<xsl:when test="string-length($name) = 0">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:when test="/inventory/featureOptions[@class = $class]/featureOption[@name = $name]/@value = 'true'">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Return whether any features in either set are not in the other set. -->
	<xsl:template name="booleanFeatureDifferences">
		<xsl:param name="featuresA" />
		<xsl:param name="featuresB" />
		<xsl:call-template name="booleanNot">
			<xsl:with-param name="boolean">
				<xsl:call-template name="booleanAnd">
					<xsl:with-param name="booleanSequence">
						<!-- Important: cannot apply-templates select=..." mode="booleanFeatures" -->
						<!-- because these features have category attributes. -->
						<xsl:for-each select="$featuresA/feature">
							<xsl:variable name="feature" select="." />
							<xsl:call-template name="booleanFeature">
								<xsl:with-param name="feature" select="$featuresB/feature[. = $feature][1]" />
							</xsl:call-template>
						</xsl:for-each>
						<xsl:for-each select="$featuresB/feature">
							<xsl:variable name="feature" select="." />
							<xsl:call-template name="booleanFeature">
								<xsl:with-param name="feature" select="$featuresA/feature[. = $feature][1]" />
							</xsl:call-template>
						</xsl:for-each>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the segment have all of these descriptive features? -->
	<xsl:template match="features" mode="booleanFeatures">
		<xsl:param name="features" />
		<xsl:call-template name="booleanAnd">
			<xsl:with-param name="booleanSequence">
				<xsl:apply-templates mode="booleanFeatures">
					<xsl:with-param name="features" select="$features" />
				</xsl:apply-templates>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the segment not match any of these descriptive features? -->
	<!-- That is, if it has at least one, return false. -->
	<xsl:template match="notAnyOf" mode="booleanFeatures">
		<xsl:param name="features" />
		<xsl:call-template name="booleanNot">
			<xsl:with-param name="boolean">
				<xsl:call-template name="booleanOr">
					<xsl:with-param name="booleanSequence">
						<xsl:apply-templates mode="booleanFeatures">
							<xsl:with-param name="features" select="$features" />
						</xsl:apply-templates>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!-- Does the segment have this feature? -->
	<xsl:template match="feature" mode="booleanFeatures">
		<xsl:param name="features" />
		<xsl:param name="position" />
		<xsl:variable name="text" select="." />
		<xsl:choose>
			<xsl:when test="$features/@class = 'distinctive' and starts-with($text, '0')">
				<xsl:variable name="featureName" select="substring($text, 2)" />
				<xsl:call-template name="booleanNot">
					<xsl:with-param name="boolean">
						<xsl:call-template name="booleanFeature">
							<xsl:with-param name="feature" select="$features/feature[substring(., 2) = $featureName][1]" />
						</xsl:call-template>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="$position">
				<xsl:call-template name="booleanFeature">
					<xsl:with-param name="feature" select="$features/feature[. = $text][@position = $position]" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="booleanFeature">
					<xsl:with-param name="feature" select="$features/feature[. = $text][1]" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Does the segment have the feature with this class? -->
	<!-- Primary base of diphthongs: class="col" or class="colgroup" or class="rowgroup" -->
	<!-- Secondary base of diphthongs: class="row" -->
	<xsl:template match="feature[@class]" mode="booleanFeatures">
		<xsl:param name="features" />
		<xsl:variable name="featureClass" select="@class" />
		<xsl:variable name="text" select="." />
		<xsl:call-template name="booleanFeature">
			<xsl:with-param name="feature" select="$features/feature[@class = $featureClass][. = $text][1]" />
		</xsl:call-template>
	</xsl:template>

	<!-- Does the segment have any feature of the category? -->
	<xsl:template match="feature[@category]" mode="booleanFeatures">
		<xsl:param name="features" />
		<xsl:variable name="category" select="@category" />
		<xsl:call-template name="booleanFeature">
			<xsl:with-param name="feature" select="$features/feature[@category = $category][1]" />
		</xsl:call-template>
	</xsl:template>

	<!-- Does the segment have this count of features for the category? -->
	<xsl:template match="feature[@count][@category]" mode="booleanFeatures">
		<xsl:param name="features" />
		<xsl:variable name="category" select="@category" />
		<xsl:variable name="count" select="@count" />
		<xsl:choose>
			<xsl:when test="count($features/feature[@category = $category]) = $count">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<!-- Return true if there is a matching feature; otherwise false. -->
	<xsl:template name="booleanFeature">
		<xsl:param name="feature" />
		<xsl:choose>
			<xsl:when test="$feature">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'false'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Return true if the sequence does not contain false; otherwise false. -->
	<xsl:template name="booleanAnd">
		<xsl:param name="booleanSequence" />
		<xsl:choose>
			<xsl:when test="contains($booleanSequence, 'false')">
				<xsl:value-of select="'false'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'true'" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Return true if the sequence contains true; otherwise false. -->
	<xsl:template name="booleanOr">
		<xsl:param name="booleanSequence" />
		<xsl:choose>
			<xsl:when test="contains($booleanSequence, 'true')">
				<xsl:value-of select="'true'" />
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