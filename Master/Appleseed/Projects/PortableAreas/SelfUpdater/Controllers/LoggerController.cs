using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using NuGet;
using SelfUpdater.Models;

namespace SelfUpdater.Controllers
{
    public class LoggerController : ILogger

        

    {

        private Dictionary<String, String> list;

        public LoggerController() {
            list = new Dictionary<string, string>();
        }

        public void Log(MessageLevel level, string message, params object[] args) {

            string msg = String.Empty;//level.ToString() + ": ";
            if (args != null) {
                for (int i = 0; i < args.Length; i++) {
                    message = message.Replace("{" + i + "}", args[i].ToString());
                }
            }
            msg += message;

            IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<SignalR.SelfUpdaterHub>();
            //hub.Clients.All.nuevoProcentaje(msg);


            // Lo escribo en un archivo para ver que anda

            try {
                var dir = HttpContext.Current.Request.MapPath("~/rb_logs") + "\\Nuget.txt";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(dir, true)) {
                    file.WriteLine(DateTime.Now.ToString());
                    file.WriteLine(msg);
                }
            }
            catch(Exception){}

            //if (!list.ContainsKey(msg)) {
            //    list.Add(msg, msg);
            //}


            // Creating message

            //if (msg.Contains("Starting") && !list.ContainsKey("install")) {
            //    list.Add("install", msg);
            //}
            if(message.Contains("Downloading")){
                int index = message.IndexOf("...");
                string mssg = message.Substring(0, index + 3);
                var pct = message.Substring(index + 3, message.Length - (index + 3));
                var model = new PercentajeModel {Msg = mssg, Pct = pct};
                hub.Clients.All.newPercentaje(model);

                //if (!list.ContainsKey("download")) {
                //    string mssg = message.Substring(0, index + 3);
                //    list.Add("download", mssg);
                //    list.Add("percent", message.Substring(index+2,message.Length - (index + 2)));

                //}
                //else {
                //    list.Remove("percent");
                //    list.Add("percent", message.Substring(index + 2, message.Length - (index + 2)));
                //}
            }
            else
            {
                hub.Clients.All.newMessage(msg);
            }
            //if (message.Contains("Successfully added")) {
            //    if (list.ContainsKey("Successful"))
            //    {
            //        list["Successful"] = message;
            //    }
            //    else
            //    {
            //        list.Add("Successful", message);
            //    }
                
            //}
            //if (message.Contains("Waiting to Reload Site")) {
            //    list.Add("Waiting", message);
            //}
            //var mensaje = String.Empty;
            //if (list.ContainsKey("install"))
            //    mensaje = "<li>" + list["install"] + "</li>";
            //if (list.ContainsKey("download") && list.ContainsKey("percent")) {
            //    mensaje += "<li>" + list["download"]  + list["percent"] + "%" + "</li>";
            //    //if (list["percent"].Contains("100"))
            //    //    mensaje += "<li> Installing package ... </li>";
            //}
            //if (list.ContainsKey("Successful")) {
            //    mensaje += "<li>" + list["Successful"] + "</li>";
            //}
            //if (list.ContainsKey("Waiting")) {
            //    mensaje += "<li>" + list["Waiting"] + "</li>";
            //}

            //HttpContext.Current.Application["NugetLogger"] = mensaje;
            
            
            
        }

        public string getLogs() {

            var msgs = String.Empty;            
            return msgs;
        }

        public FileConflictResolution ResolveFileConflict(string message)
        {
            try
            {
                var dir = HttpContext.Current.Request.MapPath("~/rb_logs") + "\\Nuget.txt";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(dir, true))
                {
                    file.WriteLine("ResolveFileConflict: " + DateTime.Now.ToString());
                    file.WriteLine(message);
                }
            }
            catch (Exception) { }

            return FileConflictResolution.OverwriteAll;
        }
    }
}
