﻿using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure.Cart_Repository;
using Talabat.Infrastructure.Generic_Repository;

namespace Talabat.APIs.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(ICartRepository), typeof(CartRepository));
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
