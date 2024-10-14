using AerTaskAPI.Utils.Cookies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AerTaskAPI.Shared.Models.Tables
{
    [Table("sessions")]
    public class Sesion
    {
        [Key]
        [Required]
        [Column("session_id")]
        public int SesionId { get; set; }
        [Required]
        [ForeignKey("user_account")]
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("session_token")]
        public string SessionToken { get; set; } = string.Empty;
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("updated_at")]
        public DateTime UpdateAt { get; set; }

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}
