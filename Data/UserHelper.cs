using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using MyESGIApi.Models;
using Utils;
namespace MyESGIApi.Data
{
    public class UserHelper
    {
        
        public static bool UserRegister(string first_name, string last_name, string email, string password, string? profile_picture_url = null, string? role = "student")
        {
            if (UserExists(email) || !FormatChecker.IsValidEmail(email) || !FormatChecker.IsValidPassword(password)) return false;

            password = BCrypt.Net.BCrypt.HashPassword(password);
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                INSERT INTO users (first_name, last_name, email_address, password, role, profile_picture_url)
                VALUES (@FirstName, @LastName, @Email, @Password, @Role, @ProfilePictureUrl)";
            connection.Execute(query, new { FirstName = first_name, LastName = last_name, Email = email, Password = password, Role = role, ProfilePictureUrl = profile_picture_url });
            return true;
        }
        public static bool UserExists(string email)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT COUNT(*) 
                FROM users 
                WHERE email_address = @Email";
            return connection.ExecuteScalar<int>(query, new { Email = email }) > 0;
        }
        public static User? GetUserById(int id)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    first_name AS FirstName, 
                    last_name AS LastName, 
                    password AS Password, 
                    role AS Role, 
                    date_joined AS JoinDate, 
                    email_address AS EmailAdress
                FROM users
                WHERE id = @Id";
            return connection.QueryFirstOrDefault<User>(query, new { Id = id });
        }
        public static IEnumerable<User> GetUsers()
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    first_name AS FirstName, 
                    last_name AS LastName, 
                    password AS Password, 
                    role AS Role, 
                    date_joined AS JoinDate, 
                    email_address AS EmailAdress,
                    profile_picture_url AS ProfilePictureUrl
                FROM users";
            return connection.Query<User>(query);
        }
        public static IEnumerable<User> GetUsersByString(string searchTerm)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    first_name AS FirstName, 
                    last_name AS LastName, 
                    password AS Password, 
                    role AS Role, 
                    date_joined AS JoinDate, 
                    email_address AS EmailAdress
                FROM users
                WHERE 
                    first_name LIKE @SearchTerm 
                    OR last_name LIKE @SearchTerm 
                    OR CONCAT(first_name, ' ', last_name) LIKE @SearchTerm 
                    OR CONCAT(last_name, ' ', first_name) LIKE @SearchTerm 
                    OR CONCAT(first_name, last_name) LIKE @SearchTerm 
                    OR CONCAT(last_name, first_name) LIKE @SearchTerm";
            return connection.Query<User>(query, new { SearchTerm = $"%{searchTerm}%" });
        }

        public static void UpdateUserPassword(int user_id, string newHashedPassword)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                UPDATE users
                SET password = @Password
                WHERE id = @Id";
            connection.Execute(query, new { Password = newHashedPassword, Id = user_id });
        }
    }
}
