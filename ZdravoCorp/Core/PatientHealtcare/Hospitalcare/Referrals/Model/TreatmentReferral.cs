using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Model
{
    public class TreatmentReferral : Referral
    {
        [JsonProperty("Duration")]
        public int Duration;
        [JsonProperty("Therapy")]
        public string Therapy;
        [JsonProperty("Additional examinations")]
        public List<string> AdditionalExaminations;

        [JsonConstructor]
        public TreatmentReferral(string doctorJmbg, string patientJmbg, string id, int duration, string therapy, List<string> additionalExaminations) : base(doctorJmbg, patientJmbg, id)
        {
            IsUsed = false;
            Duration = duration;
            Therapy = therapy;
            AdditionalExaminations = additionalExaminations;
        }


    }
}
