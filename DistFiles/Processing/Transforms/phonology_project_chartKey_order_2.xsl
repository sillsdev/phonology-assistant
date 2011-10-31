<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_chartKey_order_2.xsl 2011-08-05 -->
	<!-- Clear conditional key if it does not distinguish segments in any cell of a segment's rowgroup. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:include href="phonology_project_inventory_boolean_conditions.xsl" />

	<!-- Assume that the project inventory has descriptive features with an order attribute. -->
	<xsl:variable name="projectDescriptiveFeatures" select="/inventory/featureDefinitions[@class = 'descriptive']" />
	<xsl:variable name="orderFormat" select="translate(count($projectDescriptiveFeatures/featureDefinition), '0123456789', '0000000000')" />
	<xsl:variable name="orderZero" select="format-number(0, $orderFormat)" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="keys/chartKey[@class = 'rowConditional'][feature]">
		<xsl:variable name="rowConditionalOrder" select="@order" />
		<xsl:variable name="rowConditionalFeature" select="feature[1]" />
		<xsl:variable name="rowgroup" select="../chartKey[@class = 'rowgroup']" />
		<xsl:variable name="segments" select="../../.." />
		<xsl:variable name="boolean">
			<xsl:call-template name="booleanOr">
				<xsl:with-param name="booleanSequence">
					<xsl:for-each select="$projectDescriptiveFeatures/featureDefinition[@class = 'colgroup']">
						<xsl:variable name="colgroup" select="name" />
						<xsl:for-each select="$projectDescriptiveFeatures/featureDefinition[@class = 'col-row']">
							<xsl:variable name="col" select="name" />
							<xsl:for-each select="$segments/segment/keys[chartKey[@class = 'colgroup'] = $colgroup][chartKey[@class = 'rowgroup'] = $rowgroup][chartKey[@class = 'col'] = $col][chartKey[@class = 'rowConditional'][feature[. = $rowConditionalFeature]]]">
								<xsl:variable name="rowGeneralOrder" select="chartKey[@class = 'rowGeneral']/@order" />
								<xsl:choose>
									<xsl:when test="$segments/segment/keys[chartKey[@class = 'colgroup'] = $colgroup][chartKey[@class = 'rowgroup'] = $rowgroup][chartKey[@class = 'col'] = $col][chartKey[@class = 'rowConditional'][not(feature)]][chartKey[@class = 'rowGeneral']/@order = $rowGeneralOrder]">
										<xsl:value-of select="'true'" />
									</xsl:when>
									<xsl:when test="$segments/segment/keys[chartKey[@class = 'colgroup'] = $colgroup][chartKey[@class = 'rowgroup'] = $rowgroup][chartKey[@class = 'col'] = $col][chartKey[@class = 'rowConditional'][@order &lt; $rowConditionalOrder]][chartKey[@class = 'rowGeneral']/@order = $rowGeneralOrder]">
										<xsl:value-of select="'true'" />
									</xsl:when>
								</xsl:choose>
							</xsl:for-each>
						</xsl:for-each>
					</xsl:for-each>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:copy>
			<xsl:choose>
				<xsl:when test="$boolean = 'true'">
					<xsl:apply-templates select="@* | node()" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="@class" />
					<xsl:attribute name="order">
						<xsl:value-of select="$orderZero" />
					</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>