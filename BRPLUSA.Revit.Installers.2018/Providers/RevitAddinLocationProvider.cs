using System;

namespace BRPLUSA.Revit.Installers._2018.Providers
{
    public class RevitAddinLocationProvider
    {
        private static string ApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
        }

        private static string RevitAddinLocation
        {
            get
            {
                return ApplicationData + @"\Autodesk\Revit\Addins";
            }
        }

        private static string Revit2018AddinLocation
        {
            get
            {
                return RevitAddinLocation + @"\2018";
            }
        }

        private static string Revit2019AddinLocation
        {
            get
            {
                return RevitAddinLocation + @"\2019";
            }
        }

        private static string Revit2020AddinLocation
        {
            get
            {
                return RevitAddinLocation + @"\2020";
            }
        }

        public static string GetRevitAddinFolderLocation(RevitVersion version)
        {
            switch (version)
            {
                case RevitVersion.V2018:
                    return Revit2018AddinLocation;
                case RevitVersion.V2019:
                    return Revit2019AddinLocation;
                case RevitVersion.V2020:
                    return Revit2020AddinLocation;

                default:
                case RevitVersion.Unknown:
                    throw new Exception("Unknown version");
            }
        }
    }
}
