using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
using MyESGIApi.Services;
namespace MyESGIApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login(string email, string password)
        {
            if (!UserHelper.UserExists(email)) 
                return new NotFoundObjectResult("User does not exist");
            User user = UserHelper.GetUserByEmail(email);
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
        public IActionResult Register(string FirstName, string LastName, string Email, string Password)
        {
            string resp = UserHelper.UserRegister(FirstName, LastName, Email, Password);
            if (resp == "Account successfully created"){
                User? user = UserHelper.GetUserByEmail(Email);
                if (user == null) return new NotFoundObjectResult("User not found");
                return new OkObjectResult(new { token = JWTHelper.GenerateJWT(user) });
            }
            return new NotFoundObjectResult(resp);
        }
        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> Get()
        {
            return UserHelper.GetUsers();
        }

        [HttpGet("{id}")]
        public User? Get(int id)
        {
            return UserHelper.GetUserById(id);
        }


    }
}
