using AerTaskAPI.Shared.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace AerTaskAPI.Shared.Context
{
    public class AerTaskDbContext : DbContext
    {
        public AerTaskDbContext(DbContextOptions options) : base(options) { }

        public DbSet<UserAccount> Users { get; set; }
        public DbSet<EmailVerificationForm> TempVefCodes { get; set; }
        public DbSet<Sesion> Sesions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
    }
}
