using AerTaskAPI.Services;
using AerTaskAPI.Shared.Context;
using AerTaskAPI.Shared.Methods;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
string ConnectionString = DbDataUtils.GetStringConnection(
    Environment.GetEnvironmentVariable("DBUSERLAYOUT_HOST"),
    Environment.GetEnvironmentVariable("DBUSERLAYOUT_PASS"),
    Environment.GetEnvironmentVariable("DBUSERLAYOUT_OWN"),
    Convert.ToInt32(Environment.GetEnvironmentVariable("DBUSERLAYOUT_PORT")),
    Environment.GetEnvironmentVariable("DBUSERLAYOUT_NAME")
    );
builder.Services.AddControllers();
builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<Sessions>();
builder.Services.AddScoped<ProjectsService>();
builder.Services.AddScoped<TasksService>();
builder.Services.AddDbContext<AerTaskDbContext>(options => options.UseNpgsql(ConnectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")
                   .AllowCredentials()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCookiePolicy();

app.Run();
