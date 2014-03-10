using System;
using System.Collections.Generic;
using System.Text;
using Appleseed.Framework.Threading;
namespace Appleseed.Console.Tests
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class Program
    {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // ThreadTesting();
        }




        #region Thread Testing
        /// <summary>
        /// Main member variable...
        /// </summary>
        private static ThreadHandler th = null;

        /// <summary>
        /// Threads the testing.
        /// </summary>
        private static void ThreadTesting()
        {
            //
            // Example 1
            //
            th = new ThreadHandler();

            // registering and unregistering the event...
            th.OnJob += new HandlerForOnJob(th_OnJob);
            th.OnJob -= new HandlerForOnJob(th_OnJob);

            //
            // Example 2 - Testing...
            //
            th = new ThreadHandler(new MyThread(100));

            ThreadMenu();

            System.Console.WriteLine("Waiting for the worker thread to finish...");
            if ((th.Thread.ThreadState & System.Threading.ThreadState.Unstarted) == 0) th.Thread.Join();

            System.Console.WriteLine("FINALLY FINISHED:");
            System.Console.WriteLine("Thread state  = " + th.Thread.ThreadState.ToString());
            System.Console.WriteLine("IsAlive       = " + th.Thread.IsAlive.ToString());
            System.Console.WriteLine("IsAborted     = " + (((th.Thread.ThreadState & System.Threading.ThreadState.Aborted) > 0) ? true.ToString() : false.ToString()));

            System.Console.WriteLine("Press ENTER, ThreadState is = " + ((int)th.Thread.ThreadState).ToString());
            System.Console.ReadLine();
        }

        /// <summary>
        /// TH_s the on job.
        /// </summary>
        /// <param name="arg">The <see cref="T:Appleseed.Framework.Threading.ThreadHandlerEventArgs"/> instance containing the event data.</param>
        private static void th_OnJob(ThreadHandlerEventArgs arg)
        {
            // Example 1 - Not used...
        }

        /// <summary>
        /// Threads the menu.
        /// </summary>
        private static void ThreadMenu()
        {
            System.Console.WriteLine("Main thread id = " + System.Threading.Thread.CurrentThread.GetHashCode().ToString());

            string str = "";
            while (str != "x")
            {
                System.Console.WriteLine("\n---------------");
                System.Console.WriteLine("MENU:");
                System.Console.WriteLine("---------------");
                System.Console.WriteLine(" x - eXit menu");
                System.Console.WriteLine(" 1 - start");
                System.Console.WriteLine(" 0 - abort");
                System.Console.WriteLine(" 2 - suspend");
                System.Console.WriteLine(" s - state");
                System.Console.WriteLine("---------------\n");

                try
                {
                    str = System.Console.ReadLine();



                    switch (str)
                    {
                        case "1":
                            th.Start();
                            break;
                        case "0":
                            th.Abort();
                            break;
                        case "2":
                            //th.Thread.s
                            break;
                        case "s":
                            System.Console.WriteLine("Thread state  = " + th.Thread.ThreadState.ToString());
                            System.Console.WriteLine("IsAlive       = " + th.Thread.IsAlive.ToString());
                            System.Console.WriteLine("IsAborted     = " + (((th.Thread.ThreadState & System.Threading.ThreadState.Aborted) > 0) ? true.ToString() : false.ToString()));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("EXCEPTION = " + ex.Message + ", SOURCE = " + ex.StackTrace);
                }
                finally
                {
                }
            }
        } 
        #endregion
    }
}
