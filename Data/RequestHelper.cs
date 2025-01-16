using MyESGIApi.Models;
using Dapper;
namespace MyESGIApi.Data
{
    public class RequestHelper
    {
        public static async Task<IEnumerable<Request>> GetRequests()
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    id AS Id, 
                    authorId AS AuthorId, 
                    comment AS Comment, 
                    status AS Status, 
                    request_date AS RequestDate
                FROM equipment_requests
                ORDER BY request_date DESC
                ";
            return await connection.QueryAsync<Request>(query);
        }
        public static async Task<bool> CreateRequest(Request request){
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                INSERT INTO equipment_requests (authorId, comment, status, request_date)
                VALUES (@AuthorId, @Comment, @Status, @RequestDate)
            ";
            return await connection.ExecuteAsync(query, request) > 0;
        }
        public static async Task<bool> UpdateRequest(Request request)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                UPDATE equipment_requests
                SET status = @status , comment = @comment, request_date = @request_date
                WHERE id = @Id
            ";
            return await connection.ExecuteAsync(query, request) > 0;
        }
    }
}
