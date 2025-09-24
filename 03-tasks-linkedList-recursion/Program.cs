using TaskSchedulerDariaSt.Services;
using TaskSchedulerDariaSt.Models;

namespace TaskSchedulerDariaSt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Database configuration
            builder.Services.Configure<TaskSchedulerDatabaseSettings>(
               builder.Configuration.GetSection("TaskSchedulerDatabase"));

            // Session configuration
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".TestState.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(120);
                options.Cookie.IsEssential = true;
            });

            
            // DI Reqistration
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<ITasksService, TasksService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
