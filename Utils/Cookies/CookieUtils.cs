namespace AerTaskAPI.Utils.Cookies
{
    public class CookieUtils
    {
        public static CookieOptions GenerateSecureCookieOptions(DateTime Expiration)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = Expiration,
                IsEssential = true,
            };
        }
    }
}
