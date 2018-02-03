using BRPLUSA.Database.Databases;
using BRPLUSA.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace BRPLUSA.Database.Tests
{
    [TestFixture]
    public class WMTableWrapperTests
    {
        private const string _tableName = "MODEL_A1_HVAC";
        private readonly User _user = new User {Name = "psmith@brplusa.com"};
        private WorksharingMonitorTable _table;

        [SetUp]
        public void ResetTable()
        {
            _table = new WorksharingMonitorTable(_tableName);
            _table.Database.Database.DropCollection(_tableName);
        }

        [Test]
        public void ShouldCreateNewTableForNewlyAddedModelOnInstantiation()
        {
            _table = new WorksharingMonitorTable(_tableName);

            var filter = new BsonDocument("name", _tableName);
            var dbs = _table.Database.Database.ListCollections(new ListCollectionsOptions{ Filter = filter});

            Assert.IsTrue(dbs.Any());
        }

        [Test]
        public void ShouldHaveDefaultStateOnCreation()
        {
            _table = new WorksharingMonitorTable(_tableName);

            // check if table has any data
            var elements = _table.Table.Count(_ => true);

            Assert.IsTrue(elements > 0);
        }

        [Test]
        public void ShouldHaveGetLastInserted()
        {
            _table = new WorksharingMonitorTable(_tableName);
            var eventState = new UserClosedModelEvent(_user);

            _table.Table.InsertOne(eventState);

            // check if table has any data
            var elem = _table.GetLastInserted();
            var datesAreEqual = elem.TimeCreated == eventState.TimeCreated;

            Assert.IsTrue(datesAreEqual);
        }

        [Test]
        public void ShouldAddNewModelOpenedEvent()
        {
            _table = new WorksharingMonitorTable(_tableName);
            _table.AddModelOpenedEvent(_user);

            // check if table has any data
            var elem = _table.GetLastInserted();

            Assert.AreEqual(WorksharingEventType.ModelOpen, elem.EventType);
        }
    }
}