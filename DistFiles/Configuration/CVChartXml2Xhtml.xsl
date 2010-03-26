<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
<xsl:output method="xml" encoding="utf-8" indent="yes" omit-xml-declaration="yes"/>
<!--Generates HTML table from XML export of charts in Phonology Assistant Unicode version 3-->
<!-- Add title
min height 
min width
-->

<xsl:template match="/">
<html>
	<head>
		<title>
			<xsl:value-of select="table/@languageName"/><xsl:value-of select="table/@chartType"/><xsl:text>Chart</xsl:text>
		</title>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
		<style type="text/css">
			table {border-collapse: collapse;}

			td {border-style:solid; border-width:thin; border-color:black;font-family: Arial,sans-serif;}
			tr {min-height:20px;}
			.colheadp, .colheadc, .colheads {
			font-family: Arial,sans-serif;
			font-size: 0.8em;
			font-weight: bold;
			text-align: center;
			padding: 3px;
			background-color: rgb(230,230,230);
			}

			.rowheadp, .rowheads, .rowheadc {
			font-family: Arial,sans-serif;
			font-size: 0.8em;
			font-weight: bold;
			text-align: right;
			padding: 3px;
			background-color: rgb(245, 245, 245);
			}

			td.d {text-align: center;}
			td.d {width: 1.5em;}
			td.d {height: 1.5em;}
			td.d {border-width: 1px;}
			td.d {border-color: rgb(153, 153, 153);}

			/*The value in square brackets is the 'em' value used for the phonetic font size  */
			/*for the phonetic data in the chart. Increase or decrease the number between the */
			/*square brackets as desired or put nothing between them to use the phonetic font */
			/*size specified in Phonology Assistant. */
			/*Do not delete the following line*/
			/*Alternate-Phonetic-Font-Size [1.5]*/

			/*The following is where PA inserts phonetic font information. */
			/*Do not delete or edit the following 3 lines. */
			/*~~|td.d{Phonetic-Font-Name-Goes-Here}|~~*/
			/*~~|td.d{Phonetic-Font-Size-Goes-Here}|~~*/
			/*--|td.d{font-weight: bold;}|--*/
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
