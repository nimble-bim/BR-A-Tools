using BRPLUSA.Domain.Base;

namespace BRPLUSA.Domain.Entities
{
    public class WorksharingState : Entity
    {
        public string[] UsersSyncing { get; set; }
        public string[] UsersCurrentlyInModel { get; set; }
    }
}