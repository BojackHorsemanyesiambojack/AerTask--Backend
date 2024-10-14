namespace AerTaskAPI.Shared.Inputs
{
    public class CreateProject
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public string ProjectPrivacity { get; set; } = string.Empty;
        public string ProjectType { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
