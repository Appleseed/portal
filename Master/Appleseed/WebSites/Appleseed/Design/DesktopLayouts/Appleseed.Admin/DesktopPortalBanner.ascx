<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Src="~/DesktopModules/CoreModules/Admin/AdminLeftMenu.ascx" TagPrefix="uc1" TagName="AdminLeftMenu" %>

<script runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
        PortalTitle.DataBind();
        PortalImage.DataBind();
    }
</script>

<div class="navbar" role="navigation">
    <div class="container">
        <div class="navbar-header">
            <a class="navbar-brand" href="/1">
                <!-- Portal Logo Image Uploaded-->
                <!--<rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false"/>-->
                <!-- End Portal Logo-->
                <!-- Portal Title -->
                <!--<rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle navbar-brand" enableviewstate="false"></rbfwebui:headertitle>-->
                <!-- End Portal Title -->
            </a>
            <!-- Begin Portal Menu -->
            <!--<asp:Menu 	ID="biMenu"	runat="server" 
			            DataSourceID="biSMDS"
                        CssClass="menu main-menu" 
			            DynamicEnableDefaultPopOutImage="False" 
			            StaticEnableDefaultPopOutImage="False">                                
            </asp:Menu>-->
            <!-- End Portal Menu -->
            <a data-placement="bottom" data-original-title="Show / Hide Left" data-toggle="tooltip" class="btn btn-primary btn-sm toggle-left" id="menu-toggle">
                <i class="fa fa-bars"></i>
                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-bars.png" />
            </a>
        </div>
    </div>
</div>

<div class="menus">
    <div id="left">
        <!-- mini menu -->
        <ul class="menu" class="bg-dark">
            <li class="active">
                <button type="button" class="menu-button" href="#home-menu">
                    <i class="fa fa-home"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-home.png" />
                    <span class="link-title">Home</span>
                </button>
            </li>
            <li class="">
                <button type="button" class="menu-button" href="#users-menu">
                    <i class="fa fa-users"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                    <span class="link-title">Users</span>
                    <span class="fa arrow"></span>
                </button>
            </li>
            <li class="">
                <button type="button" class="menu-button" href="#pages-menu">
                    <i class="fa fa-files-o"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                    <span class="link-title">Pages</span>
                    <span class="fa arrow"></span>
                </button>
            </li>
            <li class="">
                <button type="button" class="menu-button" href="#modules-menu">
                    <i class="fa fa-cube"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cube.png" />
                    <span class="link-title">Modules</span>
                    <span class="fa arrow"></span>
                </button>
            </li>
            <li class="">
                <button type="button" class="menu-button" href="#files-menu">
                    <i class="fa fa-folder"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-folder.png" />
                    <span class="link-title">Files</span>
                </button>
            </li>
            <li class="">
                <button type="button" class="menu-button" href="#design-menu">
                    <i class="fa fa-photo"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-picture.png" />
                    <span class="link-title">Design</span>
                </button>
            </li>
            <li class="">
                <button type="button" class="menu-button" href="#settings-menu">
                    <i class="fa fa-cog"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                    <span class="link-title">Settings</span>
                </button>
            </li>
            <li class="">
                <button type="button" class="menu-button" href="#recycler-menu">
                    <i class="fa fa-trash-o"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-trash.png" />
                    <span class="link-title">Recycle</span>
                </button>
            </li>
            <li class="nav-divider"></li>
            <li class="nav-header">Programs</li>
            <li class="nav-divider"></li>
            <li class="">
                <button type="button" class="menu-button" href="javascript:;">
                    <i class="fa fa-tree"></i>
                    <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/konotreelogo.png" />
                    <span class="link-title">Kono Tree</span>
                </button>
            </li>
            <!--<li class="nav-divider"></li>
      <li class="nav-header">Ads</li>
      <li class="nav-divider"></li>
      <li>
        <a href="https://www.digitalocean.com/?refcode=e29769a0f3a5">
          <i class="fa fa-ticket"></i>
          <span class="link-title">AdLink</span> 
        </a> 
      </li>-->
        </ul>
        <!-- /mini menu -->
    </div>
    <!-- /#left -->

    <!-- wide sidebar -->
    <div id="left-wide">
        <div class="search-bar">
            <div class="main-search" action="">
                <div class="input-group">
                    <input type="text" class="form-control" placeholder="Live Search ...">
                    <span class="input-group-btn">
                        <button class="btn btn-primary btn-sm text-muted" type="button">
                            <i class="fa fa-search"></i>
                            <img class="fa-img search-icon" src="/Design/Themes/Appleseed.Admin/images/icons/fa-search.png" />
                        </button>
                    </span>
                </div>
            </div>
            <!-- /.main-search -->
        </div>
        <%--Start Admin menu--%>
        <div>
            <uc1:AdminLeftMenu runat="server" id="AdminLeftMenu" />
        </div>
        <%--End Admin menu--%>
        <!-- /.search-bar -->
        <div class="media user-media bg-dark dker">
            <div class="user-media-toggleHover">
                <span class="fa fa-user"></span>
            </div>
            <div class="user-wrapper bg-dark">
                <a class="user-link" href="javascript:;">
                    <img class="media-object img-thumbnail user-img" alt="User Picture" src="/Design/Themes/Appleseed.Admin/img/user.gif">
                    <span class="label label-danger user-label">16</span>
                </a>
                <div class="media-body">
                    <h5 class="media-heading">Anant</h5>
                    <ul class="list-unstyled user-info">
                        <li>
                            <a href="javascript:;">Administrator</a></li>
                        <li>Last Access :
              <br>
                            <small>
                                <i class="fa fa-calendar"></i>
                                <img class="fa-img date-icon" src="/Design/Themes/Appleseed.Admin/images/icons/fa-calendar.png" />
                                <span class="access-date">&nbsp;16 Mar 16:32</span></small>
                        </li>
                    </ul>
                </div>
            </div>
        </div>

        <!-- .wide-menu #home-menu-->
        <div id="home-menu" class="wide-menu bg-dark dker">
            <!--<div id="notice1" class="notice">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>Notice!</h4>
        <p>Lorem ipsum dolor sit amet.</p>
      </div>
      <div id="notice2" class="notice">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>Notice!</h4>
        <p>Lorem ipsum dolor sit amet.</p>
      </div>
      <div id="notice3" class="notice">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>Notice!</h4>
        <p>Lorem ipsum dolor sit amet.</p>
      </div>-->
        </div>
        <!-- /.wide-menu #home-menu -->

        <!-- .wide-menu #users-menu-->
        <div id="users-menu" class="accordion-lw wide-menu bg-dark dker">
            <a class="section" href="javascript:;">
                <span class="link-title">Roles</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-users"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                        &nbsp;Role 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-users"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                        &nbsp;Role 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-users"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                        &nbsp;Role 3
                    </a>
                </li>
            </ul>
            <a class="section" href="javascript:;">
                <span class="link-title">Users</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-users"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                        &nbsp;Manage Users
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-users"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                        &nbsp;Add New User
                    </a>
                </li>
            </ul>
        </div>
        <!-- /.wide-menu #users-menu -->

        <!-- .wide-menu #pages-menu-->
        <div id="pages-menu" class="accordion-lw wide-menu bg-dark dker">
            <a class="section" href="javascript:;">
                <span class="link-title">Pages</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-file-o"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                        &nbsp;Home
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-file-o"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                        &nbsp;About
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-file-o"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                        &nbsp;Products
                    </a>
                    <ul class="acc-inner">
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-cube"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                                &nbsp;Product 1
                            </a>
                        </li>
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-cube"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                                &nbsp;Product 2
                            </a>
                        </li>
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-cube"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                                &nbsp;Product 3
                            </a>
                        </li>
                    </ul>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-file-o"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                        &nbsp;Services
                    </a>
                    <ul class="acc-inner">
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-users"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                                &nbsp;Service 1
                            </a>
                        </li>
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-users"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                                &nbsp;Service 2
                            </a>
                        </li>
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-users"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-users.png" />
                                &nbsp;Service 3
                            </a>
                        </li>
                    </ul>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-file-o"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                        &nbsp;Contacts
                    </a>
                </li>
            </ul>
        </div>
        <!-- /.wide-menu #pages-menu -->

        <!-- .wide-menu #modules-menu-->
        <div id="modules-menu" class="accordion-lw wide-menu bg-dark dker">
            <a class="section" href="javascript:;">
                <span class="link-title">Installed Modules</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cube"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cube.png" />
                        &nbsp;Module 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cube"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cube.png" />
                        &nbsp;Module 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cube"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cube.png" />
                        &nbsp;Module 3
                    </a>
                </li>
            </ul>
            <a class="section" href="javascript:;">
                <span class="link-title">Actions</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cube"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cube.png" />
                        &nbsp;Add Modules
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cube"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cube.png" />
                        &nbsp;Update Modules
                    </a>
                </li>
            </ul>
        </div>
        <!-- /.wide-menu #modules-menu -->

        <!-- .wide-menu #files-menu-->
        <div id="files-menu" class="accordion-lw wide-menu bg-dark dker">
            <a class="section" href="javascript:;">
                <span class="link-title">Files</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li accordion-sub">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-folder"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-folder.png" />
                        &nbsp;Folder 1
                    </a>
                    <ul class="acc-inner">
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-file-o"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                &nbsp;File 1
                            </a>
                        </li>
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-file-o"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                &nbsp;File 2
                            </a>
                        </li>
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-file-o"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                &nbsp;File 3
                            </a>
                        </li>
                    </ul>
                </li>
                <li class="active acc-li accordion-sub">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-folder"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-folder.png" />
                        &nbsp;Folder 2
                    </a>
                    <ul class="acc-inner">
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-file-o"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                &nbsp;File 1
                            </a>
                        </li>
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-file-o"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                &nbsp;File 2
                            </a>
                        </li>
                        <li class="acc-li">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-file-o"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                &nbsp;File 3
                            </a>
                        </li>
                    </ul>
                </li>
                <li class="acc-li accordion-sub">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-folder"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-folder.png" />
                        &nbsp;Folder 3
                    </a>
                    <ul class="acc-inner">
                        <li class="acc-li accordion-sub">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-folder"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-folder.png" />
                                &nbsp;Subfolder 1
                            </a>
                            <ul class="acc-inner">
                                <li class="acc-li">
                                    <a class="list-button" href="javascript:;">
                                        <i class="fa fa-file-o"></i>
                                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                        &nbsp;File 1
                                    </a>
                                </li>
                                <li class="acc-li">
                                    <a class="list-button" href="javascript:;">
                                        <i class="fa fa-file-o"></i>
                                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                        &nbsp;File 2
                                    </a>
                                </li>
                                <li class="acc-li">
                                    <a class="list-button" href="javascript:;">
                                        <i class="fa fa-file-o"></i>
                                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                        &nbsp;File 3
                                    </a>
                                </li>
                            </ul>
                        </li>
                        <li class="acc-li accordion-sub">
                            <a class="list-button" href="javascript:;">
                                <i class="fa fa-folder"></i>
                                <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-folder.png" />
                                &nbsp;Subfolder 2
                            </a>
                            <ul class="acc-inner">
                                <li class="acc-li accordion-lw">
                                    <a class="list-button" href="javascript:;">
                                        <i class="fa fa-file-o"></i>
                                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                        &nbsp;File 1
                                    </a>
                                </li>
                                <li class="acc-li">
                                    <a class="list-button" href="javascript:;">
                                        <i class="fa fa-file-o"></i>
                                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                        &nbsp;File 2
                                    </a>
                                </li>
                                <li class="acc-li">
                                    <a class="list-button" href="javascript:;">
                                        <i class="fa fa-file-o"></i>
                                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-pages.png" />
                                        &nbsp;File 3
                                    </a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
        </div>
        <!-- /.wide-menu #files-menu -->

        <!-- .wide-menu #design-menu-->
        <div id="design-menu" class="accordion-lw wide-menu bg-dark dker">
            <a class="section" href="javascript:;">
                <span class="link-title">Layouts</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-picture"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-picture.png" />
                        &nbsp;Layout 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-picture"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-picture.png" />
                        &nbsp;Layout 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-picture"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-picture.png" />
                        &nbsp;Layout 3
                    </a>
                </li>
            </ul>
            <a class="section" href="javascript:;">
                <span class="link-title">Themes</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-picture"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-picture.png" />
                        &nbsp;Theme 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-picture"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-picture.png" />
                        &nbsp;Theme 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-picture"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-picture.png" />
                        &nbsp;Theme 3
                    </a>
                </li>
                <li class="acc-li">
                    <a href="sm.html">
                        <i class="fa fa-picture"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-picture.png" />
                        &nbsp;Theme 4
                    </a>
                </li>
            </ul>
        </div>
        <!-- /.wide-menu #design-menu -->

        <!-- .wide-menu #settings-menu-->
        <div id="settings-menu" class="accordion-lw wide-menu bg-dark dker">
            <a class="section" href="javascript:;">
                <span class="link-title">Global</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 3
                    </a>
                </li>
            </ul>
            <a class="section" href="javascript:;">
                <span class="link-title">Basic</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 3
                    </a>
                </li>
                <li class="acc-li">
                    <a href="sm.html">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 4
                    </a>
                </li>
            </ul>
            <a class="section" href="javascript:;">
                <span class="link-title">Advanced</span>
                <span class="fa arrow"></span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 3
                    </a>
                </li>
                <li class="acc-li">
                    <a href="sm.html">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 4
                    </a>
                </li>
            </ul>
            <a class="section" href="javascript:;">
                <span class="link-title">CDN</span>
                <span class="fa arrow"></span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 3
                    </a>
                </li>
                <li class="acc-li">
                    <a href="sm.html">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 4
                    </a>
                </li>
            </ul>
            <a class="section" href="javascript:;">
                <span class="link-title">Social</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 3
                    </a>
                </li>
                <li class="acc-li">
                    <a href="sm.html">
                        <i class="fa fa-cog"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Setting 4
                    </a>
                </li>
            </ul>
        </div>
        <!-- /.wide-menu #settings-menu -->

        <!-- .wide-menu #recycler-menu-->
        <div id="recycler-menu" class="accordion-lw wide-menu bg-dark dker">
            <a class="section" href="javascript:;">
                <span class="link-title">Recycler</span>
            </a>
            <ul class="acc-inner">
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-trash-o"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Recycled Item 1
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-trash-o"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Recycled Item 2
                    </a>
                </li>
                <li class="acc-li">
                    <a class="list-button" href="javascript:;">
                        <i class="fa fa-trash-o"></i>
                        <img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-cog.png" />
                        &nbsp;Recycled Item 3
                    </a>
                </li>
            </ul>
        </div>
        <!-- /.wide-menu #recycler-menu-->

    </div>
    <!-- /#left-wide -->
</div>

<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />



<script>
    $(function () {
        $(".wide-menu.accordion-lw, .wide-menu .accordion-sub").accordion({ collapsible: true });
    });

    $(document).ready(function () {
        var $menuDivs = $('#home-menu, #users-menu, #users-menu, #pages-menu, #files-menu, #modules-menu, #design-menu, #settings-menu, #recycler-menu');
        $('#home-menu, #users-menu, #users-menu, #pages-menu, #files-menu, #modules-menu, #design-menu, #settings-menu, #recycler-menu').hide();
        $('#home-menu').show();
        $('.menu-button').click(function () {
            var href = $(this).attr('href');
            console.log(href);
            $menuDivs.hide();
            console.log(href);
            $(href).show();
            //$('body').scrollTop(-155); not working ---
            removeHash(); // not working either.
            console.log('test2');
        });
    });

    function removeHash() {
        var scrollV, scrollH, loc = window.location;
        if ("pushState" in history)
            history.pushState("", document.title, loc.pathname + loc.search);
        else {
            // Prevent scrolling by storing the page's current scroll offset
            scrollV = document.body.scrollTop;
            scrollH = document.body.scrollLeft;

            loc.hash = "";

            // Restore the scroll offset, should be flicker free
            document.body.scrollTop = scrollV;
            document.body.scrollLeft = scrollH;
        }
    }
</script>

<script type="text/javascript">

    $("a.AspNet-Menu-Link[href='/site/1/Home']").parent().hide();
    $("a.AspNet-Menu-Link[href='/site/100/Administration']").parent().hide();

    var siteurl = window.location.href.toString();
    var pagename = siteurl.substr(siteurl.lastIndexOf("/") + 1);
    var toppage = siteurl.split('/');
    var toppagename = $("#divCurrentCategory li").text()
    if (toppage[5] == pagename) { $("#divCurrentCategory li").addClass("current"); };
    $("body").addClass(toppage[5]);
    $("body").addClass(pagename);

    $(document).ready(function () {
        $('p').each(function () {
            var $this = $(this);
            if ($this.html().replace(/\s|&nbsp;/g, '').length == 0)
                $this.addClass("hide");
        });

        var url = window.location.href.toString().split("/");
        var pagename2 = url.pop();
        var pagename1 = url.pop();
        $("#Banner_biMenu a[href$='" + pagename1 + "/" + pagename2 + "']")
            .closest("li")
            .addClass("selected-item");

        $('#menu-toggle').click(function () {
            $('#left,#left-wide').toggleClass("show");
        });

        $("#menu.wide-menu li a").click(function () {

            $("#menu.wide-menu li a + ul").slideToggle("fast");
        });
    });

</script>
