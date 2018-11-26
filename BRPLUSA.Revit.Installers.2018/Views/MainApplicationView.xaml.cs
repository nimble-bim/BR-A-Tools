using System.Windows;

namespace BRPLUSA.Revit.Installers._2018.Views
{
    /// <summary>
    /// * DOCUMENTATION
    /// *
    /// * This app is two pieces, an installer and an updater
    /// *
    /// ** ############################ INSTALLER ########################################
    /// *
    /// * The installer's process is run via Squirrel and is documented on their site here:
    /// * https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/using/install-process.md#install-process-overview
    /// *
    /// * This is the start point for the installer but I'm still trying to figure out how it's run exactly
    /// *
    /// * ############################ UPDATER ########################################
    /// *
    /// * The updater is primarily run via Revit and it's start point is "technically" in the Uninitialize method of RevitApplicationEnhancements
    /// * over in BRPLUSA.Revit.Client
    /// *
    /// * Basically, every time the user exits Revit, the app runs a check on the server to see if there's an updated version of the package available.
    /// *
    /// * #################################################################################
    /// *
    /// *
    /// </summary>
    public partial class App : Application
    {

    }
}
