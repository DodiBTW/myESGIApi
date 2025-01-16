using Microsoft.AspNetCore.Mvc;
using MyESGIApi.Models;
using MyESGIApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace MyESGIApi.Controllers
{
    [ApiController]
    [Route("requests")]
    public class RequestsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return new UnauthorizedResult();
            var user = await UserHelper.GetUserByEmail(email);
            if (user == null) return new NotFoundObjectResult("User not found");
            if (user.Role != "admin") return new UnauthorizedResult();
            var requests = await RequestHelper.GetRequests();
            return new OkObjectResult(requests);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(Request request)
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return new UnauthorizedResult();
            var user = await UserHelper.GetUserByEmail(email);
            if (user == null) return new NotFoundObjectResult("User not found");
            request.AuthorId = user.Id;
            bool response = await request.SendCreateRequest();
            if (response == false) return new BadRequestObjectResult("Invalid request");
            return new OkObjectResult(new { message = "Request created" });
        }
    }
}
