namespace BRPLUSA.Domain.Entities
{
    public class UserOpenedModelEvent : WorksharingEvent
    {
        public UserOpenedModelEvent(User user) : base(user) { }

        public UserOpenedModelEvent(string modelName) : base(modelName) { }

        protected override void Initialize()
        {
            EventType = WorksharingEventType.ModelOpen;
        }
    }
}