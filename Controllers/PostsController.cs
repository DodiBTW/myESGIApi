using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
using Utils;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;
namespace MyESGIApi.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostsController : ControllerBase 
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            try
            {
                IEnumerable<Post> posts = await Task.Run(() => PostsHelper.GetPosts());
                return Ok(posts);
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Post?>> GetById(int id)
        {
            try
            {
                Post? post = await Task.Run(() => PostsHelper.GetPostById(id));
                if (post == null) return NotFound();
                return Ok(post);
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetByAuthor(int authorId)
        {
            try
            {
                IEnumerable<Post> posts = await Task.Run(() => PostsHelper.GetPostsByAuthorId(authorId));
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
        [Authorize]
        [HttpGet("getuserposts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetUserPosts()
        {
            try
            {
                string? userId = HttpContext.User.FindFirst("UserId")?.Value;
                if (userId == null) return new StatusCodeResult(404);
                    var posts = await Task.Run(() => PostsHelper.GetPostsByAuthorId(int.Parse(userId)));
                    return Ok(posts);
                }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
        [Authorize]
        [HttpPost(Name = "createpost")]
        public async Task<IActionResult> CreatePost(Post post)
        {
            string? userId = HttpContext.User.FindFirst("UserId")?.Value;
            if (userId == null) return Unauthorized("User not logged in");
            post.AuthorId = int.Parse(userId);
            var fsPost = post.ConvertToFsharpPost();
            if (!Utils.FormatChecker.CheckPostValid(fsPost))
            {
                return new BadRequestObjectResult("Invalid post format");
            }
            bool created = await PostsHelper.CreatePost(post);
            return created ? new OkResult() : new StatusCodeResult(500);
        }
    }
}
