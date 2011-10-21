<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_1b_ambiguous_sequences.xsl 2011-08-02 -->
  <!-- In multiple base symbols, add primary attributes according to ambiguous sequences. -->
	<!-- To diacritic symbols which precede the first base symbol, add a precedesBase attribute. -->
	<!-- To repeated diacritic symbols, add an attribute and remove child elements. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:variable name="ambiguousSequences" select="/inventory/ambiguousSequences" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Add an attribute for base sequence patterns. -->
	<xsl:template match="symbol[@base = 'true']">
		<xsl:variable name="symbols" select=".." />
		<xsl:variable name="segment" select="../.." />
		<xsl:variable name="primary">
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="count($symbols/symbol[@base = 'true']) != 1">
				<xsl:variable name="literalSegment" select="$segment/@literal" />
				<xsl:variable name="primaryBase" select="$ambiguousSequences/sequence[@literal = $literalSegment][@unit = 'true']/@primaryBase" />
				<xsl:attribute name="primary">
					<xsl:choose>
						<xsl:when test="@literal = $primaryBase">
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

	<!-- Modify diacritic symbol if it is repeated. -->
	<xsl:template match="symbol[@base = 'false']">
		<xsl:variable name="literal" select="@literal" />
		<xsl:variable name="repeated">
			<xsl:apply-templates select="preceding-sibling::symbol[1][@base = 'false']" mode="repeated">
				<xsl:with-param name="literal" select="$literal" />
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="string-length($repeated) != 0">
					<xsl:attribute name="repeated">
						<xsl:value-of select="$repeated" />
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<!-- Add an attribute for diacritic symbols which can precede or follow the base symbol. -->
	<!-- For example, prenasalized versus nasal release for Superscript M, N, and so on. -->
	<!-- Modify diacritic symbol if it is repeated. -->
	<xsl:template match="symbol[@base = 'false'][not(preceding-sibling::symbol[@base = 'true'])]">
		<xsl:variable name="literal" select="@literal" />
		<xsl:variable name="repeated">
			<xsl:apply-templates select="preceding-sibling::symbol[1][@base = 'false']" mode="repeated">
				<xsl:with-param name="literal" select="$literal" />
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="precedesBase">
				<xsl:value-of select="'true'" />
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="string-length($repeated) != 0">
					<xsl:attribute name="repeated">
						<xsl:value-of select="$repeated" />
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<!-- Return true if a preceding sibling of a diacritic symbol has the same literal attribute value. -->
	<xsl:template match="symbol" mode="repeated">
		<xsl:param name="literal" />
		<xsl:choose>
			<xsl:when test="@literal = $literal">
				<xsl:value-of select="'true'" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="preceding-sibling::symbol[1][@base = 'false']" mode="repeated">
					<xsl:with-param name="literal" select="$literal" />
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>