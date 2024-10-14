using AerTaskAPI.Services;
using AerTaskAPI.Shared.Context;
using AerTaskAPI.Shared.Inputs;
using AerTaskAPI.Shared.Models;
using AerTaskAPI.Shared.Models.Tables;
using AerTaskAPI.Utils.Cookies;
using AerTaskAPI.Utils.Validations;
using Microsoft.AspNetCore.Mvc;
using Sprache;

namespace AerTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly Auth _Services;
        private readonly AerTaskDbContext _Context;
        public AuthController(Auth _Services, AerTaskDbContext _Context)
        {
            this._Services = _Services;
            this._Context = _Context;
        }

        [HttpPost]
        [Route("SignUp")]

        public async Task<ActionResult<ClientEmailVerification>> CreateAccount([FromBody] SignUp S)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var Response = await _Services.CreateAccount(S);

                if (Response == null)
                {
                    return BadRequest("Account could not be created.");
                }
                return Accepted(Response);
            }catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("SignIn")]
        public async Task<ActionResult<UserAccountProfile>> Login([FromBody]SignIn S)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var Result = await _Services.Authenticate(S);
                var User = _Services.GetProfile(Result.UserId);

                if(Result == null)
                {
                    return BadRequest("Account cannot be authenticated");
                }
                var CookieOptions = CookieUtils.GenerateSecureCookieOptions(Result.ExpiresAt);
                Response.Cookies.Append("Session_Token", Result.SessionToken, CookieOptions);
                return Ok(User);
            }catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("VerifyEmail")]

        public async Task<ActionResult<Sesion>> VerifyEmail([FromBody] EmailVerificationInput I)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var Result = await _Services.VerifyEmail(I);
                var CookieOptions = CookieUtils.GenerateSecureCookieOptions(Result.ExpiresAt);
                Response.Cookies.Append("Session_Token", Result.SessionToken, CookieOptions);
                
                return NoContent();
            }catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error ocurried during Verification");
            }
        }

        [HttpPost]
        [Route("DeleteVerificationToken")]
        public async Task<ActionResult<bool>> DeleteVerificationToken([FromBody] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var Token = await _Services.DeleteEmailVerificationToken(id);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<ActionResult<bool>> DeleteUser([FromBody] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _Services.DeleteAccount(id);

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetUser")]
        public async Task<ActionResult<UserAccountProfile>> GetDefaultUser([FromQuery] int UserId)
        {
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var Result = await _Services.GetDefaultUser(UserId);

                return Ok(Result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("SearchUser")]
        public async Task<ActionResult<List<UserAccountProfile>>> SearchUsersInput
            ([FromQuery] string Username, int Limit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var QueryResult = await _Services.SearchUsersByUserName(Username, Limit);

                return Ok(QueryResult);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
