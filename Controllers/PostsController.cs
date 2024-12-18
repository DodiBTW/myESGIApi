using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
namespace MyESGIApi.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostsController
    {
        [HttpGet(Name = "GetPosts")]
        public IEnumerable<Post> Get()
        {
            // This is the function for when we have the url {oururl}/Posts/
            return PostsHelper.GetPosts();
        }
        [HttpGet("{id}")]
        public Post? GetById(int id)
        {
            return PostsHelper.GetPostById(id);
        }
        [HttpGet("author/{authorId}")]
        public IEnumerable<Post> GetByAuthor(int authorId)
        {
            return PostsHelper.GetPostsByAuthorId(authorId);
        }
    }
}
