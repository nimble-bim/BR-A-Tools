namespace BRPLUSA.Domain.Entities.Events
{
    public class UserSyncedModelEvent : WorksharingEvent
    {
        public UserSyncedModelEvent(User user) : base(user) { }

        protected override void Initialize()
        {
            EventType = WorksharingEventType.ModelSynced;
        }
    }
}