using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using BRPLUSA.Client.AutoCAD.Services;

namespace BRPLUSA.Client.AutoCAD.Commands
{
    public static class TestCommand
    {
        [CommandMethod("TRYTEST", CommandFlags.Session)]
        public static void TryCommand()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var layers = doc.GetLayers();

            doc.Editor.WriteMessage("Printing out the layers in this file");

            foreach (var layer in layers)
            {
                doc.Editor.WriteMessage("LAYER: ", layer.Name);
            }

            doc.Editor.WriteMessage("Complete.");
        }
    }
}
