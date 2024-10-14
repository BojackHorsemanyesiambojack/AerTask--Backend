using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace AerTaskAPI.Utils.Validations
{
    public class Passwords
    {
        public static bool PasswordIsValid(string Password)
        {
            string Pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";

            return Regex.IsMatch(Password, Pattern);
        }
    }
}
