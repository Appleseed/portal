<?xml version="1.0"?>
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:Appleseed="urn:Appleseed">
	<xsl:output method="html"  version="4.0" indent="yes" encoding="utf-8"></xsl:output>
	
	<xsl:param name="Lang" select="'en'"></xsl:param>
	<xsl:variable name="DataCulture" select="//sales/@dataculture"></xsl:variable>
	
	<xsl:template match="/">
		<table width="90%" border="1pt" cellspacing="0" cellpadding="3" bordercolor="#dddddd" style="border-collapse:collapse;">
			<tr>
				<xsl:for-each select="sales/columns/column">
					<td class="normal" align="center" nowrap="nowrap">
						<xsl:value-of select="label[lang($Lang)]"/>
					</td>
				</xsl:for-each>
			</tr>
			<xsl:for-each select="sales/products/product">
				<tr>
					<td class="Normal" width="150" nowrap="nowrap">
						<i>
							<xsl:value-of select="label[lang($Lang)]"/>
						</i>
					</td>
					<td class="Normal" align="right" nowrap="nowrap">
							<xsl:value-of select="Appleseed:FormatNumber(revenue, $DataCulture, 'C2')"/>
					</td>
					<td class="Normal" align="right" nowrap="nowrap">
							<xsl:value-of select="Appleseed:FormatNumber(growth, $DataCulture, 'P2')"/>
					</td>
				</tr>
			</xsl:for-each>
		</table>
		<br/>
		

	</xsl:template>
</xsl:stylesheet>
