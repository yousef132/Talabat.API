using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;

namespace Talabat.APIs.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // services.AddAutoMapper(m => m.AddProfile(new MappingProfiles())); 
            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = (ActionContext) =>
                {

                    var errors = ActionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                     .SelectMany(p => p.Value.Errors)
                     .Select(e => e.ErrorMessage)
                     .ToArray();


                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);

                };
            });

            return services;
        }
    }
}
