using AerTaskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AerTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SesionsController : Controller
    {
        private readonly Sessions _Service;

        public SesionsController(Sessions _Service)
        {
            this._Service = _Service;
        }

        [HttpPost]
        [Route("Validate")]
        public async Task<ActionResult<Boolean>> ValidateSession()
        {
            try
            {
                string Cookie = Request.Cookies["Session_Token"];

                if (!_Service.IsValidToken(Cookie))
                {
                    return BadRequest("Void Session");
                }

                var Result = await _Service.VerificateTokenAndGetProfileIfValid(Cookie);

                if(Result == null)
                {
                    return Unauthorized("Token not valid");
                }
                return Ok(Result);
            }catch(InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
