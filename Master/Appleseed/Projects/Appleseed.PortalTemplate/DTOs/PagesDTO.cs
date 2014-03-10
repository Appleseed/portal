using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Appleseed.PortalTemplate.DTOs
{
    [Serializable]
    public class PagesDTO
    {
        public int PageID
        {
            get;
            set;
        }

        public int PageOrder
        {
            get;
            set;
        }

        public int PortalID
        {
            get;
            set;
        }

        public string PageName
        {
            get;
            set;
        }

        public string MobilePageName
        {
            get;
            set;
        }

        public string AuthorizedRoles
        {
            get;
            set;
        }

        public bool ShowMobile
        {
            get;
            set;
        }

        public Nullable<int> PageLayout
        {
            get;
            set;
        }

        public string PageDescription
        {
            get;
            set;
        }

        public List<ModulesDTO> Modules
        {
            get;
            set;
        }


        public PagesDTO ParentPage
        {
            get;
            set;
        }

        public List<TabSettingsDTO> TabSettings
        {
            get;
            set;
        }

    }
}
