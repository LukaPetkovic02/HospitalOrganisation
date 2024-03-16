using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.DrugOrders.Model;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.DrugOrders.Repository;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.DrugOrders
{
    public class DrugOrderService
    {
        public DrugOrderRepository DrugOrderRepository;

        public DrugOrderService()
        {
            DrugOrderRepository = new DrugOrderRepository();
        }
        public void CreateDrugOrder(string drug, int quantity)
        {
            DrugOrderRepository.CreateDrugOrder(drug, quantity);
        }
        public DrugOrder? FindDrugOrderById(string id)
        {
            return DrugOrderRepository.FindDrugOrderById(id);
        }
        public void CheckForArrivedDrugs()
        {
            DrugOrderRepository.CheckForArrivedDrugs();
        }
        public void Save() 
        {
            DrugOrderRepository.Save();
        }
    }
}
