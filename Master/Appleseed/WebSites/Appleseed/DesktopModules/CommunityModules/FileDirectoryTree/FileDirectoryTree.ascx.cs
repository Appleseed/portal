using System;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Web.UI.WebControls;
using LinkButton=Appleseed.Framework.Web.UI.WebControls.LinkButton;
using Localize=Appleseed.Framework.Web.UI.WebControls.Localize;
using Path=Appleseed.Framework.Settings.Path;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    ///		:::::::::::::::::::::::::
    ///		::  FileDirectoryTree  ::
    ///		:::::::::::::::::::::::::
    ///		
    ///		Developed by: Josue de la Torre, josue@jdlt.com, www.jdlt.com
    ///		Module traverses and displays files and directories as an 
    ///		HTML-representation of a nested file tree.
    /// </summary>
    /// 
    ///		::::::::::::::::::::::
    ///		::  Module History  ::
    ///		::::::::::::::::::::::
    ///		
    ///		07/23/2003	First Release - Josue de la Torre (josue@jdlt.com)
    public partial class FileDirectoryTree : PortalModuleControl
    {
        #region Declarations

        /// <summary>
        /// Literal in FDT
        /// </summary>
        protected Localize LiteralFileDirectoryTree;

        /// <summary>
        /// 
        /// </summary>
        protected LinkButton LinkButton1;

        private string path, myStyle, LinkType;

        // Jminond - addded to upgraded extension set
        private string baseImageDIR = string.Empty;

        /// <summary>
        /// Location of tree images
        /// </summary>
        protected string treeImageDIR = string.Empty;

        private string physRoot = string.Empty;
        private Hashtable availExtensions = new Hashtable();

        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            treeImageDIR = Path.WebPathCombine(this.CurrentTheme.WebPath, "/img/");
            LoadAvailableImageList();

            path = Settings["Directory"].ToString();
            myStyle = Settings["Style"].ToString();
            LinkType = Settings["LinkType"].ToString();

            // Check if the last character is an backslash.  If not, append it. 
            if (path.Length == 0)
                path = "\\";
            else
            {
                if (path.Substring(path.Length - 1, 1) != "\\")
                    path += "\\";
            }

            physRoot = Server.MapPath(Path.ApplicationRoot);

            // Support for old installs may have physical path we want virtual.
            if (path.IndexOf(":") >= 0)
            {
                // find app root from phsyical path and cut so we only have virtual path
                path = path.Substring(Path.ApplicationPhysicalPath.Length);
            }
            Trace.Warn("path = " + path);
            // Check to make sure path exists before entering render methods
            if (Directory.Exists(Server.MapPath(path)))
            {
                Write("<script language='javascript'>baseImg = '" + treeImageDIR + "';</script>");
                Write("<span style='" + myStyle + "'>\n");
                parseDirectory(Server.MapPath(path));
                // Close the span and create the Toggle javascript function.
                Write("</span>");
            }
            else
            {
                Write("<span class='Error'>Error! The directory path you specified does not exist.</span>");
            }
        }

        /// <summary>
        /// Loads array of images available
        /// </summary>
        private void LoadAvailableImageList()
        {
            string bDir = Server.MapPath(baseImageDIR);
            DirectoryInfo di = new DirectoryInfo(bDir);
            FileInfo[] rgFiles = di.GetFiles("*.gif");
            string ext = string.Empty;
            string nme = string.Empty;
            string f_Name = string.Empty;

            foreach (FileInfo fi in rgFiles)
            {
                f_Name = fi.Name;
                ext = fi.Extension;
                nme = f_Name.Substring(0, (f_Name.Length - ext.Length));
                availExtensions.Add(nme, f_Name);
            }
        }

        /// <summary>
        /// This function traverses a given directory and finds all its nested directories and
        /// files.  As the function encounters nested directories, it calls a new instance of
        /// the procedure passing the new found directory as a parameter.  Files within the
        /// directories are nested and tabulated.
        /// </summary>
        /// <param name="path">Directory path to traverse.</param>
        private void parseDirectory(string path)
        {
            string[] entry;
            try
            {
                // Retrieve all entry (entry & directories) from the current path
                entry = Directory.GetFileSystemEntries(path);
                string contentType;
                int locDot = 0;

                // For each entry in the directory...
                for (int i = 0; i < entry.Length; i++)
                {

                    // Trim the file path from the file, leaving the filename
                    string filename = entry[i].Replace(path, string.Empty);
                    if (!filename.StartsWith("."))
                    {
                        // If the current entry is a directory...
                        if (Directory.Exists(entry[i]))
                        {
                            // Find how many entry the subdirectory has and create an objectID name for the subdirectory
                            int subentries = Directory.GetFileSystemEntries(entry[i]).Length;
                            string objectID;

                            if (entry[i].Length > 0)
                                objectID = entry[i].Replace(physRoot, string.Empty).Replace("\\", "~");
                            else
                                objectID = "~";

                            // Define the span that holds the opened/closed directory icon
                            Write("<img id='" + objectID + "_img'");

                            if (Settings["Collapsed"].ToString().Equals("True"))
                                Write("src='" + treeImageDIR + "dir.gif'");
                            else
                                Write("src='" + treeImageDIR + "dir_open.gif'");

                            Write(" />&nbsp;<a href=\"javascript:Toggle('" + objectID + "')\" " +
                                // Create a hover tag that contains content details about the subdirectory.
                                  "title=\"" + subentries.ToString() + " entries found.\">" + filename + "</a>" +
                                  "&nbsp;<br />\n<div id='" + objectID + "' style='");

                            if (Settings["Collapsed"].ToString().Equals("True"))
                                Write("display:none;");
                            else
                                Write("display:block;");

                            if (!Settings["Indent"].ToString().Equals(string.Empty))
                                Write("left:" + Settings["Indent"].ToString() + "; ");

                            // Call the parseDirectory for the new subdirectory.
                            Write("POSITION: relative;'>\n");

                            parseDirectory(entry[i] + "\\");

                            Write("</div>\n");
                        }
                        else // ...the current entry is a file.
                        {
                            locDot = filename.LastIndexOf(".") + 1;

                            if (locDot > 0)
                                contentType = filename.Substring(locDot);
                            else
                                contentType = "unknown";

                            // create a file icon 
                            // jminond - switched to use extensions pack
                            if (availExtensions.ContainsKey(contentType))
                            {
                                Write("<img src='" + baseImageDIR + availExtensions[contentType].ToString() + "' />");
                            }
                            else
                            {
                                Write("<img src='" + baseImageDIR + "unknown.gif' />");
                            }
                            Write("&nbsp;");

                            if (LinkType.Equals("Network Share"))
                            {
                                Write("<a href='" + entry[i] + "' title='Last Write Time: " +
                                      File.GetLastWriteTime(entry[i]).ToString());
                                Write("' target='_" + Settings["Target"].ToString() + "'>" + filename + "</a>");
                            }
                            else
                            {
                                // Create the link to the file.
                                LinkButton lb = new LinkButton();
                                lb.Text = filename;
                                lb.CommandArgument = entry[i];
                                lb.Click += new EventHandler(Download);
                                myPlaceHolder.Controls.Add(lb);
                            }

                            Write("&nbsp;<br />\n");
                        }
                    }
                }
            }

            catch (DirectoryNotFoundException)
            {
                Write("<span class='Error'>Error!  The directory path you specified does not exist.</span>");
                return;
            }
            catch (Exception e1) // All other exceptions...
            {
                Write("<span class='Error'>" + e1.ToString() + "</span>");
                return;
            }
        }

        /// <summary>
        /// Write the tree
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            Literal l = new Literal();
            l.Text = text;
            myPlaceHolder.Controls.Add(l);
        }

        /// <summary>
        /// Downloads the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Download(object sender, EventArgs e)
        {
            string filepath = ((LinkButton) sender).CommandArgument;
            string filename =
                filepath.Substring(filepath.LastIndexOf('\\') + 1, filepath.Length - filepath.LastIndexOf('\\') - 1);

            Stream s = null;
            Byte[] buffer = new byte[0];
            try
            {
                s = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                buffer = new Byte[s.Length];
                s.Read(buffer, 0, (Int32) s.Length);
            }
            catch (Exception ex)
            {
                Response.ClearContent();
                Response.Write(ex.Message);
                Response.End();
            }
            finally
            {
                if (s != null)
                    s.Close(); //by manu
            }
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.BinaryWrite(buffer);
            Response.End();
        }

        /// <summary>
        /// Constructor set module settings
        /// </summary>
        public FileDirectoryTree()
        {
            // modified by Hongwei Shen
            SettingItemGroup group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            int groupBase = (int) group;

            var directory = new SettingItem<string, TextBox>()
                {
                    EnglishName = "Directory Path",
                    Required = true,
                    Order = groupBase + 20,
                    Group = group,
                    Value = Path.ApplicationRoot + "/portals"
                };

            // Changed to virutal root from physical

            //directory.Value = Path.ApplicationPhysicalPath;
            this.BaseSettings.Add("Directory", directory);

            var LinkType = new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("Downloadable Link;Network Share"));
            LinkType.EnglishName = "Link Type";
            LinkType.Group = group;
            LinkType.Order = groupBase + 25; //2;
            LinkType.Value = "Downloadable Link";
            this.BaseSettings.Add("LinkType", LinkType);

            var Target = new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("blank;parent;self;top"));
            Target.EnglishName = "Target Window";
            Target.Required = false;
            Target.Group = group;
            Target.Order = groupBase + 30; //3;
            Target.Value = "blank";
            this.BaseSettings.Add("Target", Target);

            var Collapsed = new SettingItem<bool, CheckBox>();
            Collapsed.EnglishName = "Collapsed View";
            Collapsed.Group = group;
            Collapsed.Order = groupBase + 35; //4;
            Collapsed.Value = true;
            this.BaseSettings.Add("Collapsed", Collapsed);

            var Style = new SettingItem<string, TextBox>();
            Style.EnglishName = "Style";
            Style.Required = false;
            Style.Group = group;
            Style.Order = groupBase + 40; //5;
            Style.Value = string.Empty;
            this.BaseSettings.Add("Style", Style);

            var Indent = new SettingItem<string, TextBox>();
            Indent.EnglishName = "SubDirectory Indent (px)";
            Indent.Required = false;
            Indent.Group = group;
            Indent.Order = groupBase + 45; //6;
            Indent.Value = "20px";
            this.BaseSettings.Add("Indent", Indent);
        }

        /// <summary>
        /// Module GUID
        /// </summary>
        public override Guid GuidID
        {
            get { return new Guid("{2502DB18-B580-4F90-8CB4-C15E6E53100B}"); }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInit event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);

            this.baseImageDIR = Path.WebPathCombine(Path.ApplicationRoot, "/aspnet_client/Ext/");
            // no need for viewstate here - jminond
            this.myPlaceHolder.EnableViewState = false;

            base.OnInit(e);
        }

        #endregion
    }
}