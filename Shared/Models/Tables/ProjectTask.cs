using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AerTaskAPI.Shared.Models.Tables
{
    [Table("project_tasks")]
    public class ProjectTask
    {
        [Key]
        [Column("p_task_id")]
        public int ProjectTaskId { get; set; }
        [Required, NotNull]
        [ForeignKey("project")]
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Required, NotNull]
        [Column("task_name")]
        public string ProjectTaskName { get; set; } = string.Empty;
        [Column("task_description")]
        public string ProjectTaskDescription { get; set; } = string.Empty;
        [Column("task_status")]
        [Required, NotNull]
        public string ProjectTaskStatus { get; set; } = string.Empty;
        [Column("user_id")]
        [ForeignKey("user_account")]
        [Required, NotNull]
        public int AddByUser { get; set; }
    }
}
