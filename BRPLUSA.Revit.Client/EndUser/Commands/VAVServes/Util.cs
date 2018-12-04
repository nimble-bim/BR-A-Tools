using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace BRPLUSA.Revit.Client.EndUser.Commands.VAVServes
{
    class Util
    {
        public static Connector[] GetConnectorsFromFamilyInstance(Element e)
        {
            if (e == null) return null;
            FamilyInstance fi = e as FamilyInstance;
            ConnectorSet connectorSet = null;
            List<Connector> cList = new List<Connector>();

            if (fi != null && fi.MEPModel != null)
            {
                try
                {
                    connectorSet = fi.MEPModel.ConnectorManager.Connectors;
                }
                catch
                {
                    return null;
                }
            }

            MEPSystem system = e as MEPSystem;
            if (system != null)
            {
                connectorSet = system.ConnectorManager.Connectors;
            }

            MEPCurve duct = e as MEPCurve;
            if (duct != null)
            {
                connectorSet = duct.ConnectorManager.Connectors;
            }

            if (connectorSet == null) return null;
            foreach (Connector c in connectorSet)
            {
                cList.Add(c);
            }

            return cList.ToArray();

        }
        //This method sets an element as the base equipment in a system
        public static void AddBaseEquipConnector(MEPSystem currentSys, Element e)
        {
            FamilyInstance fi = e as FamilyInstance;
            ConnectorSet connSet = null;
            MechanicalSystem mechSystem = currentSys as MechanicalSystem;
            List<ElementId> elementIds = new List<ElementId>();
            elementIds.Add(e.Id);


            if (fi == null || fi.MEPModel == null || mechSystem == null)
            {
                return;
            }

            connSet = fi.MEPModel.ConnectorManager.Connectors;


            foreach (Connector conn in connSet)
            {
                if (conn.MEPSystem == null || currentSys.Id == null)
                {
                    continue;
                }

                if (conn.MEPSystem.Id == currentSys.Id)
                {
                    //Revit defaulted the equipment to behave like a node
                    //To make it the base equipment for the system, we have to remove it as a node first
                    mechSystem.Remove(elementIds);

                    //Now we can set it as base equipment
                    mechSystem.BaseEquipmentConnector = conn;
                    continue;
                }
            }
        }

        public static Parameter GetParameter<T>(T fi, string paramName) where T : Element
        {
            ParameterSet paramSet = fi.Parameters;
            foreach (Parameter p in paramSet)
            {
                if (string.Compare(p.Definition.Name, paramName, true) == 0)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
