using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using BRPLUSA.Database.Databases;
using BRPLUSA.Domain.Entities;
using BRPLUSA.Revit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Domain;

namespace BRPLUSA.Revit.Client.Updaters
{
    public class WorksharingMonitorUpdater : IRegisterableUpdater
    {
        private static WorksharingMonitorTable _db;
        private static WorksharingEvent _state;

        public WorksharingMonitorUpdater()
        {
            //_state = _db.GetCurrentState();
        }

        public void NotifyModelOpen(object sender, DocumentOpenedEventArgs args)
        {
            // send notification to server
            var state = new UserOpenedModelEvent(args.Document.Title);
            WorksharingMonitorService.PostModelOpenedEvent(state);
        }

        public void NotifyModelSyncing(object sender, DocumentSynchronizingWithCentralEventArgs args)
        {
            // send notification to db
        }

        public void NotifyModelSynced(object sender, DocumentSynchronizedWithCentralEventArgs args)
        {
            // send notification to db
        }

        public void NotifyModelClosed(object sender, DocumentClosedEventArgs args)
        {
            // send notification to db
            //UnsubscribeToUpdates();
        }

        // necessary? updates will appear in browser, no?
        public void SubscribeToUpdates()
        {

        }

        public void UnsubscribeToUpdates()
        {

        }

        public void Deregister()
        {
            throw new NotImplementedException();
        }

        public void Register(Document doc)
        {
            throw new NotImplementedException();
        }
    }
}
