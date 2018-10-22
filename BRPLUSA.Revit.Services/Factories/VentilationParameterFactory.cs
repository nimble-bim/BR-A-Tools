using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Services.Factories
{
    public class VentilationParameterFactory
    {
        public static string VentilationGroupName
        {
            get { return "Ventilation"; }
        }

        public static IList<Parameter> GetVentParameters(Document doc)
        {
            if (VentilationParameterUtility.ModelHasVentParameters(doc))
                return VentilationParameterUtility.GetVentParametersFromModel(doc);

            var defs = GetVentParameterDefinitions(doc);
            var pars = new List<Parameter>();

            foreach (var d in defs)
            {
                var p = doc.GetParameterFromCategory(BuiltInCategory.OST_MEPSpaces, d.Name);
                pars.Add(p);
            }

            return pars;
        }

        /// <summary>
        /// Gets vent parameter definitions or creates them if they don't exist
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IList<Definition> GetVentParameterDefinitions(Document doc)
        {
            var file = doc.Application.OpenSharedParameterFile();
            var list = new List<Definition>();
            var achr = CreateOrGetACHRParameter(file);
            var achm = CreateOrGetACHMParameter(file);
            var oaachr = CreateOrGetOAACHRParameter(file);
            var oaachm = CreateOrGetOAACHMParameter(file);
            var pReq = CreateOrGetPressureRequiredParameter(file);
            var pMod = CreateOrGetPressureModelParameter(file);

            var vents = new Definition[]
            {
                achr, achm, oaachr, oaachm, pReq, pMod
            };

            list.AddRange(vents);

            return list;
        }

        public static Definition CreateOrGetACHRParameter(DefinitionFile file)
        {
            Definition achr = null;
            var group = RevitParameterUtility.CreateOrGetGroupInSharedParameterFile(file, VentilationGroupName);

            try
            {
                achr = group.Definitions.get_Item("ACHR");
            }

            catch (Exception e)
            {

            }

            var opts = new ExternalDefinitionCreationOptions("ACHR", ParameterType.Number);
            achr = achr ?? group.Definitions.Create(opts);


            return achr;
        }

        public static Definition CreateOrGetACHMParameter(DefinitionFile file)
        {
            Definition achm = null;
            var group = RevitParameterUtility.CreateOrGetGroupInSharedParameterFile(file, VentilationGroupName);

            try
            {
                achm = group.Definitions.get_Item("ACHM");
            }

            catch (Exception e)
            {

            }

            var opts = new ExternalDefinitionCreationOptions("ACHM", ParameterType.Number);
            achm = achm ?? group.Definitions.Create(opts);

            return achm;
        }

        public static Definition CreateOrGetOAACHRParameter(DefinitionFile file)
        {
            Definition oaachr = null;
            var group = RevitParameterUtility.CreateOrGetGroupInSharedParameterFile(file, VentilationGroupName);

            try
            {
                oaachr = group.Definitions.get_Item("OAACHR");
            }

            catch (Exception e)
            {

            }

            var opts = new ExternalDefinitionCreationOptions("OAACHR", ParameterType.Number);
            oaachr = oaachr ?? group.Definitions.Create(opts);

            return oaachr;
        }

        public static Definition CreateOrGetOAACHMParameter(DefinitionFile file)
        {
            Definition oaachm = null;
            var group = RevitParameterUtility.CreateOrGetGroupInSharedParameterFile(file, VentilationGroupName);

            try
            {
                oaachm = group.Definitions.get_Item("OAACHM");
            }

            catch (Exception e)
            {

            }

            var opts = new ExternalDefinitionCreationOptions("OAACHM", ParameterType.Number);
            oaachm = oaachm ?? group.Definitions.Create(opts);

            return oaachm;
        }

        public static Definition CreateOrGetPressureModelParameter(DefinitionFile file)
        {
            Definition press_mod = null;
            var group = RevitParameterUtility.CreateOrGetGroupInSharedParameterFile(file, VentilationGroupName);

            try
            {
                press_mod = group.Definitions.get_Item("PRESSURIZATION_MOD");
            }

            catch (Exception e)
            {

            }

            var opts = new ExternalDefinitionCreationOptions("PRESSURIZATION_MOD", ParameterType.Number);
            press_mod = press_mod ?? group.Definitions.Create(opts);

            return press_mod;
        }

        public static Definition CreateOrGetPressureRequiredParameter(DefinitionFile file)
        {
            Definition press_req = null;
            var group = RevitParameterUtility.CreateOrGetGroupInSharedParameterFile(file, VentilationGroupName);

            try
            {
                press_req = group.Definitions.get_Item("PRESSURIZATION_REQ");
            }

            catch (Exception e)
            {

            }

            var opts = new ExternalDefinitionCreationOptions("PRESSURIZATION_REQ", ParameterType.Integer);
            press_req = press_req ?? group.Definitions.Create(opts);

            return press_req;
        }
    }
}
