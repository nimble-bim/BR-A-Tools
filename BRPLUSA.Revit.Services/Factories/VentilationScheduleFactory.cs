using System;
using System.Linq;
using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Services.Factories
{
    public class VentilationScheduleFactory
    {
        private static string ScheduleName
        {
            get { return "Ventilation Schedule"; }
        }

        public static ViewSchedule CreateOrGetVentilationSchedule(Document doc)
        {
            var schedule = GetVentilationSchedule(doc) ?? RevitScheduleFactory.Create(doc, BuiltInCategory.OST_MEPSpaces, ScheduleName);

            return schedule;
        }

        public static ViewSchedule GetVentilationSchedule(Document doc)
        {
            try
            {
                var schedule = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSchedule))
                    .Cast<ViewSchedule>()
                    .FirstOrDefault(s => s.Name == ScheduleName);

                return schedule;
            }

            catch (Exception e)
            {
                return null;
            }
        }
    }
}
