using BRPLUSA.Domain.Services;
using NUnit.Framework;
using System.IO;

namespace BRPLUSA.Domain.Tests.Excels
{
    [TestFixture]
    public class ExcelServicesTest
    {
        private const string _fullName = "Mercer BOH Lighting Schedule.xlsx";
        private string _file;
        private const int _rowCountLFS = 141;
        private const int _rowCountMercer = 28;

        [OneTimeSetUp]
        public void CheckFiles()
        {
            var assemblyLocation = Path.GetDirectoryName(GetType().Assembly.Location);
            _file = assemblyLocation + @"\Artifacts\" + _fullName;

            if (!File.Exists(_file))
                throw new System.Exception("Can't find the Excel test file");
        }

        [Test]
        public void ShouldBeAbleToReadRowsFromLFSSheet()
        {
            var result1 = ExcelServices.GetRowCount(_file, 0);

            Assert.AreEqual(_rowCountLFS, result1);
        }

        [Test]
        public void ShouldBeAbleToReadRowsFromMercerSheet()
        {
            var result2 = ExcelServices.GetRowCount(_file, 1);

            Assert.AreEqual(_rowCountMercer, result2);
        }

        [Test]
        public void ShouldBeAbleToCreateDataFromMercerSheet()
        {
            var result2 = ExcelServices.GetSheetData(_file, 1);

            Assert.AreEqual(_rowCountMercer, result2);
        }

        [TearDown]
        public void Cleanup()
        {
            ExcelServices.DisposeAll();
        }
    }
}

