using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AerTaskAPI.Utils.DataManipulating;
using AerTaskAPI.Utils.Email;

namespace AerTaskAPI.Shared.Models.Tables
{
    [Table("user_account")]
    public class UserAccount
    {
        [Column("user_id")]
        [Key]
        [Required]
        public int UserId { get; set; }

        [Column("username")]
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Column("user_email")]
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string UserEmail { get; set; } = string.Empty;

        [Column("user_password")]
        [Required]
        public string UserPassword { get; set; } = string.Empty;

        [Column("user_birthdate")]
        [Required]
        public DateTime UserBirthDate { get; set; }
        [Column("email_verificated")]
        [Required]
        public bool IsEmailVerificated { get; set; }

        public void HashPassword()
        {
            UserPassword = BCrypt.Net.BCrypt.HashPassword(UserPassword);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, UserPassword);
        }

        public UserAccountProfile ToProfile()
        {
            return new UserAccountProfile
            {
                UserId = UserId,
                UserName = UserName,
            };
        }

        public Sesion GenerateSesion()
        {
            return new Sesion
            {
                UserId = UserId,
                SessionToken = Generation.GenerateSessionToken(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
            };
        }

        public EmailVerificationForm GenerateEmailVerification()
        {
            string Token = Generation.GenerateEmailVerificationToken();
            SendEmailVerification(Token);
            return new EmailVerificationForm
            {
                Token = Token,
                User = UserId,
                Email = UserEmail,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public void VerifyEmail()
        {
            IsEmailVerificated = true;
            UserBirthDate = DateTime.SpecifyKind(UserBirthDate, DateTimeKind.Utc);
        }

        private void SendEmailVerification(string Token)
        {
            try
            {
                EmailSending.GenerateEmailVerification(UserEmail, Token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public ProjectUserShow ToProjectShowing(string Role )
        {
            return new ProjectUserShow
            {
                UserId = UserId,
                UserName = UserName,
                UserRole = Role,
            };
        }
    }
}
