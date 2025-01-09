using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
using Utils;
using Microsoft.AspNetCore.Authorization;
namespace MyESGIApi.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostsController : ControllerBase 
    {
        [HttpGet(Name = "GetPosts")]
        public IEnumerable<Post> Get()
        {
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
        [Authorize]
        [HttpPost(Name = "CreatePost")]
        public IActionResult CreatePost(Post post)
        {
            string? userId = HttpContext.User.FindFirst("UserId")?.Value;
            if (userId == null) return Unauthorized("User not logged in");
            post.AuthorId = int.Parse(userId);
            var fsPost = post.ConvertToFsharpPost();
            if (!Utils.FormatChecker.CheckPostValid(fsPost))
            {
                return new BadRequestObjectResult("Invalid post format");
            }
            PostsHelper.CreatePost(post);
            return new OkResult();
        }

    }
}
