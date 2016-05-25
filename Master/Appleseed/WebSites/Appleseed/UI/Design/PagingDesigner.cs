// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagingDesigner.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Designer support for paging
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.Design
{
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.Design;

    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// Designer support for paging
    /// </summary>
    public class PagingDesigner : ControlDesigner
    {
        #region Public Methods

        /// <summary>
        /// Component is the instance of the component or control that
        ///   this designer object is associated with. This property is
        ///   inherited from System.ComponentModel.ComponentDesigner.
        /// </summary>
        /// <returns>
        /// The HTML markup used to represent the control at design time.
        /// </returns>
        public override string GetDesignTimeHtml()
        {
            var paging = (Paging)this.Component;

             var sw = new StringWriter();
                using (var tw = new HtmlTextWriter(sw))
                {
                    paging.HideOnSinglePage = false;
                    paging.RefreshButtons();

                    paging.RenderControl(tw);
                }

                return sw.ToString();
        }

        #endregion
    }
}