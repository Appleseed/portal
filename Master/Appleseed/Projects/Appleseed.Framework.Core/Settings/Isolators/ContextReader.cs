using System.Web;

namespace Appleseed.Context
{
	/// <summary>
	/// Interface for Config Reader Strategy
	/// </summary>
	public interface Strategy
	{
		/// <summary>
		/// 
		/// </summary>
		HttpContext Current
		{
			get;
		}
	}

	/// <summary>
	/// Concrete Strategy - gets web current context
	/// </summary>
	public class WebContextReader : Strategy
	{
		/// <summary>
		/// 
		/// </summary>
		public HttpContext Current
		{
			get
			{
				return HttpContext.Current;
			}
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
		/// 
		/// </summary>
		public HttpContext Current
		{
			get
			{
				return strategy.Current;
			}
			
		}
	}
}