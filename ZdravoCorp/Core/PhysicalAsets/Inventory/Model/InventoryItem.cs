using Newtonsoft.Json;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory.Model
{
    public class InventoryItem
    {
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }
        [JsonProperty("Equipmnt Id")]
        public string EquipmentId { get; set; }
        [JsonProperty("Room Id")]
        public string RoomId { get; set; }

        public InventoryItem() { }

        public InventoryItem(Room.Model.Room room, Equipment.Model.Equipment equipment)
        {
            Quantity = 0;
            if (room != null)
            {
                RoomId = room.Id;
            }
            

            if (equipment != null)
            {
                EquipmentId = equipment.Id;
            }
            
        }
    }
}
