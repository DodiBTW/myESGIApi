using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using MyESGIApi.Models;
using Utils;
using MyESGIApi.Services;
using static MyESGIApi.Data.DatabaseHelper;
namespace MyESGIApi.Data
{
    public class UserHelper
    {
        private static readonly string SelectStatement = "SELECT id AS Id, first_name AS FirstName, last_name AS LastName, password AS Password, role AS Role, date_joined AS JoinDate, email_address AS EmailAdress, profile_picture_url AS ProfilePictureUrl FROM users ";
        public static async Task<bool> UserRegister(string first_name, string last_name, string email, string password, string? profile_picture_url = null, string? role = "student")
        {
            bool userExists = await Task.Run(() => UserExists(email));
            if (!FormatChecker.IsValidPassword(password) || userExists)
                return false;

            password = BCrypt.Net.BCrypt.HashPassword(password);
            using var connection = GetConnection();
            string query = @"
                INSERT INTO users (first_name, last_name, email_address, password, role, profile_picture_url)
                VALUES (@FirstName, @LastName, @Email, @Password, @Role, @ProfilePictureUrl)";
            await Task.Run(() => connection.Execute(query, new { FirstName = first_name, LastName = last_name, Email = email, Password = password, Role = role, ProfilePictureUrl = profile_picture_url }));
            return true;
        }
        public static async Task<bool> UserExists(string email)
        {
            using var connection = GetConnection();
            string query = @"
                SELECT COUNT(*) 
                FROM users 
                WHERE email_address = @Email";
            return await Task.Run(() => connection.ExecuteScalar<int>(query, new { Email = email })) > 0;
        }
        public static async Task<User?> GetUserByEmail(string email)
        {
            using var connection = GetConnection();
            string query = SelectStatement + " WHERE email_address = @Email";
            return await Task.Run(() => connection.QueryFirstOrDefault<User>(query, new { Email = email }));
        }
        public static async Task<User?> GetUserById(int id)
        {
            using var connection = GetConnection();
            string query = SelectStatement + " WHERE id = @Id";
            return await Task.Run(() => connection.QueryFirstOrDefault<User>(query, new { Id = id }));
        }
        public static async Task<IEnumerable<User>> GetUsers(int limit = 20)
        {
            using var connection = GetConnection();
            string query;
            if (limit > 0)
                query = "SELECT TOP(@Limit) id as Id, first_name as FirstName, last_name as LastName, email_address as EmailAdress, role as Role, date_joined as JoinDate, profile_picture_url as ProfilePictureUrl FROM users";
            else
                query = SelectStatement;
            return await Task.Run(() => connection.Query<User>(query, new { Limit = limit }));
        }
        public static async Task<IEnumerable<User>> GetUsersByString(string searchTerm, int page = 1)
        {
            using var connection = GetConnection();
            string query = "SELECT TOP(20) id as Id, first_name as FirstName, last_name as LastName, email_address as EmailAdress, role as Role, date_joined as JoinDate, profile_picture_url as ProfilePictureUrl FROM users WHERE first_name LIKE @SearchTerm OR last_name LIKE @SearchTerm OFFSET @Offset";
            return await Task.Run(() => connection.Query<User>(query, new { SearchTerm = $"%{searchTerm}%", Offset = (page - 1) * 20 }));
        }
        public static async Task UpdateUserPassword(int user_id, string newHashedPassword)
        {
            using var connection = GetConnection();
            string query = @"
                UPDATE users
                SET password = @Password
                WHERE id = @Id";
            await Task.Run(() => connection.Execute(query, new { Password = newHashedPassword, Id = user_id }));
        }
    }
}
