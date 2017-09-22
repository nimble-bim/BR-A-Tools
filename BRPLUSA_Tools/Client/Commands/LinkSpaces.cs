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
    public class LinkSpaces : BaseCommand
    {
        private IEnumerable<Space> _spaces;
        private SpatialDatabaseWrapper _db;

        protected override Result Work()
        {
            _spaces = SelectSpaces();
            return ConnectSpaces();
        }

        private IEnumerable<Space> SelectSpaces()
        {
            using (var items = UiDocument.Selection)
            {
                var spaceRefs = items.PickObjects(ObjectType.Element,
                    "Please select the spaces you'd like to connect.");

                var spaceElems = spaceRefs.Select(r => CurrentDocument.GetElement(r.ElementId));

                if (spaceElems.All(s => s is Space))
                {
                    if (spaceElems.Cast<Space>().Any(s => _db.IsInDatabase(s)))
                        RequestSelectionClarification();

                    return spaceElems.Cast<Space>();
                }

                TaskDialog.Show("Selection Error", "One of the items selected is not a space - please try again");
                UiDocument.Selection.Dispose();
                return SelectSpaces();
            }
        }

        private void RequestSelectionClarification()
        {
            TaskDialog.Show("Selection Error",
                "At least one of the spaces you've selected is already connected to other spaces - please select again.");

            throw new Exception("The user aborted the picked operation");
        }

        private Result ConnectSpaces()
        {
            var result = null == _spaces 
                ? Result.Cancelled 
                : TrackSpatialProperties(_spaces);

            return result;
        }

        private Result TrackSpatialProperties(IEnumerable<Space> spaces)
        {
            bool complete;

            using (_db = new SpatialDatabaseWrapper(CurrentDocument))
            {
                complete = _db.CreateElementRelationship(spaces);
            }

            return complete ? Result.Succeeded : Result.Failed;
        }
    }
}
