using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.PortableAreas;
using System.IO;
using System.Text;
using System.Configuration;

namespace Appleseed.Core.ApplicationBus
{
    public class DBScriptsHandler : MessageHandler<DBScriptsMessage>
    {
        public override void Handle(DBScriptsMessage message)
        {
            var version = message.LastVersion;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var currentVersion = ExecuteScripts.ExecuteHelper.GetLastDBVersion(connectionString, message.AreaName);

            //I get temp directory from filesystem
            string tempPath = System.IO.Path.GetTempPath();
            DirectoryInfo directory = new DirectoryInfo(tempPath);
            var scriptsDirectoryName = Guid.NewGuid().ToString();
            var scriptsDirectory = directory.CreateSubdirectory(scriptsDirectoryName);
            Encoding encoding = Encoding.GetEncoding("ISO-8859-1");

            //I keep al the valid scripts (newer than current version for this area)
            foreach (var script in message.Scripts.Where(s => s.Version.CompareTo(currentVersion) > 0))
            {
                var fileStream = AssemblyResourceManager.GetResourceStoreForArea(message.AreaName).GetResourceStream(script.Url);

                //Save all scripts into temporary folder
                using (StreamReader fileReader = new StreamReader(fileStream))
                {
                    var fileContent = fileReader.ReadToEnd();
                    var fileData = script.Version.Split(new char[] { '_' });
                    var scriptsDateFolder = scriptsDirectory.FullName + "/" + fileData[0];
                    if (!Directory.Exists(scriptsDateFolder))
                    {
                        scriptsDirectory.CreateSubdirectory(fileData[0]);
                    }

                    File.WriteAllText(scriptsDateFolder + "/" + fileData[1] + ".sql", fileContent, encoding);
                }
            }

            //Call execute scripts helper in order to update db for this area
            ExecuteScripts.ExecuteHelper.Execute(scriptsDirectory.FullName, connectionString, message.AreaName);
        }
    }
}