<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

	<xsl:template match="PaProject">
		<PaProject>
			<xsl:attribute name="version">
				<xsl:value-of select="3.3"/>
			</xsl:attribute>
			<xsl:if test="@ShowUndefinedCharsDlg">
				<xsl:element name="ShowUndefinedCharsDlg">
					<xsl:value-of select="@ShowUndefinedCharsDlg" />
				</xsl:element>
			</xsl:if>
			<xsl:apply-templates />
		</PaProject>
	</xsl:template>

	<xsl:template match="ProjectName">
		<name>
			<xsl:apply-templates/>
		</name>
	</xsl:template>
	
	<xsl:template match="Language">
		<languageName>
			<xsl:apply-templates/>
		</languageName>
	</xsl:template>

	<!-- ignore -->
	<xsl:template match="FwDataSourceInfo/WritingSystemInfo" />
	<xsl:template match="FwDataSourceInfo/PhoneticStorageMethod" />
	<xsl:template match="FwDataSourceInfo/IsMissing" />
	<xsl:template match="SFMappings" />

	<xsl:template match="DataSourceType">
		<Type>
			<xsl:apply-templates/>
		</Type>
	</xsl:template>

	<xsl:template match="FwDataSourceInfo">
		<FwDataSourceInfo>
			<xsl:for-each select="@*">
				<xsl:attribute name="{name()}">
					<xsl:value-of select="."/>
				</xsl:attribute>
			</xsl:for-each>

			<xsl:attribute name="PhoneticStorageMethod">
				<xsl:value-of select="PhoneticStorageMethod" />
			</xsl:attribute>

			<xsl:attribute name="IsMissing">
				<xsl:value-of select="IsMissing" />
			</xsl:attribute>

			<xsl:apply-templates />
		</FwDataSourceInfo>
	</xsl:template>
	
	<xsl:template match="DataSource">
		<xsl:copy>
				<xsl:if test="DataSourceType='FW'">
				<FieldMappings>
					<xsl:for-each select="./FwDataSourceInfo/WritingSystemInfo/FieldWsInfo">
						<xsl:if test="@Ws!='0'">
							<mapping>
								<paFieldName>
									<xsl:choose>
										<xsl:when test="@FieldName='phonetic'">Phonetic</xsl:when>
										<xsl:when test="@FieldName='phonemic'">Phonemic</xsl:when>
										<xsl:when test="@FieldName='gloss1'">Gloss</xsl:when>
										<xsl:when test="@FieldName='gloss2'">Gloss-Secondary</xsl:when>
										<xsl:when test="@FieldName='gloss3'">Gloss-Other</xsl:when>
										<xsl:when test="@FieldName='ortho'">Orthographic</xsl:when>
										<xsl:when test="@FieldName='pos'">PartOfSpeech</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="@FieldName" />
										</xsl:otherwise>		
									</xsl:choose>
								</paFieldName>
								<fwWritingSystem>
									<xsl:value-of select="@Ws" />
								</fwWritingSystem>
							</mapping>
						</xsl:if>
					</xsl:for-each>
				</FieldMappings>
			</xsl:if>

			<xsl:if test="DataSourceType='SFM' or DataSourceType='Toolbox'">
				<SfmRecordMarker>
					<xsl:value-of select="./SFMappings/Mapping[@FieldName='RecMrkr']/@Marker" />
				</SfmRecordMarker>
			</xsl:if>
			<xsl:apply-templates />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="SFMappings">
		<FieldMappings>
			<xsl:apply-templates/>
		</FieldMappings>
	</xsl:template>

	<xsl:template match="Mapping">
		<xsl:if test="@Marker!='' and @FieldName!='RecMrkr' and @FieldName!=''">
			<mapping>
				<xsl:attribute name="nameInSource">
					<xsl:value-of select="@Marker" />
				</xsl:attribute>
				<paFieldName>
					<xsl:choose>
						<xsl:when test="@FieldName='Secondary Gloss'">Gloss-Secondary</xsl:when>
						<xsl:when test="@FieldName='Other Gloss'">Gloss-Other</xsl:when>
						<xsl:when test="@FieldName='Part of Speech'">PartOfSpeech</xsl:when>
						<xsl:when test="@FieldName='Audio File'">AudioFile</xsl:when>
						<xsl:when test="@FieldName='CV Pattern'">CV-Pattern-Source</xsl:when>
						<xsl:when test="@FieldName='Ethnologue Id'">EthnologueId</xsl:when>
						<xsl:when test="@FieldName='Language Name'">LanguageName</xsl:when>
						<xsl:when test="@FieldName='Notebook Ref.'">NoteBookReference</xsl:when>
						<xsl:when test="@FieldName='Free Translation'">FreeFormTranslation</xsl:when>
						<xsl:when test="@FieldName='Comment'">Note</xsl:when>
						<xsl:when test="substring(@FieldName, 9)='s Gender'">SpeakerGender</xsl:when>
						<xsl:when test="@FieldName='Speaker'">SpeakerName</xsl:when>
						<xsl:when test="@FieldName='GUID'">Guid</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="@FieldName" />
						</xsl:otherwise>
					</xsl:choose>
				</paFieldName>
				<isInterlinear>
					<xsl:value-of select="@IsInterlinear" />
				</isInterlinear>
			</mapping>
		</xsl:if>
	</xsl:template>

	<xsl:template match="SortInformationList">
		<sortFields>
			<xsl:apply-templates/>
		</sortFields>
	</xsl:template>

	<xsl:template match="SortInformation">
		<field>
			<xsl:attribute name="name">
				<xsl:choose>
					<xsl:when test="FieldInfo/@Name='Secondary Gloss'">Gloss-Secondary</xsl:when>
					<xsl:when test="FieldInfo/@Name='Other Gloss'">Gloss-Other</xsl:when>
					<xsl:when test="FieldInfo/@Name='Part of Speech'">PartOfSpeech</xsl:when>
					<xsl:when test="FieldInfo/@Name='Audio File'">AudioFile</xsl:when>
					<xsl:when test="FieldInfo/@Name='CV Pattern'">CV-Pattern-Source</xsl:when>
					<xsl:when test="FieldInfo/@Name='Ethnologue Id'">EthnologueId</xsl:when>
					<xsl:when test="FieldInfo/@Name='Language Name'">LanguageName</xsl:when>
					<xsl:when test="FieldInfo/@Name='Notebook Ref.'">NoteBookReference</xsl:when>
					<xsl:when test="FieldInfo/@Name='Free Translation'">FreeFormTranslation</xsl:when>
					<xsl:when test="FieldInfo/@Name='Comment'">Note</xsl:when>
					<xsl:when test="substring(FieldInfo[@Name], 9)='s Gender'">SpeakerGender</xsl:when>
					<xsl:when test="FieldInfo/@Name='Speaker'">SpeakerName</xsl:when>
					<xsl:when test="FieldInfo/@Name='Data Source'">DataSource</xsl:when>
					<xsl:when test="FieldInfo/@Name='Data Source Path'">DataSourcePath</xsl:when>
					<xsl:when test="FieldInfo/@Name='GUID'">Guid</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="FieldInfo/@Name" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ascending">
				<xsl:value-of select="ascending"/>
			</xsl:attribute>
		</field>
	</xsl:template>

</xsl:stylesheet>
