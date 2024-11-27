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
    }
}
