using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // ignore these endpoints , they are exist in the background
    public class ErrorsController : ControllerBase
    {
        [HttpGet]
        // if there is unauthorized request it will execute this endpoint
        // so we have to check the status code here 
        public ActionResult Error(int code)
        {
            if (code == 401)
                return Unauthorized();
            else if (code == 400)
                return BadRequest(new ApiResponse(400));
            else if (code == 404)
                return NotFound(new ApiResponse(code));

            return StatusCode(code);
        }
    }
}
