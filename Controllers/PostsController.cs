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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Post>>> Get()
        {
            try
            {
                IEnumerable<Models.Post> posts = await Task.Run(() => PostHelper.GetPosts());
                return Ok(posts);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Post?>> GetById(int id)
        {
            try
            {
                Models.Post? post = await Task.Run(() => PostHelper.GetPostById(id));
                if (post == null) return NotFound();
                return Ok(post);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<Models.Post>>> GetByAuthor(int authorId)
        {
            try
            {
                IEnumerable<Models.Post> posts = await Task.Run(() => PostHelper.GetPostsByAuthorId(authorId));
                return Ok(posts);
            }
            catch
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
                    var posts = await Task.Run(() => PostHelper.GetPostsByAuthorId(int.Parse(userId)));
                    return Ok(posts);
                }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
        [HttpPost(Name = "createpost")]
        public async Task<IActionResult> CreatePost(string title, string description, string? imgurl)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            var post = new Models.Post(0,title,description,user.Id, imgurl, DateTime.UtcNow);
            var fsPost = post.ConvertToFsharpPost();
            if (!Utils.FormatChecker.CheckPostValid(fsPost))
            {
                return new BadRequestObjectResult("Invalid post format");
            }
            bool created = await PostHelper.CreatePost(post);
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
            return Ok(new { favorites = await PostHelper.GetFavoritePosts(user) });
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
            if (!await PostHelper.FavoritePost(postId, user.Id))
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
            if (!await PostHelper.UnfavoritePost(postId, user.Id))
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
            return Ok(new { isFavorite = await PostHelper.CheckIfFavoritePost(postId, user.Id) });
        }
        [Authorize]
        [HttpDelete("DeletePost")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
                return Unauthorized("User not logged in");
            var user = UserHelper.GetUserByEmail(userEmail);
            if (user == null)
                return new NotFoundObjectResult("User not found");
            var post = await PostHelper.GetPostById(postId);
            if (post == null)
                return new NotFoundObjectResult("Post not found");
            if (post.AuthorId != user.Id)
                return new UnauthorizedObjectResult("User not authorized to delete post");
            if (!await PostHelper.DeletePost(postId))
                return new StatusCodeResult(500);
            return new OkResult();
        }
    }
}