using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using BRPLUSA.Revit.Services.Factories;
using BRPLUSA.Revit.Services.Utilities;

namespace BRPLUSA.Revit.Services.Spaces
{
    public class SpaceVentilationService
    {
        /// <summary>
        /// Gets the vent parameters necessary or creates them if they are unavailable
        /// </summary>
        /// <param name="schedule"></param>
        public static void AddVentParametersToSchedule(ViewSchedule schedule)
        {
            // check if doc has vent parameters
            // if not create them
            var hasVentParams = VentilationParameterUtility.ModelHasVentParameters(schedule.Document);

            var ventParams = hasVentParams 
                ? VentilationParameterUtility.GetVentParametersFromModel(schedule.Document) 
                : VentilationParameterUtility.CreateVentParametersInModel(schedule.Document);

            // add them to the schedule as schedulable fields
            VentilationParameterUtility.AddParameterToSchedule(schedule, ventParams.ToArray());
        }

        public static ViewSchedule CreateOrGetVentilationSchedule(Document doc)
        {
            return VentilationScheduleFactory.CreateOrGetVentilationSchedule(doc);
        }

        public static void SetVentilationParameters(Document doc)
        {
            var spaces = new FilteredElementCollector(doc)
                            .OfClass(typeof(SpatialElement))
                            .ToElements()
                            .Where(s => s is Space)
                            .Cast<Space>();

            // Get requirements from lookup
            ApplyVentRequirementsToSpaces(spaces);

            // Evaluate Model based CFM stuff
            CalculateCurrentCFMForSpaces(spaces);

            CalculatePressurizationForSpaces(spaces);
        }

        /// <summary>
        /// Gets values of Ventilation Requirement parameters from a lookup tables based on Space values
        /// </summary>
        /// <param name="spaces"></param>
        public static void ApplyVentRequirementsToSpaces(IEnumerable<Space> spaces)
        {
            foreach (var space in spaces)
            {
                VentilationParameterUtility.AssignACHRBasedOnCategory(space);
                VentilationParameterUtility.AssignOAACHRBasedOnCategory(space);
            }
        }

        public static void CalculateCurrentCFMForSpaces(IEnumerable<Space> spaces)
        {
            foreach (var space in spaces)
            {
                VentilationParameterUtility.AssignACHMBasedOnCategory(space);
                VentilationParameterUtility.AssignOAACHMBasedOnCategory(space);
            }
        }

        public static void CalculatePressurizationForSpaces(IEnumerable<Space> spaces)
        {
            foreach (var space in spaces)
            {
                VentilationParameterUtility.AssignRequiredPressurization(space);
                VentilationParameterUtility.AssignModeledPressurization(space);
            }
        }
    }
}
