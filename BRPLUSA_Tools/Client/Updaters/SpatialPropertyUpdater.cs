using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using BRPLUSA.Base;
using BRPLUSA.Data;

namespace BRPLUSA.Client.Updaters
{
    public class SpatialPropertyUpdater : BaseRegisterableUpdater
    {
        private static SpatialDatabaseWrapper _db;

        public SpatialPropertyUpdater(UIControlledApplication app)
        {
            _app = app;
            _appId = _app.ActiveAddInId;
            _updaterId = new UpdaterId(_appId, new Guid("995E2C51-3CF1-4241-8D04-EFBF335FACB0"));
        }

        public override void Execute(UpdaterData data)
        {
            _db = new SpatialDatabaseWrapper(_doc);
            CheckSpatialProperties(data);
        }

        public override string GetAdditionalInformation()
        {
            return "BR+A All Rights Reserved.";
        }

        public override ChangePriority GetChangePriority()
        {
            return ChangePriority.RoomsSpacesZones;
        }

        public override UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }

        public override string GetUpdaterName()
        {
            return "Spatial Property Updater";
        }

        public void CheckSpatialProperties(UpdaterData data)
        {
            try
            {
                var spaces = GetChangedSpaces(data);

                var tracked = spaces.Where(IsCurrentlyTracked);
                var needsUpdate = tracked.Where(NeedsUpdate).ToArray();

                if (needsUpdate.Length < 1)
                    return;

                UpdateSpace(needsUpdate);
                TaskDialog.Show("Update Complete", $"Updated {needsUpdate.Length} linked spaces");
            }

            catch (Exception e)
            {
                TaskDialog.Show("Error Checking Spatial Properties",
                    "There was an error checking the properties of a changed space - disable this updater when prompted and notify the CADD department");
            }
        }

        private IEnumerable<Space> GetChangedSpaces(UpdaterData data)
        {
            var modified = data.GetModifiedElementIds();

            var doc = data.GetDocument();
            return modified.Select(x => doc.GetElement(x)).Cast<Space>();
        }

        private static bool IsCurrentlyTracked(Space space)
        {
            return _db.IsCurrentlyTracked(space);
        }

        private static bool NeedsUpdate(Space space)
        {
            return _db.NeedsUpdate(space);
        }

        private void UpdateSpace(Space space)
        {
            _db.UpdateElement(space);
        }

        private void UpdateSpace(IEnumerable<Space> spaces)
        {
            foreach (var s in spaces)
            {
                UpdateSpace(s);
            }
        }

        public override void Register(Document doc)
        {
            base.Register(doc);
            UpdaterRegistry.AddTrigger(GetUpdaterId(), _doc, new SpaceFilter(), Element.GetChangeTypeAny());
        }
    }
}
