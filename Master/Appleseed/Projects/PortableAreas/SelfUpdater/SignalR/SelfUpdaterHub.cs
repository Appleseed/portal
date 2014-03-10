using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SelfUpdater.SignalR
{
    public class SelfUpdaterHub : Hub
    {

        public void Send(string message)
        {
            // Call the addMessage method on all clients            
            Clients.All.addMessage(message);
        }

    }
}