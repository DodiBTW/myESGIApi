using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
namespace MyESGIApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController
    {
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
        [HttpPost(Name = "register")]
        public string Register(string FirstName, string LastName, string Email, string Password)
        {
            //return UserHelper.UserRegister(user.FirstName, user.LastName, user.EmailAdress, user.Password, user.ProfilePictureUrl, user.Role);
            return UserHelper.UserRegister(FirstName, LastName, Email, Password);
        }
    }
}
