using System;
using System.Xml.Linq;

namespace FileManager.Test {
    public class Settings {

        public string BaseUrl { get; private set; }
        public string FirefoxBinaryPath { get; set; }


        private static Settings _settings;
        public static Settings CurrentSettings {
            get {
                if (_settings == null) {
                    LoadSettings("TestRun.config");
                }
                return _settings;
            }
        }

        private Settings(string url) {
            BaseUrl = url;
        }

        private static void LoadSettings(String file) {
            var settingsFile = XElement.Load(file);
            var xElement = settingsFile.Element("URL");
            if (xElement == null) throw new Exception(string.Format("Missing {0} file", file));
            var url = xElement.Value;
            _settings = new Settings(url);

            //_settings.Defaults = new DefaultValues.DefaultValues();
            //_settings.Defaults.Album = new Album() {
            //    Name = settingsFile.Element("album").Value
            //};
            //_settings.Defaults.Album2 = new Album() {
            //    Name = settingsFile.Element("album2").Value
            //};
            //_settings.Defaults.Genre = new Genre() {
            //    Name = settingsFile.Element("genre").Value
            //};

            //_settings.Defaults.User = settingsFile.Descendants("user").Select(u => new User() {
            //    Username = u.Element("username").Value,
            //    Password = u.Element("password").Value,
            //    FirstName = u.Element("firstName").Value,
            //    LastName = u.Element("lastName").Value,
            //    Address = u.Element("address").Value,
            //    City = u.Element("city").Value,
            //    State = u.Element("state").Value,
            //    PostalCode = u.Element("postalCode").Value,
            //    Country = u.Element("country").Value,
            //    PhoneNumber = u.Element("phoneNumber").Value,
            //    EmailAddress = u.Element("emailAddress").Value
            //}).FirstOrDefault();
        }
    }
}
