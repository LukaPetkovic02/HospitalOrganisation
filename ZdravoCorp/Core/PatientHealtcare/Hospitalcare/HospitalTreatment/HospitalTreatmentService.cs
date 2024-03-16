using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment
{
    public class HospitalTreatmentService
    {
        public IHospitalTreatmentRepository HospitalTreatmentRepository;

        public HospitalTreatmentService(IHospitalTreatmentRepository hospitalTreatmentRepository)
        {
            HospitalTreatmentRepository = hospitalTreatmentRepository;
        }

        public Model.HospitalTreatment FindTreatmentAccommodationById(string id)
        {
            return HospitalTreatmentRepository.FindHospitalTreatmentById(id);
        }

        public void CreateHospitalTreatment(string referralId, string patientJmbg, DateTime start, DateTime end, string therapy, string roomId)
        {
            HospitalTreatmentRepository.CreateHospitalTreatment(referralId, patientJmbg, start, end, therapy, roomId);
        }

        public void ChangeHospitalTreatmentTherapy(Model.HospitalTreatment treatment, string therapy)
        {
            HospitalTreatmentRepository.ChangeHospitalTreatmentTherapy(treatment,therapy);
        }

        public void ExtendHospitalTreatment(Model.HospitalTreatment treatment, int days)
        {
            HospitalTreatmentRepository.ExtendHospitalTreatment(treatment,days);
        }


        public void FinishHospitalTreatment(Model.HospitalTreatment treatment)
        {
            HospitalTreatmentRepository.FinishHospitalTreatment(treatment);
        }

        public List<Model.HospitalTreatment> GetAllHospitalTreatments()
        {
            return HospitalTreatmentRepository.HospitalTreatments;
        }
        public List<Model.HospitalTreatment>? GetTreatmentsInGivenRoom(string room)
        {
            return HospitalTreatmentRepository.GetTreatmentsInGivenRoom(room);
        }
        public List<Model.HospitalTreatment>? GetTreatmentsForGivenName(string name)
        {
            return HospitalTreatmentRepository.GetTreatmentsForGivenName(name);
        }
        public void Save()
        {
            HospitalTreatmentRepository.Save();
        }
    }
}
