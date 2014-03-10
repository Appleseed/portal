using System.Configuration;

namespace Appleseed.Framework.Settings
{
	/// <summary>
	/// Interface for Config Reader Strategy
	/// </summary>
	public interface Strategy
	{
		/// <summary>
		/// Fetch value for key
		/// </summary>
		/// <param name="key">key</param>
		/// <returns>string value</returns>
		string GetAppSetting(string key);
	}

	/// <summary>
	/// Concrete Strategy - reads from ConfigurationSettings.AppSettings
	/// </summary>
	public class ConfigReader : Strategy
	{
		/// <summary>
		/// Fetches value for key from ConfigurationSettings.AppSettings
		/// </summary>
		/// <param name="key">key</param>
		/// <returns>string value</returns>
		public string GetAppSetting(string key)
		{
			if (key != null && key.Length != 0)
				return ConfigurationManager.AppSettings[key];//return ConfigurationSettings.AppSettings[key];
			return null;
		}
	}

	/// <summary>
	/// Reader
	/// </summary>
	public class Reader
	{
		private Strategy strategy;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="strategy">a Concrete Strategy</param>
		public Reader(Strategy strategy)
		{
			this.strategy = strategy;
		}


		/// <summary>
		/// Fetches value for key - source depends on which ConcreteStrategy is set
		/// </summary>
		/// <param name="key">key</param>
		/// <returns>string value</returns>
		public string GetAppSetting(string key)
		{
			return strategy.GetAppSetting(key);
		}
	}

}