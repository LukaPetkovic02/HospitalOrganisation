using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.PhysicalAsets.Equipment;
using ZdravoCorp.Core.PhysicalAsets.Equipment.Model;
using ZdravoCorp.Core.PhysicalAsets.Inventory.InnerTransfer.Repository;
using ZdravoCorp.Core.PhysicalAsets.Inventory.Model;
using ZdravoCorp.Core.PhysicalAsets.Inventory.Transfer.Repository;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory
{
    public class InventoryService
    {
        public Repository.Inventory InventoryRepository { get; set; }
        public TransferRepository TransferRepository { get; set; }

        public InnerTransferRepository InnerTransferRepository { get; set; }
        public InventoryService()
        {
            InventoryRepository = new Repository.Inventory();
            TransferRepository = new TransferRepository();
            InnerTransferRepository = new InnerTransferRepository();
        }

        public List<Equipment.Model.Equipment> GetEquipment()
        {
            return InventoryRepository.EquipmentService.GetEquipments();
        }
        public List<Room.Model.Room> GetRooms()
        {
            return InventoryRepository.RoomService.GetRooms();
        }
        public Equipment.Model.Equipment GetItemEquipment(InventoryItem item)
        {
            return InventoryRepository.GetItemEquipment(item);
        }

        public Equipment.Model.Equipment GetItemEquipment(string id)
        {
            return InventoryRepository.GetItemEquipment(id);
        }

        public Room.Model.Room GetItemRoom(Room.Model.Room room)
        {
            return InventoryRepository.GetItemRoom(room);
        }

        public Room.Model.Room GetItemRoom(string id)
        {
            return InventoryRepository.GetItemRoom(id);
        }

        public Room.Model.Room GetItemRoom(InventoryItem item)
        {
            return InventoryRepository.GetItemRoom(item);
        }

        public InventoryItem GetInventoryItem(string equipmentId, string roomId)
        {
            return InventoryRepository.GetInventoryItem(equipmentId, roomId);
        }

        public List<InventoryItem> InventoryItems()
        {
            return InventoryRepository.Items;
        }

        public List<InventoryItemData> InventoryItemsData()
        {
            return InventoryRepository.GetInventoryItemsData(InventoryItems());
        }

        public List<InventoryItemData> InventoryItemsData(List<InventoryItem> items)
        {
            return InventoryRepository.GetInventoryItemsData(items);
        }

        public List<InventoryItemData> GetDynamicItemsData()
        {
            return (from InventoryItemData item in InventoryItemsData()
                    where item.EquipmentDinamic && item.Quantity < 5
                    select item).ToList();
        }

        public List<InventoryItemData>? GetDynamicEquipmentByRoomId(string roomId)
        {
            return (from inventoryItem in InventoryItemsData()
                    where inventoryItem.EquipmentDinamic && inventoryItem.RoomId == roomId && inventoryItem.Quantity > 0
                    select inventoryItem).ToList();
        }


        public void GetAllItems()
        {
            InventoryRepository.GetAllItems();
        }

        public void AddItem(InventoryItem item) {
            InventoryRepository.AddItem(item);
        }

        public bool CreateEquipment(Equipment.Model.Equipment newEquipment)
        {
            if (GetItemEquipment(newEquipment.Id) != null)
            {
                return false;
            }

            InventoryRepository.CreateEquipment(newEquipment);
            foreach (Room.Model.Room room in GetRooms())
            {
                AddItem(new InventoryItem(room, newEquipment));
            }

            GetAllItems();
            return true;
            
        }

        public bool CreateRoom(string id, RoomType type)
        {
            return CreateRoom(new Room.Model.Room(id, type));
        }
        public bool CreateRoom(Room.Model.Room newRoom)
        {
            if (GetItemRoom(newRoom) != null)
            {
                return false;
            }
            InventoryRepository.CreateRoom(newRoom);
            foreach (Equipment.Model.Equipment equipment in GetEquipment())
            {
                AddItem(new InventoryItem(newRoom, equipment));
            }

            GetAllItems();
            return true;
            
        }

        public void DeleteRoom(string id)
        {
            if (GetItemRoom(id) == null)
            {
                return;
            }
            InventoryRepository.DeleteRoom(id);
            GetAllItems();
        }

        public List<InventoryItem> FindByEquipmentType(EquipmentType type, List<InventoryItem> items)
        {
            return InventoryRepository.FindByEquipmentType(type, items);
        }

        public List<InventoryItem> FindByRoomType(RoomType room, List<InventoryItem> items)
        {
            return InventoryRepository.FindByRoomType(room,items);

        }

        public List<InventoryItem> FindByQuantity(int low, int high, List<InventoryItem> items)
        {
            return InventoryRepository.FindByQuantity(low, high, items);

        }

        public List<InventoryItem> FindOutsideStorage(List<InventoryItem> items)
        {
            return InventoryRepository.FindOutsideStorage(items);

        }

        public List<InventoryItem> FindByAtributes(string atribute, List<InventoryItem> items)
        {
            return InventoryRepository.FindByAtributes(atribute, items);
        }

        private bool ChangeItemQuantity(string EquipmentId, string RoomId, int Quantity)
        {
            InventoryItem item = GetInventoryItem(EquipmentId, RoomId);
            if (item != null && item.Quantity + Quantity >= 0)
            {
                item.Quantity += Quantity;
                return true;
            }
            return false;
        }
        public void UpdateArrivedEquipment()
        {
            List<Transfer.Model.Transfer> arrived = TransferRepository.GetArrivedItems();
            TransferRepository.DeleteOutdated();

            foreach (Transfer.Model.Transfer transfer in arrived)
            {
                ChangeItemQuantity(transfer.EquipmentId, "00", transfer.QuantityTransferred);
            }
            Save();
        }

        public void CreateTransfer(int quantity, InventoryItemData item)
        {

            TransferRepository.AddTransfer(quantity, InventoryRepository.EquipmentService.FindById(item.EquipmentId));
        }

        public void UpdateInnerArrivedEquipment()
        {
            List<InnerTransfer.Model.InnerTransfer> arrived = InnerTransferRepository.GetArrivedItems();
            InnerTransferRepository.DeleteOutdated();

            foreach (InnerTransfer.Model.InnerTransfer transfer in arrived)
            {
                if(ChangeItemQuantity(transfer.EquipmentId, transfer.RoomId, -transfer.QuantityTransferred))
                {
                    ChangeItemQuantity(transfer.EquipmentId, transfer.DeliveryRoomId, transfer.QuantityTransferred);
                }
            }
            Save();
        }

        public void CreateInnerTransfer(int quantity, InventoryItemData item, string DestinationRoomId)
        {
            InnerTransferRepository.AddTransfer(quantity, item, DestinationRoomId);
        }

        public void CreateInnerTransfer(int quantity, InventoryItemData item, string DestinationRoomId, DateTime dateTime)
        {
            InnerTransferRepository.AddTransfer(quantity, item, DestinationRoomId, dateTime);
        }

        public int OccupiedQuantity(InventoryItemData item)
        {
            int quantity = item.Quantity;
            foreach (InnerTransfer.Model.InnerTransfer transfer in InnerTransferRepository.GetInnerTransfers())
            {
                if (transfer.IsItems(item))
                {
                    quantity -= transfer.QuantityTransferred;
                }
            }
            return quantity;
        }
        public bool UseDynamicEquipment(string EquipmentId, string RoomId, int Quantity)
        {
            bool ret = ChangeItemQuantity(EquipmentId, RoomId, -Quantity);
            Save();
            return ret;
        }

        public void AllocateItemsToStorage(string RoomId)
        {
            foreach(InventoryItem item in InventoryRepository.GetRoomItems(RoomId))
            {
                ChangeItemQuantity(item.EquipmentId, "00", item.Quantity);
                item.Quantity = 0;
            }
            Save();
        }
        public void Save()
        {
            InventoryRepository.Save();
        }

        public RoomService GetRoomService()
        {
            return InventoryRepository.GetRoomService();
        }
    }
}
