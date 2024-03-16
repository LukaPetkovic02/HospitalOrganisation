using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model
{
    public class HospitalTreatment
    {
        public enum TreatmentStatus { FINISHED, INPROGRESS }

        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("PatientJmbg")]
        public string PatientJmbg { get; set; }
        [JsonProperty("ReferralId")]
        public string ReferralId { get; set; }
        [JsonProperty("Start")]
        public DateTime Start { get; set; }
        [JsonProperty("End")]
        public DateTime End { get; set; }
        [JsonProperty("Therapy")]
        public string Therapy { get; set; }
        [JsonProperty("RoomId")]
        public string RoomId { get; set; }
        [JsonProperty("Status")]
        public TreatmentStatus Status { get; set; }


        public HospitalTreatment(string id, string referralId, string patientJmbg, DateTime start, DateTime end, string therapy, string roomId)
        {
            Id = id;
            PatientJmbg = patientJmbg;
            ReferralId = referralId;
            Start = start;
            End = end;
            Therapy = therapy;
            RoomId = roomId;
            Status = TreatmentStatus.INPROGRESS;
        }

        public void Finish()
        {
            Status = TreatmentStatus.FINISHED;
        }
    }
}

   
