/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Defintions of some out of box types
-- =============================================
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Content.API;

namespace Content.API.Types
{
    [Serializable]
    public class Basic : ItemType
    {
        private Guid _typeGUID = new Guid("{5E97E7D6-F9AE-45c2-A88D-5665E4A28103}");
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Basic"/> class.
        /// </summary>
        /// <param name="typeGuid">The type GUID.</param>
        public Basic(Guid typeGuid) : base(typeGuid)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Basic"/> class.
        /// </summary>
        public Basic() : base()
        {
            base.TypeGUID = _typeGUID;
        }

        /// <summary>
        /// Fetches the type settings.
        /// </summary>
        protected virtual void FetchTypeSettings()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Some types want to define custom type attributes / settings
    /// inheriting from this class provides some functionality so that your type can add
    /// custom settings of type Setting
    /// </summary>
    [Serializable]
    public class CustomSettings : Basic
    {
        /// <summary>
        /// Some types want to define custom type attributes / settings
        /// inheriting from this class provides some functionality so that your type can add
        /// custom settings of type Setting
        /// </summary>
        private List<Setting> _defaultsettings = new List<Setting>();

        /// <summary>
        /// Gets the default custom settings.
        /// </summary>
        /// <value>The default custom settings.</value>
        protected List<Setting> GetCustomSettings()
        {
                if (_defaultsettings == null)
                    _defaultsettings = new List<Setting>();

                return _defaultsettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CustomSettings"/> class.
        /// </summary>
        protected CustomSettings() : base()
        {
            base.TypeGUID = new Guid("{5E97E7D6-F9AE-45c2-A88D-5665E4A28103}");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CustomSettings"/> class.
        /// </summary>
        /// <param name="typeGuid">The type GUID.</param>
        protected CustomSettings(Guid typeGuid) : base()
        {
            base.TypeGUID = typeGuid;
        }

        /// <summary>
        /// Fetches the item settings.
        /// </summary>
        protected override void FetchTypeSettings()
        {
            base.FetchTypeSettings();

            for (int i = 0; i <= _defaultsettings.Count - 1; i++)
            {
                if (!TypeSettings.ContainsKey(_defaultsettings[i].Key))
                    TypeSettings.Add(_defaultsettings[i].Key, _defaultsettings[i]);
            }
        }

        /// <summary>
        /// Adds the custom setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        protected void AddCustomSetting(Setting setting)
        {
            if (!SettingKeys.Contains(setting.Key))
            {
                SettingKeys.Add(setting.Key);
                TypeSettings.Add(setting);
            }
        }


    }


    [Serializable]
    public class Link : CustomSettings
    {
        private string[] _defaultsettingkeys;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Link"/> class.
        /// </summary>
        /// <value>The default custom settings.</value>
        public Link() : base()
        {
            _defaultsettingkeys = new string[5];
            _defaultsettingkeys[0] = "href";
            _defaultsettingkeys[1] = "target";
            _defaultsettingkeys[2] = "linkImage";
            _defaultsettingkeys[3] = "tooltip";
            _defaultsettingkeys[4] = "onclick";


            Setting st = new Setting();
            st.Key = _defaultsettingkeys[0];
            st.DataType = "System.String";
            st.Value = "";
            AddCustomSetting(st);

            st = new Setting();
            st.Key = _defaultsettingkeys[1];
            st.DataType = "System.String";
            st.Value = "";
            AddCustomSetting(st);

            st = new Setting();
            st.Key = _defaultsettingkeys[2];
            st.DataType = "System.String";
            st.Value = "";
            AddCustomSetting(st);

            st = new Setting();
            st.Key = _defaultsettingkeys[3];
            st.DataType = "System.String";
            st.Value = "";
            AddCustomSetting(st);

            st = new Setting();
            st.Key = _defaultsettingkeys[4];
            st.DataType = "System.String";
            st.Value = "";
            AddCustomSetting(st);

            base.TypeGUID = new Guid("{5E97E7D6-F9AE-45c2-A88D-5665E4A28103}");
        }

        /// <summary>
        /// Fetches the item settings.
        /// </summary>
        protected override void FetchTypeSettings()
        {
            base.FetchTypeSettings();
        }
        
        #region Public Properties
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        public string Href
        {
            get {
                if (TypeSettings.ContainsKey(_defaultsettingkeys[0]))
                    return TypeSettings[_defaultsettingkeys[0]].Value.ToString();  

                return string.Empty;
            }
            set {
                Setting s = TypeSettings[_defaultsettingkeys[0]];
                s.Value = value;
                TypeSettings.Update(_defaultsettingkeys[0], s);
                TypeSettings.IsDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public string Target
        {
            get {
                if (TypeSettings.ContainsKey(_defaultsettingkeys[1]))
                    return TypeSettings[_defaultsettingkeys[1]].Value.ToString();  

                return string.Empty;
            }
            set {
                Setting s = TypeSettings[_defaultsettingkeys[1]];
                s.Value = value;
                TypeSettings.Update(_defaultsettingkeys[1], s);
                TypeSettings.IsDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the image SRC.
        /// </summary>
        /// <value>The image SRC.</value>
        public string ImageSrc
        {
            get {
                if (TypeSettings.ContainsKey(_defaultsettingkeys[2]))
                    return TypeSettings[_defaultsettingkeys[2]].Value.ToString();  

                return string.Empty;
            }
            set {
                Setting s = TypeSettings[_defaultsettingkeys[2]];
                s.Value = value;
                TypeSettings.Update(_defaultsettingkeys[2], s);
                TypeSettings.IsDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>The tooltip.</value>
        public string Tooltip
        {
            get {
                if (TypeSettings.ContainsKey(_defaultsettingkeys[3]))
                    return TypeSettings[_defaultsettingkeys[3]].Value.ToString();  

                return string.Empty;
            }
            set {
                Setting s = TypeSettings[_defaultsettingkeys[3]];
                s.Value = value;
                TypeSettings.Update(_defaultsettingkeys[3], s);
                TypeSettings.IsDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the on click.
        /// </summary>
        /// <value>The on click.</value>
        public string OnClick
        {
            get {
                if (TypeSettings.ContainsKey(_defaultsettingkeys[4]))
                    return TypeSettings[_defaultsettingkeys[4]].Value.ToString();  

                return string.Empty;
            }
            set {
                Setting s = TypeSettings[_defaultsettingkeys[4]];
                s.Value = value;
                TypeSettings.Update(_defaultsettingkeys[4], s);
                TypeSettings.IsDirty = true;
            }
        }
        #endregion
    }
    [Serializable]
    public class HtmlText : CustomSettings
    {
        [Flags]
        public enum TextFormat : int
        {
            PlainText = 0,
            HTML = 1,
            XHTML = 2,
            XML = 3
        }

        private string[] _defaultsettingkeys = new string[5];

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HtmlText"/> class.
        /// </summary>
        /// <value>The default custom settings.</value>
        public HtmlText()
            : base()
        {
            Setting st = new Setting();
            st.Key = "format";
            st.DataType = "System.String";
            st.Value = "html";
            AddCustomSetting(st);

            st = new Setting();
            st.Key = "enableSyndication";
            st.DataType = "System.Boolean";
            st.Value = "true";
            AddCustomSetting(st);

            base.TypeGUID = new Guid("{5E97E7D6-F9AE-45c2-A88D-5665E4A28103}");
            FetchTypeSettings();
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public string Format
        {
            get { return "";  }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable syndication].
        /// </summary>
        /// <value><c>true</c> if [enable syndication]; otherwise, <c>false</c>.</value>
        public bool EnableSyndication
        {
            get { return true; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Fetches the item settings.
        /// </summary>
        protected override void FetchTypeSettings()
        {
            base.FetchTypeSettings();
        }
    }
    public class Announcment : HtmlText
    {

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>The expiration date.</value>
        public DateTime ExpirationDate
        {
            get { return DateTime.MaxValue; }
            set { throw new NotImplementedException(); }
        }
    }
    public class ArticlePage : HtmlText
    {

    }
    public class BlogEntry : HtmlText
    {
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [XmlIgnore]
        public Comments.ItemComments Feedback
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>The rating.</value>
        [XmlIgnore]
        public Ratings.ItemRatings Rating
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }
    }
    public class File : CustomSettings
    {

        /// <summary>
        /// Gets or sets the type of the MIME.
        /// </summary>
        /// <value>The type of the MIME.</value>
        public string MimeType
        {
            get { return ""; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return ""; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Double FileSize
        {
            get { return 0; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>The extension.</value>
        [XmlIgnore]
        public string Extension
        {
            get { return ""; }
            set { throw new NotImplementedException(); }
        }
    }
    public class Image : File
    {

    }
    public class Flash : File
    {

    }
    public class TextDocument : File
    {
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        private string Encoding
        {
            get { return ""; }
            set { throw new NotImplementedException(); }
        }
    }
    public class MediaFile : File
    {
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        private string Encoding
        {
            get { return ""; }
            set { throw new NotImplementedException(); }
        }
    }
    public class OfficeDocument : File
    {
        /// <summary>
        /// Gets or sets the office version.
        /// </summary>
        /// <value>The office version.</value>
        public Single OfficeVersion
        {
            get { return 0; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company
        {
            get { return ""; }
            set { throw new NotImplementedException(); }
        }
    }
    public class WordDocument : OfficeDocument
    {

    }
    public class ExcelDocument : OfficeDocument
    {

    }
}
