# Appleseed.Portal 1.4.131.463 Release
- Release Date: 04/08/2017

## Requirements
- This version of Appleseed.Portal requires .NET 4.6.1 to be installed.

## Major Bug Fixes
- Fixed Monitoring Page
- Fixed Title double click to edit on http/https + port numbers
- Fixes some issues in User Manager 

## Features
- Removed old dependencies 

## Known bugs that need to be fixed
- n/a


# Appleseed.Portal 1.4.98.383 Pre-Release
- Release Date: 03/31/2017

## Requirements
- This version of Appleseed.Portal requires .NET 4.6.1 to be installed.

## Major Bug Fixes
- Fixed Google Login with updating to oAuth 2.0
- Fixed JavaScript Facebook include to work with http or https

## Theme CSS Fixes
-  Fixed CSS on Double click Module Title Edit

## Features
 - Created new Member Invites module that allows admins to invite users to their site 
 - Updated ASP.NET MVC 5 and removed dependencies on MVC 4 and MVC 3
-  Removed old modules
-  Removed Aloha as an HTML Editor
-  Removed old dependencies 

## Known bugs that need to be fixed
- Double click Module Title edit on normal http still has an issue
- The monitoring page had some modules moved around.  Our team will resolve


# Appleseed.Portal 1.3.69.342 Release
- Release Date: 02/5/2017

## Requirements
- This version of Appleseed.Portal requires .NET 4.6.1 to be installed.

## Major Bug Fixes
- File Manager - Fixed an issues allowing the upload of certain file types : Main JavaScript
- File Manager - Cleaned out hard-coded Spanish language to be put in resource files 
- CKeditor - Fixed an issue where CKeditor was not saving the <section> element of html

## Theme CSS Fixes
-  Fixed Code Mirror CSS issues for all themes
-  Cleaned up new Admin Theme Sections for Mobile Browsers

## Features
- Portal now locks users out who fail their password 5 times
- Codewriter HTML Editor - Made sections 2 X 2 and gave more room for entering code / content
- File Manager - Edit / Upload list now by web.config instead of in the code
- Updated Log4Net to 2.0.7

## Known bugs that need to be fixed
- N/A



# Appleseed.Portal 1.2.43.305 Release
- Release Date: 12/19/2016

## Requirements
- This version of Appleseed.Portal requires .NET 4.6.1 to be installed.

## Major Bug Fixes
- Resolved some more connection strings not closing
- Resolved User Manager issue not searching on first click
- Resolve some performance issues on First Portal Load

## Theme CSS Fixes
- None

## Features
- New Codewriter HTML Editor type that allows separation of HTML / CSS / Javascript for portal widget creation
- Administrators can now create a user and password on install instead of using the default admin
- Portal Timeout for user login is now a portal setting instead of using the Web.config
- This version is confirmed it works on Docker and as such can run on MacOS / Windows / Linux

## Known bugs that need to be fixed
- Web.config is being changed if write is left on for your App Pool user.  Please disable web.config write after install until we disable this.



# Appleseed.Portal 1.1.0.0 Release
- Release Date: 10/5/2016

## Requirements
- This version of Appleseed.Portal requires .NET 4.6.1 to be installed.

## Major Bug Fixes
- Cleaned up more warnings and bugs

## Theme CSS Fixes
-  Minor Script tweaks

## Features
- Install Module Instance manager on installation
- Clean up all warnings and obsolete code  
- Added Images for Sorting in Recycle Module  
- Wrap HTML Module with Div Class Changes / Style changes of HTML Module  
- Integrate MVC File Manager with HTML Module  
- Expanded Width and Height on HTML Editing
- Inline HTML edit with CKEditor.  
- Proxy Module Implementation - Expose Private server resources by portal role  
- Code changes to resolved - Bug - Iframe Settings  
- Implementation of Environment check for 4.6.1 on install  
- Edit this page module list alphabetical ordering  
- Admin Theme Changes to show last logged in based on local timezone  

## Known bugs that need to be fixed
N/A 


# Appleseed.Portal 1.0.9.1 Release
- Release Date: 8/5/2016

## Requirements
- This version of Appleseed.Portal requires .NET 4.6 to be installed.

## Major Bug Fixes
- Default.css is not being pushed out from the theme on the page. It is currently looking at the portal value in site settings.  We have fixed this to look at the page if there is a page setting.
- Cleaned up more warnings and bugs
- Removed old File Manager from HTML Module
- Resolved issues with Recycle bin sorting

## Theme CSS Fixes
- Admin.Theme - Tweaks to the admin theme now the default.css bug is resolved

## Features
- Private Site option in Site Settings.  Allows a site to instantly become a private site that requires login.
- Google Analytics API Upgrade.  The Google Analytics API has been upgraded to work with the latest GA calls.
- Improved Paged User Manager 
- Recycled Pages - The ability to recover deleted pages

## Known bugs that need to be fixed
- N/A


# Appleseed.Portal 1.0.9.0 Pre-Release
- Release Date: 7/26/2016

## Requirements
- This version of Appleseed.Portal requires .NET 4.6 to be installed.

## Major Bug Fixes
- Default.css is not being pushed out from the theme on the page. It is currently looking at the portal value in site settings.  We have fixed this to look at the page if there is a page setting.
- Cleaned up more warnings and bugs
- Removed old File Manager from HTML Module

## Theme CSS Fixes
- Admin.Theme - Tweaks to the admin theme now the default.css bug is resolved

## Features
- Private Site option in Site Settings.  Allows a site to instantly become a private site that requires login.
- Google Analytics API Upgrade.  The Google Analytics API has been upgraded to work with the latest GA calls.

## Known bugs that need to be fixed
- NA

# Appleseed Portal 1.0.8.0

Release Date: 7/15/2016

## Requirements
- This version of Appleseed.Portal requires .NET 4.6 to be installed.

## Major Bug Fixes
- Edit this Page - Fixed the ability to move modules around in panes
- Cleaned up warnings and bugs
- Fixed Failed Password attempts field.  This field now collects when a failed password occurs
- Access any page with /pagenumber or /site/pagenumber when friendly URLs are enabled

## Themes
- Theme CSS Fixes
- Admin.Theme Add - A theme for the new consolidated admin area was added

## Features
- Consolidated Admin options into an Admin Theme
- Consolidated HTML Editor Options to Three : Ckeditor / CodeMirror / Aloha
- Improved Web Based Installation -- Updated SQL Section to fit better with default SQL Express install
- Improved File Manager  -- Added new MVC based File Manager to Admin area
- Added Short Links -  The ability to create links like www.domain.com/ShortLink for pages

## Known bugs that need to be fixed
- Default.css is not being pushed out from the theme on the page.  It is currently looking at the portal value in site settings.  This will be resolved in 1.0.9.x


# Appleseed Portal 1.0.7.0

Initial Github Release 

## Major Bug Fixes 
 - Connection string fixes.  Appleseed uses less open connections to the database.  Faster responsive times.  Less memory usage.

## 6 Bootstrap Themes
 - Cosmo.Bootrap 
 - Flatly.Bootstrap
 - Freelancer.Bootstrap 
 - Landing-Page.Bootstrap 
 - Modern-Business.Bootstrap 
 - Readable.Bootstrap
 
## Tested Features
 - Improved Web Based Installation
 - Custom Javascript + CSS per page
 - Custom Bootstrap Theme & Layout Support
 - Site Management
 - Improved Page Manager
 - Improved User Manager
 - Improved File Manager 
 - Role Manager
 - Role Permissions
 - HTML Version control
 - SEO Friendly URLs
 - Add custom functionality through ASP.NET Web User Controls
 - Add custom functionality through ASP.NET MVC 4 Views
 - Custom Profile Provider (stores profile info one record at a time to make searchable)
 - ASPNETDB Single Sign-on compatible 
 
## Known bugs that need to be fixed
 - Need to clean up warnings and old code
 - login remember me flags
 - Renaming module titles is broken and hard to see
