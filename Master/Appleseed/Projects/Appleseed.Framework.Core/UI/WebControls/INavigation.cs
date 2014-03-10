namespace Appleseed.Framework.Web.UI.WebControls
{
	/// <summary>
	/// Default indterface for navigation controls
	/// </summary>
	public interface INavigation
	{
		/// <summary>
		/// Indicates if control should bind when loads
		/// </summary>
		/// <value><c>true</c> if [auto bind]; otherwise, <c>false</c>.</value>
		bool AutoBind
		{
			get;
			set;
		}

		/// <summary>
		/// Describes how this control should bind to db data
		/// </summary>
		/// <value>The bind.</value>
		BindOption Bind
		{
			get;
			set;
		}

		/// <summary>
		/// Describes how this control should bind to db data
		/// </summary>
		/// <value>The parent page ID.</value>
		int ParentPageID
		{ get;set;}

	}
}