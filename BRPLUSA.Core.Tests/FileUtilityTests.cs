using System;
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
    }
}
