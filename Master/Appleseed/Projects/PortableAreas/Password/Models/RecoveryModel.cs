using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Password.Models {
    public class RecoveryModel {

        public bool error;
        public string message;
        public Guid UserId;
        public Guid token;

    }
}