<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" indent="yes" /><xsl:param name="TITLE" />
	<xsl:template match="rss">
		<!-- Do not show channel image -->
		<xsl:for-each select="channel/item">
			<p class="NormalBold">
				<a href="{link}" target="_blank"><xsl:value-of select="title" /></a>
			</p>
			<!-- only display markup for description if it's present -->
			<xsl:value-of select="description" disable-output-escaping="yes" />
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="image">
		<div>
			<a target="_blank" href="{link}" title="{description}">
				<img src="{url}" border="0" alt="{title}" align="right" />
			</a>
		</div>
	</xsl:template>

	<xsl:template match="description">
		<p class="note">
			<small><xsl:value-of select="."/></small>
		</p>
	</xsl:template>

</xsl:stylesheet>