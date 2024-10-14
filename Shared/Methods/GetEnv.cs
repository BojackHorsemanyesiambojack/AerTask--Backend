using DotNetEnv;

namespace AerTaskAPI.Shared.Methods
{
    public class GetEnv
    {
        public static string GetEmailPassword()
        {
            Env.Load();
            return Environment.GetEnvironmentVariable("EMAILPASSWORD");
        }
    }
}
