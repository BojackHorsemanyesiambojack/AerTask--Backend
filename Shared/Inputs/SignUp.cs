using AerTaskAPI.Shared.Models.Tables;

namespace AerTaskAPI.Shared.Inputs
{
    public class SignUp
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }

        public UserAccount CreateAccount()
        {
            return new UserAccount
            {
                UserEmail = UserEmail.ToLower(),
                UserPassword = BCrypt.Net.BCrypt.HashPassword(Password),
                UserName = UserName,
                UserBirthDate = DateTime.SpecifyKind(BirthDate, DateTimeKind.Utc),
                IsEmailVerificated = false
            };
        }
    }
}
