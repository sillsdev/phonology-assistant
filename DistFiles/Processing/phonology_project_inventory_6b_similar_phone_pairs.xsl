<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_6b_similar_phone_pairs.xsl 2010-05-14 -->

	<xsl:variable name="classOfSortKey" select="'placeOfArticulation'" />

	<xsl:variable name="programConfigurationFolder" select="//div[@id = 'metadata']/ul[@class = 'settings']/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="//div[@id = 'metadata']/ul[@class = 'settings']/li[@class = 'programPhoneticInventoryFile']" />
	
	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="hierarchicalFeatures" select="document($programPhoneticInventoryXML)/inventory/hierarchicalFeatures" />

	<xsl:variable name="units" select="/inventory/units[@type = 'phonetic']" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<!-- Insert an attribute to indicate whether the pair is similar according to: -->
	<!-- * hierarchical feature differences -->
	<!-- * similar phone patterns -->
	<xsl:template match="similarPhonePairs/pair">
		<xsl:variable name="phone1" select="phone[1]" />
		<xsl:variable name="phone2" select="phone[2]" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:attribute name="hierarchicalFeatures">
				<xsl:choose>
					<!-- No differences. -->
					<xsl:when test="$phone1[root[not(*)]] and $phone2[root[not(*)]]">
						<xsl:value-of select="'true'" />
					</xsl:when>
					<!-- The first phone has no difference and the second phone has only one difference. -->
					<xsl:when test="$phone1[root[not(*)]] and $phone2[root[count(*) = 1]]">
						<xsl:value-of select="'true'" />
					</xsl:when>
					<!-- The second phone has no difference and the first phone has only one difference. -->
					<xsl:when test="$phone2[root[not(*)]] and $phone1[root[count(*) = 1]]">
						<xsl:value-of select="'true'" />
					</xsl:when>
					<!-- The two units have only one difference and it is the same feature. -->
					<xsl:when test="$phone1[root[count(*) = 1]] and $phone2[root[count(*) = 1]] and $phone1/root/feature and $phone2/root/feature and substring($phone1/root/feature, 2) = substring($phone2/root/feature, 2)">
						<xsl:value-of select="'true'" />
					</xsl:when>
					<!-- The two units have only one difference and it is the same class. -->
					<xsl:when test="$phone1[root[count(*) = 1]] and $phone2[root[count(*) = 1]] and $phone1/root/class and $phone2/root/class and $phone1/root/class/@name = $phone2/root/class/@name">
						<xsl:value-of select="'true'" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'false'" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<!-- Remove temporary elements from step 6a. -->
	<xsl:template match="similarPhonePairs/pair/phone/root" />
	<xsl:template match="similarPhonePairs/pair/phone/articulatoryFeatures" />
	
	<!-- Remove pattern which does not meet the sufficient condition. -->
	<xsl:template match="similarPhonePairs/pair/pattern[phone/sufficient/*]" />

	<!-- Make sure the pattern meets the necessary condition: -->
	<!-- the candidate pair has no other feature differences. -->
	<xsl:template match="similarPhonePairs/pair/pattern/phone">
		<xsl:variable name="this" select="." />
		<xsl:variable name="literal" select="@literal" />
		<xsl:variable name="phone" select="../../phone[@literal = $literal]" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:for-each select="$phone/articulatoryFeatures/feature">
				<xsl:variable name="feature" select="." />
				<xsl:variable name="subclass" select="@subclass" />
				<xsl:choose>
					<xsl:when test="$this/articulatoryFeatures/feature[. = $feature]" />
					<xsl:when test="$this/articulatoryFeatures/feature[@subclass = $subclass]" />
					<!-- The pattern does not match a feature difference of the candidate pair. -->
					<xsl:otherwise>
						<xsl:copy-of select="." />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>