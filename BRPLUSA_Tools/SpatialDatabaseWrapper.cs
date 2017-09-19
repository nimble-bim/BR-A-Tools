using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using LiteDB;

namespace BRPLUSA_Tools
{
    public class SpatialDatabaseWrapper : IDisposable
    {
        private readonly LiteDatabase _db;

        public SpatialDatabaseWrapper()
        {
            _db = new LiteDatabase(@"SpatialData.db");
        }

        private bool IsElementTracked(ElementId id)
        {
            return false;
        }

        public SpaceWrapper FindElement(string uniqueId)
        {
            var spaces = _db.GetCollection<SpaceWrapper>("spaces");
            return spaces.Find(s => s.Id == uniqueId).FirstOrDefault();
        }

        public IEnumerable<SpaceWrapper> FindElementPeers(string uniqueId)
        {
            var elem = FindElement(uniqueId);

            var peers = elem.ConnectedSpaces.Select(FindElement);

            return peers;
        }

        public IEnumerable<SpaceWrapper> FindAllConnectedElements(string uniqueId)
        {
            var all = new List<SpaceWrapper>();

            var primary = FindElement(uniqueId);
            var peers = FindElementPeers(uniqueId);

            all.Add(primary);
            all.AddRange(peers);

            return all;
        }

        public bool CreateElementRelationship(IEnumerable<Space> spaces)
        {
            var dbSpaces = _db.GetCollection<SpaceWrapper>();

            var hasExistingSpace = spaces.Any(
                                    revSp => dbSpaces.Exists(
                                    dbSp => dbSp.Id == revSp.UniqueId));

            // if one of the spaces is already in the db
            if (hasExistingSpace)
            {
                return false;
                // update that space to contain the new spaces as it's peers
            }

            // else, add the space and it's peers along with their respective properties
            else
            {

                var ids = spaces.Select(s => s.UniqueId);

                var wrapped = MapEntities(spaces);
                foreach (var w in wrapped)
                {
                    w.ConnectedSpaces = ids;
                }
            }


            return false;
        }

        private IEnumerable<SpaceWrapper> MapEntities(IEnumerable<Space> revs)
        {
            return revs.Select(MapEntity);
        }

        private static SpaceWrapper MapEntity(Space rev)
        {
            return new SpaceWrapper
            {
                Id = rev.UniqueId,
                SpaceName = rev.Name,
                SpaceNumber = rev.Number,
                RoomName = rev.Room.Name,
                RoomNumber = rev.Room.Number,
                SpecifiedSupplyAirflow = rev.DesignSupplyAirflow,
                SpecifiedExhaustAirflow = rev.DesignExhaustAirflow,
                SpecifiedReturnAirflow = rev.DesignReturnAirflow
            };
        }

        //private void MapEntity(Space space)
        //{
        //    var mapper = BsonMapper.Global;

        //    mapper.Entity<Space>()
        //        .Index(r => r.UniqueId)
        //        .Field(r => r.Name, "space_name")
        //        .Field(r => r.Number, "space_number")
        //        .Field(r => r.Room.Name, "room_name")
        //        .Field(r => r.Room.Number, "room_number")
        //        .Field(r => r.DesignSupplyAirflow, "specified_cfm_supply")
        //        .Field(r => r.DesignExhaustAirflow, "specified_cfm_exhaust")
        //        .Field(r => r.DesignReturnAirflow, "specified_cfm_return");
        //}

        public void UpdateElement(string uniqueId)
        {
            //var spaces = _db.GetCollection<SpaceWrapper>();

            //var updatable = FindElement(uniqueId);

            //var changedValue = GetChangedValue(updatable)

            //spaces.Update(updatable);

            //UpdateElementPeers(uniqueId);
        }

        public void UpdateElementPeers(string uniqueId, LiteCollection<Space> spaces)
        {
            var peers = FindElementPeers(uniqueId);

            spaces.Update(s)
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
