using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autodesk.Revit.DB;
using BRPLUSA.Domain.Core.Wrappers;
using RevitSpace = Autodesk.Revit.DB.Mechanical.Space;
using LiteDB;
using BRPLUSA.Domain.Wrappers;

namespace BRPLUSA.Revit.Core.Data
{
    //public class SpatialDatabaseWrapper : IDisposable
    //{
    //    private readonly LiteDatabase _db;
    //    private IEnumerable<Space> Spaces => _db.GetCollection<Space>().FindAll();
    //    private readonly string _location;

    //    public SpatialDatabaseWrapper(Document doc)
    //    {
    //        var directory = Path.GetDirectoryName(doc.PathName);
    //        _location = Path.Combine(directory, "SpatialData.db");

    //        _db = new LiteDatabase(_location);
    //    }

    //    private bool IsDatabaseCreated()
    //    {
    //        return File.Exists(_location);
    //    }

    //    public bool IsCurrentlyTracked(RevitSpace space)
    //    {
    //        var x = FindElement(space.UniqueId);

    //        return x != null;
    //    }

    //    //public bool NeedsUpdate(RevitSpace space)
    //    //{
    //    //    var dbSpace = FindElement(space.UniqueId);

    //    //    var exhaustNeedsUpdate = dbSpace.ExhaustNeedsUpdate(space);
    //    //    var returnNeedsUpdate = dbSpace.ReturnNeedsUpate(space);
    //    //    var supplyNeedsUpdate = dbSpace.SupplyNeedsUpdate(space);

    //    //    return exhaustNeedsUpdate || returnNeedsUpdate || supplyNeedsUpdate;
    //    //}

    //    public Space FindElement(string uniqueId)
    //    {
    //        var spaces = _db.GetCollection<Space>();
    //        return spaces.Find(s => s.Id == uniqueId).FirstOrDefault();
    //    }

    //    public IEnumerable<Space> FindElementPeers(string uniqueId)
    //    {
    //        var elem = FindElement(uniqueId);

    //        var peers = elem.ConnectedSpaces.Select(FindElement);

    //        return peers;
    //    }

    //    public IEnumerable<Space> GetAll(string uniqueId)
    //    {
    //        var all = new List<Space>();

    //        var primary = FindElement(uniqueId);
    //        var peers = FindElementPeers(uniqueId);

    //        all.Add(primary);
    //        all.AddRange(peers);

    //        return all;
    //    }

    //    public bool CreateElementRelationship(IEnumerable<Space> spaces)
    //    {
    //        // check if any of the spaces already exist in the db
    //        // if so, prompt the user and ask whether they want to connect
    //        // all the spaces and it's peers together
    //        var hasOldSpaces = spaces.Any(IsInDatabase);

    //        return hasOldSpaces
    //            ? HandleNewAndOldSpaces(spaces)
    //            : HandleNewSpaces(spaces);
    //    }

    //    private bool HandleNewAndOldSpaces(IEnumerable<Space> spaces)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool IsInDatabase(RevitSpace space)
    //    {
    //        return IsInDatabase(space.UniqueId);
    //    }

    //    private bool IsInDatabase(string uniqueId)
    //    {
    //        if (!IsDatabaseCreated())
    //            return false;

    //        var elements = _db.GetCollection<Space>();
    //        var element = elements.FindOne(s => s.Id == uniqueId);

    //        var isInDb = element != null;

    //        return isInDb;
    //    }

    //    private bool HandleNewSpaces(IEnumerable<RevitSpace> spaces)
    //    {
    //        try
    //        {
    //            var dbSpaces = _db.GetCollection<Space>();

    //            // else, add the space and it's peers along with their respective properties
    //            var ids = spaces.Select(s => s.UniqueId).ToArray();

    //            var wrapped = MapEntities(spaces);
    //            var newWrap = new List<Space>();
    //            foreach (var w in wrapped)
    //            {
    //                //w.ConnectPeers(ids);
    //                newWrap.Add(w);
    //            }

    //            dbSpaces.Insert(newWrap);
    //            dbSpaces.EnsureIndex(s => s.Id);

    //            return true;
    //        }

    //        catch (Exception e)
    //        {
    //            return false;
    //        }
    //    }

    //    public bool BreakElementRelationship(IEnumerable<Space> spaces)
    //    {
    //        foreach (var s in spaces)
    //        {
    //            try
    //            {
    //                BreakElementRelationship(s);
    //            }

    //            catch
    //            {
    //                return false;
    //            }
    //        }

    //        return true;
    //    }

    //    public bool BreakElementRelationship(RevitSpace space)
    //    {
    //        try
    //        {
    //            //var dbSp = _db.GetCollection<Space>().FindOne(s => s.Id == space.UniqueId);

    //            //// remove the space 
    //            //RemoveSpace(space);

    //            //// then remove it's connection to whatever spaces
    //            //// it was previously connected to
    //            //var peers = dbSp.ConnectedSpaces;

    //            //foreach (var p in peers)
    //            //{
    //            //    BreakConnection(p, space.UniqueId);
    //            //}

    //            return true;
    //        }

    //        catch (Exception e)
    //        {
    //            Debug.WriteLine($"Failure attempting to break element's relationship. { e.Message }");
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="spaceConn">Space which should have the second space removed from it's list of connected spaces</param>
    //    /// <param name="spaceDisconn">Space to be removed</param>
    //    private void BreakConnection(string spaceConn, string spaceDisconn)
    //    {
    //        //var sp1 = FindElement(spaceConn);

    //        //var newConns = sp1.ConnectedSpaces.ToList();
    //        //newConns.Remove(spaceDisconn);

    //        //sp1.ConnectPeers(newConns);

    //        //UpdateElement(sp1);
    //    }

    //    private void RemoveSpace(RevitSpace space)
    //    {
    //        RemoveSpace(space.UniqueId);
    //    }

    //    private void RemoveSpace(Space space)
    //    {
    //        RemoveSpace(space.Id);
    //    }

    //    private void RemoveSpace(string revitId)
    //    {
    //        var dbSp = _db.GetCollection<Space>();

    //        dbSp.Delete(s => s.Id == revitId);
    //    }

    //    private IEnumerable<Space> MapEntities(IEnumerable<RevitSpace> revs)
    //    {
    //        return revs.Select(r => new Space(r));
    //    }

    //    ////private void MapEntity(Space space)
    //    ////{
    //    ////    var mapper = BsonMapper.Global;

    //    ////    mapper.Entity<Space>()
    //    ////        .Index(r => r.UniqueId)
    //    ////        .Field(r => r.Name, "space_name")
    //    ////        .Field(r => r.Number, "space_number")
    //    ////        .Field(r => r.Room.Name, "room_name")
    //    ////        .Field(r => r.Room.Number, "room_number")
    //    ////        .Field(r => r.DesignSupplyAirflow, "specified_cfm_supply")
    //    ////        .Field(r => r.DesignExhaustAirflow, "specified_cfm_exhaust")
    //    ////        .Field(r => r.DesignReturnAirflow, "specified_cfm_return");
    //    ////}

    //    //public void UpdateElement(Space space)
    //    //{
    //    //    var dbSpaces = _db.GetCollection<Space>();
    //    //    var dbSp = dbSpaces.FindOne(s => s.Id == space.UniqueId);

    //    //    dbSp.SpecifiedExhaustAirflow = space.DesignExhaustAirflow;
    //    //    dbSp.SpecifiedReturnAirflow = space.DesignReturnAirflow;
    //    //    dbSp.SpecifiedSupplyAirflow = space.DesignSupplyAirflow;

    //    //    dbSpaces.Update(dbSp);
    //    //}

    //    //public void UpdateElement(Space wrap)
    //    //{
    //    //    _db.GetCollection<SpaceWrapper>().Update(wrap);
    //    //}

    //    //private void UpdateElementPeers(Space parent)
    //    //{
    //    //    var dbSpaces = _db.GetCollection<SpaceWrapper>();
    //    //    var dbSp = dbSpaces.FindOne(s => s.Id == parent.UniqueId);
    //    //    var peerIds = dbSp.ConnectedSpaces;
    //    //    var peers = peerIds.Select(FindElement).ToArray();

    //    //    foreach (var peer in peers)
    //    //    {
    //    //        if(peer.ExhaustNeedsUpdate(parent))
    //    //            peer.SpecifiedExhaustAirflow = parent.DesignExhaustAirflow;

    //    //        if(peer.ReturnNeedsUpate(parent))
    //    //            peer.SpecifiedReturnAirflow = parent.DesignReturnAirflow;

    //    //        if(peer.SupplyNeedsUpdate(parent))
    //    //            peer.SpecifiedSupplyAirflow = parent.DesignSupplyAirflow;
    //    //    }

    //    //    dbSpaces.Update(peers);
    //    //}

    //    public void Dispose()
    //    {
    //        _db.Dispose();
    //    }
    //}
}
