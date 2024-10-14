using AerTaskAPI.Shared.Models.Tables;

namespace AerTaskAPI.Shared.Inputs
{
    public class CreateProjectTask
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;

        public ProjectTask GenerateProjectTask()
        {
            return new ProjectTask
            {
                ProjectTaskName = TaskName,
                ProjectTaskDescription = TaskDescription,
                ProjectId = ProjectId,
                AddByUser = UserId,
                ProjectTaskStatus = "active",
            };
        }
    }
}
