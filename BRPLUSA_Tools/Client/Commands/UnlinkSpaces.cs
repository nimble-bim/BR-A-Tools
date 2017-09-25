using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BRPLUSA.Base;
using BRPLUSA.Data;

namespace BRPLUSA.Client.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UnlinkSpaces : BaseCommand
    {
        private IEnumerable<Space> _spaces;
        private SpatialDatabaseWrapper _db;

        protected override Result Work()
        {
            _spaces = SelectSpaces();
            return DisconnectSpaces();
        }

        private IEnumerable<Space> SelectSpaces()
        {
            using (var items = UiDocument.Selection)
            {
                var spaceRefs = items.PickObjects(ObjectType.Element, 
                    new RevitSelectionFilter<Space>(),
                    "Please select the spaces you'd like to connect.");

                var spaceElems = spaceRefs.Select(r => CurrentDocument.GetElement(r.ElementId));

                return spaceElems.Cast<Space>();
            }
        }

        private Result DisconnectSpaces()
        {
            var result = null == _spaces 
                ? Result.Cancelled 
                : StopTrackingSpaces(_spaces);

            return result;
        }

        private Result StopTrackingSpaces(IEnumerable<Space> spaces)
        {
            bool complete;

            using (_db = new SpatialDatabaseWrapper(CurrentDocument))
            {
                complete = _db.BreakElementRelationship(spaces);
            }

            return complete ? Result.Succeeded : Result.Failed;
        }
    }
}
