using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory.Model
{
    public abstract class BaseTransfer
    {
        [JsonProperty("Quantity Transferred")]
        public int QuantityTransferred;
        [JsonProperty("Equipment Id")]
        public string EquipmentId;
        [JsonProperty("Order Date")]
        public DateTime OrderDate;

        public BaseTransfer() { }

        public BaseTransfer(int quantity, string equipmentId)
        {
            QuantityTransferred = quantity;
            EquipmentId = equipmentId;
            OrderDate = DateTime.Now;
        }

        public BaseTransfer(int quantity, string equipmentId, DateTime orderDate)
        {
            QuantityTransferred = quantity;
            EquipmentId = equipmentId;
            OrderDate = orderDate;
        }

        public abstract bool Arrived();
    }
}
