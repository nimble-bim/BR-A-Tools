namespace BRPLUSA.Domain.Entities
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