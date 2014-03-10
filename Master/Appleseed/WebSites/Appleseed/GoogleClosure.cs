// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleClosure.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   A C# wrapper around the Google Closure Compiler web service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Xml;

using Appleseed.Framework;

/// <summary>
/// A C# wrapper around the Google Closure Compiler web service.
/// </summary>
/// <remarks>
/// </remarks>
public class GoogleClosure
{
    #region Constants and Fields

    /// <summary>
    /// The API endpoint.
    /// </summary>
    private const string ApiEndpoint = "http://closure-compiler.appspot.com/compile";

    /// <summary>
    /// The post data.
    /// </summary>
    private const string PostData =
        "{0}output_format=xml&output_info=compiled_code&compilation_level=SIMPLE_OPTIMIZATIONS";

    #endregion

    #region Public Methods

    /// <summary>
    /// Calls the API.
    /// </summary>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <returns>
    /// The call API.
    /// </returns>
    /// <remarks>
    /// </remarks>
    public static string CallApi(string source)
    {
        using (var client = new WebClient())
        {
            client.Headers.Add("content-type", "application/x-www-form-urlencoded");
            var data = string.Format(PostData, source);
            var result = client.UploadString(ApiEndpoint, data);
            ErrorHandler.Publish(LogLevel.Debug, result);

            var doc = new XmlDocument();
            doc.LoadXml(result);
            var node = doc.SelectSingleNode("//compiledCode");
            return node != null ? node.InnerText : string.Empty;
        }
    }

    #endregion
}