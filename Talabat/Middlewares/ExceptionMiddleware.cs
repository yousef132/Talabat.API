
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    //  factory based by implementing IMiddleware interface 
    // By Convention , name must end with middleware and has a task async invoke(httpcontext) 
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IWebHostEnvironment webHost;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment webHost)
        {
            this.next = next;
            this.logger = logger;
            this.webHost = webHost;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // take some actions with the request 
                await next.Invoke(context);   // go to the next middleware

                // take some actions with the response
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);// development environment

                //log exception in (database | file) in production environment


                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";


                var response = webHost.IsDevelopment() ?
                    new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionResponse(500);


                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


                var json = JsonSerializer.Serialize(response, options);


                await context.Response.WriteAsync(json);


            }
        }
    }
}
