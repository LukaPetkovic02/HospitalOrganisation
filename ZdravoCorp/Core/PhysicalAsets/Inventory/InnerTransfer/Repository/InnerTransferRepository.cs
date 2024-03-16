using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Inventory.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory.InnerTransfer.Repository
{
    public class InnerTransferRepository
    {
        private string FilePath = "../../../Data/innerTransfer.json";
        private List<Model.InnerTransfer> innerTransferList;
        public InnerTransferRepository()
        {
            GetAllItems();
        }

        public List<Model.InnerTransfer> GetArrivedItems()
        {
            return (from Model.InnerTransfer transfer in innerTransferList where transfer.Arrived() select transfer).ToList();
        }
        public void GetAllItems()
        {
            innerTransferList = JsonConvert.DeserializeObject<List<Model.InnerTransfer>>(File.ReadAllText(FilePath));
        }

        public void DeleteOutdated()
        {
            List<Model.InnerTransfer> newInnerTransferList = new List<Model.InnerTransfer>();
            foreach (Model.InnerTransfer transfer in innerTransferList)
            {
                if (!transfer.Arrived())
                {
                    newInnerTransferList.Add(transfer);
                }
            }
            innerTransferList = newInnerTransferList;
            Save();
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(innerTransferList, Formatting.Indented));
        }

        public void AddTransfer(int quantity, InventoryItemData item, string deliveryRoomId, DateTime dateTime)
        {
            innerTransferList.Add(new Model.InnerTransfer(quantity, item, deliveryRoomId, dateTime));
            Save();
        }

        public void AddTransfer(int quantity, InventoryItemData item, string deliveryRoomId)
        {
            innerTransferList.Add(new Model.InnerTransfer(quantity, item, deliveryRoomId));
            Save();
        }

        public List<Model.InnerTransfer> GetInnerTransfers()
        {
            return innerTransferList;
        }
    }
}
