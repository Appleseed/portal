using System.Collections;

namespace Appleseed.Framework.Setup
{
	/// <summary>
	/// IInstaller inteface is used by installable modules
	/// </summary>
	public interface IInstaller
	{
        /// <summary>
        /// Installs the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
		void Install(IDictionary stateSaver);
        /// <summary>
        /// Uninstalls the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
		void Uninstall(IDictionary stateSaver);
        /// <summary>
        /// Commits the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
		void Commit(IDictionary stateSaver);
        /// <summary>
        /// Rollbacks the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
		void Rollback(IDictionary stateSaver);
	}
}