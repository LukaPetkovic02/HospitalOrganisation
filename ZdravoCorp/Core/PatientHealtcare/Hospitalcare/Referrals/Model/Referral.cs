using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Model
{
    public abstract class Referral
    {
        [JsonProperty("Used")]
        public bool IsUsed;
        [JsonProperty("DoctorJmbg")]
        public string DoctorJmbg;
        [JsonProperty("PatientJmbg")]
        public string PatientJmbg;
        [JsonProperty("Id")]
        public string Id;

        [JsonConstructor]
        protected Referral(string doctorJmbg, string patientJmbg, string id)
        {
            IsUsed = false;
            DoctorJmbg = doctorJmbg;
            PatientJmbg = patientJmbg;
            Id = id;
        }

    }
}
