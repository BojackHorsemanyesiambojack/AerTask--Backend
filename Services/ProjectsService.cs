using AerTaskAPI.Shared.Context;
using AerTaskAPI.Shared.Inputs;
using AerTaskAPI.Shared.Models;
using AerTaskAPI.Shared.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace AerTaskAPI.Services
{
    public class ProjectsService
    {
        private readonly AerTaskDbContext _Context;
        public ProjectsService(AerTaskDbContext _Context)
        {
            this._Context = _Context;
        }

        public async Task<List<Project>> GetUserProjects(int id)
        {
            try
            {
                var User = await _Context.Users.FindAsync(id);
                if(User == null)
                {
                    throw new InvalidOperationException("Asociated User not exists");
                }
                var Search = await _Context.Projects.Where(p => p.UserId == User.UserId).ToListAsync();
                if(Search.Count == 0 || Search == null)
                {
                    return null;
                }

                return Search;
            }
            catch (InvalidOperationException)
            {
                throw;
            }catch(Exception ex)
            {
                throw new Exception("An error ocurried while tryng to search user projects");
            }
        }
        public async Task<Project> CreateNewProject(CreateProject Input)
        {
            try
            {
                var NewProject = Project.GenerateProject(Input);

                await _Context.Projects.AddAsync(NewProject);
                await _Context.SaveChangesAsync();
                var Creator = await _Context.Users.FindAsync(NewProject.UserId);
                if(Creator == null)
                {
                    throw new InvalidOperationException("An error ocurried, please try again.");
                }
                var NewCreator = NewProject.GenerateProjectCreator(Creator);
                await _Context.ProjectUsers.AddAsync(NewCreator);
                await _Context.SaveChangesAsync();

                return NewProject;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("A database error occurred while creating the project.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An error ocurried while creating project.");
            }
        }
        public async Task<Project> GetDefaultProject(int id)
        {
            try
            {
                var Result = await _Context.Projects.FindAsync(id);
                if(Result == null)
                {
                    throw new InvalidOperationException("Project not found");
                }
                return Result;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried while searching project...");
            }
        }
        public async Task<bool> DeleteProject(int ProjectId)
        {
            try
            {
                var Project = await _Context.Projects.FindAsync(ProjectId);
                if(Project == null)
                {
                    throw new InvalidOperationException("Project not found");
                }
                _Context.Projects.Remove(Project);

                return true;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried while deleting project");
            }
        }
        public async Task<List<ProjectUserShow>> GetAllProjectUsers(int ProjectId)
        {
            try
            {
                var Result = await _Context.ProjectUsers
                .Where(u => u.ProjectId == ProjectId)
                .Join(_Context.Users, pu => pu.UserId, u => u.UserId,
                (pu, u) => u.ToProjectShowing(pu.UserRole))
                .ToListAsync();
                return Result;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried getting users");
            }
        }
        public async Task<List<Project>> GetAllProjectsWhereUsarParticipate(int UserId)
        {
            try
            {
                var Result = await _Context.ProjectUsers.Where(p => p.UserId == UserId)
                    .Join(_Context.Projects, pu => pu.UserId, u=> u.UserId,(pu,u) => u).ToListAsync();

                return Result;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried while tryng to get projects");
            }
        }
        public async Task<ProjectUser> AddNewCollaborator(AddColaborator I) 
        {
            try
            {
                var UserExists = await _Context.Users.FindAsync(I.UserId);
                var ProjectExists = await _Context.Projects.FindAsync(I.ProjectId);
                if(UserExists == null || ProjectExists == null )
                {
                    throw new InvalidOperationException("Asociated User Not Exists");
                }
                var UserIsAlredyInTheProject = await _Context.ProjectUsers.
                    FirstOrDefaultAsync(pu => pu.ProjectId == ProjectExists.ProjectId &&
                    pu.UserId == UserExists.UserId);
                if (UserIsAlredyInTheProject != null)
                {
                    throw new InvalidDataException("User is alredy in the project");
                }
                var NewCollaborator = I.GenerateCollaborator();
                await _Context.ProjectUsers.AddAsync(NewCollaborator);
                await _Context.SaveChangesAsync();

                return NewCollaborator;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried");
            }
        }
        public async Task<ProjectUser> UpdateUserRole(ChangeCollaborator I)
        {
            try
            {
                var User = await _Context.Users.FindAsync(I.UserId);
                if(User == null)
                {
                    throw new InvalidOperationException("Asociated User not exists");
                }
                var UserIsInProject = await _Context.ProjectUsers
                    .FirstOrDefaultAsync(p => p.UserId == I.UserId && p.ProjectId == I.ProjectId);
                if(UserIsInProject == null)
                {
                    throw new InvalidOperationException("Asociated User is not in the actual project");
                }

                var UpdatedUser = UserIsInProject.ChangeUserRole(I.Role);
                _Context.ProjectUsers.Update(UpdatedUser);
                await _Context.SaveChangesAsync();

                return UpdatedUser;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried in the update.");
            }
        }
        public async Task DeleteUserFromProject (int UserId, int ProjectId)
        {
            try
            {
                var User = await _Context.Users.FindAsync(UserId);
                if(User == null)
                {
                    throw new InvalidOperationException("Asociated user does not exist.");
                }
                var UserIsInProject = await _Context.ProjectUsers
                    .FirstOrDefaultAsync(p => p.UserId == UserId && p.ProjectId == ProjectId);
                if (UserIsInProject == null)
                {
                    throw new InvalidOperationException("Asociated User is not in the actual project");
                }

                _Context.ProjectUsers.Remove(UserIsInProject);
                await _Context.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried while tryng to delete asociated user");
            }
        }
        public async Task<ProjectUser> CheckUserInProject(int ProjectId, int UserId)
        {
            try
            {
                Console.WriteLine($"UserId: {UserId}, ProjectId: {ProjectId}");
                var User = await _Context.ProjectUsers
                    .FirstOrDefaultAsync(pu => pu.UserId == UserId && pu.ProjectId == ProjectId);
                if(User == null)
                {
                    throw new InvalidOperationException("Asociated User is not in the project");
                }

                return User;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurried");
            }
        }
    }
}
