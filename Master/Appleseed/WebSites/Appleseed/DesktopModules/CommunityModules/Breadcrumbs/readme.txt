BreadCrumbs (as a module)

Another Appleseed desktop module - more to download on http://www.Appleseedportal.net

INSTALL 
1. Go to Admin all and to add module definition. 
2. Point to install.xml install file
3. Add the module to a page
4. Edit module settings: 
5. Use it! 8) 

HISTORY
original BreadCrumbs for Appleseed  - 24/12/2002 byCory Isakson [cisakson@yahoo.com]
BreadCrumbs Ver. 0.5 beta - 27/05/2003 - adapted as a modul by Mario Hartmann

ISSUES AND KNOWN PROBLEMS:


MODULE SETTINGS
---------------
-none-


STYLE SETTINGS
--------------
the module uses following styles.
- .BreadCrumbs
- .BreadCrumbsLink
- .BreadCrumbsText


EXAMPLE FOR THE STYLESHEET
--------------------------


/* ================================
   BreadCrumbs Module
   ================================ */
.BreadCrumbs
{
	background-color: #87cefa; /*lightskyblue*/
	border: solid 1px #00008b; /*darkblue*/
	border-collapse:collapse;
}

.BreadCrumbsLink /* the current Link */
{
	color: #00008b; /*darkblue*/
	text-decoration: none;
}
A.BreadCrumbsLink:visited, A.BreadCrumbsLink:active , A.BreadCrumbsLink:link
{
	color: #00008b; /*darkblue*/
	text-decoration: none;
}
A.BreadCrumbsLink:hover
{
	color:#ffd700; /*gold*/
	text-decoration: underline;
}

.BreadCrumbsText /* seperator style */
{
	color:#00008b; /*darkblue*/
	text-decoration: none;
}
/* ================================ */