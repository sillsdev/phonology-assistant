<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2a_phonetic_sort_order.xsl 2011-09-23 -->
  <!-- Classify features for phonetic sort order. -->
	<!-- Compatible with and used by CV charts. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<!-- Assume that the project inventory has descriptive feature definitions with class, category, type, and order attributes. -->
	<xsl:variable name="projectDescriptiveFeatures" select="/inventory/featureDefinitions[@class = 'descriptive']" />

	<xsl:variable name="segments" select="/inventory/segments" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Append a sorted list of unique tone features which occur in segments. -->
	<xsl:template match="segments">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
		<tones>
			<xsl:for-each select="segment[features[@class = 'descriptive']/feature[@category = 'tone']]">
				<xsl:sort select="features[@class = 'descriptive']/feature[@category = 'tone']/@order" data-type="text" />
				<xsl:variable name="feature" select="features[@class = 'descriptive']/feature[@category = 'tone']" />
				<xsl:if test="not(preceding-sibling::segment[features[@class = 'descriptive']/feature[@category = 'tone'] = $feature])">
					<xsl:copy-of select="$feature" />
				</xsl:if>
			</xsl:for-each>
		</tones>
	</xsl:template>

	<xsl:template match="segment[not(keys)]/features[not(following-sibling::features)]">
		<xsl:copy-of select="." />
		<keys>
			<xsl:call-template name="keys">
				<xsl:with-param name="segment" select=".." />
			</xsl:call-template>
		</keys>
	</xsl:template>

	<!-- In case the keys element already exists. -->
	<xsl:template match="segment/keys">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:call-template name="keys">
				<xsl:with-param name="segment" select=".." />
			</xsl:call-template>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="keys">
		<xsl:param name="segment" />
		<xsl:variable name="features" select="$segment/features[@class = 'descriptive']" />
		<!-- Include empty sortKey elements for steps 4b and 4c. -->
		<xsl:if test="not(sortKey[@class = 'manner_or_height'])">
			<sortKey class="manner_or_height" />
		</xsl:if>
		<xsl:if test="not(sortKey[@class = 'place_or_backness'])">
			<sortKey class="place_or_backness" />
		</xsl:if>
		<sortKey class="secondary" />
		<xsl:variable name="rowgroup" select="$features/feature[@class = 'rowgroup'][not(@primary = 'false')][1]" />
		<xsl:variable name="colgroup" select="$features/feature[@class = 'colgroup'][not(@primary = 'false')][1]" />
		<xsl:variable name="col" select="$features/feature[@class = 'col'][not(@primary = 'false')][1]" />
		<chartKey class="colgroup">
			<xsl:choose>
				<!-- Sort and display near-front in the front colgroup. -->
				<xsl:when test="$colgroup = 'near-front'">
					<xsl:call-template name="order_name">
						<xsl:with-param name="name">
							<xsl:call-template name="near-backness">
								<xsl:with-param name="featureA" select="'near-front'" />
								<xsl:with-param name="featureB" select="'front'" />
								<xsl:with-param name="category" select="'height'" />
							</xsl:call-template>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<!-- Sort and display near-back in the back colgroup. -->
				<xsl:when test="$colgroup = 'near-back'">
					<xsl:call-template name="order_name">
						<xsl:with-param name="name">
							<xsl:call-template name="near-backness">
								<xsl:with-param name="featureA" select="'near-back'" />
								<xsl:with-param name="featureB" select="'back'" />
								<xsl:with-param name="category" select="'height'" />
							</xsl:call-template>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="$colgroup/@order" />
					<xsl:value-of select="$colgroup" />
				</xsl:otherwise>
			</xsl:choose>
		</chartKey>
		<chartKey class="col">
			<!-- The IPA chart determines the order of column features. -->
			<xsl:attribute name="order">
				<xsl:choose>
					<xsl:when test="$col = 'voiceless' or $col = 'unrounded'">
						<xsl:value-of select="'0'" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'1'" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="$col = 'voiceless' or $col = 'unrounded' or $col = 'rounded'">
					<xsl:value-of select="$col" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'voiced'" />
				</xsl:otherwise>
			</xsl:choose>
		</chartKey>
		<chartKey class="rowgroup" order="{$rowgroup/@order}">
			<xsl:value-of select="$rowgroup" />
		</chartKey>
		<chartKey class="rowConditional">
			<!-- Sibilant (for affricate or fricative) or lateral (for click, trill, or tap/flap) or rounded (for consonant). -->
			<xsl:apply-templates select="$features/feature[@class = 'rowConditional']" mode="voicing" />
			<!-- For example, postalveolar-velar (but not labial-velar for double closure; velar or uvular for click). -->
			<xsl:apply-templates select="$features[not(feature[. = 'double closure'] or feature[. = 'click'])]/feature[@class = 'colgroup'][@primary = 'false' and not(@position)]" mode="chartKey" />
		</chartKey>
		<chartKey class="rowGeneral">
			<!-- For consonant: breathy voiced, slack voiced, stiff voiced, creaky voiced. -->
			<xsl:apply-templates select="$features/feature[@class = 'col'][@category = 'voicing'][. != 'voiceless' and . != 'voiced']" mode="voicing" />
			<!-- For example, labial-velar for double closure; velar or uvular for click. -->
			<xsl:apply-templates select="$features[feature[. = 'double closure'] or feature[. = 'click']]/feature[@class = 'colgroup'][@primary = 'false' and not(@position)]" mode="chartKey" />
			<!-- For example, nasal click; fricative trill or vowel. -->
			<xsl:apply-templates select="$features/feature[@class = 'rowgroup'][@primary = 'false' and not(@position)]" mode="chartKey" />
			<!-- Any row feature except sibilant or lateral. -->
			<xsl:apply-templates select="$features/feature[@class = 'row'][not(@primary = 'false')][not(@category = 'tone')]" mode="chartKey">
				<xsl:sort select="@order" />
			</xsl:apply-templates>
			<xsl:if test="$features/feature[@primary = 'false'][@position]">
				<diphthong>
					<xsl:apply-templates select="$features/feature[@primary = 'false'][@position]" mode="chartKey">
						<xsl:sort select="@order" />
					</xsl:apply-templates>
				</diphthong>
			</xsl:if>
		</chartKey>
		<chartKey class="tiebreaker">
			<xsl:for-each select="$segment/symbols/symbol">
				<xsl:value-of select="@code" />
			</xsl:for-each>
		</chartKey>
		<chartKey class="tone">
			<xsl:apply-templates select="$features/feature[@category = 'tone']" mode="chartKey" />
		</chartKey>
	</xsl:template>

	<xsl:template name="chartKey">
		<xsl:copy>
			<xsl:apply-templates select="@category" />
			<xsl:apply-templates select="@order" />
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="feature" mode="chartKey">
		<xsl:call-template name="chartKey" />
	</xsl:template>

	<xsl:template match="feature[@category = 'rounding_modifier']" mode="chartKey">
		<xsl:variable name="features" select=".." />
		<xsl:variable name="category" select="@category" />
		<xsl:variable name="feature" select="$features/feature[@category = $category]" />
		<xsl:variable name="boolean">
			<xsl:call-template name="booleanOr">
				<xsl:with-param name="booleanSequence">
					<xsl:if test="$feature">
						<xsl:variable name="height" select="$features/feature[@category = 'height'][not(@primary = 'true')]" />
						<xsl:variable name="backness" select="$features/feature[@category = 'backness'][not(@primary = 'true')]" />
						<xsl:variable name="rounding" select="$features/feature[@category = 'rounding'][not(@primary = 'true')]" />
						<xsl:for-each select="$segments/segment/features[@class = 'descriptive'][feature[. = $height]][feature[. = $backness]][feature[. = $rounding]]">
							<xsl:if test="not(feature[@category = $category]) or feature[@category = $category][. != $feature]">
								<xsl:value-of select="'true'" />
							</xsl:if>
						</xsl:for-each>
					</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$boolean = 'true'">
			<xsl:call-template name="chartKey" />
		</xsl:if>
	</xsl:template>

	<xsl:template match="feature" mode="voicing">
		<xsl:variable name="features" select=".." />
		<xsl:variable name="feature" select="." />
		<xsl:variable name="boolean">
			<xsl:call-template name="booleanOr">
				<xsl:with-param name="booleanSequence">
					<xsl:variable name="manner" select="$features/feature[not(@primary = 'false')][@category = 'manner']" />
					<xsl:for-each select="$projectDescriptiveFeatures/featureDefinition[@category = 'place']">
						<xsl:variable name="place" select="name" />
						<xsl:if test="$segments/segment/features[@class = 'descriptive'][feature[not(@primary = 'false')][. = $place]][feature[not(@primary = 'false')][. = $manner]][feature[. = $feature]]">
							<xsl:for-each select="$segments/segment/features[@class = 'descriptive'][feature[not(@primary = 'false')][. = $place]][feature[not(@primary = 'false')][. = $manner]][not(feature[. = $feature])][not(feature[. = 'voiceless'])]">
								<xsl:value-of select="'true'" />
							</xsl:for-each>
						</xsl:if>
					</xsl:for-each>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$boolean = 'true'">
			<xsl:call-template name="chartKey" />
		</xsl:if>
	</xsl:template>

	<xsl:template name="near-backness">
		<xsl:param name="featureA" />
		<xsl:param name="featureB" />
		<xsl:param name="category" />
		<xsl:variable name="segments" select="/inventory/segments" />
		<xsl:variable name="boolean">
			<xsl:call-template name="booleanOr">
				<xsl:with-param name="booleanSequence">
					<xsl:for-each select="$projectDescriptiveFeatures/featureDefinition[@category = $category]">
						<xsl:variable name="feature" select="name" />
						<xsl:if test="$segments/segment/features[@class = 'descriptive'][feature[. = $feature]][feature[. = $featureA]]">
							<xsl:if test="$segments/segment/features[@class = 'descriptive'][feature[. = $feature]][feature[. = $featureB]]">
								<xsl:value-of select="'true'" />
							</xsl:if>
						</xsl:if>
					</xsl:for-each>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$boolean = 'true'">
				<xsl:value-of select="$featureA" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$featureB" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="order_name">
		<xsl:param name="name" />
		<xsl:variable name="featureDefinition" select="$projectDescriptiveFeatures/featureDefinition[name = $name]" />
		<xsl:attribute name="order">
			<xsl:value-of select="$featureDefinition/@order" />
		</xsl:attribute>
		<xsl:value-of select="$name" />
	</xsl:template>

</xsl:stylesheet>