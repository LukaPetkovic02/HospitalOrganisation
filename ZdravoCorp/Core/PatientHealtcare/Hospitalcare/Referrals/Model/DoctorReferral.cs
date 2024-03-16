using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Model
{
    public class DoctorReferral : Referral
    {
        [JsonConstructor]
        public DoctorReferral(string doctorJmbg, string patientJmbg, string id) : base(doctorJmbg, patientJmbg, id) { }

    }



}
