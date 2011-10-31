<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_7.xsl 2011-10-28 -->
  <!-- Remove temporary attributes and elements. -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Remove order attributes from descriptive features and descendents of keys in the project inventory file. -->
	<xsl:template match="features[starts-with(@class, 'descriptive')]/feature/@order" />
	<xsl:template match="keys//*/@order" />

	<!-- Remove empty chartKey elements for rows. -->
	<xsl:template match="keys//chartKey[@class = 'rowConditional'][not(feature)]" />
	<xsl:template match="keys//chartKey[@class = 'rowGeneral'][not(feature)]" />

</xsl:stylesheet>