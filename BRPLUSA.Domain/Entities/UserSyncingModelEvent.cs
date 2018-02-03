namespace BRPLUSA.Domain.Entities
{
    public class UserSyncingModelEvent : WorksharingEvent
    {
        public UserSyncingModelEvent(User user)
        {
            EventType = WorksharingEventType.ModelSyncing;
            User = user;
        }
    }
}