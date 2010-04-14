<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_1b.xsl 2010-04-14 -->
  <!-- Keep only the colgroup and rowgroup articulatory features for which there is at least one phone in this chart. -->
  <!-- Insert lists of sort keys for potential rows. -->
  <!-- Keep auxilliary articulatory features which distinguish at least one pair of phones. -->
  <!-- Embed names for which at least one phone does not specify the binary feature. -->
  <!-- Keep text of binary feature values that occur in at least one phone. -->
	<!-- Keep text of hierarchical feature values that occur in at least one phone. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Make sure that there are no duplicates, especially if the CV chart corresponds to a distribution chart. -->
	<xsl:template match="xhtml:ul[@class = 'CV chart']/xhtml:li">
		<xsl:variable name="literal" select="xhtml:span" />
		<xsl:if test="not(preceding-sibling::xhtml:li[xhtml:span = $literal])">
			<xsl:copy>
				<xsl:apply-templates select="@* | node()" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template match="xhtml:ul[@class = 'chart features']">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<!-- Make sure there is one item for row features (if any). -->
			<li class="row" xmlns="http://www.w3.org/1999/xhtml">
				<!-- Sort and concatenate all the keys. -->
				<xsl:for-each select="xhtml:li[@class = 'row']">
					<xsl:sort select="." />
					<xsl:value-of select="." />
				</xsl:for-each>
			</li>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'row']" />

	<xsl:template match="xhtml:ul[@class = 'colgroup features']/xhtml:li">
    <xsl:variable name="feature" select="." />
    <!-- Keep this chart heading feature only if at least one phone has it. -->
    <xsl:if test="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'colgroup'][. = $feature]]">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
      </xsl:copy>
    </xsl:if>
  </xsl:template>

  <xsl:template match="xhtml:ul[@class = 'rowgroup features']/xhtml:li">
    <xsl:variable name="feature" select="xhtml:span" />
    <!-- Keep this chart heading feature only if at least one phone has it. -->
    <xsl:if test="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'rowgroup'][. = $feature]]">
      <xsl:copy>
        <xsl:apply-templates select="@* | node()" />
				<ul xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'rowgroup'][. = $feature]]">
						<li>
							<!-- Sort and concatenate all the keys. -->
							<xsl:for-each select="xhtml:ul[@class = 'chart features']/xhtml:li[@class = 'row']">
								<xsl:sort select="." />
								<xsl:value-of select="." />
							</xsl:for-each>
						</li>
					</xsl:for-each>
				</ul>
			</xsl:copy>
    </xsl:if>
  </xsl:template>

  <xsl:template match="xhtml:table[@class = 'articulatory features']/xhtml:tbody/xhtml:tr">
    <xsl:variable name="feature" select="xhtml:td" />
    <!-- Keep this articulatory feature only if at least one phone has it, but at least one phone does not. -->
    <xsl:if test="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:div[@class = 'features']/xhtml:ul[@class = 'articulatory']/xhtml:li[. = $feature]]">
      <xsl:if test="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[not(xhtml:div[@class = 'features']/xhtml:ul[@class = 'articulatory']/xhtml:li[. = $feature])]">
        <xsl:copy>
          <xsl:apply-templates select="@* | node()" />
        </xsl:copy>
      </xsl:if>
    </xsl:if>
  </xsl:template>

	<!-- Keep this value of the binary feature only if at least one phone has it, but at least one phone does not. -->
	<xsl:template match="xhtml:table[@class = 'binary features']/xhtml:tbody/xhtml:tr/xhtml:td[@class = 'minus' or @class = 'plus']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="feature" select="concat(.,../xhtml:td[@class = 'name'])" />
			<xsl:if test="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:div[@class = 'features']/xhtml:ul[@class = 'binary']/xhtml:li[. = $feature]]">
				<xsl:if test="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[not(xhtml:div[@class = 'features']/xhtml:ul[@class = 'binary']/xhtml:li[. = $feature])]">
					<xsl:apply-templates />
				</xsl:if>
			</xsl:if>
		</xsl:copy>
  </xsl:template>

	<!-- Keep this value of the hierarchical feature only if at least one phone has it. -->
	<xsl:template match="xhtml:table[@class = 'hierarchical features']//xhtml:tbody/xhtml:tr/xhtml:td[@class = 'minus' or @class = 'plus']">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="feature" select="concat(., ../xhtml:td[@class = 'name'])" />
			<xsl:if test="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:div[@class = 'features']/xhtml:ul[@class = 'hierarchical']/xhtml:li[. = $feature]]">
				<xsl:apply-templates />
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<!-- Keep this univalent value of the hierarchical feature only if at least one phone has it. -->
	<xsl:template match="xhtml:table[@class = 'hierarchical features']//xhtml:tbody/xhtml:tr/xhtml:td[@class = 'univalent']/xhtml:div">
		<xsl:variable name="feature" select="." />
		<xsl:if test="//xhtml:body/xhtml:ul[@class = 'CV chart']/xhtml:li[xhtml:div[@class = 'features']/xhtml:ul[@class = 'hierarchical']/xhtml:li[. = $feature]]">
			<xsl:copy-of select="." />
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>