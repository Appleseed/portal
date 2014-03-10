using System;

namespace Appleseed.Framework.Threading
{
	/// <summary>
	/// References the method that will handle the OnJob event.
	/// </summary>
	public delegate void HandlerForOnJob(ThreadHandlerEventArgs arg);

	/// <summary>
	/// References the method that will handle the OnAbort event.
	/// </summary>
	public delegate void HandlerForOnAbort(ThreadHandlerEventArgs arg);

	/// <summary>
	/// References the method that will handle the OnFinish event.
	/// </summary>
	public delegate void HandlerForOnFinish(ThreadHandlerEventArgs arg);

	/// <summary>
	/// References the method that will handle the OnTerminate event.
	/// NOTE: This is the ThreadHandler's cleanup-event. Exceptions here are not handled, and are not taken as thread exceptions.
	/// </summary>
	public delegate void HandlerForOnTerminate(ThreadHandlerEventArgs arg);

	/// <summary>
	/// References the method that will handle the OnException event.
	/// </summary>
	public delegate void HandlerForOnException(ThreadHandlerExceptionArgs arg);
}
