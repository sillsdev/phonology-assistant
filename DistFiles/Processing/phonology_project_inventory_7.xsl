<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_7.xsl 2011-10-19 -->
	<!-- Temporarily add units for Phonology Assistant 3.3.2 or earlier. -->
  <!-- Remove temporary attributes and elements. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="@* | node()" mode="unit">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="unit" />
		</xsl:copy>
	</xsl:template>

	<!--
	<xsl:template match="div[@id = 'metadata']" />
	-->

	<!-- Remove order attributes from descriptive features and descendents of keys in the project inventory file. -->
	<xsl:template match="features[starts-with(@class, 'descriptive')]/feature/@order" />
	<xsl:template match="keys//*/@order" />

	<!-- Remove empty chartKey elements for rows. -->
	<xsl:template match="keys//chartKey[@class = 'rowConditional'][not(feature)]" />
	<xsl:template match="keys//chartKey[@class = 'rowGeneral'][not(feature)]" />

	<!--
	<xsl:template match="similarPhonePairs[not(similarPhonePair)]" />
	-->

	<xsl:template match="segment[@literal][not(@literalSegment)]">
		<xsl:variable name="literal" select="@literal" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="../segment[@literalSegment = $literal]">
				<!-- Add attribute if this is an unmarked segment corresponding to segments with tone accent. -->
				<xsl:attribute name="literalSegment">
					<xsl:value-of select="$literal" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	
	<!--
	<xsl:template match="/inventory/segments">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
		<units>
			<xsl:apply-templates select="@* | node()" mode="unit" />
		</units>
	</xsl:template>

	<xsl:template match="segment" mode="unit">
		<unit>
			<xsl:apply-templates select="@* | node()" mode="unit" />
		</unit>
	</xsl:template>

	<xsl:template match="description" mode="unit" />

	<xsl:template match="features[@class = 'descriptive']" mode="unit">
		<articulatoryFeatures>
			<xsl:apply-templates mode="unit" />
		</articulatoryFeatures>
	</xsl:template>

	<xsl:template match="features[@class = 'descriptive']/feature" mode="unit">
		<xsl:copy>
			<xsl:apply-templates mode="unit" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="features[@class = 'descriptive default']" mode="unit" />

	<xsl:template match="features[@class = 'distinctive']" mode="unit">
		<binaryFeatures>
			<xsl:apply-templates mode="unit" />
		</binaryFeatures>
	</xsl:template>

	<xsl:template match="features[@class = 'distinctive default']" mode="unit" />

	<xsl:template match="symbols" mode="unit" />

	<xsl:template match="keys" mode="unit">
		<xsl:copy>
			<sortKey class="mannerOfArticulation">
				<xsl:value-of select="sortKey[@class = 'manner_or_height']" />
			</sortKey>
			<sortKey class="placeOfArticulation">
				<xsl:value-of select="sortKey[@class = 'place_or_backness']" />
			</sortKey>
		</xsl:copy>
	</xsl:template>
	-->

</xsl:stylesheet>