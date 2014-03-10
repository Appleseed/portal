/*
 * FCKEditor File Manager Connector for Appleseed
 * Based on original FileBrowserConnector.cs from Frederico Caldeira Knabben (fredck@fckeditor.net)
 * 
 * Author : José Viladiu (jviladiu@portalServices.net) 2004/11/09
 */

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Web;
using System.Xml;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Security;
using Appleseed.Framework.Web.UI;
using Path = Appleseed.Framework.Settings.Path;

namespace Appleseed.FCKeditorV2
{
	/// <summary>
	/// FCk File Browser Connector
	/// This class generates the xml for use in the FCK file browser windows.
	/// </summary>
	public partial class FileBrowserConnector : EditItemPage //System.Web.UI.Page
	{
		private string sUserFilesPath ;
		private string sUserFilesDirectory ;

		/// <summary>
		/// Load settings
		/// </summary>
		protected override void LoadSettings()
		{
            int modId = this.PortalSettings.ActiveModule;
            if (modId < 1) {

                modId = getModId();

            }

            if (PortalSecurity.HasEditPermissions(modId) == false)
                PortalSecurity.AccessDeniedEdit();
		}

        private int getModId() {

            try {

                var query = HttpContext.Current.Request.UrlReferrer.Query;
                if (string.IsNullOrEmpty(query)) {
                    PortalSettings portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                    if (portalSettings != null)
                        return portalSettings.ActiveModule;
                }
                // Trying to get the ModId from the querystring of the urlReferedPage
                var midIndex = query.IndexOf("mid");
                if (midIndex > -1) {
                    query = query.Substring(midIndex);
                    // Could be mid=x&a=b&c=d or mid=x 

                    var index = query.IndexOf("&");
                    if (index > -1) {
                        query = query.Substring(0, index);
                    }
                    // query = mid=x
                    index = query.IndexOf("=");
                    query = query.Substring(index + 1);
                    //query = x = mid as string
                    var modid = int.Parse(query);
                    PortalSettings portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                    if (portalSettings != null) {
                        portalSettings.ActiveModule = modid;
                        HttpContext.Current.Items["PortalSettings"] = portalSettings;
                    }
                    return modid;
                }
                return 0;


            }
            catch (Exception) {
                return 0;
            }
        
        }

		/// <summary>
		/// Handles OnLoad event at Page level<br/>
		/// Performs OnLoad actions that are common to all Pages.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			// Get the main request informaiton.
			string sCommand = Request.QueryString["Command"] ;
			if ( sCommand == null ) return ;

			string sResourceType = Request.QueryString["Type"] ;
			if ( sResourceType == null ) return ;

			string sCurrentFolder = Request.QueryString["CurrentFolder"] ;
			if ( sCurrentFolder == null ) return ;

			// Check the current folder syntax (must begin and start with a slash).
			if ( ! sCurrentFolder.EndsWith( "/" ) )
				sCurrentFolder += "/" ;
			if ( ! sCurrentFolder.StartsWith( "/" ) )
				sCurrentFolder = "/" + sCurrentFolder ;

			// File Upload doesn't have to return XML, so it must be intercepted before anything.
			if ( sCommand == "FileUpload" )
			{
				this.FileUpload( sResourceType, sCurrentFolder ) ;
				return ;
			}

			// Cleans the response buffer.
			Response.ClearHeaders() ;
			Response.Clear() ;

			// Prevent the browser from caching the result.
			Response.CacheControl = "no-cache" ;

			// Set the response format.
			Response.ContentEncoding	= UTF8Encoding.UTF8 ;
			Response.ContentType		= "text/xml" ;

			XmlDocument oXML = new XmlDocument() ;
			XmlNode oConnectorNode = CreateBaseXml( oXML, sCommand, sResourceType, sCurrentFolder ) ;

			// Execute the required command.
			switch( sCommand )
			{
				case "GetFolders" :
					this.GetFolders( oConnectorNode, sResourceType, sCurrentFolder ) ;
					break ;
				case "GetFoldersAndFiles" :
					this.GetFolders( oConnectorNode, sResourceType, sCurrentFolder ) ;
					this.GetFiles( oConnectorNode, sResourceType, sCurrentFolder ) ;
					break ;
				case "CreateFolder" :
					this.CreateFolder( oConnectorNode, sResourceType, sCurrentFolder ) ;
					break ;
			}

			// Output the resulting XML.
			Response.Write( oXML.OuterXml ) ;

			Response.End() ;
		}

		#region Base XML Creation

		/// <summary>
		/// Creates the base XML.
		/// </summary>
		/// <param name="xml">The XML.</param>
		/// <param name="command">The command.</param>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="currentFolder">The current folder.</param>
		/// <returns></returns>
		private XmlNode CreateBaseXml( XmlDocument xml, string command, string resourceType, string currentFolder )
		{
			// Create the XML document header.
			xml.AppendChild( xml.CreateXmlDeclaration( "1.0", "utf-8", null ) ) ;

			// Create the main "Connector" node.
			XmlNode oConnectorNode = XmlUtil.AppendElement( xml, "Connector" ) ;
			XmlUtil.SetAttribute( oConnectorNode, "command", command ) ;
			XmlUtil.SetAttribute( oConnectorNode, "resourceType", resourceType ) ;

			// Add the current folder node.
			XmlNode oCurrentNode = XmlUtil.AppendElement( oConnectorNode, "CurrentFolder" ) ;
			XmlUtil.SetAttribute( oCurrentNode, "path", currentFolder ) ;
			XmlUtil.SetAttribute( oCurrentNode, "url", GetUrlFromPath( resourceType, currentFolder) ) ;

			return oConnectorNode ;
		}

		#endregion

		#region Command Handlers

		/// <summary>
		/// Gets the folders.
		/// </summary>
		/// <param name="connectorNode">The connector node.</param>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="currentFolder">The current folder.</param>
		private void GetFolders( XmlNode connectorNode, string resourceType, string currentFolder )
		{
			// Map the virtual path to the local server path.
			string sServerDir = this.ServerMapFolder( resourceType, currentFolder ) ;

			// Create the "Folders" node.
			XmlNode oFoldersNode = XmlUtil.AppendElement( connectorNode, "Folders" ) ;

			DirectoryInfo oDir = new DirectoryInfo( sServerDir ) ;
			DirectoryInfo[] aSubDirs = oDir.GetDirectories() ;

			for ( int i = 0 ; i < aSubDirs.Length ; i++ )
			{
				// Create the "Folders" node.
				XmlNode oFolderNode = XmlUtil.AppendElement( oFoldersNode, "Folder" ) ;
				XmlUtil.SetAttribute( oFolderNode, "name", aSubDirs[i].Name ) ;
			}
		}

		/// <summary>
		/// Gets the files.
		/// </summary>
		/// <param name="connectorNode">The connector node.</param>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="currentFolder">The current folder.</param>
		private void GetFiles( XmlNode connectorNode, string resourceType, string currentFolder )
		{
			// Map the virtual path to the local server path.
			string sServerDir = this.ServerMapFolder( resourceType, currentFolder ) ;

			// Create the "Files" node.
			XmlNode oFilesNode = XmlUtil.AppendElement( connectorNode, "Files" ) ;

			DirectoryInfo oDir = new DirectoryInfo( sServerDir ) ;
			FileInfo[] aFiles = oDir.GetFiles() ;

			for ( int i = 0 ; i < aFiles.Length ; i++ )
			{
				Decimal iFileSize = Math.Round( (Decimal)aFiles[i].Length / 1024 ) ;
				if ( iFileSize < 1 && aFiles[i].Length != 0 ) iFileSize = 1 ;

				// Create the "File" node.
				XmlNode oFileNode = XmlUtil.AppendElement( oFilesNode, "File" ) ;
				XmlUtil.SetAttribute( oFileNode, "name", aFiles[i].Name ) ;
				XmlUtil.SetAttribute( oFileNode, "size", iFileSize.ToString( CultureInfo.InvariantCulture ) ) ;
			}
		}

		/// <summary>
		/// Creates the folder.
		/// </summary>
		/// <param name="connectorNode">The connector node.</param>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="currentFolder">The current folder.</param>
		private void CreateFolder( XmlNode connectorNode, string resourceType, string currentFolder )
		{
			string sErrorNumber = "0" ;

			string sNewFolderName = Request.QueryString["NewFolderName"] ;

			if ( sNewFolderName == null || sNewFolderName.Length == 0 )
				sErrorNumber = "102" ;
			else
			{
				// Map the virtual path to the local server path of the current folder.
				string sServerDir = this.ServerMapFolder( resourceType, currentFolder ) ;

				try
				{
					Directory.CreateDirectory(System.IO.Path.Combine( sServerDir, sNewFolderName )) ;
				}
				catch ( ArgumentException )
				{
					sErrorNumber = "102" ;
				}
				catch ( PathTooLongException )
				{
					sErrorNumber = "102" ;
				}
				catch ( IOException )
				{
					sErrorNumber = "101" ;
				}
				catch ( SecurityException )
				{
					sErrorNumber = "103" ;
				}
				catch ( Exception )
				{
					sErrorNumber = "110" ;
				}
			}

			// Create the "Error" node.
			XmlNode oErrorNode = XmlUtil.AppendElement( connectorNode, "Error" ) ;
			XmlUtil.SetAttribute( oErrorNode, "number", sErrorNumber ) ;
		}

		/// <summary>
		/// Files the upload.
		/// </summary>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="currentFolder">The current folder.</param>
		private void FileUpload( string resourceType, string currentFolder )
		{
			HttpPostedFile oFile = Request.Files["NewFile"] ;

			string sErrorNumber = "0" ;
			string sFileName = string.Empty ;

			if ( oFile != null )
			{
				// Map the virtual path to the local server path.
				string sServerDir = this.ServerMapFolder( resourceType, currentFolder ) ;

				// Get the uploaded file name.
				sFileName = System.IO.Path.GetFileName( oFile.FileName ) ;

				int iCounter = 0 ;

				while ( true )
				{
					string sFilePath = System.IO.Path.Combine( sServerDir, sFileName ) ;

					if ( File.Exists( sFilePath ) )
					{
						iCounter++ ;
						sFileName = 
							System.IO.Path.GetFileNameWithoutExtension( oFile.FileName ) +
							"(" + iCounter + ")" +
								System.IO.Path.GetExtension( oFile.FileName ) ;

						sErrorNumber = "201" ;
					}
					else
					{
						oFile.SaveAs( sFilePath ) ;
						break ;
					}
				}
			}
			else
				sErrorNumber = "202" ;

			Response.Clear() ;

			Response.Write( "<script type=\"text/javascript\">" ) ;
			Response.Write( "window.parent.frames['frmUpload'].OnUploadCompleted(" + sErrorNumber + ",'" + sFileName.Replace( "'", "\\'" ) + "') ;" ) ;
			Response.Write( "</script>" ) ;

			Response.End() ;
		}

		#endregion

		#region Directory Mapping

		/// <summary>
		/// Servers the map folder.
		/// </summary>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="folderPath">The folder path.</param>
		/// <returns></returns>
		private string ServerMapFolder( string resourceType, string folderPath )
		{
			// Ensure that the directory exists.
			Directory.CreateDirectory( this.UserFilesDirectory ) ;

			// Return the resource type directory combined with the required path.
			return System.IO.Path.Combine( this.UserFilesDirectory, folderPath.TrimStart('/') ) ;
		}

		/// <summary>
		/// Gets the URL from path.
		/// </summary>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="folderPath">The folder path.</param>
		/// <returns></returns>
		private string GetUrlFromPath( string resourceType, string folderPath )
		{
			if ( resourceType == null || resourceType.Length == 0 )
				return this.UserFilesPath.TrimEnd('/') + folderPath ;
			else
				return this.UserFilesPath + folderPath ;
		}

		/// <summary>
		/// Gets the user files path.
		/// </summary>
		/// <value>The user files path.</value>
		private string UserFilesPath
		{
			get
			{
				if ( sUserFilesPath == null )
				{
					PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];
					if (portalSettings == null) return null;
                    int modId = portalSettings.ActiveModule;
                    if (modId < 1)
                        modId = getModId();
					var ms = Framework.Site.Configuration.ModuleSettings.GetModuleSettings(modId);
					string DefaultImageFolder = "default";
					if (ms.ContainsKey("MODULE_IMAGE_FOLDER") && ms["MODULE_IMAGE_FOLDER"] != null) 
					{
						DefaultImageFolder = ms["MODULE_IMAGE_FOLDER"].ToString();
					}
                    else if (portalSettings.CustomSettings.ContainsKey("SITESETTINGS_DEFAULT_IMAGE_FOLDER") && portalSettings.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"] != null) 
					{
						DefaultImageFolder = portalSettings.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"].ToString();
					}
					sUserFilesPath = Path.WebPathCombine(Path.ApplicationRoot, portalSettings.PortalPath, "images", DefaultImageFolder);
				}
				return sUserFilesPath ;
			}
		}

		/// <summary>
		/// Gets the user files directory.
		/// </summary>
		/// <value>The user files directory.</value>
		private string UserFilesDirectory
		{
			get	
			{
				if ( sUserFilesDirectory == null )
				{
					// Get the local (server) directory path translation.
					sUserFilesDirectory = Server.MapPath( this.UserFilesPath ) ;
				}
				return sUserFilesDirectory ;
			}
		}

		#endregion
	}

	internal sealed class XmlUtil
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XmlUtil"/> class.
		/// </summary>
		private XmlUtil()
		{}

		/// <summary>
		/// Appends the element.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="newElementName">New name of the element.</param>
		/// <returns></returns>
		public static XmlNode AppendElement( XmlNode node, string newElementName )
		{
			return AppendElement( node, newElementName, null ) ;
		}

		/// <summary>
		/// Appends the element.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="newElementName">New name of the element.</param>
		/// <param name="innerValue">The inner value.</param>
		/// <returns></returns>
		public static XmlNode AppendElement( XmlNode node, string newElementName, string innerValue )
		{
			XmlNode oNode ;

			if ( node is XmlDocument )
				oNode = node.AppendChild( ((XmlDocument)node).CreateElement( newElementName ) ) ;
			else
				oNode = node.AppendChild( node.OwnerDocument.CreateElement( newElementName ) ) ;

			if ( innerValue != null )
				oNode.AppendChild( node.OwnerDocument.CreateTextNode( innerValue ) ) ;

			return oNode ;
		}

		/// <summary>
		/// Creates the attribute.
		/// </summary>
		/// <param name="xmlDocument">The XML document.</param>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static XmlAttribute CreateAttribute( XmlDocument xmlDocument, string name, string value )
		{
			XmlAttribute oAtt = xmlDocument.CreateAttribute( name ) ;
			oAtt.Value = value ;
			return oAtt ;
		}

		/// <summary>
		/// Sets the attribute.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <param name="attributeValue">The attribute value.</param>
		public static void SetAttribute( XmlNode node, string attributeName, string attributeValue )
		{
			if ( node.Attributes[ attributeName ] != null )
				node.Attributes[ attributeName ].Value = attributeValue ;
			else
				node.Attributes.Append( CreateAttribute( node.OwnerDocument, attributeName, attributeValue ) ) ;
		}
	}

}
