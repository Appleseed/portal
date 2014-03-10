<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" exclude-result-prefixes="Appleseed" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:Appleseed="urn:Appleseed">
	<xsl:output method="xml" version="1.0" omit-xml-declaration="yes" indent="no"/>
	<xsl:param name="ClientScriptLocation"/>
	<xsl:param name="Orientation"/>
	<xsl:param name="ActivePageId">1</xsl:param>
	<xsl:param name="ContainerCssClass"/>
	<xsl:param name="UsePathTraceInUrl"/>
	<xsl:param name="UsePageNameInUrl"/>
	<xsl:variable name="myBranch" select="/MenuData/MenuGroup/MenuItem[descendant-or-self::MenuItem[@ID = $ActivePageId]]"/>
	<xsl:template match="/">
		<xsl:element name="div">
			<xsl:attribute name="class"><xsl:value-of select="$ContainerCssClass"/></xsl:attribute>
			<xsl:apply-templates select="$myBranch/MenuGroup"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="MenuGroup">
		<xsl:element name="ul">
			<xsl:apply-templates/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="MenuItem">
		<xsl:variable name="pathtrace">
			<xsl:for-each select="ancestor-or-self::*[attribute::UrlPageName]">
				<xsl:value-of select="string(@UrlPageName)"/>
				<xsl:text>/</xsl:text>
			</xsl:for-each>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="Appleseed:CheckRoles(string(@AuthRoles))">
				<xsl:element name="li">
					<xsl:element name="a">
						<xsl:choose>
							<xsl:when test="$UsePageNameInUrl='true' and $UsePathTraceInUrl='true'">
								<xsl:attribute name="href"><xsl:value-of select="Appleseed:BuildUrl(string(@UrlPageName),number(@ID),$pathtrace)"/></xsl:attribute>
							</xsl:when>
							<xsl:when test="$UsePageNameInUrl='true' and $UsePathTraceInUrl='false'">
								<xsl:attribute name="href"><xsl:value-of select="Appleseed:BuildUrl(string(@UrlPageName),number(@ID))"/></xsl:attribute>
							</xsl:when>
							<xsl:when test="$UsePageNameInUrl='false' and $UsePathTraceInUrl='true'">
								<xsl:attribute name="href"><xsl:value-of select="Appleseed:BuildUrl(number(@ID),$pathtrace)"/></xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="href"><xsl:value-of select="Appleseed:BuildUrl(number(@ID))"/></xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							<xsl:when test=".//MenuItem">
								<xsl:choose>
									<xsl:when test="@ID=$ActivePageId">
										<xsl:attribute name="name"><xsl:text>daddy</xsl:text></xsl:attribute>
										<xsl:attribute name="class"><xsl:text>daddy MenuItemSelected</xsl:text></xsl:attribute>
									</xsl:when>
									<xsl:otherwise>
										<xsl:attribute name="name"><xsl:text>daddy</xsl:text></xsl:attribute>
										<xsl:attribute name="class"><xsl:text>daddy</xsl:text></xsl:attribute>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="@ID=$ActivePageId">
								<xsl:attribute name="class"><xsl:text>MenuItemSelected</xsl:text></xsl:attribute>
							</xsl:when>
						</xsl:choose>
						<xsl:value-of select="@PageName"/>
					</xsl:element>
					<xsl:apply-templates/>
				</xsl:element>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
