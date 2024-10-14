using AerTaskAPI.Shared.Context;
using AerTaskAPI.Shared.Inputs;
using AerTaskAPI.Shared.Models;
using AerTaskAPI.Shared.Models.Tables;
using AerTaskAPI.Utils.Validations;
using Microsoft.EntityFrameworkCore;

namespace AerTaskAPI.Services
{
    public class Auth
    {
        private readonly AerTaskDbContext _Context;
        public Auth(AerTaskDbContext _Context)
        {
            this._Context = _Context;
        }

        public async Task<UserAccountProfile> GetProfile(int id)
        {
            var User = await _Context.Users.FindAsync(id);
            return User.ToProfile();
        }

        public async Task<UserAccount> SearchUserByEmail(string email)
        {
            try
            {
                email = email.ToLower();
                var result = await _Context.Users.FirstOrDefaultAsync(e => e.UserEmail.ToLower() == email);

                return result;
            }
            catch(Exception)
            {
                throw new Exception("An error ocurriend during the proccess");
            }
        }

        public async Task<Sesion> Authenticate(SignIn Input)
        {
            try
            {
                UserAccount searchUser = await SearchUserByEmail(Input.UserEmail);
                if (searchUser == null)
                {
                    throw new UnauthorizedAccessException("Incorrect Credentials");
                }
                bool isCorrectPassword = searchUser.VerifyPassword(Input.UserPassword);
                if (isCorrectPassword == false)
                {
                    throw new UnauthorizedAccessException("Incorrect Credentials");
                }
                if(searchUser.IsEmailVerificated == false)
                {
                    await DeleteAccount(searchUser.UserId);
                    throw new InvalidOperationException("Email not verified. Your account has been deleted. Please sign up again.");
                }
                var NewSesion = searchUser.GenerateSesion();
                await _Context.Sesions.AddAsync(NewSesion);
                await _Context.SaveChangesAsync();
                return NewSesion;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch(Exception)
            {
                throw new Exception("An error ocurried during authentication");
            }
        }

        public async Task<ClientEmailVerification> CreateAccount(SignUp Input)
        {
            try
            {
                var SearchEmailExists = await SearchUserByEmail(Input.UserEmail);
                if (SearchEmailExists != null)
                {
                    throw new InvalidOperationException("Email alredy in use");
                }
                if (!Passwords.PasswordIsValid(Input.Password))
                {
                    throw new InvalidDataException("The password must have ASCII and number characters");
                }
                var age = DateTime.Now.Year - Input.BirthDate.Year;
                if (Input.BirthDate.Date > DateTime.Now.AddYears(-age)) age--;
                if (age < 6)
                {
                    throw new UnauthorizedAccessException("The user must be older than six years.");
                }
                var Account = Input.CreateAccount();
                await _Context.Users.AddAsync(Account);
                await _Context.SaveChangesAsync();
                var Verification = Account.GenerateEmailVerification();
                await _Context.TempVefCodes.AddAsync(Verification);
                await _Context.SaveChangesAsync();

                return Verification.GenerateVerificationForClient();
            }
            catch(UnauthorizedAccessException)
            {
                throw;
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch(OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried while creating account");
            }
        }

        public async Task<Sesion> VerifyEmail(EmailVerificationInput input)
        {
            try
            {
                var verifying = await _Context.TempVefCodes.FindAsync(input.Id);
                if (verifying == null)
                {
                    throw new UnauthorizedAccessException("Verification code not found.");
                }

                if (verifying.Token != input.Token)
                {
                    throw new UnauthorizedAccessException("Incorrect token.");
                }

                var account = await _Context.Users.FindAsync(verifying.User);
                if (account == null)
                {
                    throw new Exception("Associated user account not found.");
                }

                _Context.TempVefCodes.Remove(verifying);
                account.VerifyEmail();
                _Context.Users.Update(account);

                var NewSession = account.GenerateSesion();
                await _Context.Sesions.AddAsync(NewSession);
                await _Context.SaveChangesAsync();

                return NewSession;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during email verification.", ex);
            }
        }

        public async Task<bool> DeleteEmailVerificationToken(int id)
        {
            try
            {
                var token = await _Context.TempVefCodes.FindAsync(id);
                if (token == null)
                {
                    throw new Exception("Token not found.");
                }

                _Context.TempVefCodes.Remove(token);
                await _Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting token.", ex);
            }
        }

        public async Task<bool> DeleteAccount(int id)
        {
            try
            {
                var User = await _Context.Users.FindAsync(id);
                if(User == null)
                {
                    throw new InvalidOperationException("Asociated User not exists");
                }
                _Context.Users.Remove(User);
                await _Context.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("Error while deleting account");
            }
        }
        public async Task<UserAccountProfile>GetDefaultUser(int UserId)
        {
            try
            {
                var User = await _Context.Users.FindAsync(UserId);
                if (User == null)
                {
                    throw new InvalidOperationException("Asociated User Not Exists");
                }

                return User.ToProfile();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried getting User");
            }
        }
        public async Task<List<UserAccountProfile>> SearchUsersByUserName(string UserName, int Limit = 20)
        {
            try
            {
                UserName = UserName.ToLower();
                var Users = await _Context.Users.Where(u => u.UserName.ToLower().Contains(UserName))
                    .Select(u => u.ToProfile()).Take(Limit).ToListAsync();

                return Users;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried");
            }
        }
    }
}
