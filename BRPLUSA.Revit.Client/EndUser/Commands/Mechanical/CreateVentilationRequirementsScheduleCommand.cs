using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Client.Base;
using BRPLUSA.Revit.Entities.Exceptions;
using BRPLUSA.Revit.Services.Spaces;

namespace BRPLUSA.Revit.Client.EndUser.Commands.Mechanical
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateVentilationRequirementsScheduleCommand : BaseCommand
    {
        protected override Result Work()
        {
            try
            {
                // Create Ventilation Schedule
                var schedule = SpaceVentilationService.CreateOrGetVentilationSchedule(CurrentDocument);

                // Add VentParameters to Schedule
                SpaceVentilationService.AddVentParametersToSchedule(schedule);

                // Compare Space Types to lookup table and fill out
                SpaceVentilationService.SetVentilationParameters(CurrentDocument);
            }

            catch (CancellableException e)
            {
                TaskDialog.Show("Cancelled", "The command was cancelled");
                return Result.Cancelled;
            }

            catch (Exception e)
            {
                TaskDialog.Show("Failure!", $"Something caused the command to fail here: {e}");
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}
