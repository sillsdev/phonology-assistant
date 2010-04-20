<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_4a_phonetic_sort_order.xsl 2010-04-20 -->
  <!-- Select features for phonetic sort order. -->
  <!-- Remove the order attribute from articulatory features. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="unit/articulatoryFeatures">
		<!-- TO DO: Can colgroup or rowgroup features can have a primary attribute at this step? I doubt it. -->
		<xsl:variable name="colgroupFeature" select="feature[@class = 'colgroup'][not(@primary = 'false')]" />
		<xsl:variable name="featureOrderFormat" select="translate($colgroupFeature/@order, '0123456789', '0000000000')" />
		<xsl:variable name="rowgroupFeature" select="feature[@class = 'rowgroup'][not(@primary = 'false')]" />
		<xsl:variable name="colFeature" select="feature[@class = 'col']" />
		<keys>
      <sortKey class="mannerOfArticulation" />
      <sortKey class="placeOfArticulation" />
      <xsl:if test="$colgroupFeature">
				<xsl:choose>
					<xsl:when test="$colgroupFeature = 'Near-front'">
						<chartKey class="colgroup" order="{format-number(number($colgroupFeature/@order) - 1, $featureOrderFormat)}">
							<xsl:value-of select="'Front'" />
						</chartKey>
					</xsl:when>
					<xsl:when test="$colgroupFeature = 'Near-back'">
						<chartKey class="colgroup" order="{format-number(number($colgroupFeature/@order) + 1, $featureOrderFormat)}">
							<xsl:value-of select="'Back'" />
						</chartKey>
					</xsl:when>
					<xsl:otherwise>
						<chartKey class="colgroup" order="{$colgroupFeature/@order}">
							<xsl:value-of select="$colgroupFeature" />
						</chartKey>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
      <xsl:if test="$rowgroupFeature">
        <chartKey class="rowgroup" order="{$rowgroupFeature/@order}">
					<xsl:value-of select="$rowgroupFeature" />
        </chartKey>
      </xsl:if>
      <xsl:if test="feature[@class = 'row']">
        <chartKeys class="row">
          <xsl:attribute name="order">
            <xsl:for-each select="feature[@class = 'row']">
							<xsl:sort select="@order" />
							<xsl:value-of select="@order" />
            </xsl:for-each>
          </xsl:attribute>
				</chartKeys>
				<xsl:for-each select="feature[@class = 'row']">
					<xsl:sort select="@order" />
					<chartKey class="row">
						<xsl:value-of select="." />
					</chartKey>
				</xsl:for-each>
			</xsl:if>
			<!-- The IPA chart determines the order of column features. -->
			<xsl:choose>
				<xsl:when test="$colFeature">
					<chartKey class="col" order="{$colFeature/@order}">
						<xsl:attribute name="order">
							<xsl:choose>
								<xsl:when test="$colFeature = 'Voiceless' or $colFeature = 'Unrounded'">
									<xsl:value-of select="0" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="$colFeature" />
					</chartKey>
				</xsl:when>
				<!-- Breathy Voiced or Creaky Voiced are in the Voiced column. -->
				<xsl:when test="feature[@subclass = 'stateOfGlottis']">
					<chartKey class="col" order="1">
						<xsl:value-of select="'Voiced'" />
					</chartKey>
				</xsl:when>
				<!-- If stateOfGlottis is unspecified for clicks, assume Voiceless. -->
				<xsl:when test="feature[. = 'Click' or . = 'Lateral Click']">
					<chartKey class="col" order="0">
						<xsl:value-of select="'Voiceless'" />
					</chartKey>
				</xsl:when>
			</xsl:choose>
		</keys>
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>