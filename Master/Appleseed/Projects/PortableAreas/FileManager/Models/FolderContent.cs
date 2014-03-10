using System;
using System.Collections.Generic;

namespace FileManager.Models {
    public class FolderContent
    {

        public List<Files> Files;
        public List<Files> Folders;

        public FolderContent()
        {

            Files = new List<Files>();
            Folders = new List<Files>();

        }
    }

    public class Files
    {
        public string name;
        public string fullName;
        public string folder;

    } 
}