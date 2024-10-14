using Npgsql;
using System.Data.Common;

namespace AerTaskAPI.Shared.Methods
{
    public class DbDataUtils
    {
        public static string GetStringConnection(string Host, string Pass, string Own, int Port, string Database)
        {
            var newStringConnection = new NpgsqlConnectionStringBuilder();
            newStringConnection.Host = Host;
            newStringConnection.Password = Pass;
            newStringConnection.Username = Own;
            newStringConnection.Port = Port;
            newStringConnection.Database = Database;
            newStringConnection.SslMode = SslMode.VerifyFull;

            return newStringConnection.ToString();
        }
    }
}
