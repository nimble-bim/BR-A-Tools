using Autodesk.Revit.DB;
using BRPLUSA.Revit.Core.Exceptions;

namespace BRPLUSA.Revit.Services.Factories
{
    public class RevitScheduleFactory
    {
        public static ViewSchedule Create(Document doc, BuiltInCategory elementType, string schName)
        {
            ViewSchedule schedule = null;
            var canBeModified = doc.IsModifiable;
            var isReadonly = doc.IsReadOnly;

            if(!canBeModified && isReadonly)
                throw new CancellableException("The document has an open transaction somewhere");

            using (var trans = new Transaction(doc, $"Creating new {elementType.GetType().Name} schedule"))
            {
                var opts = trans.GetFailureHandlingOptions();
                opts.SetDelayedMiniWarnings(true);

                if (!trans.HasStarted())
                    trans.Start();

                schedule = ViewSchedule.CreateSchedule(doc, new ElementId(elementType), ElementId.InvalidElementId);
                schedule.Name = schName;

                trans.Commit(opts);
            }

            return schedule;
        }
    }
}
