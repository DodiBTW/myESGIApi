using MyESGIApi.Data;
namespace MyESGIApi.Models
{
    public class Equipment
    {
        public string EquipmentName { get; set; }
        public bool InStock { get; set; }
        public string ImgUrl { get; set; }
        public Equipment() { }
        public Equipment(string equipmentName, bool inStock, string imgUrl)
        {
            EquipmentName = equipmentName;
            InStock = inStock;
            ImgUrl = imgUrl;
        }
    }
}
