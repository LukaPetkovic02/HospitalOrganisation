using System.Collections.Generic;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Repository;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals
{
    public class DoctorReferralService
    {
        public DoctorReferralRepository DoctorReferralRepository;

        public DoctorReferralService()
        {
            DoctorReferralRepository = new DoctorReferralRepository();
        }

        public List<Model.DoctorReferral>? FindDoctorReferralsByPatientJmbg(string patientJmbg)
        {
            return DoctorReferralRepository.FindDoctorReferralsByPatientJmbg(patientJmbg);
        }

        public void CreateDoctorReferral(string doctorJmbg, string patientJmbg)
        {
            DoctorReferralRepository.CreateDoctorReferral(doctorJmbg,patientJmbg);
        }
        public void UseDoctorReferral(Model.DoctorReferral doctorReferral)
        {
            DoctorReferralRepository.UseDoctorReferral(doctorReferral);
        }

        public Model.DoctorReferral? FindDoctorReferralById(string id)
        {
            return DoctorReferralRepository.FindDoctorReferralById(id);
        }

        public void GetAllDoctorReferrals()
        {
            DoctorReferralRepository.GetAllDoctorReferrals();
        }

        public void Save()
        {
            DoctorReferralRepository.Save();
        }


    }
}
