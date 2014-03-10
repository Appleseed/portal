// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The tree view adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.Adapters;

    /// <summary>
    /// The tree view adapter.
    /// </summary>
    public class TreeViewAdapter : HierarchicalDataBoundControlAdapter, IPostBackEventHandler, IPostBackDataHandler
    {
        #region Constants and Fields

        /// <summary>
        ///   The view state.
        /// </summary>
        private readonly HiddenField viewState;

        /// <summary>
        ///   The checkbox index.
        /// </summary>
        private int checkboxIndex = 1;

        /// <summary>
        ///   The extender.
        /// </summary>
        private WebControlAdapterExtender extender;

        /// <summary>
        ///   The new view state.
        /// </summary>
        private string newViewState = string.Empty;

        /// <summary>
        ///   The update view state.
        /// </summary>
        private bool updateViewState;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TreeViewAdapter" /> class.
        /// </summary>
        public TreeViewAdapter()
        {
            if (this.viewState == null)
            {
                this.viewState = new HiddenField();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets Extender.
        /// </summary>
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((this.extender == null) && (this.Control != null)) ||
                    ((this.extender != null) && (this.Control != this.extender.AdaptedControl)))
                {
                    this.extender = new WebControlAdapterExtender(this.Control);
                }

                Debug.Assert(this.extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return this.extender;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The expand to depth.
        /// </summary>
        /// <param name="nodes">
        /// The nodes.
        /// </param>
        /// <param name="expandDepth">
        /// The expand depth.
        /// </param>
        public static void ExpandToDepth(TreeNodeCollection nodes, int expandDepth)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (var node in nodes.Cast<TreeNode>().Where(node => node.Depth < expandDepth))
            {
                node.Expand();
                ExpandToDepth(node.ChildNodes, expandDepth);
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IPostBackDataHandler

        /// <summary>
        /// When implemented by a class, processes postback data for an ASP.NET server control.
        /// </summary>
        /// <param name="postDataKey">
        /// The key identifier for the control.
        /// </param>
        /// <param name="postCollection">
        /// The collection of all incoming name values.
        /// </param>
        /// <returns>
        /// true if the server control's state changes as a result of the postback; otherwise, false.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            return true;
        }

        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual void RaisePostDataChangedEvent()
        {
            var treeView = this.Control as TreeView;
            if (treeView == null)
            {
                return;
            }

            var items = treeView.Nodes;
            this.checkboxIndex = 1;
            this.UpdateCheckmarks(items);
        }

        #endregion

        #region IPostBackEventHandler

        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">
        /// A <see cref="T:System.String"/> that represents an optional event argument to be passed to the event handler.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void RaisePostBackEvent(string eventArgument)
        {
            var treeView = this.Control as TreeView;
            if (treeView == null)
            {
                return;
            }

            var items = treeView.Nodes;
            if (String.IsNullOrEmpty(eventArgument))
            {
                return;
            }

            if (eventArgument.StartsWith("s") || eventArgument.StartsWith("e"))
            {
                var selectedNodeValuePath = eventArgument.Substring(1).Replace("\\", "/");
                var selectedNode = treeView.FindNode(selectedNodeValuePath);
                if (selectedNode != null)
                {
                    var bSelectedNodeChanged = selectedNode != treeView.SelectedNode;
                    this.ClearSelectedNode(items);
                    selectedNode.Selected = true;

                    // does not raise the SelectedNodeChanged event so we have to do it manually (below).
                    if (eventArgument.StartsWith("e"))
                    {
                        selectedNode.Expand();
                    }

                    if (bSelectedNodeChanged)
                    {
                        this.Extender.RaiseAdaptedEvent("SelectedNodeChanged", new EventArgs());
                    }
                }
            }
            else if (eventArgument.StartsWith("p"))
            {
                var parentNodeValuePath = eventArgument.Substring(1).Replace("\\", "/");
                var parentNode = treeView.FindNode(parentNodeValuePath);
                if ((parentNode != null) && (parentNode.ChildNodes.Count == 0))
                {
                    parentNode.Expand(); // Raises the TreeNodePopulate event
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Loads adapter view state information that was saved by <see cref="M:System.Web.UI.Adapters.ControlAdapter.SaveAdapterViewState"/> during a previous request to the page where the control associated with this control adapter resides.
        /// </summary>
        /// <param name="state">
        /// An <see cref="T:System.Object"/> that contains the adapter view state information as a <see cref="T:System.Web.UI.StateBag"/>.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void LoadAdapterViewState(object state)
        {
            var treeView = this.Control as TreeView;
            var oldViewState = state as String;
            if ((treeView == null) || (oldViewState == null) || (oldViewState.Split('|').Length != 2))
            {
                return;
            }

            var hiddenInputName = oldViewState.Split('|')[0];
            var oldExpansionState = oldViewState.Split('|')[1];
            if (!treeView.ShowExpandCollapse)
            {
                this.newViewState = oldExpansionState;
                this.updateViewState = true;
            }
            else if (!String.IsNullOrEmpty(this.Page.Request.Form[hiddenInputName]))
            {
                this.newViewState = this.Page.Request.Form[hiddenInputName];
                this.updateViewState = true;
            }
        }

        /// <summary>
        /// Overrides the <see cref="M:System.Web.UI.Control.OnInit(System.EventArgs)"/> method for the associated control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            if (this.Extender.AdapterEnabled)
            {
                this.updateViewState = false;
                this.newViewState = string.Empty;

                var treeView = this.Control as TreeView;
                if (treeView != null)
                {
                    treeView.EnableClientScript = false;
                }
            }

            base.OnInit(e);

            if (this.Extender.AdapterEnabled)
            {
                this.RegisterScripts();
            }
        }

        /// <summary>
        /// Overrides the <see cref="M:System.Web.UI.Control.OnLoad(System.EventArgs)"/> method for the associated control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var treeView = this.Control as TreeView;
            if (!this.Extender.AdapterEnabled || !this.updateViewState || (treeView == null))
            {
                return;
            }

            treeView.CollapseAll();
            this.ExpandToState(treeView.Nodes, this.newViewState);
            this.updateViewState = false;
        }

        /// <summary>
        /// Creates the beginning tag for the Web control in the markup that is transmitted to the target browser.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> containing methods to render the target-specific output.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (this.Extender.AdapterEnabled)
            {
                this.Extender.RenderBeginTag(writer, "AspNet-TreeView");
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        /// <summary>
        /// Generates the target-specific inner markup for the Web control to which the control adapter is attached.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> containing methods to render the target-specific output.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.Extender.AdapterEnabled)
            {
                var treeView = this.Control as TreeView;
                if (treeView != null)
                {
                    writer.Indent++;
                    this.checkboxIndex = 1;
                    this.BuildItems(treeView.Nodes, true, true, writer);
                    writer.Indent--;
                    writer.WriteLine();
                }
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// <summary>
        /// Creates the ending tag for the Web control in the markup that is transmitted to the target browser.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> containing methods to render the target-specific output.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (this.Extender.AdapterEnabled)
            {
                this.Extender.RenderEndTag(writer);
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        /// <summary>
        /// Saves view state information for the control adapter.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that contains the adapter view state information as a <see cref="T:System.Web.UI.StateBag"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        protected override object SaveAdapterViewState()
        {
            var retStr = string.Empty;
            var treeView = this.Control as TreeView;
            if ((treeView != null) && (this.viewState != null))
            {
                if ((this.Page != null) && (this.Page.Form != null) &&
                    (!this.Page.Form.Controls.Contains(this.viewState)))
                {
                    var panel = new Panel();
                    panel.Controls.Add(this.viewState);
                    this.Page.Form.Controls.Add(panel);
                    var script =
                        string.Format(
                            "document.getElementById('{0}').value = GetViewState__AspNetTreeView('{1}');", 
                            this.viewState.ClientID, 
                            this.Extender.MakeChildId("UL"));
                    this.Page.ClientScript.RegisterOnSubmitStatement(
                        typeof(TreeViewAdapter), this.viewState.ClientID, script);
                }

                retStr = string.Format(
                    "{0}|{1}", this.viewState.UniqueID, this.ComposeViewState(treeView.Nodes, string.Empty));
            }

            return retStr;
        }

        /// <summary>
        /// Determines whether the specified item has children.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified item has children; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static bool HasChildren(TreeNode item)
        {
            return (item != null) && (item.ChildNodes.Count > 0);
        }

        /// <summary>
        /// Determines whether the specified item is expandable.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified item is expandable; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static bool IsExpandable(TreeNode item)
        {
            return HasChildren(item) || ((item != null) && item.PopulateOnDemand);
        }

        /// <summary>
        /// Builds the item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void BuildItem(TreeNode item, HtmlTextWriter writer)
        {
            var treeView = this.Control as TreeView;
            if ((treeView == null) || (item == null) || (writer == null))
            {
                return;
            }

            writer.WriteLine();
            writer.WriteBeginTag("li");
            writer.WriteAttribute("class", this.GetNodeClass(item));
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;
            writer.WriteLine();

            if (IsExpandable(item) && treeView.ShowExpandCollapse)
            {
                this.WriteNodeExpander(treeView, item, writer);
            }

            if (this.IsCheckbox(treeView, item))
            {
                this.WriteNodeCheckbox(treeView, item, writer);
            }
            else if (this.IsLink(item))
            {
                this.WriteNodeLink(treeView, item, writer);
            }
            else
            {
                this.WriteNodePlain(treeView, item, writer);
            }

            if (HasChildren(item))
            {
                this.BuildItems(item.ChildNodes, false, item.Expanded.Equals(true), writer);
            }

            writer.Indent--;
            writer.WriteLine();
            writer.WriteEndTag("li");
        }

        /// <summary>
        /// Builds the items.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="isRoot">
        /// if set to <c>true</c> [is root].
        /// </param>
        /// <param name="isExpanded">
        /// if set to <c>true</c> [is expanded].
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void BuildItems(TreeNodeCollection items, bool isRoot, bool isExpanded, HtmlTextWriter writer)
        {
            if (items.Count <= 0)
            {
                return;
            }

            writer.WriteLine();

            writer.WriteBeginTag("ul");

            if (isRoot)
            {
                writer.WriteAttribute("id", this.Extender.MakeChildId("UL"));
            }

            if (!isExpanded)
            {
                writer.WriteAttribute("class", "AspNet-TreeView-Hide");
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;

            foreach (TreeNode item in items)
            {
                this.BuildItem(item, writer);
            }

            writer.Indent--;
            writer.WriteLine();
            writer.WriteEndTag("ul");
        }

        /// <summary>
        /// Clears the selected node.
        /// </summary>
        /// <param name="nodes">
        /// The nodes.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void ClearSelectedNode(TreeNodeCollection nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (TreeNode node in nodes)
            {
                if (node.Selected)
                {
                    node.Selected = false;
                }

                this.ClearSelectedNode(node.ChildNodes);
            }
        }

        /// <summary>
        /// Composes the state of the view.
        /// </summary>
        /// <param name="nodes">
        /// The nodes.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The compose view state.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string ComposeViewState(TreeNodeCollection nodes, string state)
        {
            if (nodes != null)
            {
                foreach (var node in nodes.Cast<TreeNode>().Where(IsExpandable))
                {
                    if (node.Expanded.Equals(true))
                    {
                        state += "e";
                        state = this.ComposeViewState(node.ChildNodes, state);
                    }
                    else
                    {
                        state += "n";
                    }
                }
            }

            return state;
        }

        /// <summary>
        /// Expands to state.
        /// </summary>
        /// <param name="nodes">
        /// The nodes.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The expand to state.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string ExpandToState(TreeNodeCollection nodes, string state)
        {
            if ((nodes != null) && (!String.IsNullOrEmpty(state)))
            {
                foreach (TreeNode node in nodes)
                {
                    if (!IsExpandable(node))
                    {
                        continue;
                    }

                    var bExpand = state[0] == 'e';
                    state = state.Substring(1);
                    if (!bExpand)
                    {
                        continue;
                    }

                    node.Expand();
                    state = this.ExpandToState(node.ChildNodes, state);
                }
            }

            return state;
        }

        /// <summary>
        /// Gets the image SRC.
        /// </summary>
        /// <param name="treeView">
        /// The tree view.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The get image src.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string GetImageSrc(TreeView treeView, TreeNode item)
        {
            var imgSrc = string.Empty;

            if ((treeView != null) && (item != null))
            {
                imgSrc = item.ImageUrl;

                if (String.IsNullOrEmpty(imgSrc))
                {
                    if (item.Depth == 0)
                    {
                        if (!String.IsNullOrEmpty(treeView.RootNodeStyle.ImageUrl))
                        {
                            imgSrc = treeView.RootNodeStyle.ImageUrl;
                        }
                    }
                    else
                    {
                        if (!IsExpandable(item))
                        {
                            if (!String.IsNullOrEmpty(treeView.LeafNodeStyle.ImageUrl))
                            {
                                imgSrc = treeView.LeafNodeStyle.ImageUrl;
                            }
                        }
                        else if (!String.IsNullOrEmpty(treeView.ParentNodeStyle.ImageUrl))
                        {
                            imgSrc = treeView.ParentNodeStyle.ImageUrl;
                        }
                    }
                }

                if (String.IsNullOrEmpty(imgSrc) && (treeView.LevelStyles.Count > item.Depth))
                {
                    if (!String.IsNullOrEmpty(treeView.LevelStyles[item.Depth].ImageUrl))
                    {
                        imgSrc = treeView.LevelStyles[item.Depth].ImageUrl;
                    }
                }
            }

            return imgSrc;
        }

        /// <summary>
        /// Gets the node class.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The get node class.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string GetNodeClass(TreeNode item)
        {
            var value = "AspNet-TreeView-Leaf";
            if (item != null)
            {
                if (item.Depth == 0)
                {
                    value = IsExpandable(item) ? "AspNet-TreeView-Root" : "AspNet-TreeView-Root AspNet-TreeView-Leaf";
                }
                else if (IsExpandable(item))
                {
                    value = "AspNet-TreeView-Parent";
                }

                if (item.Selected)
                {
                    value += " AspNet-TreeView-Selected";
                }
                else if (IsChildNodeSelected(item))
                {
                    value += " AspNet-TreeView-ChildSelected";
                }
                else if (this.IsParentNodeSelected(item))
                {
                    value += " AspNet-TreeView-ParentSelected";
                }
            }

            return value;
        }

        /// <summary>
        /// Determines whether the specified tree view is checkbox.
        /// </summary>
        /// <param name="treeView">
        /// The tree view.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified tree view is checkbox; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsCheckbox(TreeView treeView, TreeNode item)
        {
            var bItemCheckBoxDisallowed = (item.ShowCheckBox != null) && (item.ShowCheckBox.Value == false);
            var bItemCheckBoxWanted = (item.ShowCheckBox != null) && item.ShowCheckBox.Value;
            var bTreeCheckBoxWanted = (treeView.ShowCheckBoxes == TreeNodeTypes.All) ||
                                      ((treeView.ShowCheckBoxes == TreeNodeTypes.Leaf) && (!IsExpandable(item))) ||
                                      ((treeView.ShowCheckBoxes == TreeNodeTypes.Parent) && IsExpandable(item)) ||
                                      ((treeView.ShowCheckBoxes == TreeNodeTypes.Root) && (item.Depth == 0));

            return (!bItemCheckBoxDisallowed) && (bItemCheckBoxWanted || bTreeCheckBoxWanted);
        }

        /// <summary>
        /// Determines whether [is child node selected] [the specified item].
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is child node selected] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsChildNodeSelected(TreeNode item)
        {
            var bRet = false;

            if (item != null)
            {
                bRet = IsChildNodeSelected(item.ChildNodes);
            }

            return bRet;
        }

        /// <summary>
        /// Determines whether [is child node selected] [the specified nodes].
        /// </summary>
        /// <param name="nodes">
        /// The nodes.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is child node selected] [the specified nodes]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsChildNodeSelected(TreeNodeCollection nodes)
        {
            var bRet = false;

            if (nodes != null)
            {
                bRet = nodes.Cast<TreeNode>().Any(node => node.Selected || this.IsChildNodeSelected(node.ChildNodes));
            }

            return bRet;
        }

        /// <summary>
        /// Determines whether the specified item is link.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified item is link; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsLink(TreeNode item)
        {
            return (item != null) &&
                   ((!String.IsNullOrEmpty(item.NavigateUrl)) || item.PopulateOnDemand ||
                    (item.SelectAction == TreeNodeSelectAction.Select) ||
                    (item.SelectAction == TreeNodeSelectAction.SelectExpand));
        }

        /// <summary>
        /// Determines whether [is parent node selected] [the specified item].
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is parent node selected] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsParentNodeSelected(TreeNode item)
        {
            var bRet = false;

            if ((item != null) && (item.Parent != null))
            {
                bRet = item.Parent.Selected || this.IsParentNodeSelected(item.Parent);
            }

            return bRet;
        }

        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void RegisterScripts()
        {
            this.Extender.RegisterScripts();
            var folderPath = WebConfigurationManager.AppSettings.Get("CSSFriendly-JavaScript-Path");
            if (String.IsNullOrEmpty(folderPath))
            {
                folderPath = "~/JavaScript";
            }

            var filePath = folderPath.EndsWith("/")
                               ? folderPath + "TreeViewAdapter.js"
                               : folderPath + "/TreeViewAdapter.js";
            this.Page.ClientScript.RegisterClientScriptInclude(
                this.GetType(), this.GetType().ToString(), this.Page.ResolveUrl(filePath));
        }

        /// <summary>
        /// Updates the checkmarks.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void UpdateCheckmarks(TreeNodeCollection items)
        {
            var treeView = this.Control as TreeView;
            if ((treeView == null) || (items == null))
            {
                return;
            }

            foreach (TreeNode item in items)
            {
                if (this.IsCheckbox(treeView, item))
                {
                    var name = string.Format("{0}n{1}CheckBox", treeView.UniqueID, this.checkboxIndex);
                    var bIsNowChecked = this.Page.Request.Form[name] != null;
                    if (item.Checked != bIsNowChecked)
                    {
                        item.Checked = bIsNowChecked;
                        this.Extender.RaiseAdaptedEvent("TreeNodeCheckChanged", new TreeNodeEventArgs(item));
                    }

                    this.checkboxIndex++;
                }

                if (HasChildren(item))
                {
                    this.UpdateCheckmarks(item.ChildNodes);
                }
            }
        }

        /// <summary>
        /// Writes the node checkbox.
        /// </summary>
        /// <param name="treeView">
        /// The tree view.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteNodeCheckbox(TreeView treeView, TreeNode item, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "checkbox");
            writer.WriteAttribute("id", string.Format("{0}n{1}CheckBox", treeView.ClientID, this.checkboxIndex));
            writer.WriteAttribute("name", string.Format("{0}n{1}CheckBox", treeView.UniqueID, this.checkboxIndex));

            if (!String.IsNullOrEmpty(treeView.Attributes["OnClientClickedCheckbox"]))
            {
                writer.WriteAttribute("onclick", treeView.Attributes["OnClientClickedCheckbox"]);
            }

            if (item.Checked)
            {
                writer.WriteAttribute("checked", "checked");
            }

            writer.Write(HtmlTextWriter.SelfClosingTagEnd);

            if (!String.IsNullOrEmpty(item.Text))
            {
                writer.WriteLine();
                writer.WriteBeginTag("label");
                writer.WriteAttribute("for", string.Format("{0}n{1}CheckBox", treeView.ClientID, this.checkboxIndex));
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(item.Text);
                writer.WriteEndTag("label");
            }

            this.checkboxIndex++;
        }

        /// <summary>
        /// Writes the node expander.
        /// </summary>
        /// <param name="treeView">
        /// The tree view.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteNodeExpander(TreeView treeView, TreeNode item, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("span");
            writer.WriteAttribute(
                "class", item.Expanded.Equals(true) ? "AspNet-TreeView-Collapse" : "AspNet-TreeView-Expand");
            if (HasChildren(item))
            {
                writer.WriteAttribute("onclick", "ExpandCollapse__AspNetTreeView(this)");
            }
            else
            {
                writer.WriteAttribute(
                    "onclick", 
                    this.Page.ClientScript.GetPostBackEventReference(
                        treeView, 
                        string.Format("p{0}", this.Page.Server.HtmlEncode(item.ValuePath).Replace("/", "\\")), 
                        true));
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&nbsp;");
            writer.WriteEndTag("span");
            writer.WriteLine();
        }

        /// <summary>
        /// Writes the node image.
        /// </summary>
        /// <param name="treeView">
        /// The tree view.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteNodeImage(TreeView treeView, TreeNode item, HtmlTextWriter writer)
        {
            var imgSrc = this.GetImageSrc(treeView, item);
            if (!String.IsNullOrEmpty(imgSrc))
            {
                writer.WriteBeginTag("img");
                writer.WriteAttribute("src", treeView.ResolveClientUrl(imgSrc));
                writer.WriteAttribute(
                    "alt", 
                    !String.IsNullOrEmpty(item.ToolTip)
                        ? item.ToolTip
                        : (!String.IsNullOrEmpty(treeView.ToolTip) ? treeView.ToolTip : item.Text));
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
            }
        }

        /// <summary>
        /// Writes the node link.
        /// </summary>
        /// <param name="treeView">
        /// The tree view.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteNodeLink(TreeView treeView, TreeNode item, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("a");

            if (!String.IsNullOrEmpty(item.NavigateUrl))
            {
                writer.WriteAttribute("href", this.Extender.ResolveUrl(item.NavigateUrl));
            }
            else
            {
                var codePrefix = string.Empty;
                if (item.SelectAction == TreeNodeSelectAction.Select)
                {
                    codePrefix = "s";
                }
                else if (item.SelectAction == TreeNodeSelectAction.SelectExpand)
                {
                    codePrefix = "e";
                }
                else if (item.PopulateOnDemand)
                {
                    codePrefix = "p";
                }

                writer.WriteAttribute(
                    "href", 
                    this.Page.ClientScript.GetPostBackClientHyperlink(
                        treeView, codePrefix + this.Page.Server.HtmlEncode(item.ValuePath).Replace("/", "\\"), true));
            }

            WebControlAdapterExtender.WriteTargetAttribute(writer, item.Target);

            if (!String.IsNullOrEmpty(item.ToolTip))
            {
                writer.WriteAttribute("title", item.ToolTip);
            }
            else if (!String.IsNullOrEmpty(treeView.ToolTip))
            {
                writer.WriteAttribute("title", treeView.ToolTip);
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;
            writer.WriteLine();

            this.WriteNodeImage(treeView, item, writer);
            writer.Write(item.Text);

            writer.Indent--;
            writer.WriteEndTag("a");
        }

        /// <summary>
        /// Writes the node plain.
        /// </summary>
        /// <param name="treeView">
        /// The tree view.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteNodePlain(TreeView treeView, TreeNode item, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("span");
            if (IsExpandable(item))
            {
                writer.WriteAttribute("class", "AspNet-TreeView-ClickableNonLink");
                if (treeView.ShowExpandCollapse)
                {
                    writer.WriteAttribute(
                        "onclick", "ExpandCollapse__AspNetTreeView(this.parentNode.getElementsByTagName('span')[0])");
                }
            }
            else
            {
                writer.WriteAttribute("class", "AspNet-TreeView-NonLink");
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;
            writer.WriteLine();

            this.WriteNodeImage(treeView, item, writer);
            writer.Write(item.Text);

            writer.Indent--;
            writer.WriteEndTag("span");
        }

        #endregion
    }
}