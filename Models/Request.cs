namespace MyESGIApi.Models
{
    public class Request
    {
        int Id { get; set; }
        int AuthorId { get; set; }
        string Comment { get; set; }
        string Status { get; set; }
        DateTime RequestDate { get; set; }
        public Request() { }
        public Request(int id, int authorId, string comment, string status, DateTime requestDate)
        {
            Id = id;
            AuthorId = authorId;
            Comment = comment;
            Status = status;
            RequestDate = requestDate;
        }
    }
}
