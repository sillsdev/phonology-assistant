<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_project_inventory_5b_CV_chart_lists.xsl 2011-11-03 -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:param name="base-file-level" select="'0'" />

	<xsl:variable name="classPrefix" select="'CV chart'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@* | node()" mode="XHTML">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="XHTML" />
		</xsl:copy>
	</xsl:template>

	<!-- Copy elements with XHTML namespace. -->
	<xsl:template match="*" mode="XHTML">
		<xsl:element name="{name(.)}" namespace="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="@* | node()" mode="XHTML" />
		</xsl:element>
	</xsl:template>

	<!-- Add options and details within metadata. -->
	<xsl:template match="div[@id = 'metadata']" mode="XHTML">
		<xsl:variable name="segments" select="/inventory/segments" />
		<xsl:element name="{name(.)}" namespace="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="@* | node()" mode="XHTML" />
			<ul class="options" xmlns="http://www.w3.org/1999/xhtml">
				<li class="format">XHTML</li>
				<li class="genericRelativePath">
					<xsl:if test="number($base-file-level) &gt; 0">
						<xsl:call-template name="dup">
							<xsl:with-param name="input" select="'../'" />
							<xsl:with-param name="count" select="number($base-file-level) - 1" />
						</xsl:call-template>
					</xsl:if>
				</li>
			</ul>
			<ul class="details" xmlns="http://www.w3.org/1999/xhtml">
				<li class="view">Segments</li>
				<li class="number segment">
					<xsl:value-of select="count($segments/segment[not(@literalInChart)][features[@class = 'descriptive'][feature[. = 'consonant' or . = 'vowel']]])" />
				</li>
				<li class="number consonant">
					<xsl:value-of select="count($segments/segment[not(@literalInChart)][features[@class = 'descriptive'][feature[. = 'consonant']]])" />
				</li>
				<li class="number vowel">
					<xsl:value-of select="count($segments/segment[not(@literalInChart)][features[@class = 'descriptive'][feature[. = 'vowel']]])" />
				</li>
				<li class="project name">
					<xsl:value-of select="/inventory/@projectName" />
				</li>
				<li class="language name">
					<xsl:value-of select="/inventory/@languageName" />
				</li>
				<li class="language code">
					<xsl:value-of select="/inventory/@languageCode" />
				</li>
			</ul>
		</xsl:element>
	</xsl:template>

	<!-- For information about duplicating a string, see pages 6-8 in XSLT Cookbook. -->
	<xsl:template name="dup">
		<xsl:param name="input" />
		<xsl:param name="count" select="1" />
		<xsl:choose>
			<xsl:when test="not($count) or not($input)" />
			<xsl:when test="$count = 1">
				<xsl:value-of select="$input" />
			</xsl:when>
			<xsl:otherwise>
				<!-- If $count is odd append an extra copy of input -->
				<xsl:if test="$count mod 2">
					<xsl:value-of select="$input" />
				</xsl:if>
				<!-- Recursively apply template after doubling input and halving count -->
				<xsl:call-template name="dup">
					<xsl:with-param name="input" select="concat($input,$input)" />
					<xsl:with-param name="count" select="floor($count div 2)" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/inventory">
		<html xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<meta http-equiv="content-type" content="text/html; charset=utf-8" />
				<title>
					<xsl:choose>
						<xsl:when test="@view = 'Consonant Chart'">
							<xsl:value-of select="'Chart of consonants'" />
						</xsl:when>
						<xsl:when test="@view = 'Vowel Chart'">
							<xsl:value-of select="'Chart of vowels'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'Charts of consonants and vowels'" />
						</xsl:otherwise>
					</xsl:choose>
				</title>
			</head>
			<body>
				<xsl:apply-templates select="div[@id = 'metadata']" mode="XHTML" />
				<xsl:choose>
					<xsl:when test="featureDefinitions[@class = 'descriptive']">
						<xsl:call-template name="tablesCV">
							<xsl:with-param name="view" select="@view" />
							<xsl:with-param name="featureDefinitionsDescriptive" select="featureDefinitions[@class = 'descriptive']" />
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="settings" select="div[@id = 'metadata']/ul[@class = 'settings']" />
						<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
						<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />
						<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
						<xsl:call-template name="tablesCV">
							<xsl:with-param name="view" select="@view" />
							<xsl:with-param name="featureDefinitionsDescriptive" select="document($programPhoneticInventoryXML)/inventory/featureDefinitions[@class = 'descriptive']" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</body>
		</html>
	</xsl:template>

	<xsl:template name="tablesCV">
		<xsl:param name="view" />
		<xsl:param name="featureDefinitionsDescriptive" />
		<xsl:if test="not($view = 'Vowel Chart')">
			<xsl:call-template name="tableCV">
				<xsl:with-param name="type" select="'consonant'" />
				<xsl:with-param name="segments" select="segments" />
				<xsl:with-param name="featureDefinitionsDescriptive" select="$featureDefinitionsDescriptive" />
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="not($view = 'Consonant Chart')">
			<xsl:call-template name="tableCV">
				<xsl:with-param name="type" select="'vowel'" />
				<xsl:with-param name="segments" select="segments" />
				<xsl:with-param name="featureDefinitionsDescriptive" select="$featureDefinitionsDescriptive" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="tableCV">
		<xsl:param name="type" />
		<xsl:param name="segments" />
		<xsl:param name="featureDefinitionsDescriptive" />
		<xsl:variable name="categoryColgroup">
			<xsl:choose>
				<xsl:when test="$type = 'consonant'">
					<xsl:value-of select="'place'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'backness'" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="categoryRowgroup">
			<xsl:choose>
				<xsl:when test="$type = 'consonant'">
					<xsl:value-of select="'manner'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'height'" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tiebreakerFormat" select="translate(count($segments/segment), '0123456789', '0000000000')" />
		<div class="{concat($classPrefix, ' ', $type)}" xmlns="http://www.w3.org/1999/xhtml">
			<ul class="colgroup">
				<xsl:for-each select="$featureDefinitionsDescriptive/featureDefinition[@category = $categoryColgroup]">
					<xsl:variable name="colgroup" select="name" />
					<xsl:if test="$segments/segment/keys/chartKey[@class = 'colgroup'] = $colgroup">
						<li>
							<xsl:value-of select="$colgroup" />
						</li>
					</xsl:if>
				</xsl:for-each>
			</ul>
			<ul class="rowgroup">
				<xsl:for-each select="$featureDefinitionsDescriptive/featureDefinition[@category = $categoryRowgroup]">
					<xsl:variable name="rowgroup" select="name" />
					<xsl:if test="$segments/segment/keys/chartKey[@class = 'rowgroup'] = $rowgroup">
						<li>
							<span>
								<xsl:value-of select="$rowgroup" />
							</span>
							<ul>
								<xsl:for-each select="$segments/segment[keys/chartKey[@class = 'rowgroup'] = $rowgroup]">
									<xsl:sort select="keys/chartKey[@class = 'rowConditional']/@order" data-type="text" />
									<xsl:sort select="keys/chartKey[@class = 'rowGeneral']/@order" data-type="text" />
									<xsl:sort select="keys/chartKey[@class = 'tone']/@order" data-type="text" />
									<xsl:variable name="rowConditionalOrder" select="keys/chartKey[@class = 'rowConditional']/@order" />
									<xsl:variable name="rowGeneralOrder" select="keys/chartKey[@class = 'rowGeneral']/@order" />
									<xsl:variable name="toneOrder" select="keys/chartKey[@class = 'tone']/@order" />
									<xsl:if test="not(preceding-sibling::segment[keys/chartKey[@class = 'rowgroup'] = $rowgroup][keys/chartKey[@class = 'rowConditional']/@order = $rowConditionalOrder and keys/chartKey[@class = 'rowGeneral']/@order = $rowGeneralOrder and keys/chartKey[@class = 'tone']/@order = $toneOrder])">
										<xsl:variable name="row">
											<xsl:value-of select="$rowConditionalOrder" />
											<xsl:value-of select="' '" />
											<xsl:value-of select="$rowGeneralOrder" />
											<xsl:value-of select="' '" />
											<xsl:value-of select="$toneOrder" />
										</xsl:variable>
										<li>
											<xsl:value-of select="$row" />
										</li>
										<xsl:call-template name="tiebreakerRows">
											<xsl:with-param name="segment" select="$segments/segment[keys[chartKey[@class = 'rowgroup'] = $rowgroup][chartKey[@class = 'rowConditional']/@order = $rowConditionalOrder][chartKey[@class = 'rowGeneral']/@order = $rowGeneralOrder][chartKey[@class = 'tiebreaker']]][1]" />
											<xsl:with-param name="rowgroup" select="$rowgroup" />
											<xsl:with-param name="rowConditionalOrder" select="$rowConditionalOrder" />
											<xsl:with-param name="rowGeneralOrder" select="$rowGeneralOrder" />
											<xsl:with-param name="row" select="$row" />
											<xsl:with-param name="tiebreakerFormat" select="$tiebreakerFormat" />
										</xsl:call-template>
									</xsl:if>
								</xsl:for-each>
							</ul>
						</li>
					</xsl:if>
				</xsl:for-each>
			</ul>
			<ul class="segment">
				<xsl:for-each select="$segments/segment[features[@class = 'descriptive']/feature[. = $type]]">
					<xsl:variable name="keys" select="keys" />
					<xsl:variable name="tiebreaker" select="$keys/chartKey[@class = 'tiebreaker']" />
					<li>
						<span>
							<xsl:value-of select="@literal" />
						</span>
						<ul class="chartKey">
							<li class="rowgroup">
								<xsl:value-of select="$keys/chartKey[@class = 'rowgroup']" />
							</li>
							<li class="colgroup">
								<xsl:value-of select="$keys/chartKey[@class = 'colgroup']" />
							</li>
							<li class="col">
								<xsl:variable name="col" select="$keys/chartKey[@class = 'col']" />
								<xsl:choose>
									<xsl:when test="$col = 'voiceless' or $col = 'unrounded'">
										<xsl:value-of select="0" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="1" />
									</xsl:otherwise>
								</xsl:choose>
							</li>
							<li class="row">
								<xsl:value-of select="$keys/chartKey[@class = 'rowConditional']/@order" />
								<xsl:value-of select="' '" />
								<xsl:value-of select="$keys/chartKey[@class = 'rowGeneral']/@order" />
								<xsl:value-of select="' '" />
								<xsl:value-of select="$keys/chartKey[@class = 'tone']/@order" />
								<xsl:if test="$tiebreaker">
									<xsl:value-of select="' '" />
									<xsl:call-template name="tiebreaker">
										<xsl:with-param name="segment" select="."  />
										<xsl:with-param name="tiebreakerFormat" select="$tiebreakerFormat" />
									</xsl:call-template>
								</xsl:if>
							</li>
						</ul>
					</li>
				</xsl:for-each>
			</ul>
		</div>
	</xsl:template>

	<xsl:template name="tiebreaker">
		<xsl:param name="segment" />
		<xsl:param name="tiebreakerFormat" />
		<xsl:param name="n" select="1" />
		<xsl:variable name="segmentPreceding" select="$segment/preceding-sibling::*[1]" />
		<xsl:choose>
			<xsl:when test="$segmentPreceding[keys/chartKey[@class = 'tiebreaker']]">
				<xsl:call-template name="tiebreaker">
					<xsl:with-param name="segment" select="$segmentPreceding"  />
					<xsl:with-param name="tiebreakerFormat" select="$tiebreakerFormat" />
					<xsl:with-param name="n" select="$n + 1" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="format-number($n, $tiebreakerFormat)" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="tiebreakerRows">
		<xsl:param name="segment" />
		<xsl:param name="rowgroup" />
		<xsl:param name="rowConditionalOrder" />
		<xsl:param name="rowGeneralOrder" />
		<xsl:param name="row" />
		<xsl:param name="tiebreakerFormat" />
		<xsl:param name="nMax" select="0" />
		<xsl:if test="$segment">
			<xsl:call-template name="tiebreakerRows2">
				<xsl:with-param name="segment" select="$segment" />
				<xsl:with-param name="segment2" select="$segment/following-sibling::*[1][keys[chartKey[@class = 'tiebreaker']]]" />
				<xsl:with-param name="rowgroup" select="$rowgroup" />
				<xsl:with-param name="rowConditionalOrder" select="$rowConditionalOrder" />
				<xsl:with-param name="rowGeneralOrder" select="$rowGeneralOrder" />
				<xsl:with-param name="row" select="$row" />
				<xsl:with-param name="tiebreakerFormat" select="$tiebreakerFormat" />
				<xsl:with-param name="nMax" select="$nMax" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="tiebreakerRows2">
		<xsl:param name="segment" />
		<xsl:param name="segment2" />
		<xsl:param name="rowgroup" />
		<xsl:param name="rowConditionalOrder" />
		<xsl:param name="rowGeneralOrder" />
		<xsl:param name="row" />
		<xsl:param name="tiebreakerFormat" />
		<xsl:param name="nMax" />
		<xsl:param name="n" select="1" />
		<xsl:if test="$n &gt; $nMax">
			<li xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="$row" />
				<xsl:value-of select="' '" />
				<xsl:value-of select="format-number($n, $tiebreakerFormat)" />
			</li>
		</xsl:if>
		<xsl:variable name="newMax">
			<xsl:choose>
				<xsl:when test="$n &gt; $nMax">
					<xsl:value-of select="$n" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nMax" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$segment2">
				<xsl:call-template name="tiebreakerRows2">
					<xsl:with-param name="segment" select="$segment" />
					<xsl:with-param name="segment2" select="$segment2/following-sibling::*[1][keys[chartKey[@class = 'tiebreaker']]]" />
					<xsl:with-param name="rowgroup" select="$rowgroup" />
					<xsl:with-param name="rowConditionalOrder" select="$rowConditionalOrder" />
					<xsl:with-param name="rowGeneralOrder" select="$rowGeneralOrder" />
					<xsl:with-param name="row" select="$row" />
					<xsl:with-param name="tiebreakerFormat" select="$tiebreakerFormat" />
					<xsl:with-param name="nMax" select="$newMax" />
					<xsl:with-param name="n" select="$n + 1" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="tiebreakerRows">
					<xsl:with-param name="segment" select="$segment/following-sibling::*[keys[chartKey[@class = 'rowgroup'] = $rowgroup][chartKey[@class = 'rowConditional']/@order = $rowConditionalOrder][chartKey[@class = 'rowGeneral']/@order = $rowGeneralOrder][chartKey[@class = 'tiebreaker']]][1]"  />
					<xsl:with-param name="rowgroup" select="$rowgroup" />
					<xsl:with-param name="rowConditionalOrder" select="$rowConditionalOrder" />
					<xsl:with-param name="rowGeneralOrder" select="$rowGeneralOrder" />
					<xsl:with-param name="row" select="$row" />
					<xsl:with-param name="tiebreakerFormat" select="$tiebreakerFormat" />
					<xsl:with-param name="nMax" select="$newMax" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>