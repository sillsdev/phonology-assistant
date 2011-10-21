<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_project_inventory_5d_CV_chart_view.xsl 2011-08-17 -->
	<!-- Convert from exported XHTML format to XML format with zero-based indexes. -->
	<!-- When Phonology Assistant can read the XHTML format directly, remove this step! -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

	<xsl:template match="/">
		<!-- The attributes are not essential for the PhoneChart element. -->
		<PhoneChart>
			<!-- The previous step must make sure there is only one CV charts. -->
			<xsl:apply-templates select="//xhtml:table[starts-with(@class, 'CV chart')]" />
		</PhoneChart>
	</xsl:template>

	<xsl:template match="xhtml:table[starts-with(@class, 'CV chart')]">
		<ColHeadings>
			<xsl:for-each select="xhtml:thead/xhtml:tr/xhtml:th[node()]">
				<xsl:variable name="text">
					<xsl:choose>
						<xsl:when test="@title">
							<xsl:value-of select="@title" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="text()" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<!-- The SubHeadingsVisible and Group attributes are not essential. -->
				<!--
				<Heading text="{$featureName}" SubHeadingsVisible="false" Group="0">
				-->
				<!-- Column groups always contain two columns. -->
				<Heading text="{.}">
					<SubHeading />
					<SubHeading />
				</Heading>
			</xsl:for-each>
		</ColHeadings>
		<RowHeadings>
			<xsl:for-each select="xhtml:tbody">
				<!-- The SubHeadingsVisible attribute is not essential but the Group is. -->
				<!--
				<Heading text="{$featureName}" SubHeadingsVisible="false" Group="{$group}">
				-->
				<Heading text="{xhtml:tr[1]/xhtml:th}" Group="{position() - 1}">
					<xsl:for-each select="xhtml:tr">
						<SubHeading />
					</xsl:for-each>
				</Heading>
			</xsl:for-each>
		</RowHeadings>
		<Phones>
			<xsl:apply-templates select="xhtml:tbody[1]" mode="segments">
				<xsl:with-param name="groupsPreceding" select="0" />
				<xsl:with-param name="rowsPreceding" select="0" />
			</xsl:apply-templates>
		</Phones>
	</xsl:template>

	<xsl:template match="xhtml:tbody" mode="segments">
		<xsl:param name="groupsPreceding" />
		<xsl:param name="rowsPreceding" />
		<xsl:for-each select="xhtml:tr">
			<xsl:variable name="row" select="$rowsPreceding + position() - 1" />
			<xsl:for-each select="xhtml:td">
				<xsl:if test="node()">
					<xsl:variable name="text">
						<xsl:choose>
							<xsl:when test="xhtml:span">
								<xsl:value-of select="xhtml:span" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="text()" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<!-- The Visible attributes is not essential. -->
					<!-- An empty SiblingUncertainties child element is not essential. -->
					<!--
					<Phone text="{$literal}" Visible="true" Row="{$row}" Column="{$colgroup + $col}" Group="{$group}" />
					-->
					<Phone text="{$text}" Row="{$row}" Column="{position() - 1}" Group="{$groupsPreceding}" />
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
		<xsl:apply-templates select="following-sibling::xhtml:tbody[1]" mode="segments">
			<xsl:with-param name="groupsPreceding" select="$groupsPreceding + 1" />
			<xsl:with-param name="rowsPreceding" select="$rowsPreceding + count(xhtml:tr)" />
		</xsl:apply-templates>
	</xsl:template>

</xsl:stylesheet>