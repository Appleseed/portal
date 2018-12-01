using Appleseed.Framework;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Web.UI.WebControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.DesktopModules.CoreModules.LeavesArticleMosaic
{
    public class LeavesResults
    {
        public LeavesItems _embedded { get; set; }
        public int total { get; set; }
        public int pages { get; set; }
    }

    public class Pagination
    {
        public List<PaginationItem> Items { get; set; } = new List<PaginationItem>();
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string FirstPageUrl { get; set; }
        public string LastPageUrl { get; set; }
    }

    public class PaginationItem
    {
        public int PageIndex { get; set; }
        public bool Selected { get; set; }
        public string Url { get; set; }
    }

    public class LeavesItems
    {
        public List<LeavesItem> items { get; set; } = new List<LeavesItem>();
    }

    public class LeavesItem
    {
        public string title { get; set; }
        public string url { get; set; }
        public string content { get; set; }
        public string preview_picture { get; set; }
    }

    public class APIResultItem
    {
        public string Title { get; set; }
        public string PageUrl { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string ColumnCSS { get; set; }
    }

    public class APIResult
    {
        public List<APIResultItem> Items { get; set; } = new List<APIResultItem>();
    }

    public partial class LeavesArticleMosaicModule : PortalModuleControl
    {
        public LeavesArticleMosaicModule()
        {
            var group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            var groupOrderBase = (int)SettingItemGroup.MODULE_SPECIAL_SETTINGS;

            var tags = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 1,
                Group = group,
                EnglishName = "Tags",
                Description = "A string of tags by comma : IE \"Sitecore, Datastax\" or \"Datastax\" tags=sitecore,datastax"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_TAGS", tags);

            var Order = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 2,
                Group = group,
                EnglishName = "Order",
                Description = "asc or dsc (simply the string entered order=string)"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_ORDER", Order);

            var Sort = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 3,
                Group = group,
                EnglishName = "Sort",
                Description = "created ( simply the string sort=string)"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_SORT", Sort);

            var Limit = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 4,
                Group = group,
                EnglishName = "Limit ",
                Description = "5 ( simply a number as a string field. default to 5 limit = 5)"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_LIMIT", Limit);

            var Leaves_base_url = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 5,
                Group = group,
                EnglishName = "Leaves Base Url ",
                Description = "The base url to leaves : IE http://leaves.anant.us:82/api"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_LEAVES_BASE_URL", Leaves_base_url);

            var Leaves_entries_url = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 6,
                Group = group,
                EnglishName = "Leaves Entries Url",
                Description = "The base url to leaves : IE http://leaves.anant.us:82/api/entries?"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_LEAVES_ENTRIES_URL", Leaves_entries_url);

            var Leaves_access_key = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 7,
                Group = group,
                EnglishName = "The Access Key To Use",
                Description = "API key"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_LEAVES_ACCESS_KEY", Leaves_access_key);

            var columnClassName = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 8,
                Group = group,
                EnglishName = "Column CSS Class",
                Description = "Column CSS Class"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_COLUMN_CSS_CLASS", columnClassName);

            var DescCharLimit = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 9,
                Group = group,
                EnglishName = "Article Description Char Limit",
                Description = "Article Description Char Limit"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_CHAR_LIMIT", DescCharLimit);

            var titleCharLimit = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 9,
                Group = group,
                EnglishName = "Article Title Char Limit",
                Description = "Article Title Char Limit"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_TITLE_CHAR_LIMIT", titleCharLimit);

            var NoImageUrl = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 10,
                Group = group,
                EnglishName = "No Image Url",
                Description = "No Image Url"
            };

            this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_NO_IMAGE_URL", NoImageUrl);

            //var pageSizeLimit = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            //{
            //    Order = (int)groupOrderBase + 11,
            //    Group = group,
            //    EnglishName = "Page Size Limit",
            //    Description = "Page Size Limit"
            //};

            //this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_PAGE_SIZE_LIMIT", pageSizeLimit);

            //var enabledPaging = new SettingItem<bool, CheckBox>(new BaseDataType<bool, CheckBox>())
            //{
            //    Order = (int)groupOrderBase + 11,
            //    Group = group,
            //    EnglishName = "Enable Paggination",
            //    Description = "Enable Paggination"
            //};

            //this.BaseSettings.Add("LEAVES_ARTICLE_LISTING_ENABLED_PAGGING", enabledPaging);
        }
        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{5AB6843E-5A24-4D36-BB3D-0AE815DDB3B1}"); }
        }

        public int CurrentPageIndex
        {
            get
            {
                int pageIndex = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["pi"]))
                {
                    int.TryParse(Request.QueryString["pi"], out pageIndex);
                    if (pageIndex > 0)
                    {
                        pageIndex--;
                    }
                }

                return pageIndex;
            }
        }

        public APIResult APIResults { get; set; }

        public Pagination APIPagination { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadResults();
        }

        private void LoadResults()
        {
            #region Settings
            string tags = string.Empty;
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_TAGS") && this.Settings["LEAVES_ARTICLE_LISTING_TAGS"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_TAGS"].Value.ToString()))
            {
                tags = this.Settings["LEAVES_ARTICLE_LISTING_TAGS"].Value.ToString();
            }

            string Order = "desc";
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_ORDER") && this.Settings["LEAVES_ARTICLE_LISTING_ORDER"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_ORDER"].Value.ToString()))
            {
                Order = this.Settings["LEAVES_ARTICLE_LISTING_ORDER"].Value.ToString();
            }

            string Sort = "created";
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_SORT") && this.Settings["LEAVES_ARTICLE_LISTING_SORT"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_SORT"].Value.ToString()))
            {
                Sort = this.Settings["LEAVES_ARTICLE_LISTING_SORT"].Value.ToString();
            }

            string Limit = "5";
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_LIMIT") && this.Settings["LEAVES_ARTICLE_LISTING_LIMIT"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_LIMIT"].Value.ToString()))
            {
                Limit = this.Settings["LEAVES_ARTICLE_LISTING_LIMIT"].Value.ToString();
            }

            string Leaves_base_url = string.Empty;
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_LEAVES_BASE_URL") && this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_BASE_URL"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_BASE_URL"].Value.ToString()))
            {
                Leaves_base_url = this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_BASE_URL"].Value.ToString();
            }

            string Leaves_entries_url = string.Empty;
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_LEAVES_ENTRIES_URL") && this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_ENTRIES_URL"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_ENTRIES_URL"].Value.ToString()))
            {
                Leaves_entries_url = this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_ENTRIES_URL"].Value.ToString();
            }

            string Leaves_access_key = string.Empty;
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_LEAVES_ACCESS_KEY") && this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_ACCESS_KEY"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_ACCESS_KEY"].Value.ToString()))
            {
                Leaves_access_key = this.Settings["LEAVES_ARTICLE_LISTING_LEAVES_ACCESS_KEY"].Value.ToString();
            }

            string columnCSSClass = string.Empty;
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_COLUMN_CSS_CLASS") && this.Settings["LEAVES_ARTICLE_LISTING_COLUMN_CSS_CLASS"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_COLUMN_CSS_CLASS"].Value.ToString()))
            {
                columnCSSClass = this.Settings["LEAVES_ARTICLE_LISTING_COLUMN_CSS_CLASS"].Value.ToString();
            }

            int charLimit = 200;
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_CHAR_LIMIT") && this.Settings["LEAVES_ARTICLE_LISTING_CHAR_LIMIT"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_CHAR_LIMIT"].Value.ToString()))
            {
                int.TryParse(this.Settings["LEAVES_ARTICLE_LISTING_CHAR_LIMIT"].Value.ToString(), out charLimit);
                if (charLimit <= 0)
                {
                    charLimit = 200;
                }
            }

            int titlecharLimit = 100;
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_TITLE_CHAR_LIMIT") && this.Settings["LEAVES_ARTICLE_LISTING_TITLE_CHAR_LIMIT"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_TITLE_CHAR_LIMIT"].Value.ToString()))
            {
                int.TryParse(this.Settings["LEAVES_ARTICLE_LISTING_TITLE_CHAR_LIMIT"].Value.ToString(), out titlecharLimit);
                if (titlecharLimit <= 0)
                {
                    titlecharLimit = 100;
                }
            }

            //int pageSizeLimit = 10;
            //if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_PAGE_SIZE_LIMIT") && this.Settings["LEAVES_ARTICLE_LISTING_PAGE_SIZE_LIMIT"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_PAGE_SIZE_LIMIT"].Value.ToString()))
            //{
            //    int.TryParse(this.Settings["LEAVES_ARTICLE_LISTING_PAGE_SIZE_LIMIT"].Value.ToString(), out pageSizeLimit);
            //    if (pageSizeLimit <= 0)
            //    {
            //        pageSizeLimit = 10;
            //    }
            //}

            //bool enabledPaggination = false;
            //if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_ENABLED_PAGGING") && this.Settings["LEAVES_ARTICLE_LISTING_ENABLED_PAGGING"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_ENABLED_PAGGING"].Value.ToString()))
            //{
            //    enabledPaggination = Convert.ToBoolean(this.Settings["LEAVES_ARTICLE_LISTING_ENABLED_PAGGING"].Value.ToString());
            //}

            string noImageUrl = "/desktopModules/CoreModules/LeavesArticleMosaic/No_Image_Available.jpg";
            if (this.Settings.ContainsKey("LEAVES_ARTICLE_LISTING_NO_IMAGE_URL") && this.Settings["LEAVES_ARTICLE_LISTING_NO_IMAGE_URL"].Value != null && !string.IsNullOrEmpty(this.Settings["LEAVES_ARTICLE_LISTING_NO_IMAGE_URL"].Value.ToString()))
            {
                noImageUrl = this.Settings["LEAVES_ARTICLE_LISTING_NO_IMAGE_URL"].Value.ToString();
            }

            #endregion

            if (!string.IsNullOrEmpty(Leaves_entries_url) && !string.IsNullOrEmpty(Leaves_access_key) && !string.IsNullOrEmpty(tags))
            {
                try
                {
                    StringBuilder sbTabDtls = new StringBuilder();
                    string leavesAPI = string.Format("{5}access_token={0}&limit={1}&order={2}&page={6}&sort={3}&tags={4}", Leaves_access_key, Limit, Order, Sort, tags, Leaves_entries_url, (CurrentPageIndex + 1));

                    LeavesResults results = GetResults(leavesAPI);
                    APIResults = new APIResult();
                    APIPagination = new Pagination();

                    for (int i = 0; i <= results._embedded.items.Count - 1; i++)
                    {
                        var item = results._embedded.items[i];
                        string desc = StripHTML(item.content);
                        if (desc.Length > charLimit)
                        {
                            desc = desc.Substring(0, charLimit) + " ...";
                        }
                        if (string.IsNullOrEmpty(item.preview_picture))
                        {
                            item.preview_picture = noImageUrl;
                        }

                        string title = item.title;
                        if (title.Length > titlecharLimit)
                        {
                            title = title.Substring(0, titlecharLimit) + " ...";
                        }

                        this.APIResults.Items.Add(new APIResultItem() { Content = desc, ImageUrl = item.preview_picture, Title = title, PageUrl = item.url, ColumnCSS = columnCSSClass });
                    }

                    string pathQuery = Request.Url.PathAndQuery.Split('?').GetValue(0).ToString();
                    this.APIPagination.TotalPages = results.pages;
                    this.APIPagination.TotalItems = results.total;
                    this.APIPagination.FirstPageUrl = pathQuery;
                    this.APIPagination.LastPageUrl = pathQuery + "?pi=" + this.APIPagination.TotalPages;

                    for (int i = 1; i <= this.APIPagination.TotalPages; i++)
                    {

                        if (i - 1 == CurrentPageIndex)
                        {
                            this.APIPagination.Items.Add(new PaginationItem { PageIndex = i, Selected = true, Url = pathQuery + "?pi=" + i });
                        }
                        else
                        {
                            this.APIPagination.Items.Add(new PaginationItem { PageIndex = i, Selected = false, Url = pathQuery + "?pi=" + i });
                        }
                    }

                    apiResults.DataSource = this.APIResults.Items;
                    apiResults.DataBind();
                    apiResultsPages.DataSource = this.APIPagination.Items;
                    apiResultsPages.DataBind();
                }
                catch (Exception ex)
                {
                    ErrorHandler.Publish(LogLevel.Error, ex);
                }
            }
        }

        private string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        private LeavesResults GetResults(string leavesAPI)
        {
            LeavesResults results = new LeavesResults();
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(leavesAPI);
                httpWebRequest.Method = "GET";

                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    string message = String.Format("leaves API failed. Received HTTP {0}", httpWebResponse.StatusCode);
                    ErrorHandler.Publish(LogLevel.Error, message);
                }

                // Get the response.
                WebResponse response = httpWebRequest.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                var dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                results = JsonConvert.DeserializeObject<LeavesResults>(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, ex);
            }

            return results;
        }
    }
}