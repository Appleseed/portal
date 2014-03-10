using System;

namespace Appleseed.Framework.Threading
{
	/// <summary>
	/// When event is raised from ThreadHandler the ThreadHandler itself will be passed to the delegate function.
	/// So controlling the thread, that invoked the event is possible.
	/// </summary>
	public class ThreadHandlerEventArgs : EventArgs
	{
		/// <summary>
		/// ThreadHandler, which has generated the event.
		/// </summary>
		private ThreadHandler _threadHandler;

		/// <summary>
		/// The ThreadHandler from where this event was generated.
		/// </summary>
		public ThreadHandler Handler
		{
			get
			{
				return _threadHandler;
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="threadHandler">ThreadHandler object</param>
		public ThreadHandlerEventArgs(ThreadHandler threadHandler)
		{
			_threadHandler = threadHandler;
		}
	}

	/// <summary>
	/// When unhandled exception occurs in the event delegate functions, the exception will be cathed by the thread!
	/// The ThreadHandler will raise an event, the Exception along with the ThreadHandler will be passed via these EventArgs.
	/// The thread will be aborted, after the function returns from the OnException delegate function!
	/// </summary>
	public class ThreadHandlerExceptionArgs : EventArgs
	{
		/// <summary>
		/// The ThreadHandler, where the exception occured.
		/// </summary>
		private ThreadHandler _threadHandler;

		/// <summary>
		/// The Exception object.
		/// </summary>
		private Exception _exception;

		/// <summary>
		/// The ThreadHandler, where the exception occured.
		/// </summary>
		public ThreadHandler Handler
		{
			get
			{
				return _threadHandler;
			}
		}

		/// <summary>
		/// The exception that was catched.
		/// </summary>
		public Exception Ex
		{
			get
			{
				return _exception;
			}
		}

		/// <summary>
		/// Constructor for ThreadHandlerExceptionArgs.
		/// </summary>
		/// <param name="threadHandler">ThreadHandler object</param>
		/// <param name="exception">Exception object</param>
		/// <param name="stateAtException">Thread's state at the moment of the exception.</param>
		public ThreadHandlerExceptionArgs(ThreadHandler threadHandler, Exception exception)
		{
			_threadHandler = threadHandler;
			_exception = exception;
		}
	}
}
