using System;
using System.Threading;
using System.Collections.Generic;
namespace Appleseed.Framework.Threading
{
	/// <summary>
	/// Summary description for ThreadHandler.
	/// </summary>
	public class ThreadHandler
	{

		/// <summary>
		/// The thread, which will perform the calculations.
		/// </summary>
		private System.Threading.Thread _thread;

		/// <summary>
		/// Event is raised once, when the processing starts.
		/// </summary>
		public event HandlerForOnJob OnJob;

		/// <summary>
		/// On aborting this event's delegate function may do extra calculations.
		/// </summary>
		public event HandlerForOnAbort OnAbort;

		/// <summary>
		/// If thread is finished SUCCESFULLY, this event will be raised.
		/// </summary>
		public event HandlerForOnFinish OnFinish;

		/// <summary>
		/// If thread is aborted, just before leawing the ThreadHandler's working function this event will be triggered.
		/// </summary>
		public event HandlerForOnTerminate OnTerminate;

		/// <summary>
		/// When the event delegates exceptions are not handled, the thread will catch the exception!
		/// </summary>
		public event HandlerForOnException OnException;

        /// <summary>
        /// Starting the thread.
        /// </summary>
		public void Start()
		{
			lock (this)
			{
				_thread.Start();
			}
		}

        /// <summary>
        /// Aborting the thread...
        /// </summary>
		public void Abort()
		{
			lock (this)
			{
				_thread.Abort();
			}
		}

        /// <summary>
        /// The thread responsible for the working process...
        /// </summary>
        /// <value>The thread.</value>
		public System.Threading.Thread Thread
		{
			get 
			{
				return _thread;
			}
		}

        /// <summary>
        /// Constructor...
        /// </summary>
		public ThreadHandler()
		{
			_thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.Worker));
		}


        /// <summary>
        /// Constructor...
        /// </summary>
        /// <param name="threadClass">The thread class.</param>
		public ThreadHandler(IThreadClass threadClass)
		{
			_thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.Worker));

			this.OnJob += new HandlerForOnJob(threadClass.OnJob);
			this.OnFinish += new HandlerForOnFinish(threadClass.OnFinish);
			this.OnAbort += new HandlerForOnAbort(threadClass.OnAbort);
			this.OnTerminate += new HandlerForOnTerminate(threadClass.OnTerminate);
			this.OnException += new HandlerForOnException(threadClass.OnException);
		}

        /// <summary>
        /// Threads main function.
        /// </summary>
		private void Worker()
		{
			// Create the event-arg-object which will be used in each event.
			ThreadHandlerEventArgs evArgs = null;

			lock (this)
			{
				evArgs = new ThreadHandlerEventArgs(this);
			}

			// NOTE:
			// Locking is not necessary if you assume, that all the events are assigned before starting the thread.
			// ALSO even if locking is implemented, there can a PROBLEM still arise if the last delegate function is removed after the lock on the thread is released and before the event is raised.
			try
			{
				try
				{
					// MAIN FUNCTION
					System.Threading.Monitor.Enter(this);
					if (OnJob != null)
					{
						System.Threading.Monitor.Exit(this);
						OnJob(evArgs);
						System.Threading.Monitor.Enter(this);
					}
					System.Threading.Monitor.Exit(this);

					// NOTE: It is also possible to abort the finish event !!!
					System.Threading.Monitor.Enter(this);
					if (OnFinish != null)
					{
						System.Threading.Monitor.Exit(this);
						OnFinish(evArgs);
						System.Threading.Monitor.Enter(this);
					}
					System.Threading.Monitor.Exit(this);
				}
				catch (System.Threading.ThreadAbortException)
				{
					// WHEN ABORTING
					
					// NOTE: If you call ResetAbort the ThreadAbortException will not be re-throw-ed after each catch block... Code after block finally will also be executed...
					// System.Threading.Thread.ResetAbort();
					
					System.Threading.Monitor.Enter(this);
					if (OnAbort != null)
					{
						System.Threading.Monitor.Exit(this);
						OnAbort(evArgs);
						System.Threading.Monitor.Enter(this);
					}
					System.Threading.Monitor.Exit(this);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
				// Ignore this king of exception, it was already handled...but if not reseted, it is automatically re-thrown.
			}
			catch (Exception ex)
			{
				// HANDLING EXCEPTIONS
				System.Threading.Monitor.Enter(this);
				if (OnException != null)
				{
					ThreadHandlerExceptionArgs exArgs = new ThreadHandlerExceptionArgs(this, ex);
					
					System.Threading.Monitor.Exit(this);
					OnException(exArgs);
					System.Threading.Monitor.Enter(this);
				}			
				System.Threading.Monitor.Exit(this);
			}
			finally
			{
				// CLEAN-UP - NOTE: Exceptions are not handled here, so be careful!
				System.Threading.Monitor.Enter(this);
				if (OnTerminate != null)
				{
					System.Threading.Monitor.Exit(this);
					OnTerminate(evArgs);
					System.Threading.Monitor.Enter(this);
				}			
				System.Threading.Monitor.Exit(this);
			}

			// This line executes only, when ThreadAbortException is Reset-ed...
            System.Console.WriteLine("HALO...");
		}
	}
}
