using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyESGIApi.Data;
using MyESGIApi.Models;
using Utils;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static Utils.Models;
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
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            post.AuthorId = user.Id;
            var fsPost = post.ConvertToFsharpPost();
            if (!Utils.FormatChecker.CheckPostValid(fsPost))
            {
                return new BadRequestObjectResult("Invalid post format");
            }
            bool created = await PostsHelper.CreatePost(post);
            return created ? new OkResult() : new StatusCodeResult(500);
        }
        [Authorize]
        [HttpGet("GetFavorites")]
        public IActionResult GetUserFavorites()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(email);
            return Ok(new { favorites = PostsHelper.GetFavoritePosts(user) });
        }
        [Authorize]
        [HttpPost("FavoritePost")]
        public IActionResult FavoritePost(int postId)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            if (!PostsHelper.FavoritePost(postId, user.Id))
                return new BadRequestObjectResult("Post not found");
            return new OkResult();
        }
        [Authorize]
        [HttpPost("UnfavoritePost")]
        public IActionResult UnfavoritePost(int postId)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            if (!PostsHelper.UnfavoritePost(postId, user.Id))
                return new BadRequestObjectResult("Post not found");
            return new OkResult();
        }
        [Authorize]
        [HttpPost("CheckIfFavorite")]
        public IActionResult CheckIfFavorite(int postId)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            return Ok(new { isFavorite = PostsHelper.CheckIfFavoritePost(postId, user.Id) });
        }
    }
}
