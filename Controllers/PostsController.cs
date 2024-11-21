using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using myESGIApi.Data;
namespace myESGIApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController
    {
        [HttpGet(Name = "GetPosts")]
        public IEnumerable<Post> Get()
        {
            // This is the function for when we have the url {oururl}/Posts/
            return PostsHelper.GetPosts();
        }
    }
}
