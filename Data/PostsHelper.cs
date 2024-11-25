using System;
using System.Collections.Generic;
using Dapper;
using myESGIApi.Models;

namespace myESGIApi.Data
{
    public class PostsHelper
    {
        public static IEnumerable<Post> GetPosts()
        {
            using var connection = DatabaseHelper.GetConnection();
            return connection.Query<Post>("SELECT * FROM Posts");
        }
        public static Post? GetPostById(int id)
        {
            using var connection = DatabaseHelper.GetConnection();
            return connection.QueryFirstOrDefault<Post>("SELECT * FROM Posts WHERE Id = @Id", new { Id = id });
        }
        public static IEnumerable<Post> GetPostsByAuthorId(int authorId)
        {
            using var connection = DatabaseHelper.GetConnection();
            return connection.Query<Post>("SELECT * FROM Posts WHERE AuthorId = @AuthorId ORDER BY Date DESC", new { AuthorId = authorId });
        }
    }
}
