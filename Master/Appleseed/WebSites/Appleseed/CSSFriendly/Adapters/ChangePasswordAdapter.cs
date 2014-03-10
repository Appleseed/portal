// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangePasswordAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The change password adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.Adapters;

    /// <summary>
    /// The change password adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ChangePasswordAdapter : WebControlAdapter
    {
        #region Constants and Fields

        /// <summary>
        ///   The extender.
        /// </summary>
        private WebControlAdapterExtender extender;

        /// <summary>
        ///   The state.
        /// </summary>
        private State state = State.ChangePassword;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ChangePasswordAdapter" /> class. 
        ///   Initializes a new instance of the <see cref = "T:System.Web.UI.WebControls.Adapters.WebControlAdapter" /> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ChangePasswordAdapter()
        {
            this.state = State.ChangePassword;
        }

        #endregion

        #region Enums

        /// <summary>
        /// The state.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private enum State
        {
            /// <summary>
            ///   The change password.
            /// </summary>
            ChangePassword, 

            /// <summary>
            ///   The failed.
            /// </summary>
            Failed, 

            /// <summary>
            ///   The success.
            /// </summary>
            Success, 
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the extender.
        /// </summary>
        /// <remarks>
        /// </remarks>
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

        #region Methods

        /// <summary>
        /// Creates the target-specific child controls for a composite control.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            var changePwd = this.Control as ChangePassword;
            if (changePwd != null)
            {
                if (changePwd.ChangePasswordTemplate != null)
                {
                    changePwd.ChangePasswordTemplateContainer.Controls.Clear();
                    changePwd.ChangePasswordTemplate.InstantiateIn(changePwd.ChangePasswordTemplateContainer);
                    changePwd.ChangePasswordTemplateContainer.DataBind();
                }

                if (changePwd.SuccessTemplate != null)
                {
                    changePwd.SuccessTemplateContainer.Controls.Clear();
                    changePwd.SuccessTemplate.InstantiateIn(changePwd.SuccessTemplateContainer);
                    changePwd.SuccessTemplateContainer.DataBind();
                }

                changePwd.Controls.Add(new ChangePasswordCommandBubbler());
            }
        }

        /// <summary>
        /// Called when [change password error].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnChangePasswordError(object sender, EventArgs e)
        {
            if (this.state != State.Success)
            {
                this.state = State.Failed;
            }
        }

        /// <summary>
        /// Called when [changed password].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnChangedPassword(object sender, EventArgs e)
        {
            this.state = State.Success;
        }

        /// <summary>
        /// Overrides the <see cref="M:System.Web.UI.Control.OnInit(System.EventArgs)"/> method for the associated control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        /// /
        /// PROTECTED
        /// <remarks>
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var changePwd = this.Control as ChangePassword;
            if (this.Extender.AdapterEnabled && (changePwd != null))
            {
                RegisterScripts();
                changePwd.ChangedPassword += this.OnChangedPassword;
                changePwd.ChangePasswordError += this.OnChangePasswordError;
                this.state = State.ChangePassword;
            }
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
                this.Extender.RenderBeginTag(writer, "AspNet-ChangePassword");
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
                var changePwd = this.Control as ChangePassword;
                if (changePwd != null)
                {
                    if ((this.state == State.ChangePassword) || (this.state == State.Failed))
                    {
                        if (changePwd.ChangePasswordTemplate != null)
                        {
                            changePwd.ChangePasswordTemplateContainer.RenderControl(writer);
                        }
                        else
                        {
                            WriteChangePasswordTitlePanel(writer, changePwd);
                            WriteInstructionPanel(writer, changePwd);
                            WriteHelpPanel(writer, changePwd);
                            this.WriteUserPanel(writer, changePwd);
                            this.WritePasswordPanel(writer, changePwd);
                            this.WriteNewPasswordPanel(writer, changePwd);
                            this.WriteConfirmNewPasswordPanel(writer, changePwd);
                            if (this.state == State.Failed)
                            {
                                WriteFailurePanel(writer, changePwd);
                            }

                            this.WriteSubmitPanel(writer, changePwd);
                            WriteCreateUserPanel(writer, changePwd);
                            WritePasswordRecoveryPanel(writer, changePwd);
                        }
                    }
                    else if (this.state == State.Success)
                    {
                        if (changePwd.SuccessTemplate != null)
                        {
                            changePwd.SuccessTemplateContainer.RenderControl(writer);
                        }
                        else
                        {
                            WriteSuccessTitlePanel(writer, changePwd);
                            WriteSuccessTextPanel(writer, changePwd);
                            this.WriteContinuePanel(writer, changePwd);
                        }
                    }
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
        /// Registers the scripts.
        /// </summary>
        /// /
        /// PRIVATE
        /// <remarks>
        /// </remarks>
        private static void RegisterScripts()
        {
        }

        /////////////////////////////////////////////////////////
        // Step 1: change password
        /////////////////////////////////////////////////////////

        /// <summary>
        /// Writes the change password title panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteChangePasswordTitlePanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (String.IsNullOrEmpty(changePwd.ChangePasswordTitleText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(changePwd.TitleTextStyle.CssClass))
                                ? changePwd.TitleTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-ChangePassword-ChangePasswordTitlePanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, changePwd.ChangePasswordTitleText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the create user panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteCreateUserPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (String.IsNullOrEmpty(changePwd.CreateUserUrl) && String.IsNullOrEmpty(changePwd.CreateUserText))
            {
                return;
            }

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-CreateUserPanel", string.Empty);
            WebControlAdapterExtender.WriteImage(writer, changePwd.CreateUserIconUrl, "Create user");
            WebControlAdapterExtender.WriteLink(
                writer, 
                changePwd.HyperLinkStyle.CssClass, 
                changePwd.CreateUserUrl, 
                "Create user", 
                changePwd.CreateUserText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the failure panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteFailurePanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            var className = (!String.IsNullOrEmpty(changePwd.FailureTextStyle.CssClass))
                                ? changePwd.FailureTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-ChangePassword-FailurePanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, changePwd.ChangePasswordFailureText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the help panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteHelpPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (String.IsNullOrEmpty(changePwd.HelpPageIconUrl) && String.IsNullOrEmpty(changePwd.HelpPageText))
            {
                return;
            }

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-HelpPanel", string.Empty);
            WebControlAdapterExtender.WriteImage(writer, changePwd.HelpPageIconUrl, "Help");
            WebControlAdapterExtender.WriteLink(
                writer, changePwd.HyperLinkStyle.CssClass, changePwd.HelpPageUrl, "Help", changePwd.HelpPageText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the instruction panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteInstructionPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (String.IsNullOrEmpty(changePwd.InstructionText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(changePwd.InstructionTextStyle.CssClass))
                                ? changePwd.InstructionTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-ChangePassword-InstructionPanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, changePwd.InstructionText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the password recovery panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WritePasswordRecoveryPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (String.IsNullOrEmpty(changePwd.PasswordRecoveryUrl) &&
                String.IsNullOrEmpty(changePwd.PasswordRecoveryText))
            {
                return;
            }

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-PasswordRecoveryPanel", string.Empty);
            WebControlAdapterExtender.WriteImage(writer, changePwd.PasswordRecoveryIconUrl, "Password recovery");
            WebControlAdapterExtender.WriteLink(
                writer, 
                changePwd.HyperLinkStyle.CssClass, 
                changePwd.PasswordRecoveryUrl, 
                "Password recovery", 
                changePwd.PasswordRecoveryText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /////////////////////////////////////////////////////////
        // Step 2: success
        /////////////////////////////////////////////////////////

        /// <summary>
        /// Writes the success text panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteSuccessTextPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (String.IsNullOrEmpty(changePwd.SuccessText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(changePwd.SuccessTextStyle.CssClass))
                                ? changePwd.SuccessTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-ChangePassword-SuccessTextPanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, changePwd.SuccessText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the success title panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteSuccessTitlePanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (String.IsNullOrEmpty(changePwd.SuccessTitleText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(changePwd.TitleTextStyle.CssClass))
                                ? changePwd.TitleTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-ChangePassword-SuccessTitlePanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, changePwd.SuccessTitleText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the confirm new password panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteConfirmNewPasswordPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            var textbox = changePwd.ChangePasswordTemplateContainer.FindControl("ConfirmNewPassword") as TextBox;
            if (textbox != null)
            {
                this.Page.ClientScript.RegisterForEventValidation(textbox.UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(
                    writer, "AspNet-ChangePassword-ConfirmNewPasswordPanel", string.Empty);
                this.Extender.WriteTextBox(
                    writer, 
                    true, 
                    changePwd.LabelStyle.CssClass, 
                    changePwd.ConfirmNewPasswordLabelText, 
                    changePwd.TextBoxStyle.CssClass, 
                    changePwd.ChangePasswordTemplateContainer.ID + "_ConfirmNewPassword", 
                    string.Empty);
                WebControlAdapterExtender.WriteRequiredFieldValidator(
                    writer, 
                    changePwd.ChangePasswordTemplateContainer.FindControl("ConfirmNewPasswordRequired") as
                    RequiredFieldValidator, 
                    changePwd.ValidatorTextStyle.CssClass, 
                    "ConfirmNewPassword", 
                    changePwd.ConfirmPasswordRequiredErrorMessage);
                WebControlAdapterExtender.WriteCompareValidator(
                    writer, 
                    changePwd.ChangePasswordTemplateContainer.FindControl("NewPasswordCompare") as CompareValidator, 
                    changePwd.ValidatorTextStyle.CssClass, 
                    "ConfirmNewPassword", 
                    changePwd.ConfirmPasswordCompareErrorMessage, 
                    "NewPassword");
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Writes the continue panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteContinuePanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-ContinuePanel", string.Empty);

            var id = "Continue";
            id += (changePwd.ChangePasswordButtonType == ButtonType.Button) ? "Push" : string.Empty;
            var idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, changePwd.ContinueButtonType);
            var btn = changePwd.SuccessTemplateContainer.FindControl(idWithType);
            if (btn != null)
            {
                this.Page.ClientScript.RegisterForEventValidation(btn.UniqueID);
                this.Extender.WriteSubmit(
                    writer, 
                    changePwd.ContinueButtonType, 
                    changePwd.ContinueButtonStyle.CssClass, 
                    changePwd.SuccessTemplateContainer.ID + "_" + id, 
                    changePwd.ContinueButtonImageUrl, 
                    string.Empty, 
                    changePwd.ContinueButtonText);
            }

            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the new password panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteNewPasswordPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            var textbox = changePwd.ChangePasswordTemplateContainer.FindControl("NewPassword") as TextBox;
            if (textbox != null)
            {
                this.Page.ClientScript.RegisterForEventValidation(textbox.UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-NewPasswordPanel", string.Empty);
                this.Extender.WriteTextBox(
                    writer, 
                    true, 
                    changePwd.LabelStyle.CssClass, 
                    changePwd.NewPasswordLabelText, 
                    changePwd.TextBoxStyle.CssClass, 
                    changePwd.ChangePasswordTemplateContainer.ID + "_NewPassword", 
                    string.Empty);
                WebControlAdapterExtender.WriteRequiredFieldValidator(
                    writer, 
                    changePwd.ChangePasswordTemplateContainer.FindControl("NewPasswordRequired") as
                    RequiredFieldValidator, 
                    changePwd.ValidatorTextStyle.CssClass, 
                    "NewPassword", 
                    changePwd.NewPasswordRequiredErrorMessage);
                WebControlAdapterExtender.WriteRegularExpressionValidator(
                    writer, 
                    changePwd.ChangePasswordTemplateContainer.FindControl("RegExpValidator") as
                    RegularExpressionValidator, 
                    changePwd.ValidatorTextStyle.CssClass, 
                    "NewPassword", 
                    changePwd.NewPasswordRegularExpressionErrorMessage, 
                    changePwd.NewPasswordRegularExpression);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Writes the password panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WritePasswordPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            var textbox = changePwd.ChangePasswordTemplateContainer.FindControl("CurrentPassword") as TextBox;
            if (textbox != null)
            {
                this.Page.ClientScript.RegisterForEventValidation(textbox.UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-PasswordPanel", string.Empty);
                this.Extender.WriteTextBox(
                    writer, 
                    true, 
                    changePwd.LabelStyle.CssClass, 
                    changePwd.PasswordLabelText, 
                    changePwd.TextBoxStyle.CssClass, 
                    changePwd.ChangePasswordTemplateContainer.ID + "_CurrentPassword", 
                    string.Empty);
                WebControlAdapterExtender.WriteRequiredFieldValidator(
                    writer, 
                    changePwd.ChangePasswordTemplateContainer.FindControl("CurrentPasswordRequired") as
                    RequiredFieldValidator, 
                    changePwd.ValidatorTextStyle.CssClass, 
                    "CurrentPassword", 
                    changePwd.PasswordRequiredErrorMessage);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Writes the submit panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteSubmitPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-SubmitPanel", string.Empty);

            var id = "ChangePassword";
            id += (changePwd.ChangePasswordButtonType == ButtonType.Button) ? "Push" : string.Empty;
            var idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, changePwd.ChangePasswordButtonType);
            var btn = changePwd.ChangePasswordTemplateContainer.FindControl(idWithType);
            if (btn != null)
            {
                this.Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

                var options = new PostBackOptions(
                    btn, string.Empty, string.Empty, false, false, false, true, true, changePwd.UniqueID);
                var javascript = "javascript:" + this.Page.ClientScript.GetPostBackEventReference(options);
                javascript = this.Page.Server.HtmlEncode(javascript);

                this.Extender.WriteSubmit(
                    writer, 
                    changePwd.ChangePasswordButtonType, 
                    changePwd.ChangePasswordButtonStyle.CssClass, 
                    changePwd.ChangePasswordTemplateContainer.ID + "_" + id, 
                    changePwd.ChangePasswordButtonImageUrl, 
                    javascript, 
                    changePwd.ChangePasswordButtonText);
            }

            id = "Cancel";
            id += (changePwd.ChangePasswordButtonType == ButtonType.Button) ? "Push" : string.Empty;
            idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, changePwd.CancelButtonType);
            btn = changePwd.ChangePasswordTemplateContainer.FindControl(idWithType);
            if (btn != null)
            {
                this.Page.ClientScript.RegisterForEventValidation(btn.UniqueID);
                this.Extender.WriteSubmit(
                    writer, 
                    changePwd.CancelButtonType, 
                    changePwd.CancelButtonStyle.CssClass, 
                    changePwd.ChangePasswordTemplateContainer.ID + "_" + id, 
                    changePwd.CancelButtonImageUrl, 
                    string.Empty, 
                    changePwd.CancelButtonText);
            }

            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the user panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="changePwd">
        /// The change PWD.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteUserPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (changePwd.DisplayUserName)
            {
                var textbox = changePwd.ChangePasswordTemplateContainer.FindControl("UserName") as TextBox;
                if (textbox != null)
                {
                    this.Page.ClientScript.RegisterForEventValidation(textbox.UniqueID);
                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-UserPanel", string.Empty);
                    this.Extender.WriteTextBox(
                        writer, 
                        false, 
                        changePwd.LabelStyle.CssClass, 
                        changePwd.UserNameLabelText, 
                        changePwd.TextBoxStyle.CssClass, 
                        changePwd.ChangePasswordTemplateContainer.ID + "_UserName", 
                        changePwd.UserName);
                    WebControlAdapterExtender.WriteRequiredFieldValidator(
                        writer, 
                        changePwd.ChangePasswordTemplateContainer.FindControl("UserNameRequired") as
                        RequiredFieldValidator, 
                        changePwd.ValidatorTextStyle.CssClass, 
                        "UserName", 
                        changePwd.UserNameRequiredErrorMessage);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// The change password command bubbler.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ChangePasswordCommandBubbler : Control
    {
        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.Page.IsPostBack)
            {
                return;
            }

            var changePassword = this.NamingContainer as ChangePassword;
            if (changePassword == null)
            {
                return;
            }

            var container = changePassword.ChangePasswordTemplateContainer;
            if (container == null)
            {
                return;
            }

            CommandEventArgs cmdArgs = null;
            string[] prefixes = { "ChangePassword", "Cancel", "Continue" };
            string[] postfixes = { "PushButton", "Image", "Link" };
            foreach (var prefix in prefixes)
            {
                var prefix1 = prefix;
                if (
                    postfixes.Select(postfix => prefix1 + postfix).Select(container.FindControl).Any(
                        ctrl => (ctrl != null) && (!String.IsNullOrEmpty(this.Page.Request.Params.Get(ctrl.UniqueID)))))
                {
                    switch (prefix)
                    {
                        case "ChangePassword":
                            cmdArgs = new CommandEventArgs(ChangePassword.ChangePasswordButtonCommandName, this);
                            break;
                        case "Cancel":
                            cmdArgs = new CommandEventArgs(ChangePassword.CancelButtonCommandName, this);
                            break;
                        case "Continue":
                            cmdArgs = new CommandEventArgs(ChangePassword.ContinueButtonCommandName, this);
                            break;
                    }
                }

                if (cmdArgs != null)
                {
                    break;
                }
            }

            if ((cmdArgs != null) && (cmdArgs.CommandName == ChangePassword.ChangePasswordButtonCommandName))
            {
                this.Page.Validate();
                if (!this.Page.IsValid)
                {
                    cmdArgs = null;
                }
            }

            if (cmdArgs != null)
            {
                this.RaiseBubbleEvent(this, cmdArgs);
            }
        }

        #endregion
    }
}