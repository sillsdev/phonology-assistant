<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_2d_tone.xsl 2011-10-28 -->
  <!-- Secondary sort order for segments that differ only by tone. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="tones" select="/inventory/tones" />
	
	<xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Remove chart key patterns from the project inventory file. -->
	<xsl:template match="/inventory/chartKeyPatterns" />

	<!-- Remove sorted list of tones from the project inventory file. -->
	<xsl:template match="/inventory/tones" />

	<!-- Remove class attributes from descriptive features in the project inventory file. -->
	<!-- But not from sortKey or chartKey elements! -->
	<xsl:template match="features[starts-with(@class, 'descriptive')]/feature/@class" />

	<!-- The processing step for descriptions will: -->
	<!-- * Remove descriptive feature definitions. -->
	<!-- * Remove secondary feature elements of diphthongs. -->

	<!-- The last processing step will: -->
	<!-- * Remove order attributes from descriptive features and descendents of keys. -->
	<!-- * Remove empty chartKey elements for rows. -->

	<!-- Adjust sort keys for segments which have tone features. -->
	<xsl:template match="keys[chartKey[@class = 'tone'][feature]]/sortKey">
		<xsl:variable name="keys" select=".." />
		<xsl:variable name="segment" select="../.." />
		<xsl:variable name="orderColgroup" select="$keys/chartKey[@class = 'colgroup'][not(@primary = 'false')]/@order" />
		<xsl:variable name="orderRowgroup" select="$keys/chartKey[@class = 'rowgroup'][not(@primary = 'false')]/@order" />
		<xsl:variable name="orderCol" select="$keys/chartKey[@class = 'col']/@order" />
		<xsl:variable name="orderRowConditional" select="$keys/chartKey[@class = 'rowConditional']/@order" />
		<xsl:variable name="orderRowGeneral" select="$keys/chartKey[@class = 'rowGeneral']/@order" />
		<xsl:variable name="sortKey" select="number(.) - count($segment/preceding-sibling::segment[keys/chartKey[@class = 'colgroup'][not(@primary = 'false')]/@order = $orderColgroup and keys/chartKey[@class = 'rowgroup'][not(@primary = 'false')]/@order = $orderRowgroup and keys/chartKey[@class = 'col']/@order = $orderCol and keys/chartKey[@class = 'rowConditional']/@order = $orderRowConditional and keys/chartKey[@class = 'rowGeneral']/@order = $orderRowGeneral])" />
		<xsl:variable name="sortKeyFormat" select="translate(., '0123456789', '0000000000')" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="format-number($sortKey, $sortKeyFormat)" />
			<!--
			<xsl:value-of select="." />
			-->
		</xsl:copy>
	</xsl:template>

	<!-- Adjust or remove chart keys for tone features. -->
	<xsl:template match="keys/chartKey[@class = 'tone']">
		<xsl:variable name="feature" select="feature" />
		<xsl:if test="$feature">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:attribute name="order">
					<xsl:value-of select="$tones/feature[. = $feature]/@order" />
				</xsl:attribute>
				<xsl:apply-templates />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template match="keys/chartKey[@class = 'tone']/@order" />

	<xsl:template match="keys/sortKey[@class = 'secondary']">
		<xsl:variable name="feature" select="../chartKey[@class = 'tone']/feature" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$feature">
					<xsl:value-of select="$tones/feature[. = $feature]/@order" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'0'" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="keys/chartKey[@class = 'tiebreaker']">
		<xsl:variable name="keys" select=".." />
		<xsl:variable name="segmentPreceding" select="$keys/../preceding-sibling::*[1]" />
		<xsl:if test="$segmentPreceding">
			<xsl:variable name="keysPreceding" select="$segmentPreceding/keys" />
			<xsl:if test="$keys/chartKey[@class = 'rowgroup']/@order = $keysPreceding/chartKey[@class = 'rowgroup']/@order">
				<xsl:if test="$keys/chartKey[@class = 'colgroup']/@order = $keysPreceding/chartKey[@class = 'colgroup']/@order">
					<xsl:if test="$keys/chartKey[@class = 'col']/@order = $keysPreceding/chartKey[@class = 'col']/@order">
						<xsl:if test="$keys/chartKey[@class = 'rowConditional']/@order = $keysPreceding/chartKey[@class = 'rowConditional']/@order">
							<xsl:if test="$keys/chartKey[@class = 'rowGeneral']/@order = $keysPreceding/chartKey[@class = 'rowGeneral']/@order">
								<xsl:if test="$keys/chartKey[@class = 'tone']/@order = $keysPreceding/chartKey[@class = 'tone']/@order">
									<xsl:copy-of select="." />
								</xsl:if>
							</xsl:if>
						</xsl:if>
					</xsl:if>
				</xsl:if>
			</xsl:if>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>