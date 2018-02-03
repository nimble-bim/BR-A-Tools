namespace BRPLUSA.Domain.Entities
{
    public class UserSyncedModelEvent : WorksharingEvent
    {
        public UserSyncedModelEvent(User user)
        {
            EventType = WorksharingEventType.ModelSynced;
            User = user;
        }
    }
}