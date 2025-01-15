using Xunit;
using MyESGIApi.Controllers;
using MyESGIApi.Models;
using MyESGIApi.Data;
using DotNetEnv;
namespace MyESGIApi.Tests
{
    public class PostsTester
    {
        [Fact]
        public async Task GetAuthorFromPostInstance()
        {
            Post? post = new Post(1, "Title", "Description", 1, "ImgUrl", DateTime.Now);
            User? author = await post.GetPostAuthor();
            Assert.NotNull(author);
            Assert.Equal(1, author!.Id);
        }
    }
}
