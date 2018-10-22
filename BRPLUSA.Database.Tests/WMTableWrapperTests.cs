using BRPLUSA.Database.Databases;
using BRPLUSA.Domain.Entities;
using BRPLUSA.Domain.Entities.Events;
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
        private const string _modelName = "MODEL_A1_HVAC";
        private const string _fakeName = "psmith@brplusa.com";
        private readonly User _user = new User(_fakeName);
        private readonly CurrentUser _currentUser = new CurrentUser();
        private WorksharingMonitorTable _table;

        [SetUp]
        public void ResetTable()
        {
            _table = new WorksharingMonitorTable(_modelName);
            _table.Database.Database.DropCollection(_modelName);
        }

        [Test]
        public void ShouldCreateNewTableForNewlyAddedModelOnInstantiation()
        {
            _table = new WorksharingMonitorTable(_modelName);

            var filter = new BsonDocument("name", _modelName);
            var dbs = _table.Database.Database.ListCollections(new ListCollectionsOptions{ Filter = filter});

            Assert.IsTrue(dbs.Any());
        }

        [Test]
        public void ShouldHaveDefaultStateOnCreation()
        {
            _table = new WorksharingMonitorTable(_modelName);

            // check if table has any data
            var elements = _table.Table.Count(_ => true);

            Assert.IsTrue(elements > 0);
        }

        [Test]
        public void ShouldHaveGetLastInserted()
        {
            _table = new WorksharingMonitorTable(_modelName);
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
            _table = new WorksharingMonitorTable(_modelName);
            _table.AddModelOpenedEvent(_modelName);

            // check if table has any data
            var elem = _table.GetLastInserted();

            Assert.AreEqual(WorksharingEventType.ModelOpen, elem.EventType);
        }
    }
}