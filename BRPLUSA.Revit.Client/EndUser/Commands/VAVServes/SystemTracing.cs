using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using RevitDomain = Autodesk.Revit.DB.Domain;

namespace BRPLUSA.Revit.Client.EndUser.Commands.VAVServes
{
    class SystemTracing
    {
        public static List<TracedSpace> GetDownstreamRoomNames(FamilyInstance vavFamilyInstance, Document m_document, bool includeUnterminated)
        {
            List<TracedSpace> tracedSpaces = new List<TracedSpace>();
            Element terminalEquipElement = (Element)vavFamilyInstance;
            //string notTerminatedInRoomString = "ClosedDuctNotInSpace";

            //List<string> servedRoomNames = new List<string>();

            //  get connectors
            Connector[] familyInstanceConnectors = Util.GetConnectorsFromFamilyInstance(terminalEquipElement);

            List<Connector> downstreamConnectors = GetDownstreamConnectors(familyInstanceConnectors);

            if (downstreamConnectors.Count == 0)
                return tracedSpaces;


            foreach (Connector terminalEquipConnector in downstreamConnectors)   // most terminal equipment would have just one downstream connector
            {
                //  get system for this connector
                MEPSystem connectorMEPSystem = null;
                connectorMEPSystem = GetOrCreateMEPSystemOnConnector(m_document, terminalEquipConnector);

                List<ElementId> elementIds = new List<ElementId>();
                elementIds.Add(terminalEquipElement.Id);

                MechanicalSystem connectorMechSystem = connectorMEPSystem as MechanicalSystem;

                ElementSet terminalElements = connectorMEPSystem.Elements;  // terminal element = elements at the end of the end of the branches; can return duct or family instances

                //Create a string of all Room Names & Numbers
                //Here we gather all elements connected to the system
                foreach (Element terminalElement in terminalElements)
                {
                    //  terminalElements contains the original equipment element.  need to ignore that when iterating because we just want the termination points
                    if (terminalElement.Id == vavFamilyInstance.Id)
                        continue;

                    FamilyInstance famInst = terminalElement as FamilyInstance;
                    MEPCurve ductInst = terminalElement as Duct;
                    MEPCurve flexDuctInst = terminalElement as FlexDuct;
                    MEPCurve ductEndSegment = ductInst ?? flexDuctInst;

                    //For each air terminal and equipment, identify the space it belongs to and add that as a parameter
                    //to the air terminal.  That way, the information is stored in case we want to unload the link.

                    if (famInst != null && famInst.Category.Id == m_document.Settings.Categories.get_Item(BuiltInCategory.OST_DuctFitting).Id)
                        continue;

                    TracedSpace tracedSpace = null;

                    if (famInst != null)
                    {
                        tracedSpace = GetServedSpaceForFI(famInst, terminalEquipElement.Id.IntegerValue);
                        tracedSpaces.Add(tracedSpace);
                    }
                    else if (ductEndSegment != null)
                    {
                        if (includeUnterminated)
                        {
                            tracedSpace = GetServedSpaceForOpenDuctEnd(m_document, ductEndSegment, terminalEquipElement.Id.IntegerValue);
                            tracedSpaces.Add(tracedSpace);
                        }
                    }
                }
            }

            return tracedSpaces;
        }

        private static TracedSpace GetServedSpaceForOpenDuctEnd(Document m_document, MEPCurve ductInst, int equipId)
        {
            TracedSpace ts = null;
            string servedSpaceName = null;
            foreach (Connector dCon in ductInst.ConnectorManager.Connectors)
            {
                foreach (Connector rCon in dCon.AllRefs)
                {
                    if (rCon.ConnectorType == ConnectorType.Logical)
                    {
                        Space spc = m_document.GetSpaceAtPoint(dCon.Origin);
                        if (spc != null)
                        {
                            Parameter nameParam = Util.GetParameter(spc, "Name");
                            Parameter numberParam = Util.GetParameter(spc, "Number");

                            string nameParamValue = nameParam.AsString();
                            string numberParamValue = numberParam.AsString();
                            servedSpaceName = string.Format("{0} {1}", nameParamValue, numberParamValue);
                            ts = new TracedSpace(equipId, servedSpaceName, spc.Id.IntegerValue);
                        }
                        else
                            ts = new TracedSpace(equipId, "OpenDuctNotInSpace", null);
                    }
                }
            }
            return ts;
        }

        private static TracedSpace GetServedSpaceForFI(FamilyInstance famInst, int equipId)
        {
            TracedSpace ts = null;
            string servedSpaceName = null;
            //  try to get space name
            if (famInst.Space != null)
            {
                Space spc = famInst.Space;
                Parameter nameParam = Util.GetParameter(spc, "Name");
                Parameter numberParam = Util.GetParameter(spc, "Number");

                string nameParamValue = nameParam.AsString();
                string numberParamValue = numberParam.AsString();

                if (!string.IsNullOrWhiteSpace(nameParamValue))
                    servedSpaceName = string.Format("{0} {1}", nameParamValue, numberParamValue);
                else
                    servedSpaceName = "UnnamedRoom";
                ts = new TracedSpace(equipId, servedSpaceName, spc.Id.IntegerValue);
            }
            else
            {
                ts = new TracedSpace(equipId, "ClosedDuctNotInSpace", null);
            }

            return ts;
        }

        private static MEPSystem GetOrCreateMEPSystemOnConnector(Document m_document, Connector connector)
        {
            if (connector.MEPSystem == null)
            {
                //  if there is no system, let Revit automatically create it.  system will start from the connector and continue to the air terminal                 
                using (SubTransaction subTransaction = new SubTransaction(m_document))
                {
                    subTransaction.Start();
                    ConnectorSet csi = new ConnectorSet();
                    csi.Insert(connector);

                    MechanicalSystem mechSystem = m_document.Create.NewMechanicalSystem(null, csi, connector.DuctSystemType);
                    subTransaction.Commit();
                }
            }

            return connector.MEPSystem;
        }
        private static List<Connector> GetDownstreamConnectors(Connector[] familyInstanceConnectors)
        {
            List<Connector> relevantConnectors = new List<Connector>();

            //  iterate through connectors on the family
            foreach (Connector connector in familyInstanceConnectors)
            {
                //Ignore Non-Duct Connectors
                if (RevitDomain.DomainHvac != connector.Domain)
                {
                    continue;
                }

                //Ensure duct is connected
                if (!connector.IsConnected)
                {
                    continue;
                }

                //inclusive method for finding relavent connectors
                //Questionable whether to accept return air as a "served" path
                if (connector.AssignedFlowDirection == FlowDirectionType.Out && connector.DuctSystemType == DuctSystemType.SupplyAir)
                {
                    relevantConnectors.Add(connector);
                }
                if (connector.AssignedFlowDirection == FlowDirectionType.In && (connector.DuctSystemType == DuctSystemType.ExhaustAir || connector.DuctSystemType == DuctSystemType.ReturnAir))
                {
                    relevantConnectors.Add(connector);
                }
            }
            return relevantConnectors;
        }
    }
}
