namespace BRPLUSA.Domain.Entities.Events
{
    public enum WorksharingEventType
    {
        Unknown = -2,
        None = -1,
        DefaultState = 0,
        ModelOpen = 1,
        ModelSyncing = 2,
        ModelSynced = 3,
        ModelClosed = 4,
        AllUsersExited = 5
    }
}
