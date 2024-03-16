using System.Collections.Generic;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Repository;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals
{
    public class TreatmentReferralService
    {
        public TreatmentReferralRepository TreatmentReferralRepository;

        public TreatmentReferralService()
        {
            TreatmentReferralRepository = new TreatmentReferralRepository();
        }


        public List<Model.TreatmentReferral>? FindTreatmentReferralsByPatientJmbg(string patientJmbg)
        {
            return TreatmentReferralRepository.FindTreatmentReferralsByPatientJmbg(patientJmbg);
        }

        public void CreateTreatmentReferral(string doctorJmbg, string patientJmbg, int duration, string therapy, List<string> additionalExaminations)
        {
            TreatmentReferralRepository.CreateTreatmentReferral(doctorJmbg,patientJmbg,duration,therapy,additionalExaminations);
        }


        public Model.TreatmentReferral? FindDoctorReferralById(string id)
        {
            return TreatmentReferralRepository.FindDoctorReferralById(id);
        }

        public Model.TreatmentReferral? FindReferralById(string id)
        {
            return TreatmentReferralRepository.FindReferralById(id);
        }
        public void GetAllTreatmentReferrals()
        {
            TreatmentReferralRepository.GetAllTreatmentReferrals();
        }

        public void Save()
        {
            TreatmentReferralRepository.Save();
        }
    }
}
