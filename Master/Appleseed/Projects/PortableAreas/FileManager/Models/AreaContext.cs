using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileManager.Models {
    public class AreaContext {
        private int _portalId;
        private Guid? _currentUserId;

        public AreaContext(int portalId, Guid? currentUserId) {
            _portalId = portalId;
            _currentUserId = currentUserId;
        }

        public int PortalId {
            get { return _portalId; }
        }

        public Guid? CurrentUserId {
            get { return _currentUserId; }
        }
    }
}