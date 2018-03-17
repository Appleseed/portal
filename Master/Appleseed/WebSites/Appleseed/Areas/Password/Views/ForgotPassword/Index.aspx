<%@ Page Language="C#" Inherits="Appleseed.Framework.Web.UI.Page" MasterPageFile="~/Shared/SiteMasterDefault.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <style>
        /* SignIn Cool Styling */
        .signin-page {
            width: 300px;
            margin: 0 auto;
            font-family: "Lato", "Helvetica Neue", Helvetica, Arial, sans-serif;
            font-size: 15px;
        }

        .signin-page .input-row {
            margin-bottom: 10px;
        }

        .signin-page input[type="text"],
        .signin-page input[type="password"] {
            width: 300px;
            height: 40px;
            font-size: 15px;
            border: 1px solid #cccccc !important;
            box-shadow: none;
        }

        .signin-page input[type="checkbox"] {
            margin: 0px 5px 0px 10px;
            vertical-align: -2px;
        }

            .signin-page input[type="checkbox"] + label {
                font-size: 15px;
            }

        .signin-page .forgot-link {
            font-size: 15px;
            text-decoration: none;
        }

            .signin-page .forgot-link:hover {
                color: #6696ff;
            }

        .signin-page .login-button {
            width: 300px;
            height: 44px;
            color: #fff;
            cursor: pointer;
            background: #1a242f;
            font-size: 15px !important;
        }

        .desktopmodules_coremodules_signin_signincool_ascx .module_Body {
            width: 1000px;
        }

        @media screen and (max-width: 768px) {
            .desktopmodules_coremodules_signin_signincool_ascx .module_Body {
                width: auto;
            }

            table {
                border-collapse: separate;
                border-spacing: 0;
                margin-left: auto;
                margin-right: auto;
            }
        }

        .Error {
            color: red !important;
            font-size: 20px;
        }

        #formSignIn hr {
            display: none !important;
        }
    </style>
    <div>
        <div id="Fields" class="signin-page">
            <%  string email = string.Empty;
            if (Request.QueryString["email"] != null) {
                email = Request.QueryString["email"];
           } %>
            <div class="input-row">
                <label id="EmailLabel"><%= Appleseed.Framework.General.GetString("EMAIL", "E-mail") %> </label>
                <br />
                <input type="text" id="UsersEmail" class="" value="<%= email %>" />
            </div>
            <div class="input-row">
                <input type="button" id="SendPasswordBtn" class="CommandButton" value="<%= Appleseed.Framework.General.GetString("SendEmail", "Send Password Recovery E-mail") %>" onclick="sendPasswordToken()" />
            </div>
            <label id="Message" class="Error"></label>
            <br />
        </div>
        <div id="divSuccess" style="color: green; font-size: 18px"></div>

    </div>


    <script type="text/javascript">

        function sendPasswordToken() {

            var email = $('#UsersEmail').val();

            if (email != '' && validateEmail(email)) {

                $.ajax({
                    url: '<%= Url.Action("sendPasswordToken")%>',
                    type: 'POST',
                    data: {
                        "email": email
                    },
                    success: function (data) {
                        $('#Message').text(data.Message);
                        if (data.ok) {
                            $('#Fields').hide();
                            $('#divSuccess').html('Success! We have sent you an e-mail to reset your password, please check your inbox or SPAM folder.');
                        }
                    },
                    error: function (data) {
                        $('#Message').text(data.Message);

                    }
                });
            }
            else {
                $('#Message').text('<%= Appleseed.Framework.General.GetString("InvalidEmail", "The e-mail address you entered could not be found. Make sure you are using the e-mail address associated with your user.") %>');
            }
        }

        function validateEmail(email) {
            var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (re.test(email)) {
                console.log('Email true');
                return true;
            }
            else {
                console.log('Email false');
                return false;
            }

        }

    </script>


</asp:Content>
