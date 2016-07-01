<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Appleseed.Installer.Default" %>

<html>
<head>
    <title>Appleseed.Portal Web Installer</title>
    <link rel="stylesheet" href="style/default.css" type="text/css" />
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.5.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
   
</head>
<body>
    <form runat="server">
    <table style="height: 100%; width: 100%" align="center">
        <tr>
            <td align="center" valign="middle">
                <table cellspacing="0" cellpadding="0" border="0" style="background-color: #ffffff;
                    border: solid 1px #999999; height: 525; width: 100%">
                    <tbody>
                        <tr>
                            <td colspan="2" valign="top" height="75" background="images/installer_top_bg.png"
                                style="background-repeat: repeat-x; color: #fff; font-size: 20px; padding: 10px;">
                                Appleseed.Portal Web Installer
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 200px">
                                <div align="left" style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px;
                                    padding-top: 10px;">
                                    <div class="<%=StepClass(WizardPanel.PreInstall)%>">
                                        Requirements Check</div>
                                    <div class="<%=StepClass(WizardPanel.License)%>">
                                        License</div>
                                    <div class="<%=StepClass(WizardPanel.ConnectToDb)%>">
                                        DB Login</div>
                                    <div class="<%=StepClass(WizardPanel.SelectDb)%>">
                                        Choose Database</div>
                                    <div class="<%=StepClass(WizardPanel.SiteInformation)%>">
                                        Site Information</div>
                                    <div class="<%=StepClass(WizardPanel.Install)%>">
                                        Install Appleseed</div>
                                    <div class="<%=StepClass(WizardPanel.Done)%>">
                                        Completed</div>
                                </div>
                            </td>
                            <td valign="top">
                                <div style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px; padding-top: 0px">
                                    <asp:Panel ID="PreInstall" runat="server" Visible="false">
                                        <div class="mainTitle">
                                            Pre-Installation Requirements Check</div>
                                        <div class="wizardsectionheader">
                                            <strong>Requirements:</strong></div>
                                        <div class="wizardsection">
                                            <ul>
                                                <li style="margin-bottom: 8px;">
                                                    <div class="bold">
                                                        Microsoft .NET Framework</div>
                                                    <div>
                                                        Version 4.0 of the .NET framework must be installed</div>
                                                    <div>
                                                        <a href="http://www.asp.net/Default.aspx?tabindex=0&tabid=1" target="_blank">Get help
                                                            installing the .NET framework</a></div>
                                                </li>
                                                <li style="margin-bottom: 8px;">
                                                    <div class="bold">
                                                        Internet Information Server</div>
                                                    <div>
                                                        IIS or compatible web server software must be installed</div>
                                                    <div>
                                                        <a href="http://www.google.com/search?q=install+iis" target="_blank">Get help installing
                                                            IIS</a></div>
                                                </li>
                                                <li style="margin-bottom: 8px;">
                                                    <div class="bold">
                                                        Microsoft SQL Server</div>
                                                    <div>
                                                        You must have either SQL Server 2005, SQL Server 2008, or SQL Server Express 2008
                                                        installed.</div>
                                                    <div>
                                                        <a href="http://www.microsoft.com/sqlserver/" target="_blank">Get information on SQL
                                                            Server here</a></div>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="wizardsectionheader">
                                            <strong>Environment Check (for user <%=  System.Environment.UserDomainName + @"\" + System.Environment.UserName %>):</strong></div>
                                        <div class="wizardsection">
                                            <ul>
                                                <li style="margin-bottom: 8px;">
                                                    <div class="bold">
                                                        Microsoft .NET Version : <asp:Literal EnableViewState="False" ID="lblAspNetVersion"
                                                            runat="server" /></div>
                                                </li>
                                                <li style="margin-bottom: 8px;">
                                                    <div class="bold">
                                                        Web.Config : <asp:Literal EnableViewState="False" ID="lblWebConfigWritable" runat="server" /></div>
                                                </li>
                                                <li style="margin-bottom: 8px;">
                                                    <div class="bold">
                                                        RB Logs Directory : <asp:Literal EnableViewState="False" ID="lblLogsDirWritable"
                                                            runat="server" /></div>
                                                </li>
                                                <li style="margin-bottom: 8px;">
                                                    <div class="bold">
                                                        Portals Directory : <asp:Literal EnableViewState="False" ID="lblPortalsDirWritable"
                                                            runat="server" /></div>
                                                </li>
                                            </ul>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="License" runat="server" Visible="false">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <span class="mainTitle">Appleseed License</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="right" colspan="2">
                                                    <iframe frameborder="1" scrolling="auto" src="LICENSE-2.0.html" height="300" width="680">
                                                    </iframe>
                                                </td>
                                            </tr>
                                        </table>
                                        <br>
                                        <div align="right" style="padding-right: 50px;">
                                            <asp:CheckBox ID="chkIAgree" runat="server" Text=" I Agree" Checked="false" />
                                            <br />
                                            <asp:Literal ID="errIAgree" runat="server" Visible="false"><span class="err" style="color:Red">You must agree to continue.</span></asp:Literal>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="ConnectToDb" runat="server" Visible="false">
                                        <div class="mainTitle">
                                            Appleseed Database Login</div>
                                        <div class="wizardsection">
                                            <div>
                                                Enter the database login that Appleseed will use to connect to the database.</div>
                                            <div style="color: red;">
                                                <asp:Literal EnableViewState="False" ID="lblErrMsgConnect" runat="server" /></div>
                                            <div style="padding-left: 20px; padding-top: 20px">
                                                IP address or Server Name:
                                                <asp:TextBox CssClass="dataentry" ID="db_server" runat="server" value=".\SQLExpress"></asp:TextBox><br />
                                                <asp:RadioButtonList ID="db_Connect" runat="server" SelectedIndexChanged="ConnectToDb_CheckChanged">
                                                    <asp:ListItem Value="Windows Authentication" Selected="True">Windows Authentication</asp:ListItem>
                                                    <asp:ListItem Value="SQL Server Authentication" >SQL Server Authentication</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <div style="padding-left: 20px; padding-top: 20px">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                Username:
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox CssClass="dataentry" ID="db_login" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Password:
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox CssClass="dataentry" ID="db_password" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="SelectDb" runat="server" Visible="false">
                                        <div class="mainTitle">
                                            Select Database Instance</div>
                                        <div class="wizardsection">
                                            <div>
                                                Choose the database where you would like to to install Appleseed Portal.</div>
                                            <div style="padding-left: 20px; padding-top: 20px">
                                                <div style="padding-left: 20px; padding-top: 10px; color: red">
                                                    <asp:Literal EnableViewState="False" ID="lblErrMsg" runat="server" /></div>
                                                <div style="padding-left: 20px; padding-top: 10px">
                                                    Available databases:
                                                    <asp:DropDownList ID="db_name_list" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="SiteInformation" runat="server" Visible="false">
                                        <div class="mainTitle">
                                            Enter Site Information</div>
                                        <div class="wizardsectionheader">
                                            Enter the following information to configure your site properly.
                                        </div>
                                        <div class="wizardsection">
                                            <table cellpadding="2" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        Site Title Prefix:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox CssClass="dataentry" ID="rb_portalprefix" runat="server">My Site - </asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="req_rb_portalprefix" runat="server" ControlToValidate="rb_portalprefix"
                                                            Enabled="true" Display="Dynamic"><br>* Site Prefix is required!</asp:RequiredFieldValidator>
                                                    </td>
                                                    <td width="50%" nowrap="nowrap">
                                                        example: My Site - . This will make all your page titles: "My Site - Page Name"
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="wizardsectionheader">
                                            Enter the SMTP Server and Email address that you want portal emails sent from.
                                        </div>
                                        <div class="wizardsection">
                                            <table cellpadding="2" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left" valign="top" width="100">
                                                        SMTP Server:
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox CssClass="dataentry" ID="rb_smtpserver" runat="server">localhost</asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="req_rb_smtpserver" runat="server" ControlToValidate="rb_smtpserver"
                                                            Enabled="true" Display="Dynamic"><br>* SMTP Server is required!</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" width="100">
                                                        Email From:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox CssClass="dataentry" ID="rb_emailfrom" runat="server">admin@portal.com</asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="req_rb_emailfrom" runat="server" ControlToValidate="rb_emailfrom"
                                                            Enabled="true" Display="Dynamic"><br>* Email From Address is required!</asp:RequiredFieldValidator>
                                                    </td>
                                                    <td valign="top">
                                                        The Email From address is used on all outgoing messages from the Portal.
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="Install" runat="server" Visible="false">
                                        <div class="mainTitle">
                                            Installing Appleseed</div>
                                        <div class="wizardsection">
                                            <table cellpadding="2" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left">
                                                        You are about to write configuration file and run db scripts.
                                                        <br />
                                                        Please click "Next" to continue.
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="Done" runat="server" Visible="false">
                                        <div class="mainTitle">
                                            Complete!</div>
                                        <div class="wizardsection">
                                            <div>
                                                <div>
                                                    Go to <a href="../Default.aspx">Appleseed</a>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="Errors" runat="server" Visible="false">
                                        <span class="mainTitle">Errors Occurred</span><br />
                                        <br />
                                        <div>
                                            Errors occured during the execution of this wizard.
                                        </div>
                                        <asp:Repeater ID="lstMessages" runat="server">
                                            <HeaderTemplate>
                                                <table class="err" width="580px" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <th class="err" width="100px">
                                                            Module
                                                        </th>
                                                        <th class="err">
                                                            Message
                                                        </th>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr valign="top">
                                                    <td class="err">
                                                        <%# ((InstallerMessage)Container.DataItem).Module %>
                                                    </td>
                                                    <td class="err">
                                                        <%# ((InstallerMessage)Container.DataItem).Message %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="bottom" colspan="2">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right" bgcolor="#cecece" height="45">
                                <div style="padding-right: 30px;">                                    
                                    <asp:Button ID="Previous" OnClick="PreviousPanel" runat="server" Text="< Previous"
                                        CssClass="buttons"></asp:Button>&nbsp;<asp:Button ID="Next" OnClick="NextPanel" runat="server"
                                            Text="Next >" CssClass="buttons"></asp:Button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
