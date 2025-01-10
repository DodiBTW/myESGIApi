using System;
using System.Collections.Generic;
using Dapper;
using MyESGIApi.Models;
using static MyESGIApi.Data.DatabaseHelper;
namespace MyESGIApi.Data
{
    public class PostsHelper
    {
        public static IEnumerable<Post> GetPosts()
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
            return connection.Query<Post>(query);
        }

        public static Post? GetPostById(int id)
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
            return connection.QueryFirstOrDefault<Post>(query, new { Id = id });
        }

        public static IEnumerable<Post> GetPostsByAuthorId(int authorId)
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
            return connection.Query<Post>(query, new { AuthorId = authorId });
        }
        public static bool CreatePost(Post post)
        {
            using var connection = GetConnection();
            string query = @"
                INSERT INTO posts (title, description, authorId, img_url, post_date)
                VALUES (@Title, @Description, @AuthorId, @ImgUrl, @PostDate)
                ";
            return connection.Execute(query, post) > 0;
        }
        public static bool FavoritePost(int postId, int userId)
        {
            using var connection = GetConnection();
            string query = @"
                INSERT INTO favorites (postId, userId)
                VALUES (@PostId, @UserId)
                ";
            return connection.Execute(query, new { PostId = postId, UserId = userId }) > 0;
        }
        public static bool UnfavoritePost(int postId, int userId)
        {
            using var connection = GetConnection();
            string query = @"
                DELETE FROM favorites
                WHERE postId = @PostId AND userId = @UserId
                ";
            return connection.Execute(query, new { PostId = postId, UserId = userId }) > 0;
        }
        public static bool CheckIfFavoritePost(int postId, int userId)
        {
            using var connection = GetConnection();
            string query = @"
                SELECT COUNT(*) 
                FROM favorites
                WHERE postId = @PostId AND userId = @UserId
                ";
            return connection.ExecuteScalar<int>(query, new { PostId = postId, UserId = userId }) > 0;
        }
        public static List<Post> GetFavoritePosts(User user)
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
            return connection.Query<Post>(query, new { UserId = user.Id }).AsList();
        }
    }
}
