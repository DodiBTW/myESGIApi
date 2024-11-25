using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using myESGIApi.Data;
using myESGIApi.Models;
namespace myESGIApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController
    {
        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> Get()
        {
            // This is the function for when we have the url {oururl}/Posts/
            return UserHelper.GetUsers();
        }
    }
}
