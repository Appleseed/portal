namespace Appleseed.Framework
{
    /// <summary>
    /// Enumeration for workflow version
    /// by Geert.Audenaert@Syntegra.Com
    /// Date: 6/2/2003
    /// </summary>
    public enum WorkFlowVersion
    {
        /// <summary>
        /// Production
        /// </summary>
        Production = 1,
        /// <summary>
        /// Staging
        /// </summary>
        Staging = 2
    }

    /// <summary>
    /// WorkflowState
    /// by Geert.Audenaert@Syntegra.Com
    /// Date: 27/2/2003
    /// </summary>
    public enum WorkflowState
    {
        /// <summary>
        /// The staging and production content are identical
        /// </summary>
        Original = 0,
        /// <summary>
        /// Were working on the staging content
        /// </summary>
        Working = 1,
        /// <summary>
        /// The staging content is ready and approval is been requested
        /// </summary>
        ApprovalRequested = 2,
        /// <summary>
        /// The staging content is approved and ready to be published
        /// </summary>
        Approved = 3
    }
}

namespace Appleseed.Framework.Configuration
{
    // TODO: Uncomment this for web solution compatibility, 
    // it was commented to change the core.
    /*
	/// <summary>
	/// Enumeration for workflow version
	/// by Geert.Audenaert@Syntegra.Com
	/// Date: 6/2/2003
	/// </summary>
	public enum WorkFlowVersion
	{
		/// <summary>
        /// Please use Appleseed.Framework.WorkFlowVersion
		/// </summary>
		Production = 1,
		/// <summary>
        /// Please use Appleseed.Framework.WorkFlowVersion
		/// </summary>
		Staging = 2
	}
    /// <summary>
    /// WorkflowState
    /// by Geert.Audenaert@Syntegra.Com
    /// Date: 27/2/2003
    /// </summary>
    public enum WorkflowState
    {
        /// <summary>
        /// Please use Appleseed.Framework.WorkflowState
        /// </summary>
        Original = 0,
        /// <summary>
        /// Please use Appleseed.Framework.WorkflowState
        /// </summary>
        Working = 1,
        /// <summary>
        /// Please use Appleseed.Framework.WorkflowState
        /// </summary>
        ApprovalRequested = 2,
        /// <summary>
        /// Please use Appleseed.Framework.WorkflowState
        /// </summary>
        Approved = 3
    }

*/
}