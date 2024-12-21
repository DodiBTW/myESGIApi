using Xunit;
using MyESGIApi.Data;
using MyESGIApi.Models;
using MyESGIApi.Controllers;
using DotNetEnv;

namespace MyESGIApi.Tests
{
    public class UsersTester
    {
        [Fact]
        public void TestUserPasswordChange()
        {
            User user = new User(1, "Ahmad", "Daoud", "contact@adaoud.dev", "etudiant", "password", DateTime.Now);
            user.UpdatePassword("newPassword");
            Assert.True(user.CheckPassword("newPassword"));
        }
    }
}
