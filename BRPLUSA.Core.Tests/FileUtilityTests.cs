using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BRPLUSA.Core.Tests
{
    [TestClass]
    public class FileUtilityTests
    {
        [TestMethod]
        public void ShouldFindBrplusaProjectNumberWithNumbers()
        {
            // arrange
            const string expected = "1764000.BK";
            const string path = @"K:\Jobs\1764000.BK\BIM\MEP\GICC_ELEC_v2017.rvt";

            // act
            var result = FileUtils.ParseForBrplusaProjectNumber(path);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShouldFindBrplusaProjectNumberWithLetters()
        {
            // arrange
            const string expected = "PC67700.IV";
            const string path = @"K:\Jobs\PC67700.IV\BIM\MEP\PC677_ELEC_R17.rvt";

            // act
            var result = FileUtils.ParseForBrplusaProjectNumber(path);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShouldFindBrplusaProjectNumberWithDomainChanged()
        {
            // arrange
            const string expected = "PC67700.IV";
            const string path = @"\\brplusa.com\Project3\Jobs\PC67700.IV\BIM\MEP\PC677_ELEC_R17.rvt";

            // act
            var result = FileUtils.ParseForBrplusaProjectNumber(path);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShouldCreateCorrectBackupFolderString()
        {
            //var _modelPath = "A360://ELEC - GICC_17640.rvt";

            //var desktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            //var fileName = Path.GetFileNameWithoutExtension(_modelPath);
            var cult = new CultureInfo("nl-NL");
            Thread.CurrentThread.CurrentCulture = cult;
            var now = DateTime.UtcNow.ToShortDateString().ToString(cult) + "_" + DateTime.UtcNow.ToLongTimeString().ToString(cult);
            now = now.Replace(":", String.Empty);
            //var backupFilePath = $@"{desktop}\_bim360backups\{fileName}_{now}.rvt";
            //var backupFolder = Directory.GetParent(backupFilePath).FullName;

            Assert.IsFalse(now.Contains("/") || now.Contains(@"\") || now.Contains(":"));
        }
    }
}
