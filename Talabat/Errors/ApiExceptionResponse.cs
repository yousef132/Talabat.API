namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public ApiExceptionResponse(int statusCode, string? message = null, string? Description = null) : base(500)
        {
            this.Description = Description;
        }
        public string? Description { get; set; }
    }
}
