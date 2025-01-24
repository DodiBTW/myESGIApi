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
        public static async Task<Request?> GetRequest(int id)
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
                WHERE id = @Id
                ";
            return await connection.QueryFirstOrDefaultAsync<Request>(query, new { Id = id });
        }
        public static async Task<IEnumerable<Request>> GetRequestsByAuthor(int authorId)
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
                WHERE authorId = @AuthorId
                ORDER BY request_date DESC
                ";
            return await connection.QueryAsync<Request>(query, new { AuthorId = authorId });
        }
        public static async Task<int> CreateRequest(Request request)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                INSERT INTO equipment_requests (authorId, comment, status, request_date)
                VALUES (@AuthorId, @Comment, 'Pending', @RequestDate)
                SELECT SCOPE_IDENTITY()
                ";
            return await connection.ExecuteScalarAsync<int>(query, new {AuthorId = request.AuthorId, Comment = request.Comment, RequestDate = request.RequestDate});
        }
        public static async Task UpdateRequest(Request request)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                UPDATE equipment_requests
                SET authorId = @AuthorId, comment = @Comment, status = @Status, request_date = @RequestDate
                WHERE id = @Id
                ";
            await connection.ExecuteAsync(query, request);
        }
        public static async Task DeleteRequest(int id)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                DELETE FROM equipment_requests
                WHERE id = @Id
                ";
            await connection.ExecuteAsync(query, new { Id = id });
        }
        public static async Task<IEnumerable<Equipment>> GetEquipment(int requestId)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    equipment.id AS Id, 
                    equipment.equipment_name AS EquipmentName, 
                    equipment.img_url AS ImgUrl
                FROM equipment_request_items
                JOIN equipment ON equipment.id = equipment_request_items.equipment_id
                WHERE equipment_request_items.request_id = @RequestId
                ";
            return await connection.QueryAsync<Equipment>(query, new { RequestId = requestId });
        }
        public static async Task<IEnumerable<Equipment>> GetRequestedEquipment(int requestId)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    equipment.equipment_name AS EquipmentName, 
                    equipment.img_url AS ImgUrl
                FROM requested_equipment
                JOIN equipment ON equipment.id = requested_equipment.equipment_id
                WHERE requested_equipment.request_id = @RequestId
                ";
            return await connection.QueryAsync<Equipment>(query, new { RequestId = requestId });
        }
    }
}
