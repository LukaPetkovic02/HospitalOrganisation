using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Repository
{
    public interface IHospitalTreatmentRepository
    {
        public List<Model.HospitalTreatment> HospitalTreatments { get; set; }
        public Model.HospitalTreatment? FindHospitalTreatmentById(string id);
        public void FinishHospitalTreatment(Model.HospitalTreatment treatment);
        public void ChangeHospitalTreatmentTherapy(Model.HospitalTreatment treatment, string therapy);
        public void ExtendHospitalTreatment(Model.HospitalTreatment treatment, int days);
        public void CreateHospitalTreatment(string referralId, string patientJmbg, DateTime start, DateTime end, string therapy, string roomId);
        public string GenerateHospitalTreatmentId();
        public List<Model.HospitalTreatment>? GetTreatmentsInGivenRoom(string room);
        public List<Model.HospitalTreatment>? GetTreatmentsForGivenName(string name);
        public void Save();
        public void GetAllHospitalTreatments();
    }
}
