using System.Collections.Generic;
using Newtonsoft.Json;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Model;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.Model
{
    public class Drug
    {
        [JsonProperty("Name")]
        public string Name;
        [JsonProperty("Ingredients")]
        public List<string> Ingredients;
        [JsonProperty("Quantity")]
        public int Quantity;
        [JsonProperty("NumberOfPills")]
        public int NumberOfPills;

        [JsonConstructor]
        public Drug(string name, List<string> ingredients,int quantity,int numberOfPills)
        {
            Name = name;
            Ingredients = ingredients;
            Quantity = quantity;
            NumberOfPills = numberOfPills;
        }

        public bool HasEnoughForGivenPrescription(Prescription prescription)
        {
            return ((prescription.End-prescription.Start).TotalDays * prescription.DailyDose) < (NumberOfPills * Quantity);
        }
    }
}
