using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Repository
{
    public class DoctorReferralRepository
    {
        public List<Model.DoctorReferral> DoctorReferrals;

        public DoctorReferralRepository()
        {
            GetAllDoctorReferrals();
        }

        public Model.DoctorReferral? FindDoctorReferralById(string id)
        {
            return DoctorReferrals.FirstOrDefault(doctorReferral => doctorReferral.Id == id);
        }

        public List<Model.DoctorReferral>? FindDoctorReferralsByPatientJmbg(string patientJmbg)
        {
            return DoctorReferrals.Where(doctorReferral => doctorReferral.PatientJmbg == patientJmbg).ToList();
        }

        public void CreateDoctorReferral(string doctorJmbg, string patientJmbg)
        {
            DoctorReferrals.Add(new Model.DoctorReferral(doctorJmbg, patientJmbg, GenerateDoctorReferralId()));
            Save();
        }

        public void UseDoctorReferral(Model.DoctorReferral doctorReferral)
        {
            foreach (var referral in DoctorReferrals)
            {
                if (referral.Id == doctorReferral.Id)
                {
                    referral.IsUsed = true;
                    break;
                }
            }
            Save();
        }

        public void GetAllDoctorReferrals()
        {
            DoctorReferrals = JsonConvert.DeserializeObject<List<Model.DoctorReferral>>(File.ReadAllText("../../../Data/doctorreferrals.json"));
        }


        public string GenerateDoctorReferralId()
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

        public void Save()
        {
            File.WriteAllText("../../../Data/doctorreferrals.json", JsonConvert.SerializeObject(DoctorReferrals, Formatting.Indented));
        }

    }
}
