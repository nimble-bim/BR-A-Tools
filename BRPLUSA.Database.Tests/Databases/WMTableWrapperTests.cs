using Microsoft.VisualStudio.TestTools.UnitTesting;
using BRPLUSA.Database.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BRPLUSA.Database.Databases.Tests
{
    [TestClass]
    public class WMTableWrapperTests
    {
        [TestMethod]
        public void ShouldCreateNewTableForNewlyAddedModelOnInstantiation()
        {
            var tableName = "MODEL_A1_HVAC";
            var table = new WMTableWrapper(tableName);
            var A1_HVAC_TABLE = table.Database.GetCollection<WorksharingEvent>(tableName);
            var names = table.Database.ListCollections();

            Assert.IsNotNull(A1_HVAC_TABLE);
        }

        [TestMethod]
        public void ShouldHaveDefaultStateOnCreation()
        {
            var tableName = "MODEL_A1_HVAC";
            var table = new WMTableWrapper(tableName);
            var A1_HVAC_TABLE = table.Database.GetCollection<WorksharingEvent>(tableName);
            var query = Builders<WorksharingEvent>.Filter.Where(w => w.InternalId != null);
            var state = A1_HVAC_TABLE.Find<WorksharingEvent>(query);
            var any = state.FirstOrDefault();

            Assert.IsNotNull(state);
            Assert.IsNotNull(any);
        }
    }
}