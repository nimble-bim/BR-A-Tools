using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BRPLUSA.Revit.Base;
using BRPLUSA.Revit.Data;

namespace BRPLUSA.Revit.Client.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UnlinkSpaces : BaseCommand
    {
        private IEnumerable<Space> _spaces;
        private SpatialDatabaseWrapper _db;

        protected override Result Work()
        {
            Result disconnected;
            using (_db = new SpatialDatabaseWrapper(CurrentDocument))
            {
                _spaces = SelectSpaces();
                disconnected = DisconnectSpaces();
            }

            return disconnected;
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
            var complete = _db.BreakElementRelationship(spaces);

            return complete 
                ? Result.Succeeded 
                : Result.Failed;
        }
    }
}
