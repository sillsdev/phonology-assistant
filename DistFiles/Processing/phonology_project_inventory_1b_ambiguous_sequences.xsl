<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_1b_ambiguous_sequences.xsl 2010-03-19 -->
  <!-- For each sequence that contains two base characters, identify the primary base. -->
	<!-- If any diacritics preceding the first base to follow it. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="ambiguousSequences" select="/inventory/ambiguousSequences" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="@*|node()" mode="preceding">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" mode="preceding" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="symbol[isBase[. = 'true']]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates />
		</xsl:copy>
		<!-- If this is the first base, move any preceding diacritics to follow it. -->
		<xsl:if test="not(preceding-sibling::symbol[isBase[. = 'true']])">
			<xsl:apply-templates select="preceding-sibling::symbol[isBase[. = 'false']]" mode="preceding" />
		</xsl:if>
	</xsl:template>

	<xsl:template match="symbol[isBase[. = 'false']][not(preceding-sibling::symbol[isBase[. = 'true']])]" />

	<!-- Add an attribute for potential conditional articulatory rules. -->
	<!-- For example, Prenasalized versus Nasal Release for Superscript M, N, and so on. -->
	<xsl:template match="symbol/isBase[. = 'false']" mode="preceding">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="precedesBase">
				<xsl:value-of select="'true'" />
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="symbol/isBase[. = 'true']">
		<xsl:param name="primaryBase" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="count(ancestor::sequence/symbol[isBase = 'true']) &gt; 1">
				<xsl:variable name="literalPhone" select="ancestor::unit/@literal" />
				<xsl:variable name="primaryBaseChar" select="$ambiguousSequences/sequence[@literal = $literalPhone][@unit = 'true']/@primaryBase" />
				<xsl:attribute name="primary">
					<xsl:choose>
						<xsl:when test="ancestor::symbol/@literal = $primaryBaseChar">
							<xsl:value-of select="'true'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'false'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>