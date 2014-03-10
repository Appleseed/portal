using System.Linq;

namespace System.Web.WebPages.Administration.PackageManager
{
    using NuGet;
    using System;
    using System.IO;
    using System.Runtime.Versioning;

    public class WebProjectSystem : PhysicalFileSystem, IProjectSystem
    {
        private const string BinDir = "bin";

        public WebProjectSystem(string root) : base(root)
        {
        }

        public void AddReference(string referencePath, Stream stream)
        {
            string fileName = Path.GetFileName(referencePath);
            string fullPath = this.GetFullPath(this.GetReferencePath(fileName));
            this.AddFile(fullPath, stream);
        }

        public void AddFrameworkReference(string name)
        {
            
        }


        public dynamic GetPropertyValue(string propertyName)
        {
            if ((propertyName != null) && propertyName.Equals("RootNamespace", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }
            return null;
        }

        protected virtual string GetReferencePath(string name)
        {
            return Path.Combine("bin", name);
        }

        public bool IsSupportedFile(string path)
        {
            return (!path.StartsWith("tools", StringComparison.OrdinalIgnoreCase) && !Path.GetFileName(path).Equals("app.config", StringComparison.OrdinalIgnoreCase));
        }

        public bool ReferenceExists(string name)
        {
            string referencePath = this.GetReferencePath(name);
            return this.FileExists(referencePath);
        }

        public void RemoveReference(string name)
        {
            this.DeleteFile(this.GetReferencePath(name));
            if (!GetFiles("bin", true).Any<string>())
            {
                this.DeleteDirectory("bin");
            }
        }

        public string ProjectName
        {
            get
            {
                return base.Root;
            }
        }

        public bool IsBindingRedirectSupported { get; private set; }


        public void RemoveImport(string targetPath)
        {
           
        }

        public bool FileExistsInProject(string path)
        {
            return false;
        }

        public FrameworkName TargetFramework
        {
            get
            {
                return VersionUtility.DefaultTargetFramework;
            }
        }

        

        public string ResolvePath(string path)
        {
            return path;
        }

        public void AddImport(string targetPath, ProjectImportLocation location)
        {
            
        }
    }
}

