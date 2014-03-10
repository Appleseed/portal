<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output 
     method="html"
     doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN"
     encoding="ISO-8859-1"
     indent="yes"
     />

<xsl:template match="/">
  <xsl:apply-templates/>
</xsl:template>

<xsl:template match="channel">
  <div class="header">
  <h1>
    <a>
    <xsl:attribute name="href">
      <xsl:value-of select="link" />
    </xsl:attribute>
    <xsl:attribute name="title">
      <xsl:value-of select="title" disable-output-escaping="no" />
    </xsl:attribute>
    <xsl:value-of select="title" disable-output-escaping="no" />
    </a>
  </h1>
  <p class="channeldesc">
    <xsl:value-of select="description" disable-output-escaping="no" />
  </p>
  <p>
  <xsl:apply-templates select="image"/>
  </p>
  </div>
  <ul>
  <xsl:apply-templates select="item"/>
  </ul>
</xsl:template>

<xsl:template match="image">
    <a>
    <xsl:attribute name="href">
    <xsl:value-of select="link"/>
    </xsl:attribute>
    <img>
    <xsl:attribute name="src">
      <xsl:value-of select="url"/>
    </xsl:attribute>
    <xsl:attribute name="alt">
      <xsl:value-of select="title" disable-output-escaping="no" />
    </xsl:attribute>
    <xsl:attribute name="width">
      <xsl:value-of select="width"/>
    </xsl:attribute>
    <xsl:attribute name="height">
      <xsl:value-of select="height" disable-output-escaping="no" />
    </xsl:attribute>
    </img>
    </a>
</xsl:template>

<xsl:template match="item">
<br/><li>
    <strong>
      <a>
        <xsl:attribute name="href">
          <xsl:value-of select="link"/>
        </xsl:attribute>
	<xsl:value-of select="title" disable-output-escaping="no" />
      </a>
    </strong>
	
	<!-- only display markup for description if it's present -->
    <xsl:apply-templates select="description"/>
  </li>
</xsl:template>

<xsl:template match="description">
		<br>
			<xsl:value-of select="." disable-output-escaping="no" />
		</br>
</xsl:template>

<xsl:template match="text()"/>

</xsl:stylesheet>
