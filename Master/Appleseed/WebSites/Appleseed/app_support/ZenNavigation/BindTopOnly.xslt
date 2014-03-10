<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" exclude-result-prefixes="Appleseed" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:Appleseed="urn:Appleseed">
	<xsl:output method="xml" version="1.0" omit-xml-declaration="yes" indent="no"/>
	<xsl:param name="ClientScriptLocation"/>
	<xsl:param name="ActivePageId"/>
	<xsl:param name="Orientation"/>
	<xsl:param name="ContainerCssClass"/>
	<xsl:param name="UsePathTraceInUrl"/>
	<xsl:param name="UsePageNameInUrl"/>
	<xsl:template match="/">
		<xsl:element name="div">
			<xsl:attribute name="class"><xsl:value-of select="$ContainerCssClass"/></xsl:attribute>
			<ul>
				<xsl:apply-templates select="MenuData/MenuGroup"/>
			</ul>
		</xsl:element>
	</xsl:template>
	<xsl:template match="MenuItem[@ParentPageId='0']">
    <xsl:variable name="pathtrace">
      <xsl:for-each select="ancestor-or-self::*[attribute::UrlPageName]">
        <xsl:value-of select="string(@UrlPageName)"/>
        <xsl:text>/</xsl:text>
      </xsl:for-each>
    </xsl:variable>
		<xsl:choose>
			<xsl:when test="Appleseed:CheckRoles(string(@AuthRoles))">
				<xsl:element name="li">
						<xsl:choose>
							<xsl:when test="descendant-or-self::MenuItem[@ID=$ActivePageId]">
								<xsl:attribute name="class"><xsl:text>MenuItemSelected</xsl:text></xsl:attribute>
							</xsl:when>
						</xsl:choose>
					<xsl:element name="a">
						<xsl:choose>
              <xsl:when test="$UsePageNameInUrl='true' and $UsePathTraceInUrl='true'">
                <xsl:attribute name="href">
                  <xsl:value-of select="Appleseed:BuildUrl(string(@UrlPageName),number(@ID),$pathtrace)"/>
                </xsl:attribute>
              </xsl:when>
							<xsl:when test="$UsePageNameInUrl = 'true'">
								<xsl:attribute name="href"><xsl:value-of select="Appleseed:BuildUrl(string(@UrlPageName),number(@ID))"/></xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="href"><xsl:value-of select="Appleseed:BuildUrl(number(@ID))"/></xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							<xsl:when test="descendant-or-self::MenuItem[@ID=$ActivePageId]">
								<xsl:attribute name="class"><xsl:text>MenuItemSelected</xsl:text></xsl:attribute>
							</xsl:when>
						</xsl:choose>
						<xsl:value-of select="@PageName"/>
					</xsl:element>
				</xsl:element>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
