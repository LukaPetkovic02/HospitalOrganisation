using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.DrugOrders.Model
{
    public class DrugOrder
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Drug")]
        public string Drug { get; set; }
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }
        [JsonProperty("Time created")]
        public DateTime TimeCreated { get; set; }


        [JsonConstructor]
        public DrugOrder(string id, string drug, int quantity, DateTime timeCreated)
        {
            Id = id;
            Drug = drug;
            Quantity = quantity;
            TimeCreated = timeCreated;
        }
    }
}
