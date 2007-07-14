<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
<xsl:output method="xml" encoding="utf-8" indent="yes" omit-xml-declaration="yes"/>
<!--Generates HTML table from XML export of word lists in Phonology Assistant Unicode version 3-->
<!-- Add title
min height 
min width
-->

<xsl:template match="/">
	<html>
		<head>
			<title>
				<xsl:value-of select="table/@language"/>
				<xsl:value-of select="table/@view"/>
				<xsl:text>Word List</xsl:text>
			</title>
			<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
			<style type="text/css">
				table {border-collapse: collapse;}
				td {border-style:solid; border-width:thin; border-color:black;font-family: Arial,sans-serif;}

				tr {min-height:30px;}
				td.colhead {font-family: Arial,sans-serif;}
				td.colhead {font-size: .8em;}
				td.colhead {font-weight: bold;}
				td.colhead {text-align: left;}
				td.colhead {padding: 3px;}

				tr {min-height:20px;}

				td.groupheadtext {padding: 3px;}
				td.groupheadtext {background-color: rgb(230,230,230);}
				td.groupheadtext {border-color: rgb(153, 153, 153);}
				td.groupheadtext {font-weight: bold;}
				/*~~|td.groupheadtext {Group-Head-Font-Name-Goes-Here}|~~*/
				/*~~|td.groupheadtext {Group-Head-Font-Size-Goes-Here}|~~*/

				td.groupheadcount {font-family: Arial,sans-serif;}
				td.groupheadcount {font-size: .8em;}
				td.groupheadcount {font-weight: bold;}
				td.groupheadcount {text-align: right;}
				td.groupheadcount {padding: 3px;}
				td.groupheadcount {background-color: rgb(230,230,230);}
				td.groupheadcount {border-color: rgb(153, 153, 153);}
				td.groupheadcount {border-left-style: hidden;}

				td.d {font-family: Arial,sans-serif;}
				td.d {font-size: 1.0em;}
				td.d {text-align: left;}
				td.d {width:1.5em;}
				td.d {height:1.5em;}
				td.d {border-width: 1px;}
				td.d {border-color: rgb(153, 153, 153);}

				td.d.phbefore {border-right: none;}
				td.d.phbefore {text-align: right;}
				td.d.phtarget {text-align: center;}
				td.d.phtarget {background-color: rgb(230,230,230);}
				td.d.phtarget {border-left: none;}
				td.d.phtarget {border-right: none;}
				td.d.phtarget {width: 1.0em;}
				td.d.phafter {border-left:none;}

				/*To override the font sizes used in Phonology Assistant, replace the # between*/
				/*square brackets with a numeric value which will be treated as an 'em' value. */
				/*All the non heading data in the HTML output will use that 'em' value.        */
				/*Do not delete the following line*/
				/*Alternate-Font-Size [#]*/

				/*Do not delete the following line*/
				/*Font-Settings-Go-Here*/
			</style>
		</head>
		<body>
			<xsl:apply-templates/>
		</body>
	</html>
</xsl:template>
<xsl:template match="table">
	<xsl:copy>
		<xsl:apply-templates select="@* | node()"/>
	</xsl:copy>
</xsl:template>
<xsl:template match="@* | node()">
	<xsl:copy>
		<xsl:apply-templates select="@class | @id | @rowspan | @colspan | node()"/>
	</xsl:copy>
</xsl:template>
</xsl:stylesheet>
