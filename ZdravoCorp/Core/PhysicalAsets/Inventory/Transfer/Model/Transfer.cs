using System;
using ZdravoCorp.Core.PhysicalAsets.Inventory.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory.Transfer.Model
{
    public class Transfer : BaseTransfer
    {
        public Transfer():base()
        {
            
        }

        public Transfer(int quantity, string equipmentId) : base(quantity, equipmentId) { }

        public override bool Arrived()
        {
            return (DateTime.Now - OrderDate).TotalDays >= 1;
        }
    }
}
