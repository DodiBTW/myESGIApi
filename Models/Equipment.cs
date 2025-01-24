using MyESGIApi.Data;
namespace MyESGIApi.Models
{
    public class Equipment
    {
        public int? Id { get; set; }
        public string EquipmentName { get; set; }
        public bool? Available { get; set; }
        public string? ImgUrl { get; set; }
        public Equipment() { }
        public Equipment(string equipmentName, bool available, string imgUrl)
        {
            EquipmentName = equipmentName;
            Available = available;
            ImgUrl = imgUrl;
        }
        public Equipment(string equipmentName)
        {
            EquipmentName = equipmentName;
            Available = true;
            ImgUrl = EquipmentHelper.GetEquipmentImageByName(equipmentName);
        }
        public bool? IsAvailable()
        {
            return Available;
        }
        public int GetAmountAvailable()
        {
            return EquipmentHelper.GetEquipmentAmountAvailableByName(EquipmentName);
        }
        public void UpdateEquipment()
        {
            EquipmentHelper.UpdateEquipment(this);
        }
        public static string GetEquipmentImageByName(string equipmentName)
        {
            return EquipmentHelper.GetEquipmentImageByName(equipmentName);
        }
    }
}
