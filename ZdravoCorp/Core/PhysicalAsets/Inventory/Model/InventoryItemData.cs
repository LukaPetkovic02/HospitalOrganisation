using ZdravoCorp.Core.PhysicalAsets.Equipment.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory.Model
{
    public class InventoryItemData
    {
        public int Quantity { get; set; }
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public bool EquipmentDinamic { get; set; }
        public string RoomId { get; set; }
        public RoomType RoomType { get; set; }

        public InventoryItemData(int quantity, Equipment.Model.Equipment equipment, Room.Model.Room room) {
            Quantity = quantity;
            EquipmentId = equipment.Id;
            EquipmentName = equipment.Name;
            EquipmentType = equipment.Type;
            EquipmentDinamic = equipment.Dynamic;
            RoomId = room.Id;
            RoomType = room.Type;
        }
    }
}