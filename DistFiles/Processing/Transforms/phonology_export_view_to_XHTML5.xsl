<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
xmlns:svg="http://www.w3.org/2000/svg"
exclude-result-prefixes="xhtml svg"
>

	<!-- phonology_export_view_to_XHTML5.xsl 2011-05-09 -->
	<!-- -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="yes" />

	<!-- Insert CDATA sections within style elements. -->
	<xsl:output cdata-section-elements="svg:style xhtml:style" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- For XHTML5, insert <!DOCTYPE html> preceding the document element. -->
	<xsl:template match="xhtml:html">
		<xsl:value-of disable-output-escaping="yes" select="'&lt;!DOCTYPE html&gt;&#xA;'" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="not(@xml:lang)">
				<!-- Replace lang attribute with xml:lang if it is missing. -->
				<xsl:attribute name="xml:lang">
					<xsl:value-of select="@lang" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Remove lang attribute. -->
	<xsl:template match="xhtml:html/@lang" />
	
	<!-- Replace charset -->
	<xsl:template match="xhtml:head/xhtml:meta[translate(@http-equiv, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = 'content-type']">
		<xsl:copy>
			<xsl:attribute name="charset">
				<xsl:value-of select="'UTF-8'" />
			</xsl:attribute>
		</xsl:copy>
	</xsl:template>

	<!-- Invalid and probably not needed by IE9. -->
	<!--
	<xsl:template match="xhtml:head/xhtml:meta[translate(@http-equiv, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = 'x-ua-compatible']" />
	-->

	<!-- Optional and not needed by browsers. -->
	<xsl:template match="svg:style/@type" mode="XHTML" />
	<xsl:template match="xhtml:link[translate(@rel, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = 'stylesheet']/@type" />
	<xsl:template match="xhtml:script/@type" />

	<!-- Remove cellspacing attribute. -->
	<xsl:template match="xhtml:table/@cellspacing" />

	<!-- Undo unnecessary change from HTML Tidy: -->
	<!-- Newline necessary to force start and end tags on separate lines. -->
	<xsl:template match="xhtml:script[not(*) and not(text())]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="'&#xA;'" />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>