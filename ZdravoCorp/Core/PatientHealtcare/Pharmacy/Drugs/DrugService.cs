using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.Model;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.Repository;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs
{
    public class DrugService
    {
        public DrugRepository DrugRepository;

        public DrugService()
        {
            DrugRepository = new DrugRepository();
        }

        public void CreateDrug(string name, List<string> ingredients, int quantity, int numberOfPills)
        {
            DrugRepository.CreateDrug(name,ingredients,quantity,numberOfPills);
        }

        public bool IsPatientAllergicToDrug(Patient patient, Drug drug)
        {
            return (from allergy in patient.MedicalCard.Alergies from ingredient in drug.Ingredients where allergy == ingredient select allergy).Any();
        }

        public void GiveDrug(String drug, int quantity)
        {
            DrugRepository.GiveDrug(drug, quantity);
        }
        public void AddDrugs(String drugName, int quantity)
        {
            DrugRepository.AddDrugs(drugName, quantity);
        }
        public Drug? FindByName(string name)
        {
            return DrugRepository.FindByName(name);
        }

        public List<Drug> GetAllDrugs()
        {
            return DrugRepository.GetAllDrugs();
        }

        public void Save()
        {
            DrugRepository.Save();
        }


    }
}
