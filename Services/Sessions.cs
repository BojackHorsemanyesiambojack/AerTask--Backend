using AerTaskAPI.Shared.Context;
using AerTaskAPI.Shared.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AerTaskAPI.Services
{
    public class Sessions
    {
        private readonly AerTaskDbContext _Context;
        public Sessions(AerTaskDbContext _Context)
        {
            this._Context = _Context;
        }

        public bool IsValidToken(string Token)
        {
            return !string.IsNullOrEmpty(Token);
        }

        public async Task<UserAccountProfile> VerificateTokenAndGetProfileIfValid(string Token)
        {
            try
            {
                var session = await _Context.Sesions.FirstOrDefaultAsync(e => e.SessionToken == Token);

                if (session == null || session.ExpiresAt < DateTime.UtcNow)
                {
                    throw new InvalidOperationException("Session does not exist or has expired");
                }

                var user = await _Context.Users.FindAsync(session.UserId);
                if (user == null)
                {
                    throw new InvalidOperationException("User associated with session does not exist");
                }

                return user.ToProfile();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while authenticating token", ex);
            }
        }
    }
}
