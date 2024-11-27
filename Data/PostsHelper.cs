using System;
using System.Collections.Generic;
using Dapper;
using MyESGIApi.Models;

namespace MyESGIApi.Data
{
    public class PostsHelper
    {
        public static IEnumerable<Post> GetPosts()
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    title AS Title, 
                    description AS Description, 
                    authorId, 
                    img_url AS ImgUrl, 
                    post_date AS PostDate
                FROM posts";
            return connection.Query<Post>(query);
        }

        public static Post? GetPostById(int id)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    title AS Title, 
                    description AS Description, 
                    authorId, 
                    img_url AS ImgUrl, 
                    post_date AS PostDate
                FROM posts
                WHERE id = @Id";
            return connection.QueryFirstOrDefault<Post>(query, new { Id = id });
        }

        public static IEnumerable<Post> GetPostsByAuthorId(int authorId)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    title AS Title, 
                    description AS Description, 
                    authorId, 
                    img_url AS ImgUrl, 
                    post_date AS PostDate
                FROM posts
                WHERE authorId = @AuthorId
                ORDER BY post_date DESC";
            return connection.Query<Post>(query, new { AuthorId = authorId });
        }
    }
}
