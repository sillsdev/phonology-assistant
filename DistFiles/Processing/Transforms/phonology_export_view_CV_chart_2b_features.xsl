<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_2b_features.xsl 2011-08-13 -->
	<!-- Export to XHTML, Interactive Web page, and at least one feature table. -->
	<!-- Keep the features that distinguish segments in the CV chart. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Descriptive features -->
	
	<xsl:template match="xhtml:table[@class = 'descriptive features']">
		<xsl:variable name="tableCV" select="../xhtml:table[starts-with(@class, 'CV chart')]" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="descriptive">
				<xsl:with-param name="tableCV" select="$tableCV" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@* | node()" mode="descriptive">
		<xsl:param name="tableCV" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="descriptive">
				<xsl:with-param name="tableCV" select="$tableCV" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<!-- Keep a segmental descriptive feature only if it distinguishes segments. -->
	<!-- That is, at least one segment has it and at least one segment does not. -->
	<xsl:template match="xhtml:table[@class = 'descriptive features']/xhtml:tbody/xhtml:tr" mode="descriptive">
		<xsl:param name="tableCV" />
		<xsl:variable name="feature" select="xhtml:td" />
		<xsl:if test="$tableCV//xhtml:ul[@class = 'descriptive features'][xhtml:li[. = $feature]]">
      <xsl:if test="$tableCV//xhtml:ul[@class = 'descriptive features'][not(xhtml:li[. = $feature])]">
				<xsl:copy-of select="." />
			</xsl:if>
    </xsl:if>
  </xsl:template>

	<!-- Keep a suprasegmental descriptive feature if at least one segment has it. -->
	<xsl:template match="xhtml:table[@class = 'descriptive features']/xhtml:tbody[@class = 'tone']/xhtml:tr" mode="descriptive">
		<xsl:param name="tableCV" />
		<xsl:variable name="feature" select="xhtml:td" />
		<xsl:if test="$tableCV//xhtml:ul[@class = 'descriptive features'][xhtml:li[. = $feature]]">
			<xsl:copy-of select="." />
		</xsl:if>
	</xsl:template>

	<!-- Distinctive features -->

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]">
		<xsl:variable name="ancestor" select=".." />
		<xsl:variable name="positionKeyFormat" select="translate(count($ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features']), '0123456789', '0000000000')" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates mode="distinctive">
				<xsl:with-param name="ancestor" select="$ancestor" />
				<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@* | node()" mode="distinctive">
		<xsl:param name="ancestor" />
		<xsl:param name="positionKeyFormat" />
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="distinctive">
				<xsl:with-param name="ancestor" select="$ancestor" />
				<xsl:with-param name="positionKeyFormat" select="$positionKeyFormat" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<!-- Keep a univalent distinctive feature if at least one segment has it. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'univalent']" mode="distinctive">
		<xsl:param name="ancestor" />
		<xsl:variable name="feature" select="xhtml:td[@class = 'name']" />
		<xsl:if test="$ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features'][xhtml:li[. = $feature]]">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:attribute name="title">
					<xsl:apply-templates select="$ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:td[@class = 'Phonetic'][xhtml:ul[@class = 'distinctive features']/xhtml:li[. = $feature]]/xhtml:span" mode="sortKey" />
				</xsl:attribute>
				<xsl:apply-templates mode="distinctive" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<!-- Keep a bivalent distinctive feature only if at least one of its values distinguishes segments. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]/xhtml:tbody/xhtml:tr[@class = 'bivalent']" mode="distinctive">
		<xsl:param name="ancestor" />
		<xsl:param name="positionKeyFormat" />
		<xsl:variable name="featureName" select="xhtml:td[@class = 'name']" />
		<xsl:variable name="featurePlus" select="concat('+', $featureName)" />
		<!-- Important: Although cells for minus values might contain en-dash, list items have minus. -->
		<xsl:variable name="featureMinus" select="concat('-', $featureName)" />
		<xsl:if test="($ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features'][xhtml:li[. = $featurePlus]] and $ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features'][not(xhtml:li[. = $featurePlus])]) or ($ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features'][xhtml:li[. = $featureMinus]] and $ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features'][not(xhtml:li[. = $featureMinus])])">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:apply-templates select="xhtml:td">
					<xsl:with-param name="positionKeyPlus">
						<xsl:for-each select="$ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features']">
							<xsl:if test="xhtml:li[. = $featurePlus]">
								<xsl:value-of select="format-number(position(), $positionKeyFormat)" />
							</xsl:if>
						</xsl:for-each>
					</xsl:with-param>
					<xsl:with-param name="positionKeyMinus">
						<xsl:for-each select="$ancestor//xhtml:table[starts-with(@class, 'CV chart')]//xhtml:ul[@class = 'distinctive features']">
							<xsl:if test="xhtml:li[. = $featureMinus]">
								<xsl:value-of select="format-number(position(), $positionKeyFormat)" />
							</xsl:if>
						</xsl:for-each>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<!-- Add key as temporary title attribute. -->
	
	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]//xhtml:td[. = '+']">
		<xsl:param name="positionKeyPlus" />
		<xsl:copy>
			<xsl:attribute name="title">
				<xsl:value-of select="$positionKeyPlus" />
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:table[starts-with(@class, 'distinctive features')]//xhtml:td[. = '&#x2013;' or . = '-']">
		<xsl:param name="positionKeyMinus" />
		<xsl:copy>
			<xsl:attribute name="title">
				<xsl:value-of select="$positionKeyMinus" />
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>