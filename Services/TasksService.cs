using AerTaskAPI.Shared.Context;
using AerTaskAPI.Shared.Inputs;
using AerTaskAPI.Shared.Models;
using AerTaskAPI.Shared.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace AerTaskAPI.Services
{
    public class TasksService
    {
        private readonly AerTaskDbContext _Context;
        public TasksService(AerTaskDbContext context)
        {
            _Context = context;
        }

        public async Task<List<ProjectTask_User>> GetAllProjectTasks(int ProjectId)
        {
            try
            {
                var tasks = await _Context.ProjectTasks
                .Where(p => p.ProjectId == ProjectId)
                .Join(_Context.Users,
                      task => task.AddByUser,
                      user => user.UserId,
                      (task, user) => new ProjectTask_User
                           {
                            Task = task,
                            Profile = user.ToProfile()
                           })
                      .ToListAsync();

                return tasks;

            }
            catch (Exception)
            {
                throw new Exception("An error ocurried while getting tasks");
            }
        }
        public async Task<ProjectTask> CreateProjectTask(CreateProjectTask C)
        {
            try
            {
                var User = await _Context.Users.FindAsync(C.UserId);
                var Project = await _Context.Projects.FindAsync(C.ProjectId);
                if(User == null || Project == null)
                {
                    throw new InvalidOperationException("User or Project not exists");
                }
                var NewTask = C.GenerateProjectTask();
                await _Context.ProjectTasks.AddAsync(NewTask);
                await _Context.SaveChangesAsync();

                return NewTask;
            }
            catch (InvalidOperationException)
            {
                throw;
            }catch (Exception)
            {
                throw new Exception("An error ocurried.");
            }
        }
        public async Task DeleteTaskFromProject(int ProjectTaskId)
        {
            try
            {
                var ProjectTask = await _Context.ProjectTasks.FindAsync(ProjectTaskId);
                if(ProjectTask == null)
                {
                    throw new InvalidOperationException("Task Not found");
                }
                _Context.ProjectTasks.Remove(ProjectTask);
                await _Context.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried.");
            }
        }
    }
}
