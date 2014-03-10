using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Appleseed.Console.Tests
{
    /// <summary>
    /// Demonstrates how to create a singleton
    /// </summary>
    class LoadBalancer
    {
        private static LoadBalancer instance;
        private ArrayList servers = new ArrayList();

        private Random random = new Random();

        // Lock synchronization object 
        private static object syncLock = new object();

        // Constructor (protected) 
        protected LoadBalancer()
        {
            // List of available servers 
            servers.Add("ServerI");
            servers.Add("ServerII");
            servers.Add("ServerIII");
            servers.Add("ServerIV");
            servers.Add("ServerV");
        }

        public static LoadBalancer GetLoadBalancer()
        {
            // Support multithreaded applications through 
            // 'Double checked locking' pattern which (once 
            // the instance exists) avoids locking each 
            // time the method is invoked 
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new LoadBalancer();
                    }
                }
            }

            return instance;
        }

        // Simple, but effective random load balancer 

        public string Server
        {
            get
            {
                int r = random.Next(servers.Count);
                return servers[r].ToString();
            }
        }
    }

}
