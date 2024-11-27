using BCrypt.Net;
using MyESGIApi.Data;
namespace MyESGIApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string? Password { get; set; }
        public string Role { get; set; }
        public DateTime? JoinDate { get; set; }
        public string EmailAdress { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public User() {}
        public User(int id, string firstName, string lastName, string email_address,string role, string? password = null, DateTime? dateJoined = null,  string?  profile_picture_url = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Role = role;
            JoinDate = dateJoined;
            EmailAdress = email_address;
            ProfilePictureUrl = profile_picture_url;
        }
        public bool CheckPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }
        public void UpdatePassword(string password)
        {
            string newPass = BCrypt.Net.BCrypt.HashPassword(password);
            this.Password = newPass;
            UserHelper.UpdateUserPassword(Id, newPass);
        }
    }
}
