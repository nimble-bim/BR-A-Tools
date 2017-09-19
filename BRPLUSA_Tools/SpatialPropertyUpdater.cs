using Autodesk.Revit.DB;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA_Tools
{
    public class SpatialPropertyUpdater : IUpdater
    {
        static AddInId _appId;
        private static UpdaterId _updaterId;
        private static SpatialDatabaseWrapper _db;

        public SpatialPropertyUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("995E2C51-3CF1-4241-8D04-EFBF335FACB0"));
            _db = new SpatialDatabaseWrapper();
        }

        public void Execute(UpdaterData data)
        {
            UpdateSpatialProperties(data);
        }

        public string GetAdditionalInformation()
        {
            return "BR+A All Rights Reserved.";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.RoomsSpacesZones;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }

        public string GetUpdaterName()
        {
            return "Spatial Property Updater";
        }

        public void UpdateSpatialProperties(UpdaterData data)
        {
            var modified = data.GetModifiedElementIds();

            var doc = data.GetDocument();
            var spaceIds = modified.Select(x => doc.GetElement(x).UniqueId);

            if (!spaceIds.Any(IsCurrentlyTracked))
                return;

            var tracked = spaceIds.Select(IsCurrentlyTracked);
        }

        private static bool IsCurrentlyTracked(string id)
        {
            var element = _db.FindElement(id);

            return !(element is null);
        }
    }
}
