using System;
using System.Collections.Generic;
using System.Text;

namespace Content.API
{
    [Serializable]
    [Flags]
    public enum WorkFlowVersions
    {
        Draft = 1,
        WaitingForApproval = 2,
        Approved = 3,
        Post = 0,
        ApprovalDenied = 5,
        RecycleBin = 6
    }
}
