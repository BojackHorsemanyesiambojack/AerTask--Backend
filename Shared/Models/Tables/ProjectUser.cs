using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AerTaskAPI.Shared.Models.Tables
{
    [Table("project_users")]
    public class ProjectUser
    {
        [Required, NotNull]
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required, NotNull]
        [ForeignKey("project")]
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Required, NotNull]
        [ForeignKey("user_account")]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("user_role")]
        [MaxLength(10)]
        public string UserRole { get; set; } = "viewer";

        public ProjectUser ChangeUserRole (string Role)
        {
            Role = Role.ToLower();
            string[] PossibleRoles = { "viewer", "admin", "member" };
            bool isValid = PossibleRoles.Contains(Role);
            if (!isValid)
            {
                throw new InvalidDataException("Invalid Role");
            }

            return new ProjectUser
            {
                Id = Id,
                UserId = UserId,
                UserRole = Role,
                ProjectId = ProjectId,
            };
        }
    }
}
