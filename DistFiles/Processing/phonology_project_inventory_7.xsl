<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_7.xsl 2010-04-09 -->
  <!-- Remove temporary attributes and elements. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="div[@id = 'metadata']" />

	<xsl:template match="feature/@type" />
	<xsl:template match="feature/@order" />

	<xsl:template match="keys/chartKey" />

  <xsl:template match="articulatoryFeatureChanges[add[not(feature)] and remove[not(feature)]]" />
	<xsl:template match="articulatoryFeatureChanges/add[not(feature)]" />
	<xsl:template match="articulatoryFeatureChanges/remove[not(feature)]" />

	<xsl:template match="binaryFeatureChanges[add[not(feature)] and remove[not(feature)]]" />
	<xsl:template match="binaryFeatureChanges/add[not(feature)]" />
	<xsl:template match="binaryFeatureChanges/remove[not(feature)]" />

	<!--
	<xsl:template match="similarPhonePairs[not(similarPhonePair)]" />
	-->

</xsl:stylesheet>