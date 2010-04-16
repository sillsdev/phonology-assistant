<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_distribution_chart_generalize_2.xsl 2010-04-15 -->
	<!-- Add attribute to any data cell that has a zero value, or an individual value in a generalized chart, or both. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="xhtml:table[@class = 'distribution chart']/xhtml:tbody/xhtml:tr">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates select="xhtml:th" />
			<xsl:for-each select="xhtml:td">
				<xsl:variable name="position" select="position()" />
				<xsl:variable name="individual">
					<xsl:choose>
						<!-- If there are general environments, do not identify the data cells corresponding to general columns. -->
						<xsl:when test="ancestor::xhtml:table[1]/xhtml:thead/xhtml:tr[@class = 'individual']/xhtml:th[position() = $position][not(@class)]" />
						<!-- If there are general items, identify the remaining data cells in individual columns. -->
						<xsl:when test="../@class = 'individual'">
							<xsl:value-of select="'individual'" />
						</xsl:when>
						<!-- If there are general environments, identify the remaining data cells in individual columns for non-individual rows. -->
						<xsl:when test="ancestor::xhtml:table[1]/xhtml:thead/xhtml:tr[@class = 'individual']">
							<xsl:value-of select="'individual'" />
						</xsl:when>
						<!-- If neither items nor environments are generalized, do nothing. -->
					</xsl:choose>
				</xsl:variable>
				<!-- If the search pattern has a problem: <td class="error" title="..." /> -->
				<xsl:variable name="classErrorOrZero">
					<xsl:choose>
						<xsl:when test="@class">
							<xsl:value-of select="@class" />
						</xsl:when>
						<xsl:when test=". = '0'">
							<xsl:value-of select="'zero'" />
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="class">
					<xsl:value-of select="$classErrorOrZero" />
					<xsl:if test="string-length($individual) != 0 and string-length($classErrorOrZero) != 0">
						<xsl:value-of select="' '" />
					</xsl:if>
					<xsl:value-of select="$individual" />
				</xsl:variable>
				<xsl:copy>
					<xsl:if test="string-length($class) != 0">
						<xsl:attribute name="class">
							<xsl:value-of select="$class" />
						</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates select="@title" />
					<xsl:apply-templates />
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>