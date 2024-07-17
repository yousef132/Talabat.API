namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            this.StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            string message = string.Empty;
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Resources Not Found",
                500 => "Server Side Error",
                _ => null
            };
        }
    }
}