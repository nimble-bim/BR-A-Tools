using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using BRPLUSA.Client.Revit.Interfaces;
using BRPLUSA.Domain;
using BRPLUSA.Domain.Entities;

namespace BRPLUSA.Client.Revit.Client.Updaters
{
    public class WorksharingMonitorUpdater : IRegisterableUpdater
    {
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
