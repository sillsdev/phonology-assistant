<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        ExtractCustomFields.xsl
    # Purpose:     Create excerpt of fwdata with custom fields
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2014/10/30
    # Copyright:   (c) 2014 SIL International
    # Licence:     <MIT>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">

  <xsl:output method="xml"/>

  <xsl:template match="node()|@*">
    <xsl:apply-templates select="node()|@*"/>
  </xsl:template>

  <xsl:template match="node()|@*" mode="doCopy">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*" mode="doCopy"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="/">
    <xsl:text>&#xa;</xsl:text>
    <xsl:element name="Fw7CustomField">
      <xsl:text>&#xa;</xsl:text>
      <xsl:element name="CustomFields">
        <xsl:for-each select="//CustomField[@class='LexEntry']">
          <xsl:variable name="name" select="@name"/>
          <!-- Only put out names of fields that have contents in at least on entry -->
          <xsl:if test="//Custom[@name=$name]/*[1]">
            <xsl:apply-templates select="." mode="doCopy"/>
          </xsl:if>
        </xsl:for-each>
      </xsl:element>
      <xsl:text>&#xa;</xsl:text>
      <xsl:element name="CustomValues">
        <xsl:apply-templates/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="rt[./Custom[./AStr or ./AUni]]">
    <xsl:text>&#xa;</xsl:text>
    <xsl:copy>
      <xsl:apply-templates select="@*" mode="doCopy"/>
      <xsl:element name="CustomFields">
        <xsl:apply-templates select="Custom[./AStr or ./AUni]" mode="doCopy"/>
      </xsl:element>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="Custom" mode="doCopy">
    <xsl:copy>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:value-of select="./AStr/Run/text() | ./AUni/text()"/>
      </xsl:attribute>
      <xsl:attribute name="ws">
        <xsl:value-of select="./*/@ws"/>
      </xsl:attribute>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>