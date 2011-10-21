<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
exclude-result-prefixes="xhtml"
>

  <!-- phonology_export_view_CV_chart_1.xsl 2011-10-06 -->
	<!-- Export to XHTML: title attribute of phonetic cell contains description of segment. -->
  <!-- In several column heading cells, optionally insert line breaks. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />

	<xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="breakWideColumnHeadingsCV" select="$options/xhtml:li[@class = 'breakWideColumnHeadingsCV']" />
	<xsl:variable name="headingUppercaseCV" select="$options/xhtml:li[@class = 'headingUppercaseCV']" />

	<xsl:variable name="lower" select="'abcdefghijklmnopqrstuvwxyz'" />
	<xsl:variable name="UPPER" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />

	<!-- A project phonetic inventory file contains features of phonetic or phonological segments, or both. -->
	<xsl:variable name="projectFolder" select="$settings/xhtml:li[@class = 'projectFolder']" />
	<xsl:variable name="projectPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'projectPhoneticInventoryFile']" />
	<xsl:variable name="projectPhoneticInventoryXML" select="concat($projectFolder, $projectPhoneticInventoryFile)" />
	<xsl:variable name="segments" select="document($projectPhoneticInventoryXML)/inventory/segments" />

	<!-- The program phonetic character inventory file contains the features, symbols, and so on. -->
	<xsl:variable name="programConfigurationFolder" select="$settings/xhtml:li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/xhtml:li[@class = 'programPhoneticInventoryFile']" />
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="programDescriptiveFeatures" select="document($programPhoneticInventoryXML)/inventory/featureDefinitions[@class = 'descriptive']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="/xhtml:html/xhtml:head/xhtml:title">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="title" select="." />
			<xsl:choose>
				<xsl:when test="$title = 'Consonant Chart'">
					<xsl:value-of select="'Chart of consonants'" />
				</xsl:when>
				<xsl:when test="$title = 'Vowel Chart'">
					<xsl:value-of select="'Chart of vowels'" />
				</xsl:when>
				<xsl:when test="$title = 'Segment Charts'">
					<xsl:value-of select="'Features of consonants and vowels'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$title" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>
	
	<!-- Wrap ordinary CV chart in a div and insert a heading. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'CV chart')]">
		<div xmlns="http://www.w3.org/1999/xhtml">
			<h3>
				<xsl:variable name="title" select="/xhtml:html/xhtml:head/xhtml:title" />
				<xsl:choose>
					<xsl:when test="$title = 'Consonant Chart'">
						<xsl:value-of select="'Consonant chart'" />
					</xsl:when>
					<xsl:when test="$title = 'Vowel Chart'">
						<xsl:value-of select="'Vowel chart'" />
					</xsl:when>
					<xsl:when test="$title = 'Segment Charts' or $title = 'Features of consonants and vowels'">
						<xsl:value-of select="'Charts of consonants and vowels'" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$title" />
					</xsl:otherwise>
				</xsl:choose>
			</h3>
			<xsl:variable name="following" select="following-sibling::*[1][self::xhtml:table][starts-with(@class, 'CV chart')][xhtml:tbody]" />
			<xsl:choose>
				<xsl:when test="$following">
					<div class="CV_features">
						<div class="CV2">
							<xsl:copy>
								<xsl:apply-templates select="@* | node()" />
							</xsl:copy>
							<table>
								<xsl:apply-templates select="$following/@* | $following/node()" />
							</table>
						</div>
					</div>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy>
						<xsl:apply-templates select="@* | node()" />
					</xsl:copy>
				</xsl:otherwise>
			</xsl:choose>
		</div>
	</xsl:template>

	<!-- Previous rule encloses adjacent non-empty CV charts in divs. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'CV chart')][preceding-sibling::*[1][self::xhtml:table][starts-with(@class, 'CV chart')]]" />

	<!-- But not if related to a distribution chart. -->
	<xsl:template match="xhtml:div[@class = 'distribution_environments_CV']/xhtml:table[starts-with(@class, 'CV chart')]">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Remove an empty CV chart. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'CV chart')][not(xhtml:tbody)]" />

	<!-- Append consonant or vowel to class attribute of table. -->
	<xsl:template match="xhtml:table[@class = 'CV chart']/@class">
		<xsl:variable name="td" select="..//xhtml:td[@class = 'Phonetic'][node()]" />
		<xsl:variable name="literal">
			<xsl:choose>
				<xsl:when test="$td/xhtml:span">
					<xsl:value-of select="$td/xhtml:span" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$td" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:attribute name="class">
			<xsl:value-of select="." />
			<xsl:value-of select="' '" />
			<xsl:choose>
				<xsl:when test="$segments/segment[@literal = $literal]/features[@class = 'descriptive']/feature[. = 'consonant']">
					<xsl:value-of select="'consonant'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'vowel'" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>

	<!-- Export to XHTML: title attribute of phonetic cell contains description of segment. -->
	<xsl:template match="xhtml:table[starts-with(@class, 'CV chart')]//xhtml:td[@class = 'Phonetic'][node()]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:if test="$format = 'XHTML'">
				<xsl:variable name="literal">
					<xsl:choose>
						<xsl:when test="xhtml:span">
							<xsl:value-of select="xhtml:span" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="." />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="description" select="$segments/segment[@literal = $literal]/description" />
				<xsl:if test="string-length($description) != 0">
					<xsl:attribute name="title">
						<xsl:value-of select="$description" />
					</xsl:attribute>
				</xsl:if>
			</xsl:if>
			<xsl:apply-templates />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="xhtml:table[starts-with(@class, 'CV chart')]/xhtml:tbody/xhtml:tr">
		<xsl:variable name="td" select="xhtml:td[@class = 'Phonetic'][node()][1]" />
		<xsl:variable name="span" select="$td/xhtml:span" />
		<xsl:variable name="literal">
			<xsl:choose>
				<xsl:when test="$span">
					<xsl:value-of select="$span" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$td" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="segment" select="$segments/segment[@literal = $literal]" />
		<xsl:variable name="type" select="$segment/features[@class = 'descriptive']/feature[@category = 'type']" />
		<xsl:variable name="keys" select="$segment/keys" />
		<xsl:variable name="title">
			<xsl:apply-templates select="$keys/chartKey[@class = 'rowGeneral']/feature" mode="title">
				<xsl:with-param name="type" select="$type" />
			</xsl:apply-templates>
			<xsl:apply-templates select="$keys/chartKey[@class = 'rowConditional']/feature" mode="title">
				<xsl:with-param name="type" select="$type" />
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="xhtml:th">
					<xsl:apply-templates select="xhtml:th" mode="row">
						<xsl:with-param name="title" select="$title" />
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<th scope="row" xmlns="http://www.w3.org/1999/xhtml">
						<xsl:if test="string-length($title) != 0">
							<xsl:attribute name="title">
								<xsl:value-of select="substring($title, 2)" />
							</xsl:attribute>
						</xsl:if>
					</th>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:apply-templates select="xhtml:td" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="feature" mode="title">
		<xsl:param name="type" />
		<xsl:variable name="feature" select="." />
		<xsl:if test="$segments/segment[features[@class = 'descriptive'][feature[@category = 'type'] = $type][not(feature[. = $feature])]]">
			<xsl:value-of select="' '" />
			<xsl:value-of select="$feature" />
		</xsl:if>
	</xsl:template>

	<xsl:template match="xhtml:th" mode="row">
		<xsl:param name="title" />
		<xsl:copy>
			<xsl:attribute name="scope">
				<xsl:value-of select="'row'" />
			</xsl:attribute>
			<xsl:if test="string-length($title) != 0">
				<xsl:attribute name="title">
					<xsl:value-of select="substring($title, 2)" />
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- In column heading cells, optionally insert line breaks. -->
	<!-- The title attribute consists of the descriptive feature. -->

	<xsl:template match="xhtml:table[starts-with(@class, 'CV chart')]/xhtml:thead/xhtml:tr/xhtml:th">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:choose>
				<xsl:when test="$breakWideColumnHeadingsCV = 'true'">
					<xsl:choose>
						<!-- Some descriptive features contain slash (dental/alveolar) -->
						<xsl:when test="contains(., '/')">
							<xsl:call-template name="break">
								<xsl:with-param name="title" select="." />
								<xsl:with-param name="before" select="substring-before(., '/')" />
								<xsl:with-param name="punctuation" select="'/'" />
								<xsl:with-param name="after" select="substring-after(., '/')" />
							</xsl:call-template>
						</xsl:when>
						<!-- Some descriptive features contain hyphens (for example, alveolo-palatal) -->
						<xsl:when test="contains(., '-')">
							<xsl:call-template name="break">
								<xsl:with-param name="title" select="." />
								<xsl:with-param name="before" select="substring-before(., '-')" />
								<xsl:with-param name="punctuation" select="'-'" />
								<xsl:with-param name="after" select="substring-after(., '-')" />
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="name" select="concat(translate(substring(., 1, 1), $UPPER, $lower), substring(., 2))" />
							<xsl:variable name="breakAfter" select="$programDescriptiveFeatures/featureDefinition[name = $name]/breakAfter" />
							<xsl:choose>
								<xsl:when test="$breakAfter">
									<xsl:call-template name="break">
										<xsl:with-param name="title" select="." />
										<xsl:with-param name="before">
											<xsl:call-template name="optionalUppercase1">
												<xsl:with-param name="text" select="$breakAfter" />
											</xsl:call-template>
										</xsl:with-param>
										<xsl:with-param name="after" select="substring-after($name, $breakAfter)" />
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="break">
		<xsl:param name="title" />
		<xsl:param name="before" />
		<xsl:param name="punctuation" />
		<xsl:param name="after" />
		<xsl:attribute name="title">
			<xsl:value-of select="$title" />
		</xsl:attribute>
		<xsl:value-of select="$before" />
		<xsl:value-of select="$punctuation" />
		<br xmlns="http://www.w3.org/1999/xhtml" />
		<xsl:value-of select="$after" />
	</xsl:template>

	<!-- In CV chart headings, an option can make the first letter uppercase. -->
	<xsl:template name="optionalUppercase1">
		<xsl:param name="text" />
		<xsl:choose>
			<xsl:when test="$headingUppercaseCV = 'true'">
				<xsl:value-of select="concat(translate(substring($text, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($text, 2))" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$text" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>