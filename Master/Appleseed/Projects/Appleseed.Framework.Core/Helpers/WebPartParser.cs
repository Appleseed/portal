namespace Appleseed.Framework.Helpers
{
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Xml.Serialization;

    /// <summary>
    /// WebPartParser
    ///   Added by Jakob Hansen.
    /// </summary>
    public class WebPartParser
    {
        #region Public Methods

        /// <summary>
        /// Loads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// A Appleseed.Framework.Helpers.WebPart value...
        /// </returns>
        public static WebPart Load(string fileName)
        {
            var context = HttpContext.Current;
            var partData = (WebPart)context.Cache[fileName];

            if (partData == null)
            {
                if (File.Exists(fileName))
                {
                    var reader = File.OpenText(fileName);

                    try
                    {
                        var serializer = new XmlSerializer(typeof(WebPart));
                        partData = (WebPart)serializer.Deserialize(reader);
                    }
                    finally
                    {
                        reader.Close();
                    }

                    context.Cache.Insert(fileName, partData, new CacheDependency(fileName));
                }
            }

            return partData;
        }

        #endregion
    }
}