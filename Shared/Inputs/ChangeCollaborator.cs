namespace AerTaskAPI.Shared.Inputs
{
    public class ChangeCollaborator
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string Role { get; set; }
    }
}
