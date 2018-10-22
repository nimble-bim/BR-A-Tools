namespace BRPLUSA.Domain.Entities.Events
{
    public class UserSyncingModelEvent : WorksharingEvent
    {
        public UserSyncingModelEvent(User user) : base(user) { }
        protected override void Initialize()
        {
            EventType = WorksharingEventType.ModelSyncing;
        }
    }
}