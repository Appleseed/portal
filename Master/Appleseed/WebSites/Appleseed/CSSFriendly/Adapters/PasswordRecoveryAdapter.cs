// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordRecoveryAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The password recovery adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Diagnostics;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.Adapters;

    /// <summary>
    /// The password recovery adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class PasswordRecoveryAdapter : WebControlAdapter
    {
        #region Constants and Fields

        /// <summary>
        /// The current error text.
        /// </summary>
        private string currentErrorText = string.Empty;

        /// <summary>
        /// The extender.
        /// </summary>
        private WebControlAdapterExtender extender;

        /// <summary>
        /// The state.
        /// </summary>
        private State state = State.UserName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordRecoveryAdapter"/> class. 
        ///   Initializes a new instance of the <see cref="T:System.Web.UI.WebControls.Adapters.WebControlAdapter"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public PasswordRecoveryAdapter()
        {
            this.state = State.UserName;
            this.currentErrorText = string.Empty;
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
            /// The user name.
            /// </summary>
            UserName, 

            /// <summary>
            /// The verifying user.
            /// </summary>
            VerifyingUser, 

            /// <summary>
            /// The user lookup error.
            /// </summary>
            UserLookupError, 

            /// <summary>
            /// The question.
            /// </summary>
            Question, 

            /// <summary>
            /// The verifying answer.
            /// </summary>
            VerifyingAnswer, 

            /// <summary>
            /// The answer lookup error.
            /// </summary>
            AnswerLookupError, 

            /// <summary>
            /// The send mail error.
            /// </summary>
            SendMailError, 

            /// <summary>
            /// The success.
            /// </summary>
            Success
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

        /// <summary>
        ///   Gets the password recovery membership provider.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private MembershipProvider PasswordRecoveryMembershipProvider
        {
            get
            {
                var provider = Membership.Provider;
                var passwordRecovery = this.Control as PasswordRecovery;
                if ((passwordRecovery != null) && (passwordRecovery.MembershipProvider != null) &&
                    (!String.IsNullOrEmpty(passwordRecovery.MembershipProvider)) &&
                    (Membership.Providers[passwordRecovery.MembershipProvider] != null))
                {
                    provider = Membership.Providers[passwordRecovery.MembershipProvider];
                }

                return provider;
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

            var passwordRecovery = this.Control as PasswordRecovery;
            if (passwordRecovery != null)
            {
                if ((passwordRecovery.UserNameTemplate != null) && (passwordRecovery.UserNameTemplateContainer != null))
                {
                    passwordRecovery.UserNameTemplateContainer.Controls.Clear();
                    passwordRecovery.UserNameTemplate.InstantiateIn(passwordRecovery.UserNameTemplateContainer);
                    passwordRecovery.UserNameTemplateContainer.DataBind();
                }

                if ((passwordRecovery.QuestionTemplate != null) && (passwordRecovery.QuestionTemplateContainer != null))
                {
                    passwordRecovery.QuestionTemplateContainer.Controls.Clear();
                    passwordRecovery.QuestionTemplate.InstantiateIn(passwordRecovery.QuestionTemplateContainer);
                    passwordRecovery.QuestionTemplateContainer.DataBind();
                }

                if ((passwordRecovery.SuccessTemplate != null) && (passwordRecovery.SuccessTemplateContainer != null))
                {
                    passwordRecovery.SuccessTemplateContainer.Controls.Clear();
                    passwordRecovery.SuccessTemplate.InstantiateIn(passwordRecovery.SuccessTemplateContainer);
                    passwordRecovery.SuccessTemplateContainer.DataBind();
                }
            }
        }

        /// <summary>
        /// Called when [answer lookup error].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnAnswerLookupError(object sender, EventArgs e)
        {
            this.state = State.AnswerLookupError;
            var passwordRecovery = this.Control as PasswordRecovery;
            if (passwordRecovery != null)
            {
                this.currentErrorText = passwordRecovery.GeneralFailureText;
                if (!String.IsNullOrEmpty(passwordRecovery.QuestionFailureText))
                {
                    this.currentErrorText = passwordRecovery.QuestionFailureText;
                }
            }
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

            var passwordRecovery = this.Control as PasswordRecovery;
            if (this.Extender.AdapterEnabled && (passwordRecovery != null))
            {
                RegisterScripts();
                passwordRecovery.AnswerLookupError += this.OnAnswerLookupError;
                passwordRecovery.SendMailError += this.OnSendMailError;
                passwordRecovery.UserLookupError += this.OnUserLookupError;
                passwordRecovery.VerifyingAnswer += this.OnVerifyingAnswer;
                passwordRecovery.VerifyingUser += this.OnVerifyingUser;
                this.state = State.UserName;
                this.currentErrorText = string.Empty;
            }
        }

        /// <summary>
        /// Overrides the <see cref="M:System.Web.UI.Control.OnPreRender(System.EventArgs)"/> method for the associated control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var passwordRecovery = this.Control as PasswordRecovery;
            if (passwordRecovery != null)
            {
                var provider = passwordRecovery.MembershipProvider;
            }

            // By this time we have finished doing our event processing.  That means that if errors have
            // occurred, the event handlers (OnAnswerLookupError, OnSendMailError or 
            // OnUserLookupError) will have been called.  So, if we were, for example, verifying the
            // user and didn't cause an error then we know we can move on to the next step, getting
            // the answer to the security question... if the membership system demands it.
            switch (this.state)
            {
                case State.AnswerLookupError:

                    // Leave the state alone because we hit an error.
                    break;
                case State.Question:

                    // Leave the state alone. Render a form to get the answer to the security question.
                    this.currentErrorText = string.Empty;
                    break;
                case State.SendMailError:

                    // Leave the state alone because we hit an error.
                    break;
                case State.Success:

                    // Leave the state alone. Render a concluding message.
                    this.currentErrorText = string.Empty;
                    break;
                case State.UserLookupError:

                    // Leave the state alone because we hit an error.
                    break;
                case State.UserName:

                    // Leave the state alone. Render a form to get the user name.
                    this.currentErrorText = string.Empty;
                    break;
                case State.VerifyingAnswer:

                    // Success! We did not encounter an error while verifying the answer to the security question.
                    this.state = State.Success;
                    this.currentErrorText = string.Empty;
                    break;
                case State.VerifyingUser:
                    // We have a valid user. We did not encounter an error while verifying the user.
                    this.state = this.PasswordRecoveryMembershipProvider.RequiresQuestionAndAnswer ? State.Question : State.Success;
                    this.currentErrorText = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// Called when [send mail error].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.WebControls.SendMailErrorEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnSendMailError(object sender, SendMailErrorEventArgs e)
        {
            if (!e.Handled)
            {
                this.state = State.SendMailError;
                this.currentErrorText = e.Exception.Message;
            }
        }

        /// <summary>
        /// Called when [user lookup error].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnUserLookupError(object sender, EventArgs e)
        {
            this.state = State.UserLookupError;
            var passwordRecovery = this.Control as PasswordRecovery;
            if (passwordRecovery != null)
            {
                this.currentErrorText = passwordRecovery.GeneralFailureText;
                if (!String.IsNullOrEmpty(passwordRecovery.UserNameFailureText))
                {
                    this.currentErrorText = passwordRecovery.UserNameFailureText;
                }
            }
        }

        /// <summary>
        /// Called when [verifying answer].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.WebControls.LoginCancelEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnVerifyingAnswer(object sender, LoginCancelEventArgs e)
        {
            this.state = State.VerifyingAnswer;
        }

        /// <summary>
        /// Called when [verifying user].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.WebControls.LoginCancelEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnVerifyingUser(object sender, LoginCancelEventArgs e)
        {
            this.state = State.VerifyingUser;
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
                this.Extender.RenderBeginTag(writer, "AspNet-PasswordRecovery");
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
                var passwordRecovery = this.Control as PasswordRecovery;
                if (passwordRecovery != null)
                {
                    if ((this.state == State.UserName) || (this.state == State.UserLookupError))
                    {
                        if ((passwordRecovery.UserNameTemplate != null) &&
                            (passwordRecovery.UserNameTemplateContainer != null))
                        {
                            foreach (Control c in passwordRecovery.UserNameTemplateContainer.Controls)
                            {
                                c.RenderControl(writer);
                            }
                        }
                        else
                        {
                            this.WriteTitlePanel(writer, passwordRecovery);
                            this.WriteInstructionPanel(writer, passwordRecovery);
                            WriteHelpPanel(writer, passwordRecovery);
                            this.WriteUserPanel(writer, passwordRecovery);
                            if (this.state == State.UserLookupError)
                            {
                                this.WriteFailurePanel(writer, passwordRecovery);
                            }

                            this.WriteSubmitPanel(writer, passwordRecovery);
                        }
                    }
                    else if ((this.state == State.Question) || (this.state == State.AnswerLookupError))
                    {
                        if ((passwordRecovery.QuestionTemplate != null) &&
                            (passwordRecovery.QuestionTemplateContainer != null))
                        {
                            foreach (Control c in passwordRecovery.QuestionTemplateContainer.Controls)
                            {
                                c.RenderControl(writer);
                            }
                        }
                        else
                        {
                            this.WriteTitlePanel(writer, passwordRecovery);
                            this.WriteInstructionPanel(writer, passwordRecovery);
                            WriteHelpPanel(writer, passwordRecovery);
                            this.WriteUserPanel(writer, passwordRecovery);
                            this.WriteQuestionPanel(writer, passwordRecovery);
                            this.WriteAnswerPanel(writer, passwordRecovery);
                            if (this.state == State.AnswerLookupError)
                            {
                                this.WriteFailurePanel(writer, passwordRecovery);
                            }

                            this.WriteSubmitPanel(writer, passwordRecovery);
                        }
                    }
                    else if (this.state == State.SendMailError)
                    {
                        this.WriteFailurePanel(writer, passwordRecovery);
                    }
                    else if (this.state == State.Success)
                    {
                        if ((passwordRecovery.SuccessTemplate != null) &&
                            (passwordRecovery.SuccessTemplateContainer != null))
                        {
                            foreach (Control c in passwordRecovery.SuccessTemplateContainer.Controls)
                            {
                                c.RenderControl(writer);
                            }
                        }
                        else
                        {
                            WriteSuccessTextPanel(writer, passwordRecovery);
                        }
                    }
                    else
                    {
                        // We should never get here.
                        Debug.Fail(
                            "The PasswordRecovery control adapter was asked to render a state that it does not expect.");
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
        /// Writes the answer panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteAnswerPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            var container = passwordRecovery.QuestionTemplateContainer;
            var textBox = (container != null) ? container.FindControl("Answer") as TextBox : null;
            var rfv = (textBox != null) ? container.FindControl("AnswerRequired") as RequiredFieldValidator : null;
            var id = (rfv != null) ? container.ID + "_" + textBox.ID : string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                if (textBox != null)
                {
                    this.Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);
                }

                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-AnswerPanel", string.Empty);
                this.Extender.WriteTextBox(
                    writer, 
                    false, 
                    passwordRecovery.LabelStyle.CssClass, 
                    passwordRecovery.AnswerLabelText, 
                    passwordRecovery.TextBoxStyle.CssClass, 
                    id, 
                    string.Empty);
                WebControlAdapterExtender.WriteRequiredFieldValidator(
                    writer, 
                    rfv, 
                    passwordRecovery.ValidatorTextStyle.CssClass, 
                    "Answer", 
                    passwordRecovery.AnswerRequiredErrorMessage);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /////////////////////////////////////////////////////////
        // Step 1: user name
        /////////////////////////////////////////////////////////

        /// <summary>
        /// Writes the failure panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteFailurePanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if (String.IsNullOrEmpty(this.currentErrorText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(passwordRecovery.FailureTextStyle.CssClass))
                                ? passwordRecovery.FailureTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-PasswordRecovery-FailurePanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, this.currentErrorText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the help panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteHelpPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((!String.IsNullOrEmpty(passwordRecovery.HelpPageIconUrl)) ||
                (!String.IsNullOrEmpty(passwordRecovery.HelpPageText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-HelpPanel", string.Empty);
                WebControlAdapterExtender.WriteImage(writer, passwordRecovery.HelpPageIconUrl, "Help");
                WebControlAdapterExtender.WriteLink(
                    writer, 
                    passwordRecovery.HyperLinkStyle.CssClass, 
                    passwordRecovery.HelpPageUrl, 
                    "Help", 
                    passwordRecovery.HelpPageText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /// <summary>
        /// Writes the instruction panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteInstructionPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((this.state == State.UserName) || (this.state == State.UserLookupError))
            {
                if (!String.IsNullOrEmpty(passwordRecovery.UserNameInstructionText))
                {
                    var className = (!String.IsNullOrEmpty(passwordRecovery.InstructionTextStyle.CssClass))
                                        ? passwordRecovery.InstructionTextStyle.CssClass + " "
                                        : string.Empty;
                    className += "AspNet-PasswordRecovery-UserName-InstructionPanel";
                    WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
                    WebControlAdapterExtender.WriteSpan(writer, string.Empty, passwordRecovery.UserNameInstructionText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
            else if ((this.state == State.Question) || (this.state == State.AnswerLookupError))
            {
                if (!String.IsNullOrEmpty(passwordRecovery.QuestionInstructionText))
                {
                    var className = (!String.IsNullOrEmpty(passwordRecovery.InstructionTextStyle.CssClass))
                                        ? passwordRecovery.InstructionTextStyle.CssClass + " "
                                        : string.Empty;
                    className += "AspNet-PasswordRecovery-Question-InstructionPanel";
                    WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
                    WebControlAdapterExtender.WriteSpan(writer, string.Empty, passwordRecovery.QuestionInstructionText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        /////////////////////////////////////////////////////////
        // Step 2: question
        /////////////////////////////////////////////////////////

        /// <summary>
        /// Writes the question panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteQuestionPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-QuestionPanel", string.Empty);
            this.Extender.WriteReadOnlyTextBox(
                writer, 
                passwordRecovery.LabelStyle.CssClass, 
                passwordRecovery.QuestionLabelText, 
                passwordRecovery.TextBoxStyle.CssClass, 
                passwordRecovery.Question);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the submit panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteSubmitPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((this.state == State.UserName) || (this.state == State.UserLookupError))
            {
                var container = passwordRecovery.UserNameTemplateContainer;
                var id = (container != null) ? container.ID + "_Submit" : "Submit";

                var idWithType = WebControlAdapterExtender.MakeIdWithButtonType(
                    "Submit", passwordRecovery.SubmitButtonType);
                var btn = (container != null) ? container.FindControl(idWithType) : null;

                if (btn != null)
                {
                    this.Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

                    var options = new PostBackOptions(
                        btn, string.Empty, string.Empty, false, false, false, true, true, passwordRecovery.UniqueID);
                    var javascript = "javascript:" + this.Page.ClientScript.GetPostBackEventReference(options);
                    javascript = this.Page.Server.HtmlEncode(javascript);

                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-UserName-SubmitPanel", string.Empty);
                    this.Extender.WriteSubmit(
                        writer, 
                        passwordRecovery.SubmitButtonType, 
                        passwordRecovery.SubmitButtonStyle.CssClass, 
                        id, 
                        passwordRecovery.SubmitButtonImageUrl, 
                        javascript, 
                        passwordRecovery.SubmitButtonText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
            else if ((this.state == State.Question) || (this.state == State.AnswerLookupError))
            {
                var container = passwordRecovery.QuestionTemplateContainer;
                var id = (container != null) ? container.ID + "_Submit" : "Submit";
                var idWithType = WebControlAdapterExtender.MakeIdWithButtonType(
                    "Submit", passwordRecovery.SubmitButtonType);
                var btn = (container != null) ? container.FindControl(idWithType) : null;

                if (btn != null)
                {
                    this.Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

                    var options = new PostBackOptions(
                        btn, string.Empty, string.Empty, false, false, false, true, true, passwordRecovery.UniqueID);
                    var javascript = "javascript:" + this.Page.ClientScript.GetPostBackEventReference(options);
                    javascript = this.Page.Server.HtmlEncode(javascript);

                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-Question-SubmitPanel", string.Empty);
                    this.Extender.WriteSubmit(
                        writer, 
                        passwordRecovery.SubmitButtonType, 
                        passwordRecovery.SubmitButtonStyle.CssClass, 
                        id, 
                        passwordRecovery.SubmitButtonImageUrl, 
                        javascript, 
                        passwordRecovery.SubmitButtonText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        /////////////////////////////////////////////////////////
        // Step 3: success
        /////////////////////////////////////////////////////////

        /// <summary>
        /// Writes the success text panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteSuccessTextPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if (String.IsNullOrEmpty(passwordRecovery.SuccessText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(passwordRecovery.SuccessTextStyle.CssClass))
                                ? passwordRecovery.SuccessTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-PasswordRecovery-SuccessTextPanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, passwordRecovery.SuccessText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the title panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteTitlePanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((this.state == State.UserName) || (this.state == State.UserLookupError))
            {
                if (!String.IsNullOrEmpty(passwordRecovery.UserNameTitleText))
                {
                    var className = (!String.IsNullOrEmpty(passwordRecovery.TitleTextStyle.CssClass))
                                        ? passwordRecovery.TitleTextStyle.CssClass + " "
                                        : string.Empty;
                    className += "AspNet-PasswordRecovery-UserName-TitlePanel";
                    WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
                    WebControlAdapterExtender.WriteSpan(writer, string.Empty, passwordRecovery.UserNameTitleText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
            else if ((this.state == State.Question) || (this.state == State.AnswerLookupError))
            {
                if (!String.IsNullOrEmpty(passwordRecovery.QuestionTitleText))
                {
                    var className = (!String.IsNullOrEmpty(passwordRecovery.TitleTextStyle.CssClass))
                                        ? passwordRecovery.TitleTextStyle.CssClass + " "
                                        : string.Empty;
                    className += "AspNet-PasswordRecovery-Question-TitlePanel";
                    WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
                    WebControlAdapterExtender.WriteSpan(writer, string.Empty, passwordRecovery.QuestionTitleText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        /// <summary>
        /// Writes the user panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="passwordRecovery">
        /// The password recovery.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteUserPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((this.state == State.UserName) || (this.state == State.UserLookupError))
            {
                var container = passwordRecovery.UserNameTemplateContainer;
                var textBox = (container != null) ? container.FindControl("UserName") as TextBox : null;
                var rfv = (textBox != null) ? container.FindControl("UserNameRequired") as RequiredFieldValidator : null;
                var id = (rfv != null) ? container.ID + "_" + textBox.ID : string.Empty;
                if (!String.IsNullOrEmpty(id))
                {
                    if (textBox != null)
                    {
                        this.Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);
                    }

                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-UserName-UserPanel", string.Empty);
                    this.Extender.WriteTextBox(
                        writer, 
                        false, 
                        passwordRecovery.LabelStyle.CssClass, 
                        passwordRecovery.UserNameLabelText, 
                        passwordRecovery.TextBoxStyle.CssClass, 
                        id, 
                        passwordRecovery.UserName);
                    WebControlAdapterExtender.WriteRequiredFieldValidator(
                        writer, 
                        rfv, 
                        passwordRecovery.ValidatorTextStyle.CssClass, 
                        "UserName", 
                        passwordRecovery.UserNameRequiredErrorMessage);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
            else if ((this.state == State.Question) || (this.state == State.AnswerLookupError))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-Question-UserPanel", string.Empty);
                this.Extender.WriteReadOnlyTextBox(
                    writer, 
                    passwordRecovery.LabelStyle.CssClass, 
                    passwordRecovery.UserNameLabelText, 
                    passwordRecovery.TextBoxStyle.CssClass, 
                    passwordRecovery.UserName);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        #endregion
    }
}