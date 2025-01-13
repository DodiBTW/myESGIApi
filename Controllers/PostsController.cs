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
        public async Task<ActionResult<IEnumerable<Models.Post>>> Get()
        {
            try
            {
                IEnumerable<Models.Post> posts = await Task.Run(() => PostsHelper.GetPosts());
                return Ok(posts);
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Post?>> GetById(int id)
        {
            try
            {
                Models.Post? post = await Task.Run(() => PostsHelper.GetPostById(id));
                if (post == null) return NotFound();
                return Ok(post);
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<Models.Post>>> GetByAuthor(int authorId)
        {
            try
            {
                IEnumerable<Models.Post> posts = await Task.Run(() => PostsHelper.GetPostsByAuthorId(authorId));
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
        [Authorize]
        [HttpGet("getuserposts")]
        public async Task<ActionResult<IEnumerable<Models.Post>>> GetUserPosts()
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
        public async Task<IActionResult> CreatePost(Models.Post post)
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
        public async Task<IActionResult> GetUserFavorites()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized("User not logged in");
            var user = await UserHelper.GetUserByEmail(email);
            if (user == null) return new NotFoundObjectResult("User not found");
            return Ok(new { favorites = await PostsHelper.GetFavoritePosts(user) });
        }
        [Authorize]
        [HttpPost("FavoritePost")]
        public async Task<IActionResult> FavoritePost(int postId)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            if (!await PostsHelper.FavoritePost(postId, user.Id))
                return new BadRequestObjectResult("Post not found");
            return new OkResult();
        }
        [Authorize]
        [HttpPost("UnfavoritePost")]
        public async Task<IActionResult> UnfavoritePost(int postId)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = await UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            if (!await PostsHelper.UnfavoritePost(postId, user.Id))
                return new BadRequestObjectResult("Post not found");
            return new OkResult();
        }
        [Authorize]
        [HttpPost("CheckIfFavorite")]
        public async Task<IActionResult> CheckIfFavorite(int postId)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            return Ok(new { isFavorite = await PostsHelper.CheckIfFavoritePost(postId, user.Id) });
        }
    }
}
