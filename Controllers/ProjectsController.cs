using AerTaskAPI.Services;
using AerTaskAPI.Shared.Context;
using AerTaskAPI.Shared.Inputs;
using AerTaskAPI.Shared.Models;
using AerTaskAPI.Shared.Models.Tables;
using AerTaskAPI.Utils.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AerTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly ProjectsService _Service;
        private readonly Sessions _Sesions;
        private readonly AerTaskDbContext _Context;
        public ProjectsController(ProjectsService _Service, Sessions _Sesions, AerTaskDbContext _Context)
        {
            this._Service = _Service;
            this._Sesions = _Sesions;
            this._Context = _Context;
        }

        [HttpGet]
        [Route("GetUserProjects")]
        public async Task<ActionResult<List<Project>>> GetAllUsersProjects([FromQuery]int UserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var ProjectList = await _Service.GetUserProjects(UserId);

                return Ok(ProjectList);
            }catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }catch(InvalidDataException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("CreateProject")]
        public async Task<ActionResult<Project>> CreateNewProject([FromBody] CreateProject Input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var Result = await _Service.CreateNewProject(Input);

                return Created("Project Created Succesfully:", Result);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound("Some element was missed: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetProject")]
        public async Task<ActionResult<Project>> GetProject([FromQuery] int ProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var Result = await _Service.GetDefaultProject(ProjectId);

                return Ok(Result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteProject")]
        public async Task<ActionResult> DeleteProject([FromBody] int ProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var Result = await _Service.DeleteProject(ProjectId);

                return NoContent();
            }catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetProjectUsers")]
        public async Task<ActionResult<List<ProjectUserShow>>> GetProjectUsers([FromQuery] int ProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var Response = await _Service.GetAllProjectUsers(ProjectId);

                return Ok(Response);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error ocurried");
            }
        }
        [HttpGet]
        [Route("GetProjectWithUser")]
        public async Task<ActionResult<List<Project>>> GetProjectsWithUser([FromQuery] int UserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var Result = await _Service.GetAllProjectsWhereUsarParticipate(UserId);

                return Result;
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("AddCollaborator")]
        public async Task<ActionResult<ProjectUser>> AddNewCollaborator([FromBody] AddColaborator I)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var Result = await _Service.AddNewCollaborator(I);

                return Created("Created succesfully: ", Result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }catch(InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Route("ChangeRole")]
        public async Task<ActionResult<ProjectUser>> ChangeUserRole([FromBody] ChangeCollaborator I)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var UpdateResult = _Service.UpdateUserRole(I);

                return Ok(UpdateResult);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }catch(InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteFromProject")]
        public async Task<ActionResult> DeleteUserFromProject([FromQuery] int ProjectId, int UserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                await _Service.DeleteUserFromProject(UserId, ProjectId);

                return NoContent();
            }catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("CheckUser")]
        public async Task<ActionResult<ProjectUser>> GetDefaultProjectUser([FromQuery] int ProjectId, int UserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var User = await _Service.CheckUserInProject(ProjectId, UserId);

                return Ok(User);
            }catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    } 
}
