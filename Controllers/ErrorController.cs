using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ManagementDatabase.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/Home/Error")]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            // Log the exception here (e.g., using a logging framework)

            return Problem(
                detail: exception?.Message,
                statusCode: 500,
                title: "An unexpected error occurred!");
        }
    }
}
