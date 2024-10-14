using AerTaskAPI.Shared.Models.Tables;

namespace AerTaskAPI.Shared.Inputs
{
    public class AddColaborator
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string UserRole { get; set; }
        
        public ProjectUser GenerateCollaborator()
        {
            return new ProjectUser
            {
                UserId = UserId,
                UserRole = UserRole,
                ProjectId = ProjectId
            };
        }
    }
}
