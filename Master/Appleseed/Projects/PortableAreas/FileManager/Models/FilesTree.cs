using System.Collections.Generic;

namespace FileManager.Models {
    public class FilesTree {

        public string data;
        public FilesTreeAttribute attr;
        public string state;
        public List<FilesTree> children;
    }

    public class FilesTreeAttribute {
        public string id;
    }
}