using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AerTaskAPI.Shared.Models.Tables
{
    public class UserAccountProfile
    {
        [Column("user_id")]
        [Key]
        [Required]
        public int UserId { get; set; }
        [Column("username")]
        [Required]
        public string UserName { get; set; } = string.Empty;

        public static implicit operator UserAccountProfile?(UserAccount? v)
        {
            throw new NotImplementedException();
        }
    }
}
