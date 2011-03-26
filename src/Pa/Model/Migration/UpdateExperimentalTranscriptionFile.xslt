<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes" />

	<!--<xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>-->
 
	<xsl:template match="ArrayOfExperimentalTranscription">
		<transcriptionChanges>
			<xsl:apply-templates />
		</transcriptionChanges>
	</xsl:template>

	<xsl:template match="ExperimentalTranscription">
		<change findWhat="{@ConvertFromItem}">
			<xsl:if test="@CurrentConvertToItem">
				<xsl:attribute name="replaceWith">
					<xsl:value-of select="@CurrentConvertToItem" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</change>
	</xsl:template>

	<xsl:template match="TranscriptionToConvert">
		<replacementOption literal="{.}" />
	</xsl:template>
</xsl:stylesheet>
