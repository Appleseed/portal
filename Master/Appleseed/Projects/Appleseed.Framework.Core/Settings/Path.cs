using System;
using System.Collections;
using System.Text;
using System.Web;

namespace Appleseed.Framework.Settings
{
	/// <summary>
	/// This class keeps some useful path information for Module, Core and Extension Developers
	/// </summary>
	public sealed class Path
	{
		/// <summary>
		/// Constructor
		/// </summary>
		private Path()
		{
		}

		private static string applicationRoot;// = null;

		/// <summary>
		/// ApplicationRoot
		/// Base dir for all portal code
		/// Since it is common for all portals is declared as static
		/// </summary>
		public static string ApplicationRoot
		{
			get
			{
				//Build the relative Application Path
				if (applicationRoot == null)
				{
					HttpRequest req = HttpContext.Current.Request;
					applicationRoot =
						(req.ApplicationPath == "/")
							? ""
							: req.ApplicationPath;
				}
				return applicationRoot;
			}
		}

		/// <summary>
		/// ApplicationPath, Application dependent.
		/// Used by newsletter. Needed if you want to reference a page
		/// from an external resource (an email for example)
		/// Since it is common for all portals is declared as static.
		/// e.g. http://www.mysite.com/Appleseed/
		/// </summary>
		public static string ApplicationFullPath
		{
			get
			{
				HttpRequest req = HttpContext.Current.Request;
				string myAppFullpath =  "http://" + req.Url.Host + req.ApplicationPath;
				
				if(req.Url.Port != 80)
				{
					myAppFullpath = "http://" + req.Url.Host + ":" + req.Url.Port.ToString() + req.ApplicationPath;
				}
				
				return myAppFullpath;
			}
		}

		/// <summary>
		/// ApplicationPhysicalPath.
		/// File system property
		/// </summary>
		public static string ApplicationPhysicalPath
		{
			get { return HttpContext.Current.Request.PhysicalApplicationPath; }
		}

		/// <summary>
		/// ApplicationRoot based path
		/// Since it is common for all portals is declared as static
		/// </summary>
		public static string ApplicationRootPath(string value)
		{
			return WebPathCombine(ApplicationRoot, value);
		}

		/// <summary>
		/// ApplicationRoot based path
		/// Since it is common for all portals is declared as static
		/// </summary>
		public static string ApplicationRootPath(params string[] values)
		{
			ArrayList fullValues = new ArrayList(values);
			fullValues.Insert(0, ApplicationRoot);
			return WebPathCombine((string[]) fullValues.ToArray(Type.GetType("String")));
		}

		/// <summary>
		/// WebPathCombine ensures that combined path is a valid url
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static string WebPathCombine(params string[] values)
		{
			const string webPathSeparator = "/";
			string doubleWebPathSeparator = webPathSeparator + webPathSeparator;

			if (values == null)
				throw new NullReferenceException("Path cannot be null!");

			StringBuilder s = new StringBuilder();
			for (int i = 0; i < values.Length; i++)
			{
				if (i != 0)
					s.Append(webPathSeparator);
				if (values[i] != null && values[i].Length > 0)
					s.Append(values[i]);
			}

			string returnPath = s.ToString();

			while (returnPath.IndexOf(doubleWebPathSeparator) > -1)
				returnPath = returnPath.Replace(doubleWebPathSeparator, webPathSeparator);

			returnPath = returnPath.Replace(":/", "://");

			return returnPath;
		}

	}
}