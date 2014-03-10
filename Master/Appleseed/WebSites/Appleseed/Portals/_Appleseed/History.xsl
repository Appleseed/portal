<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" version="4.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<style>
			/* styling the DevelopmentHistory */
			div.DevelHistory table{
			padding:0 0.5em 0 0.5em;
			font-size: 11pt;
			}
			div.DevelHistory th{
			background-color: #ececec;
			font-family: Tahoma, sans-serif;
			color: navy;
			text-decoration: none;
			vertical-align: top;
			}
			div.DevelHistory tr{
			text-align: left;
			font-family: Tahoma, sans-serif;
			color: black;
			text-decoration: none;
			background-color: white;
			}
			div.DevelHistory h3{
			font-family: Tahoma, sans-serif;
			font-size: 13pt;
			font-style: italic;
			font-weight: bold;
			color: Olive;
			text-decoration: underline;
			text-align: left;
			}
			div.DevelHistory h4{
			font-family: Tahoma, sans-serif;
			font-size: 12em;
			font-style: italic;
			font-weight: bold;
			color: white;
			text-decoration: none;
			text-align: left;
			background-color: Olive;
			margin: 1em 0 0em 0;
			}
			div.DevelHistory h5{
			color: red;
			}

			div.DevelHistory tr.dev{background-color: #6A6A6A;color:#efefef;}
			div.DevelHistory tr.enh{background-color: #333333;color:#efefef;}
			div.DevelHistory tr.bug{background-color: #666666;color:#efefef;}
			div.DevelHistory tr.fix{background-color: #AAAAAA;color:#333333;}
			div.DevelHistory tr.release{background-color: #FFFFFF;color:#111111;}

			div.DevelHistory td.fix{
			font-family: Tahoma, sans-serif;
			color: navy;
			text-decoration: none;
			vertical-align: top;
			}
			div.DevelHistory td.release{
			font-family: Tahoma, sans-serif;
			color: navy;
			text-decoration: none;
			vertical-align: top;
			}
			div.DevelHistory td.bug{
			font-family: Tahoma, sans-serif;
			color: navy;
			text-decoration: none;
			vertical-align: top;
			}
			div.DevelHistory td.dev
			{
			font-family: Tahoma, sans-serif;
			color: navy;
			text-decoration: none;
			vertical-align: top;
			}


			.commentLeftTD{border:1px solid black; white-space:nowrap;text-align:center; padding:5px;
			color:red; font-weight:normal; background: #deebff; font-size:9pt; text-transform:uppercase;
			}
			.commentRightTD{border:1px solid black; text-align:left; padding:5px; vertical-align:top; font-size:9pt;}
		</style>
		<div class="DevelHistory">
			<xsl:for-each select="//Version">
				<xsl:sort order="descending" select="@number"/>
				<xsl:variable name="mainver" select="@number" />
				<h3>
					Version: <xsl:value-of select="$mainver"/>
				</h3>
				<table>
					<tr>
						<th>Date:</th>
						<td>
							<xsl:value-of select="@date"/>
						</td>
					</tr>
					<tr>
						<th>Remarks:</th>
						<td>
							<xsl:copy-of select="remarks/*"/>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<blockquote>
								<table>
									<xsl:for-each select="Subversion">
										<xsl:sort order="descending" select="@number"/>
										<tr>
											<th colspan="2">
												Sub-Version: <xsl:value-of select="$mainver"/>.<xsl:value-of select="@number"/>
											</th>
										</tr>
										<tr>
											<th>Author:</th>
											<th>
												<xsl:value-of select="@author"/>
											</th>
										</tr>
										<tr>
											<th>Remarks:</th>
											<th>
												<xsl:copy-of select="remarks/*"/>
											</th>
										</tr>
										<xsl:for-each select="comment">
											<tr>
												<xsl:attribute name="class">
													<xsl:value-of select="@type"/>
												</xsl:attribute>
												<td class="commentLeftTD">
													Date: <xsl:value-of select="@date"/>
													<br />
													<b>
														<xsl:value-of select="@type"/>
													</b>
												</td>
												<td class="commentRightTD">
													<xsl:value-of select="."/>
												</td>
											</tr>
										</xsl:for-each>
									</xsl:for-each>
								</table>
							</blockquote>
						</td>
					</tr>

				</table>
			</xsl:for-each>
		</div>
	</xsl:template>
</xsl:stylesheet>
