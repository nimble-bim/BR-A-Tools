using NUnit.Framework;
using BRPLUSA.Revit.Installers._2018.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Revit.Installers._2018.Tests
{
    [TestFixture]
    public class ProductInstallUtilTests
    {
        private const string origin = @"D:\experiments\braTools\BRPLUSA.Revit.Installers.2018\bin\app-1.0.1\x64\locales\ca.pak";
        private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private string _testAppDir;

        [SetUp]
        public void Initialize()
        {
            _testAppDir = Path.Combine(_localAppData, "temp", "BRPLUSA_APP_TESTING");
        }

        [Test]
        public void ShouldBeAbleToReadVersionFromPath()
        {
            const string expected = "1.0.1";
            var value = ProductInstallationUtilities.ReadVersionFromPath(origin);

            Assert.AreEqual(expected, value);
        }

        [Test]
        public void ShouldBeAbleToGetCompleteInstallationDirectory()
        {
            
            var installDir = _testAppDir;
            var filewithparents = @"\x64\locales\ca.pak";
            var expected = installDir + filewithparents;
            var result = ProductInstallationUtilities.FinalizeInstallLocation(origin, installDir);

            Assert.AreEqual(expected, result);
        }
    }
}