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
        [HttpGet(Name = "GetPosts")]
        public IEnumerable<Models.Post> Get()
        {
            return PostsHelper.GetPosts();
        }
        [HttpGet("{id}")]
        public Models.Post? GetById(int id)
        {
            return PostsHelper.GetPostById(id);
        }
        [HttpGet("author/{authorId}")]
        public IEnumerable<Models.Post> GetByAuthor(int authorId)
        {
            return PostsHelper.GetPostsByAuthorId(authorId);
        }
        [Authorize]
        [HttpPost(Name = "CreatePost")]
        public IActionResult CreatePost(Models.Post post)
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
            PostsHelper.CreatePost(post);
            return new OkResult();
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
