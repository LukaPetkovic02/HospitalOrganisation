using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Equipment;
using ZdravoCorp.Core.PhysicalAsets.Equipment.Model;
using ZdravoCorp.Core.PhysicalAsets.Inventory.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory.Repository
{
    public class Inventory
    {
        private string FilePath = "../../../Data/inventory.json";
        public List<InventoryItem> Items { get; set; }
        public EquipmentService EquipmentService { get; set; }
        public RoomService RoomService { get; set; }

        public Inventory()
        {
            GetAllItems();
            EquipmentService = new EquipmentService();
            RoomService = new RoomService();

        }
        public void AddItem(InventoryItem item)
        {
            Items.Add(item);
            Save();
        }

        public InventoryItem GetInventoryItem(string equipmentId, string roomId)
        {
            foreach(InventoryItem item in Items)
            {
                if(item.RoomId == roomId && item.EquipmentId == equipmentId)
                {
                    return item;
                }
            }
            return null;
        }
        public RoomService GetRoomService()
        {
            return RoomService;
        }

        public Equipment.Model.Equipment GetItemEquipment(InventoryItem item)
        {
            return EquipmentService.FindById(item.EquipmentId);
        }

        public Equipment.Model.Equipment GetItemEquipment(string id)
        {
            return EquipmentService.FindById(id);
        }

        public Room.Model.Room GetItemRoom(InventoryItem item)
        {
            return RoomService.FindById(item.RoomId);
        }

        public Room.Model.Room GetItemRoom(string id)
        {
            return RoomService.FindById(id);
        }

        public Room.Model.Room GetItemRoom(Room.Model.Room room)
        {
            return RoomService.FindById(room.Id);
        }

        public List<InventoryItem> FindByEquipmentType(EquipmentType type, List<InventoryItem> items) 
        {
            return (from InventoryItem item in items
                    where GetItemEquipment(item).Type == type
                    select item).ToList();
        }

        public List<InventoryItem> FindByRoomType(RoomType room, List<InventoryItem> items) {
            return (from InventoryItem item in items
                    where GetItemRoom(item).Type == room
                    select item).ToList();
        }

        public List<InventoryItem> FindByQuantity(int low, int high, List<InventoryItem> items) {
            return (from InventoryItem item in items
                    where high >= item.Quantity && item.Quantity > low
                    select item).ToList();
        }

        public List<InventoryItem> FindOutsideStorage(List<InventoryItem> items) {
            return (from InventoryItem item in items
                    where GetItemRoom(item).Type != RoomType.Storage
                    select item).ToList();
        }

        public List<InventoryItem> FindByAtributes(string atribute, List<InventoryItem> items)
        {
            return (from InventoryItem item in items
                    where RoomService.HasAttribute(item.RoomId, atribute) || EquipmentService.HasAttribute(item.EquipmentId, atribute) || item.Quantity.ToString().ToUpper().Contains(atribute.ToUpper())
                    select item).ToList();
        }

        public List<InventoryItemData> GetInventoryItemsData(List<InventoryItem> items){
            List<InventoryItemData> ret = new List<InventoryItemData>();
            foreach(InventoryItem item in items){
                ret.Add(GetInventoryItemData(item));
            }
            return ret;
        }

        public InventoryItemData GetInventoryItemData(InventoryItem item){
            InventoryItemData ret = new InventoryItemData(item.Quantity, GetItemEquipment(item), GetItemRoom(item));
            return ret;
        }

        public List<InventoryItem> GetRoomItems(string roomId)
        {
            return (from InventoryItem item in Items
                    where item.RoomId == roomId
                    select item).ToList();
        }

        public void GetAllItems()
        {
            Items = JsonConvert.DeserializeObject<List<InventoryItem>>(File.ReadAllText(FilePath));
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Items, Formatting.Indented));
        }

        public void CreateEquipment(Equipment.Model.Equipment newEquipment)
        {
            EquipmentService.CreateEquipment(newEquipment);
        }

        public void CreateRoom(Room.Model.Room newRoom)
        {
            RoomService.CreateRoom(newRoom);
        }

        public void DeleteRoom(string id)
        {
            RoomService.DeleteRoom(id);
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            foreach (InventoryItem item in Items)
            {
                if (item.RoomId != id)
                {
                    inventoryItems.Add(item);
                }
            }
            Items = inventoryItems;
            Save();
        }
    }
}