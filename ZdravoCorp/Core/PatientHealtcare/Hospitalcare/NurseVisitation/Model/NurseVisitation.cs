using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.NurseVisitation.Model
{
    public class NurseVisitation
    {
        public enum VisitationTime { MORNING, NIGHT }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("TreatmentId")]
        string TreatmentId { get; set; }

        [JsonProperty("date")]
        DateTime Date { get; set; }

        [JsonProperty("time")]
        VisitationTime Time { get; set; }

        [JsonProperty("Observation")]

        string Observation {get; set; }

        public NurseVisitation(string id, string treatmentId, DateTime date, VisitationTime time, string observation) 
        {
            Id = id;
            TreatmentId = treatmentId;
            Date = date;
            Time = time;
            Observation = observation;
        }
    }
}
