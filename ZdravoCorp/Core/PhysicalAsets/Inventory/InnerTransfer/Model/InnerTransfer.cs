using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Inventory.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory.InnerTransfer.Model
{
    public class InnerTransfer : BaseTransfer
    {
        [JsonProperty("Room Id")]
        public string RoomId;
        [JsonProperty("Delivery Room Id")]
        public string DeliveryRoomId;

        public InnerTransfer():base() { }

        public InnerTransfer(int quantity, InventoryItemData item, string deliveryRoomId, DateTime deliveryDate):base(quantity, item.EquipmentId, deliveryDate)
        {
            DeliveryRoomId = deliveryRoomId;
            RoomId = item.RoomId;
        }

        public InnerTransfer(int quantity, InventoryItemData item, string deliveryRoomId):base(quantity, item.EquipmentId)
        {
            DeliveryRoomId = deliveryRoomId;
            RoomId = item.RoomId;
        }

        public override bool Arrived()
        {
            return OrderDate<=DateTime.Now;
        }


        public bool IsItems(InventoryItemData item)
        {
            return EquipmentId == item.EquipmentId && RoomId == item.RoomId;
        }
    }
}
