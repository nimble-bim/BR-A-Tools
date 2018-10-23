using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Core.Extensions
{
    public static class ViewScheduleExtensions
    {
        public static bool HasParameter(this ViewSchedule schedule, Parameter parameter)
        {
            var bodyData = schedule.GetTableData().GetSectionData(SectionType.Body);
            var count = bodyData.NumberOfColumns;

            for (var i = 0; i < count; i++)
            {
                var field = schedule.Definition.GetField(i);
                var id = field.ParameterId;

                if (id == parameter.Id)
                    return true;
            }

            return false;
        }
    }
}
