using AerTaskAPI.Shared.Inputs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AerTaskAPI.Shared.Models.Tables
{
    [Table("project")]
    public class Project
    {
        [Key]
        [Required]
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Required]
        [Column("project_name")]
        [MaxLength(99)]
        public string ProjectName { get; set; } = string.Empty;
        [Column("project_description")]
        public string ProjectDescription { get; set; } = string.Empty;
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdateAt { get; set; }
        [Column("project_status")]
        [MaxLength(50)]
        public string ProjectStatus { get; set; } = string.Empty;
        [Column("project_privacity")]
        [MaxLength(8)]
        public string ProjectPrivacity { get; set; } = string.Empty;
        [Column("project_type")]
        [MaxLength(10)]
        public string ProjectType { get; set; } = string.Empty;
        [Column("user_id")]
        [ForeignKey("user_account")]
        [Required, NotNull]
        public int UserId { get; set; }

        public static Project GenerateProject(CreateProject Project)
        {
            return new Project
            {
                ProjectName = Project.ProjectName,
                ProjectDescription = Project.ProjectDescription,
                ProjectType = Project.ProjectType,
                ProjectStatus = "active",
                ProjectPrivacity = Project.ProjectPrivacity,
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
                UserId = Project.UserId
            };
        }

        public ProjectUser GenerateProjectCreator(UserAccount User)
        {
            return new ProjectUser
            {
                ProjectId = ProjectId,
                UserId = User.UserId,
                UserRole = "creator"
            };
        }
    }
}
