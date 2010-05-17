<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- phonology_project_inventory_6a_similar_phone_pairs.xsl 2010-05-14 -->

	<xsl:variable name="classOfSortKey" select="'placeOfArticulation'" />

	<xsl:variable name="metadata" select="//div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/ul[@class = 'settings']" />
	<xsl:variable name="programConfigurationFolder" select="$settings/li[@class = 'programConfigurationFolder']" />
	<xsl:variable name="programPhoneticInventoryFile" select="$settings/li[@class = 'programPhoneticInventoryFile']" />

	<xsl:variable name="programPhoneticInventoryXML" select="concat($programConfigurationFolder, $programPhoneticInventoryFile)" />
	<xsl:variable name="hierarchicalFeatures" select="document($programPhoneticInventoryXML)/inventory/hierarchicalFeatures" />

	<!-- For now, the file is in the Processing folder. -->
	<!-- TO DO: When we decide what works well, move similar phone patterns to the program phonetic inventory file. -->
	<xsl:variable name="similarPhonePatternsXML" select="'similarPhonePatterns.xml'" />
	<xsl:variable name="similarPhonePatterns" select="document($similarPhonePatternsXML)/inventory/similarPhonePatterns" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

	<xsl:template match="inventory">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<similarPhonePairs>
				<xsl:call-template name="similarPhonePairs">
					<xsl:with-param name="units" select="units[@type = 'phonetic']" />
					<xsl:with-param name="type" select="'Consonant'" />
				</xsl:call-template>
				<!--
				<xsl:call-template name="similarPhonePairs">
					<xsl:with-param name="units" select="units[@type = 'phonetic']" />
					<xsl:with-param name="type" select="'Vowel'" />
				</xsl:call-template>
				-->
			</similarPhonePairs>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="similarPhonePairs">
		<xsl:param name="units" />
		<xsl:param name="type" />
		<xsl:for-each select="$units/unit[articulatoryFeatures/feature[. = $type]]">
			<xsl:sort select="keys/sortKey[@class = $classOfSortKey]" />
			<xsl:variable name="phone1" select="." />
			<xsl:for-each select="$units/unit[articulatoryFeatures/feature[. = $type]]">
				<xsl:sort select="keys/sortKey[@class = $classOfSortKey]" />
				<xsl:variable name="phone2" select="." />
				<xsl:if test="$phone1/keys/sortKey[@class = $classOfSortKey] &lt; $phone2/keys/sortKey[@class = $classOfSortKey]">
					<xsl:call-template name="similarPhonePair">
						<xsl:with-param name="type" select="$type" />
						<xsl:with-param name="phone1" select="$phone1" />
						<xsl:with-param name="phone2" select="$phone2" />
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="similarPhonePair">
		<xsl:param name="type" />
		<xsl:param name="phone1" />
		<xsl:param name="phone2" />
		<pair type="{$type}">
			<xsl:call-template name="similarPhone">
				<xsl:with-param name="phoneA" select="$phone1" />
				<xsl:with-param name="phoneB" select="$phone2" />
			</xsl:call-template>
			<xsl:call-template name="similarPhone">
				<xsl:with-param name="phoneA" select="$phone2" />
				<xsl:with-param name="phoneB" select="$phone1" />
			</xsl:call-template>
			<!-- Steps 6b and 6c evaluate similar pair patterns. -->
			<xsl:apply-templates select="$similarPhonePatterns/pattern">
				<xsl:with-param name="phoneA" select="$phone1" />
				<xsl:with-param name="phoneB" select="$phone2" />
			</xsl:apply-templates>
		</pair>
	</xsl:template>

	<xsl:template name="similarPhone">
		<xsl:param name="phoneA" />
		<xsl:param name="phoneB" />
		<phone literal="{$phoneA/@literal}">
			<!-- Step 6b evaluates candidate pairs using the hierarchical feature differences. -->
			<root>
				<xsl:apply-templates select="$phoneA/root/*" mode="root">
					<xsl:with-param name="rootOfOtherPhone" select="$phoneB/root" />
				</xsl:apply-templates>
			</root>
			<!-- The necessary condition for similar phone patterns -->
			<!-- depends on the articulatory feature differences between the candidate pair. -->
			<articulatoryFeatures>
				<xsl:for-each select="$phoneA/articulatoryFeatures/feature">
					<xsl:variable name="feature" select="." />
					<xsl:if test="not($phoneB/articulatoryFeatures/feature[. = $feature])">
						<xsl:copy-of select="." />
					</xsl:if>
				</xsl:for-each>
			</articulatoryFeatures>
		</phone>
	</xsl:template>

	<xsl:template match="class" mode="root">
		<xsl:param name="rootOfOtherPhone" />
		<xsl:variable name="name" select="@name" />
		<xsl:variable name="classOfOtherPhone" select="$rootOfOtherPhone//class[@name = $name]" />
		<xsl:choose>
			<!-- If the other phone does not have a corresponding class node, copy this. -->
			<xsl:when test="not($classOfOtherPhone)">
				<xsl:copy-of select="." />
			</xsl:when>
			<!-- If the children are class nodes but no feature nodes, skip this level of the hierarchy. -->
			<xsl:when test="class and not(feature) and not($classOfOtherPhone/feature)">
				<xsl:apply-templates mode="root">
					<xsl:with-param name="rootOfOtherPhone" select="$rootOfOtherPhone" />
				</xsl:apply-templates>
			</xsl:when>
			<!-- If the children are feature nodes but no class nodes. -->
			<xsl:when test="feature and not(class) and not($classOfOtherPhone/class)">
				<xsl:variable name="firstDifferenceInClassA">
					<xsl:apply-templates select="feature[1]" mode="firstDifferenceInClass">
						<xsl:with-param name="classOfOtherPhone" select="$classOfOtherPhone" />
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:variable name="firstDifferenceInClassB">
					<xsl:apply-templates select="$classOfOtherPhone/feature[1]" mode="firstDifferenceInClass">
						<xsl:with-param name="classOfOtherPhone" select="." />
					</xsl:apply-templates>
				</xsl:variable>
				<!-- If there are any differences in either direction, keep this layer of the hierarchy. -->
				<xsl:if test="string-length($firstDifferenceInClassA) != 0 or string-length($firstDifferenceInClassB) != 0">
					<xsl:copy>
						<xsl:apply-templates select="@*" />
						<xsl:apply-templates mode="root">
							<xsl:with-param name="rootOfOtherPhone" select="$rootOfOtherPhone" />
						</xsl:apply-templates>
					</xsl:copy>
				</xsl:if>
			</xsl:when>
			<!-- If no children or children consist of class and feature nodes. -->
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@*" />
					<xsl:apply-templates mode="root">
						<xsl:with-param name="rootOfOtherPhone" select="$rootOfOtherPhone" />
					</xsl:apply-templates>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="feature" mode="firstDifferenceInClass">
		<xsl:param name="classOfOtherPhone" />
		<xsl:variable name="feature" select="." />
		<xsl:choose>
			<xsl:when test="$classOfOtherPhone/feature[. = $feature]">
				<xsl:apply-templates select="following-sibling::feature[1]" mode="firstDifferenceInClass">
					<xsl:with-param name="classOfOtherPhone" select="$classOfOtherPhone" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="feature" mode="root">
		<xsl:param name="rootOfOtherPhone" />
		<xsl:variable name="feature" select="." />
		<xsl:if test="not($rootOfOtherPhone//feature[. = $feature])">
			<xsl:copy-of select="." />
		</xsl:if>
	</xsl:template>

	<!-- Step 6b determines whether a pattern matches either permutation of the candidate pair. -->
	<xsl:template match="similarPhonePatterns/pattern">
		<xsl:param name="phoneA" />
		<xsl:param name="phoneB" />
		<xsl:call-template name="similarPhonePatterns">
			<xsl:with-param name="phone1" select="$phoneA" />
			<xsl:with-param name="phone2" select="$phoneB" />
		</xsl:call-template>
		<xsl:call-template name="similarPhonePatterns">
			<xsl:with-param name="phone1" select="$phoneB" />
			<xsl:with-param name="phone2" select="$phoneA" />
		</xsl:call-template>
	</xsl:template>

	<!-- Insert the pattern in the candidate pair. -->
	<xsl:template name="similarPhonePatterns">
		<xsl:param name="phone1" />
		<xsl:param name="phone2" />
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates select="phone[1]">
				<xsl:with-param name="phone" select="$phone1" />
			</xsl:apply-templates>
			<xsl:apply-templates select="phone[2]">
				<xsl:with-param name="phone" select="$phone2" />
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<!-- Identify which phone of the candidate pair corresponds to a phone of the pattern. -->
	<xsl:template match="similarPhonePatterns/pattern/phone">
		<xsl:param name="phone" />
		<xsl:copy>
			<xsl:attribute name="literal">
				<xsl:value-of select="$phone/@literal" />
			</xsl:attribute>
			<!-- Copy the pattern features for the necessary condition in step 6b. -->
			<xsl:apply-templates />
			<!-- To evaluate the sufficient condition in step 6b, copy features that do not match. -->
			<sufficient>
				<xsl:apply-templates select="articulatoryFeatures/feature" mode="similarPhonePatterns">
					<xsl:with-param name="phone" select="$phone" />
				</xsl:apply-templates>
			</sufficient>
		</xsl:copy>
	</xsl:template>

	<!-- If the candidate phone does not have a feature, copy it. -->
	<xsl:template match="feature" mode="similarPhonePatterns">
		<xsl:param name="phone" />
		<xsl:variable name="feature" select="." />
		<xsl:if test="not($phone/articulatoryFeatures/feature[. = $feature])">
			<xsl:copy-of select="." />
		</xsl:if>
	</xsl:template>

	<!-- If the candidate phone does not have a feature, copy it. -->
	<xsl:template match="feature[@subclass]" mode="similarPhonePatterns">
		<xsl:param name="phone" />
		<xsl:variable name="subclass" select="@subclass" />
		<xsl:if test="not($phone/articulatoryFeatures/feature[@subclass = $subclass])">
			<xsl:copy-of select="." />
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>