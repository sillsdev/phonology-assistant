<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_4a_phonetic_sort_order.xsl 2010-03-24 -->
  <!-- Select features for phonetic sort order. -->
  <!-- Remove the order attribute from articulatory features. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="unit/articulatoryFeatures">
		<xsl:variable name="colgroupFeature" select="feature[@class = 'colgroup'][1]" />
		<xsl:variable name="featureOrderFormat" select="translate($colgroupFeature/@order, '0123456789', '0000000000')" />
		<keys>
      <sortKey class="mannerOfArticulation" />
      <sortKey class="placeOfArticulation" />
      <xsl:if test="$colgroupFeature">
				<xsl:choose>
					<xsl:when test="$colgroupFeature = 'Near-front'">
						<chartKey class="colgroup" title="Front">
							<xsl:value-of select="format-number(number($colgroupFeature/@order) - 1, $featureOrderFormat)" />
						</chartKey>
					</xsl:when>
					<xsl:when test="$colgroupFeature = 'Near-back'">
						<chartKey class="colgroup" title="Back">
							<xsl:value-of select="format-number(number($colgroupFeature/@order) + 1, $featureOrderFormat)" />
						</chartKey>
					</xsl:when>
					<xsl:otherwise>
						<chartKey class="colgroup" title="{$colgroupFeature}">
							<xsl:value-of select="$colgroupFeature/@order" />
						</chartKey>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
      <xsl:if test="feature[@class = 'rowgroup']">
        <chartKey class="rowgroup" title="{feature[@class = 'rowgroup'][1]}">
          <xsl:value-of select="feature[@class = 'rowgroup'][1]/@order" />
        </chartKey>
      </xsl:if>
      <xsl:if test="feature[@class = 'row']">
        <chartKey class="row">
          <xsl:attribute name="title">
            <xsl:for-each select="feature[@class = 'row']">
              <xsl:if test="position() != 1">
                <xsl:value-of select="','" />
              </xsl:if>
              <xsl:value-of select="." />
            </xsl:for-each>
          </xsl:attribute>
          <xsl:for-each select="feature[@class = 'row']">
            <xsl:value-of select="@order" />
          </xsl:for-each>
        </chartKey>
      </xsl:if>
			<xsl:choose>
				<xsl:when test="feature[@class = 'col']">
					<chartKey class="col" title="{feature[@class = 'col'][1]}">
						<xsl:value-of select="feature[@class = 'col'][1]/@order" />
					</chartKey>
				</xsl:when>
				<xsl:when test="feature[@subclass = 'stateOfGlottis']">
					<chartKey class="col" title="Voiced">
						<xsl:value-of select="ancestor::units[1]/unit/articulatoryFeatures/feature[. = 'Voiced']/@order" />
					</chartKey>
				</xsl:when>
			</xsl:choose>
		</keys>
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>