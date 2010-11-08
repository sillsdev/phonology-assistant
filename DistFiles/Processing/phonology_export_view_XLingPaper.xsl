<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:x="http://www.w3.org/1999/xhtml" exclude-result-prefixes="x">
    <xsl:output method="xml" encoding="UTF-8" indent="yes" doctype-system="XLingPap.dtd" doctype-public="-//XMLmind//DTD XLingPap//EN"/>
    <!-- This stylesheet transforms a chart as output by Phonology Assistant into a form which conforms to the XLingPap DTD and can then be edited with the XMLmind XML Editor usingthe XLingPap configuration.  -->
    <!--
================================================================
Phonology Assistant XML to XLingPap mapper.
  Input:    XML output of Phonology Assistant
  Output: XLingPap XML

================================================================
Revision History is at the end of this file.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
Preamble
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    -->
    <xsl:variable name="details" select="//x:ul[@class='details']"/>
    <xsl:variable name="table" select="//x:table[@class!='formatting']"/>
    <xsl:variable name="language" select="//x:table[@class='formatting']/x:tbody/x:tr"/>
    <!--
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
Main template
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
-->
    <xsl:template match="/">
        <!-- output dtd path -->
        <!--        <xsl:text disable-output-escaping="yes">&#xa;&#x3c;!DOCTYPE lingPaper PUBLIC   "-//XMLmind//DTD XLingPap//EN" "XLingPap.dtd"&#x3e;&#xa;</xsl:text>-->
        <lingPaper>
            <frontMatter>
                <title>
                    <xsl:variable name="sTitle" select="normalize-space($details/x:li[@class='view'])"/>
                    <xsl:choose>
                        <xsl:when test="string-length($sTitle) &gt; 0">
                            <xsl:value-of select="$sTitle"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:text>Phonology Assistant Export</xsl:text>
                        </xsl:otherwise>
                    </xsl:choose>
                </title>
                <author>
                    <xsl:variable name="sAuthor" select="normalize-space($details/x:li[@class='researcher'])"/>
                    <xsl:choose>
                        <xsl:when test="string-length($sAuthor) &gt; 0">
                            <xsl:value-of select="$sAuthor"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:text>[Insert author's name here]</xsl:text>
                        </xsl:otherwise>
                    </xsl:choose>
                </author>
                <date>
                    <xsl:value-of select="$details/x:li[@class='date']"/>
                    <xsl:text>&#x20;</xsl:text>
                    <xsl:value-of select="$details/x:li[@class='time']"/>
                </date>
            </frontMatter>
            <section1 id="s1">
                <secTitle>First Section</secTitle>
                <p/>
                <xsl:for-each select="$table">
                    <table border="1">
                        <xsl:for-each select="$table/x:thead">
                            <xsl:apply-templates/>
                        </xsl:for-each>
                        <xsl:for-each select="$table/x:tbody">
                            <xsl:apply-templates/>
                        </xsl:for-each>
                    </table>
                </xsl:for-each>
            </section1>
            <languages>
                <language id="lerror" color="red"/>
                <xsl:for-each select="//x:table[@class='formatting']/x:tbody/x:tr">
                    <language id="l{translate(x:td[@class='class'],' ','_')}">
                        <xsl:call-template name="DoFontAttribute">
                            <xsl:with-param name="sAttribute" select="'font-family'"/>
                        </xsl:call-template>
                        <xsl:call-template name="DoFontAttribute">
                            <xsl:with-param name="sAttribute" select="'font-size'"/>
                        </xsl:call-template>
                        <xsl:call-template name="DoFontAttribute">
                            <xsl:with-param name="sAttribute" select="'font-style'"/>
                        </xsl:call-template>
                        <xsl:call-template name="DoFontAttribute">
                            <xsl:with-param name="sAttribute" select="'font-weight'"/>
                        </xsl:call-template>
                    </language>
                </xsl:for-each>
            </languages>
            <types>
                <xsl:call-template name="CommonTypes"/>
            </types>
        </lingPaper>
    </xsl:template>
    <!--
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        td
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    -->
    <xsl:template match="x:td">
        <td>
            <xsl:choose>
                <xsl:when test="ancestor::x:table[@class='distribution chart'] and string(number(.))!='NaN'">
                    <xsl:attribute name="align">right</xsl:attribute>
                    <xsl:call-template name="OutputCellContent"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:choose>
                        <xsl:when test="contains(@class,'preceding')">
                            <xsl:attribute name="align">right</xsl:attribute>
                            <!--                            <xsl:attribute name="padding-right">0</xsl:attribute>-->
                        </xsl:when>
                        <xsl:when test="contains(@class,'item')">
                            <xsl:attribute name="align">center</xsl:attribute>
                            <xsl:attribute name="backgroundcolor">yellow</xsl:attribute>
                            <!--                            <xsl:attribute name="padding-left">0</xsl:attribute>-->
                            <!--                            <xsl:attribute name="padding-right">0</xsl:attribute>-->
                        </xsl:when>
                        <xsl:when test="contains(@class,'following')">
                            <!--                            <xsl:attribute name="padding-left">0</xsl:attribute>-->
                        </xsl:when>
                    </xsl:choose>
                    <langData>
                        <xsl:attribute name="lang">
                            <xsl:text>l</xsl:text>
                            <xsl:choose>
                                <xsl:when test="contains(@class,' ')">
                                    <xsl:value-of select="substring-before(@class, ' ')"/>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="@class"/>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:attribute>
                        <xsl:call-template name="OutputCellContent"/>
                    </langData>
                </xsl:otherwise>
            </xsl:choose>
        </td>
    </xsl:template>
    <!--
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        th
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    -->
    <xsl:template match="x:th">
        <th>
            <xsl:if test="ancestor::x:thead">
                <xsl:attribute name="align">center</xsl:attribute>
            </xsl:if>
            <xsl:copy-of select="@*[name()='colspan' or name()='rowspan']"/>
            <xsl:if test="@rowspan &gt; 1">
                <xsl:attribute name="valign">middle</xsl:attribute>
            </xsl:if>
            <xsl:call-template name="OutputCellContent"/>
        </th>
    </xsl:template>
    <!--
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        th
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    -->
    <xsl:template match="x:tr">
        <tr>
            <xsl:apply-templates/>
        </tr>
    </xsl:template>
    <!--
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        CommonTypes
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    -->
    <xsl:template name="CommonTypes">
        <type id="tHomographNumber" font-size="65%" cssSpecial="vertical-align:sub" xsl-foSpecial="baseline-shift='sub'"/>
        <type id="tVariantTypes">
            <xsl:variable name="analysisLanguage" select="//language[not(@vernacular='true')][1]"/>
            <xsl:if test="$analysisLanguage">
                <xsl:attribute name="font-family">
                    <xsl:value-of select="$analysisLanguage/@font"/>
                </xsl:attribute>
            </xsl:if>
        </type>
    </xsl:template>
    <!--
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        DoFontAttribute
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    -->
    <xsl:template name="DoFontAttribute">
        <xsl:param name="sAttribute"/>
        <xsl:variable name="sFontAttribute" select="x:td[@class=$sAttribute]"/>
        <xsl:if test="string-length($sFontAttribute) &gt; 0">
            <xsl:attribute name="{$sAttribute}">
                <xsl:value-of select="$sFontAttribute"/>
                <xsl:if test="$sAttribute='font-size'">
                    <xsl:text>pt</xsl:text>
                </xsl:if>
            </xsl:attribute>
        </xsl:if>
    </xsl:template>
    <!--
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        OutputCellContent
        - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    -->
    <xsl:template name="OutputCellContent">
        <xsl:choose>
            <xsl:when test="string-length(.) &gt; 0">
                <xsl:apply-templates/>
            </xsl:when>
            <xsl:when test="@class='error'">
                <xsl:text>Error!</xsl:text>
            </xsl:when>
            <xsl:otherwise>
                <xsl:text>&#xa0;</xsl:text>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
