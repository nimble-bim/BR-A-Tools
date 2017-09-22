using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using BRPLUSA.Entities.Wrappers;
using LiteDB;

namespace BRPLUSA.Data
{
    public class SpatialDatabaseWrapper : IDisposable
    {
        private readonly LiteDatabase _db;
        private IEnumerable<SpaceWrapper> Spaces => _db.GetCollection<SpaceWrapper>().FindAll();

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
            // check if any of the spaces already exist in the db
            // if so, prompt the user and ask whether they want to connect
            // all the spaces and it's peers together
            return spaces.Any(IsInDatabase) 
                ? HandleNewAndOldSpaces(spaces) 
                : HandleNewSpaces(spaces);
        }

        private bool HandleNewAndOldSpaces(IEnumerable<Space> spaces)
        {
            throw new NotImplementedException();
        }

        public bool IsInDatabase(Space space)
        {
            return IsInDatabase(space.UniqueId);
        }

        private bool IsInDatabase(string uniqueId)
        {
            var element = _db.GetCollection<SpaceWrapper>().FindOne(s => s.Id == uniqueId);

            return element != null;
        }

        private bool HandleNewSpaces(IEnumerable<Space> spaces)
        {
            try
            {
                var dbSpaces = _db.GetCollection<SpaceWrapper>();

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

            catch (Exception e)
            {
                return false;
            }
        }

        public bool BreakElementRelationship(IEnumerable<Space> spaces)
        {
            foreach (var s in spaces)
            {
                try
                {
                    BreakElementRelationship(s);
                }

                catch
                {
                    return false;
                }
            }

            return true;
        }

        public bool BreakElementRelationship(Space space)
        {
            try
            {
                var dbSp = _db.GetCollection<SpaceWrapper>().FindOne(s => s.Id == space.UniqueId);

                // remove the space 
                RemoveSpace(space);

                // then remove it's connection to whatever spaces
                // it was previously connected to
                var peers = dbSp.ConnectedSpaces;

                foreach (var p in peers)
                {
                    BreakConnection(p, space.UniqueId);
                }

                return true;
            }

            catch (Exception e)
            {
                Debug.WriteLine($"Failure attempting to break element's relationship. { e.Message }");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spaceConn">Space which should have the second space removed from it's list of connected spaces</param>
        /// <param name="spaceDisconn">Space to be removed</param>
        private void BreakConnection(string spaceConn, string spaceDisconn)
        {
            var sp1 = FindElement(spaceConn);

            var newConns = sp1.ConnectedSpaces.ToList();
            newConns.Remove(spaceDisconn);

            sp1.ConnectedSpaces = newConns;

            UpdateElement(sp1);
        }

        private void RemoveSpace(Space space)
        {
            RemoveSpace(space.UniqueId);
        }

        private void RemoveSpace(SpaceWrapper space)
        {
            RemoveSpace(space.Id);
        }

        private void RemoveSpace(string revitId)
        {
            var dbSp = _db.GetCollection<SpaceWrapper>();

            dbSp.Delete(s => s.Id == revitId);
        }

        private IEnumerable<SpaceWrapper> MapEntities(IEnumerable<Space> revs)
        {
            return revs.Select(r => new SpaceWrapper(r));
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
            _db.GetCollection<SpaceWrapper>().Update(new SpaceWrapper(space));

            UpdateElementPeers(space);
        }

        public void UpdateElement(SpaceWrapper wrap)
        {
            _db.GetCollection<SpaceWrapper>().Update(wrap);
        }

        private void UpdateElementPeers(Space parent)
        {
            var db = _db.GetCollection<SpaceWrapper>();
            var peerIds = db.FindOne(s => s.Id == parent.UniqueId).ConnectedSpaces;
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
