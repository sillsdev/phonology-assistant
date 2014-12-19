<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
	# Name:        AddRhymeToDistribution.xslt
	# Purpose:     Add Rhyme, Onset, Nucleus and Code charts
	#
	# Author:      Greg Trihus <greg_trihus@sil.org>
	#
	# Created:     2014/12/17
	# Copyright:   (c) 2014 SIL International
	# Licence:     <MIT>
	################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0">
	
	<xsl:output method="xml"/>
	
	<xsl:template match="node()|@*">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
	</xsl:template>
	
  <xsl:template match="*[@name = 'ExVowels']">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
    <xsl:if test="following-sibling::*[1]/@name != 'Rhyme'">
    <chart name="Rhyme">
    <searchItems>
      <item>[V]</item>
      <item>[V][C]</item>
      <item>[V][C][C]</item>
      <item>[V][C][C][C]</item>
      <item>[V][V]</item>
      <item>[V][V][C]</item>
      <item>[V][V][C][C]</item>
      <item>[V][V][C][C][C]</item>
      <item>[V][V][V]</item>
      <item>[V][V][V][C]</item>
      <item>[V][V][V][C][C]</item>
      <item>[V][V][V][C][C][C]</item>
    </searchItems>
    <searchQueries>
      <query version="3" Pattern="[C]_#">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[C]_.">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="#_#">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="#_.">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="._#">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="._.">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="_ˈ">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="_'">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="_ʹ">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="_ˌ">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="_-">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
    </searchQueries>
    <columnWidths>
      <width>159</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
    </columnWidths>
  </chart>
  <chart name="Onset">
    <searchItems>
      <item>[C]</item>
      <item>[C][C]</item>
      <item>[C][C][C]</item>
      <item>[C][C][C][C]</item>
    </searchItems>
    <searchQueries>
      <query version="3" Pattern="#_[V]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="._[V]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="ˈ_[V]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="'_[V]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="ʹ_[V]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="ˌ_[V]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="-_[V]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
    </searchQueries>
    <columnWidths>
      <width>161</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
    </columnWidths>
  </chart>
  <chart name="Nucleus">
    <searchItems>
      <item>[V]</item>
      <item>[V][V]</item>
      <item>[V][V][V]</item>
    </searchItems>
    <searchQueries>
      <query version="3" Pattern="#_#">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[C]_#">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[C]_[C]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="#_[C]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[C]_.">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="._[C]">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="#_.">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="._#">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="._.">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
    </searchQueries>
    <columnWidths>
      <width>110</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
    </columnWidths>
  </chart>
  <chart name="Coda">
    <searchItems>
      <item>[C]</item>
      <item>[C][C]</item>
      <item>[C][C][C]</item>
    </searchItems>
    <searchQueries>
      <query version="3" Pattern="[V]_#">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[V]_.">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[V]_ˈ">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[V]_'">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[V]_ʹ">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[V]_ˌ">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
      <query version="3" Pattern="[V]_-">
        <IsPatternRegExpression>false</IsPatternRegExpression>
        <ShowAllOccurrences>true</ShowAllOccurrences>
        <IncludeAllUncertainPossibilities>false</IncludeAllUncertainPossibilities>
        <IgnoreDiacritics>true</IgnoreDiacritics>
        <ignoredCharacters>˥˧,˧˩,˦˥˦,˩˨,˦˥,˥˩,˩˥,',-,.,:,|,²,³,¹,ʹ,ˈ,ˌ,ː,ˑ,˥,˦,˧,˨,˩,̀,́,̂,̄,̆,̋,̌,̏,᷄,᷅,᷆,᷇,᷈,᷉,‖,‿,⁴,⁵,⁶,⁷,⁸,⁹,↑,↓,↗,↘,ꜛ,ꜜ</ignoredCharacters>
      </query>
    </searchQueries>
    <columnWidths>
      <width>159</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
      <width>100</width>
    </columnWidths>
  </chart>
    </xsl:if>
	</xsl:template>
	
</xsl:stylesheet>