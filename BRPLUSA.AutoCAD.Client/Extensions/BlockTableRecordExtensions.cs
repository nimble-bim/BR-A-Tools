using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;

namespace BRPLUSA.Client.AutoCAD.Extensions
{
    public static class BlockTableRecordExtensions
    {
        public static IEnumerable<ObjectId> GetAllObjectsFromBlockTableRecord(this BlockTableRecord record)
        {
            var list = new List<ObjectId>();

            foreach (var id in record)
            {
                list.Add(id);
            }

            return list;
        }
    }
}