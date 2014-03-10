<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
<!--/********************************************************
 * This XSL file was created by Scott Watermasysk 
 * for usage by KBAlertz.com. 
 * 
 * Please contact me at: Scott@TripleASP.Net if you have
 * any questions, comments, or suggestions. 
 *
 * For more information: 
 * http://TripleASP.Net/KBAlertz.aspx
 * http://KBAlertz.com/Webmaster
********************************************************/-->
    <xsl:template match="/">
    <table width="100%" border="0" cellspacing="0" cellpadding="3" bordercolor="#dddddd" style="border-collapse:collapse;">
        <xsl:for-each select='NewDataSet/Table'>
            <tr>
                <td class="Normal" width="100%">
			<xsl:element name="a">
				<xsl:attribute name="href">
					<xsl:value-of select="Url"/>
				</xsl:attribute>
				<xsl:value-of select="Title"/>
			</xsl:element>
                </td>
            </tr>
        </xsl:for-each>
    </table>
    </xsl:template>    
</xsl:stylesheet>
