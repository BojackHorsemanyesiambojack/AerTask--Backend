using AerTaskAPI.Shared.Models.Tables;

namespace AerTaskAPI.Shared.Models
{
    public class ProjectTask_User
    {
        public ProjectTask Task { get; set; }
        public UserAccountProfile Profile { get; set; }
    }
}
