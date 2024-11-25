using System;
using System.Collections.Generic;
using Dapper;
using myESGIApi.Models;

namespace myESGIApi.Data
{
    public class UserHelper
    {
        public static IEnumerable<User> GetUsers()
        {
            using var connection = DatabaseHelper.GetConnection();
            return connection.Query<User>("SELECT * FROM Users");
        }
        public static User? GetUserById(int id)
        {
            using var connection = DatabaseHelper.GetConnection();
            return connection.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
        }
        //public static IEnumerable<Post> UserLogin(int authorId)
        //{
        //    using var connection = DatabaseHelper.GetConnection();
        //    return connection.Query<Post>("SELECT * FROM Posts WHERE AuthorId = @AuthorId ORDER BY Date DESC", new { AuthorId = authorId });
        //}
    }
}
