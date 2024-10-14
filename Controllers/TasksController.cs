using AerTaskAPI.Services;
using AerTaskAPI.Shared.Context;
using AerTaskAPI.Shared.Inputs;
using AerTaskAPI.Shared.Models;
using AerTaskAPI.Shared.Models.Tables;
using AerTaskAPI.Utils.Validations;
using Microsoft.AspNetCore.Mvc;

namespace AerTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly AerTaskDbContext _Context;
        private readonly TasksService _Service;
        public TasksController(AerTaskDbContext _Context, TasksService _Service) {
            this._Context = _Context;
            this._Service = _Service;
        }
        [HttpGet]
        [Route("GetProjectTasks")]
        public async Task<ActionResult<List<ProjectTask_User>>> GetAllProjectTasks([FromQuery] int ProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var TaskList = await _Service.GetAllProjectTasks(ProjectId);

                return Ok(TaskList);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("CreateProjectTask")]
        public async Task<ActionResult<ProjectTask>> CreateNewTaskInProject([FromBody] CreateProjectTask C)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            try
            {
                await SesionValidation.SearchAndValidateSesion(_Context, Request);
                var CreationResult = await _Service.CreateProjectTask(C);

                return Created("Task created succesfully", CreationResult);
            }catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteProjectTask")]
        public async Task<ActionResult> DeleteTaskFromProject([FromQuery] int ProjectTaskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _Service.DeleteTaskFromProject(ProjectTaskId);

                return NoContent();
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
    }
}
