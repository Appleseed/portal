using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;

namespace Appleseed.PortalTemplate.DTOs
{
    [Serializable]
    public class ModulesDTO
    {
        public int ModuleID
        {
            get;
            set;
        }

        public int TabID
        {
            get;
            set;
        }

        public int ModuleDefID
        {
            get;
            set;
        }

        public int ModuleOrder
        {
            get;
            set;
        }

        public string PaneName
        {
            get;
            set;
        }

        public string ModuleTitle
        {
            get;
            set;
        }

        public string AuthorizedEditRoles
        {
            get;
            set;
        }

        public string AuthorizedViewRoles
        {
            get;
            set;
        }

        public string AuthorizedAddRoles
        {
            get;
            set;
        }

        public string AuthorizedDeleteRoles
        {
            get;
            set;
        }

        public string AuthorizedPropertiesRoles
        {
            get;
            set;
        }

        public int CacheTime
        {
            get;
            set;
        }

        public Nullable<bool> ShowMobile
        {
            get;
            set;
        }

        public string AuthorizedPublishingRoles
        {
            get;
            set;
        }

        public Nullable<bool> NewVersion
        {
            get;
            set;
        }

        public Nullable<bool> SupportWorkflow
        {
            get;
            set;
        }

        public string AuthorizedApproveRoles
        {
            get;
            set;
        }
        
        public Nullable<byte> WorkflowState
        {
            get;
            set;
        }

        public Nullable<DateTime> LastModified
        {
            get;
            set;
        }

        public string LastEditor
        {
            get;
            set;
        }

        public Nullable<DateTime> StagingLastModified
        {
            get;
            set;
        }

        public string StagingLastEditor
        {
            get;
            set;
        }

        public Nullable<bool> SupportCollapsable
        {
            get;
            set;
        }

        public Nullable<bool> ShowEveryWhere
        {
            get;
            set;
        }

        public string AuthorizedMoveModuleRoles
        {
            get;
            set;
        }

        public string AuthorizedDeleteModuleRoles
        {
            get;
            set;
        }

        public List<ModuleSettingsDTO> ModuleSettings
        {
            get;
            set;
        }

        public ModuleDefinitionsDTO ModuleDefinitions
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
    }
}

