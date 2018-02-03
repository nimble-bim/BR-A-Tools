namespace BRPLUSA.Domain.Entities
{
    public class UserOpenedModelEvent : WorksharingEvent
    {
        public UserOpenedModelEvent(User user)
        {
            EventType = WorksharingEventType.ModelOpen;
            User = user;
        }
    }
}