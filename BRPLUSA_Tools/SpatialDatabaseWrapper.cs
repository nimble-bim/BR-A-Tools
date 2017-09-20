using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using LiteDB;

namespace BRPLUSA_Tools
{
    public class SpatialDatabaseWrapper : IDisposable
    {
        private readonly LiteDatabase _db;

        public SpatialDatabaseWrapper(Document doc)
        {
            var directory = Path.GetDirectoryName(doc.PathName);
            var dbLocation = Path.Combine(directory, "SpatialData.db");

            _db = new LiteDatabase(dbLocation);
        }

        public bool IsCurrentlyTracked(Space space)
        {
            var x = FindElement(space.UniqueId);

            return x != null;
        }

        public bool NeedsUpdate(Space space)
        {
            var dbSpace = FindElement(space.UniqueId);

            var exhaustNeedsUpdate = Math.Abs(space.DesignExhaustAirflow - dbSpace.SpecifiedExhaustAirflow) > .0001;
            var returnNeedsUpdate = Math.Abs(space.DesignReturnAirflow - dbSpace.SpecifiedReturnAirflow) > .0001;
            var supplyNeedsUpdate = Math.Abs(space.DesignSupplyAirflow - dbSpace.SpecifiedSupplyAirflow) > .0001;

            return exhaustNeedsUpdate || returnNeedsUpdate || supplyNeedsUpdate;
        }

        public SpaceWrapper FindElement(string uniqueId)
        {
            var spaces = _db.GetCollection<SpaceWrapper>();
            return spaces.Find(s => s.Id == uniqueId).FirstOrDefault();
        }

        public IEnumerable<SpaceWrapper> FindElementPeers(string uniqueId)
        {
            var elem = FindElement(uniqueId);

            var peers = elem.ConnectedSpaces.Select(FindElement);

            return peers;
        }

        public IEnumerable<SpaceWrapper> GetAll(string uniqueId)
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

            var exstSp = spaces.Where(
                          revSp => dbSpaces.Exists(
                          dbSp => dbSp.Id == revSp.UniqueId));

            // if any of the spaces is already in the db
            if (exstSp != null)
            {
            }

            // else, add the space and it's peers along with their respective properties
            var ids = spaces.Select(s => s.UniqueId);

            var wrapped = MapEntities(spaces);
            foreach (var w in wrapped)
            {
                w.ConnectedSpaces = ids;
            }

            dbSpaces.Insert(wrapped);
            dbSpaces.EnsureIndex(s => s.Id);

            return true;
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
                RoomName = rev.Room?.Name,
                RoomNumber = rev.Room?.Number,
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

        public void UpdateElement(Space space)
        {
            var spaces = _db.GetCollection<SpaceWrapper>();

            var updatable = FindElement(space.UniqueId);

            spaces.Update(updatable);

            var others = updatable.ConnectedSpaces;

            UpdateElementPeers(space, others, spaces);
        }

        private void UpdateElementPeers(Space parent, IEnumerable<string> peerIds, LiteCollection<SpaceWrapper> db)
        {
            var peers = peerIds.Select(FindElement);

            foreach (var peer in peers)
            {
                peer.SpecifiedExhaustAirflow = parent.DesignExhaustAirflow;
                peer.SpecifiedReturnAirflow = parent.DesignReturnAirflow;
                peer.SpecifiedSupplyAirflow = parent.DesignSupplyAirflow;
            }

            db.Update(peers);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
