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
