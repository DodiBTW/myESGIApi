namespace myESGIApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public int AuthorId { get; set; }
        public string? Role { get; set; }
        public DateTime? JoinDate { get; set; }
        public string EmailAdress { get; set; }
        public User(int id, string first_name, string last_name, string role, DateTime date_joined, string email_address)
        {
            Id = id;
            FirstName = first_name;
            LastName = last_name;
            Role = role;
            JoinDate = date_joined;
            EmailAdress = email_address;
        }
    }
}
