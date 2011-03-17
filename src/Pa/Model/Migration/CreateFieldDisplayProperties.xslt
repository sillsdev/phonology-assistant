<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

	<xsl:template match="/PaFields">
		<FieldDisplayProps>
			<xsl:attribute name="version">
				<xsl:value-of select="3.3"/>
			</xsl:attribute>
			<xsl:apply-templates />
		</FieldDisplayProps>
	</xsl:template>
	
	<xsl:template match="//PaFields/Field">
		<PaFieldDisplayProperties>
			<xsl:apply-templates />
		</PaFieldDisplayProperties>
	</xsl:template>

	<xsl:template match="//PaFields/Field/Font">
		<font>
			<xsl:apply-templates select="@*"/>
		</font>
	</xsl:template>

	<xsl:template match="//PaFields/Field/RightToLeft">
		<rightToLeft>
			<xsl:apply-templates />
		</rightToLeft>
	</xsl:template>

	<xsl:template match="/PaFields/Field/VisibleInGrid">
		<visibleInGrid>
			<xsl:apply-templates />
		</visibleInGrid>
	</xsl:template>
	
	<xsl:template match="/PaFields/Field/VisibleInRecView">
		<visibleInRecView>
			<xsl:apply-templates />
		</visibleInRecView>
	</xsl:template>
	
	<xsl:template match="/PaFields/Field/DisplayIndexInGrid">
		<displayIndexInGrid>
			<xsl:apply-templates />
		</displayIndexInGrid>
	</xsl:template>
	
	<xsl:template match="/PaFields/Field/DisplayIndexInRecView">
		<displayIndexInRecView>
			<xsl:apply-templates />
		</displayIndexInRecView>
	</xsl:template>

	<xsl:template match="/PaFields/Field/WidthInGrid">
		<widthInGrid>
			<xsl:apply-templates />
		</widthInGrid>
	</xsl:template>

	<xsl:template match="/PaFields//Field/DisplayText" />
	<xsl:template match="/PaFields//Field/FwWritingSystemType" />
	<xsl:template match="/PaFields//Field/CanBeInterlinear" />
	<xsl:template match="/PaFields//Field/IsParsed" />
	<xsl:template match="/PaFields//Field/IsPhonetic" />
	<xsl:template match="/PaFields//Field/IsPhonemic" />
	<xsl:template match="/PaFields//Field/IsTone" />
	<xsl:template match="/PaFields//Field/IsOrtho" />
	<xsl:template match="/PaFields//Field/IsGloss" />
	<xsl:template match="/PaFields//Field/IsReference" />
	<xsl:template match="/PaFields//Field/IsCVPattern" />
	<xsl:template match="/PaFields//Field/IsDate" />
	<xsl:template match="/PaFields//Field/IsDataSourcePath" />
	<xsl:template match="/PaFields//Field/IsDataSource" />
	<xsl:template match="/PaFields//Field/FwQueryFieldName" />
	<xsl:template match="/PaFields//Field/IsAudioFile" />
	<xsl:template match="/PaFields//Field/IsAudioOffset" />
	<xsl:template match="/PaFields//Field/IsAudioLength" />
	<xsl:template match="/PaFields//Field/IsCustom" />
	<xsl:template match="/PaFields//Field/IsNumeric" />
	<xsl:template match="/PaFields//Field/IsGuid" />
	<xsl:template match="/PaFields//Field/SaFieldName" />

</xsl:stylesheet>
