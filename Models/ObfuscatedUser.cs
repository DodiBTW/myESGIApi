namespace MyESGIApi.Models
{
    public class ObfuscatedUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public ObfuscatedUser(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfilePictureUrl = user.ProfilePictureUrl;
        }
    }
}
