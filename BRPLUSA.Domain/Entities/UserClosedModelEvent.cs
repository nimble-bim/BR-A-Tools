namespace BRPLUSA.Domain.Entities
{
    public class UserClosedModelEvent : WorksharingEvent
    {
        public UserClosedModelEvent(User user)
        {
            EventType = WorksharingEventType.ModelClosed;
            User = user;
        }
    }
}