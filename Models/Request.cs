using MyESGIApi.Data;

namespace MyESGIApi.Models
{
    public class Request
    {
        int Id { get; set; }
        public int AuthorId { get; set; }
        string Comment { get; set; }
        public string Status { get; set; }
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
        public async Task<bool>SendCreateRequest()
        {
            return await RequestHelper.CreateRequest(this);
        }
        public async Task<bool> UpdateRequest()
        {
            return await RequestHelper.UpdateRequest(this);
        }
    }
}
