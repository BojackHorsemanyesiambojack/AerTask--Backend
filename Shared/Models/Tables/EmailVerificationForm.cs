using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AerTaskAPI.Shared.Models.Tables
{
    [Table("temporal_verification")]
    public class EmailVerificationForm
    {
        [Column("code_id")]
        [Key]
        [Required]
        public int Id { get; set; }
        [Column("code")]
        [Required]
        public string Token { get; set; }
        [Column("email")]
        [Required]
        public string Email { get; set; }
        [Column("user_id")]
        [Required]
        [ForeignKey("user_account")]
        public int User { get; set; }
        [Column("expires_at")]
        [Required]
        public DateTime ExpiresAt { get; set; }

        public ClientEmailVerification GenerateVerificationForClient()
        {
            return new ClientEmailVerification
            {
                Email = Email,
                Id = Id
            };
        }
    }
}
