// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The login adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Diagnostics;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.Adapters;

    /// <summary>
    /// The login adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class LoginAdapter : WebControlAdapter
    {
        #region Constants and Fields

        /// <summary>
        ///   The extender.
        /// </summary>
        private WebControlAdapterExtender extender;

        /// <summary>
        ///   The state.
        /// </summary>
        private State state = State.LoggingIn;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LoginAdapter" /> class. 
        ///   Initializes a new instance of the <see cref = "T:System.Web.UI.WebControls.Adapters.WebControlAdapter" /> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public LoginAdapter()
        {
            this.state = State.LoggingIn;
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
            ///   The logging in.
            /// </summary>
            LoggingIn, 

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
            var login = this.Control as Login;
            if ((login == null) || (login.Controls.Count != 1) || (login.LayoutTemplate == null))
            {
                return;
            }

            var container = login.Controls[0];
            container.Controls.Clear();
            login.LayoutTemplate.InstantiateIn(container);
            container.DataBind();
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

            var login = this.Control as Login;
            if (!this.Extender.AdapterEnabled || (login == null))
            {
                return;
            }

            RegisterScripts();
            login.LoggedIn += this.OnLoggedIn;
            login.LoginError += this.OnLoginError;
            this.state = State.LoggingIn;
        }

        /// <summary>
        /// Called when [logged in].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnLoggedIn(object sender, EventArgs e)
        {
            this.state = State.Success;
        }

        /// <summary>
        /// Called when [login error].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnLoginError(object sender, EventArgs e)
        {
            this.state = State.Failed;
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
                this.Extender.RenderBeginTag(writer, "AspNet-Login");
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
                var login = this.Control as Login;
                if (login != null)
                {
                    if (login.LayoutTemplate != null)
                    {
                        if (login.Controls.Count == 1)
                        {
                            var container = login.Controls[0];
                            foreach (Control c in container.Controls)
                            {
                                c.RenderControl(writer);
                            }
                        }
                    }
                    else
                    {
                        this.WriteTitlePanel(writer, login);
                        WriteInstructionPanel(writer, login);
                        WriteHelpPanel(writer, login);
                        this.WriteUserPanel(writer, login);
                        this.WritePasswordPanel(writer, login);
                        this.WriteRememberMePanel(writer, login);
                        if (this.state == State.Failed)
                        {
                            WriteFailurePanel(writer, login);
                        }

                        this.WriteSubmitPanel(writer, login);
                        WriteCreateUserPanel(writer, login);
                        WritePasswordRecoveryPanel(writer, login);
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

        /// <summary>
        /// Writes the create user panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteCreateUserPanel(HtmlTextWriter writer, Login login)
        {
            if (String.IsNullOrEmpty(login.CreateUserUrl) && String.IsNullOrEmpty(login.CreateUserText))
            {
                return;
            }

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-CreateUserPanel", string.Empty);
            WebControlAdapterExtender.WriteImage(writer, login.CreateUserIconUrl, "Create user");
            WebControlAdapterExtender.WriteLink(
                writer, login.HyperLinkStyle.CssClass, login.CreateUserUrl, "Create user", login.CreateUserText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the failure panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteFailurePanel(HtmlTextWriter writer, Login login)
        {
            if (!String.IsNullOrEmpty(login.FailureText))
            {
                var className = (!String.IsNullOrEmpty(login.FailureTextStyle.CssClass))
                                    ? login.FailureTextStyle.CssClass + " "
                                    : string.Empty;
                className += "AspNet-Login-FailurePanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
                WebControlAdapterExtender.WriteSpan(writer, string.Empty, login.FailureText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Writes the help panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteHelpPanel(HtmlTextWriter writer, Login login)
        {
            if (String.IsNullOrEmpty(login.HelpPageIconUrl) && String.IsNullOrEmpty(login.HelpPageText))
            {
                return;
            }

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-HelpPanel", string.Empty);
            WebControlAdapterExtender.WriteImage(writer, login.HelpPageIconUrl, "Help");
            WebControlAdapterExtender.WriteLink(
                writer, login.HyperLinkStyle.CssClass, login.HelpPageUrl, "Help", login.HelpPageText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the instruction panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteInstructionPanel(HtmlTextWriter writer, Login login)
        {
            if (String.IsNullOrEmpty(login.InstructionText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(login.InstructionTextStyle.CssClass))
                                ? login.InstructionTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-Login-InstructionPanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, login.InstructionText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the password recovery panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WritePasswordRecoveryPanel(HtmlTextWriter writer, Login login)
        {
            if ((!String.IsNullOrEmpty(login.PasswordRecoveryUrl)) ||
                (!String.IsNullOrEmpty(login.PasswordRecoveryText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-PasswordRecoveryPanel", string.Empty);
                WebControlAdapterExtender.WriteImage(writer, login.PasswordRecoveryIconUrl, "Password recovery");
                WebControlAdapterExtender.WriteLink(
                    writer, 
                    login.HyperLinkStyle.CssClass, 
                    login.PasswordRecoveryUrl, 
                    "Password recovery", 
                    login.PasswordRecoveryText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Writes the password panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WritePasswordPanel(HtmlTextWriter writer, Login login)
        {
            this.Page.ClientScript.RegisterForEventValidation(login.FindControl("Password").UniqueID);
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-PasswordPanel", string.Empty);
            this.Extender.WriteTextBox(
                writer, 
                true, 
                login.LabelStyle.CssClass, 
                login.PasswordLabelText, 
                login.TextBoxStyle.CssClass, 
                "Password", 
                string.Empty);
            WebControlAdapterExtender.WriteRequiredFieldValidator(
                writer, 
                login.FindControl("PasswordRequired") as RequiredFieldValidator, 
                login.ValidatorTextStyle.CssClass, 
                "Password", 
                login.PasswordRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the remember me panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteRememberMePanel(HtmlTextWriter writer, Login login)
        {
            if (login.DisplayRememberMe)
            {
                this.Page.ClientScript.RegisterForEventValidation(login.FindControl("RememberMe").UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-RememberMePanel", string.Empty);
                this.Extender.WriteCheckBox(
                    writer, 
                    login.LabelStyle.CssClass, 
                    login.RememberMeText, 
                    login.CheckBoxStyle.CssClass, 
                    "RememberMe", 
                    login.RememberMeSet);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Writes the submit panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteSubmitPanel(HtmlTextWriter writer, Login login)
        {
            var id = "Login";
            var idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, login.LoginButtonType);
            var btn = login.FindControl(idWithType);
            if (btn == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-SubmitPanel", string.Empty);

            var options = new PostBackOptions(
                btn, string.Empty, string.Empty, false, false, false, true, true, login.UniqueID);
            var javascript = "javascript:" + this.Page.ClientScript.GetPostBackEventReference(options);
            javascript = this.Page.Server.HtmlEncode(javascript);

            this.Extender.WriteSubmit(
                writer, 
                login.LoginButtonType, 
                login.LoginButtonStyle.CssClass, 
                id, 
                login.LoginButtonImageUrl, 
                javascript, 
                login.LoginButtonText);

            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the title panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteTitlePanel(HtmlTextWriter writer, Login login)
        {
            if (!String.IsNullOrEmpty(login.TitleText))
            {
                var className = (!String.IsNullOrEmpty(login.TitleTextStyle.CssClass))
                                    ? login.TitleTextStyle.CssClass + " "
                                    : string.Empty;
                className += "AspNet-Login-TitlePanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
                WebControlAdapterExtender.WriteSpan(writer, string.Empty, login.TitleText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Writes the user panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteUserPanel(HtmlTextWriter writer, Login login)
        {
            this.Page.ClientScript.RegisterForEventValidation(login.FindControl("UserName").UniqueID);
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-UserPanel", string.Empty);
            this.Extender.WriteTextBox(
                writer, 
                false, 
                login.LabelStyle.CssClass, 
                login.UserNameLabelText, 
                login.TextBoxStyle.CssClass, 
                "UserName", 
                login.UserName);
            WebControlAdapterExtender.WriteRequiredFieldValidator(
                writer, 
                login.FindControl("UserNameRequired") as RequiredFieldValidator, 
                login.ValidatorTextStyle.CssClass, 
                "UserName", 
                login.UserNameRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        #endregion
    }
}