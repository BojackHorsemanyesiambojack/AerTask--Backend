using AerTaskAPI.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace AerTaskAPI.Utils.Validations
{
    public class SesionValidation
    {
        public async static Task SearchAndValidateSesion(AerTaskDbContext _Context, HttpRequest R)
        {
            string UserRequest = R.Cookies["Session_Token"];

            if (string.IsNullOrEmpty(UserRequest))
            {
                throw new InvalidDataException("Void Session");
            }
            var Exists = await _Context.Sesions.FirstOrDefaultAsync(s => s.SessionToken == UserRequest);
            if(Exists == null)
            {
                throw new InvalidOperationException("Sesion not exists or has expired");
            }
        }
    }
}
