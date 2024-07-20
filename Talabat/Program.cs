using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Extentions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.Identity;
using Talabat.Infrastructure.Identity.DataSeed;

namespace Talabat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
            });

            // made the lifetime singleton to cache the response at any time 
            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configs = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(configs);
            });

            builder.Services.AddApplicationServices()
                .AddAuthServices(builder.Configuration);
            builder.Services.AddSwaggerServices().AddSwaggerDocumentation();



            var app = builder.Build();
            #region Apply All Pending Migrations
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await context.Database.MigrateAsync();// apply migrations before running the app
                await StoreContextSeed.SeedAsync(context);
                await identityDbContext.Database.MigrateAsync();

                await ApplicationIdentityDataSeeding.SeedUserAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.ToString(), "Error During Apply Migrations");
            }

            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerService();

            }
            app.UseMiddleware<ExceptionMiddleware>();


            // execute errors controller when request path not found, unauthorized and bad request   
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
