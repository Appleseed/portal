using System;

namespace Appleseed.Framework.Web.UI.WebControls
{
	/// <summary>
	/// Common interface for paging controls
	/// </summary>
	public interface IPaging
	{
		/// <summary>
		/// 
		/// </summary>
		event EventHandler OnMove;

		/// <summary>
		/// Gets or sets the page number.
		/// </summary>
		/// <value>The page number.</value>
		int PageNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the record count.
		/// </summary>
		/// <value>The record count.</value>
		int RecordCount
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the records per page.
		/// </summary>
		/// <value>The records per page.</value>
		int RecordsPerPage
		{
			get;
			set;
		}
	}
}
