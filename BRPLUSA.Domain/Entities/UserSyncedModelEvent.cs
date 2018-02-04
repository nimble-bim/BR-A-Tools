namespace BRPLUSA.Domain.Entities
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