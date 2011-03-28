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
		<Fields>
			<xsl:attribute name="version">
				<xsl:value-of select="3.3"/>
			</xsl:attribute>
			<xsl:apply-templates />
		</Fields>
	</xsl:template>

	<xsl:template match="/PaFields/Field">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<xsl:apply-templates />
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[IsPhonetic='true']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<xsl:attribute name="type">Phonetic</xsl:attribute>
			<possibleDataSourceFieldNames>\tn;\pi</possibleDataSourceFieldNames>
			<fwWritingSystemType>Vernacular</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[@Name='Gloss']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<possibleDataSourceFieldNames>\gl;\ge</possibleDataSourceFieldNames>
			<fwWritingSystemType>Analysis</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[@Name='Gloss-Secondary']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<possibleDataSourceFieldNames>\gn</possibleDataSourceFieldNames>
			<fwWritingSystemType>Analysis</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[@Name='Gloss-Other']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<possibleDataSourceFieldNames>\gn</possibleDataSourceFieldNames>
			<fwWritingSystemType>Analysis</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[IsReference='true']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<xsl:attribute name="type">Reference</xsl:attribute>
			<possibleDataSourceFieldNames>\ref</possibleDataSourceFieldNames>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[IsDate='true']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<xsl:attribute name="type">Date</xsl:attribute>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[IsNumeric='true']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<xsl:attribute name="type">GeneralNumeric</xsl:attribute>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[IsTone='true']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<possibleDataSourceFieldNames>\tn;\pi</possibleDataSourceFieldNames>
			<fwWritingSystemType>None</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[IsOrtho='true']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<fwWritingSystemType>Vernacular</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[IsPhonemic='true']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<fwWritingSystemType>Vernacular</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[@Name='PartOfSpeech']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<possibleDataSourceFieldNames>\ps</possibleDataSourceFieldNames>
			<fwWritingSystemType>CmPossibility</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[@Name='Note']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<fwWritingSystemType>Analysis</fwWritingSystemType>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields//Field[IsAudioFile='true']">
		<field>
			<xsl:attribute name="name">
				<xsl:value-of select="@Name"/>
			</xsl:attribute>
			<xsl:attribute name="type">AudioFilePath</xsl:attribute>
			<possibleDataSourceFieldNames>\sf;\snd</possibleDataSourceFieldNames>
		</field>
	</xsl:template>

	<xsl:template match="/PaFields/Field[@Name='CVPattern']" />
	<xsl:template match="/PaFields/Field[@Name='Audio File Offset']" />
	<xsl:template match="/PaFields/Field[@Name='Audio File Length']" />
	<xsl:template match="/PaFields/Field[@Name='GUID']" />
	<xsl:template match="/PaFields/Field[@Name='DataSource']" />
	<xsl:template match="/PaFields/Field[@Name='DataSourcePath']" />
	<xsl:template match="/PaFields//Field/Font" />
	<xsl:template match="/PaFields//Field/RightToLeft" />
	<xsl:template match="/PaFields//Field/VisibleInGrid" />
	<xsl:template match="/PaFields//Field/VisibleInRecView" />
	<xsl:template match="/PaFields//Field/DisplayIndexInGrid" />
	<xsl:template match="/PaFields//Field/DisplayIndexInRecView" />
	<xsl:template match="/PaFields//Field/WidthInGrid" />
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
