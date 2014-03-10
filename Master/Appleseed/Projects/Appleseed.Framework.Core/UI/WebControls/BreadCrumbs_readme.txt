Breadcrumbs

This user control will render the breadcrumb navigation for the current tab.
It does not render anything when the user on a first level tab.


Written by:
Cory Isakson
24/Dec/2002
cisakson@yahoo.com
www.coryisakson.com

Appleseed desktop navigation module - more to download on http://www.Appleseedportal.net


INSTALL
1. Run Create_GetTabCrumbs.bat (or execute Create_GetTabCrumbs.sql in SQL Query Analyzer)
2. Copy files DeskTopBreadCrumbs.* files to folder Appleseed\Design\DesktopLayouts and add to project
3. Add the DeskTopBreadCrumbs user control to your site banner or wherever you would like it.
4. Modify the TabsDB.cs
	Add to the declarations:
	using System.Xml;

	Add to the end of the file before the last curly brace }:

		public ArrayList GetTabCrumbs(int tabID) 
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = PortalSettings.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_GetTabCrumbs", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterPageID = new SqlParameter("@TabID", SqlDbType.Int, 4);
			parameterPageID.Value = tabID;
			myCommand.Parameters.Add(parameterPageID);

			SqlParameter parameterCrumbs = new SqlParameter("@CrumbsXML", SqlDbType.NVarChar, 4000);
			parameterCrumbs.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterCrumbs);

			// Execute the command
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			// Close the connection
			myConnection.Close();

			// Build a Hashtable from the XML string returned
			ArrayList Crumbs  = new ArrayList();
			XmlDocument CrumbXML = new XmlDocument();
           		CrumbXML.LoadXml(parameterCrumbs.Value.ToString().Replace("&","&amp;"));

			//Iterate through the Crumbs XML
			foreach (XmlNode node in CrumbXML.FirstChild.ChildNodes)
			{
				PageItem tab = new PageItem();
				tab.ID = Int16.Parse(node.Attributes.GetNamedItem("tabID").Value);
				tab.Name = node.InnerText;
				tab.Order = Int16.Parse(node.Attributes.GetNamedItem("level").Value);
				Crumbs.Add(tab);
            		}

			//Return the Crumb Tab Items as an arraylist 
			return Crumbs;
		}

5. Compile


HISTORY
Ver. 1.0 - 24. dec 2002 - First realase by Cory Isakson

Ver. 1.1 - 31. jan 2003 - Update by jes
I made some crude changes to the breadcrumbs control. 
First, there was a problem that it didn't pick up the TabIndex, so I changed that. 
Plus, I didn't like that it made the current page a clickable link, so I changed that. 
Then I wanted it in a table so I could control it's formatting a bit more, 
so I changed it from a simple placeholder to a table. 

Ver. 2.0 - 28 feb 2003 - Update by Manu
Transformed in Table Webcontrol
Cleaned up the code, added support for design time
Now separator text is customizable


-------------

Additional Attributes on the Breadcrumbs control:
TextCSSClass sets the CSS class for the ">" character
LinkCSSClass sets the CSS class for the Crumb link control

Example:
<uc1:DeskTopBreadCrumbs id="DeskTopBreadCrumbs1" runat="server" TextCSSClass="ItemTitle" LinkCSSClass="ItemTitle"></uc1:DeskTopBreadCrumbs>

