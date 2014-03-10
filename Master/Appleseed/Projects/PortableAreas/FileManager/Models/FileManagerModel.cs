using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileManager.Models {
    public class FileManagerModel {

        public string PortalName {
            get; set; }

        public int ModuleId { get; set; }
        public bool ViewPermission { get; set; }
        public bool EditPermission { get; set; }
    }
}