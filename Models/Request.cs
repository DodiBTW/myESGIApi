using MyESGIApi.Data;

namespace MyESGIApi.Models
{
    public class Request
    {
        public int? Id { get; set; }
        public int AuthorId { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public List<Equipment> EquipmentList { get; set; }
        public Request() { }
        public Request(int id, int authorId, string comment, string status, DateTime requestDate)
        {
            Id = id;
            AuthorId = authorId;
            Comment = comment;
            Status = status;
            RequestDate = requestDate;
            EquipmentList = new List<Equipment>();
        }
        public Request(int id, int authorId, string comment, string status, DateTime requestDate, List<Equipment> equipment)
        {
            Id = id;
            AuthorId = authorId;
            Comment = comment;
            Status = status;
            RequestDate = requestDate;
            EquipmentList = equipment;
        }
        public void AddEquipment(Equipment equipment)
        {
            if(EquipmentList == null) EquipmentList = new List<Equipment>();
            if (Status != "Pending") throw new Exception("Cannot add equipment to a request that is not pending");
            EquipmentList.Add(equipment);
        }
        public void RemoveEquipment(Equipment equipment)
        {
            if (Status != "Pending") throw new Exception("Cannot remove equipment from a request that is not pending");
            EquipmentList.Remove(equipment);
        }
        public async Task FetchEquipment()
        {
            // Fetch equipment from database
            if(Id == null) return;
            IEnumerable<Equipment> equipments;
            if (Status != "Validated") equipments = await RequestHelper.GetEquipment(Id.Value);
            else equipments = await RequestHelper.GetRequestedEquipment(Id.Value);
            EquipmentList = equipments.ToList();
        }
        public void GiveEquipment()
        {
            Status = "Validated";
            foreach (Equipment equipment in EquipmentList)
            {
                equipment.Available = false;
                equipment.UpdateEquipment();
            }

        }
        public void ReturnEquipment()
        {
            Status = "Returned";
            foreach (Equipment equipment in EquipmentList)
            {
                equipment.Available = true;
                equipment.UpdateEquipment();
            }
        }
        public void SetComment(string comment)
        {
            Comment = comment;
        }
        public bool IsAuthor(int authorId)
        {
            return AuthorId == authorId;
        }
        public async Task Delete()
        {
            if (Id == null)
            {
                throw new Exception("Request not found");
            }
            else
            {
                await RequestHelper.DeleteRequest(Id.Value);
            }
        }
    }
}
