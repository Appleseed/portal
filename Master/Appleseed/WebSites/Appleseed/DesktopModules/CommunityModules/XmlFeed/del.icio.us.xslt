<?xml version="1.0" ?> 

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"> 

<xsl:output method="xml" omit-xml-declaration="yes" encoding="UTF-8" indent="yes"/>

<xsl:comment> XSL RDF/del.icio.us parser for use </xsl:comment>
<xsl:comment> with XMLfeed Appleseed module        </xsl:comment>
<xsl:comment> Created by Ed Daniel 1/8/2005      </xsl:comment>

<xsl:template match="*"> 

<ul> 
<xsl:for-each select="*[local-name()='item']"> 
<xsl:comment> Change this value to increase/decrease number of items displayed </xsl:comment>
<xsl:if test="position()&lt;=20"> 
<li> 
<p>
<a> <xsl:attribute name="href"> <xsl:value-of select="*[local-name()='link']"/> 
</xsl:attribute> <xsl:value-of select="*[local-name()='title']"/> </a>
<p> <xsl:attribute name="href"> </xsl:attribute> <xsl:value-of select="*[local-name()='description']"/> </p>
</p> 
</li> 
</xsl:if> 
</xsl:for-each> 
</ul>
  
</xsl:template> 
<xsl:template match="/"> 
<xsl:apply-templates/> 
</xsl:template> 
</xsl:stylesheet>