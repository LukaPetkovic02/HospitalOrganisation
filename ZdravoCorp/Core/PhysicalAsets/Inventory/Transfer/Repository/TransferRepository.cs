using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PhysicalAsets.Inventory.Transfer.Repository
{
    public class TransferRepository
    {
        private string FilePath = "../../../Data/transfer.json";
        private List<Model.Transfer> TransferList;
        public TransferRepository()
        {
            GetAllItems();
        }

        public List<Model.Transfer> GetArrivedItems()
        {
            return (from Model.Transfer transfer in TransferList where transfer.Arrived() select transfer).ToList();
        }
        public void GetAllItems()
        {
            TransferList = JsonConvert.DeserializeObject<List<Model.Transfer>>(File.ReadAllText(FilePath));
        }

        public void DeleteOutdated()
        {
            List<Model.Transfer> newTransferList = new List<Model.Transfer>();
            foreach (Model.Transfer transfer in TransferList)
            {
                if (!transfer.Arrived())
                {
                    newTransferList.Add(transfer);
                }
            }
            TransferList = newTransferList;
            Save();
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(TransferList, Formatting.Indented));
        }

        internal void AddTransfer(int quantity, Equipment.Model.Equipment equipment)
        {
            TransferList.Add(new Model.Transfer(quantity, equipment.Id));
            Save();
        }
    }
}
