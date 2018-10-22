using System;
using System.Collections.Generic;
using System.IO;

namespace BRPLUSA.Domain.Services.Ventilation
{
    public class VentilationLookupService
    {
        public static string TypicalDataLocation { get; private set; }
        public static Dictionary<string, OccupancyLookup> DefaultDatabase { get; private set; }

        static VentilationLookupService()
        {
            Initialize();
        }

        private static void Initialize()
        {
            ReadDataFile();
            SeedDefaultDatabase();
        }

        public static void ReadDataFile()
        {
            var buildLocation = Path.GetDirectoryName(typeof(VentilationLookupService).Assembly.Location);
            var dirname = Path.Combine(buildLocation, "Data");
            TypicalDataLocation = Path.Combine(dirname, "Lookup_VentilationTable.csv");
        }

        private static void SeedDefaultDatabase()
        {
            var seed = ReadCSVToList(TypicalDataLocation, 4);
            var dict = new Dictionary<string, OccupancyLookup>();
            seed.ForEach(s => dict.Add(s.OccupancyCategory, s));
            DefaultDatabase = dict;
        }

        public static double GetOAACHRBasedOnOccupancyCategory(string category)
        {
            try
            {
                var lCat = DefaultDatabase[category];
                return lCat.MechCodeAshrae.VentilationAirChangesPerHour ?? 0.0;
            }

            catch (Exception e)
            {
                return 0.0;
            }
        }

        public static double GetACHRBasedOnOccupancyCategory(string category)
        {
            try
            {
                var lCat = DefaultDatabase[category];
                return lCat.MechCodeAshrae.SupplyAirChangesPerHour ?? 0.0;
            }

            catch (Exception e)
            {
                return 0.0;
            }
        }

        public static int GetRequirePressurizationBasedOnOccupancy(string category)
        {
            try
            {
                var lCat = DefaultDatabase[category];
                var pressure = lCat.MechCodeAshrae.PressureRelationship;

                return pressure == PressureRelationship.None || pressure == PressureRelationship.Unknown
                    ? 0
                    : pressure == PressureRelationship.Negative 
                        ? -1 
                        : 1;
            }

            catch (Exception e)
            {
                return 0;
            }
        }

        public static List<OccupancyLookup> ReadCSVToList(string path = "", int startOfData = 0)
        {
            List<OccupancyLookup> occList = new List<OccupancyLookup>();
            path = path == "" ? TypicalDataLocation : path;

            var reader = File.OpenText(path);
            var csv = new CsvReader(reader);

            while (csv.Read())
            {
                if (csv.Context.Row <= startOfData)
                    continue;

                var row = csv.Context.Record;
                var occCat = OccupancyCategoryFactory.Create(row);
                occList.Add(occCat);
            }

            return occList;
        }
    }
}
