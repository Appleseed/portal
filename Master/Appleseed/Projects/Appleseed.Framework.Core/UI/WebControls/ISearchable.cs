namespace Appleseed.Framework.Web.UI.WebControls
{
	/// <summary>
	/// Default interface for searchable modules
	/// </summary>
	public interface ISearchable
	{
		/// <summary>
		/// Searchable module implementation
		/// </summary>
		/// <param name="portalID">The portal ID.</param>
		/// <param name="userID">The user ID.</param>
		/// <param name="searchStr">The search STR.</param>
		/// <param name="searchField">The search field.</param>
		/// <returns></returns>
		string SearchSqlSelect(int portalID, int userID, string searchStr, string searchField);
	}
}