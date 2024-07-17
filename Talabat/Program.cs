using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Extentions;
using Talabat.APIs.Middlewares;
using Talabat.Repository.Data;

namespace Talabat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddSwaggerServices();


            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await context.Database.MigrateAsync();// apply migrations before running the app
                await StoreContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.ToString(), "Error During Apply Migrations");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerService();

            }
            app.UseMiddleware<ExceptionMiddleware>();

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("FIRst Request");
            //    await next.Invoke();
            //    await context.Response.WriteAsync("first response");
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("second Request");
            //    await next.Invoke();
            //    await context.Response.WriteAsync("second response");
            //});
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("third Request");
            //    await next.Invoke();
            //    await context.Response.WriteAsync("third response");
            //});

            // execute errors controller when request path not found, unauthorized,bad request   
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
