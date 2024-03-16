using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.DrugOrders.Model;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.DrugOrders.Repository
{
    public class DrugOrderRepository
    {
        public List<DrugOrder> DrugOrders;

        public DrugOrderRepository()
        {
            GetAllDrugOrders();
        }
        public List<DrugOrder> GetAllDrugOrders()
        {
            return DrugOrders = JsonConvert.DeserializeObject<List<DrugOrder>>(File.ReadAllText("../../../Data/orderedDrugs.json"));
        }
        public DrugOrder? FindDrugOrderById(string id)
        {
            return DrugOrders.FirstOrDefault(drugOrder => drugOrder.Id == id);
        }

        public string GenerateDrugOrderId()
        {
            while (true)
            {
                string drugOrderId = Generator.GenerateRandomId();

                if (!(FindDrugOrderById(drugOrderId) == null))
                {
                    continue;
                }
                return drugOrderId;
            }
        }
        public void CollectDrug(DrugService drugService, DrugOrder drugOrder)
        {
            drugService.AddDrugs(drugOrder.Drug, drugOrder.Quantity);
        }
        public void DeleteOrders(List<DrugOrder> ordersToDelete)
        {
            foreach (DrugOrder order in ordersToDelete)
            {
                DrugOrders.Remove(order);
            }
            Save();
        }
        public void CheckForArrivedDrugs()
        {
            DrugService drugService = new DrugService();
            List<DrugOrder> ordersToDelete = new List<DrugOrder>();
            foreach (var drugOrder in from DrugOrder drugOrder in DrugOrders
                                      where drugOrder.TimeCreated <= DateTime.Now.AddDays(-1)
                                      select drugOrder)
            {
                CollectDrug(drugService, drugOrder);
                ordersToDelete.Add(drugOrder);
            }

            DeleteOrders(ordersToDelete);
        }
        public void CreateDrugOrder(string drug, int quantity)
        {
            DrugOrders.Add(new DrugOrder(GenerateDrugOrderId(), drug, quantity, DateTime.Now));
            Save();
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/orderedDrugs.json", JsonConvert.SerializeObject(DrugOrders, Formatting.Indented));
        }
    }
}
