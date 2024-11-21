namespace myESGIApi
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime PostDate { get; set; }
        public Post(int id, string title, string description, int authorId, string img_url, DateTime post_date)
        {
            Id = id;
            Title = title;
            Description = description;
            AuthorId = authorId;
            ImgUrl = img_url;
            PostDate = post_date;
        }
    }
}
