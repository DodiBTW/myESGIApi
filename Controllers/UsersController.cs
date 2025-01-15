using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
using MyESGIApi.Services;
using System.Security.Claims;
namespace MyESGIApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            bool userExists = await UserHelper.UserExists(email);
            if (!userExists) 
                return new NotFoundObjectResult("User does not exist");
            User? user = await UserHelper.GetUserByEmail(email);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            if (!user.CheckPassword(password)) 
                return Unauthorized("Invalid Credentials");
            var token = JWTHelper.GenerateJWT(user);
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });
            return new OkObjectResult(new {message = "Login sucessful"});
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(string FirstName, string LastName, string Email, string Password)
        {
            bool created = await UserHelper.UserRegister(FirstName, LastName, Email, Password);
            if (created){
                User? user = await UserHelper.GetUserByEmail(Email);
                if (user == null) return new NotFoundObjectResult("User not found");
                var token = JWTHelper.GenerateJWT(user);
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                });
                return new OkObjectResult(new { message = "Registered sucessfully"});
            }
            return new StatusCodeResult(500);
        }
        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<User>> Get()
        {
            return await UserHelper.GetUsers();
        }

        [HttpGet("{id}")]
        public async Task<User?> Get(int id)
        {
            return await UserHelper.GetUserById(id);
        }
        [HttpGet("IsAdmin")]
        public IActionResult IsAdmin()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(email);
            if (user == null) return new NotFoundObjectResult("User not found");
            return Ok(new { isAdmin = user.IsAdmin() });
        }
    }
}
