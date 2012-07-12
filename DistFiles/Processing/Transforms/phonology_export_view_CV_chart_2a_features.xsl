<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xhtml="http://www.w3.org/1999/xhtml"
xmlns:svg="http://www.w3.org/2000/svg"
exclude-result-prefixes="xhtml svg"
>

  <!-- phonology_export_view_CV_chart_2a_features.xsl 2012-03-14 -->
	<!-- Export to XHTML, Interactive Web page, and at least one feature table. -->
  <!-- For each Phonetic data cell: -->
  <!-- * Wrap the literal segment in a span. -->
	<!-- * Copy lists of features from the project phonetic inventory file. -->
	<!-- Following the CV chart, insert feature tables. -->

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no" />

	<xsl:variable name="metadata" select="//xhtml:div[@id = 'metadata']" />
	<xsl:variable name="settings" select="$metadata/xhtml:ul[@class = 'settings']" />
	<xsl:variable name="options" select="$metadata/xhtml:ul[@class = 'options']" />
	<xsl:variable name="details" select="$metadata/xhtml:ul[@class = 'details']" />

	<xsl:variable name="view" select="$details/xhtml:li[@class = 'view']" />

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

	<xsl:variable name="programDistinctiveFeaturesFile" select="concat($settings/xhtml:li[@class = 'programDistinctiveFeaturesName'], '.DistinctiveFeatures.xml')" />
	<xsl:variable name="programDistinctiveFeaturesXML" select="concat($programConfigurationFolder, $programDistinctiveFeaturesFile)" />
	<xsl:variable name="programDistinctiveFeatures" select="document($programDistinctiveFeaturesXML)/inventory/featureDefinitions[@class = 'distinctive']" />
	<xsl:variable name="programFeatureDependencies" select="document($programDistinctiveFeaturesXML)/inventory/featureDependencies[@class = 'distinctive']" />
	<xsl:variable name="programFeatureContradictions" select="document($programDistinctiveFeaturesXML)/inventory/featureContradictions[@class = 'distinctive']" />

  <xsl:variable name="programDefaultDiagramSagittalFile" select="'default.DiagramSagittal.svg'" />

  <xsl:variable name="format" select="$options/xhtml:li[@class = 'format']" />
	<xsl:variable name="interactiveWebPage">
		<xsl:if test="$format = 'XHTML'">
			<xsl:value-of select="$options/xhtml:li[@class = 'interactiveWebPage']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="descriptiveFeatureTable">
		<xsl:if test="$interactiveWebPage = 'true'">
			<xsl:value-of select="$options/xhtml:li[@class = 'descriptiveFeatureTable']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="distinctiveFeatureTable">
		<xsl:if test="$interactiveWebPage = 'true'">
			<xsl:value-of select="$options/xhtml:li[@class = 'distinctiveFeatureTable']" />
		</xsl:if>
	</xsl:variable>
	<xsl:variable name="distinctiveFeatureChanges">
		<xsl:if test="$interactiveWebPage = 'true'">
			<xsl:value-of select="$options/xhtml:li[@class = 'distinctiveFeatureChanges']" />
		</xsl:if>
	</xsl:variable>
  <xsl:variable name="diagram">
    <xsl:if test="$interactiveWebPage = 'true'">
      <xsl:value-of select="$options/xhtml:li[@class = 'diagram']" />
    </xsl:if>
  </xsl:variable>
  <xsl:variable name="features">
		<xsl:if test="$descriptiveFeatureTable = 'true' or $distinctiveFeatureTable = 'true' or $diagram = 'true'">
			<xsl:value-of select="'true'" />
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="enDash" select="'&#x2013;'" />

	<!-- Copy all attributes and nodes, and then define more specific template rules. -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>
  
	<xsl:template match="xhtml:div[@class = 'side_by_side'][xhtml:div[@class = 'stacked']//xhtml:table[starts-with(@class, 'CV chart')]]">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:call-template name="distinctiveFeatureTable">
				<xsl:with-param name="br" select="count(xhtml:div[@class = 'stacked']//xhtml:table[starts-with(@class, 'CV chart')][1]/xhtml:thead/xhtml:tr/xhtml:th/xhtml:br)" />
			</xsl:call-template>
      <xsl:call-template name="diagram" />
    </xsl:copy>
	</xsl:template>

  <xsl:template match="xhtml:div[@class = 'side_by_side'][xhtml:div[@class = 'stacked']//xhtml:table[starts-with(@class, 'CV chart')]]/@class">
    <xsl:choose>
      <xsl:when test="$features = 'true'">
        <xsl:attribute name="class">
          <xsl:value-of select="." />
          <xsl:value-of select="' mediator'"/>
        </xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy-of select="." />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:table[starts-with(@class, 'CV chart')]">
		<xsl:choose>
			<xsl:when test="$features = 'true'">
				<xsl:variable name="type" select="substring-after(@class, 'CV chart ')" />
				<!-- Wrap in a div. -->
				<div class="side_by_side" xmlns="http://www.w3.org/1999/xhtml">
          <xsl:attribute name="class">
            <xsl:value-of select="'side_by_side'" />
            <xsl:if test="not(ancestor::xhtml:div[@class = 'stacked'])">
              <xsl:value-of select="' mediator'" />
            </xsl:if>
          </xsl:attribute>
					<xsl:copy>
						<xsl:apply-templates select="@* | node()" />
					</xsl:copy>
					<xsl:variable name="br" select="count(xhtml:thead/xhtml:tr/xhtml:th/xhtml:br)" />
					<xsl:if test="$descriptiveFeatureTable = 'true'">
						<xsl:variable name="categoryCol">
							<xsl:choose>
								<xsl:when test="$type = 'consonant'">
									<xsl:value-of select="'voicing'" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'rounding'" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<table class="descriptive features" xmlns="http://www.w3.org/1999/xhtml">
							<colgroup>
								<col />
							</colgroup>
							<thead>
								<tr>
									<th scope="colgroup" colspan="1">
										<xsl:if test="$br != 0">
											<br />
										</xsl:if>
										<span>
											<xsl:value-of select="'descriptive'" />
										</span>
									</th>
								</tr>
							</thead>
							<tbody>
								<xsl:apply-templates select="$programDescriptiveFeatures/featureDefinition[@category = $categoryCol]" mode="table" />
							</tbody>
							<xsl:if test="$type = 'vowel'">
								<tbody>
									<xsl:apply-templates select="$programDescriptiveFeatures/featureDefinition[@class = 'subtype']" mode="table" />
								</tbody>
							</xsl:if>
							<tbody>
								<xsl:apply-templates select="$programDescriptiveFeatures/featureDefinition[@class = 'row' or @class = 'rowConditional' or ($type = 'consonant' and @class = 'rowConditional-col')][not(@category = 'tone')]" mode="table" />
							</tbody>
							<tbody class="tone">
								<xsl:apply-templates select="$programDescriptiveFeatures/featureDefinition[@category = 'tone']" mode="table" />
							</tbody>
						</table>
					</xsl:if>
					<xsl:if test="not(ancestor::xhtml:div[@class = 'stacked'])">
						<xsl:call-template name="distinctiveFeatureTable">
							<xsl:with-param name="br" select="$br" />
							<xsl:with-param name="type" select="$type" />
						</xsl:call-template>
            <xsl:call-template name="diagram" />
					</xsl:if>
				</div>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" />
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="distinctiveFeatureTable">
		<xsl:param name="br" />
		<xsl:param name="type" />
		<xsl:if test="$distinctiveFeatureTable = 'true'">
			<xsl:variable name="class">
				<xsl:value-of select="'distinctive features'" />
				<xsl:if test="$distinctiveFeatureChanges = 'true'">
					<xsl:value-of select="' changes'" />
				</xsl:if>
			</xsl:variable>
			<table class="{$class}" xmlns="http://www.w3.org/1999/xhtml">
				<colgroup>
					<col />
					<col />
				</colgroup>
				<colgroup>
					<col />
				</colgroup>
				<thead>
					<tr>
						<th scope="colgroup" colspan="3">
							<xsl:if test="$br != 0">
								<br />
							</xsl:if>
							<span>
								<xsl:value-of select="'distinctive'" />
							</span>
						</th>
					</tr>
				</thead>
				<xsl:for-each select="$programDistinctiveFeatures/featureDefinition[@category]">
					<xsl:variable name="category" select="@category" />
					<xsl:if test="not(preceding-sibling::featureDefinition[@category = $category])">
						<tbody>
							<xsl:apply-templates select="$programDistinctiveFeatures/featureDefinition[@category = $category][not($type) or not(@type != $type)]" mode="table" />
						</tbody>
					</xsl:if>
				</xsl:for-each>
			</table>
			<xsl:if test="$programFeatureDependencies/featureDependency[ifAllOf[feature]][then[feature]]">
				<ul class="distinctive dependencies" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="$programFeatureDependencies/featureDependency[ifAllOf[feature]][then[feature]]">
						<li>
							<ul class="if">
								<xsl:for-each select="ifAllOf/feature">
									<li>
										<xsl:value-of select="." />
									</li>
								</xsl:for-each>
							</ul>
							<ul class="then">
								<xsl:for-each select="then/feature">
									<li>
										<xsl:value-of select="." />
									</li>
								</xsl:for-each>
							</ul>
						</li>
					</xsl:for-each>
				</ul>
			</xsl:if>
			<xsl:if test="$programFeatureContradictions/featureContradiction[feature]">
				<ul class="distinctive contradictions" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:for-each select="$programFeatureContradictions/featureContradiction[feature]">
						<li>
							<ul>
								<xsl:for-each select="feature">
									<li>
										<xsl:value-of select="." />
									</li>
								</xsl:for-each>
							</ul>
						</li>
					</xsl:for-each>
				</ul>
			</xsl:if>
		</xsl:if>
	</xsl:template>

  <xsl:template name="diagram">
    <xsl:if test="$diagram = 'true'">
      <xsl:variable name="diagramSVG" select="concat($programConfigurationFolder, $programDefaultDiagramSagittalFile)" />
      <div class="diagram" xmlns="http://www.w3.org/1999/xhtml">
        <div class="sagittal">
          <xsl:copy-of select="document($diagramSVG)/svg:svg" />
          <p>
            <span class="Phonetic">
              <xsl:value-of select="'&#xA;'" />
            </span>
            <span class="description">
              <xsl:value-of select="'&#xA;'" />
            </span>
          </p>
        </div>
      </div>
    </xsl:if>
  </xsl:template>

	<xsl:template match="xhtml:table[starts-with(@class, 'CV chart')]//xhtml:td[@class = 'Phonetic'][node()]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:apply-templates />
			<xsl:if test="$features = 'true'">
				<xsl:variable name="literal" select="xhtml:span" />
				<xsl:variable name="segment" select="$segments/segment[@literal = $literal]" />
				<xsl:if test="$segment">
					<xsl:if test="$descriptiveFeatureTable = 'true'">
						<xsl:variable name="features" select="$segment/features[@class = 'descriptive']" />
						<ul class="descriptive features" xmlns="http://www.w3.org/1999/xhtml">
							<xsl:apply-templates select="$features/feature" mode="list" />
						</ul>
					</xsl:if>
					<xsl:if test="$distinctiveFeatureTable = 'true'">
						<ul class="distinctive features" xmlns="http://www.w3.org/1999/xhtml">
							<xsl:apply-templates select="$segment/features[@class = 'distinctive']/feature" mode="list" />
						</ul>
					</xsl:if>
				</xsl:if>
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="feature" mode="list">
		<li xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="." />
		</li>
	</xsl:template>

	<xsl:template match="featureDefinitions[@class = 'descriptive']/featureDefinition" mode="table">
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>
        <xsl:value-of select="name" />
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="featureDefinitions[@class = 'distinctive']/featureDefinition" mode="table">
		<tr class="{@class}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:choose>
				<xsl:when test="@class = 'univalent'">
					<td colspan="2" />
				</xsl:when>
				<xsl:otherwise>
					<td>
						<xsl:value-of select="'+'" />
					</td>
					<td>
						<xsl:value-of select="$enDash" />
					</td>
				</xsl:otherwise>
			</xsl:choose>
			<td class="name">
				<xsl:if test="fullname and fullname != name">
					<xsl:attribute name="title">
						<xsl:value-of select="fullname" />
					</xsl:attribute>
				</xsl:if>
				<xsl:value-of select="name" />
			</td>
		</tr>
	</xsl:template>

</xsl:stylesheet>