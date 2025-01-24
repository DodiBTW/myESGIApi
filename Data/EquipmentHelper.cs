using Dapper;
using MyESGIApi.Models;
namespace MyESGIApi.Data
{
    public class EquipmentHelper
    {
        public static List<Equipment> GetAllEquipmentTypes()
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT
                    equipment_name AS EquipmentName,
                    img_url AS ImgUrl
                FROM equipment
                ";
            return connection.Query<Equipment>(query).ToList();
        }
        public static int GetEquipmentAmountByName(string EquipmentName)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    COUNT(*) AS Amount
                FROM equipment
                WHERE equipment_name = @EquipmentName
                ";
            return connection.QueryFirstOrDefault<int>(query);
        }
        public static int GetEquipmentAmountAvailableByName(string EquipmentName)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT 
                    COUNT(*) AS Amount
                FROM equipment
                WHERE equipment_name = @EquipmentName AND available = 1
                ";
            return connection.QueryFirstOrDefault<int>(query);
        }
        public static Equipment? GetEquipmentByName(string equipmentName, int amountNeeded)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT
                    id AS Id,
                    equipment_name AS EquipmentName,
                    available AS Available,
                    img_url AS ImgUrl
                FROM equipment
                WHERE equipment_name = @EquipmentName AND available = 1
                LIMIT @AmountNeeded
                ";
            return connection.QueryFirstOrDefault<Equipment>(query, new { EquipmentName = equipmentName, AmountNeeded = amountNeeded });
        }
        public static void UpdateEquipment(Equipment equipment)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                UPDATE equipment
                SET available = @Available
                WHERE id = @Id
                ";
            connection.Execute(query, equipment);
        }
        public static void SetRequestedEquipment(int equipmentId, int requestId)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                INSERT INTO requested_equipment (equipment_name, request_id) VALUES (@EquipmentId, @RequestId)
                ";
            connection.Execute(query, new { EquipmentId = equipmentId , RequestId = requestId});
        }
        public static string? GetEquipmentImageByName(string equipmentName)
        {
            using var connection = DatabaseHelper.GetConnection();
            string query = @"
                SELECT img_url
                FROM equipment
                WHERE equipment_name = @EquipmentName
                ";
            return connection.QueryFirstOrDefault<string>(query, new { EquipmentName = equipmentName });
        }
    }
}
