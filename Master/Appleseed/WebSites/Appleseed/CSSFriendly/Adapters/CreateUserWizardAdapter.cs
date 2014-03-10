// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateUserWizardAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The create user wizard adapter.
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
    /// The create user wizard adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CreateUserWizardAdapter : WebControlAdapter
    {
        #region Constants and Fields

        /// <summary>
        ///   The current error text.
        /// </summary>
        private string currentErrorText = string.Empty;

        /// <summary>
        ///   The extender.
        /// </summary>
        private WebControlAdapterExtender extender;

        /// <summary>
        ///   The state.
        /// </summary>
        private State state = State.CreateUser;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CreateUserWizardAdapter" /> class. 
        ///   Initializes a new instance of the <see cref = "T:System.Web.UI.WebControls.Adapters.WebControlAdapter" /> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public CreateUserWizardAdapter()
        {
            this.state = State.CreateUser;
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
            ///   The create user.
            /// </summary>
            CreateUser, 

            /// <summary>
            ///   The failed.
            /// </summary>
            Failed, 

            /// <summary>
            ///   The success.
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
        ///   Gets the wizard membership provider.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private MembershipProvider WizardMembershipProvider
        {
            get
            {
                var provider = Membership.Provider;
                var wizard = this.Control as CreateUserWizard;
                if ((wizard != null) && (!String.IsNullOrEmpty(wizard.MembershipProvider)) &&
                    (Membership.Providers[wizard.MembershipProvider] != null))
                {
                    provider = Membership.Providers[wizard.MembershipProvider];
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

            var wizard = this.Control as CreateUserWizard;
            if (wizard != null)
            {
                var activeStep = wizard.ActiveStep as TemplatedWizardStep;
                if (activeStep != null)
                {
                    if ((activeStep.ContentTemplate != null) && (activeStep.Controls.Count == 1))
                    {
                        var container = activeStep.ContentTemplateContainer;
                        if (container != null)
                        {
                            container.Controls.Clear();
                            activeStep.ContentTemplate.InstantiateIn(container);
                            container.DataBind();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when [create user error].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.WebControls.CreateUserErrorEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnCreateUserError(object sender, CreateUserErrorEventArgs e)
        {
            this.state = State.Failed;
            this.currentErrorText = "An error has occurred. Please try again.";

            var wizard = this.Control as CreateUserWizard;
            if (wizard != null)
            {
                this.currentErrorText = wizard.UnknownErrorMessage;
                switch (e.CreateUserError)
                {
                    case MembershipCreateStatus.DuplicateEmail:
                        this.currentErrorText = wizard.DuplicateEmailErrorMessage;
                        break;
                    case MembershipCreateStatus.DuplicateUserName:
                        this.currentErrorText = wizard.DuplicateUserNameErrorMessage;
                        break;
                    case MembershipCreateStatus.InvalidAnswer:
                        this.currentErrorText = wizard.InvalidAnswerErrorMessage;
                        break;
                    case MembershipCreateStatus.InvalidEmail:
                        this.currentErrorText = wizard.InvalidEmailErrorMessage;
                        break;
                    case MembershipCreateStatus.InvalidPassword:
                        this.currentErrorText = wizard.InvalidPasswordErrorMessage;
                        break;
                    case MembershipCreateStatus.InvalidQuestion:
                        this.currentErrorText = wizard.InvalidQuestionErrorMessage;
                        break;
                }
            }
        }

        /// <summary>
        /// Called when [created user].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void OnCreatedUser(object sender, EventArgs e)
        {
            this.state = State.Success;
            this.currentErrorText = string.Empty;
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

            var wizard = this.Control as CreateUserWizard;
            if (this.Extender.AdapterEnabled && (wizard != null))
            {
                RegisterScripts();
                wizard.CreatedUser += this.OnCreatedUser;
                wizard.CreateUserError += this.OnCreateUserError;
                this.state = State.CreateUser;
                this.currentErrorText = string.Empty;
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
                this.Extender.RenderBeginTag(writer, "AspNet-CreateUserWizard");
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
                var wizard = this.Control as CreateUserWizard;
                if (wizard == null)
                {
                    return;
                }

                var activeStep = wizard.ActiveStep as TemplatedWizardStep;
                if (activeStep == null)
                {
                    return;
                }

                if (activeStep.ContentTemplate != null)
                {
                    activeStep.RenderControl(writer);

                    if (wizard.CreateUserStep.Equals(activeStep))
                    {
                        this.WriteCreateUserButtonPanel(writer, wizard);
                    }

                    // Might need to add logic here to render nav buttons for other kinds of
                    // steps (besides the CreateUser step, which we handle above).
                }
                else if (wizard.CreateUserStep.Equals(activeStep))
                {
                    WriteHeaderTextPanel(writer, wizard);
                    WriteStepTitlePanel(writer, wizard);
                    WriteInstructionPanel(writer, wizard);
                    WriteHelpPanel(writer, wizard);
                    this.WriteUserPanel(writer, wizard);
                    this.WritePasswordPanel(writer, wizard);
                    WritePasswordHintPanel(writer, wizard); // hbb
                    this.WriteConfirmPasswordPanel(writer, wizard);
                    this.WriteEmailPanel(writer, wizard);
                    this.WriteQuestionPanel(writer, wizard);
                    this.WriteAnswerPanel(writer, wizard);
                    WriteFinalValidators(writer, wizard);
                    if (this.state == State.Failed)
                    {
                        this.WriteFailurePanel(writer, wizard);
                    }

                    this.WriteCreateUserButtonPanel(writer, wizard);
                }
                else if (wizard.CompleteStep.Equals(activeStep))
                {
                    WriteStepTitlePanel(writer, wizard);
                    WriteSuccessTextPanel(writer, wizard);
                    this.WriteContinuePanel(writer, wizard);
                    WriteEditProfilePanel(writer, wizard);
                }
                else
                {
                    Debug.Fail(
                        "The adapter isn't equipped to handle a CreateUserWizard with a step that is neither templated nor either the CreateUser step or the Complete step.");
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
        /// Writes the edit profile panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteEditProfilePanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (String.IsNullOrEmpty(wizard.EditProfileUrl) && String.IsNullOrEmpty(wizard.EditProfileText))
            {
                return;
            }

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-EditProfilePanel", string.Empty);
            WebControlAdapterExtender.WriteImage(writer, wizard.EditProfileIconUrl, "Edit profile");
            WebControlAdapterExtender.WriteLink(
                writer, wizard.HyperLinkStyle.CssClass, wizard.EditProfileUrl, "EditProfile", wizard.EditProfileText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the final validators.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteFinalValidators(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            WebControlAdapterExtender.WriteBeginDiv(
                writer, "AspNet-CreateUserWizard-FinalValidatorsPanel", string.Empty);
            WebControlAdapterExtender.WriteCompareValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("PasswordCompare") as CompareValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "ConfirmPassword", 
                wizard.ConfirmPasswordCompareErrorMessage, 
                "Password");
            WebControlAdapterExtender.WriteRegularExpressionValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("PasswordRegExpValidator") as
                RegularExpressionValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "Password", 
                wizard.PasswordRegularExpressionErrorMessage, 
                wizard.PasswordRegularExpression);
            WebControlAdapterExtender.WriteRegularExpressionValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("EmailRegExpValidator") as
                RegularExpressionValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "Email", 
                wizard.EmailRegularExpressionErrorMessage, 
                wizard.EmailRegularExpression);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the header text panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteHeaderTextPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (String.IsNullOrEmpty(wizard.HeaderText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(wizard.HeaderStyle.CssClass))
                                ? wizard.HeaderStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-CreateUserWizard-HeaderTextPanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, wizard.HeaderText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the help panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteHelpPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (String.IsNullOrEmpty(wizard.HelpPageIconUrl) && String.IsNullOrEmpty(wizard.HelpPageText))
            {
                return;
            }

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-HelpPanel", string.Empty);
            WebControlAdapterExtender.WriteImage(writer, wizard.HelpPageIconUrl, "Help");
            WebControlAdapterExtender.WriteLink(
                writer, wizard.HyperLinkStyle.CssClass, wizard.HelpPageUrl, "Help", wizard.HelpPageText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the instruction panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteInstructionPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (String.IsNullOrEmpty(wizard.InstructionText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(wizard.InstructionTextStyle.CssClass))
                                ? wizard.InstructionTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-CreateUserWizard-InstructionPanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, wizard.InstructionText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the password hint panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WritePasswordHintPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (String.IsNullOrEmpty(wizard.PasswordHintText))
            {
                return;
            }

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-PasswordHintPanel", string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, wizard.PasswordHintText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the step title panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteStepTitlePanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (wizard.ActiveStep == null)
            {
                return;
            }

            if (String.IsNullOrEmpty(wizard.ActiveStep.Title))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(wizard.TitleTextStyle.CssClass))
                                ? wizard.TitleTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-CreateUserWizard-StepTitlePanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, wizard.ActiveStep.Title);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /////////////////////////////////////////////////////////
        // Complete step
        /////////////////////////////////////////////////////////

        /// <summary>
        /// Writes the success text panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteSuccessTextPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (String.IsNullOrEmpty(wizard.CompleteSuccessText))
            {
                return;
            }

            var className = (!String.IsNullOrEmpty(wizard.CompleteSuccessTextStyle.CssClass))
                                ? wizard.CompleteSuccessTextStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-CreateUserWizard-SuccessTextPanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, wizard.CompleteSuccessText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the answer panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteAnswerPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if ((this.WizardMembershipProvider == null) || !this.WizardMembershipProvider.RequiresQuestionAndAnswer)
            {
                return;
            }

            var textBox = wizard.FindControl("CreateUserStepContainer").FindControl("Answer") as TextBox;
            if (textBox == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-AnswerPanel", string.Empty);
            this.Extender.WriteTextBox(
                writer, 
                false, 
                wizard.LabelStyle.CssClass, 
                wizard.AnswerLabelText, 
                wizard.TextBoxStyle.CssClass, 
                "CreateUserStepContainer_Answer", 
                string.Empty);
            WebControlAdapterExtender.WriteRequiredFieldValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("AnswerRequired") as RequiredFieldValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "Answer", 
                wizard.AnswerRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the confirm password panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteConfirmPasswordPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            var textBox = wizard.FindControl("CreateUserStepContainer").FindControl("ConfirmPassword") as TextBox;
            if (textBox == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

            WebControlAdapterExtender.WriteBeginDiv(
                writer, "AspNet-CreateUserWizard-ConfirmPasswordPanel", string.Empty);
            this.Extender.WriteTextBox(
                writer, 
                true, 
                wizard.LabelStyle.CssClass, 
                wizard.ConfirmPasswordLabelText, 
                wizard.TextBoxStyle.CssClass, 
                "CreateUserStepContainer_ConfirmPassword", 
                string.Empty);
            WebControlAdapterExtender.WriteRequiredFieldValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("ConfirmPasswordRequired") as
                RequiredFieldValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "ConfirmPassword", 
                wizard.ConfirmPasswordRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the continue panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteContinuePanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            const string Id = "ContinueButton";
            var idWithType = WebControlAdapterExtender.MakeIdWithButtonType(Id, wizard.ContinueButtonType);
            var btn = wizard.FindControl("CompleteStepContainer").FindControl(idWithType);
            if (btn == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(btn.UniqueID);
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-ContinuePanel", string.Empty);
            this.Extender.WriteSubmit(
                writer, 
                wizard.ContinueButtonType, 
                wizard.ContinueButtonStyle.CssClass, 
                "CompleteStepContainer_ContinueButton", 
                wizard.ContinueButtonImageUrl, 
                string.Empty, 
                wizard.ContinueButtonText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the create user button panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteCreateUserButtonPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            var btnParentCtrl = wizard.FindControl("__CustomNav0");
            if (btnParentCtrl == null)
            {
                return;
            }

            const string Id = "_CustomNav0_StepNextButton";
            var idWithType = WebControlAdapterExtender.MakeIdWithButtonType(
                "StepNextButton", wizard.CreateUserButtonType);
            var btn = btnParentCtrl.FindControl(idWithType);
            if (btn == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

            var options = new PostBackOptions(
                btn, string.Empty, string.Empty, false, false, false, true, true, wizard.ID);
            var javascript = "javascript:" + this.Page.ClientScript.GetPostBackEventReference(options);
            javascript = this.Page.Server.HtmlEncode(javascript);

            WebControlAdapterExtender.WriteBeginDiv(
                writer, "AspNet-CreateUserWizard-CreateUserButtonPanel", string.Empty);

            this.Extender.WriteSubmit(
                writer, 
                wizard.CreateUserButtonType, 
                wizard.CreateUserButtonStyle.CssClass, 
                Id, 
                wizard.CreateUserButtonImageUrl, 
                javascript, 
                wizard.CreateUserButtonText);

            if (wizard.DisplayCancelButton)
            {
                this.Extender.WriteSubmit(
                    writer, 
                    wizard.CancelButtonType, 
                    wizard.CancelButtonStyle.CssClass, 
                    "_CustomNav0_CancelButton", 
                    wizard.CancelButtonImageUrl, 
                    string.Empty, 
                    wizard.CancelButtonText);
            }

            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the email panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteEmailPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            var textBox = wizard.FindControl("CreateUserStepContainer").FindControl("Email") as TextBox;
            if (textBox == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-EmailPanel", string.Empty);
            this.Extender.WriteTextBox(
                writer, 
                false, 
                wizard.LabelStyle.CssClass, 
                wizard.EmailLabelText, 
                wizard.TextBoxStyle.CssClass, 
                "CreateUserStepContainer_Email", 
                string.Empty);
            WebControlAdapterExtender.WriteRequiredFieldValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("EmailRequired") as RequiredFieldValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "Email", 
                wizard.EmailRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /////////////////////////////////////////////////////////
        // Step 1: Create user step
        /////////////////////////////////////////////////////////

        /// <summary>
        /// Writes the failure panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteFailurePanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            var className = (!String.IsNullOrEmpty(wizard.ErrorMessageStyle.CssClass))
                                ? wizard.ErrorMessageStyle.CssClass + " "
                                : string.Empty;
            className += "AspNet-CreateUserWizard-FailurePanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, string.Empty);
            WebControlAdapterExtender.WriteSpan(writer, string.Empty, this.currentErrorText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the password panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WritePasswordPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            var textBox = wizard.FindControl("CreateUserStepContainer").FindControl("Password") as TextBox;
            if (textBox == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-PasswordPanel", string.Empty);
            this.Extender.WriteTextBox(
                writer, 
                true, 
                wizard.LabelStyle.CssClass, 
                wizard.PasswordLabelText, 
                wizard.TextBoxStyle.CssClass, 
                "CreateUserStepContainer_Password", 
                string.Empty);
            WebControlAdapterExtender.WriteRequiredFieldValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("PasswordRequired") as RequiredFieldValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "Password", 
                wizard.PasswordRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the question panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteQuestionPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if ((this.WizardMembershipProvider == null) || !this.WizardMembershipProvider.RequiresQuestionAndAnswer)
            {
                return;
            }

            var textBox = wizard.FindControl("CreateUserStepContainer").FindControl("Question") as TextBox;
            if (textBox == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-QuestionPanel", string.Empty);
            this.Extender.WriteTextBox(
                writer, 
                false, 
                wizard.LabelStyle.CssClass, 
                wizard.QuestionLabelText, 
                wizard.TextBoxStyle.CssClass, 
                "CreateUserStepContainer_Question", 
                string.Empty);
            WebControlAdapterExtender.WriteRequiredFieldValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("QuestionRequired") as RequiredFieldValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "Question", 
                wizard.QuestionRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        /// <summary>
        /// Writes the user panel.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="wizard">
        /// The wizard.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteUserPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            var textBox = wizard.FindControl("CreateUserStepContainer").FindControl("UserName") as TextBox;
            if (textBox == null)
            {
                return;
            }

            this.Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-UserPanel", string.Empty);
            this.Extender.WriteTextBox(
                writer, 
                false, 
                wizard.LabelStyle.CssClass, 
                wizard.UserNameLabelText, 
                wizard.TextBoxStyle.CssClass, 
                "CreateUserStepContainer_UserName", 
                wizard.UserName);
            WebControlAdapterExtender.WriteRequiredFieldValidator(
                writer, 
                wizard.FindControl("CreateUserStepContainer").FindControl("UserNameRequired") as RequiredFieldValidator, 
                wizard.ValidatorTextStyle.CssClass, 
                "UserName", 
                wizard.UserNameRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        #endregion
    }
}