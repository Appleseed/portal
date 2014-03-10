// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebControlAdapterExtender.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The web control adapter extender.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The web control adapter extender.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class WebControlAdapterExtender
    {
        #region Constants and Fields

        /// <summary>
        /// The adapted control.
        /// </summary>
        private readonly WebControl adaptedControl;

        /// <summary>
        /// The disable auto access key.
        /// </summary>
        /// <remarks>
        /// used when dealing with things like read-only textboxes that should not have access keys
        /// </remarks>
        private bool disableAutoAccessKey;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebControlAdapterExtender"/> class.
        /// </summary>
        /// <param name="adaptedControl">
        /// The adapted control.
        /// </param>
        /// <remarks>
        /// </remarks>
        public WebControlAdapterExtender(WebControl adaptedControl)
        {
            this.adaptedControl = adaptedControl;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the adapted control.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public WebControl AdaptedControl
        {
            get
            {
                Debug.Assert(
                    this.adaptedControl != null, 
                    "CSS Friendly adapters internal error", 
                    "No control has been defined for the adapter extender");
                return this.adaptedControl;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether [adapter enabled].
        /// </summary>
        /// <remarks>
        /// </remarks>
        public bool AdapterEnabled
        {
            get
            {
                var bReturn = true; // normally the adapters are enabled

                // Individual controls can use the expando property called AdapterEnabled
                // as a way to turn the adapters off.
                // <asp:TreeView runat="server" AdapterEnabled="false" />
                if ((this.AdaptedControl != null) &&
                    (!String.IsNullOrEmpty(this.AdaptedControl.Attributes["AdapterEnabled"])) &&
                    (this.AdaptedControl.Attributes["AdapterEnabled"].IndexOf(
                        "false", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    bReturn = false;
                }

                return bReturn;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether [auto access key].
        /// </summary>
        /// <remarks>
        /// </remarks>
        public bool AutoAccessKey
        {
            get
            {
                // Individual controls can use the expando property called AdapterEnabled
                // as a way to turn on/off the heurisitic for automatically setting the AccessKey
                // attribute in the rendered HTML.  The default is shown below in the initialization
                // of the bReturn variable.
                // <asp:TreeView runat="server" AutoAccessKey="false" />
                var bReturn = true; // by default, the adapter will make access keys are available
                if (this.disableAutoAccessKey ||
                    ((this.AdaptedControl != null) &&
                     (!String.IsNullOrEmpty(this.AdaptedControl.Attributes["AutoAccessKey"])) &&
                     (this.AdaptedControl.Attributes["AutoAccessKey"].IndexOf(
                         "false", StringComparison.OrdinalIgnoreCase) == 0)))
                {
                    bReturn = false;
                }

                return bReturn;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Makes the type of the id with button.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The make id with button type.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string MakeIdWithButtonType(string id, ButtonType type)
        {
            var idWithType = id;
            switch (type)
            {
                case ButtonType.Button:
                    idWithType += "Button";
                    break;
                case ButtonType.Image:
                    idWithType += "ImageButton";
                    break;
                case ButtonType.Link:
                    idWithType += "LinkButton";
                    break;
            }

            return idWithType;
        }

        /// <summary>
        /// Makes the name from id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The make name from id.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string MakeNameFromId(string id)
        {
            var name = string.Empty;
            for (var i = 0; i < id.Length; i++)
            {
                var thisChar = id[i];
                var prevChar = ((i - 1) > -1) ? id[i - 1] : ' ';
                var nextChar = ((i + 1) < id.Length) ? id[i + 1] : ' ';
                if (thisChar == '_')
                {
                    if (prevChar == '_')
                    {
                        name += "_";
                    }
                    else if (nextChar == '_')
                    {
                        name += "$_";
                    }
                    else
                    {
                        name += "$";
                    }
                }
                else
                {
                    name += thisChar;
                }
            }

            return name;
        }

        /// <summary>
        /// Removes the problem children.
        /// </summary>
        /// <param name="ctrl">
        /// The CTRL.
        /// </param>
        /// <param name="stashedControls">
        /// The stashed controls.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void RemoveProblemChildren(Control ctrl, List<ControlRestorationInfo> stashedControls)
        {
            RemoveProblemTypes(ctrl.Controls, stashedControls);
        }

        /// <summary>
        /// Removes the problem types.
        /// </summary>
        /// <param name="coll">
        /// The coll.
        /// </param>
        /// <param name="stashedControls">
        /// The stashed controls.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void RemoveProblemTypes(ControlCollection coll, List<ControlRestorationInfo> stashedControls)
        {
            foreach (Control ctrl in coll)
            {
                if (typeof(RequiredFieldValidator).IsAssignableFrom(ctrl.GetType()) ||
                    typeof(CompareValidator).IsAssignableFrom(ctrl.GetType()) ||
                    typeof(RegularExpressionValidator).IsAssignableFrom(ctrl.GetType()) ||
                    typeof(ValidationSummary).IsAssignableFrom(ctrl.GetType()))
                {
                    var cri = new ControlRestorationInfo(ctrl, coll);
                    stashedControls.Add(cri);
                    coll.Remove(ctrl);
                    continue;
                }

                if (ctrl.HasControls())
                {
                    RemoveProblemTypes(ctrl.Controls, stashedControls);
                }
            }
        }

        /// <summary>
        /// Restores the problem children.
        /// </summary>
        /// <param name="stashedControls">
        /// The stashed controls.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void RestoreProblemChildren(List<ControlRestorationInfo> stashedControls)
        {
            foreach (var cri in stashedControls)
            {
                cri.Restore();
            }
        }

        /// <summary>
        /// Writes the begin div.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteBeginDiv(HtmlTextWriter writer, string className, string id)
        {
            writer.WriteLine();
            writer.WriteBeginTag("div");
            if (!String.IsNullOrEmpty(className))
            {
                writer.WriteAttribute("class", className);
            }

            if (!String.IsNullOrEmpty(id))
            {
                writer.WriteAttribute("id", id);
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;
        }

        /// <summary>
        /// Writes the compare validator.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="cv">
        /// The cv.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="controlToValidate">
        /// The control to validate.
        /// </param>
        /// <param name="msg">
        /// The MSG.
        /// </param>
        /// <param name="controlToCompare">
        /// The control to compare.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteCompareValidator(
            HtmlTextWriter writer, 
            CompareValidator cv, 
            string className, 
            string controlToValidate, 
            string msg, 
            string controlToCompare)
        {
            if (cv != null)
            {
                cv.CssClass = className;
                cv.ControlToValidate = controlToValidate;
                cv.ErrorMessage = msg;
                cv.ControlToCompare = controlToCompare;
                cv.RenderControl(writer);
            }
        }

        /// <summary>
        /// Writes the end div.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteEndDiv(HtmlTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine();
            writer.WriteEndTag("div");
        }

        /// <summary>
        /// Writes the image.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <param name="alt">
        /// The alt.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteImage(HtmlTextWriter writer, string url, string alt)
        {
            if (!String.IsNullOrEmpty(url))
            {
                writer.WriteLine();
                writer.WriteBeginTag("img");
                writer.WriteAttribute("src", url);
                writer.WriteAttribute("alt", alt);
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
            }
        }

        /// <summary>
        /// Writes the link.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteLink(HtmlTextWriter writer, string className, string url, string title, string content)
        {
            if ((!String.IsNullOrEmpty(url)) && (!String.IsNullOrEmpty(content)))
            {
                writer.WriteLine();
                writer.WriteBeginTag("a");
                if (!String.IsNullOrEmpty(className))
                {
                    writer.WriteAttribute("class", className);
                }

                writer.WriteAttribute("href", url);
                writer.WriteAttribute("title", title);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(content);
                writer.WriteEndTag("a");
            }
        }

        /// <summary>
        /// Writes the regular expression validator.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="rev">
        /// The rev.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="controlToValidate">
        /// The control to validate.
        /// </param>
        /// <param name="msg">
        /// The MSG.
        /// </param>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteRegularExpressionValidator(
            HtmlTextWriter writer, 
            RegularExpressionValidator rev, 
            string className, 
            string controlToValidate, 
            string msg, 
            string expression)
        {
            if (rev != null)
            {
                rev.CssClass = className;
                rev.ControlToValidate = controlToValidate;
                rev.ErrorMessage = msg;
                rev.ValidationExpression = expression;
                rev.RenderControl(writer);
            }
        }

        /// <summary>
        /// Writes the required field validator.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="rfv">
        /// The RFV.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="controlToValidate">
        /// The control to validate.
        /// </param>
        /// <param name="msg">
        /// The MSG.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteRequiredFieldValidator(
            HtmlTextWriter writer, RequiredFieldValidator rfv, string className, string controlToValidate, string msg)
        {
            if (rfv != null)
            {
                rfv.CssClass = className;
                rfv.ControlToValidate = controlToValidate;
                rfv.ErrorMessage = msg;
                rfv.RenderControl(writer);
            }
        }

        /// <summary>
        /// Writes the span.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteSpan(HtmlTextWriter writer, string className, string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                writer.WriteLine();
                writer.WriteBeginTag("span");
                if (!String.IsNullOrEmpty(className))
                {
                    writer.WriteAttribute("class", className);
                }

                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(content);
                writer.WriteEndTag("span");
            }
        }

        /// <summary>
        /// Writes the target attribute.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="targetValue">
        /// The target value.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void WriteTargetAttribute(HtmlTextWriter writer, string targetValue)
        {
            if ((writer != null) && (!String.IsNullOrEmpty(targetValue)))
            {
                // If the targetValue is _blank then we have an opportunity to use attributes other than "target"
                // which allows us to be compliant at the XHTML 1.1 Strict level. Specifically, we can use a combination
                // of "onclick" and "onkeypress" to achieve what we want to achieve when we used to render
                // target='blank'.
                // If the targetValue is other than _blank then we fall back to using the "target" attribute.
                // This is a heuristic that can be refined over time.
                if (targetValue.Equals("_blank", StringComparison.OrdinalIgnoreCase))
                {
                    const string Js = "window.open(this.href, '_blank', ''); return false;";
                    writer.WriteAttribute("onclick", Js);
                    writer.WriteAttribute("onkeypress", Js);
                }
                else
                {
                    writer.WriteAttribute("target", targetValue);
                }
            }
        }

        /// <summary>
        /// Makes the child id.
        /// </summary>
        /// <param name="postfix">
        /// The postfix.
        /// </param>
        /// <returns>
        /// The make child id.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public string MakeChildId(string postfix)
        {
            return this.AdaptedControl.ClientID + "_" + postfix;
        }

        /// <summary>
        /// Makes the name of the child.
        /// </summary>
        /// <param name="postfix">
        /// The postfix.
        /// </param>
        /// <returns>
        /// The make child name.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public string MakeChildName(string postfix)
        {
            return MakeNameFromId(this.MakeChildId(postfix));
        }

        /// <summary>
        /// Raises the adapted event.
        /// </summary>
        /// <param name="eventName">
        /// Name of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void RaiseAdaptedEvent(string eventName, EventArgs e)
        {
            var attr = "OnAdapted" + eventName;
            if ((this.AdaptedControl != null) && (!String.IsNullOrEmpty(this.AdaptedControl.Attributes[attr])))
            {
                var delegateName = this.AdaptedControl.Attributes[attr];
                var methodOwner = this.AdaptedControl.Parent;
                var method = methodOwner.GetType().GetMethod(delegateName);
                if (method == null)
                {
                    methodOwner = this.AdaptedControl.Page;
                    method = methodOwner.GetType().GetMethod(delegateName);
                }

                if (method != null)
                {
                    var args = new object[2];
                    args[0] = this.AdaptedControl;
                    args[1] = e;
                    method.Invoke(methodOwner, args);
                }
            }
        }

        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void RegisterScripts()
        {
            // Uso el registro de adapter para asegurarme que no se registren 2 veces los scripts extra
            if (
                !this.AdaptedControl.Page.ClientScript.IsClientScriptIncludeRegistered(
                    this.GetType(), this.GetType().ToString()))
            {
                var folderPath = WebConfigurationManager.AppSettings.Get("CSSFriendly-JavaScript-Path");
                if (String.IsNullOrEmpty(folderPath))
                {
                    folderPath = "~/JavaScript";
                }

                var filePath = folderPath.EndsWith("/")
                                   ? folderPath + "AdapterUtils.js"
                                   : folderPath + "/AdapterUtils.js";
                this.AdaptedControl.Page.ClientScript.RegisterClientScriptInclude(
                    this.GetType(), this.GetType().ToString(), this.AdaptedControl.Page.ResolveUrl(filePath));
            }
        }

        /// <summary>
        /// Renders the begin tag.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="cssClass">
        /// The CSS class.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void RenderBeginTag(HtmlTextWriter writer, string cssClass)
        {
            var id = (this.AdaptedControl != null) ? this.AdaptedControl.ClientID : string.Empty;

            if (this.AdaptedControl != null && !String.IsNullOrEmpty(this.AdaptedControl.Attributes["CssSelectorClass"]))
            {
                WriteBeginDiv(writer, this.AdaptedControl.Attributes["CssSelectorClass"], id);
                id = string.Empty;
            }

            WriteBeginDiv(writer, cssClass, id);
        }

        /// <summary>
        /// Renders the end tag.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void RenderEndTag(HtmlTextWriter writer)
        {
            WriteEndDiv(writer);

            if (!String.IsNullOrEmpty(this.AdaptedControl.Attributes["CssSelectorClass"]))
            {
                WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <returns>
        /// The resolve url.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public string ResolveUrl(string url)
        {
            var urlToResolve = url;
            var nPound = url.LastIndexOf("#");
            var nSlash = url.LastIndexOf("/");
            if ((nPound > -1) && (nSlash > -1) && ((nSlash + 1) == nPound))
            {
                // We have been given a somewhat strange URL.  It has a foreward slash (/) immediately followed
                // by a pound sign (#) like this xxx/#yyy.  This sort of oddly shaped URL is what you get when
                // you use named anchors instead of pages in the url attribute of a sitemapnode in an ASP.NET
                // sitemap like this:
                // <siteMapNode url="#Introduction" title="Introduction"  description="Introduction" />
                // The intend of the sitemap author is clearly to create a link to a named anchor in the page
                // that looks like these:
                // <a id="Introduction"></a>       (XHTML 1.1 Strict compliant)
                // <a name="Introduction"></a>     (more old fashioned but quite common in many pages)
                // However, the sitemap interpretter in ASP.NET doesn't understand url values that start with
                // a pound.  It prepends the current site's path in front of it before making it into a link
                // (typically for a TreeView or Menu).  We'll undo that problem, however, by converting this
                // sort of oddly shaped URL back into what was intended: a simple reference to a named anchor
                // that is expected to be within the current page.
                urlToResolve = url.Substring(nPound);
            }
            else
            {
                urlToResolve = this.AdaptedControl.ResolveClientUrl(urlToResolve);
            }

            // And, just to be safe, we'll make sure there aren't any troublesome characters in whatever URL
            // we have decided to use at this point.
            var newUrl = this.AdaptedControl.Page.Server.HtmlEncode(urlToResolve);

            return newUrl;
        }

        /// <summary>
        /// Writes the check box.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="labelClassName">
        /// Name of the label class.
        /// </param>
        /// <param name="labelText">
        /// The label text.
        /// </param>
        /// <param name="inputClassName">
        /// Name of the input class.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="isChecked">
        /// if set to <c>true</c> [is checked].
        /// </param>
        /// <remarks>
        /// </remarks>
        public void WriteCheckBox(
            HtmlTextWriter writer, 
            string labelClassName, 
            string labelText, 
            string inputClassName, 
            string id, 
            bool isChecked)
        {
            writer.WriteLine();
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "checkbox");
            if (!String.IsNullOrEmpty(inputClassName))
            {
                writer.WriteAttribute("class", inputClassName);
            }

            writer.WriteAttribute("id", this.MakeChildId(id));
            writer.WriteAttribute("name", this.MakeChildName(id));
            if (isChecked)
            {
                writer.WriteAttribute("checked", "checked");
            }

            if (this.AutoAccessKey && (!String.IsNullOrEmpty(labelText)))
            {
                writer.WriteAttribute("accesskey", labelText[0].ToString());
            }

            writer.Write(HtmlTextWriter.SelfClosingTagEnd);

            this.WriteLabel(writer, labelClassName, labelText, id);
        }

        // Can't be static because it uses MakeChildId
        /// <summary>
        /// Writes the label.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="forId">
        /// For id.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void WriteLabel(HtmlTextWriter writer, string className, string text, string forId)
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }

            writer.WriteLine();
            writer.WriteBeginTag("label");
            writer.WriteAttribute("for", this.MakeChildId(forId));
            if (!String.IsNullOrEmpty(className))
            {
                writer.WriteAttribute("class", className);
            }

            writer.Write(HtmlTextWriter.TagRightChar);

            if (this.AutoAccessKey)
            {
                writer.WriteBeginTag("em");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(text[0].ToString());
                writer.WriteEndTag("em");
                if (!String.IsNullOrEmpty(text))
                {
                    writer.Write(text.Substring(1));
                }
            }
            else
            {
                writer.Write(text);
            }

            writer.WriteEndTag("label");
        }

        /// <summary>
        /// Writes the read only text box.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="labelClassName">
        /// Name of the label class.
        /// </param>
        /// <param name="labelText">
        /// The label text.
        /// </param>
        /// <param name="inputClassName">
        /// Name of the input class.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void WriteReadOnlyTextBox(
            HtmlTextWriter writer, string labelClassName, string labelText, string inputClassName, string value)
        {
            var oldDisableAutoAccessKey = this.disableAutoAccessKey;
            this.disableAutoAccessKey = true;

            this.WriteLabel(writer, labelClassName, labelText, string.Empty);

            writer.WriteLine();
            writer.WriteBeginTag("input");
            writer.WriteAttribute("readonly", "readonly");
            if (!String.IsNullOrEmpty(inputClassName))
            {
                writer.WriteAttribute("class", inputClassName);
            }

            writer.WriteAttribute("value", value);
            writer.Write(HtmlTextWriter.SelfClosingTagEnd);

            this.disableAutoAccessKey = oldDisableAutoAccessKey;
        }

        // Can't be static because it uses MakeChildId

        // Can't be static because it uses MakeChildId
        /// <summary>
        /// Writes the submit.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="buttonType">
        /// Type of the button.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="imageUrl">
        /// The image URL.
        /// </param>
        /// <param name="javascript">
        /// The javascript.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void WriteSubmit(
            HtmlTextWriter writer, 
            ButtonType buttonType, 
            string className, 
            string id, 
            string imageUrl, 
            string javascript, 
            string text)
        {
            writer.WriteLine();

            var idWithType = id;

            switch (buttonType)
            {
                case ButtonType.Button:
                    writer.WriteBeginTag("input");
                    writer.WriteAttribute("type", "submit");
                    writer.WriteAttribute("value", text);
                    idWithType += "Button";
                    break;
                case ButtonType.Image:
                    writer.WriteBeginTag("input");
                    writer.WriteAttribute("type", "image");
                    writer.WriteAttribute("src", imageUrl);
                    idWithType += "ImageButton";
                    break;
                case ButtonType.Link:
                    writer.WriteBeginTag("a");
                    idWithType += "LinkButton";
                    break;
            }

            if (!String.IsNullOrEmpty(className))
            {
                writer.WriteAttribute("class", className);
            }

            writer.WriteAttribute("id", this.MakeChildId(idWithType));
            writer.WriteAttribute("name", this.MakeChildName(idWithType));

            if (!String.IsNullOrEmpty(javascript))
            {
                var pureJs = javascript;
                if (pureJs.StartsWith("javascript:"))
                {
                    pureJs = pureJs.Substring("javascript:".Length);
                }

                switch (buttonType)
                {
                    case ButtonType.Button:
                        writer.WriteAttribute("onclick", pureJs);
                        break;
                    case ButtonType.Image:
                        writer.WriteAttribute("onclick", pureJs);
                        break;
                    case ButtonType.Link:
                        writer.WriteAttribute("href", javascript);
                        break;
                }
            }

            if (buttonType == ButtonType.Link)
            {
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(text);
                writer.WriteEndTag("a");
            }
            else
            {
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
            }
        }

        /// <summary>
        /// Writes the text box.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="isPassword">
        /// if set to <c>true</c> [is password].
        /// </param>
        /// <param name="labelClassName">
        /// Name of the label class.
        /// </param>
        /// <param name="labelText">
        /// The label text.
        /// </param>
        /// <param name="inputClassName">
        /// Name of the input class.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void WriteTextBox(
            HtmlTextWriter writer, 
            bool isPassword, 
            string labelClassName, 
            string labelText, 
            string inputClassName, 
            string id, 
            string value)
        {
            this.WriteLabel(writer, labelClassName, labelText, id);

            writer.WriteLine();
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", isPassword ? "password" : "text");
            if (!String.IsNullOrEmpty(inputClassName))
            {
                writer.WriteAttribute("class", inputClassName);
            }

            writer.WriteAttribute("id", this.MakeChildId(id));
            writer.WriteAttribute("name", this.MakeChildName(id));
            writer.WriteAttribute("value", value);
            if (this.AutoAccessKey && (!String.IsNullOrEmpty(labelText)))
            {
                writer.WriteAttribute("accesskey", labelText[0].ToString().ToLower());
            }

            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
        }

        #endregion
    }

    /// <summary>
    /// The control restoration info.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ControlRestorationInfo
    {
        #region Constants and Fields

        /// <summary>
        /// The coll.
        /// </summary>
        private readonly ControlCollection coll;

        /// <summary>
        /// The ctrl.
        /// </summary>
        private readonly Control ctrl;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlRestorationInfo"/> class.
        /// </summary>
        /// <param name="ctrl">
        /// The CTRL.
        /// </param>
        /// <param name="coll">
        /// The coll.
        /// </param>
        /// <remarks>
        /// </remarks>
        public ControlRestorationInfo(Control ctrl, ControlCollection coll)
        {
            this.ctrl = ctrl;
            this.coll = coll;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the collection.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ControlCollection Collection
        {
            get
            {
                return this.coll;
            }
        }

        /// <summary>
        ///   Gets the control.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Control Control
        {
            get
            {
                return this.ctrl;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public bool IsValid
        {
            get
            {
                return (this.Control != null) && (this.Collection != null);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Restores this instance.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void Restore()
        {
            if (this.IsValid)
            {
                this.coll.Add(this.ctrl);
            }
        }

        #endregion
    }
}