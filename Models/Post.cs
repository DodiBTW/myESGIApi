using MyESGIApi.Data;

namespace MyESGIApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime PostDate { get; set; }
        public Post() { } // Used for automatic dapper object creation
        public Post(int id, string title, string? description, int authorId, string? imgUrl, DateTime postDate)
        {
            Id = id;
            Title = title;
            Description = description;
            AuthorId = authorId;
            ImgUrl = imgUrl;
            PostDate = postDate;
        }

        public User? GetPostAuthor()
        {
            return UserHelper.GetUserById(AuthorId);
        }
    }
}
