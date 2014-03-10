using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfUpdater.Models
{
    public class InstallPackagesModel
    {
        public string icon { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string author { get; set; }
        public string source { get; set; }
        
    }

    public class NugetPackagesModel
    {
        public List<InstallPackagesModel> Install { get; set; }
        public List<InstallationState> Updates { get; set; }
    }


    public class PackageModel
    {

        public string Name { get; set; }
        public string Source { get; set; }
        public string Version { get; set; }
        public bool Install { get; set; }


    }

    public class PackageModalModel
    {
        public bool ShowModal { get; set; }

        public List<dynamic> Packages { get; set; }
    }

    public class PercentajeModel
    {
        public string Msg { get; set; }
        public string Pct { get; set; }
    }
}