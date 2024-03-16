using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.Model;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.Repository
{
    public class DrugRepository
    {
        public List<Drug> Drugs;

        public DrugRepository()
        {
            GetAllDrugs();
        }

        public void CreateDrug(string name, List<string> ingredients,int quantity, int numberOfPills)
        {
            Drugs.Add(new Drug(name, ingredients,quantity,numberOfPills));
            Save();
        }

        public Drug? FindByName(string name)
        {
            return Drugs.FirstOrDefault(drug => name.Replace(" ", "").ToLower() == drug.Name.Replace(" ", "").ToLower());
        }

        public void GiveDrug(String drug, int quantity)
        {
            FindByName(drug).Quantity -= quantity;
            Save();
        }

        public void AddDrugs(String drugName, int quantity)
        {
            FindByName(drugName).Quantity += quantity;
            Save();
        }

        public List<Drug> GetAllDrugs()
        {
            return Drugs = JsonConvert.DeserializeObject<List<Drug>>(File.ReadAllText("../../../Data/drugs.json"));
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/drugs.json", JsonConvert.SerializeObject(Drugs, Formatting.Indented));
        }
    }
}
