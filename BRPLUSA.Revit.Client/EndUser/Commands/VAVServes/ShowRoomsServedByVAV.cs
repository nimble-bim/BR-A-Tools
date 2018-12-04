using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;

namespace BRPLUSA.Revit.Client.EndUser.Commands.VAVServes
{
    //Create Systems for VAV Boxes
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class ShowRoomsServedByVAV : IExternalCommand
    {
        string log = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\log.txt";
        string spacesServedParamName = "Serves";
        string equipTagParamName = "BR+A HVAC Tag";
        string spaceServedByParamName = "ServedBy";

        public Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app;
            Document doc;
            UIDocument uiDoc;

            app = commandData.Application.Application;
            doc = commandData.Application.ActiveUIDocument.Document;
            uiDoc = commandData.Application.ActiveUIDocument;

            List<FamilyInstance> vavFamilyInstances;
            vavFamilyInstances = GetValidFamilyInstances(uiDoc, equipTagParamName, spacesServedParamName);
            if (vavFamilyInstances.Count == 0)
                return Result.Failed;

            OpenDuctMsgBox openDuctBox = new OpenDuctMsgBox();
            DialogResult readOpenDuctsResult = openDuctBox.ShowDialog();

            bool includeUnterminatedEndPoints = readOpenDuctsResult == DialogResult.Yes;

            using (Transaction tran1 = new Transaction(doc, "Microdesk.TraceVAVRoomsServed"))
            {
                try
                {
                    tran1.Start();
                    List<TracedSpace> allTracedSpaces = new List<TracedSpace>();
                    foreach (FamilyInstance fi in vavFamilyInstances)
                    {
                        List<TracedSpace> spacesForElement = SystemTracing.GetDownstreamRoomNames(fi, doc, includeUnterminatedEndPoints);
                        allTracedSpaces.AddRange(spacesForElement.Where(ts => ts.SpaceId != null));
                        string servedSpacesLabel = string.Join(",", spacesForElement.Select(ts => ts.SpaceName).Distinct().OrderBy(s => s));
                        Parameter servesParam = Util.GetParameter(fi, spacesServedParamName);
                        servesParam.Set(servedSpacesLabel);
                    }

                    var groups = allTracedSpaces.GroupBy(ts => ts.SpaceId);
                    bool servedByAttributeMissing = false;
                    foreach (IGrouping<int?, TracedSpace> group in groups)
                    {
                        int spaceId = (int)group.Key;
                        Element space = doc.GetElement(new ElementId(spaceId));
                        Parameter spaceParam = Util.GetParameter(space, spaceServedByParamName);
                        if (spaceParam == null || spaceParam.IsReadOnly == true)
                        {
                            servedByAttributeMissing = true;
                            continue;
                        }
                        List<int> equipmentElementIds = group.Select(ts => ts.EquipmentId).ToList();
                        List<string> equipmentTags = new List<string>();
                        foreach (int equipmentElementId in equipmentElementIds)
                        {
                            Element equipElem = doc.GetElement(new ElementId(equipmentElementId));
                            Parameter equipParam = Util.GetParameter(equipElem, equipTagParamName);
                            string equipTag = equipParam.AsString();
                            if (string.IsNullOrWhiteSpace(equipTag))
                                equipTag = "[BlankEquipTagValue]";
                            equipmentTags.Add(equipTag);
                        }
                        string spaceServedByLabel = string.Join(",", equipmentTags.Distinct().OrderBy(s => s));
                        spaceParam.Set(spaceServedByLabel);
                    }
                    tran1.Commit();
                    if (servedByAttributeMissing)
                        MessageBox.Show(string.Format("One or more spaces were missing the expected '{0}' attribute and were skipped.", spaceServedByParamName), "Served By Parameter Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception ex)
                {
                    tran1.RollBack();
                    throw ex;
                }
            }
            return Result.Succeeded;
        }

        private List<FamilyInstance> GetValidFamilyInstances(UIDocument uiDoc, string equipTagParamName, string spacesServedParamName)
        {
            Document doc = uiDoc.Document;
            List<FamilyInstance> familyInstances = new List<FamilyInstance>();
            List<FamilyInstance> validFamilyInstances = new List<FamilyInstance>();

            ElementId mechEquipCatId = doc.Settings.Categories.get_Item(BuiltInCategory.OST_MechanicalEquipment).Id;

            //Use the mechanical equipment in items which are selected
#if REVIT2016
            Selection sel = uiDoc.Selection;
            ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();
            foreach (ElementId id in selectedIds)
            {
                Element el = doc.GetElement(id);
                FamilyInstance fi = el as FamilyInstance;
                if (fi == null)
                    continue;

                if (el.Category.Id == mechEquipCatId)
                {
                    familyInstances.Add(fi);
                }
            }
#else
            var ids = uiDoc.Selection.GetElementIds();
            var elems = ids.Select(i => uiDoc.Document.GetElement(i));
            foreach(var elem in elems)
            {
                FamilyInstance fi = elem as FamilyInstance;
                if (fi == null)
                    continue;

                if (elem.Category.Id == mechEquipCatId)
                {
                    familyInstances.Add(fi);
                }
            }
#endif

            //Alert if no Mechanical Equipment Is Selected
            if (familyInstances.Count == 0)
            {
                TaskDialog.Show("SpacesServed", "There is no mechanical equipment in the selection.  Exiting.");
                return validFamilyInstances;
            }

            //Alert if Equipment does not have the required Parameter
            List<Family> validFams = new List<Family>();
            HashSet<int> validFamilyIds = new HashSet<int>();
            familyInstances.ForEach(fi =>
            {
                Family fam = fi.Symbol.Family;
                if (validFamilyIds.Contains(fam.Id.IntegerValue))
                    return;

                //  test for existence of writable instance 'serves' parameter
                ParameterSet paramSet = fi.Parameters;
                bool hasServesParameter = ParameterSetContainsWritableParameter(spacesServedParamName, paramSet);
                bool hasEquipTagParameter = ParameterSetContainsWritableParameter(equipTagParamName, paramSet);

                if (hasServesParameter && hasEquipTagParameter)
                    validFamilyIds.Add(fi.Symbol.Family.Id.IntegerValue);
                return;
            });

            if (validFamilyIds.Count == 0)
            {
                TaskDialog.Show("Required Parameters Not Found", String.Format("The required parameters '{0}' and '{1}' were not present\n on any of the elements in the selection.  Exiting.", spacesServedParamName, equipTagParamName));
                return validFamilyInstances;
            }

            validFamilyInstances = familyInstances.Where(fi => validFamilyIds.Contains(fi.Symbol.Family.Id.IntegerValue)).ToList();
            return validFamilyInstances;
        }

        private static bool ParameterSetContainsWritableParameter(string paramName, ParameterSet paramSet)
        {
            bool hasWritableParameter = false;
            foreach (Parameter p in paramSet)
            {
                if (string.Compare(p.Definition.Name, paramName, true) == 0)
                {
                    if (p.IsReadOnly == false)
                    {
                        hasWritableParameter = true;
                        break;
                    }
                }
            }

            return hasWritableParameter;
        }

        private List<Space> GetValidSpaces(UIDocument uiDoc, string spaceServedByParam)
        {
            Document doc = uiDoc.Document;
            var validSpaces = new FilteredElementCollector(doc).OfClass(typeof(Space))
                .Cast<Space>()
                .Where(s =>
                {
                    ParameterSet paramSet = s.Parameters;
                    bool hasRequiredParameter = ParameterSetContainsWritableParameter(spaceServedByParam, paramSet);
                    if (hasRequiredParameter)
                        return true;
                    else
                        return false;
                })
                .ToList();
            return validSpaces;
        }
    }
}
