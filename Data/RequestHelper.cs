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
                FROM requests
                ORDER BY request_date DESC
                ";
            return await connection.QueryAsync<Request>(query);
        }
    }
}
