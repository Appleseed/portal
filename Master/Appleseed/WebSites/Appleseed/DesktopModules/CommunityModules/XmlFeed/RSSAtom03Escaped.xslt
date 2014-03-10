<?xml version="1.0" encoding="utf-8"?>
<!--
  Title: Atom 0.3 XSL Template
  Author: Rich Manalang (http://manalang.com)
  Description: This sample XSLT will convert any valid Atom 0.3 feed to HTML.
-->
<xsl:stylesheet version="1.0"
  xmlns:atom="http://purl.org/atom/ns#"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:dc="http://purl.org/dc/elements/1.1/">
  	<xsl:output method="html"/>
	<xsl:template match="/">
		<style>
			<xsl:comment>
			.syndication-content-area {
			}
			.syndication-title {
				font-size: 1.1em;
				font-weight: bold;
			}
			.syndication-description {
				font-size: .9em;
				margin: 0 0 10px 0;
			}
			.syndication-list {
				font-size: .8em;
				margin:0 0 0 20px;
			}
			.syndication-list-item {
				margin: 0 0 5px 0;
			}
			.syndication-list-item a,
			.syndication-list-item a:link {
				color: blue;
			}
			.syndication-list-item a:active,
			.syndication-list-item a:hover {
				color: red;
			}
			.syndication-list-item a:visited {
				color: black;
				text-decoration: none;
			}
			.syndication-list-item-date {
				font-size: .8em;
			}
			.syndication-list-item-description {
				font-size: .9em;
			}
			</xsl:comment>
		</style>
		<xsl:apply-templates select="/atom:feed"/>
	</xsl:template>
	<xsl:template match="/atom:feed">
		<div class="syndication-content-area">
			<div class="syndication-title">
				<xsl:value-of select="atom:title"/>
			</div>
			<div class="syndication-description">
				<xsl:value-of select="atom:tagline"/>
			</div>
			<ul class="syndication-list">
				<xsl:apply-templates select="atom:entry"/>
			</ul>
		</div>
	</xsl:template>
	<xsl:template match="atom:entry">
		<li class="syndication-list-item">
			<a href="{atom:link/@href}" target="_blank">
				<xsl:value-of select="atom:title" disable-output-escaping="yes"/>
			</a>
			<span class="syndication-list-item-date">
		 		(<xsl:value-of select="atom:issued"/>)
			</span>
			<div class="syndication-list-item-description">
				<xsl:value-of select="atom:summary"/>
			</div>
		</li>
	</xsl:template>
</xsl:stylesheet>