using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Repository
{
    public class TreatmentReferralRepository
    {
        public List<Model.TreatmentReferral> TreatmentReferrals;

        public TreatmentReferralRepository()
        {
            GetAllTreatmentReferrals();
        }

        public List<Model.TreatmentReferral>? FindTreatmentReferralsByPatientJmbg(string patientJmbg)
        {
            return TreatmentReferrals.Where(treatmentReferral => treatmentReferral.PatientJmbg == patientJmbg).ToList();
        }

        public Model.TreatmentReferral? FindDoctorReferralById(string id)
        {
            return TreatmentReferrals.FirstOrDefault(doctorReferral => doctorReferral.Id == id);
        }

        public Model.TreatmentReferral? FindReferralById(string id)
        {
            return TreatmentReferrals.FirstOrDefault(TreatmentReferral => TreatmentReferral.Id == id);
        }

        public void CreateTreatmentReferral(string doctorJmbg, string patientJmbg,int duration,string therapy,List<string> additionalExaminations)
        {
            TreatmentReferrals.Add(new Model.TreatmentReferral(doctorJmbg, patientJmbg, GenerateTreatmentReferralId(),duration,therapy,additionalExaminations));
            Save();
        }

        public string GenerateTreatmentReferralId()
        {
            while (true)
            {
                string referralId = Generator.GenerateRandomId();

                if (!(FindDoctorReferralById(referralId) == null))
                {
                    continue;
                }
                return referralId;
            }
        }

        public void GetAllTreatmentReferrals()
        {
            TreatmentReferrals = JsonConvert.DeserializeObject<List<Model.TreatmentReferral>>(File.ReadAllText("../../../Data/treatmentreferrals.json"));
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/treatmentreferrals.json", JsonConvert.SerializeObject(TreatmentReferrals, Formatting.Indented));
        }
    }
}
