using System;
using System.Collections.Generic;
using Dapper;
using MyESGIApi.Models;
using static MyESGIApi.Data.DatabaseHelper;
namespace MyESGIApi.Data
{
    public class PostsHelper
    {
        public static async Task<IEnumerable<Post>> GetPosts()
        {
            using var connection = GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    title AS Title, 
                    description AS Description, 
                    authorId, 
                    img_url AS ImgUrl, 
                    post_date AS PostDate
                FROM posts
                ORDER BY post_date DESC
                ";
            return await Task.Run(() => connection.Query<Post>(query));
        }

        public static async Task<Post?> GetPostById(int id)
        {
            using var connection = GetConnection();
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
            return await Task.Run(() => connection.QueryFirstOrDefault<Post>(query, new { Id = id }));
        }

        public static async Task<IEnumerable<Post>> GetPostsByAuthorId(int authorId)
        {
            using var connection = GetConnection();
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
            return await Task.Run(() => connection.Query<Post>(query, new { AuthorId = authorId }));
        }
        public static async Task<bool> CreatePost(Post post)
        {
            using var connection = GetConnection();
            string query = @"
                INSERT INTO posts (title, description, authorId, img_url, post_date)
                VALUES (@Title, @Description, @AuthorId, @ImgUrl, @PostDate)
                ";
            return await Task.Run(() => connection.Execute(query, post) > 0);
        }
        public static async Task<bool> FavoritePost(int postId, int userId)
        {
            using var connection = GetConnection();
            string query = @"
                INSERT INTO favorites (postId, userId)
                VALUES (@PostId, @UserId)
                ";
            return await Task.Run(() => connection.Execute(query, new { PostId = postId, UserId = userId })) > 0;
        }
        public static async Task<bool> UnfavoritePost(int postId, int userId)
        {
            using var connection = GetConnection();
            string query = @"
                DELETE FROM favorites
                WHERE postId = @PostId AND userId = @UserId
                ";
            return await Task.Run(() => connection.Execute(query, new { PostId = postId, UserId = userId })) > 0;
        }
        public static async Task<bool> CheckIfFavoritePost(int postId, int userId)
        {
            using var connection = GetConnection();
            string query = @"
                SELECT COUNT(*) 
                FROM favorites
                WHERE postId = @PostId AND userId = @UserId
                ";
            return await Task.Run(() => connection.ExecuteScalar<int>(query, new { PostId = postId, UserId = userId })) > 0;
        }
        public static async Task<List<Post>> GetFavoritePosts(User user)
        {
            using var connection = GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    title AS Title, 
                    description AS Description, 
                    authorId, 
                    img_url AS ImgUrl, 
                    post_date AS PostDate
                FROM posts
                WHERE id IN (
                    SELECT postId
                    FROM favorites
                    WHERE userId = @UserId
                )
                ORDER BY post_date DESC
                ";
            return await Task.Run(() => connection.Query<Post>(query, new { UserId = user.Id }).AsList());
        }
    }
}
