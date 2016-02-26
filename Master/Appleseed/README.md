# Appleseed Portal 1.0.7.x

Appleseed Portal is an open source Portal and Content Management System focused on 
providing a high-performance,easy to use framework. The software gives designers, 
developers, and modern entrepreneurs a singular system to build powerful applications 
and intranets that run the modern enterprise on the Microsoft.NET and Windows platforms.

## Requirements 

Windows 7 / 8 / 10 Physical or Cloud
SQL Server (Express) 2005/2008/2012/2014
Visual Studio 2015 ( Visual Studio 14 ) for development

## Quick Install
 - Create an Empty SQL Server Database in your SQL Server Instance
 - Give a user access to your SQL Server Databse ( Network Service is recommended )
 - Unzip the zip archive to a non-windows controled folder C:\Appleseed\ ( ie : Not the Windows folder)
 - Create a Site in IIS with a .NET 4.0 Application Pool
 - Change the Identy on the Application Pool to Network Service ( The site will run under this identity)
 - In IIS, Point the site folder to C:\Appleseed\  or where you unzipped your achive
 - Browse your Site
 - Follow the Instructions of the Web Installer
 - If you have permissions warnings, give write access to the Identy in control of your Application Pool ( ie NetworkService)
 - Web.config -- Needs write if you want the web installer to set your web.config up for you ( recommended)
 - This should be removed after install 
 - \rblogs -- Needs write to provide error / info logs
 - \Portals -- Needs write to allow writing site files 


## Website 

http://www.appleseedapp.net

## GitHub 

https://github.com/Appleseed

## Documentation 

http://learn.appleseedapp.net

## Issue Tracker  

https://github.com/Appleseed/portal/issues
