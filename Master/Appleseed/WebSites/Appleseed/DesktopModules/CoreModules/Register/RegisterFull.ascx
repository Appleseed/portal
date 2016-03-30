<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegisterFull.ascx.cs" Inherits="DesktopModules_CoreModules_Register_RegisterFull" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>

<div class="div_Password_Change">
    <div runat="server" id="NotifySaveContainer" class="ui-widget" adv="adv">
        <div style="padding: 0pt 0.7em; height: 30px;" class="ui-state-highlight ui-corner-all" id="NotifySave" >
        
            <p style="font-size:12pt; float: left">
                <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-info"></span>
            
                <asp:Label runat="server" ID="messageS_lbl"></asp:Label>
            </p>
            <a class="ui-dialog-titlebar-close ui-corner-all" href="javascript: void(0);" onclick="hide();" unselectable="on" role="button" style="float: right; padding-left:auto; margin-top: 0.5em">
                <span class="ui-icon ui-icon-closethick">close</span>
            </a>
        </div>
    
    
    </div>
</div>





<table border="0" cellpadding="0" cellspacing="0" class="registerTable">
        <tr>
        <td align="left" valign="top" width="30">
            &nbsp;
        </td>
        <td align="left" valign="top" width="661">
            <table border="0" cellpadding="0" cellspacing="0" width="661">
                <tr>
                    <td align="left" valign="top" width="10">
                        &nbsp;
                    </td>
                    <td align="left" class="textogrisinformacion" valign="top">
                        <p>
                            <span class="titulosofertas">
                                <asp:Label ID="lblTitle" runat="server"></asp:Label></span>
                        </p>
                        <asp:Panel ID="pnlForm" runat="server">
                            <table cellpadding="0" cellspacing="0" class="registerForm" style="padding-left: 30px; width:100%">
                                <tbody>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr class="registerRow">
                                        <td width="30%">
                                            <asp:Label ID="lblName" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, NAME%>" textkey="NAME"></asp:Label>
                                        </td>
                                        <td width="69%">
                                            <asp:TextBox ID="tfName" runat="server"
                                                ValidationGroup="USER"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredName" runat="server" ControlToValidate="tfName"
                                                        Display="Dynamic" ErrorMessage="<%$ Resources:Appleseed, MUST_ENTER_NAME%>" Text="You must enter a name" 
                                                        textkey="MUST_ENTER_NAME" Font-Size="11px"
                                                        ValidationGroup="USER"/>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEmail" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, MAIL%>" textkey="MAIL"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tfEmail" runat="server"
                                                ValidationGroup="USER"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tfEmail"
                                                  Display="Dynamic" ErrorMessage="MUST_ENTER_MAIL" Text="<%$ Resources:Appleseed, MUST_ENTER_MAIL%>" textkey="MUST_ENTER_MAIL" 
                                                  Font-Size="11px"
                                                  ValidationGroup="USER" />                 
                                        </td>
                                    </tr>

                                    <tr runat="server" id="trPwd">
                                        <td>
                                            <asp:Label ID="lblPwd" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, PASSWORD %>" 
                                                textkey="PASSWORD"></asp:Label>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="tfPwd" runat="server" TextMode="Password" 
                                                    ValidationGroup="USER"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPwd" runat="server" ControlToValidate="tfPwd" Display="Dynamic"
                                                    Text="<%$ Resources:Appleseed, MUST_ENTER_PASSWORD%>"  textkey="MUST_ENTER_PASSWORD" Font-Size="11px"
                                                    ValidationGroup="USER"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trPwdAgain">
                                        <td>
                                            <asp:Label ID="lblPwdAgain" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, PASSWORD_AGAIN %>" 
                                                textkey="PASSWORD_AGAIN"></asp:Label>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="tfPwdAgain" runat="server" TextMode="Password"
                                                    ValidationGroup="USER"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPwdAgain" runat="server" ControlToValidate="tfPwdAgain" Display="Dynamic"
                                                    Text="<%$ Resources:Appleseed, MUST_ENTER_PASSWORD%>"  textkey="MUST_ENTER_PASSWORD" Font-Size="11px"
                                                    ValidationGroup="USER"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cfvPwd" runat="server" 
                                                    ControlToValidate="tfPwdAgain" ControlToCompare="tfPwd" 
                                                    Type="String" Operator="Equal" Display="Dynamic"
                                                    ValidationGroup="USER"
                                                    Text="<%$ Resources:Appleseed, PASSWORD_NOT_MATCH%>" textkey="PASSWORD_NOT_MATCH" Font-Size="11px"></asp:CompareValidator>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBirthDate" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, DATE_OF_BIRTH%>" 
                                                        textkey="DATE_OF_BIRTH"></asp:Label>
                                        </td>
										<td>
												<div id="content">
													<asp:TextBox id="startdate" class="field" runat="server"></asp:TextBox>
												</div>
												<asp:RangeValidator ID="cbirthday" runat="server" ControlToValidate="startdate" Type="Date"
                                                  Display="Dynamic" ErrorMessage="INVALID_DATE" Text="<%$ Resources:Appleseed, INVALID_DATE%>" textkey="INVALID_DATE" 
                                                  Font-Size="11px"
                                                  ValidationGroup="USER" /> 
											
										</td>
                                    </tr>
   <script type="text/javascript">
      <%string data = (String) ViewState["cultureInfo"];%>
        $(document).ready(function () {
			$.datepicker.setDefaults($.datepicker.regional['<%= data %>']); 
			$("#<%= startdate.ClientID %>").datepicker (
		    {
		        yearRange: '1900:2020',
		        maxDate: 'today',
		        changeMonth: true,
		        changeYear: true,
		    });
        });    
</script>

                                 <tr>
                                        <td>
                                            <asp:Label ID="lblCompany" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, COMPANY%>" textkey="COMPANY"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tfCompany" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPhone" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, TELEPHONE%>" 
                                            textkey="TELEPHONE"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tfPhone" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCountry" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, COUNTRY%>" 
                                            textkey="COUNTRY"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList Width="70%" ID="ddlCountry" runat="server" 
                                                ValidationGroup="USER"
                                                DataTextField="Name"
                                                DataValueField="CountryID">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddlCountry" Display="Dynamic"
                                                ValidationGroup="USER"
                                                Text="<%$ Resources:Appleseed, MUST_SELECT_COUNTRY%>" textkey="MUST_SELECT_COUNTRY" Font-Size="11px"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblReceiveNews" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, RECEIVE_NEWS%>" 
                                            textkey="RECEIVE_NEWS"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbReceiveNews" runat="server" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSendNotification" runat="server" CssClass="textogrisinformacion"
                                                Text="<%$ Resources:Appleseed, SEND_NOTIFICATION%>" textkey="SEND_NOTIFICATION" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbSendNotification" runat="server" Checked="true" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr id="trCaptcha" runat="server">
                                        <td colspan="2">
                                            <recaptcha:RecaptchaControl
                                                  ID="recaptcha"
                                                  runat="server"
                                                  Theme="clean"
                                                  PrivateKey="some_private_key"
                                                  PublicKey="some_public_key"
                                                  />
                                            <div>
                                                <asp:CustomValidator ID="cvCaptcha" runat="server" Display="Dynamic"
                                                    Text="<%$ Resources:Appleseed, USER_SAVING_WRONG_CAPTCHA %>" textkey="USER_SAVING_WRONG_CAPTCHA" Font-Size="11px"
                                                    ValidationGroup="USER"
                                                    OnServerValidate="cvCaptcha_ServerValidate" ></asp:CustomValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>
                                            <asp:Label ID="lblAssignCategory" runat="server" CssClass="textogrisinformacion"
                                                Text="Asignar a la categor&iacute;a:" Visible="false"></asp:Label>
                                        </td>
                                        <td width="200px">
                                            <asp:DropDownList ID="ddlAssignCategory" Visible="false" runat="server" Width="95%" />
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td style="padding-top: 10px;" valign="top">
                                            <asp:Button runat="server" ID="ChangePassword" Text="<%$ Resources:Appleseed, CHANGE_PWD%>"></asp:Button>
                                        </td>
                                        <td style="padding-top: 10px" valign="top">
                                            <asp:Button ID="btnSave"  runat="server" OnClick="btnSave_Click" 
                                                Text="<%$ Resources:Appleseed, CONFIRM %>" textkey="CONFIRM"
                                                ValidationGroup="USER" style="margin-left: 195px"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCategoryId" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlSuceeded" runat="server">
                            <asp:Label ID="lblSuceeded" runat="server"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<asp:Panel runat='server' ID="panChangePwd" Visible="false">
    <div id="changePwdDialog" style="display:none">

        <table border="0" cellpadding="0" cellspacing="0" width="350px" >
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" class="registerForm">
                        <tr runat="server" id="trPwdMessage">
                            <td>
                                <asp:Label ID="lblCurrPwd" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, CHANGE_PWD_CURRENT_PWD %>" 
                                    textkey="CHANGE_PWD_CURRENT_PWD"></asp:Label>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtCurrentPwd" runat="server" TextMode="Password" 
                                        ValidationGroup="CHANGE_PWD"></asp:TextBox>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvCurrentPwd" runat="server" ControlToValidate="txtCurrentPwd" Display="Dynamic"
                                        Text="<%$ Resources:Appleseed, MUST_ENTER_PASSWORD%>"  textkey="MUST_ENTER_PASSWORD" Font-Size="11px"
                                        ValidationGroup="CHANGE_PWD"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvCurrentPwdCorrect" runat="server" Display="Dynamic"
                                        Text="<%$ Resources:Appleseed, CHANGE_PWD_WRONG_PWD %>" textkey="CHANGE_PWD_WRONG_PWD" Font-Size="11px"
                                        ValidationGroup="CHANGE_PWD"
                                        OnServerValidate="cvCurrentPwdCorrect_ServerValidate"></asp:CustomValidator>
                                </div>
                            </td> 
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNewPwd" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, CHANGE_PWD_ENTER_NEW_PWD %>" 
                                    textkey="CHANGE_PWD_ENTER_NEW_PWD"></asp:Label>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password"
                                        ValidationGroup="CHANGE_PWD"></asp:TextBox>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvNewPwd" runat="server" ControlToValidate="txtNewPwd" Display="Dynamic"
                                        Text="<%$ Resources:Appleseed, MUST_ENTER_PASSWORD%>"  textkey="MUST_ENTER_PASSWORD" Font-Size="11px"
                                        ValidationGroup="CHANGE_PWD"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNewPwdAgain" runat="server" CssClass="textogrisinformacion" Text="<%$ Resources:Appleseed, CHANGE_PWD_ENTER_NEW_PWD_AGAIN %>" 
                                    textkey="CHANGE_PWD_ENTER_NEW_PWD_AGAIN"></asp:Label>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtNewPwdAgain" runat="server" TextMode="Password"
                                        ValidationGroup="CHANGE_PWD"></asp:TextBox>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvNewPwdAgain" runat="server" ControlToValidate="txtNewPwdAgain" Display="Dynamic"
                                        Text="<%$ Resources:Appleseed, MUST_ENTER_PASSWORD%>"  textkey="MUST_ENTER_PASSWORD" Font-Size="11px"
                                        ValidationGroup="CHANGE_PWD"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cfvNewPwdAgain" runat="server" 
                                        ControlToValidate="txtNewPwdAgain" ControlToCompare="txtNewPwd" 
                                        Type="String" Operator="Equal" Display="Dynamic"
                                        ValidationGroup="CHANGE_PWD" 
                                        Text="<%$ Resources:Appleseed, PASSWORD_NOT_MATCH%>" textkey="PASSWORD_NOT_MATCH" Font-Size="11px"></asp:CompareValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 10px;">
                                <asp:Button ID="btnChangePwd" runat="server" OnClick="btnChangePwd_Click" Text="<%$ Resources:Appleseed, CONFIRM_PASSWORD%>" textkey="CONFIRM" ValidationGroup="CHANGE_PWD"/>
                                <asp:Button ID="btnChangePwdCancel" runat="server" Text="<%$ Resources:Appleseed, CANCEL%>" textkey="CANCEL" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
                                    
        </table>

    </div>
        

    <script type="text/javascript">
        
        $(document).ready(function () {

           
            var $pwdDialog = $('#changePwdDialog').dialog({
                autoOpen: false,
                modal: true,
                width: 400,
                open: function (type, data) { $(this).parent().appendTo('form'); }
            });

            $("#<%= ChangePassword.ClientID %>").click(function (e) {
                e.preventDefault();
                $pwdDialog.dialog('open');
                return false;
            });

            $("#<%= btnChangePwdCancel.ClientID %>").click(function (e) {
                e.preventDefault();
                $pwdDialog.dialog('close');
                return false;
            });

            <% if (ViewState["responseWithPopup"] != null)
               { %>
                    $pwdDialog.dialog('open');
            <% } %>
			
        });

        function hide(){
            $(".div_Password_Change").attr('style',"display: none;");
            return false;
        }
 
    </script>
	
   

</asp:Panel>

