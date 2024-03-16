using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.Model;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Model
{
    public class Prescription
    {
        public enum Consumption
        {
            DuringMeal,
            AfterMeal,
            BeforeMeal,
            AnyTime,
        }
        [JsonProperty("Id")]
        public string Id;
        [JsonProperty("Drug")]
        public string Drug;
        [JsonProperty("Start")]
        public DateTime Start;
        [JsonProperty("End")]
        public DateTime End;
        [JsonProperty("Daily dose")]
        public int DailyDose;
        [JsonProperty("Consumption time")]
        public Consumption ConsumptionTime;
        [JsonProperty("Patient jmbg")]
        public string PatientJmbg;
        [JsonProperty("Doctor jmbg")]
        public string DoctorJmbg;
        [JsonProperty("Used")]
        public bool IsUsed;

        [JsonConstructor]
        public Prescription(string id, string drug, DateTime start, DateTime end, int dailyDose, Consumption consumptionTime, string patientJmbg, string doctorJmbg)
        {
            Id = id;
            Drug = drug;
            Start = start;
            End = end;
            DailyDose = dailyDose;
            ConsumptionTime = consumptionTime;
            PatientJmbg = patientJmbg;
            IsUsed = false;
            DoctorJmbg = doctorJmbg;
        }

        public bool ValidatePrescriptionUse(Drug drug)
        {
            bool canBeUsed = drug.HasEnoughForGivenPrescription(this);
            canBeUsed = canBeUsed && DateTime.Today <= Start;
            
            return canBeUsed;
        }
    }
}
