using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
using Utils;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static Utils.Models;
namespace MyESGIApi.Controllers
{
    [ApiController]
    [Route("requests")]
    public class RequestsController : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create(string comment)
        {
            string? email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized("Invalid Credentials");
            var user = await UserHelper.GetUserByEmail(email);
            if (user == null) return new NotFoundObjectResult("User not found");
            var equipment = new List<Equipment>();
            Request request = new Request(0, user.Id, comment, "Pending", DateTime.Now, equipment);
            int requestId = await RequestHelper.CreateRequest(request);
            if (requestId == 1) return new OkObjectResult(new { message = "Request created successfully" });
            return new StatusCodeResult(500);
        }
        [Authorize]
        [HttpPost("AddEquipment")]
        public async Task<IActionResult> AddEquipment(int requestId, string equipmentName)
        {
            string? email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized("Invalid");
            Request? request = await RequestHelper.GetRequest(requestId);
            if (request == null) return new NotFoundObjectResult("Request not found");
            request.FetchEquipment();
            var newEquipment = new Equipment(equipmentName);
            request.AddEquipment(newEquipment);
            await RequestHelper.UpdateRequest(request);
            return new OkObjectResult(new { message = "Equipment added successfully" });
        }
        [HttpGet(Name = "GetRequests")]
        public async Task<IEnumerable<Request>> Get()
        {
            return await RequestHelper.GetRequests();
        }
        [HttpGet("{id}")]
        public async Task<Request?> Get(int id)
        {
            return await RequestHelper.GetRequest(id);
        }
        [Authorize]
        [HttpGet("getuserrequests")]
        public async Task<ActionResult<IEnumerable<Request>>> GetUserRequests()
        {
            string? email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized("Invalid Credentials");
            var user = await UserHelper.GetUserByEmail(email);
            if (user == null) return new NotFoundObjectResult("User not found");
            var request = await RequestHelper.GetRequestsByAuthor(user.Id);
            return Ok(request);
        }
        [Authorize]
        [HttpPost("Update")]
        public async Task<IActionResult> Update(int id, string description)
        {
            string? email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized("Invalid Credentials");
            var user = await UserHelper.GetUserByEmail(email);
            if (user == null) return new NotFoundObjectResult("User not found");
            Request? request = await RequestHelper.GetRequest(id);
            if (request == null) return new NotFoundObjectResult("Request not found");
            if (request.IsAuthor(user.Id)) return Unauthorized("Invalid Credentials");
            request.SetComment(description);
            await RequestHelper.UpdateRequest(request);
            return new OkObjectResult(new { message = "Request updated successfully" });
        }
        [Authorize]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            string? email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized("Invalid");
            var user = await UserHelper.GetUserByEmail(email);
            if (user == null) return new NotFoundObjectResult("User not found");
            Request? request = await RequestHelper.GetRequest(id);
            if (request == null) return new NotFoundObjectResult("Request not found");
            if (request.IsAuthor(user.Id)) return Unauthorized("Invalid");
            await request.Delete();
            return new OkObjectResult(new { message = "Request deleted successfully" });
        }
    }
}
