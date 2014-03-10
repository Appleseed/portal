using System;
using System.Threading;
using Appleseed.Framework.Threading;
namespace Appleseed.Console.Tests
{
    /// <summary>
    /// The working process...
    /// </summary>
    public class MyThread : IThreadClass
    {
        /// <summary>
        /// Internal variable
        /// </summary>
        private int _cnt;

        /// <summary>
        /// Constructor...
        /// </summary>
        /// <param name="counter"></param>
        public MyThread(int counter)
        {
            _cnt = counter;
        }

        /// <summary>
        /// This function MUST BE IMPLEMENTED...
        /// </summary>
        /// <param name="arg"></param>
        public override void OnJob(ThreadHandlerEventArgs arg)
        {
            // THE WORKING FUNCTION...
            while (_cnt > 0)
            {
                System.Console.WriteLine("Counter = " + _cnt.ToString());
                _cnt--;
                System.Threading.Thread.Sleep(500);
            }
        }

        // The following functions are only support functions. They must not be overrided.
        public override void OnFinish(ThreadHandlerEventArgs arg)
        {
            base.OnFinish(arg);
            System.Console.WriteLine("Finish");
        }

        public override void OnTerminate(ThreadHandlerEventArgs arg)
        {
            base.OnTerminate(arg);
            System.Console.WriteLine("Terminate");
        }

        public override void OnAbort(ThreadHandlerEventArgs arg)
        {
            base.OnAbort(arg);
            System.Console.WriteLine("Abort");
        }

        public override void OnException(ThreadHandlerExceptionArgs arg)
        {
            // I've commented it out, to avoid re-throwing of the catched exception...
            // base.OnException (arg);
            System.Console.WriteLine("HANDLED Exception...");
        }
    }

    
}
