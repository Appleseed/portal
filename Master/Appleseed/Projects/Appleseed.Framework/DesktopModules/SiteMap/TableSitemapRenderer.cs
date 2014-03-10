// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableSitemapRenderer.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   A concrete site map renderer. This class takes in an IList{SitemapItem} class and generates
//   a Table from it.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules.Sitemap
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;

    /// <summary>
    /// A concrete site map renderer. This class takes in an IList{SitemapItem} class and generates
    ///   a Table from it.
    /// </summary>
    public class TableSitemapRenderer : ISitemapRenderer
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TableSitemapRenderer" /> class. 
        ///   TableSitemapRenderer constructor
        /// </summary>
        public TableSitemapRenderer()
        {
            this.InitVariables();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   CSS style for the hyperlinks
        /// </summary>
        /// <value>The CSS style.</value>
        public string CssStyle { get; set; }

        /// <summary>
        ///   Url for the Crossed line image
        /// </summary>
        /// <value>The image crossed line URL.</value>
        public string ImageCrossedLineUrl { get; set; }

        /// <summary>
        ///   Url for the Last Node Line image
        /// </summary>
        /// <value>The image last node line URL.</value>
        public string ImageLastNodeLineUrl { get; set; }

        /// <summary>
        ///   Url for the other Nodes image
        /// </summary>
        /// <value>The image node URL.</value>
        public string ImageNodeUrl { get; set; }

        /// <summary>
        ///   Url for the RootNode image
        /// </summary>
        /// <value>The image root node URL.</value>
        public string ImageRootNodeUrl { get; set; }

        /// <summary>
        ///   Url for the spacer image
        /// </summary>
        /// <value>The image spacer URL.</value>
        public string ImageSpacerUrl { get; set; }

        /// <summary>
        ///   Url for the straightline image
        /// </summary>
        /// <value>The image straight line URL.</value>
        public string ImageStraightLineUrl { get; set; }

        /// <summary>
        ///   Height of the images. All images should have the same height
        /// </summary>
        /// <value>The height of the images.</value>
        public int ImagesHeight { get; set; }

        /// <summary>
        ///   Width of the images. All images should have the same width
        /// </summary>
        /// <value>The width of the images.</value>
        public int ImagesWidth { get; set; }

        /// <summary>
        ///   Width of the table. defaults to 98%
        /// </summary>
        /// <value>The width of the table.</value>
        public Unit TableWidth { get; set; }

        #endregion

        #region Implemented Interfaces

        #region ISitemapRenderer

        /// <summary>
        /// Render
        /// This function creates a tree view in a table from the SitemapItems list
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>A web control.</returns>
        /// <remarks></remarks>
        public virtual WebControl Render(IList<SitemapItem> list)
        {
            // init table with a width of 98%
            var t = new Table { Width = this.TableWidth, BorderWidth = 0, CellSpacing = 0, CellPadding = 0 };

            var cols = this.MaxLevel(list) + 2;

            // an array of chars is used to determine what images to show on each row
            // the chars have the following meaning:
            // + --> crossed line
            // | --> straight line
            // \ --> line for last node on branch
            // N --> node
            // R --> root node
            // S --> space
            var strRow = new char[cols];

            // init row to spaces
            for (var i = 0; i < cols; ++i)
            {
                strRow[i] = 'S';
            }

            for (var i = 0; i < list.Count; ++i)
            {
                // replace the cross of the previous row in a straight line on the current row
                // do the same for last_node_line and Spaces
                for (var j = 0; j < cols; ++j)
                {
                    if (strRow[j] == '+')
                    {
                        strRow[j] = '|';
                    }

                    if (strRow[j] == '\\')
                    {
                        strRow[j] = 'S';
                    }
                }

                // show a root node image if nestlevel = 0
                if (list[i].NestLevel == 0)
                {
                    strRow[list[i].NestLevel] = 'R';
                }
                else
                {
                    strRow[list[i].NestLevel] = 'N';
                }

                // everything after the node can be replaces by spaces
                for (var j = list[i].NestLevel + 1; j < cols; ++j)
                {
                    strRow[j] = ' ';
                }

                // show no lines before the node when it's a root node
                if (list[i].NestLevel > 0)
                {
                    if (this.LastItemAtLevel(i, list))
                    {
                        // if it's the last node at that level of the current branch,
                        // show a last node line
                        strRow[list[i].NestLevel - 1] = '\\';
                    }
                    else
                    {
                        // else show a crossed line
                        strRow[list[i].NestLevel - 1] = '+';
                    }
                }

                // the images are determined in the char array, now make a TableRow from it
                var r = new TableRow();
                TableCell c;

                // only use the char array till the node
                for (var j = 0; j <= list[i].NestLevel; ++j)
                {
                    c = new TableCell();
                    var img = new Image { BorderWidth = 0, Width = this.ImagesWidth, Height = this.ImagesHeight };

                    c.Width = this.ImagesWidth;

                    // what image to use
                    switch (strRow[j])
                    {
                        case '+':
                            img.ImageUrl = this.ImageCrossedLineUrl;
                            break;
                        case '\\':
                            img.ImageUrl = this.ImageLastNodeLineUrl;
                            break;
                        case '|':
                            img.ImageUrl = this.ImageStraightLineUrl;
                            break;
                        case 'S':
                            img.ImageUrl = this.ImageSpacerUrl;
                            break;
                        case 'N':
                            img.ImageUrl = this.ImageNodeUrl;
                            break;
                        case 'R':
                            img.ImageUrl = this.ImageRootNodeUrl;
                            break;
                    }

                    c.Controls.Add(img);
                    r.Cells.Add(c);
                }

                // the images are done for this row, now make the hyperlink
                c = new TableCell
                    {
                        Width = new Unit(100, UnitType.Percentage), ColumnSpan = cols - 1 - list[i].NestLevel 
                    };

                // make sure it fills the all the space left
                var lit = new Literal { Text = "&nbsp;" };
                c.Controls.Add(lit);
                var l = new HyperLink { Text = list[i].Name, NavigateUrl = list[i].Url, CssClass = this.CssStyle };

                // row is done and add everything to the table
                c.Controls.Add(l);
                r.Cells.Add(c);
                t.Rows.Add(r);
            }

            return t;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// set default values
        /// </summary>
        protected virtual void InitVariables()
        {
            // init member variables
            this.ImageRootNodeUrl = string.Empty;
            this.ImageNodeUrl = string.Empty;
            this.ImageSpacerUrl = string.Empty;
            this.ImageStraightLineUrl = string.Empty;
            this.ImageCrossedLineUrl = string.Empty;
            this.ImageLastNodeLineUrl = string.Empty;
            this.CssStyle = string.Empty;

            this.ImagesHeight = 0;
            this.ImagesWidth = 0;

            // default table width to 98%
            this.TableWidth = new Unit(98, UnitType.Percentage);
        }

        /// <summary>
        /// Returns true if node is last node for the current branch on that level
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The last item at level.
        /// </returns>
        protected virtual bool LastItemAtLevel(int index, IList<SitemapItem> list)
        {
            var level = list[index].NestLevel;

            for (var i = index + 1; i < list.Count; ++i)
            {
                if (list[i].NestLevel < level)
                {
                    return true;
                }

                if (list[i].NestLevel == level)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Max Level
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The max level.
        /// </returns>
        protected virtual int MaxLevel(IList<SitemapItem> list)
        {
            return list.Max(t => t.NestLevel);
        }

        #endregion
    }
}