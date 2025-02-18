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
    [Route("equipment")]
    public class EquipmentController : ControllerBase 
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Equipment>>> Get()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null) return Unauthorized("Invalid Credentials");
            var user = await UserHelper.GetUserByEmail(userEmail);
            if (user == null) return Unauthorized("Invalid Credentials");
            if (!user.IsAdmin()) return Unauthorized("Not an admin");
            try
            {
                IEnumerable<Models.Equipment> equipment = await Task.Run(() => EquipmentHelper.GetAllEquipment());
                return Ok(equipment);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

    }
}