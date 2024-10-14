using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace AerTaskAPI.Utils.DataManipulating
{
    public class Generation
    {
        public static string GenerateEmailVerificationToken()
        {
            byte[] tokenData = new byte[6];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenData);
            }

            string token = string.Join("", tokenData.Select(b => (b % 10).ToString()));

            return token;
        }

        public static string GenerateSessionToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@*#&$><?";
            char[] Token = new char[64];
            byte[] TokenData = new byte[64];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(TokenData);
            }
            for(int i = 0; i < Token.Length; i++)
            {
                Token[i] = chars[TokenData[i] % chars.Length];
            }
            return new string(Token);
        }
    }
}
