using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
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
            User? user = UserHelper.GetUserByEmail(email);
            if (user != null && user.CheckPassword(password))
                // Success
                return new OkObjectResult("Helo");
            return new NotFoundObjectResult("Invalid Credentials");
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

        [HttpPost("register")]
        public IActionResult Register(string FirstName, string LastName, string Email, string Password)
        {
            string resp = UserHelper.UserRegister(FirstName, LastName, Email, Password);
            if (resp == "Account successfully created")
                return CreatedAtAction(nameof(Get), new { id = Email }, new { message = resp });
            return new NotFoundObjectResult(resp);
        }
    }
}
