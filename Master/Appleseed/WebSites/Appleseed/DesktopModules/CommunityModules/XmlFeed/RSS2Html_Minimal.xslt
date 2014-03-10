<?xml version="1.0"?><xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"><xsl:output      method="html"     doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN"     encoding="ISO-8859-1"     indent="yes"     />
<xsl:template match="/">  <xsl:apply-templates/></xsl:template><xsl:template match="channel">  <ul>  <xsl:apply-templates select="item"/>  </ul></xsl:template>
<xsl:template match="item">  <li>    <strong>      <a target="_blank">        <xsl:attribute name="href">          <xsl:value-of select="link"/>        </xsl:attribute>        <xsl:value-of select="title" />      </a>    </strong>  </li></xsl:template><xsl:template match="text()"/>
</xsl:stylesheet>
