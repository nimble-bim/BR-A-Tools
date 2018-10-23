namespace BRPLUSA.Domain.Entities.Events
{
    public class UserClosedModelEvent : WorksharingEvent
    {
        public UserClosedModelEvent(User user) : base(user) { }

        public UserClosedModelEvent(string modelName) : base(modelName) { }

        protected override void Initialize()
        {
            EventType = WorksharingEventType.ModelClosed;
        }
    }
}